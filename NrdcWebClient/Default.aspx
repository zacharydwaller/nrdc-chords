<%@ Page Language="C#" %>
<%@ Import Namespace="System.Threading" %>

<!DOCTYPE html>

<script runat="server">

    Thread th;

    protected void ButtonGetSite_Click(object sender, EventArgs e)
    {
        th = new Thread(PostMeasurements);
        th.Start();
        th.Join();
    }

    protected void PostMeasurements()
    {
        ChordsService.ServiceClient client = new ChordsService.ServiceClient();
        int siteId = int.Parse(TextBoxSiteId.Text);
        int streamIndex = int.Parse(TextBoxStreamIndex.Text);
        DateTime startTime = StartTimeCalendar.SelectedDate;

        string response = client.GetMeasurements(siteId, streamIndex, startTime, DateTime.UtcNow);

        ResponseLabel.Text = response;

        client.Close();
    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NRDC-CHORDS Web Client</title>
</head>
<body>
    <form id="form1" runat="server">
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
                Select Start Date
            </p>
            <asp:Calendar ID="StartTimeCalendar" runat="server"></asp:Calendar>
        </div>
        <div>
            <asp:Button ID="PostMeasurementsButton" runat="server" Text="Post Measurements" OnClick="ButtonGetSite_Click" />
        </div>
        <div>
            <asp:Label ID="ResponseLabel" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
