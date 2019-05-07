/*
 * Creado el 30/09/2015
 * @author Lic. Selene Nochebuena Rojo
 */

using System;
using System.Collections.Generic;
using System.Text;
using Multipagos2V10.Serial;
using Multipagos2V10.Util;
using System.Threading;
using System.IO.Ports;
using Multipagos2V10.VO;
using Multipagos2V10.Exceptions;

namespace Multipagos2V10.Escucha
{
    class LeeC51
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;
        //private int reintentos = 0;
        private int contador = 0;
        private int timeout = 60000;

        public LeeC51(Puerto oPuerto, Tarjeta oTarjeta)
        {
            this.oTarjeta = oTarjeta;
            this.oPuerto = oPuerto;
            serialPort = oPuerto.getPuerto();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        /**
         * Lee los datos de respuesta del comando C31.
         */
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);

            try
            {                

                byte[] datos = oPuerto.leeDatosXCaracter();
                Conversiones con = new Conversiones();

                if (datos != null)
                {
                    if (Arreglos.isValidLrc(datos))
                    {
                        oPuerto.escribe(Comandos.ACK);

                        int iPos = 0;

                        // Comando de respuesta
                        char[] bComando = { (char)datos[++iPos], (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setComando(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bComando)));

                        // Codigo de respuesta
                        char[] bCodigo = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setCodigoRespuesta(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bCodigo)));

                        if (oTarjeta.getComando().Equals("C53") || oTarjeta.getComando().Equals("C54") || oTarjeta.getComando().Equals("C51"))
                        {
	    				    String respuesta = "";
	    				    byte []bHeader = new byte[10];
	    				    String sHeader = "";
	    				    String lonToken = "";
	    				    int iLonToken = 0;
    		    				    				
	    				    // Longitud de la trama
	        			    byte[] bLonTrama = {datos[++iPos], datos[++iPos]};
	        			    int iLongTramaHexa = int.Parse(Conversiones.toHexString(bLonTrama), System.Globalization.NumberStyles.HexNumber);

                            if (iLongTramaHexa > 0)
                            {    	        			
		        			    ++iPos; // TAG C1
		        			    byte[] lonPan = {datos[++iPos]};    		        			
	            			    int iLonPan = int.Parse(Conversiones.toHexString(lonPan), System.Globalization.NumberStyles.HexNumber);

                                if (iLonPan > 0) {
                                    byte[] bDatosPan = new byte[iLonPan];
                                    for (int j = 0; j < iLonPan; j++)
                                        bDatosPan[j] = datos[++iPos];

                                    oTarjeta.setPan(Conversiones.toHexString(bDatosPan));

                                    
                                    if (oTarjeta.getPan().Contains("F"))
                                    {
                                        oTarjeta.setPan(oTarjeta.getPan().Replace("F",""));
                                    }
                                    
                                    //Console.WriteLine("tarjeta: -->" + oTarjeta.getPan() + "<--");

                                    string bin = oTarjeta.getPan().Substring(0, 6);

                                    //amex                                
                                    if ((Convert.ToInt64(bin) >= 340000 && Convert.ToInt64(bin) <= 349999) ||
                                    (Convert.ToInt64(bin) >= 350000 && Convert.ToInt64(bin) <= 359999) ||
                                    (Convert.ToInt64(bin) >= 370000 && Convert.ToInt64(bin) <= 379999))
                                    {
                                        oTarjeta.setPlataforma("1");
                                    }
	            			    }    	            			
    	            			
	            			    ++iPos; // TAG C1
		        			    byte[] lonTitular = {datos[++iPos]};    		        			   			
	            			    int iLonTitular = int.Parse(Conversiones.toHexString(lonTitular), System.Globalization.NumberStyles.HexNumber);
    	            			
	            			    if(iLonTitular > 0){
		            			    byte []bDatosTitular = new byte[iLonTitular];
		            			    for(int j=0; j < iLonTitular; j++)
		            				    bDatosTitular[j] = datos[++iPos];

                                    oTarjeta.setName(con.HexadecimalAscii(Conversiones.toHexString(bDatosTitular).ToCharArray()));
		            			    if(oTarjeta.getName() != null){
                                        oTarjeta.setName((oTarjeta.getName().Trim()));
		            			    }
	            			    }    	            			
    	            			
	            			    ++iPos; // TAG C1
		        			    byte[] lonTrack2 = {datos[++iPos]};    		        			
	            			    int iLonTrack2 = int.Parse(Conversiones.toHexString(lonTrack2), System.Globalization.NumberStyles.HexNumber);
    	            			
	            			    if(iLonTrack2 > 0){
		            			    byte []bDatosTrack2 = new byte[iLonTrack2];
		            			    for(int j=0; j < iLonTrack2; j++)
		            				    bDatosTrack2[j] = datos[++iPos];


                                    oTarjeta.setTrack2(Constantes.encoding.GetString(bDatosTrack2));
                                    oTarjeta.setTrack2(oTarjeta.getTrack2().Replace("D", "="));
                                    oTarjeta.setPan(oTarjeta.getPan().Substring(0,15));
    		            			
	            			    }
    	            			
	            			    ++iPos; // TAG C1
		        			    byte[] lonTrack1 = {datos[++iPos]};    		        			
	            			    int iLonTrack1 = int.Parse(Conversiones.toHexString(lonTrack1), System.Globalization.NumberStyles.HexNumber);
    	            			
	            			    if(iLonTrack1 > 0){
		            			    byte []bDatosTrack1 = new byte[iLonTrack1];
		            			    for(int j=0; j < iLonTrack1; j++){
		            				    bDatosTrack1[j] = datos[++iPos];
		            			    }


                                    oTarjeta.setTrack1(Constantes.encoding.GetString(bDatosTrack1));
                                    //System.Console.WriteLine("oTarjeta.getTrack1() -->" + oTarjeta.getTrack1() + "<--");
                                    //oTarjeta.setTrack1(oTarjeta.getTrack1().Replace("^", "*"));
                                    System.Console.WriteLine("oTarjeta.getTrack1() -->" + oTarjeta.getTrack1() + "<--");

	            			    }

	            			    ++iPos; // TAG C1
		        			    byte[] lonCVV2 = {datos[++iPos]};
	            			    int iLonCVV2 = int.Parse(Conversiones.toHexString(lonCVV2), System.Globalization.NumberStyles.HexNumber);

	            			    if(iLonCVV2 > 0){
		            			    byte []bDatosCVV2 = new byte[iLonCVV2];
		            			    for(int j=0; j < iLonCVV2; j++)
		            				    bDatosCVV2[j] = datos[++iPos];

                                    //oTarjeta.setCvv2(Conversiones.toHexString(bDatosCVV2));
                                    oTarjeta.setCvv2(con.HexadecimalAscii(Conversiones.toHexString(bDatosCVV2).ToCharArray()));
	            			    }

	            			    ++iPos; // TAG C1
		        			    byte[] lonLectura = {datos[++iPos]};    		        			
	            			    int iLonLectura = int.Parse(Conversiones.toHexString(lonLectura), System.Globalization.NumberStyles.HexNumber);
    	            			
	            			    if(iLonLectura > 0){
		            			    byte []bDatosLectura = new byte[iLonLectura];
		            			    for(int j=0; j < iLonLectura; j++)
		            				    bDatosLectura[j] = datos[++iPos];

                                    oTarjeta.setMdoLectura(con.HexadecimalAscii(Conversiones.toHexString(bDatosLectura).ToCharArray()));
		            			    respuesta += oTarjeta.getMdoLectura();
	            			    }
    	            			
	            			    respuesta +=  "ADQUIRA";
    	            			
	            			    ++iPos; // TAG E1
		        			    byte[] lonTAGE1 = {datos[++iPos]};
                                int iLonTAGE1 = int.Parse(Conversiones.toHexString(lonTAGE1), System.Globalization.NumberStyles.HexNumber);
    	            			
	            			    if(iLonTAGE1 > 0){
		            			    byte []bDatosTAGE1 = new byte[iLonTAGE1];
		            			    for(int j=0; j < iLonTAGE1; j++)
		            				    bDatosTAGE1[j] = datos[++iPos];

                                    oTarjeta.setTagE1(Conversiones.toHexString(bDatosTAGE1));
		            			    respuesta += oTarjeta.getTagE1();

		            			    int inicio = oTarjeta.getTagE1().IndexOf("9F27");
		            			    String tag9F27 =  oTarjeta.getTagE1().Substring(inicio, 8);
		            			    oTarjeta.setTag9F27(tag9F27.Substring(6));

	            			    }
    	            			
	            			    respuesta +=  "ADQUIRA";
    	            			
	            			    ++iPos; // TAG E2
		        			    byte[] lonTAGE2 = {datos[++iPos]};    		        			  			
	            			    int iLonTAGE2 = int.Parse(Conversiones.toHexString(lonTAGE2), System.Globalization.NumberStyles.HexNumber);
	            			    if(iLonTAGE2 > 0){
		            			    byte []bDatosTAGE2 = new byte[iLonTAGE2];
		            			    for(int j=0; j < iLonTAGE2; j++)
		            				    bDatosTAGE2[j] = datos[++iPos];

                                    oTarjeta.setTagEMV(Conversiones.toHexString(bDatosTAGE2));
		            			    respuesta += oTarjeta.getTagEMV();
	            			    }

                                if (oTarjeta.getTrack2() == null || oTarjeta.getTrack2().Length == 0)
                                {

                                    respuesta += "ADQUIRA";

                                    for (int j = 0; j < 10; j++)
                                        bHeader[j] = datos[++iPos];

                                    sHeader = con.HexadecimalAscii(Conversiones.toHexString(bHeader).ToCharArray());

                                    if (sHeader.Length == 10)
                                    {
                                        lonToken = sHeader.Substring(4, 5);
                                    }

                                    if (lonToken.Length > 0)
                                    {
                                        iLonToken = Int32.Parse(lonToken);
                                    }

                                    if (iLonToken > 0)
                                    {
                                        byte[] bTokenES = new byte[iLonToken];
                                        for (int j = 0; j < iLonToken; j++)
                                            bTokenES[j] = datos[++iPos];

                                        oTarjeta.setTokenES(sHeader + con.HexadecimalAscii(Conversiones.toHexString(bTokenES).ToCharArray()));
                                        respuesta += oTarjeta.getTokenES();

                                        string token = oTarjeta.getTokenES().Substring(31, 19);
                                        oTarjeta.setNumeroSerie(token.Substring(10));
                                    }
                                    else
                                    {
                                        oTarjeta.setTokenES(sHeader);
                                    }

                                    respuesta += "ADQUIRA";

                                    lonToken = "";
                                    iLonToken = 0;

                                    for (int i = 0; i < bHeader.Length; i++)
                                    {
                                        bHeader[i] = (byte)0;
                                    }

                                    for (int j = 0; j < 10; j++)
                                        bHeader[j] = datos[++iPos];

                                    sHeader = con.HexadecimalAscii(Conversiones.toHexString(bHeader).ToCharArray());
                                    if (sHeader.Length == 10)
                                    {
                                        lonToken = sHeader.Substring(4, 5);
                                    }

                                    if (lonToken.Length > 0)
                                    {
                                        iLonToken = Int32.Parse(lonToken);
                                    }

                                    if (iLonToken > 0)
                                    {
                                        byte[] bTokenR1 = new byte[iLonToken];

                                        for (int j = 0; j < iLonToken; j++)
                                            bTokenR1[j] = datos[++iPos];

                                        oTarjeta.setTokenR1(sHeader + con.HexadecimalAscii(Conversiones.toHexString(bTokenR1).ToCharArray()));
                                        respuesta += oTarjeta.getTokenR1();
                                    }
                                    else
                                    {
                                        oTarjeta.setTokenR1(sHeader);
                                    }

                                    respuesta += "ADQUIRA";

                                    lonToken = "";
                                    iLonToken = 0;

                                    for (int i = 0; i < bHeader.Length; i++)
                                    {
                                        bHeader[i] = (byte)0;
                                    }

                                    for (int j = 0; j < 10; j++)
                                        bHeader[j] = datos[++iPos];

                                    sHeader = con.HexadecimalAscii(Conversiones.toHexString(bHeader).ToCharArray());

                                    if (sHeader.Length == 10)
                                    {
                                        lonToken = sHeader.Substring(4, 5);
                                    }

                                    if (lonToken.Length > 0)
                                    {
                                        iLonToken = Int32.Parse(lonToken);
                                    }

                                    if (iLonToken > 0)
                                    {
                                        byte[] bTokenEZ = new byte[iLonToken];
                                        for (int j = 0; j < iLonToken; j++)
                                            bTokenEZ[j] = datos[++iPos];

                                        oTarjeta.setTokenEZ(sHeader + con.HexadecimalAscii(Conversiones.toHexString(bTokenEZ).ToCharArray()));
                                        respuesta += oTarjeta.getTokenEZ();
                                    }
                                    else
                                    {
                                        oTarjeta.setTokenEZ(sHeader);
                                    }

                                    respuesta += "ADQUIRA";

                                    lonToken = "";
                                    iLonToken = 0;
                                    for (int i = 0; i < bHeader.Length; i++)
                                    {
                                        bHeader[i] = (byte)0;
                                    }

                                    for (int j = 0; j < 10; j++)
                                        bHeader[j] = datos[++iPos];

                                    sHeader = con.HexadecimalAscii(Conversiones.toHexString(bHeader).ToCharArray());

                                    if (sHeader.Length == 10)
                                    {
                                        lonToken = sHeader.Substring(4, 5);
                                    }

                                    if (lonToken.Length > 0)
                                    {
                                        iLonToken = Int32.Parse(lonToken);
                                    }

                                    if (iLonToken > 0)
                                    {
                                        byte[] bTokenEY = new byte[172];
                                        for (int j = 0; j < 172; j++)
                                            bTokenEY[j] = datos[++iPos];


                                        oTarjeta.setTokenEY(sHeader + con.HexadecimalAscii(Conversiones.toHexString(bTokenEY).ToCharArray()));
                                        respuesta += oTarjeta.getTokenEY();
                                    }
                                    else
                                    {
                                        oTarjeta.setTokenEY(sHeader);
                                    }

                                    respuesta += "ADQUIRA";
                                    oTarjeta.setTagEMV(respuesta);
                                }
                                else
                                {
                                    for (int j = 0; j < 10; j++)
                                        bHeader[j] = datos[++iPos];

                                    sHeader = con.HexadecimalAscii(Conversiones.toHexString(bHeader).ToCharArray());

                                    if (sHeader.Length == 10)
                                    {
                                        lonToken = sHeader.Substring(4, 5);
                                    }

                                    if (lonToken.Length > 0)
                                    {
                                        iLonToken = Int32.Parse(lonToken);
                                    }

                                    if (iLonToken > 0)
                                    {
                                        byte[] bTokenES = new byte[iLonToken];
                                        for (int j = 0; j < iLonToken; j++)
                                            bTokenES[j] = datos[++iPos];

                                        oTarjeta.setTokenES(sHeader + con.HexadecimalAscii(Conversiones.toHexString(bTokenES).ToCharArray()));
                                        respuesta += oTarjeta.getTokenES();
                                        string token = oTarjeta.getTokenES().Substring(31, 19);
                                        oTarjeta.setNumeroSerie(token.Substring(10));
                                    }
                                }

                                //Si la lectura de la tarjeta no fue exitosa
                                if (!oTarjeta.getCodigoRespuesta().Equals("00"))
                                {
                                    oTarjeta.setStatusLectura(0);
                                    contador += timeout; 
                                }
                                else
                                {
                                    if ((oTarjeta.getCodigoRespuesta().Equals("10") ||
                                        oTarjeta.getCodigoRespuesta().Equals("21") ||
                                        oTarjeta.getCodigoRespuesta().Equals("22") ||
                                        oTarjeta.getCodigoRespuesta().Equals("42")) &&
                                        oTarjeta.getPan() != null && oTarjeta.getPan().Length > 0)
                                    {
                                        oTarjeta.setStatusLectura(0);
                                        contador += timeout; 
                                    }
                                    else
                                    {
                                        oTarjeta.setStatusLectura(2);
                                        contador += timeout; 
                                    }
                                }
	        			    }else{
    	        				
	        				    CodigosRespuesta oCodigo = new CodigosRespuesta();
		    				    oTarjeta.setDscCodRespuesta(oCodigo.getDescripcionCodigo(oTarjeta.getCodigoRespuesta()));
		    				    oTarjeta.setMensaje(oTarjeta.getDscCodRespuesta());
                                oTarjeta.setStatusLectura(2);
                                contador += timeout; 
	        			    }
	    			    }
                    }
                    else
                    {
                        oPuerto.escribe(Comandos.NACK);
                        oTarjeta.setStatusLectura(2);
                        contador += timeout; 

                        /*serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                        reintentos++;
                        if (reintentos == 3)
                        {
                            oTarjeta.setStatusLectura(2);
                            serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                        }*/
                    }
                }
            }
            catch (PinPadException pe)
            {
                oTarjeta.setStatusLectura(2);
                contador += timeout; 
                oTarjeta.setMensajeError("" + pe.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> pe C51: -->" + pe.Message);
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                contador += timeout; 
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex C51 -->: " + ex.Message);
            }
        }

        /**
         * Espera por la respuesta de los comando C31, C32 y C33.
         */
        public void espera()
        {
            // Se esperan datos del puerto serial.
            while (oTarjeta.getStatusLectura() == -1 && contador <= 60000)
            {
                Thread.Sleep(5);
                contador += 5;
            }

            if (oTarjeta.getStatusLectura() != 1)
            {
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
            }
        }
    }
}
