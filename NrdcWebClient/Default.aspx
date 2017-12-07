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
        int siteId = int.Parse(TextBoxSiteId.Text);
        int streamIndex = int.Parse(TextBoxStreamIndex.Text);
        DateTime startTime = StartTimeCalendar.SelectedDate;

        string response = client.GetMeasurements(siteId, streamIndex, startTime, DateTime.UtcNow);

        ResponseLabel.Text = response;
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

    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">

    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <meta name="description" content=""/>
    <meta name="author" content="Zachary Waller, Pat J, Paul Marquis, Tom Trowbridge"/>

    <title>NRDC-CHORDS Web Client</title>

    <!-- Bootstrap core CSS -->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>

    <!-- Custom styles for this template -->
    <link href="css/heroic-features.css" rel="stylesheet"/>

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

    

    <!-- Date Picker -->
    <form id="form1" runat="server">
        <div>
            <asp:TreeView ID="NetworkTree" runat="server"></asp:TreeView>
        </div>
        <div>
            <p>
                Select Site ID
            </p>
            <asp:TextBox ID="TextBoxSiteId" runat="server"></asp:TextBox>
        </div>
        <div>
            <p>
                Select Stream Index
            </p>
            <asp:TextBox ID="TextBoxStreamIndex" runat="server"></asp:TextBox>
        </div>
        <div>
            <p>
                Select Start Date</p>
            <p>
                &nbsp;<asp:Calendar ID="StartTimeCalendar" runat="server"></asp:Calendar>
            </p>
        </div>
        <div>
            <asp:Button ID="PostMeasurementsButton" runat="server" Text="Post Measurements" OnClick="ButtonGetSite_Click" />
        </div>
        <div>
            <asp:Label ID="ResponseLabel" runat="server" Text=""></asp:Label>
        </div>
    </form>

    <!-- Footer -->
    <footer class="py-5 bg-dark">
      <div class="container">
        <p class="m-0 text-center text-white">Copyright &copy; Zachary Waller, Paul Marquis, Pat J, Tom Trowbridge 2017</p>
      </div>
      <!-- /.container -->
    </footer>
</body>
</html>
