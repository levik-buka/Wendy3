using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class MeterConfig : DateRange
    {
        public ulong StartReadOut { get; set; }
        public ulong? EndReadOut { get; set; }

        public static MeterConfig CreateEmpty()
        {
            return new MeterConfig {
                Id = 0L,
                Start = new DateTime(),
                StartReadOut = 0U
            };
        }

        public long GetConsumption()
        {
            return Convert.ToInt64((EndReadOut ?? 0L) - StartReadOut);
        }
    }
}
