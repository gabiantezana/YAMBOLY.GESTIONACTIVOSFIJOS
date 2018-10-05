using SAPADDON.USERMODEL._FormattedSearches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE
{
    /// <summary>
    /// Configuración de series para gestión de activos fijos 
    /// </summary>
    [DBStructure]
    [SAPTable(TableDescription = "AF- Conf. Series", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public static class MSS_CFSE
    {
        #region Campos heredados

        [SAPField(IsSystemField = true)]
        public static string Code { get; set; }

        [SAPField(IsSystemField = true)]
        public static string Name { get; set; }

        #endregion

        #region Campos de usuario

        [SAPField(FieldDescription = "Serie SAP Entrega", IsRequired = SAPbobsCOM.BoYesNoEnum.tYES)]
        public static string MSS_SEEN { get; set; }

        [SAPField(FieldDescription = "Serie SAP Devolucion")]
        public static string MSS_SEDE { get; set; }

        [SAPField(FieldDescription = "Numero maximo de lineas", FieldType = SAPbobsCOM.BoFieldTypes.db_Numeric, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Quantity, FieldSize = 4)]
        public static string MSS_NULI { get; set; }

        [SAPField(FieldDescription = "Codigo de almacen", VinculatedTable = "OWHS")]
        public static string MSS_COAL { get; set; }

        #endregion

    }
}
