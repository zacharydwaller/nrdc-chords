using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using NCInterface.Structures.Grafana;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace NCInterface.Controllers
{
    [RoutePrefix("Grafana")]
    public class GrafanaController : ApiController
    {
        private static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        };
        private static string header = "Bearer";

        private static string credentials ="eyJrIjoicmtVcVA4MjN6dTVKWFNRWUliMUJmVTdVUlJKdWpOclEiLCJuIjoidGVzdDEiLCJpZCI6MX0=";



        [Route("CreateAdmin")]
        [HttpGet]
        public string CreateAdmin(string adminName)

        {

            var uri = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com:3000/api/auth/keys";
            
            User testUser = new User { name = adminName, role = "Admin" };

            var jsonContent = JsonConvert.SerializeObject(testUser, Config.DefaultSerializationSettings);

            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");            

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header,credentials);

            var response = client.PostAsync(uri, stringContent).Result;

            return response.Content.ReadAsStringAsync().Result;

        }


        [Route("CreateDashboard")]
        [HttpGet]
        public string CreateDashboard(string dashName)

        {

            var uri = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com:3000/api/dashboards/db";

    
            Response testBoard = new Response { dashboard = new Dashboard {title = "test" } } ;

            var jsonContent = JsonConvert.SerializeObject(testBoard, Config.DefaultSerializationSettings);

            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header, credentials);

            var response = client.PostAsync(uri, stringContent).Result;

            return response.Content.ReadAsStringAsync().Result;
            ///return jsonContent;

        }




    }
}
