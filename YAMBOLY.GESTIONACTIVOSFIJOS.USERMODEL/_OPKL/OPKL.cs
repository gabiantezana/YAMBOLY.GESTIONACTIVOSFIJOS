using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OPKL
{
    [SAPTable(IsSystemTable = true)]
    public class OPKL
    {
        [SAPField(FieldDescription = "Volumen total  (m3)", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_VOLU { get; set; }

        [SAPField(FieldDescription = "Peso total (Kg)", FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string MSS_PESO { get; set; }

        [Obsolete]
        [SAPField(FieldDescription = "Rutas")]
        public string MSS_RUTA { get; set; }

        [SAPField(FieldDescription = "Despacho DocEntry", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric) ]
        public string MSS_DESP { get; set; }

        #region Datos de despacho

        [SAPField(FieldDescription = "Lic. Condcucir")]
        public string MSS_LICE { get; set; }

        [SAPField(FieldDescription = "Certificado MTC")]
        public string MSS_CMTC { get; set; }

        [SAPField(FieldDescription = "Razon Social")]
        public string MSS_RAZO { get; set; }

        [SAPField(FieldDescription = "Numero articulos total.",
               FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string MSS_ARTI { get; set; }

        [SAPField(FieldDescription = "Código")]
        public string MSS_CODI { get; set; }

        [SAPField(FieldDescription = "Marca")]
        public string MSS_MARC { get; set; }

        [SAPField(FieldDescription = "Placa")]
        public string MSS_PLAC { get; set; }

        [SAPField(FieldDescription = "Nombres Conductor")]
        public string MSS_CONN { get; set; }

        [SAPField(FieldDescription = "Núm. Direcciones de entrega", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_None)]
        public string MSS_NUME { get; set; }

        //No usarrrrrrrrrrrrrrrrrrrr
        [Obsolete]
        [SAPField(FieldDescription = "No usarrrrrrrrrr")]
        public string MSS_PLA2 { get; set; }

        #endregion

    }
}
