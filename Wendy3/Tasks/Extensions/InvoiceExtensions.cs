using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Tasks.Extensions
{
    public static class InvoiceExtensions
    {
        public static Model.InvoiceShared GetInvoiceById(this IEnumerable<Model.InvoiceShared> invoices, long id)
        {
            Contract.Requires(invoices != null);

            return invoices.FirstOrDefault(invoice => invoice.Id == id);
        }

        public static Model.UserInvoice GetInvoiceByOwner(this IEnumerable<Model.UserInvoice> invoices, string owner)
        {
            Contract.Requires(invoices != null);

            return invoices.FirstOrDefault(invoice => invoice.InvoiceOwner == owner);
        }

        public static Model.InvoiceShared GetInvoiceByEndDate(this IEnumerable<Model.InvoiceShared> invoices, DateTime endDate)
        {
            Contract.Requires(invoices != null);

            return invoices.FirstOrDefault(invoice => invoice.End == endDate);
        }

        public static Model.DateRange GetReadOutPeriod(Model.InvoiceShared prevInvoice, Model.InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);

            return new Model.DateRange { Start = prevInvoice?.GetReadOutDate().AddDays(1) ?? invoice.Start, End = invoice.GetReadOutDate() };
        }

    }
}
