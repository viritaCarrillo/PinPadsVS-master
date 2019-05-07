using System;
using System.Collections.Generic;
using System.Text;

namespace Multipagos2V10.Exceptions
{
    class PinPadException : Exception
    {
        public PinPadException(string mensaje) : base(mensaje) { }
    }
}
