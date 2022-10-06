<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SampleDispatch.aspx.cs" Inherits="LIS_Phlebotomy_SampleDispatch" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    
    

    <script type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtSearchNumeric.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }

        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "Black";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "Black";
        }

        function Validatetextbox() {

            var ddlSearch = $get('<%=ddlSearch.ClientID%>');
            var txtSearch = $get('<%=txtSearchCretria.ClientID%>');

            if (txtSearch.value != '') {
                if (ddlSearch.value == 'Patient Name') {

                    var filter = /^[a-zA-Z-\s]+$/;
                    if (!filter.test(txtSearch.value)) {
                        txtSearch.value = '';
                        alert("Invalid Search Criteria! Please Enter The Character Value");
                        return false;
                    }
                    if (txtSearch.value.length < 3) {
                        alert("Invalid Search Criteria! Please Enter atleast 3 Charachter");
                        return false;
                    }

                }
            }

        }

        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
    </script>

    <style type="text/css">
        .RadGrid_Office2007 .rgSelectedRow
        {
            background-color: #ffcb60 !important;
        }
       .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { background:#c1e5ef !important;border:1px solid #98abb1 !important; border-top:none !important; color: #333 !important; outline:none !important;}
       .RadGrid_Office2007 .rgGroupHeader { background: #c1e5ef !important; color:#333;}
       .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol { background: #c1e5ef !important; border-color: #c1e5ef !important;}
        #ctl00_ContentPlaceHolder1_Label12 { float: left; width: 100px;}
    </style>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
        
        
        
            
            <div class="container-fluid header_main form-group">
                <div class="col-md-3"><h2><asp:Label ID="lblHeader" runat="server" Text="Sample&nbsp;Dispatch" /></h2></div>
                <div class="col-md-5"><asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" /></div>
                <div class="col-md-4 text-right">
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Dispatch Data" OnClick="btnSaveData_OnClick"
                        CssClass="btn btn-primary" Text="Dispatch Data" />&nbsp;
                    <asp:HyperLink ID="HyperLink1" Text="Dispatch&nbsp;Cancellation" Style="text-decoration: none;" CssClass="btn btn-default" onmouseover="LinkBtnMouseOver(this.id);"
                        onmouseout="LinkBtnMouseOut(this.id);" 
                         runat="server" />
                     <%--"/LIS/Phlebotomy/DispatchLookup.aspx?PType=<% common.myStr(Request.QueryString["PType"]);%>"--%>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                        ShowSummary="False" ValidationGroup="SaveData" />
                </div>    
            </div>
            
            
            
            
            
            
            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnFilter">
                <div class="container-fluid">
                    
                    <div class="row form-group">
                        
                        <div class="col-md-3">
                            <div class="row">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="col-md-4"><asp:Label ID="Label1" runat="server" Text="Station&nbsp;<span style='color: Red'>*</span>" /></div>
                                        <div class="col-md-8">
                                            <telerik:RadComboBox ID="ddlStation" SkinID="DropDown" runat="server" AutoPostBack="true"
                                                EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlStation_SelectedIndexChanged" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlStation"
                                            ValidationGroup="SaveData" Display="None" runat="server" ErrorMessage="Select Station !"
                                            Text="" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        
                        <div class="col-md-3" runat="server" visible="false">
                            <div class="row">
                                <div class="col-md-4"><asp:Label ID="lblEntrySites" runat="server" Text="Entry&nbsp;Site&nbsp;&nbsp;<span style='color: Red'>*</span>" /></div>
                                <div class="col-md-8"><telerik:RadComboBox ID="ddlEntrySites" runat="server" Width="100%" MarkFirstMatch="true" /></div>
                            </div>
                        </div>
                        
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-5 PaddingRightSpacing"><asp:Label ID="Label12" runat="server" Text='<%$ Resources:PRegistration, SubDepartment%>' /></div>
                                <div class="col-md-7"><telerik:RadComboBox ID="ddlSubDepartment" runat="server" MarkFirstMatch="true" EmptyMessage="[All Sub Departments]" Width="100%" /></div>
                            </div>
                        </div>
                        
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4 PaddingRightSpacing"><asp:Label ID="Label8" runat="server" Text="Search By" /></div>
                                <div class="col-md-8">
                                    <div class="row">
                                        <div class="col-md-7">
                                            <telerik:RadComboBox ID="ddlSearch" SkinID="DropDown" EmptyMessage="[ Select ]" Width="100%"
                                                runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, LABNO%>' Value="LN" Selected="true"/>
                                                    <telerik:RadComboBoxItem Text='Manual Lab No' Value="MLN" />
                                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="RN"/>
                                                    <%-- <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, ipno%>' Value="IP"  />--%>
                                                    <telerik:RadComboBoxItem Text="Patient Name" Value="PN" />
                                                    <telerik:RadComboBoxItem Text="Bed No." Value="BN" />
                                                    <telerik:RadComboBoxItem Text="Ward Name" Value="WN" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-5 PaddingLeftSpacing">
                                            <asp:TextBox ID="txtSearchCretria" Width="100%" runat="server" Text="" MaxLength="20"
                                                Visible="false" />
                                            <asp:TextBox ID="txtSearchNumeric" Width="100%" runat="server" Text="" MaxLength="13"
                                                onkeyup="return validateMaxLength();" />
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                FilterType="Custom" TargetControlID="txtSearchNumeric" ValidChars="0123456789" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row form-group">
                        <div class="col-md-12 text-right">
                            <asp:Button ID="btnFilter" runat="server" Text="Refresh" CssClass="btn btn-primary"
                                OnClientClick="return Validatetextbox()" OnClick="btnFilter_OnClick" />
                        </div>
                    </div>
                </div>
                
            </asp:Panel>
            
            <div class="container-fluid">
                   
                    <div class="row form-group">
                        <asp:Panel ID="PanelN" runat="server" Width="100%" Height="430px" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvDetails" runat="server" Width="100%" Height="430px" AllowPaging="false"
                                        PageSize="10" AllowMultiRowSelection="True" Skin="Office2007" ShowGroupPanel="false"
                                        AutoGenerateColumns="False" GridLines="none" OnItemCommand="gvDetails_ItemCommand"
                                        OnPageIndexChanged="gvDetails_PageIndexChanged" OnItemDataBound="gvDetails_ItemDataBound"
                                        OnColumnCreated="gvDetails_ColumnCreated" OnPreRender="gvDetails_OnPreRender">
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
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">
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
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Source" DataField="Source"
                                                    HeaderStyle-Width="6%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                        <asp:HiddenField ID="hdnStationId" runat="server" Value='<%#Eval("StationId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, LABNO%>'
                                                    DataField="LABNO" HeaderStyle-Width="12%">
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
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                    DataField="regno" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Patient&nbsp;Name"
                                                    DataField="PatientName" HeaderStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Service&nbsp;Name"
                                                    DataField="ServiceName" HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="SampleId" HeaderStyle-Width="100px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiagSampleId" runat="server" Text='<%#Eval("DiagSampleId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="ServiceId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, department%>'
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubDeptId" runat="server" Text='<%#Eval("SubDeptId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, SubDepartment%>'
                                                    HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubName" runat="server" Text='<%#Eval("SubName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="UStatusColor" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                                                    Visible="false">
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
                        
                    <div class="row form-group">
                        <div class="col-md-12">
                            <ucl:legend ID="Legend1" runat="server" />
                        </div>
                    </div>    
                        
                    <div class="row form-group">    
                        <div class="col-md-12">
                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                Behaviors="Close,Move,Pin,Resize,Maximize">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </div>   
                    </div>    
                        
                </div>
                
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
