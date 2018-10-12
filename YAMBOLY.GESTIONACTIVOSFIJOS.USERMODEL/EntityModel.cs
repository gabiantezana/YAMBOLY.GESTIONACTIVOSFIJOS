using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL
{
    public class EntityModel { }

    public class SAPTableEntity
    {
        public String TableName { get; set; } = String.Empty;
        public String TableDescription { get; set; } = String.Empty;
        public BoUTBTableType TableType { get; set; } = BoUTBTableType.bott_NoObject;
        public List<SAPFieldEntity> UserFieldList { get; set; } = new List<SAPFieldEntity>();
    }

    public class SAPFieldEntity
    {
        public String TableName { get; set; } = String.Empty;
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

        #region Búsquedas formateadas para formularios

        public string FormattedSearchName { get; set; }
        public string FormattedSearchCategory { get; set; }

        #endregion
    }

    public class SAPUDOEntity
    {
        public SAPUDOEntity() { }

        public String Name { get; set; } = String.Empty;
        public String Code { get; set; } = String.Empty;
        public String HeaderTableName { get; set; } = String.Empty;
        public String[] FindColumns { get; set; }

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

        public String[] FormColumnsName { get; set; } = new String[] { };
        public String[] FormColumnsDescription { get; set; } = new String[] { };
        public String[] ChildFormColumns { get; set; } = new String[] { };

        public String[] ChildTableNameList { get; set; }

    }

    public class SAPQueryEntity
    {
        public string Query { get; set; }
        public string QueryName { get; set; }
        public string QueryCategory { get; set; }
    }

    public class SAPFormattedSearchEntity
    {
        public string QueryName { get; set; }
        public string QueryCategory { get; set; }
        public string FormId { get; set; }
        public string FieldId { get; set; }
    }

    public class MenuEntity
    {
        public string MenuUid { get; set; }
        public string MenuTitle { get; set; }
        public MenuType MenuType { get; set; }
        public SubMenuType SubMenuType { get; set; }
        public string ParentMenuId { get; set; }
        public int FolderLevel { get; set; }
    }

    public enum MenuType
    {
        MenuPrincipal = 1,
        SubMenu = 2,
    }

    //DontchangeIds
    public enum SubMenuType
    {
        String = 1,//DONT CHANGE ID
        Folder = 2,//DONT CHANGE ID
    }


    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
