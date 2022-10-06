<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TemplateServiceRemarksTagging.aspx.cs"
    Inherits="LIS_Format_TemplateServiceRemarksTagging" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Service Tagging</title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Format&nbsp;Name : " />
                    <asp:Label ID="lblRemarksName" runat="server" SkinID="label" Columns="50" Font-Bold="true" />
                </td>
                <td align="center" style="font-size: 12px;">
                    <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                </td>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" ToolTip="Save" SkinID="Button" Text="Save"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false"
                        SkinID="Button" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="2" cellspacing="2">
            <tr>
                <td align="left">
                    <%--<asp:Label ID="label44" runat="server" SkinID="label" Text="Categories" />--%>
                    <asp:Label ID="label44" runat="server" SkinID="label" Text="Department" />
                </td>
                <td align="left">
                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>
                    <telerik:RadComboBox ID="ddlMainDept" runat="server" SkinID="DropDown" AllowCustomText="false"
                        Width="220px" EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlMainDept_OnSelectedIndexChanged" />
                    <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <%--<asp:Label ID="label1" runat="server" SkinID="label" Text="Sub&nbsp;Categories" />--%>
                    <asp:Label ID="label1" runat="server" SkinID="label" Text="Sub&nbsp;Department" />
                </td>
                <td>
                    <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>--%>
                    <telerik:RadComboBox ID="ddlsubDept" runat="server" SkinID="DropDown" AllowCustomText="false"
                        Width="200px" AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                        OnSelectedIndexChanged="ddlsubDept_OnSelectedIndexChanged" />
                    <%--   </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
                <td colspan="2" align="center">

                    <script language="JavaScript" type="text/javascript">
                        function LinkBtnMouseOver(lnk) {
                            document.getElementById(lnk).style.color = "red";
                        }
                        function LinkBtnMouseOut(lnk) {
                            document.getElementById(lnk).style.color = "blue";
                        }
                    </script>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="label3" runat="server" SkinID="label" Text="CPT&nbsp;Code" />
                </td>
                <td>
                    <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">--%>
                    <asp:TextBox ID="txtcptcode" SkinID="textbox" runat="server" MaxLength="8" Width="65px" />
                    <%-- </asp:Panel>
                       </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <asp:Label ID="label2" runat="server" SkinID="label" Text="Service&nbsp;Name" />
                </td>
                <td>
                    <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">--%>
                    <asp:TextBox ID="txtservicename" runat="server" SkinID="textbox" Width="200px" MaxLength="245" />
                    <%--   </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <%--<asp:UpdatePanel ID="up1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServiceDetail" />
                        </Triggers>
                        <ContentTemplate>--%>
                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="buttonBlue"
                        OnClick="btnSearch_Click" Text="Filter" />
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvServiceDetail" />
                            <asp:AsyncPostBackTrigger ControlID="ddlMainDept" />
                            <asp:AsyncPostBackTrigger ControlID="ddlsubDept" />
                            <asp:AsyncPostBackTrigger ControlID="txtcptcode" />
                            <asp:AsyncPostBackTrigger ControlID="txtservicename" />
                        </Triggers>
                        <ContentTemplate>--%>
                    <asp:Button ID="btnClearFilter" runat="server" CausesValidation="true" CssClass="buttonBlue"
                        OnClick="btnClearFilter_Click" Text="Clear Filter" />
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" style="width:50%">
                    <asp:GridView ID="gvServiceDetail" runat="server" BorderWidth="0" Width="100%" SkinID="gridview"
                        PageSize="18" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="gvServiceDetail_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Service Name" DataField="ServiceName" />
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkBtnServiceTag" runat="server" CommandName="ServiceTag" Text="Select"
                                        CommandArgument='<%# Eval("Serviceid") %>' ForeColor="DodgerBlue" OnClick="LnkBtnServiceTag_click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <span style="text-decoration: bold; color: Red;">No Rows Found. </span>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
                <td colspan="3" valign="top" style="width:50%">
                    <asp:GridView ID="gvSelectedService" runat="server" BorderWidth="0" AutoGenerateColumns="false"
                        SkinID="gridview" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="Service Name" DataField="ServiceName" />
                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" CommandName="Delete1"
                                        CommandArgument='<%#Eval("Serviceid") %>' ImageUrl="/images/DeleteRow.png" 
                                        ToolTip="Click here to delete a row" OnClick="imgDelete_OnClick" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
