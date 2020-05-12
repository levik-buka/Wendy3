using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wendy.Tasks.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wendy3Tests.Utils;

namespace Wendy.Tasks.Utils.Tests
{
    [TestClass()]
    public class XMLTests
    {
        [TestMethod()]
        public void DeserializeWendyXMLTest()
        {
            string wendyXml = Resource.GetResourceAsString("Wendy.xml");
            var wendyData = XML.DeserializeXML<Model.Wendy1.OldWendyFile>(wendyXml, nameSpace: null);

            Assert.AreEqual(0U, wendyData.Config.StartConsumption);
            
            Assert.AreEqual(14, wendyData.Invoices.Count);
            {
                int index = 0;
                Assert.AreEqual(0, wendyData.Invoices[index].Id);
                Assert.AreEqual(true, wendyData.Invoices[index].Balanced);
                Assert.AreEqual(0U, wendyData.Invoices[index].Estimation);
            }
            {
                int index = 1;
                Assert.AreEqual(1, wendyData.Invoices[index].Id);
                Assert.AreEqual(false, wendyData.Invoices[index].Balanced);
                Assert.AreEqual(18U, wendyData.Invoices[index].Estimation);
            }


            Assert.AreEqual(28, wendyData.Users.Count);
            {
                int index = 1;
                Assert.AreEqual(0, wendyData.Users[index].InvoiceId);
                Assert.AreEqual("Apartment A", wendyData.Users[index].User);
            }
            {
                int index = 2;
                Assert.AreEqual(1, wendyData.Users[index].InvoiceId);
                Assert.AreEqual("Apartment B", wendyData.Users[index].User);
            }

            Assert.AreEqual(2, wendyData.UserNames.Count);
            Assert.AreEqual("Apartment B", wendyData.UserNames[0].Name);
            Assert.AreEqual(0U, wendyData.UserNames[0].StartConsumption);

            Assert.AreEqual(4, wendyData.ConfigFees.Count);
            Assert.AreEqual(false, wendyData.ConfigFees[0].BeginSpecified);
            Assert.AreEqual(true, wendyData.ConfigFees[0].EndSpecified);
            Assert.AreEqual(new DateTime(2007, 12, 31), wendyData.ConfigFees[0].End);
        }
    }
}