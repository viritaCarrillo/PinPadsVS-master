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
    class LeeC14
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;

        public LeeC14(Puerto oPuerto, Tarjeta oTarjeta)
        {
            this.oTarjeta = oTarjeta;
            this.oPuerto = oPuerto;
            serialPort = oPuerto.getPuerto();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        /**
        * Lee los datos de respuesta del comando C12.
        */
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);

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
                        char[] bComando = { (char)datos[++iPos], (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setComando(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bComando)));

                        // Codigo Respuesta
                        char[] bCodigo = { (char)datos[++iPos], (char)datos[++iPos] };
                        oTarjeta.setCodigoRespuesta(Constantes.encoding.GetString(Constantes.encoding.GetBytes(bCodigo)));

                        // Si la lectura del comando C12 es exitosa.
                        if (oTarjeta.getCodigoRespuesta().Equals("00"))
                        {
                            oTarjeta.setBEjecucion(true);
                            oTarjeta.setMensaje("CARGA DE BINES EXITOSA");
                        }
                        else
                        {
                            oTarjeta.setBEjecucion(false);
                            oTarjeta.setMensaje("FALLA AL CARGAR LOS BINES");
                        }

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
                System.Console.WriteLine("Error --> pe C25: -->" + pe.Message);
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex C25 -->: " + ex.Message);
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
