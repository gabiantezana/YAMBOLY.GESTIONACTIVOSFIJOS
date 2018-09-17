using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPADDON.USERMODEL.OITM
{
    [SAPTable(IsSystemTable = true)]c
    public class OITM
    {
        [SAPField(FieldDescription = "Pendiente de aprobación",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Alpha,
                  FieldSize = 2,
                  ValidValues = new String[] { "Y", "N" },
                  ValidDescription = new String[] { "SI", "NO" },
                  DefaultValue = "N"
        )]
        public static String MSS_PNDT { get; set; }

        [SAPField(FieldDescription = "Estado",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Alpha,
                  FieldSize = 2,
                  ValidValues = new String[] { "Y", "N" },
                  ValidDescription = new String[] { "APROBADO", "NO APROBADO" },
                  DefaultValue = "N"
                    )]
        public static String MSS_ESTD { get; set; }

        [SAPField(FieldDescription = "Usuario de aprobación", FieldSize = 30)]
        public static String MSS_USRR { get; set; }

        [SAPField(FieldDescription = "Fecha de aprobación",
                  FieldType = SAPbobsCOM.BoFieldTypes.db_Date
        )]
        public static DateTime MSS_FECH { get; set; }

        [SAPField(IsSystemField = true)]
        public static String validFor { get; set; }
    }
}
