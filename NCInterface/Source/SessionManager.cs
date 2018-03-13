using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Text;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface
{ 
    //Contains functions to create and modify Session
    public static class SessionManager
    {
        public static Dictionary<string, Session> SessionDict { get; private set; }
        public static int KeyLength { get; } = 5; 
        //Initializes new SessionDict
        static SessionManager()
        {
            SessionDict = new Dictionary<string, Session>();
        }

        /// <summary>
        /// Initializes a new session and adds it to the dictionary. Does not begin streaming data.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>A string container with the SessionKey or a failure message</returns>
        public static Container<string> InitializeSession(SessionInitializer args)
        {
            var validation = args.Validate();
            if (validation.Success)
            {
                // Construct new session from args
                var session = new Session(GetRandomKey().Data[0], args);
                // Create instrument on CHORDS and set session's instrument ID
                var createInstContainer = ChordsBot.CreateInstrument(session.SessionKey);
                if (!createInstContainer.Success) return new Container<string>("", false, createInstContainer.Message);
                int id = createInstContainer.Data[0];
                session.SetInstrument(id);
                // Map session streams to CHORDS variables
                var confVarsContainer = ChordsBot.ConfigureVariables(session);
                if (!confVarsContainer.Success) return new Container<string>("", false, confVarsContainer.Message);
                // Add session to dict
                SessionDict.Add(session.SessionKey, session);
                return new Container<string>(session.SessionKey, true);
            }
            else
            {
                return new Container<string>("", false, validation.Message);
            }
        }

        /// <summary>
        /// Gets a Session by its key from the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A session Container with the Session designated by the SessionKey or a failure message</returns>
        public static Container<Session> GetSession(string key)
        {
            Session session;
            if(SessionDict.TryGetValue(key.ToUpper(), out session))
            {
                return new Container<Session>(session);
            }
            else
            {
                return new Container<Session>("Session key not found.");
            }
        }

        /// <summary>
        /// Returns all sessions as a list
        /// </summary>
        /// <returns>A Session Container with all Sessions in the list</returns>
        public static Container<Session> GetSessionList()
        {
            return new Container<Session>(SessionDict.Values.ToList<Session>());
        }

        /// <summary>
        /// Refreshes a session, streams all data from the last streamed time to the EndTime, or to Now if session is realtime.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A string Container with either a success or failure message</returns>
        public static Container RefreshSession(string key)
        {
            var sessionContainer = GetSession(key);
            if (sessionContainer.Success)
            {
                var session = sessionContainer.Data[0];
                // Get ending time
                DateTime end;
                if (session.Realtime)
                {
                    end = DateTime.UtcNow;
                }
                else
                {
                    // TODO: This check is always true for some reason
                    /*
                    if (session.LastMeasTime >= session.EndTime)
                    {
                        // Non-realtime stream already completed
                        return new Container();
                    }
                    */
                    end = session.EndTime;
                }
                // Stream data
                var threads = new List<Thread>();
                StringBuilder sb = new StringBuilder(); 
                //Creates a thread for each stream 
                foreach (int id in session.StreamIDs)
                {
                    var refreshObj = new StreamRefresher(session, id, end);
                    //var thread = new Thread(refreshObj.Refresh);
                    string message = "";
                    Thread thread = new Thread(() => { message = refreshObj.Refresh().Message; });
                    sb.Append(message);
                    threads.Add(thread);
                    thread.Start();
                }
                foreach(var thread in threads)
                {
                    thread.Join();
                }
                return new Container(true, sb.ToString());
            }
            else
            {
                return new Container(sessionContainer.Message);
            }
        }

        /// <summary>
        /// Generates a random unused session key
        /// </summary>
        /// <returns>String Container with generated session key</returns>
        public static Container<string> GetRandomKey()
        {
            Random rand = new Random();
            string key;
            do
            {
                key = "";
                for (int i = 0; i < SessionManager.KeyLength; i++)
                {
                    key += (char)rand.Next('A', 'Z');
                }
            } while (SessionDict.ContainsKey(key));

            return new Container<string>(key, true);
        }
    }
}