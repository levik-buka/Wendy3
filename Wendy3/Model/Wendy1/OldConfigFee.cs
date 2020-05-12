using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wendy.Model.Wendy1
{
    public class OldConfigFee
    {
        private DateTime? begin;
        [XmlAttribute]
        public DateTime Begin { get { return begin.Value; } set { begin = value; } }
        public bool BeginSpecified { get { return begin.HasValue; } }

        private DateTime? end;
        [XmlAttribute]
        public DateTime End { get { return end.Value; } set { end = value; } }
        public bool EndSpecified { get { return end.HasValue; } }
        public decimal WaterFee { get; set; }
        public decimal WasteFee { get; set; }
        public decimal VAT { get; set; }
    }
}
