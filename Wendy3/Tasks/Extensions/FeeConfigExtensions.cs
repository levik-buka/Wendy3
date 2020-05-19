using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Model;

namespace Wendy.Tasks.Extensions
{
    public static class FeeConfigExtensions
    {
        public static IEnumerable<FeeConfig> GetFeeConfigHistoryForPeriod(this IEnumerable<FeeConfig> feeConfigs, DateRange period)
        {
            Contract.Requires(feeConfigs != null);

            return feeConfigs.Where(config => config.Intersects(period));
        }

        public static FeeConfig GetFeeConfigOfPeriodOrThrowException(this IEnumerable<FeeConfig> feeConfigs, DateRange period)
        {
            Contract.Requires(feeConfigs != null);
            Contract.Requires(period != null);

            IEnumerable<FeeConfig> configs = feeConfigs.GetFeeConfigHistoryForPeriod(period);
            if (!configs.Any())
            {
                throw new InvalidDataException($"Unable to find fee configurations for period {period.Start.ToShortDateString()} - {period.End?.ToShortDateString()}");
            }
            if (configs.Count() > 1)
            {
                throw new InvalidDataException($"Unable to find only one fee configuration for period {period.Start.ToShortDateString()} - {period.End?.ToShortDateString()}");
            }

            return configs.First();
        }

        public static IEnumerable<FeeConfig> GetFeeConfigHistoryInPeriod(this IEnumerable<FeeConfig> feeConfigs, DateRange period)
        {
            Contract.Requires(feeConfigs != null);

            return feeConfigs.Where(config => config.In(period));
        }

        public static decimal RoundToCents(this decimal fee)
        {
            return Math.Round(fee, 2);
        }
    }
}
