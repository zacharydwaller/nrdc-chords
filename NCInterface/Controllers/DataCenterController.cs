using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface.Controllers
{
    [RoutePrefix("DataCenter")]
    public class DataCenterController : ApiController
    {
        private static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        };

        // GET: DataCenter
        [HttpGet]
        public Container<Network> GetNetworkList()
        {
            return DataCenter.GetNetworkList();
        }

        [Route("{networkAlias}")]
        [HttpGet]
        public Container<Network> GetNetwork(string networkAlias)
        {
            return DataCenter.GetNetwork(networkAlias);
        }

        /// <summary>
        /// Gets a list of sites from a network
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        [Route("{networkAlias}/sites")]
        [HttpGet]
        public Container<Site> GetSiteList(string networkAlias)
        {
            return DataCenter.GetSiteList(networkAlias);
        }

        /// <summary>
        /// Gets site metadata given its ID.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/sites")]
        [HttpGet]
        public Container<Site> GetSite(string networkAlias, [FromUri] int siteID)
        {
            return DataCenter.GetSite(networkAlias, siteID);
        }

        /// <summary>
        /// Gets a list of systems from the specified site.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/systems")]
        [HttpGet]
        public Container<NrdcSystem> GetSystemList(string networkAlias, [FromUri] int siteID = 0)
        {
            return DataCenter.GetSystemList(networkAlias, siteID);
        }

        /// <summary>
        /// Gets the system metadata from a given site and system ID
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/systems")]
        [HttpGet]
        public Container<NrdcSystem> GetSystem(string networkAlias, [FromUri] int siteID, [FromUri] int systemID)
        {
            return DataCenter.GetSystem(networkAlias, siteID, systemID);
        }

        /// <summary>
        /// Gets a list of deployments from the specified system.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/deployments")]
        [HttpGet]
        public Container<Deployment> GetDeploymentList(string networkAlias, [FromUri] int systemID = 0)
        {
            return DataCenter.GetDeploymentList(networkAlias, systemID);
        }

        /// <summary>
        /// Gets deployment metadata from a given system and deployment ID
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/deployments")]
        [HttpGet]
        public Container<Deployment> GetDeployment(string networkAlias, [FromUri] int systemID, [FromUri] int deploymentID)
        {
            return DataCenter.GetDeployment(networkAlias, systemID, deploymentID);
        }

        /// <summary>
        /// Gets a list of data streams from a specified deployment.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        [Route("{networkAlias}/streams")]
        [HttpGet]
        public Container<Structures.Data.DataStream> GetDataStreamList(string networkAlias, [FromUri] int deploymentID = 0)
        {
            return DataCenter.GetDataStreams(networkAlias, deploymentID);
        }

        /// <summary>
        /// Gets a datastream by ID. Provide its deployment ID for faster searching.
        /// Will search all streams in network if stream is not found within provided deployment (slow).
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="streamID"></param>
        /// <param name="deploymentID">
        /// Optional. Leave empty to search in all deployments in network or specify to get a quicker search.
        /// </param>
        /// <returns></returns>
        [Route("{networkAlias}/streams")]
        [HttpGet]
        public Container<Structures.Data.DataStream> GetDataStream(string networkAlias, [FromUri] int streamID, [FromUri] int deploymentID = 0)
        {
            return DataCenter.GetDataStream(networkAlias, streamID, deploymentID);
        }

        /// <summary>
        /// Retrieves a list of measurements from the provided stream and time range and uploads them to the CHORDS instance.
        /// Optionally provide deployment ID to find stream quicker.
        /// </summary>
        /// <param name="streamID"></param>
        /// <param name="deploymentID">Optional. If left empty will take longer to find stream metadata.</param>
        /// /// <param name="startTime"></param>
        /// <param name="endTime">Optional. If left empty will be set to current time.</param>
        /// <returns></returns>
        [Route("{networkAlias}/pushData")]
        [HttpGet]
        public Container<string> PostMeasurements(
            string networkAlias,
            [FromUri] int streamID = 0,
            [FromUri] int deploymentID = 0,
            [FromUri] string startTime = "",
            [FromUri] string endTime = "")
        {
            //int instrumentID = stream.Deployment.ID;
            int instrumentID = 1;

            // Get the stream metadata
            var streamContainer = DataCenter.GetDataStream(networkAlias, streamID, deploymentID);
            Structures.Data.DataStream stream;

            if (streamContainer.Success)
            {
                stream = streamContainer.Data[0];
            }
            else
            {
                // Couldn't get stream, return error
                return new Container<string>("", false, streamContainer.Message);
            }

            // Get DateTimes
            DateTime start, end;
            
            if(!DateTime.TryParse(startTime, out start))
            {
                return new Container<string>("", false, "Unable to read startTime: " + startTime);
            }

            if(endTime == "")
            {
                end = DateTime.UtcNow;
            }
            else if(!DateTime.TryParse(endTime, out end))
            {
                return new Container<string>("", false, "Unable to read endTime: " + endTime);
            }

            // Retrieve measurements
            var apiResponse = DataCenter.GetMeasurements(networkAlias, stream, start, end);

            if (apiResponse.Success)
            {
                var measurementList = apiResponse.Data as List<Structures.Data.Measurement>;

                foreach (var meas in measurementList)
                {
                    string chordsResponse = CreateMeasurement(meas, instrumentID);

                    if (chordsResponse != CreateMeasurementSuccess)
                    {
                        // Measurement creation, return message from CHORDS
                        return new Container<string>(chordsResponse, true);
                    }
                }

                return new Container<string>("Number of Measurements Created: " + measurementList.Count, true);
            }
            else
            {
                // GetMeasurements failed, return reason message
                return new Container<string>(apiResponse.Message);
            }
        }

        /// <summary>
        /// Posts a single measurement to the CHORDS instance.
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns></returns>
        protected string CreateMeasurement(Structures.Data.Measurement measurement, int instrumentID)
        {
            string uri = CreateMeasurementUri(measurement, instrumentID, true);
            var httpTask = client.GetAsync(Config.ChordsHostUrl + uri);

            try
            {
                httpTask.Wait();
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }

            if (httpTask.Result.IsSuccessStatusCode)
            {
                var contentString = httpTask.Result.Content.ReadAsStringAsync().Result;

                return contentString;
            }
            else
            {
                return httpTask.Result.ReasonPhrase;
            }
        }

        private const string CreateMeasurementSuccess = "Measurement created.";

        /// <summary>
        /// Builds a CHORDS Put Data uri given the provided measurement and test data flag.
        /// </summary>
        /// <param name="measurement"></param>
        /// <param name="isTestData"></param>
        /// <returns></returns>
        private string CreateMeasurementUri(Structures.Data.Measurement measurement, int instrumentID, bool isTestData = true)
        {
            string dataPath = "temp";
            string key = "key";

            string uri = String.Format("measurements/url_create?instrument_id={0}&{1}={2}&key={3}", instrumentID, dataPath, measurement.Value, key);

            // Insert timestamp
            // Get measurement timestamp, using current local time for now
            // The ToString() arg formats the date as ISO-8601
            String timestamp;

            if (measurement.TimeStamp != null)
            {
                //DateTime measTime = DateTime.Parse(measurement.TimeStamp);
                //timestamp = measTime.ToString("o");
                timestamp = measurement.TimeStamp;
            }
            else
            {
                timestamp = DateTime.Now.ToString("o");
            }

            uri += "&at=" + timestamp;

            // Insert test tag if needed
            if (isTestData)
            {
                uri += "&test";
            }

            return uri;
        }

    }
}
