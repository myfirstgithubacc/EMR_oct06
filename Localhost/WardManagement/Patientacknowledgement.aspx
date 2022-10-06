<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Patientacknowledgement.aspx.cs" Inherits="WardManagement_Patientacknowledgement" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Acknowledgement</title>


    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr.css" rel="stylesheet" />



    
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
    function OnClientIsValidPasswordClose(oWnd, args) {

        var arg = args.get_argument();
        if (arg) {
            var IsValidPassword = arg.IsValidPassword;

            $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPassword.ClientID%>').click();
    }

</script>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-3 col-sm-3 col-xs-3">
                        <h2>Patient Acknowledgement</h2>
                    </div>
                    <div class="col-md-9 col-sm-9 col-xs-9 text-center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
                    </div>
                </div>

                <div class="row" id="Table1" runat="server">
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    
                    <div class="col-md-12 col-sm-12 col-xs-12" id="Table2" runat="server">

                            <div class="col-md-4 col-sm-4 col-xs-12 div_center">

                                <div class="row p-t-b-5 bg-white form-group" id="trEdod" runat="server" >
                                    <div class="col-md-5 col-sm-5 col-5">
                                        <asp:Label ID="Label1" runat="server" Text="Acknowledgement&nbsp;Date :"></asp:Label><span style="color: Red">*</span>
                                    </div>
                                    <div class="col-md-7 col-sm-7 col-7">

                                        <%--<telerik:RadDatePicker ID="dtpEod" Width="100%" runat="server"></telerik:RadDatePicker>--%>
                                        <telerik:RadDateTimePicker ID="txtAckDate" Enabled="false" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy HH:mm" />
                                    </div>

                                </div>
                                <div class="row p-t-b-5 bg-white form-group">

                                    <div class="col-md-5 col-sm-5 col-5">
                                        <asp:Label ID="lblentered" runat="server" Visible="false" Text="Entered&nbsp;by&nbsp;:" />
                                    </div>
                                    <div class="col-md-7 col-sm-7 col-7">
                                        <asp:Label ID="lblenteredby" Visible="false" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="row p-t-b-5 bg-white form-group">
                                    <div class="col-md-5 col-sm-5 col-5"></div>
                                    <div class="col-md-7 col-sm-7 col-7">
                                        <asp:Button ID="btnSave" runat="server" Text="Save " ToolTip="Save(Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />
                                        <asp:Button ID="btnclose" runat="server" Text="Close " ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="window.close();" />
                                    </div>
                                </div>

                            <div class="row m-t">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="rwm1" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                <asp:Button ID="btnIsValidPassword" runat="server" CausesValidation="false"
                                    Style="visibility: hidden;" OnClick="btnIsValidPassword_OnClick" Width="1px" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                            </div>

                        </div>
                    </div>
                </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
