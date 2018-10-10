using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class SapIntefaceHelper
    {
    }

    public interface ISAPForm : ISAPEvents
    {
        //Form GetForm();
    }

    public interface ISAPEvents
    {
        bool HandleItemEvents(SAPbouiCOM.ItemEvent itemEvent);
        bool HandleFormDataEvents(SAPbouiCOM.BusinessObjectInfo oBusinessObjectInfo);
        bool HandleMenuDataEvents(SAPbouiCOM.MenuEvent menuEvent);
        bool HandleRightClickEvent(SAPbouiCOM.ContextMenuInfo menuInfo);
    }

    public interface ISAPTable
    {
        String TableName { get; set; }
        String TableDescription { get; set; }
    }


    public interface ISAPField
    {
        String FieldDescription { get; set; }
        BoFieldTypes FieldType { get; set; }
        BoFldSubTypes FieldSubType { get; set; }
        Int32 FieldSize { get; set; }
        BoYesNoEnum IsRequired { get; set; }
        String[] ValidValues { get; set; }
        String[] ValidDescription { get; set; }
        String DefaultValue { get; set; }
        String VinculatedTable { get; set; }
        Boolean IsSearchField { get; set; }
    }
}
