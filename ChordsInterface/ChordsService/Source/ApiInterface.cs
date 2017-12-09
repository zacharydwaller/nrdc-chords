using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ChordsInterface.Api
{
    public static class ApiInterface
    {
        /// <summary>
        ///     Gets all of the sensor networks in NRDC. Populates the Data and Infrastructure Url dictionaries.
        /// </summary>
        /// <returns></returns>
        public static Container<Infrastructure.NetworkList> GetNetworkList()
        {
            var container = new Container<Infrastructure.NetworkList>();
            string uri = ChordsInterface.NetworkDiscoveryUrl;
            string message = GetHttpContent(uri);

            var networkList = Json.Parse<Infrastructure.NetworkList>(message);

            if(networkList.Success)
            {
                // Populate dictionaries with network Urls
                // Add or update each entry in dictionary
                foreach(var network in networkList.Data)
                {
                    ChordsInterface.DataUrlDict[network.Alias] = network.DataUrl;
                    ChordsInterface.InfrastructureUrlDict[network.Alias] = network.InfrastructureUrl;
                }

                return container.Pass(networkList);
            }
            else
            {
                return container.Fail("Unable to retrieve network list. Message from API: " + networkList.Message);
            }
        }

        /// <summary>
        ///     Returns a list of all the sites in a given sensor network.
        /// </summary>
        /// <returns></returns>
        public static Container<Chords.SiteList> GetSiteList(string networkAlias)
        {
            var container = new Container<Chords.SiteList>();
            string infrastructureUrl = GetInfrastructureUrl(networkAlias);

            // Couldn't find infrastructure Url
            if(infrastructureUrl == string.Empty)
            {
                return container.Fail("Unable to find Infrastructure Url for network alias: " + networkAlias);
            }

            string uri = infrastructureUrl + "infrastructure/sites";
            string message = GetHttpContent(uri);
            
            var sitelist = JsonConvert.DeserializeObject<Infrastructure.SiteList>(message);

            if(sitelist.Success)
            {
                var chordsList = Converter.Convert(sitelist);
                return new Container<Chords.SiteList>(chordsList);
            }
            else
            {
                return new Container<Chords.SiteList>(null, false, "Could not retrieve site list. Message from NRDC: " + sitelist.Message);
            }
        }

        /// <summary>
        ///     Retrieves the site metadata for a given site ID.
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static Container<Chords.Site> GetSite(int siteID)
        {
            var siteListContainer = GetSiteList("NevCAN");
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

        /// <summary>
        ///     Returns a list of systems that belong to a specified site.
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static Container<Chords.SystemList> GetSystemList(int siteID)
        {
            string uri = ChordsInterface.InfrastructureServiceUrl + ChordsInterface.NevCanAlias + "infrastructure/site/" + siteID.ToString() + "/systems";
            string message = GetHttpContent(uri);

            var systemList = Json.Parse<Infrastructure.SystemList>(message);

            var container = new Container<Chords.SystemList>();

            if (systemList.Success)
            {
                var chordsList = Converter.Convert(systemList);
                return container.Pass(chordsList);
            }
            else
            {
                return container.Fail("Could not retrieve system list. Message from NRDC: " + systemList.Message);
            }
        }
        
        /// <summary>
        ///     Returns a list of all deployments belonging to a system.
        /// </summary>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public static Container<Chords.InstrumentList> GetInstrumentList(int systemID)
        {
            var uri = ChordsInterface.InfrastructureServiceUrl + ChordsInterface.NevCanAlias + "infrastructure/system/" + systemID.ToString() + "/deployments";
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
                return container.Fail("Could not retrieve deployment list. Message from NRDC: " + deploymentList.Message);
            }
        }

        /// <summary>
        ///     Returns a list of all data streams belonging to a deployment.
        /// </summary>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        public static Container<Data.DataStreamList> GetDataStreams(int deploymentID)
        {
            var container = new Container<Data.DataStreamList>();

            // Get data streams
            string uri = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/streams/deployment/" + deploymentID.ToString();
            string message = GetHttpContent(uri);

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
                    return container.Fail("No data streams found with DeploymentId: " + deploymentID.ToString());
                }
            }
            else
            {
                // Stream list retrieval failed, return with reason message
                return container.Fail(streamlist.Message);
            }
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
        public static Container<Data.DataStream> GetDataStream(int streamID, int deploymentID = -1)
        {
            var container = new Container<Data.DataStream>();
            string[] uri = new string[2];
            string message;
            Data.DataStreamList streamList;
            Data.DataStream stream;

            uri[0] = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/streams/deployment/" + deploymentID.ToString();
            uri[1] = ChordsInterface.DataServiceUrl + ChordsInterface.NevCanAlias + "data/streams/all";

            for(int i = 0; i < 2; i++)
            {
                // Deployment ID not provided
                if (i == 0 && deploymentID <= 0) continue;

                // Check data stream list
                message = GetHttpContent(uri[i]);
                streamList = Json.Parse<Data.DataStreamList>(message);
                stream = streamList.Data.FirstOrDefault(s => s.ID == streamID);

                // Stream found
                if (stream != null)
                {
                    return container.Pass(stream);
                }
            }

            // Stream not found
            string failMessage = "Data Stream not found. Stream ID: " + streamID;
            
            if(deploymentID > 0)
            {
                failMessage = failMessage + " Deployment ID: " + deploymentID;
            }

            return container.Fail(failMessage);
        }

        /// <summary>
        ///     Retrieves a list of measurements from the data stream. From startTime to endTime.
        /// </summary>
        /// <param name="stream">Data stream to retrieve measurements from. Get this from the GetDataStream function.</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>A list of measurements</returns>
        public static Container<Chords.MeasurementList> GetMeasurements(Data.DataStream stream, DateTime startTime, DateTime endTime)
        {
            var container = new Container<Chords.MeasurementList>();
            // Create stream request HTTP message
            string startTimeString = startTime.ToString("s");
            string endTimeString = endTime.ToString("s");
            var dataSpecification = new Data.DataSpecification(stream, startTimeString, endTimeString);

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

                        foreach (var nrdcMeasurement in dataDownloadResponse.Data.Measurements)
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

        /* Private Methods */

        /// <summary>
        ///     Http.GetAsync wrapper. Makes a GET call to the uri and returns the response content as a string.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string GetHttpContent(string uri)
        {
            var response = ChordsInterface.Http.GetAsync(uri).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        ///     Returns the Infrastructure Url for the specified sensor network. Returns empty string if no Url was found.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        private static string GetInfrastructureUrl(string networkAlias)
        {
            string infrastructureUrl;

            // Check for url in dictionary
            if (!ChordsInterface.InfrastructureUrlDict.TryGetValue(networkAlias, out infrastructureUrl))
            {
                // Couldn't find in dictionary, call GetNetworks to populate dictionaries and try again
                var networkList = GetNetworkList();

                if (networkList.Success)
                {
                    // Try the dictionary again
                    if (ChordsInterface.InfrastructureUrlDict.TryGetValue(networkAlias, out infrastructureUrl))
                    {
                        return infrastructureUrl;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    // Still couldn't find url, return empty string
                    return string.Empty;
                }
            }

            return infrastructureUrl;
        }

        /// <summary>
        ///     Returns the Data Url for the specified sensor network. Returns empty string if no Url was found.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        private static string GetDataUrl(string networkAlias)
        {
            string dataUrl;

            // Check for url in dictionary
            if (!ChordsInterface.DataUrlDict.TryGetValue(networkAlias, out dataUrl))
            {
                // Couldn't find in dictionary, call GetNetworks to populate dictionaries and try again
                var networkList = GetNetworkList();

                if (networkList.Success)
                {
                    return ChordsInterface.DataUrlDict[networkAlias];
                }
                else
                {
                    // Still couldn't find url, return empty string
                    return string.Empty;
                }
            }

            return dataUrl;
        }
    }
}
