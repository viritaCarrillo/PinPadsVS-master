using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Multipagos2V10.Interface
{
    interface IDatosLectura
    {
        void lee(string monto,
                 int tpoTransaccion,    // interred 5.1
                 int cdMoneda,          // interred 5.1
                 bool consulta);        // interred 5.1

        bool termina(string cdAutorizacion, 
                     string criptograma, 
                     string script, 
                     string cdRespuesta,  // interred 5.1
                     string ingreso,      // interred 5.1
                     string banderaLlave, // interred 5.1
                     string banderaBines, // interred 5.1
                     string tokenET,      // interred 5.1
                     int telecarga);     // interred 5.1

        string compra(string secuencia,
                      string referencia,
                      int    val_1,
                      string servicio,
                      int    c_cur,
                      string importe,
                      string tarjethabiente,
                      string val_3,
                      string val_6,
                      string val_11,
                      string val_12,
                      int    entidad,
                      string val_16,
                      string val_19,
                      string val_20,
                      string email,
                      string accion,
                      string emv,
                      int    puntos);

        string cancelacion(string secuencia,
                           string referencia,
                           int    val_1,
                           string servicio,
                           int    c_cur,
                           string importe,
                           string tarjethabiente,
                           string val_3,
                           string val_6,
                           int    entidad,
                           string val_16,
                           string val_19,
                           string val_20,
                           string email,
                           string accion,
                           string tagsemv,
                           string autorizacion);

        string encriptaRijndael(string ccNum); //
        string encriptaHmac(string textEncrip); //
        string getMensaje(); //
        
        string consulta(int entidad,
                        int nivel1,
                        int moneda,
                        int servicio,
                        string transmision,
                        string referencia,
                        string tarjeta,
                        string digest,
                        string tpoTarjeta,
                        string accion,
                        string mail,
                        string titular,
                        string emv); //
        
        string binAutorizado(int entidad, 
                             string bin,
                             string mail, 
                             string accion); //

        string getNumTarjeta(); //
        string getTagEMV(); //
        string getTag9F27(); //
        string getName(); //        
        string getTrack1(); //
        string getTrack2(); //
        string getCdRespuesta();

        string status(int entidad,
                             int val_1,
                             int c_cur,
                             int servicio,
                             string secuencia,
                             string referencia,
                             string mail,
                             string acccion); //

        string imprime(); //
        string reimprime(int entidad,
                         int nivel1,
                         int nivel2,
                         int servicio,
                         string referencia,
                         string secuencia,
                         string operacion,
                         string autorizacion); //

        string imprimeDeclinado(int entidad,
                                int nivel1,
                                int nivel2,
                                string servicio,
                                string tarjeta,
                                string importe,
                                string titular,
                                string autorizacion,
                                int plazo); //

        string desplegarPagareComercio(string datos);
        string desplegarPagareCliente(string datos);
        string imprimirPagareComercio(string datos);
        string imprimirPagareCliente(string datos);        
        string imprimirPagareClienteLogos(string datos);
        string imprimirPagareComercioLogos(string datos);
        string imprimirCliente(string datos, string impresora, int moneda);
        string imprimirComercio(string datos, string impresora, int moneda);
        string imprimirComercio(string datos, string impresora, int moneda, int ancho, int carGrande, int carChico);
        string imprimirCliente(string datos, string impresora, int moneda, int ancho, int carGrande, int carChico);
        string imprimirCAT(string datos, string impresora, int moneda, int ancho, int carGrande, int carChico);

        string[] impresoras();

        // interred 5.1
        void llave();
        string banderaLLave();
        string telecarga();
        void carga(string tokenEX);
        void actualiza();
        //

        // AMEX
        bool compraAmex(string secuencia,
                          string referencia,
                          int entidad,
                          int val1,
                          string servicio,
                          string titular,
                          string importe,
                          string tarjeta,
                          string cdSeguridad,
                          string digest,
                          string mail,
                          string telefono,
                          string nombre,
                          string apellidos,
                          string cdPostal,
                          string direccion,
                          string track1,
                          string track2,
                          int val_19,
                          int val_20,
                          int moneda,
                          string tagsEMV,
                          string tagsE1,
                          string correo,
                          string serie);

        bool cancelaAmex(int cdEntidad,
                            int cdNivel1,
                            int moneda,
                            string servicio,
                            string autorizacion,
                            int consecutivo,
                            string origen);

        string getPlataforma();
        string getCvv2();
        string getTagE1();
        string getNumeroSerie();
        string getCriptograma();
        string getMdoLectura();
        int getReverso();

        // certificacion
        string valida(string entidad, string tarjeta, string mail, string accion);
        //string devolucion(string secuencia, string referencia, int val_1, string servicio, int c_cur, string importe, int entidad, string email, string accion, string autorizacion);
        Hashtable mapea(string xml);
        void desconecta();
    }
}
