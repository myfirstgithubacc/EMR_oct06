<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClearCache.aspx.cs" Inherits="ClearCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnClearCache" runat="server" Text="Clear All Cache" SkinID="Button" OnClick="btnClearCache_OnClick" />
    </div>
    </form>
</body>
</html>
