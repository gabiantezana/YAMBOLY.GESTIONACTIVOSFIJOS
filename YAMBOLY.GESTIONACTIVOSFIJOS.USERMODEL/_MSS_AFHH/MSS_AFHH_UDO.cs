using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_AFHH
{
    [DBStructure]
    [SAPUDO(Name = "AF- HistorialUbicaciones",
          HeaderTableType = typeof(MSS_AFHH),
          ChildTableTypeList = new[] { typeof(MSS_AFHH_LINES) },
           //CanCreateDefaultForm = BoYesNoEnum.tYES,
           CanFind = BoYesNoEnum.tYES,
           EnableEnhancedForm = BoYesNoEnum.tYES,
           RebuildEnhancedForm = BoYesNoEnum.tYES,
           ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document
    )]
    public class MSS_AFHH_UDO
    {
    }
}
