using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCInterface.Structures;

namespace NCInterface.Structures
{
    public class StreamRefresher
    {
        public Session Session { get; private set; }
        public int StreamID { get; private set; }
        public DateTime EndTime { get; private set; }

        public StreamRefresher(Session session, int streamID, DateTime endTime)
        {
            Session = session;
            StreamID = streamID;
            EndTime = endTime;
        }

        public void Refresh()
        {
            // Get stream object
            var streamContainer = DataCenter.GetDataStream(Session.NetworkAlias, StreamID);

            if (!streamContainer.Success) return;

            // Get data download
            var dataContainer = DataCenter.GetMeasurements(Session.NetworkAlias, streamContainer.Data[0], Session.LastMeasTime, EndTime);

            if (!dataContainer.Success) return;

            // Push data
            var dataDownload = dataContainer.Data;
            var pushDataContainer = ChordsBot.PushMeasurementList(Session, dataDownload);

            if(pushDataContainer.Success)
            {
                Session.Refresh(DateTime.Parse(dataDownload.Last().TimeStamp));
            }
        }
    }
}