using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class UserInvoice : Invoice
    {
        public string InvoiceOwner { get; set; }

        public UserInvoice(string user, DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
            : base(readOutDate, estimatedReadout, realReadOut)
        {
            InvoiceOwner = user;
        }
    }
}
