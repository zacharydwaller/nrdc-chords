using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Net;

namespace ChordsPusher
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com";
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/sites");

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("utf8", "\u2713"));
            keyValues.Add(new KeyValuePair<string, string>("authenticity_token", "2FssNbWnR5rP414AtSjwBNJP7sYs+3VLbmKEPyBq5lf1SyaAS5gYHFJHR8zQ3gweEC9A5TJlVsDYb5wMYYEmOg=="));
            keyValues.Add(new KeyValuePair<string, string>("instrument[name]", "Instrument"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[topic_category_id]", "1"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[description]", "Instrument"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[site_id]", "2"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[display_points]", "120"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[plot_offset_value]", "1"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[plot_offset_units]", "weeks"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[sample_rate_seconds]", "60"));
            keyValues.Add(new KeyValuePair<string, string>("commit", "Create Instrument"));

            request.Content = new FormUrlEncodedContent(keyValues);

            var response = client.SendAsync(request).Result;

            Console.WriteLine(string.Format("{0} {1}", response.StatusCode, response.ReasonPhrase));

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
