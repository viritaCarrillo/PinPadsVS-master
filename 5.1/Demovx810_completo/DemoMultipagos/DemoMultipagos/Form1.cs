/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO.Ports;*/
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Collections;

using log4net;
using log4net.Config;


namespace DemoMultipagos
{
    public partial class Form1 : Form
    {        
        Multipagos2V10.LeeBandaChip leemp2 = null;

        public Form1()
        {

            InitializeComponent();

            leemp2 = new Multipagos2V10.LeeBandaChip();

            String[] lista = leemp2.impresoras();
            for (int i = 0; i < lista.Length; i++)
            {
                impresora.Items.Add(lista[i]);
            }

            impresora.SelectedIndex = 0;

            string banderaLlave = leemp2.banderaLLave();

            if (leemp2.getMensaje() != null && leemp2.getMensaje().Length > 0)
            {
                MessageBox.Show(leemp2.getMensaje());
            }

            System.Console.WriteLine("banderaLlave: -->" + banderaLlave + "<--");

            if (banderaLlave.Equals("1"))
            {

                MessageBox.Show("Sincronizando pinpad, espere un momento \nPor favor no deconecte la terminal");
                string res = "";
                leemp2.llave();

                res = leemp2.compra("",
                                            "",
                                            Convert.ToInt32(nivel1.Text),
                                            servicio.Text,
                                            Convert.ToInt32(moneda.Text),
                                            "", // importe
                                            "", // titular
                                            "", //val_3
                                            "", // val_6
                                            "", // val_11
                                            "", // val_12
                                            Convert.ToInt32(entidad.Text),
                                            "", // val_16
                                            "",  // val_19
                                            "", // val_20
                                            mail.Text,
                                            "CARGA",
                                            leemp2.getTagEMV(),
                                            2); //applabel


                String tokenEX = "";
                if (res != null)
                {
                    string[] datos = res.Split('|');
                    tokenEX = datos[10];

                }
                leemp2.carga(tokenEX);

                MessageBox.Show(leemp2.getMensaje());

                //falta actualizar properties a 0
            }
        }

