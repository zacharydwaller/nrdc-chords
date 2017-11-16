using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace ChordsInterface.Api
{ 
    public static class ApiInterface
    {
        public static async Task<Nrdc.SiteList> GetSitesAsync()
        {
            HttpClient httpObject = new HttpClient();
            string networkAlias = ChordsInterface.DataCenterUrl;

            var result = await httpObject.GetAsync(networkAlias + "/infrastructure/sites");
            string message = await result.Content.ReadAsStringAsync();

            Nrdc.SiteList sitelist = Json.Parse<Nrdc.SiteList>(message);

            /** Convert sitelist to list of ChordsSite here**/

            return sitelist;
        }

        public static async Task<Nrdc.Site> GetSiteAsync(int siteID)
        {
            var sitelist = await GetSitesAsync();

            if (sitelist.Success)
            {
                foreach (var site in sitelist.Data)
                {
                    if(site.ID == siteID)
                    {
                        return site;
                    }
                }
            }

            return null;
        }

        public static async void GetDataAsync()
        {
            HttpClient httpObject = new HttpClient();
            string networkAlias = ChordsInterface.DataCenterUrl;

            var result = await httpObject.GetAsync(networkAlias + "/infrastructure/sites"); 
            string message = await result.Content.ReadAsStringAsync();

            Nrdc.SiteList sites = Json.Parse<Nrdc.SiteList>(message);

            Console.Out.WriteLine(sites.Success);

            foreach (Nrdc.Site varSite in sites.Data)
            {
                Console.Out.WriteLine(varSite.ID + " " + varSite.Name + " " + varSite.Network + " " + varSite.TimeZoneName);
                var result2 = await httpObject.GetAsync(networkAlias + "/infrastructure/site/" + varSite.ID + "/systems");
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
