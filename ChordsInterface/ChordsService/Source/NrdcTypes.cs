using System;
using System.Collections.Generic;
using System.Text;

namespace ChordsInterface.Nrdc
{
    public abstract class NrdcType { }

    public class SiteList : NrdcType
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<Site> Data { get; set; }
    }

    public class Site : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
    }

    public class SystemList
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<System> Data { get; set; }
    }

    public class System : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Measurement : NrdcType
    {
        public int Stream { get; set; }
        public string TimeStamp { get; set; }
        public Decimal value { get; set; }
    }

    //put actual data structure
    public class AggregateMeasurement : NrdcType
    {
        public int Stream { get; set; }
        public string TimeStamp { get; set; }
        public Decimal value { get; set; }
        public long NumberOfConstituentValues { get; set; }
        public short AggregateType { get; set; }
    }

    public class Deployment : NrdcType
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Property : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Unit : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Abbreviation { get; set; }
    }

    public class Datatype : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class AggregateDataSpecification : NrdcType
    {
        public Interval AggregateInterval { get; set; }
        public string TimeZoneID { get; set; }
        public string EndDateTime { get; set; }
        public long Skip { get; set; }
        public long Take { get; set; }
        public IList<DataStream> DataStreams;
    }

    public class AggregatedDataDownload : NrdcType
    {
        public long TotalNumberOfMeasurements { get; set; }
        public long StartIndex { get; set; }
        public long EndIndex { get; set; }
        public IList<AggregateMeasurement> Measurements { get; set; }
    }

    public class CsvJobStatus : NrdcType
    {
        public string JobID { get; set; }
        public string CurrentStatus { get; set; }
        public bool IsError { get; set; }
        public bool IsComplete { get; set; }
        public long WrittenNumberOfMeasurements { get; set; }
        public long TotalNumberOfMeasurements { get; set; }
        public string DataFileUrl { get; set; }
    }

}
