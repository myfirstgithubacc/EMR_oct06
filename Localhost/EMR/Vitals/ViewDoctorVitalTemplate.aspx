<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewDoctorVitalTemplate.aspx.cs"
    Inherits="EMR_Vitals_ViewDoctorVitalTemplate" Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">

        function pageLoad() {
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnclose" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="1" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table width="97%" cellpadding="1" cellspacing="2">
                <tr>
                    <td>
                        <strong>Unit System</strong> &nbsp;<asp:Label ID="lbldescription" ForeColor="Blue"
                            Font-Bold="true" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <strong>Description</strong>&nbsp;<asp:Label ID="lblMeasurement" ForeColor="Blue"
                            Font-Bold="true" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="pnlDoctors" runat="server" ScrollBars="Auto">
                            <asp:GridView ID="gvDoctors" SkinID="gridview" CellPadding="2" runat="server" AutoGenerateColumns="false"
                                ShowHeader="true" Width="100%" PageSize="25" AllowPaging="true" OnRowDataBound="gvDoctors_OnRowDataBound"
                                OnPageIndexChanging="gvDoctors_OnPageIndexChanging" PagerSettings-Visible="true"
                                PageIndex="0" PagerSettings-Mode="NumericFirstLast" OnRowCommand="gvDoctors_RowCommand">
                                <Columns>
                                    <%--<asp:BoundField DataField="DoctorName" HeaderText="Provider Name" />--%>
                                    <asp:TemplateField HeaderText="Provider Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorName" CssClass="emrFullDivCenter" runat="server" Text='<%#Eval("DoctorName")%>' />
                                        <asp:HiddenField ID ="hdnId" runat="server"  Value='<%#Eval("id")%>'  />
                                            <asp:HiddenField ID ="hdnDoctorId" runat="server"   Value='<%#Eval("DoctorId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField>
                                        <ItemTemplate>
                                             <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                                ToolTip="Delete" Width="16px" Height="16px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                ToolTip="Delete" Width="16px" Height="16px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblMess" SkinID="label" ForeColor="Red" Font-Size="13px" runat="server"
                                        Text="No Doctor(s) found!..." />
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:GridView ID="gvVitalServices" CellPadding="2" SkinID="gridview" runat="server"
                            AutoGenerateColumns="false" ShowHeader="true" Width="100%" PageSize="25" AllowPaging="true"
                            OnRowDataBound="gvVitalServices_OnRowDataBound" PagerSettings-Visible="true"
                            OnPageIndexChanging="gvVitalServices_OnPageIndexChanging" PageIndex="0" PagerSettings-Mode="NumericFirstLast">
                            <Columns>
                                <asp:BoundField DataField="VitalId" />
                                <asp:BoundField DataField="Vital" HeaderText="Vitals" />
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMess" SkinID="label" ForeColor="Red" Font-Size="13px" runat="server"
                                    Text="No Vital Service(s) found!..." />
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
