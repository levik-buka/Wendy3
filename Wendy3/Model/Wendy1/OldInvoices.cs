using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model.Wendy1
{
    public class OldInvoices
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ulong Consumption { get; set; }
        public ulong Estimation { get; set; }
        public decimal WaterFee { get; set; }
        public decimal WasteFee { get; set; }
        public decimal BasicFee { get; set; }
        public bool Balanced { get; set; }
    }
}
