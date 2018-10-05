using YAMBOLY.GESTIONACTIVOSFIJOS.FORM;
//using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_APROForm;
using System;
using System.Windows.Forms;
namespace YAMBOLY.GESTIONACTIVOSFIJOS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]////
        private static void Main()
        { 
            try
            {
                /*string url = "http://www.contoso.com/code/c#/somecode.cs";
                string enc = HttpUtility.UrlEncode(url)

                System.Diagnostics.Debug.WriteLine("Original: {0} ... Encoded {1}", url, enc);
                */
                BaseApplication application = new BaseApplication();
                application.InitializeApplication();

                GC.KeepAlive(application);
                Application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
        }
    }
}
