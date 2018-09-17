using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONF
{
    [DBStructure]
    [SAPTable(TableDescription = "Conf. Addon Despachos", TableType =SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public class MSS_CONF
    {
        [SAPField(IsSystemField = true)]
        public string Code { get; set; }

        [SAPField(IsSystemField = true)]
        public string Name { get; set; }

        [SAPField(FieldDescription = "SN por defecto Yamboly")]
        public string MSS_SOCI { get; set; }

        [Obsolete]
        [SAPField(FieldDescription = "SN por defecto Yamboly")]
        public string MSS_XXXX { get; set; }
    }
}
