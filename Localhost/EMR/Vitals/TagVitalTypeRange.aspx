<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TagVitalTypeRange.aspx.cs"
    Inherits="EMR_Vitals_TagVitalUnit" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: White;">
    <form id="frmTagVitalWithUnit" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" class="clsheader">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td width="30%" align="left">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                Define Range &nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right" style="padding-right: 10px">
                                                    <asp:Button ID="ibtnSaveVital" runat="server" SkinID="Button" OnClick="ibtnSaveVital_Click"
                                                        Text="Save" />
                                                    &nbsp;&nbsp;
                                                    <asp:Button ID="btnclose" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();"
                                                        OnClick="btnclose_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="1" style="height: 13px; color: green; font-size: 12px;
                            font-weight: bold;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="txtmsg" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellpadding="5" cellspacing="5">
                    <tr>
                        <td width="15%">
                            Vital
                        </td>
                        <td width="20%">
                            <telerik:RadComboBox SkinID="DropDown" ID="ddlvital" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlvital_SelectedIndexChanged" Width="170px" Filter="Contains">
                            </telerik:RadComboBox>
                        </td>
                        <td width="15%">
                            Vital Type
                        </td>
                        <td width="50%">
                            <asp:Label ID="lblVitalType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" width="10%">
                            Gender
                        </td>
                        <td align="left" valign="top" width="30%">
                            <telerik:RadComboBox SkinID="DropDown" ID="ddGender" runat="server" OnSelectedIndexChanged="ddlvital_SelectedIndexChanged"
                                Width="170px" Filter="Contains">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="Male" Value="M" />
                                    <telerik:RadComboBoxItem runat="server" Text="Female" Value="F" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            Unit
                        </td>
                        <td>
                            <telerik:RadComboBox SkinID="DropDown" ID="ddUnit" runat="server" 
                                Width="170px" Filter="Contains">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle">
                            Age From
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtAgeFrom" runat="server" Width="90px"></asp:TextBox>
                            <telerik:RadComboBox SkinID="DropDown" ID="ddAgeTypeFrom" runat="server" OnSelectedIndexChanged="ddlvital_SelectedIndexChanged"
                                Width="90px" Filter="Contains">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="Days" Value="D" />
                                    <telerik:RadComboBoxItem runat="server" Text="Week" Value="W" />
                                    <telerik:RadComboBoxItem runat="server" Text="Month" Value="M" />
                                    <telerik:RadComboBoxItem runat="server" Text="Year" Value="Y" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td align="left" valign="top">
                            Age To
                        </td>
                        <td align="left" valign="top">
                            <asp:TextBox ID="txtAgeTo" runat="server" Width="90px"></asp:TextBox>
                            <telerik:RadComboBox SkinID="DropDown" ID="ddAgeTypeTo" runat="server" OnSelectedIndexChanged="ddlvital_SelectedIndexChanged"
                                Width="90px" Filter="Contains">
                                <Items>
                                    <telerik:RadComboBoxItem runat="server" Text="Days" Value="D" />
                                    <telerik:RadComboBoxItem runat="server" Text="Week" Value="W" />
                                    <telerik:RadComboBoxItem runat="server" Text="Month" Value="M" />
                                    <telerik:RadComboBoxItem runat="server" Text="Year" Value="Y" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Min Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinValue" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            Max Value
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxValue" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Min <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration, PanicValue%>' SkinID="label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinPanicValue" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblMaxPanic" runat="server" Text="Max&nbsp Panic&nbsp Value" SkinID="label"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMaxPanicValue" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="gvDetail" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" OnRowDataBound="gvDetail_RowDataBound" 
                                Width="565px" OnSelectedIndexChanged="gvDetail_OnSelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField HeaderText="S No" DataField="Slno" ReadOnly="true" HeaderStyle-Width="100">
                                        <HeaderStyle Width="100px"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                    <asp:BoundField HeaderText="Unit Name" DataField="UnitName" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Age From" DataField="AgeFrom" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Age To" DataField="AgeTo" ReadOnly="true" />
                                    <asp:BoundField DataField="MinValue" HeaderText="Min Value" />
                                    <asp:BoundField DataField="MaxValue" HeaderText="Max Value" />
                                    <asp:BoundField DataField="MinPanicValue" HeaderText="Min Panic Value" />
                                    <asp:BoundField DataField="MaxPanicValue" HeaderText="MaxPanicValue" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                     <asp:CommandField ShowSelectButton="true" SelectText="Edit" />
                                    <asp:BoundField HeaderText="" DataField="UnitID" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Age From" DataField="AgeFromType" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Age To" DataField="AgeToType" ReadOnly="true" />
                                    <asp:BoundField HeaderText="" DataField="ID" ReadOnly="true" />
                                    <asp:BoundField DataField="Sex" HeaderText="" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:HiddenField ID="hdnVitalRangeId" runat="server" />
                     <asp:HiddenField ID="hdnVitalId" runat="server" />
                    </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
