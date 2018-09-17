using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class SapEnumHelper
    {
    }
    public enum SAPPermissionType
    {
        pt_father,
        pt_child
    }

    public enum SAPState
    {
        Active = 0,
        Inactive = 1,
    }
}
