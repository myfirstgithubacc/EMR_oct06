<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTDetails.aspx.cs" Inherits="OT_Scheduler_OTDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scrpmgr" runat="server">
    </asp:ScriptManager>
    <div>
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr class="clsheader">
                <td align="left" style="width: 150px; padding-left: 10px;">
                    <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="OT Booking Details"
                        Font-Bold="true" />
                </td>
                <td align="center">
                    <asp:Label ID="lblMessage" runat="server" SkinID="label" ForeColor="Red" Font-Bold="true" />
                </td>
                <td align="right">
                    <asp:Button ID="btnPrint" runat="server" Text="Print" SkinID="Button" visible="false"  />
                </td>
            </tr>
        </table>
        <%--<asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>--%>
        <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
            border-style: solid none solid none; border-width: 1px; border-color: #808080;"
            cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td width="25px">
                    <asp:ImageButton ID="btnPImage" runat="server" CausesValidation="false" Height="20"
                        Width="20" ImageUrl="~/Images/PImageBackGround.gif" />
                </td>
                <td>
                    <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                        Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                        Font-Bold="true"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Text='<%$ Resources:PRegistration, regno%>'
                        SkinID="label" Font-Bold="true"></asp:Label><span style="font-weight: bold;">:</span>
                    <asp:Label ID="lblRegistrationNo" runat="server" Text="" SkinID="label"></asp:Label>
                    <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="Gender:" SkinID="label" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblGender" runat="server" Text="" SkinID="label"></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" style="background: #E0EBFD; margin-left: 0px; padding-top: 0px;
            border-style: solid none solid none; border-width: 1px; border-color: #808080;"
            cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td valign="top">
                    <ajax:RoundedCornersExtender ID="rce" runat="server" TargetControlID="pnlTheater"
                        Radius="6" Corners="All" />
                    <asp:Panel ID="pnlTheater" runat="server" Height="80px" Width="250px">
                        <table width="99%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Theater:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblTheaterName" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="Date:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOTBookingDate" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="From Time:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFromTome" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Text="To Time:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblToTime" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td valign="top">
                    <asp:Panel ID="pnlService" runat="server" Height="80px" Width="250px">
                        <table width="99%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Text="Dept.:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Text="Sub Dept.:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubDepartment" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Text="Service:" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblServiceName" runat="server" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table width="99%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:GridView ID="gvSurgeon" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="Surgeon(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvAsstSurgeon" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="Asst. Surgeon(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvAnesthetist" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="Anesthetist(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvAssttAnesthetist" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyq" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="Asst. Anesthetist(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table width="99%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:GridView ID="gvPerfusionist" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="Perfusionist(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvScrubNurse" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="ScrubNurse(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvFloorNurse" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="Floor Nurse(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvOTTechnician" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="SurgeonName" HeaderText="OT Technician(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <table width="99%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:GridView ID="gvEquipment" SkinID="gridview" AutoGenerateColumns="false" runat="server"
                                    AllowPaging="false" CellPadding="4" Width="99%">
                                    <RowStyle BackColor="Transparent" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:BoundField DataField="EquipmentName" HeaderText="Equipment(s)" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</body>
</html>
