using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbouiCOM;
using System;
using System.Collections.Generic;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_VEHICForm
{
    class MSS_CONFForm : BaseApplication, ISAPForm
    {
        private Form _Form { get; set; }
        public Form GetForm() => _Form;
        public static string GetFormName() => "Configuración";

        public MSS_CONFForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), EmbebbedFileName.MSS_CONFForm), FormID.MSS_CONF.IdToString());
            if (_Form != null)
            {
                dictionary.Add(_Form.UniqueID, this);

                //Si ya existe una configuración, la carga.
                if (DoQuery(EmbebbedFileName.MSS_CONF_GetItem).RecordCount > 0)
                    LoadLastRecord();
                else
                    _Form.Mode = BoFormMode.fm_ADD_MODE;

                _Form.Freeze(true);
                SetMenuButtons();
                _Form.Freeze(false);
                _Form.Visible = true;
            }
        }



        private void SetMenuButtons()
        {
            _Form.EnableMenu(MenuUID.MenuCrear.IdToString(), false);
            _Form.EnableMenu(MenuUID.MenuBuscar.IdToString(), false);
            _Form.EnableMenu(MenuUID.RegistroDatosPrimero.IdToString(), false);
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo.IdToString(), false);
            _Form.EnableMenu(MenuUID.RegistroDatosSiguiente.IdToString(), false);
            _Form.EnableMenu(MenuUID.RegistroDatosAnterior.IdToString(), false);
        }

        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            if (itemEvent.ItemUID == FormItemIds.btnSave.IdToString())
            {
                if (itemEvent.BeforeAction)
                    return OnSave(GetApplication().Forms.ActiveForm);
                else
                    LoadLastRecord();
            }

      
            return true;
        }

        public bool HandleItemPressed(SAPbouiCOM.ItemEvent oEvent) { return true; }
        public bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo)
        {
            return true;
        }

        public bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent) { return true; }
        public bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo) { return true; }



        public bool OnSave(Form form)
        {
            var code = Guid.NewGuid().GetHashCode();
            form.Items.Item(FormItemIds.txtCode.IdToString()).Specific.Value = code;
            return true;
        }



        public void LoadLastRecord()
        {
            //Ve al último registro
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo.IdToString(), true);
            GetApplication().ActivateMenuItem(MenuUID.RegistroDatosUltimo.IdToString());
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo.IdToString(), false);

            //Modo update
            GetForm().Mode = BoFormMode.fm_UPDATE_MODE;
        }

        public static string GetConfigValue(string columnName)
        {
            string value = null;
            var query = DoQuery(EmbebbedFileName.MSS_CONF_GetItem);
            if (query.RecordCount > 0)
                value = query.Fields.Item(columnName).Value;
            return value;
        }

        public enum FormItemIds
        {
            btnSave = 1,
            txtCode = 4,
        }
    }
}