        private void enviar_Click(object sender, EventArgs e)
        {
            //System.Console.WriteLine("Inicio");
            enviar.Enabled = false;    
            string digest = leemp2.encriptaHmac(secuencia.Text + referencia.Text + importe.Text + leemp2.getCvv2());
            string digestVal = "";//digest.Text;
            string res = "";
            int puntos = 2;
            string valida = null;
            string lealtad = "";
            string pan = "";
            string tags = "";

            valida = leemp2.valida(entidad.Text, leemp2.encriptaRijndael(leemp2.getNumTarjeta()), mail.Text, "CONSULTA");
            Hashtable hdatos = leemp2.mapea(valida);
            lealtad = (string)hdatos["binBancomer"];

            if (lealtad.Equals("SI") && transaccion.Text.Equals("1"))
            {
                if (MessageBox.Show("CON PUNTOS", "PUNTOS BANCOMER", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    puntos = 1;
                else
                    puntos = 0;
            }
            else
            {
                puntos = 2;
            }

            if (transaccion.Text.Equals("1"))
            {
                tags = leemp2.getTagEMV();
                pan = leemp2.getNumTarjeta();
                res = leemp2.compra(secuencia.Text, 
                                    referencia.Text, 
                                    Convert.ToInt32(nivel1.Text), 
                                    servicio.Text, 
                                    Convert.ToInt32(moneda.Text), 
                                    importe.Text, 
                                    titular.Text,
                                    leemp2.encriptaRijndael(leemp2.getNumTarjeta()),
                                    digest,         
                                    "",             
                                    "",             
                                    Convert.ToInt32(entidad.Text), 
                                    tpTarjeta.Text, 
                                    val_19.Text,    
                                    periodo.Text,   
                                    mail.Text, 
                                    "PAGO", 
                                    leemp2.getTagEMV(),
                                    puntos); 
            }
            else if (transaccion.Text.Equals("8"))
            {
                res = leemp2.consulta(Convert.ToInt32(entidad.Text), 
                                      Convert.ToInt32(nivel1.Text), 
                                      Convert.ToInt32(moneda.Text), 
                                      Convert.ToInt32(servicio.Text), 
                                      secuencia.Text, 
                                      referencia.Text,
                                      leemp2.encriptaRijndael(leemp2.getNumTarjeta()),
                                      digestVal, 
                                      "1", 
                                      "CONSULTA", 
                                      mail.Text, 
                                      titular.Text, 
                                      leemp2.getTagEMV());
            }
            else if (transaccion.Text.Equals("2"))
            {
                res = leemp2.cancelacion(secuencia.Text, 
                                         referencia.Text, 
                                         Convert.ToInt32(nivel1.Text), 
                                         servicio.Text, 
                                         Convert.ToInt32(moneda.Text), 
                                         importe.Text, 
                                         titular.Text,
                                         leemp2.encriptaRijndael(leemp2.getNumTarjeta()), 
                                         digestVal, 
                                         Convert.ToInt32(entidad.Text), 
                                         tpTarjeta.Text, 
                                         val_19.Text, 
                                         periodo.Text, 
                                         mail.Text, 
                                         "CANCELACION",
                                         leemp2.getTagEMV(),                                       
                                         autorizacion.Text);
            }


            if (reverso.Text.Equals("1"))
            {
                res = null;
            }

            if (res != null)
            {
                string criptograma = "";
                string script = "";
                string aut = "";
                string con = "";
                string llave = "";
                string bines = "";
                string cdRespuesta = "";
                string tokenET = "";
                int telecarga = 0;

                string[] datos = res.Split('|');
                if (datos.Length == 14 && !transaccion.Text.Equals("8"))
                {
                    string respuesta = datos[1];
                    aut = datos[0];
                    script = datos[7];
                    criptograma = datos[8];
                    con = "";
                    llave = datos[11];
                    bines = datos[12];
                    cdRespuesta = datos[2];
                    tokenET = datos[9];
                    telecarga = Int32.Parse(datos[13]);

                    if (aut == null || aut.Length == 0)
                        aut = "000000";

                    if (criptograma == null)
                        criptograma = "";

                    if (script == null)
                        script = "";

                    if (llave == null)
                    {
                        llave = "";
                    }

                    //bool fin = false;
                    bool fin = leemp2.termina(aut, criptograma, script, cdRespuesta, "05", llave, bines, tokenET, 0);


                    if (!fin)
                    {                        
                        res = leemp2.cancelacion(secuencia.Text,
                                             referencia.Text,
                                             Convert.ToInt32(nivel1.Text),
                                             servicio.Text,
                                             Convert.ToInt32(moneda.Text),
                                             importe.Text,
                                             titular.Text,
                                             leemp2.encriptaRijndael(pan),
                                             digestVal,
                                             Convert.ToInt32(entidad.Text),
                                             tpTarjeta.Text,
                                             val_19.Text,
                                             periodo.Text,
                                             mail.Text,
                                             "RVTO",
                                             tags,
                                             "000000");

                        if(res != null){
                            MessageBox.Show("DECLINADA");
                        }
                    }

                    autorizacion.Text = aut;
                    consecutivo.Text = con;


                    if (leemp2.getReverso() == 1)
                    {
                        // invocar cancelacion
                        res = leemp2.cancelacion(secuencia.Text,
                                             referencia.Text,
                                             Convert.ToInt32(nivel1.Text),
                                             servicio.Text,
                                             Convert.ToInt32(moneda.Text),
                                             importe.Text,
                                             titular.Text,
                                             leemp2.encriptaRijndael(leemp2.getNumTarjeta()),
                                             digestVal,
                                             Convert.ToInt32(entidad.Text),
                                             tpTarjeta.Text,
                                             val_19.Text,
                                             periodo.Text,
                                             mail.Text,
                                             "REVERSO",
                                             leemp2.getTagEMV(),
                                             aut);
                    }



                    if (leemp2.getMensaje() != "")
                    {
                        MessageBox.Show("mensaje: " + leemp2.getMensaje());
                    }
                    else
                    {

                        MessageBox.Show(respuesta);

                        if (llave.Equals("2"))
                        {
                            //
                            MessageBox.Show("Sincronizando pinpad, espere un momento \nPor favor no deconecte la terminal");
                            res = "";
                            leemp2.llave();
                            res = leemp2.compra("",
                                                "",
                                                Convert.ToInt32(nivel1.Text),
                                                servicio.Text,
                                                Convert.ToInt32(moneda.Text),
                                                "", // importe
                                                "", // titular
                                                "", //val_3
                                                digest, // val_6
                                                "", // val_11
                                                "", // val_12
                                                Convert.ToInt32(entidad.Text),
                                                "", // val_16
                                                "",  // val_19
                                                "", // val_20
                                                mail.Text,
                                                "CARGA",
                                                leemp2.getTagEMV(),
                                                2); //applabel

                            Console.WriteLine("respuesta: " + res);

                            if (res != null)
                            {
                                /*hrespuesta = leemp2.mapea(res);
                                tokenET = (string)hrespuesta["tokenET"];

                                if (tokenET == null)
                                {
                                    tokenET = "";
                                }*/
                            }

                            leemp2.carga(tokenET);
                            MessageBox.Show(leemp2.getMensaje());

                            //
                        }
                    }
                }
                else
                {
                    if (transaccion.Text.Equals("8"))
                    {
                        Hashtable tmp = leemp2.mapea(res);
                        string auttmp = (string)tmp["autorizacion"];
                        string restmp = (string)tmp["cdRespuesta"];
                        //bool aux = leemp2.termina(auttmp, "", "", restmp, "05", "0", "0", "", 0);
                        MessageBox.Show("APROBADA");
                    }
                    else
                    {
                        //bool repuesta = leemp2.termina("000000", "", "", "100", "05", llave, bines, tokenET, 0);
                        MessageBox.Show("DECLINADA");
                    }
                }
            }
            else if (reverso.Text.Equals("1"))
            {
                Thread.Sleep(30000);
                //REVERSO
                res = leemp2.cancelacion(secuencia.Text,
                                         referencia.Text,
                                         Convert.ToInt32(nivel1.Text),
                                         servicio.Text,
                                         Convert.ToInt32(moneda.Text),
                                         importe.Text,
                                         titular.Text,
                                         leemp2.encriptaRijndael(leemp2.getNumTarjeta()),
                                         digestVal,
                                         Convert.ToInt32(entidad.Text),
                                         tpTarjeta.Text,
                                         val_19.Text,
                                         periodo.Text,
                                         mail.Text,
                                         "REVERSO",
                                         leemp2.getTagEMV(),
                                         "000000");

                // falta ver que mandar a la terminal
                leemp2.termina("000000", "", "", "99", "05", "", "", "", 0);
            }

            enviar.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool respuesta = false;            
            string digest = leemp2.encriptaHmac(secuencia.Text + referencia.Text + importe.Text + leemp2.getCvv2());

            respuesta = leemp2.compraAmex( secuencia.Text, 
                                           referencia.Text, 
                                           Convert.ToInt32(entidad.Text), 
                                           Convert.ToInt32(nivel1.Text), 
                                           servicio.Text, 
                                           titular.Text, 
                                           importe.Text,
                                           leemp2.encriptaRijndael(leemp2.getNumTarjeta()),
                                           leemp2.encriptaRijndael(leemp2.getCvv2()), 
                                           digest,
                                           "", // correo
                                           "" , // telefono
                                           "", // nombre
                                           "", // apellidos
                                           "", // codigo postal
                                           "", // direccion
                                           leemp2.encriptaRijndael(leemp2.getTrack1()),
                                           leemp2.encriptaRijndael(leemp2.getTrack2()), 
                                           Convert.ToInt32(val_19.Text), 
                                           Convert.ToInt32(periodo.Text), 
                                           Convert.ToInt32(moneda.Text),
                                           leemp2.getTagEMV(),
                                           leemp2.getTagE1(),
                                           mail.Text,
                                           leemp2.getNumeroSerie());


            /*respuesta = leemp2.compraAmex(secuencia.Text,
                                           referencia.Text,
                                           Convert.ToInt32(entidad.Text),
                                           Convert.ToInt32(nivel1.Text),
                                           servicio.Text,
                                           titular.Text,
                                           importe.Text,
                                           "373953192351004", //leemp2.encriptaRijndael(tarjeta.Text),
                                           digest,
                                           correo.Text,
                                           telefono.Text,
                                           nombre.Text,
                                           apellidos.Text,
                                           cdPostal.Text,
                                           direccion.Text,
                                           "",
                                           leemp2.encriptaRijndael("373953192351004=1703150412345"),
                                           Convert.ToInt32(val_19.Text),
                                           Convert.ToInt32(periodo.Text),
                                           Convert.ToInt32(moneda.Text),
                                           tagEMV.Text,
                                           mail.Text);*/


            if (respuesta)
            {

                //string criptograma = olee.getCripograma();
                string script = leemp2.getScript();
                string autorizacion = leemp2.getAutorizacion();

                System.Console.WriteLine("pagare:" + leemp2.getPagare());
                System.Console.WriteLine("script: " + script);
                //System.Console.WriteLine("criptograma: " + criptograma );
                System.Console.WriteLine("autorizacion: " + autorizacion);
                System.Console.WriteLine("Imprimir: " + leemp2.isImprimir());
                System.Console.WriteLine("mensaje: " + leemp2.getMensaje());
                System.Console.WriteLine("mensaje: " + leemp2.getMensajeError());

                if (autorizacion.Equals(""))
                    autorizacion = "000000";

                MessageBox.Show(leemp2.getPagare());
                MessageBox.Show("-->" + leemp2.getCriptograma() + "<--");
                //olee.terminaTransaccion(autorizacion, criptograma, script);
                //leemp2.termina(autorizacion, leemp2.getCriptograma(), leemp2.getScript(), leemp2.getCodigoRespuesta(), mdoLectura.Text, "0", "0", "", 0);
                leemp2.termina(autorizacion, leemp2.getCriptograma(), leemp2.getScript(), "00", leemp2.getMdoLectura(), "0", "0", "", 0);

                leemp2.imprimirComercio(leemp2.getPagare(), "PDFCreator", 0, 255, 32, 49);
            }
            else
            {
                
                string autorizacion = leemp2.getAutorizacion();
                if (autorizacion.Equals(""))
                    autorizacion = "000000";

                //leemp2.termina(autorizacion, leemp2.getCriptograma(), leemp2.getScript(), leemp2.getCodigoRespuesta(), mdoLectura.Text, "0", "0", "", 0);
                leemp2.termina(autorizacion, "", leemp2.getScript(), "00", leemp2.getMdoLectura(), "0", "0", "", 0);
                MessageBox.Show(leemp2.getMensaje());
            }
            //else
                //leemp2.terminaTransaccion("000000", "", "");
        }

        private void tarjeta_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("entidad       -->" + entidad.Text + "<--");
            System.Console.WriteLine("nivel1        -->" + nivel1.Text + "<--");
            System.Console.WriteLine("moneda        -->" + moneda.Text + "<--");
            System.Console.WriteLine("servicio      -->" + servicio.Text + "<--");
            System.Console.WriteLine("autorizacion  -->" + autorizacion.Text + "<--");
            System.Console.WriteLine("consecutivo   -->" + consecutivo.Text + "<--");
            System.Console.WriteLine("origen   -->" + origen.Text + "<--");

            /*bool respuesta = leemp2.cancelaAmex(Convert.ToInt32(entidad.Text), 
                                         Convert.ToInt32(nivel1.Text), 
                                         Convert.ToInt32(moneda.Text), 
                                         servicio.Text, 
                                         Convert.ToInt32(autorizacion.Text), 
                                         Convert.ToInt32(consecutivo.Text),
                                         origen.Text);*/

            /*if (respuesta)
            {
                leemp2.imprimirComercio(leemp2.getPagare(), "PDFCreator", 0, 255, 32, 49);
                Console.WriteLine(leemp2.getPagare());
            }
            else
            {
                MessageBox.Show(leemp2.getMensaje());
            }*/
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int tipo = 3;

            if(transaccion.Text.Equals("2")){
                tipo = 0;
            }
            
            if (transaccion.Text.Equals("8"))
            {               
                leemp2.lee(importe.Text, 3, Int32.Parse(moneda.Text), true);                
            }else{
                leemp2.lee(importe.Text, tipo, Int32.Parse(moneda.Text), false);
            }           
            
            if (leemp2.getMensaje().Equals(""))
            {
                tarjeta.Text = "************" + leemp2.getNumTarjeta().Substring(12);
                titular.Text = leemp2.getName();
                plataforma.Text = leemp2.getPlataforma();                
            }
            else
            {
                System.Console.WriteLine("demo error -->" + leemp2.getMensajeError() + "<--");
                System.Console.WriteLine("demo error -->" + leemp2.getMensaje() + "<--");


                MessageBox.Show(leemp2.getMensaje() + leemp2.getMensajeError());
                MessageBox.Show("codigo:" + leemp2.getCdRespuesta() + "<->" + leemp2.getMensaje() + leemp2.getMensajeError());

                if (leemp2.getCdRespuesta().Equals("62"))
                {
                    MessageBox.Show("Sincronizando pinpad, espere un momento \nPor favor no deconecte la terminal");
                    string res = "";
                    leemp2.llave();

                    res = leemp2.compra("",
                                                "",
                                                Convert.ToInt32(nivel1.Text),
                                                servicio.Text,
                                                Convert.ToInt32(moneda.Text),
                                                "", // importe
                                                "", // titular
                                                "", //val_3
                                                "", // val_6
                                                "", // val_11
                                                "", // val_12
                                                Convert.ToInt32(entidad.Text),
                                                "", // val_16
                                                "",  // val_19
                                                "", // val_20
                                                mail.Text,
                                                "CARGA",
                                                leemp2.getTagEMV(),
                                                2); //applabel


                    String tokenEX = "";
                    if (res != null)
                    {
                        string[] datos = res.Split('|');
                        tokenEX = datos[10];

                    }
                    leemp2.carga(tokenEX);

                    MessageBox.Show(leemp2.getMensaje());
                }
            }
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            tarjeta.Text = "";
            titular.Text = "";
            plataforma.Text = "";
            autorizacion.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //string x =lee.encriptaRijndael(trac2.Text);
            //string y = lee.encriptaHmac(referencia.Text + secuencia.Text + importe.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //string x = lee.getBinesDeLealtad("","");
            //MessageBox.Show(x);

            //string x = lee.getBinesDeLealtad("snochebuena@adquira.com.mx", "CONSULTA");
            //System.Console.WriteLine(x);
            //MessageBox.Show(x + "-->" + x.Length);


            //string x = lee.encriptaHmac(referencia.Text + secuencia.Text + importe.Text);
            //string x = lee.encriptaHmac("123456789012345678901234567890" + "123456789012345678901234567890" + "1234567890.00");
            //string x = lee.encriptaHmac("selene");
            //MessageBox.Show(x.Length + "-->" + x + "-->" + "123456789012345678901234567890" + "123456789012345678901234567890" + "1234567890.00");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            /*string res = lee.getStatus(Convert.ToInt32(entidad.Text), 
                                       Convert.ToInt32(nivel1.Text), 
                                       Convert.ToInt32(moneda.Text), 
                                       Convert.ToInt32(servicio.Text), 
                                       secuencia.Text, 
                                       referencia.Text, 
                                       mail.Text, "STATUS", "1");*/

            //MessageBox.Show(res);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            string respuesta = null;

            respuesta = leemp2.devolucion(secuencia.Text,
                                          referencia.Text,
                                          Convert.ToInt32(nivel1.Text),
                                          servicio.Text,
                                          Convert.ToInt32(moneda.Text),
                                          importe.Text,
                                          Convert.ToInt32(entidad.Text),
                                          mail.Text,
                                          "DEVOLUCION",
                                          autorizacion.Text);

            if(respuesta != null){
                string []datos = respuesta.Split('|');
                if(datos.Length > 2){
                    string aut = datos[1];
                    if (!aut.Equals("000000"))
                    {
                        MessageBox.Show("APROBADA");
                    }
                    else
                    {
                        MessageBox.Show("DECLINADA");
                    }
                }
            }
        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            //string script = "72379F180430000252860E04DA9F580901D9C01D7B8C53CAEA860E04DA9F5809022B180A637AB5EFEF860E04DA9F580903325928288DA7DB70";
            string script = "";
            //string criptograma = "910AE7F24ADA3198CBE13030";
            string criptograma = "";

            leemp2.termina("000000", criptograma, script, "99", "05", "0", "0", "", 0);

            //lee.terminaTransaccion(autorizacion.Text,"","");
            /*if (autorizacion.Text.Length == 0)
                autorizacion.Text = "000000";
            lee.terminaTransaccion(autorizacion.Text, "", "");
            lee.terminaTransaccion(autorizacion.Text, "", "");
            lee.terminaTransaccion(autorizacion.Text, "", "");*/

            //leemp2.termina("123456", "", "", "00", 1, "0", "0", "", 0);

            /*ILog log = null;            
            log4net.Config.XmlConfigurator.Configure();
            log = log4net.LogManager.GetLogger("log4Net");
            log.Info("---->SELENE NOCHEBUENA ROJO<----");*/

            //leemp2.termina("000000", "", "", "49", mdoLectura.Text, "0", "0", "", 0);
            //leemp2.termina("123456", "", "", "49", "1", "0", "1", "! ET00366 A65815990105500000000C5100001503200117B17962F99CDF3A3359D5BAD7F105525124B6CD9644333936DFD32BDD35D6D415D6A6B171A1131ADB67D01F863EF3F6F4B00D94B2E8A493D9FD04C3D9E32D34DCA1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2A1407B37568238E2130B60AC", 0);
            //leemp2.termina("123456", "", "", "49", "1", "0", "1", "", 0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            /*string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                         "<transacciones registros=\"2\">" +
                         "<registro >" +
	                     "<entidad>10710</entidad>" +
	                     "<nivel1>38272</nivel1>" +
	                     "<nivel2>0</nivel2>" +
	                     "<servicio>1196</servicio>" +
                         "<referencia>2013061101</referencia>" +
	                     "<secuencia>01</secuencia>" +
                         "<autorizacion>adq347</autorizacion>" +
                         "</registro>" +
                         "<registro numero=\"2\">" +
	                     "<entidad>10700</entidad>" +
	                     "<nivel1>0</nivel1>" +
	                     "<nivel2>0</nivel2>" +
	                     "<servicio>99</servicio>" +
	                     "<referencia>2013060702</referencia>" +
	                     "<secuencia>02</secuencia>" +
                         "<autorizacion>adq456</autorizacion>" +
                         "</registro>" +	
                         "</transacciones>";

            System.Console.WriteLine(lee.enviar(xml, entidad.Text, mail.Text));
            */
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            /*String pan = leemp2.encriptaRijndael(tarjeta.Text);
            String bines = leemp2.valida(entidad.Text, pan, mail.Text, "CONSULTA");
            MessageBox.Show(bines);
            System.Console.WriteLine(bines);*/           
        }

