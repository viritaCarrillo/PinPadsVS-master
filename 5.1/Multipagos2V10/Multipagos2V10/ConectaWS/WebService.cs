using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Multipagos2V10.ServiciosWebMultipagosInterred5_1;
using Multipagos2V10.ServicioWebMultipagosAmex;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Multipagos2V10.Util;
using Multipagos2V10.Exceptions;
using Multipagos2V10.VO;

using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

namespace Multipagos2V10.ConectaWS
{
    [Serializable]
    class WebService
    {
        ServiciosWebMultipagosInterred5_1.ProcesadorPagosFull webService = null;
        ServicioWebMultipagosAmex.insPagoAmex wsAmex = null;
        //ServiciosWebMultipagosPuntos5_1.ProcesadorPuntosFull wsPuntos = null;
        ServiciosWebMultipagosInterredDevolucion5_1.ProcesaDevolucionesFull wsDev = null;

        String version = "5.1.0";
        Respuesta oRespuesta = new Respuesta();
        Log logD = new Log();

        public string valida(string entidad, string tarjeta, string mail, string accion){
            string res  = null;
            try
            {
                config();
                res = webService.valida(entidad, tarjeta, mail, accion);
            }
            catch (PinPadException)
            {
                res = null;
            }
            /*catch (SoapException se)
            {
                res = null;
                MessageBox.Show("SoapException:" + se.Message);
            }
            catch (WebException we)
            {
                res = null;
                MessageBox.Show("WebException:" + we.Message);
            }
            catch (Exception e)
            {
                res = null;
                MessageBox.Show("Exception:" + e.Message);
            }*/

            return res;
        }

        public void desconecta()
        {
            config();
            webService.getAfiliaciones(10946);

        }
        public Respuesta compra(string transmision,
                                string referencia,
                                int val_1,
                                string t_servicio,
                                int c_cur,
                                string t_importe,
                                string tarjethabiente,
                                string val_3,
                                string val_6,
                                string val_11,
                                string val_12,
                                int cd_entidad,
                                string val_16,
                                string val_19,
                                string val_20,
                                string email,
                                string accion,
                                string tagsemv,
                                int puntos)
        {
            Cifrado c = new Cifrado();
            config();            

            logD.getLog().Info("transmision -->" +  transmision + "<--");
            logD.getLog().Info("referencia -->" + referencia + "<--");
            logD.getLog().Info("val_1 -->" + val_1 + "<--");
            logD.getLog().Info("t_servicio -->" + t_servicio + "<--");
            logD.getLog().Info("c_cur -->" + c_cur + "<--");
            logD.getLog().Info("t_importe -->" + t_importe + "<--");
            logD.getLog().Info("t_importe -->" + t_importe + "<--");
            logD.getLog().Info("val_3 -->" + val_3 + "<--");
            logD.getLog().Info("val_6 -->" + val_6 + "<--");
            logD.getLog().Info("val_11 -->" + val_11 + "<--");
            logD.getLog().Info("val_12 -->" + val_12 + "<--");
            logD.getLog().Info("cd_entidad -->" + cd_entidad + "<--");
            logD.getLog().Info("val_16 -->" + val_16 + "<--");
            logD.getLog().Info("val_19 -->" + val_19 + "<--");
            logD.getLog().Info("val_20 -->" + val_20 + "<--");
            logD.getLog().Info("email -->" + email + "<--");
            logD.getLog().Info("accion -->" + accion + "<--");
            logD.getLog().Info("tagsemv -->" + tagsemv + "<--");
            logD.getLog().Info("puntos -->" + puntos + "<--");

            try
            {

                oRespuesta = new Respuesta();

                oRespuesta.setReferencia(referencia);
                oRespuesta.setTrasmicion(transmision);
                oRespuesta.setEntidad(cd_entidad);
                oRespuesta.setnivel1(val_1);
                oRespuesta.setnivel2(c_cur);
                oRespuesta.setservicio(Int16.Parse(t_servicio));


                string res = webService.procesaCompraOL(transmision,
                                                        referencia,
                                                         val_1,
                                                         t_servicio,
                                                         c_cur,
                                                         t_importe,
                                                         tarjethabiente,
                                                         val_3,
                                                         "",            //val_4
                                                         "",            // val_5
                                                         val_6,
                                                         val_11,
                                                         val_12,
                                                         cd_entidad,
                                                         val_16,
                                                         "1",            // val_17
                                                         "",            // val_18
                                                         val_19,
                                                         val_20,
                                                         email,
                                                         accion,
                                                         "",
                                                         "7",
                                                         "",            // 5F34
                                                         tagsemv,  
                                                         "",            // sflag
                                                         "",            // tag9F26
                                                         "",            // tag9F27
                                                         0,
                                                         puntos);

                oRespuesta.setRes(res);
                //logD.getLog().Info("res -->" + res + "<--");

                if (res != null)
                {
                    string[] sRes = new string[9];
                    sRes = res.Split('|');

                    if (sRes != null)
                    {
                        oRespuesta.setAutorizacion(sRes[0]);
                        oRespuesta.setMensaje(sRes[1]);

                        if (sRes[3].Equals("SI"))
                            oRespuesta.setImprimir(true);
                        else
                            oRespuesta.setImprimir(false);

                        oRespuesta.setFecha(sRes[4]);
                        oRespuesta.setHora(sRes[5]);

                        // falta el de termina sRes[6]

                        oRespuesta.setScript(sRes[7]);
                        oRespuesta.setCriptograma(sRes[8]);

                        if (oRespuesta.isImprimir() && !accion.Equals("CARGA"))
                        {

                            string pagare = "";
                            pagare = webService.getPagare(oRespuesta.getAutorizacion(),
                                                          oRespuesta.getreRerencia(),
                                                          oRespuesta.getEntidad(),
                                                          "1",
                                                          "",
                                                          oRespuesta.getNivel1(),
                                                          oRespuesta.getNivel2(),
                                                          oRespuesta.getServicio(),
                                                          transmision);
                            if (pagare != null)
                            {
                                pagare = pagare.Replace("&Oacute;", "O");
                                pagare = pagare.Replace("&oacute;", "o");
                                pagare = pagare.Replace("&Eacute;", "E");
                                pagare = pagare.Replace("&eacute;", "e");
                                pagare = pagare.Replace("&Iacute;", "I");
                                pagare = pagare.Replace("&iacute;", "i");
                                oRespuesta.setMensaje(obtenerValor("mensaje", pagare));
                            }

                            oRespuesta.setPagare(pagare);
                        }
                    }
                    else
                    {
                        oRespuesta.setMensaje("RESPUESTA INCORRECTA WEBSERVICE");
                    }
                }
                else
                {
                    oRespuesta.setMensaje("SIN RESPUESTA WEBSERVICE");
                }                
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setMensaje(ppe.Message);
                logD.getLog().Info(ppe.Message);
            }
            catch (SoapException)
            {
                oRespuesta.setMensaje("Verifique la URL en el archivo pinpad.config");
                logD.getLog().Info("Verifique la URL en el archivo pinpad.config");
            }
            catch (WebException we)
            {
                oRespuesta.setMensaje("EL WEBSERVICE NO RESPONDE");
                logD.getLog().Info("EL WEBSERVICE NO RESPONDE: " + we.Message);
            }
            catch (Exception e)
            {
                oRespuesta.setMensaje("Comuniquese con el administrador y reporte el siguiente error: " + e.Message);
                logD.getLog().Info("Comuniquese con el administrador y reporte el siguiente error: " + e.Message);
            }

            return oRespuesta;

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
                                string tagsemv)
        {
            
            Cifrado c = new Cifrado();

            string xml = "";

            try
            {
                config();

                string res = webService.consulta(entidad,
                                               nivel1,
                                               moneda,
                                               servicio,
                                               transmision,
                                               referencia,
                                               "0.00",
                                               tarjeta,
                                               "", //vencimiento,
                                               "", //cvv2,
                                               digest,
                                               tpoTarjeta,
                                               "1", // ingreso
                                               accion,
                                               mail,
                                               0,
                                               "", //track2_val,
                                               tagsemv,
                                               "", // tag5F34
                                               ""); // terminal

                if (res == null)
                    xml = getXmlError("301"); // SIN RESPUESTA WEBSERVICE
                else
                    xml = res;

            }
            catch (PinPadException ppe)
            {
                xml = getXmlError(ppe.Message);
            }
            catch (SoapException se)
            {
                xml = getXmlError("307");                
            }
            catch (WebException we)
            {
                xml = getXmlError("304");                
            }
            catch (Exception e)
            {
                xml = getXmlError("305");
            }

            return xml;

        }


