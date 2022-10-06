<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignedStaffDetails.aspx.cs" Inherits="WardManagement_AssignedStaffDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
 <%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Acknowledgement</title>
    <%-- <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />--%>

    <%--<link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />--%>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
</head>

<script type="text/javascript">
    if (window.captureEvents) {
        window.captureEvents(Event.KeyUp);
        window.onkeyup = executeCode;
    }
    else if (window.attachEvent) {
        document.attachEvent('onkeyup', executeCode);
    }

    function executeCode(evt) {
        if (evt == null) {
            evt = window.event;
        }
        var theKey = parseInt(evt.keyCode, 10);
        switch (theKey) {

            case 113: //F2
                $get('<%=btnSave.ClientID%>').click();
                break;

            case 119:  // F8
                $get('<%=btnclose.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }
</script>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-2">
                <h2>Assigned Staff Details</h2>
            </div>
            <div class="col-md-10 text-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
            </div>
        </div>
        <div class="row">
            <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
        </div>

        <div class="container-fluid" id="Table1" runat="server">
            
            <div class="row margin_Top">
            </div>
            <div class="row margin_Top">
            </div>
            <div class="row margin_Top">
            </div>
            <div class="row margin_Top">
            </div>
            <div class="row margin_Top">
            </div>
            <div class="row margin_Top">
            </div>
            <div class="row margin_Top">
            </div>
            <div class="row" id="Table2" runat="server">
                <div class="col-md-4 m-auto">

                    <div class="row">
                        <div class="row" style="margin-bottom: 5px; width:100%;">
                            <div class="col-md-4 col-3  PaddingRightSpacing label2">
                                <asp:Label ID="Label1" runat="server" Text="Assigned Staff :"></asp:Label><span style="color: Red">*</span>
                            </div>
                            <div class="col-md-8 col-9">

                                <telerik:RadComboBox ID="ddlAssignedStaff"  Filter="Contains" runat="server" width="100%"/>
                                   

                            </div>
                        </div>

                        <div class="row " style="width:100%;">
                            <div class="col-md-4 col-3 PaddingRightSpacing label2">
                                <asp:Label ID="lblentered" runat="server" Text="Remarks :" />
                            </div>
                            <div class="col-md-8 col-9">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3" width="100%" Text="" />
                            </div>
                        </div>
                      

                        <div class="row margin_Top m-auto">
                            <div class="col-6">
                                <asp:Button ID="btnSave" runat="server" Text="Save " ToolTip="Save(Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />&nbsp;&nbsp;
                            </div>
                            <div class="col-6">
                                
                                <asp:Button ID="btnclose" runat="server" Text="Close" ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="window.close();" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
