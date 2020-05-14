using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
