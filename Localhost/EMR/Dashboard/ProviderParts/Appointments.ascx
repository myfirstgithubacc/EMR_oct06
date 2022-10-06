<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Appointments.ascx.cs"
    Inherits="EMR_Dashboard_ProviderParts_Appointmentl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">
    function OnClientClose(oWnd) {
        $get('<%=btnFilter.ClientID%>').click();
    }
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
            <tr>
                <td align="left">
                    <asp:TextBox ID="txtApp" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left" style="width: 120px">
                            <telerik:RadComboBox ID="ddlTime" runat="server" AutoPostBack="True" SkinID="DropDown"
                                    Width="150px" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                    <Items>
                                    <telerik:RadComboBoxItem Text="Select All" Value="" />
                                    <telerik:RadComboBoxItem Text="Today" Value="DD0" Selected="True"/>
                                    <telerik:RadComboBoxItem Text="This Week" Value="WW0"/>
                                    <telerik:RadComboBoxItem Text="This Month" Value="MM0"/>
                                    <telerik:RadComboBoxItem Text="Next Week" Value="WW+1"/>
                                    <telerik:RadComboBoxItem Text="Next Two Weeks" Value="WW+2"/>
                                    <telerik:RadComboBoxItem Text="Next Month" Value="MM+1"/>
                                    <telerik:RadComboBoxItem Text="Last Week" Value="WW-1"/>
                                    <telerik:RadComboBoxItem Text="Last Two Weeks" Value="WW-2"/>
                                    <telerik:RadComboBoxItem Text="Last One Month" Value="MM-1"/>
                                    <telerik:RadComboBoxItem Text="Last Year" Value="YY-1"/>
                                    <telerik:RadComboBoxItem Text="Date Range" Value="4"/>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td id="tdDateRange" runat="server" align="left" width="400px">
                                &nbsp;
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                &nbsp;<asp:Label ID="lblFrom" runat="server" SkinID="label" Text="From"></asp:Label>&nbsp;&nbsp;
                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00"
                                    Width="100px">
                                </telerik:RadDatePicker>
                                &nbsp;
                                <asp:Label ID="lblTo" runat="server" SkinID="label" Text="To"></asp:Label>
                                &nbsp;&nbsp;
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100px">
                                </telerik:RadDatePicker>
                            </td>
                            <td align="left">
                                &nbsp;&nbsp;
                                <asp:Button ID="btnFilter" runat="server" SkinID="Button" Text="Filter" OnClick="btnFilter_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%">
                    <asp:Panel ID="Panel4" runat="server" ScrollBars="Vertical" Width="100%" Height="200px">
                        <asp:GridView ID="GVAppointment" ForeColor="#333333" runat="server" AllowPaging="True"
                            SkinID="gridview" Width="99%" AutoGenerateColumns="False" CellPadding="0" EmptyDataText="No Appointments!"
                            EmptyDataRowStyle-ForeColor="Red" OnPageIndexChanging="GVAppointment_PageIndexChanging"
                            OnRowDataBound="GVAppointment_RowDataBound" OnSelectedIndexChanging="GVAppointment_SelectedIndexChanging"
                            PageSize="15">
                            <EmptyDataRowStyle ForeColor="Red" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltEncDate" runat="server" Text="Appt Date"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--  <asp:Label ID="lblEncDate" runat="server" Text='<%#Eval("AppointmentDate")%>' Width="127px"></asp:Label>--%>
                                        <asp:LinkButton ID="lnkEncdate" runat="server" CommandName="Select" CommandArgument='<%#Eval("AllValues")%>'
                                            onmouseover="this.style.textDecoration='underline';" onmouseout="this.style.textDecoration='none';"
                                            Font-Strikeout="false" Text='<%#Eval("AppointmentDate")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltDuration" runat="server" Text="Duration"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDuration" runat="server" Text='<%#Eval("Duration")%>' Width="57px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Patient&nbsp;Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkName" runat="server" OnClick="lbtnName_Click" CommandArgument='<%#Eval("AllValues")%>'
                                            onmouseover="this.style.textDecoration='underline';" CommandName="Name" onmouseout="this.style.textDecoration='none';"
                                            Font-Strikeout="false" Text='<%#Eval("Name")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltVisitType" runat="server" Text="Visit&nbsp;Type&nbsp;&nbsp;&nbsp;"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVisitType" runat="server" Text='<%#Eval("VisitType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltStatus" runat="server" Text="Status"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStats" runat="server" Text='<%#Eval("Status")%>' Width="41px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EncounterNo" HeaderText="Encounter No" SortExpression="EncounterNo" />
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltRoomNo" runat="server" Text="Room"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoomNo" runat="server" Text='<%#Eval("RoomNo")%>' Width="47px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltReasonForVisit" runat="server" Text="Reason&nbsp;For&nbsp;Visit"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' Width="47px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltReasonForVisit" runat="server" Text="Recurrence"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrenceRule" runat="server" Text='<%#Eval("RecurrenceRule")%>'
                                            Width="47px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FacilityName" HeaderText="Facility" />
                               <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor")%>'
                                            ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table border="0" style="height: 17px; width: 100%;" cellpadding="0" cellspacing="0">
                        <tr style="background-color: #EFF3FB">
                            <td>
                            </td>
                            <td colspan="2" align="center">
                                <asp:Panel ID="pnlOrders" runat="server">
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnAppointments" runat="server" />
                    <asp:HiddenField ID="hdnAppointmentsCtrl" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </td>
            </tr>
        </table>
        <div id="dvUpdateStatus" runat="server" visible="false" style="width: 300px; z-index: 100;
            border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White;
            border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute;
            bottom: 0; height: 60px; left: 350px; top: 100px">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <table width="99%" border="0" cellpadding="0" cellspacing="2px">
                        <tr>
                            <td colspan="5" align="center">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Would you like to check this patient in?"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3" align="center">
                                <asp:Button ID="btnUpdateYes" SkinID="Button" runat="server" Text="Yes" OnClick="btnUpdateYes_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnUpdateNo" SkinID="Button" runat="server" Text="No" OnClick="btnUpdateNo_Click" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
