using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbouiCOM;
using System.Collections.Generic;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT;
using System;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;
using System.Reflection;
using SAPADDON.USERMODEL.Helper;
using SAPADDON.USERMODEL._FormattedSearches;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFPE;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OITM;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._ODLN;
using SAPADDON.USERMODEL._DLN1;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE;
using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CONTForm
{
    class MSS_CONTForm_Modal : BaseApplication, ISAPForm
    {
        public const string FormType = nameof(MSS_CONTForm_Modal);
        private Form _Form { get; set; }
        MSS_CONTForm _ParentForm { get; set; }

        public MSS_CONTForm_Modal(Dictionary<string, ISAPForm> dictionary, MSS_CONTForm parentForm)
        {
            _ParentForm = parentForm;

            string docEntry = _ParentForm.GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value;
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_LISTARETORNO)).Replace(PARAM1, docEntry).Replace(PARAM2, MSS_CONT_LINES.ESTADO.RETORNADO.KEY);
            var rs = DoQuery(query);
            if (rs.RecordCount <= 0)
                throw new CustomException("No se encontraron ítems disponibles para retorno y/o todos los ítems se encuentran en estado retornado.");

            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetResourceString(System.Reflection.Assembly.GetExecutingAssembly(), this.GetType().Name), FormType);
            dictionary.Add(_Form.UniqueID, this);
            FillGrid(docEntry);
        }

        private void FillGrid(string docEntry)
        {
            _Form.Mode = BoFormMode.fm_ADD_MODE;

            string dataTableId = Guid.NewGuid().ToString();
            Grid grid = GetItemEspecific("3");

            DataTable dataTable = _Form.DataSources.DataTables.Add(dataTableId);
            dataTable.ExecuteQuery(FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_LISTARETORNO)).Replace(PARAM1, docEntry));

            if (!dataTable.IsEmpty)
            {
                grid.DataTable = dataTable;
                grid.Columns.Item(0).Type = BoGridColumnType.gct_CheckBox;
                //oEditCol.LinkedObjectType = "4";

                grid.Columns.Item(1).Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox;
                ((SAPbouiCOM.ComboBoxColumn)grid.Columns.Item(1)).ValidValues.Add("1", OITM.ValidValues.MSS_EAAF.Disponible.DESCRIPTION);//TODO: REMOVE HARDCODE
                ((SAPbouiCOM.ComboBoxColumn)grid.Columns.Item(1)).ValidValues.Add("2", OITM.ValidValues.MSS_EAAF.EnMantenimiento.DESCRIPTION);//TODO: REMOVE HARDCODE

                ((SAPbouiCOM.ComboBoxColumn)grid.Columns.Item(1)).DisplayType = BoComboDisplayType.cdt_Description;
            }
        }

        private dynamic GetItemEspecific(string itemName)
        {
            return _Form.Items.Item(itemName).Specific;
        }

        private List<FixedAsset> GetSelectedItems()
        {
            string itemCode;
            string state;

            var list = new List<FixedAsset>();
            for (var i = 0; i < GetGrid().DataTable.Rows.Count; i++)
            {
                var check = GetGrid().DataTable.GetValue(0, i) as string == ConstantHelper.SAP_YES_NO.YES;
                if (check)
                {
                    state = GetGrid().DataTable.GetValue(1, i) as string;
                    itemCode = GetGrid().DataTable.GetValue(2, i) as string;
                    switch (state)
                    {
                        case "1"://TODO: REMOVE HARDCODE
                            state = OITM.ValidValues.MSS_EAAF.Disponible.ID;
                            break;
                        case "2":
                            state = OITM.ValidValues.MSS_EAAF.EnMantenimiento.ID;
                            break;
                        default:
                            throw new Exception("Seleccione un estado válido para la fila número " + (i + 1));
                    }
                    list.Add(new FixedAsset() { ItemCode = itemCode, State = state });
                }

            }
            if (list.Count == 0)
                throw new Exception("Seleccione al menos un ítem para retornar.");

            return list;
        }

        public Grid GetGrid()
        {
            return GetItemEspecific("3") as Grid;
        }

        private void ShowModal(string[] itemCodes)
        {
            FormCreationParams formParams = GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);
            formParams.UniqueID = "aefsiissjf";
            formParams.Modality = BoFormModality.fm_Modal;

            var form = GetApplication().Forms.AddEx(formParams);
            form.Settings.Enabled = true;
            form.AutoManaged = false;
            form.Mode = BoFormMode.fm_OK_MODE;
            form.Title = "Retorno de activos fijos";
            form.Width = 300;
            form.Height = 300;

            form.Visible = true;

            SAPbouiCOM.Item itmbtn = form.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            itmbtn.Visible = true;
            itmbtn.Top = 220;   // inline with the OK button
            itmbtn.Left = form.Width - 110;
            itmbtn.Height = 18;
            itmbtn.Width = 70;
            itmbtn.FromPane = 0;
            itmbtn.ToPane = 0;

            SAPbouiCOM.Button btn = (SAPbouiCOM.Button)itmbtn.Specific;
            btn.Caption = "Cancelar";
            btn.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;


            itmbtn = form.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            itmbtn.Visible = true;
            itmbtn.Top = 220;   // inline with the OK button
            itmbtn.Left = form.Width - 270;
            itmbtn.Height = 18;
            itmbtn.Width = 70;
            itmbtn.FromPane = 0;
            itmbtn.ToPane = 0;

            btn = (SAPbouiCOM.Button)itmbtn.Specific;
            btn.Caption = "Aceptar";
            btn.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;





        }

        #region Events

        public bool HandleItemEvents(ItemEvent itemEvent)
        {
            if (itemEvent.ActionSuccess && itemEvent.ItemUID == "4")//TODO: Remove hardcode
            {

                _ParentForm.CrearRetorno(GetSelectedItems());
                _Form.Close();
            }
            return true;
        }
        public bool HandleItemPressed(ItemEvent oEvent) { return true; }
        public bool HandleFormDataEvents(BusinessObjectInfo oBusinessObjectInfo)
        {
            return true;
        }
        public bool HandleMenuDataEvents(MenuEvent menuEvent)
        {
            switch (menuEvent.MenuUID)
            {
                default:
                    break;
            }

            return true;
        }
        public bool HandleRightClickEvent(ContextMenuInfo menuInfo) { return true; }

        #endregion
    }
}
