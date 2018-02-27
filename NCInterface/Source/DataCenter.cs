using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;

namespace NCInterface
{
    public static class DataCenter
    {
        /// <summary>
        /// Dictionary of sensor network infrastructure urls.
        /// Key: Network Alias.
        /// </summary>
        public static Dictionary<string, string> InfrastructureUrlDict { get; private set; }

        /// <summary>
        /// Dictionary of sensor network data urls.
        /// Key: Network Alias.
        /// </summary>
        public static Dictionary<string, string> DataUrlDict { get; private set; }

        private static HttpClient http = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        };

        /// <summary>
        /// Constructor, creates Url dictionaries
        /// </summary>
        static DataCenter()
        {
            // Create Url dictionaries
            InfrastructureUrlDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            DataUrlDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        ///     Gets all of the sensor networks in NRDC. Populates the Data and Infrastructure Url dictionaries.
        /// </summary>
        /// <returns></returns>
        public static Container<Network> GetNetworkList()
        {
            string uri = Config.NetworkDiscoveryUrl;
            string message = GetHttpContent(uri);

            var networkList = JsonConvert.DeserializeObject<Container<Network>>(message, Config.DefaultDeserializationSettings);

            if (networkList.Success)
            {
                // Populate dictionaries with network Urls
                DataUrlDict.Clear();
                InfrastructureUrlDict.Clear();
                foreach (var network in networkList.Data)
                {
                    network.DataUrl = network.DataUrl.Replace("sensor.nevada.edu", "134.197.38.160");
                    network.InfrastructureUrl = network.InfrastructureUrl.Replace("sensor.nevada.edu", "134.197.38.160");
                    DataUrlDict[network.Alias] = network.DataUrl;
                    InfrastructureUrlDict[network.Alias] = network.InfrastructureUrl;
                }

                return networkList;
            }
            else
            {
                return new Container<Network>("Unable to retrieve network list. Message from API: " + networkList.Message);
            }
        }

        /// <summary>
        /// Gets the metadata related to a sensor network
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        public static Container<Network> GetNetwork(string networkAlias)
        {
            var networkList = GetNetworkList();

            if(networkList.Success)
            {
                var network = networkList.Data.FirstOrDefault(n => n.Alias.Equals(networkAlias, StringComparison.InvariantCultureIgnoreCase));
                
                if(network != null)
                {
                    return new Container<Network>(network);
                }
                else
                {
                    return new Container<Network>("Network not found. Alias: " + networkAlias);
                }
            }
            return new Container<Network>(networkList.Message);
        }

        /// <summary>
        ///     Returns a list of all the sites in a given sensor network.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        public static Container<Site> GetSiteList(string networkAlias)
        {
            var urlContainer = GetInfrastructureUrl(networkAlias);

            // Couldn't find infrastructure Url
            if (!urlContainer.Success)
            {
                return new Container<Site>(urlContainer.Message);
            }

            string uri = urlContainer.Data[0] + "infrastructure/sites";
            string message = GetHttpContent(uri);

            var sitelist = JsonConvert.DeserializeObject<Container<Site>>(message, Config.DefaultDeserializationSettings);

            if (sitelist.Success)
            {
                return sitelist;
            }
            else
            {
                return new Container<Site>("Could not retrieve site list. Message from NRDC: " + sitelist.Message);
            }
        }

        /// <summary>
        ///     Retrieves the site metadata for a given site ID.
        /// </summary>
        /// <param name="sensorNetwork"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static Container<Site> GetSite(string sensorNetwork, int siteID)
        {
            var siteList = GetSiteList(sensorNetwork);

            if (siteList.Success)
            {
                var site = siteList.Data.FirstOrDefault(s => s.ID == siteID);

                if (site != null)
                {
                    return new Container<Site>(site);
                }
                else
                {
                    return new Container<Site>("Site not found. ID: " + siteID.ToString());
                }
            }

            return new Container<Site>(siteList.Message);
        }

        /// <summary>
        ///     Returns a list of systems that belong to a specified site.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static Container<NrdcSystem> GetSystemList(string networkAlias, int siteID)
        {
            var urlContainer = GetInfrastructureUrl(networkAlias);

            // Couldn't find infrastructure Url
            if (!urlContainer.Success)
            {
                return new Container<NrdcSystem>(urlContainer.Message);
            }

            string uri = urlContainer.Data[0] + "infrastructure/site/" + siteID.ToString() + "/systems";
            string message = GetHttpContent(uri);

            var systemList = JsonConvert.DeserializeObject<Container<NrdcSystem>>(message, Config.DefaultDeserializationSettings);

            if (systemList.Data != null && systemList.Data.Count != 0)
            {
                return systemList;
            }
            else
            {
                return new Container<NrdcSystem>("Could not retrieve system list from site ID: " + siteID.ToString());
            }
        }

        /// <summary>
        ///     Retrieves the system metadata for a given system ID.
        /// </summary>
        /// <param name="sensorNetwork"></param>
        /// <param name="siteID"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public static Container<NrdcSystem> GetSystem(string sensorNetwork, int siteID, int systemID)
        {
            var systemList = GetSystemList(sensorNetwork, siteID);

            if (systemList.Success)
            {
                var system = systemList.Data.FirstOrDefault(s => s.ID == systemID);

                if (system != null)
                {
                    return new Container<NrdcSystem>(system);
                }
                else
                {
                    return new Container<NrdcSystem>("System not found. Site " + siteID + ", System ID: " + systemID.ToString());
                }
            }

            return new Container<NrdcSystem> (systemList.Message);
        }

        /// <summary>
        ///     Returns a list of all deployments belonging to a system.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public static Container<Deployment> GetDeploymentList(string networkAlias, int systemID)
        {
            var urlContainer = GetInfrastructureUrl(networkAlias);

            // Couldn't find infrastructure Url
            if (!urlContainer.Success)
            {
                return new Container<Deployment>(urlContainer.Message);
            }

            var uri = urlContainer.Data[0] + "infrastructure/system/" + systemID.ToString() + "/deployments";
            var message = GetHttpContent(uri);

            var deploymentList = JsonConvert.DeserializeObject<Container<Deployment>>(message, Config.DefaultDeserializationSettings);

            if (deploymentList.Data != null && deploymentList.Data.Count != 0)
            {
                return deploymentList;
            }
            else
            {
                return new Container<Deployment>("Could not retrieve deployment list from system ID: " + systemID.ToString());
            }
        }

        /// <summary>
        ///     Gets a deployment's metadata given a system and deployment ID.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="systemID"></param>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        public static Container<Deployment> GetDeployment(string networkAlias, int systemID, int deploymentID)
        {
            var deploymentList = GetDeploymentList(networkAlias, systemID);

            if (deploymentList.Success)
            {
                var deployment = deploymentList.Data.FirstOrDefault(n => n.ID == deploymentID);

                if (deployment != null)
                {
                    return new Container<Deployment>(deployment);
                }
                else
                {
                    return new Container<Deployment>("Deployment not found. ID: " + deploymentID.ToString());
                }
            }
            return new Container<Deployment>(deploymentList.Message);
        }

        /// <summary>
        ///     Returns a list of all data streams belonging to a deployment.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="deploymentID"></param>
        /// <returns></returns>
        public static Container<Structures.Data.DataStream> GetDataStreams(string networkAlias, int deploymentID)
        {
            var urlContainer = GetDataUrl(networkAlias);

            // Couldn't find data services Url
            if (!urlContainer.Success)
            {
                return new Container<Structures.Data.DataStream>(urlContainer.Message);
            }

            // Get data streams
            string uri = urlContainer.Data[0] + "data/streams/deployment/" + deploymentID.ToString();
            string message = GetHttpContent(uri);

            var streamList = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(message, Config.DefaultDeserializationSettings);

            // Check stream list
            if (streamList.Success)
            {
                if (streamList.Data.Count > 0)
                {
                    return streamList;
                }
                else
                {
                    // No streams found
                    return new Container<Structures.Data.DataStream>("No data streams found with DeploymentId: " + deploymentID.ToString());
                }
            }
            else
            {
                // Stream list retrieval failed, return with reason message
                return new Container<Structures.Data.DataStream>(streamList.Message);
            }
        }

        /// <summary>
        ///     Gets a datastream by ID. Optionally provide its deployment ID for faster searching.
        ///     Will search all streams in network if stream is not found within provided deployment (slow).
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <param name="streamID"></param>
        /// <param name="deploymentID">
        ///     Optional. Leave empty to search in all deployments in network or specify to get a quicker search.
        /// </param>
        /// <returns></returns>
        public static Container<Structures.Data.DataStream> GetDataStream(string networkAlias, int streamID, int deploymentID = -1)
        {
            var urlContainer = GetDataUrl(networkAlias);

            // Couldn't find data services Url
            if (!urlContainer.Success)
            {
                return new Container<Structures.Data.DataStream>(urlContainer.Message);
            }

            string[] uri = new string[2];
            string message;

            uri[0] = urlContainer.Data[0] + "data/streams/deployment/" + deploymentID.ToString();
            uri[1] = urlContainer.Data[0] + "data/streams/all";

            // Check via deployment ID first, then via all streams
            for (int i = 0; i < 2; i++)
            {
                // Deployment ID not provided
                if (i == 0 && deploymentID <= 0) continue;

                // Check data stream list
                message = GetHttpContent(uri[i]);
                var streamList = JsonConvert.DeserializeObject<Container<Structures.Data.DataStream>>(message, Config.DefaultDeserializationSettings);
                var stream = streamList.Data.FirstOrDefault(s => s.ID == streamID);

                // Stream found
                if (stream != null)
                {
                    return new Container<Structures.Data.DataStream>(stream);
                }
            }

            // Stream not found
            string failMessage = "Data Stream not found. Stream ID: " + streamID;

            // Add deployment ID to fail message if it was provided
            if (deploymentID > 0)
            {
                failMessage = failMessage + " Deployment ID: " + deploymentID;
            }

            return new Container<Structures.Data.DataStream>(failMessage);
        }

        /// <summary>
        ///     Retrieves a list of measurements from the data stream. From startTime to endTime.
        /// </summary>
        /// <param name="stream">Data stream to retrieve measurements from. Get this from the GetDataStream function.</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>A list of measurements</returns>
        public static Container<Structures.Data.Measurement> GetMeasurements(string networkAlias, Structures.Data.DataStream stream, DateTime startTime, DateTime endTime)
        {
            // Create stream request HTTP message
            string startTimeString = startTime.ToUniversalTime().ToString("s");
            string endTimeString = endTime.ToUniversalTime().ToString("s");
            var dataSpecification = new Structures.Data.DataSpecification(stream, startTimeString, endTimeString);

            var jsonContent = JsonConvert.SerializeObject(dataSpecification, Config.DefaultSerializationSettings);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Create HTTP POST
            var urlContainer = GetDataUrl(networkAlias);

            if (!urlContainer.Success)
            {
                return new Container<Structures.Data.Measurement>(urlContainer.Message);
            }

            string uri = urlContainer.Data[0] + "data/download";

            var response = http.PostAsync(uri, stringContent).Result;

            // Check HTTP response
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;

                var dataDownloadResponse = JsonConvert.DeserializeObject<Structures.Data.DataDownloadResponse>(content, Config.DefaultDeserializationSettings);

                // Check data download response
                if (dataDownloadResponse.Success)
                {
                    // Check data download
                    if (dataDownloadResponse.Data.TotalNumberOfMeasurements > 0)
                    {
                        var measurementList = dataDownloadResponse.Data.Measurements;

                        return new Container<Structures.Data.Measurement>(measurementList);
                    }
                    else
                    {
                        // No measurements returned
                        return new Container<Structures.Data.Measurement>("No measurements found");
                    }
                }
                else
                {
                    // Data download failed
                    return new Container<Structures.Data.Measurement>("Data Download failed. Response from data center: " + dataDownloadResponse.Message);
                }
            }
            else
            {
                // HTTP didn't return OK
                return new Container<Structures.Data.Measurement>("Error From: " + response.RequestMessage + "\n" + response.ReasonPhrase);
            }
        }

