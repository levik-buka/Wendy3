﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class MeterConfig : DateRange
    {
        public ulong StartReadOut { get; set; }
        public ulong EndReadOut { get; set; }
    }
}
