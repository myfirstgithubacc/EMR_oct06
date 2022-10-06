<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PatientDashboardNew.aspx.cs" Inherits="EMR_Dashboard_PatientDashboardNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Src="~/Include/Components/PatientQView.ascx" TagName="PatientQView" TagPrefix="ucPatientDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<style type="text/css">
    .Labelheader
    {
	    color: Black;
	    font-family: arial;
	    height: 14px;
	    font-weight: bold;
	    font-size: 12px;
	    background-image: url(/Images/coversheet.gif);
	    background-repeat: repeat-x;
	    margin-bottom: 0px;
	    letter-spacing: 1px;
	    vertical-align: middle;
    }
    </style>
    <script type="text/javascript">

        function addChiefComplaintsOnClientClose(oWnd, args) {
            $get('<%=btnAddChiefComplaintsClose.ClientID%>').click();
        }
        function addAllergiesOnClientClose(oWnd, args) {
            $get('<%=btnAddAllergiesClose.ClientID%>').click();
        }
        function addVitalsOnClientClose(oWnd, args) {
            $get('<%=btnAddVitalsClose.ClientID%>').click();
        }

        function addTemplatesOnClientClose(oWnd, args) {
            $get('<%=btnAddTemplatesClose.ClientID%>').click();
        }
        function addDiagnosisOnClientClose(oWnd, args) {
            $get('<%=btnAddDiagnosisClose.ClientID%>').click();
        }
        function addOrdersAndProceduresOnClientClose(oWnd, args) {
            $get('<%=btnAddOrdersAndProceduresClose.ClientID%>').click();
        }
        function addPrescriptionsOnClientClose(oWnd, args) {
            $get('<%=btnAddPrescriptionsClose.ClientID%>').click();
        }
        
    </script>

    <script type="text/javascript">
        //<![CDATA[
        /***********************************************
        Splitter examples
        ***********************************************/
        var isSplitterResized = false;
        function ResizeSplitter(delta) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            if (isSplitterResized) {
                delta *= -1;
            }
            var newWidth = splitter.get_width() + delta;
            isSplitterResized = !isSplitterResized;
            splitter.resize(newWidth, null);
        }

        var SplitterResizeModes = ['AdjacentPane', 'Proportional', 'EndPane'];
        var resizeModeInt = 0;
        function ChangeSplitterResizeMode() {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            if (!SplitterResizeModes[resizeModeInt]) resizeModeInt = 0;
            splitter.set_resizeMode(Telerik.Web.UI.SplitterResizeMode[SplitterResizeModes[resizeModeInt]]);

            alert('Resize Mode set to [' + SplitterResizeModes[splitter.get_resizeMode() - 1] + ']');
            resizeModeInt++;
        }

        /***********************************************
        Pane examples
        ***********************************************/
        function PrintPane(paneID) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(paneID);

            if (!pane) return;
            var cssFileAbsPath = location.href.substr(0, location.href.toString().lastIndexOf('/') + 1) + 'printStyles.css';
            var arrExtStylsheetFiles = [
                              cssFileAbsPath
                         ];

            pane.print(arrExtStylsheetFiles);

        }

        function ScrollPane(paneID, scrollX, scrollY) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(paneID);

            if (!pane) return;

            pane.setScrollPos(scrollX, scrollY);
        }

        function GetPaneState(paneID) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(paneID);

            if (!pane) return;

            return pane.get_clientState();
        }

        function ToggleCollapsePane(paneID) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(paneID);

            if (!pane) return;

            if (pane.get_collapsed()) {
                pane.expand();
            }
            else {
                pane.collapse();
            }
        }

        function ToggleCollapseEndPane() {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var endPane = splitter.getEndPane();
            if (endPane.get_collapsed()) {
                endPane.expand(Telerik.Web.UI.SplitterDirection.Backward);
            }
            else {
                endPane.collapse(Telerik.Web.UI.SplitterDirection.Backward);
            }
        }

        var paneResized = false;
        function ResizePane(delta, paneID) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(paneID);
            if (!pane) return;
            if (paneResized) delta *= -1;
            paneResized = !paneResized;
            pane.resize(delta);
        }

        function LoadExternalContent(url, targetPaneID) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(targetPaneID);
            if (!pane) return;
            pane.set_contentUrl(url);
        }

        function TogglePaneLock(paneID) {
            var splitter = $find("<%=RadSplitter1.ClientID%>");
            var pane = splitter.getPaneById(paneID);
            if (!pane) return;
            if (pane.get_locked()) {
                pane.set_locked(false);
            }
            else {
                pane.set_locked(true);
            }
        }
        //]]>
    </script>

    <script type="text/javascript">

        function setVitalValue(val, valName) {

            $get('<%=hdnVitalvalue.ClientID%>').value = val;
            $get('<%=hdnVitalName.ClientID%>').value = valName;

            var oWnd = radopen("/EMR/Vitals/Vitalgraph.aspx?Value=" + $get('<%=hdnVitalvalue.ClientID%>').value +
                                    "&Name=" + $get('<%=hdnVitalName.ClientID%>').value, "RadWindowForNew");

            oWnd.setSize(1000, 620)
            oWnd.center();
            oWnd.VisibleStatusbar = "false";
            oWnd.set_status(""); // would like to remove statusbar, not just blank it
        }

    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr class="clsheader">
                    <td id="tdHeader" align="left" style="padding-left: 10px; width: 170px" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Patient&nbsp;Dashboard" />
                    </td>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Date&nbsp;Range&nbsp;" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlTime" runat="server" Width="120px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Complete History" Value="" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Today" Value="DD0" />
                                            <telerik:RadComboBoxItem Text="Last Week" Value="WW-1" />
                                            <telerik:RadComboBoxItem Text="Last Month" Value="MM-1" />
                                            <telerik:RadComboBoxItem Text="Last Year" Value="YY-1" />
                                            <telerik:RadComboBoxItem Text="Date Range" Value="4" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td id="tdDateRange" runat="server" visible="false">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="&nbsp;From&nbsp;" />
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="110px" DateInput-ReadOnly="true" />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" SkinID="label" Text="&nbsp;To&nbsp;" />
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="110px" DateInput-ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <telerik:RadComboBox runat="server" ID="ComboPatientSearch" Width="250px" Height="150px"
                            EnableLoadOnDemand="true" HighlightTemplatedItems="true" EmptyMessage="Encounter No, Provider name"
                            DropDownWidth="460px" OnItemsRequested="RadComboBoxProduct_ItemsRequested">
                            <HeaderTemplate>
                                <table style="width: 450px" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 150px" align="left">
                                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="Date" />
                                        </td>
                                        <td style="width: 150px" align="left">
                                            <asp:Label ID="Label4" runat="server" SkinID="label" Text="Encounter No" />
                                        </td>
                                        <td style="width: 150px" align="left">
                                            <asp:Label ID="Label5" runat="server" SkinID="label" Text="Provider Name" />
                                        </td>
                                        <td style="width: 150px" align="left">
                                            <asp:Label ID="Label6" runat="server" SkinID="label" Text="Facility" />
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 450px" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 150px;" align="left">
                                            <%# DataBinder.Eval(Container, "Text")%>
                                        </td>
                                        <td style="width: 150px;" align="left">
                                            <%# DataBinder.Eval(Container, "Attributes['EncounterNo']")%>
                                        </td>
                                        <td style="width: 150px">
                                            <%# DataBinder.Eval(Container, "Attributes['DcotorName']")%>
                                        </td>
                                        <td style="width: 150px">
                                            <%# DataBinder.Eval(Container, "Attributes['FacilityName']")%>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                        &nbsp;
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Button ID="btnFilter" runat="server" ToolTip="Filter Data" OnClick="btnFilter_OnClick"
                                        SkinID="Button" Text="Filter" />
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnClearFilter" runat="server" SkinID="Button" ToolTip="Clear Filter"
                                        Text="Clear Filter" OnClick="btnClearFilter_OnClick" />
                                    &nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnAttachment" runat="server" Text="Attachment(s)" SkinID="Button"
                                        OnClick="btnAttachment_OnClick" />&nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                        OnClientClick="window.close();" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table id="tblPatientDetails" width="100%" border="0" style="background: #F5DEB3;
                margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px;
                border-color: #808080;" cellpadding="0" cellspacing="0" align="left">
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="Patient:" Font-Bold="true" />
                        <asp:Label ID="lblPatientName" runat="server" SkinID="label" Text="" ForeColor="#990066"
                            Font-Bold="true" />
                        <asp:Label ID="Label7" runat="server" SkinID="label" Text="Reg#:" Font-Bold="true" />
                        <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Text="" ForeColor="#990066"
                            Font-Bold="true" />
                        <asp:Label ID="Label9" runat="server" SkinID="label" Text="Enc#:" Font-Bold="true" />
                        <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text="" ForeColor="#990066"
                            Font-Bold="true" />
                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="DOB:" Font-Bold="true" />
                        <asp:Label ID="lblDOB" runat="server" SkinID="label" Text="" ForeColor="#990066"
                            Font-Bold="true" />
                        <asp:Label ID="Label8" runat="server" SkinID="label" Text="Mobile No.:" Font-Bold="true" />
                        <asp:Label ID="lblMobileNo" runat="server" SkinID="label" Text="" ForeColor="#990066"
                            Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="3" align="center" style="color: green; font-size: 12px; font-weight: bold;">
                        <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel9" runat="server" Width="99%" Height="510px" Style="border-width: 1px; border-style: solid;
                border-color: SkyBlue">
                <telerik:RadSplitter ID="RadSplitter1" runat="server" Width="99%" Height="100%">
                    <telerik:RadPane ID="LeftPane" runat="server" Height="100%" Width="240px">
                        <table id="tblEncounters" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0"
                            style="margin-left: 2px">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" style="vertical-align: text-top; height: 90px;
                                        width: 100%;">
                                        <tr>
                                            <td>
                                                <ucPatientDetails:PatientQView ID="patientQV" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="Labelheader">
                                    <table width="100px" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label17" runat="server" SkinID="label" Text="&nbsp;Encounters" />
                                            </td>
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto">
                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvEncounters" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                    ShowHeader="true" AllowPaging="false" Width="230px" PageSize="10" OnRowCommand="gvEncounters_OnRowCommand"
                                                    OnRowDataBound="gvEncounters_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Enc# / Date" HeaderStyle-Width="20px" ItemStyle-Width="20px" >
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEncounterNo" runat="server" CommandName="SelectEncounter"
                                                                    CommandArgument='<%#Eval("EncounterNo") %>' Text='<%#Eval("EncounterNo") %>'
                                                                    CausesValidation="false" Font-Underline="true"/>
                                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("ID") %>' />
                                                                <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                                <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP") %>' />
                                                                <asp:HiddenField ID="hdnEncDate" runat="server"  Value='<%#Eval("EncDate") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Doctor">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDoctor" runat="server" SkinID="label" Text='<%#Eval("Doctor") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="8px" ItemStyle-Width="8px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBtnCaseSheet" runat="server" ImageUrl="~/Images/VisitDetail.png"
                                                                    CommandName="CaseSheet" ToolTip="Case Sheet" Height="17px" Width="15px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Forward" />
                    <telerik:RadPane ID="RightPane1" runat="server">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table id="tblChiefComplaints" runat="server" width="99%" border="0" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label10" runat="server" SkinID="label" Text="&nbsp;Chief&nbsp;Complaints&nbsp;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="imgBtnAddChiefComplaints" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Chief Complaints" Height="17px" Width="16px" OnClick="lnkAddChiefComplaints_OnClick" />
                                                            <%--<asp:LinkButton ID="lnkAddChiefComplaints" runat="server" Text="Add New" OnClick="lnkAddChiefComplaints_OnClick" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvChiefComplaints" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="false" Width="100%" AllowPaging="true" PageSize="4" OnRowCommand="gvChiefComplaints_OnRowCommand"
                                                                OnRowDataBound="gvChiefComplaints_RowDataBound" OnPageIndexChanging="gvChiefComplaints_PageIndexChanging">
                                                                
                                                                <Columns>
                                                                
                                                                    <asp:TemplateField HeaderStyle-Width="100%" HeaderStyle-Height="1px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text="" />
                                                                            <asp:HiddenField ID="hdnProblemDescription" runat="server" Value='<%#Eval("ProblemDescription")%>' />
                                                                            <asp:HiddenField ID="hdnEncodedDate" runat="server" Value='<%#Eval("EncodedDate")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblVitals" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 40px">
                                                            <asp:Label ID="Label14" runat="server" SkinID="label" Text="&nbsp;Vitals&nbsp;" />
                                                        </td>
                                                        <td style="width: 30px">
                                                            <asp:ImageButton ID="imgBtnAddVitals" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Vitals" Height="17px" Width="16px" OnClick="lnkAddVitals_OnClick" />
                                                            <%--<asp:LinkButton ID="lnkAddVitals" runat="server" Text="Add New" OnClick="lnkAddVitals_OnClick" />--%>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="Label18" runat="server" SkinID="label" Text="Note: Click on value of vital to view vital graph" />
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvVitals" SkinID="gridview" runat="server" AutoGenerateColumns="true"
                                                                ShowHeader="true" Width="100%" AllowPaging="true" PageSize="4" 
                                                                OnRowDataBound="gvVitals_RowDataBound" OnPageIndexChanging="gvVitals_PageIndexChanging" />
                                                            <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                                                            <asp:HiddenField ID="hdnVitalName" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblAllergies" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label11" runat="server" SkinID="label" Text="&nbsp;Allergies&nbsp;" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgBtnAddAllergies" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Allergies" Height="17px" Width="16px" OnClick="lnkAddAllergies_OnClick" />
                                                            <%--<asp:LinkButton ID="lnkAddAllergies" runat="server" Text="Add New" OnClick="lnkAddAllergies_OnClick" />--%>
                                                            &nbsp;
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lblAllergiesEmptyData" runat="server" SkinID="label" Text="No Allergies"
                                                                Style="font-weight: bold; color: Red;" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvAllergies" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="false" Width="100%" AllowPaging="true" PageSize="4" OnRowCommand="gvAllergies_OnRowCommand"
                                                                OnRowDataBound="gvAllergies_RowDataBound" OnPageIndexChanging="gvAllergies_PageIndexChanging">
                                                               
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="100%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text="" />
                                                                            <asp:HiddenField ID="hdnAllergyName" runat="server" Value='<%#Eval("AllergyName")%>' />
                                                                            <asp:HiddenField ID="hdnAllergyType" runat="server" Value='<%#Eval("AllergyType")%>' />
                                                                            <asp:HiddenField ID="hdnReaction" runat="server" Value='<%#Eval("Reaction")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblTemplates" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label19" runat="server" SkinID="label" Text="&nbsp;Examinations&nbsp;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="imgBtnTemplates" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Templates" Height="17px" Width="16px" OnClick="lnkAddTemplates_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel10" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvTemplates" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="true" Width="100%" AllowPaging="true" PageSize="4" OnRowCommand="gvTemplates_OnRowCommand"
                                                                OnRowDataBound="gvTemplates_RowDataBound" OnPageIndexChanging="gvTemplates_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="40px">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkTemplate" runat="server" CommandName="SelectTemplate" CommandArgument='<%#Eval("TemplateId") %>'
                                                                                Text='Select' CausesValidation="false" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Template Name" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblDiagnosis" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label12" runat="server" SkinID="label" Text="&nbsp;Diagnosis&nbsp;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="imgBtnDiagnosis" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Diagnosis" Height="18px" Width="16px" OnClick="lnkAddDiagnosis_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel3" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvDiagnosis" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="false" Width="100%" AllowPaging="true" PageSize="4" OnRowCommand="gvDiagnosis_OnRowCommand"
                                                                OnRowDataBound="gvDiagnosis_RowDataBound" OnPageIndexChanging="gvDiagnosis_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="100%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text="" />
                                                                            <asp:HiddenField ID="hdnICDDescription" runat="server" Value='<%#Eval("ICDDescription")%>' />
                                                                            <asp:HiddenField ID="hdnICDCode" runat="server" Value='<%#Eval("ICDCode")%>' />
                                                                            <asp:HiddenField ID="hdnDiagnosisCondition1" runat="server" Value='<%#Eval("DiagnosisCondition1")%>' />
                                                                            <asp:HiddenField ID="hdnDiagnosisCondition2" runat="server" Value='<%#Eval("DiagnosisCondition2")%>' />
                                                                            <asp:HiddenField ID="hdnDiagnosisCondition3" runat="server" Value='<%#Eval("DiagnosisCondition3")%>' />
                                                                            <asp:HiddenField ID="hdnEntryDate" runat="server" Value='<%#Eval("EntryDate")%>' />
                                                                            <asp:HiddenField ID="hdnISChronic" runat="server" Value='<%#Eval("ISChronic")%>' />
                                                                            <asp:HiddenField ID="hdnISResolved" runat="server" Value='<%#Eval("ISResolved")%>' />
                                                                            <asp:HiddenField ID="hdnPrimaryDiagnosis" runat="server" Value='<%#Eval("PrimaryDiagnosis")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblOrdersAndProcedures" runat="server" width="99%" border="0" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label13" runat="server" SkinID="label" Text="&nbsp;Orders&nbsp;And&nbsp;Procedures&nbsp;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="imgbtnAddOrdersAndProcedures" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Orders And Procedures" Height="17px" Width="16px" OnClick="lnkAddOrdersAndProcedures_OnClick" />
                                                            <%--<asp:LinkButton ID="lnkAddOrdersAndProcedures" runat="server" Text="Add New" OnClick="lnkAddOrdersAndProcedures_OnClick" />--%>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvOrdersAndProcedures" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="false" Width="100%" AllowPaging="true" PageSize="10" OnRowCommand="gvOrdersAndProcedures_OnRowCommand"
                                                                OnRowDataBound="gvOrdersAndProcedures_RowDataBound" OnPageIndexChanging="gvOrdersAndProcedures_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="100%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text="" />
                                                                            <asp:HiddenField ID="hdnServiceName" runat="server" Value='<%#Eval("ServiceName")%>' />
                                                                            <asp:HiddenField ID="hdnOrderDate" runat="server" Value='<%#Eval("OrderDate")%>' />
                                                                            <asp:HiddenField ID="hdnLabStatus" runat="server" Value='<%#Eval("LabStatus")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblLabResults" runat="server" width="99%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label15" runat="server" SkinID="label" Text="&nbsp;Lab Results&nbsp;-&nbsp;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:LinkButton ID="lnkShowLabHistory" runat="server" Text="Lab History" OnClick="lnkShowLabHistory_OnClick" />
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel6" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvLabResults" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="false" Width="100%" AllowPaging="true" PageSize="10" OnRowCommand="gvLabResults_OnRowCommand"
                                                                OnRowDataBound="gvLabResults_RowDataBound" OnPageIndexChanging="gvLabResults_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="100%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text="" />
                                                                            <asp:HiddenField ID="hdnFieldName" runat="server" Value='<%#Eval("FieldName")%>' />
                                                                            <asp:HiddenField ID="hdnResultDate" runat="server" Value='<%#Eval("ResultDate")%>' />
                                                                            <asp:HiddenField ID="hdnResult" runat="server" Value='<%#Eval("Result")%>' />
                                                                            <asp:HiddenField ID="hdnUnitName" runat="server" Value='<%#Eval("UnitName")%>' />
                                                                            <asp:HiddenField ID="hdnPatientName" runat="server" Value='<%#Eval("PatientName")%>' />
                                                                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value='<%#Eval("RegistrationNo")%>' />
                                                                            <asp:HiddenField ID="hdnOrderDate" runat="server" Value='<%#Eval("OrderDate")%>' />
                                                                            <asp:HiddenField ID="hdnSampleCollectedDate" runat="server" Value='<%#Eval("SampleCollectedDate")%>' />
                                                                            <asp:HiddenField ID="hdnLabFlagValue" runat="server" Value='<%#Eval("LabFlagValue")%>' />
                                                                            <asp:HiddenField ID="hdnTestResultStatus" runat="server" Value='<%#Eval("TestResultStatus")%>' />
                                                                            <asp:HiddenField ID="hdnReviewedComments" runat="server" Value='<%#Eval("ReviewedComments")%>' />
                                                                            <asp:HiddenField ID="hdnMinValue" runat="server" Value='<%#Eval("MinValue")%>' />
                                                                            <asp:HiddenField ID="hdnSymbol" runat="server" Value='<%#Eval("Symbol")%>' />
                                                                            <asp:HiddenField ID="hdnMaxValue" runat="server" Value='<%#Eval("MaxValue")%>' />
                                                                            <asp:HiddenField ID="hdnAbnormalValue" runat="server" Value='<%#Eval("AbnormalValue")%>' />
                                                                            <asp:HiddenField ID="hdnFieldType" runat="server" Value='<%#Eval("FieldType")%>' />
                                                                            <asp:HiddenField ID="hdnReviewedStatus" runat="server" Value='<%#Eval("ReviewedStatus")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblPrescriptions" runat="server" width="99%" border="0" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <td class="Labelheader">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label16" runat="server" SkinID="label" Text="&nbsp;Prescriptions&nbsp;" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:ImageButton ID="imgBtnAddPrescriptions" runat="server" ImageUrl="~/Images/add.gif"
                                                                ToolTip="Add Prescriptions" Height="18px" Width="16px" OnClick="lnkAddPrescriptions_OnClick" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="Panel7" runat="server" ScrollBars="Auto">
                                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvPrescriptions" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="false" Width="100%" AllowPaging="true" PageSize="10" OnRowCommand="gvPrescriptions_OnRowCommand"
                                                                OnRowDataBound="gvPrescriptions_RowDataBound" OnPageIndexChanging="gvPrescriptions_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="100%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDetails" runat="server" SkinID="label" Text="" />
                                                                            <asp:HiddenField ID="hdnItemName" runat="server" Value='<%#Eval("ItemName")%>' />
                                                                            <asp:HiddenField ID="hdnStartDate" runat="server" Value='<%#Eval("StartDate")%>' />
                                                                            <asp:HiddenField ID="hdnDose" runat="server" Value='<%#Eval("Dose")%>' />
                                                                            <asp:HiddenField ID="hdnUnitName" runat="server" Value='<%#Eval("UnitName")%>' />
                                                                            <asp:HiddenField ID="hdnQty" runat="server" Value='<%#Eval("Qty")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </asp:Panel>
            <table width="99%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                    <td>
                        <asp:Button ID="btnAddChiefComplaintsClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddChiefComplaintsClose_OnClick" />
                        <asp:Button ID="btnAddAllergiesClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddAllergiesClose_OnClick" />
                        <asp:Button ID="btnAddVitalsClose" runat="server" Style="visibility: hidden;" OnClick="btnAddVitalsClose_OnClick" />
                        <asp:Button ID="btnAddTemplatesClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddTemplatesClose_OnClick" />
                        <asp:Button ID="btnAddDiagnosisClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddDiagnosisClose_OnClick" />
                        <asp:Button ID="btnAddOrdersAndProceduresClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddOrdersAndProceduresClose_OnClick" />
                        <asp:Button ID="btnAddPrescriptionsClose" runat="server" Style="visibility: hidden;"
                            OnClick="btnAddPrescriptionsClose_OnClick" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
