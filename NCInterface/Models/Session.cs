using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCInterface.Structures
{
    public class Session
    {
        public string SessionKey { get; private set; }
        public string NetworkAlias { get; private set; }
        public List<int> StreamIDs { get; private set; }
        public int InstrumentID { get; private set; }
        public DateTime LastMeasTime { get; private set; }
        public DateTime LastRefresh { get; private set; }

        public Session(string sessionKey, string networkAlias, List<int> streamIDs, DateTime startTime)
        {
            SessionKey = sessionKey;
            NetworkAlias = networkAlias;
            StreamIDs = new List<int>(streamIDs.ToArray());
            LastMeasTime = startTime;
        }

        public Session(string sessionKey, SessionInitializer initializer)
        {
            SessionKey = sessionKey;
            NetworkAlias = initializer.NetAlias;
            StreamIDs = new List<int>(initializer.StreamIDs);
            LastMeasTime = DateTime.Parse(initializer.StartTime);
        }

        public void SetInstrument(int id)
        {
            InstrumentID = id;
        }

        /// <summary>
        /// Refreshes the session. Sets LastRefresh to current time and LastMeasTime to the endTime provided.
        /// </summary>
        /// <param name="endTime"></param>
        public void Refresh(DateTime endTime)
        {
            LastRefresh = DateTime.Now;
            LastMeasTime = endTime;
        }
    }
}