        public Respuesta cancelacion(string  secuencia,
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

            try
            {

                config();
                oRespuesta = new Respuesta();

                oRespuesta.setReferencia(referencia);
                oRespuesta.setEntidad(entidad);
                oRespuesta.setnivel1(val_1);
                oRespuesta.setnivel2(c_cur);
                oRespuesta.setservicio(Int32.Parse(servicio));
                oRespuesta.setAutorizacion(autorizacion);

                string res = webService.cancelacion(secuencia,
                                                       referencia,
                                                       val_1,
                                                       servicio,
                                                       c_cur,
                                                       importe,
                                                       tarjethabiente,
                                                       val_3,
                                                       "",      // val_4
                                                       "",      // val_5
                                                       val_6,
                                                       entidad,
                                                       val_16,
                                                       "",      // val_17
                                                       "",      // val_18
                                                       val_19,
                                                       val_20,
                                                       email,
                                                       accion,
                                                       "",
                                                       "7",
                                                       "",      // tag5f34
                                                       tagsemv,
                                                       "",      // sFlag
                                                       "",      // tag9f26 
                                                       "",      // tag9f27
                                                       autorizacion);


                if (res != null)
                {

                    oRespuesta.setRes(res);
                    //oRespuesta.setMensajeError(obtenerValor("mensaje", res));    
                    string[] sRes = new string[9];
                    sRes = res.Split('|');

                    if (sRes != null)
                    {
                        oRespuesta.setAutorizacion(sRes[0]);
                        oRespuesta.setMensaje(sRes[1]);

                        if (sRes[3].Equals("SI"))
                            oRespuesta.setImprimir(true);
                        else
                            oRespuesta.setImprimir(false);

                        oRespuesta.setFecha(sRes[4]);
                        oRespuesta.setHora(sRes[5]);

                        // falta el de termina sRes[6]

                        oRespuesta.setScript(sRes[7]);
                        oRespuesta.setCriptograma(sRes[8]);

                        if (oRespuesta.isImprimir())
                        {

                            string pagare = "";
                            pagare = webService.getPagare(oRespuesta.getAutorizacion(),
                                                          oRespuesta.getreRerencia(),
                                                          oRespuesta.getEntidad(),
                                                          "2",
                                                          "",
                                                          oRespuesta.getNivel1(),
                                                          oRespuesta.getNivel2(),
                                                          oRespuesta.getServicio(),
                                                          secuencia);
                            if (pagare != null)
                            {
                                pagare = pagare.Replace("&Oacute;", "O");
                                pagare = pagare.Replace("&oacute;", "o");
                                pagare = pagare.Replace("&Eacute;", "E");
                                pagare = pagare.Replace("&eacute;", "e");
                                pagare = pagare.Replace("&Iacute;", "I");
                                pagare = pagare.Replace("&iacute;", "i");
                                oRespuesta.setMensaje(obtenerValor("mensaje", pagare));
                            }

                            oRespuesta.setPagare(pagare);
                        }
                    }
                    else
                    {
                        oRespuesta.setMensaje("RESPUESTA INCORRECTA WEBSERVICE");
                    }
                }
                else
                {
                    // RESPUESTA INCORRECTA WEBSERVICE
                    oRespuesta.setRes(getXmlError("300"));
                }
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setRes(getXmlError(ppe.Message));
            }            
            catch (SoapException se)
            {
                oRespuesta.setRes(getXmlError("307"));
            }
            catch (WebException we)
            {
                oRespuesta.setRes(getXmlError("304"));
            }
            catch (Exception e)
            {
                oRespuesta.setRes(getXmlError("305"));             
            }

            return oRespuesta;

        }

        public Respuesta devolucion(int cd_entidad,
                                    int val_1,
                                    int c_cur,
                                    string t_servicio,
                                    string referencia,
                                    string cdAutorizacion,
                                    string t_importe,
                                    string email,
                                    string accion,
                                    string secuencia)
        {
            configDevolucion();

            try
            {
                oRespuesta = new Respuesta();

                oRespuesta.setReferencia(referencia);
                oRespuesta.setEntidad(cd_entidad);
                oRespuesta.setnivel1(val_1);
                oRespuesta.setnivel2(c_cur);
                oRespuesta.setservicio(Int16.Parse(t_servicio));
                oRespuesta.setTrasmicion(secuencia);

                string res = wsDev.procesaDevolucion(cd_entidad,
                                                     val_1,
                                                     c_cur,
                                                     Int16.Parse(t_servicio),
                                                     referencia,
                                                     cdAutorizacion,
                                                     t_importe,
                                                     email,
                                                     accion,
                                                     "K",
                                                     "",
                                                     "",
                                                     "",
                                                     secuencia);

                oRespuesta.setRes(res);

                if (res != null)
                {
                    string[] sRes = new string[6];
                    sRes = res.Split('|');

                    if (sRes != null)
                    {
                        oRespuesta.setAutorizacion(sRes[0]);
                        oRespuesta.setMensaje(sRes[1]);                        

                        if (sRes[4].Equals("SI"))
                            oRespuesta.setImprimir(true);
                        else
                            oRespuesta.setImprimir(false);

                        oRespuesta.setFecha(sRes[4]);
                        oRespuesta.setHora(sRes[5]);

                        //if (oRespuesta.isImprimir()) //Ajuste para validar los mensajes de RECHAZO por EGLOBAL
                        if (oRespuesta.isImprimir() && (oRespuesta.getMensaje().Equals("0") || oRespuesta.getMensaje().Equals("00") || oRespuesta.getMensaje().Equals("") || oRespuesta.getMensaje().Equals("APROBADA")))
                        {
                            string pagare = "";
                            if (c_cur == 0)
                            {
                                pagare = webService.imprimePagare(oRespuesta.getAutorizacion(),
                                                                  oRespuesta.getreRerencia(),
                                                                  oRespuesta.getEntidad(),
                                                                  "3",
                                                                  "",
                                                                  oRespuesta.getNivel1(),
                                                                  oRespuesta.getNivel2(),
                                                                  oRespuesta.getServicio(),
                                                                  oRespuesta.getTransmicion());

                                if (pagare != null)
                                {
                                    pagare = pagare.Replace("&Oacute;", "O");
                                    pagare = pagare.Replace("&oacute;", "o");
                                    pagare = pagare.Replace("&Eacute;", "E");
                                    pagare = pagare.Replace("&eacute;", "e");
                                    pagare = pagare.Replace("&Iacute;", "I");
                                    pagare = pagare.Replace("&iacute;", "i");

                                    oRespuesta.setMensaje(obtenerValor("mensaje", pagare));
                                    if (oRespuesta.getMensaje() != null && oRespuesta.getMensaje().Length > 0)
                                        oRespuesta.setRes(getXmlError(oRespuesta.getMensaje()));
                                    else
                                        oRespuesta.setRes(pagare);
                                }
                            }
                        }
                        else
                        {
                            oRespuesta.setRes(getXmlError(oRespuesta.getMensaje()));
                        }
                    }
                    else
                    {
                        // RESPUESTA INCORRECTA WEBSERVICE
                        oRespuesta.setRes(getXmlError("301"));
                    }
                }
                else
                {
                    // SIN RESPUESTA WEBSERVICE
                    oRespuesta.setRes(getXmlError("301"));
                }
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setRes(getXmlError(ppe.Message));
            }
            catch (SoapException se)
            {
                oRespuesta.setRes(getXmlError("304"));
            }
            catch (WebException we)
            {
                // EL WEBSERVICE NO RESPONDE
                oRespuesta.setRes(getXmlError("304"));
            }
            catch (Exception e)
            {
                oRespuesta.setRes(getXmlError("305"));
            }

            return oRespuesta;

        }

