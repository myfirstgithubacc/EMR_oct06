<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeEncounterDate.aspx.cs" Inherits="WardManagement_ChangeEncounterDate" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Probable&nbsp;Discharge</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        div#UpdatePanel1{
            overflow:hidden;
        }

    </style>
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

            case 114: //F3
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
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
</script>

<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>
  <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid header_main">
                    <div class="row">
                        <div class="col-md-2">
                            <h2>Change&nbsp;Probable&nbsp;Discharge</h2>
                        </div>
                        <div class="col-md-10 text-center">
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" /></div>
                    </div>
                </div>

        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="left">
                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>    
            </tbody>
        </table>




                <div class="container-fluid" id="Table1" runat="server">
                    <div class="row" id="Table2" runat="server">
                        <div class="m-auto col-md-4 col-sm-6 col-12" style="border: 1px solid #98c9d7; background: #c1e5ef; padding: 5px 22px;">
                            <div class="row form-groupTop01" style="display: none; background: #fff;">
                                <div class="col-md-5 label2">
                                    <asp:Label ID="Lable1" runat="server" Text="Change&nbsp;Status" /></div>
                                <div class="col-md-7">
                                    <telerik:RadComboBox ID="ddlStatus" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                </div>
                            </div>

                            <div class="row form-groupTop01" id="trEdod" runat="server" style="background: #fff; padding-bottom: 4px;">
                                <div class="col-md-5 PaddingRightSpacing label2">
                                    <asp:Label ID="Label1" runat="server" Text="Expected Date Of Discharge"></asp:Label><span style="color: Red">*</span></div>
                                <div class="col-md-7">
                                    <telerik:RadDatePicker ID="dtpEod" Width="100%" runat="server"></telerik:RadDatePicker>
                                </div>
                            </div>

                            <div class="row form-groupTop01" id="trStatus" runat="server" style="display: none; background: #fff;">
                                <div class="col-md-5 label2">
                                    <asp:Label ID="ltrldischargestatus" runat="server" Text="<%$ Resources:PRegistration, dischargestatus %>"></asp:Label><span style="color: Red">*</span></div>
                                <div class="col-md-7">
                                    <telerik:RadComboBox ID="ddldischargestatus" runat="server" Width="100%"></telerik:RadComboBox>
                                </div>
                            </div>

                            <div class="row margin_Top text-center">

                                <div class="col-md-12">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" ToolTip="Save(Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnclose" runat="server" Text="Close " ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="window.close();" />
                                </div>
                            </div>

                </div>
            </div>
        </div>
                  <div>
                    <asp:UpdatePanel ID="up1" runat="server">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="rwm1" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>