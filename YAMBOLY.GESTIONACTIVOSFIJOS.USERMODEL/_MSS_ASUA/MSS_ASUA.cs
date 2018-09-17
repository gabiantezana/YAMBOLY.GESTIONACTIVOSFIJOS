using SAPbobsCOM;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_ASUA
{
    [SAPTable(TableDescription = "Asig. Almacén a usuario", TableType = BoUTBTableType.bott_MasterData)]
    public class MSS_ASUA
    {
        [SAPField(IsSystemField = true)]
        public string Code { get; set; }

        [SAPField(IsSystemField = true)]
        public string Name { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [SAPField(FieldDescription = "Usuario")]
        public string MSS_USUA { get; set; }

        /// <summary>
        /// Almacén
        /// </summary>
        [SAPField(FieldDescription = "Almacén")]
        public string MSS_ALMA { get; set; }
    }
}
