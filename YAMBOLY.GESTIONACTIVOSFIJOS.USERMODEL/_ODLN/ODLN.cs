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
    public static class ODLN
    {
        [SAPField(FieldDescription = "Serie contrato")]
        public static string MSS_SECO { get; set; }

        [SAPField(FieldDescription = "Numero contrato")]
        public static string MSS_NUCO { get; set; }
    }
}
