<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VitalsTemplate.aspx.cs" Inherits="EMR_Vitals_VitalsTemplate" Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Panel1" GroupingText="Select Template" runat="server">
        
        <asp:ListBox ID="lstTemplate" runat="server">
        <asp:ListItem Selected="True" Text="MICCU1" Value="1"></asp:ListItem>
        <asp:ListItem Selected="False" Text="MICCU2" Value="2"></asp:ListItem>
        <asp:ListItem Selected="False" Text="MICCU3" Value="3"></asp:ListItem>
        <asp:ListItem Selected="False" Text="MICCU4" Value="4"></asp:ListItem>
        
        </asp:ListBox>
        <br />
       <%-- <div style="text-align:center;">--%>
        <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="Modalbox.hide();" ImageUrl="/Images/Ok.jpg" />
      <%--  </div>--%>
        </asp:Panel>
    </div>
    </form>

</body>
</html>
