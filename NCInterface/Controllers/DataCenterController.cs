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
    [RoutePrefix("DataCenter")]
    public class DataCenterController : ApiController
    {
        private static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        };

        // GET: DataCenter
        [HttpGet]
        public Container<Network> GetNetworkList()
        {
            return DataCenter.GetNetworkList();
        }

        [Route("{networkAlias}")]
        [HttpGet]
        public Container<Network> GetNetwork(string networkAlias)
        {
            return DataCenter.GetNetwork(networkAlias);
        }

        /// <summary>
        /// Gets a list of sites from a network
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        [Route("{networkAlias}/sites")]
        [HttpGet]
        public Container<Site> GetSiteList(string networkAlias)
        {
            return DataCenter.GetSiteList(networkAlias);
        }

        /// <summary>
        /// Gets site metadata given its ID.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/sites")]
        [HttpGet]
        public Container<Site> GetSite(string networkAlias, [FromUri] int siteID)
        {
            return DataCenter.GetSite(networkAlias, siteID);
        }

        /// <summary>
        /// Gets a list of systems from the specified site.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/systems")]
        [HttpGet]
        public Container<NrdcSystem> GetSystemList(string networkAlias, [FromUri] int siteID = 0)
        {
            return DataCenter.GetSystemList(networkAlias, siteID);
        }

        /// <summary>
        /// Gets the system metadata from a given site and system ID
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/systems")]
        [HttpGet]
        public Container<NrdcSystem> GetSystem(string networkAlias, [FromUri] int siteID, [FromUri] int systemID)
        {
            return DataCenter.GetSystem(networkAlias, siteID, systemID);
        }

        /// <summary>
        /// Gets a list of deployments from the specified system.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        [Route("{networkAlias}/deployments")]
        [HttpGet]
        public Container<Deployment> GetDeployment(string networkAlias, [FromUri] int systemID, [FromUri] int deploymentID)
        {
            return DataCenter.GetDeployment(networkAlias, systemID, deploymentID);
        }

        /// <summary>
        /// Gets a list of data streams from a specified deployment.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/streams")]
        [HttpGet]
        public Container<Structures.Data.DataStream> GetDataStreamList(string networkAlias, [FromUri] int deploymentID = 0)
        {
            return DataCenter.GetDataStreams(networkAlias, deploymentID);
        }

        /// <summary>
        /// Gets a datastream by ID. Provide its deployment ID for faster searching.
        /// Will search all streams in network if stream is not found within provided deployment (slow).
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="streamID"></param>
        /// <param name="deploymentID">
        /// Optional. Leave empty to search in all deployments in network or specify to get a quicker search.
        /// </param>
        /// <returns></returns>
        [Route("{networkAlias}/streams")]
        [HttpGet]
        public Container<Structures.Data.DataStream> GetDataStream(string networkAlias, [FromUri] int streamID, [FromUri] int deploymentID = 0)
        {
            return DataCenter.GetDataStream(networkAlias, streamID, deploymentID);
        }
        
    }
}
