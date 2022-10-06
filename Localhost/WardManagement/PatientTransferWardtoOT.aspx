<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientTransferWardtoOT.aspx.cs" Inherits="WardManagement_PatientTransferWardtoOT" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Send to OT</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
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
                case 114: //F3
                    $get('<%=btnCancel.ClientID%>').click();
                    break;
                case 119: // F8
                    $get('<%=btnclose.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }
        function MaxLenTxt(TXT) {
            if (TXT.value.length > 500) {
                alert("Text length should not be greater then 500 ...");

                TXT.value = TXT.value.substring(0, 500);
                TXT.focus();
            }
        }
    </script>

<body style="overflow:hidden;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="row">
                <div class="col-3 col-md-3">
                    <h2>Send to OT</h2>
                </div>
                <div class="col-9 col-md-9 text-center">
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" /></div>
            </div>
        </div>


        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="center">
                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" ForeColor="Green" Font-Bold="true"></asp:Label>
                    </td>
                </tr>    
            </tbody>
        </table>

        <div class="container-fluid" id="Table1" runat="server">
            <div class="row" id="Table2" runat="server">
                <div class=" col-xs-10  col-md-7 m-auto">
                    <div class="row form-group">
                        <div class="col-xs-2 col-md-1 label2">
                            <asp:Label ID="Label1" runat="server" Text="Theater" /></div>
                        <div class="col-xs-4 col-md-3">
                            <telerik:RadComboBox ID="ddlTheater" runat="server" CssClass="drapDrowHeight" Width="100%" />
                        </div>
                    </div>
                    <div class="row form-group">
                        <div class="col-xs-2 col-md-1 label2">
                            <asp:Label ID="Lable1" runat="server" Text="Remarks" /></div>
                        <div class="col-xs-8 col-md-8">
                            <asp:TextBox ID="txtWardRemarks" runat="server" TextMode="MultiLine" Style="height: 80px; width: 100%;" MaxLength="500" onkeyup="return MaxLenTxt(this);" /></div>
                    </div>

                    <div class="row form-group">
                        <div class="col-xs-12 col-md-12 text-center">
                            <asp:Button ID="btnSave" runat="server" Text="Send To OT " ToolTip="Send to OT(Ctrl+F2)" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel " ToolTip="Cancel(Ctrl+F3)" CssClass="btn btn-primary" OnClick="btnCancel_OnClick" />
                            <asp:Button ID="btnclose" runat="server" Text="Close " ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>