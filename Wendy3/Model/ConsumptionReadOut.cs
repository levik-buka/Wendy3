using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        private ConsumptionValue Consumption { get; set; }

        public ConsumptionReadOut(DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
        {
            ReadOutDate = readOutDate;
            ReadOut = new ConsumptionValue(estimatedReadout, realReadOut);
            Consumption = new ConsumptionValue(0, 0);
        }

        public ConsumptionValue GetReadOut()
        {
            Contract.Ensures(ReadOut != null);

            return ReadOut;
        }

        public ConsumptionValue GetConsumption()
        {
            Contract.Ensures(Consumption != null);

            return Consumption;
        }
    }
}