        /// <summary>
        ///     Http.GetAsync wrapper. Makes a GET call to the uri and returns the response content as a string.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetHttpContent(string uri)
        {
            var response = http.GetAsync(uri).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        ///     Returns the Infrastructure Url for the specified sensor network. Returns empty string if no Url was found.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        private static Container<string> GetInfrastructureUrl(string networkAlias)
        {
            string infrastructureUrl;

            // Check for url in dictionary
            if (!InfrastructureUrlDict.TryGetValue(networkAlias, out infrastructureUrl))
            {
                // Couldn't find in dictionary, call GetNetworks to populate dictionaries and try again
                var networkList = GetNetworkList();

                if (networkList.Success)
                {
                    // Try the dictionary again
                    if (InfrastructureUrlDict.TryGetValue(networkAlias, out infrastructureUrl))
                    {
                        return new Container<string>(infrastructureUrl, true);
                    }
                }

                // Still couldn't find url, return empty string
                return new Container<string>("Couldn't find Infrastructure Services Url for Sensor Network: " + networkAlias);
            }

            // Url was in dictionary
            return new Container<string>(infrastructureUrl, true);
        }

        /// <summary>
        ///     Returns the Data Url for the specified sensor network. Returns empty string if no Url was found.
        /// </summary>
        /// <param name="networkAlias"></param>
        /// <returns></returns>
        private static Container<string> GetDataUrl(string networkAlias)
        {
            string dataUrl;

            // Check for url in dictionary
            if (!DataUrlDict.TryGetValue(networkAlias, out dataUrl))
            {
                // Couldn't find in dictionary, call GetNetworks to populate dictionaries and try again
                var networkList = GetNetworkList();

                if (networkList.Success)
                {
                    // Try the dictionary again
                    if (DataUrlDict.TryGetValue(networkAlias, out dataUrl))
                    {
                        return new Container<string>(dataUrl, true);
                    }
                }

                // Still couldn't find url, return empty string
                return new Container<string>("Couldn't find Data Services Url for Sensor Network: " + networkAlias);
            }

            // Url was in dictionary
            return new Container<string>(dataUrl, true);
        }
    }
}