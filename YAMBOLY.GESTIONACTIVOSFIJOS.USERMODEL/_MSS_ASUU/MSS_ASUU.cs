using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_ASUU
{
    [SAPTable(TableDescription = "Asig. usuarios aprobadores", TableType = BoUTBTableType.bott_MasterData)]
    public class MSS_ASUU
    {
        [SAPField(IsSystemField = true)]
        public string Code { get; set; }

        [SAPField(IsSystemField = true)]
        public string Name { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [SAPField(
            FieldDescription = "Usuario creador")]
        public string MSS_USUA { get; set; }

        /// <summary>
        /// Almacén
        /// </summary>
        [SAPField(FieldDescription = "Usuario aprobador")]
        public string MSS_APRO { get; set; }
    }
}
