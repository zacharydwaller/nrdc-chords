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

    public class SystemList : NrdcType
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
        public Decimal Value { get; set; }
    }

    public class AggregateMeasurement : NrdcType
    {
        public int Stream { get; set; }
        public string TimeStamp { get; set; }
        public Decimal Value { get; set; }
        public long NumberOfConstituentValues { get; set; }
        public short AggregateType { get; set; }
    }

    public class Deployment : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Category : NrdcType
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

    public class DataType : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Interval : NrdcType
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

    public class DataStreamList : NrdcType
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<DataStream> Data { get; set; }
    }

    public class DataStream : NrdcType
    {
        public int ID { get; set; }
        public Site Site { get; set; }
        public System System { get; set; }
        public Deployment Deployment { get; set; }
        public Category Category { get; set; }
        public Property Property { get; set; }
        public Unit Units { get; set; }
        public DataType DataType { get; set; }
        public String MeasurementInterval { get; set; }
    }

    public class DataStreamRequest : NrdcType
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

    public class DataSpecification : NrdcType
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

    public class AggregateDataSpecification : NrdcType
    {
        public Interval AggregateInterval { get; set; }
        public string TimeZoneID { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public long Skip { get; set; }
        public long Take { get; set; }
        public IList<DataStreamRequest> DataStreams { get; set; }
    }

    public class DataDownloadResponse : NrdcType
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DataDownload Data { get; set; }
    }

    public class DataDownload : NrdcType
    {
        public long TotalNumberOfMeasurements { get; set; }
        public long StartIndex { get; set; }
        public long EndIndex { get; set; }
        public IList<Measurement> Measurements { get; set; }
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
