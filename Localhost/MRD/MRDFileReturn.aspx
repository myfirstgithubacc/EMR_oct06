<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="MRDFileReturn.aspx.cs" Inherits="MRD_MRDFileReturn" Title="Untitled Page" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function showMenu(e, menu, issueId) {
            $get('<%=hdnGIssueId.ClientID%>').value = $get(issueId).value;

            var menu = $find(menu);
            menu.show(e);
        }
  
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table border="0" class="clsheader" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 220px">
                        &nbsp;
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Manual File Return" />
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>
            <table id="Table1" cellpadding="0" cellspacing="0" runat="server" border="0" width="100%">
                <tr>
                    <td align="center" style="color: green; font-size: 12px; font-weight: bold;">
                        <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                    </td>
                </tr>
            </table>
            <table id="Table2" border="0" cellpadding="0" cellspacing="4" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="File Status" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFileStatus" Width="170px" runat="server" EmptyMessage="[ Select ]"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlFileStatus_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="File Issued" Value="ISS" Selected="true" />
                                <telerik:RadComboBoxItem Text="File Returned" Value="RTN" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="Patient Type" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPatientType" Width="60px" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlFileStatus_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="" Selected="true" />
                                <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                <telerik:RadComboBoxItem Text="IPD" Value="I" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <%-- <td>
                        <asp:Panel ID="Panel1" runat="server" BorderWidth="1px" BorderColor="LightBlue">
                            <asp:RadioButtonList ID="rdoStatusType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Requested" Value="REQ" Selected="True" />
                                <asp:ListItem Text="Issued" Value="ISS" />
                                <asp:ListItem Text="Return" Value="RET" />
                            </asp:RadioButtonList>
                        </asp:Panel>
                    </td>--%>
                    <td align="right">
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Date" />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlTime" runat="server" Width="120px" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Today" Value="Today" Selected="true" />
                                <telerik:RadComboBoxItem Text="Last Week" Value="LastWeek" />
                                <telerik:RadComboBoxItem Text="Last Two Weeks" Value="LastTwoWeeks" />
                                <telerik:RadComboBoxItem Text="Last One Month" Value="LastOneMonth" />
                                <telerik:RadComboBoxItem Text="Last Three Months" Value="LastThreeMonths" />
                                <telerik:RadComboBoxItem Text="Last Year" Value="LastYear" />
                                <telerik:RadComboBoxItem Text="Date Range" Value="DateRange" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlTime" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblDate" runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnFilter" runat="server" SkinID="Button" Text="Filter" OnClick="btnFilter_OnClick" />
                    </td>
                </tr>
            </table>
            <table id="Table3" cellpadding="0" cellspacing="0" runat="server" border="0" width="99%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvData" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="500px" Width="100%" ScrollBars="Auto"
                                    BorderWidth="1px" BorderColor="LightBlue">
                                    <asp:GridView ID="gvData" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                        Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false"
                                        OnRowDataBound="gvData_RowDataBound" OnRowCommand="gvData_OnRowCommand">
                                        <Columns>
                                            <%-- <asp:CommandField HeaderText='Select' ControlStyle-ForeColor="Blue" SelectText="Select"
                                                ShowSelectButton="true" ItemStyle-Width="30px">
                                                <ControlStyle ForeColor="Blue" />
                                            </asp:CommandField>--%>
                                            <asp:TemplateField HeaderText='Date' ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTransctionDate" runat="server" Text='<%#Eval("TransctionDate")%>' />
                                                    <asp:HiddenField ID="hdnIssueId" runat="server" Value='<%#Eval("IssueId")%>' />
                                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                    <asp:HiddenField ID="hdnMRDStatusCode" runat="server" Value='<%#Eval("MRDStatusCode")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Width="100%" Text='<%#Eval("RegistrationNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Width="100%" Text='<%#Eval("EncounterNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, PatientName%>'>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Width="100%" Text='<%#Eval("PatientName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Age / Gender' ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientAgeGender" runat="server" Width="100%" Text='<%#Eval("PatientAgeGender")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Department' ItemStyle-Width="140px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepartmentName" runat="server" Width="100%" Text='<%#Eval("DepartmentName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Requested By' ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestedBy" runat="server" Width="100%" Text='<%#Eval("RequestedBy")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Remarks' ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Width="100%" Text='<%#Eval("Remarks")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Update Status" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnCategory" runat="server" ImageUrl="~/Images/T.Png" />
                                                    <telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true"
                                                        EnableShadows="true" Width="100px" OnItemClick="menuStatus_ItemClick" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnGIssueId" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
