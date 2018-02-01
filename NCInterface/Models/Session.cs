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

        public Session(string sessionKey, string networkAlias, List<int> streamIDs, int instrumentID, DateTime startTime)
        {
            SessionKey = sessionKey;
            NetworkAlias = networkAlias;
            StreamIDs = new List<int>(streamIDs.ToArray());
            InstrumentID = instrumentID;
            LastMeasTime = startTime;
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