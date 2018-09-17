using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONF;
using SAPbobsCOM;
//using YAMBOLY.GESTIONACTIVOSFIJOS.MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_VEHICForm
{
    class MSS_VEHIForm : BaseApplication, ISAPForm
    {
        private Form _Form { get; set; }
        public Form GetForm() => _Form;
        public static string GetFormName() => "Maestro de vehículos";


        public MSS_VEHIForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), EmbebbedFileName.MSS_VEHIForm), FormID.MSS_VEHI.IdToString());
            if (_Form != null)
            {
                dictionary.Add(_Form.UniqueID, this);
                _Form.Mode = BoFormMode.fm_ADD_MODE;

                _Form.Freeze(true);

                FillChooseFromLists();

                _Form.Freeze(false);
                _Form.Visible = true;
            }

        }

        private void FillChooseFromLists()
        {
            SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = _Form.ChooseFromLists;
            SAPbouiCOM.ChooseFromList chooseFromList =
                chooseFromListCollection.Item(FormItemIds.cflActivoFijo.IdToString());

            /*Agregar CFL manualmente
            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams;
            oCFLCreationParams = oAplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)
            oCFLCreationParams.MultiSelection = False;
            oCFLCreationParams.ObjectType = oObjectCode;
            oCFLCreationParams.UniqueID = oID;
            //Añadimos la nueva ChooseFromList a la lista del formulario.
            oCFL = oCFLs.Add(oCFLCreationParams)*/

            //Añadimos la condición deseada, para ello creamos un objeto de condiciones, y uno de condición.
            var conditions = chooseFromList.GetConditions();

            var condition = conditions.Add();
            condition.Alias = "ItemType";
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = "F";

            //' Fijamos las condiciones
            chooseFromList.SetConditions(conditions);
        }

        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            _Form = GetApplication().Forms.GetForm(itemEvent.FormTypeEx, itemEvent.FormTypeCount);

            if (GetForm().Mode == BoFormMode.fm_FIND_MODE)
                return true;

            if (itemEvent.ItemUID == FormItemIds.btnSave.IdToString() && itemEvent.EventType == BoEventTypes.et_ITEM_PRESSED && itemEvent.BeforeAction)
                return OnSave(GetApplication().Forms.ActiveForm);

            if (itemEvent.ItemUID == FormItemIds.cbOrigen.IdToString() && itemEvent.ActionSuccess)
                FillBusinessPartner();

            return true;
        }
        public bool HandleItemPressed(SAPbouiCOM.ItemEvent oEvent) { return true; }
        public bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo)
        {
            return true;
        }

        public bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent) { return true; }
        public bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo) { return true; }
        public void FillBusinessPartnerData()
        {

        }

        public void FillBusinessPartner()
        {
            var selectedItem = ((ComboBox)(GetForm().Items.Item(FormItemIds.cbOrigen.IdToString()).Specific)).Value.Trim() as string;
            string newBusinessPartner = null;

            if (selectedItem == OrigenVehiculo.Propio)
            {
                newBusinessPartner = MSS_CONFForm.GetConfigValue(new MSS_CONF().GetMemberName(x => x.MSS_SOCI));
                if (string.IsNullOrEmpty(newBusinessPartner))
                    throw new Exception("No se ha definido el socio de negocios relacionado a la sociedad en el módulo de configuración.");

                GetForm().Items.Item(FormItemIds.txtBusinessPartner.IdToString()).Enabled = false;
                ((EditText)(GetForm().Items.Item(FormItemIds.txtBusinessPartner.IdToString()).Specific)).Value = newBusinessPartner;
            }
            else
            {
                GetForm().Items.Item(FormItemIds.txtBusinessPartner.IdToString()).Enabled = true;
                ((EditText)(GetForm().Items.Item(FormItemIds.txtBusinessPartner.IdToString()).Specific)).Value = null;
            }
        }

        public bool OnSave(Form form)
        {
            var txtCodigo = ((EditText)form.Items.Item(FormItemIds.txtCode.IdToString()).Specific).Value.ToSafeString().Trim();
            var txtPlaca = ((EditText)form.Items.Item(FormItemIds.txtPlaca.IdToString()).Specific).Value.ToSafeString().Trim();
            var txtMarca = ((EditText)form.Items.Item(FormItemIds.txtMarca.IdToString()).Specific).Value.ToSafeString().Trim();
            var txtCertificadoMTC = ((EditText)form.Items.Item(FormItemIds.txtCertificadoMTC.IdToString()).Specific).Value.ToSafeString().Trim();


            var txtBusinessPartner = ((EditText)form.Items.Item(FormItemIds.txtBusinessPartner.IdToString()).Specific).Value.ToSafeString().Trim();

            var txtCapacidadMinCarga = ((EditText)form.Items.Item(FormItemIds.txtCapacidadMinCarga.IdToString()).Specific).Value.ToSafeString().Trim();
            var txtCapacidadMaxCarga = ((EditText)form.Items.Item(FormItemIds.txtCapacidadMaxCarga.IdToString()).Specific).Value.ToSafeString().Trim();

            var txtCapacidadMinVolumen = ((EditText)form.Items.Item(FormItemIds.txtCapacidadMinVolumen.IdToString()).Specific).Value.ToSafeString().Trim();
            var txtCapacidadMaxVolumen = ((EditText)form.Items.Item(FormItemIds.txtCapacidadMaxVolumen.IdToString()).Specific).Value.ToSafeString().Trim();

            var txtNumeroMinRepartos = ((EditText)form.Items.Item(FormItemIds.txtNumeroMinRepartos.IdToString()).Specific).Value.ToSafeString().Trim();
            var txtNumeroMaxRepartos = ((EditText)form.Items.Item(FormItemIds.txtNumeroMaxRepartos.IdToString()).Specific).Value.ToSafeString().Trim();

            var txtOrigen = ((ComboBox)form.Items.Item(FormItemIds.cbOrigen.IdToString()).Specific).Value.ToSafeString().Trim();
            var hasBusinessPartnerSelected = ((EditText)form.Items.Item(FormItemIds.txtBusinessPartner.IdToString()).Specific).Value != string.Empty;
            var hasActivoFijoSelected = ((EditText)form.Items.Item(FormItemIds.txtActivoFijo.IdToString()).Specific).Value != string.Empty;
            var txtEstado = ((ComboBox)form.Items.Item(FormItemIds.cbEstado.IdToString()).Specific).Value.ToSafeString().Trim();


            var socioNegociosPorDefectSociedad = MSS_CONFForm.GetConfigValue(new MSS_CONF().GetMemberName(x => x.MSS_SOCI));
            if (socioNegociosPorDefectSociedad == null)
                throw new Exception("No se ha definido el socio de negocios en el módulo de configuración.");

            string message = null;

            if (string.IsNullOrEmpty(txtCodigo))
                message = "Debe ingresar el código.";

            else if (txtCodigo.Length != 7)
                message = "El código debe tener 7 dígitos";

            else if (string.IsNullOrEmpty(txtEstado))
                message = "Debe seleccionar el estado.";

            else if (string.IsNullOrEmpty(txtOrigen))
                message = "Debe seleccionar un origen.";

            /*else if (txtOrigen == OrigenVehiculo.Propio && !hasActivoFijoSelected)
                message = "Debe seleccionar un activo fijo.";*/

            else if (txtOrigen == OrigenVehiculo.Tercero && !hasBusinessPartnerSelected)
                message = "Debe seleccionar un socio de negocios.";

            else if ((txtOrigen == OrigenVehiculo.Propio && !hasBusinessPartnerSelected) || (txtOrigen == OrigenVehiculo.Propio && txtBusinessPartner != socioNegociosPorDefectSociedad))
                message = "Debe seleccionar el socio de negocios: " + socioNegociosPorDefectSociedad;

            else if (string.IsNullOrEmpty(txtPlaca))
                message = "Debe ingresar la placa.";

            else if (txtPlaca.Length != 7)
                message = "La placa debe tener 7 dígitos";

            else if (string.IsNullOrEmpty(txtMarca))
                message = "Debe ingresar la marca.";

            else if (string.IsNullOrEmpty(txtCertificadoMTC))
                message = "Debe ingresar el certificado MTC.";

            else if (txtCertificadoMTC.Length != 9)
                message = "El certificado MTC debe tener 9 dígitos";

            else if (string.IsNullOrEmpty(txtCapacidadMinCarga))
                message = "Debe ingresar la capacidad mínima de carga.";

            else if (Convert.ToDouble(txtCapacidadMinCarga) <= 0)
                message = "La capacidad mínima de carga debe ser mayor a 0.";

            else if (string.IsNullOrEmpty(txtCapacidadMaxCarga))
                message = "Debe ingresar la capacidad máxima de carga.";

            else if (Convert.
                ToDouble(txtCapacidadMaxCarga) <= 0)
                message = "La capacidad máxima de carga debe ser mayor a 0.";

            else if (string.IsNullOrEmpty(txtCapacidadMinVolumen))
                message = "Debe ingresar la capacidad mínima de volumen.";

            else if (Convert.ToDouble(txtCapacidadMinVolumen) <= 0)
                message = "La capacidad mínima de volumen debe ser mayor a 0.";

            else if (string.IsNullOrEmpty(txtCapacidadMaxVolumen))
                message = "Debe ingresar la capacidad máxima de volumen.";

            else if (Convert.ToDouble(txtCapacidadMaxVolumen) <= 0)
                message = "La capacidad máxima de volumen debe ser mayor a 0.";

            else if (string.IsNullOrEmpty(txtNumeroMinRepartos))
                message = "Debe ingresar el número mínimo de repartos.";

            else if (Convert.ToDouble(txtNumeroMinRepartos) <= 0)
                message = "El número mínimo de repartos debe ser mayor a 0.";

            else if (string.IsNullOrEmpty(txtNumeroMaxRepartos))
                message = "Debe ingresar el número máxixmo de repartos.";

            else if (Convert.ToDouble(txtNumeroMinRepartos) <= 0)
                message = "El número máximo de repartos debe ser mayor a 0.";

            if (message != null)
            {
                ShowMessage(MessageType.Error, message);
                return false;
            }

            ((EditText)form.Items.Item(FormItemIds.txtCode.IdToString()).Specific).Value = txtCodigo;

            return true;
        }

        public enum FormItemIds
        {
            btnSave = 1,
            txtCode = 3,
            txtPlaca = 26,
            txtMarca = 27,
            cbOrigen = 4,
            txtBusinessPartner = 6,
            txtCertificadoMTC = 50,
            cbEstado = 23,
            txtActivoFijo = 24,
            txtCapacidadMinCarga = 39,
            txtCapacidadMaxCarga = 40,
            txtCapacidadMinVolumen = 41,
            txtCapacidadMaxVolumen = 42,
            txtNumeroMinRepartos = 43,
            txtNumeroMaxRepartos = 44,
            cflActivoFijo = 101,
        }
    }
}
