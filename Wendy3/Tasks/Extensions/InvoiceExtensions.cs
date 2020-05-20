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

        public static IEnumerable<Model.UserInvoice> GetUserInvoicesByOwner(this IEnumerable<Model.InvoiceShared> invoices, string owner)
        {
            Contract.Requires(invoices != null);

            return invoices.Select(invoice => invoice.UserInvoices.GetInvoiceByOwner(owner));
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

        public static IEnumerable<Model.InvoiceShared> GetEstimatedInvoicesInPeriod(this IEnumerable<Model.InvoiceShared> invoices, Model.DateRange period)
        {
            Contract.Requires(invoices != null);

            return invoices.Where(invoice => (invoice.Balanced == false && invoice.In(period)));
        }

        /// <summary>
        /// real invoice 1.1.2020 - 31.12.2020
        /// estimated invoices: 1.1.2020 - 31.3.2020, 1.4.2020 - 30.6.2020, 1.7.2020 - 30.9.2020
        /// unestimated invoice: 1.10.2020 - 31.12.2020
        /// </summary>
        /// <param name="realInvoice"></param>
        /// <param name="estimatedInvoices"></param>
        /// <returns></returns>
        public static Model.InvoiceShared CreateUnestimated(this Model.InvoiceShared realInvoice, IEnumerable<Model.InvoiceShared> estimatedInvoices)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(estimatedInvoices != null);

            var unestimatedInvoice = new Model.InvoiceShared(realInvoice.Id, estimatedInvoices.LastOrDefault()?.End ?? realInvoice.Start, realInvoice.End)
            {
                Balanced = realInvoice.Balanced
            };

            unestimatedInvoice.GetReadOut().Estimated = realInvoice.GetReadOut().Estimated;
            unestimatedInvoice.GetReadOut().Real = realInvoice.GetReadOut().Real;
            unestimatedInvoice.GetConsumption().Estimated = realInvoice.GetConsumption().Estimated - estimatedInvoices.Sum(invoice => invoice.GetConsumption().Estimated);
            unestimatedInvoice.GetConsumption().Real = realInvoice.GetConsumption().Real - estimatedInvoices.Sum(invoice => invoice.GetConsumption().Real);

            foreach(var userInvoice in realInvoice.UserInvoices)
            {
                var estimatedUserInvoices = estimatedInvoices.GetUserInvoicesByOwner(userInvoice.InvoiceOwner);

                var unestimatedUserInvoice = new Model.UserInvoice(userInvoice.InvoiceOwner, userInvoice.GetReadOutDate(),
                    userInvoice.GetReadOut().Estimated, userInvoice.GetReadOut().Real);

                unestimatedUserInvoice.GetConsumption().Estimated = userInvoice.GetConsumption().Estimated - estimatedUserInvoices.Sum(invoice => invoice.GetConsumption().Estimated);
                unestimatedUserInvoice.GetConsumption().Real = userInvoice.GetConsumption().Real - estimatedUserInvoices.Sum(invoice => invoice.GetConsumption().Real);

                unestimatedInvoice.UserInvoices.Add(unestimatedUserInvoice);
            }

            return unestimatedInvoice;
        }

        public static ulong Sum<T>(this IEnumerable<T> invoices, Func<T, ulong> selector)
        {
            Contract.Requires(invoices != null);
            Contract.Requires(selector != null);

            ulong sum = 0;
            invoices.Select(invoice =>
            {
                sum += selector(invoice);
                return false;
            });

            return sum;
        }
    }
}
