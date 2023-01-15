using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Utils.Entity;

namespace Wendy.Meter.Entity
{
    /// <summary>
    /// Meter configuration
    /// </summary>
    public class MeterConfig : DateRange
    {
        /// <summary>
        /// read out on start of the period
        /// </summary>
        public ulong StartReadOut { get; set; }
        /// <summary>
        /// read out (if available) on end of the period
        /// </summary>
        public ulong? EndReadOut { get; set; }

        /// <summary>
        /// Create empty meter configuration
        /// </summary>
        /// <returns></returns>
        public static MeterConfig CreateEmpty()
        {
            return new MeterConfig {
                Id = 0L,
                Start = new DateTime(),
                StartReadOut = 0U
            };
        }

        /// <summary>
        /// Returns consumption of the period
        /// </summary>
        /// <returns></returns>
        public long GetConsumption()
        {
            return Convert.ToInt64((EndReadOut ?? 0L) - StartReadOut);
        }
    }
}
