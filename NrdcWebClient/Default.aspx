<%@ Page Language="C#" Theme="NCTheme" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>
<%@ Import Namespace="System.Threading" %>

<!DOCTYPE html>

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
        <div id="NetTab" class="tab" onclick="openTab('NetTab', 'NetContent');">Select a Sensor Network</div>
        <div id="StreamTab" class="tab" onclick="openTab('StreamTab', 'StreamContent');">Select a Data Stream</div>
        <div id="VisTab" class="tab" onclick="openTab('VisTab', 'VisContent');">Visualize</div>
    </div>

    <!-- Expanding grid -->
    <form runat="server">

        <!-- Select Network Tab -->
        <div id="NetContent" class="tab-content" style="display:none;">
            <!-- Close Button -->
            <span onclick="this.parentElement.style.display='none'" class="closebtn">&times;</span>

            <!-- Network List -->
            NevCAN/Solar Nexus/Walker Basin

        </div>

        <!-- Select Stream Tab -->
        <div id="StreamContent" class="tab-content" style="display:none;">
            <!-- Close Button -->
            <span onclick="this.parentElement.style.display='none'" class="closebtn">&times;</span>

            <!-- Network Hierarchy -->
            <asp:TreeView ID="NetworkTree" runat="server" MaxDataBindDepth="5" OnTreeNodePopulate="NetworkTree_TreeNodePopulate">
                <Nodes>
                    <asp:TreeNode PopulateOnDemand="True" Text="System List" Value="System List"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>

        </div>

        <!-- Visualize Tab -->
        <div id="VisContent" class="tab-content" style="display:none;">
            <!-- Close Button -->
            <span onclick="this.parentElement.style.display='none'" class="closebtn">&times;</span>

            <!-- Start Date Calender -->
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

    <script>
        function resetTabs()
        {
            var tabs = document.getElementsByClassName("tab-content");

            // Close tabs
            var i;
            for (i = 0; i < tabs.length; i++)
            {
                tabs[i].style.display = "none";
            }
        }

        function resetCols()
        {
            var cols = document.getElementsByClassName("tab");
            
            // Recolor cols
            var i;
            for(i = 0; i < cols.length; i++)
            {
                cols[i].style.backgroundColor = "AliceBlue";
                cols[i].style.color = "Blue";
            }
        }

        function openTab(colName, tabName)
        {
            resetTabs();
            resetCols();

            // Color selected col
            var selectedCol = document.getElementById(colName);
            selectedCol.style.backgroundColor = "blue";
            selectedCol.style.color = "white";

            // Open selected tab
            var selectedTab = document.getElementById(tabName);
            selectedTab.style.display = "block";
            selectedTab.style.backgroundColor = "blue";
        }
    </script>

    <!-- Footer -->
    <footer>
        <p>Copyright &copy; Zachary Waller, Paul Marquis, Pat J, Tom Trowbridge 2017</p>
    </footer>
</body>
</html>
