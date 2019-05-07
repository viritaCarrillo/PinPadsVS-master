/*
 * Creado el 30/09/2015
 * @author Lic. Selene Nochebuena Rojo
 * modificado: 18/07/2018
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Collections;
using Multipagos2V10.Impresion;
using Multipagos2V10.Serial;
using Multipagos2V10.VO;
using Multipagos2V10.Util;
using Multipagos2V10.Escucha;
using Multipagos2V10.Interface;
using Multipagos2V10.ConectaWS;
using Multipagos2V10.FileProperties;
using Multipagos2V10.Exceptions;
using System.Windows.Forms;

namespace Multipagos2V10
{
    [ClassInterface(ClassInterfaceType.None)]
    public class LeeBandaChip : Multipagos2V10.Interface.IDatosLectura
    {
        Puerto oPuerto = new Puerto();
        Tarjeta oTarjeta = null;
        Respuesta oRespuesta = null;
        WebService oWebService = new WebService();
        Informacion oInformacion = null;
        private string mensaje = "";
        private string mensajeError = "";
        private byte[] serie = null;
        Log logD = new Log();

        /**
         * Lee los datos de la tarjeta, si tienen chip a traves del chip en caso contario por la banda
         **/
        public void lee(string monto,  int tpoTransaccion, int cdMoneda, bool consulta)
        {
            try
            {

                oTarjeta = new Tarjeta();
                
                if (validaMonto(monto))
                {
                    if (oPuerto.abrePuertos())
                    {
                        string bines = "";
                        bines = Parametros.getParameter("bines");
                        if (bines == null)
                        {
                            bines = "        ";
                        }

                        oPuerto.escribe(Comandos.getComandoZ3(bines));
                        
                        oPuerto.leeACK();
                        oPuerto.escribe(Comandos.setComandoC51(monto, tpoTransaccion, cdMoneda, consulta));
                        if (oPuerto.leeACK())
                        {
                            oTarjeta = new Tarjeta();
                            LeeC51 oC51 = new LeeC51(oPuerto, oTarjeta);
                            oC51.espera();

                            // SE INICIA LA LECTURA POR LA BANDA
                            if (!oTarjeta.getCodigoRespuesta().Equals("00") && oTarjeta.getStatusLectura() != 0)
                            {                                
                                if (oTarjeta.getCodigoRespuesta().Equals("10") ||
                                        oTarjeta.getCodigoRespuesta().Equals("21") ||
                                        oTarjeta.getCodigoRespuesta().Equals("22") ||
                                        oTarjeta.getCodigoRespuesta().Equals("42"))
                                {
                                    oPuerto.escribe(Comandos.setComandoC51(monto, tpoTransaccion, cdMoneda, consulta));

                                    if (oPuerto.leeACK())
                                    {
                                        oTarjeta = new Tarjeta();
                                        //oTarjeta.setNumeroSerie(serie);
                                        LeeC51 oC51_1 = new LeeC51(oPuerto, oTarjeta);
                                        oC51_1.espera();

                                        if (!oTarjeta.getCodigoRespuesta().Equals("00") && oTarjeta.getStatusLectura() != 0)
                                        {
                                            if (oTarjeta.getCodigoRespuesta().Equals("10") ||
                                                    oTarjeta.getCodigoRespuesta().Equals("21") ||
                                                    oTarjeta.getCodigoRespuesta().Equals("22") ||
                                                    oTarjeta.getCodigoRespuesta().Equals("42"))
                                            {
                                                oPuerto.escribe(Comandos.setComandoC51(monto, tpoTransaccion, cdMoneda, consulta));

                                                if (oPuerto.leeACK())
                                                {
                                                    oTarjeta = new Tarjeta();
                                                    //oTarjeta.setNumeroSerie(serie);
                                                    LeeC51 oC51_2 = new LeeC51(oPuerto, oTarjeta);
                                                    oC51_2.espera();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //logD.getLog().Info("EMV -->" + getTagEMV() + "<--");
                        }

                        setMensaje(oTarjeta.getMensaje());
                        oPuerto.cierraPuerto();
                    }
                    else
                    {
                        setMensaje("No se abrio el puerto");
                        System.Console.WriteLine("No se abrio el puerto");
                    }
                }
                else
                {
                    oTarjeta.setMensaje("Monto invalido, el formato es a dos decimales (#.00)");
                    setMensaje("MONTO INVALIDO, EL FORMATO ES A DOS DECIMALES (#.00)");
                }
            }
            catch (DirectoryNotFoundException) {
                setMensajeError("OCURRIO UN ERROR DE CONFIGURACION:\n\nNECESITA CREAR EL DIRECTORIO c://flap/config/ Y COLOCAR LOS ARCHIVOS DE CONFIGURACION");
                oTarjeta.setMensajeError("OCURRIO UN ERROR DE CONFIGURACION:\n\nNECESITA CREAR EL DIRECTORIO c://flap/config/ Y COLOCAR LOS ARCHIVOS DE CONFIGURACION");
            }
            catch (FileNotFoundException)
            {
                setMensajeError("OCURRIO UN ERROR DE CONFIGURACION:\n\nNO SE ENCONTRO EL ARCHIVO pinpad.config EN LA RUTA c://flap/config/");
                oTarjeta.setMensajeError("OCURRIO UN ERROR DE CONFIGURACION:\n\nNO SE ENCONTRO EL ARCHIVO pinpad.config EN LA RUTA c://flap/config/");
            }

            catch (PinPadException ppe)
            {
                System.Console.WriteLine("PinPadException: " + ppe.Message);

                /*oTarjeta = new Tarjeta();
                oTarjeta.setMensaje(ppe.Message);
                setMensaje(ppe.Message);
                oPuerto.cierraPuerto();*/
            }
            catch (Exception e)
            {
                oTarjeta = new Tarjeta();
                setMensajeError(e.Message);
                oTarjeta.setMensajeError(e.Message);

                if (oPuerto != null)
                    oPuerto.cierraPuerto();

                System.Console.WriteLine("Principal_e: " + e.Message);
            }
        }

        public bool termina(string cdAutorizacion, 
                            string criptograma, 
                            string script, 
                            string cdRespuesta, 
                            string ingreso, 
                            string banderaLlave, 
                            string banderaBines, 
                            string tokenET, 
                            int telecarga)
        {
            String com = "";
            String tipo = "";
            String longitud = "";
            String numero = "";
            bool res = true;
            

            try
            {

                String banderaActual = Parametros.getParameter("banderaLlave");

		        if(banderaActual != null && !banderaActual.Equals("1")){
			        if(banderaLlave != null){
                        List<String> lDatos = null;
                        lDatos = Parametros.leeArchivo(Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION);
                        Parametros.escribeArchivo(lDatos, Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION, banderaLlave, "banderaLlave");
			        }
		        }

                if (cdRespuesta != null && !cdRespuesta.Equals("00"))
                {
                    cdAutorizacion = "";
                }
                if (oPuerto.abrePuertos())
                {
                    if (script != null && script.Length > 0)
                    {
                        com = script.Substring(4);
                        tipo = script.Substring(0, 2);
                        longitud = script.Substring(2, 2);

                        longitud = (new Conversiones().HexadecimalDecimal(Constantes.encoding.GetChars(Constantes.encoding.GetBytes(longitud)))).ToString();

                        for (int i = 0; longitud.Length < 3; i++)
                            longitud = "0" + longitud;

                        if (com.Length <= 249)
                        {
                            numero = "01";
                        }
                        else
                        {
                            numero = "02";
                        }
                    }

                    if (script != null && script.Length > 0)
                    {// ejecuta el script (71/72)
                        oPuerto.escribe(Comandos.setComandoC25(Constantes.encoding.GetBytes(tipo), Constantes.encoding.GetBytes(numero), Constantes.encoding.GetBytes(longitud), com));
                        LeeC25 oEscuchaC25 = new LeeC25(oPuerto, oTarjeta);
                        oEscuchaC25.espera();

                        oPuerto.escribe(Comandos.setComandoC12(tipo));
                        LeeC12 oLeeC12 = new LeeC12(oPuerto, oTarjeta);
                        oLeeC12.espera();
                    }

                    oPuerto.escribe(Comandos.setComandoC54(cdAutorizacion, criptograma, cdRespuesta, ingreso));
                    if (oPuerto.leeACK())
                    {
                        oTarjeta = new Tarjeta();
                        oTarjeta.setStatusLectura(-1);
                        LeeC54 oLeeC54= new LeeC54(oPuerto, oTarjeta);
                        oLeeC54.espera();

                        if (oTarjeta.getStatusLectura() == 1)
                        {
                            if (oTarjeta.getTag9F27() != null && oTarjeta.getTag9F27().Length > 0)
                            {
                                String inicio = oTarjeta.getTag9F27().Substring(0, 1);

                                if (inicio.Equals("0") && cdAutorizacion.Length != 0)
                                {
                                    oTarjeta.setReverso(1);
                                }
                                else
                                {
                                    oTarjeta.setReverso(0);
                                }
                            }
                            else
                            {
                                oTarjeta.setReverso(0);
                            }
                        }
                        else
                        {
                            oPuerto.escribe(Comandos.ACK);
                            oPuerto.cierraPuerto();
                            res = false;
                        }
                    }

                    // se cancelan todos los servicio pendientes
                    oPuerto.escribe(Comandos.getComando72());
                    oPuerto.leeACK();                                                                              
                    
                    if(banderaBines != null && banderaBines.Equals("1")){
						
						if(tokenET == null){
							tokenET = "";
						}
						
						if(tokenET != null && tokenET.Length >0){
		        			oPuerto.escribe(Comandos.getComandoC14(tokenET));
		        		
			        		//if(oPuerto.leeACK()){

                                oTarjeta = new Tarjeta();
			        			LeeC14 oLeeC14 = new LeeC14(oPuerto, oTarjeta);
								oLeeC14.espera();																
                                oTarjeta.setMensaje("");

			        		//}	        			        		
		        		}else{
                            oTarjeta.setMensaje("Error en la carga de bines, token ET invalido");
		        		}
					}                    

                    oPuerto.escribe(Comandos.getComandoZ2("Multipagos", true));
                    oPuerto.leeACK();

                    oPuerto.cierraPuerto();
                }
                else
                {
                    oTarjeta.setMensaje("NO SE ABRIO EL PUERTO");
                    setMensajeError("OCURRIO UN ERROR DE CONFIGURACION: \n\n.NO SE ABRIO EL PUERTO");
                }
            }
            catch (Exception e)
            {
                oTarjeta.setMensajeError("" + e.Message);
                oPuerto.cierraPuerto();
            }

            return res;
        }

        /**
         * Valida que el monto tenga el formato requerido para la interred (#.00)
         **/
        private bool validaMonto(string monto)
        {
            bool valido = false;

            try
            {
                if (monto != null && monto.Length >= 4 && monto.Substring(monto.Length - 3, 1).Equals("."))
                    valido = true;

            }
            catch (IndexOutOfRangeException)
            {
            }

            return valido;
        }

        //
        /**
         * Invoca el werbservice pare que procesa una compra.
         **/
        public string compra(string secuencia,
                             string referencia,
                             int    val_1,
                             string servicio,
                             int    c_cur,
                             string importe,
                             string tarjethabiente,
                             string val_3,
                             string val_6,
                             string val_11,
                             string val_12,
                             int    entidad,
                             string val_16,
                             string val_19,
                             string val_20,
                             string email,
                             string accion,                           
                             string emv,
                             int    puntos)
        {            

            oRespuesta = oWebService.compra(secuencia,
                                            referencia,
                                            val_1,
                                            servicio,
                                            c_cur,
                                            importe,
                                            tarjethabiente,
                                            val_3,
                                            val_6,
                                            val_11,
                                            val_12,
                                            entidad,
                                            val_16,
                                            val_19,
                                            val_20,
                                            email,
                                            accion,
                                            emv, 
                                            puntos);

            setMensaje(oRespuesta.getMensaje());

            return oRespuesta.getRes();
        }

        /**
         * Invoca el werbservice pare que procesa una cancelacion.
         **/
        public string cancelacion(string secuencia,
                                  string referencia,
                                  int    val_1,
                                  string servicio,
                                  int    c_cur,
                                  string importe,
                                  string tarjethabiente,
                                  string val_3,                                
                                  string val_6,
                                  int    entidad,
                                  string val_16,                                  
                                  string val_19,
                                  string val_20,
                                  string email,
                                  string accion,
                                  string tagsemv,                                
                                  string autorizacion)
        {

            oRespuesta = oWebService.cancelacion(secuencia,
                                                 referencia,
                                                 val_1,
                                                 servicio,
                                                 c_cur,
                                                 importe,
                                                 tarjethabiente,
                                                 val_3,
                                                 val_6,
                                                 entidad,
                                                 val_16,
                                                 val_19,
                                                 val_20,
                                                 email,
                                                 accion,
                                                 tagsemv,
                                                 autorizacion);

            return oRespuesta.getRes();
        }

        public string encriptaRijndael(string ccNum)
        {

            string cifrado = "";
            cifrado = Cifrado.encriptaRijndael(ccNum);

            return cifrado;
        }

        public string encriptaHmac(string textEncrip)
        {
            IniFile ini = new IniFile("C:\\Flap/config/hmac.properties");
            byte[] keyByte = Constantes.encoding.GetBytes(ini.IniReadValue("Info", "KEY"));
            string shmac = "";

            try
            {
                if (keyByte.Length == 0)
                {
                    string path = typeof(LeeBandaChip).Assembly.Location;
                    path = path.Replace("vx810v1.dll", "config\\hmac.properties");
                    ini = new IniFile(path);
                    keyByte = UTF8Encoding.Default.GetBytes(ini.IniReadValue("Info", "KEY"));
                }

                if (keyByte.Length > 0)
                {

                    HMACSHA1 hmac = new HMACSHA1(keyByte);
                    byte[] messageBytes = Constantes.encoding.GetBytes(textEncrip);
                    byte[] hashmessage = hmac.ComputeHash(messageBytes);
                    shmac = byteToString(hashmessage);
                }
            }
            catch (Exception)
            {
            }

            return shmac;

        }

        private string byteToString(byte[] buff)
        {
            string sBinary = string.Empty;

            for (int i = 0; i < buff.Length; i++)
            {
                sBinary += buff[i].ToString("X2");
            }
            return sBinary;
        }

        /**
         * Obtiene el mensaje que es asignado cuando algo falla
         * en lectura de la tarjeta.
         **/
        public string getMensaje()
        {
            return mensaje;
        }

        private void setMensaje(string mensaje)
        {
            this.mensaje = mensaje;
        }

        /**
         * Obtiene el mensaje de error, es asignado cuando ocurre
         * una excepcion mientras se lee la tarjeta.
         **/
        public string getMensajeError()
        {
            return mensajeError;
        }

        public void setMensajeError(string mensajeError)
        {
            if (mensajeError.Length > 0)
                this.mensajeError = "HA OCURRIDO UN ERROR. CONTACTE A SU PROVEEDOR Y REPORTE EL SIGUIENTE ERROR: \n\n" + mensajeError;
        }



        public bool compraAmex(string secuencia,
                               string referencia,
                               int entidad,
                               int val1,
                               string servicio,
                               string titular,
                               string importe,
                               string tarjeta,
                               string cdSeguridad,
                               string digest,
                               string mail,
                               string telefono,
                               string nombre,
                               string apellidos,
                               string cdPostal,
                               string direccion,
                               string track1,
                               string track2,
                               int val_19,
                               int val_20,
                               int moneda,
                               string tagsEMV,
                               string tagsE1,
                               string correo,
                               string serie)
        {

            string vencimiento = "";
            string tmp = Cifrado.desencriptaRijndael(track2);

            if (tmp != null && tmp.Length > 0)
            {
                int inicio = tmp.IndexOf("=") + 1;
                vencimiento = tmp.Substring(inicio, 4);
            }

            vencimiento = encriptaRijndael(vencimiento);
            oRespuesta = oWebService.compraAmex(secuencia,
                                         referencia,
                                         entidad,
                                         val1,
                                         servicio,
                                         titular,
                                         importe,
                                         tarjeta,
                                         vencimiento,
                                         cdSeguridad,
                                         digest,
                                         mail,
                                         telefono,
                                         nombre,
                                         apellidos,
                                         cdPostal,
                                         direccion,
                                         track1,
                                         track2,
                                         val_19,
                                         val_20,
                                         moneda,
                                         tagsEMV,
                                         tagsE1,
                                         correo,
                                         serie);

            setMensaje(oRespuesta.getMensaje());

            bool autorizada = false;

            if(oRespuesta.getCodigoRespuesta().Equals("00"))
            {
                autorizada = true;
            }

            return autorizada;
        }


        public string consulta(int entidad,
                               int nivel1,
                               int moneda,
                               int servicio,
                               string transmision,
                               string referencia,
                               string tarjeta,
                               string digest,
                               string tpoTarjeta,
                               string accion,
                               string mail,
                               string titular,
                               string emv)
        {
            string xml = "";                              

            xml = oWebService.consulta(entidad,
                                        nivel1,
                                        moneda,
                                        servicio,
                                        transmision,
                                        referencia,
                                        tarjeta,
                                        digest,
                                        tpoTarjeta,
                                        accion,
                                        mail,
                                        titular,
                                        emv);


            return xml;
        }

        public bool cancelaAmex(int cdEntidad, 
                                  int cdNivel1, 
                                  int moneda, 
                                  string servicio, 
                                  string autorizacion, 
                                  int consecutivo, 
                                  string origen)
        {

            oRespuesta = oWebService.cancelaAmex(cdEntidad, cdNivel1, moneda, servicio, autorizacion, consecutivo, origen);

            setMensaje(oRespuesta.getMensaje());

            bool autorizada = false;

            if (oRespuesta.getCodigoRespuesta().Equals("00"))
            {
                autorizada = true;
            }

            return autorizada;
        }

        public string binAutorizado(int entidad, string bin, string mail, string accion)
        {
            return oWebService.isBancomer(entidad, mail, accion, bin);
        }

        /**
         * Obtiene en numero de tarjeta.
         */
        public string getNumTarjeta()
        {
            string tarjeta = "";

            if (oTarjeta != null)
                tarjeta = oTarjeta.getPan();

            return tarjeta;
        }


        /**
         * Obtienen los tags EMV requeridos por la interred.
         **/
        public string getTagEMV()
        {
            string tagEMV = "";

            if (oTarjeta != null)
                tagEMV = oTarjeta.getTagEMV();

            return tagEMV;
        }

        /**
         * Obtiene el valor del tag9F27, el cual indica si se debe o no
         * enviar la transaccion a la interred. Si el valor  es  00  se 
         * debe enviar la transacion, en caso contrario solo se imprime 
         * o despliega el pagare.
         **/
        public string getTag9F27()
        {
            string tag9F27 = "";

            if (oTarjeta != null)
                tag9F27 = oTarjeta.getTag9F27();

            return tag9F27;
        }

        /**
         * Obtiene el nombre del titular.
         **/
        public string getName()
        {
            string nombre = "";

            if (oTarjeta != null)
                nombre = oTarjeta.getName();

            return nombre;
        }

        /**
         * Obtienen el track 1, es decir el nombre del titular.
         **/
        public string getTrack1()
        {
            string track1 = "";

            if (oTarjeta != null)
                track1 = oTarjeta.getTrack1();

            return track1;
        }

        /**
         * Obtiene el valor del track 2.
         **/
        public string getTrack2()
        {
            string track2 = "";

            if (oTarjeta != null)
                track2 = oTarjeta.getTrack2();

            return track2;
        }
        //


        public string status(int entidad,
                             int val_1,
                             int c_cur,
                             int servicio,
                             string secuencia,
                             string referencia,
                             string mail,
                             string acccion)
        {
            string res = "";

            res = oWebService.getStatus(entidad,
                                         val_1,
                                         c_cur,
                                         servicio,
                                         secuencia,
                                         referencia,
                                         mail,
                                         acccion);

            return res;
        }

        public Hashtable mapea(string xml)
        {
            Hashtable map = null;
            map = oWebService.mapea(xml);

            return map;
        }


        public void llave()
        {
            try
            {   
                
                if (oPuerto.abrePuertos())
                {
                    string bines = "";
                    bines = Parametros.getParameter("bines");

                    if (bines != null)
                    {
                        oPuerto.escribe(Comandos.getComandoZ3(bines));
                        oPuerto.leeACK();
                    }

                    oPuerto.escribe(Comandos.getComandoZ10());
                    oTarjeta = new Tarjeta();
                    LeeZ10 oLeeZ10 = new LeeZ10(oPuerto, oTarjeta);
                    oLeeZ10.espera();

                    oPuerto.cierraPuerto();

                    logD.getLog().Info("EMV -->" + getTagEMV() + "<--");
                }
                else
                {
                    oTarjeta.setMensaje("NO SE ABRIO EL PUERTO");
                    setMensajeError("OCURRIO UN ERROR DE CONFIGURACION: \n\n.NO SE ABRIO EL PUERTO");
                }
            }
            catch (Exception e)
            {
                oTarjeta.setMensajeError("" + e.Message);
                oPuerto.cierraPuerto();
            }
        }

        public void carga(string tokenEX)
        {
            logD.getLog().Info("carga -->" + tokenEX + "<--");
            try
            {
                if (oPuerto.abrePuertos())
                {
                    
                        oPuerto.escribe(Comandos.getComandoZ11(tokenEX));
                        if (oPuerto.leeACK())
                        {
                            oTarjeta = new Tarjeta();
                            LeeZ11 oLeeZ11 = new LeeZ11(oPuerto, oTarjeta);
                            oLeeZ11.espera();
                            setMensaje(oTarjeta.getMensaje());

                            if (oTarjeta.getMensaje() != null && oTarjeta.getMensaje().Equals("CARGA EXITOSA"))
                            {
                                List<String> lDatos = null;
                                lDatos = Parametros.leeArchivo(Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION);
                                Parametros.escribeArchivo(lDatos, Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION, "0", "banderaLlave");
                            }
                            else
                            {
                                setMensaje("FALLA AL CARGAR LA LLAVE");
                            }
                        }

                    oPuerto.cierraPuerto();
                }
                else
                {
                    oTarjeta.setMensaje("NO SE ABRIO EL PUERTO");
                    setMensajeError("OCURRIO UN ERROR DE CONFIGURACION: \n\n.NO SE ABRIO EL PUERTO");
                }
            }
            catch (Exception e)
            {
                oTarjeta.setMensajeError("" + e.Message);
                oPuerto.cierraPuerto();
            }
        }

        public string banderaLLave()
        {
            string bandera = "";
            try
            {
                bandera = Parametros.getParameter("banderaLlave");
            }
            catch (PinPadException ppe) {
                setMensaje(ppe.Message);
            }

            if (bandera == null)
            {
                bandera = "0";
            }

            return bandera;
                
        }

        public string telecarga()
        {
            string telecarga = "";
            telecarga = Parametros.getParameter("telecarga");
            if (telecarga == null)
            {
                telecarga = "0";
            }

            return telecarga;

        }

        public void actualiza()
        {
            string version = "";
            string nombre = "";
            string fix = "";
            string ruta = "c://Flap/carga/";

            try
            {
                // lista de archivos (falta)               


                if (Directory.Exists(ruta))
                {
                    DirectoryInfo directorio = new DirectoryInfo(ruta);
                    FileInfo[] archivos = directorio.GetFiles("*.*");
                    
                    if (oPuerto.abrePuertos())
                    {
                        
                            oPuerto.escribe(Comandos.getComandoQ8());
                            oTarjeta = new Tarjeta();
                            LeeQ8 oLeeQ8 = new LeeQ8(oPuerto, oTarjeta);
                            oLeeQ8.espera();

                        nombre = oTarjeta.getTokenES().Substring(9, 6);
                        version = oTarjeta.getTokenES().Substring(15, 14);

                        if (version != null)
                        {
                            version = version.Trim();
                            fix = version.Substring(4,2);
                            version = version.Substring(1, 2);                            
                        }

                        for (int z = 0; z < archivos.Length; z++)
                        {

                            string archivo = archivos[z].Name;
                            string nombreRep = archivo.Substring(0, 6);
                            string versionRep = archivo.Substring(7, 2);
                            string fixRep = archivo.Substring(10, 2);
                            string extension = archivo.Substring(12);

                            if (nombreRep.Equals(nombre))
                            {

                                int iversion = Int32.Parse(version);
                                int iversionRep = Int32.Parse(versionRep);
                                int ifix = Int32.Parse(fix);
                                int ifixRep = Int32.Parse(fixRep);

                                if (iversion <= iversionRep && ifix <= ifixRep)
                                {
                                    
                                    string aplicacion = nombreRep + "v" + versionRep + "_" + fixRep + extension;

                                    FileStream fs = new FileStream("c://Flap/carga/" + aplicacion, FileMode.Open);
                                    StreamReader input = new StreamReader(fs);
                                    BinaryReader inputb = new BinaryReader(fs);
                                    BufferedStream bufin = new BufferedStream(fs);

                                    int leidos = 0;
                                    byte selene;
                                    int x = 0;

                                    do
                                    {
                                        byte[] datos = new byte[512];
                                        //leidos = inputb.Read(datos, 0, 512);

                                        leidos = bufin.Read(datos, 0, 512);
                                        

                                        /*while (x < 512)
                                        {
                                            selene = inputb.ReadByte();
                                            datos[x] = selene;
                                            x++;
                                            Console.WriteLine("x->" + x);
                                        }*/
                                        if (leidos > 0)
                                        //if (true)
                                        {
                                            x = 0;
                                            byte[] tmp = new byte[leidos];
                                          
                                            for (int w = 0; w < leidos; w++)
                                            {                                                    
                                                tmp[w] = datos[w];
                                            }

                                            oPuerto.escribe(Comandos.getComandoQ7(tmp));
                                            //if (oPuerto.leeACK())
                                            //{
                                                oTarjeta = new Tarjeta();
                                                oTarjeta.setStatusLectura(-1);
                                                LeeQ7 oLeeQ7 = new LeeQ7(oPuerto, oTarjeta);
                                                oLeeQ7.espera();
                                            //}
                                            //else
                                            //{
                                            //    mensaje = "Error en la carga";
                                            //}
                                        }
                                    } while (leidos > 0 && oTarjeta.getCodigoRespuesta().Equals("00"));

                                    // indica el fin de la transmision
                                    oPuerto.escribe(Comandos.getComandoQ7(null));

                                    //if (oPuerto.leeACK())
                                    //{
                                        oTarjeta = new Tarjeta();
                                        LeeQ7 oLeeQ7_fin = new LeeQ7(oPuerto, oTarjeta);
                                        oLeeQ7_fin.espera();

                                        if (oTarjeta.getCodigoRespuesta().Equals("00"))
                                        {
                                            mensaje = "Proceso exitos";
                                        }
                                        else if (oTarjeta.getCodigoRespuesta().Equals("99"))
                                        {
                                            mensaje = "Operacion cancelada";
                                        }
                                        else
                                        {
                                            mensaje = "Proceso fallo (" + oTarjeta.getCodigoRespuesta() + "), llame al soporte";
                                        }
                                    /*}
                                    else
                                    {
                                        mensaje = "No se abrio el puerto";
                                    }*/
                                }
                                else
                                {
                                    mensaje = "No se realizo la carga, la version es menor o igual a la existente";
                                }
                            }
                            else
                            {
                                if (mensaje.Length == 0)
                                {
                                    mensaje = "Nombre de la version no corresponde";
                                }
                            }

                            break;
                        }

                        oPuerto.cierraPuerto();
                    }
                    else
                    {
                        oTarjeta.setMensaje("NO SE ABRIO EL PUERTO");
                        setMensajeError("OCURRIO UN ERROR DE CONFIGURACION: \n\n.NO SE ABRIO EL PUERTO");
                    }
                }
                else
                {
                    oTarjeta.setMensaje("Error al actualizar la version, ruta inexistente");
                }
            }
            catch (Exception e)
            {
                oTarjeta.setMensajeError("" + e.Message);
                oPuerto.cierraPuerto();
            }
        }

        public string imprime()
        {
            string pagare = "";

            if (oRespuesta != null)
                pagare = oRespuesta.getPagare();

            return pagare;
        }

        public string reimprime(int entidad,
                                int nivel1,
                                int nivel2,
                                int servicio,                                
                                string referencia,                                
                                string secuencia,
                                string operacion,
                                string autorizacion)
        {
            string pagare = "";

            pagare = oWebService.reimprime( autorizacion,
                                            referencia,
                                            entidad,
                                            operacion,
                                            nivel1,
                                            nivel2,
                                            servicio,
                                            secuencia);

            return pagare;
        }

        public string imprimeDeclinado(int entidad,
                                       int nivel1,
                                       int nivel2,
                                       string servicio,
                                       string tarjeta,
                                       string importe,
                                       string titular,
                                       string autorizacion,
                                       int plazo)
        {
            string pagare = "";


            pagare = oWebService.imprimeDeclinado(entidad,
                                                  nivel1,
                                                  nivel2,
                                                  servicio,
                                                  tarjeta,
                                                  importe,
                                                  titular,
                                                  autorizacion,
                                                  plazo);


            return pagare;
        }

        //
        /**
         * Despliega en pantalla el pagare requerido por la 
         * interred.
         **/
        public string desplegarPagareComercio(string datos)
        {
            IPagare ventana = new IPagare();
            return ventana.visualizarPagare(datos + "|", "tarjeta");
        }

        public string desplegarPagareCliente(string datos)
        {
            IPagare ventana = new IPagare();
            return ventana.visualizarPagare(datos + "|", "tarjetaEnmascarada");
        }

        public string imprimirComercio(string datos, string impresora, int moneda)
        {
            IPagare ventana = new IPagare();
            setMensaje("");
            setMensajeError("");
            return ventana.imprimirPagarePredeterminada(datos + "|", "tarjeta", impresora, moneda);
        }

        public string imprimirCliente(string datos, string impresora, int moneda)
        {
            setMensaje("");
            setMensajeError("");
            IPagare ventana = new IPagare();
            return ventana.imprimirPagarePredeterminada(datos + "|", "tarjetaEnmascarada", impresora, moneda);
        }

        public string imprimirComercio(string datos, string impresora, int moneda, int ancho, int carGrande, int carChico)
        {
            setMensaje("");
            setMensajeError("");
            IPagare ventana = new IPagare();
            return ventana.imprimirPagareConfig(datos + "|", "tarjeta", impresora, moneda, ancho, carChico, carGrande);
        }

        public string imprimirCliente(string datos, string impresora, int moneda, int ancho, int carGrande, int carChico)
        {
            setMensaje("");
            setMensajeError("");
            IPagare ventana = new IPagare();
            return ventana.imprimirPagareConfig(datos + "|", "tarjetaEnmascarada", impresora, moneda, ancho, carChico, carGrande);
        }

        // CAT
        public string imprimirCAT(string datos, string impresora, int moneda, int ancho, int carGrande, int carChico)
        {
            setMensaje("");
            setMensajeError("");
            IPagare ventana = new IPagare();
            return ventana.imprimirPagareCAT(datos + "|", "tarjeta", impresora, moneda, ancho, carChico, carGrande);
        }

        //
        public string imprimirPagareComercioLogos(string datos)
        {
            IPagare ventana = new IPagare();
            return ventana.imprimirPagareLogos(datos + "|", "tarjeta");
        }

        public string imprimirPagareClienteLogos(string datos)
        {
            IPagare ventana = new IPagare();
            return ventana.imprimirPagareLogos(datos + "|", "tarjetaEnmascarada");
        }        

        public string imprimirPagareComercio(string datos, string impresora)
        {
            IPagare ventana = new IPagare();
            return ventana.imprimirPagare(datos + "|", "tarjeta");
        }

        public string imprimirPagareCliente(string datos, string impresora)
        {
            IPagare ventana = new IPagare();
            return ventana.imprimirPagare(datos + "|", "tarjetaEnmascarada");
        }

        /**
         * Imprime el pagare requerido por la interred.
         **/
        public string imprimirPagareComercio(string datos)
        {
            IPagare ventana = new IPagare();
            return ventana.imprimirPagare(datos + "|", "tarjeta");
        }

        public string imprimirPagareCliente(string datos)
        {
            IPagare ventana = new IPagare();
            return ventana.imprimirPagare(datos + "|", "tarjetaEnmascarada");
        }


        public string[] impresoras()
        {
            IPagare ventana = new IPagare();
            return ventana.getImpresoras();
        }

        public string getPlataforma()
        {
            string plataforma = "";

            if (oTarjeta != null)
                plataforma = oTarjeta.getPlataforma();

            return plataforma;
        }

        public string getCvv2()
        {
            string codigoSeguridad = "";

            if (oTarjeta != null)
                codigoSeguridad = oTarjeta.getCvv2();

            return codigoSeguridad;
        }

        public string getTagE1()
        {
            string tagE1 = "";

            if (oTarjeta != null)
                tagE1 = oTarjeta.getTagE1();

            return tagE1;
        }

        public string getScript()
        {
            string script = "";

            if (oRespuesta != null)
                script = oRespuesta.getScript();

            return script;
        }

        /*
         * Regresa el numero de autorizacion de una transaccion.
         */
        public string getAutorizacion()
        {
            string autorizacion = "";

            if (oRespuesta != null)
                autorizacion = oRespuesta.getAutorizacion();

            return autorizacion;

        }

        public string getPagare()
        {
            string pagare = "";

            if (oRespuesta != null)
                pagare = oRespuesta.getPagare();

            return pagare;
        }

        /*
         * Indica si se necesita imprimir el pagare.
         */
        public bool isImprimir()
        {
            bool imprimir = false;

            if (oRespuesta != null)
                imprimir = oRespuesta.isImprimir();

            return imprimir;
        }

        /*
         * Indica si una operacion fue autorizada.
         */
        /*public string getCodigoRespuesta()
        {
            string cdRespuesta = "";

            if (oRespuesta != null)
                cdRespuesta = oRespuesta.getCodigoRespuesta();

            return cdRespuesta;
        }*/

        public string getNumeroSerie()
        {
            string serie = "";

            if (oTarjeta != null)
                serie = oTarjeta.getNumeroSerie();

            return serie;
        }

        public string getCriptograma()
        {
            string tag91 = "";

            if (oRespuesta != null)
                tag91 = oRespuesta.getCriptograma();

            return tag91;
        }

        public string getMdoLectura()
        {
            string mdoLectura = "";

            if (oTarjeta != null)
                mdoLectura = oTarjeta.getMdoLectura();

            return mdoLectura;
        }

        public int getReverso()
        {
            int reverso = 0;

            if (oTarjeta != null)
                reverso = oTarjeta.getReverso();

            return reverso;
        }

        public string getCdRespuesta()
        {
            string cdRespuesta = "";

            if (oTarjeta != null)
                cdRespuesta = oTarjeta.getCodigoRespuesta();

            return cdRespuesta;
        }

        public string getConsecutivo()
        {
            string consecutivo = "";

            if (oRespuesta != null)
                consecutivo = oRespuesta.getConsecutivo();

            return consecutivo;
        }

        // solo es por certificacion
        public string devolucion(string secuencia,
                                 string referencia,
                                 int val_1,
                                 string servicio,
                                 int c_cur,
                                 string importe,                                                                  
                                 int entidad,                                 
                                 string email,
                                 string accion,
                                 string autorizacion)
        {

            oRespuesta = oWebService.devolucion( entidad,
                                                 val_1,
                                                 c_cur,
                                                 servicio,
                                                 referencia,
                                                 autorizacion,
                                                 importe,
                                                 email,
                                                 accion,
                                                 secuencia);

            setMensaje(oRespuesta.getMensaje());

            return oRespuesta.getRes();
        }

        public string valida(string entidad, string tarjeta, string mail, string accion)
        {
            return oWebService.valida(entidad,tarjeta, mail, accion);
        }

        public void desconecta()
        {
            oWebService.desconecta();
        }
    }
}
