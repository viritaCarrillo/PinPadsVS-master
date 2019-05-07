using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.VO
{
    class Tarjeta
    {
        private String nuTarjeta;			// Numero de tarjeta.
        private String anio;				// Año de vencimiento.
        private String mes;					// Mes de vencimiento.
        private String track1;				// Track1.
        private String track2;				// Track2.
        private String mensaje;				// Mensaje para desplegar en la pinpad.
        private String codigoRespuesta;		// Codigo de respuesta al ejeuctar un comando.
        private String dscCodRespuesta;		// Descripcion del codigo de respuesta.
        private int tamAID;
        private String AID;
        private int tamPrefName;
        private String prefName;
        private int tamAppLabel;
        private String appLabel;
        private int tamServCode;
        private String servCode;
        private int tamName;				// Tamaño del nombre del tarjeta habiente.
        private String name;				// Nombre del tarjeta habiente.
        private int tamTrack2;				// Tamaño del track2.
        private bool bEjecucion;			// Para indicar el exito o fracaso al ejecutar un comando
        private int tamPan;
        private String Pan;
        private String comando;
        private String dia;
        private String numeroSerie;			// Numero de serie de la pinpad.
        private String tag5F34;
        private String tagEMV;              // TAGS EMV
        private String signatureFlag;
        private String tag9F27;             // Indicador de lectura.
        private String tag9F26;             // ARQC

        private String mensajeError;    // Mensaje de error.
        private byte[] llave;		    // Llave de encripcion.	
        private int statusLectura;      // 0 Tiempo agotado, 1 - Exitosa, 2 - Fallida
        private int evento;             // 1 - Activo, 2 - Desactiva.
        private String tpoTarjeta;      // Tipo de tarjeta (1-VISA, 2-MASTERCARD)
        private bool banda;
        private String plataforma;


        private String cvv2;
        private String mdoLectura;
        private String tagE1;
        private String tokenES;
        private String tokenEY;
        private String tokenEZ;
        private String tokenR1;
        private int reverso;

        public Tarjeta()
        {
            nuTarjeta = "";
            anio = "";
            mes = "";
            track1 = "";
            track2 = "";
            mensaje = "";
            codigoRespuesta = "";
            tamAID = 0;
            AID = "";
            tamPrefName = 0;
            prefName = "";
            tamAppLabel = 0;
            appLabel = "";
            tamServCode = 0;
            servCode = "";
            bEjecucion = false;
            comando = "";
            tagEMV = "";
            tag5F34 = "";
            signatureFlag = "";
            tag9F27 = "";
            tag9F26 = "";
            mensajeError = "";
            numeroSerie = "";
            statusLectura = -1;
            evento = 0;
            tpoTarjeta = "";
            banda = false;
            setReverso(0);
            setTokenES("");
            setTokenEY("");
            setTokenEZ("");
            setTokenR1("");
        }

        /**
        * Establece el numero de la tarjeta.
        * @param nuTarjeta - N&uacute;mero de la tarjeta.
        */
        public void setNuTarjeta(String nuTarjeta)
        {
            this.nuTarjeta = nuTarjeta;
        }

        /**
         * @return Devuelve el n&uacute;mero de la tarjeta.
         */
        public String getNuTarjeta()
        {
            return nuTarjeta;
        }

        /**
         * Establece el año de vencimiento.
         * @param anio - Anio de vencimiento.
         */
        public void setAnio(String anio)
        {
            this.anio = anio;
        }

        /**
         * @return Devuelve el año de vencimiento.
         */
        public String getAnio()
        {
            return anio;
        }

        /**
         * Establece el mes de vencimiento.
         * @param mes - Mes de vencimiento.
         */
        public void setMes(String mes)
        {
            this.mes = mes;
        }

        /**
         * @return Devuelve el mes de vencimiento.
         */
        public String getMes()
        {
            return mes;
        }

        /**
         * Establece el valor del track2
         * @param track2 - Track2.
         */
        public void setTrack2(String track2)
        {
            this.track2 = track2;
        }

        /**
         * @return Devuelve el track2.
         */
        public String getTrack2()
        {
            return track2;
        }

        /**
         * Establece el valor del track1
         * @param track1 - Track1.
         */
        public void setTrack1(String track1)
        {
            this.track1 = track1;
        }

        /**
         * @return Devuelve el valor del track1.
         */
        public String getTrack1()
        {
            return track1;
        }

        /**
         * Establece el mensaje a desplegar en la pinpad.
         * @param mensaje - Mensaje a desplegar en la pinpad.
         */
        public void setMensaje(String mensaje)
        {
            this.mensaje = mensaje;
        }

        /**
         * @return Devuelve el mensaje a desplegar en la pinpad.
         */
        public String getMensaje()
        {
            return mensaje;
        }

        /**
         * @param tamAID El tamAID a establecer.
         */
        public void setTamAID(int tamAID)
        {
            this.tamAID = tamAID;
        }

        /**
         * @return Devuelve tamAID.
         */
        public int getTamAID()
        {
            return tamAID;
        }

        /**
         * @param aID El aID a establecer.
         */
        public void setAID(String aID)
        {
            AID = aID;
        }

        /**
         * @return Devuelve aID.
         */
        public String getAID()
        {
            return AID;
        }

        /**
         * @param tamPrefName El tamPrefName a establecer.
         */
        public void setTamPrefName(int tamPrefName)
        {
            this.tamPrefName = tamPrefName;
        }

        /**
         * @return Devuelve tamPrefName.
         */
        public int getTamPrefName()
        {
            return tamPrefName;
        }

        /**
         * @param prefName El prefName a establecer.
         */
        public void setPrefName(String prefName)
        {
            this.prefName = prefName;
        }

        /**
         * @return Devuelve prefName.
         */
        public String getPrefName()
        {
            return prefName;
        }

        /**
         * @param tamAppLabel El tamAppLabel a establecer.
         */
        public void setTamAppLabel(int tamAppLabel)
        {
            this.tamAppLabel = tamAppLabel;
        }

        /**
         * @return Devuelve tamAppLabel.
         */
        public int getTamAppLabel()
        {
            return tamAppLabel;
        }

        /**
         * @param appLabel El appLabel a establecer.
         */
        public void setAppLabel(String appLabel)
        {
            this.appLabel = appLabel;
        }

        /**
         * @return Devuelve appLabel.
         */
        public String getAppLabel()
        {
            return appLabel;
        }

        /**
         * @param tamSerCode El tamSerCode a establecer.
         */
        public void setTamServCode(int tamServCode)
        {
            this.tamServCode = tamServCode;
        }

        /**
         * @return Devuelve tamSerCode.
         */
        public int getTamServCode()
        {
            return tamServCode;
        }

        /**
         * @param servCode El servCode a establecer.
         */
        public void setServCode(String servCode)
        {
            this.servCode = servCode;
        }

        /**
         * @return Devuelve servCode.
         */
        public String getServCode()
        {
            return servCode;
        }

        /**
         * Establece la longitud del tarjeta habiente.
         * @param tamName - Longitud del tarjeta habiente.
         */
        public void setTamName(int tamName)
        {
            this.tamName = tamName;
        }

        /**
         * @return Devuelve la longitud del tarjeta habiente.
         */
        public int getTamName()
        {
            return tamName;
        }

        /**
         * Establece el nombre del tarjeta habiente.
         * @param name - Nombre del tarjeta habiente.
         */
        public void setName(String name)
        {
            this.name = name;
        }

        /**
         * @return Devuelve el nombre del tarjeta habiente.
         */
        public String getName()
        {
            return name;
        }

        /**
         * Establece la longitud del trajeta habiente.
         * @param tamTrack2 El tamTrack2 a establecer.
         */
        public void setTamTrack2(int tamTrack2)
        {
            this.tamTrack2 = tamTrack2;
        }

        /**
         * @return Devuelve la longitud del track2.
         */
        public int getTamTrack2()
        {
            return tamTrack2;
        }

        /**
         * Establece el codigo de respuesta.
         * @param codigoRespueta - Codigo de respuesta.
         */
        public void setCodigoRespuesta(String codigoRespuesta)
        {
            this.codigoRespuesta = codigoRespuesta;
        }

        /**
         * @return Devuelve el codigo de respuesta.
         */
        public String getCodigoRespuesta()
        {
            return codigoRespuesta;
        }

        /**
         * Establece la descripci&oacute;n del codigo de respuesta.
         * @param dscCodRespuesta - Codigo de respuesta.
         */
        public void setDscCodRespuesta(String dscCodRespuesta)
        {
            this.dscCodRespuesta = dscCodRespuesta;
        }

        /**
         * @return Devuelve el codigo de respuesta.
         */
        public String getDscCodRespuesta()
        {
            return dscCodRespuesta;
        }

        /**
         * @param bEjecucion El bEjecucion a establecer.
         */
        public void setBEjecucion(bool bEjecucion)
        {
            this.bEjecucion = bEjecucion;
        }

        /**
         * @return Devuelve bEjecucion.
         */
        public bool getBEjecucion()
        {
            return bEjecucion;
        }

        /**
         * @param comando El comando a establecer.
         */
        public void setComando(String comando)
        {
            this.comando = comando;
        }

        /**
         * @return Devuelve comando.
         */
        public String getComando()
        {
            return comando;
        }

        /**
         * @param tamPan El tamPan a establecer.
         */
        public void setTamPan(int tamPan)
        {
            this.tamPan = tamPan;
        }

        /**
         * @return Devuelve tamPan.
         */
        public int getTamPan()
        {
            return tamPan;
        }

        /**
         * @param pan El pan a establecer.
         */
        public void setPan(String pan)
        {
            Pan = pan;
        }

        /**
         * @return Devuelve pan.
         */
        public String getPan()
        {
            return Pan;
        }

        /**
         * Establece el dia de vencimiento.
         * @param dia - Dia de vencimiento.
         */
        public void setDia(String dia)
        {
            this.dia = dia;
        }

        /**
         * @return Devuelve el dia de vencimiento.
         */
        public String getDia()
        {
            return dia;
        }

        /**
         * Establece el numero de serie de la pinpad.
         * @param numeroSerie - N&uacute;mero de serie de la pinpad.
         */
        public void setNumeroSerie(String numeroSerie)
        {
            this.numeroSerie = numeroSerie;
        }

        /**
         * @return Devuelve el n&uacute;mero de serie de la pinpad.
         */
        public String getNumeroSerie()
        {
            return numeroSerie;
        }

        public void setTag5F34(String tag5F34)
        {
            this.tag5F34 = tag5F34;
        }

        public String getTag5F34()
        {
            return tag5F34;
        }

        public void setTagEMV(String tagEMV)
        {
            this.tagEMV = tagEMV;
        }

        public String getTagEMV()
        {
            return tagEMV;
        }

        public void setSignatureFlag(String signatureFlag)
        {
            this.signatureFlag = signatureFlag;
        }

        public String getSignatureFlag()
        {
            return signatureFlag;
        }

        public void setTag9F27(String tag9F27)
        {
            this.tag9F27 = tag9F27;
        }

        public String getTag9F27()
        {
            return tag9F27;
        }

        public void setTag9F26(String tag9F26)
        {
            this.tag9F26 = tag9F26;
        }

        public String getTag9F26()
        {
            return tag9F26;
        }



        /**
         * @param llave El llave a establecer.
         */
        public void setLlave(byte[] llave)
        {
            this.llave = llave;
        }

        /**
         * @return Devuelve llave.
         */
        public byte[] getLlave()
        {
            return llave;
        }

        public void setStatusLectura(int statusLectura)
        {
            this.statusLectura = statusLectura;
        }

        public int getStatusLectura()
        {
            return statusLectura;
        }

        /**
         * @param mensaje - Asigna un mensaje cuando ocurre un error.
         */
        public void setMensajeError(String mensajeError)
        {
            this.mensajeError = mensajeError;
        }

        /**
         * @return Devuelve mensaje cuando ocurra un error.
         */
        public String getMensajeError()
        {
            return mensajeError;
        }

        public void setEvento(int evento)
        {
            this.evento = evento;
        }

        public int getEvento()
        {
            return evento;
        }

        public void setTpoTarjeta(String tpoTarjeta)
        {
            this.tpoTarjeta = tpoTarjeta;
        }

        public string getTpoTarjeta()
        {
            return this.tpoTarjeta;
        }

        public void setBanda(bool banda)
        {
            this.banda = banda;
        }

        public bool isBanda()
        {
            return banda;
        }

        public void setCvv2(String cvv2)
        {
            this.cvv2 = cvv2;
        }

        public String getCvv2()
        {
            return cvv2;
        }

        public void setMdoLectura(String mdoLectura)
        {
            this.mdoLectura = mdoLectura;
        }

        public String getMdoLectura()
        {
            return mdoLectura;
        }

        public void setTagE1(String tagE1)
        {
            this.tagE1 = tagE1;
        }

        public String getTagE1()
        {
            return tagE1;
        }

        public void setTokenES(String tokenES)
        {
            this.tokenES = tokenES;
        }

        public String getTokenES()
        {
            return tokenES;
        }

        public void setTokenEY(String tokenEY)
        {
            this.tokenEY = tokenEY;
        }

        public String getTokenEY()
        {
            return tokenEY;
        }

        public void setTokenEZ(String tokenEZ)
        {
            this.tokenEZ = tokenEZ;
        }

        public String getTokenEZ()
        {
            return tokenEZ;
        }

        public void setTokenR1(String tokenR1)
        {
            this.tokenR1 = tokenR1;
        }

        public String getTokenR1()
        {
            return tokenR1;
        }

        public void setReverso(int reverso)
        {
            this.reverso = reverso;
        }

        public int getReverso()
        {
            return reverso;
        }

        public void setPlataforma(String plataforma)
        {
            this.plataforma = plataforma;
        }
        public String getPlataforma()
        {
            return plataforma;
        }

    }
}
