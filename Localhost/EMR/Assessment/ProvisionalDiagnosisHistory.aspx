<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ProvisionalDiagnosisHistory.aspx.cs" Inherits="EMR_Assessment_ProvisionalDiagnosisHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="aspl" TagName="Left" Src="~/Include/Components/ucTree.ascx" %>
<%@ Register TagPrefix="aspl" TagName="Top" Src="~/Include/Components/MasterComponent/TopPanel.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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


    </script>

    <div>
        <table width="100%" class="clsheader">
            <tr>
                <td>
                Provisional Diagnosis History
                </td>
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSave" runat="server" Text="Add To Today's Diagnosis" SkinID="Button"
                                OnClick="btnSave_Click" />
                            <asp:Button ID="btnBack" runat="server" Text="Back" SkinID="Button" OnClick="btnBack_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table border="0" width="98%" cellpadding="3" cellspacing="0" style="margin-left: 8px;
            margin-right: 8px">
            <tr>
                <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Facility" SkinID="label"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlFacility" runat="server" Width="150px" Skin="Outlook"
                                AutoPostBack="false">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'
                                SkinID="label"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlProvider" runat="server" Width="150px" Skin="Outlook"
                                AutoPostBack="false">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Status" SkinID="label" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlStatus" runat="server" Width="150px" Skin="Outlook" AutoPostBack="false"
                                Visible="false">
                            </telerik:RadComboBox>
                        </td>
                        
                        <td>
                            <asp:Button ID="btnRefresh" runat="server" Text="Filter" SkinID="Button" OnClick="btnRefresh_Click"
                                OnClientClick="ValidateDateRange()" Width="100px" />
                            <asp:Button ID="btnClearFilter" runat="server" Text="Clear&nbsp;Filter" Visible="True"
                                SkinID="Button" OnClick="btnClearFilter_Click" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text=" Visit Type" SkinID="label"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSource" SkinID="DropDown" runat="server" Width="100px"
                                AutoPostBack="false">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" Selected="True" />
                                    <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                    <telerik:RadComboBoxItem Text="IPD" Value="I" />
                                   <%-- <telerik:RadComboBoxItem Text="ER" Value="E" />
                                    <telerik:RadComboBoxItem Text="MHC" Value="M" />--%>
                                    <%-- <telerik:RadComboBoxItem Text="Package" Value="P" />--%>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="  Date Range" SkinID="label"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddldateRange" runat="server" Width="150px" Skin="Outlook"
                                OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Select All" Value="" runat="server" />
                                    <telerik:RadComboBoxItem Text="Today" Value="DD0" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Week" Value="WW-1" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Month" Value="MM-1" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Six Months" Value="MM-6" runat="server" />
                                    <telerik:RadComboBoxItem Text="Last Year" Value="YY-1" runat="server" />
                                    <telerik:RadComboBoxItem Text="Date Range" Value="6" runat="server" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td colspan="4">
                            <asp:UpdatePanel ID="updaterng" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <asp:Label ID="lblFrom" runat="server" SkinID="label" Text="From"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00"
                                                        Width="100px">
                                                    </telerik:RadDatePicker>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTo" runat="server" SkinID="label" Text="To"></asp:Label>
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
                                    <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <style>.rgAdvPart{display:none;}</style>
        <table width="100%" cellpadding="0" cellspacing="0">
            
            <tr>
                <td style="padding-left: 3px;" valign="top">
                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvDiagnosisHistory" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                AllowFilteringByColumn="false" runat="server" Width="98%" AutoGenerateColumns="False"
                                Height="450px" PageSize="12" EnableLinqExpressions="False" AllowPaging="true"
                                AllowMultiRowSelection="true" CellSpacing="0" OnPageIndexChanged="gvDiagnosisHistory_PageIndexChanged"
                               >
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                    Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <PagerStyle ShowPagerText="False" />
                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="View">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="30px" />
                                            <ItemStyle Width="30px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EncounterDate" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Visit Date" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterDate" runat="server" Text='<%#Eval("VisitDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="70px" />
                                            <ItemStyle Width="70px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="VisitType" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Visit Type" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVisitTypeName" runat="server" Text='<%#Eval("VisitType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="60px" />
                                            <ItemStyle Width="60px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EncounterNo" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Enc.#" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                            <ItemStyle Width="50px" />
                                        </telerik:GridTemplateColumn>
                                      
                                        <telerik:GridTemplateColumn UniqueName="DoctorName" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Doctor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="120px" />
                                            <ItemStyle Width="120px" />
                                        </telerik:GridTemplateColumn>
                                     
                                        <telerik:GridTemplateColumn UniqueName="ICDDescription" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Provisional Diagnosis" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblDescription" runat="server" Text='<%#Eval("ProvisionalDiagnosis")%>'
                                                    OnClick="lnkbtnProblem_OnClick" Enabled ="false" style="text-decoration:none;"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" />
                                            <ItemStyle Width="250px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" ShowFilterIcon="false" AutoPostBackOnFilter="false"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                               
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                            <ItemStyle Width="300px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Remarks" AllowFiltering="false" ShowFilterIcon="false"
                                            HeaderText="Remarks" FilterControlWidth="99%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' style="text-decoration:none;"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" />
                                            <ItemStyle Width="250px" />
                                        </telerik:GridTemplateColumn>
                                       
                                      
                                      
                                      
                                     
                                     
                                       
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

