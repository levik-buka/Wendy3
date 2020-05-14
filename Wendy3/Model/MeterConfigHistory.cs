using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    public class MeterConfigHistory
    {
        public List<MeterConfig> MeterConfigs { get; }

        public MeterConfigHistory()
        {
            MeterConfigs = new List<MeterConfig>();
        }
        public MeterConfigHistory(IEnumerable<MeterConfig> meterConfigs)
        {
            MeterConfigs = meterConfigs.ToList();
        }
    }
}