        public Respuesta compraAmex(string transmicion,
                                    string referencia,
                                    int entidad,
                                    int val1,
                                    string servicio,
                                    string titular,
                                    string importe,
                                    string tarjeta,
                                    string vencimiento,
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
                                    string tagE1,
                                    string correo,
                                    string serie)
        {
            
            string ingreso = "";
            Respuesta oRespuesta = new Respuesta();

            try
            {
                ServicioWebMultipagosAmex.InsPagoAmexRequestType peticion = new ServicioWebMultipagosAmex.InsPagoAmexRequestType();
                ServicioWebMultipagosAmex.TarjetaChipType chip = new ServicioWebMultipagosAmex.TarjetaChipType();
                ServicioWebMultipagosAmex.TarjetaDeslizadaType banda = new ServicioWebMultipagosAmex.TarjetaDeslizadaType();
                ServicioWebMultipagosAmex.ValidacionAVSType avs = new ServicioWebMultipagosAmex.ValidacionAVSType();
                ServicioWebMultipagosAmex.EnvioCompraType envio = new ServicioWebMultipagosAmex.EnvioCompraType();
                ServicioWebMultipagosAmex.InsPagoAmexResponseType respuesta = null;

                configAmex();

                string tag9F10 = "";
                string tag9F37 = "";
                string tag9F36 = "";
                string tag95 = "";
                string tag9A = "";
                string tag9C = "";
                string tag9F02 = "";
                string tag5F2A = "";
                string tag9F1A = "";
                string tag82 = "";
                string tag9F03 = "";
                string tag9F26 = "";
                string tag9F27 = "";
                string tag5F34 = "";
                string aid = "";
                string label = "";
                string tag9F1E = "";

                System.Console.WriteLine("tagsEMV -->" + tagsEMV + "<--");

                if (tagsEMV != null && tagsEMV.Length > 0)
                {
                    //E1 = datos[1];
                    //E2 = datos[2];
                    tag9F10 = getTag(tagsEMV, "9F10");
                    tag9F37 = getTag(tagsEMV, "9F37");
                    tag9F36 = getTag(tagsEMV, "9F36");
                    tag95 = getTag(tagsEMV, "95");
                    tag9A = getTag(tagsEMV, "9A");
                    tag9C = getTag(tagsEMV, "9C");
                    tag9F02 = getTag(tagsEMV, "9F02");
                    tag5F2A = getTag(tagsEMV, "5F2A");
                    tag9F1A = getTag(tagsEMV, "9F1A");
                    tag82 = getTag(tagsEMV, "82");
                    tag9F03 = getTag(tagsEMV, "9F03");
                    tag9F26 = getTag(tagsEMV, "9F26");
                    tag9F27 = getTag(tagsEMV, "9F27");
                    tag5F34 = getTag(tagE1, "5F34");
                }

                string valor = "";
                int longitud = 0;
                int posicion = 0;

                string tag4F = "";
                string tam4F = "";

                if (tagE1 != null && tagE1.Length > 0)
                {
                    tag4F = tagE1.Substring(posicion, 2);
                    tam4F = tagE1.Substring(posicion += tag4F.Length, 2);
                    longitud = Convert.ToInt32(new Conversiones().HexadecimalDecimal(tam4F.ToCharArray()));

                    posicion += 2;
                    if (longitud > 0)
                    {
                        aid = tagE1.Substring(posicion, longitud * 2);
                        posicion += longitud * 2;
                    }


                    string tag9F12 = "";
                    string tam9F12 = "";
                    valor = "";
                    tag9F12 = tagE1.Substring(posicion, 4);
                    tam9F12 = tagE1.Substring(posicion += tag9F12.Length, 2);
                    longitud = Convert.ToInt32(new Conversiones().HexadecimalDecimal(tam9F12.ToCharArray()));
                    posicion += 2;
                    if (longitud > 0)
                    {
                        valor = tagE1.Substring(posicion, longitud * 2);
                        posicion += valor.Length;
                    }

                    string tag50 = "";
                    string tam50 = "";
                    tag50 = tagE1.Substring(posicion, 2);
                    tam50 = tagE1.Substring(posicion += tag50.Length, 2);

                    longitud = Convert.ToInt32(new Conversiones().HexadecimalDecimal(tam50.ToCharArray()));
                    posicion += 2;
                    if (longitud > 0)
                    {
                        label = tagE1.Substring(posicion, longitud * 2);
                        posicion += valor.Length;
                    }

                    tag9F1E = getTag(tagsEMV, "9F1E"); // numero de serie
                }
                peticion.s_transm = transmicion;
                peticion.c_referencia = referencia;

                peticion.val_1 = val1;
                peticion.t_servicio = servicio;
                peticion.titular = titular;
                peticion.clave_entidad = entidad;
                peticion.t_importe = importe;
                peticion.val_3 = tarjeta;
                peticion.val_4 = vencimiento;
                peticion.val_5 = cdSeguridad;
                peticion.val_6 = digest;
                peticion.val_11 = mail;
                peticion.val_12 = telefono;
                peticion.val_19 = val_19;
                peticion.val_20 = val_20;
                peticion.c_cur = moneda;

                if (tag9F27 != null && tag9F27.Length > 0)
                {
                    chip.operationEnvironment = 1; // 1 - chip
                    chip.applicationCryptogram = tag9F26;
                    chip.issuerApplicationData = tag9F10;
                    chip.unpredictableNumber = tag9F37;
                    chip.applicationTransactionCounter = tag9F36;
                    chip.terminalVerificationResult = tag95;
                    chip.transactionDate = tag9A;
                    chip.transactionType = tag9C;
                    chip.amountAuthorized = tag9F02;
                    chip.transactionCurrencyCode = tag5F2A;
                    chip.terminalCountryCode = tag9F1A;
                    chip.applicationInterchangeProfile = tag82;
                    chip.amountOther = tag9F03;
                    chip.applicationPANSequence = tag5F34;
                    chip.cryptogramInformationData = tag9F27;
                    chip.track2 = track2;
                    peticion.extraData = tagE1 + tagsEMV;
                    peticion.tarjetaChip = chip;                    
                    ingreso = "C";
                }
                else
                {
                    banda.track1 = track1;
                    banda.track2 = track2;
                    banda.operationEnvironment = 3;
                    peticion.tarjetaDeslizada = banda;
                    peticion.extraData = serie;
                    ingreso = "S";
                }

                avs.apellidosTitular = apellidos;
                avs.codigoPostal = cdPostal;
                avs.direccion = direccion;
                avs.nombreTitular = nombre;
                avs.datosEnvio = envio;
                peticion.validacionAVS = avs;

                respuesta = wsAmex.CallinsPagoAmex(peticion);

                string campo55 = respuesta.integratedCircuit;
                string criptograma = "";
                string script = "";

                if (campo55 != null && campo55.Length > 0)
                {
                    int pos = 11; //144AGNS0001
                    int tam = 0;
                    string tag = "";
                    try
                    {                        
                        string tamHexa = campo55.Substring(pos, 2);
                        tam = Convert.ToInt32(new Conversiones().HexadecimalDecimal(Convert.ToString(tamHexa).ToCharArray()));
                        if(tam > 0){
                            criptograma = "91" + tamHexa + campo55.Substring(pos + 2, tam * 2);
                        }
                    }catch(Exception){
                    }

                    if (criptograma == null && criptograma.Length == 0)
                        pos = 0;
                    else
                        pos = pos + 4 + tam * 2;

                    try
                    {
                        tag = campo55.Substring(pos, 2);
                        if (tag.Equals("71") || tag.Equals("72"))
                        {
                            tam = Convert.ToInt32(new Conversiones().HexadecimalDecimal(Convert.ToString(campo55.Substring(pos + 2, 2)).ToCharArray()));
                            Console.WriteLine("tam: " + tam);
                            script = campo55.Substring(pos, (tam * 2) + 4);
                        }
                    }catch(Exception){}
                }

                string promocion = "";

                if (val_19 == 1)
                    promocion = val_20 + " MESES SIN INTERESES";
                else if (val_19 == 2)
                    promocion = val_20 + " MESES CON INTERESES";

                if (respuesta != null)
                {
                    if (respuesta.mensaje != null && respuesta.mensaje.Length == 0 && !respuesta.authorizationCode.Equals("000000"))
                    {
                        oRespuesta.setAutorizacion(respuesta.authorizationCode);
                        oRespuesta.setMensaje(respuesta.mensaje);
                        oRespuesta.setScript(script);
                        oRespuesta.setCriptograma(criptograma);
                        oRespuesta.setImprimir(true);
                        oRespuesta.setConsecutivo(respuesta.consecutivoPago);

                        oRespuesta.setCodigoRespuesta(respuesta.codigoRespuesta + "");

                        if (oRespuesta.getCodigoRespuesta().Length <= 1)
                        {
                            oRespuesta.setCodigoRespuesta("0" + oRespuesta.getCodigoRespuesta());
                        }


                        string fecha = formateaFechaHora(respuesta.horaTransaccion);
                        fecha = fecha.Substring(0, 7);

                        string hora = formateaFechaHora(respuesta.horaTransaccion);
                        hora = hora.Substring(7, 5);

                        string firma = respuesta.firma;
                        oRespuesta.setPagare(getPagare(respuesta.afiliacion,
                                                        respuesta.authorizationCode,
                                                        respuesta.nombreComercio,
                                                        respuesta.direccionComercio,
                                                        fecha,
                                                        hora,
                                                        enmascaraTarjeta(Cifrado.desencriptaRijndael(tarjeta)),
                                                        Cifrado.desencriptaRijndael(vencimiento),
                                                       "VENTA",
                                                       importe,
                                                       titular,
                                                       promocion,
                                                       tag9F26,
                                                       ingreso,
                                                       respuesta.aid,
                                                       aid,
                                                       respuesta.consecutivoPago,
                                                       serie, 
                                                       firma));

                        oRespuesta.setMensaje("APROBADA");
                    }
                    else
                    {
                        if (respuesta.codigoRespuesta == 0)
                        {
                            oRespuesta.setCodigoRespuesta("99");
                        }
                        else
                        {
                            oRespuesta.setCodigoRespuesta(respuesta.codigoRespuesta + "");
                        }
                        oRespuesta.setMensaje(respuesta.mensaje);
                        oRespuesta.setAutorizacion("000000");
                        oRespuesta.setImprimir(false);
                    }
                }
                else
                {
                    oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador");
                    oRespuesta.setCodigoRespuesta("99");
                    oRespuesta.setAutorizacion("000000");
                    oRespuesta.setImprimir(false);
                }
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador" + ppe.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (SoapException se)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador" + se.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (WebException we)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador" + we.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (NullReferenceException nre)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador" + nre.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (Exception e)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador");
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }

            return oRespuesta;

        }

