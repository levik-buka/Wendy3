using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Wendy.Meter.Entity;
using Wendy.Utils.Entity;

namespace Wendy.Meter.Service
{
    /// <summary>
    /// Extension methods for meter configurations
    /// </summary>
    public static class MeterConfigExtensions
    {
        /// <summary>
        /// Get meter configurations actived at the time of period
        /// </summary>
        /// <param name="meterConfigs"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<MeterConfig> GetMeterConfigHistoryForPeriod(this IEnumerable<MeterConfig> meterConfigs, DateRange period)
        {
            Contract.Requires(meterConfigs != null);

            return meterConfigs.Where(config => config.Intersects(period));
        }

        /// <summary>
        /// Get meter configurations which started and ended in the period
        /// </summary>
        /// <param name="meterConfigs"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<MeterConfig> GetMeterConfigHistoryInPeriod(this IEnumerable<MeterConfig> meterConfigs, DateRange period)
        {
            Contract.Requires(meterConfigs != null);

            return meterConfigs.Where(config => config.In(period));
        }

        /// <summary>
        /// Get meter configurations by meter's user
        /// </summary>
        /// <param name="userMeterConfigs"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static IEnumerable<MeterConfig> GetMeterConfigHistoryByOwner(this IEnumerable<UserMeterConfigHistory> userMeterConfigs, string owner)
        {
            Contract.Requires(userMeterConfigs != null);

            return userMeterConfigs.First(meter => meter.MeterUser == owner).MeterConfigs;
        }
    }
}
