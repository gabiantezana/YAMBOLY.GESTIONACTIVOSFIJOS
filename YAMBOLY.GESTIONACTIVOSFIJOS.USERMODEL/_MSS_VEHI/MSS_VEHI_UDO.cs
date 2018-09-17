using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_VEHIC
{
    [DBStructure]
    [SAPUDO(Name = "MAESTRO_VEHICULOS",
            HeaderTableType = typeof(MSS_VEHI),
            CanCreateDefaultForm = BoYesNoEnum.tYES,
            //CanFind =  BoYesNoEnum.tYES,
            EnableEnhancedForm = BoYesNoEnum.tYES,
            RebuildEnhancedForm = BoYesNoEnum.tYES,
            ObjectType = SAPbobsCOM.BoUDOObjType.boud_MasterData
    )]
    public class MSS_VEHI_UDO
    {

    }
}
