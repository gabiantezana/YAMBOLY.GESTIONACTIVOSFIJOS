using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbouiCOM;
using System.Collections.Generic;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT;
using System;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;
using SAPADDON.USERMODEL.Helper;
using SAPADDON.USERMODEL._FormattedSearches;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFPE;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OITM;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._ODLN;
using SAPADDON.USERMODEL._DLN1;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFSE;
using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
using SAPbobsCOM;
using System.Linq;
using SAPADDON.HELPER;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHH;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CONTForm
{
    class MSS_CONTForm : BaseApplication, ISAPForm
    {
        public const string FormType = nameof(MSS_CONT);
        private Form _Form { get; set; }

        public MSS_CONTForm()
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetResourceString(System.Reflection.Assembly.GetExecutingAssembly(), this.GetType().Name), FormType);
            GetFormOpenList().Add(_Form.UniqueID, this);
            CreateMenuAndButtons();

            LoadSeries();
            EnableMenuItems();
            SetEstadoAsPendiente();
            _Form.EnableFormatSearch();

        }

        private void CreateMenuAndButtons()
        {
            MenuCreationParams oCreationPackage = GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams);
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;

            oCreationPackage.UniqueID = MENU_IMPRIMIR;
            oCreationPackage.String = MENU_IMPRIMIR_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);

            oCreationPackage.UniqueID = MENU_LEGALIZAR;
            oCreationPackage.String = MENU_LEGALIZAR_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);

            oCreationPackage.UniqueID = MENU_RECHAZAR;
            oCreationPackage.String = MENU_RECHAZAR_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);

            oCreationPackage.UniqueID = MENU_ADDLINE_MATRIX1;
            oCreationPackage.String = MENU_ADDLINE_MATRIX1_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);

            oCreationPackage.UniqueID = MENU_ADDLINE_MATRIX2;
            oCreationPackage.String = MENU_ADDLINE_MATRIX2_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);

            oCreationPackage.UniqueID = MENU_REMOVELINE_MATRIX1;
            oCreationPackage.String = MENU_REMOVELINE_MATRIX1_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);

            oCreationPackage.UniqueID = MENU_REMOVELINE_MATRIX2;
            oCreationPackage.String = MENU_REMOVELINE_MATRIX2_CAPTION;
            _Form.Menu.AddEx(oCreationPackage);
            _Form.EnableMenu(oCreationPackage.UniqueID, false);


            #region Matrix options 


            #endregion
        }

        private void EnableMenuItems()
        {
            _Form.Items.Item("3").Visible = false;
            _Form.EnableMenu(MENU_IMPRIMIR, false);
            _Form.EnableMenu(MENU_LEGALIZAR, false);
            _Form.EnableMenu(MENU_RECHAZAR, false);

            switch ((GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Value)
            {
                case MSS_CONT.ESTADO.PENDIENTE.KEY:
                    _Form.EnableMenu(MENU_IMPRIMIR, true);
                    _Form.EnableMenu(MENU_RECHAZAR, true);
                    break;

                case MSS_CONT.ESTADO.IMPRESO.KEY:
                    _Form.EnableMenu(MENU_LEGALIZAR, true);
                    _Form.EnableMenu(MENU_RECHAZAR, true);
                    break;

                case MSS_CONT.ESTADO.RECHAZADO.KEY:
                    break;
                case MSS_CONT.ESTADO.LEGALIZADO.KEY:
                    //---------------------- Lógica para habilitar botón de Concesión------------------------ 
                    var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_RELATED_DELIVERY));
                    var series = (GetItemEspecific(nameof(MSS_CONT.Series)) as ComboBox)?.Value;
                    var docNum = (GetItemEspecific(nameof(MSS_CONT.DocNum)) as EditText)?.Value;
                    query = query.Replace(PARAM1, series)
                                .Replace(PARAM2, docNum);

                    if (DoQuery(query).RecordCount > 0)//Tiene una o más entregas asociadas
                        (_Form.Items.Item("3").Specific as Button).Caption = BUTTON_RETORNAR_CAPTION;

                    else//No tiene ninguna entrega asociada
                        (_Form.Items.Item("3").Specific as Button).Caption = BUTTON_CONCESION_CAPTION;

                    _Form.Items.Item("3").Visible = true;
                    break;
                //------------------------------------------------------------------------------------------- 
                default:
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
            return _Form.Items.Item("MATRIX1").Specific as Matrix;
        }

        private Matrix GetMatrix2()
        {
            return _Form.Items.Item("MATRIX2").Specific as Matrix;
        }

        private DBDataSource GetDataSource(int index = 0)
        {
            return _Form.DataSources.DBDataSources.Item(index);
        }

        public dynamic GetItemEspecific(string itemName)
        {
            return _Form.Items.Item(itemName).Specific;
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
                    itemCode = rs.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Value;
                    UpdateItem(itemCode, OITM.ValidValues.MSS_EAAF.Disponible.ID);
                    rs.MoveNext();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);
            }
            //-------------------------------Marca como reservado solo los que están en la matriz----------------------------------------
            for (var i = 1; i <= GetMatrix().VisualRowCount; i++)
            {
                itemCode = GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Cells.Item(i).Specific.Value;
                UpdateItem(itemCode, OITM.ValidValues.MSS_EAAF.Reservado.ID);
            }
        }

        private void ShowModal()
        {
            new MSS_CONTForm_Modal(GetFormOpenList(), this);
        }

        private void RemoveLine()
        {

        }

        private List<FixedAsset> GetItemsFromContrato(string docEntry)
        {
            var queryLines = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO_LINES)).Replace(PARAM1, docEntry);
            var rsLines = DoQuery(queryLines);

            var list = new List<FixedAsset>();
            while (!rsLines.EoF)
            {
                list.Add(new FixedAsset() { ItemCode = rsLines.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Value, State = OITM.ValidValues.MSS_EAAF.Asignado.ID });
                rsLines.MoveNext();
            }
            return list;
        }

        #region Queries

        private bool UserHasPermission(string warehouse, string state)
        {
            string columnName = null;
            switch (state)
            {
                case MSS_CONT.ESTADO.PENDIENTE.KEY:
                    columnName = nameof(MSS_CFPE.U_MSS_PEPE);
                    break;
                case MSS_CONT.ESTADO.IMPRESO.KEY:
                    columnName = nameof(MSS_CFPE.U_MSS_PEIM);
                    break;
                case MSS_CONT.ESTADO.LEGALIZADO.KEY:
                    columnName = nameof(MSS_CFPE.U_MSS_PELE);
                    break;
                case MSS_CONT.ESTADO.RECHAZADO.KEY:
                    columnName = nameof(MSS_CFPE.U_MSS_PERE);
                    break;
                default:
                    throw new Exception("No se reconoce el estado: " + state);
            }

            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_PERMISO_ALMACEN_ESTADO));
            query = query.Replace(PARAM1, GetCompany().UserName).Replace(PARAM2, warehouse);
            query = query.Replace(PARAM3, columnName);
            if (DoQuery(query).RecordCount == 0)
                return false;

            return true;
        }

        private void UpdateContState(string docEntry, string newState)
        {
            string wareHouseCode = GetItemEspecific(nameof(MSS_CONT.U_MSS_ADES)).Value;
            if (!UserHasPermission(wareHouseCode, newState)) throw new CustomException("El usuario no tiene permiso para cambiar el estado");
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_UPDATE_CONT_STATE)).Replace(PARAM1, docEntry).Replace(PARAM2, newState);
            switch (newState)
            {
                case MSS_CONT.ESTADO.PENDIENTE.KEY:
                    break;
                case MSS_CONT.ESTADO.IMPRESO.KEY:
                    DoQuery(query);
                    GetApplication().ActivateMenuItem(MenuUID.RegistroActualizar);
                    //TODO: Implementar lógica de impresión
                    break;
                case MSS_CONT.ESTADO.LEGALIZADO.KEY:
                    DoQuery(query);
                    GetApplication().ActivateMenuItem(MenuUID.RegistroActualizar);
                    break;
                case MSS_CONT.ESTADO.RECHAZADO.KEY:
                    //TODO: Implementar lógica de rechazo
                    DoQuery(query);
                    GetApplication().ActivateMenuItem(MenuUID.RegistroActualizar);
                    break;
                default:
                    break;
            }

            DoQuery(query);
        }

        #endregion

        #region Events

        public bool HandleItemEvents(ItemEvent itemEvent)
        {
            if (itemEvent.ItemUID == nameof(MSS_CONT.Series) && itemEvent.ActionSuccess && itemEvent.EventType == BoEventTypes.et_COMBO_SELECT)
                LoadNumeration();

            else if (itemEvent.ItemUID == "1" && itemEvent.BeforeAction)
                return Crear();

            else if (itemEvent.ItemUID == "3" && itemEvent.ActionSuccess)
            {
                switch (GetItemEspecific("3").Caption)
                {
                    case BUTTON_CONCESION_CAPTION:
                        CrearConcesion();
                        break;
                    case BUTTON_RETORNAR_CAPTION:
                        ShowModal();
                        break;
                    default:
                        throw new NotImplementedException();
                }
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

                case BoEventTypes.et_FORM_LOAD://TODO:
                case BoEventTypes.et_FORM_DATA_LOAD:
                case BoEventTypes.et_DATASOURCE_LOAD:
                case BoEventTypes.et_MATRIX_LOAD:
                    if (oBusinessObjectInfo.ActionSuccess)
                        EnableMenuItems();
                    break;
            }
            return true;
        }
        public bool HandleMenuDataEvents(MenuEvent menuEvent)
        {
            string itemUID = _Form.GetCurrentItemUID();
            int rowCount = 0;
            switch (menuEvent.MenuUID)
            {
                case MenuUID.MenuCrear:
                    SetEstadoAsPendiente();
                    break;
                case MENU_IMPRIMIR:
                    Imprimir();
                    break;
                case MENU_RECHAZAR:
                    Rechazar();
                    break;
                case MENU_LEGALIZAR:
                    Legalizar();
                    break;
                case MENU_ADDLINE_MATRIX1:
                    if (!menuEvent.BeforeAction)
                    {
                        _Form.DataSources.DBDataSources.Item(1).Clear();
                        rowCount = GetMatrix().RowCount;
                        GetMatrix().AddRow(1, rowCount);
                        GetMatrix().SelectRow(1 + rowCount, true, false);
                    }
                    break;
                case MENU_ADDLINE_MATRIX2:
                    if (!menuEvent.BeforeAction)
                    {
                        _Form.DataSources.DBDataSources.Item(2).Clear();
                        rowCount = GetMatrix2().RowCount;
                        GetMatrix2().AddRow(1, rowCount);
                        GetMatrix2().SelectRow(1 + rowCount, true, false);
                    }
                    break;
                case MENU_REMOVELINE_MATRIX1:
                    if (menuEvent.BeforeAction)
                    {
                        int? currentRowNumber;
                        currentRowNumber = GetMatrix().GetRowNumberFocus();
                        if (currentRowNumber != null)
                        {
                            GetMatrix().FlushToDataSource();
                            GetDataSource(1).RemoveRecord(currentRowNumber.Value - 1);
                        }

                    }
                    else
                        GetMatrix().LoadFromDataSource();
                    break;
                case MENU_REMOVELINE_MATRIX2:
                    if (menuEvent.BeforeAction)
                    {
                        int? currentRowNumber;
                        currentRowNumber = GetMatrix2().GetRowNumberFocus();
                        if (currentRowNumber != null)
                        {
                            GetMatrix2().FlushToDataSource();
                            GetDataSource(2).RemoveRecord(currentRowNumber.Value - 1);
                        }
                    }
                    else
                        GetMatrix2().LoadFromDataSource();
                    break;
                default:
                    break;
            }

            return true;
        }
        public bool HandleRightClickEvent(ContextMenuInfo menuInfo)
        {
            _Form.EnableMenu(MENU_ADDLINE_MATRIX1, false);
            _Form.EnableMenu(MENU_ADDLINE_MATRIX2, false);
            _Form.EnableMenu(MENU_REMOVELINE_MATRIX1, false);
            _Form.EnableMenu(MENU_REMOVELINE_MATRIX2, false);

            if (menuInfo.BeforeAction && (menuInfo.ItemUID == "MATRIX1" || menuInfo.ItemUID == "MATRIX2"))//&& menuInfo.Row != 0)
            {
                switch (menuInfo.ItemUID)
                {
                    case "MATRIX1":
                        _Form.EnableMenu(MENU_ADDLINE_MATRIX1, true);
                        if (GetMatrix().GetRowNumberFocus().HasValue)
                            _Form.EnableMenu(MENU_REMOVELINE_MATRIX1, true);
                        break;
                    case "MATRIX2":
                        _Form.EnableMenu(MENU_ADDLINE_MATRIX2, true);
                        if (GetMatrix2().GetRowNumberFocus().HasValue)
                            _Form.EnableMenu(MENU_REMOVELINE_MATRIX2, true);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return true;
        }

        #endregion

        #region CreateDocuments

        private bool Crear()
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

        private void Imprimir()
        {
            UpdateContState(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value, MSS_CONT.ESTADO.IMPRESO.KEY);
            ShowMessage(MessageType.Success, "El documento se imprimió correctamente.");
        }

        private void Rechazar()
        {
            string docEntry = GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value;

            GetItemsFromContrato(docEntry).ForEach(x => UpdateItem(x.ItemCode, OITM.ValidValues.MSS_EAAF.Disponible.ID));
            UpdateContState(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value, MSS_CONT.ESTADO.RECHAZADO.KEY);
            ShowMessage(MessageType.Success, "El documento se rechazó correctamente.");
        }

        private void Legalizar()
        {
            UpdateContState(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value, MSS_CONT.ESTADO.LEGALIZADO.KEY);
            ShowMessage(MessageType.Success, "El documento se legalizó correctamente.");
        }

        private void CrearConcesion()
        {
            string docEntry = GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value;
            string message = "Se procederá a realizar la concesión temporal de los activos fijos. Esta acción no se puede revertir. Desea continuar?";

            CrearDocumentos(message, BoObjectTypes.oDeliveryNotes, GetItemsFromContrato(docEntry));
            ShowMessage(MessageType.Success, "Se realizó correctamente la concesión de activos fijos.");
        }

        public void CrearRetorno(List<FixedAsset> SelectedItems)
        {
            string message = "Se procederá a realizar el retorno de los activos fijos. Esta acción no se puede revertir. Desea continuar?";

            CrearDocumentos(message, BoObjectTypes.oReturns, SelectedItems);
            ShowMessage(MessageType.Success, "Se realizó correctamente el retorno de activos fijos.");
        }

        private void CrearDocumentos(string message, BoObjectTypes boObjectType, List<FixedAsset> itemCodes)
        {
            if (GetApplication().MessageBox(message, 1, "Sí", "No") == 1)
            {
                try
                {
                    //------------------------------------Obtiene cantidad de líneas máximas por documento------------------------------------------
                    string almacen = GetItemEspecific(nameof(MSS_CONT.U_MSS_ADES)).Value as string;
                    if (string.IsNullOrEmpty(almacen))
                        throw new Exception("Error al obtener el almacén");

                    var queryString = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_MSS_CFSE)).Replace(PARAM1, almacen);
                    int? maxLines = DoQuery(queryString).Fields.Item(nameof(MSS_CFSE.U_MSS_NULI)).Value as int?;

                    var docEntry = GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value;
                    string queryLines = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO_LINES)).Replace(PARAM1, docEntry.ToString());
                    var rsLines = DoQuery(queryLines);
                    //------------------------------------------------------------------------------------------------------------------------------

                    //Inicia transacción
                    GetCompany().StartTransaction();
                    //Crea documento(s) en SAP
                    CreateDocument(boObjectType, itemCodes.Select(x => x.ItemCode).ToList(), maxLines);

                    //Actualiza estado de activos fijos
                    itemCodes.ForEach(x => UpdateItem(x.ItemCode, x.State));

                    //Finaliza transacción
                    GetCompany().EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                }
                catch (Exception)
                {
                    if (GetCompany().InTransaction)
                        GetCompany().EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    throw;
                }
            }
        }

        private void CreateDocument(BoObjectTypes boObjectType, List<string> itemCodeList, int? maxLinesPerDocument, int lineaAEmpezar = 1)
        {
            var query_ = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_RELATED_DELIVERY)).Replace(PARAM1, (GetItemEspecific(nameof(MSS_CONT.Series)) as ComboBox).Value).Replace(PARAM2, (GetItemEspecific(nameof(MSS_CONT.DocNum)) as EditText).Value);
            if (DoQuery(query_).RecordCount > 0)
                throw new Exception("Ya se generó una cesión temporal para este documento.");

            var docEntry = (GetItemEspecific(nameof(MSS_CONT.DocEntry)) as EditText).Value;
            var queryContrato = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO)).Replace(PARAM1, docEntry.ToString());
            var rsQueryContrato = DoQuery(queryContrato);

            var querySeries = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_MSS_CFSE)).Replace(PARAM1, rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_ADES)).Value);
            string series;

            switch (boObjectType)
            {
                case BoObjectTypes.oDeliveryNotes:
                    series = DoQuery(querySeries).Fields.Item(nameof(MSS_CFSE.U_MSS_SEEN)).Value as string;
                    break;
                case BoObjectTypes.oReturns:
                    series = DoQuery(querySeries).Fields.Item(nameof(MSS_CFSE.U_MSS_SEDE)).Value as string;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (string.IsNullOrEmpty(series))
                throw new Exception("No se ha definido una serie para este almacén");

            SAPbobsCOM.Documents document = GetCompany().GetBusinessObject(boObjectType);

            document.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
            document.CardCode = rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_CCOD)).Value;
            document.DocCurrency = rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_MONE)).Value;
            document.Series = Convert.ToInt32(series);
            document.DocDate = DateTime.Now;
            document.DocDueDate = DateTime.Now;
            document.TaxDate = DateTime.Now;
            document.Indicator = ConstantHelper.DeliveryNoteDefaultInfo.Indicator;
            document.PayToCode = rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_CDEN)).Value;
            //TODO: ? CONDICIÓN DE PAGO
            document.UserFields.Fields.Item(nameof(ODLN.U_MSSL_TOP)).Value = ConstantHelper.DeliveryNoteDefaultInfo.U_MSSL_TOP;
            document.UserFields.Fields.Item(nameof(ODLN.U_MSS_SERC)).Value = Convert.ToString(rsQueryContrato.Fields.Item(nameof(MSS_CONT.Series)).Value);
            document.UserFields.Fields.Item(nameof(ODLN.U_MSS_NUMC)).Value = Convert.ToString(rsQueryContrato.Fields.Item(nameof(MSS_CONT.DocNum)).Value);

            /*
            var queryLines = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO_LINES)).Replace(PARAM1, docEntry.ToString());
            var rsLines = DoQuery(queryLines);

            for (var i = 1; i < lineNumber; i++)
                rsLines.MoveNext();

            var documentLine = 0;
            while (!rsLines.EoF)
            {
                document.Lines.ItemCode = rsLines.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Value;
                document.Lines.Price = 0.00;
                document.Lines.TaxCode = ConstantHelper.DeliveryNoteDefaultInfo.TaxCode;
                document.Lines.WarehouseCode = rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_ADES)).Value;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGP)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGP;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGD)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGD;
                document.Lines.WTLiable = SAPbobsCOM.BoYesNoEnum.tNO;
                document.Lines.Add();
                rsLines.MoveNext();

                documentLine++;
                if (documentLine == maxLinesPerDocument)
                    break;
            }*/

            for (int i = 0; i < itemCodeList.Count; i++)
            {
                document.Lines.ItemCode = itemCodeList[(i + lineaAEmpezar) - 1];
                document.Lines.Price = 0.00;
                document.Lines.TaxCode = ConstantHelper.DeliveryNoteDefaultInfo.TaxCode;
                document.Lines.WarehouseCode = rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_ADES)).Value;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGP)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGP;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGD)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGD;
                document.Lines.WTLiable = SAPbobsCOM.BoYesNoEnum.tNO;

                if (i < maxLinesPerDocument)
                {
                    document.Lines.Add();
                    RegisterItemHistorial(itemCodeList[(i + lineaAEmpezar) - 1], document);
                }
                else
                {
                    CreateDocument(boObjectType, itemCodeList, maxLinesPerDocument, i + lineaAEmpezar + 1);
                    break;
                }
            }

            if (document.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                throw new SapException();
            else
            {
                //Registra historial de cada item agregado
                itemCodeList.ForEach(x => RegisterItemHistorial(x, document));
            }
        }

        private void RegisterItemHistorial(string itemCode, string itemDescription, Documents document)
        {
            var queryString = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_MSS_AFHH_UDO_BY_ITEMCODE)).Replace(PARAM1, itemCode);
            var rs = DoQuery(queryString);
            string docEntry = Convert.ToString(rs.Fields.Item(nameof(MSS_AFHH.DocEntry)));
            bool isNewRegister = string.IsNullOrEmpty(docEntry);

            var generalService = GetCompany().GetCompanyService().GetGeneralService(nameof(MSS_AFHH));
            var generalData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

            if (!isNewRegister)
            {
                var headerParams = (GeneralDataParams)GetCompany().GetCompanyService().GetGeneralService(nameof(MSS_AFHH)).GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                headerParams.SetProperty(nameof(MSS_AFHH.DocEntry), docEntry);
                generalData = generalService.GetByParams(headerParams);
            }

            generalData.SetProperty(nameof(MSS_AFHH.U_MSS_ITCO), itemCode);
            generalData.SetProperty(nameof(MSS_AFHH.U_MSS_ITDE), itemDescription);

            var oChildren = generalData.Child(nameof(MSS_AFHH_LINES));
            var oChild = oChildren.Add();
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_COAL), GetItemEspecific(nameof(MSS_CONT.U_MSS_ADES)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_COCL), GetItemEspecific(nameof(MSS_CONT.U_MSS_CCOD)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_COCO), GetItemEspecific(nameof(MSS_CONT.DocNum)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_CODI), document.ShipToCode);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_CODO), document.DocNum);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_DEUB), document.ShipToCode);//Descripción aquí 
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_FECH), DateTime.Today);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_NOCL), document.CardName);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_SECO), GetItemEspecific(nameof(MSS_CONT.Series)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_SEDO), document.Series);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_TIDO), document.Indicator);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_TIUB), MSS_AFHH_LINES.TIPOUBICACION.CLIENTE.ID);

            //Attempt to Add the Record
            if (isNewRegister)
                generalService.Add(generalData);
            else
                generalService.Update(generalData);
        }

        #endregion

        #region Constants

        private const string MENU_LEGALIZAR = "MENU_LEGALIZAR";
        private const string MENU_LEGALIZAR_CAPTION = "Legalizar";

        private const string MENU_IMPRIMIR = "MENU_IMPRIMIR";
        private const string MENU_IMPRIMIR_CAPTION = "Imprimir";

        private const string MENU_RECHAZAR = "MENU_RECHAZAR";
        private const string MENU_RECHAZAR_CAPTION = "Rechazar";

        private const string BUTTON_CONCESION_CAPTION = "Concesión";
        private const string BUTTON_RETORNAR_CAPTION = "Retorno";

        private const string MENU_ADDLINE_MATRIX1 = "MENU_ADDLINE_MATRIX1";
        private const string MENU_ADDLINE_MATRIX1_CAPTION = "Agregar línea";

        private const string MENU_ADDLINE_MATRIX2 = "MENU_ADDLINE_MATRIX2";
        private const string MENU_ADDLINE_MATRIX2_CAPTION = "Agregar línea";


        private const string MENU_REMOVELINE_MATRIX1 = "MENU_REMOVELINE_MATRIX1";
        private const string MENU_REMOVELINE_MATRIX1_CAPTION = "Eliminar línea";

        private const string MENU_REMOVELINE_MATRIX2 = "MENU_REMOVELINE_MATRIX2";
        private const string MENU_REMOVELINE_MATRIX2_CAPTION = "Eliminar línea";

        #endregion
    }
}
