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
                userField.ValidValues, userField.ValidDescription, userField.DefaultValue, userField.VinculatedTable);
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
            String vinculatedTable)
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
    }


}
