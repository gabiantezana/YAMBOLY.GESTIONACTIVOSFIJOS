using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.GESTIONACTIVOSFIJOS.USERMODEL._RDR1
{
    [SAPTable(IsSystemTable = true)]
    public class RDR1
    {
        [SAPField(IsSystemField = true)]
        public string DocEntry { get; set; }

        [SAPField(IsSystemField = true)]
        public string DocNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string LineNum { get; set; }

        [SAPField(IsSystemField = true)]
        public string CardCode { get; set; }

        [SAPField(IsSystemField = true)]
        public string CardName { get; set; }

        [SAPField(IsSystemField = true)]
        public string ShipToCode { get; set; }

        [SAPField(IsSystemField = true)]
        public string Address { get; set; }

        [SAPField(IsSystemField = true)]
        public string ItemCode { get; set; }

        [SAPField(IsSystemField = true)]
        public string Dscription { get; set; }

        [SAPField(IsSystemField = true)]
        public string unitMsr { get; set; }

        [SAPField(IsSystemField = true)]
        public string OpenCreQty { get; set; }

        [SAPField(IsSystemField = true)]
        public string OnHand { get; set; }

        [SAPField(IsSystemField = true)]
        public string SWeight1 { get; set; }

        [SAPField(IsSystemField = true)]
        public string SVolume { get; set; }

    }
}
