using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;


namespace ApiInterface
{ 
    
    public class ApiInterface
    {
        HttpClient httpObject;

        public string NetworkAlias { get; private set; }

        public ApiInterface()
        {
            httpObject = new HttpClient();
            NetworkAlias = "http://sensor.nevada.edu/Services/GIDMIS/Infrastructure/NRDC.Services.Infrastructure.InfrastructureService.svc/NevCAN";
        }

        public async void GetDataAsync()
        {
            int sitecounter = 0;
            var result = await httpObject.GetAsync(NetworkAlias + "/infrastructure/sites"); 
            string message = await result.Content.ReadAsStringAsync();

            SiteList sites = JsonConvert.DeserializeObject<SiteList>(message);

            Console.Out.WriteLine(sites.Success);

            foreach (Site varSite in sites.Data)
            {
                Console.Out.WriteLine(varSite.ID + " " + varSite.Name + " " + varSite.Network + " " + varSite.TimeZoneName);
                var result2 = await httpObject.GetAsync(NetworkAlias + "/infrastructure/site/" + varSite.ID + "/systems");
                string message2 = await result2.Content.ReadAsStringAsync();

                varSite.SystemList = JsonConvert.DeserializeObject<SystemList>(message2); 
                foreach ( System varSystem in varSite.SystemList.Data)
                {
                    Console.Out.WriteLine("\t" + varSystem.ID + " " + varSystem.InstallationLocation);
                }
            }
            
        }
    } 
    
}
