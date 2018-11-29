using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;

namespace SAPADDON.USERMODEL._DLN1
{
    [SAPTable(IsSystemTable = true)]
    public class DLN1
    {
        [SAPField(IsSystemField = true)]
        public string U_MSSL_CGP { get; set; }

        [SAPField(IsSystemField = true)]
        public string U_MSSL_CGD { get; set; }
    }
}
