using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NCInterface.Controllers
{
    public class NCInterfaceController : ApiController
    {
        // GET: api/NCInterface
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
