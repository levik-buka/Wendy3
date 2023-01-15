using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Meter.Entity
{
    /// <summary>
    /// User's meter configuration history
    /// </summary>
    public class UserMeterConfigHistory : MeterConfigHistory
    {
        /// <summary>
        /// Name of meter's user
        /// </summary>
        public string MeterUser { get; set; }
    }
}
