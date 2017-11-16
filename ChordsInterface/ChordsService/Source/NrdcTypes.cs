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
    public class Measurements : NrdcType
    {

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
