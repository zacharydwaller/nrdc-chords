using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface.Controllers
{
    public class DataCenterController : ApiController
    {
        // GET: DataCenter
        public Container<Network> Get()
        {
            return DataCenter.GetNetworkList();
        }

        [Route("DataCenter/{networkAlias}")]
        [HttpGet]
        public Container<Network> GetNetwork(string networkAlias)
        {
            var network = DataCenter.GetNetworkList()
                .Data.FirstOrDefault(n => n.Alias.Equals(networkAlias, StringComparison.InvariantCultureIgnoreCase));
            
            if(network != null)
            {
                return new Container<Network>(network);
            }
            else
            {
                return new Container<Network>(string.Format("Network with alias \"{0}\" not found.", networkAlias));
            }
        }

        /// <summary>
        /// Gets a list of sites from a network
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        [Route("DataCenter/{networkAlias}/sites")]
        [HttpGet]
        public Container<string> GetSiteList(string networkAlias)
        {
            return new Container<string>("sites from " + networkAlias, true);
        }
    }
}
