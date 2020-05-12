using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class InvoiceHistory
    {
        public List<InvoiceShared> Invoices { get; } = new List<InvoiceShared>();
        public List<FeeConfig> FeeConfigHistory { get; } = new List<FeeConfig>();
        public MeterConfigHistory MainMeterConfigHistory { get; set;  } = new MeterConfigHistory();
        public List<UserMeterConfigHistory> UserMeterConfigHistory { get; } = new List<UserMeterConfigHistory>();
    }
}
