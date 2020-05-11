using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class ConsumptionReadOut
    {
        public DateTime ReadOutDate { get; set; }
        public ConsumptionValue ReadOut { get; set; }
        public ConsumptionValue Consumption { get; set; }
    }
}
