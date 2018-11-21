using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPADDON.HELPER
{
    public static class SapExtensionHelper
    {
        #region Matrix

        public static int? GetRowNumberFocus(this Matrix matrix)
        {
            int? rowNumber = matrix.GetCellFocus()?.rowIndex;
            return rowNumber == -1 ? null : rowNumber;
        }

        public static string GetCurrentItemUID(this Form form)
        {
            return form.ActiveItem;

            for (int i = 0; i < form.Items.Count; i++)
            {
                var item = form.Items.Item(i).Specific;
                if (item is Matrix)
                {
                    var rowNumberFocus = (item as Matrix).GetRowNumberFocus();
                    if (rowNumberFocus != null)
                        return form.Items.Item(i).UniqueID;
                }
            }
            return null;
        }

        #endregion
    }
}
