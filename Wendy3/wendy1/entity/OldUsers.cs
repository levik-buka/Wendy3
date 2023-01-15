using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#pragma warning disable CS1591

namespace Wendy.Wendy1.Entity
{
    public class OldUsers
    {
        [XmlAttribute("Id")]
        public long InvoiceId { get; set; }
        public string User { get; set; }
        public ulong Consumption { get; set; }
        public decimal WaterFee { get; set; }
        public decimal WasteFee { get; set; }
        public decimal BasicFee { get; set; }
        public bool Balanced { get; set; }
    }
}
