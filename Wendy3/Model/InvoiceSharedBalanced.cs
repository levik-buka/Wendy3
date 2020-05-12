using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Model
{
    public class InvoiceSharedBalanced : InvoiceShared
    {
        public InvoiceSharedBalanced(long id, DateTime startDate, DateTime? endDate) 
            : base (id, startDate, endDate)
        {

        }

        override public bool IsBalanced() { return true; }
    }
}
