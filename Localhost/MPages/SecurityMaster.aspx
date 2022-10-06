<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SecurityMaster.aspx.cs" Inherits="MPages_SecurityMaster" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />   

    <style>
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {border-top:none !important; background: 0 -2300px repeat-x #c1e5ef !important; color:#333 !important;}
        .RadGrid_Office2007 .rgGroupHeader { background: #c1e5ef !important; color: #567db0;}    
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol { background: #c1e5ef none !important; border-color: #d7e6f7;}
        .textName { width:150px; float:left;}
    </style>


    <script type="text/javascript">
        function Unselect() {
            var OptionButton = document.getElementById('<%=ddlGroupName.ClientID %>')
            OptionButton[0].selected = "0";
        }
        function ClearText() {
            var txt = document.getElementById('<%=txtGroupName.ClientID %>')
            txt.value = '';
        }
    </script>

    <asp:UpdatePanel ID="update" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-3"><h2>Roles & Permissions</h2></div>
                <div class="col-md-7 col-sm-7 text-center"><asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Green" /></div>
                <div class="col-md-2 col-sm-2 text-right"><asp:Button ID="btnSave" Text="Save Permissions" CssClass="btn btn-primary" runat="server" OnClick="btnSave_Click" /></div>
            </div>


            <div class="container-fluid">

                <div class="row form-group">
                    <div class="col-md-3 col-sm-4">
                        <div class="row form-groupTop01">
                            <div class="col-md-5 col-sm-4 label2 PaddingRightSpacing"><span class="textName"><asp:Label ID="lblGroupName" runat="server" Text="Create New Group"></asp:Label></span></div>
                            <div class="col-md-5 col-sm-6 PaddingRightSpacing"><asp:TextBox ID="txtGroupName" runat="server" MaxLength="100" Width="100%"></asp:TextBox></div>
                            <div class="col-md-2 col-sm-2">
                                <asp:Button ID="btnCreateGroup" runat="server" OnClick="btnCreateGroup_Click" CssClass="btn btn-primary" Text="Create" />
                                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CssClass="btn btn-primary" Text="Update" />
                            </div>
                        </div>

                        <div class="row form-groupTop01">
                            <div class="col-md-5 col-sm-4"></div>
                            <div class="col-md-7 col-sm-8 PaddingRightSpacing"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkIsAdmin" runat="server" Text="Administrator Group" /></div></div>
                        </div>

                    </div>

                    <div class="col-md-9 col-sm-8"></div>
                </div>

                <hr style="width:100%; float:left; margin:5px 0;" />




                <div class="row form-group">
                    <div class="col-md-3 col-sm-4">
                        
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12"><asp:Label ID="Label1" runat="server" Text="Module Name"></asp:Label></div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12">
                                <asp:DropDownList ID="ddlModuleName" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged" Width="100%">
                                    <asp:ListItem Text="Select Module" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-9 col-sm-8">
                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12"><asp:Label ID="Label2" runat="server" Text="Group Name"></asp:Label></div>
                        </div>
                        <div class="row form-group">
                            <div class="col-md-3 col-sm-4 PaddingRightSpacing">
                                <asp:DropDownList ID="ddlGroupName" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlGroupName_SelectedIndexChanged" Width="100%">
                                    <asp:ListItem Selected="True" Text="Select Group" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                               <%-- <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" SkinID="Button"
                                    Text="Search" />--%>
                            </div>
                            <div class="col-md-9 col-sm-8 PaddingRightSpacing">
                                <asp:Button ID="btnGroupDepartment" runat="server" OnClick="btnGroupDepartment_Click" CssClass="btn btn-primary" Text="Group&nbsp;Department(s)&nbsp;Tagging" />
                                <asp:Button ID="btnGroupOptionEdit" runat="server" CssClass="btn btn-primary" Text="Group Option Tagging" onclick="btnGroupOptionEdit_Click" />
                                <asp:Button ID="btnDashboardPermission" runat="server" OnClick="btnDashboardPermission_Click" CssClass="btn btn-primary" Text="Edit Dashboard Permission" />
                                <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" CssClass="btn btn-primary" Text="Edit Group" />
                                <span id="pnlUpdate" runat="server">
                                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn btn-primary" Text="Cancel" />
                                </span>
                                <asp:Label ID="lblmsg" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>


                 <div class="row form-group">
                    <div class="col-md-3 col-sm-4">
                        <div class="row form-group">
                            <div style="border: 1px solid #000000; overflow: scroll; height: 375px">
                                <asp:GridView ID="gvPages" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvPages_RowDataBound"
                                    OnSelectedIndexChanged="gvPages_SelectedIndexChanged" ShowHeader="true" SkinID="gridviewOrderNew"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="PageID" />
                                        <asp:BoundField DataField="ModuleID" />
                                        <asp:BoundField DataField="ModuleName" />
                                        <asp:BoundField DataField="PageName" HeaderText="Module Pages" />
                                        <asp:CommandField ShowSelectButton="true" SelectText="Select" ControlStyle-ForeColor="Blue">
                                            <ItemStyle Width="35px" />
                                        </asp:CommandField>
                                    </Columns>
                                </asp:GridView>
                                <telerik:RadGrid ID="RadgvPages" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="False"
                                    AllowSorting="False" ShowGroupPanel="false" GridLines="none" OnItemDataBound="RadgvPages_OnItemDataBound"
                                    OnSelectedIndexChanged="RadgvPages_OnSelectedIndexChanged" Width="100%" Skin="Office2007">
                                    <ClientSettings>
                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                    </ClientSettings>
                                    <MasterTableView Width="100%">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Module Page(s)">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnPageID" runat="server" Value='<%#Eval("PageID") %>' />
                                                    <asp:HiddenField ID="HdnParentId" runat="server" Value='<%#Eval("ParentId") %>' />
                                                    <asp:HiddenField ID="hdnModuleID" runat="server" Value='<%#Eval("ModuleID") %>' />
                                                    <asp:HiddenField ID="hdnModuleName" runat="server" Value='<%#Eval("ModuleName") %>' />
                                                    <asp:Label ID="lbl_PageName" runat="server" Text='<%#Eval("PageName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridButtonColumn Text="Select" ItemStyle-ForeColor="Blue" CommandName="Select"
                                                ItemStyle-Width="35px" HeaderText="">
                                            </telerik:GridButtonColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>

                        <div class="row form-group text-right">
                            <asp:Button ID="btnAll" runat="server" CssClass="btn btn-primary" Text="Select All" OnClick="btnAll_Click" />
                        </div>
                        

                    </div>

                    <div class="col-md-9 col-sm-8">
                        <div class="row form-group">
                            <telerik:RadGrid ID="gvSecurity" runat="server" 
                                Skin="Office2007" Width="100%" AllowSorting="true"  Selecting-AllowRowSelect="true"
                                AllowPaging="false" ShowGroupPanel="false" GridLines="none" OnItemDataBound="gvSecurity_ItemDataBound"
                                OnItemCommand="gvSecurity_ItemCommand" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                Height="375px">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <GroupHeaderItemStyle Font-Bold="True" />
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True" FrozenColumnsCount="1">
                                    </Scrolling>
                                </ClientSettings>
                                <MasterTableView Width="100%">
                                    <GroupByExpressions>
                                        <telerik:GridGroupByExpression>
                                            <SelectFields>
                                                <telerik:GridGroupByField FieldName="ModuleName" FieldAlias=":" SortOrder="None">
                                                </telerik:GridGroupByField>
                                            </SelectFields>
                                            <GroupByFields>
                                                <telerik:GridGroupByField FieldName="ModuleName" SortOrder="None"></telerik:GridGroupByField>
                                            </GroupByFields>
                                        </telerik:GridGroupByExpression>
                                    </GroupByExpressions>
                                    <RowIndicatorColumn Visible="True">
                                    </RowIndicatorColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn SortExpression="Id" HeaderText="Id" DataField="Id" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Page Id" HeaderStyle-Width="35px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpageid" runat="server" Text='<%#Eval("PageId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="35px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Page Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("PageName") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnModuleId" runat="server" Value='<%#Eval("ModuleId") %>' />
                                                <asp:HiddenField ID="hdnModuleName" runat="server" Value='<%#Eval("ModuleName") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="New" HeaderStyle-Width="35px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkInsertData" runat="server" Checked='<%#Eval("InsertData") %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="35px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Edit" HeaderStyle-Width="35px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEditData" runat="server" Checked='<%#Eval("EditData") %>' Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="35px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Cancel" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCancelData" runat="server" Checked='<%#Eval("CancelData") %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Display" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkDisplayData" runat="server" Checked='<%#Eval("DisplayData") %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Print" HeaderStyle-Width="35px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkPrintData" runat="server" Checked='<%#Eval("PrintData") %>'
                                                    Enabled="false" />
                                            </ItemTemplate>
                                            <HeaderStyle Width="35px" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn Text="Delete" CommandName="Delete" HeaderText="Delete"
                                            HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-ForeColor="Blue">
                                            <HeaderStyle Width="90px" />
                                            <ItemStyle Width="90px" />
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                                <GroupingSettings ShowUnGroupButton="true" />
                            </telerik:RadGrid>
                        </div>

                        <div class="row form-group">
                            <div class="col-md-2"><span class="pull-left">Legend</span>  <span runat="server" style="width: 40px; height:20px; float:left; margin:0 0 0 5px; padding:0; background:#ffc0cb;;"></span></div>
                            <div class="col-md-3">Administrator Group</div>
                        </div>
                        
                    </div>
                </div>
            </div>


            <asp:HiddenField ID="hdnGroupId" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
