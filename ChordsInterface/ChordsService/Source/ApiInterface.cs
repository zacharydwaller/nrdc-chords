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
        public static Container<Chords.MeasurementList> GetMeasurements(int siteID, int streamIndex, int hoursBack = 24)
        {
            var dataStreamContainer = GetDataStream(siteID, streamIndex);
            var container = new Container<Chords.MeasurementList>();

            if (dataStreamContainer.Success)
            {
                var stream = dataStreamContainer.Object;

                // Create stream request HTTP message
                string startTime = DateTime.UtcNow.AddHours(-hoursBack).ToString("s");
                string endTime = DateTime.UtcNow.ToString("s");

                var dataSpecification = new Data.DataSpecification(stream, startTime, endTime);

                var jsonContent = Json.Serialize(dataSpecification);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Create HTTP POST
                string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/download";
                var response = ChordsInterface.Http.PostAsync(uri, stringContent).Result;

                // Check HTTP response
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;

                    var dataDownloadResponse = Json.Parse<Data.DataDownloadResponse>(content);

                    // Check data download response
                    if (dataDownloadResponse.Success)
                    {
                        // Check data download
                        if (dataDownloadResponse.Data.TotalNumberOfMeasurements > 0)
                        {
                            var chordsList = new Chords.MeasurementList();

                            foreach(var nrdcMeasurement in dataDownloadResponse.Data.Measurements)
                            {
                                var chordsMeasurement = Converter.Convert(nrdcMeasurement);
                                chordsList.Data.Add(chordsMeasurement);
                            }

                            return container.Pass(chordsList);
                        }
                        else
                        {
                            return container.Fail("No measurements found");
                        }
                    }
                    else
                    {
                        // Data download failed
                        return container.Fail(dataDownloadResponse.Message);
                    }
                }
                else
                {
                    // HTTP didn't return OK
                    return container.Fail("Error From: " + response.RequestMessage + "\n" + response.ReasonPhrase);
                }
            }
            else
            {
                // GetDataStream failed, return with reason message
                return container.Fail(dataStreamContainer.Message);
            }
        }

        public static Container<Chords.SiteList> GetSiteList()
        {
            string uri = ChordsInterface.InfrastructureServiceUrl + "infrastructure/sites";

            var result = ChordsInterface.Http.GetAsync(uri).Result;
            string message = result.Content.ReadAsStringAsync().Result;

            var sitelist = Json.Parse<Infrastructure.SiteList>(message);
            var chordsList = Converter.Convert(sitelist);

            return new Container<Chords.SiteList>(chordsList);
        }

        public static Container<Chords.Site> GetSite(int siteID)
        {
            var siteListContainer = GetSiteList();
            var container = new Container<Chords.Site>();

            if (siteListContainer.Success)
            {
                var sitelist = siteListContainer.Object;
                var site = sitelist.Data.FirstOrDefault(s => s.ID == siteID);

                if (site != null)
                {
                    return container.Pass(site);
                }
                else
                {
                    return container.Fail("Site not found. ID: " + siteID.ToString());
                }
            }

            return container.Fail(siteListContainer.Message);
        }

        public static Container<Chords.SystemList> GetSystemList(int siteID)
        {
            string uri = ChordsInterface.InfrastructureServiceUrl + "infrastructure/site/" + siteID.ToString() + "/systems";

            var result = ChordsInterface.Http.GetAsync(uri).Result;
            string message = result.Content.ReadAsStringAsync().Result;

            var systemList = Json.Parse<Infrastructure.SystemList>(message);

            var container = new Container<Chords.SystemList>();

            if (systemList.Success)
            {
                var chordsList = Converter.Convert(systemList);
                return container.Pass(chordsList);
            }
            else
            {
                return container.Fail("Could not get System List: Site ID: " + siteID.ToString());
            }
        }
        
        public static Container<Chords.InstrumentList> GetInstrumentList(int systemID)
        {
            var uri = ChordsInterface.InfrastructureServiceUrl + "infrastructure/system/" + systemID.ToString() + "/deployments";
            var message = GetHttpContent(uri);

            var deploymentList = Json.Parse<Infrastructure.DeploymentList>(message);

            var container = new Container<Chords.InstrumentList>();

            if(deploymentList.Success)
            {
                var chordsList = Converter.Convert(deploymentList);
                return container.Pass(chordsList);
            }
            else
            {
                return container.Fail("Could not get Instrument List: System ID: " + systemID.ToString());
            }
        }

        /* Private Methods */

        private static string GetHttpContent(string uri)
        {   
            var response = ChordsInterface.Http.GetAsync(uri).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        private static Container<Data.DataStreamList> GetDataStreams(int siteID)
        {
            var container = new Container<Data.DataStreamList>();

            if (siteID < 1)
            {
                return container.Fail("Site ID out of range");
            }

            // Get data streams
            string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/streams/site/" + siteID.ToString();
            var result = ChordsInterface.Http.GetAsync(uri).Result;
            string message = result.Content.ReadAsStringAsync().Result;

            var streamlist = Json.Parse<Data.DataStreamList>(message);

            // Check stream list
            if (streamlist.Success)
            {
                if (streamlist.Data.Count > 0)
                {
                    return container.Pass(streamlist);
                }
                else
                {
                    // No streams found
                    return container.Fail("No data streams found with SiteId: " + siteID.ToString());
                }
            }
            else
            {
                // Stream list retrieval failed, return with reason message
                return container.Fail(streamlist.Message);
            }
        }

        // Returns Container where Object is Data.DataStream
        private static Container<Data.DataStream> GetDataStream(int siteID, int streamIndex)
        {
            var streamListContainer = GetDataStreams(siteID);
            var container = new Container<Data.DataStream>();

            // Get container
            if (streamListContainer.Success)
            {
                var streamlist = streamListContainer.Object;

                if (streamIndex >= 0 && streamIndex < streamlist.Data.Count)
                {
                    return container.Pass(streamlist.Data[streamIndex]);
                }
                else
                {
                    // Stream index too high for stream count
                    return container.Fail("Stream index out of range");
                }
            }
            else
            {
                // GetDataStreams failed, return with reason message
                return container.Fail(streamListContainer.Message);
            }
        }
    }
}
