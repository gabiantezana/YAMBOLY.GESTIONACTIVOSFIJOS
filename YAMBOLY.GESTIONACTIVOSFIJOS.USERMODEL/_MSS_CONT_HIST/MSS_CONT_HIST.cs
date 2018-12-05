using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT_HIST
{
    /// <summary>
    /// Historial de contrato
    /// </summary>
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_Document, TableDescription = "AF- Contratos Historial")]
    public class MSS_CONT_HIST : MSS_CONT
    {
        [SAPField(FieldDescription = "Tipo registro", ValidValues = new[] { VALIDVALUES.TIPOREGISTRO.DOCUMENTOBASE.KEY, VALIDVALUES.TIPOREGISTRO.ADENDA.KEY },
                                                      ValidDescription  = new[] { VALIDVALUES.TIPOREGISTRO.DOCUMENTOBASE.DESCRIPTION, VALIDVALUES.TIPOREGISTRO.ADENDA.DESCRIPTION })]
        public string U_MSS_TYPE { get; set; }

        [SAPField(FieldDescription = "Código adenda")]
        public string U_MSS_ADEN { get; set; }

        [SAPField(FieldDescription = "Fecha adenda")]
        public string U_MSS_FECH { get; set; }

        [SAPField(FieldDescription = "Comentario adenda")]
        public string U_MSS_COME { get; set; }

        public class VALIDVALUES
        {
            public static class TIPOREGISTRO
            {
                public static class DOCUMENTOBASE { public const string KEY = "DOCUMENTOBASE"; public const string DESCRIPTION = "Documento base"; }
                public static class ADENDA { public const string KEY = "ADENDA"; public const string DESCRIPTION = "Adenda"; }
            }
        }
    }
}
