using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_DESPForm;
using static YAMBOLY.GESTIONACTIVOSFIJOS.HELPER.ConstantHelper;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CONFForm
{
    class MSS_DESP_LISTForm : BaseApplication, ISAPForm
    {
        private Form _Form { get; set; }
        private Form _ParentForm { get; set; }
        public Form GetForm() => _Form;
        public static string GetFormName() => "Seleccionar despachos";

        int noneSelectedRowColor = 15198183;
        int selectedRowColor = 9434879;

        public MSS_DESP_LISTForm(Dictionary<string, ISAPForm> dictionary, Form parentForm)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), EmbebbedFileName.MSS_DESPListForm), FormID.MSS_DESP_LIST.IdToString());
            if (_Form != null)
            {
                dictionary.Add(_Form.UniqueID, this);
                _Form.Freeze(true);
                _ParentForm = parentForm;

                InitializeGrid();
                _Form.Freeze(false);
                _Form.Visible = true;
            }
        }

        public void InitializeGrid()
        {
            GetForm().Items.Item(FormItemIds.grid.IdToString()).Enabled = false;
            FillGrid();
        }

        public void FillGrid()
        {

            var dataTableId = new Guid().GetHashCode().ToSafeString();
            _Form.DataSources.DataTables.Add(dataTableId);

            var fechaDespacho = ((EditText)_ParentForm.Items.Item(MSS_DESPForm.FormItemIds.txtFecha.IdToString()).Specific).Value;
            var codigoAlmacen = ((EditText)_ParentForm.Items.Item(MSS_DESPForm.FormItemIds.txtAlmacen.IdToString()).Specific).Value;

            if (string.IsNullOrEmpty(fechaDespacho))
                throw new CustomException("Seleccione una fecha");
            else if (string.IsNullOrEmpty(codigoAlmacen))
                throw new CustomException("Seleccione un almacén");

            var queryString = GetQueryString(EmbebbedFileName.RDR1_GetList).Replace(ConstantHelper.PARAM1, fechaDespacho).Replace(ConstantHelper.PARAM2, codigoAlmacen);

            _Form.DataSources.DataTables.Item(0).ExecuteQuery(queryString);

            var oGrid = (SAPbouiCOM.Grid)_Form.Items.Item(FormItemIds.grid.IdToString()).Specific;
            oGrid.DataTable = _Form.DataSources.DataTables.Item(dataTableId);

            if (!oGrid.DataTable.IsEmpty)
            {
                oGrid.CollapseLevel = 2;
                oGrid.CollapseLevel = 3;
                oGrid.CollapseLevel = 4;
                oGrid.CollapseLevel = 5;
                oGrid.CollapseLevel = 6;

                oGrid.Columns.Item(oGrid.Columns.Count - 1).Type = BoGridColumnType.gct_CheckBox;
            }

            for (var i = 0; i < oGrid.Rows.Count; i++)
            {
                GetGrid().CommonSetting.SetRowBackColor(i + 1, noneSelectedRowColor);
            }
        }

        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            if (itemEvent.ItemUID == FormItemIds.btnSeleccionar.IdToString() && itemEvent.ActionSuccess)
            {
                SetSelectedItems(itemEvent);
                return true;
            }
            if (itemEvent.EventType == BoEventTypes.et_FORM_CLOSE && itemEvent.ActionSuccess)
            {
                //_ParentForm.Freeze(false);
                return true;
            }

            if (itemEvent.EventType == BoEventTypes.et_DOUBLE_CLICK && itemEvent.BeforeAction)
            {
                PaintRow(itemEvent);
            }
            return true;
        }

        public void PaintRow(ItemEvent itemEvent)
        {
            try
            {
                var checkColumnIndex = GetGrid().Columns.Count - 1;//La columna con check button es la última.

                #region SingleSelection
                if (GetGrid().Rows.IsLeaf(itemEvent.Row))//If row has not childs
                {
                    var rowindex = GetGrid().GetDataTableRowIndex(itemEvent.Row);

                    string valueToSet = ConstantHelper.SAP_YES_NO.YES;
                    int colorToPaint = selectedRowColor;

                    if (GetGrid().DataTable.GetValue(checkColumnIndex, rowindex) == valueToSet)
                    {
                        valueToSet = ConstantHelper.SAP_YES_NO.NO;
                        colorToPaint = noneSelectedRowColor;
                    }

                    GetGrid().DataTable.SetValue(checkColumnIndex, rowindex, valueToSet);

                    var currentRowNumber = GetCurrentRowNumberInGridGrouping(itemEvent.Row);
                    GetGrid().CommonSetting.SetRowBackColor(currentRowNumber, colorToPaint);
                }
                #endregion

                #region GroupSelections
                else
                {
                    var InsideBranchesTotal = 0; //Número de brazos contenidos en la celda seleccionada
                    int expandedleafs = 0; //filas expandidas
                    int countbranch = 0; //Ramas
                    bool lastbranchExpanded = true; //último brazo expandido

                    //first parent branches count
                    for (int i = itemEvent.Row; i < GetGrid().Rows.Count; i++)
                    {
                        if (!GetGrid().Rows.IsLeaf(i))//If the rows has childs
                        {
                            lastbranchExpanded = GetGrid().Rows.IsExpanded(i);
                            InsideBranchesTotal++;
                        }
                        else
                            break;
                    }

                    var countBranch2 = 0;
                    for (int i = itemEvent.Row; i < GetGrid().Rows.Count; i++)
                    {
                        if (!GetGrid().Rows.IsLeaf(i))//If the rows has childs
                        {
                            lastbranchExpanded = GetGrid().Rows.IsExpanded(i);
                            countbranch++;
                            countBranch2++;
                        }
                        else
                        {
                            if (countBranch2 >= InsideBranchesTotal && expandedleafs > 0) break;
                            if (lastbranchExpanded)
                            {
                                if (countBranch2 <= InsideBranchesTotal)
                                {
                                    expandedleafs++;

                                    var dataTableRowIndex = GetGrid().GetDataTableRowIndex(i);
                                    var value = GetGrid().DataTable.GetValue(checkColumnIndex, dataTableRowIndex);
                                    var paint = value == ConstantHelper.SAP_YES_NO.YES ? false : true;
                                    if (paint)
                                    {
                                        GetGrid().DataTable.SetValue(checkColumnIndex, dataTableRowIndex, ConstantHelper.SAP_YES_NO.YES);
                                        GetGrid().CommonSetting.SetRowBackColor((expandedleafs + countbranch + itemEvent.Row), selectedRowColor);
                                    }
                                    else
                                    {
                                        GetGrid().DataTable.SetValue(checkColumnIndex, dataTableRowIndex, ConstantHelper.SAP_YES_NO.NO);
                                        GetGrid().CommonSetting.SetRowBackColor((expandedleafs + countbranch + itemEvent.Row), noneSelectedRowColor);
                                    }

                                    countBranch2 = 0;//Se reinicia el contador de brazitos :3
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex) { ExceptionHelper.LogException(ex); }
        }

        private int GetCurrentRowNumberInGridGrouping(int itemEventRow)
        {
            var rowindex = GetGrid().GetDataTableRowIndex(itemEventRow);
            int expandedleafs = 0;//filas expandidas
            int countbranch = 0;//Ramas
            bool lastbranchExpanded = true; //último brazo expandido


            for (int i = 0; i <= itemEventRow; i++)
            {
                if (!GetGrid().Rows.IsLeaf(i))//If the rows has open childs
                {
                    lastbranchExpanded = GetGrid().Rows.IsExpanded(i);
                    countbranch++;
                }
                else
                    if (lastbranchExpanded)
                    expandedleafs++;
            }

            return countbranch + expandedleafs;
        }

        public bool HandleItemPressed(SAPbouiCOM.ItemEvent oEvent) { return true; }

        public bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo)
        {
            return OnSave(GetApplication().Forms.ActiveForm);
        }

        public bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent) { return true; }

        public bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo) { return true; }

        public bool OnSave(Form form)
        {
            return true;
        }

        [Obsolete]
        public void LoadData(Form form)
        {

            form.DataSources.DataTables.Add("DATA");
            form.DataSources.DataTables.Item("DATA").ExecuteQuery("SELECT CardCode, CardName FROM OCRD");

            ((SAPbouiCOM.Matrix)(form.Items.Item("mtxList").Specific)).Columns.Item("ColCC").DataBind.Bind("DATA", "CardCode");
            ((SAPbouiCOM.Matrix)(form.Items.Item("mtxList").Specific)).Columns.Item("ColCN").DataBind.Bind("DATA", "CardName");
            ((SAPbouiCOM.Matrix)(form.Items.Item("mtxList").Specific)).Clear();
            ((SAPbouiCOM.Matrix)(form.Items.Item("mtxList").Specific)).LoadFromDataSource();
            ((SAPbouiCOM.Matrix)(form.Items.Item("mtxList").Specific)).AutoResizeColumns();

            SAPbouiCOM.Grid oGrid = (Grid)this._Form.Items.Item(FormItemIds.grid.IdToString()).Specific;
            oGrid.AutoResizeColumns();
        }

        public void SetSelectedItems(SAPbouiCOM.ItemEvent itemEvent)
        {
            var grid = (SAPbouiCOM.Grid)GetApplication().Forms.ActiveForm.Items.Item(FormItemIds.grid.IdToString()).Specific;
            var columnCodeIndex = grid.DataTable.Columns.Count - 1;//My internal code is in the last column.

            var items = GetSelectedRows();

            //Close form
            GetApplication().Forms.ActiveForm.Close();

            //Add lines to principal matrix
            var parentForm = GetFormOpenList()[_ParentForm.UniqueID];
            ((MSS_DESPForm)parentForm).AddItemsToMatrix(items.ToArray());

        }

        public List<string> GetSelectedRows()
        {
            var columnCodeIndex = GetGrid().DataTable.Columns.Count - 2;//My internal code is in the last column.(DocEntry +'-'+LineNUm)
            var checkColumnIndex = GetGrid().Columns.Count - 1;//La columna con check button es la última.

            var items = new List<string>();
            var rowCount = GetGrid().Rows.Count;

            for (int i = 1; i <= rowCount; i++)
            {
                try
                {
                    var dataTableRowIndex = GetGrid().GetDataTableRowIndex(i);
                    if (dataTableRowIndex != -1)
                        try
                        {
                            if (GetGrid().DataTable.GetValue(checkColumnIndex, dataTableRowIndex) == ConstantHelper.SAP_YES_NO.YES)
                            {
                                var code = GetGrid().DataTable.GetValue(columnCodeIndex, dataTableRowIndex);
                                items.Add(code);
                            }
                        }
                        catch (Exception ex) { }
                }
                catch (Exception ex) { }
            }

            return items;
        }

        public Grid GetGrid()
        {
            return (SAPbouiCOM.Grid)GetForm().Items.Item(FormItemIds.grid.IdToString()).Specific;
        }

        public enum FormItemIds
        {
            grid = 4,
            btnSeleccionar = 5,
        }
    }
}
