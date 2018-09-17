using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION
{
    public class SapException : Exception //
    {
        public Int32 SapErrorCode { get; set; }
        public String SapErrorMessage { get; set; }
        public String Message { get; set; }
        public SapExceptionEntity sapExceptionEntity { get; set; }

        public SapException(string message = null) : base(message)
        {
            this.Message = message;
            sapExceptionEntity = new SapExceptionEntity();
        }

        public SapException(string message, Exception inner) : base(message, inner) { }

        public SapException(SapExceptionEntity sapException, String message = null)
               : base(sapException.errorMessage)
        {
            SapErrorCode = sapException.errorCode;
            SapErrorMessage = sapException.errorMessage;
            Message = sapException.message;
        }
    }
    //Para evitar referencia circular
    public class SapExceptionEntity
    {
        public Int32 errorCode { get; set; }
        public String errorMessage { get; set; }
        public String message { get; set; }
    }
}
