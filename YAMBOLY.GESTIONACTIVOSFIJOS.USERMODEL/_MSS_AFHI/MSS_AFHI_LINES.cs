namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHI
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines)]
    public class MSS_AFHI_LINES
    {
        [SAPField(IsSystemField = true)]
        public int LineId { get; set; }

        //Opciones: Propia(01) o Cliente (02)
        [SAPField(FieldDescription = "Tipo de ubicacion", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_TIUB { get; set; }

        [SAPField(FieldDescription = "Codigo de cliente", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_COCL { get; set; }

        [SAPField(FieldDescription = "Nombre de cliente", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_NOCL { get; set; }

        [SAPField(FieldDescription = "Codigo de almacen")]
        public string MSS_COAL { get; set; }

        [SAPField(FieldDescription = "Codigo de direccion")]
        public string MSS_CODI { get; set; }

        [SAPField(FieldDescription = "Detalle de ubicacion")]
        public string MSS_DEUB { get; set; }

        [SAPField(FieldDescription = "Fecha")]
        public string MSS_FECH { get; set; }

        [SAPField(FieldDescription = "Tipo de documento")]
        public string MSS_TIDO { get; set; }

        [SAPField(FieldDescription = "Serie de documento")]
        public string MSS_SEDO { get; set; }

        [SAPField(FieldDescription = "Correlativo de documento")]
        public string MSS_CODO { get; set; }

        [SAPField(FieldDescription = "Serie de contrato")]
        public string MSS_SECO { get; set; }

        [SAPField(FieldDescription = "Correlativo de contrato")]
        public string MSS_COCO { get; set; }


    }
}
