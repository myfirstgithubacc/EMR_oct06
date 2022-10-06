<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Admission.ascx.cs" Inherits="EMR_Dashboard_ProviderParts_Admission" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table width="100%" height="200px">
    <tr>
        <td valign="top">
            <asp:TextBox ID="txtAdmission" runat="server" Style="visibility: hidden; position: absolute;"></asp:TextBox>
            <asp:GridView ID="gvAdmission" runat="server" SkinID="gridview2" Width="100%" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="RegistrationNo" HeaderText="Reg#" />
                    <asp:BoundField DataField="Patient_Name" HeaderText="Name" />
                    <asp:BoundField DataField="EncounterNo" HeaderText="IP No." />
                    <asp:BoundField DataField="Admission_Date" HeaderText="Admission Date" />
                    <asp:BoundField DataField="Doctor_Name" HeaderText="Provider" />
                    <asp:BoundField DataField="BedNo" HeaderText="Bed No" />
                    <asp:BoundField DataField="WardName" HeaderText="Ward Name" />
                     <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