        public Respuesta cancelaAmex(int entidad,
                                     int val1,
                                     int c_cur,
                                     string servicio,
                                     string autorizacion,
                                     int consecutivo,
                                     string origen)
        {
            oRespuesta = new Respuesta();
            string ingreso = "";

            try
            {
                ServicioWebMultipagosAmex.CancelPagoType oCancela = new ServicioWebMultipagosAmex.CancelPagoType();
                ServicioWebMultipagosAmex.CancelPagoResponseType respuesta = null;

                if (origen != null)
                {
                    if (origen.Equals("S"))
                        ingreso = "6";
                    else if (origen.Equals("C"))
                        ingreso = "7";
                }

                oCancela.clave_entidad = entidad;
                oCancela.val_1 = val1;
                oCancela.c_cur = c_cur;
                oCancela.t_servicio = servicio;
                oCancela.cd_origen = ingreso;
                oCancela.cd_autpgo = autorizacion;
                oCancela.nu_conpgo = consecutivo;

                configAmex();
                respuesta = wsAmex.CancelaPagoAmex(oCancela);

                if (respuesta != null)
                {
                    if (respuesta.mensaje != null && respuesta.mensaje.Length == 0)
                    {
                        string fecha = "";
                        string hora = "";

                        if(respuesta.horaTransaccion != null && respuesta.horaTransaccion.Length >= 12){
                            fecha = formateaFechaHora(respuesta.horaTransaccion);
                            fecha = fecha.Substring(0, 7);

                            hora = formateaFechaHora(respuesta.horaTransaccion);
                            hora = hora.Substring(7, 5);
                        }

                        oRespuesta.setAutorizacion(respuesta.authorizationCode);
                        oRespuesta.setMensaje(respuesta.mensaje);
                        oRespuesta.setScript("");
                        oRespuesta.setCriptograma("");
                        oRespuesta.setImprimir(true);
                        
                        oRespuesta.setCodigoRespuesta(respuesta.codigoRespuesta + "");

                        if (oRespuesta.getCodigoRespuesta().Length <= 1)
                        {
                            oRespuesta.setCodigoRespuesta("0" + oRespuesta.getCodigoRespuesta());
                        }
                        
                        oRespuesta.setPagare(getPagare(respuesta.afiliacion,
                                                       respuesta.authorizationCode,
                                                       respuesta.nombreComercio,
                                                       respuesta.direccionComercio,
                                                       fecha,
                                                       hora,
                                                       "***********" + respuesta.tdc,
                                                       respuesta.vencimiento,                                                       
                                                       "CANCELACION",
                                                       respuesta.monto,
                                                       respuesta.titular,
                                                       "",  // promocion
                                                       "",  // tag9F26
                                                       "M", // modo de ingreso
                                                       "",  // AID
                                                       "",  // LABEL
                                                       respuesta.consecutivoPago,
                                                       respuesta.afiliacion,
                                                       ""));                        
                    }
                    else
                    {
                        oRespuesta.setMensaje(respuesta.mensaje);
                        oRespuesta.setImprimir(false);
                        oRespuesta.setAutorizacion("000000");
                        oRespuesta.setCriptograma("");
                        oRespuesta.setScript("");

                        if (respuesta != null)
                        {
                            oRespuesta.setCodigoRespuesta(respuesta.codigoRespuesta + "");
                            if (oRespuesta.getCodigoRespuesta().Length <= 1)
                            {
                                oRespuesta.setCodigoRespuesta("0" + oRespuesta.getCodigoRespuesta());
                            }
                        }
                        else
                        {
                            oRespuesta.setCodigoRespuesta("99");
                        }
                    }
                }
                else
                {
                    oRespuesta.setMensaje("Ha ocurrido un error, contacte al administrador");
                    oRespuesta.setCodigoRespuesta("99");
                    oRespuesta.setAutorizacion("000000");
                }
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador (ppe)" + ppe.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (SoapException se)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador (se)" + se.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (WebException we)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador (we)" + we.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (NullReferenceException nre)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador (nre)" + nre.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }
            catch (Exception e)
            {
                oRespuesta.setMensaje("Ha ocurrido error contacte a su administrador (e)" + e.Message);
                oRespuesta.setCodigoRespuesta("99");
                oRespuesta.setAutorizacion("000000");
                oRespuesta.setImprimir(false);
            }

            return oRespuesta;

        }
        // nuevo
        /*public string cancelaAmex(int entidad,
                                  int val1,
                                  int c_cur,
                                  string servicio,
                                  int autorizacion,
                                  int consecutivo,
                                  string origen)
        {

            string xml = "";
            string ingreso = "";

            Log logD = new Log();

            logD.getLog().Info("cancelaAmex.autorizacion:  -->" + autorizacion + "<--");
            logD.getLog().Info("cancelaAmex.consecutivo:  -->" + consecutivo + "<--");
            logD.getLog().Info("cancelaAmex.origen:  -->" + origen + "<--");
            logD.getLog().Info("cancelaAmex.version:  -->" + version + "<--");

            try
            {
                ServicioWebMultipagosAmex.CancelPagoType oCancela = new ServicioWebMultipagosAmex.CancelPagoType();
                ServicioWebMultipagosAmex.InvalidatePagoAmexResponseType response = null;

                if (origen != null)
                {
                    if (origen.Equals("D@1"))
                        ingreso = "6";
                    else if (origen.Equals("I@1"))
                        ingreso = "7";
                }

                oCancela.clave_entidad = entidad;
                oCancela.val_1 = val1;
                oCancela.c_cur = c_cur;
                oCancela.t_servicio = servicio;
                oCancela.cd_origen = ingreso;
                oCancela.cd_autpgo = autorizacion;
                oCancela.nu_conpgo = consecutivo;

                configAmex();
                response = wsAmex.invalidatePagoAmex(oCancela);

                if (response != null)
                {
                    xml = response.xmlPagare;
                }
                else
                {
                    // SIN RESPUESTA DEL WS
                    xml = getXmlAmexError("401");
                }
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setRes(getXmlError(ppe.Message));
                logD.getLog().Info("cancelacionAMEX: PinPadException (" + ppe.Message + ")");
                logD.getLog().Info("cancelacionAMEX: PinPadException (" + ppe.StackTrace + ")");
            }
            catch (PinPadExceptionUsuario ppeu)
            {
                oRespuesta.setRes(getXmlError(ppeu.Message));
                logD.getLog().Info("cancelacionAMEX: PinPadExceptionUsuario (" + ppeu.Message + ")");
                logD.getLog().Info("cancelacionAMEX: PinPadExceptionUsuario (" + ppeu.StackTrace + ")");
            }
            catch (SoapException se)
            {
                xml = getXmlAmexError("404");
                logD.getLog().Info("Error en la cancelacion: HA OCURRIDO UN ERROR DE CONFIGURACION. VERIFIQUE CON SU PROVEEDOR LA URL (304)");
                logD.getLog().Info("SoapException: " + se.Message);
                logD.getLog().Info("SoapException: " + se.StackTrace);
            }
            catch (WebException we)
            {
                xml = getXmlAmexError("404");
                logD.getLog().Info("Error en la cancelacion: EL WEBSERVICE NO RESPONDE (304)");
                logD.getLog().Info("WebException: " + we.Message);
                logD.getLog().Info("WebException: " + we.StackTrace);
            }
            catch (NullReferenceException nre)
            {
                xml = getXmlAmexError("411");
                logD.getLog().Info("Error en la cancelacion (411)");
                logD.getLog().Info("NullReferenceException: " + nre.Message);
                logD.getLog().Info("NullReferenceException: " + nre.StackTrace);
            }
            catch (Exception e)
            {
                xml = getXmlAmexError("405");
                logD.getLog().Info("Error en la cancelacion: Comuniquese con su administrador (405)");
                logD.getLog().Info("Exception: " + e.Message);
                logD.getLog().Info("Exception: " + e.StackTrace);
            }

            return xml;

        }*/
        //

        public Respuesta pagareDeclinado(int cd_entidad,
                                          string nu_afiliacion,
                                          string serie,
                                          string val_3,
                                          float t_importe,
                                          string tag9f26,
                                          string tarjethabiente,
                                          string val_8,
                                          string t_servicio,
                                          int val_1,
                                          int c_cur)
        {
            string declinado = "";

            try
            {
                config();
                declinado = webService.getPagareDeclinado(cd_entidad,
                                                   nu_afiliacion,
                                                   serie,
                                                   val_3,
                                                   t_importe,
                                                   tag9f26,
                                                   tarjethabiente,
                                                   val_8,
                                                   t_servicio,
                                                   val_1,
                                                   c_cur);
                if (declinado != null)
                {
                    declinado = declinado.Replace("&Oacute;", "O");
                    declinado = declinado.Replace("&oacute;", "o");
                    declinado = declinado.Replace("&Eacute;", "E");
                    declinado = declinado.Replace("&eacute;", "e");
                    declinado = declinado.Replace("&Iacute;", "I");
                    declinado = declinado.Replace("&iacute;", "i");

                    oRespuesta.setMensaje(obtenerValor("mensaje", declinado));
                }
                oRespuesta.setPagare(declinado);
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setMensaje(ppe.Message);
            }
            catch (SoapException se)
            {
                // HA OCURRIDO UN ERROR DE CONFIGURACION:\n\nVERIFIQUE CON SU PROVEEDOR LA URL
                oRespuesta.setMensaje("307");
            }
            catch (WebException we)
            {
                // EL WEBSERVICE NO RESPONDE
                oRespuesta.setMensaje("304");
            }

            return oRespuesta;
        }


