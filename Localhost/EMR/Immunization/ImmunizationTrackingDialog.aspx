<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImmunizationTrackingDialog.aspx.cs"
    Inherits="EMR_Immunization_ImmunizationTrackingDialog" Title="Immunization" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        function returnToParent() {

            var oArg = new Object();

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function CloseScreen() {
            window.close();
            return true;
        }
        function maxLength(txtMaxLength) {
            var txtMaxLength = document.getElementById(txtMaxLength).value;
            if (maxtextbox.value > 100) {
                alert("Maximum Limit is 100");
                return;
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Managerscript" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-md-6 col-sm-6 col-xs-6">
                         <h2><asp:Label ID="Label1" runat="server" Text="Immunization Name :"></asp:Label>&nbsp;
                          <asp:Label ID="ImmName" runat="server"></asp:Label></h2>
                    </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 text-right">
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="CloseScreen();" />
                                <asp:Button ID="btnSave" runat="server" ToolTip="Save" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                        </div>
                        </div>

                <div class="row text-center">
                    <asp:Label ID="lblMessage" runat="server" style="position:relative;width:100%;margin:0px;" Text="&nbsp;"></asp:Label>
                </div>

                    <div class="row">
                        <div class="col-md-3 col-sm-3 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Literal ID="ltrlFacility" runat="server" Text="Facility"></asp:Literal>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadComboBox ID="ddlFacility" runat="server" EmptyMessage="Select" Width="100%"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5 col-sm-5 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <asp:CheckBox ID="ChkPatientorGuardian" runat="server" AutoPostBack="true" OnCheckedChanged="ChkPatientorGuardian_CheckedChanged" />
                                        <asp:Literal ID="ltrlPatientorGuardian" runat="server" Text="Patient or Guardian refuses immunization"></asp:Literal>&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkVaccineGivenByOutsider" runat="server" AutoPostBack="true" OnCheckedChanged="ChkVaccineGivenByOutsider_CheckedChanged" />
                                        <asp:Literal ID="ltrlVaccineGivenByOutsider" runat="server" Text="Vaccine Given Outside"></asp:Literal>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Literal ID="ltrlBrand" runat="server" Text="Brand"></asp:Literal>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadComboBox ID="ddlBrand" runat="server" EmptyMessage="Select" Width="100%"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlBrand_OnSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>
                        
                    </div>
                    <div class="row">
                        <div class="col-md-3 col-sm-3 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                    <asp:Literal ID="ltrlLot" runat="server" Text="Batch No :" />
                                        <span id="spnStarBatchNo" runat="server" class="red">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:TextBox ID="txtBatchNo" runat="server" Width="100%" Style="text-transform: uppercase"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-2 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                   <asp:Literal ID="ltrExpiryDate" runat="server" Text="Expiry Date"></asp:Literal>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                   <telerik:RadDatePicker ID="RadExpiryDate" runat="server" MinDate="01/01/1900" Width="100%" AutoPostBackControl="Both">
                                        <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                    </telerik:RadDatePicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                   <asp:Literal ID="ltrlGivenBy" runat="server" Text="Given By"></asp:Literal>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                   <telerik:RadComboBox ID="ddlProviders" runat="server" EmptyMessage="/Select" Width="100%"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                       <div class="col-md-4 col-sm-4 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                   <asp:Literal ID="ltrlGivenDate" runat="server" Text="Given Date"></asp:Literal>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-7">
                                            <telerik:RadDateTimePicker ID="RadGivenDatetime" runat="server" MinDate="01/01/1900" Width="100%" AutoPostBackControl="Both">
                                        <DateInput DateFormat="dd/MM/yyyy hh:mm"></DateInput>
                                    </telerik:RadDateTimePicker>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-5">
                                            <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="300px" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Width="40%"></telerik:RadComboBox>
                                        
                                            <asp:Literal ID="ltDateTime" runat="server" Text="HH   MM"></asp:Literal>
                                        </div>
                                    </div>
                                   
                                        
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                         
                        <div class="col-md-12 col-sm-12 colxs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-1 col-sm-1 col-xs-3">
                                   <asp:Literal ID="ltrlComments" runat="server" Text="Remarks"></asp:Literal>
                                </div>
                                <div class="col-md-11 col-sm-11 col-xs-9">
                                   <asp:TextBox ID="txtComments" runat="server" Height="55px" MaxLength="100" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        </div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                            <asp:Label ID="lblid" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                    </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
