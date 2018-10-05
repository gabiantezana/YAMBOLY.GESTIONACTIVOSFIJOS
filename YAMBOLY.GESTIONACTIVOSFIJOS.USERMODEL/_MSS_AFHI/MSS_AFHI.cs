using System;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHI
{
    /// <summary>
    /// Historial de ubicaciones de activos fijos
    /// </summary>
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_Document)]
    public class MSS_AFHI
    {
        [SAPField(IsSystemField = true)]
        public int DocEntry { get; set; }

        [SAPField(IsSystemField = true)]
        public int DocNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string UserSign { get; set; }

        [SAPField(FieldDescription = "Codigo AF")]
        public string MSS_ITCO { get; set; }

        [SAPField(FieldDescription = "Descripcion AF")]
        public string MSS_ITDE { get; set; }
    }
}