        public Respuesta pagareDeclinadoChip(int cd_entidad,
                                              string val_3,
                                              String t_importe,
                                              string tag9f26,
                                              string tarjethabiente,
                                              string val_8,
                                              string t_servicio,
                                              int val_1,
                                              int c_cur,
                                              int plazo)
        {
            string declinado = "";

            try
            {
                config();
                declinado = webService.getPagareDeclinadoChip(cd_entidad,
                                                   val_3,
                                                   t_importe,
                                                   tag9f26,
                                                   tarjethabiente,
                                                   val_8,
                                                   t_servicio,
                                                   val_1,
                                                   c_cur,
                                                   plazo);
                if (declinado != null)
                {
                    declinado = declinado.Replace("&Oacute;", "O");
                    declinado = declinado.Replace("&oacute;", "o");
                    declinado = declinado.Replace("&Eacute;", "E");
                    declinado = declinado.Replace("&eacute;", "e");
                    declinado = declinado.Replace("&Iacute;", "I");
                    declinado = declinado.Replace("&iacute;", "i");

                    oRespuesta.setMensaje(obtenerValor("mensaje", declinado));
                }
                oRespuesta.setPagare(declinado);
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setRes(getXmlError(ppe.Message));
            }            
            catch (SoapException se)
            {
                // HA OCURRIDO UN ERROR DE CONFIGURACION:\n\nVERIFIQUE CON SU PROVEEDOR LA URL
                oRespuesta.setMensaje("306");
            }
            catch (WebException we)
            {
                // EL WEBSERVICE NO RESPONDE
                oRespuesta.setMensaje("304");
            }

            return oRespuesta;
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void config()
        {

            string url = Parametros.getParameter("URL");
            string ipProxy = Parametros.getParameter("PROXY");
            string spuerto = Parametros.getParameter("PUERTO");

            logD.getLog().Info("url -->" + url + "<--");
            if (url != null && url.Length > 0)
            {
                webService = new ServiciosWebMultipagosInterred5_1.ProcesadorPagosFull(url);
                webService.Timeout = 40000; // se puso un tiempo de espera de 40 segundos

                logD.getLog().Info("ur");
                if ((ipProxy != null) && (ipProxy != ""))
                {
                    logD.getLog().Info("proxi");
                    if (spuerto != null && spuerto.Length > 0)
                    {
                        int puertoProxy = Convert.ToInt32(spuerto);

                        if (puertoProxy > 0)
                        {

                            try
                            {
                                IPAddress validaIP = IPAddress.Parse(ipProxy);

                                WebProxy objproxy = new WebProxy(ipProxy, puertoProxy);

                                objproxy.Credentials = CredentialCache.DefaultCredentials;

                                WebRequest.DefaultWebProxy = objproxy;

                                webService.Proxy = objproxy;

                                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                                CredentialCache cache = new CredentialCache();
                            }
                            catch (Exception e)
                            {
                                //logD.getLog().Info("config.Exception (305)-->" + e.Message + "<--");
                                throw new PinPadException("305");
                            }
                        }
                        else
                        {
                            //logD.getLog().Info("config: HA OCURRIDO UN ERROR DE CONFIGURACION: PARAMETRO PUERTO INVALIDO, VERIFIQUE SU ARCHIVO pinpad.config (308)");
                            throw new PinPadException("308");
                        }
                    }
                    else
                    {
                        //logD.getLog().Info("config: HA OCURRIDO UN ERROR DE CONFIGURACION: NECESITA CONFIGURAR EL PARAMETRO PUERTO EN EL ARCHIVO pinpad.config (309)");
                        throw new PinPadException("309");
                    }
                }
            }
            else
            {
                //logD.getLog().Info("config: (310) HA OCURRIDO UN ERROR DE CONFIGURACION: NECESITA CONFIGURAR EL PARAMETRO URL EN EL ARCHIVO pinpad.config (310)");
                throw new PinPadException("310");
            }
        }

        /*private void configPuntos()
        {
            string url = Parametros.getParameter("URLPUNTOS");
            string ipProxy = Parametros.getParameter("PROXY");
            string spuerto = Parametros.getParameter("PUERTO");
            Log logD = new Log();

            if (url != null && url.Length > 0)
            {
                wsPuntos = new ServiciosWebMultipagosPuntos5_1.ProcesadorPuntosFull(url);
                wsPuntos.Timeout = 40000; // se puso un tiempo de espera de 40 segundos

                if ((ipProxy != null) && (ipProxy != ""))
                {
                    if (spuerto != null && spuerto.Length > 0)
                    {
                        int puertoProxy = Convert.ToInt32(spuerto);

                        if (puertoProxy > 0)
                        {

                            try
                            {
                                IPAddress validaIP = IPAddress.Parse(ipProxy);

                                WebProxy objproxy = new WebProxy(ipProxy, puertoProxy);

                                objproxy.Credentials = CredentialCache.DefaultCredentials;

                                WebRequest.DefaultWebProxy = objproxy;

                                webService.Proxy = objproxy;

                                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                                CredentialCache cache = new CredentialCache();
                            }
                            catch (Exception e)
                            {
                                logD.getLog().Info("ConfigPuntos.Exception (305): " + e.Message);
                                throw new PinPadException("305");
                            }
                        }
                        else
                        {
                            logD.getLog().Info("configPuntos: HA OCURRIDO UN ERROR DE CONFIGURACION: PARAMETRO PUERTO INVALIDO, VERIFIQUE SU ARCHIVO pinpad.config (308)");
                            throw new PinPadExceptionUsuario("308");
                        }
                    }
                    else
                    {
                        logD.getLog().Info("configPuntos: HA OCURRIDO UN ERROR DE CONFIGURACION: NECESITA CONFIGURAR EL PARAMETRO PUERTO EN EL ARCHIVO pinpad.config (309)");
                        throw new PinPadExceptionUsuario("309");
                    }
                }
            }
            else
            {
                logD.getLog().Info("configPuntos: HA OCURRIDO UN ERROR DE CONFIGURACION: NECESITA CONFIGURAR EL PARAMETRO URL EN EL ARCHIVO pinpad.config (310)");
                throw new PinPadExceptionUsuario("310");
            }
        }*/

        private void configAmex()
        {
            string url = Parametros.getParameter("URLAMEX");
            string ipProxy = Parametros.getParameter("PROXY");
            string spuerto = Parametros.getParameter("PUERTO");

            if (url != null && url.Length > 0)
            {

                wsAmex = new ServicioWebMultipagosAmex.insPagoAmex(url);
                wsAmex.Timeout = 40000; // se puso un tiempo de espera de 40 segundos

                if ((ipProxy != null) && (ipProxy != ""))
                {
                    if (spuerto != null && spuerto.Length > 0)
                    {
                        int puertoProxy = Convert.ToInt32(spuerto);

                        if (puertoProxy > 0)
                        {

                            try
                            {
                                IPAddress validaIP = IPAddress.Parse(ipProxy);

                                WebProxy objproxy = new WebProxy(ipProxy, puertoProxy);

                                objproxy.Credentials = CredentialCache.DefaultCredentials;

                                WebRequest.DefaultWebProxy = objproxy;

                                webService.Proxy = objproxy;

                                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                                CredentialCache cache = new CredentialCache();
                            }
                            catch (Exception e)
                            {
                                throw new PinPadException(e.Message);
                            }
                        }
                        else
                        {
                            throw new PinPadException("\n\nHA OCURRIDO UN ERROR DE CONFIGURACION: \n\nPARAMETRO PUERTO INVALIDO, VERIFIQUE SU ARCHIVO pinpad.config");
                        }
                    }
                    else
                    {
                        throw new PinPadException("\n\nHA OCURRIDO UN ERROR DE CONFIGURACION: \n\nNECESITA CONFIGURAR EL PARAMETRO PUERTO EN EL ARCHIVO pinpad.config");
                    }
                }
            }
            else
            {
                throw new PinPadException("\n\nHA OCURRIDO UN ERROR DE CONFIGURACION:\n\nNECESITA CONFIGURAR EL PARAMETRO URLAMEX EN EL ARCHIVO pinpad.config");
            }
        }

        private void configDevolucion()
        {
            string url = Parametros.getParameter("URLDEV");
            string ipProxy = Parametros.getParameter("PROXY");
            string spuerto = Parametros.getParameter("PUERTO");

            if (url != null && url.Length > 0)
            {
                wsDev = new ServiciosWebMultipagosInterredDevolucion5_1.ProcesaDevolucionesFull(url);
                wsDev.Timeout = 40000; // se puso un tiempo de espera de 40 segundos

                if ((ipProxy != null) && (ipProxy != ""))
                {
                    if (spuerto != null && spuerto.Length > 0)
                    {
                        int puertoProxy = Convert.ToInt32(spuerto);

                        if (puertoProxy > 0)
                        {

                            try
                            {
                                IPAddress validaIP = IPAddress.Parse(ipProxy);

                                WebProxy objproxy = new WebProxy(ipProxy, puertoProxy);

                                objproxy.Credentials = CredentialCache.DefaultCredentials;

                                WebRequest.DefaultWebProxy = objproxy;

                                webService.Proxy = objproxy;

                                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);

                                CredentialCache cache = new CredentialCache();
                            }
                            catch (Exception e)
                            {
                                throw new PinPadException(e.Message);
                            }
                        }
                        else
                        {
                            //logD.getLog().Info("HA OCURRIDO UN ERROR DE CONFIGURACION: PARAMETRO PUERTO INVALIDO, VERIFIQUE SU ARCHIVO pinpad.config (308)");
                            throw new PinPadException("308");
                        }
                    }
                    else
                    {
                        //logD.getLog().Info("HA OCURRIDO UN ERROR DE CONFIGURACION: NECESITA CONFIGURAR EL PARAMETRO PUERTO EN EL ARCHIVO pinpad.config (309)");
                        throw new PinPadException("309");
                    }
                }
            }
            else
            {
                //logD.getLog().Info("HA OCURRIDO UN ERROR DE CONFIGURACION: NECESITA CONFIGURAR EL PARAMETRO URL EN EL ARCHIVO pinpad.config (310)");
                throw new PinPadException("310");
            }
        }

