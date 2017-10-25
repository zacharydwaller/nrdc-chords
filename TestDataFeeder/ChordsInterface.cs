using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace TestDataFeeder
{
    public class ChordsInterface
    {
        private const string createMeasurementPath = "measurements/url_create?";
        private const string instrumentIdPath = "instrument_id=";
        private const string dataPath = "temp=";
        private const string timestampPath = "at=";
        private const string keyPath = "key=";
        private const string keyValue = "key";
        private const string testTag = "test";

        private HttpClient http;

        // Current AWS portal: "http://ec2-52-8-224-195.us-west-1.compute.amazonaws.com/"
        public string PortalUrl { get; set; }

        public ChordsInterface(string portalUrl)
        {
            PortalUrl = portalUrl;
            http = new HttpClient();
        }

        /*
         *Async method that attempts to log a measurement to the CHORDS instrument
         */
        public async Task<bool> CreateMeasurementAsync(uint instrumentId, int dataValue)
        {
            // Send HTTP GET request to CHORDS to log a measurement
            string uri = CreateMeasurementUri(instrumentId, dataValue, true);
            var httpTask = await http.GetAsync(uri);

            if(!httpTask.IsSuccessStatusCode)
            {
                System.Console.Error.WriteLine("HTTP Request failed. Uri: " + uri);
                System.Console.Error.WriteLine(httpTask.ReasonPhrase);
                return false;
            }

            return true;
        }

        private string CreateMeasurementUri(uint instrumentId, int dataValue, bool isTestData = true)
        {
            string uri =
                PortalUrl +
                createMeasurementPath +
                instrumentIdPath + instrumentId.ToString() +
                "&" + dataPath + dataValue.ToString() +
                "&" + keyPath + keyValue;

            // Insert timestamp
            // Get measurement timestamp, using current local time for now
            // The ToString() args format the date as ISO-8601
            String timestamp = DateTime.Now.ToString("s", System.Globalization.CultureInfo.InvariantCulture);

            uri += "&" + timestampPath + timestamp;

            // Insert test tag if needed
            if(isTestData)
            {
                uri += "&" + testTag;
            }

            return uri;
        }
    }
}
