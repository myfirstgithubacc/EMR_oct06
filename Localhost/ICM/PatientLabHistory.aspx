<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientLabHistory.aspx.cs" Inherits="ICM_PatientLabHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Lab Result History</title>
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />--%>
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.ServiceId = document.getElementById("hdnServiceId").value;
            oArg.LabData = document.getElementById("hdnLabData").value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-lg-3 col-md-4 col-12 text-left">
                            <asp:Label ID="lblPatientName" runat="server" Font-Bold="true" SkinID="label"></asp:Label>
                        </div>
                        <div class="col-lg-5 col-md-4 col-12 text-center">
                            <asp:UpdatePanel ID="upErrorMessage" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdnServiceId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnLabData" runat="server" Value="" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-lg-4 col-md-4 col-12 text-right">
                            <asp:CheckBox ID="chkAllInvestigation" Font-Bold="true" runat="server" Text="All Investigations" SkinID="checkbox" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCloseW" Text="Update Summary" runat="server" CausesValidation="false"
                            ToolTip="Update Dicharge Summary" CssClass="btn btn-primary" OnClick="btnCloseW_Click" />
                            <asp:Button ID="btnSearch" runat="server" ToolTip="Click here to Filter lab history" OnClick="btnSearch_OnClick"
                                Text="Filter" CssClass="btn btn-primary" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary"
                                OnClientClick="window.close();" />
                        </div>
                    </div>
                    <div class="row">
                        <div class=" col-lg-4 col-md-4 col-6 form-group mt-1">
                            <div class="row">
                                <div class="col-lg-4 col-4">
                                    <asp:Label ID="lblReportType" runat="server" Text="For Station" />
                                </div>
                                <div class="col-lg-8 col-8">
                                    <telerik:RadComboBox ID="ddlReportFor" AutoPostBack="true" OnSelectedIndexChanged="ddlReportFor_OnSelectedIndexChanged"
                                        runat="server" SkinID="DropDown" Width="100%" AppendDataBoundItems="true">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-6">
                            <div class="row">
                                <div class=" col-6">
                                    <div class="row">
                                        <div class="col-3">
                                            <asp:Label ID="Label5" runat="server" Text="From" SkinID="label" />
                                        </div>
                                        <div class="col-9 pr-0">
                                            <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="row">
                                        <div class="col-2">
                                            <asp:Label ID="Label6" runat="server" SkinID="label" Text="To" />
                                        </div>
                                        <div class="col-9 pr-0">
                                            <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-6">
                            <div class="row">
                                <div class="col-6" style="white-space: nowrap;">
                                    <asp:CheckBox ID="chkAbnormalValue" runat="server" />
                                    <asp:Label ID="Label2" runat="server" Text="Abnormal Result(s)" ForeColor="DarkViolet"
                                        SkinID="label"></asp:Label>

                                </div>
                                <div class="col-6" style="white-space: nowrap;">
                                    <asp:CheckBox ID="chkCriticalValue" runat="server" />

                                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, PanicResult%>' ForeColor="Red" SkinID="label"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-12">
                            <div class="row">
                                <div class="col-8">
                                    <div class="row">

                                        <div class="col-md-4 col-5">
                                            <asp:Label ID="lblSubDept" runat="server" SkinID="label" Width="100%" Text='<%$ Resources:PRegistration, SubDepartment%>' />
                                        </div>
                                        <div class="col-md-8 col-7">
                                            <telerik:RadComboBox ID="ddlSubDepartment" runat="server" MarkFirstMatch="true" Width="100%"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged"
                                                AppendDataBoundItems="true" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-4">

                                    <telerik:RadComboBox runat="server" ID="ddlService" Width="100%" Height="150px"
                                        EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="Select Service"
                                        DropDownWidth="470px" OnItemsRequested="ddlService_OnItemsRequested" ShowMoreResultsBox="true"
                                        EnableVirtualScrolling="true">
                                        <HeaderTemplate>
                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="width: 150px" align="left">Service Name
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                <table style="width: 440px" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 150px;" align="left">
                                                            <%# DataBinder.Eval(Container, "Attributes['ServiceName']")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>

                                </div>
                            </div>
                        </div>
                    </div>
                <table border="0" cellspacing="0" cellpadding="0" width="100%">


                    <tr>
                        <td colspan="5">
                            <table>
                                <tr>
                                    <td></td>
                                    <td>

                                        <%--EmptyMessage="[Select Sub Department]"--%>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" ControlsToSkip="Buttons"
                    runat="server" DecorationZoneID="dvSearchZone" Skin="Office2007"></telerik:RadFormDecorator>
                <div id="dvSearchZone">
                    <table border="0" cellspacing="1" cellpadding="1" width="100%">
                        <tr>
                            <td style="width: 100%" colspan="2">

                                <telerik:RadComboBox ID="ddlStatus" Visible="false" SkinID="DropDown" runat="server"
                                    EmptyMessage="All" Width="165px" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td align="right">

                                <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                                    Behaviors="Close">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="dvGridZone" runat="server">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">

                        <ContentTemplate>

                            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Office2007" SelectedIndex="0"
                                MultiPageID="RadMultiPage1">
                                <Tabs>
                                    <telerik:RadTab Text="Lab Result" ToolTip="Lab Result">
                                    </telerik:RadTab>
                                    <telerik:RadTab Text="Lab Result ( Discharge Summary ) " ToolTip="Lab Result ( Discharge Summary ) ">
                                    </telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0"
                                ScrollBars="Auto" Width="100%" Style="background: #e0ebfd;">
                                <telerik:RadPageView ID="rpvItem" runat="server" Style="background: #e0ebfd; border-width: 1px">
                                    <table border="0" cellpadding="0" cellspacing="0" style="background: #e0ebfd; border-width: 1px"
                                        width="100%">
                                        <tr>
                                            <td id="tdGrid" runat="server" valign="top" style="width: 100%;">
                                                <telerik:RadGrid ID="gvResultFinal" runat="server" Width="98%" Skin="Office2007"
                                                    BorderWidth="0" AllowFilteringByColumn="false" ShowGroupPanel="false" AllowPaging="true"
                                                    PageSize="15" AllowSorting="true" Height="430px" AllowMultiRowSelection="false"
                                                    AutoGenerateColumns="false" ShowStatusBar="true" AllowCustomPaging="true" OnItemDataBound="gvResultFinal_OnItemDataBound"
                                                    OnItemCommand="gvResultFinal_OnItemCommand" OnPageIndexChanged="gvResultFinal_OnPageIndexChanged">
                                                    <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                                        EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                            AllowColumnResize="false" />
                                                    </ClientSettings>
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">
                                                                No&nbsp;Record&nbsp;Found.
                                                            </div>
                                                        </NoRecordsTemplate>
                                                        <GroupHeaderItemStyle Font-Bold="true" />
                                                        <Columns>
                                                            <%--<telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                                            <telerik:GridTemplateColumn UniqueName="Source" DefaultInsertValue="" HeaderText="Source"
                                                                Visible="true" HeaderStyle-Width="6%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="OrderDate" DefaultInsertValue="" HeaderText="Order Date"
                                                                Visible="True" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" Visible="true"
                                                                HeaderStyle-Width="7%">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblLabHeader" runat="server" Text='<%$ Resources:PRegistration, LABNO%>'></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ipno%>'
                                                                Visible="false" HeaderStyle-Width="8%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                                Visible="false" HeaderStyle-Width="8%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Provider" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, Provider%>'
                                                                Visible="true" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation"
                                                                Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkServiceName" Visible="false" runat="server" Text='<%#Eval("ServiceName") %>'
                                                                        CommandName="Investigation" CommandArgument='<%#Eval("ServiceName") %>' ToolTip="click here to view lab result in graph"
                                                                        ForeColor="Black" />
                                                                    <asp:Label ID="lblServiceName" Visible="true" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="imgViewImage" runat="server" ImageUrl="~/Icons/RIS.jpg" ToolTip="View Scan Image"
                                                            OnClick="imgViewImage_Click" Visible="false" CommandName='<%#Eval("AccessionNo")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                                                                Visible="true" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                                                    <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result"
                                                                        CommandArgument="None" Visible="true" ForeColor="Black"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="print" DefaultInsertValue="" HeaderText="Print"
                                                                Visible="true" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkprint" runat="server" CommandName="Print" CommandArgument="None"
                                                                        Text="Print" ForeColor="Black"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName1" DefaultInsertValue="" HeaderText="RegistrationId"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="AbnormalValue" DefaultInsertValue="" HeaderText="AbnormalValue"
                                                                Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="CriticalValue" DefaultInsertValue="" HeaderText="CriticalValue"
                                                                Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName7" DefaultInsertValue="" HeaderText="Age/Gender"
                                                                AllowFiltering="false" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName10" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName11" DefaultInsertValue="" HeaderText="Sample&nbsp;ID"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName12" DefaultInsertValue="" HeaderText="Status&nbsp;ID"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName15" DefaultInsertValue="" HeaderText="StationId"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStationId" runat="server" Text='<%#Eval("StationId") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="UniqueName14" DefaultInsertValue="" HeaderText="ServiceId"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ResultRemarksId" DefaultInsertValue="" HeaderText="ResultRemarksId"
                                                                AllowFiltering="false" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResultRemarksId" runat="server" Text='<%#Eval("ResultRemarksId") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="StatusCode" DefaultInsertValue="" HeaderText="StatusCode"
                                                                AllowFiltering="false" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Add To Cart" ItemStyle-Width="100px" HeaderStyle-Width="100px"
                                                                AllowFiltering="false">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAddList" runat="server" Text="Add To List"
                                                                        CommandName="AddList" ToolTip="click here Add To List" ForeColor="Black" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="rpvPayment" runat="server">
                                    <table cellpadding="0px" border="0" style="background: #e0ebfd; border-width: 1px"
                                        width="100%">
                                        <tr>
                                            <td>
                                                <telerik:RadGrid ID="gvLabHistoryCart" runat="server" Width="98%" Skin="Office2007"
                                                    BorderWidth="0" AllowFilteringByColumn="false" ShowGroupPanel="false" AllowPaging="false"
                                                    PageSize="15" AllowSorting="true" Height="430px" AllowMultiRowSelection="false"
                                                    AutoGenerateColumns="false" ShowStatusBar="true" AllowCustomPaging="true"
                                                    OnItemCommand="gvLabHistoryCart_OnItemCommand">
                                                    <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                                        EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                            AllowColumnResize="false" />
                                                    </ClientSettings>
                                                    <GroupingSettings CaseSensitive="false" />
                                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                        <NoRecordsTemplate>
                                                            <div style="font-weight: bold; color: Red;">
                                                                No&nbsp;Record&nbsp;Found.
                                                            </div>
                                                        </NoRecordsTemplate>
                                                        <GroupHeaderItemStyle Font-Bold="true" />
                                                        <Columns>

                                                            <telerik:GridTemplateColumn UniqueName="OrderDate" DefaultInsertValue="" HeaderText="Order Date"
                                                                Visible="True" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" Visible="true"
                                                                HeaderStyle-Width="7%">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblLabHeader" runat="server" Text='<%$ Resources:PRegistration, LABNO%>'></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Provider" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, Provider%>'
                                                                Visible="true" HeaderStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation"
                                                                Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceName" Visible="true" runat="server" Text='<%#Eval("ServiceName") %>' />

                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                                                                Visible="true" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblresult" runat="server" Visible="true" Text='<%#Eval("Result") %>' />

                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="ServiceId" DefaultInsertValue="" HeaderText="ServiceId"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn UniqueName="UniqueName11" DefaultInsertValue="" HeaderText="Sample&nbsp;ID"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>

                                                            <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Delete" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete"
                                                                        CommandName="Delete" ToolTip="Delete" ForeColor="Black" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn UniqueName="Source" DefaultInsertValue="" HeaderText="Source"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>


                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                        Behaviors="Close">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowPopup" runat="server">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
