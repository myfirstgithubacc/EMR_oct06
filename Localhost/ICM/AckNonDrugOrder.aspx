<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="AckNonDrugOrder.aspx.cs" Inherits="AckNonDrugOrder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }

        function executeCode(evt) {
            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }
    </script>

    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr class="clsheader">
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Acknowledge Non Drug Order" ToolTip="Acknowledge Non Drug Order"></asp:Label>
                    </td>
                    <td align="left">
                       <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </td>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Acknowledge" SkinID="Button" OnClick="btnSave_OnClick" />
                    </td>
                </tr>
            </table>
            <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Panel ID="pnlDoctor" runat="server">
                                        <table border="0" cellpadding="0">
                                            <tr>
                                                <td valign="top" >
                                                    Non Drug Orders <span style="color: Red">*</span>
                                                </td>
                                                <td valign="top">
                                                    <telerik:RadEditor runat="server" ID="txtPrescription" EnableTextareaMode="false"
                                                        Skin="Office2007" Width="600px" Height="110px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                                        ToolsFile="~/Include/XML/PrescriptionRTF.xml" EditModes="Design">
                                                        <CssFiles>
                                                            <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                                        </CssFiles>
                                                        <SpellCheckSettings AllowAddCustom="true" />
                                                        <ImageManager ViewPaths="~/medical_illustration" />
                                                    </telerik:RadEditor>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                                        <tr>
                                                            <td style="width: 110px">
                                                                Date
                                                            </td>
                                                            <td>
                                                                <asp:UpdatePanel ID="udpdateoforder" runat="server">
                                                                    <ContentTemplate>
                                                                        <telerik:RadDateTimePicker ID="dtpdate" runat="server" DateInput-DateFormat="dd/MM/yyyy hh:mm tt"
                                                                            DateInput-DateDisplayFormat="dd/MM/yyyy hh:mm tt" Calendar-DayNameFormat="FirstLetter"
                                                                            TabIndex="0" AutoPostBackControl="Calendar" Calendar-EnableAjaxSkinRendering="True"
                                                                            Width="200px" PopupDirection="BottomRight" Enabled="false" Calendar-Enabled="False"
                                                                            DateInput-Enabled="False">
                                                                        </telerik:RadDateTimePicker>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label1" runat="server" Text="Order Type"></asp:Label>
                                                                <span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlOrderType" SkinID="DropDown" runat="server" Width="80px"
                                                                    AutoPostBack="false">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="Routine" Value="R" />
                                                                        <telerik:RadComboBoxItem Text="Urgent" Value="U" />
                                                                        <telerik:RadComboBoxItem Text="Stat" Value="S" />
                                                                        <telerik:RadComboBoxItem Text="SOS" Value="O" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblDoctor" runat="server" Text="Doctor"></asp:Label>
                                                                <span style="color: Red">*</span>
                                                            </td>
                                                            <td>
                                                                <telerik:RadComboBox ID="ddlDoctor" Width="200px" MarkFirstMatch="true" runat="server"
                                                                    SkinID="DropDown" EmptyMessage="Select">
                                                                </telerik:RadComboBox>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" Width="80px" MarkFirstMatch="true"
                                                                    runat="server" AutoPostBack="false">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Value="1" Text="Active" Selected="true" />
                                                                        <telerik:RadComboBoxItem Value="0" Text="In-Active" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <asp:Label ID="lblModBy" Visible="false" runat="server" Text="Modify By" />&nbsp;
                                                                <asp:Label ID="lblModifyBy" Visible="false" Font-Bold="true" runat="server" Text="" />&nbsp;
                                                                <asp:Label ID="lblModDate" Visible="false" runat="server" Text="Modify Date" />&nbsp;
                                                                <asp:Label ID="lblModifyDate" Visible="false" Font-Bold="true" runat="server" Text="" />
                                                                <asp:Label ID="lblAck" Visible="false" runat="server" Text="Acknowledge By" />&nbsp;
                                                                <asp:Label ID="lblAcknowledgeBy" Visible="false" Font-Bold="true" runat="server"
                                                                    Text="" />&nbsp;
                                                                <asp:Label ID="lblAckDate" Visible="false" runat="server" Text="Acknowledge Date" />&nbsp;
                                                                <asp:Label ID="lblAcknowledgeDate" Visible="false" Font-Bold="true" runat="server"
                                                                    Text="" />&nbsp;
                                                                <asp:Label ID="lblAckRem" Visible="false" runat="server" Text="Acknowledge Remark" />&nbsp;
                                                                <asp:Label ID="lblAckRemark" Visible="false" Font-Bold="true" runat="server" Text="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlgvdata" runat="server" Height="200px" ScrollBars="Vertical">
                                        <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                            AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                            Width="99%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                             AllowPaging="false" PageSize="5" OnItemCommand="gvData_OnItemCommand"
                                            OnItemDataBound="gvData_OnItemDataBound">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <MasterTableView TableLayout="Auto">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <ItemStyle Wrap="true" />
                                                <Columns>
                                                    <telerik:GridTemplateColumn HeaderText="Id" CurrentFilterFunction="Contains" AllowFiltering="False"
                                                        ShowFilterIcon="false" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNonDrugOrderId" runat="server" SkinID="label" Text='<%#Eval("NonDrugOrderId")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnAcknowledge" runat="server" Value='<%#Eval("Acknowledge")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Order Date" CurrentFilterFunction="Contains"
                                                        UniqueName="Name" AllowFiltering="False" AutoPostBackOnFilter="False" ShowFilterIcon="false"
                                                        FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderDate" runat="server" SkinID="label" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="140px" />
                                                        <ItemStyle Width="140px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText=" Non Drug Orders" CurrentFilterFunction="Contains"
                                                        UniqueName="Prescription" AllowFiltering="False" AutoPostBackOnFilter="False"
                                                        ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrescription" runat="server" SkinID="label" Text='<%#Eval("Prescription")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Order Type" CurrentFilterFunction="Contains"
                                                        AllowFiltering="False" AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrderType" runat="server" SkinID="label" Text='<%#Eval("OrderTypeName")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%#Eval("OrderType")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="80px" />
                                                        <ItemStyle Width="80px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Doctor Name" CurrentFilterFunction="Contains"
                                                        AllowFiltering="False" AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="180px" />
                                                        <ItemStyle Width="180px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Acknowledge By" CurrentFilterFunction="Contains"
                                                        UniqueName="AcknowledgeBy" AllowFiltering="False" AutoPostBackOnFilter="False"
                                                        ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAcknowledgeBy" runat="server" SkinID="label" Text='<%#Eval("AcknowledgeBy")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnNurseId" runat="server" Value='<%#Eval("NurseId")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="120px" />
                                                        <ItemStyle Width="120px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Acknowledge Date" CurrentFilterFunction="Contains"
                                                        UniqueName="AcknowledgeDate" AllowFiltering="False" AutoPostBackOnFilter="False"
                                                        ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAcknowledgeDate" runat="server" SkinID="label" Text='<%#Eval("AcknowledgeDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Width="100px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Acknowledge Remarks" CurrentFilterFunction="Contains"
                                                        UniqueName="AcknowledgeRemarks" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAcknowledgeRemarks" runat="server" SkinID="label" Text='<%#Eval("AcknowledgeRemarks")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="180px" />
                                                        <ItemStyle Width="180px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Encoded By" CurrentFilterFunction="Contains"
                                                        UniqueName="EncodedBy" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncodedBy" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle Width="150px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Encoded Date" CurrentFilterFunction="Contains"
                                                        UniqueName="EncodedDate" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncodedDate" runat="server" SkinID="label" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="140px" />
                                                        <ItemStyle Width="140px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Modify By" Visible="false" CurrentFilterFunction="Contains"
                                                        UniqueName="ModifyBy" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModifyBy" runat="server" SkinID="label" Text='<%#Eval("ModifyBy")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Modify Date" Visible="false" CurrentFilterFunction="Contains"
                                                        UniqueName="ModifyDate" ShowFilterIcon="false" FilterControlWidth="100%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModifyDate" runat="server" SkinID="label" Text='<%#Eval("ModifyDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-HorizontalAlign="Center"
                                                        ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbnSelect" runat="server" Text="Select" CommandName="Select"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="50px" />
                                                        <ItemStyle Width="50px" />
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtack" runat="server" Text=" " Width="20" BorderColor="LightGreen"
                                        BorderWidth="0" BackColor="LightGreen"></asp:TextBox>
                                    Acknowledge Order
                                    <asp:Panel ID="pnlNurse" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    Acknowledge By <span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlNurse" Width="200px" MarkFirstMatch="true" runat="server"
                                                        SkinID="DropDown" EmptyMessage="Select" Enabled="false" >
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Acknowledge Date & Time
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <telerik:RadDateTimePicker ID="dtpAckdate" runat="server" DateInput-DateFormat="dd/MM/yyyy hh:mm tt"
                                                                DateInput-DateDisplayFormat="dd/MM/yyyy hh:mm tt" Calendar-DayNameFormat="FirstLetter"
                                                                TabIndex="0" AutoPostBackControl="Both" Calendar-EnableAjaxSkinRendering="True"
                                                                Width="200px" PopupDirection="BottomRight" Enabled="false" SkinID="DropDown">
                                                            </telerik:RadDateTimePicker>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Acknowledge Remark <span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAckRemark" runat="server" SkinID="textbox" Width="250px" TextMode="MultiLine"
                                                        Text="" MaxLength="200"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnNonDrugOrder" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
