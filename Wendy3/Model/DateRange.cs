using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wendy.Model
{
    public class DateRange
    {
        public long Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
    }
}
