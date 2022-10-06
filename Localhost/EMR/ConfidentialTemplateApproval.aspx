<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master" CodeFile="ConfidentialTemplateApproval.aspx.cs" Inherits="EMR_ConfidentialTemplateRequest" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%--<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader"  
     Src="~/Include/Components/TopPanelNew.ascx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />


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


    <%--    <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>--%>

    <div class="container-fluid header_main form-group">
        <div class="col-md-4">
            <h2>Confidential Template Approval</h2>
        </div>
        <div class="col-md-8 text-center">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
        </div>
    </div>

    <%--  <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="left">
                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                       
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>--%>

    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>


            <div class="container-fluid" id="Table1" runat="server">
                <div class="row" id="Table2" runat="server">
                    <div class="col-md-12">
                        <div class="col-md-4">
                            <asp:Label ID="lblConfidentialTemplate" runat="server" Text="Confidential Template Approval"></asp:Label>
                            <telerik:RadComboBox ID="ddlConfidentialTemplateApproval" runat="server" EmptyMessage="[ Select ]"
                                Width="176px" Height="400px" DropDownWidth="350px" Filter="Contains"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateMain_SelectedIndexChanged" />

                            <%--<div class="col-md-5">
                            <asp:Label ID="lblIsVulnerable" runat="server" Text="Is Vulnerable"></asp:Label>
                        </div>
                        <div class="col-md-7">
                            <asp:CheckBox ID="chkIsVulnerable" Text="" runat="server" Checked="true" OnCheckedChanged="chkIsVulnerable_CheckedChanged" AutoPostBack="true" />
                        </div>--%>
                        </div>
                        <div class="col-md-4">
                            
                            <telerik:RadComboBox ID="ddlEmployee" runat="server" EmptyMessage="[ Select ]"
                                Width="176px" Height="400px" DropDownWidth="350px" Filter="Contains"
                                AutoPostBack="true" />
                           <asp:Label ID="Label1" runat="server" Text="Employee"></asp:Label>
                             <div style="width:200px; float:right">
                            <asp:CheckBox ID="chkApproved" Text="Approve / Rejected" AutoPostBack="true" runat="server" /></div>
                            <%--  <div class="col-md-5 label2">
                            <asp:Label ID="Lable1" runat="server" Text="Vulnerable Type" /></div>
                        <div class="col-md-7">
                            <telerik:RadComboBox ID="ddlVulnerableType" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                        </div>--%>
                        </div>

                        <div class="col-md-4">
                            
                            <div class="col-md-7">
                                <asp:Button ID="btnSave" runat="server" Text="Save (Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />
                                <asp:Button ID="btnclose" runat="server" Text="Close (Ctrl+F8)" Visible="false" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="window.close();" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
