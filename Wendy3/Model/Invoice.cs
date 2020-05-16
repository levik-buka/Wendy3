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
        public WaterFee BasicFee { get; set; }
        public WaterFee UsageFee { get; set; }

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
    }
}
