using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.VO
{
    class Informacion
    {
        private string lNivel2;
        private string nivel1Val;
        private string nivel1Dsc;
        private string nivel2Val;
        private string nivel2Dsc;
        private string servicioVal;
        private string servicioDsc;
        private string entidadDsc;
        private string afiliacion;
        private string correo;
        private string msjUsuario;
        private string msjError;

        private string dscNivel1;
        private string dscNivel2;
        private string dscTpoServicio;
        private string financiamiento;

        public Informacion()
        {
            lNivel2 = "";
            nivel1Val = "";
            nivel1Dsc = "";
            nivel2Val = "";
            nivel2Dsc = "";
            servicioVal = "";
            servicioDsc = "";
            entidadDsc = "";
            afiliacion = "";
            msjUsuario = "";
            msjError = "";
            dscNivel1 = ""; ;
            dscNivel2 = "";
            dscTpoServicio = "";
            financiamiento = "";
        }

        public void setLNivel2(string lNivel2)
        {
            this.lNivel2 = lNivel2;
        }

        public string getLNivel2()
        {
            return lNivel2;
        }


        public void setNivel1Val(string nivel1Val)
        {
            this.nivel1Val = nivel1Val;
        }

        public string getNivel1Val()
        {
            return nivel1Val;
        }

        public void setNivel1Dsc(string nivel1Dsc)
        {
            this.nivel1Dsc = nivel1Dsc;
        }

        public string getNivel1Dsc()
        {
            return nivel1Dsc;
        }



        public void setNivel2Val(string nivel2Val)
        {
            this.nivel2Val = nivel2Val;
        }

        public string getNivel2Val()
        {
            return nivel2Val;
        }

        public void setNivel2Dsc(string nivel2Dsc)
        {
            this.nivel2Dsc = nivel2Dsc;
        }

        public string getNivel2Dsc()
        {
            return nivel2Dsc;
        }

        public void setServicioVal(string servicioVal)
        {
            this.servicioVal = servicioVal;
        }

        public string getServicioVal()
        {
            return servicioVal;
        }

        public void setServicioDsc(string servicioDsc)
        {
            this.servicioDsc = servicioDsc;
        }

        public string getServicioDsc()
        {
            return servicioDsc;
        }

        public void setEntidadDsc(string entidadDsc)
        {
            this.entidadDsc = entidadDsc;
        }

        public string getEntidadDsc()
        {
            return entidadDsc;
        }

        public void setAfiliacion(string afiliacion)
        {
            this.afiliacion = afiliacion;
        }

        public string getAfiliacion()
        {
            return afiliacion;
        }

        public void setCorreo(string correo)
        {
            this.correo = correo;
        }

        public string getCorreo()
        {
            return correo;
        }

        public void setMsjUsuario(string msjUsuario)
        {
            this.msjUsuario = msjUsuario;
        }

        public string getMsjUsuario()
        {
            return msjUsuario;
        }

        public void setMsjError(string msjError)
        {
            this.msjError = msjError;
        }

        public string getMsjError()
        {
            return msjError;
        }

        public void setDcsNivel1(string dscNivel1)
        {
            this.dscNivel1 = dscNivel1;
        }

        public string getDcsNivel1()
        {
            return dscNivel1;
        }

        public void setDcsNivel2(string dscNivel2)
        {
            this.dscNivel2 = dscNivel2;
        }

        public string getDcsNivel2()
        {
            return dscNivel2;
        }

        public void setDsTpoServicio(string dscTpoServicio)
        {
            this.dscTpoServicio = dscTpoServicio;
        }

        public string getDcsTpoServicio()
        {
            return dscTpoServicio;
        }


        public void setFinanciamiento(string financiamiento)
        {
            this.financiamiento = financiamiento;
        }

        public string getFinanciamiento()
        {
            return financiamiento;
        }
    }
}
