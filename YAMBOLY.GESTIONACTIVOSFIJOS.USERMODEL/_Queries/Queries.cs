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
        public class ACCTIVOSFIJOS4//todo: cambiar el nombre
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
            public static class MSS_FS_GET_MATRIX1_OITM_LIST { }

            [Query]
            public static class MSS_FS_GET_MATRIX1_ITEMCODE { }

            [Query]
            public static class MSS_FS_GET_MATRIX1_ITEMNAME { }

            [Query]
            public static class MSS_FS_GET_MATRIX1_VALORADQUISICION { }

            #endregion

            #region Adendas

            [Query]
            public static class MSS_FS_GET_MATRIX2_OITM_LIST { }

            [Query]
            public static class MSS_FS_GET_MATRIX2_ITEMCODE { }

            [Query]
            public static class MSS_FS_GET_MATRIX2_ITEMNAME { }

            [Query]
            public static class MSS_FS_GET_MATRIX2_VALORADQUISICION { }

            #endregion

        }

        #region Internal Queries

        public static class MSS_QS_GET_PERMISO_ALMACEN_ESTADO { }

        /// <summary>
        /// PARAM1: DocEntry
        /// </summary>
        public static class MSS_QS_GET_CONTRATO { }

        /// <summary>
        /// PARAM1: DocEntry
        /// </summary>
        public static class MSS_QS_GET_CONTRATO_LINES { }

        public static class MSS_QS_UPDATE_OITM_STATE { }

        /// <summary>
        /// Actualiza estado de contrato
        /// Params{ PARAM1: DocEntry, PARAM2: NewStateId }
        /// </summary>
        public static class MSS_QS_UPDATE_CONT_STATE { }

        /// <summary>
        /// Obtener configuración de series por almacén
        /// PARAM1: Almacén
        /// </summary>
        public static class MSS_QS_GET_MSS_CFSE { }

        /// <summary>
        /// PARAM1: MSS_CONT.Series, PARAM2: MSS_CONT.DocNum
        /// </summary>
        public static class MSS_QS_GET_RELATED_DELIVERY { }

        /// <summary>
        /// Devuelve la lista de ítems activos que pueden devolverse en un contrato
        /// </summary>
        public static class MSS_QS_GET_LISTARETORNO { }

        /// <summary>
        /// Devuelve un ítem del UDO Historial de Activos Fijos. PARAM1: ItemCode(U_MSS_ITCO)
        /// </summary>
        public static class MSS_QS_GET_MSS_AFHH_UDO_BY_ITEMCODE { }

        #endregion
    }
}
