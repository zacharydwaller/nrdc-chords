using System;
using System.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
using NCInterface.Configuration;

namespace NCInterface
{ 
    //Holds CHORDS URL and configuration settings, holds JSON settings
    public static class Config
    {
        public static string ChordsHostUrl { get; private set; }
        public static string NetworkDiscoveryUrl { get; private set; }
        public static string DefaultTimeZoneID { get { return "Pacific Standard Time"; } }
        public static int DefaultSampleRate { get; private set; } = 10;

        /// <summary>
        /// NRDC download request can only return 1000 measurements at a time.
        /// </summary>
        public static int MaxMeasurements { get; private set; } = 1000;

        /// <summary>
        /// Default Json.NET Serialization/Deserialization settings
        /// </summary>
        public static JsonSerializerSettings DefaultSerializationSettings;
        public static JsonSerializerSettings DefaultDeserializationSettings;

        /// <summary>
        /// HTTP Timeout
        /// </summary>
        public static int DefaultTimeout = 10000;

        //Gets CHORDS config settings, checks for NULL and sets values
        static Config()
        {
            // Read web.config
            Configuration.Chords chordsSection = ConfigurationManager.GetSection("chords") as Configuration.Chords;
            Configuration.DataCenter dcSection = ConfigurationManager.GetSection("dataCenter") as Configuration.DataCenter;
            if (chordsSection != null)
            {
                ChordsHostUrl = chordsSection.HostUrl;
            }
            if (dcSection != null)
            {
                NetworkDiscoveryUrl = dcSection.NetworkDiscoveryUrl;
            }
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