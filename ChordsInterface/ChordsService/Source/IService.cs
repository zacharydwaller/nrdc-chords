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
        Api.Container<Chords.SiteList> GetSiteList();

        [OperationContract]
        Api.Container<Chords.Site> GetSite(int siteID);

        [OperationContract]
        Api.Container<Chords.SystemList> GetSystemList(int siteID);

        [OperationContract]
        Api.Container<Chords.InstrumentList> GetInstrumentList(int systemID);

        [OperationContract]
        Api.Container<Data.DataStreamList> GetDataStreams(int deploymentID);

        [OperationContract]
        string GetMeasurements(Data.DataStream stream, DateTime startTime, DateTime endTime);

        [OperationContract]
        string CreateMeasurement(Chords.Measurement measurement);
    }
}
