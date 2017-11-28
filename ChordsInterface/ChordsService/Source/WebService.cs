using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using ChordsInterface.Chords;

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

        public string GetSiteList()
        {
            var container = Api.ApiInterface.GetSiteList();
            return Api.Json.Serialize(container);
        }
        
        public string GetSite(int siteID)
        {
            var container = Api.ApiInterface.GetSite(siteID);
            return Api.Json.Serialize(container);
        }

        public string GetSystemList(int siteID)
        {
            var container = Api.ApiInterface.GetSystemList(siteID);
            return Api.Json.Serialize(container);
        }

        public string GetInstrumentList(int systemID)
        {
            return string.Empty;
        }

        public string GetMeasurements(int siteID, int streamIndex, int hoursBack)
        {
            var apiResponse = Api.ApiInterface.GetMeasurements(siteID, streamIndex, hoursBack);

            if(apiResponse.Success)
            {
                var measurementList = apiResponse.Object as MeasurementList;

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