        private string obtenerValor(String parametro, String datos)
        {
            string valor = "";

            int inicio = datos.IndexOf(parametro);

            char[] cDatos = datos.ToCharArray();
            int tam = cDatos.Length;

            if (inicio >= 0)
            {
                inicio += parametro.Length + 1;
                while (inicio < tam && cDatos[inicio] != '|')
                {
                    valor += Convert.ToString(cDatos[inicio++]);
                }
            }

            return valor;
        }

        // Comienza codigo de amex


        // Comienza codigo de amex

        /*private string getXml(string datos)
        {
            string xml = "";

            try
            {
                xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n" +
                             "<interred-response-envelope version=\"1.0\">\n" +
                             "<interred-response>\n";
                xml += "<param name=\"imprimir\" value=\"SI\"/>\n";

                if (!obtenerValor("mensaje", datos).Equals(""))
                    xml += "<param name=\"mensaje\" value=\"" + obtenerValor("mensaje", datos) + "\"/>\n";

                if (!obtenerValor("razonSocial", datos).Equals(""))
                    xml += "<param name=\"razonSocial\" value=\"" + obtenerValor("razonSocial", datos) + "\"/>\n";

                if (!obtenerValor("comercio", datos).Equals(""))
                    xml += "<param name=\"comercio\" value=\"" + obtenerValor("comercio", datos) + "\"/>\n";

                if (!obtenerValor("direccion", datos).Equals(""))
                    xml += "<param name=\"direccion\" value=\"" + obtenerValor("direccion", datos) + "\"/>\n";

                if (!obtenerValor("afiliacion", datos).Equals(""))
                    xml += "<param name=\"afiliacion\" value=\"" + obtenerValor("afiliacion", datos) + "\"/>\n";

                if (!obtenerValor("fechaTransaccion", datos).Equals(""))
                    xml += "<param name=\"fhTransaccion\" value=\"" + obtenerValor("fechaTransaccion", datos) + "\"/>\n";

                if (!obtenerValor("horaTransaccion", datos).Equals(""))
                    xml += "<param name=\"hora\" value=\"" + obtenerValor("horaTransaccion", datos) + "\"/>\n";

                if (!obtenerValor("tarjeta", datos).Equals(""))
                    xml += "<param name=\"tarjeta\" value=\"" + obtenerValor("tarjeta", datos) + "\"/>\n";

                if (!obtenerValor("emisor", datos).Equals(""))
                    xml += "<param name=\"emisor\" value=\"" + obtenerValor("emisor", datos) + "\"/>\n";

                if (!obtenerValor("tipoOperacion", datos).Equals(""))
                    xml += "<param name=\"operacion\" value=\"" + obtenerValor("tipoOperacion", datos) + "\"/>\n";

                if (!obtenerValor("monto", datos).Equals(""))
                    xml += "<param name=\"monto\" value=\"" + obtenerValor("monto", datos) + "\"/>\n";

                if (!obtenerValor("tipoPago", datos).Equals(""))
                    xml += "<param name=\"ingreso\" value=\"" + obtenerValor("tipoPago", datos) + "\"/>\n";

                if (!obtenerValor("autorizacion", datos).Equals(""))
                    xml += "<param name=\"autorizacion\" value=\"" + obtenerValor("autorizacion", datos) + "\"/>\n";

                if (!obtenerValor("referencia", datos).Equals(""))
                    xml += "<param name=\"referencia\" value=\"" + obtenerValor("referencia", datos) + "\"/>\n";

                if (!obtenerValor("criptograma", datos).Equals(""))
                    xml += "<param name=\"criptograma\" value=\"" + obtenerValor("criptograma", datos) + "\"/>\n";

                if (!obtenerValor("firma", datos).Equals(""))
                    xml += "<param name=\"firma\" value=\"" + obtenerValor("firma", datos) + "\"/>\n";

                if (!obtenerValor("titular", datos).Equals(""))
                    xml += "<param name=\"titular\" value=\"" + obtenerValor("titular", datos) + "\"/>\n";

                if (!obtenerValor("promocion", datos).Equals(""))
                    xml += "<param name=\"promocion\" value=\"" + obtenerValor("promocion", datos) + "\"/>\n";

                if (!obtenerValor("plan", datos).Equals(""))
                    xml += "<param name=\"plan\" value=\"" + obtenerValor("plan", datos) + "\"/>\n";

                if (!obtenerValor("aid", datos).Equals(""))
                    xml += "<param name=\"aid\" value=\"" + obtenerValor("aid", datos) + "\"/>\n";

                if (!obtenerValor("tag91", datos).Equals(""))
                    xml += "<param name=\"tag91\" value=\"" + obtenerValor("tag91", datos) + "\"/>\n";




                // interred 5.1
                if (!obtenerValor("cdRespuesta", datos).Equals(""))
                    xml += "<param name=\"cdRespuesta\" value=\"" + obtenerValor("cdRespuesta", datos) + "\"/>\n";

                if (!obtenerValor("llave", datos).Equals(""))
                    xml += "<param name=\"llave\" value=\"" + obtenerValor("llave", datos) + "\"/>\n";

                if (!obtenerValor("bines", datos).Equals(""))
                    xml += "<param name=\"bines\" value=\"" + obtenerValor("bines", datos) + "\"/>\n";

                if (!obtenerValor("tokenET", datos).Equals(""))
                    xml += "<param name=\"tokenET\" value=\"" + obtenerValor("tokenET", datos) + "\"/>\n";

                if (!obtenerValor("tokenEX", datos).Equals(""))
                    xml += "<param name=\"tokenEX\" value=\"" + obtenerValor("tokenEX", datos) + "\"/>\n";

                xml += "</interred-response>\n" +
                       "</interred-response-envelope>\n";
            }
            catch (Exception)
            {
                // Error al generar el pagare, contacte a su administrador
                xml = getXmlError("312");
            }

            if (xml.Length == 0)
            {
                // Error al generar el pagare, contacte a su administrador
                xml = getXmlError("312");
            }

            return xml;
        }*/

        private string getXmlError(string mensaje)
        {
            string xml = "<interred-response-envelope version=\"1.0\">\n" +
                         "<interred-response>\n" +
                         "<param name=\"mensaje\" value=\"" + mensaje + "\"/>\n" +
                         "<param name=\"imprimir\" value=\"NO\"/>\n" +
                         "</interred-response>\n" +
                         "</interred-response-envelope>\n";

            return xml;

        }

        private string getXmlAmexError(string mensaje)
        {
            string xml = "<amex-response-envelope version=\"1.0\">\n" +
                         "<amex-response>\n" +
                         "<param name=\"mensaje\" value=\"" + mensaje + "\"/>\n" +
                         "<param name=\"imprimir\" value=\"NO\"/>\n" +
                         "</amex-response>\n" +
                         "</amex-response-envelope>\n";

            return xml;

        }       

