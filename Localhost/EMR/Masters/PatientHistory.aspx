<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PatientHistory.aspx.cs" Inherits="LIS_Phlebotomy_PatientHistory" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtRegNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }
        function ddlServiceNameOnClientItemChecked(sender, eventArgs) {
            $get('<%=btnCallcombo.ClientID%>').click();
        }

        function showResultPopup(e, sDiagSampleId, sServiceId, sSource, sStatusCode, sServiceName, sAgeGender, sFieldId, sType, sCellId, sRowId) {
            $get('<%=hdnsDiagSampleId.ClientID%>').value = sDiagSampleId;
            $get('<%=hdnsServiceId.ClientID%>').value = sServiceId;
            $get('<%=hdnsSource.ClientID%>').value = sSource;
            $get('<%=hdnsStatusCode.ClientID%>').value = sStatusCode;
            $get('<%=hdnsServiceName.ClientID%>').value = sServiceName;
            $get('<%=hdnsAgeGender.ClientID%>').value = sAgeGender;
            $get('<%=hdnsFieldId.ClientID%>').value = sFieldId;
            $get('<%=hdnsType.ClientID%>').value = sType;
            $get('<%=hdnsCellId.ClientID%>').value = sCellId;
            $get('<%=hdnsRowId.ClientID%>').value = sRowId;
            $get('<%=btnCallPopup.ClientID%>').click();
        }

        function SearchOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=btnFilterView.ClientID%>').click();
        }
    </script>

    <div class="VisitHistoryDiv">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-3 col-sm-3">
                    <div class="WordProcessorDivText">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="Patient Lab Result History"></asp:Label></h2>
                    </div>
                </div>
                <div class="col-md-7 col-sm-7">
                    <asp:Label ID="lblMessage" CssClass="PatientMessageText" runat="server"></asp:Label>
                </div>
                <div class="col-md-2 col-sm-2">
                    <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="PatientBtn01" ToolTip="Close"
                        OnClientClick="window.close();" Visible="false" />
                    <asp:Button ID="btnSearch" runat="server" ToolTip="Click here to refresh grid" OnClick="btnSearch_OnClick"
                        Text="Refresh" CssClass="PatientBtn01" />
                </div>
            </div>
        </div>
    </div>
    <div class="VisitHistoryBorder">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12" id="pdetails" runat="server">
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" ControlsToSkip="Buttons"
        runat="server" DecorationZoneID="dvSearchZone" Skin="Office2007" />
    <div id="dvSearchZone">
        <div class="VitalHistory-Div02">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="PatientHistoryDiv">
                                    <h2>Duration</h2>
                                    <h3>
                                        <span class="PatientHistoryDivRadio"></span>
                                        <asp:RadioButtonList ID="rbtnSearh" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnSearh_SelectedIndexChanged"
                                            AutoPostBack="True" ForeColor="Navy">
                                            <asp:ListItem Value="MM-1">&nbsp;1 Month&nbsp;&nbsp;</asp:ListItem>
                                            <asp:ListItem Value="MM-3">&nbsp;3 Months&nbsp;&nbsp;</asp:ListItem>
                                            <asp:ListItem Value="MM-6">&nbsp;6 Months&nbsp;&nbsp;</asp:ListItem>
                                            <asp:ListItem Value="YY-1" Selected="True">&nbsp;1 Year&nbsp;&nbsp;</asp:ListItem>
                                            <asp:ListItem Value="DateRange">&nbsp;Date Range&nbsp;&nbsp;</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </h3>
                                    <%-- <h3><input type="radio" class="PatientHistoryDivRadio" /><span>1 Month</span></h3>
                                <h3><input type="radio" class="PatientHistoryDivRadio" /><span>3 Months</span></h3>
                                <h3><input type="radio" class="PatientHistoryDivRadio" /><span>6 Months</span></h3>
                                <h3><input type="radio" class="PatientHistoryDivRadio" /><span>12 Months</span></h3>
                                <h3><input type="radio" class="PatientHistoryDivRadio" /><span>Date Rage </span></h3>--%>
                                    <h4>
                                        <asp:Label ID="lblFromDate" runat="server" Text="From" /></h4>
                                    <h6>
                                        <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="97px" DateInput-ReadOnly="true" />
                                    </h6>
                                    <h5>
                                        <asp:Label ID="lblToDate" runat="server" Text="To" /></h5>
                                    <h6>
                                        <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="97px" DateInput-ReadOnly="true" />
                                    </h6>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="PatientHistoryDiv01">
                            <h2>
                                <asp:Label ID="lblServiceName" runat="server" Text="Lab Service" /></h2>
                            <telerik:RadComboBox ID="ddlServiceName" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                                EmptyMessage="Select Service" AppendDataBoundItems="true" DropDownWidth="300px"
                                Width="165px" />
                        </div>
                        <div class="PatientHistoryDiv02">
                            <span class="PatientHistoryCheckBox">
                                <asp:CheckBox ID="chkAbnormalValue" runat="server" /></span> <span class="PatientHistoryCheckText">
                                    <asp:Label ID="Label2" runat="server" Text="Abnormal&nbsp;Result(s)" ForeColor="DarkViolet"
                                        SkinID="label" /></span> <span class="PatientHistoryCheckBox01">
                                            <asp:CheckBox ID="chkCriticalValue" runat="server" /></span> <span class="PatientHistoryCheckText01">
                                                <asp:Label ID="Label1" runat="server" Text="Critical&nbsp;Result(s)" ForeColor="Red"
                                                    SkinID="label" /></span>
                        </div>
                        <div class="PatientHistoryDiv03">
                            <h3>
                                <asp:RadioButtonList ID="rdbTestaxis" runat="server" RepeatDirection="Horizontal"
                                    OnSelectedIndexChanged="rdbTestaxis_SelectedIndexChanged" AutoPostBack="True"
                                    ForeColor="Navy">
                                    <asp:ListItem Value="YA" Selected="True">Test on Y-axis</asp:ListItem>
                                    <asp:ListItem Value="XA">Test on X-axis&nbsp;</asp:ListItem>
                                </asp:RadioButtonList>
                            </h3>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <table border="0" cellspacing="0" cellpadding="2">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Facility" Visible="false" />
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlFacility" SkinID="DropDown" runat="server" Width="200px"
                        AppendDataBoundItems="true" Visible="false">
                        <Items>
                            <telerik:RadComboBoxItem Text="All" Value="0" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td>
                    <asp:Label ID="lblReportType" runat="server" Text="For&nbsp;Station" Visible="false" />
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlReportFor" runat="server" SkinID="DropDown" Visible="false"
                        Width="180px" AppendDataBoundItems="true">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <table cellspacing="1" cellpadding="0">
                        <tr>
                            <td>
                                <asp:Label ID="LblSearch" runat="server" SkinID="label" Text="Search&nbsp;By" Visible="false" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" SkinID="DropDown" Width="65px"
                                    AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlSearchCriteria_OnSelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, ipno%>' Value="1" />
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="2" Selected="true" />
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegNo" runat="server" MaxLength="13" Width="75px" SkinID="textbox"
                                    AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged" Visible="false" onkeyup="return validateMaxLength();" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                <asp:TextBox ID="txtEncNo" runat="server" MaxLength="10" Width="80px" SkinID="textbox"
                                    AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Result&nbsp;View" SkinID="label" Visible="false" />
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlView" runat="server" SkinID="DropDown" Width="180px"
                        AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlView_OnSelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Text="Grid View" Value="GV" Selected="true" />
                            <telerik:RadComboBoxItem Text="Test (X-axis) and Date (Y-axis)" Value="XA" />
                            <telerik:RadComboBoxItem Text="Test (Y-axis) and Date (X-axis)" Value="YA" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td colspan="2">
                    <asp:Label ID="lblPatientName" runat="server" Font-Bold="true" SkinID="label" />
                    <asp:Button ID="btnCustomView" runat="server" SkinID="Button" Text="Customized View"
                        OnClick="btnCustomView_OnClick" Visible="false" />
                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Skin="Metro"
                        Behaviors="Close">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" />
                        </Windows>
                    </telerik:RadWindowManager>
                </td>
            </tr>
        </table>
    </div>
    <div class="ImmunizationDD-Div">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-12" id="dvGridZone" runat="server">
                    <asp:Panel ID="pnl" runat="server" BorderWidth="1px" Width="100%" Height="468" ScrollBars="Auto">
                        <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" Height="465" Skin="Office2007"
                            BorderWidth="0" AllowFilteringByColumn="false" ShowGroupPanel="false" AllowPaging="true"
                            PageSize="15" AllowSorting="true" AllowMultiRowSelection="false" AutoGenerateColumns="false"
                            ShowStatusBar="true" AllowCustomPaging="true" OnItemDataBound="gvResultFinal_OnItemDataBound"
                            OnItemCommand="gvResultFinal_OnItemCommand" OnPageIndexChanged="gvResultFinal_OnPageIndexChanged"
                            Visible="false">
                            <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red; margin: 2em 0 0 0; padding: 0; text-align: center;">
                                        No&nbsp;Record&nbsp;Found.
                                    </div>
                                </NoRecordsTemplate>
                                <GroupHeaderItemStyle Font-Bold="true" />
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="Source" DefaultInsertValue="" HeaderText="Source"
                                        HeaderStyle-CssClass="PatientPopHeight" Visible="true" HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSource" CssClass="PatientPopHeight" runat="server" Text='<%#Eval("Source") %>' />
                                            <asp:HiddenField ID="hdnResultHTML" runat="server" Value='<%#Eval("ResultHTML") %>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                            <asp:HiddenField ID="hdnReviewedStatus" runat="server" Value='<%#Eval("ReviewedStatus") %>' />
                                            <asp:HiddenField ID="hdnReviewedComments" runat="server" Value='<%#Eval("ReviewedComments") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="OrderDate" DefaultInsertValue="" HeaderText="Order Date"
                                        Visible="True" HeaderStyle-Width="100px">
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
                                    <telerik:GridTemplateColumn UniqueName="ManualLabNo" Visible="true" HeaderStyle-Width="90px">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblManualLabHeaer" runat="server" Text="Manual Lab No"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblManualLabNo" runat="server" Text='<%#Eval("ManualLabNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ipno%>'
                                        Visible="True" HeaderStyle-Width="8%">
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
                                        HeaderStyle-Width="200px" ItemStyle-Width="200px" Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkServiceName" runat="server" Text='<%#Eval("ServiceName") %>'
                                                CommandName="Investigation" CommandArgument='<%#Eval("ServiceName") %>' ToolTip="click here to view lab result in graph"
                                                ForeColor="Black" />
                                            &nbsp;&nbsp;
                                            <asp:ImageButton ID="imgViewImage" runat="server" ImageUrl="~/Icons/RIS.jpg" ToolTip="View Scan Image"
                                                OnClick="imgViewImage_Click" Visible="false" CommandName='<%#Eval("AccessionNo")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                                        Visible="true" ItemStyle-Width="150px" HeaderStyle-Width="150px">
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
                                                Text="Print" ForeColor="Black" />
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
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <asp:GridView ID="gvLabDetailsXaxis" runat="server" AutoGenerateColumns="true" Width="100%"
                            SkinID="gridview2" HeaderStyle-Font-Size="7px" OnRowDataBound="gvLabDetailsXaxis_OnRowDataBound"
                            RowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" />
                    </asp:Panel>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Skin="Metro"
                        Behaviors="Close">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowPopup" runat="server" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:Button ID="btnCallcombo" runat="server" Style="visibility: hidden;" OnClick="btnCallcombo_OnClick" />
                    <asp:Button ID="btnCallPopup" runat="server" Style="visibility: hidden;" OnClick="btnCallPopup_OnClick" />
                    <asp:Button ID="btnFilterView" runat="server" Style="visibility: hidden;" OnClick="btnFilterView_OnClick" />
                    <asp:HiddenField ID="hdnsStatusCode" runat="server" />
                    <asp:HiddenField ID="hdnsDiagSampleId" runat="server" />
                    <asp:HiddenField ID="hdnsServiceId" runat="server" />
                    <asp:HiddenField ID="hdnsSource" runat="server" />
                    <asp:HiddenField ID="hdnsServiceName" runat="server" />
                    <asp:HiddenField ID="hdnsAgeGender" runat="server" />
                    <asp:HiddenField ID="hdnsFieldId" runat="server" />
                    <asp:HiddenField ID="hdnsType" runat="server" />
                    <asp:HiddenField ID="hdnsCellId" runat="server" />
                    <asp:HiddenField ID="hdnsRowId" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
