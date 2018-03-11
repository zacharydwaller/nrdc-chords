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
    //Creates an API to call SessionManager functions via HTTP
    [RoutePrefix("Session")]
    public class SessionController : ApiController
    {
        /// <summary>
        /// Creates SessionInitializer object with given parameters and calls the SessionManager function InitializeSession to create the session
        /// </summary>
        /// <param name="netAlias"></param> 
        /// <param name="streamIDs"></param> 
        /// <param name="startTime"></param> 
        /// <param name="endTime"></param> 
        /// <param name="description"></param>
        /// <returns>A string response message from the InitializeSession function</returns>
        [Route("NewSession")]
        [HttpGet]
        public Container<string> InitializeSession([FromUri] string netAlias, [FromUri] int[] streamIDs, [FromUri] string startTime = null, [FromUri] string endTime = null, [FromUri] string description = "")
        {
            var args = new SessionInitializer(netAlias, streamIDs, startTime, endTime, description);
           
            return SessionManager.InitializeSession(args);
        }

        /// <summary>
        /// Calls the SessionManager function GetSession to find a session by its key
        /// </summary>
        /// <param name="key"></param> 
        /// <returns>A Session Container associated with the specified key</returns>
        [Route("GetSession")]
        [HttpGet]
        public Container<Session> GetSession([FromUri] string key)
        {
            return SessionManager.GetSession(key);
        }

        /// <summary>
        /// Calls the SessionManager function GetSessionList to get a list of all sessions
        /// </summary>
        /// <param name=""></param> 
        /// <returns>A Session Container that has a list of all sessions</returns>
        [Route("GetSessionList")]
        [HttpGet]
        public Container<Session> GetSessionList()
        {
            return SessionManager.GetSessionList();
        }

        /// <summary>
        /// Calls the SessionManager function RefreshSession to stream all data in the session after the last streamed time
        /// </summary>
        /// <param name="key"></param> 
        /// <returns>A string Container with a success or failure message</returns>
        [Route("RefreshSession")]
        [HttpGet]
        public Container RefreshSession([FromUri] string key)
        {
            return SessionManager.RefreshSession(key);
        }

        /// <summary>
        /// Calls the SessionManager function GetRandomKey to generate a random unused session key
        /// </summary>
        /// <param name=""></param> 
        /// <returns>A string Container with the randomly generated key</returns>
        [Route("RandomKey")]
        [HttpGet]
        public Container<string> GetNewKey()
        {
            return SessionManager.GetRandomKey();
        }
    }
}
