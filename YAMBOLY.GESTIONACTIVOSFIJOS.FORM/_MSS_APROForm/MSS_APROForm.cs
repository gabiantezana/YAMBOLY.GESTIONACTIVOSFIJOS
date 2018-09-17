using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OPKL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_DESP;
using System.Threading;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._OHEM;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._ORDR;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_APROForm
{
    public class MSS_APROForm : BaseApplication, ISAPForm
    {
        private Form _Form { get; set; }
        public Form GetForm() => _Form;
        public static string GetFormName() => "Aprobación de despachos";

        Thread hilo { get; set; }

        public MSS_APROForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), EmbebbedFileName.MSS_APROForm), FormID.MSS_APRO.IdToString());
            if (_Form != null)
            {
                dictionary.Add(_Form.UniqueID, this);
                _Form.Freeze(true);
                FillGrid();
                _Form.Freeze(false);
                _Form.Visible = true;
            }
        }

        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent)
        {
            _Form = GetApplication().Forms.ActiveForm;

            if (itemEvent.ItemUID == FormItemIds.btnSave.IdToString() && itemEvent.ActionSuccess)
                AprobarDespachos();
            if (itemEvent.ItemUID == FormItemIds.btnRechazar.IdToString() && itemEvent.ActionSuccess)
                RechazarDespachos();

            return true;
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

        public void FillGrid()
        {
            GetForm().Mode = BoFormMode.fm_ADD_MODE;

            var dataTableId = Guid.NewGuid().GetHashCode().ToSafeString();

            DataTable dataTable = _Form.DataSources.DataTables.Add(dataTableId);
            var queryString = GetQueryString(EmbebbedFileName.MSS_APRO_GetList).Replace(ConstantHelper.PARAM1, GetCompany().UserName);
            dataTable.ExecuteQuery(queryString);
            var grid = (SAPbouiCOM.Grid)_Form.Items.Item(FormItemIds.grid.IdToString()).Specific;


            if (!dataTable.IsEmpty)
            {
                grid.DataTable = dataTable;
                grid.Columns.Item(0).Type = BoGridColumnType.gct_CheckBox;

                for (var i = 0; i < grid.Columns.Count; i++){
                    grid.Columns.Item(i).Editable = false;
                }

                grid.Columns.Item(0).Editable = true;

            }
            else
            {
                _Form.Items.Item(FormItemIds.lblListado.IdToString()).Specific.Caption = "No se encontraron despachos para aprobar.";
                _Form.Items.Item(FormItemIds.btnSave.IdToString()).Enabled = false;
                _Form.Items.Item(FormItemIds.grid.IdToString()).Enabled = false;
            }

        }

        public List<String> GetSelectedItems()
        {
            List<String> ItemsToActive = new List<String>();
            Grid grid = _Form.Items.Item(FormItemIds.grid.IdToString()).Specific;
            for (var i = 0; i < grid.DataTable.Rows.Count; i++)
            {
                if (grid.DataTable.GetValue(0, i).ToString() == ConstantHelper.SAP_YES_NO.YES)
                {
                    String itemCode = grid.DataTable.GetValue(1, i).ToString();
                    ItemsToActive.Add(itemCode);
                }
            }
            return ItemsToActive;
        }

        private void RechazarDespachos()
        {

            if (hilo?.IsAlive == true)
            {
                ShowMessage(MessageType.Info, "Espere mientras se procesa la solicitud anterior.");
            }
            else
            {
                //Creamos el delegado 
                ThreadStart delegado = new ThreadStart(DissaproveItems);
                //Creamos la instancia del hilo 
                hilo = new Thread(delegado);
                //Iniciamos el hilo 
                hilo.Start();
            }
        }

        private void AprobarDespachos()
        {
            if (hilo?.IsAlive == true)
            {
                ShowMessage(MessageType.Warning, "Espere mientras se procesa la solicitud anterior.");
            }
            else
            {
                //Creamos el delegado 
                ThreadStart delegado = new ThreadStart(ApproveItems);
                //Creamos la instancia del hilo 
                hilo = new Thread(delegado);
                //Iniciamos el hilo 
                hilo.Start();
            }
        }

        public void ApproveItems()
        {
            var itemsToApprove = GetSelectedItems();
            if (itemsToApprove.Count == 0)
                return;

            ShowMessage(MessageType.Success, "Aprobando despachos. Por favor, espere...");
            foreach (var docentry in itemsToApprove)
            {
                AprobarDespacho(Convert.ToInt32(docentry));
            }
            FillGrid();
        }

        public void DissaproveItems()
        {
            var itemsToApprove = GetSelectedItems();
            if (itemsToApprove.Count == 0)
                return;

            ShowMessage(MessageType.Success, "Desaprobando despachos. Por favor, espere...");
            foreach (var docentry in itemsToApprove)
            {
                RechazarDespacho(Convert.ToInt32(docentry));
            }

            FillGrid();
        }


        public static void AprobarDespacho(int docEntry, bool hasParentTransaction = false)
        {
            try
            {
                if (!hasParentTransaction)
                    GetCompany().StartTransaction();

                //Crea el picking
                CreatePicking(docEntry);

                //Inserta datos del despacho en todas las órdenes referenciadas.
                UpdateUserFieldsInSalesOrder(docEntry);

                //Cambia estado de despacho.
                DoQuery(GetQueryString(EmbebbedFileName.MSS_DESP_ApproveItem).Replace(ConstantHelper.PARAM1, docEntry.ToString())
                                                                              .Replace(ConstantHelper.PARAM2, ConstantHelper.EstadoPlanificacionDespacho.Aprobado));

                //Finaliza transacción
                if (!hasParentTransaction)
                    GetCompany().EndTransaction(BoWfTransOpt.wf_Commit);

                ShowMessage(MessageType.Success, "Despacho: " + docEntry + " aprobado exitosamente");
            }
            catch (Exception ex)
            {

                if (GetCompany().InTransaction && !hasParentTransaction)
                    GetCompany().EndTransaction(BoWfTransOpt.wf_RollBack);

                var errorMessage = ex.Message;
                if (ex is SapException)
                    errorMessage = GetSapError();

                errorMessage = errorMessage.Replace("'", "*").Replace('"', '*');

                DoQuery(GetQueryString(EmbebbedFileName.MSS_DESP_UpdateItem).Replace(ConstantHelper.PARAM1, docEntry.ToString())
                                                                            .Replace(ConstantHelper.PARAM2, errorMessage));

                DoQuery(GetQueryString(EmbebbedFileName.MSS_DESP_ApproveItem).Replace(ConstantHelper.PARAM1, docEntry.ToString())
                                                                             .Replace(ConstantHelper.PARAM2, ConstantHelper.EstadoPlanificacionDespacho.Rechazado));

                var messageToShow = "Error al aprobar despacho N°" + docEntry + " : " + errorMessage + ".El despacho pasará a estado rechazado.";

                ShowAlert(messageToShow);
                HandleApplicationException(ex, messageToShow);
            }
        }

        public static void RechazarDespacho(int docEntry)
        {
            try
            {
                DoQuery(GetQueryString(EmbebbedFileName.MSS_DESP_ApproveItem).Replace(ConstantHelper.PARAM1, docEntry.ToString())
                                                                             .Replace(ConstantHelper.PARAM2, ConstantHelper.EstadoPlanificacionDespacho.Rechazado));

                ShowMessage(MessageType.Success, "Despacho: " + docEntry + " rechazado exitosamente");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex is SapException)
                    errorMessage = GetSapError();

                errorMessage = errorMessage.Replace("'", "*").Replace('"', '*');

                var messageToShow = "Error al rechazar despacho N°" + docEntry + " : " + errorMessage;

                ShowAlert(messageToShow);
                HandleApplicationException(ex, messageToShow);

            }
        }

        private static void UpdateUserFieldsInSalesOrder(int despachoDocEntry)
        {
            var queryString = GetQueryString(EmbebbedFileName.MSS_DESP_GetDetail).Replace(ConstantHelper.PARAM1, despachoDocEntry.ToString());
            var rs = DoQuery(queryString);
            if (rs.RecordCount > 0)
                for (int i = 0; i < rs.RecordCount; i++)
                {
                    var docEntryOrder = Convert.ToString(rs.Fields.Item(new MSS_DESP_LINES().GetMemberName(x => x.MSS_DOCE)).Value);
                    var queryStringUpdate = GetQueryString(EmbebbedFileName.ORDR_UpdateAfterApproveDesp).Replace(ConstantHelper.PARAM1, despachoDocEntry.ToString()).Replace(ConstantHelper.PARAM2, docEntryOrder);
                    DoQuery(queryStringUpdate);
                    rs.MoveNext();
                }
        }

        [Obsolete]
        private static void UpdateOrder(ORDR order)
        {

            Documents documents = (GetCompany().GetBusinessObject(BoObjectTypes.oOrders) as Documents);
            if (documents.GetByKey(order.DocEntry))
            {
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_CECI)).Value = order.MSS_CECI;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSSL_NCD)).Value = order.MSSL_NCD;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_CECI)).Value = order.MSS_CECI;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_DEST)).Value = order.MSS_DEST;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_FETR)).Value = order.MSS_FETR;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSSL_LCD)).Value = order.MSSL_LCD;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSSL_MVH)).Value = order.MSSL_MVH;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_ORIG)).Value = order.MSS_ORIG;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSSL_PVH)).Value = order.MSSL_PVH;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_RUCD)).Value = order.MSS_RUCD;
                //documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSSL_RTR)).Value = order.MSSL_RTR;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSS_RZOD)).Value = order.MSS_RZOD;
                documents.UserFields.Fields.Item(new ORDR().GetMemberName(x => x.MSSL_NTR)).Value = order.MSSL_NTR;

                documents.Update();
            }
            else
            {
                throw new Exception("Document does not exists. DocEntry :" + order.DocEntry);
            }
        }

        public static void CreatePicking(int despachoDocEntry)
        {
            var queryString = GetQueryString(EmbebbedFileName.MSS_DESP_GetItem).Replace(ConstantHelper.PARAM1, despachoDocEntry.ToString());
            var queryStringRutas = GetQueryString(EmbebbedFileName.MSS_DESP_GetRoutes).Replace(ConstantHelper.PARAM1, despachoDocEntry.ToString());
            var recordSet = DoQuery(queryString);
            var recordSetRutas = DoQuery(queryStringRutas);

            var despacho = new MSS_DESP();
            var oPKL = new OPKL();
            var empleado = new OHEM();

            SAPbobsCOM.PickLists picking = (SAPbobsCOM.PickLists)GetCompany().GetBusinessObject(BoObjectTypes.oPickLists);

            picking.PickDate = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_FECH)).Value;
            picking.OwnerCode = Convert.ToInt32(recordSet.Fields.Item(despacho.GetMemberName(x => x.UserSign, false)).Value);

            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_VOLU)).Value = Convert.ToDouble(recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_VOLU)).Value);
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_PESO)).Value = Convert.ToDouble(recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_PESO)).Value);
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_DESP)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.DocEntry, false)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_LICE)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_LICE)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_CMTC)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_CMTC)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_RAZO)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_RAZO)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_ARTI)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_ARTI)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_CODI)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_CODI)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_MARC)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_MARC)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_PLAC)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_PLAC)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_CONN)).Value = recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_CONN)).Value;
            picking.UserFields.Fields.Item(oPKL.GetMemberName(x => x.MSS_NUME)).Value = Convert.ToInt32(recordSet.Fields.Item(despacho.GetMemberName(x => x.MSS_NUME)).Value);


            var queryStringLines = GetQueryString(EmbebbedFileName.MSS_DESP_GetDetail).Replace(ConstantHelper.PARAM1, despachoDocEntry.ToString());
            var recordSetLines = DoQuery(queryStringLines);
            var detalleDespacho = new MSS_DESP_LINES();
            if (recordSetLines.RecordCount > 0)
            {
                for (int i = 0; i < recordSetLines.RecordCount; i++)
                {
                    picking.Lines.BaseObjectType = BoObjectTypes.oOrders.IdToString(); //Sales order;
                    picking.Lines.OrderEntry = recordSetLines.Fields.Item(detalleDespacho.GetMemberName(x => x.MSS_DOCE)).Value;
                    picking.Lines.OrderRowID = recordSetLines.Fields.Item(detalleDespacho.GetMemberName(x => x.MSS_LINE)).Value;
                    picking.Lines.ReleasedQuantity = recordSetLines.Fields.Item(detalleDespacho.GetMemberName(x => x.MSS_CAND)).Value;
                    picking.Lines.Add();
                    recordSetLines.MoveNext();
                }
            }

            if (picking.Add() == ConstantHelper.DefaulSuccessSAPNumber)
                ShowMessage(MessageType.Success, "Picking creado");
            else
                throw new SapException();
        }

        public enum FormItemIds
        {
            btnSave = 4,
            lblListado = 5,
            grid = 6,
            btnRechazar = 10,
        }
    }
}
