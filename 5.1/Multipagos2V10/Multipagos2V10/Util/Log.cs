using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Config;

namespace Multipagos2V10.Util
{
    class Log
    {
        private ILog log = null;

        public Log()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("C://flap/config/log4net.xml"));
            log = log4net.LogManager.GetLogger("log4Net");
        }

        public ILog getLog()
        {
            return log;
        }
    }
}
