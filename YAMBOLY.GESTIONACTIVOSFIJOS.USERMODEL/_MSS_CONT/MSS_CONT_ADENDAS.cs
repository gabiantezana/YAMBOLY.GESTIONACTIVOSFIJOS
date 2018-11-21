using static SAPADDON.USERMODEL._FormattedSearches.Queries;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines, TableDescription ="AF- Contratos.Adendas" , MatrixIdInForm ="MATRIX2")]
    public class MSS_CONT_ADENDAS
    {
        [SAPField(FieldType = SAPbobsCOM.BoFieldTypes.db_Date, FieldDescription = "Fecha adenda")]
        public string U_MSS_DATE { get; set; }

        [SAPField(FieldDescription = "Código adenda")]
        public string U_MSS_CODE { get; set; }

        [SAPField(FieldDescription = "Código inventario AF", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX2_OITM_LIST))]
        public string U_MSS_AFCI { get; set; }

        [SAPField(FieldDescription = "Código  AF", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX2_ITEMCODE), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFCO { get; set; }

        [SAPField(FieldDescription = "Descripción AF", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX2_ITEMNAME), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFDE { get; set; }

        [SAPField(FieldDescription = "Valor adquisición", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX2_VALORADQUISICION), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFVA { get; set; }

        [SAPField(FieldDescription = "Anexo",
              ValidValues = new[] { ANEXO.ANEXO4A.KEY, ANEXO.ANEXO4B.KEY },
              ValidDescription = new[] { ANEXO.ANEXO4B.VALUE, ANEXO.ANEXO4B.VALUE })]
        public string U_MSS_ANEX { get; set; }

        [SAPField(FieldDescription = "Estado",
              ValidValues = new[] { ESTADO.ENCONCESION.KEY, ESTADO.RETORNADO.VALUE },
              ValidDescription = new[] { ESTADO.RETORNADO.VALUE, ESTADO.RETORNADO.VALUE })]
        public string U_MSS_ESTD { get; set; }

        #region Valores válidos

        public static class ANEXO
        {
            public static class ANEXO4A { public const string KEY = "1"; public const string VALUE = "Anexo 4A"; }
            public static class ANEXO4B { public const string KEY = "2"; public const string VALUE = "Anexo 4B"; }
        }

        public static class ESTADO
        {
            public static class ENCONCESION { public const string KEY = "1"; public const string VALUE = "En concesión"; }
            public static class RETORNADO { public const string KEY = "2"; public const string VALUE = "Retornado"; }
        }

        #endregion 

    }
}
