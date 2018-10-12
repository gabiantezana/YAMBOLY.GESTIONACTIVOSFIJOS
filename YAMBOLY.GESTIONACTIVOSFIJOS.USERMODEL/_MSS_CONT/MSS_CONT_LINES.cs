namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines)]
    public class MSS_CONT_LINES
    {
        [SAPField(FieldDescription = "Código inventario AF")]
        public string U_MSS_AFCI { get; set; }

        [SAPField(FieldDescription = "Código  AF")]
        public string U_MSS_AFCO { get; set; }

        [SAPField(FieldDescription = "Descripción AF")]
        public string U_MSS_AFDE { get; set; }

        [SAPField(FieldDescription = "Valor adquisición")]
        public string U_MSS_AFVA { get; set; }

        [SAPField(FieldDescription = "Anexo")]
        public string U_MSS_ANEX { get; set; }
    }
}
