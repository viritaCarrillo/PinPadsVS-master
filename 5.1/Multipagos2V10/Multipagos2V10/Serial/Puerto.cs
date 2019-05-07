using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using Multipagos2V10.Util;
using System.Threading;
using System.IO;
using Multipagos2V10.Exceptions;

namespace Multipagos2V10.Serial
{
    class Puerto
    {
        SerialPort port;

        /**
        * Abre el puerto serial.
        * @return <b>true</b> si se abrio el puerto <b>false</b> en caso contario.
        */
        public bool abrePuerto(string nombrePuerto)
        {

            bool bAbrir = false;

            try
            {
                port = new SerialPort();
                port.PortName = nombrePuerto;
                port.BaudRate = 19200;
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.One;

                port.Open();
                if (port.IsOpen)
                {
                    bAbrir = true;
                }
            }
            catch (IOException ioe)
            {
                System.Console.WriteLine("Puerto.abrePuerto IOE:" + ioe.Message);
            }

            return bAbrir;
        }

        /**
	    * Cierra el puerto serial.
	    */
        public void cierraPuerto()
        {
            if (port != null)
                port.Close();
        }

        /**
	    * Escribe un mensaje en el puerto serial.
	    * @param instruccion - Instrucci&oacute;n que ejecutara la pinpad.
	    */
        public void escribe(byte[] datos)
        {
            port.Write(datos, 0, datos.Length);
        }

        /**
        * Lee el buffer del puerto serial en busca de datos (lee byte por byte).
        * @return - Un arreglo de bytes con los datos leidos.
        */
        public byte[] leeDatosXCaracter()
        {
            byte[] cDatos = null;
            try
            {
                int iTamDatos = 0;
                char[] datos = new char[800];

                int iEspera = 0;

                do
                {
                    iEspera++;
                    int caracter = 0;
                    int espera = 0;

                    while (port.BytesToRead > 0)
                    {
                        caracter = port.ReadByte();
                        datos[iTamDatos++] = (char)caracter;
                        espera++;

                        if((espera / 10) == 0){
                            Thread.Sleep(1);
                        }
                    }

                    if (iTamDatos == 0)
                    {
                        Thread.Sleep(100);
                    }

                } while ((iEspera <= 15 && iTamDatos == 0));

                cDatos = new byte[iTamDatos];
                for (int i = 0; i < iTamDatos; i++)
                    cDatos[i] = (byte)datos[i];
            }
            catch (IOException ioe)
            {
                System.Console.WriteLine("Puerto.leeDatosXCaracter_IOE: " + ioe);
            }

            return cDatos;

        }


        /**
	    * Lee el buffer del puerto serial en busca de un ACK (06).
	    * @return tue si el dato leido es un ACK, false en caso contario.
	    */
        public bool leeACK()
        {
            byte[] datos = new byte[500];
            bool bLee = false;

            //datos = leeDatosXCaracter();
            datos = leeDatosACK();

            if (Conversiones.toHexString(datos).Equals("06"))
                bLee = true;

            return bLee;
        }

        public byte[] leeDatosACK()
        {
            char[] datos = new char[1];
            byte[] cDatos = new byte[1];
            int iEspera = 0;
            int caracter = -1;

            do
            {
                Thread.Sleep(500);
                iEspera++;
                caracter = port.ReadByte();
                if (caracter != -1)
                {
                    datos[0] = (char)caracter;
                    cDatos[0] = (byte)datos[0];
                    break;
                }
            } while (caracter == 0 && iEspera < 3);

            return cDatos;
        }

        public byte[] leeACK_C34()
        {
            byte[] datos = new byte[500];
            datos = leeDatosXCaracter();
            return datos;
        }


        /**
	    * Lee del puerto serial el numero de serie de la pinpad.
	    * @return - Un arreglo de bytes con el numero de serie.
	    */
        public byte[] leeNumSerie()
        {
            byte[] nSeriePinPad = leeDatosXCaracter();
            byte[] bNumSerie = new byte[12];


            if (Conversiones.toHexString(nSeriePinPad).Substring(0, 2).Equals("06"))
            {
                escribe(Constantes.ACK);
                for (int i = 5, j = 0; i < nSeriePinPad.Length - 2; i++)
                    if (nSeriePinPad[i] != 45)// quita el guion
                        bNumSerie[j++] = nSeriePinPad[i];
            }

            port.DiscardOutBuffer();
            leeDatosXCaracter();



            return bNumSerie;

        }

        /**
        * Obtiene el puerto serial
        * @return - El puerto serial.
        */
        public SerialPort getPuerto()
        {
            return port;
        }

        public bool abrePuertos()
        {
            bool bAbre = false;

            try
            {
                string[] puertos = SerialPort.GetPortNames();

                string puerto = Parametros.getParameter("defaultPort");

                if (puerto != null && puerto.Length > 0)
                {
                    bAbre = abrePuerto(puerto);
                }

                if (!bAbre)
                {
                    for (int i = 0; i < puertos.Length; i++)
                    {
                        if (abrePuerto(puertos[i]))
                        {
                            escribe(Comandos.getComandoZ3("A6581599"));

                            byte[] datoslee = leeDatosXCaracter();
                            if (datoslee != null)
                            {
                                if (Conversiones.toHexString(datoslee).Equals("06"))
                                {
                                    bAbre = true;
                                    List<String> lDatos = null;
                                    lDatos = Parametros.leeArchivo(Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION);
                                    Parametros.escribeArchivo(lDatos, Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION, puertos[i], "defaultPort");
                                    break;
                                }
                                else
                                    cierraPuerto();
                            }
                            else
                            {
                                cierraPuerto();
                            }
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException uae)
            {
                System.Console.WriteLine("abrePuertos.uae: " + uae.Message);
                cierraPuerto();
            }                        

            return bAbre;
        }


        public void imprimir(string datosaImprimir)
        {
            escribe(Constantes.encoding.GetBytes(datosaImprimir));
            byte[] datos = leeDatosXCaracter();

        }


        public byte[] leeDatos()
        {
            byte[] datos = null;

            try
            {

                int iEspera = 0;
                do
                {
                    iEspera++;

                    int bytes = port.BytesToRead;
                    if (bytes > 0)
                    {
                        datos = new byte[bytes];
                        port.Read(datos, 0, bytes);
                    }

                    if (datos == null)
                        Thread.Sleep(700);

                } while ((iEspera <= 3 && datos == null));
            }
            catch (IOException ioe)
            {
                System.Console.WriteLine("Puerto.leeDatosXCaracter_IOE: " + ioe);
            }

            return datos;

        }
    }
}
