<%@ Page Language="C#" %>

<!DOCTYPE html>

<script runat="server">

    protected void ButtonGetSite_Click(object sender, EventArgs e)
    {
        ChordsService.ServiceClient client = new ChordsService.ServiceClient();
        var container = client.GetSite(int.Parse(TextBoxSiteId.Text));
        
        if(container.Success)
        {
            var site = container.Object;
            SiteAlias.Text = site.Name;
        }

    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBoxSiteId" runat="server"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="ButtonGetSite" runat="server" Text="Button" OnClick="ButtonGetSite_Click" />
        </div>
        <div>
            <asp:Label ID="SiteAlias" runat="server" Text="Site Alias"></asp:Label>
        </div>
    </form>
</body>
</html>
