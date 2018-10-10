using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SAPADDON.USERMODEL._FormattedSearches.FormattedSearches;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFPE
{
    /// <summary>
    /// Configuración de permisos de usuarios para gestión de activos fijos
    /// </summary>
    [DBStructure]
    [SAPTable(TableDescription = "AF- Conf. Permisos", TableType = SAPbobsCOM.BoUTBTableType.bott_Document)]
    public class MSS_CFPE
    {
        [SAPField(FieldDescription = "Codigo usuario", FormattedSearchType = typeof(FormattedSearch.FS_MSS_LISTAR_USUARIOS))]
        public string MSS_COUS { get; set; }

        [SAPField(FieldDescription = "Codigo almacen", FormattedSearchType = typeof(FormattedSearch.FS_MSS_LISTAR_ALMACENES))]
        public string MSS_COAL { get; set; }

        [SAPField(FieldDescription = "Permiso estado pendiente", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_PEPE { get; set; }

        [SAPField(FieldDescription = "Permiso estado impreso", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_PEIM { get; set; }

        [SAPField(FieldDescription = "Permiso estado legalizado", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_PELE { get; set; }

        [SAPField(FieldDescription = "Permiso estado rechazado", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_PERE { get; set; }

        [SAPField(FieldDescription = "Permiso estado cesión  temporal", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_PECE { get; set; }

        [SAPField(FieldDescription = "Permiso estado retorno AF", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_PERT { get; set; }
    }
}
