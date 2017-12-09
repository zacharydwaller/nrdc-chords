using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ChordsInterface
{
    public static class ChordsInterface
    {
        // NRDC API information.
        public static string ChordsHostUrl { get { return chordsHostUrl; } }
        public static string NetworkDiscoveryUrl { get { return networkDiscoveryUrl; } }

        // Dictionary of sensor network infrastructure urls
        // Key: Network Alias; Value: Infrastructure Url
        public static Dictionary<string, string> InfrastructureUrlDict { get; private set; } = new Dictionary<string, string>();

        // Dictionary of sensor network data urls
        // Key: Network Alias; Value: Data Url
        public static Dictionary<string, string> DataUrlDict { get; private set; } = new Dictionary<string, string>();

        public static string InfrastructureServiceUrl { get { return infrastructureServiceUrl; } }
        public static string DataServiceUrl { get { return dataServiceUrl; } }

        public static string NevCanAlias { get { return nevCanAlias; } }
        public static string WalkerBasinAlias { get { return walkerBasinAlias; } }
        public static string SolarNexusAlias { get { return solarNexusAlias; } }

        public static string DefaultTimeZoneID { get { return "Pacific Standard Time";  } }

        private static string chordsHostUrl = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/";
        private static string networkDiscoveryUrl = "http://sensor.nevada.edu/services/GIDMIS/networks/nrdc.services.discovery.sitenetworks.svc/networks";

        private static string infrastructureServiceUrl = "http://sensor.nevada.edu/Services/GIDMIS/Infrastructure/NRDC.Services.Infrastructure.InfrastructureService.svc/";
        private static string dataServiceUrl = "http://sensor.nevada.edu/Services/GIDMIS/Data/NRDC.Services.Data.DataService.svc/";

        private static string nevCanAlias = "NevCAN/";
        private static string walkerBasinAlias = "WalkerBasin/";
        private static string solarNexusAlias = "SolarNexus/";

        // In milliseconds
        public static double DefaultTimeout = 5000;

        // NRDC download request can only return 1000 measurements at a time.
        public static int MaxMeasurements = 1000;

        // Static HttpClient instance. Use this for all HTTP calls.
        public static HttpClient Http = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(DefaultTimeout)
        };
    }
}
