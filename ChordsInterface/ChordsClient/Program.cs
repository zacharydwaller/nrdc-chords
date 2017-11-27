using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using ChordsInterface.Api;

namespace ChordsClient
{
    class Program
    {
        public static Chords.ServiceClient client = new Chords.ServiceClient();

        static void Main(string[] args)
        {
            /*
            InstrumentCreator ic = new InstrumentCreator();
            ic.CreateInstrument();
            Console.In.ReadLine();
            return;
            */

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

            string result = client.GetMeasurements(siteID, streamIndex, hours);

            Console.WriteLine(result);

            Console.ReadKey();
        }
    }

    public class InstrumentCreator
    {
        public static HttpClient http = new HttpClient();

        public InstrumentCreator()
        {

        }

        public void CreateInstrument()
        {
            string uri = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/instruments"; 

            byte[] checkBytes = { 0xE2, 0x9C, 0x93 };
            string check = Encoding.UTF8.GetString(checkBytes);

            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("utf8", check));
            formData.Add(new KeyValuePair<string, string>("authenticity_token", "2FssNbWnR5rP414AtSjwBNJP7sYs+3VLbmKEPyBq5lf1SyaAS5gYHFJHR8zQ3gweEC9A5TJlVsDYb5wMYYEmOg=="));
            formData.Add(new KeyValuePair<string, string>("instrument[name]", "Instrument"));
            formData.Add(new KeyValuePair<string, string>("instrument[topic_category_id]", "1"));
            formData.Add(new KeyValuePair<string, string>("instrument[description]", "Description"));
            formData.Add(new KeyValuePair<string, string>("instrument[site_id]", "1"));
            formData.Add(new KeyValuePair<string, string>("instrument[display_points]", "120"));
            formData.Add(new KeyValuePair<string, string>("instrument[plot_offset_value]", "1"));
            formData.Add(new KeyValuePair<string, string>("instrument[plot_offset_units]", "weeks"));
            formData.Add(new KeyValuePair<string, string>("instrument[sample_rate_seconds]", "60"));
            formData.Add(new KeyValuePair<string, string>("commit", "Create Instrument"));

            var content = new FormUrlEncodedContent(formData);

            Console.Write(content.ReadAsStringAsync().Result);

            var response = http.PostAsync(uri, content).Result;

            Console.Write(response);
        }
    }
}
