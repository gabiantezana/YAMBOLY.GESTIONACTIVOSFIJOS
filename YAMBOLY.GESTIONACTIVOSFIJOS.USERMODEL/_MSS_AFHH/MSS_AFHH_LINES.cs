namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHH
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines, TableDescription = "AF- HistorialUbicaciones.Lines")]
    public class MSS_AFHH_LINES
    {
        [SAPField(IsSystemField = true)]
        public int LineId { get; set; }

        [SAPField(FieldDescription = "Tipo de ubicacion",
              ValidValues = new[] { TIPOUBICACION.PROPIA.ID, TIPOUBICACION.CLIENTE.ID },
              ValidDescription = new[] { TIPOUBICACION.PROPIA.DESCRIPCION, TIPOUBICACION.CLIENTE.DESCRIPCION })]
        public string U_MSS_TIUB { get; set; }

        [SAPField(FieldDescription = "Codigo de cliente")]
        public string U_MSS_COCL { get; set; }

        [SAPField(FieldDescription = "Nombre de cliente")]
        public string U_MSS_NOCL { get; set; }

        [SAPField(FieldDescription = "Codigo de almacen")]
        public string U_MSS_COAL { get; set; }

        [SAPField(FieldDescription = "Codigo de direccion")]
        public string U_MSS_CODI { get; set; }

        [SAPField(FieldDescription = "Detalle de ubicacion")]
        public string U_MSS_DEUB { get; set; }

        [SAPField(FieldDescription = "Fecha", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public string U_MSS_FECH { get; set; }

        [SAPField(FieldDescription = "Tipo de documento")]
        public string U_MSS_TIDO { get; set; }

        [SAPField(FieldDescription = "Serie de documento")]
        public string U_MSS_SEDO { get; set; }

        [SAPField(FieldDescription = "Correlativo de documento")]
        public string U_MSS_CODO { get; set; }

        [SAPField(FieldDescription = "Serie de contrato")]
        public string U_MSS_SECO { get; set; }

        [SAPField(FieldDescription = "Correlativo de contrato")]
        public string U_MSS_COCO { get; set; }

        #region Valores válidos
        public static class TIPOUBICACION
        {
            public static class PROPIA { public const string ID = "PROPIA"; public const string DESCRIPCION = "Propia"; }
            public static class CLIENTE { public const string ID = "CLIENTE"; public const string DESCRIPCION = "Cliente"; }
        }

        #endregion

    }
}
