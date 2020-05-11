using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class FeeConfig : DateRange
    {
        public BasicFeeConfig MonthlyBasicFee { get; set; }
        public WaterFee MonthlyUsageFee { get; set; }
        public decimal VAT { get; set; }
        public bool VATIncludedIntoMonthlyFees { get; set; }
    }
}
