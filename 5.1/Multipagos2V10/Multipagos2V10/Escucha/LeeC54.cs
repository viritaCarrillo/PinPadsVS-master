/*
 * Creado el 02/10/2015
 * @authora Lic. Selene Nochebuena Rojo
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
using System.Windows.Forms;

namespace Multipagos2V10.Escucha
{
    class LeeC54
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;

        public LeeC54(Puerto oPuerto, Tarjeta oTarjeta)
        {
            this.oPuerto = oPuerto;
            this.oTarjeta = oTarjeta;
            serialPort = oPuerto.getPuerto();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        /**
        * Lee los datos de respuesta del comando C54.
        */
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
            try
            {                
                byte[] datos = oPuerto.leeDatosXCaracter();

                if (datos != null)
                {
                    
                    if (Arreglos.isValidLrc(datos))
                    {
                        oPuerto.escribe(Comandos.ACK);

                        int iPos = 0;

                        // Comando de respuesta
                        char[] bComando = { (char)datos[++iPos], (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setComando(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bComando)));

                        // Codigo Respuesta
                        char[] bCodigo = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setCodigoRespuesta(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bCodigo)));

                       //
                        // Longitud de la trama
                       byte[] bLonTrama = { datos[++iPos], datos[++iPos] };
                       int iLongTrama = int.Parse(Conversiones.toHexString(bLonTrama), System.Globalization.NumberStyles.HexNumber);

                       if (iLongTrama > 0)
                       {
                            byte[] bDatos = new byte[iLongTrama];
                            for (int j = 0; j < iLongTrama; j++)
                                bDatos[j] = datos[++iPos];

                            String tagE2 = Conversiones.toHexString(bDatos);
                            int inicio = 0;

                            inicio = tagE2.IndexOf("9F27");

                            if (inicio >= 0)
                            {
                                String tag9F27 = tagE2.Substring(inicio += 6, 2);
                                oTarjeta.setTag9F27(tag9F27);
                            }
                        }
                       //


                        oTarjeta.setStatusLectura(1);
                    }
                    else
                    {
                        oPuerto.escribe(Comandos.NACK);
                        oTarjeta.setStatusLectura(2);
                    }
                }
            }
            catch (PinPadException pe)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError("" + pe.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> pe C34: -->" + pe.Message);
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex C34 -->: " + ex.Message);
            }
        }

        /**
         * Espera por la respuesta del comando C34.
         */
        public void espera()
        {
            int contador = 0;
            //while (oTarjeta.getStatusLectura() == -1 && contador < 120000)
            while (oTarjeta.getStatusLectura() == -1 && contador < 10000)
            { 
                Thread.Sleep(5);
                contador+=5;
            }
            
        }
    }
}
