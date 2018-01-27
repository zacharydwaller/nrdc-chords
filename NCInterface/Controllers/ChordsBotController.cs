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
            var bot = new ChordsBot(Config.ChordsHostUrl);
            return new Container<string>(bot.Login(), true);
        }
    }
}
