using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wendy.Utils.Entity
{
    /// <summary>
    /// Represents date period
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        private DateTime startDate;
        /// <summary>
        /// Start date of period
        /// </summary>
        public DateTime Start { get { return startDate; } set { startDate = value.Date; } }  // pick up only date, not time
        private DateTime? endDate;
        /// <summary>
        /// End date of period
        /// </summary>
        public DateTime? End { get { return endDate; } set { endDate = value?.Date; } }     // pick up only date, not time

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string PeriodToString()
        {
            var formatProvider = new DateTimeFormatInfo();

            return String.Format(formatProvider, $"{Start.ToString("dd.MM.yyyy", formatProvider)} - {End?.ToString("dd.MM.yyyy", formatProvider)}");
        }

        /// <summary>
        /// Converts days of period to months
        /// </summary>
        /// <returns></returns>
        public decimal GetMonths()
        {
            if (!End.HasValue)
            {
                throw new InvalidOperationException($"Cannot calculate months in DateRange {startDate:d} - inf, because of missing end date");
            }

            if (End.Value == Start) return 0;

            return ((End.Value - Start).Days + 1) * 12 / 365m;    // end date included
        }

        /// <summary>
        /// Is date in the period
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool Covers(DateTime date)
        {
            if (Start <= date)
            {
                if (!End.HasValue || date <= End)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Does two periods interset
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool Intersects(DateRange period)
        {
            Contract.Requires(period != null);

            if (!Covers(period.Start))
            {
                return period.Covers(Start);
            }

            return true;
        }

        /// <summary>
        /// Does period starts and end inside the other period
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool In(DateRange period)
        {
            Contract.Requires(period != null);

            if (period.Covers(Start))
            {
                if (!period.End.HasValue)
                {
                    return true;
                }
                if (!End.HasValue)
                {
                    return false;
                }
                
                return period.Covers(End.Value);
            }

            return false;
        }

    }
}
