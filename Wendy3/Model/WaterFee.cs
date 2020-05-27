using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    /// <summary>
    /// Class presents water fee
    /// </summary>
    public class WaterFee
    {
        /// <summary>
        /// Clean water fee without VAT
        /// </summary>
        public decimal CleanWaterFee { get; set; }
        /// <summary>
        /// Waste water fee without VAT
        /// </summary>
        public decimal WasteWaterFee { get; set; }

        [JsonIgnore]
        public decimal VAT { get; set; } = 1m;

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
            return ToString(Price.ShowFormat.withoutVAT);
        }

        public string ToString(Price.ShowFormat fmt)
        {
            return String.Format(new NumberFormatInfo(), 
                $"{GetCleanWaterPrice().ToString(fmt)} / {GetWasteWaterPrice().ToString(fmt)} / {GetTotalPrice().ToString(fmt)}");
        }

        /// <summary>
        /// Calculates total sum of water fee
        /// </summary>
        /// <returns></returns>
        public Price GetTotalPrice()
        {
            return new Price(CleanWaterFee + WasteWaterFee, VAT);
        }

        /// <summary>
        /// Return clean wate price with or without VAT
        /// </summary>
        /// <returns></returns>
        public Price GetCleanWaterPrice()
        {
            return new Price(CleanWaterFee, VAT);
        }

        /// <summary>
        /// Return waste water price with or without price
        /// </summary>
        /// <returns></returns>
        public Price GetWasteWaterPrice()
        {
            return new Price(WasteWaterFee, VAT);
        }
    }
}
