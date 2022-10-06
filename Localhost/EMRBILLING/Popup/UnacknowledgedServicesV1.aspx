<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnacknowledgedServicesV1.aspx.cs"
    Inherits="EMRBILLING_Popup_UnacknowledgedServicesV1" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Activities</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>

<script type="text/javascript">
    function disp_confirm() {
        var r = confirm("Are you sure to cancel all services..!")

        if (r == true) {
            $get('<%=btnYes.ClientID%>').click();
        }
    }
</script>


<body>
    <form id="form1" runat="server" style="overflow:hidden;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="container-fluid header_main">
            <div class="row">
                <div class="col-md-5 col-sm-5">
                    <div class="row">
                        <div class="col-md-1 col-sm-2 col-2 PaddingRightSpacing">
                            <asp:Label ID="lbl" runat="server" Text="Select"></asp:Label></div>
                        <div class="col-md-9 col-sm-8 col-7">
                            <telerik:RadComboBox ID="ddlFilter" runat="server" Width="100%" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlFilter_OnSelectedIndexChanged"></telerik:RadComboBox>
                        </div>
                        <div class="col-md-2 col-sm-2 col-3 text-left PaddingLeftSpacing">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_OnClick" /></div>
                    </div>
                </div>
                
                <div class="col-md-5 col-sm-5">
                    <div class="row">
                        <div class="col-md-3 col-sm-4 PaddingSpacing">
                            <asp:Label ID="lblCremrk" runat="server" Text="Cancel Remark "></asp:Label></div>
                        <div class="col-md-7 col-sm-6 PaddingLeftSpacing">
                            <asp:TextBox ID="txtRemark" runat="server" Width="100%"></asp:TextBox></div>
                        <div class="col-md-2 col-sm-2 PaddingLeftSpacing">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                            <asp:Button ID="btnCancelUnPerform" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClientClick="disp_confirm();" />
                            <asp:Button ID="btnYes" runat="server" OnClick="btnYes_OnClick" Style="visibility: hidden; float:left; height:1px;" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnYes" />
                                </Triggers>
                                </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="col-md-2 col-sm-2 text-right">
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 text-center">
                    <asp:Label ID="lblMesaage" runat="server"></asp:Label></div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="VisitHistoryBorderNew">

                <div class="row">
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <asp:GridView ID="gvUnacknowledgedServices" runat="server" SkinID="gridviewOrderNew" Width="100%" OnRowDataBound="gvUnacknowledgedServices_OnRowDataBound" HeaderStyle-ForeColor="#333" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table table-bordered"></asp:GridView>
            </div>
        </div>

    </form>
</body>
</html>
