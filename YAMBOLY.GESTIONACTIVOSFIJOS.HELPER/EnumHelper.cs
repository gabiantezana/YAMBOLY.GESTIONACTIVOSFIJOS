using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class EnumHelper
    {
    }

    public enum FormConductorItems
    {
        ButtonSave = 1
    }

    public enum _150FormItems
    {
        SaveButton = 1,
        ItemCode = 5,
        Active = 10002050,
        Inactive = 10002051,
        OptionBtn3 = 10002052,

    }

    public enum ArticleActivationFormItems
    {
        LblListado = -1,
        Grid = -2,
        Button = -3,
    }

    public enum EmbebbedFileName
    {
        AddonMenu = 1,
        SampleForm = 2,
        ArticleActivationForm = 3,
        MSS_VEHIForm = 4,
        //MSS_CONFForm = 5,
        MSS_DESPForm = 6,
        MSS_DESPListForm = 7,
        MSS_APROForm = 8,
        MSS_ASUAForm = 9,
        MSS_ASUUForm = 10,
        MSS_CONFForm = 11,

        #region Queries

        MSS_APRO_GetList = 100,
        MSS_DESP_ApproveItem = 101,
        MSS_DESP_GetDetail = 102,
        MSS_DESP_GetItem = 103,
        MSS_VEHIC_GetItem = 104,
        RDR1_GetItem = 105,
        RDR1_GetList = 106,
        MSS_ASUA_UDO_GetList = 107,
        MSS_ASUU_UDO_GetList = 108,
        MSS_ASUA_UDO_GetListByUser = 109,
        MSS_DESP_GetLastDocumentCreated = 110,
        ORDR_GetItem = 111,
        MSS_DESP_UpdateItem = 112,
        MSS_DESP_GetRoutes = 113,
        MSS_CONF_GetItem = 114,
        ORDR_UpdateAfterApproveDesp = 115,
        OHEM_GetItem = 116,
        MSS_DESP_GetItemOnHandQuantity = 117,
        OITM_GetItem = 118,
        MSS_DESP_LINES_RemoveNull = 119,
        MSS_DESP_UpdateItem2 = 120,
        #endregion
    }

    public enum FormID
    {
        ArticleActivationForm = -100,
        _150 = 150,
        MSS_VEHI = -101,
        MSS_CONF = -102,
        MSS_DESP = -103,
        MSS_DESP_LIST = -104,
        MSS_APRO = -105,
        MSS_ASUA = -106,
        MSS_ASUU = -107,
    }

    public enum MenuUID
    {
        _150Form = 3073,
        MenuBuscar = 1281,
        MenuCrear = 1282,
        RegistroDatosSiguiente = 1288,
        RegistroDatosAnterior = 1289,
        RegistroDatosPrimero = 1290,
        RegistroDatosUltimo = 1291,
        AddLine = 1292,
        RemoveLine = 1293,
        RegistroActualizar = 1304,
        InventoryMenu = 3072,
        //ArticleActivation = -1000,
        AddonGestionDespachosMenu = -2000,
        ConfiguracionAddonSubMenu = -2002,
        MaestroVehiculosSubMenu = -2003,
        DespachoVehiculosSubMenu = -2004,
        AprobacionDespachoVehiculosSubMenu = -2005,
        AutorizacionusuarioAlmacenSubMenu = -2006,
        DefinicionUsuarioAprobadorSubMenu = -2007,

    }

    public enum MessageType
    {
        Success = 1,
        Info = 2,
        Error = 3,
        Warning = 4,
    }

    public enum SaveType
    {
        SaveAsPending = 1,
        SaveAsNoPending = 2,
        KeepOriginalDBValues = 3
    }

}
