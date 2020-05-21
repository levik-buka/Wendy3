using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Wendy.Model
{
    public class DateRange
    {
        public long Id { get; set; }
        private DateTime startDate;
        public DateTime Start { get { return startDate; } set { startDate = value.Date; } }  // pick up only date, not time
        private DateTime? endDate;
        public DateTime? End { get { return endDate; } set { endDate = value?.Date; } }     // pick up only date, not time

        public string PeriodToString()
        {
            var formatProvider = new DateTimeFormatInfo();

            return String.Format(formatProvider, $"{Start.ToString("dd.MM.yyyy", formatProvider)} - {End?.ToString("dd.MM.yyyy", formatProvider)}");
        }

        public decimal GetMonths()
        {
            if (!End.HasValue)
            {
                throw new InvalidOperationException($"Cannot calculate months in DateRange {startDate:d} - inf, because of missing end date");
            }

            if (End.Value == Start) return 0;

            return ((End.Value - Start).Days + 1) * 12 / 365m;    // end date included
        }

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

        public bool Intersects(DateRange period)
        {
            Contract.Requires(period != null);

            if (!Covers(period.Start))
            {
                return period.Covers(Start);
            }

            return true;
        }

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
