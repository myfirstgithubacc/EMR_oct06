<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ICD9SubGroupMaster.aspx.cs" Inherits="MRD_Master_ICD9GroupMaster" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ICD10 Sub Group Master</title>
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
                                    <td colspan="3" align="center" style="font-size: 12px;">
                                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Group" />
                                        <span style="color: Red">*</span>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlGroup" runat="server" Width="200px"
                                            EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" />

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="Sub Group Name" />&nbsp;<font
                                            color="red">*</font>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubGroupName" runat="server" SkinID="textbox" Width="360px" MaxLength="50" />
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" SkinID="label" ToolTip="Range Start" Text="Range Start" />
                                    </td>
                                <td>
                                    <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRangeStart" runat="server" SkinID="textbox" Width="60px" MaxLength="50"/>       
                                        </td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" SkinID="label" ToolTip="Range End" Text="Range End" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRangeEnd" runat="server" SkinID="textbox" Width="60px" MaxLength="50" />
                                        </td>
                                    </tr>
                                    </table>
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
                </table>
                
                <table width="100%">
                    <tr>
                        <td>
                            <table border="0" width="100%">
                                
                                <tr>
                                    <td colspan="2">
                                        <telerik:RadGrid ID="gvIcdSubGroupMaster" Skin="Office2007" PagerStyle-ShowPagerText="false"
                                            BorderWidth="1" AllowFilteringByColumn="true" ShowHeader="true" PagerStyle-Visible="true"
                                            AllowPaging="True" PageSize="10" runat="server" 
                                            AutoGenerateColumns="False" ShowStatusBar="true"
                                            EnableLinqExpressions="false" Width="100%" GroupingSettings-CaseSensitive="false"
                                            OnSelectedIndexChanged="gvIcdSubGroupMaster_OnSelectedIndexChanged" 
                                            OnPageIndexChanged="gvIcdSubGroupMaster_PageIndexChanged">
                                            <FilterMenu EnableImageSprites="False">
                                            </FilterMenu>
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="False" />
                                            <MasterTableView AllowFilteringByColumn="true" TableLayout="Auto">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                    Visible="True">
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                    Visible="True">
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="GroupName" DefaultInsertValue="" HeaderText="Group Name"
                                                        DataField="GroupName" SortExpression="GroupName" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("GroupName") %>' />
                                                            <asp:HiddenField ID="hdnGroupId" runat="server" Value='<%#Eval("GroupId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="SubGroupName" DefaultInsertValue="" HeaderText="Sub Group Name"
                                                        DataField="SubGroupName" SortExpression="SubGroupName" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubGroupName" runat="server" Text='<%#Eval("SubGroupName") %>' />
                                                            <asp:HiddenField ID="hdnsubGroupId" runat="server" Value='<%#Eval("subGroupId") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                     <telerik:GridTemplateColumn UniqueName="Range" DefaultInsertValue="" HeaderText="Range"
                                                        DataField="Range" SortExpression="Range" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRange" runat="server" Text='<%#Eval("Range") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RangeStart" DefaultInsertValue="" HeaderText="RangeStart"
                                                        DataField="RangeStart" SortExpression="RangeStart" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRangeStart" runat="server" Text='<%#Eval("RangeStart") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RangeEnd" DefaultInsertValue="" HeaderText="RangeEnd"
                                                        DataField="RangeEnd" SortExpression="RangeEnd" AutoPostBackOnFilter="true"
                                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                        FilterControlWidth="98%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRangeEnd" runat="server" Text='<%#Eval("RangeEnd") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                                        AllowFiltering="false" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActive" runat="server" Text='<%#Eval("Active") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="80px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" FooterStyle-ForeColor="Blue"
                                                        FooterStyle-Font-Bold="true" HeaderStyle-Width="40px" >
                                                        <FooterStyle Font-Bold="True" ForeColor="Blue" />
                                                        <HeaderStyle Width="40px" />
                                                    </telerik:GridButtonColumn>
                                                </Columns>
                                                <EditFormSettings>
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                </EditFormSettings>
                                            </MasterTableView>
                                            <GroupingSettings CaseSensitive="False" />
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