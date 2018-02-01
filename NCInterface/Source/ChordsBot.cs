using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using NCInterface.Structures;

namespace NCInterface
{
    public static class ChordsBot
    {
        public static string PortalUrl { get; private set; }

        //private static PhantomJSDriver Driver { get; set; }
        private static ChromeDriver Driver { get; set; }

        private static string Email { get; set; } = @"chords@mailinator.com";
        private static string Password { get; set; } = "nrdc2018";

        public static void Initialize(string portalUrl)
        {
            PortalUrl = portalUrl;
          //  Driver = new PhantomJSDriver();
            Driver = new ChromeDriver();
            Login();
        }

        private static string Login()
        { 
            Driver.Url = PortalUrl + loginUrl;
            Driver.Navigate();

            var email = Driver.FindElementById("user_email");
            var pass = Driver.FindElementById("user_password");
            var submit = Driver.FindElementByName("commit");

            email.SendKeys(Email);
            pass.SendKeys(Password);

            submit.Click();

            var cookies = Driver.Manage().Cookies.AllCookies;

            var sb = new StringBuilder();
            if(cookies.Count > 0)
            {
                sb.AppendLine("\n\nLogin Successful.\nCookies Retrieved:");
                foreach (var cookie in cookies)
                {
                    sb.AppendLine(string.Format("{0}: {1}", cookie.Name, cookie.Value));
                }
            }

            return sb.ToString();
        }

      

        public static Container<int> CreateInstrument(string name)
        {
            string newInstrument = @"/instruments/new";

            var keyValues = CreateInstrumentData();
            Driver.Url = PortalUrl + newInstrument;
            Driver.Navigate();

            Driver.FindElementById("instrument_name").SendKeys(name);
            Driver.FindElementById("instrument_sample_rate_seconds").SendKeys(Config.DefaultSampleRate.ToString());
            Driver.FindElementByName("commit").Click();

            int id;
            string idString = Driver.Url.Substring(Driver.Url.LastIndexOf("/")+1);
            if (int.TryParse(idString, out id))
            {
                return new Container<int>(id, true);
            }
            else
            {
                return new Container<int>("Instrument URL could not be parsed: "+ Driver.Url);
            }
        }

        public static Container<int> DeleteInstrument(int instrumentID)
        {

            string instrumentPage = @"/instruments/";
            Driver.Url = PortalUrl + instrumentPage;
            Driver.Navigate();

            var destroyButtons = Driver.FindElements(By.ClassName("button_to"));

            string idString = string.Format("/instruments/{0}", instrumentID);

            var instrButton = destroyButtons.FirstOrDefault(b => b.GetAttribute("action").Contains(idString));
            instrButton.Click();

            Driver.SwitchTo().Alert().Accept();

            if(instrButton != null)
            {
                return new Container<int>(instrumentID);
            }
            else
            {
                return new Container<int>("Could not delete instrument ID: " + instrumentID);
            }
        }


        public static Container<string> ConfigureVariables (int instrumentID, List<int> DataStreamIDs)
        {
            //DataCenter.GetDataStream
            return new Container<string>("test");
        }

        private static List<KeyValuePair<string,string>> CreateInstrumentData()
        {
            /*
             * To retrive authenticity-token:
             * GET {PortalUrl}/instruments/new
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
            return keyValues;
        }

        private static string loginUrl = @"/users/sign_in";
    }
}
