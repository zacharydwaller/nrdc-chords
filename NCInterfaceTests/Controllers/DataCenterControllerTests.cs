using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCInterface.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using NCInterface.Structures;
using Newtonsoft.Json;

namespace NCInterface.Controllers.Tests
{
    [TestClass()]
    public class DataCenterControllerTests
    {

        /// <summary>
        /// Launch service without debugging using Ctrl+F5 then run tests
        /// </summary>

        public string BaseUrl { get; set; } = "http://localhost:3485/DataCenter";
        public HttpClient http = new HttpClient();

        [TestMethod()]
        public void GetNetworkListTest()
        {
            string uri = BaseUrl;
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Network>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(3, container.Data.Count);
            Assert.AreEqual("NevCAN", container.Data[0].Alias);
        }

        [TestMethod()]
        public void GetNetworkTest()
        {
            // Get Sucess
            string uri = BaseUrl + "/NevCAN";
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Network>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual("NevCAN", container.Data[0].Alias);

            // Get Fail
            uri = BaseUrl + "/Fail";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Network>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetSiteListTest()
        {
            // Get Success
            string uri = BaseUrl + "/NevCAN/sites";
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Site>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(12, container.Data.Count);
            Assert.AreEqual("Sheep 1", container.Data[0].Alias);

            // Get Fail - bad net alias
            uri = uri.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Site>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetSiteTest()
        {
            string format = BaseUrl + "/NevCAN/sites?siteID={0}";

            // Get Success
            string uri = string.Format(format, 1);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Site>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual("Sheep 1", container.Data[0].Alias);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Site>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad site id
            uri = string.Format(format, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Site>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetSystemListTest()
        {
            string format = BaseUrl + "/NevCAN/systems?siteID={0}";

            // Get Success
            string uri = string.Format(format, 1);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(2, container.Data.Count);
            Assert.AreEqual("Meteorological", container.Data[1].Name);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad site id
            uri = string.Format(format, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - no site id
            uri = BaseUrl + "/NevCAN/systems";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetSystemTest()
        {
            string format = BaseUrl + "/NevCAN/systems?siteID={0}&systemID={1}";

            // Get Success
            string uri = String.Format(format, 1, 13);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual("Meteorological", container.Data[0].Name);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad site id
            uri = string.Format(format, 999, 13);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad system id
            uri = string.Format(format, 1, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - no site id
            uri = BaseUrl + "/NevCAN/systems";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.NrdcSystem>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetDeploymentListTest()
        {
            string format = BaseUrl + "/NevCAN/deployments?systemID={0}";

            // Get Success
            string uri = string.Format(format, 13);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(22, container.Data.Count);
            Assert.AreEqual("Air temperature (10-meter) monitor", container.Data[0].Name);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad systemID
            uri = string.Format(format, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - no site id
            uri = BaseUrl + "/NevCAN/deployments";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetDeploymentTest()
        {
            string format = BaseUrl + "/NevCAN/deployments?systemID={0}&deploymentID={1}";

            // Get Success
            string uri = string.Format(format, 13, 13);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual("Air temperature (10-meter) monitor", container.Data[0].Name);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad systemID
            uri = string.Format(format, 999, 13);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad deploymentID
            uri = string.Format(format, 13, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad parameters
            uri = string.Format(format, 999, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - no parameters
            uri = BaseUrl + "/NevCAN/deployments";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Infrastructure.Deployment>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetDataStreamListTest()
        {
            string format = BaseUrl + "/NevCAN/streams?deploymentID={0}";

            // Get Success
            string uri = string.Format(format, 24);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(8, container.Data.Count);
            Assert.AreEqual(48, container.Data[0].ID);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad deploymentID
            uri = string.Format(format, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - no parameters
            uri = BaseUrl + "/NevCAN/streams";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);
        }

        [TestMethod()]
        public void GetDataStreamTest()
        {
            string format = BaseUrl + "/NevCAN/streams?streamID={0}&deploymentID={1}";
            string formatNoDeployment = BaseUrl + "/NevCAN/streams?streamID={0}";

            // Get Success - with deployment ID
            string uri = string.Format(format, 48, 24);
            var content = DataCenter.GetHttpContent(uri);
            var container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual(48, container.Data[0].ID);

            // Get Success - without deployment ID
            uri = string.Format(format, 48, 24);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual(48, container.Data[0].ID);

            // Get Success - bad deploymentID
            uri = string.Format(format, 48, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsTrue(container.Success);
            Assert.AreEqual(1, container.Data.Count);
            Assert.AreEqual(48, container.Data[0].ID);

            // Get Fail - bad net alias
            uri = format.Replace("NevCAN", "Fail");
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad streamID
            uri = string.Format(format, 9999, 24);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - bad parameters
            uri = string.Format(format, 9999, 999);
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);

            // Get Fail - no parameters
            uri = BaseUrl + "/NevCAN/streams";
            content = DataCenter.GetHttpContent(uri);
            container = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(content);

            Assert.IsFalse(container.Success);
        }
    }
}