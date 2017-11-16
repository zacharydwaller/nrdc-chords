using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

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

        public string CreateMeasurement(Chords.Measurement measurement)
        {
            string uri = CreateMeasurementUri(measurement.Instrument, measurement.Value, true);
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

        public string PullSite(int siteId)
        {
            var site = Api.ApiInterface.GetSiteAsync(siteId).Result;

            if (site != null) return site.Name;
            else return "Site could not be found.";
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
