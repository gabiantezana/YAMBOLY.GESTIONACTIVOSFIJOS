using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._ORDR
{
    [SAPTable(IsSystemTable = true)]
    public class ORDR
    {
        [SAPField(IsSystemField = true)]
        public int DocEntry { get; set; }

        #region Datos generales de traslado
        /// <summary>
        /// Fecha de despacho
        /// </summary>
        [SAPField(FieldDescription = "Fecha inicio traslado", FieldType = SAPbobsCOM.BoFieldTypes.db_Date)]
        public DateTime MSS_FETR { get; set; }

        /// <summary>
        /// Dirección de almacén de despacho
        /// </summary>
        [SAPField(FieldDescription = "Punto de partida")]
        public string MSS_ORIG { get; set; }

        /// <summary>
        /// Dirección del socio de negocios en la orden de venta
        /// </summary>
        [SAPField(FieldDescription = "Punto de llegada")]
        public string MSS_DEST { get; set; }

        #endregion

        #region Datos destinatario

        /// <summary>
        /// RUC del socio de negocio de la OV
        /// </summary>
        [SAPField(FieldDescription = "RUC Destinatario")]
        public string MSS_RUCD { get; set; }

        /// <summary>
        /// Razón social de socio de negocio de la OV
        /// </summary>
        [SAPField(FieldDescription = "Rz. Destinatario")]
        public string MSS_RZOD { get; set; }

        #endregion


        #region Datos de Transportista

        /// <summary>
        /// RUC del vehículo asociado al despacho por placa
        /// </summary>
        [SAPField(FieldDescription = "RUC Transportista")]
        public string MSSL_RTR { get; set; }
        //public string MSS_RUCT { get; set; } //Reemplazado por la nueva localización

        /// <summary>
        /// Razón social de vehículo asociado al despacho por placa
        /// </summary>
        [SAPField(FieldDescription = "Rz. Social Transportista")]
        public string MSSL_NTR { get; set; }
        //public string MSS_RZOT { get; set; }

        /// <summary>
        /// Dirección del transportista
        /// </summary>
        [SAPField(FieldDescription = "Dir. Transportista")]
        public string MSSL_DTR { get; set; }


        #endregion

        #region Datos del vehiculo

        /// <summary>
        /// Marca del vehículo asociado a la placa
        /// </summary>
        [SAPField(FieldDescription = "Marca vehiculo")]
        public string MSSL_MVH { get; set; }

        /// <summary>
        /// Placa del vehículo de despacho
        /// </summary>
        [SAPField(FieldDescription = "Placa vehiculo")]
        public string MSSL_PVH { get; set; }


        /// <summary>
        /// certificado de circulación de vehículo asociado al despacho por placa.
        /// </summary>
        [SAPField(FieldDescription = "N° Certificado de circulacion")]
        public string MSS_CECI { get; set; }


        #endregion

        #region Datos del conductor

        /// <summary>
        /// Nombres del conductor asociado al despacho
        /// </summary>
        [SAPField(FieldDescription = "Conductor")]
        public string MSSL_NCD { get; set; }

        /// <summary>
        /// Licencia de conducir de conductor asociado al despacho
        /// </summary>
        [Obsolete]
        [SAPField(FieldDescription = "Lic. Conducir Conductor")]
        public string MSSL_LCD { get; set; }

        #endregion

        #region Aplicativo móvil

        [SAPField(FieldDescription = "Origen aplicativo movil", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSSM_CRM { get; set; }

        #endregion


        [SAPField(FieldDescription = "-")]
        public string MSS_ABCD { get; set; }

    }

}
