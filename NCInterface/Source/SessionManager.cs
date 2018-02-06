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

        public static Container InitializeSession(SessionInitializer args)
        {
            if (args.Validate().Success)
            {

            }
            else
            {

            }

            return null;
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