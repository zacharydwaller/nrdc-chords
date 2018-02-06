using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCInterface.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCInterface.Structures.Tests
{
    [TestClass()]
    public class SessionInitializerTests
    {
        [TestMethod()]
        public void ValidateTest()
        {
            var sinit = new SessionInitializer();

            // Valid data - null endtime
            sinit.StreamIDs = new int[] { 1, 2, 3 };
            sinit.StartTime = DateTime.UtcNow.AddHours(-1).ToString("s");
            sinit.EndTime = null;
        
            Assert.IsTrue(sinit.Validate().Success);

            // Valid - provided endtime
            sinit.EndTime = DateTime.UtcNow.ToString("s");

            Assert.IsTrue(sinit.Validate().Success);

            // Null IDs
            var ids = sinit.StreamIDs;
            sinit.StreamIDs = null;

            Assert.IsFalse(sinit.Validate().Success);

            // Null startTime
            var start = sinit.StartTime;
            sinit.StreamIDs = ids;
            sinit.StartTime = null;

            Assert.IsFalse(sinit.Validate().Success);

            // Invalid ID
            sinit.StartTime = start;
            sinit.StreamIDs[0] = -1;

            Assert.IsFalse(sinit.Validate().Success);

            // StartTime > EndTime
            sinit.StreamIDs[0] = 1;
            sinit.StartTime = DateTime.UtcNow.AddHours(1).ToString("s");

            Assert.IsFalse(sinit.Validate().Success);
        }
    }
}