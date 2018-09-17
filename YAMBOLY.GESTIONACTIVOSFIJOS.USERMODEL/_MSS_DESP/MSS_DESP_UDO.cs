using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_DESP
{
    [DBStructure]
    [SAPUDO(Name = "DESPACHO_VEHICULOS",
          HeaderTableType = typeof(MSS_DESP),
          ChildTableTypeList = new[] { typeof(MSS_DESP_LINES) },
          ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document,
          //CanFind = BoYesNoEnum.tYES, //TODO:
          ManageSeries = BoYesNoEnum.tYES
    )]
    public class MSS_DESP_UDO
    {
    }
}
