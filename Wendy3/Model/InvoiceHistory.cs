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

            InvoiceShared prevInvoice = Invoices.GetInvoiceByEndDate(invoice.Start.AddDays(-1));

            DateRange readOutPeriod = GetReadOutPeriod(prevInvoice, invoice);
            IEnumerable<MeterConfig> meterConfigs = MainMeterConfigHistory.MeterConfigs.GetMeterConfigHistoryForPeriod(readOutPeriod);


            return CalculateCommonConsumption(prevInvoice, invoice, meterConfigs);
        }

        private static ulong CalculateCommonConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            DateRange readOutPeriod = GetReadOutPeriod(prevInvoice, invoice);

            ulong startReadOut = prevInvoice?.GetReadOut().Real ?? 0U;
            ulong endReadOut = invoice.GetReadOut().Real;
            ulong realConsumption = 0U;

            MeterConfig firstConfig = meterConfigs.FirstOrDefault();
            if (firstConfig != null)
            {
                // if meter changed on start of period, use meter's start readout
                if (firstConfig.Start == readOutPeriod.Start)
                {
                    startReadOut = firstConfig.StartReadOut;
                }
                // if meter changed in the middle of period
                if (firstConfig.Start < readOutPeriod.Start && firstConfig.EndReadOut.HasValue)
                {
                    realConsumption = firstConfig.EndReadOut.Value - startReadOut;
                }
            }

            // add consumption of the period if meters changed many times
            realConsumption += Convert.ToUInt64(meterConfigs.GetMeterConfigHistoryInPeriod(readOutPeriod).Sum(config => config.GetConsumption()));

            MeterConfig lastConfig = meterConfigs.LastOrDefault();
            if (lastConfig != null)
            {
                // if meter not changed
                if (!lastConfig.EndReadOut.HasValue)
                {
                    if (lastConfig != firstConfig)
                    {
                        startReadOut = lastConfig.StartReadOut;
                    }
                    realConsumption += endReadOut - startReadOut;
                }
            }

            invoice.SetConsumption(invoice.GetConsumption().Estimated, realConsumption);

            return realConsumption;
        }

        private static ulong CalculateUserConsumption(UserInvoice prevInvoice, UserInvoice invoice, ulong meterReadOut)
        {
            Contract.Requires(prevInvoice != null);
            Contract.Requires(invoice != null);

            return 0U;
        }

        private static DateRange GetReadOutPeriod(InvoiceShared prevInvoice, InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);

            return new DateRange { Start = prevInvoice?.GetReadOutDate().AddDays(1) ?? invoice.Start, End = invoice.GetReadOutDate() };
        }
    }
}
