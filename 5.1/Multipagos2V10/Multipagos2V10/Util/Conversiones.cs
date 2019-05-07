using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Multipagos2V10.Util
{
    class Conversiones
    {
        private static char[] CHEXADEC = { '0', '1', '2', '3', '4', '5', '6', '7', 
									   '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private String[,] sMapeo = {{"0", "00", "0"},
                                    {"1", "01", "1"},
                                    {"2", "02", "2"},
                                    {"3", "03", "3"},
                                    {"4", "04", "4"},
                                    {"5", "05", "5"},
                                    {"6", "06", "6"},
                                    {"7", "07", "7"},
                                    {"8", "08", "8"},
                                    {"9", "09", "9"},
                                    {"A", "0A", "10"},
                                    {"B", "0B", "11"},
                                    {"C", "0C", "12"},
                                    {"D", "0D", "13"},
                                    {"E", "0E", "14"},
                                    {"F", "0F", "15"}};

        private Hashtable hHexadecimal;

        public Conversiones()
        {
            hHexadecimal = new Hashtable();
            asignaMapeo();
        }

        private void asignaMapeo()
        {
            for (int i = 0; i < sMapeo.GetLength(0); i++)
            {
                hHexadecimal.Add(sMapeo[i, 0], sMapeo[i, 2]);
            }
        }

        /**
        * Convierte un arreglo de bytes a Hexadecimal.
        * @param bDato - Arreglo de bytes.
        * @return  Una cadena con el valor hexadecimal.
        */
        public static String toHexString(byte[] bDato)
        {
            string sHexa = "";
            int alto = 0;
            int bajo = 0;

            for (int i = 0; i < bDato.Length; i++)
            {
                alto = ((bDato[i] & 0xF0) >> 4);
                bajo = (bDato[i] & 0x0F);
                sHexa += CHEXADEC[alto];
                sHexa += CHEXADEC[bajo];
            }

            return sHexa;
        }

        /**
        * Convierte un arreglo de bytes en hexadecimal.
        * @param bDato - Arreglo de bytes.
        * @return - Un arreglo de chars con el valor hexadecimal.
        */
        public static char[] toHexChar(byte[] bDato)
        {
            int alto = 0;
            int bajo = 0;
            char[] cHexa = new char[2 * bDato.Length];

            for (int i = 0, j = 0; i < bDato.Length; i++, j++)
            {
                alto = ((bDato[i] & 0xf0) >> 4);
                bajo = (bDato[i] & 0x0f);
                cHexa[j] = CHEXADEC[alto];
                cHexa[++j] = CHEXADEC[bajo];
            }

            return cHexa;
        }

        /**
        * Pasa a formato BCD un arreglo de bytes.
        * @param datos - Arreglo de bytes.
        * @return - Un arreglo de bytres en formato BCD.
        */
        public static byte[] keyBCD(byte[] datos)
        {
            byte[] bDatBcd = new byte[(datos.Length) / 2];

            for (int j = 0, i = 0; j < (datos.Length) / 2; j++)
                bDatBcd[j] = (byte)(((int.Parse(((object)(char)datos[i++]).ToString(), System.Globalization.NumberStyles.HexNumber)) << 4) |
                                    (int.Parse(((object)(char)datos[i++]).ToString(), System.Globalization.NumberStyles.HexNumber)));
            return bDatBcd;
        }

        /**
        * Convierte de hexadecimal a decimal.
        * @param hexa - El valor hexadecimal.
        * @return - Un string con el valor decimal.
        */
        public static String toHexaDecimal(String hexa)
        {
            byte[] bDatos = Constantes.encoding.GetBytes(hexa);
            String sDatos = "";
            byte[] caracter = new byte[2];
            for (int i = 0; i < hexa.Length; )
            {
                caracter[0] = bDatos[i++];
                caracter[1] = bDatos[i++];
                sDatos += ((char)(int.Parse(Constantes.encoding.GetString(caracter), System.Globalization.NumberStyles.HexNumber))).ToString();
            }

            return sDatos;
        }

        public int HexadecimalDecimal(char[] sHexa)
        {
            int iDecimal = 0;

            for (int i = (sHexa.Length - 1), iPotencia = 0; i >= 0; i--, iPotencia++)
            {
                string sValor = (string)hHexadecimal[sHexa[i].ToString()];
                int y = (int)Math.Pow(16, iPotencia);
                iDecimal += int.Parse(sValor) * y;
            }

            return iDecimal;
        }

        public string HexadecimalAscii(char[] sHexadecimal)
        {
            string ascii = "";

            for (int z = 0; z < sHexadecimal.Length; )
            {
                char[] sHexa = { sHexadecimal[z++], sHexadecimal[z++] };
                ascii += (((char)(new Conversiones().HexadecimalDecimal(sHexa)))).ToString();
            }

            return ascii;
        }
    }
}
