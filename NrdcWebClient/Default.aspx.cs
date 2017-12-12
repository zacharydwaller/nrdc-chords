﻿using System;
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

    string chordsViewPage = "http://ec2-13-57-134-131.us-west-1.compute.amazonaws.com/instruments/1";

    string networkAlias = "NevCAN";

    string deploymentID;
    string streamID;

    static string nevadaBlue = "rgba(0,46,98,0.75)";

    static string defaultButtonColor = "rgba(240,248,255,0.75)";
    string selectedButtonColor = "rgba(0,46,98,1.0)";

    /// <summary>
    ///     This method is executed as soon as the page loads.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //StartTimeCalendar.SelectedDate = DateTime.UtcNow.AddDays(-1);
        ViewState["networkAlias"] = networkAlias;
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
        ViewState["networkAlias"] = networkAlias = "NevCAN";
        NetworkButtonClick(sender as Button);
    }

    /// <summary>
    ///     Sets the current sensor network to Walker Basin Hydroclimate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WalkerBasinButton_Click(object sender, EventArgs e)
    {
        ViewState["networkAlias"] = networkAlias = "WalkerBasinHydro";
        NetworkButtonClick(sender as Button);
    }

    /// <summary>
    ///     Sets the current sensor network to Solar Energy Nexus
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SolarNexusButton_Click(object sender, EventArgs e)
    {
        ViewState["networkAlias"] = networkAlias = "SolarNexus";
        NetworkButtonClick(sender as Button);
    }

    /// <summary>
    ///     Is executed with the GetSite button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void StreamButton_Click(object sender, EventArgs e)
    {
        /*
        th = new Thread(PostMeasurements);
        th.Start();
        th.Join();
        */
        PostMeasurements();
    }

    protected void ChordsButton_Click(object sender, EventArgs e)
    {
        string scriptCall = string.Format("window.open('{0}');", chordsViewPage);

        Page.ClientScript.RegisterStartupScript(
            this.GetType(), "OpenWindow", scriptCall, true);
    }

    /* Other Methods */

    /// <summary>
    ///     Posts measurements from the currently selected data stream from the start time until the current time.
    /// </summary>
    protected void PostMeasurements()
    {
        networkAlias = ViewState["networkAlias"] as string;
        streamID = ViewState["streamID"] as string;
        deploymentID = ViewState["deploymentID"] as string;

        var container = client.GetDataStream(networkAlias, int.Parse(streamID), int.Parse(deploymentID));
        
        if(container.Success)
        {
            DateTime startTime = StartTimeCalendar.SelectedDate;

            var response = client.GetMeasurements(networkAlias, container.Object, startTime, DateTime.Now);

            if(!response.Success)
            {
                NodeLabel.Text = response.Message;
            }
        }
        else
        {
            NodeLabel.Text = container.Message + " ";
        }
    }

    /// <summary>
    ///     Selects the provided button and repopulates the network hierarchy.
    /// </summary>
    /// <param name="button"></param>
    protected void NetworkButtonClick(Button button)
    {
        UncolorNetworkButtons();
        button.Style["color"] = "White";
        button.Style["background-color"] = selectedButtonColor;

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
        NevCanButton.Style["color"] = nevadaBlue;
        NevCanButton.Style["background-color"] = defaultButtonColor;
        WalkerBasinButton.Style["color"] = nevadaBlue;
        WalkerBasinButton.Style["background-color"] = defaultButtonColor;
        SolarNexusButton.Style["color"] = nevadaBlue;
        SolarNexusButton.Style["background-color"] = defaultButtonColor;
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
        ViewState["streamID"] = streamID = node.Value;
        // node parent is its deployment - need deploymentID for fast GetDataStream search
        ViewState["deploymentID"] = streamID = node.Parent.Value;

        NodeLabel.Text = "Selected Data Stream ID: " + streamID.ToString();
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