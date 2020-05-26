using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    /// <summary>
    /// Basic water fee plan
    /// </summary>
    public class BasicFeeConfig
    {
        /// <summary>
        /// Basic fee coefficient
        /// </summary>
        public decimal Coefficient { get; set; }
        /// <summary>
        /// Basic water fee
        /// </summary>
        public WaterFee BasicFee { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coefficient"></param>
        /// <param name="cleanWaterFee"></param>
        /// <param name="wasteWaterFee"></param>
        public BasicFeeConfig(decimal coefficient, decimal cleanWaterFee, decimal wasteWaterFee)
        {
            Coefficient = coefficient;
            BasicFee = new WaterFee(cleanWaterFee, wasteWaterFee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public decimal GetCleanWaterBasicFeeWithVAT(decimal VAT)
        {
            Contract.Requires(BasicFee != null);

            return Coefficient * BasicFee.CleanWaterFee * VAT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public decimal GetCleanWaterBasicFeeWithoutVAT(decimal VAT)
        {
            Contract.Requires(BasicFee != null);

            return Coefficient * BasicFee.CleanWaterFee / VAT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public decimal GetWasteWaterBasicFeeWithVAT(decimal VAT)
        {
            Contract.Requires(BasicFee != null);

            return Coefficient * BasicFee.WasteWaterFee * VAT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public decimal GetWasteWaterBasicFeeWithoutVAT(decimal VAT)
        {
            Contract.Requires(BasicFee != null);

            return Coefficient * BasicFee.WasteWaterFee / VAT;
        }
    }
}
