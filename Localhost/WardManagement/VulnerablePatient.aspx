<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VulnerablePatient.aspx.cs" Inherits="WardManagement_ChangeEncounterPatient" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vulnerable Patient</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

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
</script>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-2">
                <h2>Vulnerable Patient</h2>
            </div>
            <div class="col-md-10 text-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
            </div>
        </div>

        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="left">
                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                        <%--  <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>--%>
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>




        <div class="container-fluid" id="Table1" runat="server">
            <div class="row" id="Table2" runat="server">
                <div class="m-auto col-md-4 col-sm-6 col-12" style="border: 1px solid #85cee3; background: #c1e5ef;  ">
                    <div class="row form-groupTop01 " style="background:#fff;  margin: 5px -6px;">
                        <div class="col-12">
                            <div class="row">
                                <div class="col-md-5 col-5">
                                    <asp:Label ID="lblIsVulnerable" runat="server" Text="Is Vulnerable"></asp:Label>
                                </div>
                                <div class="col-md-7 col-7">
                                    <asp:CheckBox ID="chkIsVulnerable" Text="" runat="server" Checked="true" OnCheckedChanged="chkIsVulnerable_CheckedChanged" AutoPostBack="true" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row form-groupTop01"style="background:#fff;    margin: 5px -6px;"> 
                        <div class="col-12">
                            <div class="row">
                                <div class="col-md-5 col-5 label2">
                                    <asp:Label ID="Lable1" runat="server" Text="Vulnerable Type" />
                                </div>
                                <div class="col-md-7 col-7">
                                    <telerik:RadComboBox ID="ddlVulnerableType" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row margin_Top text-center mb-1">
                       
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ToolTip=" Save(Ctrl+F2)" ValidationGroup="S" OnClick="btnSave_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close " CssClass="btn btn-primary" ToolTip="Close(Ctrl+F8)" CausesValidation="false" OnClientClick="window.close();" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
