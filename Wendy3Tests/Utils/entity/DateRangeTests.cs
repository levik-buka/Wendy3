using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wendy.Utils.Entity.Tests
{
    [TestClass()]
    public class DateRangeTests
    {
        [TestMethod()]
        public void InTest()
        {
            DateRange dr = new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31) };

            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31) }));
            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 02, 01) }));
            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2019, 12, 31), End = new DateTime(2020, 02, 01) }));
            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2020, 01, 01), End = null }));
            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2019, 12, 31), End = null }));

            Assert.IsFalse(dr.In(new DateRange { Start = new DateTime(2020, 01, 02), End = new DateTime(2020, 01, 31) }));
            Assert.IsFalse(dr.In(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 30) }));
            Assert.IsFalse(dr.In(new DateRange { Start = new DateTime(2020, 01, 02), End = new DateTime(2020, 01, 30) }));
            Assert.IsFalse(dr.In(new DateRange { Start = new DateTime(2020, 01, 02), End = null }));
        }

        [TestMethod()]
        public void EndlessInTest()
        {
            DateRange dr = new DateRange { Start = new DateTime(2020, 01, 01), End = null };

            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2020, 01, 01), End = null }));
            Assert.IsTrue(dr.In(new DateRange { Start = new DateTime(2019, 12, 31), End = null }));

            Assert.IsFalse(dr.In(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31) }));
            Assert.IsFalse(dr.In(new DateRange { Start = new DateTime(2020, 01, 02), End = null }));
        }

        [TestMethod()]
        public void IntersectsTest()
        {
            DateRange dr = new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31) };

            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 02, 01) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2019, 12, 31), End = new DateTime(2020, 02, 01) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 01), End = null }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2019, 12, 31), End = null }));

            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 02), End = new DateTime(2020, 01, 31) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 30) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 02), End = new DateTime(2020, 01, 30) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 02), End = null }));

            Assert.IsFalse(dr.Intersects(new DateRange { Start = new DateTime(2019, 12, 01), End = new DateTime(2019, 12, 31) }));
            Assert.IsFalse(dr.Intersects(new DateRange { Start = new DateTime(2020, 02, 01), End = new DateTime(2020, 02, 28) }));
            Assert.IsFalse(dr.Intersects(new DateRange { Start = new DateTime(2020, 02, 01), End = null }));
        }

        [TestMethod()]
        public void EndlessIntersectsTest()
        {
            DateRange dr = new DateRange { Start = new DateTime(2020, 01, 01), End = null };

            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 01), End = null }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2019, 12, 31), End = null }));

            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 31) }));
            Assert.IsTrue(dr.Intersects(new DateRange { Start = new DateTime(2020, 01, 02), End = null }));

            Assert.IsFalse(dr.Intersects(new DateRange { Start = new DateTime(2019, 12, 01), End = new DateTime(2019, 12, 31) }));
        }

        [TestMethod()]
        public void GetMonthsTest()
        {
            {
                DateRange dr = new DateRange { Start = new DateTime(2007, 08, 18), End = new DateTime(2007, 12, 19) };
                Assert.AreEqual(4.077m, Math.Round(dr.GetMonths(), 3));
                Assert.AreEqual(123, (dr.End.Value - dr.Start).Days);
            }

            {
                DateRange dr = new DateRange { Start = new DateTime(2007, 12, 20), End = new DateTime(2007, 12, 31) };
                Assert.AreEqual(0.395m, Math.Round(dr.GetMonths(), 3));
                Assert.AreEqual(11, (dr.End.Value - dr.Start).Days);
            }
        }
    }
}