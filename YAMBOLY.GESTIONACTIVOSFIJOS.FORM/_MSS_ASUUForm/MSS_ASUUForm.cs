using System;
using System.Collections.Generic;
using System.Reflection;
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_ASUA;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_ASUU;
using SAPbobsCOM;
using SAPbouiCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_ASUUForm
{
    public class MSS_ASUUForm : BaseApplication, ISAPForm
    {
        private Form _Form { get; set; }
        public Form GetForm() => _Form;
        public string GetFormId() => FormID.MSS_ASUU.IdToString();
            public static string GetFormName() => "Asignación de usuarios aprobadores";

        public MSS_ASUUForm(Dictionary<string, ISAPForm> dictionary)
        {
            if (FormTypeWasOpened(GetType())) return;

            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(EmbebbedFileName.MSS_ASUUForm), GetFormId());

            if (_Form == null) return;

            dictionary.Add(_Form.UniqueID, this);
            _Form.Freeze(true);

            SetMenuButtons();
            FillMatrix();

            _Form.Freeze(false);
            _Form.Visible = true;
        }

        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            _Form = GetApplication().Forms.GetFormByTypeAndCount(itemEvent.FormType, itemEvent.FormTypeCount);

            //Elimina el id del formulario guardado en sesión.
            if (itemEvent.EventType == BoEventTypes.et_FORM_CLOSE)
                GetFormOpenList().Remove(itemEvent.FormUID);


            else if (itemEvent.ItemUID == FormItemIds.btnSave.IdToString() && itemEvent.BeforeAction)
                SaveTable();

            else if (itemEvent.EventType == BoEventTypes.et_CHOOSE_FROM_LIST && itemEvent.ActionSuccess)
            {
                var cFLEvent = itemEvent as ChooseFromListEvent;
                _Form = GetApplication().Forms.GetFormByTypeAndCount(cFLEvent.FormType, cFLEvent.FormTypeCount);
                SetValuesFromChooseFromList(cFLEvent);
            }

            return true;
        }
        public bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo)
        {
            return true;
        }
        public bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent)
        {
            _Form = GetApplication().Forms.ActiveForm;

            if (menuEvent.MenuUID == MenuUID.AddLine.IdToString())
                AddBlankLine();

            else if (menuEvent.MenuUID == MenuUID.RemoveLine.IdToString() && !menuEvent.BeforeAction)
                RemoveLine(menuEvent);

            return true;
        }
        public bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo) { return true; }

        public enum FormItemIds
        {
            btnSave = -1,
            matrix = 3,
            cflUsuarios = 10,
            clfUsuariosAprobadores = 11,
        }

        #region Logic

        private void SetMenuButtons()
        {
            _Form.EnableMenu(MenuUID.AddLine.IdToString(), true);
            _Form.EnableMenu(MenuUID.RemoveLine.IdToString(), true);
            _Form.EnableMenu(MenuUID.MenuCrear.IdToString(), false);
            _Form.EnableMenu(MenuUID.MenuBuscar.IdToString(), false);
        }

        private void FillMatrix()
        {
            GetDataSource().Query();
            GetMatrix().LoadFromDataSource();
            //GetMatrix().AutoResizeColumns();
        }

        private void AddBlankLine()
        {
            //Inserts record in dataSource
            //GetDataSource().InsertRecord(GetMatrix().RowCount);

            //Inserts record in matrix
            GetMatrix().AddRow();

            //Reload dataSource with Matrix information
            GetMatrix().FlushToDataSource();
        }

        private void RemoveLine(MenuEvent menuEvent)
        {
            while (GetMatrix().GetNextSelectedRow(0, BoOrderType.ot_RowOrder) != -1)
            {
                var lRow = GetMatrix().GetNextSelectedRow(0, BoOrderType.ot_RowOrder);
                var lRowCount = GetMatrix().RowCount;
                if (lRow >= lRowCount)//If arrived the last empty line.
                    break;
                GetMatrix().DeleteRow(lRow);
                GetDataSource().RemoveRecord(lRow - 1);
            }

            GetMatrix().FlushToDataSource();
            //int selRow = GetMatrix().GetNextSelectedRow();
        }

        private void SetValuesFromChooseFromList(ChooseFromListEvent chooseFromListEvent)
        {
            var chooseFromListcolumnNameToGet = "USER_CODE";
            var dataSourceColumnName = string.Empty;


            if (chooseFromListEvent.ChooseFromListUID == FormItemIds.cflUsuarios.IdToString())
            {
                dataSourceColumnName = new MSS_ASUU().GetMemberName(x => x.MSS_USUA);
            }

            else if (chooseFromListEvent.ChooseFromListUID == FormItemIds.clfUsuariosAprobadores.IdToString())
            {
                dataSourceColumnName = new MSS_ASUU().GetMemberName(x => x.MSS_APRO);
            }

            var valueToSet = chooseFromListEvent.SelectedObjects.GetValue(chooseFromListcolumnNameToGet, 0);

            GetDataSource().SetValue(dataSourceColumnName, chooseFromListEvent.Row - 1, valueToSet);
            GetMatrix()?.LoadFromDataSource();
        }

        private Matrix GetMatrix()
        {
            return _Form.Items.Item(FormItemIds.matrix.IdToString()).Specific as Matrix;
        }

        private DBDataSource GetDataSource()
        {
            return _Form.DataSources.DBDataSources.Item(0);
        }

        private void SaveTable()
        {
            GetCompany().StartTransaction();
            try
            {

                DeleteAllItemsUDO();
                var itemsToSave = GetItemList();

                foreach (var item in itemsToSave)
                {
                    try
                    {
                        SaveUDO(item);
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(MessageType.Error, ex.Message);
                    }
                }

                GetCompany().EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (Exception ex)
            {
                GetCompany().EndTransaction(BoWfTransOpt.wf_RollBack);
                throw;
            }

            FillMatrix();
        }

        public List<MSS_ASUU> GetItemList()
        {
            var list = new List<MSS_ASUU>();
            var rowCount = GetMatrix().VisualRowCount;
            for (var i = 1; i <= rowCount; i++)
            {
                list.Add(new MSS_ASUU()
                {
                    MSS_USUA = GetMatrix().Columns.Item(1).Cells.Item(i).Specific.Value,
                    MSS_APRO = GetMatrix().Columns.Item(2).Cells.Item(i).Specific.Value,
                });
            }
            return list;
        }

        public static void SaveUDO(MSS_ASUU item)
        {
            var mService = GetCompany().GetCompanyService().GetGeneralService(nameof(MSS_ASUU));
            var generalData = mService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData) as GeneralData;

            //generalData.

            var code = item.MSS_USUA + item.MSS_APRO;
            generalData.SetProperty(item.GetMemberName(x => x.Code, false), code);
            generalData.SetProperty(item.GetMemberName(x => x.MSS_USUA), item.MSS_USUA);
            generalData.SetProperty(item.GetMemberName(x => x.MSS_APRO), item.MSS_APRO);

            mService.Add(generalData);
        }

        private void DeleteAllItemsUDO()
        {
            var items = DoQuery(EmbebbedFileName.MSS_ASUU_UDO_GetList);
            if (!(items?.RecordCount > 0)) return;

            for (var i = 1; i <= items.RecordCount; i++)
            {
                DeleteUDO(items.Fields.Item(0).Value);
                items.MoveNext();
            }

            //GetDataSource().Query();
            //GetMatrix().LoadFromDataSource();
        }

        public static void DeleteUDO(string code)
        {
            var oGeneralService = GetCompany().GetCompanyService().GetGeneralService(nameof(MSS_ASUU));
            var oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams);
            oGeneralParams.SetProperty(new MSS_ASUA().GetMemberName(x => x.Code, false), code);
            oGeneralService.Delete(oGeneralParams);
        }

        #endregion
    }
}
