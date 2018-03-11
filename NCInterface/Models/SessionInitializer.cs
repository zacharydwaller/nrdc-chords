using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCInterface.Structures
{
    public class SessionInitializer
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string NetAlias { get; set; }
        public int[] StreamIDs { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool Realtime { get; set; } = false;

        public SessionInitializer()
        {

        }

        public SessionInitializer(string netAlias, int[] streamIDs, string startTime = null, string endTime = null, string name = "", string description = "")
        {
            NetAlias = netAlias;
            StreamIDs = streamIDs;
            StartTime = startTime;
            EndTime = endTime;
            Name = name;
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

            // Process End and Start times
            DateTime start, end;

            // Check if EndTime is provided
            if (EndTime != null)
            {
                // Try to parse
                 if(DateTime.TryParse(EndTime, out end))
                 {
                    // Set time of EndTime to 11:59:59 PM
                    // Gets the Date, which is at 0:00:00 hours
                    // Adds 1 day to set it to the next day at 0:00:00 hrs
                    // Minus 1 second to set it to original date at 23:59:59 hrs
                    end = end.Date.AddDays(1).AddSeconds(-1);
                }
                 else
                 {
                    // Couldn't parse
                    return new Container("EndTime could not be parsed.");
                 }

            }
            else
            {
                // if EndTime is empty, set it to Now and make session realtime
                end = DateTime.UtcNow;
                Realtime = true;
            }

            // Check if StartTime is provided
            if (StartTime != null)
            {
                // Try to parse
                if (DateTime.TryParse(StartTime, out start))
                {
                    // Set start's time to 0:00:00 hrs
                    start = start.Date;

                    // If StartTime > EndTime reject the request
                    if(start > end)
                    {
                        return new Container("StartTime cannot be greater than EndTime");
                    }
                }
                else
                {
                    // Could not parse
                    return new Container("StartTime could not be parsed.");
                }
            }
            else
            {
                // If Start is empty, set it to the end - 24 hours
                start = end.AddHours(-24);
            }

            // Finally set the date time strings
            StartTime = start.ToString("s");
            EndTime = end.ToString("s");

            // Check IDs
            if (StreamIDs.Length <= 0)
            {
                return new Container("Must select at least one data stream.");
            }

            foreach(int id in StreamIDs)
            {
                if (id <= 0)
                {
                    return new Container(String.Format("Stream ID {0} is invalid", id));
                }
            }

            return new Container();
        }
    }
}