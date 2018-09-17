using YAMBOLY.GESTIONACTIVOSFIJOS.EXCEPTION;
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.DATAACCESS
{
    public class BaseDataAccess
    {
        private static Dictionary<string, ISAPForm> _FormOpenList;
        private static Dictionary<string, ISAPEvents> _FormEventList;

        private static SAPbouiCOM.SboGuiApi _GuiApi { get; set; }
        private static SAPbouiCOM.Application _Application { get; set; }
        private static SAPbobsCOM.Company _Company { get; set; }

        public static Dictionary<string, ISAPForm> GetFormOpenList() => _FormOpenList;
        public static Dictionary<string, ISAPEvents> GetFormEventList() => _FormEventList;

        public void ConnectApplication()
        {
            _FormOpenList = new Dictionary<string, ISAPForm>();
            _ConnectApplication();
            ConnectCompany();
        }

        public static SAPbouiCOM.Application GetApplication() => _Application;

        public static SAPbobsCOM.Company GetCompany() => _Company;

        private static void _ConnectApplication()
        {
            String[] args = System.Environment.GetCommandLineArgs();
            String connectionString = String.Empty;
            if (args.Count() > 1)
            {
                _GuiApi = new SAPbouiCOM.SboGuiApi();
                _GuiApi.Connect(args[1]);
                _Application = _GuiApi.GetApplication(ConstantHelper.ApplicationId);
            }
            else
                throw new CustomException("Error obtaining arguments string for connection");
        }

        private static void ConnectCompany()
        {
            if (_Company == null)
            {
                if (_Application == null)
                    throw new CustomException("Application has not been initialized.");
                _Company = _Application.Company.GetDICompany();
            }
            if (!_Company.Connected)
                _ConnectCompany();

        }

        private static void _ConnectCompany()
        {
            Int32 resultReturn = _Company.Connect();
            if (resultReturn != ConstantHelper.DefaulSuccessSAPNumber)
                throw new SapException();
        }

        public static void DisconnectCompany()
        {
            try
            {
                if (_Company != null)
                {
                    if (_Company.Connected)
                        _Company.Disconnect();
                    _Company = null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static SapExceptionEntity GetErrorFromSAP()
        {
            SapExceptionEntity SapResponse = new SapExceptionEntity();
            try
            {
                SapResponse.errorCode = _Company.GetLastErrorCode();
                SapResponse.errorMessage = _Company.GetLastErrorDescription();
            }
            catch (Exception) { }
            return SapResponse;
        }

        public void ClearLatestVersionsInDBSchema()
        {

        }

        public void GenerateDBSchema()
        {
            CreateSchema();
        }

        public void CreateSchema()
        {
            DBSchema dBSchema = new UserModel().GetDBSchema();
            dBSchema.TableList.ForEach(x => SapMethodsHelper.CreateTable(GetCompany(), x));
            dBSchema.FieldList.ForEach(x => SapMethodsHelper.CreateField(GetCompany(), x));
            dBSchema.UDOList.ForEach(x => SapMethodsHelper.CreateUDO(GetCompany(), x));
        }

        #region Queries 

        public string GetQuery(EmbebbedFileName embebbedFileName)
        {
            return XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), embebbedFileName);
        }

        public SAPbobsCOM.Recordset DoQuery(EmbebbedFileName embebbedFileName)
        {
            var oRecordSet = ((SAPbobsCOM.Recordset)(GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            oRecordSet.DoQuery(GetQuery(embebbedFileName));
            return oRecordSet;
        }
        public SAPbobsCOM.Recordset DoQuery(string query)
        {


            var oRecordSet = ((SAPbobsCOM.Recordset)(GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
            oRecordSet.DoQuery(query);
            return oRecordSet;


        }

        #endregion
    }
}
