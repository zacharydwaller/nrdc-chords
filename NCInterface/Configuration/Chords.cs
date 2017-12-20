using System.Configuration;

namespace NCInterface.Configuration
{
    /// <summary>
    /// CHORDS configuration section. Provides the CHORDS Host URL.
    /// </summary>
    public class Chords : ConfigurationSection
    {
        /// <summary>
        /// Get the CHORDS Host Url.
        /// </summary>
        [ConfigurationProperty("hostUrl", IsRequired = true)]
        public string HostUrl
        {
            get { return base["hostUrl"] as string; }
        }
    }
}