using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Tasks.Extensions
{
    public static class InvoiceExtensions
    {
        public static Model.InvoiceShared GetInvoiceById(this IEnumerable<Model.InvoiceShared> invoices, long id)
        {
            return invoices.Where(invoice => invoice.Id == id).FirstOrDefault();
        }

        public static Model.InvoiceShared GetInvoiceByEndDate(this IEnumerable<Model.InvoiceShared> invoices, DateTime endDate)
        {
            return invoices.Where(invoice => invoice.End == endDate).FirstOrDefault();
        }
    }
}
