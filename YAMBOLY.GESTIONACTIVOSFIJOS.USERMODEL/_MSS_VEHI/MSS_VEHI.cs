using System;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_VEHIC
{
    [DBStructure]
    [SAPTable("Tabla vehículos", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public class MSS_VEHI
    {

        [SAPField(IsSystemField = true)]
        public string Code { get; set; }

        [SAPField(IsSystemField = true)]
        public string Name { get; set; }

        /// <summary>
        /// ORIGEN
        /// </summary>
        [SAPField(
        FieldDescription = "Origen",
        ValidValues = new[] { "01", "02" },
        ValidDescription = new[] { "PROPIO", "TERCERO" })]
        public string MSS_ORIG { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [SAPField(
        FieldDescription = "Estado",
        ValidValues = new[] { "DI", "RU", "MA", "BA" },
        ValidDescription = new[] { "Disponible", "Ruta", "En mantenimiento", "Baja" })]
        public string MSS_ESTA { get; set; }

        /// <summary>
        /// Activo fijo
        /// </summary>
        [SAPField(FieldDescription = "Activo fijo")]
        public string MSS_CODA { get; set; }

        /// <summary>
        /// Socio Negocio
        /// </summary>
        [SAPField(FieldDescription = "Socio Negocio")]
        public string MSS_CODS { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        [SAPField(FieldDescription = "Tipo")]
        public string MSS_TIPO { get; set; }

        /// <summary>
        /// Categoría
        /// </summary>
        [SAPField(FieldDescription = "Categoría")]
        public string MSS_CATE { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [SAPField(FieldDescription = "Placa", FieldSize = 7)]
        public string MSS_PLAC { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [SAPField(FieldDescription = "Modelo")]
        public string MSS_MODE { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [SAPField(FieldDescription = "Marca")]
        public string MSS_MARC { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [SAPField(FieldDescription = "Color")]
        public string MSS_COLO { get; set; }

        /// <summary>
        /// Combustible
        /// </summary>
        [SAPField(FieldDescription = "Combustible")]
        public string MSS_COMB { get; set; }

        /// <summary>
        /// Form. Rodante
        /// </summary>
        [SAPField(FieldDescription = "Form. Rodante")]
        public string MSS_FORM { get; set; }

        /// <summary>
        /// Vin
        /// </summary>
        [SAPField(FieldDescription = "Vin")]
        public string MSS_VIN { get; set; }

        /// <summary>
        /// Serie/Chasis
        /// </summary>
        [SAPField(FieldDescription = "Serie/Chasis")]
        public string MSS_SERI { get; set; }

        /// <summary>
        /// Año fabricación
        /// </summary>
        [SAPField(FieldDescription = "Año fabricación",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public string MSS_ANOF { get; set; }

        /// <summary>
        /// Año modelo
        /// </summary>
        [SAPField(FieldDescription = "Año modelo",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public int MSS_ANOM { get; set; }

        /// <summary>
        /// Versión
        /// </summary>
        [SAPField(FieldDescription = "Versión")]
        public string MSS_VERS { get; set; }

        /// <summary>
        /// Ejes
        /// </summary>
        [SAPField(FieldDescription = "Ejes",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public int MSS_EJES { get; set; }

        /// <summary>
        /// Asientos
        /// </summary>
        [SAPField(FieldDescription = "Asientos",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public int MSS_ASIE { get; set; }

        /// <summary>
        /// Pasajeros
        /// </summary>
        [SAPField(FieldDescription = "Pasajeros",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public int MSS_PASA { get; set; }

        /// <summary>
        /// Ruedas
        /// </summary>
        [SAPField(FieldDescription = "Ruedas",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public int MSS_RUED { get; set; }

        /// <summary>
        /// Carrocería
        /// </summary>
        [SAPField(FieldDescription = "Carrocería")]
        public string MSS_CARR { get; set; }

        /// <summary>
        /// Potencia
        /// </summary>
        [SAPField(FieldDescription = "Potencia")]
        public string MSS_POTE { get; set; }

        /// <summary>
        /// Cilindros
        /// </summary>
        [SAPField(FieldDescription = "Cilindros")]
        public int MSS_CILS { get; set; }

        /// <summary>
        /// Cilindrada
        /// </summary>
        [SAPField(FieldDescription = "Cilindrada")]
        public string MSS_CILA { get; set; }

        /// <summary>
        /// Peso bruto kg.
        /// </summary>
        [SAPField(FieldDescription = "Peso bruto kg.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]

        public double MSS_PBKG { get; set; }

        /// <summary>
        /// Peso neto kg.
        /// </summary>
        [SAPField(FieldDescription = "Peso neto kg.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_PNKG { get; set; }

        /// <summary>
        /// Carga útil kg.
        /// </summary>
        [SAPField(FieldDescription = "Carga útil kg.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_CUKG { get; set; }

        /// <summary>
        /// Longitud m.
        /// </summary>
        [SAPField(FieldDescription = "Longitud m.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_LONM { get; set; }

        /// <summary>
        /// Altura m.
        /// </summary>
        [SAPField(FieldDescription = "Altura m.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_ALTM { get; set; }

        /// <summary>
        /// Ancho m.
        /// </summary>
        [SAPField(FieldDescription = "Ancho m.",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double MSS_ANCM { get; set; }

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
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType =SAPbobsCOM.BoFldSubTypes.st_Measurement)]
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

        [SAPField(FieldDescription = "Certificado MTC", FieldSize = 9)]
        public string MSS_CMTC { get; set; }

        [SAPField(FieldDescription = "Licencia Conducir")]
        public string MSS_LICE { get; set; }


        /// <summary>
        /// NO USARRRRRRR
        /// </summary>
        [Obsolete]
        [SAPField(FieldDescription = "Número máximo de repartos",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric)]
        public double MSS_NRPR4 { get; set; }
    }

}
