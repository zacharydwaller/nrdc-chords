﻿using System;
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
using NCInterface.Structures.Data;

namespace NCInterface
{
    public static class ChordsBot
    {
        public static string PortalUrl { get; private set; }

        //private static PhantomJSDriver Driver { get; set; }
        private static ChromeDriver Driver { get; set; }

        private static HttpClient Client { get; set; }

        private static string Email { get; set; } = @"chords@mailinator.com";
        private static string Password { get; set; } = "nrdc2018";

        private static string KeyValue { get; set; } = "key";

        public static void Initialize(string portalUrl)
        {
            PortalUrl = portalUrl;

          //  Driver = new PhantomJSDriver();
            Driver = new ChromeDriver();

            Client = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
            };

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


        public static Container<string> ConfigureVariables (Session session)
        {
             
            string instrumentIDPage = @"instruments/" + session.InstrumentID;
            Driver.Url = PortalUrl + instrumentIDPage;
            Driver.Navigate();
            int counter = 0;
             
              foreach (int ID  in session.StreamIDs)
              {
                  var testStream = DataCenter.GetDataStream(session.NetworkAlias, session.StreamIDs[counter]);
                  var testData = testStream.Data;


                  Driver.ExecuteScript("document.getElementsByName('var[shortname]')[0].setAttribute('type', 'text');");
                  Driver.ExecuteScript("document.getElementsByName('var[name]')[0].setAttribute('type', 'text');");
                  Driver.FindElementById("var_shortname").Clear();
                  Driver.FindElementById("var_shortname").SendKeys(session.StreamIDs[counter].ToString());
                  Driver.FindElementById("var_name").Clear();
                  Driver.FindElementById("var_name").SendKeys(testStream.Data[0].Site.Name + " , " + testStream.Data[0].Deployment.Name + " , " + testStream.Data[0].DataType.Name + " , " + testStream.Data[0].Property.Name);
                  Driver.FindElement(By.XPath("//input[@name='commit' and @value='Add a New Variable']")).Click();

                
                 var testTable = Driver.FindElement(By.XPath("/html/body/div[2]/div[10]/div/table/tbody/tr[last()]/td[3]") );
                testTable.FindElement(By.CssSelector("input")).Clear();
                testTable.FindElement(By.CssSelector("input")).SendKeys("testttttt");

                

                //Tested getting the data stream, will implement creating the variable on CHORDS next
                counter++;
              }

           
                  

            

            

            
            
            return new Container<string>("Success", true);

        }

        public static Container PushMeasurementList(Session session, IList<Measurement> measurementList)
        {
            foreach(var meas in measurementList)
            {
                var container = PushMeasurement(session, meas);

                if (!container.Success) return new Container(container.Message);
            }

            return new Container();
        }

        public static Container PushMeasurement(Session session, Measurement measurement)
        {
            string uri = CreateMeasurementUri(session, measurement);

            var httpTask = Client.GetAsync(Config.ChordsHostUrl + uri);

            try
            {
                httpTask.Wait();
            }
            catch (Exception e)
            {
                
                return new Container(e.InnerException.Message);
            }

            // Measurement successfully created
            if (httpTask.Result.IsSuccessStatusCode)
            {
                return new Container();
            }
            // Creation failed
            else
            {
                return new Container(httpTask.Result.ReasonPhrase);
            }
        }

        private static string CreateMeasurementUri(Session session, Measurement measurement)
        {
            string uri =
                "measurements/url_create?" +
                "instrument_id=" + session.InstrumentID.ToString() +
                "&" + measurement.Stream.ToString() + "=" + measurement.Value.ToString() +
                "&key=" + KeyValue;

            // Insert timestamp
            // Get measurement timestamp, using current local time for now
            // The ToString() arg formats the date as ISO-8601
            String timestamp;

            if (measurement.TimeStamp != null)
            {
                timestamp = measurement.TimeStamp;
            }
            else
            {
                timestamp = DateTime.Now.ToString("s");
            }

            uri += "&at" + timestamp;

            return uri;
        }

        private static string loginUrl = @"/users/sign_in";
    }
}
