using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordsClient
{
    class Program
    {
        public static Chords.ServiceClient client = new Chords.ServiceClient();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the NRDC-CHORDS Interface prototype.");
            
            // Get System ID
            string sysIDStr;
            int siteID;

            do
            {
                Console.Write("Enter a Site ID: ");
                sysIDStr = Console.ReadLine();
            } while (int.TryParse(sysIDStr, out siteID) == false);

            // Get Stream index
            string streamIndexStr;
            int streamIndex;

            do
            {
                Console.Write("Enter a Stream ID: ");
                streamIndexStr = Console.ReadLine();
            } while (int.TryParse(streamIndexStr, out streamIndex) == false);

            // Get number of hours back 
            string hoursStr;
            int hours;

            do
            {
                Console.Write("Enter amount of past hours to pull data from: ");
                hoursStr = Console.ReadLine();
            } while (int.TryParse(hoursStr, out hours) == false);

            Console.WriteLine("Streaming data...");

            string result = client.PullMeasurements(siteID, streamIndex, hours);

            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
