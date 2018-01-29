using System.Configuration;

namespace NCInterface.Configuration
{
    /// <summary>
    /// Data Center configuration section. Provides the Data Center's Network Discovery URL
    /// </summary>
    public class DataCenter : ConfigurationSection
    {
        /// <summary>
        /// Get the Network Discovery Url
        /// </summary>
        [ConfigurationProperty("networkDiscoveryUrl", IsRequired = true)]
        public string NetworkDiscoveryUrl
        {
            get { return base["networkDiscoveryUrl"] as string; }
        }
    }
}