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
        Api.Container<Chords.SiteList> GetSiteList();

        [OperationContract]
        Api.Container<Chords.Site> GetSite(int siteID);

        [OperationContract]
        Api.Container<Chords.SystemList> GetSystemList(int siteID);

        [OperationContract]
        Api.Container<Chords.InstrumentList> GetInstrumentList(int systemID);

        [OperationContract]
        Api.Container<Data.DataStreamList> GetDataStreamList(int deploymentID);

        [OperationContract]
        Api.Container<Data.DataStream> GetDataStream(int streamID, int deploymentID = -1);

        [OperationContract]
        string GetMeasurements(Data.DataStream stream, DateTime startTime, DateTime endTime = default(DateTime));

        [OperationContract]
        string CreateMeasurement(Chords.Measurement measurement);
    }
}
