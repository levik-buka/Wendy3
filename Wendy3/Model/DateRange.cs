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
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

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


    }
}
