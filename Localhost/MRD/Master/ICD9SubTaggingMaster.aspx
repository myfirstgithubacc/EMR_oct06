<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" 
CodeFile="ICD9SubTaggingMaster.aspx.cs" Inherits="MRD_Master_ICD9SubTaggingMaster"  Title="" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



      

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  <script type="text/javascript">
            function OnClientClose(oWnd, args) {
                $get('<%=btnGetInfo.ClientID%>').click();
            }
            </script>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                    </Windows>
                </telerik:RadWindowManager>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr class="clsheader">
                    <td id="Td1" align="left" style="padding-left: 10px; width: 150px" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Donor Type Master"
                            ToolTip="" Font-Bold="true" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnNew" runat="server" ToolTip="New Record" SkinID="Button" Text="New"
                            OnClick="btnNew_OnClick" />
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" OnClick="btnSaveData_OnClick"
                            SkinID="Button" Text="Save" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false"
                            SkinID="Button" OnClientClick="window.close();" />
                    </td>
                </tr>
            </table>
            <table border="0" width="98%" cellpadding="3" cellspacing="0" style="margin-left: 8px;
                margin-right: 8px">
                <tr>
                    <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                        <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="4" style="margin-left: 10px;">
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Group" />
                        <span style="color: Red">*</span>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlGroup" runat="server" Width="200px"
                            EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" 
                            onselectedindexchanged="ddlDonationType_SelectedIndexChanged1" />
                   
                                <asp:ImageButton ID="imgBtnAddGroup" runat="server" Enabled="false" ImageUrl="~/Images/PopUp.jpg"
                                    ToolTip="Add Group" Height="18px" Visible="true" CausesValidation="false"
                                     OnClick="imgBtnAddGroup_Click"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSubGroup" runat="server" SkinID="label" Text="Sub Group" />
                        <span style="color: Red">*</span>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubGroup" runat="server" Width="200px" AutoPostBack="true"
                            EmptyMessage="[ Select ]" MarkFirstMatch="true" CheckBoxes="false"  EnableCheckAllItemsCheckBox="false"
                            onselectedindexchanged="ddlDonorType_SelectedIndexChanged" />                   
                    
                                <asp:ImageButton ID="imgBtnAddSubGroup" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                    ToolTip="Add Sub Group" Height="18px" Visible="true" CausesValidation="false"
                                     OnClick="imgBtnAddSubGroup_Click"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblIcd9Description" runat="server" SkinID="label" 
                            Text="ICD10 Description" />
                        <span style="color: Red">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtICD9Desc" SkinID="textbox" runat="server" Width="400px"
                            MaxLength="250" Style="text-transform: uppercase;" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblICD9code" runat="server" SkinID="label" Text="ICD10 Code" />
                        <%--<span style="color: Red">*</span>--%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtICD9Code" SkinID="textbox" runat="server" Width="40px"
                            MaxLength="20" Style="text-align: left" />
<%--                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                            FilterType="Custom, Numbers" TargetControlID="txtICD9Code">
                        </cc1:FilteredTextBoxExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' />
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" Enabled="false" runat="server" Width="80px">
                            <Items>
                                <telerik:RadComboBoxItem Text="Active" Value="1" />
                                <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                     <td>
                        <asp:Button  ID="btnSubmit" runat="server" SkinID="Button" Text="Refresh" 
                             onclick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" 
                            PagerStyle-ShowPagerText="false" AllowMultiRowSelection="false" 
                            runat="server" Width="100%"
                            AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                            GridLines="Both" AllowPaging="True" Height="350px" PageSize="10" OnPageIndexChanged="gvData_OnPageIndexChanged"
                            OnItemCommand="gvData_OnItemCommand" 
                            OnItemDataBound="gvData_ItemDataBound" onprerender="gvData_PreRender" 
                            AllowCustomPaging="True">
                            <GroupingSettings CaseSensitive="false" />
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <PagerStyle ShowPagerText="False" />
                            <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="False">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.</div>
                                </NoRecordsTemplate>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                                <ItemStyle Wrap="true" />
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                    Visible="True">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                    Visible="True">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="ICDCode" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="ICDCode" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="ICD Code" FilterControlWidth="99%" ItemStyle-Wrap="true" HeaderStyle-Wrap="false"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Width="150px" Wrap="False" />
                                        <ItemStyle Wrap="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Description" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="Description" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="Description" FilterControlWidth="99%" ItemStyle-Wrap="true" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Description")%>' />
                                            <asp:HiddenField ID="hdnICDId" runat="server" Value='<%#Eval("ICDId")%>' />
                                            <asp:HiddenField ID="hdnStatus" runat="server" Value='<%#Eval("Status")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="GroupName" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="GroupName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="StartsWith" HeaderText="Group Name" FilterControlWidth="99%"
                                        ItemStyle-Wrap="true" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroupName" runat="server" Text='<%#Eval("GroupName")%>' />
                                            <asp:HiddenField ID="hdnGroupid" runat="server" Value='<%#Eval("GroupId")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="True" />
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn UniqueName="SubGroupName" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="SubGroupName" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="StartsWith" HeaderText="Sub Group Name" FilterControlWidth="99%"
                                        ItemStyle-Wrap="true" HeaderStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubGroupName" runat="server" Text='<%#Eval("SubGroupName")%>' />
                                            <asp:HiddenField ID="hdnSubGroupId" runat="server" Value='<%#Eval("SubGroupId")%>' />
                                        </ItemTemplate>
                                        <HeaderStyle Wrap="False" />
                                        <ItemStyle Wrap="True" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="50px"
                                        HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" 
                                        ItemStyle-HorizontalAlign="Center" >
                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
             <asp:Button ID="btnGetInfo" runat="server" Text="GetInfo" CausesValidation="false"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfo_Click"/>
                    <asp:HiddenField ID="hdnItemId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
