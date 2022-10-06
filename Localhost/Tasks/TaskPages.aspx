<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaskPages.aspx.cs" Inherits="Tasks_TaskPages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/Tasks/Component/Tasks.ascx" TagName="Tasks" TagPrefix="uc11" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc11:Tasks ID="Tasks1" runat="server"></uc11:Tasks>
    </div>
    </form>
</body>
</html>
