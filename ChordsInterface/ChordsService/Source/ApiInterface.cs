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
        // Returns Container where Object is Nrdc.DataStreamList
        public static Container GetDataStreams(int siteID)
        {
            if(siteID < 1)
            {
                return new Container(null, false, "Site ID out of range");
            }

            // Get data streams
            string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/streams/site/" + siteID.ToString();
            var result = ChordsInterface.Http.GetAsync(uri).Result;
            string message = result.Content.ReadAsStringAsync().Result;

            var streamlist = Json.Parse<Nrdc.DataStreamList>(message);

            // Check stream list
            if(streamlist.Success)
            {
                if (streamlist.Data.Count > 0)
                {
                    return new Container(streamlist);
                }
                else
                {
                    // No streams found
                    return new Container(null, false, "No data streams found with SiteId: " + siteID.ToString());
                }
            }
            else
            {
                // Stream list retrieval failed, return with reason message
                return new Container(null, false, streamlist.Message);
            }
        }

        // Returns Container where Object is Nrdc.DataStream
        public static Container GetDataStream(int siteID, int streamIndex)
        {
            var container = GetDataStreams(siteID);

            // Get container
            if (container.Success)
            {
                var streamlist = container.Object as Nrdc.DataStreamList;

                if (streamIndex >= 0 && streamIndex < streamlist.Data.Count)
                {
                    return new Container(streamlist.Data[streamIndex]);
                }
                else
                {
                    // Stream index too high for stream count
                    return new Container(null, false, "Stream index out of range");
                }
            }
            else
            {
                // GetDataStreams failed, return with reason message
                return container;
            }
        }

        // Returns Container where Object is Nrdc.DataDownloadResponse
        public static Container GetMeasurements(int siteID, int streamIndex, int hoursBack = 24)
        {
            var container = GetDataStream(siteID, streamIndex);

            if (container.Success)
            {
                var stream = container.Object as Nrdc.DataStream;

                // Create stream request HTTP message
                string startTime = DateTime.UtcNow.AddHours(-48 - hoursBack).ToString("s");
                string endTime = DateTime.UtcNow.ToString("s");

                var dataSpecification = new Nrdc.DataSpecification(stream, startTime, endTime);

                var jsonContent = Json.Serialize(dataSpecification);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Create HTTP POST
                string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/download";
                var response = ChordsInterface.Http.PostAsync(uri, stringContent).Result;

                // Check HTTP response
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;

                    var dataDownloadResponse = Json.Parse<Nrdc.DataDownloadResponse>(content);

                    // Check data download response
                    if (dataDownloadResponse.Success)
                    {
                        // Check data download
                        if (dataDownloadResponse.Data.TotalNumberOfMeasurements > 0)
                        {
                            return new Container(dataDownloadResponse);
                        }
                        else
                        {
                            return new Container(null, false, "No measurements found");
                        }
                    }
                    else
                    {
                        // Data download failed
                        return new Container(null, false, dataDownloadResponse.Message);
                    }
                }
                else
                {
                    // HTTP didn't return OK
                    return new Container(null, false, "Error From: " + response.RequestMessage + "\n" + response.ReasonPhrase);
                }
            }
            else
            {
                // GetDataStream failed, return with reason message
                return container;
            }
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

    public class Container
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Nrdc.NrdcType Object { get; set; }

        public Container(Nrdc.NrdcType obj, bool success = true, string message = default(string))
        {
            Object = obj;
            Success = success;
            Message = message;
        }
    }
}
