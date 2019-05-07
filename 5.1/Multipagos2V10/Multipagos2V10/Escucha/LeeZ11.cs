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
    class LeeZ11
    {
        private Puerto oPuerto;
        private Tarjeta oTarjeta;
        private SerialPort serialPort;
        private int contador = 0;

        public LeeZ11(Puerto oPuerto, Tarjeta oTarjeta)
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

                //Quitar o poner dependiendo de los comenrarios de arriba
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


                        Console.WriteLine("codigo: " + oTarjeta.getCodigoRespuesta());
                        if (oTarjeta.getCodigoRespuesta().Equals("00"))
                        {
                            oTarjeta.setMensaje("CARGA EXITOSA");
                        }
                        else
                        {
                            oTarjeta.setMensaje("FALLA AL CARGAR LA LLAVE");
                        }

                        //Si la lectura de la tarjeta no fue exitosa
                        if (!oTarjeta.getCodigoRespuesta().Equals("00"))
                        {
                            oTarjeta.setStatusLectura(2);
                            contador += Constantes.TIMEOUT;
                        }
                        else
                        {
                            oTarjeta.setStatusLectura(1);
                            contador += Constantes.TIMEOUT;
                        }
                    }
                    else
                    {
                        oTarjeta.setMensaje("FALLA AL CARGAR LA LLAVE");
                    }
                }
                else
                {
                    oTarjeta.setMensaje("FALLA AL CARGAR LA LLAVE");
                }
            }
            catch (PinPadException pe)
            {
                oTarjeta.setStatusLectura(2);
                contador += Constantes.TIMEOUT;
                oTarjeta.setMensajeError("" + pe.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> pe Z11: -->" + pe.Message);
                oTarjeta.setMensaje("FALLA AL CARGAR LA LLAVE");
            }
            catch (Exception ex)
            {
                oTarjeta.setStatusLectura(2);
                contador += Constantes.TIMEOUT;
                oTarjeta.setMensajeError(ex.Message);
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(port_DataReceived);
                System.Console.WriteLine("Error --> ex Z11 -->: " + ex.Message);
                oTarjeta.setMensaje("FALLA AL CARGAR LA LLAVE");
            }
        }

        /**
         * Espera por la respuesta de los comando C31, C32 y C33.
         */
        public void espera()
        {

            // Se esperan datos del puerto serial.
            while (oTarjeta.getStatusLectura() == -1 && contador <= Constantes.TIMEOUT)
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
