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
            Console.Out.WriteLine("Contructor");
        }

        public async void GetDataAsync()
        {
            var result = await httpObject.GetAsync(NetworkAlias + "/infrastructure/sites");
            string message = await result.Content.ReadAsStringAsync();

            Console.Out.WriteLine(message);
        }
    }
}
