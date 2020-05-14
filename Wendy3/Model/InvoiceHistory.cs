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

            IEnumerable<MeterConfig> meterConfigs = MainMeterConfigHistory.MeterConfigs.GetMeterConfigHistoryForPeriod(readOutPeriod);
            ulong realCommonConsumption = CalculateRealCommonConsumption(prevInvoice, invoice, meterConfigs);
            ulong estimatedCommonConsumption = CalculateEstimatedCommonConsumption(prevInvoice, invoice, meterConfigs);
            ulong totalEstimatedUserConsumption = 0U;

            foreach (var userInvoice in invoice.UserInvoices)
            {
                UserInvoice prevUserInvoice = prevInvoice?.UserInvoices.GetInvoiceByOwner(userInvoice.InvoiceOwner);
                IEnumerable<MeterConfig> userMeterConfigs = UserMeterConfigHistory.
                    GetMeterConfigHistoryByOwner(userInvoice.InvoiceOwner).
                    GetMeterConfigHistoryForPeriod(readOutPeriod);

                ulong realUserConsumption = CalculateRealUserConsumption(readOutPeriod, prevUserInvoice, userInvoice, userMeterConfigs);
                totalEstimatedUserConsumption += 
                    CalculateEstimatedUserConsumption(userInvoice, realUserConsumption, new ConsumptionValue(estimatedCommonConsumption, realCommonConsumption));
            }

            FineGrainEstimatedUserConsumption(
                invoice.UserInvoices,
                Convert.ToInt64(totalEstimatedUserConsumption - estimatedCommonConsumption),
                new ConsumptionValue(estimatedCommonConsumption, realCommonConsumption)
            );

            return realCommonConsumption;
        }

        private static ulong CalculateRealCommonConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            DateRange readOutPeriod = GetReadOutPeriod(prevInvoice, invoice);

            ulong startRealReadOut = prevInvoice?.GetReadOut().Real ?? 0U;
            ulong endRealReadOut = invoice.GetReadOut().Real;

            ulong commonRealConsumption = CalculateConsumption(readOutPeriod, startRealReadOut, endRealReadOut, meterConfigs);

            invoice.SetConsumption(invoice.GetConsumption().Estimated, commonRealConsumption);
            return commonRealConsumption;
        }

        private static ulong CalculateEstimatedCommonConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            DateRange readOutPeriod = GetReadOutPeriod(prevInvoice, invoice);

            ulong commonEstimatedConsumption = invoice.GetConsumption().Estimated;
            if (commonEstimatedConsumption == 0)
            {
                ulong startEstimatedReadOut = prevInvoice?.GetReadOut().Estimated ?? 0U;
                ulong endEstimatedReadOut = invoice.GetReadOut().Estimated;

                commonEstimatedConsumption = CalculateConsumption(readOutPeriod, startEstimatedReadOut, endEstimatedReadOut, meterConfigs);

                invoice.SetConsumption(commonEstimatedConsumption, invoice.GetConsumption().Real);
            }

            return commonEstimatedConsumption;
        }

        private static ulong CalculateRealUserConsumption(DateRange readOutPeriod, UserInvoice prevInvoice, UserInvoice invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            ulong startRealReadOut = prevInvoice?.ReadOut.GetReadOut().Real ?? 0U;
            ulong endRealReadOut = invoice.ReadOut.GetReadOut().Real;
            invoice.ReadOut.Consumption.Real = CalculateConsumption(readOutPeriod, startRealReadOut, endRealReadOut, meterConfigs);

            return invoice.ReadOut.Consumption.Real;
        }

        private static ulong CalculateEstimatedUserConsumption(UserInvoice invoice, ulong realUserConsumption, ConsumptionValue commonConsumption)
        {
            Contract.Requires(commonConsumption != null);

            decimal estimatedUserConsumption = Convert.ToDecimal(commonConsumption.Estimated * realUserConsumption) / Convert.ToDecimal(commonConsumption.Real);
            invoice.ReadOut.Consumption.Estimated = Convert.ToUInt64(Math.Round(estimatedUserConsumption));

            return invoice.ReadOut.Consumption.Estimated;
        }

        private static void FineGrainEstimatedUserConsumption(IEnumerable<UserInvoice> userInvoices, long estimatedConsumptionDiff, ConsumptionValue commonConsumption)
        {
            foreach(var userInvoice in userInvoices)
            {
                if (estimatedConsumptionDiff > 0)
                {
                    userInvoice.ReadOut.Consumption.Estimated--;
                    estimatedConsumptionDiff--;
                }
                else if (estimatedConsumptionDiff < 0)
                {
                    userInvoice.ReadOut.Consumption.Estimated++;
                    estimatedConsumptionDiff++;
                }
                else return;
            }
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
