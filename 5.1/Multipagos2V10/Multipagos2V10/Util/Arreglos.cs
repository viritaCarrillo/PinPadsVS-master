using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.Util
{
    class Arreglos
    {
        /**
        * Verifica si dos arreglos de bytes son iguales.
        * @param arrUno - Arreglo de datos uno.
        * @param arrDos - Arreglo de datos dos.
        * @return <b>true</b> si lo arreglo son iguales, <b>false</b>
        * en caso contario.
        */
        public static bool isEqual(byte[] arrUno, byte[] arrDos)
        {
            bool bIgual = true;

            if (arrUno.Length == arrDos.Length)
            {
                for (int i = 0; i < arrUno.Length; i++)
                {
                    if (arrUno[i] != arrDos[i])
                    {
                        bIgual = false;
                        break;
                    }
                }
            }
            else
                bIgual = false;

            return bIgual;
        }

        /**
         * Concatena dos arreglos de bytes.
         * @param argInicial - Arreglo inicial.
         * @param argFinal - Arreglo final.
         * @return Un arreglo que contiene los arreglos inicial y final.
         */
        public static byte[] concatena(byte[] argInicial, byte[] argFinal)
        {
            byte[] concatenado = new byte[argInicial.Length + argFinal.Length];

            for (int i = 0; i < argInicial.Length; i++)
                concatenado[i] = argInicial[i];

            for (int i = argInicial.Length, j = 0; i < (argInicial.Length + argFinal.Length); i++, j++)
                concatenado[i] = argFinal[j];

            return concatenado;
        }

        /**
        * Concatena un byte al final de un arreglo de bytes.
        * @param arreglo - Arreglo de bytes
        * @param elemento - Byte.
        * @return Un arreglo que contiene el arreglo y el byte.
        */
        public static byte[] concatena(byte[] arreglo, byte elemento)
        {
            byte[] concatenado = new byte[arreglo.Length + 1];

            for (int i = 0; i < arreglo.Length; i++)
                concatenado[i] = arreglo[i];

            concatenado[arreglo.Length] = elemento;

            return concatenado;
        }

        /***
         * Calcula el XOR de una cadena quitando la posici&oacute;n inicial.
         * @param cadena - Cadena a la que se le calculara el XOR.
         * @return El valor XOR de la cadena.
         */
        public static byte getXOR(byte[] cadena)
        {
            int resultado = 0;

            for (int i = 1; i < cadena.Length; i++)
                resultado = resultado ^ (int)cadena[i];

            return (byte)resultado;
        }

        /***
         * Calcula el XOR de dos arreglos de bytes.
         * @param bInicial - Arreglos inicial.
         * * @param bInicial - Arreglos final.
         * @return Un arreglo de bytes con el XOR.
         */
        public static byte[] getXOR(byte[] bInicial, byte[] bFinal)
        {
            byte[] bXOR = new byte[bInicial.Length];

            if (bInicial.Length == bFinal.Length)
            {
                for (int i = 0; i < bInicial.Length; i++)
                {
                    int ixor = (int)bInicial[i] ^ (int)bFinal[i];
                    bXOR[i] = (byte)ixor;
                }
            }
            else
                bXOR = null;

            return bXOR;
        }

        /**
         * Verifica si el LRC del bloque de datos es correcto.
         * @param bloque - Datos a verificar.
         * @return <b>true</b> si el LRC es correcto, <b>false</b>
         * en caso contrario.
         */
        public static bool isValidLrc(byte[] bloque)
        {

            byte[] lrc = { (byte)bloque[bloque.Length - 1] }; // LRC
            bool bLrc = false;

            //obtener la cadena sin LRC del packet 81
            byte[] lrcC = new byte[bloque.Length - 1];
            for (int i = 0; i < bloque.Length - 1; i++)
                lrcC[i] = bloque[i];


            //Calcular el LRC
            byte[] lrcNew = { getXOR(lrcC) };
            if (isEqual(lrcNew, lrc))
                bLrc = true;

            return bLrc;
            //return false;
        }
    }
}
