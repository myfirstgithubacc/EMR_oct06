<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ItemCategoryMaster.aspx.cs"
    Inherits="Pharmacy_ItemCategoryMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    
     <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>

        <script type="text/javascript">
            function OnClientClose(oWnd, args) {
                $get('<%=btnGetStatus.ClientID%>').click();
            }
        </script>

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                    </Windows>
                </telerik:RadWindowManager>
               
               
               
               <div class="container-fluid header_main margin_bottom">
	            <div class="col-md-3 col-xs-7">
                         <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" CssClass="alert_new text-danger text-success" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
	            </div>
	            <div class="col-md-5 col-xs-5 text-right pull-right">
            	<asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record"  CssClass="btn btn-primary"
                                Text="New" OnClick="btnNew_OnClick" />
                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                CssClass="btn btn-primary" Text="Save" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary"
                                OnClientClick="window.close();" />
	            </div>

            </div>
               
               
             <div class="container-fluid">
                     <div class="row form-group">
                            <div class="col-md-6 col-xs-6">
                                    <div class="col-md-6 col-xs-6"><asp:Label ID="Label1" runat="server" SkinID="label" ToolTip='<%$ Resources:PRegistration, ItemCategory%>'
                                Text='<%$ Resources:PRegistration, ItemCategory%>' />
                            &nbsp;<span style='color: Red'>*</span></div>
                                    <div class="col-md-6 col-xs-6"><asp:TextBox ID="txtCategoryName" SkinID="textbox" runat="server" Width="100%" MaxLength="100" /></div>
                            </div>
                            <div class="col-md-6 col-xs-6">
                                    <div class="col-md-6 col-xs-6"><asp:Label ID="Label4" runat="server" SkinID="label" Text="Short Name" />
                            <span style='color: Red'>*</span></div>
                                    <div class="col-md-6 col-xs-6"><asp:TextBox ID="txtShortName" SkinID="textbox" runat="server" Width="100%" MaxLength="5" /></div>
                            </div>
                     </div>
                     
                     <div class="row form-group">
                        <div class="col-md-6 col-xs-6">
                            <div class="col-md-6 col-xs-6"> <asp:Label ID="Label2" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, ItemNature%>' />
                            &nbsp;<span style='color: Red'>*</span></div>
                            <div class="col-md-6 col-xs-6"> 
                            <telerik:RadComboBox ID="ddlCategoryType" SkinID="DropDown" runat="server" Width="100%"
                                MarkFirstMatch="true" EmptyMessage="[ Select ]" />
                            <asp:ImageButton ID="imgBtnItemNature" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                ToolTip="Add&nbsp;New&nbsp;Item&nbsp;Nature" Height="18px" Visible="false" CausesValidation="false"
                                OnClick="imgBtnItemNature_Click" />
                            </div>
                        </div>
                        
                        <div class="col-md-6 col-xs-6">
                            <div class="col-md-6 col-xs-6"><asp:Label ID="Label3" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' /></div>
                            <div class="col-md-6 col-xs-6"> <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="100%">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Active" Value="1" />
                                    <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                </Items>
                            </telerik:RadComboBox></div>
                        </div>
                        
                        
                     </div>
             </div>
                
                
                
                
                
              
                
                
                
              
                <table border="0" width="100%" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%" Height="200px">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvDetails" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                            AllowFilteringByColumn="false" AllowMultiRowSelection="False" runat="server"
                                            AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                            GridLines="Both" AllowPaging="True" PageSize="10" OnPageIndexChanged="gvDetails_PageIndexChanged"
                                            OnPreRender="gvDetails_PreRender" OnSelectedIndexChanged="gvDetails_OnSelectedIndexChanged"
                                            OnItemDataBound="gvDetails_ItemDataBound" OnItemCreated="gvDetails_ItemCreated">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="ItemCategoryId" DefaultInsertValue="" HeaderText="CategoryId"
                                                        Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemCategoryId" runat="server" Text='<%#Eval("ItemCategoryId") %>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ItemCategoryName" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ItemCategory%>'
                                                        AllowFiltering="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemCategoryName" runat="server" Text='<%#Eval("ItemCategoryName") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ItemCategoryShortName" DefaultInsertValue=""
                                                        HeaderText='Short Name' AllowFiltering="false" HeaderStyle-Width="80px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemCategoryShortName" runat="server" Text='<%#Eval("ItemCategoryShortName") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="CategoryType" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, itemNature%>'
                                                        AllowFiltering="false" HeaderStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCategoryType" runat="server" Text='<%#Eval("CategoryType") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="UniqueName5" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, status%>'
                                                        AllowFiltering="false" HeaderStyle-Width="60px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="50px"
                                                        HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </td>
                        <tr>
                            <td>
                                <asp:Button ID="btnGetStatus" runat="server" Text="GetInfo" CausesValidation="true"
                                    SkinID="button" Style="visibility: hidden;" OnClick="btnGetStatus_Click" />
                            </td>
                        </tr>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
