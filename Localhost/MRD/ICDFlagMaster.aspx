<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ICDFlagMaster.aspx.cs" Inherits="MRD_ICDFlagMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ICD Flag Master</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../style.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updItemFlagMaster" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table width="100%" border="0" cellpadding="3" cellspacing="0">
                    <tr>
                        <%--class="clsheader"--%>
                        <td align="left">
                            &nbsp;
                            <%--<asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Item&nbsp;Flag&nbsp;Master"
                                Font-Bold="true" />--%>
                        </td>
                        <td align="right">
                            <asp:Button ID="btnNew" Text="New" runat="server" ToolTip="New&nbsp;Record" CausesValidation="false"
                                SkinID="Button" OnClick="btnNew_Click"/>
                            <asp:Button ID="btnSave" Text="Save" runat="server" ToolTip="Save&nbsp;Data" CausesValidation="false"
                                SkinID="Button"  OnClick="btnSave_Click"/>
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false"
                                SkinID="Button" OnClientClick="window.close();" />
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <table border="0" width="100%">
                                <tr>
                                    <td colspan="4" align="center" style="font-size: 12px;">
                                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 50px">
                                        <asp:Label ID="Label5" runat="server" SkinID="label" ToolTip="ICD&nbsp;Flag&nbsp;Name" Text="Name" />&nbsp;<font
                                            color="red">*</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtICDFlagName" runat="server" SkinID="textbox" Width="360px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlstatus" SkinID="DropDown" runat="server" Width="80px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Active" Value="1" Selected="true" />
                                                <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <telerik:RadGrid ID="gvItemFlagMaster" Skin="Office2007" PagerStyle-ShowPagerText="false"
                                            BorderWidth="1" AllowFilteringByColumn="true" ShowHeader="true" PagerStyle-Visible="true"
                                            AllowPaging="True" PageSize="10" runat="server" AutoGenerateColumns="False" ShowStatusBar="true"
                                            EnableLinqExpressions="false" Width="100%" GroupingSettings-CaseSensitive="false"
                                            OnSelectedIndexChanged="gvItemFlagMaster_OnSelectedIndexChanged" OnPageIndexChanged="gvItemFlagMaster_PageIndexChanged">
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <MasterTableView AllowFilteringByColumn="true" TableLayout="Auto">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="ICDFlagName" DefaultInsertValue="" HeaderText="Name"
                                                        DataField="ICDFlagName" SortExpression="ICDFlagName" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblICDFlagName" runat="server" Text='<%#Eval("ICDFlagName") %>' />
                                                            <asp:HiddenField ID="hdnICDFlagId" runat="server" Value='<%#Eval("ICDFlagId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                                        AllowFiltering="false" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" FooterStyle-ForeColor="Blue"
                                                        FooterStyle-Font-Bold="true" HeaderStyle-Width="40px" />
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                <asp:AsyncPostBackTrigger ControlID="btnNew" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
