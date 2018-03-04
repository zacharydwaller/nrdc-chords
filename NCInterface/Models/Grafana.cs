using System.Collections.Generic;
using Newtonsoft.Json;

namespace NCInterface.Structures.Grafana

{
    public  class GrafanaAPI
    {
     public static string APIKey = "eyJrIjoicmtVcVA4MjN6dTVKWFNRWUliMUJmVTdVUlJKdWpOclEiLCJuIjoidGVzdDEiLCJpZCI6MX0";
        //eyJrIjoicmtVcVA4MjN6dTVKWFNRWUliMUJmVTdVUlJKdWpOclEiLCJuIjoidGVzdDEiLCJpZCI6MX0=
     public static string Header = "Bearer";
    }
    public class User
    {
        public string name { get; set; }
        public string role { get; set; }
    }


    public class Dashboard
    {
        [JsonProperty("title")]
        public string  title = "testtt" ;
        //public string id = null;
        //public string uid = null;
    }


    [JsonObject(MemberSerialization.OptIn)]
    public class Response
    {
        [JsonProperty("dashboard")]
        public Dashboard dashboard { get; set; }
        
        //public string [] title = { "testtt" };
        //public string id = null;
        //public string uid = null;
        //public string[] tags = { "templated" };
        //public string timezone = "browser";
        //public string[] rows = { };
        //public int schemaVersion = 6;
        //public int version = 0;
        //public int folderId = 0;
        //public bool overwrite = false;
         int test;
         

    }
}