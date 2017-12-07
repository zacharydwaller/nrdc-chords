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
        if (e.Node.ChildNodes.Count == 0)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    PopulateNetworks(e.Node);
                    break;
                case 1:
                    PopulateSites(e.Node);
                    break;
            }
        }
    }

    protected void PopulateNetworks(TreeNode node)
    {
        var newNode = new TreeNode("NevCan");
        newNode.PopulateOnDemand = true;
        newNode.SelectAction = TreeNodeSelectAction.Expand;
        node.ChildNodes.Add(newNode);
    }

    protected void PopulateSites(TreeNode node)
    {
        var newNode = new TreeNode("Sheep 1");
        newNode.PopulateOnDemand = true;
        newNode.SelectAction = TreeNodeSelectAction.Expand;
        node.ChildNodes.Add(newNode);
    }
}