using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Multipagos2V10.Exceptions;
using System.Windows.Forms;

namespace Multipagos2V10.Util
{
    class Parametros
    {
        /*Este Metodo permite obtener el valor de una propiedad del archivo pinpad.config*/
        /*El formato debera ser:  <nombre-propiedad>=<valor-propiedad>                   */

        public static string getParameter(string nameParameter)
        {

            String[] arreglo = new String[5];
            FileStream fs = null;
            string valorParameter = null;
            try
            {
                try
                {
                    fs = new FileStream("C://flap/config/pinpad.config", FileMode.OpenOrCreate);
                }
                catch (DirectoryNotFoundException) { }
                catch (FileNotFoundException) { }
                if (fs == null)
                {
                    string path = typeof(Parametros).Assembly.Location;
                    path = path.Replace("Multipagos2V10.dll", "config\\pinpad.config");
                    fs = new FileStream(path, FileMode.OpenOrCreate);
                }

                StreamReader sr = new StreamReader(fs);
                string line = "";
                int lineNo = 0;
                while (line != null)
                {
                    line = sr.ReadLine();
                    if ((line == null) || (line == ""))
                    {
                        lineNo++;
                    }
                    else
                    {
                        line.Trim();
                        int posEqual = line.IndexOf("=");
                        string aux = line.Substring(0, posEqual);
                        if ((aux.Equals(nameParameter)))
                        {
                            valorParameter = line.Substring(posEqual + 1).Trim();
                        }
                        else
                        {
                            lineNo++;
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("DirectoryNotFoundException");
                throw new PinPadException("OCURRIO UN ERROR DE CONFIGURACION:\n\nNECESITA CREAR EL DIRECTORIO c://flap/config/ Y COLOCAR LOS ARCHIVOS DE CONFIGURACION");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("FileNotFoundException");
                throw new PinPadException("OCURRIO UN ERROR DE CONFIGURACION:\n\nNO SE ENCONTRO EL ARCHIVO pinpad.config EN LA RUTA c://flap/config/");
            }
            catch (Exception e)
            {
                throw new PinPadException("OCURRIO UN ERROR DE CONFIGURACION:\n\nERROR EN LA LECTURA DEL ARCHIVO pinpad.config");
            }
            finally
            {
                if (fs != null)
                    try { fs.Close(); }
                    catch (Exception) { };
            }

            return valorParameter;
        }

        /*Método que obtiene la llave del archivo especificado*/
        /*lo almacena en un arreglo de bytes y lo retorna*/
        public static byte[] getKey()
        {
            FileStream fs = null;
            byte[] llave = null;

            try
            {
                try
                {
                    fs = new FileStream("c://Adquira/config/llave.txt", FileMode.Open);
                }
                catch (DirectoryNotFoundException) { }
                catch (FileNotFoundException) { }

                if (fs == null)
                {
                    string path = typeof(Parametros).Assembly.Location;
                    path = path.Replace("vx810v1.dll", "config\\llave.txt");
                    fs = new FileStream(path, FileMode.Open);
                }

                llave = new byte[fs.Length];
                BinaryReader sr = new BinaryReader(fs);
                sr.Read(llave, 0, Convert.ToInt32(fs.Length));
                fs.Close();

            }
            catch (DirectoryNotFoundException)
            {
                throw new PinPadException("OCURRIO UN ERROR DE CONFIGURACION:\n\nNECESITA CREAR EL DIRECTORIO c://Adquira/config/ Y COLOCAR LOS ARCHIVOS DE CONFIGURACION");
            }
            catch (FileNotFoundException)
            {
                throw new PinPadException("OCURRIO UN ERROR DE CONFIGURACION:\n\nNO SE ENCONTRO EL ARCHIVO llave.txt EN LA RUTA c://Adquira/config/");
            }
            catch (Exception)
            {
                throw new PinPadException("OCURRIO UN ERROR DE CONFIGURACION:\n\nERROR EN LA LECTURA DEL ARCHIVO llave.txt");
            }
            finally
            {
                if (fs != null)
                    try { fs.Close(); }
                    catch (Exception) { };
            }

            return llave;
        }

        public static bool eliminaArchivo(string archivo)
        {
            bool borra = false;

            try
            {
                File.Delete(archivo);
                borra = true;
            }
            catch (Exception) { }

            return borra;
        }


        public static void escribeArchivo(List<String> lDatos, string archivo, string valor, string parametro)
        {
            FileStream fArchivo = null;
            bool encontrado = false;

            try
            {
                if (!Directory.Exists(Constantes.RUTA_CONFIGURACION + Constantes.ARCHIVO_CONFIGURACION))
                    Directory.CreateDirectory(Constantes.RUTA_CONFIGURACION);

                fArchivo = new FileStream(archivo, FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter output = new StreamWriter(fArchivo);

                string linea = ""; ;
                foreach (String elemento in lDatos)
                {
                    linea = elemento;
                    if (linea.StartsWith(parametro))
                    {
                        linea = parametro + "=" + valor;
                        encontrado = true;
                    }
                    output.WriteLine(linea);
                }

                if (!encontrado)
                {
                    output.WriteLine(parametro + "=" + valor);
                }

                output.Close();
                fArchivo.Close();
            }
            catch (Exception e)
            {
                throw new PinPadException("escribeArchivo: " + e.Message);
            }
        }

        public static List<String> leeArchivo(string archivo)
        {
            FileStream fArchivo = null;
            List<String> lArchivo = new List<String>();

            try
            {
                fArchivo = new FileStream(archivo, FileMode.Open, FileAccess.Read);

                StreamReader input = new StreamReader(fArchivo);

                String linea = "";
                while ((linea = input.ReadLine()) != null)
                {
                    linea = linea.Trim();
                    lArchivo.Add(linea);
                }

                input.Close();
                fArchivo.Close();
            }
            catch (DirectoryNotFoundException) { }
            catch (FileLoadException) { }
            catch (IOException) { }
            catch (Exception e)
            {
                throw new PinPadException("leeArchivo: " + e.Message);
            }

            return lArchivo;
        }
    }
}
