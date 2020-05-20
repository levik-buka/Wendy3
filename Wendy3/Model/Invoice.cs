using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
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

        public TotalFee GetTotalFee(decimal VAT)
        {
            return new TotalFee(GetBasicFee().GetTotalFee(VAT).VATLessFee + GetUsageFee().GetTotalFee(VAT).VATLessFee, VAT);
        }

    }
}
