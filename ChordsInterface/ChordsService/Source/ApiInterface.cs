using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ChordsInterface.Api
{ 
    public static class ApiInterface
    {
        public static Nrdc.DataStreamList GetDataStreams(int siteID)
        {
            string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/streams/site/" + siteID.ToString();

            var result = ChordsInterface.Http.GetAsync(uri).Result;
            string message = result.Content.ReadAsStringAsync().Result;

            var streamlist = Json.Parse<Nrdc.DataStreamList>(message);

            return streamlist;
        }

        public static Nrdc.DataStream GetDataStream(int siteID, int streamIndex)
        {
            var streamlist = GetDataStreams(siteID);

            return streamlist.Data[streamIndex];
        }

        public static Nrdc.DataDownloadResponse GetMeasurements(int siteID, int streamIndex)
        {
            var stream = GetDataStream(siteID, streamIndex);

            // Make better constructors for these data structures
            var streamRequest = new Nrdc.DataStreamRequest();
            streamRequest.DataStreamID = stream.ID;
            streamRequest.UnitsID = stream.Units.ID;

            var dataSpecification = new Nrdc.DataSpecification();
            dataSpecification.TimeZoneID = ChordsInterface.DefaultTimeZoneID;
            dataSpecification.StartDateTime = DateTime.UtcNow.AddHours(-24).ToString("s");
            dataSpecification.EndDateTime = DateTime.UtcNow.ToString("s");
            dataSpecification.Skip = 0;
            dataSpecification.Take = ChordsInterface.MaxMeasurements;
            dataSpecification.DataStreams.Add(streamRequest);

            var jsonContent = Json.Serialize(dataSpecification);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/download";

            var response = ChordsInterface.Http.PostAsync(uri, stringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var dataDownloadResponse = Json.Parse<Nrdc.DataDownloadResponse>(content);

            return dataDownloadResponse;
        }

        public static Nrdc.SiteList GetSites()
        {
            string uri = ChordsInterface.InfrastructureServiceUrl + "infrastructure/sites";

            var result = ChordsInterface.Http.GetAsync(uri).Result;
            string message = result.Content.ReadAsStringAsync().Result;

            var sitelist = Json.Parse<Nrdc.SiteList>(message);

            return sitelist;
        }

        public static Nrdc.Site GetSite(int siteID)
        {
            var sitelist = GetSites();

            if (sitelist.Success)
            {
                foreach (var site in sitelist.Data)
                {
                    if(site.ID == siteID)
                    {
                        return site;
                    }
                }
            }

            return null;
        }
    } 
    
}
