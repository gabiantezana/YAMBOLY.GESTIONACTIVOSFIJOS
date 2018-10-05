using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHI
{
    [DBStructure]
    [SAPUDO(Name = "HISTORIAL DE UBICACIONES DE ACTIVOS FIJOS",
          HeaderTableType = typeof(MSS_AFHI),
          ChildTableTypeList = new[] { typeof(MSS_AFHI_LINES) },
          ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document
          //CanFind = BoYesNoEnum.tYES, //TODO:
    )]
    public class MSS_DESP_UDO
    {
    }
}
