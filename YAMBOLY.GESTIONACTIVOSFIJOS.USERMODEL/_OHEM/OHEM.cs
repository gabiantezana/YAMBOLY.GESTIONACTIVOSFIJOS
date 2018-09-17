using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OHEM
{
    /// <summary>
    /// Tabla de empleados
    /// </summary>
    [SAPTable(IsSystemTable = true)]
    public class OHEM
    {
        [SAPField(IsSystemField =true)]
        public string lastName { get; set; }

        [SAPField(IsSystemField =true)]
        public string firstName { get; set; }

        [SAPField(IsSystemField =true)]
        public string empID { get; set; }

        [SAPField(IsSystemField =true)]
        public string middleName { get; set; }

        [SAPField(FieldDescription = "Licencia de conducir", FieldSize = 30)]
        public string MSS_LICE { get; set; }

        [SAPField(FieldDescription = "Es conductor", ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string MSS_COND { get; set; }

    }
}
