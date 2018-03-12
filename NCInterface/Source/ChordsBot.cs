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
using NCInterface.Structures.Data;

namespace NCInterface
{ 
    //Class that automatically opens, logs in, and performs other functions in CHORDS 
    public static class ChordsBot
    {
        public static string PortalUrl { get; private set; }
        //private static PhantomJSDriver Driver { get; set; }
        private static ChromeDriver Driver { get; set; }
        private static HttpClient Client { get; set; }
        //sets log in information for CHORDS account 
        private static string Email { get; set; } = @"chords@mailinator.com";
        private static string Password { get; set; } = "nrdc2018";
        private static string KeyValue { get; set; } = "key";

        /// <summary>
        /// Initializes the Selenium webdriver and logs into the CHORDS portal
        /// </summary>
        /// <param name="portalUrl"></param>
        public static void Initialize(string portalUrl)
        {
            PortalUrl = portalUrl;

            //Driver = new PhantomJSDriver();
            Driver = new ChromeDriver();

            Client = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(Config.DefaultTimeout)
            };

            Login();
        }

        /// <summary>
        /// Creates a new CHORDS instrument with the given name
        /// </summary>
        /// <param name=""></param>
        /// <returns>A string with a success message and list of cookies retrieved</returns>
        private static string Login()
        {  
            //Enters sign in URL into address bar and navigates to it
            Driver.Url = PortalUrl + @"/users/sign_in";
            Driver.Navigate(); 
            //Finds the email and password fields
            var email = Driver.FindElementById("user_email");
            var pass = Driver.FindElementById("user_password");
            var submit = Driver.FindElementByName("commit");
            //Sends email and password to those fields
            email.SendKeys(Email);
            pass.SendKeys(Password);
            //Clicks the commit button to save changes
            submit.Click();
            //Gets all cookies and appends their data to a string
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

        /// <summary>
        /// Creates a new CHORDS instrument with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The instrument's ID as an int Container, or an error message as a string</returns>
        public static Container<int> CreateInstrument(string name)
        {
            //Writes the new instrument page URL to a string and navigates there
            string newInstrument = @"/instruments/new";
            Driver.Url = PortalUrl + newInstrument;
            Driver.Navigate();
            //Finds name and sample rate fields, clears old data and updates with new data
            Driver.FindElementById("instrument_name").SendKeys(name);
            Driver.FindElementById("instrument_sample_rate_seconds").Clear();
            Driver.FindElementById("instrument_sample_rate_seconds").SendKeys("60");
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

        /// <summary>
        /// Deletes the CHORDS instrument that has the provided ID
        /// </summary>
        /// <param name="instrumentID"></param>
        /// <returns>An int Container with the instrument ID of the deleted instrument or an error message</returns>
        public static Container<int> DeleteInstrument(int instrumentID)
        {
            //Navigates to the CHORDS delete instrument page
            string instrumentPage = @"/instruments/";
            Driver.Url = PortalUrl + instrumentPage;
            Driver.Navigate(); 
            //Finds button to destroy selected instrument by ID
            var destroyButtons = Driver.FindElements(By.ClassName("button_to"));
            string idString = string.Format("/instruments/{0}", instrumentID);
            var instrButton = destroyButtons.FirstOrDefault(b => b.GetAttribute("action").Contains(idString));
            instrButton.Click();
            //Closes dialogue box that opens and shifts browser focus
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

        /// <summary>
        /// Maps CHORDS instrument variables to a session's list of streams
        /// </summary>
        /// <param name="session"></param>
        /// <returns>A string success or failure message</returns>
        public static Container ConfigureVariables (Session session)
        { 
            //Navigates to CHORDS instrument page
            string instrumentIDPage = @"instruments/" + session.InstrumentID;
            Driver.Url = PortalUrl + instrumentIDPage;
            Driver.Navigate(); 
            //Makes a graph variable for each stream in the session
            for (int index = 0; index < session.StreamIDs.Count(); index++)
            { 
                //Get data stream for a particular session, change variable values accordingly
                var Stream = DataCenter.GetDataStream(session.NetworkAlias, session.StreamIDs[index]);
                Driver.ExecuteScript("document.getElementsByName('var[shortname]')[0].setAttribute('type', 'text');");
                Driver.ExecuteScript("document.getElementsByName('var[name]')[0].setAttribute('type', 'text');");
                Driver.FindElementById("var_shortname").Clear();
                Driver.FindElementById("var_name").Clear();
                Driver.FindElementById("var_shortname").SendKeys(session.StreamIDs[index].ToString());
                Driver.FindElementById("var_name").SendKeys(Stream.Data[0].Site.Alias + " , " + Stream.Data[0].Deployment.Name + " , " + Stream.Data[0].DataType.Name + " , " + Stream.Data[0].Property.Name);
                Driver.FindElement(By.XPath("//input[@name='commit' and @value='Add a New Variable']")).Click();
                //Sets measurment units for new variables
                var Table = Driver.FindElement(By.XPath("/html/body/div[2]/div[10]/div/table/tbody/tr[last()]/td[3]") );
                Table.FindElement(By.CssSelector("input")).Clear();
                Table.FindElement(By.CssSelector("input")).SendKeys(Stream.Data[0].Units.Name);
            }
            /*
            var testStream = DataCenter.GetDataStream(session.NetworkAlias, session.StreamIDs[0]);
            var testData = testStream.Data;

            var testTable = Driver.FindElement(By.XPath("/html/body/div[2]/div[10]/div/table/tbody/tr[last()]/td[3]"));
            testTable.FindElement(By.CssSelector("input")).Clear();
            testTable.FindElement(By.CssSelector("input")).SendKeys(testStream.Data[0].Units.Name);
            // testTable.FindElement(By.CssSelector("input")).Click();
            testTable.FindElement(By.CssSelector("input")).SendKeys(Keys.Down);
            //  Driver.FindElement(By.CssSelector("div")).Click();
            // testTable.FindElement(By.CssSelector("input")).SendKeys(Keys.Enter);
            
            */
            //*[@id="unit_for_tag_47"]
            return new Container();
        }

        /// <summary>
        /// Streams a list of measurements to a CHORDS instrument provided by the session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="measurementList"></param>
        /// <returns>String Container with a success or failure message</returns>
        public static Container PushMeasurementList(Session session, IList<Measurement> measurementList)
        {
            foreach(var meas in measurementList)
            {
                var container = PushMeasurement(session, meas);
                if (!container.Success) return new Container(container.Message);
            }
            return new Container();
        }

        /// <summary>
        /// Pushes a single measurement to a CHORDS instrument provided by the session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="measurement"></param>
        /// <returns>String Container with a success or failure message</returns>
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

        /// <summary>
        /// Creates the URI needed to push the measurements to CHORDS
        /// </summary>
        /// <param name="session"></param>
        /// <param name="measurement"></param>
        /// <returns>A string with the proper formatting based on the parameters</returns>
        private static string CreateMeasurementUri(Session session, Measurement measurement)
        {
            string uri =
                String.Format("measurements/url_create?instrument_id={0}&{1}={2}&key={3}",
                session.InstrumentID.ToString(), measurement.Stream, measurement.Value, KeyValue);
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
            uri += string.Format("&at={0}", timestamp);
            return uri;
        }
    }
}
