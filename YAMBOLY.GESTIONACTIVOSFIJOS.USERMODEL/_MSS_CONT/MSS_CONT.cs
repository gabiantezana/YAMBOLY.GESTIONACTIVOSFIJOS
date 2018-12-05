using System;
using static SAPADDON.USERMODEL._FormattedSearches.Queries;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    /// <summary>
    /// Contratos
    /// </summary>
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_Document, TableDescription = "AF- Contratos")]
    public class MSS_CONT
    {
        [SAPField(IsSystemField = true)]
        public string DocEntry { get; set; }

        [SAPField(IsSystemField = true)]
        public string DocNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string Series { get; set; }

        [SAPField(FieldDescription ="Código")]
        public string U_MSS_CODE { get; set; }
        
        #region Datos generales

        [SAPField(FieldDescription = "Tipo contrato",
            ValidValues = new[] { TIPO.COMODATO.KEY, TIPO.DISTRIBUCION.KEY },
            ValidDescription = new[] { TIPO.COMODATO.VALUE, TIPO.DISTRIBUCION.VALUE })]
        public string U_MSS_TICO { get; set; }

        [SAPField(FieldDescription = "Estado",
            ValidValues = new[] { ESTADO.PENDIENTE.KEY, ESTADO.LEGALIZADO.KEY, ESTADO.IMPRESO.KEY, ESTADO.RECHAZADO.KEY },
            ValidDescription = new[] { ESTADO.PENDIENTE.VALUE, ESTADO.LEGALIZADO.VALUE, ESTADO.IMPRESO.VALUE, ESTADO.RECHAZADO.VALUE })]
        public string U_MSS_ESTA { get; set; }

        [SAPField(FieldDescription = "Almacén despacho", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_LIST_ALMACENES_PERMITIDOS))]
        public string U_MSS_ADES { get; set; }

        [SAPField(FieldDescription = "Moneda", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_LIST_MONEDAS))]
        public string U_MSS_MONE { get; set; }

        [SAPField(FieldDescription = "Monto anulación contrato", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string U_MSS_MANU { get; set; }

        [SAPField(FieldDescription = "Monto indemnización confidencialidad", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string U_MSS_MIND { get; set; }

        [SAPField(FieldDescription = "Monto garantía por activos", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string U_MSS_MGAR { get; set; }

        [SAPField(FieldDescription = "Fecha inicio contrato", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public string U_MSS_FINI { get; set; }

        [SAPField(FieldDescription = "Rendimiento mínimo", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string U_MSS_REND { get; set; }

        #endregion

        #region Datos de cliente

        [SAPField(FieldDescription = "Código cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_LIST))]
        public string U_MSS_CCOD { get; set; }

        [SAPField(FieldDescription = "Nombre cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CARDNAME), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_CCOD))]
        public string U_MSS_CNOM { get; set; }

        [SAPField(FieldDescription = "RUC Cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_LICTRADNUM), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_CCOD))]
        public string U_MSS_CRUC { get; set; }

        [SAPField(FieldDescription = "Código dirección fiscal cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_ADDRESS_LIST))]
        public string U_MSS_CDFI { get; set; }

        [SAPField(FieldDescription = "Desc. dirección fiscal cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_ADDRESS_DESCRIPTION_U_MSS_CDFD), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_CDFI))]
        public string U_MSS_CDFD { get; set; }

        [SAPField(FieldDescription = "Código dirección entrega cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_ADDRESS_LIST))]
        public string U_MSS_CDEN { get; set; }

        [SAPField(FieldDescription = "Desc. dirección entrega cliente", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_ADDRESS_DESCRIPTION_U_MSS_CDED), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_CDEN))]
        public string U_MSS_CDED { get; set; }

        [SAPField(FieldDescription = "Partida electrónica cliente")]
        public string U_MSS_CPAR { get; set; }

        [SAPField(FieldDescription = "Dep. reg. personas jurídicas cliente")]
        public string U_MSS_CDEP { get; set; }

        #endregion

        #region Datos de representante

        [SAPField(FieldDescription = "Código representante", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_LIST))]
        public string U_MSS_RCOD { get; set; }

        [SAPField(FieldDescription = "Nombre representante", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_NAME_U_MSS_RNOM), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_RCOD))]
        public string U_MSS_RNOM { get; set; }

        [SAPField(FieldDescription = "DNI representante", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_DNI_U_MSS_RDNI), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_RCOD))]
        public string U_MSS_RDNI { get; set; }

        [SAPField(FieldDescription = "Dirección representante", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_ADDRESS_U_MSS_RDIR), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_RCOD))]
        public string U_MSS_RDIR { get; set; }

        #endregion

        #region Datos de depositario

        [SAPField(FieldDescription = "Código depositario", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_LIST))]
        public string U_MSS_DCOD { get; set; }

        [SAPField(FieldDescription = "Nombre depositario", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_NAME_U_MSS_DNOM), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_DCOD))]
        public string U_MSS_DNOM { get; set; }

        [SAPField(FieldDescription = "DNI depositario", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_DNI_U_MSS_DDNI), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_DCOD))]
        public string U_MSS_DDNI { get; set; }

        [SAPField(FieldDescription = "Dirección depositario", FormattedSearchType = typeof(ACCTIVOSFIJOS4.MSS_FS_GET_BP_CONTACTPERSON_ADDRESS_U_MSS_DDIR), ForceRefresh = true, ParentFieldOnChange = nameof(U_MSS_DCOD))]
        public string U_MSS_DDIR { get; set; }

        #endregion

        #region  Datos pagaré

        [SAPField(FieldDescription = "Fecha emisión", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public string U_MSS_PFEM { get; set; }

        [SAPField(FieldDescription = "Monto", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string U_MSS_PMON { get; set; }

        [SAPField(FieldDescription = "Nombre cliente")]
        public string U_MSS_PNOM { get; set; }

        [SAPField(FieldDescription = "Tasa interés moratorio")]
        public string U_MSS_PTAS { get; set; }

        [SAPField(FieldDescription = "Domicilio")]
        public string U_MSS_PDOM { get; set; }

        #endregion

        #region Valores válidos

        public class ESTADO
        {
            public class PENDIENTE { public const string KEY = "PENDIENTE"; public const string VALUE = "Pendiente"; }
            public class IMPRESO { public const string KEY = "IMPRESO"; public const string VALUE = "Impreso"; }
            public class LEGALIZADO { public const string KEY = "LEGALIZADO"; public const string VALUE = "Legalizado"; }
            public class RECHAZADO { public const string KEY = "RECHAZADo"; public const string VALUE = "Rechazado"; }
        }

        public class TIPO
        {
            public class COMODATO { public const string KEY = "COMODATO"; public const string VALUE = "Comodato"; }
            public class DISTRIBUCION { public const string KEY = "DISTRIBUCION"; public const string VALUE = "Distribución"; }
        }

        #endregion 
    }
}
