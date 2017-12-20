using System;
using System.Configuration;
using System.Collections.Generic;
using NCInterface.Configuration;

namespace NCInterface
{
    public static class Config
    {
        public static string ChordsHostUrl { get; private set; }
        public static string NetworkDiscoveryUrl { get; private set; }

        /// <summary>
        /// Dictionary of sensor network infrastructure urls.
        /// Key: Network Alias.
        /// </summary>
        public static Dictionary<string, string> InfrastructureUrlDict { get; private set; }

        /// <summary>
        /// Dictionary of sensor network data urls.
        /// Key: Network Alias.
        /// </summary>
        public static Dictionary<string, string> DataUrlDict { get; private set; }
            

        static Config()
        {
            Chords chordsSection = ConfigurationManager.GetSection("chords") as Chords;
            DataCenter dcSection = ConfigurationManager.GetSection("dataCenter") as DataCenter;

            if (chordsSection != null)
            {
                ChordsHostUrl = chordsSection.HostUrl;
            }

            if(dcSection != null)
            {
                NetworkDiscoveryUrl = dcSection.NetworkDiscoveryUrl;
            }

            InfrastructureUrlDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            DataUrlDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }
    }
}