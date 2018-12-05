using static SAPADDON.USERMODEL._FormattedSearches.Queries;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines, TableDescription = "AF- Contratos.Lines", MatrixIdInForm = "MATRIX1")]
    public class MSS_CONT_LINES
    {
        [SAPField(FieldDescription = "Código inventario AF", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX1_OITM_LIST))]
        public string U_MSS_AFCI { get; set; }

        [SAPField(FieldDescription = "Código  AF", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX1_ITEMCODE), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFCO { get; set; }

        [SAPField(FieldDescription = "Descripción AF", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX1_ITEMNAME), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFDE { get; set; }

        [SAPField(FieldDescription = "Valor adquisición", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_MATRIX1_VALORADQUISICION), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_AFCI))]
        public string U_MSS_AFVA { get; set; }
                
        [SAPField(FieldDescription = "Anexo",
              ValidValues = new[] { ANEXO.ANEXO4A.KEY, ANEXO.ANEXO4B.KEY },
              ValidDescription = new[] { ANEXO.ANEXO4B.VALUE, ANEXO.ANEXO4B.VALUE })]
        public string U_MSS_ANEX { get; set; }

        [SAPField(FieldDescription = "Estado",
              ValidValues = new[] { ESTADO.ENCONCESION.KEY, ESTADO.RETORNADO.KEY },
              ValidDescription = new[] { ESTADO.ENCONCESION.VALUE, ESTADO.RETORNADO.VALUE })]
        public string U_MSS_ESTD { get; set; }

        [SAPField(FieldDescription = "Código Adenda")]
        public string U_MSS_ADEN { get; set; }

        [SAPField(FieldDescription = "Tipo registro",
            ValidValues = new[] { TIPOREGISTRO.CONTRATOINICIAL.KEY, TIPOREGISTRO.ADENDA.KEY },
              ValidDescription = new[] { TIPOREGISTRO.CONTRATOINICIAL.VALUE, TIPOREGISTRO.ADENDA.VALUE })]
        public string U_MSS_TIPO { get; set; }

        #region Valores válidos

        public static class TIPOREGISTRO
        {
            public static class CONTRATOINICIAL { public const string KEY = "CONTRATOINICIAL"; public const string VALUE = "Contrato inicial"; }
            public static class ADENDA { public const string KEY = "ADENDA"; public const string VALUE = "Adenda"; }
        }

        public static class ANEXO
        {
            public static class ANEXO4A { public const string KEY = "ANEXO4A"; public const string VALUE = "Anexo 4A"; }
            public static class ANEXO4B { public const string KEY = "ANEXO4B"; public const string VALUE = "Anexo 4B"; }
        }

        public static class ESTADO
        {
            public static class RESERVADO { public const string KEY = "RESERVADO"; public const string VALUE = "Reservado"; }
            public static class ENCONCESION { public const string KEY = "ENCONCESION"; public const string VALUE = "En concesión"; }
            public static class RETORNADO { public const string KEY = "RETORNADO"; public const string VALUE = "Retornado"; }
        }

        #endregion 
    }
}
