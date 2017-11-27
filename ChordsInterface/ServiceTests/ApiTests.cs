using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChordsInterface.Data;
using ChordsInterface.Api;

namespace ServiceTests
{
    [TestClass]
    public class ApiTests
    {
        [TestMethod]
        public void TestGetDataStreams()
        {
            Container container;

            // Test valid ID
            container = ApiInterface.GetDataStreams(1);
            Assert.IsNotNull(container.Object);
            Assert.IsTrue(container.Success);

            var streamlist = container.Object as DataStreamList;

            Assert.IsNotNull(streamlist);
            Assert.AreNotEqual(0, streamlist.Data.Count);

            // Test invalid IDs
            container = ApiInterface.GetDataStreams(-1);
            Assert.IsNull(container.Object);
            Assert.IsFalse(container.Success);
            Assert.AreNotEqual(string.Empty, container.Message);

            container = ApiInterface.GetDataStreams(int.MaxValue);
            Assert.IsNull(container.Object);
            Assert.IsFalse(container.Success);
            Assert.AreNotEqual(string.Empty, container.Message);
        }

        [TestMethod]
        public void TestGetDataStream()
        {
            Container container;

            // Test valid site Id and stream index
            container = ApiInterface.GetDataStream(1, 1);
            Assert.IsNotNull(container.Object);
            Assert.IsTrue(container.Success);

            var stream = container.Object as DataStream;

            Assert.IsNotNull(stream);
            Assert.AreEqual(1, stream.Site.ID);

            // Test invalid site id
            container = ApiInterface.GetDataStream(-1, 1);
            Assert.IsNull(container.Object);
            Assert.IsFalse(container.Success);
            Assert.AreNotEqual(string.Empty, container.Message);

            container = ApiInterface.GetDataStream(int.MaxValue, 1);
            Assert.IsNull(container.Object);
            Assert.IsFalse(container.Success);
            Assert.AreNotEqual(string.Empty, container.Message);

            // Test invalid stream index
            container = ApiInterface.GetDataStream(1, -1);
            Assert.IsNull(container.Object);
            Assert.IsFalse(container.Success);
            Assert.AreNotEqual(string.Empty, container.Message);

            container = ApiInterface.GetDataStream(1, int.MaxValue);
            Assert.IsNull(container.Object);
            Assert.IsFalse(container.Success);
            Assert.AreNotEqual(string.Empty, container.Message);
        }
    }
}
