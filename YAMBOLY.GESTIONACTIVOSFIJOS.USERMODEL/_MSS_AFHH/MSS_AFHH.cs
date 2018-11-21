using System;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHH
{
    /// <summary>
    /// Historial de ubicaciones de activos fijos
    /// </summary>
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_Document, TableDescription = "AF- HistorialUbicaciones")]
    public class MSS_AFHH
    {
        [SAPField(IsSystemField = true)]
        public int DocEntry { get; set; }

        [SAPField(IsSystemField = true)]
        public int DocNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string UserSign { get; set; }

        [SAPField(FieldDescription = "Codigo AF")]
        public string U_MSS_ITCO { get; set; }

        [SAPField(FieldDescription = "Descripcion AF")]
        public string U_MSS_ITDE { get; set; }
    }
}
