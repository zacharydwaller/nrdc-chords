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

    public class Interval :
}
