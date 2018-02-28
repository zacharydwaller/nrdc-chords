using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using NCInterface.Structures.Grafana;
using Newtonsoft.Json;

namespace NCInterface.Controllers
{
    [RoutePrefix("Grafana")]
    public class GrafanaController : ApiController
    {
        private static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        };

        [HttpGet]
        public string TestFunction()
        {
            string uri = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com:3000/api/dashboards/db" ;
            Dashboard testBoard = new Dashboard { title = "testboard" };
            var jsonContent = JsonConvert.SerializeObject(testBoard, Formatting.Indented);
            var stringContent = new StringContent(jsonContent,Encoding.UTF8 ,"application/json");

            var response = client.PostAsync(uri, stringContent).Result;
            
           if (response.IsSuccessStatusCode)
            {
                return ("success");
            }

            //return json;

            return ("test");

        }

    }
}
