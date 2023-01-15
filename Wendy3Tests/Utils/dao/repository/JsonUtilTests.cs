using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wendy3Tests.Utils;
using Wendy.Wendy1.Entity;
using Wendy.Wendy1.Dao.Controller;
using Wendy.Invoice.Entity;

namespace Wendy.Utils.Dao.Repository.Tests
{
    [TestClass()]
    public class JsonUtilTests
    {
        [TestMethod()]
        public void SerializeToJsonTest()
        {
            string wendyXml = Resource.GetResourceAsString("Wendy.xml");
            var wendyData = XmlUtil.DeserializeXML<OldWendyFile>(wendyXml, nameSpace: null);
            var invoiceHistory = OldWendyFileConverter.ToInvoiceHistory(wendyData);

            string invoiceHistoryJson = JsonUtil.SerializeToJson(invoiceHistory);
            Assert.AreEqual(28436, invoiceHistoryJson.Length);
        }

        [TestMethod()]
        public void DeserializeWendyJsonTest()
        {
            string wendyJson = Resource.GetResourceAsString("Wendy.json");
            var invoiceHistory = JsonUtil.DeserializeJson<InvoiceHistory>(wendyJson);

            Assert.AreEqual(1, invoiceHistory.MainMeterConfigHistory.MeterConfigs.Count);
            Assert.AreEqual(0, invoiceHistory.MainMeterConfigHistory.MeterConfigs[0].Id);
            Assert.AreEqual(new DateTime(), invoiceHistory.MainMeterConfigHistory.MeterConfigs[0].Start);
            Assert.AreEqual(null, invoiceHistory.MainMeterConfigHistory.MeterConfigs[0].End);
            Assert.AreEqual(0U, invoiceHistory.MainMeterConfigHistory.MeterConfigs[0].StartReadOut);
            Assert.AreEqual(null, invoiceHistory.MainMeterConfigHistory.MeterConfigs[0].EndReadOut);

            Assert.AreEqual(2, invoiceHistory.UserMeterConfigHistory.Count);
            Assert.AreEqual("Apartment B", invoiceHistory.UserMeterConfigHistory[0].MeterUser);
            Assert.AreEqual(1, invoiceHistory.UserMeterConfigHistory[0].MeterConfigs.Count);
            Assert.AreEqual(0, invoiceHistory.UserMeterConfigHistory[0].MeterConfigs[0].Id);
            Assert.AreEqual(new DateTime(), invoiceHistory.UserMeterConfigHistory[0].MeterConfigs[0].Start);
            Assert.AreEqual(null, invoiceHistory.UserMeterConfigHistory[0].MeterConfigs[0].End);
            Assert.AreEqual(0U, invoiceHistory.UserMeterConfigHistory[0].MeterConfigs[0].StartReadOut);
            Assert.AreEqual(null, invoiceHistory.UserMeterConfigHistory[0].MeterConfigs[0].EndReadOut);

            Assert.AreEqual(4, invoiceHistory.FeeConfigHistory.Count);
            Assert.AreEqual(new DateTime(), invoiceHistory.FeeConfigHistory[0].Start);
            Assert.AreEqual(new DateTime(2007, 12, 31), invoiceHistory.FeeConfigHistory[0].End);
            Assert.AreEqual(1.04m, invoiceHistory.FeeConfigHistory[0].MonthlyUsageFee.CleanWaterFee);
            Assert.AreEqual(1.34m, invoiceHistory.FeeConfigHistory[0].MonthlyUsageFee.WasteWaterFee);
            Assert.AreEqual(22, invoiceHistory.FeeConfigHistory[0].VAT);
            Assert.AreEqual(false, invoiceHistory.FeeConfigHistory[0].VATIncludedIntoMonthlyFees);

            Assert.AreEqual(14, invoiceHistory.Invoices.Count);
            {
                InvoiceShared invoice = invoiceHistory.Invoices[0];
                Assert.AreEqual(0, invoice.Id);
                Assert.AreEqual(new DateTime(2007, 08, 18), invoice.Start);
                Assert.AreEqual(new DateTime(2007, 12, 19), invoice.End);
                Assert.AreEqual(true, invoice.Balanced);
                Assert.AreEqual(new DateTime(2007, 12, 19), invoice.GetReadOutDate());
                Assert.AreEqual(76U, invoice.GetReadOut().Estimated);
                Assert.AreEqual(76U, invoice.GetReadOut().Real);
                Assert.AreEqual(0U, invoice.GetConsumption().Estimated);
                Assert.AreEqual(0U, invoice.GetConsumption().Real);
                Assert.AreEqual(11.24m, invoice.GetBasicFee().CleanWaterFee);
                Assert.AreEqual(0m, invoice.GetBasicFee().WasteWaterFee);
                Assert.AreEqual(79.04m, invoice.GetUsageFee().CleanWaterFee);
                Assert.AreEqual(101.84m, invoice.GetUsageFee().WasteWaterFee);
            }
            {
                InvoiceShared invoice = invoiceHistory.Invoices[1];
                Assert.AreEqual(1, invoice.Id);
                Assert.AreEqual(new DateTime(2007, 12, 20), invoice.Start);
                Assert.AreEqual(new DateTime(2007, 12, 31), invoice.End);
                Assert.AreEqual(false, invoice.Balanced);
                Assert.AreEqual(new DateTime(2007, 12, 31), invoice.GetReadOutDate());
                Assert.AreEqual(91U, invoice.GetReadOut().Estimated);
                Assert.AreEqual(91U, invoice.GetReadOut().Real);
                Assert.AreEqual(18U, invoice.GetConsumption().Estimated);
                Assert.AreEqual(0U, invoice.GetConsumption().Real);
                Assert.AreEqual(1.09m, invoice.GetBasicFee().CleanWaterFee);
                Assert.AreEqual(0m, invoice.GetBasicFee().WasteWaterFee);
                Assert.AreEqual(18.72m, invoice.GetUsageFee().CleanWaterFee);
                Assert.AreEqual(24.12m, invoice.GetUsageFee().WasteWaterFee);
            }

            Assert.AreEqual(2, invoiceHistory.Invoices[0].UserInvoices.Count);
            {
                UserInvoice userInvoice = invoiceHistory.Invoices[0].UserInvoices[1];
                Assert.AreEqual("Apartment A", userInvoice.InvoiceOwner);
                Assert.AreEqual(new DateTime(2007, 12, 19), userInvoice.GetReadOutDate());
                Assert.AreEqual(48U, userInvoice.GetReadOut().Estimated);
                Assert.AreEqual(48U, userInvoice.GetReadOut().Real);
                Assert.AreEqual(0U, userInvoice.GetConsumption().Estimated);
                Assert.AreEqual(0U, userInvoice.GetConsumption().Real);
                Assert.AreEqual(5.62m, userInvoice.GetBasicFee().CleanWaterFee);
                Assert.AreEqual(0m, userInvoice.GetBasicFee().WasteWaterFee);
                Assert.AreEqual(49.92m, userInvoice.GetUsageFee().CleanWaterFee);
                Assert.AreEqual(64.32m, userInvoice.GetUsageFee().WasteWaterFee);
            }

            Assert.AreEqual(2, invoiceHistory.Invoices[1].UserInvoices.Count);
            {
                UserInvoice userInvoice = invoiceHistory.Invoices[1].UserInvoices[0];
                Assert.AreEqual("Apartment B", userInvoice.InvoiceOwner);
                Assert.AreEqual(new DateTime(2007, 12, 31), userInvoice.GetReadOutDate());
                Assert.AreEqual(34U, userInvoice.GetReadOut().Estimated);
                Assert.AreEqual(34U, userInvoice.GetReadOut().Real);
                Assert.AreEqual(0U, userInvoice.GetConsumption().Estimated);
                Assert.AreEqual(0U, userInvoice.GetConsumption().Real);
                Assert.AreEqual(0.55m, userInvoice.GetBasicFee().CleanWaterFee);
                Assert.AreEqual(0m, userInvoice.GetBasicFee().WasteWaterFee);
                Assert.AreEqual(8.32m, userInvoice.GetUsageFee().CleanWaterFee);
                Assert.AreEqual(10.72m, userInvoice.GetUsageFee().WasteWaterFee);
            }
        }
    }
}