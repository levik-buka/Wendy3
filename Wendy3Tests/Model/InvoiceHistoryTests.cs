using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wendy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy3Tests.Utils;
using Wendy.Tasks.Utils;
using Wendy.Tasks.Extensions;

namespace Wendy.Model.Tests
{
    [TestClass()]
    public class InvoiceHistoryTests
    {
        [TestMethod()]
        public void CalculateConsumptionTest()
        {
            string wendyJson = Resource.GetResourceAsString("Wendy.json");
            var invoiceHistory = JsonUtil.DeserializeJson<Model.InvoiceHistory>(wendyJson);

            Assert.AreEqual(76U, invoiceHistory.CalculateConsumption(0));
            Assert.AreEqual(76U, invoiceHistory.Invoices.GetInvoiceById(0).GetConsumption().Estimated);
            Assert.AreEqual(76U, invoiceHistory.Invoices.GetInvoiceById(0).GetConsumption().Real);
            Assert.AreEqual(48U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Estimated);
            Assert.AreEqual(48U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Real);
            Assert.AreEqual(28U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Estimated);
            Assert.AreEqual(28U, invoiceHistory.Invoices.GetInvoiceById(0).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Real);

            Assert.AreEqual(15U, invoiceHistory.CalculateConsumption(1));
            Assert.AreEqual(18U, invoiceHistory.Invoices.GetInvoiceById(1).GetConsumption().Estimated);
            Assert.AreEqual(15U, invoiceHistory.Invoices.GetInvoiceById(1).GetConsumption().Real);
            Assert.AreEqual(11U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Estimated);
            Assert.AreEqual(9U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Real);
            Assert.AreEqual(7U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Estimated);
            Assert.AreEqual(6U, invoiceHistory.Invoices.GetInvoiceById(1).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Real);

            Assert.AreEqual(49U, invoiceHistory.CalculateConsumption(6));
            Assert.AreEqual(74U, invoiceHistory.Invoices.GetInvoiceById(6).GetConsumption().Estimated);
            Assert.AreEqual(49U, invoiceHistory.Invoices.GetInvoiceById(6).GetConsumption().Real);
            Assert.AreEqual(42U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Estimated);
            Assert.AreEqual(28U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Real);
            Assert.AreEqual(32U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Estimated);
            Assert.AreEqual(21U, invoiceHistory.Invoices.GetInvoiceById(6).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Real);

            Assert.AreEqual(75U, invoiceHistory.CalculateConsumption(8), "consumption for whole year from prev balanced invoice (0)");
            Assert.AreEqual(75U, invoiceHistory.Invoices.GetInvoiceById(8).GetConsumption().Estimated);
            Assert.AreEqual(75U, invoiceHistory.Invoices.GetInvoiceById(8).GetConsumption().Real);
            Assert.AreEqual(43U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Estimated);
            Assert.AreEqual(43U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment A").ReadOut.Consumption.Real);
            Assert.AreEqual(32U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Estimated);
            Assert.AreEqual(32U, invoiceHistory.Invoices.GetInvoiceById(8).UserInvoices.GetInvoiceByOwner("Apartment B").ReadOut.Consumption.Real);
        }

        [TestMethod()]
        public void CalculateAllConsumptionTest()
        {
            string wendyJson = Resource.GetResourceAsString("Wendy.json");
            var invoiceHistory = JsonUtil.DeserializeJson<Model.InvoiceHistory>(wendyJson);
            
            invoiceHistory.CalculateAllConsumption();
        }
    }
}