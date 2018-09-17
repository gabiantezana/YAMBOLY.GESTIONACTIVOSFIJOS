using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_ASUA
{
    [DBStructure]
    [SAPUDO(Name = "ASIG_ALMACEN_USUARIO",
        HeaderTableType = typeof(MSS_ASUA),
        ObjectType = SAPbobsCOM.BoUDOObjType.boud_MasterData,
        CanDelete = BoYesNoEnum.tYES
    )]
    public class MSS_ASUA_UDO
    {
        //[SAPField(IsSystemField = true)]
        //public int DocEntry { get; set; }
    }
}
