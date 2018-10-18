using static SAPADDON.USERMODEL._FormattedSearches.Queries;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines)]
    public class MSS_CONT_LINES
    {
        [SAPField(FieldDescription = "Código inventario AF", FormattedSearchType = typeof(ACCTIVOSFIJOS3.MSS_FS_GET_OITM_LIST))]
        public string U_MSS_AFCI { get; set; }

        [SAPField(FieldDescription = "Código  AF", FormattedSearchType = typeof(ACCTIVOSFIJOS3.MSS_FS_GET_OITM_ITEMCODE), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFCO { get; set; }

        [SAPField(FieldDescription = "Descripción AF", FormattedSearchType = typeof(ACCTIVOSFIJOS3.MSS_FS_GET_OITM_ITEMNAME), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFDE { get; set; }

        [SAPField(FieldDescription = "Valor adquisición")]
        public string U_MSS_AFVA { get; set; }

        [SAPField(FieldDescription = "Anexo")]
        public string U_MSS_ANEX { get; set; }
    }
}
