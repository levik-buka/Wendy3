using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class InvoiceHistory
    {
        public List<InvoiceShared> Invoices { get; set; }
        public List<FeeConfig> FeeConfigHistory { get; set; }
        public MeterConfigHistory MainMeterConfigHistory { get; set; }
        public List<UserMeterConfigHistory> UserMeterConfigHistory { get; set; }
    }
}
