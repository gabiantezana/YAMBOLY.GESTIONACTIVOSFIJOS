using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION
{
    public class CustomException : Exception
    {
        public CustomException(Exception ex)
        {
            ExceptionHelper.LogException(ex);
        }

        public CustomException(string message) : base(message) { }


        public CustomException(Application application, Exception ex)
        {
            application.StatusBar.SetText(ex.ToString());
            ExceptionHelper.LogException(ex);
        }

        public CustomException(Application application, Exception ex, String messageToShow)
        {
            application.StatusBar.SetText(messageToShow);
            ExceptionHelper.LogException(ex);
        }
    }
}