        private void button9_Click(object sender, EventArgs e)
        {
            String res = "";

            MessageBox.Show("Espere un momento inicia la sincronizacion de la pinpad \nPor favor no desconecte la terminal");
            leemp2.llave();

            res = leemp2.compra(secuencia.Text,
                                referencia.Text,
                                Convert.ToInt32(nivel1.Text),
                                servicio.Text,
                                Convert.ToInt32(moneda.Text),
                                "",     // importe
                                "",     // titular
                                "",     // val_3
                                "",     // val_6
                                "",     // val_11
                                "",     // val_12
                                Convert.ToInt32(entidad.Text),
                                "",     // val_16
                                "",     // val_19
                                "",     // val_20
                                mail.Text,
                                "CARGA",
                                leemp2.getTagEMV(),
                                2);

            Console.WriteLine("respuesta_CARGA: " +  res);
            string tokenEX = "";

            if (res != null && res.Length >0)
            {
                string []datos = res.Split('|');

                tokenEX = datos[10];
                Console.WriteLine("tokenEX" + tokenEX);

                if (tokenEX == null)
                {
                    tokenEX = "";                    
                }                
            }
            else
            {
                tokenEX = "";
            }

            Console.WriteLine("respuesta_CARGA: " + res);
            leemp2.carga(tokenEX);
            //leemp2.carga("");
            MessageBox.Show(leemp2.getMensaje());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            leemp2.actualiza();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            

            string datos = leemp2.reimprime(Convert.ToInt32(entidad.Text),
                                            Convert.ToInt32(nivel1.Text),
                                            Convert.ToInt32(moneda.Text),
                                            Convert.ToInt32(servicio.Text),
                                            referencia.Text,
                                            secuencia.Text,
                                            transaccion.Text,
                                            autorizacion.Text);

            MessageBox.Show(datos);
            //datos = "razonSocial=BBVA BANCOMER|comercio=DOMI ADQUIRA|direccion=Prol Paseo de la Reforma 1200 C.P. 5340 - Cruz Manca MEXICO, DF|afiliacion=3795903|serie=80607307|fechaTransaccion=19ABR18|horaTransaccion=16:52|tarjeta=************1922|emisor=BBVA BANCOMER/CREDITO|linea=======================================|leyenda1=CONSULTA DE BENEFICIOS|leyenda2=Puntos BBVA Bancomer|poolId=0000000001|saldoActualPuntos=1000000|saldoActualPesos=70000.00|autorizacion=019728|ingreso=|cdRespuesta=00|";
            leemp2.imprimirCliente(datos, impresora.Text, 0, 270, 10, 5);
            leemp2.imprimirComercio(datos, impresora.Text, 0, 270, 10, 5);
        }
    }

    //

    //
}