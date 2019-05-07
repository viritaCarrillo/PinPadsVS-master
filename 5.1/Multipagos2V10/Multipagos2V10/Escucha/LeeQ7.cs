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
    class LeeQ7
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;

        public LeeQ7(Puerto oPuerto, Tarjeta oTarjeta)
        {
            this.oTarjeta = oTarjeta;
            this.oPuerto = oPuerto;
            serialPort = oPuerto.getPuerto();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        /**
        * Lee los datos de respuesta del comando Q7.
        */
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
            Conversiones con = new Conversiones();

            try
            {
                oPuerto.leeACK();
                byte[] datos = oPuerto.leeDatosXCaracter();

                if (datos != null)
                {
                    if (Arreglos.isValidLrc(datos))
                    {
                        oPuerto.escribe(Comandos.ACK);
                        Thread.Sleep(1000);
                        int iPos = 0;
                        // Comando de respuesta
                        char[] bComando = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setComando(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bComando)));

                        // Codigo de respuesta
                        char[] bCodigo = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setCodigoRespuesta(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bCodigo)));
                        Console.WriteLine("CODIGO: " + oTarjeta.getCodigoRespuesta());
                        oTarjeta.setStatusLectura(1);
                    }
                    else
                    {
                        System.Console.WriteLine("LRC_INVALIDO Q7");
                        oTarjeta.setStatusLectura(2);
                        oTarjeta.setMensaje("Error en la actualizacion");
                    }
                }
            }
            catch (PinPadException pe)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError("" + pe.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> pe Q7: -->" + pe.Message);
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex Q7 -->: " + ex.Message);
            }
        }

        /**
         * Espera por la respuesta del comando C12.
         */
        public void espera()
        {
            oTarjeta.setStatusLectura(-1);
            while (oTarjeta.getStatusLectura() == -1)
            {
                Thread.Sleep(5);
            }
        }
    }
}
