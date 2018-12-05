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
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT_HIST;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CONTForm
{
    class MSS_CONTForm : BaseApplication, ISAPForm
    {
        #region Form properties
        bool IsUpdate { get; set; }
        #endregion 

        public const string FormType = nameof(MSS_CONT);
        private LoadType _LoadType { get; set; }
        private string _CodigoAdenda { get; set; }
        private Form _Form { get; set; }

        public MSS_CONTForm()
        {
            _LoadType = LoadType.Contrato;
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetResourceString(System.Reflection.Assembly.GetExecutingAssembly(), this.GetType().Name), FormType);
            GetFormOpenList().Add(_Form.UniqueID, this);
            CreateMenuAndButtons();

            LoadSeries();
            LoadForm();
            _Form.EnableFormatSearch();
            _Form.Items.Item("MATRIX2").Enabled = false;
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

        private void SetEnableAllItemsInForm(bool enable)
        {
            string[] itemsNeverChange = { nameof(MSS_CONT.DocNum), nameof(MSS_CONT.DocEntry), nameof(MSS_CONT.Series), "1", "2" };

            for (var i = 0; i < _Form.Items.Count; i++)
            {
                var item = _Form.Items.Item(i);
                if (item.Type == BoFormItemTypes.it_EDIT
                    || item.Type == BoFormItemTypes.it_COMBO_BOX
                    || item.Type == BoFormItemTypes.it_MATRIX
                    || item.Type == BoFormItemTypes.it_EXTEDIT
                    && !itemsNeverChange.Contains(item.UniqueID))
                    item.Enabled = enable;
            }
        }

        private void EnableAdendaItemsInForm(bool visible)
        {
            //Campos no mapeados en el formulario pero se emplean para guardar en el UDO de historial cuando el formulario está en tipo Adenda
            var adendaInfoItems = new string[] { nameof(MSS_CONT_HIST.U_MSS_ADEN), LABEL_CODIGOADENDA, nameof(MSS_CONT_HIST.U_MSS_FECH), LABEL_FECHAADENDA, nameof(MSS_CONT_HIST.U_MSS_COME), LABEL_COMENTARIOSADENDA };
            adendaInfoItems.ToList().ForEach(x => _Form.Items.Item(x).Visible = visible);
            adendaInfoItems.ToList().ForEach(x => _Form.Items.Item(x).Enabled = visible);

            /*
            _Form.Items.Item(BUTTON_CREARADENDA_UID).Visible = !visible;
            _Form.Items.Item(BUTTON_CESION_UID).Visible = !visible;
            _Form.Items.Item(BUTTON_RETORNO_UID).Visible = !visible;*/
        }

        private void LoadForm()
        {
            //-----------DESHABILITA TODOS LOS BOTONES CON LÓGICA DE VISIBILIDAD POR ESTADO Y TIPO------------------
            _Form.Items.Item(BUTTON_CESION_UID).Visible = false;
            _Form.Items.Item(BUTTON_RETORNO_UID).Visible = false;
            _Form.Items.Item(BUTTON_CREARADENDA_UID).Visible = false;

            _Form.EnableMenu(MENU_IMPRIMIR, false);
            _Form.EnableMenu(MENU_LEGALIZAR, false);
            _Form.EnableMenu(MENU_RECHAZAR, false);

            SetEnableAllItemsInForm(true);
            EnableAdendaItemsInForm(false);

            if (_Form.Title.Contains(TITLE_GENERAR_ADENDA))
                _Form.Title = _Form.Title.Replace(TITLE_GENERAR_ADENDA, string.Empty);
            //-----------------------------------------------------------------------------------------------------

            bool isUpdate = !string.IsNullOrEmpty(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value.ToString());
            if (isUpdate)
            {
                //Deshabilita el combo de series
                _Form.Items.Item(nameof(MSS_CONT.Series)).Enabled = false;
                _Form.Items.Item(nameof(MSS_CONT.U_MSS_TICO)).Enabled = false;

                string currentState = (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Value.Trim();
                switch (currentState)
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
                        SetEnableAllItemsInForm(false);
                        break;

                    case MSS_CONT.ESTADO.LEGALIZADO.KEY:
                        SetEnableAllItemsInForm(false);
                        //---------------------- Lógica para habilitar botón de Concesión------------------------ 
                        var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_RELATED_DELIVERY));
                        var series = (GetItemEspecific(nameof(MSS_CONT.Series)) as ComboBox)?.Value;
                        var docNum = (GetItemEspecific(nameof(MSS_CONT.DocNum)) as EditText)?.Value;
                        query = query.Replace(PARAM1, series)
                                    .Replace(PARAM2, docNum);
                        /*
                        if (DoQuery(query).RecordCount > 0)//Tiene una o más entregas asociadas
                            (_Form.Items.Item(BUTTON_RETORNO_UID).Specific as Button).Caption = BUTTON_RETORNAR_CAPTION;

                        else//No tiene ninguna entrega asociada
                            (_Form.Items.Item(BUTTON_CESION_RETORNO_UID).Specific as Button).Caption = BUTTON_CONCESION_CAPTION;*/

                        _Form.Items.Item(BUTTON_CESION_UID).Visible = true;
                        _Form.Items.Item(BUTTON_RETORNO_UID).Visible = true;
                        _Form.Items.Item(BUTTON_CREARADENDA_UID).Visible = true;
                        break;
                    //------------------------------------------------------------------------------------------- 
                    default:
                        break;
                }
            }
            else
            {
                //-----------------------------ESTADO POR DEFECTO AL CREAR CONTRATO: PENDIENTE---------------------------
                (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Item.Enabled = true;
                (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Select(MSS_CONT.ESTADO.PENDIENTE.KEY);
                (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Active = false;
                (GetItemEspecific(nameof(MSS_CONT.U_MSS_ESTA)) as ComboBox).Item.Enabled = false;
            }

            if (_LoadType == LoadType.Adenda)
            {
                _Form.Title += TITLE_GENERAR_ADENDA;

                EnableAdendaItemsInForm(true);
                //Las adendas solo pueden agregar o quitar ítems del contrato:
                _Form.Items.Item("MATRIX1").Enabled = true;
                _Form.Items.Item(BUTTON_CREARADENDA_UID).Visible = false;
                _Form.Items.Item(BUTTON_CESION_UID).Visible = false;
                _Form.Items.Item(BUTTON_RETORNO_UID).Visible = false;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">0= cabecera, 1= contLines, 2= adendas</param>
        /// <returns></returns>
        private DBDataSource GetDataSource(int index = 0)
        {
            return _Form.DataSources.DBDataSources.Item(index);
        }

        public dynamic GetItemEspecific(string itemName)
        {
            return _Form.Items.Item(itemName).Specific;
        }

        private void UpdateActivoFijoState(string itemCode, string state)
        {
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_UPDATE_OITM_STATE)).Replace(PARAM1, itemCode).Replace(PARAM2, state);
            DoQuery(query);
        }

        private void UpdateContLineState(string itemCode, string state, string docEntryContrato)
        {
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_UPDATE_CONTLINES_STATE)).Replace(PARAM1, itemCode).Replace(PARAM2, state).Replace(PARAM3, docEntryContrato);
            DoQuery(query);
        }

        private void UpdateItemsStateAndFixedAssets()
        {
            string docEntry = (GetItemEspecific(nameof(MSS_CONT.DocEntry)) as EditText).Value;
            bool isUpdate = !string.IsNullOrEmpty(docEntry);
            //-----------------------------------Recorre DataSource para identificar ítems eliminados-----------------------------------
            foreach (var item in GetItemsFromContrato(docEntry))
            {
                string itemCodeInMatrix = null;
                //Recorre matriz para buscar el ítemCode
                for (var j = 1; j <= GetMatrix().VisualRowCount; j++)
                {
                    if (item.ItemCode == GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Cells.Item(j).Specific.Value.Trim())//El ítem existe también en la matriz
                    {
                        itemCodeInMatrix = item.ItemCode;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(itemCodeInMatrix))//No se encontró en la matriz, por lo tanto fue eliminado
                    UpdateActivoFijoState(item.ItemCode, OITM.ValidValues.MSS_EAAF.Disponible.ID);
            }
            //------------------------------------------------------------------------------------------------------------------------------

            //Recorre matriz
            string itemCode, itemState;
            for (var j = 1; j <= GetMatrix().VisualRowCount; j++)
            {
                itemCode = GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Cells.Item(j).Specific.Value;
                itemState = GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_ESTD)).Cells.Item(j).Specific.Value;

                if (string.IsNullOrEmpty(itemState))//Si es una nueva línea agregada, SETEA CAMPOS POR DEFECTO
                {
                    GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_ESTD)).Cells.Item(j).Specific.Value = MSS_CONT_LINES.ESTADO.RESERVADO.KEY;
                    GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_TIPO)).Cells.Item(j).Specific.Value = MSS_CONT_LINES.TIPOREGISTRO.CONTRATOINICIAL.KEY;
                    if (_LoadType == LoadType.Adenda)
                        GetMatrix().Columns.Item(nameof(MSS_CONT_LINES.U_MSS_TIPO)).Cells.Item(j).Specific.Value = MSS_CONT_LINES.TIPOREGISTRO.ADENDA.KEY;

                    //TODO: HANDLE TRANSACTION
                    UpdateActivoFijoState(itemCode, OITM.ValidValues.MSS_EAAF.Reservado.ID);//Actualiza estado en maestro de AF.
                }
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
            if (string.IsNullOrEmpty(docEntry))
                return new List<FixedAsset>();

            var queryLines = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_CONTRATO_LINES)).Replace(PARAM1, docEntry);
            var rsLines = DoQuery(queryLines);

            var list = new List<FixedAsset>();
            while (!rsLines.EoF)
            {
                list.Add(new FixedAsset() { ItemCode = rsLines.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_AFCO)).Value, State = rsLines.Fields.Item(nameof(MSS_CONT_LINES.U_MSS_ESTD)).Value });
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

        private void OnCrearAdendaClick()
        {
            var message = "Las siguientes modificaciones que se realicen al contrato generarán una adenda. ¿Desea continuar?";
            if (GetApplication().MessageBox(message, 1, "Sí", "No") == 1)
            {
                _LoadType = LoadType.Adenda;
                LoadForm();
            }
        }
        #endregion

        #region Events

        public bool HandleItemEvents(ItemEvent itemEvent)
        {
            if (itemEvent.ItemUID == nameof(MSS_CONT.Series) && itemEvent.ActionSuccess && itemEvent.EventType == BoEventTypes.et_COMBO_SELECT)
                LoadNumeration();

            else if (itemEvent.ItemUID == "1" && itemEvent.BeforeAction)
                return CreateUpdate();

            else if (itemEvent.ItemUID == BUTTON_CREARADENDA_UID && itemEvent.ActionSuccess)
                OnCrearAdendaClick();

            else if (itemEvent.ItemUID == BUTTON_CESION_UID && itemEvent.ActionSuccess)
                CrearConcesion();

            else if (itemEvent.ItemUID == BUTTON_RETORNO_UID && itemEvent.ActionSuccess)
                ShowModal();

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
                    if (oBusinessObjectInfo.ActionSuccess)
                    {
                        _LoadType = LoadType.Contrato;
                        LoadForm();
                    }
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
                    LoadForm();
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
                        if (_Form.Items.Item("MATRIX1").Enabled)
                            _Form.EnableMenu(MENU_ADDLINE_MATRIX1, true);
                        if (GetMatrix().GetRowNumberFocus().HasValue)
                            _Form.EnableMenu(MENU_REMOVELINE_MATRIX1, true);
                        break;
                    case "MATRIX2":
                        if (_Form.Items.Item("MATRIX2").Enabled)
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

        private bool CreateUpdate()
        {
            var isUpdate = !string.IsNullOrEmpty(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value.ToString());
            //-----------------------------------------VALIDA ALMACÉN-----------------------------------------
            if (!IsUpdate)
            {

            }

            var almacen = (GetItemEspecific(nameof(MSS_CONT.U_MSS_ADES)) as EditText).Value;
            if (string.IsNullOrEmpty(almacen))
                throw new Exception("Seleccione el almacén");

            //---------------Valida que el usuariotenga permiso para crear estado pendiente--------------------------
            var query = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_PERMISO_ALMACEN_ESTADO));
            query = query.Replace(PARAM1, GetCompany().UserName).Replace(PARAM2, almacen);
            query = query.Replace(PARAM3, nameof(MSS_CFPE.U_MSS_PEPE));
            if (DoQuery(query).RecordCount == 0)
                throw new Exception("El usuario no tiene permiso");

            //------------------------------VALIDA NÚMERO DE FILAS AL CREAR-----------------------------------------------------
            if (!isUpdate && GetMatrix().RowCount <= 0)
                throw new Exception("Agregue al menos un ítem al detalle.");

            //----------------------------Valida que el documento se pueda modificar--------------------------------
            if (isUpdate)
            {
                if (_LoadType == LoadType.Contrato)
                {
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
                }
                else if (_LoadType == LoadType.Adenda)
                {
                    if (string.IsNullOrEmpty(GetItemEspecific(nameof(MSS_CONT_HIST.U_MSS_ADEN)).Value.Trim()))
                        throw new Exception("Ingrese el código de adenda");

                    if (string.IsNullOrEmpty(GetItemEspecific(nameof(MSS_CONT_HIST.U_MSS_FECH)).Value.Trim()))
                        throw new Exception("Ingrese la fecha de adenda");

                    var message = "Se generará una adenda a este contrato. Esta acción es irreversible. ¿Desea continuar?";
                    if (GetApplication().MessageBox(message, 1, "Sí", "No") != 1)
                        return false;
                }
            }

            //---------------------------Actualiza estados de activos fijos y detalle de Contrato--------------------------
            UpdateItemsStateAndFixedAssets();

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

            GetItemsFromContrato(docEntry).ForEach(x => UpdateContLineState(x.ItemCode, OITM.ValidValues.MSS_EAAF.Disponible.ID, docEntry));
            UpdateContState(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value, MSS_CONT.ESTADO.RECHAZADO.KEY);

            var contratoItems = GetItemsFromContrato(docEntry);
            contratoItems.ForEach(x => UpdateActivoFijoState(x.ItemCode, OITM.ValidValues.MSS_EAAF.Disponible.ID));

            //Guarda en la tabla de modificaciones legales
            //TODO:
            ShowMessage(MessageType.Success, "El documento se rechazó correctamente.");
        }

        private void Legalizar()
        {
            UpdateContState(GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value, MSS_CONT.ESTADO.LEGALIZADO.KEY);
            //TODO: GUARDA HISTORIAL DE CONTRATO
            ShowMessage(MessageType.Success, "El documento se legalizó correctamente.");
        }

        private void CrearConcesion()
        {
            string docEntry = GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value;
            string message = "Se procederá a realizar la concesión temporal de los activos fijos. Esta acción no se puede revertir. Desea continuar?";

            var pendingToConcesion = GetItemsFromContrato(docEntry).Where(x => x.State == MSS_CONT_LINES.ESTADO.RESERVADO.KEY).ToList();
            if (pendingToConcesion.Count() == 0)
                throw new CustomException("No existen ítems pendientes de concesión");
            try
            {
                if (GetApplication().MessageBox(message, 1, "Sí", "No") == 1)
                {
                    GetCompany().StartTransaction();

                    CrearDocumentos(BoObjectTypes.oDeliveryNotes, pendingToConcesion);
                    pendingToConcesion.ForEach(x => UpdateContLineState(x.ItemCode, MSS_CONT_LINES.ESTADO.ENCONCESION.KEY, docEntry));
                    pendingToConcesion.ForEach(x => UpdateActivoFijoState(x.ItemCode, OITM.ValidValues.MSS_EAAF.Asignado.ID));
                    GetCompany().EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);

                    ShowMessage(MessageType.Success, "Se realizó correctamente la concesión de activos fijos.");
                }
            }
            catch (Exception)
            {
                if (GetCompany().InTransaction)
                    GetCompany().EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                throw;
            }
        }

        public void CrearRetorno(List<FixedAsset> SelectedItems)
        {
            string docEntry = GetItemEspecific(nameof(MSS_CONT.DocEntry)).Value;
            string message = "Se procederá a realizar el retorno de los activos fijos. Esta acción no se puede revertir. Desea continuar?";

            try
            {
                if (GetApplication().MessageBox(message, 1, "Sí", "No") == 1)
                {
                    GetCompany().StartTransaction();

                    CrearDocumentos(BoObjectTypes.oReturns, SelectedItems);
                    SelectedItems.ForEach(x => UpdateContLineState(x.ItemCode, MSS_CONT_LINES.ESTADO.RETORNADO.KEY, docEntry));
                    SelectedItems.ForEach(x => UpdateActivoFijoState(x.ItemCode, x.State));
                    GetCompany().EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    ShowMessage(MessageType.Success, "Se realizó correctamente el retorno de activos fijos.");
                }
            }
            catch (Exception)
            {
                if (GetCompany().InTransaction)
                    GetCompany().EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                throw;
            }
        }

        private void CrearDocumentos(BoObjectTypes boObjectType, List<FixedAsset> itemCodes)
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

            //Crea documento(s) en SAP
            CreateDocument(boObjectType, itemCodes.Select(x => x.ItemCode).ToList(), maxLines);

        }

        private void CreateDocument(BoObjectTypes boObjectType, List<string> itemCodeList, int? maxLinesPerDocument, int indexAEmpezar = 0)
        {
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


            for (int i = 0; i < itemCodeList.Skip(indexAEmpezar).ToList().Count; i++)
            {
                document.Lines.ItemCode = itemCodeList[indexAEmpezar];
                document.Lines.Price = 0.00;
                document.Lines.TaxCode = ConstantHelper.DeliveryNoteDefaultInfo.TaxCode;
                document.Lines.WarehouseCode = rsQueryContrato.Fields.Item(nameof(MSS_CONT.U_MSS_ADES)).Value;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGP)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGP;
                document.Lines.UserFields.Fields.Item(nameof(DLN1.U_MSSL_CGD)).Value = ConstantHelper.DeliveryNoteDefaultInfo.Lines.U_MSSL_CGD;
                document.Lines.WTLiable = SAPbobsCOM.BoYesNoEnum.tNO;

                if (i < maxLinesPerDocument)
                {
                    indexAEmpezar++;
                    document.Lines.Add();
                }
                else
                {
                    CreateDocument(boObjectType, itemCodeList, maxLinesPerDocument, indexAEmpezar);
                    break;
                }
            }

            if (document.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                throw new SapException();
            else
            {
                //Registra historial de cada item agregado
                itemCodeList.ForEach(x => RegisterItemHistorial(x, GetCompany().GetNewObjectKey(), boObjectType));
            }
        }

        private void RegisterItemHistorial(string itemCode, string lastDocEntry, BoObjectTypes objectType)
        {
            int? docEntryHistorial = null;
            int docEntryDocumento = Convert.ToInt32(lastDocEntry);
            Documents document = GetCompany().GetBusinessObject(objectType);
            if (!document.GetByKey(docEntryDocumento))
                throw new CustomException("Ocurrió un error al cargar el documento. DocEntry: " + docEntryDocumento);

            var queryString = FileHelper.GetResourceString(nameof(Queries.MSS_QS_GET_MSS_AFHH_UDO_BY_ITEMCODE)).Replace(PARAM1, itemCode);

            var rs = DoQuery(queryString);
            if (rs.RecordCount > 0)
                docEntryHistorial = rs.Fields.Item(nameof(MSS_AFHH.DocEntry)).Value;

            bool isNewRegister = docEntryHistorial == null;

            var generalService = GetCompany().GetCompanyService().GetGeneralService(nameof(MSS_AFHH));
            var generalData = (GeneralData)generalService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

            if (!isNewRegister)
            {
                var headerParams = (GeneralDataParams)GetCompany().GetCompanyService().GetGeneralService(nameof(MSS_AFHH)).GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);
                headerParams.SetProperty(nameof(MSS_AFHH.DocEntry), docEntryHistorial);
                generalData = generalService.GetByParams(headerParams);
            }

            generalData.SetProperty(nameof(MSS_AFHH.U_MSS_ITCO), itemCode);
            generalData.SetProperty(nameof(MSS_AFHH.U_MSS_ITDE), itemCode);

            var oChildren = generalData.Child(nameof(MSS_AFHH_LINES));
            var oChild = oChildren.Add();
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_COAL), GetItemEspecific(nameof(MSS_CONT.U_MSS_ADES)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_COCL), GetItemEspecific(nameof(MSS_CONT.U_MSS_CCOD)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_COCO), GetItemEspecific(nameof(MSS_CONT.DocNum)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_CODI), document.ShipToCode);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_CODO), document.DocNum.ToString());
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_DEUB), document.ShipToCode);//Descripción aquí 
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_FECH), DateTime.Today);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_NOCL), document.CardName);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_SECO), GetItemEspecific(nameof(MSS_CONT.Series)).Value);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_SEDO), document.Series.ToString());
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_TIDO), document.Indicator);
            oChild.SetProperty(nameof(MSS_AFHH_LINES.U_MSS_TIUB), MSS_AFHH_LINES.TIPOUBICACION.CLIENTE.ID);

            //Attempt to Add the Record
            if (isNewRegister)
                generalService.Add(generalData);
            else
                generalService.Update(generalData);
        }

        private void SaveAsAdenda()
        {


        }

        #endregion

        #region Constants

        private const string MENU_LEGALIZAR = "MENU_LEGALIZAR";
        private const string MENU_LEGALIZAR_CAPTION = "Legalizar";

        private const string MENU_IMPRIMIR = "MENU_IMPRIMIR";
        private const string MENU_IMPRIMIR_CAPTION = "Imprimir";

        private const string MENU_RECHAZAR = "MENU_RECHAZAR";
        private const string MENU_RECHAZAR_CAPTION = "Rechazar";

        private const string BUTTON_CESION_UID = "BTNCESION";
        private const string BUTTON_RETORNO_UID = "BTNRETORNO";

        private const string MENU_ADDLINE_MATRIX1 = "MENU_ADDLINE_MATRIX1";
        private const string MENU_ADDLINE_MATRIX1_CAPTION = "Agregar línea";

        private const string MENU_ADDLINE_MATRIX2 = "MENU_ADDLINE_MATRIX2";
        private const string MENU_ADDLINE_MATRIX2_CAPTION = "Agregar línea";


        private const string MENU_REMOVELINE_MATRIX1 = "MENU_REMOVELINE_MATRIX1";
        private const string MENU_REMOVELINE_MATRIX1_CAPTION = "Eliminar línea";

        private const string MENU_REMOVELINE_MATRIX2 = "MENU_REMOVELINE_MATRIX2";
        private const string MENU_REMOVELINE_MATRIX2_CAPTION = "Eliminar línea";

        private const string ITEM_ADENDASTAB_UID = "ADENDASTAB";
        //private const string ITEM_ADENDATAB_UID = "ADENDATAB";
        private const string BUTTON_CREARADENDA_UID = "BTNADENDA";

        private const string TITLE_GENERAR_ADENDA = " Creación de adenda";

        private const string LABEL_CODIGOADENDA = "L_MSS_ADEN";
        private const string LABEL_FECHAADENDA = "L_MSS_FECH";
        private const string LABEL_COMENTARIOSADENDA = "L_MSS_COME";
        #endregion

        private enum LoadType
        {
            Contrato = 1,
            Adenda = 2,
        }
    }
}
