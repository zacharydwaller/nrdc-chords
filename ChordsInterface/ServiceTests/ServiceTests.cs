using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChordsInterface;
using ChordsInterface.Chords;

namespace ServiceTests
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void TestCreateMeasurement()
        {
            var service = new ServiceReference.ServiceClient();

            var meas = new Measurement();
            meas.Value = 10;

            // Valid instrument id
            meas.InstrumentID = 1;
            string result = service.CreateMeasurement(meas);
            Assert.AreEqual("Measurement created.".ToUpper(), result.ToUpper());

            // Invalid instrument id
            meas.InstrumentID = 0;
            result = service.CreateMeasurement(meas);
            Assert.AreNotEqual("Measurement created.".ToUpper(), result.ToUpper());

            // Too high instrument id
            meas.InstrumentID = int.MaxValue;
            result = service.CreateMeasurement(meas);
            Assert.AreNotEqual("Measurement created.".ToUpper(), result.ToUpper());
        }
    }
}
