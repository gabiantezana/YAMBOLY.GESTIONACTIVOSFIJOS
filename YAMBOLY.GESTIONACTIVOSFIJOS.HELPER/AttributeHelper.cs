using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPADDON.HELPER
{
    public class AttributeHelper { }
  
    public class SAPFieldAttribute : Attribute, ISAPField
    {
        public SAPFieldAttribute()
        {
            /*
            FieldName = String.Empty;
            FieldDescription = String.Empty;
            FieldType = BoFieldTypes.db_Alpha;
            FieldSubType = BoFldSubTypes.st_None;
            FieldSize = 0;
            IsRequired = BoYesNoEnum.tNO;
            ValidValues = new String[] { };
            ValidDescription = new String[] { };
            DefaultValue = String.Empty;
            VinculatedTable = String.Empty;
            IsSearchField = false;*/
        }

        public String FieldName { get; set; } = String.Empty;
        public String FieldDescription { get; set; } = String.Empty;
        public BoFieldTypes FieldType { get; set; } = BoFieldTypes.db_Alpha;
        public BoFldSubTypes FieldSubType { get; set; } = BoFldSubTypes.st_None;
        public Int32 FieldSize { get; set; } = 200;
        public BoYesNoEnum IsRequired { get; set; } = BoYesNoEnum.tNO;
        public String[] ValidValues { get; set; } = new String[] { };
        public String[] ValidDescription { get; set; } = new String[] { };
        public String DefaultValue { get; set; } = String.Empty;
        public String VinculatedTable { get; set; } = String.Empty;
        public Boolean IsSearchField { get; set; } = false;
    }

    public class DBStructureAttribute : Attribute { }

    public class SAPTableAttribute : Attribute, ISAPTable
    {
        public SAPTableAttribute(String tableDescription) { this.TableDescription = tableDescription; }
        public String TableName { get; set; }
        public String TableDescription { get; set; }
    }
}
