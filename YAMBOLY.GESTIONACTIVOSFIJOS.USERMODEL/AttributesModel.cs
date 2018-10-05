using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL
{
    internal static class AttributesModel { }

    internal class DBStructure : Attribute { }

    internal class SAPTableAttribute : Attribute
    {
        public SAPTableAttribute() { UserFieldList = new List<SAPFieldAttribute>(); }
        public SAPTableAttribute(String tableDescription) { this.TableDescription = tableDescription; }
        public String TableName { get; set; }
        public String TableDescription { get; set; }
        public BoUTBTableType TableType { get; set; }
        public Boolean IsSystemTable { get; set; } = false;
        private List<SAPFieldAttribute> _UserFieldList { get; set; }
        public List<SAPFieldAttribute> UserFieldList
        {
            get
            {
                if (_UserFieldList != null)
                    _UserFieldList.ForEach(x => x.TableName = TableName);
                return _UserFieldList;
            }
            set { _UserFieldList = value; }
        }
    }

    internal class SAPFieldAttribute : Attribute
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

        public String TableName { get; set; } = String.Empty;
        public String FieldName { get; set; } = String.Empty;
        public String _FieldDescription { get; set; }
        public String FieldDescription { get { return _FieldDescription ?? FieldName; } set { _FieldDescription = value; } }
        public BoFieldTypes FieldType { get; set; } = BoFieldTypes.db_Alpha;
        public BoFldSubTypes FieldSubType { get; set; } = BoFldSubTypes.st_None;
        public Int32 FieldSize { get; set; } = 200;
        public BoYesNoEnum IsRequired { get; set; } = BoYesNoEnum.tNO;
        public String[] ValidValues { get; set; } = new String[] { };
        public String[] ValidDescription { get; set; } = new String[] { };
        public String DefaultValue { get; set; } = String.Empty;
        public String VinculatedTable { get; set; } = String.Empty;
        public Boolean IsSearchField { get; set; } = true;
        public Boolean ShowFieldInDefaultForm { get; set; } = true;
        public Boolean IsSystemField { get; set; } = false;
        public Boolean IsPrimaryKey { get; set; } = false;

        public string FormattedSearchName { get; set; }
        public string FormattedSearchCategory { get; set; }

    }

    internal class SAPUDOAttribute : Attribute
    {
        public SAPUDOAttribute() { }
        public String Name { get; set; } = String.Empty;
        public BoUDOObjType ObjectType { get; set; } = BoUDOObjType.boud_MasterData;
        public BoYesNoEnum CanCancel { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanClose { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanCreateDefaultForm { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanFind { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanLog { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum ManageSeries { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum EnableEnhancedForm { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum RebuildEnhancedForm { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanDelete { get; set; } = BoYesNoEnum.tNO;

        public String[] FormColumns { get; set; } = new String[] { };
        public String[] ChildFormColumns { get; set; } = new String[] { };

        public Type HeaderTableType { get; set; }
        public Type[] ChildTableTypeList { get; set; } = new Type[] { };
    }

    internal class FormattedSearchListAttribute : Attribute { }
    internal class MenuListAttribute : Attribute { }

    internal class FormattedSearchAttribute : Attribute
    {
        public string query { get; set; }
        public string queryName { get; set; }
        public string categoryName { get; set; }
    }

    internal class MenuItemAttribute : Attribute
    {
        public string menuUID { get; set; }
        public string menuTitle { get; set; }
        
    }

    public class FormNameAndFields
    {
        public string FormName { get; set; }
        public string[] FieldNames { get; set; }
    }

    internal static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(
         this PropertyInfo type,
         Func<TAttribute, TValue> valueSelector)
         where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }

    }


}
