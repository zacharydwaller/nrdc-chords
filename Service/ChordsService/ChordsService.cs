using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net.Http;

namespace ChordsService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class ChordsService : IService
    {
        private const string createMeasurementPath = "measurements/url_create?";
        private const string instrumentIdPath = "instrument_id=";
        private const string dataPath = "temp=";
        private const string timestampPath = "at=";
        private const string keyPath = "key=";
        private const string keyValue = "key";
        private const string testTag = "test";
        private const string portalUrl = "http://ec2-52-8-224-195.us-west-1.compute.amazonaws.com/";

        public string CreateMeasurement(Measurement measurement)
        {
            HttpClient http = new HttpClient();
            string uri = CreateMeasurementUri(measurement.Instrument, measurement.Value, true);
            var httpTask = http.GetAsync(portalUrl + uri);

            try
            {
                httpTask.Wait();
            }
            catch(Exception e)
            {
                return e.Message;
            }

            var httpResponse = httpTask.Result;

            var readTask = httpResponse.Content.ReadAsStringAsync();
            readTask.Wait();

            var contentString = readTask.Result;

            return contentString;
        }

        private string CreateMeasurementUri(uint instrumentId, int dataValue, bool isTestData = true)
        {
            string uri =
                createMeasurementPath +
                instrumentIdPath + instrumentId.ToString() +
                "&" + dataPath + dataValue.ToString() +
                "&" + keyPath + keyValue;

            // Insert timestamp
            // Get measurement timestamp, using current local time for now
            // The ToString() arg formats the date as ISO-8601
            String timestamp = DateTime.Now.ToString("o");

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
