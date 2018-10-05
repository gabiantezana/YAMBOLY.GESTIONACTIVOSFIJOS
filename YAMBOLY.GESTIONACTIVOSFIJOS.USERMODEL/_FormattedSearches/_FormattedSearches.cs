using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE;

namespace SAPADDON.USERMODEL._FormattedSearches
{
    public class _FormattedSearches
    {
        [FormattedSearchList]
        public class _FormattedSearch
        {
            [FormattedSearch(categoryName = nameof(YAMBOLY.GESTIONACTIVOSFIJOS), query = @"SELECT ""AcctCode"", ""AcctCode"" || ' - ' || ""AcctName"" FROM ""OACT""")]
            public static string FS_MSS_LISTAR_SERIES_ENTREGA { get; set; }

            [FormattedSearch(categoryName = nameof(YAMBOLY.GESTIONACTIVOSFIJOS), query = @"SELECT ""AcctCode"", ""AcctCode"" || ' - ' || ""AcctName"" FROM ""OACT""")]
            public static string FS_MSS_LISTAR_SERIES_DEVOLUCION { get; set; }

            [FormattedSearch(categoryName = nameof(YAMBOLY.GESTIONACTIVOSFIJOS), query = @"SELECT ""WhsCode"", ""WhsCode""|| ' - ' || ""WhsName"" FROM ""OWHS""")]
            public static string FS_MSS_LISTAR_ALMACENES { get; set; }
        }
    }
}
