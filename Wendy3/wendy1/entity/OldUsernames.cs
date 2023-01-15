using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

#pragma warning disable CS1591

namespace Wendy.Wendy1.Entity
{
    public class OldUserNames
    {
        [XmlAttribute]
        public string Name { get; set; }
        public ulong StartConsumption { get; set; }
    }
}
