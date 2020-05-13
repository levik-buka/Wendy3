using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Model;

namespace Wendy.Tasks.Extensions
{
    public static class MeterConfigExtensions
    {
        public static IEnumerable<MeterConfig> GetMeterConfigHistoryForPeriod(this IEnumerable<MeterConfig> meterConfigs, DateRange period)
        {
            Contract.Requires(meterConfigs != null);

            return meterConfigs.Where(config => config.Intersects(period));
        }
    }
}
