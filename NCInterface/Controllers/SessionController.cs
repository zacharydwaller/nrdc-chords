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
        public Container InitializeSession([FromUri] SessionInitializer args)
        {
            return SessionManager.InitializeSession(args);
        }

        [Route("RandomKey")]
        [HttpGet]
        public Container<string> GetNewKey()
        {
            return SessionManager.GetRandomKey();
        }
    }
}
