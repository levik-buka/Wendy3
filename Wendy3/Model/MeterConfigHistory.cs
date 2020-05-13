using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    public class MeterConfigHistory
    {
        public List<MeterConfig> MeterConfigs { get; }

        public MeterConfigHistory()
        {
            MeterConfigs = new List<MeterConfig>();
        }
        public MeterConfigHistory(IEnumerable<MeterConfig> meterConfigs)
        {
            MeterConfigs = meterConfigs.ToList();
        }

        public ulong GetConsumptionUntilDate(DateTime invoiceStartDate, DateTime readOutDate)
        {
            IEnumerable<MeterConfig> configs = MeterConfigs.GetMeterConfigHistoryForPeriod(new DateRange { Start = invoiceStartDate, End = readOutDate });
            
            long consumption = configs.Sum(meter => meter.GetConsumption());

            return Convert.ToUInt64(consumption);
        }
    }
}
