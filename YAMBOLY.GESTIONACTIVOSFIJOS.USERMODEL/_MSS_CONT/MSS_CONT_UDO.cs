using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    [DBStructure]
    [SAPUDO(Name = "AF- Contratos",
          HeaderTableType = typeof(MSS_CONT),
          ChildTableTypeList = new[] { typeof(MSS_CONT_LINES), typeof(MSS_CONT_ADENDAS) },
          ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document,
          ManageSeries = BoYesNoEnum.tYES
    )]
    public class MSS_CONT_UDO
    {
    }
}
