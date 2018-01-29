using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NCInterface.Structures;

namespace NCInterface.Controllers
{
    [RoutePrefix("ChordsBot")]
    public class ChordsBotController : ApiController
    {
        [Route("Login")]
        [HttpGet]
        public Container<String> ChordsLogin()
        {
            //return new Container<string>(ChordsBot.Login(), true);
            return new Container<string>("", true);
        }
    }
}
