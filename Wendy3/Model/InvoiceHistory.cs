using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Tasks.Extensions;

namespace Wendy.Model
{
    public class InvoiceHistory
    {
        public List<InvoiceShared> Invoices { get; } = new List<InvoiceShared>();
        public List<FeeConfig> FeeConfigHistory { get; } = new List<FeeConfig>();
        public MeterConfigHistory MainMeterConfigHistory { get; set; } = new MeterConfigHistory();
        public List<UserMeterConfigHistory> UserMeterConfigHistory { get; } = new List<UserMeterConfigHistory>();

        public void CalculateConsumption()
        {

        }

        public ulong CalculateConsumption(int invoiceId)
        {
            InvoiceShared invoice = Invoices.GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with id {invoiceId} not found");
            }

            InvoiceShared prevInvoice = Invoices.GetInvoiceByEndDate(invoice.Start.AddDays(-1)) ?? InvoiceShared.CreateEmpty();

            return CalculateCommonConsumption(prevInvoice, invoice, MainMeterConfigHistory.GetConsumptionUntilDate(invoice.Start, invoice.GetReadOutDate()));
        }

        private static ulong CalculateCommonConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, ulong meterReadOut)
        {
            Contract.Requires(prevInvoice != null);
            Contract.Requires(invoice != null);

            ulong realConsumption = invoice.GetReadOut().Real - prevInvoice.GetReadOut().Real + meterReadOut;
            invoice.SetConsumption(invoice.GetConsumption().Estimated, realConsumption);

            return realConsumption;
        }

        private static ulong CalculateUserConsumption(UserInvoice prevInvoice, UserInvoice invoice, ulong meterReadOut)
        {
            Contract.Requires(prevInvoice != null);
            Contract.Requires(invoice != null);

            return 0U;
        }
    }
}
