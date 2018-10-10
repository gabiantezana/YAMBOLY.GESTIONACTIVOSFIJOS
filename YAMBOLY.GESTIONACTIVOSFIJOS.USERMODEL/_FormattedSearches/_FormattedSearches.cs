using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE;

namespace SAPADDON.USERMODEL._FormattedSearches
{
    public class FormattedSearches
    {
        public const string MSS_GESTIONACTIVOSFIJOS = "MSS_GESTIONACTIVOSFIJOS";
        
        [QueryList]
        public class FormattedSearch
        {
            [Query(CategoryName = MSS_GESTIONACTIVOSFIJOS, Query = @"SELECT ""AcctCode"", ""AcctCode"" || ' - ' || ""AcctName"" FROM ""OACT""")]
            public static class FS_MSS_LISTAR_SERIES_ENTREGA { }

            [Query(CategoryName = MSS_GESTIONACTIVOSFIJOS, Query = @"SELECT ""AcctCode"", ""AcctCode"" || ' - ' || ""AcctName"" FROM ""OACT""")]
            public static class FS_MSS_LISTAR_SERIES_DEVOLUCION { }

            [Query(CategoryName = MSS_GESTIONACTIVOSFIJOS, Query = @"SELECT ""WhsCode"", ""WhsCode""|| ' - ' || ""WhsName"" FROM ""OWHS""")]
            public static class FS_MSS_LISTAR_ALMACENES { }

            [Query(CategoryName = MSS_GESTIONACTIVOSFIJOS, Query = @"SELECT ""WhsCode"", ""WhsCode""|| ' - ' || ""WhsName"" FROM ""OWHS""")]
            public static class FS_MSS_LISTAR_USUARIOS { }
        }
    }
}
