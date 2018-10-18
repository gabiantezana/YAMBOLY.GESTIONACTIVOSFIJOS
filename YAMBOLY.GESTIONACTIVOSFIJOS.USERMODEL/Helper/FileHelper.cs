using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAPADDON.USERMODEL.Helper
{
    public class FileHelper
    {
        public static string GetResourceString(string resourceName)
        {
            var resourceFullName = Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList().FirstOrDefault(x => x.Contains(resourceName));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found: " + resourceName);

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceFullName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
