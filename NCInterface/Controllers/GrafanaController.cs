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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;

namespace NCInterface.Controllers
{ 
    //Creates API to call GrafanaController functions via HTTP
    [RoutePrefix("Grafana")]
    public class GrafanaController : ApiController
    {

        //Sets timeout time for HttpClient object
        private static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
        }; 
        //Sets API key and header to be able to make changes to Grafana
        private static string header = "Bearer";
        private static string credentials ="eyJrIjoicmtVcVA4MjN6dTVKWFNRWUliMUJmVTdVUlJKdWpOclEiLCJuIjoidGVzdDEiLCJpZCI6MX0=";

        /// <summary>
        /// Sets the user credentials for a new admin and posts request to Grafana
        /// </summary>
        /// <param name="adminName"></param>
        /// <returns>A string containing the HTTP response to the post</returns>
        [Route("CreateAdmin")]
        [HttpGet]
        public string CreateAdmin(string adminName)
        {
            var uri = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com:3000/api/auth/keys";
            User testUser = new User { name = adminName, role = "Admin" };
            //Serializes user information into JSON
            var jsonContent = JsonConvert.SerializeObject(testUser, Config.DefaultSerializationSettings);
            //Encodes string for JSON
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");            
            //Sets client authorization credentials
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header,credentials);
            //Gets and returns response from post
            var response = client.PostAsync(uri, stringContent).Result;
            return response.Content.ReadAsStringAsync().Result;

        }

        /*Creating Dashboards
         * 
         * A Dashboard will have:
         * Rows: A row can contain multiple item to display information (ex: one row can have a table right next to a graph)
         * and a Dashboard Name
         * 
         * Each items in a row are called a "panel" which can be multiple things that display information like graphs, tables, etc..
         * To create a panels, we need to initalize them and then send that information to a 'Row' object
         * A dashboard will need to be initalized with its name and a list of rows.
         * 
         */
        /// <summary>
        /// Initiates and formats JSON Dashboard object to send data to Grafana
        /// </summary>
        /// <param name="dashName"></param>
        /// <returns>A string containing the HTTP response to the POST</returns>
        [Route("CreateDashboard")]
        [HttpGet]
        public string CreateDashboard(string dashName, string sessionKey)

        {
            //URI to contact Grafana's API for interacting with Dashboards
            var uri = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com:3000/api/dashboards/db";
            //Creating lists to be use to initialize the Dashboard constructor
            var reqs = new List<Require>();
            var DashRows = new List<Row>();
            var panelsList = new List<Panel>();
            var Yaxes = new List<Yax>();
            var Yaxis = new Yax { format = "short", label = null, logBase = 1, show = true };
            var Yaxis1 = new Yax { format = "short", label = null, logBase = 1, show = true };
            var timeRange = new Time { from = "now-1M/M", to = "now-1M/M" };
            Yaxes.Add(Yaxis);
            Yaxes.Add(Yaxis1);
            //Initializing the Dashboard's "requirement" objects and target objects for Panel
            var panelReqs = new Require { type = "panel", id = "graph", name = "graph" };
            var dataReqs = new Require { type = "datasource", id = "influxdb", name = "InfluxDB", version = "1.0.0" };
           // ChordsBot.GetTarget(sessionKey);
            //Panel initialization
            var panel = new Panel { title = "test",
                //  aliasColors = { },
                bars = false,
                datasource = "NRDC",
                fill = 1,
                nullPointMode = "null",
                lines = true,
                // links = { },
                linewidth = 1,
                pointradius = 5,
                renderer = "flot",
                //seriesOverrides = { },
                span = 12,
                stack = false,
                steppedLine = false,
                tooltip = new Tooltip { shared = true, sort = 0, value_type = "individual" },
                type = "graph",
                xaxis = new Xaxis { mode = "time ", name = null, show = true },
                yaxes = Yaxes,
                legend = new Legend { avg = false, current = false, max = false, min = false, show = true, total = false, values = false }
            };
            //formats data and sends JSON request
            reqs.Add(panelReqs);
            reqs.Add(dataReqs);
            panelsList.Add(panel);
            var dashRow = new Row { panels = panelsList, title = "Dashboard Row", titleSize = "h6", height = "250px"};
            DashRows.Add(dashRow);
            Response testBoard = new Response { dashboard = new Dashboard {title = dashName, rows = DashRows, time = timeRange , version = 3}} ;
            var jsonContent = JsonConvert.SerializeObject(testBoard, Config.DefaultSerializationSettings);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(header, credentials);
            var response = client.PostAsync(uri, stringContent).Result;
            return response.Content.ReadAsStringAsync().Result;

            

        }
    }
}
