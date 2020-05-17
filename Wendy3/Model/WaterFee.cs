using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class WaterFee
    {
        public decimal CleanWaterFee { get; set; }
        public decimal WasteWaterFee { get; set; }

        public WaterFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            CleanWaterFee = cleanWaterFee;
            WasteWaterFee = wasteWaterFee;
        }

        public void ResetWaterFee()
        {
            CleanWaterFee = 0m;
            WasteWaterFee = 0m;
        }
    }
}
