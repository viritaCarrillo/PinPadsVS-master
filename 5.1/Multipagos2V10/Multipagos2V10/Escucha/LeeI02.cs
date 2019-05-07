/*
 * Creado el 02/10/2015
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
    class LeeI02
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;
        private Log logD = new Log();
        private int reintentos = 0;

        public LeeI02(Puerto oPuerto, Tarjeta oTarjeta)
        {
            this.oTarjeta = oTarjeta;
            this.oPuerto = oPuerto;
            serialPort = oPuerto.getPuerto();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        /**
         * Lee los datos de respuesta del comando I02.
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

                        // Codigo de respuesta
                        char[] bCodigo = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setCodigoRespuesta(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bCodigo)));

                        //Si la lectura de la tarjeta no fue exitosa
                        if (!oTarjeta.getCodigoRespuesta().Equals("00"))
                        {
                            oTarjeta.setStatusLectura(2);
                        }
                        else
                        {
                            oTarjeta.setStatusLectura(1);
                        }
                    }
                }
            }
            catch (PinPadException pe)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError("" + pe.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> pe I02: -->" + pe.Message);
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex I02 -->: " + ex.Message);
            }
        }

        /**
         * Espera por la respuesta de los comando C31, C32 y C33.
         */
        public void espera()
        {
            // Se esperan datos del puerto serial.
            while (oTarjeta.getStatusLectura() == -1)
            {
                Thread.Sleep(5);
            }

            if (oTarjeta.getStatusLectura() != 1)
            {
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
            }
        }
    }
}
