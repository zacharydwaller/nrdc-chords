using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChordsInterface.Api
{
    /// <summary>
    ///     A simple wrapper for Newtonsoft's Json.Net. Simply builds in serialization settings.
    /// </summary>
    public class Json
    {
        /// <summary>
        ///     Parses a given Json string. Uses Json.Net converter with Null Value and Missing Member handling set to Ignore.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Parse<T> (string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        ///     Serializes a given object to a Json string. Uses Json.Net converter with String Escape handling set to EscapeNonAscii.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize (object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}