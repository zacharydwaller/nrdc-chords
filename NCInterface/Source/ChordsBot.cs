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
            string instrumentIDPage = @"/instruments/" + session.InstrumentID;
            Driver.Url = PortalUrl + instrumentIDPage;
            Driver.Navigate();
            int counter = 0;
            foreach(int ID  in session.StreamIDs)
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

                //Tested getting the data stream, will implement creating the variable on CHORDS next
                counter++;
            }


            return new Container<string>("test");
        }

        private static string loginUrl = @"/users/sign_in";
    }
}
