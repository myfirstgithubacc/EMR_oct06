<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RejectedSampleDetails.aspx.cs" Inherits="LIS_Phlebotomy_RejectedSampleDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        tr.clsGridheaderorderNew th, td {
           
            padding: 6px 10px !important;
            white-space: nowrap !important;
        }
         tr.clsGridheaderorderNew th{
              color: #fff !important;
            background: #337ab7 !important;
         }
         td.rcbInputCell.rcbInputCellLeft{
             padding:0!important;
         }
         div#PanelN{
             overflow-x:auto!important;
         }
    </style>
</head>
<script type="text/javascript">
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
    <form id="form1" runat="server" style="overflow: hidden;">
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <div class="container-fluid header_main form-group">
                    <div class="row">
                        <div class="col-md-3 col-sm-4 col-12" id="tdHeader" runat="server">
                            <h2>
                                <asp:Label ID="lblHeader" runat="server" Text="Rejected Sample Details" /></h2>
                        </div>
                        <div class="col-md-6 col-sm-8 col-12 text-center">
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                        </div>
                        <div class="col-md-3 col12 text-right">
                            <asp:Label ID="lblDateRange" CssClass="margin_Top" runat="server" ForeColor="Maroon" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>
                </div>

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 col-12">
                                    <div class="row">
                                        <div class="col-6">
                                            <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" Width="100%" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged">
                                                <Items>
                                                    <%-- <telerik:RadComboBoxItem Text="Ward" Value="W" Selected="true" />--%>
                                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, UHID%>' Value="R" />
                                                    <telerik:RadComboBoxItem Text="Encounter No" Value="ENC" />
                                                    <telerik:RadComboBoxItem Text="Patient Name" Value="P" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-6">
                                            <asp:TextBox ID="txtSearchRegNo" runat="server" Text="" Width="100%" MaxLength="10" onkeyup="return validateMaxLength();" Visible="false" />
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchRegNo" ValidChars="0123456789" />
                                            <asp:TextBox ID="txtSearch" Width="100%" runat="server" MaxLength="30" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-8 mb-1 ">
                                    <div class="row">
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-4">
                                                    <asp:Label ID="lblDate" runat="server" Text="From"></asp:Label>
                                                </div>
                                                <div class="col-8">
                                                    <telerik:RadDatePicker ID="dtpDatefrom" runat="server" MinDate="01/01/1990" Width="100%" TabIndex="5"
                                                        DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="row">
                                                <div class="col-4">
                                                    <span>To</span>
                                                </div>
                                                <div class="col-8">
                                                    <telerik:RadDatePicker ID="dtpDateto" runat="server" MinDate="01/01/1990" Width="100%" TabIndex="5"
                                                        DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                                    </telerik:RadDatePicker>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-4">
                                    <div class="row">
                                        <div class="col-12">
                                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                                            <asp:Button ID="btnACK" runat="server" Text="Acknowledge" CssClass="btn btn-primary" OnClick="btnACK_Click" />
                                            <asp:Button ID="btnIsValidPassword" runat="server" OnClick="btnIsValidPassword_OnClick" CssClass="BillingFullBtn" Text="Search" CausesValidation="false" Style="visibility: hidden;" Width="1" />
                                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%" >
                                <asp:GridView ID="gvDetails" runat="server" SkinID="gridviewOrderNew" Width="100%" AutoGenerateColumns="false" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvDetails_PageIndexChanging" OnRowDataBound="gvDetails_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Facility Name" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFacilityName" runat="server" SkinID="label" Text='<%#Eval("FacilityName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Source" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSource" runat="server" SkinID="label" Text='<%#Eval("Source") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lab No" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabNo" runat="server" SkinID="label" Text='<%#Eval("LabNo") %>' />
                                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%#Eval("EncounterNo") %>' />
                                                <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<%#Eval("DiagSampleId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Text='<%#Eval("RegistrationNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" SkinID="label" Text='<%#Eval("OrderDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Service Name" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceName" runat="server" SkinID="label" Text='<%#Eval("ServiceName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Patient Name" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" SkinID="label" Text='<%#Eval("PatientName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Age / Gender" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeGender" runat="server" SkinID="label" Text='<%#Eval("AgeGender") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bed No" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBedNo" runat="server" SkinID="label" Text='<%#Eval("BedNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ward Name" HeaderStyle-Width="145px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWardName" runat="server" SkinID="label" Text='<%#Eval("WardName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Referred By" HeaderStyle-Width="195px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReferredBy" runat="server" SkinID="label" Text='<%#Eval("ReferredBy") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Whom Informed" HeaderStyle-Width="195px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTowhomInformed" runat="server" SkinID="label" Text='<%#Eval("TowhomInformed") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCancelRemarks" runat="server" Text='<%#Eval("CancelRemarks") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acknowledge By ( Date Time )" HeaderStyle-Width="195px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblACKByName" runat="server" Text='<%#Eval("ACKByName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsACK" Checked='<%#Eval("IsACK") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:UpdatePanel ID="uphidden" runat="server">
                                <ContentTemplate>
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                        Width="1200" Height="600" Left="10" Top="10">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                                Width="900" Height="600" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
