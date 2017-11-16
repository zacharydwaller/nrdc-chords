using System;
using System.Collections.Generic;
using System.Text;

namespace ChordsInterface.Nrdc
{
    public abstract class NrdcType { }

    public class Site : NrdcType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
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
    public class AggregateMeasurements : NrdcType
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
    (
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

    )
    */
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

}
