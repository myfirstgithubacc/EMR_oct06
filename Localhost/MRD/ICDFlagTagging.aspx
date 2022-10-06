<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ICDFlagTagging.aspx.cs" Inherits="MRD_ICDFlagTagging" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

        <script type="text/javascript" src="/Include/JS/Functions.js" language="javascript">
        </script>

        <script type="text/javascript">
            function OnClientClose(oWnd, args) {
                $get('<%=btnGetInfo.ClientID%>').click();
            }
            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }

            function cboItem_OnClientSelectedIndexChanged(sender, args) {

                var item = args.get_item();
                $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=btnGetInfo.ClientID%>').click();
            }
        </script>

        <div>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <table width="100%" border="0" cellpadding="2" cellspacing="0">
                        <tr class="clsheader">
                            <td id="tdHeader" align="left" style="padding-left: 10px; width: 200px" runat="server">
                                <asp:Label ID="lblHeader" runat="server" SkinID="label" ToolTip="ICD&nbsp;Flag&nbsp;Tagging"
                                    Text="ICD&nbsp;Flag&nbsp;Tagging" Font-Bold="true" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" SkinID="Button"
                                    Text="New" OnClick="btnNew_OnClick" />
                                <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                    SkinID="Button" ValidationGroup="SaveData" Text="Save" />
                            </td>
                        </tr>
                    </table>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                                <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                            </td>
                           
                        </tr>
                    </table>
                    <table border="0" style="margin-left: 10px;" cellpadding="2" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label ID="label155" runat="server" SkinID="label" ToolTip="ICD(s)" Text="ICD Code" />&nbsp;<span
                                    style='color: Red'>*</span>
                            </td>
                            <td>
                            <telerik:RadComboBox runat="server" ID="ddlIcd" Width="340px" Height="400px" AutoPostBack="true"
                                  EnableLoadOnDemand="true" HighlightTemplatedItems="true" EmptyMessage="ICD Code,Description"
                                  DropDownWidth="600px"
                                  OnItemsRequested="RadComboBoxProduct_ItemsRequested" ShowMoreResultsBox="true"
                                  EnableVirtualScrolling="true" 
                                  onselectedindexchanged="ddlIcd_SelectedIndexChanged1" >
                            <HeaderTemplate>
                            <table style="width: 570px" cellspacing="0" cellpadding="0">
                               <tr>
                               <td style="width: 100px" align="left">
                                  ICD Code
                               </td>
                               <td style="width: 200px" align="left">
                                  Description
                               </td>
                               </tr>
                             </table>
                             </HeaderTemplate>
                     <ItemTemplate>
                     <table style="width: 570px" cellspacing="0" cellpadding="0">
                          <tr>
                            <td style="width: 100px">
                               <%# DataBinder.Eval(Container, "Attributes['ICDCode']")%>
                            </td>
                            <td style="width: 200px;" align="left">
                               <%# DataBinder.Eval(Container, "Attributes['Description']")%>
                            </td>
                          </tr>
                     </table>
                     </ItemTemplate>
                    </telerik:RadComboBox>
                              <%-- <telerik:RadComboBox ID="ddlIcd" runat="server" MarkFirstMatch="true" 
                                    DropDownWidth="400px"  Width="400px" AppendDataBoundItems="true" 
                                    EmptyMessage="Select" onselectedindexchanged="ddlIcd_SelectedIndexChanged">
                               </telerik:RadComboBox>--%>
                            </td>
                        </tr>
                    </table>
                    <table border="0" style="margin-left: 10px;" cellpadding="4" cellspacing="0">
                        <tr>
                            <td>
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="chkUnchk" runat="server" Text="All&nbsp;Select&nbsp;/&nbsp;Unselect&nbsp;Flag(s)"
                                    AutoPostBack="true" OnCheckedChanged="chkUnchk_OnCheckedChanged" />
                            </td>
                            <td align="right" style="padding-left:50px">
                                <asp:Label ID="label1" runat="server" SkinID="label" Text="Add&nbsp;New&nbsp;ICD Code&nbsp;Flag" />
                            </td>
                            <td>
                                <asp:ImageButton ID="imgBtnChargeType" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                    ToolTip="Add&nbsp;New&nbsp;ICD Code&nbsp;Flag" Height="18px" Visible="true" CausesValidation="false"
                                     OnClick="imgBtnChargeType_Click"/>
                            </td>
                            <td style="padding-left:50px">
                                <asp:Label ID="label2" runat="server" SkinID="label" Font-Bold="true" Text="Tagged&nbsp;ICD Code&nbsp;Flag" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 350px;">
                                <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Auto">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="gvTax" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                                AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                GridLines="Both" AllowPaging="false" Height="99%" >
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="chkCollection" HeaderText="" HeaderStyle-Width="30px"
                                                            AllowFiltering="false">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkCollection" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="ICDFlagId" HeaderText="ICDFlagId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemFlagId" runat="server" Text='<%#Eval("ICDFlagId")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="ICDFlagName" DefaultInsertValue="" HeaderText="ICDFlagName"
                                                            DataField="ICDFlagName" SortExpression="ICDFlagName" AutoPostBackOnFilter="true"
                                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                                            FilterControlWidth="99%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemFlagName" runat="server" Text='<%#Eval("ICDFlagName")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                            <td style="width: 350px;padding-left:50px">
                                <asp:Panel ID="Panel2" runat="server" Height="400px" ScrollBars="Auto">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="gvTagged" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                                AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                GridLines="Both" AllowPaging="false" Height="99%">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="ICDFlagId" HeaderText="ICDFlagId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblICDFlagId" runat="server" Text='<%#Eval("ICDFlagId")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="ICDFlagName" DefaultInsertValue="" HeaderText="ICD Flag(s)"
                                                            DataField="ICDFlagName" SortExpression="ICDFlagName" AutoPostBackOnFilter="true"
                                                            CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                                            FilterControlWidth="99%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblICDFlagName" runat="server" Text='<%#Eval("ICDFlagName")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnGetInfo" runat="server" Text="GetInfo" CausesValidation="false"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfo_Click"/>
                    <asp:HiddenField ID="hdnItemId" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

