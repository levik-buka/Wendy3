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
            if (!feeConfigs.Any())
            {
                throw new InvalidDataException($"Unable to calculate fee for invoice ({invoice.Id}), because of missing fee configurations");
            }

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
                IEnumerable<InvoiceShared> estimatedInvoiced = invoiceHistory.Invoices.GetEstimatedInvoicesInPeriod(invoice);
                CalculateRealFee(invoice, estimatedInvoiced, feeConfigs);
            }
        }

        private static void CalculateRealFee(InvoiceShared realInvoice, IEnumerable<InvoiceShared> estimatedInvoices, IEnumerable<FeeConfig> feeConfigs)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(feeConfigs != null);
            Contract.Requires(estimatedInvoices != null);

            realInvoice.GetBasicFee().ResetWaterFee();
            realInvoice.GetUsageFee().ResetWaterFee();
            realInvoice.UserInvoices.ForEach(invoice =>
            {
                invoice.GetBasicFee().ResetWaterFee();
                invoice.GetUsageFee().ResetWaterFee();
            });

            foreach (var estimatedInvoice in estimatedInvoices)
            {
                CalculateRealFee(realInvoice, estimatedInvoice, feeConfigs.GetFeeConfigOfPeriodOrThrowException(estimatedInvoice));
            }

            InvoiceShared unestimatedInvoice = realInvoice.CreateUnestimated(estimatedInvoices);
            CalculateRealFee(realInvoice, unestimatedInvoice, feeConfigs.GetFeeConfigOfPeriodOrThrowException(unestimatedInvoice));
        }

        private static bool CalculateRealFee(InvoiceShared realInvoice, InvoiceShared estimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(estimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            if (!CalculateCommonRealBasicFee(realInvoice, estimatedInvoice, feeConfig)) return false;
            if (!CalculateCommonRealUsageFee(realInvoice, estimatedInvoice, feeConfig)) return false;

            foreach (var userInvoice in realInvoice.UserInvoices)
            {
                UserInvoice estimatedUserInvoice = estimatedInvoice.UserInvoices.GetInvoiceByOwner(userInvoice.InvoiceOwner);

                CalculateUserRealBasicFee(estimatedInvoice, userInvoice, realInvoice.UserInvoices.Count, feeConfig);
                CalculateUserRealUsageFee(userInvoice, estimatedUserInvoice, feeConfig);
            }

            return true;
        }

        private static bool CalculateEstimatedFee(InvoiceShared estimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(estimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            if (!CalculateCommonEstimatedBasicFee(estimatedInvoice, feeConfig)) return false;
            if (!CalculateCommonEstimatedUsageFee(estimatedInvoice, feeConfig)) return false;

            foreach (var userInvoice in estimatedInvoice.UserInvoices)
            {
                CalculateUserEstimatedBasicFee(estimatedInvoice, userInvoice, estimatedInvoice.UserInvoices.Count, feeConfig);
                CalculateUserEstimatedUsageFee(userInvoice, feeConfig);
            }

            return true;
        }

        private static void CalculateUserRealUsageFee(UserInvoice userRealInvoice, UserInvoice userEstimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(userRealInvoice != null);
            Contract.Requires(userEstimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            userRealInvoice.GetUsageFee().CleanWaterFee += (userEstimatedInvoice.GetConsumption().Real * feeConfig.GetMonthlyCleanWaterUsageFeeWithoutVAT()).RoundToCents();
            userRealInvoice.GetUsageFee().WasteWaterFee += (userEstimatedInvoice.GetConsumption().Real * feeConfig.GetMonthlyWasteWaterUsageFeeWithoutVAT()).RoundToCents();
        }

        private static void CalculateUserEstimatedUsageFee(UserInvoice userEstimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(userEstimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            userEstimatedInvoice.GetUsageFee().CleanWaterFee = (userEstimatedInvoice.GetConsumption().Estimated * feeConfig.GetMonthlyCleanWaterUsageFeeWithoutVAT()).RoundToCents();
            userEstimatedInvoice.GetUsageFee().WasteWaterFee = (userEstimatedInvoice.GetConsumption().Estimated * feeConfig.GetMonthlyWasteWaterUsageFeeWithoutVAT()).RoundToCents();
        }

        private static void CalculateUserRealBasicFee(DateRange invoicePeriod, UserInvoice userRealInvoice, int userCount, FeeConfig feeConfig)
        {
            Contract.Requires(invoicePeriod != null);
            Contract.Requires(userRealInvoice != null);
            Contract.Requires(feeConfig != null);
            Contract.Requires(userCount > 0);

            userRealInvoice.GetBasicFee().CleanWaterFee += (invoicePeriod.GetMonths() * feeConfig.GetMonthlyCleanWaterBasicFeeWithoutVAT() / userCount).RoundToCents();
            userRealInvoice.GetBasicFee().WasteWaterFee += (invoicePeriod.GetMonths() * feeConfig.GetMonthlyWasteWaterBasicFeeWithoutVAT() / userCount).RoundToCents();
        }

        private static void CalculateUserEstimatedBasicFee(DateRange invoicePeriod, UserInvoice userEstimatedInvoice, int userCount, FeeConfig feeConfig)
        {
            Contract.Requires(invoicePeriod != null);
            Contract.Requires(userEstimatedInvoice != null);
            Contract.Requires(feeConfig != null);
            Contract.Requires(userCount > 0);

            userEstimatedInvoice.GetBasicFee().CleanWaterFee = (invoicePeriod.GetMonths() * feeConfig.GetMonthlyCleanWaterBasicFeeWithoutVAT() / userCount).RoundToCents();
            userEstimatedInvoice.GetBasicFee().WasteWaterFee = (invoicePeriod.GetMonths() * feeConfig.GetMonthlyWasteWaterBasicFeeWithoutVAT() / userCount).RoundToCents();
        }

        private static bool CalculateCommonRealUsageFee(InvoiceShared realInvoice, InvoiceShared estimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(estimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            realInvoice.GetUsageFee().CleanWaterFee += (estimatedInvoice.GetConsumption().Real * feeConfig.GetMonthlyCleanWaterUsageFeeWithoutVAT()).RoundToCents();
            realInvoice.GetUsageFee().WasteWaterFee += (estimatedInvoice.GetConsumption().Real * feeConfig.GetMonthlyWasteWaterUsageFeeWithoutVAT()).RoundToCents();

            return true;
        }

        private static bool CalculateCommonEstimatedUsageFee(InvoiceShared estimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(estimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            if (estimatedInvoice.Balanced == false)
            {
                estimatedInvoice.GetUsageFee().CleanWaterFee = (estimatedInvoice.GetConsumption().Estimated * feeConfig.GetMonthlyCleanWaterUsageFeeWithoutVAT()).RoundToCents();
                estimatedInvoice.GetUsageFee().WasteWaterFee = (estimatedInvoice.GetConsumption().Estimated * feeConfig.GetMonthlyWasteWaterUsageFeeWithoutVAT()).RoundToCents();

                return true;
            }

            return false;
        }

        private static bool CalculateCommonRealBasicFee(InvoiceShared realInvoice, InvoiceShared estimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(realInvoice != null);
            Contract.Requires(estimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            realInvoice.GetBasicFee().CleanWaterFee += (estimatedInvoice.GetMonths() * feeConfig.GetMonthlyCleanWaterBasicFeeWithoutVAT()).RoundToCents();
            realInvoice.GetBasicFee().WasteWaterFee += (estimatedInvoice.GetMonths() * feeConfig.GetMonthlyWasteWaterBasicFeeWithoutVAT()).RoundToCents();

            return true;
        }

        private static bool CalculateCommonEstimatedBasicFee(InvoiceShared estimatedInvoice, FeeConfig feeConfig)
        {
            Contract.Requires(estimatedInvoice != null);
            Contract.Requires(feeConfig != null);

            if (estimatedInvoice.Balanced == false)
            {
                estimatedInvoice.GetBasicFee().CleanWaterFee = (estimatedInvoice.GetMonths() * feeConfig.GetMonthlyCleanWaterBasicFeeWithoutVAT()).RoundToCents();
                estimatedInvoice.GetBasicFee().WasteWaterFee = (estimatedInvoice.GetMonths() * feeConfig.GetMonthlyWasteWaterBasicFeeWithoutVAT()).RoundToCents();

                return true;
            }

            return false;
        }
    }
}
