using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCInterface.Structures
{
    public class SessionInitializer
    {
        public string Description { get; set; }
        public string NetAlias { get; set; }
        public int[] StreamIDs { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Realtime { get; set; } = false;

        public SessionInitializer()
        {

        }

        public SessionInitializer(string netAlias, int[] streamIDs, string startTime = null, string endTime = null, string description = "")
        {
            NetAlias = netAlias;
            StreamIDs = streamIDs;
            StartTime = startTime;
            EndTime = endTime;
            Description = description;

            Validate();
        }

        /// <summary>
        /// Returns true if Session Initializer args are valid.
        /// </summary>
        /// <returns></returns>
        public Container Validate()
        {
            // Check nulls
            if (NetAlias == null)
            {
                return new Container("Network Alias is null.");
            }
            if (StreamIDs == null)
            {
                return new Container("StreamID list is null.");
            }

            // if EndTime is empty, set it to Now and set the Realtime flag
            if (EndTime == null)
            {
                EndTime = DateTime.UtcNow.ToString("s");
                Realtime = true;
            }

            // If StartTime is empty, set it to EndTime - 24 horus
            if (StartTime == null)
            {
                StartTime = DateTime.Parse(EndTime).AddHours(-24).ToString("s");
            }

            // Check IDs
            if (StreamIDs.Length <= 0)
            {
                return new Container("List of StreamIDs is empty.");
            }


            foreach(int id in StreamIDs)
            {
                if (id <= 0)
                {
                    return new Container(String.Format("Stream ID {0} is invalid", id));
                }
            }

            // Check if StartTime, EndTime can be parsed
            DateTime start, end;
            
            if(DateTime.TryParse(StartTime, out start) == false)
            {
                return new Container("StartTime could not be parsed.");
            }
            if(DateTime.TryParse(EndTime, out end) == false)
            {
                return new Container("EndTime could not be parsed.");
            }
            
            // Make sure StartTime < EndTime
            if (start >= end)
            {
                return new Container("EndTime is sooner than StartTime.");
            }

            return new Container();
        }
    }
}