        private string getXMLCancel(ServicioWebMultipagosAmex.CancelPagoResponseType oRespuesta,
                                    string tpoOperacion,
                                    string ingreso)
        {
            string xml = "";

            try
            {
                xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n" +
                             "<amex-response-envelope version=\"1.0\">\n" +
                             "<amex-response>\n";

                xml += "<param name=\"imprimir\" value=\"SI\"/>\n";

                if (oRespuesta.nombreComercio != null && oRespuesta.nombreComercio.Length > 0)
                    xml += "<param name=\"comercio\" value=\"" + oRespuesta.nombreComercio + "\"/>\n";

                if (oRespuesta.direccionComercio != null && oRespuesta.direccionComercio.Length > 0)
                    xml += "<param name=\"direccion\" value=\"" + oRespuesta.direccionComercio + "\"/>\n";

                if (oRespuesta.afiliacion != null && oRespuesta.afiliacion.Length > 0)
                    xml += "<param name=\"afiliacion\" value=\"" + oRespuesta.afiliacion + "\"/>\n";

                if (oRespuesta.afiliacion != null && oRespuesta.afiliacion.Length > 0)
                    xml += "<param name=\"serie\" value=\"" + oRespuesta.afiliacion + "\"/>\n";

                if (oRespuesta.horaTransaccion != null && oRespuesta.horaTransaccion.Length > 0)
                {
                    string fh = formateaFechaHora(oRespuesta.horaTransaccion);
                    fh = fh.Substring(0, 7);
                    xml += "<param name=\"fhTransaccion\" value=\"" + fh + "\"/>\n";
                }

                if (oRespuesta.horaTransaccion != null && oRespuesta.horaTransaccion.Length > 0)
                {
                    string hr = formateaFechaHora(oRespuesta.horaTransaccion);
                    hr = hr.Substring(7, 5);
                    xml += "<param name=\"hora\" value=\"" + hr + "\"/>\n";
                }

                if (oRespuesta.tdc != null && oRespuesta.tdc.Length > 0)
                    xml += "<param name=\"tarjeta\" value=\"" + "***********" + oRespuesta.tdc + "\"/>\n";

                if (oRespuesta.vencimiento != null && oRespuesta.vencimiento.Length > 0)
                    xml += "<param name=\"fhVencimiento\" value=\"" + oRespuesta.vencimiento + "\"/>\n";

                if (tpoOperacion != null && tpoOperacion.Length > 0)
                    xml += "<param name=\"operacion\" value=\"" + tpoOperacion + "\"/>\n";

                if (oRespuesta.monto != null && oRespuesta.monto.Length > 0)
                    xml += "<param name=\"monto\" value=\"" + oRespuesta.monto + "\"/>\n";

                if (ingreso != null && ingreso.Length > 0)
                    xml += "<param name=\"ingreso\" value=\"" + ingreso + "\"/>\n";

                if (oRespuesta.authorizationCode != null)
                    xml += "<param name=\"autorizacion\" value=\"" + oRespuesta.authorizationCode + "\"/>\n";

                xml += "<param name=\"consecutivo\" value=\"" + oRespuesta.consecutivoPago + "\"/>\n";

                xml += "<param name=\"firma\" value=\"_________________________\"/>\n";

                if (oRespuesta.titular != null && oRespuesta.titular.Length > 0)
                    xml += "<param name=\"titular\" value=\"" + oRespuesta.titular + "\"/>\n";

                xml += "</amex-response>\n" +
                       "</amex-response-envelope>\n";
            }
            catch (Exception)
            {
                // Error al generar el pagare, contacte a su administrador
                xml = getXmlError("412");
            }

            if (xml.Length == 0)
            {
                // Error al generar el pagare, contacte a su administrador
                xml = getXmlError("412");
            }

            return xml;
        }

        private string formateaFechaHora(string fecha)
        {
            string fmt = "";
            try
            {
                string anio = fecha.Substring(0, 2);
                string mes = fecha.Substring(2, 2);
                string dia = fecha.Substring(4, 2);
                string hora = fecha.Substring(6, 2);
                string minuto = fecha.Substring(8, 2);
                string segundo = fecha.Substring(10, 2);

                DateTime date = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), Convert.ToInt32(dia), Convert.ToInt32(hora), Convert.ToInt32(minuto), Convert.ToInt32(segundo), 000);
                fmt = String.Format("{0:ddMMMyyHH:mm}", date);
            }
            catch (FormatException)
            {
            }
            catch (ArgumentException)
            {
            }

