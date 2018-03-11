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
    public class ChordsBotController : ApiController //Creates API to call ChordsBot functions via HTTP
    {
        /// <summary>
        /// Calls the ChordsBot login function
        /// </summary>
        /// <param name=""></param>
        /// <returns>String Container confirmation</returns>
        [Route("Login")]
        [HttpGet]
        public Container<String> ChordsLogin()
        {
            return new Container<string>("", true);
        }

        /// <summary>
        /// Calls the ChordsBot create instrument function
        /// </summary>
        /// <param name=""></param>
        /// <returns>Instrument ID as an int Container</returns>
        [Route("CreateInstrument")]
        [HttpGet]
        public Container<int> CreateInstrument()
        {
            return ChordsBot.CreateInstrument("test");
        }

        /// <summary>
        /// Calls the ChordsBot delete instrument function  
        /// </summary>
        /// <param name="instrumentId"></param>
        /// <returns>Instrument ID as an int Container</returns>
        [Route("DeleteInstrument")]
        [HttpGet]
        public Container<int> DeleteInstrument([FromUri] int instrumentId)
        {
            return ChordsBot.DeleteInstrument(instrumentId);
        }

        /// <summary>
        /// Calls the ChordsBot configure variables function 
        /// </summary>
        /// <param name="instrumentId"></param>
        /// <returns>ChordsBot object Container corresponding to the session input</returns>
        [Route("ConfigureVariables")]
        [HttpGet]
        public Container ConfigureVariables([FromUri] int instrumentId)
        {
            DateTime testDate = new DateTime(2008,5,1,8,30,52); //Creates a DateTime object at an arbitrary date
            List<int> DataStreamID = new List<int>();//Creates a List of ints to identify datastreams
            DataStreamID.Add(1);
            Session session = new Session("testKey", "nevcan", DataStreamID, testDate);//Creates a session object with the test DateTime object and list of DataStreamIDs
            return ChordsBot.ConfigureVariables(session);
        }
    }
}
