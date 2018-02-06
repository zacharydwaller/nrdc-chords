using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NCInterface.Structures;
using NCInterface.Structures.Infrastructure;
using NCInterface.Utilities;

namespace NCInterface
{
    public static class SessionManager
    {
        public static Dictionary<string, Session> SessionDict { get; private set; }
        public static int KeyLength { get; } = 5;

        static SessionManager()
        {
            SessionDict = new Dictionary<string, Session>();
        }

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