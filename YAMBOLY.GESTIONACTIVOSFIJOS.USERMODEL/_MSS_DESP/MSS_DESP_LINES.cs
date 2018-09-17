namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_DESP
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines)]
    public class MSS_DESP_LINES
    {
        [SAPField(IsSystemField = true)]
        public int LineId { get; set; }

        [SAPField(FieldDescription = "DocEntry Orden de venta", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_DOCE { get; set; }

        [SAPField(FieldDescription = "Row Id Orden de venta", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_LINE { get; set; }

        [SAPField(FieldDescription = "Codigo Orden de venta")]
        public string MSS_ORDE { get; set; }

        [SAPField(FieldDescription = "Código de Cliente")]
        public string MSS_CODC { get; set; }

        [SAPField(FieldDescription = "Nombre de Cliente")]
        public string MSS_NOMB { get; set; }

        [SAPField(FieldDescription = "ID Dirección Entrega")]
        public string MSS_IDDI { get; set; }

        [SAPField(FieldDescription = "Detalle de Dirección")]
        public string MSS_DIRE { get; set; }

        [SAPField(FieldDescription = "Código de Artículo", VinculatedTable = "OITM")]
        public string MSS_CODA { get; set; }

        [SAPField(FieldDescription = "Descripción")]
        public string MSS_DESC { get; set; }

        [SAPField(FieldDescription = "Unidad de Medida")]
        public string MSS_UNID { get; set; }

        [SAPField(FieldDescription = "Cantidad Pendiente", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_CANP { get; set; }

        [SAPField(FieldDescription = "Cantidad a despachar", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_CAND { get; set; }

        [SAPField(FieldDescription = "Cantidad disponible", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_CANA { get; set; }

        [SAPField(FieldDescription = "Peso total (Kg)", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_PEST { get; set; }

        [SAPField(FieldDescription = "Volumen total (m3)", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_VOLT { get; set; }
    }
}
