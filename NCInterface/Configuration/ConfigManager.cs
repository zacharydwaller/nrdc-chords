using System;
using System.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
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

        /// <summary>
        /// Default Json.NET Serialization/Deserialization settings
        /// </summary>
        public static JsonSerializerSettings DefaultSerializationSettings;
        public static JsonSerializerSettings DefaultDeserializationSettings;

        static Config()
        {
            // Read web.config
            Chords chordsSection = ConfigurationManager.GetSection("chords") as Chords;
            DataCenter dcSection = ConfigurationManager.GetSection("dataCenter") as DataCenter;

            if (chordsSection != null)
            {
                ChordsHostUrl = chordsSection.HostUrl;
            }

            if (dcSection != null)
            {
                NetworkDiscoveryUrl = dcSection.NetworkDiscoveryUrl;
            }

            // Create Url dictionaries
            InfrastructureUrlDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            DataUrlDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            // Set default Json.NET settings
            DefaultSerializationSettings = new JsonSerializerSettings()
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };

            DefaultDeserializationSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }
    }
}