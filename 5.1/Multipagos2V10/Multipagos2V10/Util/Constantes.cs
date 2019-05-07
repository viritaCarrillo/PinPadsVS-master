using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.Util
{
    class Constantes
    {
        public static byte[] ACK = { (byte)0x06 };                                              // Envia un ACK a la pinpad.        
        public static byte[] NACK = { (byte)0x15 };                                             // Envia un NACK a la pinpad.    
        public static byte[] STX = { (byte)0x02 };                                              // Inicio de texto.
        public static byte[] ETX = { (byte)0x03 };                                              // Fin de texto.   
        public static byte[] LIMPIA_PAN = { (byte)0x1A };                                       // Limpia la pantalla de la pinpad.    
        public static byte[] COMANDO_Z2 = { (byte)'Z', (byte)'2' };                             // Envia un mensaje para ser desplegado en la pinpad
        public static byte[] COMANDO_I02 = { (byte)'I', (byte)'0', (byte)'2' };				                        // Termina la transaccion chip.
        public static byte[] TEST_PINPAD = { (byte)'1', (byte)'1' };                            // Envia un mensaje para ser desplegado en la pinpad

        
        public static byte[] LONGTIEMPO = { (byte)0x01 };				                        // Numero de bytes del tiempo de espera para la lectura del chip.
        public static byte[] TIEMPO = { (byte)0x40 };				                            // Tiempo de espera para la lectura del chip.
        public static byte[] LONGTERMINAL = { (byte)0x01 };				                        // Numero de bytes de terminal. 
        public static byte[] TERMINAL = { (byte)0x01 };				                            // Terminal.
        public static byte[] LONGFECHA = { (byte)0x03 };				                        // Numero de bytes de la fecha (AAMMDD).
        public static byte[] LONGHORA = { (byte)0x03 };				                            // Numero de byte de la hora (HHMMSS).
        public static byte[] LONGTPOTRAN = { (byte)0x01 };				                        // (C01) Numero de bytes del tipo de transaccion. 
        public static byte[] TPOTRAN = { (byte)0x01 };				                            // (C01) Tipo de transaccion (0 - credito, 1 - Debito).
        public static byte[] LONGMONTO = { (byte)0x04 };				                        // (C01) Numero de bytes del monto operacion.
        public static byte[] LONGMONTOBACK = { (byte)0x04 };				                    // (C01) Numero de bytes del monto back.
        public static byte[] MONTOBACK = { (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00 };	// (C01) Monto back.
        public static byte[] LONGCODE = { (byte)0x02 };			                                // (C01) Numero de bytes del codigo.
        public static byte[] CODE = { (byte)0x04, (byte)0x84 };	                                // (C01) Codigo del peso.
        public static byte[] LONGISSUERN = { (byte)0x01 };				                        // (C02) Numero de bytes del issurer number.
        public static byte[] ISSUERN = { (byte)0x00 };				                            // (C02) Issuer number.
        public static byte[] LONGACQUIRERN = { (byte)0x01 };				                    // (C02) Numero de bytes del acquirer number.
        public static byte[] ACQUIRERN = { (byte)0x00 };				                        // (C02) Acquirer Number.
        public static byte[] LONGTERMINALD = { (byte)0x01 };				                    // (C03) Numero de bytes de la desicion de la terminal.
        public static byte[] TERMINALD = { (byte)0x00 };				                        // (C03) Desicion de la terminal (0 - Sin desicion, 1 - Forzar en linea, 2 - Declinada).

        public static byte[] LONGRESHOST = { (byte)0x01 };				                        // Numero de bytes de la respuesta del host.
        public static byte[] RESHOST = { (byte)0x00 };				                            // (C34)Respuesta del host (0 - Existosa, 1 - rechazo, 2 - Fallo la conexion, 3 - Referencia autorizada, referencia rechazada).    
        public static byte[] CERODOS = { (byte)0x00 };				                            // Ceros.

        public static byte[] COMMAND_06 = { (byte)'0', (byte)'6' };                             // Obtiene el numero de serie de la pinpad.

        public static byte[] LONGTAGSh = { (byte)0x25 };					                    // LONG TAGS
        public static byte[] TAGC1 = { (byte)0xC1 };					                        // Tag para la entrada de parametros (TLV).
        public static byte[] TAGE1 = { (byte)0xE1 };					                        // Tag para la entrada de objetos. 
        public static byte[] TAG9F39 = { (byte)0x9F, (byte)0x39 };			                    // Tag 9F39.
        public static byte[] TAG5F34 = { (byte)0x5F, (byte)0x34 };			                    // Tag 9F34.
        public static byte[] TAG8A = { (byte)0x8A };					                        // Tag 8A.
        public static byte[] TAG5F2A = { (byte)0x5F, (byte)0x2A };			                    // Tag 5F2A.
        public static byte[] TAG82 = { (byte)0x82 };					                        // Tag 82.
        public static byte[] TAG84 = { (byte)0x84 }; 					                        // Tag 84.
        public static byte[] TAG91 = { (byte)0x91 };					                        // Tag 91.
        public static byte[] TAG95 = { (byte)0x95 };					                        // Tag 95.
        public static byte[] TAG9A = { (byte)0x9A };					                        // Tag 9A.
        public static byte[] TAG9C = { (byte)0x9C };					                        // Tag 9C.
        public static byte[] TAG9F02 = { (byte)0x9F, (byte)0x02 };			                    // Tag 9F02.
        public static byte[] TAG9F03 = { (byte)0x9F, (byte)0x03 };			                    // Tag 9F03.
        public static byte[] TAG9F09 = { (byte)0x9F, (byte)0x09 };			                    // Tag 9F09.
        public static byte[] TAG9F10 = { (byte)0x9F, (byte)0x10 };			                    // Tag 9F10.
        public static byte[] TAG9F1A = { (byte)0x9F, (byte)0x1A };			                    // Tag 9F1A.
        public static byte[] TAG9F1E = { (byte)0x9F, (byte)0x1E };			                    // Tag 9F1E.
        public static byte[] TAG9F26 = { (byte)0x9F, (byte)0x26 };			                    // Tag 9F26.
        public static byte[] TAG9F27 = { (byte)0x9F, (byte)0x27 };			                    // Tag 9F27.
        public static byte[] TAG9F33 = { (byte)0x9F, (byte)0x33 };			                    // Tag 9F33.
        public static byte[] TAG9F34 = { (byte)0x9F, (byte)0x34 };			                    // Tag 9F34.
        public static byte[] TAG9F35 = { (byte)0x9F, (byte)0x35 };			                    // Tag 9F35.
        public static byte[] TAG9F36 = { (byte)0x9F, (byte)0x36 };			                    // Tag 9F36.
        public static byte[] TAG9F37 = { (byte)0x9F, (byte)0x37 };			                    // Tag 9F37.
        public static byte[] TAG9F41 = { (byte)0x9F, (byte)0x41 };			                    // Tag 9F41.
        public static byte[] TAG9F53 = { (byte)0x9F, (byte)0x53 };			                    // Tag 9F53.
        public static byte[] TAG71 = { (byte)0x71 };					                        // Tag 9F71.
        public static byte[] TAG72 = { (byte)0x72 };					                        // Tag 9F72.
        public static byte[] TAG9F5B = { (byte)0x9F, (byte)0x5B };			                    // Tag 9F5B.
        public static byte[] TAG89 = { (byte)0x89 };                                            // Tag 89.       
        public static byte[] LON8A = { (byte)0x02 };                                            // Lonigud Tag 8A.   
        public static byte[] D8A = { (byte)0x30, (byte)0x30 };
        public static byte[] LONCODAUTORIZACION = { (byte)0x06 };                               // Longitud del codigo de autorizacion.
        public static byte[] LONTAG34 = { (byte)0x17 };                                         // Tag34
        public static byte[] TAG9F21 = { (byte)0x9F, (byte)0x21 };                              // Tag9F21

        public static byte[] COMMAND_Q4_ACT = { (byte)'Q', (byte)'4', (byte)'0' };              // Activa la banda magnetica.    
        public static byte[] COMMAND_Q4_DSACT = { (byte)'Q', (byte)'4', (byte)'1' };            // Desactiva la banda mangnetica.


        // interred 3.4
        public static byte[] COMANDO_C25 = { (byte)'C', (byte)'2', (byte)'5' };                 // COMANDO C25 (ejecucion de scripts).
        public static byte[] Bandera_C25 = { (byte)'1' };	                                    // Bandera de archivo 1 (limpiar el archivo antes de ejecutar).
        public static byte[] COMANDO_C12 = { (byte)'C', (byte)'1', (byte)'2' };                 // Comando C12 (ejecucion de scripts).
        public static byte[] COMANDO_72 = { (byte)'7', (byte)'2' };
        public static byte[] COMANDO_71 = { (byte)'7', (byte)'1' };

        // interred 5.1
        public static byte[] COMANDO_Z11 =  { (byte)'Z', (byte)'1', (byte)'1' };				// Envio de la llave inicial DUKPT
        public static byte[] COMANDO_Z10 =  { (byte)'Z', (byte)'1', (byte)'0' };				// Solicitud de llave aleotaria
        public static byte[] COMANDO_C14 =  { (byte)'C', (byte)'1', (byte)'4' };				// Para el envio de la tabla de bines de excepcion
        public static byte[] COMANDO_Z3 =   { (byte)'Z', (byte)'3' };					        // Para el envio del id de bines
        public static byte[] COMANDO_Q8 =   { (byte)'Q', (byte)'8' };					        // Marca, modelo y version de la terminal
        public static byte[] COMANDO_Q7  =  { (byte)'Q', (byte)'7'};					        // Transmite archivos a la terminal
        public static byte[] LONGITUD_Q7 = { (byte)0x03, (byte)0x00 };                            // Transmite archivos a la terminal		                        // Transmite archivos a la terminal
        public static byte[] CEROS_Q7     = {(byte)0x0,(byte)0x0};
        public static byte[] COMANDO_C51 = { (byte)'C', (byte)'5', (byte)'1' };				    // COMANDO C51.
        public static byte[] LONGCOMANDOC51 = {(byte)0x00, (byte)0x4A};                         // Numero de bytes del comando C51.
        public static byte[] LONGMD    = {(byte)0x01};					                        // Longitud de la desicion del comercio.        
        public static byte[] SINRESPUESTAHOST    = {(byte)0x02};
        public static byte[] RECHAZADA    = {(byte)0x01};
        public static byte[] SINCRONIZAR    = {(byte)0x03};
        public static byte[] FALLIDO = { (byte)0x04 };
        public static byte[] COMANDO_C54 = { (byte)'C', (byte)'5', (byte)'4' };				    // COMANDO_C54.
        public static byte[] TAGE2     =  {(byte)0xE2};
        public static byte[] TAGSE2    = {(byte)0x0D, (byte)0x9F,(byte)0x26, (byte)0x9F,(byte)0x27,(byte)0x9F, (byte)0x36, 
    										(byte)0x95, (byte)0x9F,(byte)0x10,(byte)0x9F,(byte)0x37,(byte)0x9B,(byte)0x8A};
        public static byte[] LONRESPUESTA    = {(byte)0x02};
        public static byte[] EXITOSA    = {(byte)0x00};
        
        public static ASCIIEncoding encoding = new ASCIIEncoding();

        // Indica el estado de la lectura en el puerto serial (0 - Se permite la lectura, 1 - Leyendo , 2 - Sin permisos de lectura)
        public static int PERMISOS_LECTURA = 0;

        // Archivo de configuracion del COMMM, PUERTO y PROXI
        public static string ARCHIVO_CONFIGURACION = "pinpad.config";

        // Ruta donde se encuentran los archivos de configuracion
        public static string RUTA_CONFIGURACION = "C://flap/config/";

        public static int STATUS_BANDA = 1;

        // Tamanios de las fuentes para la impresion del pagare.
        public static int FUENTE_MEDIA = 273;
        public static int FUENTE_CHICA = 318;
        public static int TIMEOUT = 10000;

    }
}
