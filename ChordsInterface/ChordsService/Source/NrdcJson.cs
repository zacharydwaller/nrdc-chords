using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChordsInterface.Api
{
    public class Json
    {
        public static T Parse<T> (string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize (object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}