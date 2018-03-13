using System.Collections.Generic;

/// <summary>
///     Contains a set of data structures used by the NRDC Infrastructure API.
/// </summary>
namespace NCInterface.Structures.Infrastructure
{ 
    //Stores Network data from NRDC API
    public class Network
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string InfrastructureUrl { get; set; }
        public string DataUrl { get; set; }
        public string ImageRetrievalUrl { get; set; }
        public string WebCamInteractionUrl { get; set; }
    }

    //Stores Site data from NRDC API
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
        public int TimeZoneOffset { get; set; }
        public string CreationDate { get; set; }
        public string ModificationDate { get; set; }
        public string GPSLandmark { get; set; }
    }

    //Stores System data from NRDC API
    public class NrdcSystem
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

    //Stores Deployment data from NRDC API
    public class Deployment
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