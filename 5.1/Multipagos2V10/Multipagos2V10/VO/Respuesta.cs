using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.VO
{
    class Respuesta
    {
        private string autorizacion;
        private string referencia;
        private int entidad;
        private string mensaje;
        private bool imprimir;
        private int operacion;
        private int nivel1;
        private int nivel2;
        private int servicio;
        private string pagare;
        private string res;
        private string fecha;
        private string hora;
        private string transmicion;
        private string criptograma;
        private string script;
        private string codigoRespuesta;
        private string alabel;
        private string aid;
        private string titular;
        private string consecutivo; //amex

        public Respuesta()
        {
            autorizacion = "";
            referencia = "";
            entidad = 0;
            mensaje = "";
            //mensajeError = "";
            imprimir = false;
            operacion = 0;
            nivel1 = 0;
            nivel2 = 0;
            servicio = 0;
            pagare = "";
            res = "";
            fecha = "";
            hora = "";
            transmicion = "";
            criptograma = "";
            script = "";
            codigoRespuesta = "";
            consecutivo = "";
            alabel = "";
            aid = "";
            titular = "";
        }

        public void setAutorizacion(string autorizacion)
        {
            this.autorizacion = autorizacion;
        }

        public string getAutorizacion()
        {
            return autorizacion;
        }

        public void setReferencia(string referencia)
        {
            this.referencia = referencia;
        }

        public string getreRerencia()
        {
            return referencia;
        }

        public void setEntidad(int entidad)
        {
            this.entidad = entidad;
        }

        public int getEntidad()
        {
            return entidad;
        }

        public void setMensaje(string mensaje)
        {
            this.mensaje = mensaje;
        }

        public string getMensaje()
        {
            return mensaje;
        }

        public void setImprimir(bool imprimir)
        {
            this.imprimir = imprimir;
        }

        public bool isImprimir()
        {
            return imprimir;
        }

        public void setoperacion(int operacion)
        {
            this.operacion = operacion;
        }

        public int getOperacion()
        {
            return operacion;
        }

        public void setnivel1(int nivel1)
        {
            this.nivel1 = nivel1;
        }

        public int getNivel1()
        {
            return nivel1;
        }

        public void setnivel2(int nivel2)
        {
            this.nivel2 = nivel2;
        }

        public int getNivel2()
        {
            return nivel2;
        }

        public void setservicio(int servicio)
        {
            this.servicio = servicio;
        }

        public int getServicio()
        {
            return servicio;
        }

        public void setPagare(string pagare)
        {
            this.pagare = pagare;
        }

        public string getPagare()
        {
            return pagare;
        }


        public void setRes(string res)
        {
            this.res = res;
        }

        public string getRes()
        {
            return res;
        }

        public void setHora(string hora)
        {
            this.hora = hora;
        }

        public string getHora()
        {
            return hora;
        }

        public void setFecha(string fecha)
        {
            this.fecha = fecha;
        }

        public string getFecha()
        {
            return fecha;
        }

        public void setTrasmicion(string transmicion)
        {
            this.transmicion = transmicion;
        }

        public string getTransmicion()
        {
            return transmicion;
        }

        public void setCriptograma(string criptograma)
        {
            this.criptograma = criptograma;
        }

        public string getCriptograma()
        {
            return criptograma;
        }

        public void setScript(string script)
        {
            this.script = script;
        }

        public string getScript()
        {
            return script;
        }

        public void setCodigoRespuesta(string codigoRespuesta)
        {
            this.codigoRespuesta = codigoRespuesta;
        }

        public string getCodigoRespuesta()
        {
            return codigoRespuesta;
        }

        public void setConsecutivo(string consecutivo)
        {
            this.consecutivo = consecutivo;
        }

        public string getConsecutivo()
        {
            return consecutivo;
        }
    }
}
