﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Wendy.Fee.Entity;
using Wendy.Meter.Entity;

namespace Wendy.Invoice.Entity
{
    /// <summary>
    /// Invoice history
    /// </summary>
    public class InvoiceHistory
    {
        /// <summary>
        /// Invoices
        /// </summary>
        public List<InvoiceShared> Invoices { get; } = new List<InvoiceShared>();
        /// <summary>
        /// Fee plans
        /// </summary>
        public List<FeeConfig> FeeConfigHistory { get; } = new List<FeeConfig>();
        /// <summary>
        /// Main meter configurations
        /// </summary>
        public MeterConfigHistory MainMeterConfigHistory { get; set; } = new MeterConfigHistory();
        /// <summary>
        /// Users' meter configurations
        /// </summary>
        public List<UserMeterConfigHistory> UserMeterConfigHistory { get; } = new List<UserMeterConfigHistory>();
    }
}
