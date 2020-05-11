using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class BasicFeeConfig
    {
        public decimal Coefficient { get; set; }
        public WaterFee BasicFee { get; set; }
    }
}
