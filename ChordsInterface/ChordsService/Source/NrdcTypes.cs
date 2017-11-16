using System;
using System.Collections.Generic;
using System.Text;

namespace ChordsInterface.Nrdc
{
    public class Site
    {
        public int ID { get; set; }
        public string UniqueIdentifier { get; set; }
        public int Network { get; set; }
        public int LandOwner { get; set; }
        public int PermitHolder { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Notes { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public int TimeZoneOffse { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
        public string GPSLandmark { get; set; }
    }

    public class System
    {
        public int ID { get; set; }
        public string UniqueIdentifier { get; set; }
        public int Manager { get; set; }
        public int Site { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Power { get; set; }
        public string InstallationLocation { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
    }
}
