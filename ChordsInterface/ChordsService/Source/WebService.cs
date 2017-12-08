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

        public Api.Container<Chords.SiteList> GetSiteList()
        {
            return Api.ApiInterface.GetSiteList();
        }
        
        public Api.Container<Chords.Site> GetSite(int siteID)
        {
            return Api.ApiInterface.GetSite(siteID);
        }

        public Api.Container<Chords.SystemList> GetSystemList(int siteID)
        {
            return Api.ApiInterface.GetSystemList(siteID);
        }

        public Api.Container<Chords.InstrumentList> GetInstrumentList(int systemID)
        {
            return Api.ApiInterface.GetInstrumentList(systemID);
        }

        public Api.Container<Data.DataStreamList> GetDataStreamList(int deploymentID)
        {
            return Api.ApiInterface.GetDataStreams(deploymentID);
        }

        public Api.Container<Data.DataStream> GetDataStream(int streamID, int deploymentID = -1)
        {
            return Api.ApiInterface.GetDataStream(streamID, deploymentID);
        }

        public string GetMeasurements(Data.DataStream stream, DateTime startTime, DateTime endTime)
        {
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
