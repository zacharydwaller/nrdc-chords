<%@ Page Language="C#" Theme="NCTheme" %>
<%@ Import Namespace="System.Threading" %>

<!DOCTYPE html>

<script runat="server">

    Thread th;
    ChordsService.ServiceClient client = new ChordsService.ServiceClient();

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

    protected void Page_Load(object sender, EventArgs e)
    {
        StartTimeCalendar.SelectedDate = DateTime.UtcNow.AddDays(-1);
    }

    protected void NetworkTree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        if(e.Node.ChildNodes.Count == 0)
        {
            switch(e.Node.Depth)
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

</script>

<script>
    function openTab(tabName)
    {
        var i, x;
        x = document.getElementsByClassName("containerTab");
        for (i = 0; i < x.length; i++)
        {
            x[i].style.display = "none";
        }
        document.getElementById(tabName).style.display = "block";
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">

    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <meta name="description" content=""/>
    <meta name="author" content="Zachary Waller, Pat J, Paul Marquis, Tom Trowbridge"/>

    <title>NRDC-CHORDS Web Client</title>

</head>
<body>

    <!-- Navigation -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
      <div class="container">
        <a class="navbar-brand" href="#">NRDC-CHORDS Companion Site</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarResponsive">
          <ul class="navbar-nav ml-auto">
            <li class="nav-item active">
              <a class="nav-link" href="#">Home
                <span class="sr-only">(current)</span>
              </a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">About</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Services</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Contact</a>
            </li>
          </ul>
        </div>
      </div>
    </nav>

    <!-- Column Grid -->
    <div class="row">
        <div class="column" onclick="openTab('b1');" style="background:green;">Select a Sensor Network</div>
        <div class="column" onclick="openTab('b2');" style="background:blue;">Select a Data Stream</div>
        <div class="column" onclick="openTab('b3');" style="background:red;">Visualize</div>
    </div>

    <!-- Expanding grid -->
    <form runat="server">
        <!-- Select Network Tab -->
        <div id="b1" class="containerTab" style="display:none;background:green">
            <!-- If you want the ability to close the container, add a close button -->
            <span onclick="this.parentElement.style.display='none'" class="closebtn">x</span>
            NevCAN/Solar Nexus/Walker Basin
        </div>

        <!-- Select Stream Tab -->
        <div id="b2" class="containerTab" style="display:none;background:blue">
            <span onclick="this.parentElement.style.display='none'" class="closebtn">x</span>
            <asp:TreeView ID="NetworkTree" runat="server" MaxDataBindDepth="5" OnTreeNodePopulate="NetworkTree_TreeNodePopulate">
                <Nodes>
                    <asp:TreeNode PopulateOnDemand="True" Text="System List" Value="System List"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>

        <!-- Visualize Tab -->
        <div id="b3" class="containerTab" style="display:none;background:red">
            <span onclick="this.parentElement.style.display='none'" class="closebtn">x</span>
            <div>
                <p>
                    Select Start Date
                </p>
                <asp:Calendar ID="StartTimeCalendar" runat="server"></asp:Calendar>
            </div>
            <div>
                <asp:Button ID="PostMeasurementsButton" runat="server" Text="Post Measurements" OnClick="ButtonGetSite_Click" />
            </div>
        </div>
    </form>

    <!-- Footer -->
    <footer>
        <p>Copyright &copy; Zachary Waller, Paul Marquis, Pat J, Tom Trowbridge 2017</p>
    </footer>
</body>
</html>
