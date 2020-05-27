using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    /// <summary>
    /// Fee plan
    /// </summary>
    public class FeeConfig : DateRange
    {
        /// <summary>
        /// Monthly basic water fee
        /// </summary>
        public BasicFeeConfig MonthlyBasicFee { get; set; } = new BasicFeeConfig(0, 0, 0);
        /// <summary>
        /// Monthly usage water fee
        /// </summary>
        public WaterFee MonthlyUsageFee { get; set; } = new WaterFee(0, 0);
        /// <summary>
        /// VAT percentage
        /// </summary>
        public decimal VAT { get; set; }
        /// <summary>
        /// Is VAT included into monthly water feees
        /// </summary>
        public bool VATIncludedIntoMonthlyFees { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyCleanWaterBasicFeeWithVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetCleanWaterBasicFeeWithVAT(GetVATInMonthlyFees());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyCleanWaterBasicFeeWithoutVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetCleanWaterBasicFeeWithoutVAT(GetVAT());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyWasteWaterBasicFeeWithVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetWasteWaterBasicFeeWithVAT(GetVATInMonthlyFees());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyWasteWaterBasicFeeWithoutVAT()
        {
            Contract.Requires(MonthlyBasicFee != null);

            return MonthlyBasicFee.GetWasteWaterBasicFeeWithoutVAT(GetVAT());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyCleanWaterUsageFeeWithVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.CleanWaterFee * GetVATInMonthlyFees();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyCleanWaterUsageFeeWithoutVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.CleanWaterFee / GetVAT();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyWasteWaterUsageFeeWithVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.WasteWaterFee * GetVATInMonthlyFees();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal GetMonthlyWasteWaterUsageFeeWithoutVAT()
        {
            Contract.Requires(MonthlyUsageFee != null);

            return MonthlyUsageFee.WasteWaterFee / GetVAT();
        }

        private decimal GetVATInMonthlyFees()
        {
            return VATIncludedIntoMonthlyFees ? 1m : VAT.VATPercentToDecimal();
        }

        private decimal GetVAT()
        {
            return VATIncludedIntoMonthlyFees ? VAT.VATPercentToDecimal() : 1m;
        }
    }
}
