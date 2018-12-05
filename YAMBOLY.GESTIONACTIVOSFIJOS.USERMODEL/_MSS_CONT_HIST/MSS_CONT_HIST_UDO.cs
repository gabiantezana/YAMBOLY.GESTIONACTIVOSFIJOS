using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT_HIST
{
    [DBStructure]
    [SAPUDO(Name = "AF- Contratos Historial",
          HeaderTableType = typeof(MSS_CONT_HIST),
          ChildTableTypeList = new[] { typeof(MSS_CONT_HIST_LINES) },
          ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document,
          ManageSeries = BoYesNoEnum.tYES
    )]
    public class MSS_CONT_HIST_UDO
    {
    }
}
