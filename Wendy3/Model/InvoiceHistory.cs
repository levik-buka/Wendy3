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

        public void CalculateAllConsumption()
        {
            foreach(var invoice in Invoices)
            {
                CalculateConsumption(invoice);
            }
        }

        public ulong CalculateConsumption(int invoiceId)
        {
            InvoiceShared invoice = Invoices.GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with id {invoiceId} not found");
            }

            return CalculateConsumption(invoice);
        }

        public ulong CalculateConsumption(InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);

            InvoiceShared prevInvoice = Invoices.GetInvoiceByEndDate(invoice.Start.AddDays(-1));

            DateRange readOutPeriod = GetReadOutPeriod(prevInvoice, invoice);

            foreach (var userInvoice in invoice.UserInvoices)
            {
                UserInvoice prevUserInvoice = prevInvoice?.UserInvoices.GetInvoiceByOwner(userInvoice.InvoiceOwner);
                IEnumerable<MeterConfig> userMeterConfigs = UserMeterConfigHistory.
                    GetMeterConfigHistoryByOwner(userInvoice.InvoiceOwner).
                    GetMeterConfigHistoryForPeriod(readOutPeriod);

                CalculateUserConsumption(readOutPeriod, prevUserInvoice, userInvoice, userMeterConfigs);
            }

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
            
            ulong commonConsumption = CalculateConsumption(readOutPeriod, startReadOut, endReadOut, meterConfigs);

            invoice.SetConsumption(invoice.GetConsumption().Estimated, commonConsumption);
            return commonConsumption;
        }

        private static ulong CalculateUserConsumption(DateRange readOutPeriod, UserInvoice prevInvoice, UserInvoice invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            ulong startReadOut = prevInvoice?.ReadOut.GetReadOut().Real ?? 0U;
            ulong endReadOut = invoice.ReadOut.GetReadOut().Real;

            invoice.ReadOut.Consumption.Real = CalculateConsumption(readOutPeriod, startReadOut, endReadOut, meterConfigs);
            return invoice.ReadOut.Consumption.Real;
        }

        private static ulong CalculateConsumption(DateRange readOutPeriod, ulong startReadOut, ulong endReadOut, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(meterConfigs != null);

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

            return realConsumption;
        }

        private static DateRange GetReadOutPeriod(InvoiceShared prevInvoice, InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);

            return new DateRange { Start = prevInvoice?.GetReadOutDate().AddDays(1) ?? invoice.Start, End = invoice.GetReadOutDate() };
        }
    }
}
