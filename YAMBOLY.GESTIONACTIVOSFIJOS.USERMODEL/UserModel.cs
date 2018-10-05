using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL
{
    public class UserModel
    {
        private DBSchema _Schema { get; set; } = new DBSchema();

        private void DefineMenuItems(string containingFolderName = null)
        {
            try
            {
                IEnumerable<Type> formattedSearchesListTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.GetAttributeValue((MenuListAttribute att) => att != null)));
                if (!String.IsNullOrEmpty(containingFolderName))
                    formattedSearchesListTypes.Where(x => string.Equals(x.Namespace, containingFolderName, StringComparison.Ordinal));

                //Por cada clase que tenga el atributo lista de menús
                foreach (Type type in formattedSearchesListTypes)
                {
                    //LEE LAS CLASES DENTRO DE LA CLASE CON 
                    foreach (var item in type.GetNestedTypes())
                    {
                        var menuList = GetMenuList(item);
                        menuList.menuType = MenuType.MenuPrincipal;
                        _Schema.MenuList.Add(menuList);
                    }

                    foreach (var item in type.GetProperties())
                    {
                        _Schema.MenuList.Add(GetMenuList(item));
                    }

                    foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        GetMenuList(p);
                        var v = p.GetValue(null); // static classes cannot be instanced, so use null...
                                                  //do something with v
                        Console.WriteLine(v.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int folderLevel = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Puede ser de tipo atributo o de tipo clase</param>
        private MenuEntity GetMenuList(MemberInfo memberInfo)
        {
            MenuEntity entity = new MenuEntity();
            if (Attribute.IsDefined(memberInfo, typeof(MenuItemAttribute)))
            {
                MenuItemAttribute attribute = (MenuItemAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(MenuItemAttribute), false);
                entity = new MenuEntity()
                {
                    menuTitle = attribute.menuTitle,
                    menuUid = memberInfo.Name,
                };
                var definedClassType = memberInfo as Type;
                if (definedClassType?.IsClass == true)
                {
                    var currentClass = ((Type)memberInfo).UnderlyingSystemType;
                    foreach (var item in currentClass.GetNestedTypes())
                    {
                        var child = GetMenuList(item);
                        child.parentMenuId = entity.menuUid;
                        child.menuType = MenuType.SubMenu;
                        child.subMenuType = SubMenuType.Folder;
                        child.FolderLevel = folderLevel;
                        _Schema.MenuList.Add(child);
                        folderLevel++;
                        //entity.submmenuList.Add(child);
                    }
                    //Se extraen los menús simples
                    foreach (var p in currentClass.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        var child = GetMenuList(p);
                        child.parentMenuId = entity.menuUid;
                        child.menuType = MenuType.SubMenu;
                        child.subMenuType = SubMenuType.String;
                        _Schema.MenuList.Add(child);
                        //entity.submmenuList.Add(child);
                    }
                }
            }
            entity.menuType = MenuType.SubMenu;
            return entity;
        }

        private void DefineFormattedSearchs(string containingFolderName = null)
        {
            try
            {
                IEnumerable<Type> formattedSearchesListTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.GetAttributeValue((FormattedSearchListAttribute att) => att != null)));
                if (!String.IsNullOrEmpty(containingFolderName))
                    formattedSearchesListTypes.Where(x => string.Equals(x.Namespace, containingFolderName, StringComparison.Ordinal));

                //Por cada clase que tenga el atributo Listadebúsquedasformateadas
                foreach (Type type in formattedSearchesListTypes)
                {
                    //Por cada ítem de tipo búsqueda formateada dentro de un listado
                    foreach (var itemType in type.GetProperties())
                    {
                        if (Attribute.IsDefined(itemType, typeof(FormattedSearchAttribute)))
                        {
                            SAPFormattedSearchEntity entity = new SAPFormattedSearchEntity
                            {
                                query = itemType.GetAttributeValue((FormattedSearchAttribute att) => att.query),
                                queryName = itemType.GetAttributeValue((FormattedSearchAttribute att) => att.queryName) ?? itemType.Name,
                                queryCategory = itemType.GetAttributeValue((FormattedSearchAttribute att) => att.categoryName),
                            };
                            /*
                            var fieldsAndFormIds = itemType.GetAttributeValue((FormattedSearchAttribute att) => att.fieldAndFormNames);
                            foreach (var value in fieldsAndFormIds)
                            {
                                if (value.Split('.')?.Count() == 2)
                                {
                                    entity.formId = value.Split('.')[0];
                                    entity.fieldId = value.Split('.')[1];
                                }

                                _Schema.FormattedSearchList.Add(entity);
                            }*/
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating formatted search: ", ex);
            }
        }

        private void DefineSAPTablesAndFields(String containingFolderName = null)
        {
            try
            {
                IEnumerable<Type> tableTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.GetAttributeValue((SAPTableAttribute att) => att != null)));
                if (!String.IsNullOrEmpty(containingFolderName))
                    tableTypes.Where(x => String.Equals(x.Namespace, containingFolderName, StringComparison.Ordinal));

                foreach (Type type in tableTypes)
                {
                    Boolean isSystemTable = type.GetAttributeValue((SAPTableAttribute att) => att.IsSystemTable);
                    SAPTableEntity table = new SAPTableEntity
                    {
                        TableName = type.Name,
                        TableDescription = type.GetAttributeValue((SAPTableAttribute att) => att.TableDescription),
                        TableType = type.GetAttributeValue((SAPTableAttribute att) => att.TableType),
                    };

                    foreach (var itemType in type.GetProperties())
                    {
                        if (!itemType.GetAttributeValue((SAPFieldAttribute att) => att.IsSystemField))
                        {
                            SAPFieldEntity userField = new SAPFieldEntity
                            {
                                FieldName = itemType.Name,
                                FieldDescription = itemType.GetAttributeValue((SAPFieldAttribute att) => att.FieldDescription) ?? itemType.Name,
                                FieldSize = itemType.GetAttributeValue((SAPFieldAttribute att) => att.FieldSize),
                                FieldType = itemType.GetAttributeValue((SAPFieldAttribute att) => att.FieldType),
                                FieldSubType = itemType.GetAttributeValue((SAPFieldAttribute att) => att.FieldSubType),
                                IsRequired = itemType.GetAttributeValue((SAPFieldAttribute att) => att.IsRequired),
                                IsSearchField = itemType.GetAttributeValue((SAPFieldAttribute att) => att.IsSearchField),
                                TableName = (isSystemTable ? "" : "@") + table.TableName,
                                ValidDescription = itemType.GetAttributeValue((SAPFieldAttribute att) => att.ValidDescription),
                                ValidValues = itemType.GetAttributeValue((SAPFieldAttribute att) => att.ValidValues),
                                DefaultValue = itemType.GetAttributeValue((SAPFieldAttribute att) => att.DefaultValue),
                            };
                            _Schema.FieldList.Add(userField);
                        }
                    }
                    if (!isSystemTable)
                        _Schema.TableList.Add(table);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating the database structure: ", ex);
            }
        }

        private void DefineSAPUDOs(String containingFolderName = null)
        {
            try
            {
                IEnumerable<Type> udoTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.GetAttributeValue((SAPUDOAttribute att) => att != null)));
                foreach (Type type in udoTypes)
                {
                    SAPUDOEntity udo = new SAPUDOEntity();
                    udo.Code = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType.Name);
                    udo.Name = type.GetAttributeValue((SAPUDOAttribute att) => att.Name);
                    udo.HeaderTableName = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType.Name);
                    udo.FindColumns = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                                        .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSearchField)).Select(y => y.Name).ToArray();
                    udo.ChildTableNameList = type.GetAttributeValue((SAPUDOAttribute attr) => attr.ChildTableTypeList).Select(y => y.Name).ToArray();
                    udo.ObjectType = type.GetAttributeValue((SAPUDOAttribute att) => att.ObjectType);
                    udo.CanCancel = type.GetAttributeValue((SAPUDOAttribute att) => att.CanCancel);
                    udo.CanCreateDefaultForm = type.GetAttributeValue((SAPUDOAttribute att) => att.CanCreateDefaultForm);
                    udo.CanClose = type.GetAttributeValue((SAPUDOAttribute att) => att.CanClose);
                    udo.CanDelete = type.GetAttributeValue((SAPUDOAttribute att) => att.CanDelete);
                    udo.CanFind = type.GetAttributeValue((SAPUDOAttribute att) => att.CanFind);
                    udo.CanLog = type.GetAttributeValue((SAPUDOAttribute att) => att.CanLog);
                    udo.ChildFormColumns = type.GetAttributeValue((SAPUDOAttribute att) => att.ChildFormColumns);
                    udo.EnableEnhancedForm = type.GetAttributeValue((SAPUDOAttribute att) => att.EnableEnhancedForm);
                    udo.FormColumnsName = type.GetAttributeValue((SAPUDOAttribute att) => att.FormColumns);

                    if (udo.CanCreateDefaultForm == BoYesNoEnum.tYES)
                    {
                        var userFieldsName = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.ShowFieldInDefaultForm && attr2.IsSystemField == false)).Select(y => "U_" + y.Name).ToArray();

                        var defaultFields = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.ShowFieldInDefaultForm && attr2.IsSystemField)).Select(y => y.Name).ToArray();

                        var userFieldsDescription = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.ShowFieldInDefaultForm)).Select(y => y.GetAttributeValue((SAPFieldAttribute attr2) => attr2.FieldDescription)).ToArray();

                        udo.FormColumnsName = defaultFields.Concat(userFieldsName).ToArray();
                        udo.FormColumnsDescription = userFieldsDescription;
                    }

                    if (udo.CanFind == BoYesNoEnum.tYES)
                    {
                        var userFieldsName = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSearchField && attr2.IsSystemField == false)).Select(y => "U_" + y.Name).ToArray();

                        var defaultFields = type.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSearchField && attr2.IsSystemField)).Select(y => y.Name).ToArray();

                        udo.FindColumns = defaultFields.Concat(userFieldsName).ToArray();
                    }

                    udo.ManageSeries = type.GetAttributeValue((SAPUDOAttribute att) => att.ManageSeries);
                    udo.RebuildEnhancedForm = type.GetAttributeValue((SAPUDOAttribute att) => att.RebuildEnhancedForm);
                    _Schema.UDOList.Add(udo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating udo schema: ", ex);
            }

        }

        public DBSchema GetDBSchema()
        {
            DefineSAPTablesAndFields();
            DefineSAPUDOs();
            DefineFormattedSearchs();
            DefineMenuItems();
            return _Schema;
        }

        [Obsolete]
        public DBSchema GetDBSchema(String containerFolderName)
        {
            DefineSAPTablesAndFields(containerFolderName);
            DefineSAPUDOs(containerFolderName);
            DefineFormattedSearchs();
            return _Schema;
        }
    }

    public class DBSchema
    {
        public List<SAPTableEntity> TableList { get; set; } = new List<SAPTableEntity>();
        public List<SAPFieldEntity> FieldList { get; set; } = new List<SAPFieldEntity>();
        public List<SAPUDOEntity> UDOList { get; set; } = new List<SAPUDOEntity>();
        public List<SAPFormattedSearchEntity> FormattedSearchList { get; set; } = new List<SAPFormattedSearchEntity>();
        public List<MenuEntity> MenuList { get; set; } = new List<MenuEntity>();
    }

}
