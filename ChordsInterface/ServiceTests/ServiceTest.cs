using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceTests
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestService.ServiceClient stuff = new TestService.ServiceClient();

            string result = stuff.PullSite(1);

            Assert.AreEqual("Sheep 1", result);
        }
    }
}
