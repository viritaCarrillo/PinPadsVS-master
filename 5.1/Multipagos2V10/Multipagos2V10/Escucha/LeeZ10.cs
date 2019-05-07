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
    class LeeZ10
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;

        public LeeZ10(Puerto oPuerto, Tarjeta oTarjeta)
        {
            this.oTarjeta = oTarjeta;
            this.oPuerto = oPuerto;
            serialPort = oPuerto.getPuerto();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        /**
        * Lee los datos de respuesta del comando Z10.
        */
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);

            try
            {
                oPuerto.leeACK();
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

                        // Codigo Respuesta
                        char[] bCodigo = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setCodigoRespuesta(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bCodigo)));

                        // Longitud de la trama
                        byte[] bLonTrama = { datos[++iPos], datos[++iPos] };
                        int iLonTrama = int.Parse(Conversiones.toHexString(bLonTrama), System.Globalization.NumberStyles.HexNumber);

                        byte []tokens = new byte[iLonTrama];
        			    for(int j=0; j < iLonTrama; j++)
        				    tokens[j] = datos[++iPos];

                        oTarjeta.setTagEMV(con.HexadecimalAscii(Conversiones.toHexString(tokens).ToCharArray()));

                        oTarjeta.setStatusLectura(1);
                    }
                    else
                    {
                        System.Console.WriteLine("LRC_INVALIDO");
                        oTarjeta.setStatusLectura(2);
                    }
                }
            }
            catch (PinPadException pe)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError("" + pe.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> pe Z10: -->" + pe.Message);
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex Z10 -->: " + ex.Message);
            }
        }

        /**
         * Espera por la respuesta del comando Z10.
         */
        public void espera()
        {
            while (oTarjeta.getStatusLectura() == -1) {
                Thread.Sleep(5);
            }
        }
    }
}
