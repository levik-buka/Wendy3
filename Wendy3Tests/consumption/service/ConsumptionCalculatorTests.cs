using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wendy3Tests.Utils;
using Wendy.Utils.Dao.Repository;
using Wendy.Invoice.Entity;
using Wendy.Invoice.Service;

namespace Wendy.Consumption.Service.Tests
{
    [TestClass()]
    public class ConsumptionCalculatorTests
    {
        [TestMethod()]
        public void CalculateConsumptionTest()
        {
            string wendyJson = Resource.GetResourceAsString("Wendy.json");
            var invoiceHistory = JsonUtil.DeserializeJson<InvoiceHistory>(wendyJson);

            var consumptionCalc = new ConsumptionCalculator(invoiceHistory);

            Assert.AreEqual(76U, consumptionCalc.CalculateConsumption(0));
            Assert.AreEqual(76U, invoiceHistory.Invoices.GetInvoiceById(0).GetConsumption().Estimated);
            Assert.AreEqual(76U, invoiceHistory.Invoices.GetInvoiceById(0).GetConsumption().Real);
            Assert.AreEqual(48U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Estimated);
            Assert.AreEqual(48U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Real);
            Assert.AreEqual(28U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Estimated);
            Assert.AreEqual(28U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Real);

            Assert.AreEqual(15U, consumptionCalc.CalculateConsumption(1));
            Assert.AreEqual(18U, invoiceHistory.Invoices.GetInvoiceById(1).GetConsumption().Estimated);
            Assert.AreEqual(15U, invoiceHistory.Invoices.GetInvoiceById(1).GetConsumption().Real);
            Assert.AreEqual(11U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Estimated);
            Assert.AreEqual(9U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Real);
            Assert.AreEqual(7U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Estimated);
            Assert.AreEqual(6U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Real);

            Assert.AreEqual(49U, consumptionCalc.CalculateConsumption(6));
            Assert.AreEqual(74U, invoiceHistory.Invoices.GetInvoiceById(6).GetConsumption().Estimated);
            Assert.AreEqual(49U, invoiceHistory.Invoices.GetInvoiceById(6).GetConsumption().Real);
            Assert.AreEqual(42U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Estimated);
            Assert.AreEqual(28U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Real);
            Assert.AreEqual(32U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Estimated);
            Assert.AreEqual(21U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Real);

            Assert.AreEqual(75U, consumptionCalc.CalculateConsumption(8), "consumption for whole year from prev balanced invoice (0)");
            Assert.AreEqual(75U, invoiceHistory.Invoices.GetInvoiceById(8).GetConsumption().Estimated);
            Assert.AreEqual(75U, invoiceHistory.Invoices.GetInvoiceById(8).GetConsumption().Real);
            Assert.AreEqual(43U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Estimated);
            Assert.AreEqual(43U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment A").GetConsumption().Real);
            Assert.AreEqual(32U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Estimated);
            Assert.AreEqual(32U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment B").GetConsumption().Real);
        }

        [TestMethod()]
        public void CalculateAllConsumptionTest()
        {
            string wendyJson = Resource.GetResourceAsString("Wendy.json");
            var invoiceHistory = JsonUtil.DeserializeJson<InvoiceHistory>(wendyJson);

            new ConsumptionCalculator(invoiceHistory).CalculateConsumption();
        }
    }
}