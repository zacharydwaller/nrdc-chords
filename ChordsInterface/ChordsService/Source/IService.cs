using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChordsInterface.Service
{
    /// <summary>
    ///     NRDC-CHORDS Web Service interface.
    /// </summary>
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Api.Container<Infrastructure.NetworkList> GetNetworkList();

        [OperationContract]
        Api.Container<Chords.SiteList> GetSiteList(string networkAlias);

        [OperationContract]
        Api.Container<Chords.Site> GetSite(string networkAlias, int siteID);

        [OperationContract]
        Api.Container<Chords.SystemList> GetSystemList(string networkAlias, int siteID);

        [OperationContract]
        Api.Container<Chords.InstrumentList> GetInstrumentList(string networkAlias, int systemID);

        [OperationContract]
        Api.Container<Data.DataStreamList> GetDataStreamList(string networkAlias, int deploymentID);

        [OperationContract]
        Api.Container<Data.DataStream> GetDataStream(string networkAlias, int streamID, int deploymentID = -1);

        [OperationContract]
        string GetMeasurements(Data.DataStream stream, DateTime startTime, DateTime endTime = default(DateTime));

        [OperationContract]
        string CreateMeasurement(Chords.Measurement measurement);
    }
}
