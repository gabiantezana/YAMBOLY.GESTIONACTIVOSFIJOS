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

        [SAPField(FieldDescription = "Valor adquisición", FormattedSearchType = typeof(ACCTIVOSFIJOS3.MSS_FS_GET_OITM_VALORADQUISICION), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFVA { get; set; }

        [SAPField(FieldDescription = "Anexo",
              ValidValues = new[] { ANEXO.ANEXO4A.KEY, ANEXO.ANEXO4B.KEY },
              ValidDescription = new[] { ANEXO.ANEXO4B.VALUE, ANEXO.ANEXO4B.VALUE })]
        public string U_MSS_ANEX { get; set; }

        #region Valores válidos

        public static class ANEXO
        {
            public static class ANEXO4A { public const string KEY = "01"; public const string VALUE = "Anexo 4A"; }
            public static class ANEXO4B { public const string KEY = "02"; public const string VALUE = "Anexo 4B"; }
        }

        #endregion 
    }
}
