<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RejectedDrug.aspx.cs" Inherits="EMRBILLING_Popup_RejectedDrug" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rejected Drugs</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div>
            <div class="container-fluid">
                <div class="VisitHistoryBorderNew">

                    <div class="row">
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <asp:CheckBox ID="chkRejectedDrug" Text="Show All Rejected Items" AutoPostBack="true" OnCheckedChanged="chkRejectedDrug_OnCheckedChanged" runat="server" />
                            <span style="width: 100%; float: left; margin: 0; padding: 0; z-index: 1; text-align: left; position: absolute; top: 0;">
                                <asp:Label ID="lblMessage" runat="server" Text="" />
                            </span>
                        </div>

                        <div class="row">
                            <asp:GridView ID="gvRejectedDrug" runat="server" SkinID="gridviewOrderNew" Width="100%" HeaderStyle-ForeColor="#333" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table table-bordered"></asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
