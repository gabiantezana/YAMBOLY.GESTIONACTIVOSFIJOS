using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbouiCOM;
using System.Collections.Generic;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT;
using System;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;
using System.Reflection;
using SAPADDON.USERMODEL.Helper;
using SAPADDON.USERMODEL._FormattedSearches;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CONTForm
{
    class MSS_CONTForm : BaseApplication, ISAPForm
    {
        public const string FormType = nameof(MSS_CONT);
        private Form _Form { get; set; }

        public MSS_CONTForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetResourceString(System.Reflection.Assembly.GetExecutingAssembly(), this.GetType().Name), FormType);
            dictionary.Add(_Form.UniqueID, this);
            LoadSeries();
            SetEstadoAsPendiente();
            GetMatrix().AddRow();
            _Form.EnableFormatSearch();
        }

        private void SetEstadoAsPendiente()
        {
            (GetItem(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Select(MSS_CONT.ESTADO.PENDIENTE.KEY);
            (GetItem(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Active = false;
            (GetItem(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Item.Enabled = false;
        }

        private void LoadSeries()
        {
            try
            {
                (GetItem(nameof(MSS_CONT.Series)) as ComboBox)?.ValidValues.LoadSeries(_Form.BusinessObject.Type, BoSeriesMode.sf_View);
                LoadNumeration();
            }
            catch (Exception ex)
            { }
        }

        private void LoadNumeration()
        {
            var cbSeries = (GetItem(nameof(MSS_CONT.Series)) as ComboBox);
            GetDataSource().SetValue(nameof(MSS_CONT.DocNum), 0, _Form.BusinessObject.GetNextSerialNumber(cbSeries?.Selected?.Value, _Form.BusinessObject.Type).ToSafeString());
            _Form.Refresh();
        }

        private Matrix GetMatrix()
        {
            return _Form.Items.Item("MATRIX").Specific as Matrix;
        }

        private DBDataSource GetDataSource()
        {
            return _Form.DataSources.DBDataSources.Item(0);
        }

        private dynamic GetItem(string itemName)
        {
            return _Form.Items.Item(itemName).Specific;
        }

        #region Queries

        private void SetBusinessPartnerInfo(string cardCode)
        {
            string query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_OBTENER_CLIENTE));
            query = query.Replace(ConstantHelper.PARAM1, cardCode);
            var recordSet = new SapMethodsHelper().DoQuery(GetCompany(), query);
            if (recordSet.RecordCount > 0)
            {
                (GetItem(nameof(MSS_CONT.U_MSS_CNOM)) as EditText).Value = recordSet.Fields.Item(0).Value;
                (GetItem(nameof(MSS_CONT.U_MSS_CRUC)) as EditText).Value = recordSet.Fields.Item(1).Value;
            }
        }

        private void GetAddress(string cardCode, string address)
        {

        }

        #endregion

        #region Events

        public bool HandleItemEvents(ItemEvent itemEvent)
        {
            if (itemEvent.ItemUID == MSS_CONT.Series && itemEvent.ActionSuccess && itemEvent.EventType == BoEventTypes.et_COMBO_SELECT)
                LoadNumeration();

            return true;
        }
        public bool HandleItemPressed(ItemEvent oEvent) { return true; }
        public bool HandleFormDataEvents(BusinessObjectInfo oBusinessObjectInfo) { return true; }
        public bool HandleMenuDataEvents(MenuEvent menuEvent) { return true; }
        public bool HandleRightClickEvent(ContextMenuInfo menuInfo) { return true; }

        #endregion
    }
}
