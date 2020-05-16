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

        public static IEnumerable<MeterConfig> GetMeterConfigHistoryInPeriod(this IEnumerable<MeterConfig> meterConfigs, DateRange period)
        {
            Contract.Requires(meterConfigs != null);

            return meterConfigs.Where(config => config.In(period));
        }

        public static IEnumerable<MeterConfig> GetMeterConfigHistoryByOwner(this IEnumerable<UserMeterConfigHistory> userMeterConfigs, string owner)
        {
            Contract.Requires(userMeterConfigs != null);

            return userMeterConfigs.First(meter => meter.MeterUser == owner).MeterConfigs;
        }

    }
}
