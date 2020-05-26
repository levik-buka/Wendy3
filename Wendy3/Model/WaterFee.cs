using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    /// <summary>
    /// Class presents water fee
    /// </summary>
    public class WaterFee
    {
        /// <summary>
        /// VATless clean water fee
        /// </summary>
        public decimal CleanWaterFee { get; set; }
        /// <summary>
        /// VATless waste water fee
        /// </summary>
        public decimal WasteWaterFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cleanWaterFee"></param>
        /// <param name="wasteWaterFee"></param>
        public WaterFee(decimal cleanWaterFee, decimal wasteWaterFee)
        {
            CleanWaterFee = cleanWaterFee;
            WasteWaterFee = wasteWaterFee;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetWaterFee()
        {
            CleanWaterFee = 0m;
            WasteWaterFee = 0m;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(new NumberFormatInfo(), 
                $"{CleanWaterFee,10:C2} / {WasteWaterFee,10:C2} / {(CleanWaterFee + WasteWaterFee),10:C2}");
        }

        /// <summary>
        /// Calculates total sum of water fee
        /// </summary>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public TotalFee GetTotalFee(decimal VAT)
        {
            return new TotalFee(CleanWaterFee + WasteWaterFee, VAT);
        }
    }
}
