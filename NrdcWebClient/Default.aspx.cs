using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{

    Thread th;
    ChordsService.ServiceClient client = new ChordsService.ServiceClient();

    ChordsService.DataStream selectedStream;

    protected void Page_Load(object sender, EventArgs e)
    {
        StartTimeCalendar.SelectedDate = DateTime.UtcNow.AddDays(-1);
    }

    protected void ButtonGetSite_Click(object sender, EventArgs e)
    {
        th = new Thread(PostMeasurements);
        th.Start();
        th.Join();
    }

    protected void PostMeasurements()
    {
        /*
        int siteId = int.Parse(TextBoxSiteId.Text);
        int streamIndex = int.Parse(TextBoxStreamIndex.Text);
        DateTime startTime = StartTimeCalendar.SelectedDate;

        string response = client.GetMeasurements(siteId, streamIndex, startTime, DateTime.UtcNow);

        ResponseLabel.Text = response;
        */
    }

    protected void NetworkTree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        string network = "NevCAN";
        if (e.Node.ChildNodes.Count == 0)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    NetworkTree.Nodes[0].Text = network;
                    PopulateSites(e.Node);
                    break;
                case 1:
                    PopulateSystems(e.Node);
                    break;
                case 2:
                    PopulateDeployments(e.Node);
                    break;
                case 3:
                    PopulateStreams(e.Node);
                    break;
            }
        }
    }

    protected void PopulateSites(TreeNode parent)
    {
        var container = client.GetSiteList();
        var sitelist = container.Object;

        foreach(var site in sitelist.Data)
        {
            string tooltip =
                site.Name + "\n" +
                "ID: " + site.ID + "\n" +
                "Latitude: " + site.Latitude + "\n" +
                "Longitude: " + site.Longitude + "\n" +
                "Elevation: " + site.Elevation;

            var node = new TreeNode(site.Name, site.ID.ToString())
            {
                PopulateOnDemand = true,
                SelectAction = TreeNodeSelectAction.Expand,
                ToolTip = tooltip
            };

            parent.ChildNodes.Add(node);
        }
    }

    protected void PopulateSystems(TreeNode parent)
    {
        var container = client.GetSystemList(int.Parse(parent.Value));
        var systemList = container.Object;

        foreach(var system in systemList.Data)
        {
            string tooltip =
                system.Name + "\n" +
                "ID: " + system.ID;

            var node = new TreeNode(system.Name, system.ID.ToString())
            {
                PopulateOnDemand = true,
                SelectAction = TreeNodeSelectAction.Expand,
                ToolTip = tooltip
            };

            parent.ChildNodes.Add(node);
        }
    }

    protected void PopulateDeployments(TreeNode parent)
    {
        var container = client.GetInstrumentList(int.Parse(parent.Value));
        var deploymentList = container.Object;

        foreach (var deployment in deploymentList.Data)
        {
            string tooltip =
                deployment.Name + "\n" +
                "ID: " + deployment.ID;

            var node = new TreeNode(deployment.Name, deployment.ID.ToString())
            {
                PopulateOnDemand = true,
                SelectAction = TreeNodeSelectAction.Expand,
                ToolTip = tooltip
            };

            parent.ChildNodes.Add(node);
        }
    }

    protected void PopulateStreams(TreeNode parent)
    {
        var container = client.GetDataStreams(int.Parse(parent.Value));
        var deploymentList = container.Object;

        foreach (var stream in deploymentList.Data)
        {
            string nodeText = "Data Stream. Type: " + stream.DataType.Name + ". Interval: " + stream.MeasurementInterval;
            var node = new TreeNode(nodeText, stream.ID.ToString())
            {
                PopulateOnDemand = true,
                SelectAction = TreeNodeSelectAction.Select,
                Expanded = true
            };

            parent.ChildNodes.Add(node);
        }
    }
}