using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Multipagos2V10.Util;
using System.Drawing.Printing;
using System.IO;

namespace Multipagos2V10.Impresion
{
    public partial class IPagare : Form
    {
        PrintDocument doc = new PrintDocument();
        private int ancho = 255;
        private int carGrande = 32;
        private int carChico = 49;

        public IPagare()
        {
            InitializeComponent();
        }
        public static string obtenerValor(String parametro, String datos)
        {
            string valor = "";

            int inicio = datos.IndexOf(parametro);

            char[] cDatos = datos.ToCharArray();
            int tam = cDatos.Length;

            if (inicio >= 0)
            {
                inicio += parametro.Length + 1;
                while (inicio < tam && cDatos[inicio] != '|')
                {
                    valor += Convert.ToString(cDatos[inicio++]);
                }
            }

            return valor;
        }

        /*
         * Asigna los valores del formulario.
         */
        public string visualizarPagare(string datos, string tpoPagare)
        {
            string mensaje = "";

            if (asignaFormulario(datos + "|", tpoPagare))
            {

                //Alinea el texto al centro
                header.Location = new System.Drawing.Point(obtenerPosicion(header.Size.Width, this.Size.Width), 30);

                comercio.Text = saltoLineaSF(comercio.Text, 33);
                comercio.Location = new System.Drawing.Point(obtenerPosicion(comercio.Size.Width, this.Size.Width), 60);

                direccion.Location = new System.Drawing.Point(obtenerPosicion(direccion.Size.Width, this.Size.Width), 90);
                direccion.Text = saltoLineaSF(direccion.Text, 42);

                emisor.Text = saltoLineaSF(emisor.Text, 78);
                emisor.Location = new System.Drawing.Point(obtenerPosicion(emisor.Size.Width, this.Size.Width), 170);
                tarjeta.Location = new System.Drawing.Point(obtenerPosicion(tarjeta.Size.Width, this.Size.Width), 190);
                tpoTransaccion.Location = new System.Drawing.Point(obtenerPosicion(tpoTransaccion.Size.Width, this.Size.Width), 210);
                monto.Location = new System.Drawing.Point(obtenerPosicion(monto.Size.Width, this.Size.Width), 230);
                aid.Location = new System.Drawing.Point(obtenerPosicion(aid.Size.Width, this.Size.Width), 373);
                label.Location = new System.Drawing.Point(obtenerPosicion(label.Size.Width, this.Size.Width), 388);
                firma.Location = new System.Drawing.Point(obtenerPosicion(firma.Size.Width, this.Size.Width), 405);
                titular.Location = new System.Drawing.Point(obtenerPosicion(titular.Size.Width, this.Size.Width), 430);
                promosion.Text = saltoLineaSF(promosion.Text, 30);
                promosion.Location = new System.Drawing.Point(obtenerPosicion(promosion.Size.Width, this.Size.Width), 437);
                tpPagare.Location = new System.Drawing.Point(obtenerPosicion(tpPagare.Size.Width, this.Size.Width),650);

                this.Show();
            }
            else
                mensaje = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return mensaje;
        }


        /*
         * Manda a la impresora el pagare. 
         */
        public string imprimirPagareLogos(string datos, string tarjeta)
        {
            string mensaje = "";

            if (asignaFormulario(datos, tarjeta))
            {
                doc.PrintPage += new PrintPageEventHandler(this.imprimeLogos);
                PrintDialog dlg = new PrintDialog();
                dlg.Document = doc;


                if (dlg.ShowDialog() == DialogResult.OK)
                    doc.Print();
            }
            else
                mensaje = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return mensaje;

        }

        public string imprimirPagare(string datos, string tarjeta)
        {
            string mensaje = "";

            if (asignaFormulario(datos, tarjeta))
            {
                doc.PrintPage += new PrintPageEventHandler(this.imprimeLogo);
                PrintDialog dlg = new PrintDialog();
                dlg.Document = doc;

                if (dlg.ShowDialog() == DialogResult.OK)
                    doc.Print();
            }
            else
                mensaje = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return mensaje;
        }

        public string imprimirPagarePredeterminada(string datos, string tarjeta, string predeterminada, int moneda)
        {
            string mensaje = "";

            if (asignaFormularioPD(datos, tarjeta, moneda))
            {
                doc.PrintPage += new PrintPageEventHandler(this.imprimeLogoPredeterminada);

                string impresora = impresoraPredeterminada(predeterminada);

                if (impresora != null && impresora.Length > 0)
                {
                    doc.PrinterSettings.PrinterName = impresoraPredeterminada(impresora);
                    doc.Print();
                }
                else
                {
                    mensaje = "No hay ninguna impresora";
                }
            }
            else
                mensaje = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return mensaje;
        }

        //nuevo
        public string imprimirPagareConfig(string datos, string tarjeta, string predeterminada, int moneda, int ancho, int carChico, int carGrande)
        {
            string mensaje = "";
            this.ancho = ancho;
            this.carGrande = carGrande;
            this.carChico = carChico;

            if (asignaFormularioPD(datos, tarjeta, moneda))
            {
                doc.PrintPage += new PrintPageEventHandler(this.imprimeLogoConfig);

                string impresora = impresoraPredeterminada(predeterminada);

                if (impresora != null && impresora.Length > 0)
                {
                    doc.PrinterSettings.PrinterName = impresoraPredeterminada(impresora);
                    doc.Print();
                }
                else
                {
                    mensaje = "No hay ninguna impresora";
                }
            }
            else
                mensaje = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return mensaje;
        }
        //nuevo

        // CAT
        public string imprimirPagareCAT(string datos, string tarjeta, string predeterminada, int moneda, int ancho, int carChico, int carGrande)
        {
            string mensaje = "";
            this.ancho = ancho;
            this.carGrande = carGrande;
            //this.carChico = carChico;

            if (asignaFormularioPD(datos, tarjeta, moneda))
            {
                doc.PrintPage += new PrintPageEventHandler(this.imprimeCAT);

                string impresora = impresoraPredeterminada(predeterminada);

                if (impresora != null && impresora.Length > 0)
                {
                    doc.PrinterSettings.PrinterName = impresoraPredeterminada(impresora);
                    doc.Print();
                }
                else
                {
                    mensaje = "No hay ninguna impresora";
                }
            }
            else
                mensaje = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return mensaje;
        }
        //

        private string impresoraPredeterminada(string predeterminada)
        {
            string impresoraSelecionada = null;

            foreach (string impresora in PrinterSettings.InstalledPrinters)
            {
                if (predeterminada.Equals(impresora))
                {
                    impresoraSelecionada = impresora;
                    break;
                }
            }

            return impresoraSelecionada;
        }

        public string[] getImpresoras()
        {
            string[] impresoras = new string[PrinterSettings.InstalledPrinters.Count];

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                impresoras[i] = PrinterSettings.InstalledPrinters[i];

            return impresoras;
        }

