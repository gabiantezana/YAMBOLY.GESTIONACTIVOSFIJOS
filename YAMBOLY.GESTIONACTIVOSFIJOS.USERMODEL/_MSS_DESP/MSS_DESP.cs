using System;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_DESP
{
    [SAPTable(TableType = SAPbobsCOM.BoUTBTableType.bott_Document)]
    public class MSS_DESP
    {
        [SAPField(IsSystemField = true)]
        public int DocEntry { get; set; }

        [SAPField(IsSystemField = true)]
        public int DocNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string UserSign { get; set; }

        [SAPField(FieldDescription = "Fecha", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public string MSS_FECH { get; set; }

        [SAPField(FieldDescription = "Conductor")]
        public string MSS_COND { get; set; }

        [SAPField(FieldDescription = "Nombres Conductor")]
        public string MSS_CONN { get; set; }

        [SAPField(FieldDescription = "Lic. Condcucir")]
        public string MSS_LICE { get; set; }

        [SAPField(FieldDescription = "Almacén")]
        public string MSS_ALMA { get; set; }

        [SAPField(FieldDescription = "Estado", ValidValues = new[] { "PENDIENTE", "APROBADO", "RECHAZADO" }, ValidDescription = new[] { "PENDIENTE", "APROBADO", "RECHAZADO" })]
        public string MSS_ESTA { get; set; }

        [SAPField(FieldDescription = "Volumen total (m3)", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_VOLU { get; set; }

        [SAPField(FieldDescription = "Peso total (Kg)", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_PESO { get; set; }

        [SAPField(FieldDescription = "Núm. Direcciones de entrega", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_NUME { get; set; }

        [SAPField(FieldDescription = "Direcciones de entrega")]
        public string MSS_DIRE { get; set; }

        [SAPField(FieldDescription = "Numero articulos total.",
               FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string MSS_ARTI { get; set; }

        [SAPField(FieldDescription = "Motivo de desaprobacion")]
        public string MSS_MOTI { get; set; }

        #region Datos de Vehículo

        [SAPField(FieldDescription = "Código")]
        public string MSS_CODI { get; set; }

        [SAPField(FieldDescription = "Marca")]
        public string MSS_MARC { get; set; }

        [SAPField(FieldDescription = "Placa")]
        public string MSS_PLAC { get; set; }

        [SAPField(FieldDescription = "Certificado MTC")]
        public string MSS_CMTC { get; set; }

        [SAPField(FieldDescription = "Razon Social")]
        public string MSS_RAZO { get; set; }

        /// <summary>
        /// Capacidad mínima de carga kg.
        /// </summary>
        [SAPField(FieldDescription = "Capacidad mínima de carga kg.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_CMIC { get; set; }

        /// <summary>
        /// Capacidad máxima de carga kg
        /// </summary>
        [SAPField(FieldDescription = "Capacidad máxima de carga kg.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_CMAC { get; set; }

        /// <summary>
        /// "Capacidad mínima de volumen kg.
        /// </summary>
        [SAPField(FieldDescription = "Capacidad mínima de volumen m3.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_CMIV { get; set; }

        /// <summary>
        /// "Capacidad máxima de volumen kg.
        /// </summary>
        [SAPField(FieldDescription = "Capacidad máxima de volumen m3.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_CMAV { get; set; }

        /// <summary>
        /// Número mínimo de repartos
        /// </summary>
        [SAPField(FieldDescription = "Mínimo número de repartos.",
                 FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string MSS_NMIR { get; set; }

        /// <summary>
        /// Número máximo de repartos
        /// </summary>
        [SAPField(FieldDescription = "Numero mínimo de repartos.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string MSS_NMAR { get; set; }

        /// <summary>
        /// Error SAP
        /// </summary>
        [SAPField(FieldDescription = "Error SAP")]
        public string MSS_ERRO { get; set; }

        /// <summary>
        /// Comentario
        /// </summary>
        [SAPField(FieldDescription = "Comentario")]
        public string MSS_COME { get; set; }

        [Obsolete]
        [SAPField(FieldDescription = "NO USAR")]
        public string MSS_NOUS { get; set; }

        #endregion

    }
}
