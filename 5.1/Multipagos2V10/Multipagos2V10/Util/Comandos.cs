using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.Util
{
    class Comandos : Constantes
    {
        /**
        * Construye el comando Z2, que se encarga de desplegar un mensaje en la pinpad.
        * @param message - Mensaje a desplegar.
        * @param clean - <b>true</b> limpia la pantalla, <b>false</b> No limpia la pantalla
        * @return El comando Z2.
        */
        public static byte[] getComandoZ2(string mensaje, bool clean)
        {
            byte[] comando = null;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();


            comando = STX;														// Inicio de texto                        
            if (mensaje.Length > 32)											// Valida el tamaño del mensaje (32 caracteres)
                mensaje = mensaje.Substring(0, 32);

            comando = Arreglos.concatena(comando, COMANDO_Z2);					// Comando Z2 (Desplegar mensaje).                
            if (clean)
                comando = Arreglos.concatena(comando, LIMPIA_PAN);				// Limpia la pantalla.   

            comando = Arreglos.concatena(comando, encoding.GetBytes(mensaje));	// Mensaje a desplegar        
            comando = Arreglos.concatena(comando, ETX);							// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));	// LRC        

            return comando;
        }

        /**
     * Construye el comando I02, indica a la terminal que retire la tarjeta.
     * @return El comando I02.
     */
        public static byte[] getComandoI02()
        {
            byte[] comando = null;

            comando = STX;														    // Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_I02);						// Comando Z2 (Desplegar mensaje).                
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

      
     /**
     * Construye el comando C51, que se encargan de obtener los datos de la tarjeta.
     * @param sMonto - Monto de operaci&oacute;n.
     * @param tpoTransaccion - Tipo de transaccion. 3 - Todas excepto devolucion (interedes que soportan amex)
     *  4 - Devolución (interredes que soportan amex)
     * @param cdMoneda - Codigo de moneda. 0 - Pesos, 1 - Dolares
     * @param consultaPuntos - Identificador de consulta de puntos. true -consulta de puntos, false - no es consulta de puntos
     * @return El comando C51.
     */
        public static byte[] setComandoC51(string sMonto, int tpoTransaccion, int cdMoneda, bool consultaPuntos)
        {
            byte[] result = null;

            sMonto = sMonto.Replace(".", "");

            String sHexaMonto = Convert.ToInt64(sMonto).ToString("X");

            string sCeros = "";

            for (int i = sHexaMonto.Length; i < 8; i++)
                sCeros += "0";

            sHexaMonto = sCeros + sHexaMonto;
            byte[] mtoBcd = Conversiones.keyBCD(Constantes.encoding.GetBytes(sHexaMonto));

            // Asigna la fecha y la hora del sistema
            string sFechaHora = DateTime.Now.ToString("yyMMdd hhmmss");
            string sFecha = sFechaHora.Substring(0, 6);
            string sHora = sFechaHora.Substring(7, 6);

            // Convierte la fecha y la hora en formato BCD
            byte[] bFecha = Conversiones.keyBCD(Constantes.encoding.GetBytes(sFecha));
            byte[] bHora = Conversiones.keyBCD(Constantes.encoding.GetBytes(sHora));
        
            // tipo de transaccion (para interredes que no soportan amex)
            byte[] tpoTran = new byte[1];
            if(tpoTransaccion == 0){		// 0 - cancelacion
        	    tpoTran[0] = (byte)0x11;
            }else if(tpoTransaccion == 1){  // 1 - todas excepto devolución 
        	    tpoTran[0] = (byte)0x01;
            }else if(tpoTransaccion == 2){  // 2 - devolucion
        	    tpoTran[0] = (byte)0x02;
            }else if(tpoTransaccion == 3){  // 3 - Para transacciones amex (todas excepto devolución)
        	    tpoTran[0] = (byte)0x03;
            }else if(tpoTransaccion == 4){  // 4 - Para transacciones amex
        	    tpoTran[0] = (byte)0x04;
            }
        
        
            // moneda
            byte[] moneda = new byte[2];        
            if(cdMoneda == 0){
        	    moneda[0] = (byte)0x4;
        	    moneda[1] = (byte)0x84;
            }else if(cdMoneda == 1){
        	    moneda[0] = (byte)0x08;
        	    moneda[1] = (byte)0x40;
            }

            // consulta de puntos
            byte[] consulta = new byte[1];        
            if(consultaPuntos)
        	    consulta[0] = (byte)0x01;
            else 
        	    consulta[0] = (byte)0x00;
        
            result = STX ;												    // Inicio de texto.       
            result = Arreglos.concatena( result, COMANDO_C51 );				// Comando C31.      
            result = Arreglos.concatena( result, LONGCOMANDOC51 );			// Numero de bytes del comando C31.
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGTIEMPO );				// Numero de bytes del tiempo de espera.
            result = Arreglos.concatena( result, TIEMPO );					// Numero de segundos que se espera para la lectura del chip.
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGFECHA );        		// Numero de bytes de la fecha.                        
            result = Arreglos.concatena( result, bFecha );					// Fecha del servidor.        
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGHORA );				// Numero de bytes de la hora del servidor.
            result = Arreglos.concatena( result, bHora );					// Hora del servidor.
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGTPOTRAN );				// Numero de bytes del tipo de transaccion.
            result = Arreglos.concatena( result, tpoTran );					// Tipo de transaccion (1- Compra/Cancelacio, 2 - Devolucion) (No se usa)
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGMONTO );				// Numero de bytes del monto de operacion.
            result = Arreglos.concatena( result, mtoBcd);					// Monto de operacion. BCD
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGMONTOBACK );
            result = Arreglos.concatena( result, MONTOBACK );				// Monto Back.
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGCODE );				// Numero de bytes del code.
            result = Arreglos.concatena( result, moneda );					// Codigo de moneda.
            result = Arreglos.concatena( result, TAGC1 );					// TAG C1.
            result = Arreglos.concatena( result, LONGMD );					// Lon Merchant D.
            result = Arreglos.concatena( result, consulta );					// 00 - El pinpad no tiene desicion, 01 forzar en linea                                                                          
            result = Arreglos.concatena( result, TAGE1 );					// TAG E1.        
            result = Arreglos.concatena( result, LONGTAGSh );				// Numero de bytes de los TAGS.
            result = Arreglos.concatena( result, TAG5F2A );        			// TAG 5F2A.
            result = Arreglos.concatena( result, TAG82 );					// TAG 82.
            result = Arreglos.concatena( result, TAG84 );					// TAG 84.
            result = Arreglos.concatena( result, TAG95 );					// TAG 95.
            result = Arreglos.concatena( result, TAG9A );					// TAG 9A.        
            result = Arreglos.concatena( result, TAG9C );        			// TAG 9C.
            result = Arreglos.concatena( result, TAG9F02 );        			// TAG 9F02.
            result = Arreglos.concatena( result, TAG9F03 );        			// TAG 9F03.
            result = Arreglos.concatena( result, TAG9F09);        			// TAG 9F09.
            result = Arreglos.concatena( result, TAG9F10 );        			// TAG 9F10.
            result = Arreglos.concatena( result, TAG9F1A );        			// TAG 9F1A.
            result = Arreglos.concatena( result, TAG9F1E );					// TAG 9F1E.
            result = Arreglos.concatena( result, TAG9F26 );					// TAG 9F26.
            result = Arreglos.concatena( result, TAG9F27 );					// TAG 9F27.
            result = Arreglos.concatena( result, TAG9F33 );					// TAG 9F33.
            result = Arreglos.concatena( result, TAG9F34 );					// TAG 9F34.
            result = Arreglos.concatena( result, TAG9F35 );					// TAG 9F35.
            result = Arreglos.concatena( result, TAG9F36 );					// TAG 9F36.
            result = Arreglos.concatena( result, TAG9F37 );					// TAG 9F37.
            result = Arreglos.concatena( result, TAG9F41 );					// TAG 9F41.
            result = Arreglos.concatena( result, TAG9F53 );					// TAG 9F53.                    
            result = Arreglos.concatena( result, ETX );						// Fin de texto.        
            result = Arreglos.concatena( result, Arreglos.getXOR( result ) );	// LRC.        
            
            return result;
        }

        /**
        * Construye el comando C54, que se encarga de terminar la transacci&oacute;n.
        * @return El comando C54.
        */
        public static byte[] setComandoC54(string cdAutorizacion, string criptograma, string cdRespuesta, string ingreso){
        byte[] tmp = null;
        byte[] result = null;
        byte []lonautorizacion = new byte[1];         
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

        // Asigna la fecha y la hora del sistema
        string sFechaHora = DateTime.Now.ToString("yyMMdd hhmmss");
        string sFecha = sFechaHora.Substring(0, 6);
        string sHora = sFechaHora.Substring(7, 6);

        // Convierte la fecha y la hora en formato BCD
        byte[] bFecha = Conversiones.keyBCD(Constantes.encoding.GetBytes(sFecha));
        byte[] bHora = Conversiones.keyBCD(Constantes.encoding.GetBytes(sHora));

        
        tmp = TAGC1 ;						// TAG C1.
        tmp = Arreglos.concatena( tmp, LONGRESHOST );				// Numero de bytes de la respuesta del host.

        if (cdRespuesta.Equals("00"))
        {
            tmp = Arreglos.concatena(tmp, EXITOSA);				// Exitosa
            lonautorizacion[0] = (byte)0x06;
        }
        else
        {
            int respuesta = 0;
            bool sinRespuesta = false;
            respuesta = Int32.Parse(cdRespuesta);

            if (respuesta == 100)
            {
                tmp = Arreglos.concatena(tmp, SINRESPUESTAHOST);
            }
            else
            {
                tmp = Arreglos.concatena(tmp, RECHAZADA);
            }
            if (cdRespuesta.Equals("03"))
            {
                tmp = Arreglos.concatena(tmp, SINCRONIZAR);	// Abortar transaccion - Sincronizar pinpad
            }                
            lonautorizacion[0] = (byte)0x00;

            if (respuesta >= 100)
            {
                if (respuesta == 100 || respuesta == 103 || respuesta == 503)
                {
                    cdRespuesta = cdRespuesta.Substring(0, 2);
                }
                else
                {
                    cdRespuesta = cdRespuesta.Substring(1, 2);
                }
            }
        }

        tmp = Arreglos.concatena( tmp, TAGC1 );						// TAG C1.        
        tmp = Arreglos.concatena( tmp, lonautorizacion );               
        if(cdAutorizacion.Length == 6){
        	tmp = Arreglos.concatena( tmp, encoding.GetBytes(cdAutorizacion) );
    	}
        
        tmp = Arreglos.concatena( tmp, TAGC1 );						// TAG C1.
        if(cdRespuesta.Equals("3") ){
        	tmp = Arreglos.concatena( tmp, CERODOS );        	
        }else{
        	tmp = Arreglos.concatena( tmp, LONRESPUESTA );
            tmp = Arreglos.concatena(tmp, encoding.GetBytes(cdRespuesta));
        }
        
        
        // tag 91
        if(criptograma != null && criptograma.Length > 0){
            byte [] criptoBCD = Conversiones.keyBCD(encoding.GetBytes(criptograma));
	        tmp = Arreglos.concatena( tmp, criptoBCD );

        }else{        	
        	byte []cripto = {(byte)0x91, (byte)0x00};
        	tmp = Arreglos.concatena( tmp, cripto );
        }
        
        tmp = Arreglos.concatena( tmp, TAGC1 );
        if(!cdRespuesta.Equals("3")){
        	tmp = Arreglos.concatena( tmp, LONGFECHA );
        	tmp = Arreglos.concatena( tmp, bFecha );
        }else{
        	tmp = Arreglos.concatena( tmp, CERODOS );
        }
                        
        tmp = Arreglos.concatena( tmp, TAGC1 );
        if(!cdRespuesta.Equals("03")){
        	tmp = Arreglos.concatena( tmp, LONGHORA );
        	tmp = Arreglos.concatena( tmp, bHora );
        }else{
        	tmp = Arreglos.concatena( tmp, CERODOS );
        }
           
        
        tmp = Arreglos.concatena( tmp, TAGE2 );
        if (ingreso != null && ingreso.Length >0 && ingreso.Equals("05") && !cdRespuesta.Equals("03"))
        {	                      
        	tmp = Arreglos.concatena( tmp, TAGSE2 );
        }else{
        	tmp = Arreglos.concatena( tmp, CERODOS );
        }
        
        String longitud = Convert.ToInt64(tmp.Length).ToString("X");

        for(int i=0; longitud.Length<4; i++){
        	longitud = "0" + longitud;
        }

        byte []bLonC34BCD = new byte[2];
        byte []bLonC34 = Conversiones.keyBCD(encoding.GetBytes(longitud));
        

        result = STX ;												        // Inicio de texto.        
        result = Arreglos.concatena( result, COMANDO_C54 );				    // Comando C34.
        result = Arreglos.concatena(result, bLonC34);				        // Numero de bytes del comando C34.
        result = Arreglos.concatena( result, tmp);
        result = Arreglos.concatena( result, ETX );						    // Fin de texto.
        result = Arreglos.concatena( result, Arreglos.getXOR( result ) );	// LRC.        
        
        return result;
    }

        public static byte[] setComandoC12(String tpoScript)
        {
            byte[] result = null;

            result = STX;
            result = Arreglos.concatena(result, COMANDO_C12);				// Comando C12

            if (tpoScript.Equals("71"))
                result = Arreglos.concatena(result, COMANDO_71);			// script 71
            else if (tpoScript.Equals("72"))
                result = Arreglos.concatena(result, COMANDO_72);			// script 72

            result = Arreglos.concatena(result, ETX);						// Fin de texto.                
            result = Arreglos.concatena(result, Arreglos.getXOR(result));	// LRC.

            return result;
        }

        public static byte[] setComandoC25(byte[] tpoScript, byte[] numScripts, byte[] longScripts, String datos)
        {
            byte[] result = null;

            result = STX;												                // Inicio de texto.
            result = Arreglos.concatena(result, COMANDO_C25);				            // Comando C25.        
            result = Arreglos.concatena(result, tpoScript);				                // Tipo de script (71 o 72).
            result = Arreglos.concatena(result, Bandera_C25);				            // Bandera del archivo (fijo 1 - Limpiar antes de ejecutar)
            result = Arreglos.concatena(result, numScripts);				            // Numero de script (mayor a 249 - 81 para la primera parte , 91 para la segunda).
            result = Arreglos.concatena(result, longScripts);				            // Longitud del script.
            result = Arreglos.concatena(result, Constantes.encoding.GetBytes(datos));   // Datos del script.
            result = Arreglos.concatena(result, ETX);						            // Fin de texto.
            result = Arreglos.concatena(result, Arreglos.getXOR(result));	            // LRC.

            return result;
        }

        /*public static byte[] setComandoC34(string cdAutorizacion, string criptograma)
        {
            byte[] result = null;

            // Asigna la fecha y la hora del sistema
            string sFechaHora = DateTime.Now.ToString("yyMMdd hhmmss");
            string sFecha = sFechaHora.Substring(0, 6);
            string sHora = sFechaHora.Substring(7, 6);

            // Convierte la fecha y la hora en formato BCD
            byte[] bFecha = Conversiones.keyBCD(Constantes.encoding.GetBytes(sFecha));
            byte[] bHora = Conversiones.keyBCD(Constantes.encoding.GetBytes(sHora));

            int longC34 = 28;

            if (criptograma != null && criptograma.Length > 0)
                longC34 += criptograma.Length / 2;
            else
                longC34 += 2;

            string longitud = Convert.ToString(longC34);

            longitud = Convert.ToInt64(longitud).ToString("X");

            for (int i = 0; longitud.Length < 4; i++)
            {
                longitud = "0" + longitud;
            }

            byte[] bLonC34 = Conversiones.keyBCD(Constantes.encoding.GetBytes(longitud));

            result = STX;												    // Inicio de texto.        
            result = Arreglos.concatena(result, COMANDO_C34);			    // Comando C34.
            result = Arreglos.concatena(result, bLonC34);		            // Numero de bytes del comando C34.        
            result = Arreglos.concatena(result, TAGC1);					    // TAG C1.
            result = Arreglos.concatena(result, LONGRESHOST);			    // Numero de bytes de la respuesta del host.
            result = Arreglos.concatena(result, RESHOST);				    // Respuesta del host
            result = Arreglos.concatena(result, TAGE1);                     // TAG E1.

            byte[] LONTAG34 = new byte[1];
            if (criptograma != null && criptograma.Length > 0)
            {
                LONTAG34[0] = (byte)0x23;
            }
            else
                LONTAG34[0] = (byte)0x17;

            result = Arreglos.concatena(result, LONTAG34);                  // Longitud del tag 34.
            result = Arreglos.concatena(result, TAG89);                     // Tag 89.
            result = Arreglos.concatena(result, LONCODAUTORIZACION);        // Longitud del codigo de autorizacion (6 Digitos)
            result = Arreglos.concatena(result, Constantes.encoding.GetBytes(cdAutorizacion));  // Codigo de autorizacion.

            if (criptograma != null && criptograma.Length > 0)
            {	// criptograma enviado por el emisor
                byte[] bcd = Conversiones.keyBCD(Constantes.encoding.GetBytes(criptograma));
                result = Arreglos.concatena(result, bcd);
            }

            result = Arreglos.concatena(result, TAG8A);                     // Tag 8A.
            result = Arreglos.concatena(result, LON8A);                     // Longitud del tag 8A.
            result = Arreglos.concatena(result, D8A);                       // Valor del tag 8A.
            result = Arreglos.concatena(result, TAG9A);                     // Tag 9A.
            result = Arreglos.concatena(result, LONGFECHA);                 // Longitud de la fecha (formato yymmdd).
            result = Arreglos.concatena(result, bFecha);                    // Fecha (en formato BCD)
            result = Arreglos.concatena(result, TAG9F21);                   // Tag 9F21.
            result = Arreglos.concatena(result, LONGHORA);                  // Loogitud de la hora (formato hhmmss).
            result = Arreglos.concatena(result, bHora);                     // Hora en formato BCD.
            result = Arreglos.concatena(result, ETX);						// Fin de texto.
            result = Arreglos.concatena(result, Arreglos.getXOR(result));	// LRC.      

            return result;
        }*/

        /**
     * Construye el comando 72, cancela todos los servicios pendientes en el pinpad.
     * @return El comando 72.
     */
        public static byte[] getComando72()
        {
            byte[] comando = null;

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_72);						// Limpiar pinpad de procesos pendientes.                
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

        /**
     * Construye el comando Z11, envio de la llave inicial DUKPT.
     * @return El comando Z11.
     */
        public static byte[] getComandoZ11(String tokenEX)
        {
            byte[] comando = null;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_Z11);						// LLave inicial DUKPT.
            comando = Arreglos.concatena(comando, encoding.GetBytes(tokenEX));		// Token EX
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

        /**
     * Construye el comando Z10, solicitud de llave aleatoria.
     * @return El comando Z10.
     */
        public static byte[] getComandoZ10()
        {
            byte[] comando = null;

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_Z10);						// LLave inicial DUKPT.
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

        /**
     * Construye el comando C14, para el envio de la tabla de excepcion.
     * @return El comando C14.
     */
        public static byte[] getComandoC14(String tokenET)
        {
            byte[] comando = null;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_C14);						// Comando C14
            comando = Arreglos.concatena(comando, encoding.GetBytes(tokenET));		// Token ES
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

        /**
     * Construye el comando CZ3, para del id de bines de excepcion.
     * @return El comando CZ3.
     */
        public static byte[] getComandoZ3(String idBines)
        {
            byte[] comando = null;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_Z3);						// Comando Z3
            comando = Arreglos.concatena(comando, encoding.GetBytes(idBines));		// Token ES
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

        /**
     * Construye el comando Q8, recupera marca, modelo y version de la terminal.
     * @return El comando Q8.
     */
        public static byte[] getComandoQ8()
        {
            byte[] comando = null;

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_Q8);						// Comando Q8
            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }

        /**
     * Construye el comando Q7, transmite archivos al pinpad.
     * @return El comando Q7.
     */
        public static byte[] getComandoQ7(byte[] datos)
        {
            byte[] comando = null;

            string longitud = datos.Length.ToString();
            longitud = Convert.ToInt64(longitud).ToString("X");

            for (int i = 0; longitud.Length < 4; i++)
            {
                longitud = "0" + longitud;
            }

            Console.WriteLine("longitud: " + longitud);

            byte[] bLon = Conversiones.keyBCD(Constantes.encoding.GetBytes(longitud));

            comando = STX;															// Inicio de texto                        
            comando = Arreglos.concatena(comando, COMANDO_Q7);						// Comando Q7

            if (datos != null)
            { // no va al terminar la transmision
                comando = Arreglos.concatena(comando, bLon);					// Longitud del bloque a transmitir
            }
            else
            {
                comando = Arreglos.concatena(comando, CEROS_Q7);					// Longitud del bloque a transmitir
            }

            if (datos != null)
            { // no va al terminar la transmision
                comando = Arreglos.concatena(comando, datos);						// datos a trasmitir a la terminal
            }

            comando = Arreglos.concatena(comando, ETX);								// Fin de texto        
            comando = Arreglos.concatena(comando, Arreglos.getXOR(comando));		// LRC        

            return comando;
        }
    }
}
