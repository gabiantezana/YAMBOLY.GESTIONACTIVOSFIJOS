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

                foreach (Type type in formattedSearchesListTypes)
                {
                    foreach (var item in type.GetNestedTypes())
                    {
                        var menuList = GetMenuItem(item);
                        menuList.MenuType = MenuType.MenuPrincipal;
                        _Schema.MenuList.Add(menuList);
                    }

                    foreach (var item in type.GetProperties())
                    {
                        _Schema.MenuList.Add(GetMenuItem(item));
                    }

                    foreach (var p in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        GetMenuItem(p);
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
        private MenuEntity GetMenuItem(MemberInfo memberInfo)
        {
            MenuEntity entity = new MenuEntity();
            if (Attribute.IsDefined(memberInfo, typeof(MenuItemAttribute)))
            {
                MenuItemAttribute attribute = (MenuItemAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(MenuItemAttribute), false);
                entity = new MenuEntity()
                {
                    MenuTitle = attribute.MenuTitle,
                    MenuUid = memberInfo.Name,
                };
                var definedClassType = memberInfo as Type;
                if (definedClassType?.IsClass == true)
                {
                    var currentClass = ((Type)memberInfo).UnderlyingSystemType;
                    foreach (var item in currentClass.GetNestedTypes())
                    {
                        var child = GetMenuItem(item);
                        child.ParentMenuId = entity.MenuUid;
                        child.MenuType = MenuType.SubMenu;
                        child.SubMenuType = SubMenuType.Folder;
                        child.FolderLevel = folderLevel;
                        if (!string.IsNullOrEmpty(child.MenuTitle) && !string.IsNullOrEmpty(child.MenuUid))
                        {
                            _Schema.MenuList.Add(child);
                            folderLevel++;
                        }
                    }
                    foreach (var p in currentClass.GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        var child = GetMenuItem(p);
                        child.ParentMenuId = entity.MenuUid;
                        child.MenuType = MenuType.SubMenu;
                        child.SubMenuType = SubMenuType.String;
                        if (!string.IsNullOrEmpty(child.MenuTitle) && !string.IsNullOrEmpty(child.MenuUid))
                        {
                            _Schema.MenuList.Add(child);
                        }
                    }
                }
            }
            entity.MenuType = MenuType.SubMenu;
            return entity;
        }

        private void DefineFormattedSearchs(string containingFolderName = null)
        {
            try
            {
                IEnumerable<Type> formattedSearchesListTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.GetAttributeValue((QueryListAttribute att) => att != null)));
                if (!String.IsNullOrEmpty(containingFolderName))
                    formattedSearchesListTypes.Where(x => string.Equals(x.Namespace, containingFolderName, StringComparison.Ordinal));

                //Por cada clase que tenga el atributo Listadebúsquedasformateadas
                foreach (Type type in formattedSearchesListTypes)
                {
                    //Por cada ítem de tipo búsqueda formateada dentro de un listado
                    //foreach (var itemType in type.GetProperties())
                    foreach (var itemType in type.GetNestedTypes())
                    {
                        if (Attribute.IsDefined(itemType, typeof(QueryAttribute)))
                        {
                            SAPQueryEntity entity = new SAPQueryEntity
                            {
                                Query = itemType.GetAttributeValue((QueryAttribute att) => att.Query),
                                QueryName = itemType.GetAttributeValue((QueryAttribute att) => att.QueryName) ?? itemType.Name,
                                QueryCategory = itemType.GetAttributeValue((QueryAttribute att) => att.CategoryName),
                            };
                            _Schema.FormattedSearchList.Add(entity);
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

        private void DefineSAPUDOsAndFormattedSearchFields(String containingFolderName = null)
        {
            try
            {
                IEnumerable<Type> udoTypes = Assembly.GetExecutingAssembly().GetTypes().Where(x => (x.GetAttributeValue((SAPUDOAttribute att) => att != null)));
                foreach (Type udoType in udoTypes)
                {
                    SAPUDOEntity udo = new SAPUDOEntity();
                    udo.Code = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType.Name);
                    udo.Name = udoType.GetAttributeValue((SAPUDOAttribute att) => att.Name);
                    udo.HeaderTableName = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType.Name);
                    udo.FindColumns = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                                        .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSearchField)).Select(y => y.Name).ToArray();
                    udo.ChildTableNameList = udoType.GetAttributeValue((SAPUDOAttribute attr) => attr.ChildTableTypeList).Select(y => y.Name).ToArray();
                    udo.ObjectType = udoType.GetAttributeValue((SAPUDOAttribute att) => att.ObjectType);
                    udo.CanCancel = udoType.GetAttributeValue((SAPUDOAttribute att) => att.CanCancel);
                    udo.CanCreateDefaultForm = udoType.GetAttributeValue((SAPUDOAttribute att) => att.CanCreateDefaultForm);
                    udo.CanClose = udoType.GetAttributeValue((SAPUDOAttribute att) => att.CanClose);
                    udo.CanDelete = udoType.GetAttributeValue((SAPUDOAttribute att) => att.CanDelete);
                    udo.CanFind = udoType.GetAttributeValue((SAPUDOAttribute att) => att.CanFind);
                    udo.CanLog = udoType.GetAttributeValue((SAPUDOAttribute att) => att.CanLog);
                    udo.ChildFormColumns = udoType.GetAttributeValue((SAPUDOAttribute att) => att.ChildFormColumns);
                    udo.EnableEnhancedForm = udoType.GetAttributeValue((SAPUDOAttribute att) => att.EnableEnhancedForm);
                    udo.FormColumnsName = udoType.GetAttributeValue((SAPUDOAttribute att) => att.FormColumns);

                    if (udo.CanCreateDefaultForm == BoYesNoEnum.tYES)
                    {
                        var userFieldsName = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.ShowFieldInDefaultForm && attr2.IsSystemField == false)).Select(y => "U_" + y.Name).ToArray();

                        var defaultFields = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.ShowFieldInDefaultForm && attr2.IsSystemField)).Select(y => y.Name).ToArray();

                        var userFieldsDescription = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.ShowFieldInDefaultForm)).Select(y => y.GetAttributeValue((SAPFieldAttribute attr2) => attr2.FieldDescription)).ToArray();

                        udo.FormColumnsName = defaultFields.Concat(userFieldsName).ToArray();
                        udo.FormColumnsDescription = userFieldsDescription;
                    }

                    if (udo.CanFind == BoYesNoEnum.tYES)
                    {
                        var userFieldsName = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSearchField && attr2.IsSystemField == false)).Select(y => "U_" + y.Name).ToArray();

                        var defaultFields = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                            .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSearchField && attr2.IsSystemField)).Select(y => y.Name).ToArray();

                        udo.FindColumns = defaultFields.Concat(userFieldsName).ToArray();
                    }

                    udo.ManageSeries = udoType.GetAttributeValue((SAPUDOAttribute att) => att.ManageSeries);
                    udo.RebuildEnhancedForm = udoType.GetAttributeValue((SAPUDOAttribute att) => att.RebuildEnhancedForm);
                    _Schema.UDOList.Add(udo);

                    //----------------------------------------------------DEFINE CAMPOS ASOCIADOS A BÚSQUEDAS FORMATEADAS------------------------------------------------------
                    var list = udoType.GetAttributeValue((SAPUDOAttribute att) => att.HeaderTableType).GetProperties()
                                        .Where(z => z.GetAttributeValue((SAPFieldAttribute attr2) => attr2.FormattedSearchType != null)).Select(y => y).ToArray();

                    foreach (var item in list)
                    {
                        var formattedSearchType = item.GetAttributeValue((SAPFieldAttribute attr2) => attr2.FormattedSearchType);
                        _Schema.FormattedSearchFieldList.Add(new SAPFormattedSearchEntity()
                        {
                            FieldId = item.GetAttributeValue((SAPFieldAttribute attr2) => attr2.IsSystemField) ? item.Name : "U_" + item.Name,
                            FormId = udo.Code,
                            QueryCategory = formattedSearchType.GetAttributeValue((QueryAttribute x) => x.CategoryName),
                            QueryName = formattedSearchType.Name
                        });
                    };
                    //----------------------------------------------------DEFINE CAMPOS ASOCIADOS A BÚSQUEDAS FORMATEADAS------------------------------------------------------

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
            DefineFormattedSearchs();
            DefineSAPUDOsAndFormattedSearchFields();
            DefineMenuItems();
            return _Schema;
        }

        [Obsolete]
        public DBSchema GetDBSchema(String containerFolderName)
        {
            DefineSAPTablesAndFields(containerFolderName);
            DefineSAPUDOsAndFormattedSearchFields(containerFolderName);
            DefineFormattedSearchs();
            return _Schema;
        }
    }

    public class DBSchema
    {
        public List<SAPTableEntity> TableList { get; set; } = new List<SAPTableEntity>();
        public List<SAPFieldEntity> FieldList { get; set; } = new List<SAPFieldEntity>();
        public List<SAPUDOEntity> UDOList { get; set; } = new List<SAPUDOEntity>();
        public List<SAPQueryEntity> FormattedSearchList { get; set; } = new List<SAPQueryEntity>();
        public List<MenuEntity> MenuList { get; set; } = new List<MenuEntity>();
        public List<SAPFormattedSearchEntity> FormattedSearchFieldList { get; set; } = new List<SAPFormattedSearchEntity>();
    }

}
