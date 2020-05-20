using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class ConsumptionValue
    {
        public ulong Estimated { get; set; }
        public ulong Real { get; set; }

        public ConsumptionValue(ulong estimatedConsumption, ulong realConsumption)
        {
            Estimated = estimatedConsumption;
            Real = realConsumption;
        }

        public override string ToString()
        {
            return String.Format(new NumberFormatInfo(), $"{Estimated} / {Real}");
        }
    }
}
