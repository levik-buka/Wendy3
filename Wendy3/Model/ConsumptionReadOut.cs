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
        public ConsumptionValue ReadOut { get; }
        public ConsumptionValue Consumption { get; private set; }

        public ConsumptionReadOut(DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
        {
            ReadOutDate = readOutDate;
            ReadOut = new ConsumptionValue(estimatedReadout, realReadOut);
            Consumption = null;
        }

        public void ResetReadOut()
        {
            ReadOut.Estimated = 0;
            ReadOut.Real = 0;
            ResetConsumption();
        }

        public void ResetConsumption()
        {
            Consumption = null;
        }
    }
}
