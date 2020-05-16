using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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

        public static IEnumerable<FeeConfig> GetFeeConfigHistoryInPeriod(this IEnumerable<FeeConfig> feeConfigs, DateRange period)
        {
            Contract.Requires(feeConfigs != null);

            return feeConfigs.Where(config => config.In(period));
        }
    }
}
