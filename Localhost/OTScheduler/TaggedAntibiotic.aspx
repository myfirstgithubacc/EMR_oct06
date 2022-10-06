<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaggedAntibiotic.aspx.cs" Inherits="OTScheduler_TaggedAntibiotic" %>
<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tagged Antibiotics</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-12 PaddingSpacing">
                    <telerik:RadScriptManager id="ScriptManager1" runat="server"></telerik:RadScriptManager>
                    <asp:UpdatePanel ID ="updatePanel2" runat="server" >
                        <ContentTemplate><telerik:RadListBox ID ="rlbListAntibiotic" runat="server" Width="100%" Height="240px"></telerik:RadListBox></ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>