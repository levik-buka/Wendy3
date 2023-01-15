using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wendy3Tests.Utils;
using Wendy.Invoice.Entity;
using Wendy.Invoice.Service;
using Wendy.Utils.Dao.Repository;
using Wendy.Consumption.Service;

namespace Wendy.Fee.Service.Tests
{
    [TestClass()]
    public class FeeCalculatorTests
    {
        private void AssertInvoicesFee(InvoiceShared expected, InvoiceShared actual)
        {
            Assert.AreEqual(expected.GetBasicFee().CleanWaterFee, actual.GetBasicFee().CleanWaterFee, $"different clean water basic fee of invoice {actual.Id}");
            Assert.AreEqual(expected.GetBasicFee().WasteWaterFee, actual.GetBasicFee().WasteWaterFee, $"different waste water basic fee of invoice {actual.Id}");
            Assert.AreEqual(expected.GetUsageFee().CleanWaterFee, actual.GetUsageFee().CleanWaterFee, $"different clean water usage fee of invoice {actual.Id}");
            Assert.AreEqual(expected.GetUsageFee().WasteWaterFee, actual.GetUsageFee().WasteWaterFee, $"different waste water usage fee of invoice {actual.Id}");

            Assert.AreEqual(expected.UserInvoices.Count, actual.UserInvoices.Count);
            for (int i = 0; i < expected.UserInvoices.Count; i++)
            {
                UserInvoice expectedUserInvoice = expected.UserInvoices[i];
                UserInvoice actualUserInvoice = actual.UserInvoices.GetInvoiceByOwner(expectedUserInvoice.InvoiceOwner);
                Assert.IsNotNull(actualUserInvoice, $"missing user '{actualUserInvoice.InvoiceOwner}' invoice {actual.Id}");

                Assert.AreEqual(expectedUserInvoice.GetBasicFee().CleanWaterFee, actualUserInvoice.GetBasicFee().CleanWaterFee,
                    $"different clean water basic fee of user '{actualUserInvoice.InvoiceOwner}' invoice {actual.Id}");
                Assert.AreEqual(expectedUserInvoice.GetBasicFee().WasteWaterFee, actualUserInvoice.GetBasicFee().WasteWaterFee,
                    $"different waste water basic fee of user '{actualUserInvoice.InvoiceOwner}' invoice {actual.Id}");
                Assert.AreEqual(expectedUserInvoice.GetUsageFee().CleanWaterFee, actualUserInvoice.GetUsageFee().CleanWaterFee,
                    $"different clean water usage fee of user '{actualUserInvoice.InvoiceOwner}' invoice {actual.Id}");
                Assert.AreEqual(expectedUserInvoice.GetUsageFee().WasteWaterFee, actualUserInvoice.GetUsageFee().WasteWaterFee,
                    $"different waste water usage fee of user '{actualUserInvoice.InvoiceOwner}' invoice {actual.Id}");
            }
        }

        [TestMethod()]
        public void CalculateFeeTest()
        {
            string wendyJson = Resource.GetResourceAsString("Wendy.json");
            var invoiceHistory = JsonUtil.DeserializeJson<InvoiceHistory>(wendyJson);
            var invoiceHistoryForAsserts = JsonUtil.DeserializeJson<InvoiceHistory>(wendyJson);

            var consumptionCalc = new ConsumptionCalculator(invoiceHistory);
            var feeCalc = new FeeCalculator(invoiceHistory);

            consumptionCalc.CalculateConsumption();

            {
                int invoiceId = 0;  // balanced
                feeCalc.CalculateFee(invoiceId);
                AssertInvoicesFee(invoiceHistoryForAsserts.Invoices.GetInvoiceById(invoiceId), invoiceHistory.Invoices.GetInvoiceById(invoiceId));
            }

            {
                int invoiceId = 14; // estimated
                feeCalc.CalculateFee(invoiceId);
                AssertInvoicesFee(invoiceHistoryForAsserts.Invoices.GetInvoiceById(invoiceId), invoiceHistory.Invoices.GetInvoiceById(invoiceId));
            }

            {
                int invoiceId = 15; // estimated
                feeCalc.CalculateFee(invoiceId);
                AssertInvoicesFee(invoiceHistoryForAsserts.Invoices.GetInvoiceById(invoiceId), invoiceHistory.Invoices.GetInvoiceById(invoiceId));
            }

            {
                int invoiceId = 16; // estimated
                feeCalc.CalculateFee(invoiceId);
                AssertInvoicesFee(invoiceHistoryForAsserts.Invoices.GetInvoiceById(invoiceId), invoiceHistory.Invoices.GetInvoiceById(invoiceId));
            }

            {
                int invoiceId = 17; // balanced
                feeCalc.CalculateFee(invoiceId);
                AssertInvoicesFee(invoiceHistoryForAsserts.Invoices.GetInvoiceById(invoiceId), invoiceHistory.Invoices.GetInvoiceById(invoiceId));
            }
        }
    }
}