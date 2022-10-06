<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Appointments.ascx.cs"
    Inherits="EMR_Dashboard_Parts_Appointments" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
  
    <tr>
        <td style="width: 100%">
            <asp:HiddenField ID="hdnAppointmentType" runat="server" />
            <asp:Panel ID="Panel3" runat="server" ScrollBars="None" Width="100%" Height="90%">
             <telerik:radformdecorator id="RadFormDecorator1" decoratedcontrols="All" runat="server"
                            decorationzoneid="dvZone1" skin="Metro"></telerik:radformdecorator>
                        <div id="dvZone1" style="width: 100%">
                        <asp:GridView ID="GDAppointment" runat="server" AllowPaging="true" ForeColor="#333333"
                            GridLines="Both" PageSize="9" Width="95%"  
                            OnRowDataBound="GDAppointment_RowDataBound" AutoGenerateColumns="false" CellPadding="0"
                            ShowHeader="true" SkinID="gridview" 
                            onpageindexchanging="GDAppointment_PageIndexChanging">
                            <HeaderStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:BoundField HeaderText="Encounter Date" DataField="EncounterDate" />
                                <asp:TemplateField HeaderText="Doctor&nbsp;Name">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkName" runat="server" OnClick="lbtnName_Click" CommandName="DoctorName" CommandArgument='<%#Eval("AllValues")%>' onmouseover="this.style.textDecoration='underline';" onmouseout="this.style.textDecoration='none';" Text='<%#Eval("DoctorName")%>' ForeColor="Blue" Font-Underline="true"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Diagnosis" DataField="Diagnosis" />
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                
                                
                                <asp:BoundField HeaderText="Gender" DataField="Gender" />
                                <asp:BoundField HeaderText="EncounterId" DataField="EncounterId" />
                                <asp:BoundField HeaderText="FacilityId" DataField="FacilityId" />
                                <asp:BoundField HeaderText="DoctorId" DataField="DoctorId" />
                                <asp:BoundField HeaderText="RegistrationId" DataField="RegistrationId" />
                                
                               
                            </Columns>
                        </asp:GridView>
                        </div>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td align="left">
            <table border="0" style="height: 17px; width: 100%;" cellpadding="0" cellspacing="0">
                <tr style="background-color: #EFF3FB">
                    <td colspan="2" align="center">
                                <asp:Panel ID="pnlAppointment" runat="server">
                                </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="hdnToDate" runat="server" style="visibility:hidden; position:absolute;" />
            <asp:TextBox ID="hdnFromDate" runat="server" style="visibility:hidden; position:absolute;" />
            <asp:TextBox ID="hdnEncounterNumber" runat="server" style="visibility:hidden; position:absolute;" />
            <asp:TextBox ID="hdnDateVale" runat="server" style="visibility:hidden; position:absolute;" />
        </td>
    </tr>
</table>
