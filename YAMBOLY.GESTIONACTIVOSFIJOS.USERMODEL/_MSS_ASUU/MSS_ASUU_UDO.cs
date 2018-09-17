using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_ASUU
{
    [DBStructure]
    [SAPUDO(Name = "ASIG_USUARIOAPROBADOR_USUARIO",
        HeaderTableType = typeof(MSS_ASUU),
        ObjectType = SAPbobsCOM.BoUDOObjType.boud_MasterData,
        CanDelete = BoYesNoEnum.tYES
    )]
    public class MSS_ASUU_UDO
    {
    }
}
