using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class Invoice
    {
        public ConsumptionReadOut ReadOut { get; set; }
        public WaterFee BasicFee { get; set; }
        public WaterFee UsageFee { get; set; }
    }
}
