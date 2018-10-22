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

            _Form.EnableMenu(MenuUID.AddLine, true);
            _Form.EnableMenu(MenuUID.RemoveLine, true);
            CreateRightClickMenu();
        }


        private void CreateRightClickMenu()
        {
            MenuCreationParams oCreationPackage = GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams);
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;

            oCreationPackage.UniqueID = MENU_IMPRIMIR;
            oCreationPackage.String = MENU_IMPRIMIR_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);

            oCreationPackage.UniqueID = MENU_LEGALIZAR;
            oCreationPackage.String = MENU_LEGALIZAR_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);

            oCreationPackage.UniqueID = MENU_CANCELAR;
            oCreationPackage.String = MENU_CANCELAR_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);

            _Form.EnableMenu(MENU_IMPRIMIR, false);
            _Form.EnableMenu(MENU_LEGALIZAR, false);
            _Form.EnableMenu(MENU_CANCELAR, false);
        }

        private void EnableMenuItems()
        {
            switch ((GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Value)
            {
                case MSS_CONT.ESTADO.PENDIENTE.KEY:
                    _Form.EnableMenu(MENU_IMPRIMIR, true);
                    _Form.EnableMenu(MENU_LEGALIZAR, false);
                    _Form.EnableMenu(MENU_CANCELAR, true);
                    break;

                case MSS_CONT.ESTADO.IMPRESO.KEY:
                    _Form.EnableMenu(MENU_IMPRIMIR, false);
                    _Form.EnableMenu(MENU_LEGALIZAR, true);
                    _Form.EnableMenu(MENU_CANCELAR, true);
                    break;

                case MSS_CONT.ESTADO.RECHAZADO.KEY:
                case MSS_CONT.ESTADO.LEGALIZADO.KEY:
                default:
                    _Form.EnableMenu(MENU_IMPRIMIR, false);
                    _Form.EnableMenu(MENU_LEGALIZAR, false);
                    _Form.EnableMenu(MENU_CANCELAR, false);
                    break;
            }
        }

        private void SetEstadoAsPendiente()
        {
            (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Item.Enabled = true;
            (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Select(MSS_CONT.ESTADO.PENDIENTE.KEY);
            (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Active = false;
            (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Item.Enabled = false;
        }

        private void LoadSeries()
        {
            try
            {
                (GetItemEspecific(nameof(MSS_CONT.Series)) as ComboBox)?.ValidValues.LoadSeries(_Form.BusinessObject.Type, BoSeriesMode.sf_View);
            }
            catch (Exception ex)
            { }
        }

        private void LoadNumeration()
        {
            var cbSeries = (GetItemEspecific(nameof(MSS_CONT.Series)) as ComboBox);
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

        private dynamic GetItemEspecific(string itemName)
        {
            return _Form.Items.Item(itemName).Specific;
        }

        private bool BeforeSave()
        {
            //-------------------------------------------Valida que el documento se pueda modificar--------------------------------------------
            string currentState = (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Value.Trim();
            switch (currentState)
            {
                case MSS_CONT.ESTADO.PENDIENTE.KEY:
                case null:
                case "":
                    break;
                case MSS_CONT.ESTADO.IMPRESO.KEY:
                case MSS_CONT.ESTADO.RECHAZADO.KEY:
                case MSS_CONT.ESTADO.LEGALIZADO.KEY:
                default:
                    throw new Exception("El documento no puede ser modificado con el estado actual.");
            }


            var almacen = (GetItemEspecific(nameof(MSS_CONT.U_MSS_ADES)) as EditText).Value;
            if (string.IsNullOrEmpty(almacen))
                throw new Exception("Seleccione el almacén");

            //-------------------------Valida que el usuariotenga permiso para crear estado pendiente-------------------------------------
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_PERMISO_ALMACEN_ESTADO));
            query = query.Replace(PARAM1, GetCompany().UserName).Replace(PARAM2, almacen);
            query = query.Replace(PARAM3, nameof(MSS_CFPE.U_MSS_PEPE));
            if (DoQuery(query).RecordCount == 0)
                throw new Exception("El usuario no tiene permiso");

            //----------------------------------------------------Actualiza campos---------------------------------------------------------
            ResetItemsState();
            return true;
        }

        private void ChangeStateItemsOfGrid()
        {
            string itemCode;
            var state = OITM.ValidValues.MSS_EAAF.Reservado.ID;
            for (var i = 1; i <= GetMatrix().VisualRowCount; i++)
            {
                itemCode = GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_AFCI)).Cells.Item(i).Specific.Value;
                UpdateItem(itemCode, state);
            }
        }

        private void UpdateItem(string itemCode, string state)
        {
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_UPDATE_OITM_STATE)).Replace(PARAM1, itemCode).Replace(PARAM2, state);
            DoQuery(query);
        }

        private void ResetItemsState()
        {
            //---------------------------------Vuelve a poner disponibles todos los activos asociados a este contrato---------------------------
            string docEntry = (GetItemEspecific(nameof(MSS_CONT.DocEntry)) as EditText).Value;
            string itemCode;
            if (!string.IsNullOrEmpty(docEntry))
            {
                var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO_LINES)).Replace(PARAM1, docEntry);
                var rs = DoQuery(query);
                for (var i = 1; i <= rs.RecordCount; i++)
                {
                    itemCode = rs.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_AFCI)).Value;
                    UpdateItem(itemCode, OITM.ValidValues.MSS_EAAF.Disponible.ID);
                    rs.MoveNext();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);
            }
            //-------------------------------Marca como reservado solo los que están en la matriz----------------------------------------
            for (var i = 1; i <= GetMatrix().VisualRowCount; i++)
            {
                itemCode = GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_AFCI)).Cells.Item(i).Specific.Value;
                UpdateItem(itemCode, OITM.ValidValues.MSS_EAAF.Reservado.ID);
            }
        }

        #region Queries



        #endregion

        #region Events

        public bool HandleItemEvents(ItemEvent itemEvent)
        {
            if (itemEvent.ItemUID == nameof(MSS_CONT.Series) && itemEvent.ActionSuccess && itemEvent.EventType == BoEventTypes.et_COMBO_SELECT)
                LoadNumeration();

            if (itemEvent.ItemUID == "1")
            {
                if (itemEvent.BeforeAction)
                    return BeforeSave();
            }
            return true;
        }
        public bool HandleItemPressed(ItemEvent oEvent) { return true; }
        public bool HandleFormDataEvents(BusinessObjectInfo oBusinessObjectInfo)
        {
            switch (oBusinessObjectInfo.EventType)
            {
                case BoEventTypes.et_FORM_DATA_ADD:
                case BoEventTypes.et_FORM_DATA_UPDATE:
                    break;

                case BoEventTypes.et_FORM_DATA_LOAD:
                    if (oBusinessObjectInfo.ActionSuccess)
                        EnableMenuItems();
                    break;
            }
            return true;
        }
        public bool HandleMenuDataEvents(MenuEvent menuEvent)
        {
            switch (menuEvent.MenuUID)
            {
                case MenuUID.AddLine:
                    if (!menuEvent.BeforeAction)
                    {
                        // clean the DBDataSources associated to the matrix
                        _Form.DataSources.DBDataSources.Item(1).Clear();

                        // add the line to the matrix
                        int rowCount = GetMatrix().RowCount;
                        GetMatrix().AddRow(1, rowCount);
                        GetMatrix().SelectRow(1 + rowCount, true, false);

                        //And to delete a line:
                        /*  nq33
                        int selRow = GetMatrix().GetNextSelectedRow(0, BoOrderType.ot_SelectionOrder);
                        if (selRow == -1)
                            return false;
                        GetMatrix().DeleteRow(selRow);*/
                    }
                    break;
                case MenuUID.MenuCrear:
                    SetEstadoAsPendiente();
                    break;
                case MENU_IMPRIMIR:
                    ShowAlert("Imprimir");
                    break;
                case MENU_CANCELAR:
                    ShowAlert("Cancelar");
                    break;
                case MENU_LEGALIZAR:
                    ShowAlert("Legalizar");
                    break;
                default:
                    break;
            }

            return true;
        }
        public bool HandleRightClickEvent(ContextMenuInfo menuInfo) { return true; }

        #endregion

        #region CreateDocuments

        private bool CreateDocument(int contratoDocEntry)
        {

            SAPbobsCOM.Documents document = GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
            document.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;

            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO)).Replace(PARAM1, contratoDocEntry.ToString());
            var rs = DoQuery(query);

            var querySeries = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO)).Replace(PARAM1, contratoDocEntry.ToString());
            var rsSeries = DoQuery(querySeries);
            int serieDoc = rsSeries.Fields.Item(nameof(MSS_CFSE.U_MSS_SEEN)).Value;


            document.CardCode = rs.Fields.Item(nameof(MSS_CONT.U_MSS_CCOD)).Value;
            document.DocCurrency = rs.Fields.Item(nameof(MSS_CONT.U_MSS_MONE)).Value;
            document.Series = serieDoc;
            document.DocDate = DateTime.Now;
            document.DocDueDate = DateTime.Now;
            document.TaxDate = DateTime.Now;
            document.Indicator = ConstantHelper.DeliveryNoteDefaultInfo.Indicator;
            document.PayToCode = rs.Fields.Item(nameof(MSS_CONT.U_MSS_CDEN)).Value;
            //TODO: ? CONDICIÓN DE PAGO

            //CAMPOS DE USUARIO
            document.UserFields.Fields.Item(nameof(ODLN.U_MSSL_TOP)).Value = ConstantHelper.DeliveryNoteDefaultInfo.U_MSSL_TOP;
            document.UserFields.Fields.Item(nameof(ODLN.MSS_SECO)).Value = rs.Fields.Item(nameof(MSS_CONT.Series)).Value;
            document.UserFields.Fields.Item(nameof(ODLN.MSS_NUCO)).Value = rs.Fields.Item(nameof(MSS_CONT.DocNum)).Value;

            var queryLines = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO_LINES)).Replace(PARAM1, contratoDocEntry.ToString());
            var rsLines = DoQuery(queryLines);
            for (var i = 1; i <= rsLines.RecordCount; i++)
            {
                //DOCUMENT LINES
                document.Lines.ItemCode = rsLines.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Value;
                document.Lines.Price = 0.00;
                document.Lines.TaxCode = ConstantHelper.DeliveryNoteDefaultInfo.TaxCode;
                document.Lines.WarehouseCode = rs.Fields.Item(nameof(MSS_CONT.U_MSS_ADES)).Value;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGP)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGP;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGD)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGD;
                document.Lines.WTLiable = SAPbobsCOM.BoYesNoEnum.tNO;
                document.Lines.Add();
                rs.MoveNext();
            }


            if (document.Add() == ConstantHelper.DefaulSuccessSAPNumber)
                return true;
            return false;
        }

        #endregion

        #region Constants

        private const string MENU_LEGALIZAR = "MENU_LEGALIZAR";
        private const string MENU_LEGALIZAR_CAPTION = "Legalizar";

        private const string MENU_IMPRIMIR = "MENU_IMPRIMIR";
        private const string MENU_IMPRIMIR_CAPTION = "Imprimir";

        private const string MENU_CANCELAR = "MENU_CANCELAR";
        private const string MENU_CANCELAR_CAPTION = "Cancelar";

        #endregion
    }
}
