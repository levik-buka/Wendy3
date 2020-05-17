using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class FeeConfig : DateRange
    {
        public BasicFeeConfig MonthlyBasicFee { get; set; } = new BasicFeeConfig(0, 0, 0);
        public WaterFee MonthlyUsageFee { get; set; } = new WaterFee(0, 0);
        public decimal VAT { get; set; }
        public bool VATIncludedIntoMonthlyFees { get; set; }

        public decimal GetMonthlyCleanWaterBasicFeeWithVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetCleanWaterBasicFeeWithVAT(GetVATInMonthlyFees());
        }

        public decimal GetMonthlyCleanWaterBasicFeeWithoutVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetCleanWaterBasicFeeWithoutVAT(GetVAT());
        }

        public decimal GetMonthlyWasteWaterBasicFeeWithVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetWasteWaterBasicFeeWithVAT(GetVATInMonthlyFees());
        }

        public decimal GetMonthlyWasteWaterBasicFeeWithoutVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetWasteWaterBasicFeeWithoutVAT(GetVAT());
        }

        public decimal GetMonthlyCleanWaterUsageFeeWithVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.CleanWaterFee * GetVATInMonthlyFees();
        }

        public decimal GetMonthlyCleanWaterUsageFeeWithoutVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.CleanWaterFee / GetVAT();
        }

        public decimal GetMonthlyWasteWaterUsageFeeWithVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.WasteWaterFee * GetVATInMonthlyFees();
        }

        public decimal GetMonthlyWasteWaterUsageFeeWithoutVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.WasteWaterFee / GetVAT();
        }

        private decimal GetVATInMonthlyFees()
        {
            return VATIncludedIntoMonthlyFees ? 1m : (1m + (VAT / 100));
        }

        private decimal GetVAT()
        {
            return VATIncludedIntoMonthlyFees ? (1m + (VAT / 100)) : 1m;
        }
    }
}
