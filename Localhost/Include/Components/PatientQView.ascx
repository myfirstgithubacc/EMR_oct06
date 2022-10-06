<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PatientQView.ascx.cs"
    Inherits="Include_Components_PatientQView" %>
<style type="text/css">
    .text04
    {
        font-family: Arial;
        font-size: 12px;
        color: #ffffff;
        font-weight: bold;
        padding-left: 16px;
        text-transform:capitalize;
    }
</style>
<table id="tblPatientDet" runat="server" cellpadding="0" cellspacing="0" width="235px"
    style="border: 1px solid #C0C0C0; background-color: #3366FF;">
    <tr>
        <td style="width: 70px;">
            <asp:Image ID="imgFindPatient" Height="85px" Width="75px" runat="server" ImageUrl="~/Images/no_photo.jpg" />
        </td>
        <td valign="top" style="padding-left: 2px;">
            <%--  <asp:Panel ID="Panel5" runat="server" ScrollBars="Vertical" Width="100%" >--%>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lbl_PatientName" runat="server" CssClass="text04"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_AgeGender" runat="server" CssClass="text04"></asp:Label>
                    </td>
                </tr>
                <%--  <tr>
                    <td>
                        <asp:Label ID="lbl_Dob"  runat="server" ForeColor="White"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Address" runat="server" CssClass="text04"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_PhoneHome" runat="server"  CssClass="text04"></asp:Label>
                    </td>
                </tr>
                <%--<tr>
                    <td>
                        <asp:Label ID="lbl_Prim_Insurance" runat="server"  CssClass="text04"></asp:Label>
                    </td>
                </tr>--%>
            </table>
            <%-- </asp:Panel>--%>
        </td>
    </tr>
</table>
