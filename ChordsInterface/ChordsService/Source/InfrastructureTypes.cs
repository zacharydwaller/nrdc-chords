using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChordsInterface.Nrdc;

namespace ChordsInterface.Infrastructure
{
    public class SiteList : NrdcType
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<Site> Data { get; set; }
    }

    public class SystemList : NrdcType
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<System> Data { get; set; }
    }

    public class DeploymentList : NrdcType
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<Deployment> Data { get; set; }
    }

    public class Site : NrdcType
    {
        public string __type { get; set; }
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
        public int TimeZoneOffset { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
        public string GPSLandmark { get; set; }
    }

    public class System : NrdcType
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

    public class Deployment : NrdcType
    {
        public int ID { get; set; }
        public string UniqueIdentifier { get; set; }
        public int System { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public string CenterOffset { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }
        public double HeightFromGround { get; set; }
        public string ParentLogger { get; set; }
        public string Notes { get; set; }
        public string EstablishedDate { get; set; }
        public string AbandonedDate { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
    }
}
