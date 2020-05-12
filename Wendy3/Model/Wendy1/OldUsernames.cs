using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wendy.Model.Wendy1
{
    public class OldUserNames
    {
        [XmlAttribute]
        public string Name { get; set; }
        public ulong StartConsumption { get; set; }
    }
}