            return fmt;
        }


        public string isBancomer(int entidad, string mail, string accion, string bin)
        {
            string xml = "";

            try
            {
                config();

                xml = webService.valida(entidad + "", bin, mail, accion);
            }
            catch (PinPadException ppe)
            {
                oRespuesta.setRes(getXmlError(ppe.Message));
            }
            catch (SoapException se)
            {
                xml = getXmlError("304");
                //logD.getLog().Info("isBancomer EL WEBSERVICE NO RESPONDE (304)");                
            }
            catch (WebException we)
            {
                xml = getXmlError("304");
                //logD.getLog().Info("isBancomer EL WEBSERVICE NO RESPONDE (304)");                
            }
            catch (NullReferenceException nre)
            {
                xml = getXmlError("311");
                //logD.getLog().Info("isBancomer (311)");                
            }
            catch (Exception e)
            {
                xml = getXmlError("305");
                //logD.getLog().Info("isBancomer Comuniquese con su administrador (305)");
                //logD.getLog().Info("Exception: " + e.Message);
            }


            return xml;
        }


        public string getStatus(int cdEntidad,
                                int cdNivel1,
                                int cdNivel2,
                                int tpServicio,
                                string secuencia,
                                string referencia,
                                string mail,
                                string acccion)
        {
            string res = "";
            config();

            try
            {

                res = webService.getStatus(cdEntidad,
                                           cdNivel1,
                                           cdNivel2,
                                           tpServicio,
                                           secuencia,
                                           referencia,
                                           mail,
                                           acccion);

            }
            catch (PinPadException ppe)
            {
                asignaXML("FAILURE", "5", ppe.Message);
            }
            catch (SoapException se)
            {
                asignaXML("FAILURE", "5", se.Message);
            }
            catch (WebException we)
            {
                asignaXML("FAILURE", "5", we.Message);
            }
            catch (Exception e)
            {
                asignaXML("FAILURE", "5", e.Message);
            }

            return res;
        }

        private string getTag(string tags, string tag)
        {
            string valor = "";
            int longitud = 0;
            int posicion = 0;

            posicion = tags.IndexOf(tag);

            if (posicion >= 0)
            {
                longitud = Convert.ToInt32(new Conversiones().HexadecimalDecimal(tags.Substring(posicion + tag.Length, 2).ToCharArray()));
                valor = tags.Substring(posicion + tag.Length + 2, longitud * 2);
            }

            return valor;
        }

        private string getTag50(string tags, string tag)
        {
            string valor = "";
            int longitud = 0;
            int posicion = 0;

            string tag4F = "";
            string tam4F = "";
            tag4F = tags.Substring(0, 2);
            tam4F = tags.Substring(2, 2);

            longitud = Convert.ToInt32(new Conversiones().HexadecimalDecimal(tag4F.ToCharArray()));
            valor = tags.Substring(posicion + tag4F.Length + 2, longitud * 2);
            posicion += valor.Length;
            posicion = tags.IndexOf(tag);

            if (posicion >= 0)
            {
                longitud = Convert.ToInt32(new Conversiones().HexadecimalDecimal(tags.Substring(posicion + tag.Length, 2).ToCharArray()));
                valor = tags.Substring(posicion + tag.Length + 2, longitud * 2);
            }

            return valor;
        }

        public Hashtable mapea(string xml)
        {
            string[] tokens = xml.Split('<');
            Hashtable hTokens = new Hashtable();

            foreach (string token in tokens)
            {
                int inicio = token.IndexOf("param name=");

                if (inicio >= 0)
                {
                    inicio = inicio + 12;

                    int fin = token.IndexOf("\"", inicio);

                    String parametro = "";
                    String valor = "";
                    if (inicio >= 0 && fin >= 0)
                    {
                        parametro = token.Substring(inicio, fin - inicio);

                        int iniciovalor = token.IndexOf("value=") + 7;
                        int finvalor = token.IndexOf("\"", iniciovalor);

                        if (iniciovalor > 0 && finvalor > 0)
                            valor = token.Substring(iniciovalor, finvalor - iniciovalor);

                        if (parametro.Length > 0)
                        {
                            hTokens.Add(parametro, valor);
                        }
                    }
                }
            }

            return hTokens;
        }

        //
        private string getXmlAmex(ServicioWebMultipagosAmex.InsPagoAmexResponseType oRespuesta,
                                  string tarjeta,
                                  string vencimiento,
                                  string tpoOperacion,
                                  string monto,
                                  string titular,
                                  string promocion,
                                  string criptograma,
                                  string script,
                                  string ingreso)
        {
            string xml = "";


            try
            {
                xml = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n" +
                             "<amex-response-envelope version=\"1.0\">\n" +
                             "<amex-response>\n";


                xml += "<param name=\"imprimir\" value=\"SI\"/>\n";

                if (oRespuesta.nombreComercio != null && oRespuesta.nombreComercio.Length > 0)
                    xml += "<param name=\"comercio\" value=\"" + oRespuesta.nombreComercio + "\"/>\n";

                if (oRespuesta.direccionComercio != null && oRespuesta.direccionComercio.Length > 0)
                    xml += "<param name=\"direccion\" value=\"" + oRespuesta.direccionComercio + "\"/>\n";

                if (oRespuesta.afiliacion != null && oRespuesta.afiliacion.Length > 0)
                    xml += "<param name=\"afiliacion\" value=\"" + oRespuesta.afiliacion.Trim() + "\"/>\n";

                if (oRespuesta.afiliacion != null && oRespuesta.afiliacion.Length > 0)
                    xml += "<param name=\"serie\" value=\"" + oRespuesta.afiliacion.Trim() + "\"/>\n";


                if (oRespuesta.horaTransaccion != null && oRespuesta.horaTransaccion.Length > 0)
                {
                    string fh = formateaFechaHora(oRespuesta.horaTransaccion);
                    fh = fh.Substring(0, 7);
                    xml += "<param name=\"fhTransaccion\" value=\"" + fh + "\"/>\n";
                }

                if (oRespuesta.horaTransaccion != null && oRespuesta.horaTransaccion.Length > 0)
                {
                    string hr = formateaFechaHora(oRespuesta.horaTransaccion);
                    hr = hr.Substring(7, 5);
                    xml += "<param name=\"hora\" value=\"" + hr + "\"/>\n";
                }

                if (tarjeta != null && tarjeta.Length > 0)
                {
                    int tam = tarjeta.Length - 4;
                    tarjeta = "***********" + tarjeta.Substring(tam);
                    xml += "<param name=\"tarjeta\" value=\"" + tarjeta + "\"/>\n";
                }

                if (vencimiento != null && vencimiento.Length > 0)
                    xml += "<param name=\"fhVencimiento\" value=\"" + vencimiento + "\"/>\n";

                xml += "<param name=\"emisor\" value=\"AMERICAN EXPRESS\"/>\n";

                if (tpoOperacion != null && tpoOperacion.Length > 0)
                    xml += "<param name=\"operacion\" value=\"" + tpoOperacion + "\"/>\n";

                if (monto != null && monto.Length > 0)
                    xml += "<param name=\"monto\" value=\"" + monto + "\"/>\n";

                if (ingreso != null && ingreso.Length > 0)
                    xml += "<param name=\"ingreso\" value=\"" + ingreso + "\"/>\n";

                if (oRespuesta.authorizationCode != null)
                    xml += "<param name=\"autorizacion\" value=\"" + oRespuesta.authorizationCode + "\"/>\n";

                xml += "<param name=\"consecutivo\" value=\"" + oRespuesta.consecutivoPago + "\"/>\n";

                xml += "<param name=\"firma\" value=\"_________________________\"/>\n";

                if (titular != null && titular.Length > 0)
                    xml += "<param name=\"titular\" value=\"" + titular + "\"/>\n";

                if (promocion != null && promocion.Length > 0)
                    xml += "<param name=\"promocion\" value=\"" + promocion + "\"/>\n";

                if (criptograma != null && criptograma.Length > 0)
                    xml += "<param name=\"criptograma\" value=\"" + criptograma + "\"/>\n";

                if (script != null && script.Length > 0)
                    xml += "<param name=\"script\" value=\"" + script + "\"/>\n";

                xml += "</amex-response>\n" +
                       "</amex-response-envelope>\n";

            }
            catch (Exception e)
            {
                // Error al generar el pagare, contacte a su administrador
                xml = getXmlError("412");

            }

            if (xml.Length == 0)
            {
                // Error al generar el pagare, contacte a su administrador
                xml = getXmlError("412");
            }

            return xml;
        }

        public string reimprime(string autorizacion,
                                string referencia,
                                int entidad,
                                string operacion,
                                int nivel1,
                                int nivel2,
                                int servicio,
                                string secuencia)
        {
            string pagare = "";

            config();
            try
            {
                pagare = webService.imprimePagare(autorizacion,
                                    referencia,
                                    entidad,
                                    operacion,
                                    "",
                                    nivel1,
                                    nivel2,
                                    servicio,
                                    secuencia);
            }
            catch (SoapException)
            {
                oRespuesta.setMensaje("Verifique la URL en el archivo pinpadd.config");
            }
            catch (WebException)
            {
                oRespuesta.setMensaje("EL WEBSERVICE NO RESPONDE");
            }
            catch (Exception e)
            {
                oRespuesta.setMensaje("Comuniquese con el administrador y reporte el siguiente error: " + e.Message);
            }

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

            config();
            try
            {
                pagare = webService.getPagareDeclinadoChip(entidad,
                                                           tarjeta,
                                                           importe,
                                                           "",
                                                           titular,
                                                           autorizacion,
                                                           servicio,
                                                           nivel1,
                                                           nivel2,
                                                           plazo);
            }
            catch (SoapException)
            {
                oRespuesta.setMensaje("Verifique la URL en el archivo pinpadd.config");
            }
            catch (WebException)
            {
                oRespuesta.setMensaje("EL WEBSERVICE NO RESPONDE");
            }
            catch (Exception e)
            {
                oRespuesta.setMensaje("Comuniquese con el administrador y reporte el siguiente error: " + e.Message);
            }

            return pagare;
        }

        private string asignaXML(string operacion, string code, string dscCode)
        {
            string xml = "";
            string AbreEncabezado = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"yes\"?>" +
                                    "<ntrd-response-envelope operation=\"" + operacion + "\" version=\"1.0\">";
            string CierraEncabezado = "</ntrd-response-envelope>";
            string statusCode = "<status code=\"" + code + "\">" + dscCode + "</status>";

            xml = AbreEncabezado;
            xml += statusCode;

            xml += CierraEncabezado;

            return xml;

        }

        private string getPagare(string afiliacion,
                                  string autorizacion,
                                  string comercio,
                                  string direccion,
                                  string fecha,
                                  string hora,
                                  string tarjeta,
                                  string vencimiento,
                                  string tpoOperacion,
                                  string monto,
                                  string titular,
                                  string promocion,
                                  string arqc,
                                  string ingreso,
                                  string aid,
                                  string alabel,
                                  string referencia,
                                  string serie,
                                  string firma)
        {
            string pagare = "";

            try
            {

                if (afiliacion != null && afiliacion.Length > 0)
                    pagare += "afiliacion=" + afiliacion.Trim() + "|";
                else
                    pagare += "afiliacion=|";

                if (autorizacion != null && autorizacion.Length > 0)
                    pagare += "autorizacion=" + autorizacion + "|";
                else
                    pagare += "autorizacion=|";

                if (comercio != null && comercio.Length > 0)
                    pagare += "comercio=" + comercio + "|";
                else
                    pagare += "comercio=|";


                if (arqc != null && arqc.Length > 0)
                    pagare += "criptograma=" + arqc + "|";
                else
                    pagare += "criptograma=|";

                if (direccion != null && direccion.Length > 0)
                    pagare += "direccion=" + direccion + "|";
                else
                    pagare += "direccion=|";

                pagare += "emisor=AMERICAN EXPRESS|";

                if (fecha != null && fecha.Length > 0)
                {
                    pagare += "fechaTransaccion=" + fecha + "|";
                }
                else
                    pagare += "fechaTransaccion=|";

                if (vencimiento != null && vencimiento.Length > 0)

                    pagare += "fechaVencimiento=" + vencimiento + "|";
                else
                    pagare += "fechaVencimiento=|";

                pagare += "firma=" + firma +"|";

                if (hora != null && hora.Length > 0)
                {
                    pagare += "horaTransaccion=" + hora + "|";
                }
                else
                    pagare += "horaTransaccion=|";

                if (monto != null && monto.Length > 0)
                    pagare += "monto=" + monto + "|";
                else
                    pagare += "monto=|";

                if (promocion != null && promocion.Length > 0)
                    pagare += "promocion=" + promocion + "|";
                else
                    pagare += "promocion=|";

                pagare += "razonSocial=BBVA BANCOMER|";

                if (serie != null && serie.Length > 0)
                    pagare += "serie=" + serie + "|";
                else
                    pagare += "serie=|";

                if (tarjeta != null && tarjeta.Length > 0)
                {
                    pagare += "tarjeta=" + tarjeta + "|";
                }
                else
                    pagare += "tarjeta=|";

                if (tpoOperacion != null && tpoOperacion.Length > 0)
                    pagare += "tipoOperacion=" + tpoOperacion + "|";
                else
                    pagare += "tipoOperacion=|";

                if (ingreso != null && ingreso.Length > 0)
                    pagare += "tipoPago=" + ingreso + "|";
                else
                    pagare += "tipoPago=|";

                if (titular != null && titular.Length > 0)
                    pagare += "titular=" + titular + "|";
                else
                    pagare += "titular=|";

                if (referencia != null && referencia.Length > 0)
                    pagare += "referencia=" + referencia + "|";
                else
                    pagare += "referencia=|";

                if (aid != null && aid.Length > 0)
                    pagare += "aid=" + aid + "|";
                else
                    pagare += "aid=|";

                if (alabel != null && alabel.Length > 0)
                {
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                    Conversiones con = new Conversiones();
                    string applicationLabel = alabel;

                    applicationLabel = encoding.GetString(Conversiones.keyBCD(Constantes.encoding.GetBytes(alabel)));
                    pagare += "label=" + applicationLabel + "|";
                }
                else
                    pagare += "label=|";
            }
            catch (Exception)
            {
                pagare = "afiliacion=|autorizacion=|comercio=criptograma=|direccion=|";
                pagare += "emisor=|fechaTransaccion=|fechaVencimiento=|firma=|horaTransaccion=|";
                pagare += "monto=|promocion=|razonSocial=|serie=|tarjeta=|operacion=|";
                pagare += "ingreso=|titular=|referencia=|";
            }
            return pagare;
        }

        private string enmascaraTarjeta(String tarjeta)
        {
            String enTarjeta = "";

            for (int i = 0; i < tarjeta.Length - 4; i++)
            {
                enTarjeta += "*";
            }

            enTarjeta = enTarjeta + tarjeta.Substring(tarjeta.Length - 4, 4);

            return enTarjeta;

        }
    }
}
