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
        public static string ChordsHostUrl { get { return chordsHostUrl; } }
        public static string InfrastructureServiceUrl { get { return infrastructureServiceUrl; } }
        public static string DataServiceUrl { get { return dataServiceUrl; } }
        public static string NevCanAlias { get { return nevCanAlias; } }

        public static string DefaultTimeZoneID { get { return "Pacific Standard Time";  } }

        private static string chordsHostUrl = "	http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com";
        private static string infrastructureServiceUrl = "http://sensor.nevada.edu/Services/GIDMIS/Infrastructure/NRDC.Services.Infrastructure.InfrastructureService.svc/NevCAN/";
        private static string dataServiceUrl = "http://sensor.nevada.edu/Services/GIDMIS/Data/NRDC.Services.Data.DataService.svc/";
        private static string nevCanAlias = "NevCAN/";

        // In milliseconds
        public static double DefaultTimeout = 500;

        public static int MaxMeasurements = 1000;

        public static HttpClient Http = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(DefaultTimeout)
        };

        public static string GetCurrentTimestamp()
        {
            // The ToString() arg formats the date as ISO-8601
            return DateTime.Now.ToString("o");
        }
    }
}
