using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    /// <summary>
    /// Water consumption
    /// </summary>
    public class ConsumptionValue
    {
        /// <summary>
        /// Estimated water consumption
        /// </summary>
        public ulong Estimated { get; set; }
        /// <summary>
        /// Real water consumption
        /// </summary>
        public ulong Real { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estimatedConsumption"></param>
        /// <param name="realConsumption"></param>
        public ConsumptionValue(ulong estimatedConsumption, ulong realConsumption)
        {
            Estimated = estimatedConsumption;
            Real = realConsumption;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(new NumberFormatInfo(), $"{Estimated} / {Real}");
        }
    }
}
