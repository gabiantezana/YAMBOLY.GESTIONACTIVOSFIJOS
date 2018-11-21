using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._ODLN
{
    /// <summary>
    /// Tabla entrega por venta SAP
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class ODLN
    {   
        [SAPField(FieldDescription = "Serie contrato")]
        public static string U_MSS_SERC { get; set; }

        [SAPField(FieldDescription = "Numero contrato")]
        public static string U_MSS_NUMC { get; set; }

        [SAPField(IsSystemField = true)]
        public static string U_MSSL_TOP { get; set; }
    }
}
