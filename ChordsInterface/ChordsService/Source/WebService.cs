using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

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

        public string PullMeasurements(int siteID, int streamIndex)
        {
            var response = Api.ApiInterface.GetMeasurements(siteID, streamIndex);

            if (response.Success)
            {
                var dataDownload = response.Data;

                foreach(var nMeas in dataDownload.Measurements)
                {
                    var cMeas = Api.Converter.Convert(nMeas);

                    // TODO: Get the real instrument ID
                    cMeas.InstrumentID = 0;

                    CreateMeasurement(cMeas);
                }
                
                return "Number of Measurements Created: " + dataDownload.TotalNumberOfMeasurements.ToString();
            }
            else
            {
                return response.Message;
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
                return e.Message;
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
