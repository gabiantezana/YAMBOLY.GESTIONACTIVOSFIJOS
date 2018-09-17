using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPADDON.USERMODEL.Conductor
{
    //[DBStructure]
    //[SAPTable("Tabla conductor cabecera", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public static class MSS_CDTR
    {
        [SAPField(FieldDescription = "Id conductor", IsSearchField = true)]
        public static String MSS_IDCONDUCTOR { get; set; }

        [SAPField(FieldDescription = "Id externo conductor")]
        public static String MSS_IDEXTERNOCONDUCTOR { get; set; }

        [SAPField(FieldDescription = "Nombres", IsSearchField = true)]
        public static String MSS_NOMBRES { get; set; }

        [SAPField(FieldDescription = "Apellidos", IsSearchField = true)]
        public static String MSS_APELLIDOS { get; set; }

        [SAPField(FieldDescription = "Es activo")]
        public static String MSS_ESACTIVO { get; set; }

        [SAPField(FieldDescription = "Teléfono")]
        public static String MSS_TELEFONO { get; set; }

        [SAPField(FieldDescription = "Celular")]
        public static String MSS_CELULAR { get; set; }

        [SAPField(FieldDescription = "Correo electrónico ")]
        public static String MSS_CORREOE { get; set; }
    }

}
