using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class ConsumptionReadOut
    {
        public DateTime ReadOutDate { get; }
        [Newtonsoft.Json.JsonProperty("ReadOut")] // needed because of private
        private ConsumptionValue ReadOut { get; set; }
        [Newtonsoft.Json.JsonProperty("Consumption")]   // needed because of private set
        public ConsumptionValue Consumption { get; private set; }

        public ConsumptionReadOut(DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
        {
            ReadOutDate = readOutDate;
            ReadOut = new ConsumptionValue(estimatedReadout, realReadOut);
            Consumption = new ConsumptionValue(0, 0);
        }

        public void SetReadOut(ulong estimatedReadout, ulong realReadOut)
        {
            ReadOut.Estimated = estimatedReadout;
            ReadOut.Real = realReadOut;
        }

        public ConsumptionValue GetReadOut()
        {
            return ReadOut;
        }

    }
}
