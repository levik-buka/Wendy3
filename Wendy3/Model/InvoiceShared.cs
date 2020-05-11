using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class InvoiceShared : DateRange
    {
        public Invoice CommonInvoice { get; set; }
        public List<UserInvoice> UserInvoices { get; set; }
    }
}
