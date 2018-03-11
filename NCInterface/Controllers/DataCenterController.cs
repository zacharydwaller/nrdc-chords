using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface.Controllers
{
    //Creates API to call DataCenter functions via HTTP
    [RoutePrefix("DataCenter")] 
    public class DataCenterController : ApiController
    {
        //Sets timeout time for HttPClient object
        private static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        };

        /// <summary>
        /// Calls the DataCenter GetNetworkList function
        /// </summary>
        /// <param name=""></param>
        /// <returns>Network Container of all sensor networks</returns>
        [HttpGet]
        public Container<Network> GetNetworkList()
        {
            return DataCenter.GetNetworkList();
        }

        /// <summary>
        /// Calls the DataCenter GetNetwork function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns>Network Container with metadata of a network</returns>
        [Route("{networkAlias}")]
        [HttpGet]
        public Container<Network> GetNetwork(string networkAlias)
        {
            return DataCenter.GetNetwork(networkAlias);
        }

        /// <summary>
        /// Calls the DataCenter GetSiteList function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns>A list of all sites in the given sensor network</returns>
        [Route("{networkAlias}/sites")]
        [HttpGet]
        public Container<Site> GetSiteList(string networkAlias)
        {
            return DataCenter.GetSiteList(networkAlias);
        }

        /// <summary>
        /// Calls the DataCenter GetSite function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns>A Site Container with the Site metadata for the given siteID</returns>
        [Route("{networkAlias}/sites")]
        [HttpGet]
        public Container<Site> GetSite(string networkAlias, [FromUri] int siteID)
        {
            return DataCenter.GetSite(networkAlias, siteID);
        }

        /// <summary>
        /// Calls the DataCenter GetSystemList function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns>An NrdcSystem Container with a list of all systems at the specified site</returns>
        [Route("{networkAlias}/systems")]
        [HttpGet]
        public Container<NrdcSystem> GetSystemList(string networkAlias, [FromUri] int siteID = 0)
        {
            return DataCenter.GetSystemList(networkAlias, siteID);
        }

        /// <summary>
        /// Calls the DataCenter GetSystem function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <param name="systemID"></param>
        /// <returns>An NrdcSystem Container with the system metadata for the given site and system ID</returns>
        [Route("{networkAlias}/systems")]
        [HttpGet]
        public Container<NrdcSystem> GetSystem(string networkAlias, [FromUri] int siteID, [FromUri] int systemID)
        {
            return DataCenter.GetSystem(networkAlias, siteID, systemID);
        }

        /// <summary>
        /// Calls the DataCenter GetDeploymentList function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <returns>A Deployment Container with a list of all Deployments in the given system</returns>
        [Route("{networkAlias}/deployments")]
        [HttpGet]
        public Container<Deployment> GetDeploymentList(string networkAlias, [FromUri] int systemID = 0)
        {
            return DataCenter.GetDeploymentList(networkAlias, systemID);
        }

        /// <summary>
        /// Gets deployment metadata from a given system and deployment ID
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <param name="deploymentID"></param>
        /// <returns>A Deployment Container with the Deployment metadata from the Deployment with the specified systemID and deploymentID</returns>
        [Route("{networkAlias}/deployments")]
        [HttpGet]
        public Container<Deployment> GetDeployment(string networkAlias, [FromUri] int systemID, [FromUri] int deploymentID)
        {
            return DataCenter.GetDeployment(networkAlias, systemID, deploymentID);
        }

        /// <summary>
        /// Calls the DataCenter GetDataStreams function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="deploymentID"></param>
        /// <returns>A DataStream Container with all datastreams belonging to the specified Deployment</returns>
        [Route("{networkAlias}/streams")]
        [HttpGet]
        public Container<Structures.Data.DataStream> GetDataStreamList(string networkAlias, [FromUri] int deploymentID = 0)
        {
            return DataCenter.GetDataStreams(networkAlias, deploymentID);
        }

        /// <summary>
        /// Calls the DataCenter GetDataStream function
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="streamID"></param>
        /// <param name="deploymentID">
        /// Optional. Leave empty to search in all deployments in network or specify to get a quicker search.
        /// </param>
        /// <returns>A DataStream container found by searching for the specified streamID</returns>
        [Route("{networkAlias}/streams")]
        [HttpGet]
        public Container<Structures.Data.DataStream> GetDataStream(string networkAlias, [FromUri] int streamID, [FromUri] int deploymentID = 0)
        {
            return DataCenter.GetDataStream(networkAlias, streamID, deploymentID);
        }
        
    }
}
