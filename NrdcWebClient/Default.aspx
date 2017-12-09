<%@ Page Language="C#" Theme="NCTheme" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

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
    <nav>
        <ul>
            <li><a href="Default.aspx">Home</a></li>
            <li><a href="About.aspx">About</a></li>
        </ul>
    </nav>

    <!-- Tab Grid -->
    <div class="tab-container">
        <div class="tab-row">
            <div id="NetTab" class="tab" onclick="openTab('NetTab', 'NetContent');">
                Select a Sensor Network
            </div>

            <div id="StreamTab" class="tab" onclick="openTab('StreamTab', 'StreamContent');">
                Select a Data Stream
            </div>

            <div id="VisTab" class="tab" onclick="openTab('VisTab', 'VisContent');">
                Visualize
            </div>
        </div>

        <!-- Expanding grid -->
        <form runat="server">

            <!-- Select Network Tab -->
            <div id="NetContent" class="tab-content" style="display:none;">

                <!-- Network List -->
                <asp:Button ID="NevCanButton" runat="server" CssClass="network-button" Text="NevCAN" />
                <asp:Button ID="WalkerBasinButton" runat="server" CssClass="network-button" Text="Walker Basin Hydroclimate" />
                <asp:Button ID="SolarNexusButton" runat="server" CssClass="network-button" Text="Solar Energy Nexus" />

            </div>

            <!-- Select Stream Tab -->
            <div id="StreamContent" class="tab-content" style="display:none;">

                <!-- Network Hierarchy -->
                <asp:TreeView ID="NetworkTree" runat="server" MaxDataBindDepth="4" OnTreeNodePopulate="NetworkTree_TreeNodePopulate" ExpandDepth="1">
                    <Nodes>
                        <asp:TreeNode PopulateOnDemand="True" Text="Sensor Network" Value="Sensor Network" SelectAction="Expand"></asp:TreeNode>
                    </Nodes>
                </asp:TreeView>

            </div>

            <!-- Visualize Tab -->
            <div id="VisContent" class="tab-content" style="display:none;">

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
    </div>

    <script>

        var nevadaBlue = "rgb(0,46,98)";
        var aliceBlueTransp = "rgba(240,248,255,0.75)";
        var nevadaBlueTransp = "rgba(0,46,98,0.75)";

        function resetTabContents()
        {
            var contents = document.getElementsByClassName("tab-content");

            // Close tabs
            var i;
            for (i = 0; i < contents.length; i++)
            {
                contents[i].style.display = "none";
            }
        }

        function resetTabs()
        {
            var tabs = document.getElementsByClassName("tab");
            var i;

            for(i = 0; i < tabs.length; i++)
            {
                tabs[i].style.color = nevadaBlue;
                tabs[i].style.backgroundColor = aliceBlueTransp;
            }
        }

        function closeTab()
        {
            resetTabs();
            resetTabContents();
        }

        function openTab(tabName, tabContentName)
        {
            resetTabs();
            resetTabContents();

            // Color selected tab
            var tab = document.getElementById(tabName);
            tab.style.color = "white";
            tab.style.backgroundColor = nevadaBlueTransp;

            // Open tab content
            var tabContent = document.getElementById(tabContentName);
            tabContent.style.display = "block";
        }
    </script>

    <!-- Footer -->
    <footer>
        <p>Copyright &copy; Zachary Waller, Paul Marquis, Pat J, Tom Trowbridge 2017</p>
    </footer>
</body>
</html>
