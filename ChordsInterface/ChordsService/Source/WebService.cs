using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;

namespace ChordsInterface.Service
{
    public class WebService : IService
    {
        private const string createMeasurementPath = "measurements/url_create?";
        private const string instrumentIdPath = "instrument_id=";
        private const string dataPath = "temp=";
        private const string timestampPath = "at=";
        private const string keyPath = "key=";
        private const string keyValue = "key";
        private const string testTag = "test";

        private const string CreateMeasurementSuccess = "Measurement created.";

        public Api.Container<Infrastructure.NetworkList> GetNetworkList()
        {
            return Api.ApiInterface.GetNetworkList();
        }

        /// <summary>
        ///     Gets a list of all sites from a specified sensor network.
        /// </summary>
        /// <returns></returns>
        public Api.Container<Chords.SiteList> GetSiteList()
        {
            return Api.ApiInterface.GetSiteList();
        }
        
        /// <summary>
        ///     Gets site metadata given its ID.
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public Api.Container<Chords.Site> GetSite(int siteID)
        {
            return Api.ApiInterface.GetSite(siteID);
        }

        /// <summary>
        ///     Gets a list of systems from the specified site.
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public Api.Container<Chords.SystemList> GetSystemList(int siteID)
        {
            return Api.ApiInterface.GetSystemList(siteID);
        }

        /// <summary>
        ///     Gets a list of deployments from the specified system.
        /// </summary>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public Api.Container<Chords.InstrumentList> GetInstrumentList(int systemID)
        {
            return Api.ApiInterface.GetInstrumentList(systemID);
        }

        /// <summary>
        ///     Gets a list of data streams from a specified deployment.
        /// </summary>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        public Api.Container<Data.DataStreamList> GetDataStreamList(int deploymentID)
        {
            return Api.ApiInterface.GetDataStreams(deploymentID);
        }

        /// <summary>
        ///     Gets a datastream by ID. Optionally provide its deployment ID for faster searching.
        ///     Will search all streams in network if stream is not found within provided deployment (slow).
        /// </summary>
        /// <param name="streamID"></param>
        /// <param name="deploymentID">
        ///     Optional. Leave empty to search in all deployments in network or specify to get a quicker search.
        /// </param>
        /// <returns></returns>
        public Api.Container<Data.DataStream> GetDataStream(int streamID, int deploymentID = -1)
        {
            return Api.ApiInterface.GetDataStream(streamID, deploymentID);
        }

        /// <summary>
        ///     Retrieves a list of measurements from the provided stream and time range and uploads them to the CHORDS instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime">Optional. If left empty will be set to current time.</param>
        /// <returns></returns>
        public string GetMeasurements(Data.DataStream stream, DateTime startTime, DateTime endTime = default(DateTime))
        {
            if(endTime == default(DateTime))
            {
                endTime = DateTime.UtcNow;
            }

            var apiResponse = Api.ApiInterface.GetMeasurements(stream, startTime, endTime);

            if(apiResponse.Success)
            {
                var measurementList = apiResponse.Object as Chords.MeasurementList;

                foreach(var meas in measurementList.Data)
                {
                    // TODO: Get the real instrument ID
                    meas.InstrumentID = 1;

                    string chordsResponse = CreateMeasurement(meas);

                    if(chordsResponse != CreateMeasurementSuccess)
                    {
                        // Measurement creation, return message from CHORDS
                        return chordsResponse;
                    }
                }

                return "Number of Measurements Created: " + measurementList.Data.Count;
            }
            else
            {
                // GetMeasurements failed, return reason message
                return apiResponse.Message;
            }
        }

        /// <summary>
        ///     Posts a single measurement to the CHORDS instance.
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns></returns>
        public string CreateMeasurement(Chords.Measurement measurement)
        {
            string uri = CreateMeasurementUri(measurement, true);
            var httpTask = ChordsInterface.Http.GetAsync(ChordsInterface.ChordsHostUrl + uri);

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

        /// <summary>
        ///     Builds a CHORDS Put Data uri given the provided measurement and test data flag.
        /// </summary>
        /// <param name="measurement"></param>
        /// <param name="isTestData"></param>
        /// <returns></returns>
        private string CreateMeasurementUri(Chords.Measurement measurement, bool isTestData = true)
        {
            string uri =
                createMeasurementPath +
                instrumentIdPath + measurement.InstrumentID.ToString() +
                "&" + dataPath + measurement.Value.ToString() +
                "&" + keyPath + keyValue;

            // Insert timestamp
            // Get measurement timestamp, using current local time for now
            // The ToString() arg formats the date as ISO-8601
            String timestamp;

            if(measurement.TimeStamp != null)
            {
                //DateTime measTime = DateTime.Parse(measurement.TimeStamp);
                //timestamp = measTime.ToString("o");
                timestamp = measurement.TimeStamp;
            }
            else
            {
                timestamp = DateTime.Now.ToString("o");
            }

            uri += "&" + timestampPath + timestamp;

            // Insert test tag if needed
            if (isTestData)
            {
                uri += "&" + testTag;
            }

            return uri;
        } 

    }
}
