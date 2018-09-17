using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class SapEntityHelper { }

    public class UDOEntity
    {
        public UDOEntity()
        {
            this.HeaderTable = new TableEntity();
            this.ChildTableList = new List<TableEntity>();
        }

        public String Name { get; set; }
        public BoYesNoEnum CanCancel { get; set; }
        public BoYesNoEnum CanClose { get; set; }
        public BoYesNoEnum CanCreateDefaultForm { get; set; }
        public BoYesNoEnum CanFind { get; set; }
        public BoYesNoEnum CanLog { get; set; }
        public BoUDOObjType ObjectType { get; set; }
        public BoYesNoEnum ManageSeries { get; set; }
        public BoYesNoEnum EnableEnhancedForm { get; set; }
        public BoYesNoEnum RebuildEnhancedForm { get; set; }
        public String[] FormColumns { get; set; }
        public String[] ChildFormColumns { get; set; }
        public BoYesNoEnum CanDelete { get; set; }

        public TableEntity HeaderTable { get; set; }
        public List<TableEntity> ChildTableList { get; set; }
        public string Code { get { return this.HeaderTable.TableName; } }
        public string TableName { get { return this.HeaderTable.TableName; } }
        public string[] FindColumns
        {
            get
            {
                try
                {
                    return this.HeaderTable.UserFieldList.Where(x => x.IsSearchField).Select(y => "U_" + y.FieldName).ToArray();
                }
                catch (Exception ex) { throw new Exception("Error obtaining find columns for UDO", ex); }
            }
        }
        public String[] ChildTableNames
        {
            get
            {
                try
                {
                    return ChildTableList.Select(x => x.TableName).ToArray();
                }
                catch (Exception ex) { throw new Exception("Error obtaining child table names for UDO", ex); }
            }
        }
    }

    public class TableEntity : Attribute, ISAPTable
    {
        public TableEntity()
        {
            UserFieldList = new List<UserFieldEntity>();
        }
        private List<UserFieldEntity> _UserFielList { get; set; }

        public String TableName { get; set; }
        public String TableDescription { get; set; }
        public BoUTBTableType TableType { get; set; }
        public List<UserFieldEntity> UserFieldList
        {
            get
            {
                if (_UserFielList != null)
                    _UserFielList.ForEach(x => x.TableName = TableName);
                return _UserFielList;
            }
            set { _UserFielList = value; }
        }
    }

    public class UserFieldEntity
    {
        public UserFieldEntity()
        {
            FieldSubType = BoFldSubTypes.st_None;
            IsRequired = BoYesNoEnum.tNO;
            FieldSize = 0;
            DefaultValue = String.Empty;
            VinculatedTable = String.Empty;
            ValidValues = new String[] { };
            ValidDescription = new String[] { };
        }

        public String TableName { get; set; }
        public String FieldName { get; set; }

        public String _FieldDescription { get; set; }
        public String FieldDescription { get { return _FieldDescription ?? FieldName; } set { _FieldDescription = value; } }

        public BoFieldTypes FieldType { get; set; }
        public BoFldSubTypes FieldSubType { get; set; }
        public Int32 FieldSize { get; set; }
        public BoYesNoEnum IsRequired { get; set; }
        public String[] ValidValues { get; set; }
        public String[] ValidDescription { get; set; }
        public String DefaultValue { get; set; }
        public String VinculatedTable { get; set; }
        public Boolean IsSearchField { get; set; }
    }
}
