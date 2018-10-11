using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CONT
{
    [DBStructure]
    [SAPUDO(Name = "Contrato de concesión de activos fijos",
          HeaderTableType = typeof(MSS_CONT),
          ChildTableTypeList = new[] { typeof(MSS_CONT_LINES) },
          ObjectType = SAPbobsCOM.BoUDOObjType.boud_Document,
          ManageSeries = BoYesNoEnum.tYES
    )]
    public class MSS_CONT_UDO
    {
    }
}