        private void imprimeLogos(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Image bancomer = Image.FromFile("C:\\flap\\config\\logoBancomer.gif");
                Image comercio = Image.FromFile("C:\\flap\\config\\logoComercio.png");

                ev.Graphics.DrawImage(bancomer, 10, 20);
                ev.Graphics.DrawImage(comercio, 210, 10);

                this.generaPagare(sender, ev, 60);
            }
            catch (FileNotFoundException)
            {
                Font grande = new Font("Courier New", 10);
                ev.Graphics.DrawString(header.Text, grande, Brushes.Black, obtenerPosicion(header.Size.Width, Constantes.FUENTE_MEDIA), 30);
                this.generaPagare(sender, ev, 30);
            }
        }

        private void imprimeLogo(object sender, PrintPageEventArgs ev)
        {
            try
            {
                ev.Graphics.DrawImage(Image.FromFile("C:\\flap\\config\\logoBancomer.gif"), 70, 20);

                this.generaPagare(sender, ev, 60);
            }
            catch (FileNotFoundException)
            {
                Font grande = new Font("Courier New", 10);
                ev.Graphics.DrawString(header.Text, grande, Brushes.Black, obtenerPosicion(header.Size.Width, Constantes.FUENTE_MEDIA), 30);
                this.generaPagare(sender, ev, 30);
            }
        }

