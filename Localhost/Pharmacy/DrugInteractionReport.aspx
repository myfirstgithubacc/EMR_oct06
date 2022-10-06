<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="DrugInteractionReport.aspx.cs" Inherits="Pharmacy_DrugInteractionReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function OpenCIMSWindow() {
            var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

            var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(ReportContent.value);
            WindowObject.document.close();
            WindowObject.focus();
        }

        function validateMaxLength() {
            var txt = $get('<%=txtSearchRegNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }

        function MaxLenTxt(TXT, intMax) {
            if (TXT.value.length > intMax) {
                TXT.value = TXT.value.substr(0, intMax);
                alert("Maximum length is " + intMax + " characters only.");
            }
        }

    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr style="background-color: #E0EBFD">
                    <td style="width: 170px">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Font-Bold="true" Text="&nbsp;Drug Interaction Report" />
                    </td>
                    <td align="center" style="font-size: 12px;">
                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                    </td>
                    <td style="width: 150px">
                        <asp:Button ID="btnSave" runat="server" SkinID="Button" ToolTip="Save Data" Text="Save"
                            Width="90px" OnClick="btnSave_OnClick" />
                    </td>
                </tr>
            </table>
            <table style="margin-left: 10px" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td>
                        <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" Width="110px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Registration No" Value="R" Selected="true" />
                                <telerik:RadComboBoxItem Text="Encounter No" Value="E" />
                                <telerik:RadComboBoxItem Text="Patient Name" Value="P" />
                                <telerik:RadComboBoxItem Text="Indent No" Value="I" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Panel ID="Panel234" runat="server" DefaultButton="btnSearch">
                            <asp:TextBox ID="txtSearchRegNo" SkinID="textbox" Width="150px" runat="server" Text=""
                                MaxLength="13" onkeyup="return validateMaxLength();" />
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                FilterType="Custom" TargetControlID="txtSearchRegNo" ValidChars="0123456789" />
                            <asp:TextBox ID="txtSearch" runat="server" SkinID="textbox" Width="150px" MaxLength="20"
                                Visible="false" />
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Label ID="lblProvider" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, Doctor%>' />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" Filter="Contains" runat="server"
                            Width="200px" Height="300px" />
                        <%--AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged" --%>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="Visit Type" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVisitType" runat="server" Width="90px">
                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlVisitType_SelectedIndexChanged"--%>
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                <telerik:RadComboBoxItem Text="ER" Value="E" />
                                <telerik:RadComboBoxItem Text="OP" Value="O" />
                                <telerik:RadComboBoxItem Text="IP" Value="I" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Label ID="Label10" runat="server" SkinID="label" Text="Ward" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlWard" runat="server" Width="200px" Height="300px" Filter="Contains" />
                        <%--AutoPostBack="true" OnSelectedIndexChanged="ddlWard_SelectedIndexChanged"--%>
                    </td>

                    <td>
                        <asp:Label ID="lblItemcategory" runat="server" SkinID="label" Text="Item&nbsp;Category" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlItemcategory" MarkFirstMatch="true" Filter="Contains" runat="server"
                            Width="200px" Height="300px" />
                        <%--AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged" --%>
                    </td>


                </tr>
                <tr>
                    <td colspan="2">
                        <table cellpadding="0" cellspacing="2">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="From" />
                                    <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                </td>
                                <td>
                                    <asp:Label ID="Label6" runat="server" SkinID="label" Text="To" />
                                    <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text='Interaction Type' />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInteractionType" runat="server" Width="200px">
                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlInteractionType_SelectedIndexChanged"--%>
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                <telerik:RadComboBoxItem Text="Drug to Drug Interaction" Value="DD" />
                                <telerik:RadComboBoxItem Text="Drug Health Interaction" Value="DH" />
                                <telerik:RadComboBoxItem Text="Drug Allergy Interaction" Value="DA" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Status" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInteractionRemarksStatus" runat="server" Width="90px">
                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlInteractionRemarksStatus_SelectedIndexChanged"--%>
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="A" />
                                <telerik:RadComboBoxItem Text="Reviewed" Value="R" />
                                <telerik:RadComboBoxItem Text="Unreviewed" Value="U" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                     <td>
                        <asp:Label ID="lblDrugSeverity" runat="server" SkinID="label" Text="Severity" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlDrugSeverity" runat="server" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="ddlDrugSeverity_OnSelectedIndexChanged">
                            <%--AutoPostBack="true" OnSelectedIndexChanged="ddlInteractionRemarksStatus_SelectedIndexChanged"--%>
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                <telerik:RadComboBoxItem Text="Severe" Value="S" />
                                <telerik:RadComboBoxItem Text="Moderate" Value="MO"  />
                                <telerik:RadComboBoxItem Text="Minor" Value="MI" />
                                <telerik:RadComboBoxItem Text="Caution" Value="C" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" SkinID="Button" ToolTip="Filter" Text="Filter"
                            OnClick="btnSearch_OnClick" Width="60px" />
                    </td>
                    <td>
                        <asp:Button ID="btnClearSearch" runat="server" SkinID="Button" ToolTip="Clear Filter"
                            Text="Clear Filter" OnClick="btnClearSearch_OnClick" />

                        <asp:Button ID="btnExport" runat="server" SkinID="Button" CausesValidation="false"
                            Text="Export" OnClick="btnExport_OnClick" />
                    </td>
                     </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" SkinID="label" Width="20px" Height="16px" BackColor="LightGreen"
                                        BorderWidth="1px" />
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" SkinID="label" Text="Reviewed" />
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" SkinID="label" Width="20px" Height="16px" BackColor="CadetBlue"
                                        BorderWidth="1px" />
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="Current Drug" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="2" cellspacing="0" width="99%">
                <tr>
                    <td valign="top" style="width: 1060px">
                        <asp:Panel ID="pnlgrid" runat="server" Height="490px" Width="1060px" BorderWidth="1px"
                            BorderColor="SkyBlue" ScrollBars="Auto">
                            <asp:UpdatePanel ID="upProgress" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvItem" runat="server" Skin="Office2007" ShowGroupPanel="false"
                                        BorderWidth="0" CellPadding="0" CellSpacing="0" Width="1055px" Height="480px"
                                        AllowPaging="true" PageSize="150" AllowCustomPaging="true" PagerSettings-Position="TopAndBottom"
                                        AutoGenerateColumns="false" AllowMultiRowSelection="false" OnItemCommand="gvItem_OnItemCommand"
                                        OnItemDataBound="gvItem_ItemDataBound" OnPageIndexChanged="gvItem_OnPageIndexChanged">
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="4" />
                                        </ClientSettings>
                                        <MasterTableView TableLayout="Fixed" Width="100%" Font-Size="8">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red;">
                                                    No Record Found.
                                                </div>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Select" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="45px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkSelect" runat="server" Text='Select' CausesValidation="false"
                                                            CommandName="RowSelect" CommandArgument='<%#Eval("IndentDetailsId") %>' Font-Underline="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="UHID" HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="IP No." HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Patient Name" HeaderStyle-Width="160px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                        <asp:HiddenField ID="hdnIndentDetailsId" runat="server" Value='<%#Eval("IndentDetailsId") %>' />
                                                        <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId") %>' />
                                                        <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId") %>' />
                                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                        <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP") %>' />
                                                        <asp:HiddenField ID="hdnInteractionRemarksStatus" runat="server" Value='<%#Eval("InteractionRemarksStatus") %>' />
                                                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                        <asp:HiddenField ID="hdnCIMSTYPE" runat="server" Value='<%#Eval("CIMSTYPE") %>' />
                                                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                        <asp:HiddenField ID="hdnIsBD" runat="server" Value='<%#Eval("IsBD") %>' />
                                                        <asp:HiddenField ID="hdnIsMG" runat="server" Value='<%#Eval("IsMG") %>' />
                                                        <asp:HiddenField ID="hdnIsDD" runat="server" Value='<%#Eval("IsDD") %>' />
                                                        <asp:HiddenField ID="hdnIsDH" runat="server" Value='<%#Eval("IsDH") %>' />
                                                        <asp:HiddenField ID="hdnIsDA" runat="server" Value='<%#Eval("IsDA") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Gender / Age" HeaderStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Drug Name" HeaderStyle-Width="160px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="BD" HeaderTooltip="Brand Details" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details"
                                                            CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="MG" HeaderTooltip="Monograph" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                                            CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DD" HeaderTooltip="Drug to Drug Interaction"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                                            CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                            Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DH" HeaderTooltip="Drug Health Interaction"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                            CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DA" HeaderTooltip="Drug Allergy Interaction"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction"
                                                            CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA"
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="BD" HeaderTooltip="Brand Details" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details"
                                                            CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="MG" HeaderTooltip="Monograph" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                                            CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DD" HeaderTooltip="Drug to Drug Interaction"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                                            CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                            Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DH" HeaderTooltip="Drug Health Interaction"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                                            CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="DA" HeaderTooltip="Drug Allergy Interaction"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="35px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction"
                                                            CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA"
                                                            Text="&nbsp;" Font-Underline="false" Width="100%" Visible="false" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Indent No" HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIndentNo" runat="server" Text='<%#Eval("IndentNo") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Indent Date" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIndentDate" runat="server" Text='<%#Eval("IndentDate") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Serum Creatine" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerumCreatine" runat="server" Text='<%#Eval("SerumCreatine") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Visit Type" HeaderStyle-Width="65px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVisitType" runat="server" Text='<%#Eval("VisitType") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("Location") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Provider" HeaderStyle-Width="180px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reason For Over Ruling DD Interaction" HeaderStyle-Width="220px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOverrideCommentsDrugToDrug" runat="server" Text='<%#Eval("OverrideCommentsDrugToDrug") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reason For Over Ruling DH Interaction" HeaderStyle-Width="220px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOverrideCommentsDrugHealth" runat="server" Text='<%#Eval("OverrideCommentsDrugHealth") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reason For Over Ruling DA Interaction" HeaderStyle-Width="220px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOverrideComments" runat="server" Text='<%#Eval("OverrideComments") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Pharmacist Interaction Remarks" HeaderStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPharmacistInteractionRemarks" runat="server" TextMode="MultiLine"
                                                            Text='<%#Eval("PharmacistInteractionRemarks") %>' MaxLength="2000" Style="min-height: 30px; max-height: 30px; min-width: 295px; max-width: 295px;"
                                                            onkeyup="return MaxLenTxt(this,2000);" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Prescription Detail" HeaderStyle-Width="280px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Issue Qty" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueQty" runat="server" Text='<%#Eval("IssueQty") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Drug Alert Remarks" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDrugAlertRemarks" runat="server" Text='<%#Eval("DrugAlertRemarks") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvItem" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlCurrentDetails" runat="server" Height="490px" Width="100%" BorderWidth="1px"
                            BorderColor="SkyBlue" ScrollBars="Auto">
                            <asp:GridView ID="gvPrevious" runat="server" Width="100%" AllowPaging="false"
                                SkinID="gridview" AutoGenerateColumns="False" OnRowDataBound="gvPrevious_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Requested Drug(s)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Issued Drug(s)">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssuedItemName" runat="server" Text='<%#Eval("IssuedItemName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                    </td>
                    <td>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="upd1"
        DisplayAfter="2000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div style="width: 154; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px">
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
