using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wendy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy3Tests.Utils;
using Wendy.Tasks.Utils;

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
            Assert.AreEqual(15U, invoiceHistory.CalculateConsumption(1));
        }
    }
}