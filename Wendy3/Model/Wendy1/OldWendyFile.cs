using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wendy.Model.Wendy1
{
    [XmlRoot("ElectraData")]
    public class OldWendyFile
    {
        public OldConfig Config { get; set; }
        [XmlElement("Invoices")]
        public List<OldInvoices> Invoices { get; set; }
        [XmlElement("Users")]
        public List<OldUsers> Users { get; set; }
        [XmlElement("UserNames")]
        public List<OldUserNames> UserNames { get; set; }
        [XmlElement("ConfigFee")]
        public List<OldConfigFee> ConfigFees { get; set; }
    }
}
