using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.HELPER
{
    public class EntityHelper
    {
    }
    
    public class SubMenuItem
    {
        public string Title { get; set; }
        public string UniqueId { get; set; }
    }
    
    public class ItemQuantity
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
    }

    public class EstadoDespacho
    {
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public string ErrorSAP { get; set; }
    }
}
