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

    string networkAlias = "NevCAN";

    int selectedDeploymentID;
    int selectedStreamID;

    System.Drawing.Color nevadaBlue = System.Drawing.Color.FromArgb(0, 46, 98);

    System.Drawing.Color defaultButtonColor = System.Drawing.Color.FromArgb(190, System.Drawing.Color.AliceBlue);
    System.Drawing.Color selectedButtonColor = System.Drawing.Color.DarkSlateBlue;

    /// <summary>
    ///     This method is executed as soon as the page loads.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        StartTimeCalendar.SelectedDate = DateTime.UtcNow.AddDays(-1);
        NetworkTree.Nodes[0].Text = networkAlias;
    }

    /* Button Click Methods */

    /// <summary>
    ///     Sets the current sensor network to NevCAN
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NevCanButton_Click(object sender, EventArgs e)
    {
        networkAlias = "NevCAN";
        NetworkButtonClick(sender as Button);
    }

    /// <summary>
    ///     Sets the current sensor network to Walker Basin Hydroclimate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WalkerBasinButton_Click(object sender, EventArgs e)
    {
        networkAlias = "WalkerBasinHydro";
        NetworkButtonClick(sender as Button);
    }

    /// <summary>
    ///     Sets the current sensor network to Solar Energy Nexus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SolarNexusButton_Click(object sender, EventArgs e)
    {
        networkAlias = "SolarNexus";
        NetworkButtonClick(sender as Button);
    }

    /// <summary>
    ///     Is executed with the GetSite button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonGetSite_Click(object sender, EventArgs e)
    {
        th = new Thread(PostMeasurements);
        th.Start();
        th.Join();
    }

    /* Other Methods */

    /// <summary>
    ///     Posts measurements from the currently selected data stream from the start time until the current time.
    /// </summary>
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

    /// <summary>
    ///     Selects the provided button and repopulates the network hierarchy.
    /// </summary>
    /// <param name="button"></param>
    protected void NetworkButtonClick(Button button)
    {
        UncolorNetworkButtons();
        button.BackColor = selectedButtonColor;

        TreeNode root = NetworkTree.Nodes[0];
        root.Text = networkAlias;
        root.ChildNodes.Clear();
        PopulateSites(root);
    }

    /// <summary>
    ///     Resets all the network buttons to the default color.
    /// </summary>
    protected void UncolorNetworkButtons()
    {
        NevCanButton.BackColor = defaultButtonColor;
        WalkerBasinButton.BackColor = defaultButtonColor;
        SolarNexusButton.BackColor = defaultButtonColor;
    }

    /* Network Tree Methods */

    /// <summary>
    ///     Is called whenever the network tree needs to be loaded dynamically
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NetworkTree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        if (e.Node.ChildNodes.Count == 0)
        {
            switch (e.Node.Depth)
            {
                case 0:
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

    /// <summary>
    ///     Is called when the user selects a data stream in the network tree
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void NetworkTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeNode node = NetworkTree.SelectedNode;

        // node Value is the streamID
        selectedStreamID = int.Parse(node.Value);
        // node parent is its deployment - need deploymentID for fast GetDataStream search
        selectedDeploymentID = int.Parse(node.Parent.Value);
    }

    /// <summary>
    ///     Populates the sites in the network tree
    /// </summary>
    /// <param name="parent"></param>
    protected void PopulateSites(TreeNode parent)
    {
        var container = client.GetSiteList(networkAlias);
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

    /// <summary>
    ///     Populates the systems in the network tree
    /// </summary>
    /// <param name="parent"></param>
    protected void PopulateSystems(TreeNode parent)
    {
        var container = client.GetSystemList(networkAlias, int.Parse(parent.Value));
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

    /// <summary>
    ///     Populates the deployments in the network tree
    /// </summary>
    /// <param name="parent"></param>
    protected void PopulateDeployments(TreeNode parent)
    {
        var container = client.GetInstrumentList(networkAlias, int.Parse(parent.Value));
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

    /// <summary>
    ///     Populates the streams in the network tree
    /// </summary>
    /// <param name="parent"></param>
    protected void PopulateStreams(TreeNode parent)
    {
        var container = client.GetDataStreamList(networkAlias, int.Parse(parent.Value));

        if(container.Success)
        {
            var deploymentList = container.Object;

            foreach (var stream in deploymentList.Data)
            {
                string tooltip =
                    "ID: " + stream.ID + "\n" +
                    "Category: " + stream.Category.Name + "\n" +
                    "Property: " + stream.Property.Name + "\n" +
                    "Units: " + stream.Units.Name + "\n" +
                    "Data Type: " + stream.DataType.Name + "\n" +
                    "Interval: " + stream.MeasurementInterval;

                string nodeText = "Data Stream. Type: " + stream.DataType.Name + ". Interval: " + stream.MeasurementInterval;
                var node = new TreeNode(nodeText, stream.ID.ToString())
                {
                    SelectAction = TreeNodeSelectAction.Select,
                    Expanded = true,
                    ToolTip = tooltip
                };

                parent.ChildNodes.Add(node);
            }
        }
        else
        {
            var node = new TreeNode(container.Message, "-1")
            {
                SelectAction = TreeNodeSelectAction.None,
                Expanded = true
            };

            parent.ChildNodes.Add(node);
        }
    }
}