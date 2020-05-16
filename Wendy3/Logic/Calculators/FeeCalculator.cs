using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy.Model;
using Wendy.Tasks.Extensions;

namespace Wendy.Logic.Calculators
{
    public class FeeCalculator
    {
        private readonly InvoiceHistory invoiceHistory;

        public FeeCalculator(InvoiceHistory invoiceHistory)
        {
            Contract.Requires(invoiceHistory != null);

            this.invoiceHistory = invoiceHistory;
        }

        public void CalculateFees()
        {
            Contract.Requires(invoiceHistory != null);

            invoiceHistory.Invoices.ForEach(CalculateFee);
        }

        public void CalculateFee(int invoiceId)
        {
            Contract.Requires(invoiceHistory != null);

            InvoiceShared invoice = invoiceHistory.Invoices.GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                throw new KeyNotFoundException($"Invoice with id {invoiceId} not found");
            }

            CalculateFee(invoice);
        }

        public void CalculateFee(InvoiceShared invoice)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(invoiceHistory != null);

            IEnumerable<FeeConfig> feeConfigs = invoiceHistory.FeeConfigHistory.GetFeeConfigHistoryForPeriod(invoice);
            if (invoice.Balanced == false)  // estimated invoide should have only one fee config
            {
                if (feeConfigs.Count() > 1)
                {
                    throw new InvalidDataException($"Unable to calculate fee for estimated invoice ({invoice.Id}), because found too much fee configurations for invoice period");
                }

                CalculateEstimatedFee(invoice, feeConfigs.First());
            }
            else
            {
                IEnumerable<InvoiceShared> estimatedInvoiced = invoiceHistory.Invoices.GetEstimatedInvoicesForPeriod(invoice);
                CalculateRealFee(invoice, feeConfigs, estimatedInvoiced);
            }
        }

        private void CalculateRealFee(InvoiceShared realInvoice, IEnumerable<FeeConfig> feeConfigs, IEnumerable<InvoiceShared> estimatedInvoices)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(feeConfigs != null);

            if (!feeConfigs.Any())
            {
                throw new InvalidDataException($"Unable to calculate fee for invoice ({realInvoice.Id}), because of missing fee configurations");
            }

        }

        private static bool CalculateEstimatedFee(InvoiceShared invoice, FeeConfig feeConfig)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(feeConfig != null);

            if (!CalculateCommonEstimatedBasicFee(invoice, feeConfig)) return false;
            if (!CalculateCommonEstimatedUsageFee(invoice, feeConfig)) return false;

            foreach (var userInvoice in invoice.UserInvoices)
            {
                CalculateUserEstimatedBasicFee(invoice, userInvoice, feeConfig);
                CalculateUserEstimatedUsageFee(userInvoice, feeConfig);
            }

            return true;
        }

        private static void CalculateUserEstimatedUsageFee(UserInvoice userInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(userInvoice != null);
            Contract.Requires(feeConfig != null);

            userInvoice.GetUsageFee().CleanWaterFee = userInvoice.GetConsumption().Estimated * feeConfig.GetMonthlyCleanWaterUsageFeeWithVAT();
            userInvoice.GetUsageFee().WasteWaterFee = userInvoice.GetConsumption().Estimated * feeConfig.GetMonthlyWasteWaterUsageFeeWithVAT();
        }


        private static void CalculateUserEstimatedBasicFee(DateRange invoicePeriod, UserInvoice userInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(invoicePeriod != null);
            Contract.Requires(userInvoice != null);
            Contract.Requires(feeConfig != null);

            userInvoice.GetBasicFee().CleanWaterFee = invoicePeriod.GetMonths() * feeConfig.GetMonthlyCleanWaterBasicFeeWithVAT();
            userInvoice.GetBasicFee().WasteWaterFee = invoicePeriod.GetMonths() * feeConfig.GetMonthlyWasteWaterBasicFeeWithVAT();
        }

        private static bool CalculateCommonEstimatedUsageFee(InvoiceShared invoice, FeeConfig feeConfig)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(feeConfig != null);

            if (invoice.Balanced == false)
            {
                invoice.GetUsageFee().CleanWaterFee = invoice.GetConsumption().Estimated * feeConfig.GetMonthlyCleanWaterUsageFeeWithVAT();
                invoice.GetUsageFee().WasteWaterFee = invoice.GetConsumption().Estimated * feeConfig.GetMonthlyWasteWaterUsageFeeWithVAT();

                return true;
            }

            return false;
        }

        private static bool CalculateCommonEstimatedBasicFee(InvoiceShared invoice, FeeConfig feeConfig)
        {
            Contract.Requires(invoice != null);
            Contract.Requires(feeConfig != null);

            if (invoice.Balanced == false)
            {
                invoice.GetBasicFee().CleanWaterFee = invoice.GetMonths() * feeConfig.GetMonthlyCleanWaterBasicFeeWithVAT();
                invoice.GetBasicFee().WasteWaterFee = invoice.GetMonths() * feeConfig.GetMonthlyWasteWaterBasicFeeWithVAT();

                return true;
            }

            return false;
        }
    }
}
