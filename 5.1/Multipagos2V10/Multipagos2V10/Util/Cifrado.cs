using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Multipagos2V10.Exceptions;
using System.IO;
using Multipagos2V10.FileProperties;
using System.Threading;

namespace Multipagos2V10.Util
{
    class Cifrado
    {
        public static string encriptaRijndael(string ccNum)
        {
            IniFile ini = new IniFile("C:\\flap/config/Rijndael.properties");
            byte[] keyArray = UTF8Encoding.Default.GetBytes(ini.IniReadValue("Info", "KEY"));
            byte[] resultArray = new byte[0];
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(ccNum);
            string cifrado = "";
            try
            {
                if (keyArray.Length > 0)
                {
                    RijndaelManaged rDel = new RijndaelManaged();
                    rDel.Key = keyArray;
                    rDel.Mode = CipherMode.ECB;
                    rDel.Padding = PaddingMode.Zeros;
                    ICryptoTransform cTransform = rDel.CreateEncryptor();
                    resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    int numblock = resultArray.Length / 16;
                    byte[] datos = new byte[16];
                    int j = 0;

                    for (int z = 0; z < numblock; z++)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            datos[i] = resultArray[j++];
                        }
                        cifrado += Convert.ToBase64String(datos, 0, datos.Length);
                    }
                }
                else
                {
                    cifrado = "OCURRIO UN ERROR DE CONFIGURACION: \n\nNO SE ENCONTRO LA LLAVE DE CIFRADO(Rijndael),\nVERIFIQUE EL ARCHIVO Rijndael.properties";
                }

            }
            catch (Exception e)
            {               
                cifrado = "ERROR EN EL CIFRADO (Rijndael).";
            }

            return cifrado;
        }

        public static string desencriptaRijndael(string ccNumEn)
        {
            IniFile ini = new IniFile("C:\\flap/config/Rijndael.properties");
            byte[] keyArray = UTF8Encoding.Default.GetBytes(ini.IniReadValue("Info", "KEY"));


            if (keyArray.Length == 0)
            {
                string path = typeof(LeeBandaChip).Assembly.Location;
                path = path.Replace('\\', '/');
                path = path.Replace("vx810v1.dll", "config/");
                ini = new IniFile(path + "/Rijndael.properties");
                keyArray = UTF8Encoding.Default.GetBytes(ini.IniReadValue("Info", "KEY"));
            }


            string res = "";

            int numbloks = ccNumEn.Length/24;
            for (int z = 0; z < numbloks; z++)
            {

                string cadena = ccNumEn.Substring(24 * z, 24);
                byte[] toEncryptArray = Convert.FromBase64String(cadena);
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.Zeros;

                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                int pos = 0;
                while (pos < resultArray.Length && resultArray[pos] != 0)
                {
                    pos++;
                }

                byte[] tmp = new byte[pos];

                for (int i = 0; i < tmp.Length; i++)
                    tmp[i] = resultArray[i];



                res += UTF8Encoding.UTF8.GetString(tmp);

            }

            return res;
            
        }
    }
}
