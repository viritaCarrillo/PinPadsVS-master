﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Multipagos2V10.ServiciosWebMultipagosInterredDevolucion5_1 {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Net;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ProcesaDevolucionesFullSoapBinding", Namespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx")]
    public partial class ProcesaDevolucionesFull : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback procesaDevolucionOLOperationCompleted;
        
        private System.Threading.SendOrPostCallback procesaDevolucionOLCEOperationCompleted;
        
        private System.Threading.SendOrPostCallback procesaDevolucionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ProcesaDevolucionesFull(string url) {
            this.Url = url;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //this.Url = global::Multipagos2V10.Properties.Settings.Default.Multipagos2V10_ServiciosWebMultipagosInterredDevolucion5_1_ProcesaDevolucionesFull;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event procesaDevolucionOLCompletedEventHandler procesaDevolucionOLCompleted;
        
        /// <remarks/>
        public event procesaDevolucionOLCECompletedEventHandler procesaDevolucionOLCECompleted;
        
        /// <remarks/>
        public event procesaDevolucionCompletedEventHandler procesaDevolucionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx", ResponseNamespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("procesaDevolucionOLReturn")]
        public string procesaDevolucionOL(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13) {
            object[] results = this.Invoke("procesaDevolucionOL", new object[] {
                        val_1,
                        val_2,
                        val_3,
                        val_4,
                        val_5,
                        val_6,
                        val_7,
                        val_8,
                        val_9,
                        val_10,
                        val_11,
                        val_12,
                        val_13});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void procesaDevolucionOLAsync(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13) {
            this.procesaDevolucionOLAsync(val_1, val_2, val_3, val_4, val_5, val_6, val_7, val_8, val_9, val_10, val_11, val_12, val_13, null);
        }
        
        /// <remarks/>
        public void procesaDevolucionOLAsync(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13, object userState) {
            if ((this.procesaDevolucionOLOperationCompleted == null)) {
                this.procesaDevolucionOLOperationCompleted = new System.Threading.SendOrPostCallback(this.OnprocesaDevolucionOLOperationCompleted);
            }
            this.InvokeAsync("procesaDevolucionOL", new object[] {
                        val_1,
                        val_2,
                        val_3,
                        val_4,
                        val_5,
                        val_6,
                        val_7,
                        val_8,
                        val_9,
                        val_10,
                        val_11,
                        val_12,
                        val_13}, this.procesaDevolucionOLOperationCompleted, userState);
        }
        
        private void OnprocesaDevolucionOLOperationCompleted(object arg) {
            if ((this.procesaDevolucionOLCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.procesaDevolucionOLCompleted(this, new procesaDevolucionOLCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx", ResponseNamespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("procesaDevolucionOLCEReturn")]
        public string procesaDevolucionOLCE(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13) {
            object[] results = this.Invoke("procesaDevolucionOLCE", new object[] {
                        val_1,
                        val_2,
                        val_3,
                        val_4,
                        val_5,
                        val_6,
                        val_7,
                        val_8,
                        val_9,
                        val_10,
                        val_11,
                        val_12,
                        val_13});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void procesaDevolucionOLCEAsync(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13) {
            this.procesaDevolucionOLCEAsync(val_1, val_2, val_3, val_4, val_5, val_6, val_7, val_8, val_9, val_10, val_11, val_12, val_13, null);
        }
        
        /// <remarks/>
        public void procesaDevolucionOLCEAsync(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13, object userState) {
            if ((this.procesaDevolucionOLCEOperationCompleted == null)) {
                this.procesaDevolucionOLCEOperationCompleted = new System.Threading.SendOrPostCallback(this.OnprocesaDevolucionOLCEOperationCompleted);
            }
            this.InvokeAsync("procesaDevolucionOLCE", new object[] {
                        val_1,
                        val_2,
                        val_3,
                        val_4,
                        val_5,
                        val_6,
                        val_7,
                        val_8,
                        val_9,
                        val_10,
                        val_11,
                        val_12,
                        val_13}, this.procesaDevolucionOLCEOperationCompleted, userState);
        }
        
        private void OnprocesaDevolucionOLCEOperationCompleted(object arg) {
            if ((this.procesaDevolucionOLCECompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.procesaDevolucionOLCECompleted(this, new procesaDevolucionOLCECompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx", ResponseNamespace="http://webservice.devolucion.cifrada.interred.adquira.com.mx", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("procesaDevolucionReturn")]
        public string procesaDevolucion(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13, string val_14) {
            object[] results = this.Invoke("procesaDevolucion", new object[] {
                        val_1,
                        val_2,
                        val_3,
                        val_4,
                        val_5,
                        val_6,
                        val_7,
                        val_8,
                        val_9,
                        val_10,
                        val_11,
                        val_12,
                        val_13,
                        val_14});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void procesaDevolucionAsync(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13, string val_14) {
            this.procesaDevolucionAsync(val_1, val_2, val_3, val_4, val_5, val_6, val_7, val_8, val_9, val_10, val_11, val_12, val_13, val_14, null);
        }
        
        /// <remarks/>
        public void procesaDevolucionAsync(int val_1, int val_2, int val_3, int val_4, string val_5, string val_6, string val_7, string val_8, string val_9, string val_10, string val_11, string val_12, string val_13, string val_14, object userState) {
            if ((this.procesaDevolucionOperationCompleted == null)) {
                this.procesaDevolucionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnprocesaDevolucionOperationCompleted);
            }
            this.InvokeAsync("procesaDevolucion", new object[] {
                        val_1,
                        val_2,
                        val_3,
                        val_4,
                        val_5,
                        val_6,
                        val_7,
                        val_8,
                        val_9,
                        val_10,
                        val_11,
                        val_12,
                        val_13,
                        val_14}, this.procesaDevolucionOperationCompleted, userState);
        }
        
        private void OnprocesaDevolucionOperationCompleted(object arg) {
            if ((this.procesaDevolucionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.procesaDevolucionCompleted(this, new procesaDevolucionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void procesaDevolucionOLCompletedEventHandler(object sender, procesaDevolucionOLCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class procesaDevolucionOLCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal procesaDevolucionOLCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void procesaDevolucionOLCECompletedEventHandler(object sender, procesaDevolucionOLCECompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class procesaDevolucionOLCECompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal procesaDevolucionOLCECompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void procesaDevolucionCompletedEventHandler(object sender, procesaDevolucionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class procesaDevolucionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal procesaDevolucionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591