using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Wendy3Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UnitTest()
        {
            string wendyXml = Utils.Resource.GetResourceAsString("Wendy.xml");
            var wendyData = Wendy.Tasks.Utils.XmlUtil.DeserializeXML<Wendy.Model.Wendy1.OldWendyFile>(wendyXml, nameSpace: null);

            Assert.AreEqual(0U, wendyData.Config.StartConsumption);
            Assert.AreEqual(14, wendyData.Invoices.Count);
            Assert.AreEqual(28, wendyData.Users.Count);
            Assert.AreEqual(2, wendyData.UserNames.Count);
            Assert.AreEqual(4, wendyData.ConfigFees.Count);
        }
    }
}
