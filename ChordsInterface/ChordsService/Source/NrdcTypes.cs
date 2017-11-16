using System;
using System.Collections.Generic;
using System.Text;

namespace Nrdc
{
    public class SystemList
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<System> Data { get; set; }
    }

    public class System
    {
        public int ID;
        public string UniqueIdentifier { get; set; }
        public int Manager;
        public int Site;
        public string Name { get; set; }
        public string Details { get; set; }
        public string Power { get; set; }
        public string InstallationLocation { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
    }
    public class SiteList
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<Site> Data { get; set; }
    }
    public class Site
    {
        public int ID;
        public string UniqueIdentifier { get; set; }
        public int Network;
        public int LandOwner;
        public int PermitHolder;
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Notes { get; set; }
        public double Latitude;
        public double Longitude;
        public double Elevation;
        public string TimeZoneName { get; set; }
        public string TimeZoneAbbreviation { get; set; }
        public int TimeZoneOffset;
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
        public string GPSLandmark { get; set; }
        public SystemList SystemList { get; set; }
    }
}
