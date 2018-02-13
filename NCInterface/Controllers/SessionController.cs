using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;


namespace NCInterface.Controllers
{
    [RoutePrefix("Session")]
    public class SessionController : ApiController
    {
        [Route("NewSession")]
        [HttpGet]
        public Container<string> InitializeSession([FromUri] string netAlias, [FromUri] int[] streamIDs, [FromUri] string startTime = null, [FromUri] string endTime = null)
        {
            var args = new SessionInitializer(netAlias, streamIDs, startTime, endTime);
           
            return SessionManager.InitializeSession(args);
        }

        [Route("GetSession")]
        [HttpGet]
        public Container<Session> GetSession([FromUri] string key)
        {
            return SessionManager.GetSession(key);
        }

        [Route("RefreshSession")]
        [HttpGet]
        public Container RefreshSession([FromUri] string key)
        {
            return SessionManager.RefreshSession(key);
        }

        [Route("RandomKey")]
        [HttpGet]
        public Container<string> GetNewKey()
        {
            return SessionManager.GetRandomKey();
        }
    }
}
