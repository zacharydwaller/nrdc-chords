using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ChordsInterface.Api
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
            var result = await httpObject.GetAsync(NetworkAlias + "/infrastructure/sites"); 
            string message = await result.Content.ReadAsStringAsync();

            Nrdc.SiteList sites = Json.Parse<Nrdc.SiteList>(message);

            Console.Out.WriteLine(sites.Success);

            foreach (Nrdc.Site varSite in sites.Data)
            {
                Console.Out.WriteLine(varSite.ID + " " + varSite.Name + " " + varSite.Network + " " + varSite.TimeZoneName);
                var result2 = await httpObject.GetAsync(NetworkAlias + "/infrastructure/site/" + varSite.ID + "/systems");
                string message2 = await result2.Content.ReadAsStringAsync();

                var systemList = Json.Parse<Nrdc.SystemList>(message2);
                foreach (Nrdc.System varSystem in systemList.Data)
                {
                    Console.Out.WriteLine("\t" + varSystem.ID + " " + varSystem.InstallationLocation);
                }
            }
            
        }
    } 
    
}
