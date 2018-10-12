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
        public class MSS_GESTION_ACTIVOS_FIJOS
        {
            [Query]
            public static class FS_MSS_LISTAR_SERIES_ENTREGA { }
            [Query]
            public static class FS_MSS_LISTAR_SERIES_DEVOLUCION { }
            [Query]
            public static class FS_MSS_LISTAR_ALMACENES { }
            [Query]
            public static class FS_MSS_LISTAR_ALMACENES_PERMITIDOS { }
            [Query]
            public static class FS_MSS_LISTAR_USUARIOS { }
            [Query]
            public static class FS_MSS_LISTAR_CLIENTES { }
            [Query]
            public static class FS_MSS_LISTAR_MONEDAS { }
            [Query]
            public static class FS_MSS_LISTAR_DIRECCIONES_DESTINO { }
            [Query]
            public static class FS_MSS_LISTAR_PERSONAS_CONTACTO { }
        }
    }
}
