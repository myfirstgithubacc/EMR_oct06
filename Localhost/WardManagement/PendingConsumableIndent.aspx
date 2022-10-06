<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PendingConsumableIndent.aspx.cs" Inherits="WardManagement_PendingConsumableIndent" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />


    <script src="../../Include/JS/Validate.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

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


            evt.returnValue = false;
            return false;
        }
        function SearchIndentOnClientClose(oWnd) {
            $get('<%=btnFilter.ClientID%>').click();
        }

        function returnToParent() {
            window.close();
        }

    </script>


    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server" />

            <div class="container-fluid header_main">
                <div class="col-md-2">
                    <h2>
                        <span id="tdHeader" runat="server">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Pending Drug Indent" />
                        </span>
                    </h2>
                </div>
                <div class="col-md-10 text-right">
                    <asp:Button ID="btnclose" Text="Close (Ctrl+F8)" runat="server" CssClass="ICCAViewerBtn" Visible="false"
                        OnClientClick="window.close();" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Status&nbsp;
                    <telerik:RadComboBox ID="ddlClosedStatus" runat="server" Width="100px"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlClosedStatus_OnSelectedIndexChanged">
                        <Items>
                            <telerik:RadComboBoxItem Text="All" Value="2" />
                            <telerik:RadComboBoxItem Text="Closed" Value="1" />
                            <telerik:RadComboBoxItem Text="Pending" Value="0" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>

                </div>
                <div class="col-md-4">
                    Ward&nbsp;Name
                <telerik:RadComboBox ID="ddlWard" runat="server" Width="200px" DropDownWidth="250px" Height="300px"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlWard_OnSelectedIndexChanged" />

                </div>
                <div class="col-md-3" id="divDateFilter" runat="server">
                    From&nbsp;
                <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="70px" AutoPostBack="true" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />

                    &nbsp;To&nbsp;
                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="70px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnFilter" runat="server" CausesValidation="false" OnClick="btnFilter_OnClick"
                        CssClass="btn btn-primary" Text="Filter" />
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" />
                </div>
            </div>
            <div id="tabPaymodedetails" runat="server" class="container-fluid">
                <div class="row">
                    <asp:GridView ID="gvIndent" runat="server" SkinID="gridview2" Width="100%" AllowPaging="true" PageSize="10" AutoGenerateColumns="False"
                        OnRowCommand="gvIndent_RowCommand" OnRowDataBound="gvIndent_RowDataBound" OnPageIndexChanging="gvIndent_PageIndexChanging">
                        <EmptyDataTemplate>
                            <div style="font-weight: bold; color: Red;">
                                No Record Found.
                            </div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="25px">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:PRegistration, RegNo%>" HeaderStyle-Width="90px"
                                ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Width="100%" Text='<%#Eval("RegistrationNo")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:PRegistration, EncounterNo%>" HeaderStyle-Width="90px"
                                ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Width="100%" Text='<%#Eval("EncounterNo")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:PRegistration, PatientName%>" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:Label ID="lblPatientName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("PatientName")%>' />

                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId")%>' />
                                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                    <asp:HiddenField ID="hdnIsClosedByNurse" runat="server" Value='<%#Eval("IsClosedByNurse")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Age Gender" HeaderStyle-Width="110px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAgeGender" runat="server" SkinID="label" Width="100%" Text='<%#Eval("AgeGender")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Consulting Doctor" HeaderStyle-Width="160px">
                                <ItemTemplate>
                                    <asp:Label ID="lblConsultingDoctorName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("ConsultingDoctorName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Advising Doctor" HeaderStyle-Width="160px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvisingDoctorName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("AdvisingDoctorName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Indent No" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Width="100%" Text='<%#Eval("IndentNo")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Indent Date" HeaderStyle-Width="90px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Width="100%" Text='<%#Eval("IndentDate")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Indent By" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentBy" runat="server" SkinID="label" Width="100%" Text='<%#Eval("IndentBy")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Indent Store" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndentStore" runat="server" SkinID="label" Width="100%" Text='<%#Eval("DepartmentName")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Print" HeaderStyle-Width="30px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnPrint" runat="server" ToolTip="Click here to print prescription"
                                        CommandName="PRINT" CommandArgument='<%#Eval("IndentId")%>' CausesValidation="false" Text="Print" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnMakeOrder" runat="server" CommandName="MAKEORDER" CausesValidation="false">Select</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <table width="99%" cellpadding="1" cellspacing="1" style="background: #e0ebfd;">
                <tr>
                    <td align="left">
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Pin, Move, Close, Maximize, Resize" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                            Behaviors="Close,Move,Pin,Resize,Maximize">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <asp:HiddenField ID="hdnGIndentId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnGIndentNo" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                        <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                        <asp:HiddenField ID="hdnEncounterId" runat="server" />
                        <asp:HiddenField ID="hdnEncounterNo" runat="server" />
                        <asp:HiddenField ID="hdnCIMSOutput" runat="server" />



                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

