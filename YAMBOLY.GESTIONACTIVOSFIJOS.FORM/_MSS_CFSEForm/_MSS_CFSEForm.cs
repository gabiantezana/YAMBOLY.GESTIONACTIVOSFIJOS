using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CFSEForm
{
    class MSS_CFSEForm : BaseApplication, ISAPForm
    {
        public const string FormType = nameof(MSS_CFSEForm);
        private Form _Form { get; set; }
        public Form GetForm() => _Form;
        public static string GetFormName() => "Configuración";

        public MSS_CFSEForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), nameof(MSS_CFSEForm)), FormType);
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
            _Form.EnableMenu(MenuUID.MenuCrear, false);
            _Form.EnableMenu(MenuUID.MenuBuscar, false);
            _Form.EnableMenu(MenuUID.RegistroDatosPrimero, false);
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo, false);
            _Form.EnableMenu(MenuUID.RegistroDatosSiguiente, false);
            _Form.EnableMenu(MenuUID.RegistroDatosAnterior, false);
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
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo, true);
            GetApplication().ActivateMenuItem(MenuUID.RegistroDatosUltimo);
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo, false);

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
