﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    /// <summary>
    /// Meter configuration history
    /// </summary>
    public class MeterConfigHistory
    {
        /// <summary>
        /// List of meter configurations
        /// </summary>
        public List<MeterConfig> MeterConfigs { get; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MeterConfigHistory()
        {
            MeterConfigs = new List<MeterConfig>();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="meterConfigs"></param>
        public MeterConfigHistory(IEnumerable<MeterConfig> meterConfigs)
        {
            MeterConfigs = meterConfigs.ToList();
        }
    }
}
