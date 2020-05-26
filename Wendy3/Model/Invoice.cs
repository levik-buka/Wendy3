using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1303

namespace Wendy.Model
{
    /// <summary>
    /// Invoice data
    /// </summary>
    public class Invoice
    {
        [Newtonsoft.Json.JsonProperty("ReadOut")]   // needed because of private set
        private ConsumptionReadOut ReadOut { get; set; }
        [Newtonsoft.Json.JsonProperty("BasicFee")]   // needed because of private set
        private WaterFee BasicFee { get; set; }
        [Newtonsoft.Json.JsonProperty("UsageFee")]   // needed because of private set
        private WaterFee UsageFee { get; set; }

        private FeeConfig feeConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOutDate"></param>
        /// <param name="estimatedReadout"></param>
        /// <param name="realReadOut"></param>
        public Invoice(DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
        {
            ReadOut = new ConsumptionReadOut(readOutDate, estimatedReadout, realReadOut);
            BasicFee = new WaterFee(0, 0);
            UsageFee = new WaterFee(0, 0);
        }

        /// <summary>
        /// Return consumption of invoice
        /// </summary>
        /// <returns></returns>
        public ConsumptionValue GetConsumption()
        {
            Contract.Requires(ReadOut != null);

            return ReadOut.GetConsumption();
        }

        /// <summary>
        /// Return read-out date of invoice
        /// </summary>
        /// <returns></returns>
        public DateTime GetReadOutDate()
        {
            Contract.Requires(ReadOut != null);

            return ReadOut.ReadOutDate;
        }

        /// <summary>
        /// Return read-outs of invoice
        /// </summary>
        /// <returns></returns>
        public ConsumptionValue GetReadOut()
        {
            Contract.Requires(ReadOut != null);

            return ReadOut.GetReadOut();
        }

        /// <summary>
        /// Return basic water fees of invoice
        /// </summary>
        /// <returns></returns>
        public WaterFee GetBasicFee()
        {
            Contract.Ensures(BasicFee != null);

            return BasicFee;
        }

        /// <summary>
        /// Return usage water fee of invoice
        /// </summary>
        /// <returns></returns>
        public WaterFee GetUsageFee()
        {
            Contract.Ensures(UsageFee != null);

            return UsageFee;
        }

        /// <summary>
        /// Return fee plan used by invoice
        /// </summary>
        /// <returns></returns>
        public FeeConfig GetFeeConfig()
        {
            return feeConfig;
        }

        /// <summary>
        /// Set fee plan used by invoice
        /// </summary>
        /// <param name="feeConfig"></param>
        public void SetFeeConfig(FeeConfig feeConfig)
        {
            this.feeConfig = feeConfig;
        }

        /// <summary>
        /// Return total water fee of invoice
        /// </summary>
        /// <returns></returns>
        public TotalFee GetTotalFee()
        {
            if (feeConfig == null)
            {
                throw new InvalidOperationException("Invoice without knowledge about fee configuration");
            }

            return new TotalFee(GetBasicFee().GetTotalFee(feeConfig.VAT).VATLessFee + GetUsageFee().GetTotalFee(feeConfig.VAT).VATLessFee, feeConfig.VAT);
        }

    }
}
