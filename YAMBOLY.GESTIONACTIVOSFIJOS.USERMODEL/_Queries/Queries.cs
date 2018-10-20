using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE;

namespace SAPADDON.USERMODEL._FormattedSearches
{
    public class Queries
    {
        [QueryCategory]
        public class ACCTIVOSFIJOS3//todo: cambiar el nombre
        {
            [Query]
            public static class MSS_FS_LIST_SERIES_ENTREGA { }

            [Query]
            public static class MSS_FS_LIST_SERIES_DEVOLUCION { }

            [Query]
            public static class MSS_FS_LIST_ALMACENES { }

            [Query]
            public static class MSS_FS_LIST_ALMACENES_PERMITIDOS { }

            [Query]
            public static class MSS_FS_LIST_USUARIOS { }

            [Query]
            public static class MSS_FS_LIST_MONEDAS { }

            #region Clientes

            [Query]
            public static class MSS_FS_GET_BP_LIST { }

            [Query]
            public static class MSS_FS_GET_BP_CARDNAME { }

            [Query]
            public static class MSS_FS_GET_BP_LICTRADNUM { }

            #region Direcciones

            [Query]
            public static class MSS_FS_GET_BP_ADDRESS_LIST { }

            [Query]
            public static class MSS_FS_GET_BP_ADDRESS_DESCRIPTION_U_MSS_CDFD { }

            [Query]
            public static class MSS_FS_GET_BP_ADDRESS_DESCRIPTION_U_MSS_CDED { }

            #endregion

            #region Personas de contacto

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_LIST { }

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_NAME_U_MSS_RNOM { }

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_NAME_U_MSS_DNOM { }

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_DNI_U_MSS_RDNI { }

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_DNI_U_MSS_DDNI { }

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_ADDRESS_U_MSS_RDIR { }

            [Query]
            public static class MSS_FS_GET_BP_CONTACTPERSON_ADDRESS_U_MSS_DDIR { }


            #endregion

            #endregion

            #region Document Lines

            [Query]
            public static class MSS_FS_GET_OITM_LIST { }

            [Query]
            public static class MSS_FS_GET_OITM_ITEMCODE { }

            [Query]
            public static class MSS_FS_GET_OITM_ITEMNAME { }

            [Query]
            public static class MSS_FS_GET_OITM_VALORADQUISICION { }

            #endregion

        }

        #region Internal Queries

        public static class MSS_QS_GET_PERMISO_ALMACEN_ESTADO { }

        public static class MSS_QS_GET_CONTRATO { }

        public static class MSS_QS_GET_CONTRATO_LINES { }

        public static class MSS_QS_UPDATE_OITM_STATE { }
        #endregion
    }
}
