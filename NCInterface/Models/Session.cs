using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCInterface.Structures
{
    public class Session
    {
        public string SessionKey { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string NetworkAlias { get; private set; }
        public List<int> StreamIDs { get; private set; }
        public int InstrumentID { get; private set; }
        public DateTime LastMeasTime { get; private set; }
        public DateTime LastRefresh { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool Realtime { get; private set; }

        public Session(string sessionKey, string networkAlias, List<int> streamIDs, DateTime startTime, bool realtime = false, string name = "", string description = "")
        {
            SessionKey = sessionKey;
            NetworkAlias = networkAlias;
            StreamIDs = new List<int>(streamIDs.ToArray());
            LastMeasTime = startTime;
            Realtime = realtime;
            Name = name;
            Description = description;
        }

        public Session(string sessionKey, SessionInitializer initializer)
        {
            // If initializer name is null or empty string, set name to session key
            if(initializer.Name == null || initializer.Name == "")
            {
                Name = sessionKey;
            }
            else
            {
                Name = initializer.Name;
            }

            SessionKey = sessionKey;
            Description = initializer.Description;
            NetworkAlias = initializer.NetAlias;
            StreamIDs = new List<int>(initializer.StreamIDs);
            LastMeasTime = DateTime.Parse(initializer.StartTime);
            EndTime = DateTime.Parse(initializer.EndTime);
            Realtime = initializer.Realtime;
        }

        public void SetInstrument(int id)
        {
            InstrumentID = id;
        }

        /// <summary>
        /// Refreshes the session. Sets LastRefresh to current time and LastMeasTime to the endTime provided.
        /// </summary>
        /// <param name="endTime"></param>
        public void Refresh(DateTime newLastMeasTime)
        {
            LastRefresh = DateTime.UtcNow;
            LastMeasTime = newLastMeasTime;
        }
    }
}