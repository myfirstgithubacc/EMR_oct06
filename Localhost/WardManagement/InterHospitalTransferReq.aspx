<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InterHospitalTransferReq.aspx.cs" Inherits="WardManagement_InterHospitalTransferReq" %>

<!DOCTYPE html>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inter Hospital Transfer Request</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
   <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
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

    function MaxLenTxt(TXT, MAX) {
        if (TXT.value.length > MAX) {
            alert("Text length should not be greater then " + MAX + " ...");

            TXT.value = TXT.value.substring(0, MAX);
            TXT.focus();
        }
    }

    function openRadWindow(strPageNameWithQueryString) {
        var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
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

    function Confirm() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Have you returned all the medicines ?")) {
            confirm_value.value = "Yes";
        } else {
            confirm_value.value = "No";
        }
        document.forms[0].appendChild(confirm_value);
    }


</script>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-3">
                <h2>Inter Hospital Transfer Request</h2>
            </div>
            <div class="col-md-9 text-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
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
                <div class="col-md-4 div_center">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <div class="row form-group bg-white p-1">
                                <div class="col-5 label2">
                                    <asp:Label ID="Lable1" runat="server" Text="To Hospital" />
                                </div>
                                <div class="col-7">
                                    <telerik:RadComboBox ID="ddlToHospital" AutoPostBack="true" OnSelectedIndexChanged="ddlToHospital_SelectedIndexChanged" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                </div>
                            </div>
                            
                              <div class="row form-group bg-white p-1">
                                <div class="col-5 label2">
                                    <asp:Label ID="Label1" runat="server" Text="Specilization" />
                                </div>
                                <div class="col-7">
                                     <telerik:RadComboBox ID="ddlSpecilization"  runat="server" MarkFirstMatch="true" Filter="Contains" AppendDataBoundItems="true"
                                            Height="300px" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSpecilization_SelectedIndexChanged" />

                                </div>
                            </div>

                              <div class="row form-group bg-white p-1">
                                <div class="col-5 label2">
                                    <asp:Label ID="Label3" runat="server" Text="Provider" />
                                </div>
                                <div class="col-7">
                                     <telerik:RadComboBox ID="ddlProvider" runat="server" MarkFirstMatch="true" Filter="Contains"
                                            Height="300px" Width="100%"   />

                                </div>
                            </div>
                             <div class="row form-group bg-white p-1">
                                <div class="col-5 label2">
                                    <asp:Label ID="Label2" runat="server" Text="Bed Category" />
                                </div>
                                <div class="col-7">
                                     <telerik:RadComboBox ID="ddlbedcategory" runat="server" MarkFirstMatch="true" Filter="Contains"
                                            Height="300px" Width="100%" />
                                </div>
                            </div>

                           
                               
                        </ContentTemplate>
                    </asp:UpdatePanel>
                  
                    <div class="row form-group bg-white p-1" id="trAdmittingDoctor" runat="server" visible="false">
                        <div class="col-5 label2">
                            <asp:Label ID="ltrladmitingdoctor" runat="server" Text="Advising Doctor"></asp:Label>
                            <span style="color: Red">*</span>
                        </div>
                        <div class="col-7">
                            <telerik:RadComboBox ID="ddladmitingdoctor" runat="server" Width="100%" MarkFirstMatch="true" />
                        </div>
                    </div>
                   
                    <div class="row margin_Top">
                       
                        <div class="col-md-6 m-auto text-center">
                            <asp:Button ID="btnSave" runat="server" Text="Save " ToolTip="Save(Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click"  />
                            <asp:Button ID="btnclose" runat="server" Text="Close " ToolTip="Close(Ctrl+F8)" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="window.close();" />                            
                        </div>
                    </div>
                </div>
            </div>
          
        </div>
        

        <table width="100%" cellpadding="0" cellspacing="0" style="background: #ffffff;">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                Width="1200" Height="600" Left="10" Top="10">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                        Width="900" Height="600" />
                                </Windows>
                            </telerik:RadWindowManager>

                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCompanyCode" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnadmitingdoctor" runat="server" Value="N" />
                            <asp:HiddenField ID="hdnTransferId" runat="server" Value="0" />
                            <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                           
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
