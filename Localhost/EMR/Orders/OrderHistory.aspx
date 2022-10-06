<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="OrderHistory.aspx.cs" Inherits="EMR_Orders_OrderHistory_t" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function ValidateDateRange() {

            try {
                // make sure start date is before the end date

                var date1obj = $get('ctl00_ContentPlaceHolder1_txtFromDate');
                var date2obj = $get('ctl00_ContentPlaceHolder1_txtToDate');

                if (date1obj == null || date2obj == null) {
                    //could not get date object.

                    return true;
                }

                if (date1obj.value.length == 0 || date2obj.value.length == 0) {
                    //required fields.
                    //alert("Dates are required fields.")
                    alert('test')
                    return true;
                }
                else {

                    var fromDate = date1obj.value;
                    var toDate = date2obj.value
                    if (Date.parse(fromDate) > Date.parse(toDate)) {
                        alert("Invalid Date Range!")
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            catch (e) {
                return;
            }
        }
        function SelectHeaderCheckBox(CheckBox, GridName) {
            var TargetBaseControl = document.getElementById(GridName);
            var TargetChildControl = "chkInner";
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var iCount = 1; iCount < Inputs.length; ++iCount) {
                if (Inputs[iCount].type == 'checkbox' && Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0) {
                    Inputs[iCount].checked = CheckBox.checked;
                }
            }
        }

    </script>

    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td class="clsheader" style="padding-left: 10px;" align="left">
                Order History
            </td>
            <td class="clsheader" align="left" style="color: #000000;">
            </td>
            <td class="clsheader">
            </td>
            <td class="clsheader" align="center">
                <asp:Label ID="lblPatientInfo" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            </td>
            <td class="clsheader" align="right">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAddToDay" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td align="right" class="clsheader">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnAddToDay" runat="server" SkinID="Button" Text="Add To Today" Visible="false"
                            OnClick="btnAddToDay_Click" />
                        <asp:Button ID="btnNewOrder" runat="server" Text="Back" SkinID="Button" Visible="false"
                            OnClick="btnNewOrder_OnClick" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <table width="100%" cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td align="left" valign="top">
                            Facility
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel ID="updFacility" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="ddlFacility" runat="server" Width="150px">
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td align="left" valign="top">
                            Provider
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updProvider" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="ddlProvider" runat="server" Width="200px" AppendDataBoundItems="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lableId" runat="server" Text='<%$ Resources:PRegistration, UHID%>'
                                SkinID="label"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtRegno" runat="server" SkinID="textbox" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Visit Type
                        </td>
                        <td>
                            <asp:UpdatePanel ID="updDiagnosis" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="ddlVisitType" SkinID="DropDown" runat="server" Width="100px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                            <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                            <telerik:RadComboBoxItem Text="IPD" Value="I" />
                                            <telerik:RadComboBoxItem Text="ER" Value="E" />
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, MHC%>' Value="M" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td valign="top" align="left">
                            Date Range
                        </td>
                        <td valign="top">
                            <asp:UpdatePanel ID="updDateRange" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="ddldateRange" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td width="10%">
                            <asp:UpdatePanel ID="updaterng" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td align="right" valign="top">
                                                    From
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00"
                                                        Width="100px">
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td align="right" valign="top">
                                                    To
                                                </td>
                                                <td>
                                                    <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100px">
                                                    </telerik:RadDatePicker>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddldateRange" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td width="40%" valign="top">
                            <asp:Button ID="btnRefresh" runat="server" Text="Filter" SkinID="Button" OnClick="btnRefresh_Click"
                                OnClientClick="ValidateDateRange()" />&nbsp;
                            <asp:Button ID="btnClearFilter" runat="server" Text="Clear&nbsp;Filter" SkinID="Button"
                                OnClick="btnClearFilter_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <table width="100%" cellpadding="0" cellspacing="0" style="padding: 3px">
                    <tr>
                        <td style="width: 100%">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvOrderMain" SkinID="gridview" CellPadding="2" runat="server" AutoGenerateColumns="false"
                                        ShowHeader="true" Width="100%" OnRowDataBound="gvOrderMain_RowDataBound" HeaderStyle-HorizontalAlign="Left"
                                        OnSelectedIndexChanged="gvOrderMain_SelectedIndexChanged" AllowPaging="true"
                                        PageSize="8" OnPageIndexChanging="gvOrderMain_PageIndexChanging">
                                        <Columns>
                                            <asp:CommandField SelectText="Select" ShowSelectButton="true" ItemStyle-Width="40px"
                                                HeaderStyle-Width="40px" />
                                            <asp:TemplateField ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderId" runat="server" Text='<%#Eval("OrderId") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, UHID%>' ItemStyle-Width="80px"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Patient Name" ItemStyle-Width="220px" HeaderStyle-Width="220px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Age/Gender" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Visit&nbsp;Date" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitDate" runat="server" Text='<%#Eval("VisitDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Visit Type" ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitType" runat="server" Text='<%#Eval("OPIP") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Facility Name" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Doctor Name" ItemStyle-Width="150px" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Status" ItemStyle-Width="120px" HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order No" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderNo" runat="server" Text='<%#Eval("OrderNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Date" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBillStatus" runat="server" Text='<%#Eval("BillStatus") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MobileNo" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="280px" HeaderStyle-Width="280px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings PageButtonCount="6" />
                                    </asp:GridView>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAddToDay" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <table width="100%" cellpadding="0" cellspacing="0" style="padding: 3px">
                    <tr>
                        <td style="width: 100%">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblTitle" runat="server" Text="Order Details&nbsp;" SkinID="label"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label><asp:Label ID="lblTitleOrderNo" runat="server"
                                            SkinID="label" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    <div style="height: 240px; width: 100%; overflow: auto;">
                                        <asp:GridView ID="gvOrderHistory" SkinID="gridview" CellPadding="2" runat="server"
                                            AutoGenerateColumns="false" ShowHeader="true" Width="99%" OnRowDataBound="gvOrderHistory_RowDataBound"
                                            HeaderStyle-HorizontalAlign="Left" OnSelectedIndexChanging="gvOrderHistory_SelectedIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Dept. Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                        <asp:HiddenField ID="hdnStat" runat="server" Value='<%#Eval("Stat") %>' />
                                                        <asp:HiddenField ID="hdnOrderDate" runat="server" Value='<%#Eval("OrderDate") %>'>
                                                        </asp:HiddenField>
                                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnVisitType" runat="server" Value='<%#Eval("VisitType") %>'>
                                                        </asp:HiddenField>
                                                        <asp:HiddenField ID="hdnFacilityId" runat="server" Value='<%#Eval("FacilityId") %>' />
                                                        <asp:HiddenField ID="hdnServiceStatus" runat="server" Value='<%#Eval("ServiceStatus") %>' />
                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubName" runat="server" Text='<%#Eval("SubName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RefServiceCode" HeaderText="Service Code" ItemStyle-Width="60px"
                                                    HeaderStyle-Width="60px" />
                                                <asp:TemplateField HeaderText="Service&nbsp;Name" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ICD Codes" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("IcdId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Facility Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doctor Name" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("LabStatus") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPreauthorizedNo" runat="server" Text='<%#Eval("PreauthorizedNo") %>'
                                                            SkinID="textbox" BackColor="White" Enabled="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Stat" HeaderText="STAT" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="400px" ItemStyle-Width="400px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' BackColor="White"
                                                            Enabled="false" Height="25px" Width="400px" TextMode="MultiLine"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                                    <asp:AsyncPostBackTrigger ControlID="btnAddToDay" />
                                    <asp:AsyncPostBackTrigger ControlID="gvOrderMain" />
                                    <asp:AsyncPostBackTrigger ControlID="gvOrderHistory" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblColorName" runat="server" Text="Color Legend" Font-Bold="true"
                    SkinID="label"></asp:Label>
                &nbsp;<asp:Label ID="lblCancelColor" runat="server" BackColor="Aqua" Text="Today Order/Service"></asp:Label>
                <asp:HiddenField ID="hdnOrderId" runat="server" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                    Behaviors="Close">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowPopup" runat="server">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
            </td>
        </tr>
    </table>
</asp:Content>
