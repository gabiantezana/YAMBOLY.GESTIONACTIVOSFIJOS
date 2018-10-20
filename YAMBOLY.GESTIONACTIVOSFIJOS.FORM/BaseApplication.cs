using YAMBOLY.GESTIONACTIVOSFIJOS.DATAACCESS;
using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
//using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_APROForm;
//using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_VEHICForm;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;
using SAPADDON.USERMODEL._Menu;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE;
using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CFSEForm;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM
{
    public class BaseApplication
    {
        public BaseApplication() { }

        public void InitializeApplication()
        {
            try
            {
                ConnectApplication();
                ShowMessage(MessageType.Success, "El Addon se está cargando...");

                ShowMessage(MessageType.Success, "Generando la estructura de base de datos...");
                CreateDataBaseStructure(GetCompany());

                ShowMessage(MessageType.Success, "Creando autorizaciones");
                CreateAuthorizations();

                SetApplicationEventFilters();
                SetApplicationEvents();
                ShowMessage(MessageType.Success, "El Addon se cargó exitosamente");
            }
            catch (Exception ex)
            {
                HandleApplicationException(ex);
                throw;
            }
        }

        public void ConnectApplication()
        {
            new BaseDataAccess().ConnectApplication();
            SetApplicationEventFilters();
        }

        public void CreateDataBaseStructure(SAPbobsCOM.Company company)
        {
            new BaseDataAccess().GenerateDBSchema();
        }

        #region Helper

        public static SAPbouiCOM.Application GetApplication()
        {
            return BaseDataAccess.GetApplication();
        }

        public static Form GetActiveForm()
        {
            return GetApplication()?.Forms.ActiveForm;
        }

        public static SAPbobsCOM.Company GetCompany()
        {
            return BaseDataAccess.GetCompany();
        }

        protected static void ShowMessage(MessageType messageType = MessageType.Success, String message = null)
        {
            String _message = ADDON_NAME + ": ";
            if (GetApplication() != null)
            {
                BoStatusBarMessageType _messageType;
                switch (messageType)
                {
                    case MessageType.Success:
                        message = message ?? ConstantHelper.DEFAULT_SUCCESS_MESSAGE;
                        _messageType = BoStatusBarMessageType.smt_Success;
                        break;
                    case MessageType.Error:
                        message = message ?? ConstantHelper.DEFAULT_ERROR_MESSAGE;
                        _messageType = BoStatusBarMessageType.smt_Error;
                        break;
                    case MessageType.Info:
                        _messageType = BoStatusBarMessageType.smt_None;
                        break;
                    case MessageType.Warning:
                        _messageType = BoStatusBarMessageType.smt_Warning;
                        break;
                    default:
                        _messageType = BoStatusBarMessageType.smt_None;
                        break;
                }
                _message += message;

                GetApplication().StatusBar.SetText(_message, BoMessageTime.bmt_Short, _messageType);
            }
        }

        public static String GetSapError()
        {
            try
            {
                var response = BaseDataAccess.GetErrorFromSAP();
                return "SAP Error CODE: " + response.errorCode + " ,ERROR MESSAGE: " + response.errorMessage;
            }
            catch (Exception ex)
            {
                throw new CustomException(GetApplication(), ex);
            }
        }

        protected static Dictionary<string, ISAPForm> GetFormOpenList()
        {
            return BaseDataAccess.GetFormOpenList();
        }

        [Obsolete]
        public static bool FormTypeWasOpened(Type type, bool bringToFront = true)
        {
            var existingItem = GetFormOpenList().FirstOrDefault(x => x.Value.GetType() == type).Value;
            if (existingItem == null) return false;
            //TODO: //Resolve
            //if (bringToFront)
            //existingItem.GetForm().Select();
            return true;
        }

        public static int ShowAlert(string message)
        {
            return GetApplication().MessageBox(message);
        }

        #endregion

        public string GetSelectedItemFromChooseFromList(SAPbouiCOM.ItemEvent itemEvent)
        {
            var dataTable = ((SAPbouiCOM.IChooseFromListEvent)itemEvent).SelectedObjects;
            var selectedIndex = ((SAPbouiCOM.IChooseFromListEvent)itemEvent).Row;
            var selectedValue = dataTable.GetValue(0, selectedIndex);

            return Convert.ToString(selectedValue);
        }

        #region Queries

        public static SAPbobsCOM.Recordset DoQuery(string query)
        {
            return new BaseDataAccess().DoQuery(query);
        }

        public static string GetQueryString(EmbebbedFileName embebbedFileName)
        {
            return new BaseDataAccess().GetQuery(embebbedFileName);
        }

        public static SAPbobsCOM.Recordset DoQuery(EmbebbedFileName embebbedFileName)
        {
            return new BaseDataAccess().DoQuery(embebbedFileName);
        }

        #endregion

        #region Filters

        private void SetApplicationEventFilters()
        {
            GetApplication().SetFilter(GetEventFilters());
        }

        private EventFilters GetEventFilters()
        {
            EventFilters FilterList = new EventFilters();

            EventFilter eventFilter = FilterList.Add(SAPbouiCOM.BoEventTypes.et_MENU_CLICK);

            eventFilter = FilterList.Add(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            eventFilter.AddEx(nameof(MSS_CFSE));
            eventFilter.AddEx(nameof(MSS_CONT));

            /*
            eventFilter.AddEx(FormID.MSS_VEHI.IdToString());
            eventFilter.AddEx(FormID.MSS_DESP.IdToString());
            eventFilter.AddEx(FormID.MSS_DESP_LIST.IdToString());
            eventFilter.AddEx(FormID.MSS_APRO.IdToString());
            eventFilter.AddEx(FormID.MSS_ASUA.IdToString());
            eventFilter.AddEx(FormID.MSS_ASUU.IdToString());
            eventFilter.AddEx(FormID.MSS_CONF.IdToString());*/

            eventFilter = FilterList.Add(SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*
            eventFilter.AddEx(FormID.MSS_DESP.IdToString());
            eventFilter.AddEx(FormID.MSS_ASUA.IdToString());
            eventFilter.AddEx(FormID.MSS_ASUU.IdToString());*/

            eventFilter = FilterList.Add(SAPbouiCOM.BoEventTypes.et_COMBO_SELECT);
            eventFilter.AddEx(nameof(MSS_CONT));
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*
            eventFilter.AddEx(FormID.MSS_DESP.IdToString());
            eventFilter.AddEx(FormID.MSS_CONF.IdToString());
            eventFilter.AddEx(FormID.MSS_VEHI.IdToString());*/

            eventFilter = FilterList.Add(BoEventTypes.et_VALIDATE);
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*eventFilter.AddEx(FormID.MSS_DESP.IdToString());*/

            eventFilter = FilterList.Add(BoEventTypes.et_FORM_DATA_ADD);
            eventFilter.AddEx(nameof(MSS_CONT));
            eventFilter.AddEx(nameof(MSS_CFSE));

            eventFilter = FilterList.Add(BoEventTypes.et_FORM_DATA_UPDATE);
            eventFilter.AddEx(nameof(MSS_CONT));

            /*eventFilter.AddEx(FormID.MSS_DESP.IdToString());*/

            eventFilter = FilterList.Add(BoEventTypes.et_FORM_CLOSE);
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*
            eventFilter.AddEx(FormID.MSS_ASUA.IdToString());
            eventFilter.AddEx(FormID.MSS_ASUU.IdToString());
            eventFilter.AddEx(FormID.MSS_DESP_LIST.IdToString());*/

            eventFilter = FilterList.Add(BoEventTypes.et_DOUBLE_CLICK);
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*eventFilter.AddEx(FormID.MSS_DESP_LIST.IdToString());*/

            eventFilter = FilterList.Add(BoEventTypes.et_FORM_DATA_LOAD);
            eventFilter.AddEx(nameof(MSS_CONT));
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*eventFilter.AddEx(FormID.MSS_DESP.IdToString());*/

            eventFilter = FilterList.Add(BoEventTypes.et_LOST_FOCUS);
            eventFilter.AddEx(nameof(MSS_CFSE));
            /*eventFilter.AddEx(FormID.MSS_DESP.IdToString());*/

            return FilterList;
        }

        #endregion

        #region Events

        private void AppEvent(BoAppEventTypes EventType)
        {
            try
            {
                switch (EventType)
                {
                    default:
                        BaseDataAccess.DisconnectCompany();
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                HandleApplicationException(ex);
            }

        }


        private void MenuEvent(ref SAPbouiCOM.MenuEvent menuEvent, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (!menuEvent.BeforeAction)
                {
                    switch (menuEvent.MenuUID)
                    {
                        case _Menu.MENU_PRINCIPAL.MENU_CONFIGURACION.MENU_CONFIGURACIONSERIES:
                            new _MSS_CFSEForm.MSS_CFSEForm(GetFormOpenList());
                            break;

                        case _Menu.MENU_PRINCIPAL.MENU_CONFIGURACION.MENU_CONFIGURACIONPERMISOS:
                            new _MSS_CFPEForm.MSS_CFPEForm(GetFormOpenList());
                            break;

                        case _Menu.MENU_PRINCIPAL.MENU_CONTRATOCONCESIONACTIVOFIJO:
                            new _MSS_CONTForm.MSS_CONTForm(GetFormOpenList());
                            break;

                        default:
                            if (GetFormOpenList().ContainsKey(GetActiveForm().UniqueID))
                                GetFormOpenList()[GetActiveForm().UniqueID].HandleMenuDataEvents(menuEvent);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleApplicationException(ex);
            }
            finally
            {
                BubbleEvent = true;
                GC.Collect();
            }
        }

        private void ItemEvent(string formUID, ref ItemEvent itemEvent, out bool BubbleEvent)
        {
            var _bubbleEvent = true;
            try
            {
                if (GetFormOpenList().ContainsKey(formUID))
                    _bubbleEvent = GetFormOpenList()[formUID].HandleItemEvents(itemEvent);
            }
            catch (Exception ex)
            {
                _bubbleEvent = false;
                HandleApplicationException(ex);
            }
            finally
            {
                GC.Collect();
                BubbleEvent = _bubbleEvent;
            }
        }

        private void FormDataEvent(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            var _bubbleEvent = true;
            try
            {
                if (GetFormOpenList().ContainsKey(BusinessObjectInfo.FormUID))
                    _bubbleEvent = GetFormOpenList()[BusinessObjectInfo.FormUID].HandleFormDataEvents(BusinessObjectInfo);
            }
            catch (Exception ex)
            {
                HandleApplicationException(ex);
            }
            finally
            {
                BubbleEvent = _bubbleEvent;
                GC.Collect();
            }
        }

        private void RightClickEvent(ref ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            try
            {
                if (GetFormOpenList().ContainsKey(eventInfo.FormUID))
                    GetFormOpenList()[eventInfo.FormUID].HandleRightClickEvent(eventInfo);
            }
            catch (Exception ex)
            {
                HandleApplicationException(ex);
            }
            finally
            {
                BubbleEvent = true;
                GC.Collect();
            }
        }

        private void SetApplicationEvents()
        {
            GetApplication().AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(AppEvent);
            GetApplication().MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(MenuEvent);
            GetApplication().ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(ItemEvent);
            GetApplication().FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(FormDataEvent);
            GetApplication().RightClickEvent += new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(RightClickEvent);
        }

        #endregion

        #region Handle Exceptions

        public static dynamic HandleApplicationException(Exception ex, string message = null)
        {
            Exception _ex = ex;
            try
            {
                if (ex.GetType() == typeof(SapException))
                {
                    _ex = new Exception(GetSapError(), _ex);
                    ShowMessage(MessageType.Error, message + " " + _ex.Message);
                    ExceptionHelper.LogException(_ex);
                }
                else if (ex.GetType() == typeof(CustomException))
                {
                    ShowMessage(MessageType.Warning, message ?? ex.Message);
                }
                else
                {
                    ShowMessage(MessageType.Error, ex.Message);
                    ExceptionHelper.LogException(ex);
                }
            }
            catch
            {
                throw;
            }

            return null;
        }

        #endregion

        #region Authorizations

        private static void CreateAuthorizations()
        {
            /*  
            SAPbobsCOM.UserPermissionTree permission = GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);
            if (!permission.GetByKey(PARENTPERMISSIONKEY))
            {
                permission.PermissionID = PARENTPERMISSIONKEY;
                permission.Name = PARENTPERMISSIONNAME;
                permission.Options = SAPbobsCOM.BoUPTOptions.bou_FullNone;
                permission.Add();
            }

            if (!permission.GetByKey(nameof(MSS_ASUU)))
            {
                permission.PermissionID = nameof(MSS_ASUU);
                permission.Name = MSS_ASUUForm.GetFormName();
                permission.Options = SAPbobsCOM.BoUPTOptions.bou_FullNone;
                permission.ParentID = PARENTPERMISSIONKEY;
                permission.UserPermissionForms.FormType = FormID.MSS_ASUU.IdToString();
                permission.Add();
            }

            if (!permission.GetByKey(nameof(MSS_ASUA)))
            {
                permission.PermissionID = nameof(MSS_ASUA);
                permission.Name = MSS_ASUAForm.GetFormName();
                permission.Options = SAPbobsCOM.BoUPTOptions.bou_FullNone;
                permission.ParentID = PARENTPERMISSIONKEY;
                permission.UserPermissionForms.FormType = FormID.MSS_ASUA.IdToString();
                permission.Add();
            }


            if (!permission.GetByKey(nameof(MSS_CONF)))
            {
                permission.PermissionID = nameof(MSS_CONF);
                permission.Name = MSS_CONFForm.GetFormName();
                permission.Options = SAPbobsCOM.BoUPTOptions.bou_FullNone;
                permission.ParentID = PARENTPERMISSIONKEY;
                permission.UserPermissionForms.FormType = FormID.MSS_CONF.IdToString();
                permission.Add();
            }
            */
        }

        #endregion
    }
}

