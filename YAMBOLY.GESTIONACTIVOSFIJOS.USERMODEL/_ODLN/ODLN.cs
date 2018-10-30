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
        [SAPField(FieldDescription = "Serie contrato", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None, FieldSize = 10)]
        public static string U_MSS_SECT { get; set; }

        [SAPField(FieldDescription = "Numero contrato", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None, FieldSize = 10)]
        public static string U_MSS_NUCT { get; set; }

        [SAPField(IsSystemField = true)]
        public static string U_MSSL_TOP { get; set; }
    }
}
