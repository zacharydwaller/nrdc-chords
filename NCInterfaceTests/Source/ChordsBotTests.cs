using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCInterface.Tests
{
    [TestClass()]
    public class ChordsBotTests
    {
         
        
        [TestInitialize]
        public void TestInitialize()
        {
            ChordsBot.Initialize(Config.ChordsHostUrl);
        }

        [TestMethod()]
        public void CreateInstrumentTest()
        {
            var result = ChordsBot.CreateInstrument("test");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Data[0] > 0);

            result = ChordsBot.CreateInstrument("/u1F914");
            Assert.IsFalse(result.Success);

        }
    }
}