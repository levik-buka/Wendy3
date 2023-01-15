using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Wendy.Consumption.Entity;
using Wendy.Invoice.Entity;
using Wendy.Utils.Entity;

namespace Wendy.Invoice.Service
{
    /// <summary>
    /// Extension methods for invoices
    /// </summary>
    public static class InvoiceExtensions
    {
        /// <summary>
        /// Get invoice by id
        /// </summary>
        /// <param name="invoices"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static InvoiceShared GetInvoiceById(this IEnumerable<InvoiceShared> invoices, long id)
        {
            Contract.Requires(invoices != null);

            return invoices.FirstOrDefault(invoice => invoice.Id == id);
        }

        /// <summary>
        /// Get user invoice by user
        /// </summary>
        /// <param name="invoices"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static UserInvoice GetInvoiceByOwner(this IEnumerable<UserInvoice> invoices, string owner)
        {
            Contract.Requires(invoices != null);

            return invoices.FirstOrDefault(invoice => invoice.InvoiceOwner == owner);
        }

        /// <summary>
        /// Get list of user invoices by user
        /// </summary>
        /// <param name="invoices"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static IEnumerable<UserInvoice> GetUserInvoicesByOwner(this IEnumerable<InvoiceShared> invoices, string owner)
        {
            Contract.Requires(invoices != null);

            return invoices.Select(invoice => invoice.UserInvoices.GetInvoiceByOwner(owner));
        }

        /// <summary>
        /// Get invoice by end date
        /// </summary>
        /// <param name="invoices"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static InvoiceShared GetInvoiceByEndDate(this IEnumerable<InvoiceShared> invoices, DateTime endDate)
        {
            Contract.Requires(invoices != null);

            return invoices.FirstOrDefault(invoice => invoice.End == endDate);
        }

        /// <summary>
        /// Calculate read-out period by previous and next invoices
        /// </summary>
        /// <param name="prevInvoice"></param>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static DateRange GetReadOutPeriod(InvoiceShared prevInvoice, InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);

            return new DateRange { Start = prevInvoice?.GetReadOutDate().AddDays(1) ?? invoice.Start, End = invoice.GetReadOutDate() };
        }

        /// <summary>
        /// Return list of estimated invoices in the period
        /// </summary>
        /// <param name="invoices"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static IEnumerable<InvoiceShared> GetEstimatedInvoicesInPeriod(this IEnumerable<InvoiceShared> invoices, DateRange period)
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
        public static InvoiceShared CreateUnestimated(this InvoiceShared realInvoice, IEnumerable<InvoiceShared> estimatedInvoices)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(estimatedInvoices != null);

            var unestimatedInvoice = new InvoiceShared(realInvoice.Id, estimatedInvoices.LastOrDefault()?.End ?? realInvoice.Start, realInvoice.End)
            {
                Balanced = realInvoice.Balanced
            };

            unestimatedInvoice.GetReadOut().Estimated = realInvoice.GetReadOut().Estimated;
            unestimatedInvoice.GetReadOut().Real = realInvoice.GetReadOut().Real;
            unestimatedInvoice.GetConsumption().Estimated = realInvoice.GetConsumption().Estimated; // - estimatedInvoices.Sum(invoice => invoice.GetConsumption().Estimated);
            unestimatedInvoice.GetConsumption().Real = realInvoice.GetConsumption().Real; // - estimatedInvoices.Sum(invoice => invoice.GetConsumption().Real);

            foreach(var userInvoice in realInvoice.UserInvoices)
            {
                //var estimatedUserInvoices = estimatedInvoices.GetUserInvoicesByOwner(userInvoice.InvoiceOwner);

                var unestimatedUserInvoice = new UserInvoice(userInvoice.InvoiceOwner, userInvoice.GetReadOutDate(),
                    userInvoice.GetReadOut().Estimated, userInvoice.GetReadOut().Real);

                unestimatedUserInvoice.GetConsumption().Estimated = userInvoice.GetConsumption().Estimated; // - estimatedUserInvoices.Sum(invoice => invoice.GetConsumption().Estimated);
                unestimatedUserInvoice.GetConsumption().Real = userInvoice.GetConsumption().Real; // - estimatedUserInvoices.Sum(invoice => invoice.GetConsumption().Real);

                unestimatedInvoice.UserInvoices.Add(unestimatedUserInvoice);
            }

            return unestimatedInvoice;
        }

        /// <summary>
        /// Calculate ulong sum of functor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="invoices"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static ulong Sum<T>(this IEnumerable<T> invoices, Func<T, ulong> selector)
        {
            Contract.Requires(invoices != null);
            Contract.Requires(selector != null);

            ulong sum = invoices.Select(invoice => selector(invoice)).Sum();
            return sum;
        }

        /// <summary>
        /// Calculate ulong sum
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ulong Sum(this IEnumerable<ulong> source)
        {
            Contract.Requires(source != null);

            ulong sum = 0;
            foreach(ulong value in source)
            {
                sum += value;
            }

            return sum;
        }

        /// <summary>
        /// Calculate consumption sum
        /// </summary>
        /// <param name="consumption"></param>
        /// <returns></returns>
        public static ulong Sum(this ConsumptionValue consumption)
        {
            Contract.Requires(consumption != null);

            return consumption.Estimated + consumption.Real;
        }

        /// <summary>
        /// Checks if sum of user invoices differs from sum of common invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="commonSum"></param>
        /// <param name="usersSum"></param>
        /// <returns></returns>
        public static bool IsSumOfUserInvoicesDifferent(this InvoiceShared invoice, Func<InvoiceShared, ulong> commonSum, Func<UserInvoice, ulong> usersSum)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(commonSum != null);
            Contract.Requires(usersSum != null);

            ulong invoiceSum = commonSum(invoice);
            ulong userSum = invoice.UserInvoices.Sum(userInvoice => usersSum(userInvoice));

            return invoiceSum != userSum;
        }

        /// <summary>
        /// Checks if sum of user invoices differs from sum of common invoice
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="commonSum"></param>
        /// <param name="usersSum"></param>
        /// <returns></returns>
        public static bool IsSumOfUserInvoicesDifferent(this InvoiceShared invoice, Func<InvoiceShared, decimal> commonSum, Func<UserInvoice, decimal> usersSum)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(commonSum != null);
            Contract.Requires(usersSum != null);

            decimal invoiceSum = commonSum(invoice);
            decimal userSum = invoice.UserInvoices.Sum(userInvoice => usersSum(userInvoice));

            return invoiceSum != userSum;
        }
    }
}
