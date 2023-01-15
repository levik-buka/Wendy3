using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Consumption.Entity
{
    /// <summary>
    /// Meter's read-out
    /// </summary>
    public class ConsumptionReadOut
    {
        /// <summary>
        /// Read-out date
        /// </summary>
        public DateTime ReadOutDate { get; }
        [Newtonsoft.Json.JsonProperty("ReadOut")] // needed because of private
        private ConsumptionValue ReadOut { get; set; }
        [Newtonsoft.Json.JsonProperty("Consumption")]   // needed because of private set
        private ConsumptionValue Consumption { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOutDate"></param>
        /// <param name="estimatedReadout"></param>
        /// <param name="realReadOut"></param>
        public ConsumptionReadOut(DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
        {
            ReadOutDate = readOutDate;
            ReadOut = new ConsumptionValue(estimatedReadout, realReadOut);
            Consumption = new ConsumptionValue(0, 0);
        }

        /// <summary>
        /// Return read-outs
        /// </summary>
        /// <returns></returns>
        public ConsumptionValue GetReadOut()
        {
            Contract.Ensures(ReadOut != null);

            return ReadOut;
        }

        /// <summary>
        /// Return consumption
        /// </summary>
        /// <returns></returns>
        public ConsumptionValue GetConsumption()
        {
            Contract.Ensures(Consumption != null);

            return Consumption;
        }
    }
}
