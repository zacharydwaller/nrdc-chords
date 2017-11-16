using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsInterface.Chords
{
    [DataContract]
    public class Site
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int ID { get; set; }
        [DataMember] public double Latitude { get; set; }
        [DataMember] public double Longitude { get; set; }
        [DataMember] public double Elevation { get; set; }
        [DataMember] public string Description { get; set; }
    }

    [DataContract]
    public class Instrument
    {
        [DataMember] public bool Status { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int ID { get; set; }
        // Interval in seconds
        [DataMember] public long Interval { get; set; }
        [DataMember] public DateTime LastMeasurement { get; set; }
    }

    [DataContract]
    public class Measurement
    {
        [DataMember] public uint Instrument { get; set; }
        [DataMember] public int Value { get; set; }
    }
}
