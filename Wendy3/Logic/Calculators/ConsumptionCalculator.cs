using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Model;
using Wendy.Tasks.Extensions;

namespace Wendy.Logic.Calculators
{
    public class ConsumptionCalculator
    {
        private readonly InvoiceHistory invoiceHistory;

        public ConsumptionCalculator(InvoiceHistory invoiceHistory)
        {
            Contract.Requires(invoiceHistory != null);

            this.invoiceHistory = invoiceHistory;
        }

        public void CalculateConsumption()
        {
            Contract.Requires(invoiceHistory != null);

            InvoiceShared prevInvoice = null;
            foreach (var invoice in invoiceHistory.Invoices)
            {
                CalculateConsumption(prevInvoice, invoice, 
                    invoiceHistory.MainMeterConfigHistory.MeterConfigs,
                    invoiceHistory.UserMeterConfigHistory);
                prevInvoice = invoice;
            }
        }

        public ulong CalculateConsumption(int invoiceId)
        {
            Contract.Requires(invoiceHistory != null);

            InvoiceShared invoice = invoiceHistory.Invoices.GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with id {invoiceId} not found");
            }

            InvoiceShared prevInvoice = invoiceHistory.Invoices.GetInvoiceByEndDate(invoice.Start.AddDays(-1));
            return CalculateConsumption(prevInvoice, invoice, 
                invoiceHistory.MainMeterConfigHistory.MeterConfigs,
                invoiceHistory.UserMeterConfigHistory);
        }

        private static ulong CalculateConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, 
            IEnumerable<MeterConfig> mainMeterConfigHistory, IEnumerable<UserMeterConfigHistory> userMeterConfigHistory)
        {
            Contract.Requires(invoice != null);


            DateRange readOutPeriod = InvoiceExtensions.GetReadOutPeriod(prevInvoice, invoice);

            IEnumerable<MeterConfig> meterConfigs = mainMeterConfigHistory.GetMeterConfigHistoryForPeriod(readOutPeriod);
            ulong realCommonConsumption = CalculateRealCommonConsumption(prevInvoice, invoice, meterConfigs);
            ulong estimatedCommonConsumption = CalculateEstimatedCommonConsumption(prevInvoice, invoice, meterConfigs);
            ulong totalEstimatedUserConsumption = 0U;

            foreach (var userInvoice in invoice.UserInvoices)
            {
                UserInvoice prevUserInvoice = prevInvoice?.UserInvoices.GetInvoiceByOwner(userInvoice.InvoiceOwner);
                IEnumerable<MeterConfig> userMeterConfigs = userMeterConfigHistory.
                    GetMeterConfigHistoryByOwner(userInvoice.InvoiceOwner).
                    GetMeterConfigHistoryForPeriod(readOutPeriod);

                ulong realUserConsumption = CalculateRealUserConsumption(readOutPeriod, prevUserInvoice, userInvoice, userMeterConfigs);
                totalEstimatedUserConsumption +=
                    CalculateEstimatedUserConsumption(userInvoice, realUserConsumption, new ConsumptionValue(estimatedCommonConsumption, realCommonConsumption));
            }

            FineGrainEstimatedUserConsumption(
                invoice.UserInvoices,
                totalEstimatedUserConsumption,
                new ConsumptionValue(estimatedCommonConsumption, realCommonConsumption)
            );

            return realCommonConsumption;
        }

        private static ulong CalculateRealCommonConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            DateRange readOutPeriod = InvoiceExtensions.GetReadOutPeriod(prevInvoice, invoice);

            ulong startRealReadOut = prevInvoice?.GetReadOut().Real ?? 0U;
            ulong endRealReadOut = invoice.GetReadOut().Real;

            ulong commonRealConsumption = CalculateConsumption(readOutPeriod, startRealReadOut, endRealReadOut, meterConfigs);

            invoice.GetConsumption().Real = commonRealConsumption;
            return commonRealConsumption;
        }

        private static ulong CalculateEstimatedCommonConsumption(InvoiceShared prevInvoice, InvoiceShared invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            DateRange readOutPeriod = InvoiceExtensions.GetReadOutPeriod(prevInvoice, invoice);

            ulong commonEstimatedConsumption = invoice.GetConsumption().Estimated;
            if (commonEstimatedConsumption == 0)
            {
                ulong startEstimatedReadOut = prevInvoice?.GetReadOut().Estimated ?? 0U;
                ulong endEstimatedReadOut = invoice.GetReadOut().Estimated;

                commonEstimatedConsumption = CalculateConsumption(readOutPeriod, startEstimatedReadOut, endEstimatedReadOut, meterConfigs);

                invoice.GetConsumption().Estimated = commonEstimatedConsumption;
            }

            return commonEstimatedConsumption;
        }

        private static ulong CalculateRealUserConsumption(DateRange readOutPeriod, UserInvoice prevInvoice, UserInvoice invoice, IEnumerable<MeterConfig> meterConfigs)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(meterConfigs != null);

            ulong startRealReadOut = prevInvoice?.GetReadOut().Real ?? 0U;
            ulong endRealReadOut = invoice.GetReadOut().Real;

            ulong realConsumption = CalculateConsumption(readOutPeriod, startRealReadOut, endRealReadOut, meterConfigs);
            invoice.GetConsumption().Real = realConsumption;

            return realConsumption;
        }

        private static ulong CalculateEstimatedUserConsumption(UserInvoice invoice, ulong realUserConsumption, ConsumptionValue commonConsumption)
        {
            Contract.Requires(commonConsumption != null);

            decimal estimatedUserConsumption = Convert.ToDecimal(commonConsumption.Estimated * realUserConsumption) / Convert.ToDecimal(commonConsumption.Real);

            invoice.GetConsumption().Estimated = Convert.ToUInt64(Math.Round(estimatedUserConsumption));
            return invoice.GetConsumption().Estimated;
        }

        private static void FineGrainEstimatedUserConsumption(IEnumerable<UserInvoice> userInvoices, 
            ulong totalCalculatedEstimatedConsumption, ConsumptionValue commonConsumption)
        {
            long estimatedConsumptionDiff = Convert.ToInt64(totalCalculatedEstimatedConsumption - commonConsumption.Estimated);

            foreach (var userInvoice in userInvoices)
            {
                if (estimatedConsumptionDiff > 0)
                {
                    userInvoice.GetConsumption().Estimated--;
                    estimatedConsumptionDiff--;
                }
                else if (estimatedConsumptionDiff < 0)
                {
                    userInvoice.GetConsumption().Estimated++;
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
    }
}
