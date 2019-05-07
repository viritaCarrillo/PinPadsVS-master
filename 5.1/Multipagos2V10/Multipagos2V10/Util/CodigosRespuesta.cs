/*
 * Creado el 30/09/2015
 * @author Lic. Selene Nochebuena Rojo
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Multipagos2V10.Util
{
    class CodigosRespuesta
    {
        private Hashtable hCodigos;
        private static String[,] sCodigos = {{"00","OPERACION EXITOSA"}, 
								   		  {"01","MENSAJE INVALIDO"},
										  {"02","FORMATO DE DATOS INVALIDO"},										  
										  {"06","TIMEOUT"},
										  {"10","FALLA EN LECTURA DE CHIP"},
										  {"20","TARJETA DIGITADA"},
										  {"21","USE BANDA MAGNETICA"},
										  {"23","TARJETA REMOVIDA"},
										  {"25","TARJETA NO SOPRTADA"},
										  {"26","APLICACION INVALIDA"},
										  {"27","TARJETA OPERADOR INVALIDA"},
										  {"29","TARJETA VENCIDA"},
										  {"30","FECHA INVALIDA"},										  
										  {"50","LLAME A SOPORTE"},
										  {"51","CHECK VALUE INCORRECTO"},
										  {"52","LLAVE INEXISTENTE"},
										  {"53","PROBLEMAS DE CRIPO HSM"},
										  {"54","INICIALICE LLAVE"},
										  {"55","FIRMA INCORRECTA"},
										  {"56","ERROR EN LONGITUD DE TELECARGA"},										  
										  {"60","COMANDO NO PERMITIDO"},
										  {"61","LLAVES INCIALIZADAS. COMANDO NO RECONOCIDO"},
										  {"62","NO SE HA REALIZADO INCIALIZACION DE LLAVES"},
										  {"63","ERROR DE LECTURA"},
										  {"99","OTRA FALLA"}};

        public CodigosRespuesta()
        {
            hCodigos = setCodigosRespuesta();
        }

        /**
         * @return Una tabla de hash con los codigos de respuesta de la pinpad.
         */
        public Hashtable setCodigosRespuesta()
        {
            Hashtable hCodRespuesta = new Hashtable();

            for (int i = 0; i < sCodigos.GetLength(0); i++)
                hCodRespuesta.Add(sCodigos[i, 0], sCodigos[i, 1]);

            return hCodRespuesta;
        }

        /**
         * Busca un codigo de respueta en la tabla hash.
         * @param codigo - Codigo regresado en al respuesta de la pinpad.
         * @return La descripci&oacute;n del codigo de respuesta.
         */
        public string getDescripcionCodigo(string codigo)
        {
            string descripcion = "";


            if (hCodigos.ContainsKey(codigo))
            {
                foreach (DictionaryEntry dsc in hCodigos)
                {
                    if (dsc.Key.Equals(codigo))
                    {
                        descripcion = (string)dsc.Value;
                        break;
                    }
                }
            }

            return descripcion;
        }
    }
}
