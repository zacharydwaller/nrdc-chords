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
        public static string DataCenterUrl { get { return dataCenterUrl; } }

        private static string chordsHostUrl = "http://ec2-52-8-224-195.us-west-1.compute.amazonaws.com/";
        private static string dataCenterUrl = "http://sensor.nevada.edu/Services/GIDMIS/Infrastructure/NRDC.Services.Infrastructure.InfrastructureService.svc/NevCAN";

        public static HttpClient http = new HttpClient();
    }
}
