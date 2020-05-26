using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#pragma warning disable CS1591

namespace Wendy.Model.Wendy1
{
    [XmlRoot("ElectraData")]
    public class OldWendyFile
    {
        public OldConfig Config { get; set; }
        [XmlElement("Invoices")]
        public List<OldInvoices> Invoices { get; } = new List<OldInvoices>();
        [XmlElement("Users")]
        public List<OldUsers> Users { get; } = new List<OldUsers>();
        [XmlElement("UserNames")]
        public List<OldUserNames> UserNames { get; } = new List<OldUserNames>();
        [XmlElement("ConfigFee")]
        public List<OldConfigFee> ConfigFees { get; } = new List<OldConfigFee>();
    }
}
