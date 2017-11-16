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
        public static T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

namespace ChordsInterface.Nrdc
{
    public class SiteList
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<Site> Data { get; set; }
    }

    public class SystemList
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<System> Data { get; set; }
    }

    //What JSON returns
    public class DataList
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<System> Data { get; set; }
    }

    public class AggregatedDataDownload : NrdcType
    {
        public long TotalNumberOfMeasurements { get; set; }
        public long StartIndex { get; set; }
        public long EndIndex { get; set; }
        public IList<AggregateMeasurement> Measurements { get; set; }
    }
}
