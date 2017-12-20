using System;
using System.Collections.Generic;
using System.Web.Http;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface.Controllers
{
    public class NCInterfaceController : ApiController
    {
        // GET: api/NCInterface
        public Container<Network> Get()
        {
            return new Container<Network>(Config.NetworkDiscoveryUrl);
        }

        // GET: api/NCInterface/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NCInterface
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/NCInterface/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/NCInterface/5
        public void Delete(int id)
        {
        }
    }
}
