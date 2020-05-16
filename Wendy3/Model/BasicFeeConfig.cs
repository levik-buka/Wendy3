using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class BasicFeeConfig
    {
        public decimal Coefficient { get; set; }
        public WaterFee BasicFee { get; }

        public BasicFeeConfig(decimal coefficient, decimal cleanWaterFee, decimal wasteWaterFee)
        {
            Coefficient = coefficient;
            BasicFee = new WaterFee(cleanWaterFee, wasteWaterFee);
        }

        public decimal GetCleanWaterBasicFeeWithVAT(decimal VAT)
        {
            Contract.Requires(BasicFee != null);

            return Coefficient * BasicFee.CleanWaterFee * VAT;
        }

        public decimal GetWasteWaterBasicFeeWithVAT(decimal VAT)
        {
            Contract.Requires(BasicFee != null);

            return Coefficient * BasicFee.WasteWaterFee * VAT;
        }
    }
}
