using System;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    /// <summary>
    /// Historial de ubicaciones de activos fijos
    /// </summary>
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_Document)]
    public class MSS_CONT
    {
        #region Datos generales

        [SAPField(FieldDescription = "Tipo contrato")]
        public string MSS_TICO { get; set; }

        [SAPField(FieldDescription = "Estado")]
        public string MSS_ESTA { get; set; }

        [SAPField(FieldDescription = "Almacén despacho")]
        public string MSS_ADES { get; set; }

        [SAPField(FieldDescription = "Moneda")]
        public string MSS_MONE { get; set; }

        [SAPField(FieldDescription = "Monto anulación contrato", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string MSS_MANU { get; set; }

        [SAPField(FieldDescription = "Monto indemnización confidencialidad", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string MSS_MIND { get; set; }

        [SAPField(FieldDescription = "Monto garantía por activos", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string MSS_MGAR { get; set; }

        [SAPField(FieldDescription = "Fecha inicio contrato", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public string MSS_FINI { get; set; }

        [SAPField(FieldDescription = "Rendimiento mínimo", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string MSS_REND { get; set; }

        #endregion

        #region Datos de cliente

        [SAPField(FieldDescription = "Código cliente")]
        public string MSS_CCOD { get; set; }

        [SAPField(FieldDescription = "Nombre cliente")]
        public string MSS_CNOM { get; set; }

        [SAPField(FieldDescription = "RUC Cliente")]
        public string MSS_CRUC { get; set; }

        [SAPField(FieldDescription = "Código dirección fiscal cliente")]
        public string MSS_CDFI { get; set; }

        [SAPField(FieldDescription = "Código dirección entrega cliente")]
        public string MSS_CDEN { get; set; }

        [SAPField(FieldDescription = "Desc. dirección entrega cliente")]
        public string MSS_CDED { get; set; }

        [SAPField(FieldDescription = "Partida electrónica cliente")]
        public string MSS_CPAR { get; set; }

        [SAPField(FieldDescription = "Dep. reg. personas jurídicas cliente")]
        public string MSS_CDEP { get; set; }

        #endregion

        #region Datos de representante

        [SAPField(FieldDescription = "Código representante")]
        public string MSS_RCOD { get; set; }

        [SAPField(FieldDescription = "Nombre representante")]
        public string MSS_RNOM { get; set; }

        [SAPField(FieldDescription = "DNI representante")]
        public string MSS_RDNI { get; set; }

        [SAPField(FieldDescription = "Dirección representante")]
        public string MSS_RDIR { get; set; }

        #endregion

        #region Datos de depositario

        [SAPField(FieldDescription = "Código depositario")]
        public string MSS_DCOD { get; set; }

        [SAPField(FieldDescription = "Nombre depositario")]
        public string MSS_DNOM { get; set; }

        [SAPField(FieldDescription = "DNI depositario")]
        public string MSS_DDNI { get; set; }

        [SAPField(FieldDescription = "Dirección depositario")]
        public string MSS_DDIR { get; set; }

        #endregion

        #region  Datos pagaré

        [SAPField(FieldDescription = "Fecha emisión", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public string MSS_PFEM { get; set; }

        [SAPField(FieldDescription = "Monto", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Price)]
        public string MSS_PMON { get; set; }

        [SAPField(FieldDescription = "Nombre cliente")]
        public string MSS_PNOM { get; set; }

        [SAPField(FieldDescription = "Tasa interés moratorio")]
        public string MSS_PTAS { get; set; }

        [SAPField(FieldDescription = "Domicilio")]
        public string MSS_PDOM { get; set; }

        #endregion
    }
}
