using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class QueryHelper
    {
        public QueryHelper(BoDataServerTypes serverType) { this._serverType = serverType; }

        private BoDataServerTypes _serverType { get; set; }
        private String _SQLQuery { get; set; }
        private String _HANAQuery { get; set; }
        private String ReturnQuery()
        {
            switch (_serverType)
            {
                case BoDataServerTypes.dst_HANADB:
                    return _HANAQuery;
                default:
                    return _SQLQuery;
            }
        }

        public String Q_GET_FIELD_ID(String tableName, String fieldName)
        {
            _SQLQuery = "select FieldID from CUFD where TableID = '" + tableName + "' and AliasID = '" + fieldName + "'";
            _HANAQuery = "select \"FieldID\" from \"CUFD\" where \"TableID\" = '" + tableName + "' and \"AliasID\" = '" + fieldName + "'";
            return ReturnQuery();

        }


    }
}


