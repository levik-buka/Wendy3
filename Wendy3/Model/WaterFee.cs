using System;
using System.Collections.Generic;
using System.Globalization;
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

        public override string ToString()
        {
            return String.Format(new NumberFormatInfo(), 
                $"{CleanWaterFee,10:C2} / {WasteWaterFee,10:C2} / {(CleanWaterFee + WasteWaterFee),10:C2}");
        }

        public TotalFee GetTotalFee(decimal VAT)
        {
            return new TotalFee(CleanWaterFee + WasteWaterFee, VAT);
        }
    }
}
