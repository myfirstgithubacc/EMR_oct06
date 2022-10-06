<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TagVitalUnit.aspx.cs" Inherits="EMR_Vitals_TagVitalUnit"
    Title="" %>

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
                                                        Tag Vital With Unit &nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td align="right" style="padding-right: 10px">
                                            <%--<asp:ImageButton ID="ibtnSaveVital" runat="server" ImageUrl="/Images/save.gif" OnClick="ibtnSaveVital_Click" />--%>
                                            <asp:Button ID="ibtnSaveVital" runat="server" SkinID="Button" OnClick="ibtnSaveVital_Click"
                                                Text="Save" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnclose" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <td width="5%">
                    Vital
                </td>
                <td>
                    &nbsp;&nbsp;
                    <asp:DropDownList SkinID="DropDown" ID="ddlvital" runat="server" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlvital_SelectedIndexChanged" Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="middle">
                    Unit
                </td>
                <td align="left" valign="top">
                    <asp:CheckBoxList ID="chkunit" SkinID="checkbox" RepeatDirection="Horizontal" CellSpacing="2"
                        CellPadding="3" RepeatColumns="5" runat="server">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvUnitView" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" OnRowEditing="gvUnitView_RowEditing" OnRowCancelingEdit="gvUnitView_RowCancelingEdit"
                        OnRowUpdating="gvUnitView_RowUpdating" OnRowDataBound="gvUnitView_RowDataBound"
                        Width="565px">
                        <Columns>
                            <asp:BoundField HeaderText="S No" DataField="Slno" ReadOnly="true" />
                            <asp:BoundField HeaderText="Unit Name" DataField="UnitName" ReadOnly="true" />
                            <asp:BoundField HeaderText="Symbol" DataField="Symbol" ReadOnly="true" />
                            <asp:BoundField HeaderText="Measurement System" DataField="MeasurementSystem" ReadOnly="true" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%#Eval("Status") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlUnitStatusEdit" SelectedValue='<%#Eval("Active") %>' runat="server">
                                        <asp:ListItem Selected="True" Value="True" Text="Active"></asp:ListItem>
                                        <asp:ListItem Value="False" Text="Inactive"></asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    Status</HeaderTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:BoundField HeaderText="S No" DataField="UnitId" ReadOnly="true" />
                            <asp:BoundField HeaderText="S No" DataField="VitalId" ReadOnly="true" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
