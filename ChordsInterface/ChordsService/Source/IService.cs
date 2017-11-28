using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsInterface.Service
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetSiteList();

        [OperationContract]
        string GetSite(int siteID);

        [OperationContract]
        string GetMeasurements(int siteID, int streamIndex, int hoursBack);

        [OperationContract]
        string CreateMeasurement(Chords.Measurement measurement);
    }
}
