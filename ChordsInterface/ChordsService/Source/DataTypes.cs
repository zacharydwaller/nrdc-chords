using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
///     Contains a set of data structures used by the NRDC Data API.
/// </summary>
namespace ChordsInterface.Data
{
    public class Site
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
    }

    public class NrdcSystem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Deployment
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Property
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Unit
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class DataType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Interval
    {
        public uint Size { get; set; }
	    public short Scales { get; set;}

        /*
            Scales:
			0 - Year,
			1 - Quarter,
			2 - Month,
			3 - DayOfYear,
			4 - Week,
			5 - Day,
			6 - Hour,
			7 - Minute,
			8 - Seconds,
			9 - Milliseconds,
			10 - Microseconds,
			11 - Nanoseconds
        */
    }

    [DataContract]
    public class DataStreamList
    {
        [DataMember] public bool Success { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public IList<DataStream> Data { get; set; }
    }

    [DataContract]
    public class DataStream
    {
        [DataMember] public int ID { get; set; }
        [DataMember] public Site Site { get; set; }
        [DataMember] public NrdcSystem System { get; set; }
        [DataMember] public Deployment Deployment { get; set; }
        [DataMember] public Category Category { get; set; }
        [DataMember] public Property Property { get; set; }
        [DataMember] public Unit Units { get; set; }
        [DataMember] public DataType DataType { get; set; }
        [DataMember] public String MeasurementInterval { get; set; }
    }

    public class DataStreamRequest
    {
        // Data stream you want
        public int DataStreamID { get; set; }
        // Units you want the data stream to be in
        public int UnitsID { get; set; }

        public DataStreamRequest() { }

        public DataStreamRequest(DataStream stream)
        {
            DataStreamID = stream.ID;
            UnitsID = stream.Units.ID;
        }
    }

    public class DataSpecification
    {
        public string TimeZoneID { get; set; } = ChordsInterface.DefaultTimeZoneID;
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public long Skip { get; set; } = 0;
        public long Take { get; set; } = ChordsInterface.MaxMeasurements;
        public IList<DataStreamRequest> DataStreams { get; set; } = new List<DataStreamRequest>();

        public DataSpecification() { }

        public DataSpecification(DataStream dataStream, string startTime, string endTime)
        {
            var streamRequest = new DataStreamRequest(dataStream);
            DataStreams.Add(streamRequest);
            StartDateTime = startTime;
            EndDateTime = endTime;
        }
    }

    public class AggregateDataSpecification
    {
        public Interval AggregateInterval { get; set; }
        public string TimeZoneID { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public long Skip { get; set; }
        public long Take { get; set; }
        public IList<DataStreamRequest> DataStreams { get; set; }
    }

    public class Measurement
    {
        public int Stream { get; set; }
        public string TimeStamp { get; set; }
        public Decimal Value { get; set; }
    }

    public class AggregateMeasurement
    {
        public int Stream { get; set; }
        public string TimeStamp { get; set; }
        public Decimal Value { get; set; }
        public long NumberOfConstituentValues { get; set; }
        public short AggregateType { get; set; }
    }

    public class DataDownloadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DataDownload Data { get; set; }
    }

    public class DataDownload
    {
        public long TotalNumberOfMeasurements { get; set; }
        public long StartIndex { get; set; }
        public long EndIndex { get; set; }
        public IList<Measurement> Measurements { get; set; }
    }

    public class AggregatedDataDownload
    {
        public long TotalNumberOfMeasurements { get; set; }
        public long StartIndex { get; set; }
        public long EndIndex { get; set; }
        public IList<AggregateMeasurement> Measurements { get; set; }
    }

    public class CsvJobStatus
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
