using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
//using YAMBOLY.GESTIONACTIVOSFIJOS.MODEL;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;
using Newtonsoft.Json;
using System.Text;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class SapMethodsHelper
    {
        public static void CreateUDO(Company company, SAPUDOEntity udo)
        {
            _CreateUDO(company, udo);
        }

        public static void CreateTable(Company company, SAPTableEntity table)
        {
            CreateTable(company, table.TableName, table.TableDescription, table.TableType);
        }

        public static void CreateField(Company company, SAPFieldEntity userField)
        {
            CreateField(company, userField.TableName, userField.FieldName, userField.FieldDescription,
                userField.FieldType, userField.FieldSubType, userField.FieldSize, userField.IsRequired,
                userField.ValidValues, userField.ValidDescription, userField.DefaultValue, userField.VinculatedTable, userField.FormattedSearchCategory, userField.FormattedSearchName);
        }

        private static void _CreateUDO(Company company, SAPUDOEntity udo)
        {
            CreateUDOMD(company, udo.Code, udo.Name, udo.HeaderTableName, udo.FindColumns, udo.ChildTableNameList,
                udo.CanCancel, udo.CanClose, udo.CanDelete, udo.CanCreateDefaultForm, udo.FormColumnsName,
                udo.FormColumnsDescription, udo.CanFind, udo.CanLog, udo.ObjectType, udo.ManageSeries,
                udo.EnableEnhancedForm, udo.RebuildEnhancedForm, udo.ChildFormColumns);
        }

        private static void CreateTable(Company _Company, String tableName, String tableDescription,
            SAPbobsCOM.BoUTBTableType tableType)
        {
            SAPbobsCOM.UserTablesMD oUserTablesMD =
                (SAPbobsCOM.UserTablesMD)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            try
            {
                if (!oUserTablesMD.GetByKey(tableName))
                {
                    oUserTablesMD.TableName = tableName;
                    oUserTablesMD.TableDescription = tableDescription;
                    oUserTablesMD.TableType = tableType;
                    if (oUserTablesMD.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                        throw new SapException();
                }
            }
            catch (Exception ex)
            {
                var message = +_Company.GetLastErrorCode() + "- " + _Company.GetLastErrorDescription() + "in " + tableName + "|" + tableDescription;
                throw new Exception(message, ex);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD);
                oUserTablesMD = null;
                GC.Collect();
            }

        }

        private static void CreateField(Company _Company, String tableName, String fieldName, String fieldDescription,
            SAPbobsCOM.BoFieldTypes fieldType, SAPbobsCOM.BoFldSubTypes fieldSubType, Int32? fieldSize,
            SAPbobsCOM.BoYesNoEnum isRequired, String[] validValues, String[] validDescription, String defaultValue,
            String vinculatedTable, string formattedSearchCategory, string formattedSearchName)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD =
                (SAPbobsCOM.UserFieldsMD)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
            try
            {
                tableName = tableName ?? String.Empty;
                fieldName = fieldName ?? String.Empty;
                fieldDescription = fieldDescription ?? String.Empty;
                fieldSize = fieldSize ?? ConstantHelper.DefaultFieldSize;
                validValues = validValues ?? new String[] { };
                validDescription = validDescription ?? new String[] { };
                defaultValue = defaultValue ?? String.Empty;
                vinculatedTable = vinculatedTable ?? String.Empty;

                //string _tableName = "@" + tableName;
                int iFieldID = GetFieldID(_Company, tableName, fieldName);
                if (!oUserFieldsMD.GetByKey(tableName, iFieldID))

                //CUFD udf = new SBODemoCLEntities().CUFD.FirstOrDefault(x => x.TableID == tableName && x.AliasID.ToString() == fieldName);
                //if (udf == null)
                {
                    oUserFieldsMD.TableName = tableName;
                    oUserFieldsMD.Name = fieldName;
                    oUserFieldsMD.Description = fieldDescription;
                    oUserFieldsMD.Type = fieldType;
                    if (fieldType != SAPbobsCOM.BoFieldTypes.db_Date && fieldType != BoFieldTypes.db_Numeric)
                    {
                        oUserFieldsMD.EditSize = fieldSize.Value;
                        oUserFieldsMD.SubType = fieldSubType;
                    }

                    if (vinculatedTable != "") oUserFieldsMD.LinkedTable = vinculatedTable;
                    else
                    {
                        if (validValues.Length > 0)
                        {
                            for (Int32 i = 0; i <= (validValues.Length - 1); i++)
                            {
                                oUserFieldsMD.ValidValues.Value = validValues[i];
                                if (validDescription.Length >= i)
                                    oUserFieldsMD.ValidValues.Description = validDescription[i];
                                else
                                    oUserFieldsMD.ValidValues.Description = validValues[i];

                                oUserFieldsMD.ValidValues.Add();
                            }
                        }
                        oUserFieldsMD.Mandatory = isRequired;
                        if (defaultValue != "") oUserFieldsMD.DefaultValue = defaultValue;
                    }

                    if (oUserFieldsMD.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                        throw new SapException();
                    else
                    {
                        if ((!string.IsNullOrEmpty(formattedSearchCategory)) && !string.IsNullOrEmpty(formattedSearchName))
                            AssignFormattedSearchToField(_Company, formattedSearchCategory, formattedSearchName, tableName, fieldName);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = +_Company.GetLastErrorCode() + "- " + _Company.GetLastErrorDescription() + "in " + tableName + "|" + fieldName + "|" + fieldDescription + "|" + fieldType + "|" + fieldSubType + "|" + fieldSize + "|";
                throw new Exception(message, ex);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();
            }
        }

        private static void CreateUDOMD(Company _Company, String sCode, String sName, String sTableName,
            String[] sFindColumns,
            String[] sChildTables, SAPbobsCOM.BoYesNoEnum eCanCancel, SAPbobsCOM.BoYesNoEnum eCanClose,
            SAPbobsCOM.BoYesNoEnum eCanDelete, SAPbobsCOM.BoYesNoEnum eCanCreateDefaultForm, String[] sFormColumnNames,
            string[] formColumnDescription,
            SAPbobsCOM.BoYesNoEnum eCanFind, SAPbobsCOM.BoYesNoEnum eCanLog, SAPbobsCOM.BoUDOObjType eObjectType,
            SAPbobsCOM.BoYesNoEnum eManageSeries, SAPbobsCOM.BoYesNoEnum eEnableEnhancedForm,
            SAPbobsCOM.BoYesNoEnum eRebuildEnhancedForm, String[] sChildFormColumns)
        {
            SAPbobsCOM.UserObjectsMD oUserObjectMD =
                (SAPbobsCOM.UserObjectsMD)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            try
            {

                if (!oUserObjectMD.GetByKey(sCode))
                {
                    oUserObjectMD.Code = sCode;
                    oUserObjectMD.Name = sName;
                    oUserObjectMD.ObjectType = eObjectType;
                    oUserObjectMD.TableName = sTableName;
                    oUserObjectMD.CanCancel = eCanCancel;
                    oUserObjectMD.CanClose = eCanClose;
                    oUserObjectMD.CanDelete = eCanDelete;
                    oUserObjectMD.CanCreateDefaultForm = eCanCreateDefaultForm;
                    oUserObjectMD.EnableEnhancedForm = eEnableEnhancedForm;
                    oUserObjectMD.RebuildEnhancedForm = eRebuildEnhancedForm;
                    oUserObjectMD.CanFind = eCanFind;
                    oUserObjectMD.CanLog = eCanLog;
                    oUserObjectMD.ManageSeries = eManageSeries;

                    if (sFindColumns != null)
                    {
                        for (Int32 i = 0; i < sFindColumns.Length; i++)
                        {
                            oUserObjectMD.FindColumns.ColumnAlias = sFindColumns[i];
                            oUserObjectMD.FindColumns.Add();
                        }
                    }
                    if (sChildTables != null)
                    {
                        for (Int32 i = 0; i < sChildTables.Length; i++)
                        {
                            oUserObjectMD.ChildTables.TableName = sChildTables[i];
                            oUserObjectMD.ChildTables.Add();
                        }
                    }
                    if (sFormColumnNames != null)
                    {
                        oUserObjectMD.UseUniqueFormType = SAPbobsCOM.BoYesNoEnum.tYES;

                        for (Int32 i = 0; i < sFormColumnNames.Length; i++)
                        {
                            oUserObjectMD.FormColumns.FormColumnAlias = sFormColumnNames[i];
                            oUserObjectMD.FormColumns.FormColumnDescription = formColumnDescription[i];
                            oUserObjectMD.FormColumns.Editable = BoYesNoEnum.tYES;
                            oUserObjectMD.FormColumns.Add();
                        }
                    }
                    if (sChildFormColumns != null)
                    {
                        if (sChildTables != null)
                        {
                            for (Int32 i = 0; i < sChildFormColumns.Length; i++)
                            {
                                oUserObjectMD.FormColumns.SonNumber = 1;
                                oUserObjectMD.FormColumns.FormColumnAlias = sChildFormColumns[i];
                                oUserObjectMD.FormColumns.Add();
                            }
                        }
                    }
                    if (oUserObjectMD.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                        throw new SapException();
                }
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
                oUserObjectMD = null;
                GC.Collect();
            }
        }


        private static void RegistrarAutorizaciones(Company _Company, String s_PermissionID, String s_PermissionName,
            SAPPermissionType oPermissionType, String s_FatherID, String s_FormTypeEx)
        {
            SAPbobsCOM.UserPermissionTree oUserPermissionTree = null;

            oUserPermissionTree =
                (SAPbobsCOM.UserPermissionTree)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes
                    .oUserPermissionTree);
            if (!oUserPermissionTree.GetByKey(s_PermissionID))
            {
                oUserPermissionTree.PermissionID = s_PermissionID;
                oUserPermissionTree.Name = s_PermissionName;
                oUserPermissionTree.Options = SAPbobsCOM.BoUPTOptions.bou_FullReadNone;
                if (oPermissionType == SAPPermissionType.pt_child)
                {
                    oUserPermissionTree.UserPermissionForms.FormType = s_FormTypeEx;
                    oUserPermissionTree.ParentID = s_FatherID;
                }
                if (oUserPermissionTree.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                {
                    throw new SapException();
                }
            }
        }

        private static int GetFieldID(Company company, string tableName, string fieldName)
        {
            int iRetVal = -1;
            SAPbobsCOM.Recordset sboRec = (SAPbobsCOM.Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                sboRec.DoQuery(new QueryHelper(company.DbServerType).Q_GET_FIELD_ID(tableName, fieldName));
                if (!sboRec.EoF) iRetVal = Convert.ToInt32(sboRec.Fields.Item("FieldID").Value.ToString());
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sboRec);
                sboRec = null;
                GC.Collect();
            }
            return iRetVal;
        }

        private void CreaCampoMD(Company company, string NombreTabla, string NombreCampo, string DescCampo,
            SAPbobsCOM.BoFieldTypes TipoCampo, SAPbobsCOM.BoFldSubTypes SubTipo, int Tamano,
            SAPbobsCOM.BoYesNoEnum Obligatorio, string[] validValues, string[] validDescription, string valorPorDef,
            string tablaVinculada)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = null;
            try
            {
                if (NombreTabla == null) NombreTabla = "";
                if (NombreCampo == null) NombreCampo = "";
                if (Tamano == 0) Tamano = 10;
                if (validValues == null) validValues = new string[0];
                if (validDescription == null) validDescription = new string[0];
                if (valorPorDef == null) valorPorDef = "";
                if (tablaVinculada == null) tablaVinculada = "";

                oUserFieldsMD =
                    (SAPbobsCOM.UserFieldsMD)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NombreTabla;
                oUserFieldsMD.Name = NombreCampo;
                oUserFieldsMD.Description = DescCampo;
                oUserFieldsMD.Type = TipoCampo;
                if (TipoCampo != SAPbobsCOM.BoFieldTypes.db_Date) oUserFieldsMD.EditSize = Tamano;
                oUserFieldsMD.SubType = SubTipo;

                if (tablaVinculada != "") oUserFieldsMD.LinkedTable = tablaVinculada;
                else
                {
                    if (validValues.Length > 0)
                    {
                        for (int i = 0; i <= (validValues.Length - 1); i++)
                        {
                            oUserFieldsMD.ValidValues.Value = validValues[i];
                            if (validDescription.Length > 0)
                                oUserFieldsMD.ValidValues.Description = validDescription[i];
                            else oUserFieldsMD.ValidValues.Description = validValues[i];
                            oUserFieldsMD.ValidValues.Add();
                        }
                    }
                    oUserFieldsMD.Mandatory = Obligatorio;
                    if (valorPorDef != "") oUserFieldsMD.DefaultValue = valorPorDef;
                }

                int sf = oUserFieldsMD.Add();
            }
            catch (Exception ex)
            {
                throw new SapException();
            }
            finally
            {

            }
        }

        public static void CreateUserKey(Company company, string tableName, string keyName, string columnAlias)
        {
            UserKeysMD keyMd = (UserKeysMD)company.GetBusinessObject(BoObjectTypes.oUserKeys);
            keyMd.TableName = tableName;
            keyMd.KeyName = keyName;
            keyMd.Elements.ColumnAlias = columnAlias;
            keyMd.Unique = BoYesNoEnum.tYES;
            keyMd.Add();
        }

        public static String GetUserFieldDBName(String fieldName)
        {
            return "U_" + fieldName;
        }

        public static void CreateMenu(Application application, int menuId, string uniqueId, string title, string pathImageBmp)
        {


            var menu = application.Menus.Item(menuId.ToString());
            var subMenus = menu.SubMenus;

            //**********************************************************
            //Setting the menu type as Pop Up menu and setting the
            //UID,image and position in the main menu for the new menu
            //**********************************************************

            var oCreationPackage = ((MenuCreationParams)(application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams)));
            oCreationPackage.Type = BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = uniqueId;
            oCreationPackage.String = title;
            oCreationPackage.Enabled = true;
            //oCreationPackage.Image = sPath + "ball.bmp";
            oCreationPackage.Position = 15;

            try

            {
                //**********************************************************
                //Adding the new menu to the main menu
                //**********************************************************
                subMenus.AddEx(oCreationPackage);

                menu = application.Menus.Item("route");
                //**********************************************************
                //Adding the sub menu of string type to the newly added menu
                //**********************************************************

                subMenus = menu.SubMenus;
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "routesheet";
                oCreationPackage.String = "RouteSheet";
                //oCreationPackage.Image = sPath + "routesheet.bmp";
                subMenus.AddEx(oCreationPackage);
            }
            catch (Exception ex)
            {
            }
        }

        public static void AddNumerationToMatrix(ref Matrix matrix)
        {
            for (int i = 1; i <= matrix.RowCount; i++)
            {
                matrix.Columns.Item(0).Cells.Item(i).Specific.Value = i.ToString();
            }

            for (int i = 1; i <= matrix.RowCount - 1; i++)

            {

                SAPbouiCOM.EditText cellID = (SAPbouiCOM.EditText)matrix.GetCellSpecific("V_-1", i);

                cellID.String = i.ToString();

            }
        }

        public static bool AssignFormattedSearchToField(Company oCompany, string queryCategory, string queryName, string formID, string itemID)
        {
            string columnID = "-1";
            SAPbobsCOM.FormattedSearches oFormattedSearches = null;
            int result = 0;

            try
            {
                oFormattedSearches = (SAPbobsCOM.FormattedSearches)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);
                var exists = oFormattedSearches.GetByKey(GetIDFormattedSearchs(oCompany, formID, itemID, columnID));

                if (exists)
                {
                    result = oFormattedSearches.Remove();
                    exists = false;
                }

                oFormattedSearches = null;

                if (!exists)
                {
                    oFormattedSearches = (SAPbobsCOM.FormattedSearches)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);
                    oFormattedSearches.FormID = formID;
                    oFormattedSearches.ItemID = itemID;
                    if (columnID != "-1") { oFormattedSearches.ColumnID = columnID; }
                    oFormattedSearches.Action = SAPbobsCOM.BoFormattedSearchActionEnum.bofsaQuery;
                    oFormattedSearches.QueryID = GetIDQuery(oCompany, queryName, GetIDQueryCategory(oCompany, queryCategory));
                    oFormattedSearches.Refresh = SAPbobsCOM.BoYesNoEnum.tNO;

                    result = oFormattedSearches.Add();
                    if (result != 0)
                        return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new SapException();
            }
        }

        [Obsolete]//TODO:
        public static bool CreateFMSMD(Company oCompany, string queryName, string query, string formID, string itemID, string queryCategoryFMS,
                                string columnID = "-1", bool autoRefresh = false, bool forceRefresh = false,
                                string autoRefreshField = "", bool removeIfExists = false)
        {
            SAPbobsCOM.FormattedSearches oFormattedSearches = null;
            int result = 0;
            bool ifExistis = false;

            try
            {
                if (CreateQuery(oCompany, queryName, query, queryCategoryFMS, true, false))
                {
                    oFormattedSearches = (SAPbobsCOM.FormattedSearches)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);
                    ifExistis = oFormattedSearches.GetByKey(GetIDFormattedSearchs(oCompany, formID, itemID, columnID));

                    if ((ifExistis) && (removeIfExists))
                    {
                        result = oFormattedSearches.Remove();
                        ifExistis = false;
                    }

                    oFormattedSearches = null;

                    if (!ifExistis)
                    {
                        oFormattedSearches = (SAPbobsCOM.FormattedSearches)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches);
                        oFormattedSearches.FormID = formID;
                        oFormattedSearches.ItemID = itemID;
                        if (columnID != "-1") { oFormattedSearches.ColumnID = columnID; }
                        oFormattedSearches.Action = SAPbobsCOM.BoFormattedSearchActionEnum.bofsaQuery;
                        oFormattedSearches.QueryID = GetIDQuery(oCompany, queryName, GetIDQueryCategory(oCompany, queryCategoryFMS));
                        if ((autoRefresh) && (autoRefreshField != ""))
                        {
                            oFormattedSearches.Refresh = SAPbobsCOM.BoYesNoEnum.tYES;
                            if (columnID == "-1") { oFormattedSearches.ByField = SAPbobsCOM.BoYesNoEnum.tYES; }
                            else { oFormattedSearches.ByField = SAPbobsCOM.BoYesNoEnum.tNO; }
                            oFormattedSearches.FieldID = autoRefreshField;
                            if (forceRefresh)
                                oFormattedSearches.ForceRefresh = SAPbobsCOM.BoYesNoEnum.tYES;
                            else
                                oFormattedSearches.ForceRefresh = SAPbobsCOM.BoYesNoEnum.tNO;
                        }
                        else { oFormattedSearches.Refresh = SAPbobsCOM.BoYesNoEnum.tNO; }

                        result = oFormattedSearches.Add();
                        if (result != 0)
                            throw new SapException();
                    }
                }
                else { return false; }

                return true;
            }
            catch (Exception ex)
            {
                throw new SapException();
            }
        }

        public static bool CreateQuery(Company oCompany, string queryName, string query, string queryCategory, bool createCategory = true, bool removeIfExists = false)
        {
            SAPbobsCOM.UserQueries oUserQueries = null;
            string sqlQuery = string.Empty;
            int result = 0;

            try
            {
                if (removeIfExists) { RemoveQuery(oCompany, queryName, queryCategory); }
                if (createCategory) { CreateQueryCategory(oCompany, queryCategory); }

                var exists = GetIDQuery(oCompany, queryName, GetIDQueryCategory(oCompany, queryCategory)) == -1;
                if (exists)
                {
                    oUserQueries = (SAPbobsCOM.UserQueries)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                    oUserQueries.Query = query;
                    oUserQueries.QueryCategory = GetIDQueryCategory(oCompany, queryCategory);
                    oUserQueries.QueryDescription = queryName;
                    result = oUserQueries.Add();
                    if (result != 0)
                        return false;
                    return true;
                }
            }
            catch
            {
                throw new SapException();
            }

            return true;
        }

        private static int GetIDQuery(Company oCompany, string queryName, int categoryID)
        {
            SAPbobsCOM.Recordset oRecordSet = null;

            try
            {
                oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery(QueryGetIDQuery(oCompany.DbServerType, queryName, categoryID));

                if (oRecordSet.RecordCount > 0)
                {
                    return int.Parse(oRecordSet.Fields.Item(0).Value.ToString());
                }
                else { return -1; }
            }
            catch
            {
                throw new SapException();
            }
        }

        private static string QueryGetIDQuery(SAPbobsCOM.BoDataServerTypes bo_DataServerTypes, string queryName, int categoryID)
        {
            var m_sSQL = new StringBuilder();

            switch (bo_DataServerTypes)
            {
                case SAPbobsCOM.BoDataServerTypes.dst_HANADB:
                    m_sSQL.Append("SELECT \"IntrnalKey\" FROM \"OUQR\" ");
                    m_sSQL.AppendFormat("WHERE \"QName\" = '{0}' ", queryName);
                    m_sSQL.AppendFormat("AND \"QCategory\" = {0} ", categoryID);
                    break;
                default:
                    m_sSQL.Append("SELECT IntrnalKey FROM OUQR ");
                    m_sSQL.AppendFormat("WHERE QName = '{0}' ", queryName);
                    m_sSQL.AppendFormat("AND QCategory = {0} ", categoryID);
                    break;
            }

            return m_sSQL.ToString();
        }

        private static int GetIDQueryCategory(Company oCompany, string queryCategory)
        {
            SAPbobsCOM.Recordset oRecordSet = null;

            try
            {
                oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery(QueryGetIDQueryCategory(oCompany.DbServerType, queryCategory));

                if (oRecordSet.RecordCount > 0)
                {
                    return int.Parse(oRecordSet.Fields.Item(0).Value.ToString());
                }
                else { return -1; }
            }
            catch
            {
                throw new SapException();
            }

        }

        private static string QueryGetIDQueryCategory(SAPbobsCOM.BoDataServerTypes bo_DataServerTypes, string queryCategory)
        {
            var m_sSQL = new StringBuilder();

            switch (bo_DataServerTypes)
            {
                case SAPbobsCOM.BoDataServerTypes.dst_HANADB:
                    m_sSQL.Append("SELECT \"CategoryId\" FROM \"OQCN\" ");
                    m_sSQL.AppendFormat("WHERE \"CatName\" = '{0}' ", queryCategory);
                    break;
                default:
                    m_sSQL.Append("SELECT CategoryId FROM OQCN ");
                    m_sSQL.AppendFormat("WHERE CatName = '{0}' ", queryCategory);
                    break;
            }

            return m_sSQL.ToString();
        }

        private static void RemoveQuery(Company oCompany, string queryName, string queryCategory)
        {
            SAPbobsCOM.UserQueries oUserQueries = null;
            int categoryID = -1;
            int queryID = -1;

            try
            {
                categoryID = GetIDQueryCategory(oCompany, queryCategory);
                queryID = GetIDQuery(oCompany, queryName, categoryID);
                oUserQueries = (SAPbobsCOM.UserQueries)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);
                if (oUserQueries.GetByKey(queryID, categoryID))
                {
                    if (oUserQueries.Remove() != 0)
                    {
                        throw new Exception(oCompany.GetLastErrorDescription());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SapException();
            }
        }

        private static void CreateQueryCategory(Company oCompany, string queryCategory)
        {
            SAPbobsCOM.QueryCategories oQueryCategories = null;

            if (GetIDQueryCategory(oCompany, queryCategory) == -1)
            {
                oQueryCategories = (SAPbobsCOM.QueryCategories)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQueryCategories);
                oQueryCategories.Name = queryCategory;
                oQueryCategories.Permissions = "YYYYYYYYYYYYYYYYYYYY";
                if (oQueryCategories.Add() != 0)
                {
                    throw new Exception(oCompany.GetLastErrorDescription());
                }
            }
        }

        private static int GetIDFormattedSearchs(Company oCompany, string formID, string itemID, string columnID)
        {
            SAPbobsCOM.Recordset oRecordSet = null;

            try
            {
                oRecordSet = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery(QueryGetIDFormattedSearchs(oCompany.DbServerType, formID, itemID, columnID));

                if (oRecordSet.RecordCount > 0)
                {
                    return int.Parse(oRecordSet.Fields.Item(0).Value.ToString());
                }
                else { return -1; }
            }
            catch (Exception ex)
            {
                throw new SapException();
            }
        }

        private static string QueryGetIDFormattedSearchs(SAPbobsCOM.BoDataServerTypes bo_DataServerTypes, string formID, string itemID, string columnID)
        {
            var m_sSQL = new StringBuilder();
            switch (bo_DataServerTypes)
            {
                case SAPbobsCOM.BoDataServerTypes.dst_HANADB:
                    m_sSQL.Append("SELECT \"IndexID\" FROM \"CSHS\" ");
                    m_sSQL.AppendFormat("WHERE \"FormID\" = '{0}' ", formID);
                    m_sSQL.AppendFormat("AND \"ItemID\" = '{0}' ", itemID);
                    m_sSQL.AppendFormat("AND \"ColID\" = '{0}'", columnID);
                    break;
                default:
                    m_sSQL.Append("SELECT IndexID FROM CSHS ");
                    m_sSQL.AppendFormat("WHERE FormID = '{0}' ", formID);
                    m_sSQL.AppendFormat("AND ItemID = '{0}' ", itemID);
                    m_sSQL.AppendFormat("AND ColID = '{0}'", columnID);
                    break;
            }

            return m_sSQL.ToString();
        }

        public static void CreatePrincipalMenul(Application application, string menuUid, string menuTitle)
        {
            MenuCreationParams oCreationPackage = ((MenuCreationParams)(application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams)));
            Menus oMenus = application.Menus;
            SAPbouiCOM.MenuItem oMenuItem = application.Menus.Item("43520");

            if (!application.Menus.Exists(menuUid))
            {
                oMenus = oMenuItem.SubMenus;
                oCreationPackage.Type = BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = menuUid;
                oCreationPackage.String = menuTitle;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = 15;

                //Agrega el nuevo menú al listado principal.
                oMenus.AddEx(oCreationPackage);
            }
        }

        public static void CreateSubMenu(Application application, string principalMenuId, string menuUid, string menuTitle, BoMenuType menuType)
        {
            MenuCreationParams oCreationPackage = ((MenuCreationParams)(application.CreateObject(BoCreatableObjectType.cot_MenuCreationParams)));
            //Obtiene el menú principal
            var oMenuItem = application.Menus.Item(principalMenuId);

            //Instancia submenus del nuevo menú
            var oMenus = oMenuItem.SubMenus;
            if (!oMenuItem.SubMenus.Exists(menuUid))
            {
                //Crea un nuevo submenú
                oCreationPackage.Type = menuType;
                oCreationPackage.UniqueID = menuUid;
                oCreationPackage.String = menuTitle;
                //oCreationPackage.Image = sPath + "routesheet.bmp";
                //Agrega el nuevo submenú
                oMenus.AddEx(oCreationPackage);
            }
        }
    }
}
