using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OITM
{
    [SAPTable(IsSystemTable = true)]
    public static class OITM
    {
        [SAPField(FieldSize = 2,
                  ValidValues = new string[] {
                                               ValidValues.MSS_EAAF.Disponible.ID,
                                               ValidValues.MSS_EAAF.Reservado.ID,
                                               ValidValues.MSS_EAAF.Asignado.ID,
                                               ValidValues.MSS_EAAF.EnMantenimiento.ID,
                                               ValidValues.MSS_EAAF.DeBaja.ID
                                              },          
                  ValidDescription = new string[] {
                                               ValidValues.MSS_EAAF.Disponible.DESCRIPTION,
                                               ValidValues.MSS_EAAF.Reservado.DESCRIPTION,
                                               ValidValues.MSS_EAAF.Asignado.DESCRIPTION,
                                               ValidValues.MSS_EAAF.EnMantenimiento.DESCRIPTION,
                                               ValidValues.MSS_EAAF.DeBaja.DESCRIPTION
                                              }
                  )]
        public static string MSS_EAAF { get; set; }
    }

    public static class ValidValues
    {
        public class MSS_EAAF
        {
            public static class Disponible
            {
                public const string ID = "01";
                public const string DESCRIPTION = "Disponible";
            }

            public static class Reservado
            {
                public const string ID = "02";
                public const string DESCRIPTION = "Reservado";
            }

            public static class Asignado
            {
                public const string ID = "03";
                public const string DESCRIPTION = "Asignado";
            }

            public static class EnMantenimiento
            {
                public const string ID = "04";
                public const string DESCRIPTION = "En mantenimiento";
            }

            public static class DeBaja
            {
                public const string ID = "05";
                public const string DESCRIPTION = "De baja";
            }
        }
    }
}