        /*
         * Asigna las posiciones donde se imprimira el pagare.
         */
        private void generaPagare(Object sender, PrintPageEventArgs e, int pos)
        {
            string dato = "";
            Font media = new Font("Courier New", 8);
            Font mediaNegrita = new Font("Courier New", 8, FontStyle.Bold);
            Font chica = new Font("Courier New", 6);
            Font mini = new Font("Courier New", 5);
            Font grande = new Font("Courier New", 10);

            int y = pos;
            int incremento = 25;
            //e.Graphics.DrawString(header.Text, grande, Brushes.Black, obtenerPosicion(header.Size.Width , Constantes.FUENTE_MEDIA), 0);

            dato = saltoLinea(comercio.Text, 30);
            e.Graphics.DrawString(dato, media, Brushes.Black, obtenerPosicion(comercio.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);
            if (dato.IndexOf("\n") != -1)
                y += 15;
            dato = "";

            dato = saltoLinea(direccion.Text, 40);
            e.Graphics.DrawString(dato, media, Brushes.Black, obtenerPosicion(direccion.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);
            if (dato.IndexOf("\n") != -1)
                y += 15;
            dato = "";

            e.Graphics.DrawString(afiliacion.Text + " - " + terminal.Text, media, Brushes.Black, obtenerPosicion(afiliacion.Size.Width + terminal.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);

            dato = saltoLinea(emisor.Text, 30);
            e.Graphics.DrawString(dato, media, Brushes.Black, obtenerPosicion(emisor.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);
            if (dato.IndexOf("\n") != -1)
                y += 15;
            dato = "";

            e.Graphics.DrawString(fecha.Text, media, Brushes.Black, 10, y + incremento);
            e.Graphics.DrawString(hora.Text, media, Brushes.Black, 168, y += incremento);
            e.Graphics.DrawString(tarjeta.Text, media, Brushes.Black, obtenerPosicion(tarjeta.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);
            //e.Graphics.DrawString(vencimiento.Text, media, Brushes.Black, 10, y + incremento);
            e.Graphics.DrawString(mdoIngreso.Text, media, Brushes.Black, 168, y += incremento);
            e.Graphics.DrawString(tpoTransaccion.Text, media, Brushes.Black, obtenerPosicion(tpoTransaccion.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);
            e.Graphics.DrawString(monto.Text, mediaNegrita, Brushes.Black, obtenerPosicion(monto.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);

            if (autorizacion.Text.Length > 0)
                e.Graphics.DrawString(autorizacion.Text, media, Brushes.Black, 10, y += incremento);

            if (referencia.Text.Length > 0)
            {
                e.Graphics.DrawString(referencia.Text, media, Brushes.Black, 10, y += incremento);
            }

            if (arqc.Text.Length > 0)
                e.Graphics.DrawString(arqc.Text, media, Brushes.Black, 10, y += incremento);

            if (firma.Text.Length > 0)
            {
                y += 15;
                e.Graphics.DrawString(firma.Text, media, Brushes.Black, 10, y += incremento);
            }

            /*if (titular.Text.Length > 0)
                e.Graphics.DrawString(titular.Text, media, Brushes.Black, obtenerPosicion(titular.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);
*/
            if (promosion.Text.Length > 0)
                e.Graphics.DrawString(promosion.Text, media, Brushes.Black, obtenerPosicion(promosion.Size.Width, Constantes.FUENTE_MEDIA), y += incremento);

            leyenda_1.Text = asignaEspacios("PAGARÉ NEGOCIABLE UNICAMENTE CON", "INSTITUCIONES DE CREDITO.");

            e.Graphics.DrawString(leyenda_1.Text, chica, Brushes.Black, obtenerPosicion(leyenda_1.Size.Width, Constantes.FUENTE_CHICA), y += (incremento + 10));

            leyenda_2.Text = "Por este pagaré me obligo incondicionalmente a pagar a \n" +
                             "a la orden del  banco  acreditante  el importe de este \n" +
                             "título.Este pagaré procede del contrato de apertura de \n" +
                             "credito que el banco acreditante y  el tarjetahabiente \n" +
                             "tienen celebrado.";

            e.Graphics.DrawString(leyenda_2.Text, chica, Brushes.Black, 0, y += incremento);
        }


        private void imprimeLogoPredeterminada(object sender, PrintPageEventArgs ev)
        {
            try
            {
                this.generaPagarePredeterminado(sender, ev);
            }
            catch (FileNotFoundException)
            {
            }
        }

        //nuevo
        private void imprimeLogoConfig(object sender, PrintPageEventArgs ev)
        {
            try
            {
                this.generaPagareConfig(sender, ev);
            }
            catch (FileNotFoundException)
            {
            }
        }
        //nuevo

        // CAT
        private void imprimeCAT(object sender, PrintPageEventArgs ev)
        {
            try
            {
                this.generaPagareCAT(sender, ev);
            }
            catch (FileNotFoundException)
            {
            }
        }
        //

        private void generaPagarePredeterminado(Object sender, PrintPageEventArgs e)
        {
            string[] texto = null;
            Font mediana = new Font("Arial", 9.0f);
            Font medianaNegrita = new Font("Arial", 9.0f, FontStyle.Bold);
            Font chica = new Font("Arial", 8.0f);
            Font grande = new Font("Arial", 10.0f);
            int alto = 15;
            int ancho = 255;
            int numCaracteres = 32;
            int y = 0;

            StringFormat centro = new StringFormat();
            StringFormat izquierdo = new StringFormat();
            StringFormat derecho = new StringFormat();

            centro.Alignment = StringAlignment.Center;
            izquierdo.Alignment = StringAlignment.Near;
            derecho.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(header.Text, grande, Brushes.Black, new RectangleF(0, 0, ancho, alto), centro);
            y += alto;

            texto = divideTexto(comercio.Text, numCaracteres);
            if (texto != null)
            {
                if (texto.Length == 1)
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                else
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    e.Graphics.DrawString(texto[1], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                //y += alto;
            }

            texto = null;
            texto = divideTexto(direccion.Text, numCaracteres);

            if (texto != null)
            {
                if (texto.Length == 1)
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                else if (texto.Length == 2)
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    e.Graphics.DrawString(texto[1], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                else if (texto.Length == 3)
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    e.Graphics.DrawString(texto[1], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    e.Graphics.DrawString(texto[2], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                y += alto;
            }

            e.Graphics.DrawString(afiliacion.Text + " - " + terminal.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            e.Graphics.DrawString(fecha.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(hora.Text, mediana, Brushes.Black, new RectangleF(0, y, 245, alto), derecho);
            y += alto;

            /*MessageBox.Show(emisor.Text);
            e.Graphics.DrawString(emisor.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            /*texto = null;
            if (emisor.Text.Length > 0)
            {
                texto = divideTexto(emisor.Text, numCaracteres);
                if (texto != null)
                {
                    if (texto.Length == 1)
                    {
                        e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    }
                    else
                    {
                        e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                        e.Graphics.DrawString(texto[1], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    }
                    y += alto;
                }

                texto = null;
            }*/

            e.Graphics.DrawString(tarjeta.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            //e.Graphics.DrawString(vencimiento.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(mdoIngreso.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            //y += alto;

            e.Graphics.DrawString(tpoTransaccion.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            e.Graphics.DrawString(monto.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            if (mdoIngreso.Text.Length > 0)
            {
                e.Graphics.DrawString("INGRESO: " + mdoIngreso.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (autorizacion.Text.Length > 0)
            {
                e.Graphics.DrawString(autorizacion.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (referencia.Text.Length > 0)
            {
                e.Graphics.DrawString(referencia.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (arqc.Text.Length > 0)
            {
                e.Graphics.DrawString(arqc.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (firma.Text.Length > 0)
            {
                e.Graphics.DrawString(firma.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto; Se quito por peticion de Olivia
            }

            /*if (titular.Text.Length > 0)
            {
                e.Graphics.DrawString(titular.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }*/

            if (promosion.Text.Length > 1)
            {
                e.Graphics.DrawString(promosion.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }

            texto = new string[2];
            texto[0] = "PAGARE NEGOCIABLE UNICAMENTE CON";
            texto[1] = "INSTITUCIONES DE CREDITO.";

            e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            e.Graphics.DrawString(texto[1], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            texto = new string[5];
            texto[0] = "Por este pagaré me obligo incondicionalmente a";
            texto[1] = "pagar a la orden del banco acreditante el importe";
            texto[2] = "de este título. Este pagaré procede del contrato";
            texto[3] = "de apertura de crédito que el banco  acreditante";
            texto[4] = "y el tarjetahabiente tienen celebrado.";

            e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(texto[1], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(texto[2], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(texto[3], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(texto[4], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);

            /*y += alto;
            y += alto;

            e.Graphics.DrawString("", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);*/

            e.Graphics.DrawString(tpPagare.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;
            y += alto;

            e.Graphics.DrawString("", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
        }

        //nuevo
        private void generaPagareConfig(Object sender, PrintPageEventArgs e)
        {
            string[] texto = null;
            Font mediana = new Font("Arial", 9.0f);
            Font medianaNegrita = new Font("Arial", 9.0f, FontStyle.Bold);
            Font chica = new Font("Arial", 8.0f);
            Font grande = new Font("Arial",  10.0f);
            Font chicaFirma = new Font("Arial", 5.0f);
            int alto = 15;
            int y = 0;

            StringFormat centro = new StringFormat();
            StringFormat izquierdo = new StringFormat();
            StringFormat derecho = new StringFormat();

            centro.Alignment = StringAlignment.Center;
            izquierdo.Alignment = StringAlignment.Near;
            derecho.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(header.Text, grande, Brushes.Black, new RectangleF(0, 0, ancho, alto), centro);
            y += alto;

            e.Graphics.DrawString(comercio.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            /*
            texto = saltoLineaConfig(comercio.Text, carGrande);            
            if (texto != null)
            {
                if (texto.Length == 1)
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                else
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    e.Graphics.DrawString(texto[1], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                //y += alto;
            }*/
            

            texto = null;
            texto = saltoLineaConfig(direccion.Text, carGrande);

            for (int i = 0; i < texto.Length; i++)
                e.Graphics.DrawString(texto[i], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            
            e.Graphics.DrawString(afiliacion.Text + " - " + terminal.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            
            e.Graphics.DrawString(fecha.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(hora.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
            y += alto;


            e.Graphics.DrawString(emisor.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            /*
            texto = null;
            if (emisor.Text.Length > 0)
            {
                texto = saltoLineaConfig(emisor.Text, carGrande);
                if (texto != null)
                {
                    if (texto.Length == 1)
                    {
                        e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    }
                    else
                    {
                        e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                        e.Graphics.DrawString(texto[1], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    }
                    y += alto;
                }

                texto = null;
            }*/

            
            e.Graphics.DrawString(tarjeta.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            
            e.Graphics.DrawString(tpoTransaccion.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;
            

            if (totalPesos.Text.Length > 0)
            {
                //e.Graphics.DrawString("TOTAL " + leyendaMoneda.Text + " $ " + totalPesos.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto;

                e.Graphics.DrawString("TOTAL " + leyendaMoneda.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                e.Graphics.DrawString(" $ " + totalPesos.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                y += alto;
            }

            if (totalPuntos.Text.Length > 0)
            {
                //e.Graphics.DrawString("PAGADO CON PUNTOS BANCOMER $" + totalPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto;

                e.Graphics.DrawString("PAGADO CON PUNTOS BANCOMER", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                e.Graphics.DrawString("$ " + totalPuntos.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                y += alto;
            }

            if (monto.Text.Length > 0)
            {

                if (totalPuntos.Text.Length > 0)
                {
                    e.Graphics.DrawString("____________", mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                }
                e.Graphics.DrawString("TOTAL A PAGAR", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                e.Graphics.DrawString("$ " + monto.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                y += alto;

                //e.Graphics.DrawString("TOTAL A PAGAR $ " + monto.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto;
            }

            if (mdoIngreso.Text.Length > 0)
            {
                e.Graphics.DrawString("INGRESO: " + mdoIngreso.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if(consultaPuntos.Text.Equals("")){
                if (autorizacion.Text.Length > 0)
                {
                    e.Graphics.DrawString(autorizacion.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                    y += alto;
                }
            }

            if (referencia.Text.Length > 0)
            {
                e.Graphics.DrawString(referencia.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (arqc.Text.Length > 0)
            {
                e.Graphics.DrawString(arqc.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (aid.Text.Length > 0)
            {
                e.Graphics.DrawString(aid.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (label.Text.Length > 0)
            {
                e.Graphics.DrawString(label.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (firma.Text.Length > 0)
            {
                e.Graphics.DrawString(firma.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            }

            /*if (titular.Text.Length > 0)
            {
                e.Graphics.DrawString(titular.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }*/

            if (promosion.Text.Length > 1)
            {
                e.Graphics.DrawString(promosion.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }

            // puntos
            if (consultaPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(consultaPuntos.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }

            if (linea.Text.Length > 0)
            {
                e.Graphics.DrawString(linea.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (leyendaPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(leyendaPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }


            if (poolid.Text.Length > 0)
            {
                e.Graphics.DrawString(poolid.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (saldoAnteriorPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoAnteriorPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoAnteriorPesos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoAnteriorPesos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (saldoRedimidoPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoRedimidoPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoRedimidoPesos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoRedimidoPesos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoActualPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoActualPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoActualPesos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoActualPesos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (leyendaPuntosVen.Text.Length > 0)
            {
                e.Graphics.DrawString(leyendaPuntosVen.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            // puntos
          
            if(consultaPuntos.Text.Equals("")){
                texto = null;
                String leyenda = "PAGARE NEGOCIABLE UNICAMENTE";
                e.Graphics.DrawString(leyenda, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += 5;

                leyenda = " CON INSTITUCIONES DE CREDITO.";
                e.Graphics.DrawString(leyenda, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
                            
                leyenda = "Por este pagaré me obligo incondicionalmente a pagar a la orden del banco acreditante el importe de este título. Este pagaré procede del contrato de apertura de crédito que el banco acreditante y el tarjetahabiente tienen celebrado.";
                texto = formatea(leyenda, carChico);

                for (int i = 0; i < texto.Length; i++)
                    e.Graphics.DrawString(texto[i], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;

            }
            e.Graphics.DrawString(tpPagare.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;
            y += alto;

            e.Graphics.DrawString("", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            
        }
        //nuevo

        public static string[] divideTexto(string dato, int longitud)
        {
            string[] texto = null;

            if (dato.Length >= longitud)
            {
                if (dato.Substring(longitud).Length > 0)
                {
                    texto = new string[3];
                    string divide = dato.Substring(longitud, 1);

                    if (divide.Equals(" "))
                    {
                        texto[0] = dato.Substring(0, longitud);
                        if (dato.Length >= longitud * 2)
                        {
                            int pos = longitud * 2;
                            texto[1] = dato.Substring(longitud, pos);
                            texto[2] = dato.Substring(pos + 1);
                        }
                        else
                        {
                            texto[1] = dato.Substring(longitud);
                        }
                    }
                    else
                    {
                        int pos = dato.Substring(0, longitud).LastIndexOf(" ");
                        texto[0] = dato.Substring(0, pos);

                        if (dato.Length >= longitud * 2)
                        {
                            int pos_2 = dato.Substring(pos + 1, longitud).LastIndexOf(" ");

                            texto[1] = dato.Substring(pos + 1, pos_2);
                            texto[2] = dato.Substring(pos + 1 + pos_2 + 1);

                        }
                        else
                        {
                            texto[1] = dato.Substring(pos + 1);
                        }
                    }
                }
            }
            else
            {
                texto = new string[1];
                texto[0] = dato;
            }

            return texto;
        }

        //  CAT
        private void generaPagareCAT(Object sender, PrintPageEventArgs e)
        {
            string[] texto = null;
            Font mediana = new Font("Arial", 9.0f);
            Font medianaNegrita = new Font("Arial", 9.0f, FontStyle.Bold);
            Font chica = new Font("Arial", 8.0f);
            Font grande = new Font("Arial", 10.0f);
            Font chicaFirma = new Font("Arial", 7.0f);
            int alto = 15;
            int y = 0;

            StringFormat centro = new StringFormat();
            StringFormat izquierdo = new StringFormat();
            StringFormat derecho = new StringFormat();

            centro.Alignment = StringAlignment.Center;
            izquierdo.Alignment = StringAlignment.Near;
            derecho.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(header.Text, grande, Brushes.Black, new RectangleF(0, 0, ancho, alto), centro);
            y += alto;

            e.Graphics.DrawString(comercio.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            /*
            texto = saltoLineaConfig(comercio.Text, carGrande);            
            if (texto != null)
            {
                if (texto.Length == 1)
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                else
                {
                    e.Graphics.DrawString(texto[0], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    e.Graphics.DrawString(texto[1], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                }
                //y += alto;
            }*/


            texto = null;
            texto = saltoLineaConfig(direccion.Text, carGrande);

            for (int i = 0; i < texto.Length; i++)
                e.Graphics.DrawString(texto[i], mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;


            e.Graphics.DrawString(afiliacion.Text + " - " + terminal.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;


            e.Graphics.DrawString(fecha.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            e.Graphics.DrawString(hora.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
            y += alto;


            e.Graphics.DrawString(emisor.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            /*
            texto = null;
            if (emisor.Text.Length > 0)
            {
                texto = saltoLineaConfig(emisor.Text, carGrande);
                if (texto != null)
                {
                    if (texto.Length == 1)
                    {
                        e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    }
                    else
                    {
                        e.Graphics.DrawString(texto[0], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                        e.Graphics.DrawString(texto[1], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                    }
                    y += alto;
                }

                texto = null;
            }*/


            e.Graphics.DrawString(tarjeta.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            e.Graphics.DrawString(tpoTransaccion.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            if (totalPesos.Text.Length > 0)
            {
                //e.Graphics.DrawString("TOTAL " + leyendaMoneda.Text + " $ " + totalPesos.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto;

                e.Graphics.DrawString("TOTAL " + leyendaMoneda.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                e.Graphics.DrawString(" $ " + totalPesos.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                y += alto;
            }

            if (totalPuntos.Text.Length > 0)
            {
                //e.Graphics.DrawString("PAGADO CON PUNTOS BANCOMER $" + totalPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto;

                e.Graphics.DrawString("PAGADO CON PUNTOS BANCOMER", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                e.Graphics.DrawString("$ " + totalPuntos.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                y += alto;
            }

            if (monto.Text.Length > 0)
            {

                if (totalPuntos.Text.Length > 0)
                {
                    e.Graphics.DrawString("____________", mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                }
                e.Graphics.DrawString("TOTAL A PAGAR", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                e.Graphics.DrawString("$ " + monto.Text, mediana, Brushes.Black, new RectangleF(0, y, (ancho - 10), alto), derecho);
                y += alto;

                //e.Graphics.DrawString("TOTAL A PAGAR $ " + monto.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                //y += alto;
            }

            if (mdoIngreso.Text.Length > 0)
            {
                e.Graphics.DrawString("INGRESO: " + mdoIngreso.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (autorizacion.Text.Length > 0)
            {
                e.Graphics.DrawString(autorizacion.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (referencia.Text.Length > 0)
            {
                e.Graphics.DrawString(referencia.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (arqc.Text.Length > 0)
            {
                e.Graphics.DrawString(arqc.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (aid.Text.Length > 0)
            {
                e.Graphics.DrawString(aid.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (label.Text.Length > 0)
            {
                e.Graphics.DrawString(label.Text, mediana, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += 20;
            }

            if (firma.Text.Length > 0)
            {
                e.Graphics.DrawString(firma.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            }

            /*if (titular.Text.Length > 0)
            {
                e.Graphics.DrawString(titular.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }*/

            if (promosion.Text.Length > 1)
            {
                e.Graphics.DrawString(promosion.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }

            // puntos
            if (consultaPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(consultaPuntos.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
                y += alto;
            }

            if (linea.Text.Length > 0)
            {
                e.Graphics.DrawString(linea.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (leyendaPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(leyendaPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }


            if (poolid.Text.Length > 0)
            {
                e.Graphics.DrawString(poolid.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (saldoAnteriorPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoAnteriorPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoAnteriorPesos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoAnteriorPesos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (saldoRedimidoPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoRedimidoPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoRedimidoPesos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoRedimidoPesos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoActualPuntos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoActualPuntos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            if (saldoActualPesos.Text.Length > 0)
            {
                e.Graphics.DrawString(saldoActualPesos.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }

            if (leyendaPuntosVen.Text.Length > 0)
            {
                e.Graphics.DrawString(leyendaPuntosVen.Text, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
                y += alto;
            }
            // puntos

            texto = null;
            String leyenda = "PAGARE NEGOCIABLE UNICAMENTE";
            e.Graphics.DrawString(leyenda, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += 5;

            leyenda = " CON INSTITUCIONES DE CREDITO.";
            e.Graphics.DrawString(leyenda, chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;

            /*
            for (int i = 0; i < texto.Length; i++)
                e.Graphics.DrawString(texto[i], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;
            */

            leyenda = "Por este pagaré me obligo incondicionalmente a pagar a la orden del banco acreditante el importe de este título. Este pagaré procede del contrato de apertura de crédito que el banco acreditante y el tarjetahabiente tienen celebrado.";
            texto = formatea(leyenda, carChico);

            for (int i = 0; i < texto.Length; i++)
                e.Graphics.DrawString(texto[i], chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), izquierdo);
            y += alto;


            //e.Graphics.DrawString(tpPagare.Text, medianaNegrita, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);
            y += alto;
            y += alto;

            e.Graphics.DrawString("", chica, Brushes.Black, new RectangleF(0, y += alto, ancho, alto), centro);

        }
        //

        /*
         * Asigna los valores del pagare.
         */
        private bool asignaFormulario(string datos, string tpoPagare)
        {
            bool bEmisor = false;

            if (!obtenerValor("emisor", datos).Equals(""))
            {

                if (!obtenerValor("comercio", datos).Equals(""))
                    comercio.Text = obtenerValor("comercio", datos);

                if (!obtenerValor("direccion", datos).Equals(""))
                    direccion.Text = obtenerValor("direccion", datos);

                if (!obtenerValor("afiliacion", datos).Equals(""))
                    afiliacion.Text = obtenerValor("afiliacion", datos);

                if (!obtenerValor("serie", datos).Equals(""))
                    terminal.Text = obtenerValor("serie", datos);

                if (!obtenerValor("fechaTransaccion", datos).Equals(""))
                    fecha.Text = "FECHA: " + obtenerValor("fechaTransaccion", datos);

                if (!obtenerValor("horaTransaccion", datos).Equals(""))
                    hora.Text = "HORA: " + obtenerValor("horaTransaccion", datos);

                if (!obtenerValor(tpoPagare, datos).Equals(""))
                    tarjeta.Text = obtenerValor(tpoPagare, datos);
                else
                    tarjeta.Text = "";

                /*if (!obtenerValor("fechaVencimiento", datos).Equals(""))
                    vencimiento.Text = "VENC: " + obtenerValor("fechaVencimiento", datos);*/

                if (!obtenerValor("emisor", datos).Equals(""))
                    emisor.Text = obtenerValor("emisor", datos);

                if (!obtenerValor("tarjeta", datos).Equals(""))
                    tarjeta.Text = obtenerValor("tarjeta", datos);

                if (!obtenerValor("tipoOperacion", datos).Equals(""))
                    tpoTransaccion.Text = obtenerValor("tipoOperacion", datos);

                if (!obtenerValor("monto", datos).Equals(""))
                    monto.Text = "MONTO: $ " + obtenerValor("monto", datos) + " M.N";

                if (!obtenerValor("leyendaMoneda", datos).Equals(""))
                    leyendaMoneda.Text = "Moneda: " + obtenerValor("leyendaMoneda", datos);
                else
                    leyendaMoneda.Text = "";

                if (!obtenerValor("totalPesos", datos).Equals(""))
                    totalPesos.Text = "tPesos: " + obtenerValor("totalPesos", datos);
                else
                    totalPesos.Text = "";

                if (!obtenerValor("totalPuntos", datos).Equals(""))
                    totalPuntos.Text = "tPuntos: " + obtenerValor("totalPuntos", datos);
                else
                    totalPuntos.Text = "";

                if (!obtenerValor("autorizacion", datos).Equals("") && !obtenerValor("autorizacion", datos).Equals("000000"))
                    autorizacion.Text = "APROBACION: " + obtenerValor("autorizacion", datos);
                else
                    autorizacion.Text = "";

                if (!obtenerValor("referencia", datos).Equals(""))
                    referencia.Text = "REF: " + obtenerValor("referencia", datos);
                else
                    referencia.Text = "";

                if (!obtenerValor("tipoPago", datos).Equals(""))
                    mdoIngreso.Text = obtenerValor("tipoPago", datos);

                if (!obtenerValor("criptograma", datos).Equals(""))
                    arqc.Text = "ARQC: " + obtenerValor("criptograma", datos);
                else
                    arqc.Text = "";

                if (!obtenerValor("aid", datos).Equals(""))
                    aid.Text = "AID: " + obtenerValor("aid", datos);
                else
                    aid.Text = "";

                if (!obtenerValor("label", datos).Equals(""))
                    label.Text = "LABEL: " + obtenerValor("label", datos);
                else
                    label.Text = "";

                if (!obtenerValor("firma", datos).Equals(""))
                    firma.Text = obtenerValor("firma", datos);
                else
                    firma.Text = "";

                if (!obtenerValor("titular", datos).Equals(""))
                    titular.Text = obtenerValor("titular", datos);
                else
                    titular.Text = "";

                if (!obtenerValor("promocion", datos).Equals(""))
                    promosion.Text = obtenerValor("promocion", datos);
                else
                    promosion.Text = "";

                if (!obtenerValor("linea", datos).Equals(""))
                    linea.Text = "LINEA: " + obtenerValor("linea", datos);
                else
                    linea.Text = "";

                if (!obtenerValor("leyendaPuntos", datos).Equals(""))
                    leyendaPuntos.Text = "LLEYENDA PUNTOS: " + obtenerValor("leyendaPuntos", datos);
                else
                    leyendaPuntos.Text = "";

                if (!obtenerValor("poolid", datos).Equals(""))
                    poolid.Text = "POOLID: " + obtenerValor("poolid", datos);
                else
                    poolid.Text = "";

                if (!obtenerValor("label1", datos).Equals(""))
                    label1.Text = "LEYENDA MONEDA: " + obtenerValor("label1", datos);
                else
                    label1.Text = "";

                if (!obtenerValor("consultaPuntos", datos).Equals(""))
                    consultaPuntos.Text = "LEYENDA CONSUTA: " + obtenerValor("consultaPuntos", datos);
                else
                    consultaPuntos.Text = "";

                if (!obtenerValor("saldoAnteriorPuntos", datos).Equals(""))
                    saldoAnteriorPuntos.Text = "SALDOANTERIORPUNTOS: " + obtenerValor("saldoAnteriorPuntos", datos);
                else
                    saldoAnteriorPuntos.Text = "";

                if (!obtenerValor("saldoAnteriorPesos", datos).Equals(""))
                    saldoAnteriorPesos.Text = "SALDOANTERIORPESOS: " + obtenerValor("saldoAnteriorPesos", datos);
                else
                    saldoAnteriorPesos.Text = "";

                if (!obtenerValor("saldoRedimidoPuntos", datos).Equals(""))
                    saldoRedimidoPuntos.Text = "SALDOREDIMIDOPUNTOS: " + obtenerValor("saldoRedimidoPuntos", datos);
                else
                    saldoRedimidoPuntos.Text = "";

                if (!obtenerValor("saldoRedimidoPesos", datos).Equals(""))
                    saldoRedimidoPesos.Text = "SALDOREDIMIDOPESOS: " + obtenerValor("saldoRedimidoPesos", datos);
                else
                    saldoRedimidoPesos.Text = "";

                if (!obtenerValor("saldoActualPuntos", datos).Equals(""))
                    saldoActualPuntos.Text = "SALDOACTUALPUNTOS: " + obtenerValor("saldoActualPuntos", datos);
                else
                    saldoActualPuntos.Text = "";

                if (!obtenerValor("saldoActualPesos", datos).Equals(""))
                    saldoActualPesos.Text = "SALDOACTUALPESOS: " + obtenerValor("saldoActualPesos", datos);
                else
                    saldoActualPesos.Text = "";

                if (!obtenerValor("leyendaPuntosVen", datos).Equals(""))
                    leyendaPuntosVen.Text = "LeyendaPuntosVen: " + obtenerValor("leyendaPuntosVen", datos);
                else
                    leyendaPuntosVen.Text = "";

                if (tpoPagare.Equals("tarjeta"))
                    tpPagare.Text = "C-O-M-E-R-C-I-O";
                else
                    tpPagare.Text = "C-L-I-E-N-T-E";


                bEmisor = true;

            }
            else
                bEmisor = false;

            return bEmisor;

        }

        private bool asignaFormularioPD(string datos, string tpoPagare, int moneda)
        {
            
            bool bEmisor = false;

            if (!obtenerValor("comercio", datos).Equals(""))
                comercio.Text = obtenerValor("comercio", datos);

            if (!obtenerValor("direccion", datos).Equals(""))
            {
                direccion.Text = obtenerValor("direccion", datos);
            }

            if (!obtenerValor("afiliacion", datos).Equals(""))
                afiliacion.Text = obtenerValor("afiliacion", datos);

            if (!obtenerValor("afiliacion", datos).Equals(""))
                terminal.Text = obtenerValor("serie", datos);

            if (!obtenerValor("fechaTransaccion", datos).Equals(""))
                fecha.Text = "FECHA: " + obtenerValor("fechaTransaccion", datos);

            if (!obtenerValor("horaTransaccion", datos).Equals(""))
                hora.Text = "HORA: " + obtenerValor("horaTransaccion", datos);

            if (!obtenerValor("tarjeta", datos).Equals(""))
                tarjeta.Text = obtenerValor("tarjeta", datos);
            else
                tarjeta.Text = "";

            /*if (!obtenerValor("fechaVencimiento", datos).Equals(""))
                vencimiento.Text = "VENC: " + obtenerValor("fechaVencimiento", datos);*/

            
            if (!obtenerValor("emisor", datos).Equals("") && !obtenerValor("emisor", datos).Equals("EMISOR DESCONOCIDO"))
                emisor.Text = obtenerValor("emisor", datos);
            else
                emisor.Text = "";


            if (!obtenerValor("tipoOperacion", datos).Equals(""))
            {
                tpoTransaccion.Text = obtenerValor("tipoOperacion", datos);
                if (moneda == 1)
                    tpoTransaccion.Text += " USD";
            }
            if (!obtenerValor("autorizacion", datos).Equals("") && !obtenerValor("autorizacion", datos).Equals("000000"))
                autorizacion.Text = "APROBACION: " + obtenerValor("autorizacion", datos);
            else
                autorizacion.Text = "";

            if (!obtenerValor("referencia", datos).Equals(""))
                referencia.Text = "REF: " + obtenerValor("referencia", datos);
            else
                referencia.Text = "";

            if (!obtenerValor("tipoPago", datos).Equals(""))
                mdoIngreso.Text = obtenerValor("tipoPago", datos);

            if (!obtenerValor("criptograma", datos).Equals(""))
                arqc.Text = "ARQC: " + obtenerValor("criptograma", datos);
            else
                arqc.Text = "";

            if (!obtenerValor("firma", datos).Equals(""))
                firma.Text = obtenerValor("firma", datos);
            else
                firma.Text = "";

            if (obtenerValor("titular", datos).Length > 0 && !obtenerValor("firma", datos).Equals("AUTORIZADO MEDIANTE FIRMA ELECTRONICA") && !obtenerValor("firma", datos).Equals("AUTORIZADO SIN FIRMA") && !obtenerValor("tipoOperacion", datos).Equals("DECLINADA"))
            {
                /*if (firma.Text.Length > 0)
                {
                    titular.Text = "NOMBRE " + obtenerValor("titular", datos);
                }
                else
                {
                    titular.Text = "";
                }*/
            }
            //else
            //{
                /*
                if (!obtenerValor("firma", datos).Equals("AUTORIZADO MEDIANTE FIRMA ELECTRONICA") && !obtenerValor("firma", datos).Equals("AUTORIZADO SIN FIRMA") && !obtenerValor("tipoOperacion", datos).Equals("DECLINADA"))
                {
                    titular.Text = "NOMBRE_2 ";
                }
                else
                {*/
                   // titular.Text = "";
                //}
            //}

            if (!obtenerValor("promocion", datos).Equals(""))
                promosion.Text = obtenerValor("promocion", datos);
            else
                promosion.Text = "";

            if (!obtenerValor("label", datos).Equals(""))
                label.Text = "AL: " + obtenerValor("label", datos);
            else
                label.Text = "";


            if (!obtenerValor("aid", datos).Equals(""))
                aid.Text = "AID: " + obtenerValor("aid", datos);
            else
                aid.Text = "";

            bEmisor = true;

            if (tpoPagare.Equals("tarjeta"))
            {
                tpPagare.Text = "C-O-M-E-R-C-I-O";
                linea.Text = "";
                leyendaPuntos.Text = "";
                poolid.Text = "";
                saldoAnteriorPuntos.Text = "";
                saldoAnteriorPesos.Text = "";
                saldoRedimidoPuntos.Text = "";
                saldoRedimidoPesos.Text = "";
                saldoActualPuntos.Text = "";
                saldoActualPesos.Text = "";
                totalPesos.Text = "";
                totalPuntos.Text = "";
                leyendaPuntosVen.Text = "";
                leyendaMoneda.Text = "";
                consultaPuntos.Text = "";

                //
                if (!obtenerValor("saldoRedimidoPuntos", datos).Equals(""))
                {
                    totalPesos.Text = obtenerValor("total", datos);
                    totalPuntos.Text = obtenerValor("saldoRedimidoPesos", datos);
                }
                monto.Text = obtenerValor("monto", datos);
                //

                if (!obtenerValor("monto", datos).Equals(""))
                {
                    if (moneda == 1)
                        monto.Text = "MONTO: USD " + obtenerValor("monto", datos);
                    else
                        monto.Text = obtenerValor("monto", datos) + " M.N";
                }
                else
                {
                    monto.Text = "";
                }
            }
            else
            {
                tpPagare.Text = "C-L-I-E-N-T-E";

                if (!obtenerValor("leyenda1", datos).Equals(""))
                {
                    consultaPuntos.Text = obtenerValor("leyenda1", datos);
                }
                else
                {
                    consultaPuntos.Text = "";
                }
                if (!obtenerValor("moneda", datos).Equals(""))
                {
                    leyendaMoneda.Text = obtenerValor("moneda", datos);
                }
                else
                {
                    leyendaMoneda.Text = "";
                }

                if (!obtenerValor("linea", datos).Equals(""))
                {
                    linea.Text = obtenerValor("linea", datos);
                }
                else
                    linea.Text = "";

                if (!obtenerValor("puntos", datos).Equals(""))
                {
                    leyendaPuntos.Text = obtenerValor("puntos", datos);
                }
                else
                {
                    leyendaPuntos.Text = "";
                }
                if (!obtenerValor("poolid", datos).Equals(""))
                {
                    poolid.Text = obtenerValor("poolid", datos);
                }
                else
                {
                    poolid.Text = "";
                }
                if (!obtenerValor("saldoAnteriorPuntos", datos).Equals(""))
                {
                    saldoAnteriorPuntos.Text = "Saldo Anterior: " + obtenerValor("saldoAnteriorPuntos", datos) + " (PTS)";
                }
                else
                {
                    saldoAnteriorPuntos.Text = "";
                }
                if (!obtenerValor("saldoAnteriorPesos", datos).Equals(""))
                {
                    saldoAnteriorPesos.Text = "Importe Pesos: $" + obtenerValor("saldoAnteriorPesos", datos);
                }
                else
                {
                    saldoAnteriorPesos.Text = "";
                }
                if (!obtenerValor("saldoRedimidoPuntos", datos).Equals(""))
                {
                    saldoRedimidoPuntos.Text = "Saldo Redimido: " + obtenerValor("saldoRedimidoPuntos", datos) + " (PTS)";
                    totalPesos.Text = obtenerValor("total", datos);
                    totalPuntos.Text = obtenerValor("saldoRedimidoPesos", datos);
                    monto.Text = obtenerValor("monto", datos);
                }
                else
                {
                    saldoRedimidoPuntos.Text = "";
                    totalPesos.Text = "";
                    totalPuntos.Text = "";
                    if (!obtenerValor("total", datos).Equals(""))
                    {
                        monto.Text = obtenerValor("total", datos) + " " + leyendaMoneda.Text ;
                    }
                    else
                    {
                        monto.Text = "";
                    }
                }
                if (!obtenerValor("saldoRedimidoPesos", datos).Equals(""))
                {
                    saldoRedimidoPesos.Text = "Importe Pesos: $" + obtenerValor("saldoRedimidoPesos", datos);
                }
                else
                {
                    saldoRedimidoPesos.Text = "";
                }
                if (!obtenerValor("saldoActualPuntos", datos).Equals(""))
                {
                    saldoActualPuntos.Text = "Saldo Actual: " + obtenerValor("saldoActualPuntos", datos) + " (PTS)";
                }
                else
                {
                    saldoActualPuntos.Text = "";
                }
                if (!obtenerValor("saldoActualPesos", datos).Equals(""))
                {
                    saldoActualPesos.Text = "Importe Pesos: $" + obtenerValor("saldoActualPesos", datos);
                }
                else
                {
                    saldoActualPesos.Text = "";
                }

                if (!obtenerValor("vigencia", datos).Equals(""))
                {
                    leyendaPuntosVen.Text = "PTS Expiran " + obtenerValor("vigencia", datos);
                }
                else
                {
                    leyendaPuntosVen.Text = "";
                }
            }


            return bEmisor;

        }

        public string obtenerPagareT4205(string datos, string tpoPagare)
        {
            string datosImprimir = "";
            string cliente = "";

            if (asignaFormulario(datos + "|", tpoPagare))
            {

                if (tpoPagare.Equals("tarjetaEnmascarada"))
                    cliente = "@cnn C-L-I-E-N-T-E @br";


                datosImprimir += "@logo1 @br" +
                                 "@cnn" + saltoLineaT4250(comercio.Text, 40) + "@br" +
                                 "@cnn" + saltoLineaT4250(direccion.Text, 40) + "@br" +
                                 "@cnn" + afiliacion.Text + " - " + terminal.Text + "@br" +
                                 "@cnn" + saltoLineaT4250(emisor.Text, 40) + "@br" +
                                 "@lnn" + fecha.Text + "              " + hora.Text + "@br" +
                                 "@cnn" + tarjeta.Text + "@br" +
                                 cliente +
                                 //"@lnn" + vencimiento.Text + "                 " +
                                 mdoIngreso.Text + "@br" +
                                 "@cnb" + tpoTransaccion.Text + "@br" +
                                 "@cnn" + monto.Text + "@br";

                if (autorizacion.Text.Length > 0)
                    datosImprimir += "@lnn" + autorizacion.Text + "@br";

                if (referencia.Text.Length > 0)
                    datosImprimir += "@lnn" + referencia.Text + "@br";

                if (arqc.Text.Length > 0)
                    datosImprimir += "@lnn" + arqc.Text + "@br";

                if (firma.Text.Length > 0)
                {
                    datosImprimir += "@br";
                    datosImprimir += "@lnn" + firma.Text + "@br";
                }

                //if (titular.Text.Length > 0)
                   // datosImprimir += "@cnn" + titular.Text + "@br";
                else
                {
                    if (!tpoTransaccion.Text.Equals("DECLINADA"))
                        datosImprimir += "@br";
                }

                if (promosion.Text.Length > 0)
                    datosImprimir += "@cnn" + promosion.Text + "@br";

                datosImprimir += "@cnn" + "PAGARE NEGOCIABLE UNICAMENTE CON" +
                                  "@cnn" + "INSTITUCIONES DE CREDITO" + "@br" +
                                  "@lnn" + "POR ESTE PAGARE ME OBLIGO INCONDICIONAL-" +
                                  "@lnn" + "MENTE A PAGAR A LA ORDEN DEL BANCO ACRE-" +
                                  "@lnn" + "DITANTE  EL IMPORTE DE ESTE CREDITO QUE " +
                                  "@lnn" + "EL BANCO  ACREDITANTE  Y EL  TARJETAHA-" +
                                  "@lnn" + "BIENTE TIENEN CELEBRADO." +
                                  "@br" + "@br" + "@br" + "@br" + "@br" + "@br" + "@br";

            }
            else
                datosImprimir = "No se encontro el emisor de la tarjeta, no se genero el pagare.";

            return datosImprimir;
        }


        /*
         * Obtiene la posicion de x donde se colocara el texto.
         */
        private int obtenerPosicion(int longitud, int tam)
        {
            int pos = 0;

            if (longitud < tam)
                pos = Math.Abs((int)(tam - longitud)) / 2;
            else
                pos = 20;

            return pos;
        }

        /*
         * Asigna un salto de linea cada 37 caracteres.
         */
        public static string saltoLinea(string dato, int longitud)
        {
            if (dato.Length >= longitud)
            {
                if (dato.Substring(longitud).Length > 0)
                {
                    string divide = dato.Substring(longitud, 1);

                    if (divide.Equals(" "))
                    {
                        if (dato.Length > (longitud * 2))
                            dato = asignaEspacios(dato.Substring(0, longitud), dato.Substring(longitud, longitud));
                        else
                            dato = asignaEspacios(dato.Substring(0, longitud), dato.Substring(longitud));
                    }
                    else
                    {
                        int pos = dato.Substring(0, longitud).LastIndexOf(" ");

                        if (dato.Length > (longitud * 2))
                            dato = asignaEspacios(dato.Substring(0, pos), dato.Substring(pos + 1, longitud));
                        else
                            dato = asignaEspacios(dato.Substring(0, pos), dato.Substring(pos + 1));
                    }
                }
            }

            return dato;
        }

        /*
        * Asigna un salto de linea cada 40 caracteres.
        */
        public static string saltoLineaSF(string dato, int longitud)
        {
            if (dato.Length >= longitud)
            {
                if (dato.Substring(longitud).Length > 0)
                {
                    string divide = dato.Substring(longitud, 1);

                    if (divide.Equals(" "))
                    {
                        if (dato.Length > (longitud * 2))
                            dato = dato.Substring(0, longitud) + System.Environment.NewLine + dato.Substring(longitud, longitud);
                        else
                            dato = dato.Substring(0, longitud) + System.Environment.NewLine + dato.Substring(longitud);
                    }
                    else
                    {
                        int pos = dato.Substring(0, longitud).LastIndexOf(" ");

                        if (dato.Length > (longitud * 2))
                            dato = dato.Substring(0, pos) + System.Environment.NewLine + dato.Substring(pos + 1, longitud);
                        else
                            dato = dato.Substring(0, pos) + System.Environment.NewLine + dato.Substring(pos + 1);
                    }
                }
            }

            return dato;
        }


        public static string saltoLineaT4250(string dato, int longitud)
        {
            if (dato.Length >= longitud)
            {
                if (dato.Substring(longitud).Length > 0)
                {
                    string divide = dato.Substring(longitud, 1);

                    if (divide.Equals(" "))
                    {
                        if (dato.Length > (longitud * 2))
                            dato = dato.Substring(0, longitud) + "@cnn" + dato.Substring(longitud, longitud);
                        else
                            dato = dato.Substring(0, longitud) + "@cnn" + dato.Substring(longitud);
                    }
                    else
                    {
                        int pos = dato.Substring(0, longitud).LastIndexOf(" ");

                        if (dato.Length > (longitud * 2))
                            dato = dato.Substring(0, pos) + "@cnn" + dato.Substring(pos + 1, longitud);
                        else
                            dato = dato.Substring(0, pos) + "@cnn" + dato.Substring(pos + 1);
                    }
                }
            }

            return dato;
        }

        /*
         * Asigna espacios en blanco para alinear el texto centrado.
         */
        public static string asignaEspacios(string total, string parcial)
        {

            string dato = total + System.Environment.NewLine;

            if (total.Length > parcial.Length)
            {
                for (int i = 0; i < ((int)(total.Length - parcial.Length) / 2); i++)
                    dato = dato + " ";
            }
            else
            {
                for (int i = 0; i < ((int)(parcial.Length - total.Length) / 2); i++)
                    dato = " " + dato;
            }

            dato += parcial;

            return dato;
        }

        //Nuevo
        public String justifica(String linea, int tamLinea)
        {
            int pos = 0;
            int rango = 1;

            while ((linea.Length < tamLinea))
            {
                if ((pos = linea.IndexOf(' ', pos)) != -1)
                {
                    pos += rango;
                    linea = linea.Insert(pos++, " ");
                    pos++;
                }
                else
                {
                    rango++;
                    pos = 0;
                }
            }

            return linea;
        }

        public static String[] saltoLineaConfig(String texto, int tamLinea)
        {
            String[] fmtTexto = null;
            String[] divTexto = texto.Split(' ');
            double tamLineas = Math.Ceiling(Convert.ToDouble(texto.Length / tamLinea)) + 3;
            fmtTexto = new String[(int)tamLineas];
            int pos = 0;
            String tmp = "";
            String aux = "";

            for (int i = 0; i < (int)tamLineas; i++)
            {

                fmtTexto[i] = "";
                tmp = "";
                aux = "";
                while (tmp.Length <= tamLinea)
                {
                    if (pos < divTexto.Length)
                    {
                        tmp += divTexto[pos++] + " ";
                        if (tmp.Length <= tamLinea)
                        {
                            aux = tmp;
                        }
                        else
                        {
                            pos--;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                fmtTexto[i] = aux.Trim();
            }

            int tam = 0;

            for (int i = 0; i < fmtTexto.Length; i++)
            {
                if (fmtTexto[i].Length > 0)
                    tam++;
            }

            String[] respuesta = new String[tam];

            for (int i = 0; i < tam; i++)
            {
                respuesta[i] = fmtTexto[i];
            }

            return respuesta;
        }

        public String[] formatea(String linea, int tamLinea)
        {
            String[] formato;

            formato = saltoLineaConfig(linea, tamLinea);
            for (int i = 0; i < formato.Length - 1; i++)
            {
                formato[i] = justifica(formato[i], tamLinea);
            }

            return formato;
        }

        private void IPagare_Load(object sender, EventArgs e)
        {

        }

        private void leyendaMoneda_Click(object sender, EventArgs e)
        {

        }
        //Nuevo
    }
}