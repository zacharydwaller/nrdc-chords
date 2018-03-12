using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCInterface.Structures;

namespace NCInterface.Structures
{ 
    //Gets new data from data stream from the time last streams up until the end time or current time
    public class StreamRefresher
    {
        public Session Session { get; private set; }
        public int StreamID { get; private set; }
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Assigns StreamRefresher values according to parameters
        /// </summary>
        /// <param name="session"></param>
        /// <param name="streamID"></param>
        /// <param name="endTime"></param>
        public StreamRefresher(Session session, int streamID, DateTime endTime)
        {
            Session = session;
            StreamID = streamID;
            EndTime = endTime;
        }

        /// <summary>
        /// Calls Data streaming function with assigned parameters
        /// </summary> 
        /// <param name=""></param>
        /// <returns>A string Container with a success or failure message</returns>
        public Container Refresh()
        {
            // Get stream object
            var streamContainer = DataCenter.GetDataStream(Session.NetworkAlias, StreamID);
            if (!streamContainer.Success) return new Container(streamContainer.Message);
            Data.DataStream stream = streamContainer.Data[0];
            // Stream from the last measured time up until the session's target end time
            DateTime start = Session.LastMeasTime;
            // Since NRDC DataDownload only returns 1000 measurements, several will have to be done
            // Keep looping until all data is streamed
            // start is equal to EndTime when stream is complete
            while (start != EndTime)
            {
                var container = StreamTimeRange(stream, start, EndTime, out start);
                if(container.Success == false)
                {
                    // Something failed
                    return container;
                }
            }
            return new Container();
        }

        /// <summary>
        /// Streams data just in a particular time range.
        /// Will set newStart and return success if it needs to stream again.
        /// Start will equal End and return success if it is finished.
        /// Will return an error container if something failed.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="newStart">The new starting time that should be used for the next stream</param>
        /// <returns> A string Container with a success or failure message</returns>
        private Container StreamTimeRange(Data.DataStream stream, DateTime start, DateTime end, out DateTime newStart)
        {
            // Get data download
            var dataContainer = DataCenter.GetMeasurements(Session.NetworkAlias, stream, start, end);
            // DataDownload failed, return error 
            if (!dataContainer.Success)
            {
                newStart = end;
                return new Container(dataContainer.Message);
            }
            // Push data
            var dataDownload = dataContainer.Data;
            var pushDataContainer = ChordsBot.PushMeasurementList(Session, dataDownload);
            if (dataDownload.Count < Config.MaxMeasurements)
            {
                // Less than MaxMeasurements downloaded, stream is done after push
                newStart = end;
                return new Container();
            }
            else
            {
                // MaxMeasurements downloaded, need more streaming
                string lastTimestamp = dataDownload[dataDownload.Count - 1].TimeStamp;
                newStart = DateTime.Parse(lastTimestamp);
                return new Container();
            }
        }
    }
}