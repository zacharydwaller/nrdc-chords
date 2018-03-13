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
        [JsonProperty("rows")]
        public List<Row> rows { get; set; }
        [JsonProperty("time")]
        public Time time { get; set; }

        [JsonProperty("version")]
        public int version { get; set; }
        [JsonProperty("thresholds")]
        public List<object> thresholds { get; set; }
    }

    public class Input
    {
        public string name { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string pluginId { get; set; }
        public string pluginName { get; set; }
    }

    public class Require
    {
        public string type { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
    }

    public class Annotations
    {
        public List<object> list { get; set; }
    }

    public class AliasColors
    {
    }

    public class Legend
    {
        public bool avg { get; set; }
        public bool current { get; set; }
        public bool max { get; set; }
        public bool min { get; set; }
        public bool show { get; set; }
        public bool total { get; set; }
        public bool values { get; set; }
    }

    public class GroupBy
    {
        public List<string> @params { get; set; }
        public string type { get; set; }
    }

    public class Target
    {
        public string dsType { get; set; }
        public List<GroupBy> groupBy { get; set; }
        public string measurement { get; set; }
        public string policy { get; set; }
        public string refId { get; set; }
        public string resultFormat { get; set; }
       // public List<List<>> select { get; set; }
        public List<object> tags { get; set; }
    }

    public class Tooltip
    {
        public bool shared { get; set; }
        public int sort { get; set; }
        public string value_type { get; set; }
    }

    public class Xaxis
    {
        public string mode { get; set; }
        public object name { get; set; }
        public bool show = true;
        public List<object> values { get; set; }
    }

    public class Yax
    {
        public string format { get; set; }
        public object label { get; set; }
        public int logBase { get; set; }
        public object max { get; set; }
        public object min { get; set; }
        public bool show = true;
    }

    public class Panel
    {
        public AliasColors aliasColors { get; set; }
        public bool bars { get; set; }
        public string datasource { get; set; }
        public int fill { get; set; }
        public int id { get; set; }
        public Legend legend { get; set; }
        public bool lines { get; set; }
        public int linewidth { get; set; }
        public List<object> links { get; set; }
        public string nullPointMode { get; set; }
        public bool percentage { get; set; }
        public int pointradius { get; set; }
        public bool points { get; set; }
        public string renderer { get; set; }
        public List<object> seriesOverrides { get; set; }
        public int span { get; set; }
        public bool stack { get; set; }
        public bool steppedLine { get; set; }
        public List<Target> targets { get; set; }
        public List<object> thresholds { get; set; }
        public object timeFrom { get; set; }
        public object timeShift { get; set; }
        public string title { get; set; }
        public Tooltip tooltip { get; set; }
        public string type { get; set; }
        public Xaxis xaxis { get; set; }
        public List<Yax> yaxes { get; set; }
       // public Yax yaxis {get; set;}
    }

    public class Row
    {
        public bool collapse { get; set; }
        public string height { get; set; }
        public List<Panel> panels { get; set; }
        public object repeat { get; set; }
        public object repeatIteration { get; set; }
        public object repeatRowId { get; set; }
        public bool showTitle { get; set; }
        public string title { get; set; }
        public string titleSize { get; set; }
    }

    public class Templating
    {
        public List<object> list { get; set; }
    }

    public class Time
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Timepicker
    {
        public List<string> refresh_intervals { get; set; }
        public List<string> time_options { get; set; }
    }

    public class Response
    {

        [JsonProperty("dashboard")]
        public Dashboard dashboard { get; set; }
        public List<Input> __inputs { get; set; }
        public List<Require> __requires { get; set; }
        public Annotations annotations { get; set; }
        public bool editable { get; set; }
        public object gnetId { get; set; }
        public int graphTooltip { get; set; }
        public bool hideControls { get; set; }
        public object id { get; set; }
        public List<object> links { get; set; }
        public List<Row> rows { get; set; }
        public int schemaVersion { get; set; }
        public string style { get; set; }
        public List<object> tags { get; set; }
        public Templating templating { get; set; }
        public Time time { get; set; }
        public Timepicker timepicker { get; set; }
        public string timezone { get; set; }
        public string title { get; set; }
        public int version { get; set; }
    }

}