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
        private static string sitesUrn = "infrastructure/sites";

        public static async Task<Nrdc.SiteList> GetSitesAsync()
        {
            string uri = ChordsInterface.DataCenterUrl + sitesUrn;

            var result = await ChordsInterface.Http.GetAsync(uri);
            string message = await result.Content.ReadAsStringAsync();

            var sitelist = Json.Parse<Nrdc.SiteList>(message);

            return sitelist;
        }

        public static async Task<Chords.Site> GetSiteAsync(int siteID)
        {
            var sitelist = await GetSitesAsync();

            if (sitelist.Success)
            {
                foreach (var site in sitelist.Data)
                {
                    if(site.ID == siteID)
                    {
                        return Converter.Convert(site);
                    }
                }
            }

            return null;
        }
    } 
    
}
