using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wendy.Tasks.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy3Tests.Utils;
using Wendy.Tasks.Utils;

namespace Wendy.Tasks.Converters.Tests
{
    [TestClass()]
    public class OldWendyFileConverterTests
    {
        [TestMethod()]
        public void ToInvoiceHistoryTest()
        {
            string wendyXml = Resource.GetResourceAsString("Wendy.xml");
            var wendyData = XmlUtil.DeserializeXML<Model.Wendy1.OldWendyFile>(wendyXml, nameSpace: null);

            var invoiceHistory = OldWendyFileConverter.ToInvoiceHistory(wendyData);

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
                int index = 0;
                Assert.AreEqual(0, invoiceHistory.Invoices[index].Id);
                Assert.AreEqual(true, invoiceHistory.Invoices[index].IsBalanced());
                //Assert.AreEqual(0U, invoiceHistory.Invoices[index].GetReadOut());
            }
            //{
            //    int index = 1;
            //    Assert.AreEqual(1, wendyData.Invoices[index].Id);
            //    Assert.AreEqual(false, wendyData.Invoices[index].Balanced);
            //    Assert.AreEqual(18U, wendyData.Invoices[index].Estimation);
            //}


            //Assert.AreEqual(28, wendyData.Users.Count);
            //{
            //    int index = 1;
            //    Assert.AreEqual(0, wendyData.Users[index].InvoiceId);
            //    Assert.AreEqual("Apartment A", wendyData.Users[index].User);
            //}
            //{
            //    int index = 2;
            //    Assert.AreEqual(1, wendyData.Users[index].InvoiceId);
            //    Assert.AreEqual("Apartment B", wendyData.Users[index].User);
            //}
        }
    }
}