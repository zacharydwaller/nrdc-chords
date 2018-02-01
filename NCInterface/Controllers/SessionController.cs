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
    [RoutePrefix("DataCenter")]
    public class SessionController : ApiController
    {
        private Dictionary<string, Session> SessionDict = new Dictionary<string, Session>();

        public Container<string> InitializeSession([FromBody] SessionInitializer args)
        {
            return new Container<string>("Not implemented");
        }
    }
}
