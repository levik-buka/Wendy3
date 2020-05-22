using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class Invoice
    {
        [Newtonsoft.Json.JsonProperty("ReadOut")]   // needed because of private set
        private ConsumptionReadOut ReadOut { get; set; }
        [Newtonsoft.Json.JsonProperty("BasicFee")]   // needed because of private set
        private WaterFee BasicFee { get; set; }
        [Newtonsoft.Json.JsonProperty("UsageFee")]   // needed because of private set
        private WaterFee UsageFee { get; set; }

        private FeeConfig feeConfig;

        public Invoice(DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
        {
            ReadOut = new ConsumptionReadOut(readOutDate, estimatedReadout, realReadOut);
            BasicFee = new WaterFee(0, 0);
            UsageFee = new WaterFee(0, 0);
        }

        public ConsumptionValue GetConsumption()
        {
            Contract.Requires(ReadOut != null);

            return ReadOut.GetConsumption();
        }

        public DateTime GetReadOutDate()
        {
            Contract.Requires(ReadOut != null);

            return ReadOut.ReadOutDate;
        }

        public ConsumptionValue GetReadOut()
        {
            Contract.Requires(ReadOut != null);

            return ReadOut.GetReadOut();
        }

        public WaterFee GetBasicFee()
        {
            Contract.Ensures(BasicFee != null);

            return BasicFee;
        }

        public WaterFee GetUsageFee()
        {
            Contract.Ensures(UsageFee != null);

            return UsageFee;
        }

        public FeeConfig GetFeeConfig()
        {
            return feeConfig;
        }

        public void SetFeeConfig(FeeConfig feeConfig)
        {
            this.feeConfig = feeConfig;
        }

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
