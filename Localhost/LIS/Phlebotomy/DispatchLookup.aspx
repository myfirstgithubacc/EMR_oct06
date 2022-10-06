<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DispatchLookup.aspx.cs" Inherits="LIS_Phlebotomy_DispatchLookup" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "Black";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "Black";
        }

        function ConfirmCancelDispatch() {
            if (confirm("Are you sure you want to cancel dispatch?") == true) {
                return true;
            }
            else {
                return false;
            }
        }
        function Validatetextbox() {

            var ddlSearch = $get('<%=ddlSearch.ClientID%>');
            var txtSearch = $get('<%=txtSearchCretria.ClientID%>');
            if (txtSearch.value != '') {
                if ((ddlSearch.value == 'MR#') || (ddlSearch.value == 'Accession #') || (ddlSearch.value == 'Lab No')) {
                    var n2 = txtSearch.value;
                    txtSearch.value = parseFloat(txtSearch.value);
                    if (txtSearch.value == 'NaN' && n2 != txtSearch.value) {
                        txtSearch.value = '';
                        alert("Invalid Search Criteria!  Please Enter The Numeric Value");
                        return false;
                    }
                }
            }

        }

    </script>

    <style type="text/css">
        .RadGrid_Office2007 .rgSelectedRow
        {
            background-color: #ffcb60 !important;
        }
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { background:#c1e5ef !important; border:1px solid #79abb9 !important;}
       .RadGrid_Office2007 .rgGroupHeader { background: #c1e5ef !important; color:#333;}
       .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol { background: #c1e5ef !important; border-color: #c1e5ef !important;}
        #ctl00_ContentPlaceHolder1_Label3 {float: left; width: 100px;}

        @media (min-width: 992px) and (max-width: 1199px) {
            #ctl00_ContentPlaceHolder1_txtFromDate_dateInput_wrapper,
            #ctl00_ContentPlaceHolder1_txtToDate_dateInput_wrapper {
                min-width: 58px !important;
                float: left !important;
            }

            #ctl00_ContentPlaceHolder1_Label6 {
                width: 15px !important;
                float: left !important;
                margin: 0 !important;
                padding: 0 !important;
            }
        }

    </style>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-3"><h2><asp:Label ID="lblHeader" runat="server" Text="Sample&nbsp;Lookup" /></h2></div>
                <div class="col-md-5 text-center"><asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" /></div>
                <div class="col-md-4 text-right">
                    <asp:HyperLink ID="HyperLink1" Text="Sample&nbsp;Dispatch" CssClass="btn btn-default"
                        Style="text-decoration: none;" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                         runat="server" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Dispatch Cancellation" CssClass="btn btn-primary"
                        OnClick="btnSaveData_OnClick" ValidationGroup="SaveData" Text="Dispatch Cancellation"
                        Visible="true" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="SaveData" />
                </div>
            </div>
            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnRefresh">
                <div class="container-fluid">
                        
                    <div class="row form-group">
                            
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4"><asp:Label ID="Label2" runat="server" Text="Date&nbsp;From" /></div>
                                <div class="col-md-8 PaddingRightSpacing01">
                                    <div class="row">
                                        <div class="col-md-5 PaddingRightSpacing"><telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                        <div class="col-md-1 label2 PaddingCenterSpacing"><asp:Label ID="Label8" runat="server" Text="To" /></div>
                                        <div class="col-md-5 PaddingLeftSpacing"><telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="col-md-2"><asp:Label ID="Label14" runat="server" Text="Station&nbsp;<span style='color: Red'>*</span>" /></div>
                                        <div class="col-md-4">
                                            <telerik:RadComboBox ID="ddlStation" runat="server" Width="100%" MarkFirstMatch="true"
                                                AutoPostBack="true" EmptyMessage="[ Select ]" OnSelectedIndexChanged="ddlStation_SelectedIndexChanged" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStation"
                                                Display="None" ErrorMessage="Select Station !" Text="" ValidationGroup="SaveData" />
                                                
                                                
                                        </div>
                                            
                                        <div class="col-md-2"><asp:Label ID="Label7" runat="server" Text="Entry&nbsp;Site&nbsp;&lt;span style='color: Red'&gt;*&lt;/span&gt;" /></div>
                                        <div class="col-md-4">
                                            <telerik:RadComboBox ID="ddlEntrySites" runat="server" EmptyMessage="[ Select ]" MarkFirstMatch="true" SkinID="DropDown" Width="100%" />
                                        </div>     
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-3 PaddingRightSpacing"><asp:Label ID="Label3" runat="server" Text="Search By" /></div>
                                <div class="col-md-9">
                                    <div class="row">
                                        <div class="col-md-6 PaddingRightSpacing">
                                            <telerik:RadComboBox ID="ddlSearch" EmptyMessage="[ Select ]" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, LABNO%>' Value="LN" Selected="true" />
                                                    <telerik:RadComboBoxItem Text='Manual Lab No' Value="MLN" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-6"><asp:TextBox ID="txtSearchCretria" runat="server" MaxLength="20" Text="" Width="100%" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-2">
                            <div class="row">
                                <div class="col-md-4"><asp:Label ID="Label13" runat="server" Text="Entry&nbsp;Site" /></div>
                                <div class="col-md-8"><telerik:RadComboBox ID="ddlEntrySitesActual" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="true" /></div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-group">

                        <div class="col-md-offset-7 col-md-3 text-right pull-right">
                            <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" OnClick="btnRefresh_OnClick" OnClientClick=" return Validatetextbox()" Text="Refresh" ToolTip="Refresh" />    
                        </div>
                    </div>

                </div>
            </asp:Panel>
            
                <div class="container-fluid">
                    <div class="row form-group">
                        <%--<h2><asp:Label ID="Label4" runat="server" Text="Dispatched Details" /></h2>
                        <br />--%>
                        <asp:Panel ID="PanelN" runat="server" BorderWidth="0" Width="100%" Height="420px"
                            ScrollBars="None">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvDetails" runat="server" Width="100%" Height="420px" AllowPaging="false"
                                        PageSize="10" AllowMultiRowSelection="True" Skin="Office2007" ShowGroupPanel="false"
                                        AutoGenerateColumns="False" GridLines="none" OnPageIndexChanged="gvDetails_PageIndexChanged"
                                        OnItemDataBound="gvDetails_ItemDataBound" OnColumnCreated="gvDetails_ColumnCreated"
                                        OnPreRender="gvDetails_OnPreRender">
                                        <PagerStyle Mode="NextPrevAndNumeric" />
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" Scrolling-AllowScroll="true"
                                            Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="true" />
                                        </ClientSettings>
                                        <MasterTableView Width="100%" TableLayout="Fixed" GroupLoadMode="Client">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important;
                                                    margin: 1em 0; padding: 0; font-size: 11px;">
                                                    No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldAlias=":" FieldName="SubName" HeaderText="" FormatString="" />
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="SubDeptId" SortOrder="None" />
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <Columns>
                                                <telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderStyle-Width="30px" />
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Facility" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Source" AutoPostBackOnFilter="true"
                                                    CurrentFilterFunction="Contains" DataField="Source" ShowFilterIcon="false" AllowFiltering="true"
                                                    FilterControlWidth="99%" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                                    DataField="LabNo" ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%"
                                                    HeaderStyle-Width="13%">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblLabHeader" runat="server" Text='<%$ Resources:PRegistration, LABNO%>'></asp:Label></HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ManualLabNo" DefaultInsertValue="" HeaderText='Manual Lab No'
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                                    FilterControlWidth="99%" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManualLabNo" runat="server" Text='<%#Eval("ManualLabNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="DispatchedDate" DefaultInsertValue="" HeaderText='Dispatched Date'
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                                    FilterControlWidth="99%" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDispatchedDate" runat="server" Text='<%#Eval(" DispatchedDate") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="RegistrationNo"
                                                    ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Patient&nbsp;Name"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="PatientName"
                                                    ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Service&nbsp;Name"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="ServiceName"
                                                    ShowFilterIcon="false" AllowFiltering="true" FilterControlWidth="99%" HeaderStyle-Width="35%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="SampleId" AllowFiltering="false"
                                                    HeaderStyle-Width="100px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiagSampleId" runat="server" Text='<%#Eval("DiagSampleId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="ServiceId" AllowFiltering="false"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, department%>'
                                                    AllowFiltering="false" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubDeptId" runat="server" Text='<%#Eval("SubDeptId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, SubDepartment%>'
                                                    AllowFiltering="false" Visible="false" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubName" runat="server" Text='<%#Eval("SubName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="UStatusColor" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                                                    Visible="false" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                </div>
            

            
                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-md-12"><ucl:legend ID="Legend1" runat="server" /></div>
                    </div>
                </div>
            



        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>