using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsInterface.Chords
{
    [DataContract]
    public abstract class ChordsType { }

    [DataContract]
    public class SiteList : ChordsType
    {
        [DataMember] public List<Site> Data { get; set; } = new List<Site>();
    }

    [DataContract]
    public class SystemList : ChordsType
    {
        [DataMember] public List<System> Data { get; set; } = new List<System>();
    }

    [DataContract]
    public class InstrumentList : ChordsType
    {
        [DataMember] public List<Instrument> Data { get; set; } = new List<Instrument>();
    }

    [DataContract]
    public class MeasurementList : ChordsType
    {
        [DataMember] public List<Measurement> Data { get; set; } = new List<Measurement>();
    }

    [DataContract]
    public class Site : ChordsType
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int ID { get; set; }
        [DataMember] public double Latitude { get; set; }
        [DataMember] public double Longitude { get; set; }
        [DataMember] public double Elevation { get; set; }
        [DataMember] public string Description { get; set; }
    }

    [DataContract]
    public class System : ChordsType
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public int ID { get; set; }
    }

    // Maps to NRDC Deployment
    [DataContract]
    public class Instrument : ChordsType
    {
        [DataMember] public bool Status { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public int ID { get; set; }
        // Interval in seconds
        [DataMember] public long Interval { get; set; }
        [DataMember] public DateTime LastMeasurement { get; set; }
    }

    // Maps to NRDC DataStream
    [DataContract]
    public class Variable : ChordsType
    {
        [DataMember] public int ID { get; set; }
        [DataMember] public int InstrumentID { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Label { get; set; }
    }

    [DataContract]
    public class Measurement : ChordsType
    {
        [DataMember] public uint InstrumentID { get; set; }
        [DataMember] public string TimeStamp { get; set; }
        [DataMember] public decimal Value { get; set; }
    }
}
