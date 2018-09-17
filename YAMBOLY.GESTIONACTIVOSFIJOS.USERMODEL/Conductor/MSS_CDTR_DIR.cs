using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPADDON.USERMODEL.Conductor
{
    //[DBStructure]
    //[SAPTable("Tabla conductor-direcciones", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)]
    public static class MSS_CDTR_DIR 
    {
        [SAPField(FieldDescription ="Dirección completa")]
        public static String MSS_DIRECCION { get; set; }

        [SAPField(FieldDescription ="País")]
        public static String MSS_PAIS { get; set; }
    }
}
