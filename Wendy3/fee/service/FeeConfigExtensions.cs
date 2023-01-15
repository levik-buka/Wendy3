using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using Wendy.Fee.Entity;
using Wendy.Utils.Entity;

namespace Wendy.Fee.Service
{
    /// <summary>
    /// Extension methods for fee plans
    /// </summary>
    public static class FeeConfigExtensions
    {
        /// <summary>
        /// Return fee plans active at the time of the period
        /// </summary>
        /// <param name="feeConfigs"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<FeeConfig> GetFeeConfigHistoryForPeriod(this IEnumerable<FeeConfig> feeConfigs, DateRange period)
        {
            Contract.Requires(feeConfigs != null);

            return feeConfigs.Where(config => config.Intersects(period));
        }

        /// <summary>
        /// Return first and the only one fee plan of the period. Throws exceptions if no plans to too much plans
        /// </summary>
        /// <param name="feeConfigs"></param>
        /// <param name="period"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Return fee plans started and ended in the period
        /// </summary>
        /// <param name="feeConfigs"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<FeeConfig> GetFeeConfigHistoryInPeriod(this IEnumerable<FeeConfig> feeConfigs, DateRange period)
        {
            Contract.Requires(feeConfigs != null);

            return feeConfigs.Where(config => config.In(period));
        }

        /// <summary>
        /// Round decimals to 2 digits
        /// </summary>
        /// <param name="fee"></param>
        /// <returns></returns>
        public static decimal RoundToCents(this decimal fee)
        {
            return Math.Round(fee, 2);
        }

        /// <summary>
        /// Calculate price with VAT
        /// </summary>
        /// <param name="VATLessFee"></param>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public static decimal AddVAT(this decimal VATLessFee, decimal VAT)
        {
            return VATLessFee * VAT.VATPercentToDecimal();
        }

        /// <summary>
        /// Converts VAT percent to decimal presentation
        /// </summary>
        /// <param name="VAT"></param>
        /// <returns></returns>
        public static decimal VATPercentToDecimal(this decimal VAT)
        {
            return (1m + VAT / 100);
        }
    }
}
