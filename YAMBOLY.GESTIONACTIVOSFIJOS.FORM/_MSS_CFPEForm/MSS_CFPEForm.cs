
using YAMBOLY.GESTIONACTIVOSFIJOS.HELPER;
using SAPbouiCOM;
using System.Collections.Generic;
using YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._MSS_CFPE;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.FORM._MSS_CFPEForm
{
    class MSS_CFPEForm : BaseApplication, ISAPForm
    {
        public const string FormType = nameof(MSS_CFPE);
        private Form _Form { get; set; }

        public MSS_CFPEForm(Dictionary<string, ISAPForm> dictionary)
        {
            _Form = SapFormHelper.CreateForm(GetApplication(), XMLHelper.GetResourceString(System.Reflection.Assembly.GetExecutingAssembly(), this.GetType().Name), FormType);
            dictionary.Add(_Form.UniqueID, this);
        }

        public bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent) { return true; }
        public bool HandleItemPressed(SAPbouiCOM.ItemEvent oEvent) { return true; }
        public bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo) { return true; }
        public bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent) { return true; }
        public bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo) { return true; }
    }
}
