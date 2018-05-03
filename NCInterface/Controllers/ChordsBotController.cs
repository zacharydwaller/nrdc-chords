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
    //Creates API to call ChordsBot functions via HTTP
    public class ChordsBotController : ApiController 
    {
        /// <summary>
        /// Calls the ChordsBot ChordsLogin function
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
        /// Calls the ChordsBot CreateInstrument function
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
        /// Calls the ChordsBot DeleteInstrument function  
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
        /// Prepares DateTime and int parameters, calls the ChordsBot ConfigureVariables function 
        /// </summary>
        /// <param name="instrumentId"></param>
        /// <returns>ChordsBot object Container corresponding to the session input</returns>
        [Route("ConfigureVariables")]
        [HttpGet]
        public Container ConfigureVariables([FromUri] int instrumentId)
        {
            //Creates a DateTime object at an arbitrary date
            DateTime testDate = new DateTime(2008,5,1,8,30,52);
            //Creates a List of ints to identify datastreams
            List<int> DataStreamID = new List<int>(); 
            DataStreamID.Add(1);
            //Creates a session object with the test DateTime object and list of DataStreamIDs
            Session session = new Session("testKey", "nevcan", DataStreamID, testDate); 
            return ChordsBot.ConfigureVariables(session);
        }


        /// <summary>
        /// Get information about target data by passing in a Session object so that Grafana know which datastreams on CHORDS to plot on a specific graph
        /// </summary>
        /// <param name="instrumentId"></param>
        /// <returns>A list of strings containing the variable IDs of each datastreams in the Session object</returns>
        [Route("GetTarget")]
        [HttpGet]
        public List<string> GetTarget([FromUri] int instrumentId)
        {
            return ChordsBot.GetTarget(instrumentId);
        }


    }
}
