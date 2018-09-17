using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_APROForm;
using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CONFForm;
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_DESP;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_VEHIC;
using SAPbouiCOM;
using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OHEM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_DESPForm
{
    class MSS_DESPForm : BaseApplication, ISAPForm
    {
        private Form _Form { get; set; }
        public Form GetForm() => _Form;
        public static string GetFormName() => "Planificación de despachos";

        private string lastDate { get; set; }
        private EstadoDespacho estadoDespacho { get; set; }

        public MSS_DESPForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(Assembly.GetExecutingAssembly(), EmbebbedFileName.MSS_DESPForm), FormID.MSS_DESP.IdToString());
            if (_Form != null)
            {
                dictionary.Add(_Form.UniqueID, this);
                _Form.Mode = BoFormMode.fm_ADD_MODE;

                _Form.Freeze(true);

                LoadSeries();
                FillChooseFromList();
                DisableItems();

                _Form.Freeze(false);
                _Form.Visible = true;
            }
        }

        #region Handle Events


        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            _Form = GetApplication().Forms.GetForm(itemEvent.FormTypeEx, itemEvent.FormTypeCount);
            //_Form = GetActiveForm();

            if (GetForm().Mode == BoFormMode.fm_FIND_MODE)
                return true;

            if (itemEvent.ItemUID == FormItemIds.txtFecha.IdToString()) //Si el usuario cambia la fecha, se limpia el detalle del udo.
            {
                if (itemEvent.BeforeAction)
                    lastDate = (GetForm().Items.Item(FormItemIds.txtFecha.IdToString()).Specific as EditText).Value;

                if (itemEvent.ActionSuccess)
                {
                    var newDate = (GetForm().Items.Item(FormItemIds.txtFecha.IdToString()).Specific as EditText).Value;
                    if (lastDate != newDate)
                    {
                        ClearMatrix();
                    }
                }
            }

            if (itemEvent.ItemUID == FormItemIds.btnSave.IdToString() && itemEvent.BeforeAction && itemEvent.EventType == BoEventTypes.et_ITEM_PRESSED)
            {
                return BeforeSave();
            }

            if (itemEvent.ItemUID == FormItemIds.btnSave.IdToString() && itemEvent.ActionSuccess)
            {
                AfterSave();
                return true;
            }

            if (itemEvent.ItemUID == FormItemIds.btnAddDetail.IdToString() && itemEvent.ActionSuccess)
            {

                var fechaDespacho = ((EditText)GetForm().Items.Item(MSS_DESPForm.FormItemIds.txtFecha.IdToString()).Specific).Value;
                if (string.IsNullOrEmpty(fechaDespacho))
                    throw new CustomException("Seleccione una fecha");

                new MSS_DESP_LISTForm(GetFormOpenList(), GetApplication().Forms.ActiveForm);
                return true;
            }

            if (itemEvent.EventType == BoEventTypes.et_CHOOSE_FROM_LIST)
            {
                return HandleChooseFromListEvents(itemEvent);
            }

            if (itemEvent.ItemUID == FormItemIds.cbSerie.IdToString() && itemEvent.ActionSuccess && itemEvent.EventType == BoEventTypes.et_COMBO_SELECT)
            {
                LoadNumeration();
                return true;
            }

            if (itemEvent.ColUID == FormColumIds.CantidadADespachar.IdToString() && itemEvent.EventType == BoEventTypes.et_VALIDATE)
            {
                var itsCorrect = ValidateQuantityDeliveredCell(GetForm());
                if (itsCorrect)
                    SumarYValidarMontosFinales();
                return itsCorrect;
            }

            return true;
        }

        public bool HandleChooseFromListEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            _Form = GetApplication().Forms.GetForm(itemEvent.FormTypeEx, itemEvent.FormTypeCount);

            if (GetForm().Mode == BoFormMode.fm_FIND_MODE)
                return true;

            if (itemEvent.ItemUID == FormItemIds.txtCodigoVehiculo.IdToString() && itemEvent.ActionSuccess)
            {
                var selectedItem = GetSelectedItemFromChooseFromList(itemEvent);
                FillVehicleData(GetApplication().Forms.GetForm(itemEvent.FormType.ToString(), itemEvent.FormTypeCount), selectedItem);
                return true;
            }

            if (itemEvent.ItemUID == FormItemIds.txtConductor.IdToString() && itemEvent.ActionSuccess)
            {
                var selectedItem = GetSelectedItemFromChooseFromList(itemEvent);
                FillConductorData(GetApplication().Forms.GetForm(itemEvent.FormType.ToString(), itemEvent.FormTypeCount), selectedItem);
            }
            return true;
        }

        public bool HandleItemPressed(SAPbouiCOM.ItemEvent oEvent) { return true; }

        public bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo)
        {
            _Form = GetApplication().Forms.Item(oBusinessObjectInfo.FormUID);

            if (GetForm().Mode == BoFormMode.fm_FIND_MODE)
                return true;

            if (oBusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD && oBusinessObjectInfo.ActionSuccess)
            {
                if (GetForm().Items.Item(FormItemIds.txtEstado.IdToString()).Specific.Value != ConstantHelper.EstadoPlanificacionDespacho.Aprobado)
                {
                    SumarYValidarMontosFinales();
                }
            }

            return true;
        }

        public bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent)
        {
            _Form = GetActiveForm();

            if (GetForm().Mode == BoFormMode.fm_FIND_MODE)
                return true;

            if (menuEvent.MenuUID == MenuUID.RemoveLine.IdToString() && !menuEvent.BeforeAction)
            {
                GetMatrix().FlushToDataSource();
                SumarYValidarMontosFinales();
            }

            return true;
        }

        public bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo)
        {
            return true;
        }

        #endregion

        private void DisableItems()
        {

            GetForm().Items.Item(FormItemIds.txtDiferenciaPeso.IdToString()).Visible = false;
            GetForm().Items.Item(FormItemIds.txtDiferenciaPeso.IdToString()).Specific.Value = null;

            GetForm().Items.Item(FormItemIds.txtDiferenciaVolumen.IdToString()).Visible = false;
            GetForm().Items.Item(FormItemIds.txtDiferenciaVolumen.IdToString()).Specific.Value = null;

            GetForm().Items.Item(FormItemIds.txtDiferenciaNumeroRepartos.IdToString()).Visible = false;
            GetForm().Items.Item(FormItemIds.txtDiferenciaNumeroRepartos.IdToString()).Specific.Value = null;

            GetMatrix().Columns.Item(FormColumIds.CantidadADespachar.IdToString()).Editable = true;

            GetForm().EnableMenu(MenuUID.AddLine.IdToString(), true);
            GetForm().EnableMenu(MenuUID.RemoveLine.IdToString(), true);
        }

        private void SetMenuButtons()
        {
            _Form.EnableMenu(MenuUID.RegistroDatosSiguiente.IdToString(), true);
            _Form.EnableMenu(MenuUID.RegistroDatosAnterior.IdToString(), true);
            _Form.EnableMenu(MenuUID.RegistroDatosPrimero.IdToString(), false);
            _Form.EnableMenu(MenuUID.RegistroDatosUltimo.IdToString(), false);
        }

        private int? GetDocEntryFromXML(string xml)
        {
            var xDoc = new XmlDocument();

            xDoc.LoadXml(xml);
            var docEntry = xDoc.InnerText;
            if (!string.IsNullOrEmpty(docEntry))
                return Convert.ToInt32(docEntry);
            else
                return null;
        }


        internal static SAPbouiCOM.DataTable GetDataTableFromCFL(SAPbouiCOM.Form oForm, SAPbouiCOM.ItemEvent oEvent)
        {
            var selectedIndex = ((SAPbouiCOM.IChooseFromListEvent)oEvent).Row;
            var dt = ((SAPbouiCOM.IChooseFromListEvent)oEvent).SelectedObjects;
            var adsd = dt.GetValue(0, selectedIndex);
            return ((SAPbouiCOM.IChooseFromListEvent)oEvent).SelectedObjects;
        }

        private void FillChooseFromList()
        {

            SAPbouiCOM.ChooseFromListCollection chooseFromListCollection = _Form.ChooseFromLists;

            //--------------------------------CFL VEHÍCULOS-----------------------------------------
            var chooseFromList = chooseFromListCollection.Item(FormItemIds.cflVehiculo.IdToString());
            var conditions = chooseFromList.GetConditions();
            var condition = conditions.Add();
            condition.Alias = new MSS_VEHI().GetMemberName(x => x.MSS_ESTA);
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = ConstantHelper.EstadoVehiculo.Disponible;
            chooseFromList.SetConditions(conditions);

            //--------------------------------CFL CONDUCTORES-----------------------------------------
            chooseFromList = chooseFromListCollection.Item(FormItemIds.cflConductor.IdToString());
            conditions = chooseFromList.GetConditions();
            condition = conditions.Add();
            condition.Alias = new OHEM().GetMemberName(x => x.MSS_COND);
            condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            condition.CondVal = ConstantHelper.SAP_YES_NO.YES;
            chooseFromList.SetConditions(conditions);

            //--------------------------------CFL ALMACENES-----------------------------------------
            chooseFromList = chooseFromListCollection.Item(FormItemIds.cflAlmacen.IdToString());
            var almacenesAutorizadosDeUsuario = GetListAlmacenesAutorizadosDeUsuario();

            for (var i = 0; i <= almacenesAutorizadosDeUsuario.Count; i++)
            {
                conditions = chooseFromList.GetConditions();
                condition = conditions.Add();
                condition.Alias = "WhsCode";
                condition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                condition.CondVal = almacenesAutorizadosDeUsuario.Count == 0
                    ? new Guid().GetHashCode().ToString() //Artificio para no traer almacenes si el usuario no tiene ninguno.
                    : almacenesAutorizadosDeUsuario[i];

                //Mientras no sea el último ítem de la lista, se agrega la condicional OR. :p
                if (i != almacenesAutorizadosDeUsuario.Count - 1)
                {
                    condition.Relationship = BoConditionRelationship.cr_OR;
                    chooseFromList.SetConditions(conditions);
                }
                else
                {
                    chooseFromList.SetConditions(conditions);
                    break;
                }
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(chooseFromList);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(chooseFromListCollection);
        }

        private List<string> GetListAlmacenesAutorizadosDeUsuario()
        {
            var almacenesAutorizados = new List<string>();
            var queryString = GetQueryString(EmbebbedFileName.MSS_ASUA_UDO_GetListByUser).Replace(ConstantHelper.PARAM1, GetApplication().Company.UserName);
            var rs = DoQuery(queryString);

            if (!(rs?.RecordCount > 0)) return almacenesAutorizados;

            for (var i = 1; i <= rs.RecordCount; i++)
            {
                almacenesAutorizados.Add(rs.Fields.Item(2).Value);
                rs.MoveNext();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);

            return almacenesAutorizados;
        }

        private void LoadSeries()
        {
            try
            {
                var cbSeries = ((ComboBox)(_Form.Items.Item(FormItemIds.cbSerie.IdToString()).Specific));
                cbSeries.ValidValues.LoadSeries(_Form.BusinessObject.Type, BoSeriesMode.sf_View);
            }
            catch { }
        }

        private void LoadNumeration()
        {
            var form = GetApplication().Forms.ActiveForm;
            var cbSeries = ((ComboBox)(form.Items.Item(FormItemIds.cbSerie.IdToString()).Specific));

            var txtNum = ((EditText)(form.Items.Item(FormItemIds.txtNumero.IdToString()).Specific));
            //txtNum.Value = form.BusinessObject.GetNextSerialNumber(cbSeries.Selected?.Value, form.BusinessObject.Type).ToSafeString();
            GetDataSource().SetValue(new MSS_DESP().GetMemberName(x => x.DocNum, false), 0, form.BusinessObject.GetNextSerialNumber(cbSeries.Selected?.Value, form.BusinessObject.Type).ToSafeString());
            GetForm().Refresh();
        }

        public MSS_DESP_LINES GetLineFromDB(string concatenedDocEntryAndLineNumeOfOrder)
        {
            var columnValues = new List<string>();
            var query = GetQueryString(EmbebbedFileName.RDR1_GetItem)
                                        .Replace(ConstantHelper.PARAM1, concatenedDocEntryAndLineNumeOfOrder.Split('-')[0])
                                        .Replace(ConstantHelper.PARAM2, concatenedDocEntryAndLineNumeOfOrder.Split('-')[1]);

            MSS_DESP_LINES item = new MSS_DESP_LINES(); ;
            var rs = DoQuery(query);
            if (rs.RecordCount > 0)
            {

                item.MSS_DOCE = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_DOCE)).Value);
                item.MSS_ORDE = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_ORDE)).Value);
                item.MSS_LINE = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_LINE)).Value);
                item.MSS_CODC = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_CODC)).Value);
                item.MSS_NOMB = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_NOMB)).Value);
                item.MSS_IDDI = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_IDDI)).Value);
                item.MSS_DIRE = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_DIRE)).Value);
                item.MSS_CODA = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_CODA)).Value);
                item.MSS_DESC = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_DESC)).Value);
                item.MSS_UNID = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_UNID)).Value);
                item.MSS_CANP = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_CANP)).Value);
                item.MSS_CANA = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_CANA)).Value);
                item.MSS_CAND = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_CAND)).Value);
                item.MSS_PEST = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_PEST)).Value);
                item.MSS_VOLT = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_VOLT)).Value);

            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);
            return item;

        }


        public void AddItemsToMatrix(string[] internalCodes)
        {
            //Artificio para quitarle el focus a la cantidad a despachar.
            GetForm().Items.Item(FormItemIds.txtFecha.IdToString()).Click(BoCellClickType.ct_Regular);

            GetMatrix().Columns.Item(FormColumIds.CantidadADespachar.IdToString()).Editable = false;

            var rowList = new List<MSS_DESP_LINES>();
            foreach (var item in internalCodes)
            {
                if (!ItemWasAddedToTable(item))
                    rowList.Add(GetLineFromDB(item));
            }

            foreach (var item in rowList)
            {
                GetMatrix().AddRow();
                (GetMatrix().Columns.Item(0).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = null;
                (GetMatrix().Columns.Item(FormColumIds.OrderDocEntry.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_DOCE;
                (GetMatrix().Columns.Item(FormColumIds.OrderNum.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_ORDE;
                (GetMatrix().Columns.Item(FormColumIds.OrderLineNumRef.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_LINE;
                (GetMatrix().Columns.Item(FormColumIds.CardCodeClient.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_CODC;
                (GetMatrix().Columns.Item(FormColumIds.CardNameClient.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_NOMB;
                (GetMatrix().Columns.Item(FormColumIds.IdDireccionEntrega.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_IDDI;
                (GetMatrix().Columns.Item(FormColumIds.DetalleDireccion.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_DIRE;
                (GetMatrix().Columns.Item(FormColumIds.CodigoArticulo.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_CODA;
                (GetMatrix().Columns.Item(FormColumIds.DescripcionArticulo.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_DESC;
                (GetMatrix().Columns.Item(FormColumIds.UnidadMedidaArticulo.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_UNID;
                (GetMatrix().Columns.Item(FormColumIds.CantidadPendiente.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_CANP;
                (GetMatrix().Columns.Item(FormColumIds.CantidadAlmacen.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_CANA;
                (GetMatrix().Columns.Item(FormColumIds.CantidadADespachar.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_CAND;
                (GetMatrix().Columns.Item(FormColumIds.PesoUND.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_PEST;
                (GetMatrix().Columns.Item(FormColumIds.VolumenUND.IdToString()).Cells.Item(GetMatrix().VisualRowCount).Specific).Value = item.MSS_VOLT;
            }

            SumarYValidarMontosFinales();
            GetMatrix().Columns.Item(FormColumIds.CantidadADespachar.IdToString()).Editable = true;

            GetMatrix().FlushToDataSource();
            GetMatrix().AutoResizeColumns();

            if (GetForm().Mode != BoFormMode.fm_ADD_MODE)
                GetForm().Mode = BoFormMode.fm_UPDATE_MODE;

        }

        [Obsolete]
        public void AddRowToMatrix(MSS_DESP_LINES row, int indexToWrite)
        {
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.LineId), indexToWrite, (indexToWrite + 1).ToString());
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_DOCE), indexToWrite, row.MSS_DOCE);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_ORDE), indexToWrite, row.MSS_ORDE);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_LINE), indexToWrite, row.MSS_LINE);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_CODC), indexToWrite, row.MSS_CODC);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_NOMB), indexToWrite, row.MSS_NOMB);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_IDDI), indexToWrite, row.MSS_IDDI);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_DIRE), indexToWrite, row.MSS_DIRE);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_CODA), indexToWrite, row.MSS_CODA);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_CANA), indexToWrite, row.MSS_CANA);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_CAND), indexToWrite, row.MSS_CAND);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_CANP), indexToWrite, row.MSS_CANP);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_DESC), indexToWrite, row.MSS_DESC);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_PEST), indexToWrite, row.MSS_PEST);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_UNID), indexToWrite, row.MSS_UNID);
            GetDataSourceDetail().SetValue(row.GetMemberName(x => x.MSS_VOLT), indexToWrite, row.MSS_VOLT);



        }

        public bool ItemWasAddedToTable(string internalCode)
        {
            var matrix = ((Matrix)GetApplication().Forms.ActiveForm.Items.Item(FormItemIds.matrix.IdToString()).Specific);
            for (var i = 1; i <= GetMatrix().VisualRowCount; i++)
            {
                var docEntry = matrix.Columns.Item(FormColumIds.OrderDocEntry.IdToString()).Cells.Item(i).Specific.Value;
                var lineNumRef = matrix.Columns.Item(FormColumIds.OrderLineNumRef.IdToString()).Cells.Item(i).Specific.Value;
                var _internalCode = docEntry + "-" + lineNumRef;
                if (_internalCode == internalCode)
                    return true;
            }

            return false;
        }

        private double MultiplicarValoresPorCantidadDeArticulos(int columnIndexAMultiplicar)
        {
            var matrix = GetForm().Items.Item(FormItemIds.matrix.IdToString()).Specific as Matrix;
            double total = 0;
            for (var i = 1; i < GetMatrix().VisualRowCount + 1; i++)
            {
                string stringValueCantidadArticulos = Convert.ToString(GetMatrix().Columns.Item(FormColumIds.CantidadADespachar.IdToString()).Cells.Item(i).Specific.Value);
                var cantidadDeArticulos = double.Parse(stringValueCantidadArticulos.Replace(",", ""), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                string valorAMultiplicarString = Convert.ToString(GetMatrix().Columns.Item(columnIndexAMultiplicar).Cells.Item(i).Specific.Value);
                double valorAMultiplicarDouble = double.Parse(valorAMultiplicarString.Replace(",", ""), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);

                total += (cantidadDeArticulos * valorAMultiplicarDouble);
            }

            return total;
        }

        public void SumarYValidarMontosFinales()
        {
            double min, max, total;
            int columnIndexToSum;
            Item txtDiferencia;
            EditText txtTotal;

            #region Sumar y validar volumen

            columnIndexToSum = GetMatrix().Columns.Count - 1;
            Double.TryParse((GetForm().Items.Item(FormItemIds.txtCapacidadMinVolumen.IdToString()).Specific as EditText).Value, out min);
            Double.TryParse((GetForm().Items.Item(FormItemIds.txtCapacidadMaxVolumen.IdToString()).Specific as EditText).Value, out max);
            total = MultiplicarValoresPorCantidadDeArticulos(columnIndexToSum);
            txtDiferencia = GetForm().Items.Item(FormItemIds.txtDiferenciaVolumen.IdToString());
            txtTotal = GetForm().Items.Item(FormItemIds.txtVolumenTotal.IdToString()).Specific as EditText;

            if (total < min)
            {
                (txtDiferencia.Specific as EditText).Value = "Faltan " + (min - total).ToString("#0.00") + " para llegar al volumen mínimo";
                txtDiferencia.Visible = true;
            }
            else if (total > max)
            {
                (txtDiferencia.Specific as EditText).Value = "Excede en " + (total - max).ToString("#0.00") + " al volumen máximo";
                txtDiferencia.Visible = true;
            }
            else
            {
                (txtDiferencia.Specific as EditText).Value = null;
                txtDiferencia.Visible = false;
            }

            txtTotal.Value = total.ToString("#0.00");

            #endregion

            #region Sumar y validar Peso

            columnIndexToSum -= 1;
            Double.TryParse((GetForm().Items.Item(FormItemIds.txtCapacidadMinCarga.IdToString()).Specific as EditText).Value, out min);
            Double.TryParse((GetForm().Items.Item(FormItemIds.txtCapacidadMaxCarga.IdToString()).Specific as EditText).Value, out max);
            total = MultiplicarValoresPorCantidadDeArticulos(columnIndexToSum);
            txtDiferencia = GetForm().Items.Item(FormItemIds.txtDiferenciaPeso.IdToString());
            txtTotal = GetForm().Items.Item(FormItemIds.txtPesoTotal.IdToString()).Specific as EditText;

            if (total < min)
            {
                (txtDiferencia.Specific as EditText).Value = "Faltan " + (min - total).ToString("#0.00") + " para llegar a la carga mínima";
                txtDiferencia.Visible = true;
            }
            else if (total > max)
            {
                (txtDiferencia.Specific as EditText).Value = "Excede en " + (total - max).ToString("#0.00") + " a la carga máxima";
                txtDiferencia.Visible = true;
            }
            else
            {
                (txtDiferencia.Specific as EditText).Value = null;
                txtDiferencia.Visible = false;
            }

            txtTotal.Value = total.ToString("#0.00");
            //-------------------------------------------------------Cantidad direcciones -------------------------------------------------------

            #endregion

            #region Sumar y validar direcciones

            Double.TryParse((GetForm().Items.Item(FormItemIds.txtNumeroMinRepartos.IdToString()).Specific as EditText).Value, out min);
            Double.TryParse((GetForm().Items.Item(FormItemIds.txtNumeroMaxRepartos.IdToString()).Specific as EditText).Value, out max);
            total = GetCantidadDeDireccionesADespachar();

            txtDiferencia = GetForm().Items.Item(FormItemIds.txtDiferenciaNumeroRepartos.IdToString());
            txtTotal = GetForm().Items.Item(FormItemIds.txtDireccionesTotal.IdToString()).Specific as EditText;

            if (total < min)
            {
                (txtDiferencia.Specific as EditText).Value = "Faltan " + (min - total) + " para llegar al n. repartos mínimo";
                txtDiferencia.Visible = true;
            }
            else if (total > max)
            {
                (txtDiferencia.Specific as EditText).Value = "Excede en " + (total - max) + " al n. repartos máximo";
                txtDiferencia.Visible = true;
            }
            else
            {
                (txtDiferencia.Specific as EditText).Value = null;
                txtDiferencia.Visible = false;
            }

            txtTotal.Value = total.ToString();


            #endregion

            #region Sumar Cantidad de Artículos

            columnIndexToSum -= 1;
            total = GetTotalSumOfColum(columnIndexToSum);
            txtTotal = GetForm().Items.Item(FormItemIds.txtTotalArticulos.IdToString()).Specific as EditText;

            txtTotal.Value = total.ToString();

            #endregion

        }


        private void ClearMatrix()
        {
            var matrix = ((Matrix)GetApplication().Forms.ActiveForm.Items.Item(FormItemIds.matrix.IdToString()).Specific);
            matrix.Clear();
        }

        private double GetTotalSumOfColum(int columnIndex)
        {
            var matrix = GetForm().Items.Item(FormItemIds.matrix.IdToString()).Specific as Matrix;
            double total = 0;
            for (var i = 1; i < GetMatrix().VisualRowCount + 1; i++)
            {
                string stringValue = Convert.ToString(GetMatrix().Columns.Item(columnIndex).Cells.Item(i).Specific.Value);
                total += double.Parse(stringValue.Replace(",", ""), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return total;
        }

        private bool ValidateQuantityDeliveredCell(Form form)
        {
            form = GetForm();
            var cellFocus = (form.Items.Item(FormItemIds.matrix.IdToString()).Specific as Matrix).GetCellFocus();

            Double.TryParse(GetMatrix().Columns.Item(cellFocus.ColumnIndex).Cells.Item(cellFocus.rowIndex).Specific.Value, out double cantidadADespachar);
            Double.TryParse(GetMatrix().Columns.Item(cellFocus.ColumnIndex - 2).Cells.Item(cellFocus.rowIndex).Specific.Value, out double cantidadPendiente);//La cantidad pendiente está ubicada dos columnas antes de la cant. a despachar.

            if (cantidadADespachar > cantidadPendiente)
            {
                ShowMessage(MessageType.Error, "La cantidad a despachar no  puede ser mayor a la cantidad pendiente");
                return false;
            }
            else
                return true;

        }

        private bool ValidateItemsQuantityTotalForWarehouse()
        {
            const int itemIndexColum = 8;
            const int quantityColum = 13;

            var listItems = new List<ItemQuantity>();

            for (var i = 1; i < GetMatrix().VisualRowCount + 1; i++)
            {
                listItems.Add(new ItemQuantity()
                {
                    ItemCode = GetMatrix().Columns.Item(itemIndexColum).Cells.Item(i).Specific.Value,
                    Quantity = Convert.ToDouble(GetMatrix().Columns.Item(quantityColum).Cells.Item(i).Specific.Value)

                });
            }

            List<ItemQuantity> groupedList = listItems.GroupBy(x => x.ItemCode).Select(x => new ItemQuantity()
            {
                ItemCode = x.Key,
                Quantity = x.Sum(y => y.Quantity)
            }).ToList();

            var whsCode = GetForm().Items.Item(FormItemIds.txtAlmacen.IdToString()).Specific.Value;
            foreach (var item in groupedList)
            {
                string queryString = GetQueryString(EmbebbedFileName.MSS_DESP_GetItemOnHandQuantity).Replace(ConstantHelper.PARAM1, item.ItemCode)
                                                                                                    .Replace(ConstantHelper.PARAM2, whsCode);

                double cantidadDisponibleEnAlmacen = Convert.ToDouble(DoQuery(queryString).Fields.Item(0).Value);
                if (cantidadDisponibleEnAlmacen < item.Quantity)
                {
                    ShowAlert("La cantidad total de artículos a despachar para el artículo N°" + item.ItemCode + " asciende a " + item.Quantity.ToString("#0.00") + " la cual supera a la disponible en almacén: " + cantidadDisponibleEnAlmacen.ToString("#0.00"));
                    return false;
                }
            }

            return true;
        }

        private void FillVehicleData(Form form, string vehicleCode)
        {
            var query = GetQueryString(EmbebbedFileName.MSS_VEHIC_GetItem).Replace(ConstantHelper.PARAM1, vehicleCode);

            var rs = DoQuery(query);
            if (rs.RecordCount > 0)
            {
                form.Items.Item(FormItemIds.txtPlacaVehiculo.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_PLAC)).Value;
                form.Items.Item(FormItemIds.txtMarcaVehiculo.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_MARC)).Value;
                form.Items.Item(FormItemIds.txtCertifOperVehiculo.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_CMTC)).Value;
                form.Items.Item(FormItemIds.txtRazonSocialVehiculo.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_RAZO)).Value;

                form.Items.Item(FormItemIds.txtCapacidadMinCarga.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_CMIC)).Value;
                form.Items.Item(FormItemIds.txtCapacidadMaxCarga.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_CMAC)).Value;
                form.Items.Item(FormItemIds.txtCapacidadMinVolumen.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_CMIV)).Value;
                form.Items.Item(FormItemIds.txtCapacidadMaxVolumen.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_CMAV)).Value;
                form.Items.Item(FormItemIds.txtNumeroMinRepartos.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_NMIR)).Value;
                form.Items.Item(FormItemIds.txtNumeroMaxRepartos.IdToString()).Specific.Value = rs.Fields.Item(new MSS_DESP().GetMemberName(x => x.MSS_NMAR)).Value;

                SumarYValidarMontosFinales();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);

        }

        private void FillConductorData(Form form, string conductorCode)
        {
            var query = GetQueryString(EmbebbedFileName.OHEM_GetItem).Replace(ConstantHelper.PARAM1, conductorCode);
            var rs = DoQuery(query);
            if (rs.RecordCount > 0)
            {
                form.Items.Item(FormItemIds.txtLicenciaConducirConductor.IdToString()).Specific.Value = rs.Fields.Item(new OHEM().GetMemberName(x => x.MSS_LICE)).Value;
                var nombresCompletos = rs.Fields.Item(new OHEM().GetMemberName(x => x.firstName, false)).Value + " "
                                        + rs.Fields.Item(new OHEM().GetMemberName(x => x.middleName, false)).Value + " "
                                        + rs.Fields.Item(new OHEM().GetMemberName(x => x.lastName, false)).Value;

                form.Items.Item(FormItemIds.txtNombresConductor.IdToString()).Specific.Value = nombresCompletos;

            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);

        }

        public int GetCantidadDeDireccionesADespachar()
        {
            const int docEntryIndexColumn = 7;
            var listDirecciones = new List<string>();
            for (var i = 1; i < GetMatrix().VisualRowCount + 1; i++)
            {
                string value = GetMatrix().Columns.Item(docEntryIndexColumn).Cells.Item(i).Specific.Value;
                if (!string.IsNullOrEmpty(value.Trim()))
                    listDirecciones.Add(value.Trim());
            }

            return listDirecciones.Distinct().Count();
        }

        private bool BeforeSave()
        {
            try
            {
                GetMatrix().FlushToDataSource();
                GetMatrix().LoadFromDataSource();
                if (GetForm().Mode == BoFormMode.fm_EDIT_MODE || GetForm().Mode == BoFormMode.fm_UPDATE_MODE)
                {
                    var filasEliminadas = GetMatrix().RowCount - GetMatrix().VisualRowCount;
                    if (filasEliminadas > 0)
                    {
                        var response = GetApplication().MessageBox("Se eliminarán " + filasEliminadas + " filas. ¿Desea continuar?", 1, "Si", "No");
                        switch (response)
                        {
                            //NO:
                            default:
                                return false;
                            case 1: //SI
                                break;
                        }
                    }
                }

                #region Lógica de modo de update

                //El despacho no se puede modificar si ya ha sido aprobado o aún está pendiente.
                if (GetForm().Mode == BoFormMode.fm_UPDATE_MODE)
                {
                    var despachoDocEntry = GetForm().Items.Item(FormItemIds.txtDocEntry.IdToString()).Specific.Value;
                    var queryString = GetQueryString(EmbebbedFileName.MSS_DESP_GetItem).Replace(ConstantHelper.PARAM1, despachoDocEntry);
                    var rs = DoQuery(queryString);

                    string estadoDespacho = Convert.ToString(rs.Fields.Item(new MSS_VEHI().GetMemberName(x => x.MSS_ESTA)).Value);

                    switch (estadoDespacho)
                    {
                        case ConstantHelper.EstadoPlanificacionDespacho.Aprobado:
                            GetApplication().MessageBox("No se puede modificar el documento cuando ya ha sido aprobado.");
                            return false;
                        case ConstantHelper.EstadoPlanificacionDespacho.Pendiente:
                            GetApplication().MessageBox("No se puede modificar el documento cuando está en proceso de aprobación.");
                            return false;
                        case ConstantHelper.EstadoPlanificacionDespacho.Rechazado:
                            break;
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(rs);

                }

                #endregion

                #region Validaciones

                if (string.IsNullOrEmpty((_Form.Items.Item(FormItemIds.cbSerie.IdToString()).Specific as ComboBox).Value))
                {
                    ShowMessage(MessageType.Error, "Seleccione una serie");
                    return false;
                }

                if (string.IsNullOrEmpty((_Form.Items.Item(FormItemIds.txtFecha.IdToString()).Specific as EditText).Value))
                {
                    ShowMessage(MessageType.Error, "Ingrese la fecha");
                    return false;
                }

                var totalWeightVehiculo = (_Form.Items.Item(FormItemIds.txtCapacidadMaxCarga.IdToString()).Specific as EditText).Value;
                var totalVolumeVehiculo = (_Form.Items.Item(FormItemIds.txtCapacidadMaxVolumen.IdToString()).Specific as EditText).Value;
                var numeroRepartosMaximo = (_Form.Items.Item(FormItemIds.txtNumeroMinRepartos.IdToString()).Specific as EditText).Value;
                var placa = (_Form.Items.Item(FormItemIds.txtPlacaVehiculo.IdToString()).Specific as EditText).Value;
                if (string.IsNullOrEmpty(totalWeightVehiculo)
                    || string.IsNullOrEmpty(totalVolumeVehiculo)
                    || string.IsNullOrEmpty(numeroRepartosMaximo)
                    || string.IsNullOrEmpty(placa))
                {
                    ShowMessage(MessageType.Error, "Faltan datos del vehículo");
                    return false;
                }


                if (string.IsNullOrEmpty((_Form.Items.Item(FormItemIds.txtAlmacen.IdToString()).Specific as EditText).Value))
                {
                    ShowMessage(MessageType.Error, "Ingrese el almacén");
                    return false;
                }

                if ((GetForm().Items.Item(FormItemIds.matrix.IdToString()).Specific as Matrix).RowCount <= 0)
                {
                    ShowMessage(MessageType.Error, "Agregue al menos un ítem al detalle.");
                    return false;
                }

                var totalWeightDespacho = (_Form.Items.Item(FormItemIds.txtPesoTotal.IdToString()).Specific as EditText).Value;
                var totalVolumeDespacho = (_Form.Items.Item(FormItemIds.txtVolumenTotal.IdToString()).Specific as EditText).Value;
                if (string.IsNullOrEmpty(totalWeightDespacho) || string.IsNullOrEmpty(totalVolumeDespacho))
                {
                    ShowMessage(MessageType.Error, "No se ha calculado el peso o volumen total del despacho.");
                    return false;
                }



                if (!ValidateItemsQuantityTotalForWarehouse())
                    return false;

                var motivoPendiente = string.Empty;
                var itsApprove = true;

                if (GetForm().Items.Item(FormItemIds.txtDiferenciaPeso.IdToString()).Visible)
                {
                    motivoPendiente += GetForm().Items.Item(FormItemIds.txtDiferenciaPeso.IdToString()).Specific.Value;
                    itsApprove = false;
                }

                if (GetForm().Items.Item(FormItemIds.txtDiferenciaPeso.IdToString()).Visible)
                {
                    motivoPendiente += (", " + GetForm().Items.Item(FormItemIds.txtDiferenciaVolumen.IdToString()).Specific.Value);
                    itsApprove = false;
                }

                if (GetForm().Items.Item(FormItemIds.txtDiferenciaNumeroRepartos.IdToString()).Visible)
                {
                    motivoPendiente += (", " + GetForm().Items.Item(FormItemIds.txtDiferenciaNumeroRepartos.IdToString()).Specific.Value);
                    itsApprove = false;
                }

                if (motivoPendiente.ToSafeString().Length > 200) motivoPendiente = motivoPendiente.Substring(0, 199);

                #endregion

                if (itsApprove)
                {
                    estadoDespacho = new EstadoDespacho() { Estado = ConstantHelper.EstadoPlanificacionDespacho.Aprobado, Motivo = string.Empty, ErrorSAP = string.Empty };
                    return true;
                }
                else
                {
                    var response = GetApplication().MessageBox("La planificación del despacho pasará por un proceso de aprobación. ¿Desea continuar?", 1, "Si", "No");
                    switch (response)
                    {
                        default:
                            return false;
                        case 1: //SI 
                            estadoDespacho = new EstadoDespacho() { Estado = ConstantHelper.EstadoPlanificacionDespacho.Pendiente, Motivo = motivoPendiente, ErrorSAP = string.Empty };
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(MessageType.Error, ex.ToSafeString());
                return false;
            }
        }

        public void AfterSave()
        {
            try
            {
                if (estadoDespacho == null)//Controla el error en el modo find
                {
                    ExceptionHelper.LogException(new Exception("No se encontró el estado del despacho en memoria"));
                    return;
                }

                DisableItems();

                int lastDocEntry = Convert.ToInt32(DoQuery(GetQueryString(EmbebbedFileName.MSS_DESP_GetLastDocumentCreated).Replace(ConstantHelper.PARAM1, GetCompany().UserName)).Fields.Item("DocEntry").Value);

                var queryGetDespacho = GetQueryString(EmbebbedFileName.MSS_DESP_GetItem).Replace(ConstantHelper.PARAM1, lastDocEntry.ToString());


                //Elimina todas las filas que tienen estado eliminar: U_MSS_DOCE = NULL;
                var query = GetQueryString(EmbebbedFileName.MSS_DESP_LINES_RemoveNull).Replace(ConstantHelper.PARAM1, lastDocEntry.ToSafeString());
                DoQuery(query);

                //Actualiza campos
                var queryUpdateDespacho = GetQueryString(EmbebbedFileName.MSS_DESP_UpdateItem2).Replace("param1", estadoDespacho.Estado)
                                                                                       .Replace("param2", estadoDespacho.Motivo)
                                                                                       .Replace("param3", estadoDespacho.ErrorSAP)
                                                                                       .Replace("param4", lastDocEntry.ToString());
                DoQuery(queryUpdateDespacho);


                //Valida si el despacho está aprobado
                if (estadoDespacho.Estado == ConstantHelper.EstadoPlanificacionDespacho.Aprobado)
                    MSS_APROForm.AprobarDespacho(lastDocEntry);

                if (GetForm().Mode != BoFormMode.fm_ADD_MODE)
                    GetApplication().ActivateMenuItem(MenuUID.RegistroActualizar.IdToString());

            }
            catch (Exception ex)
            {
                HandleApplicationException(ex);
            }
        }

        private Matrix GetMatrix()
        {
            return GetForm().Items.Item(FormItemIds.matrix.IdToString()).Specific;
        }

        private DBDataSource GetDataSource()
        {
            return GetForm().DataSources.DBDataSources.Item(0);
        }

        private DBDataSource GetDataSourceDetail()
        {
            return GetForm().DataSources.DBDataSources.Item(1);
        }

        public enum FormItemIds
        {
            btnSave = 1,

            cbSerie = 10,
            txtNumero = 11,
            txtFecha = 12,

            txtCodigoVehiculo = 20,
            txtPlacaVehiculo = 21,
            txtMarcaVehiculo = 22,
            txtCertifOperVehiculo = 23,

            txtRazonSocialVehiculo = 30,
            txtCapacidadMinVolumen = 31,
            txtCapacidadMinCarga = 32,
            txtNumeroMinRepartos = 33,

            txtCapacidadMaxCarga = 40,
            txtCapacidadMaxVolumen = 41,
            txtNumeroMaxRepartos = 42,

            txtConductor = 50,
            txtNombresConductor = 51,
            txtLicenciaConducirConductor = 52,
            txtAlmacen = 53,
            txtDocEntry = 54,

            txtPesoTotal = 60,
            txtVolumenTotal = 61,
            txtDireccionesTotal = 62,
            txtTotalArticulos = 630,

            txtDiferenciaPeso = 70,
            txtDiferenciaVolumen = 71,
            txtDiferenciaNumeroRepartos = 72,


            //txtJustificacion = 90,
            txtEstado = 91,
            //txtComentarioDeAprobador = 92,
            txtMotivoPendiente = 93,
            txtMensajeInternoSAP = 94,

            cflVehiculo = 100,
            cflConductor = 101,
            cflAlmacen = 102,

            matrix = 3000,
            btnAddDetail = 3001,

        }

        public enum FormColumIds
        {
            LineId = -300,
            OrderDocEntry = -301,
            OrderNum = -302,
            OrderLineNumRef = -303,
            CardCodeClient = -304,
            CardNameClient = -305,
            IdDireccionEntrega = -306,
            DetalleDireccion = -307,
            CodigoArticulo = -308,
            DescripcionArticulo = -309,
            UnidadMedidaArticulo = -310,
            CantidadPendiente = -311,
            CantidadAlmacen = -312,
            CantidadADespachar = -313,
            PesoUND = -314,
            VolumenUND = -315,
        }
    }
}
