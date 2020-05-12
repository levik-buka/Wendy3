using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class WaterFee
    {
        public decimal CleanWaterFee { get; }
        public decimal WasteWaterFee { get; }

        public WaterFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            CleanWaterFee = cleanWaterFee;
            WasteWaterFee = wasteWaterFee;
        }
    }
}
