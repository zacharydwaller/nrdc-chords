using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;

namespace ChordsPusher
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com";

            /*
             * GET http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/instruments/new
             * And get the meta name="crsf-token" value. Use this as the authenticity-token
             */

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("utf8", "\u2713"));
            keyValues.Add(new KeyValuePair<string, string>("authenticity_token", "FbbywjE1ZxQEzXAY5Zw6GTSSLiVk4A8BwM6gAZGNi3XqOZ+Jfg5JvmG8RKT4KFv50+Y1ArS3LQmzjTjWxXI7Iw=="));
            keyValues.Add(new KeyValuePair<string, string>("instrument[name]", "Instrument"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[topic_category_id]", "1"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[description]", "Instrument"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[site_id]", "1"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[display_points]", "120"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[plot_offset_value]", "1"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[plot_offset_units]", "weeks"));
            keyValues.Add(new KeyValuePair<string, string>("instrument[sample_rate_seconds]", "60"));
            keyValues.Add(new KeyValuePair<string, string>("commit", "Create Instrument"));

            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };

            client.DefaultRequestHeaders.ExpectContinue = false;

            var request = new HttpRequestMessage(HttpMethod.Post, "/instruments");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*; q=0.8");
            request.Headers.Add("Referer", "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/instruments/new");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9");
            
            request.Content = new FormUrlEncodedContent(keyValues);

            Console.WriteLine(request);

            var response = client.SendAsync(request).Result;

            Console.WriteLine(string.Format("{0} {1}", response.StatusCode, response.ReasonPhrase));

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            //var cookieContainer = new CookieContainer();
            //var postData = new FormUrlEncodedContent(keyValues);
            //ServicePointManager.Expect100Continue = false;
            //var request = (HttpWebRequest)WebRequest.Create(url + "/instruments");
            //request.Referer = url + "instruments/new";
            //request.CookieContainer = cookieContainer;
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.Method = "POST";
            //byte[] bytes = postData.ReadAsByteArrayAsync().Result;
            //request.ContentLength = bytes.Length;
            //var outStream = request.GetRequestStream();
            //outStream.Write(bytes, 0, bytes.Length);
            //outStream.Close();
            //var response = request.GetResponse();

            //var reader = new StreamReader(response.GetResponseStream());
            //string str = reader.ReadToEnd().Trim();

            //Console.Out.WriteLine(str);
        }
    }
}
