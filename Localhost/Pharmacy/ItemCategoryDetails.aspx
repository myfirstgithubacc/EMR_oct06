<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ItemCategoryDetails.aspx.cs" Inherits="Pharmacy_ItemCategoryDetails"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function OnClientFindClose(oWnd, args) {
            $get('<%=btnFindClose.ClientID%>').click();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
        
        <div class="container-fluid header_main">
	        <div class="col-md-2">
		        <h2><asp:Label ID="lblDemographics" runat="server" Text='<%$ Resources:PRegistration, ItemCategory%>'
                            ToolTip='<%$ Resources:PRegistration, ItemCategory%>' /></h2>
	        </div>
	        <div class="col-md-7"><asp:Label ID="lblMessage" runat="server"  Text="&nbsp;" CssClass="relativ alert_new text-center text-success" /></div>
	        <div class="col-md-3 text-right pull-right">
        	    <asp:Button ID="btnNew" runat="server" Text="New" CssClass="btn btn-primary" ToolTip="New Record"
                            CausesValidation="false" OnClick="btnNew_OnClick" />
                        <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" AccessKey="S"  CssClass="btn btn-primary"
                            Text="Save" ToolTip="Save Data" />
	        </div>

        </div>
        
        
      
            
            
         
                <div class="subheading_main container-fluid">
                            <div class="col-md-2"><asp:Label ID="Label17" runat="server" Text='<%$ Resources:PRegistration, ItemCategory%>' /></div>
                            <div class="col-md-3">
                                     <asp:Label ID="lblAddCategory" runat="server" SkinID="label" Text='<%$  Resources:PRegistration, ItemCategory%>' />
                                     <asp:ImageButton ID="ibtnPopup" runat="server" ImageUrl="~/Images/PopUp.jpg" ToolTip="Add New Item Category"
                                        Height="18px" Width="17px" CausesValidation="false" OnClick="ibtnPopup_Click" />
                            </div>
                </div>
                
                
                
            <div class="container-fluid">
            
            <div class="row">
                <div class="col-md-6">
             
             <h5 class="h5">   
             <asp:Label ID="Label1" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, ItemSubCategory%>' />
             </h5>
             
              <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnl33" runat="server" ScrollBars="Auto" Height="490px" BorderWidth="1px"
                                    BorderColor="LightBlue">
                                    <telerik:RadGrid ID="gvInvoice" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                        AllowFilteringByColumn="true" AllowMultiRowSelection="false" runat="server" Width="100%"
                                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                        GridLines="Both" AllowPaging="True" Height="99%" PageSize="10" OnPageIndexChanged="gvInvoice_PageIndexChanged"
                                        OnPreRender="gvInvoice_PreRender" OnItemCommand="gvInvoice_ItemCommand" OnItemDataBound="gvInvoice_ItemDataBound">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
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
                                               <%-- <telerik:GridTemplateColumn UniqueName="ItemCategoryNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ItemCatNo%>'
                                                    DataField="ItemCategoryNo" SortExpression="ItemCategoryNo" AutoPostBackOnFilter="true"
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                    FilterControlWidth="99%" HeaderStyle-Width="70px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCategoryNo" runat="server" Text='<%#Eval("ItemCategoryNo")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>--%>
                                                <telerik:GridTemplateColumn UniqueName="ItemCategoryShortName" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ItemCatShortName%>'
                                                    DataField="ItemCategoryShortName" SortExpression="ItemCategoryShortName" AutoPostBackOnFilter="true"
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                    FilterControlWidth="99%" HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCategoryShortName" runat="server" Text='<%#Eval("ItemCategoryShortName")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                
                                                <telerik:GridTemplateColumn UniqueName="ItemCategoryName" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ItemCategory%>'
                                                    DataField="ItemCategoryName" SortExpression="ItemCategoryName" AutoPostBackOnFilter="true"
                                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                                    FilterControlWidth="99%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCategoryName" runat="server" Text='<%#Eval("ItemCategoryName")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn UniqueName="CategoryType" DataField="CategoryType" HeaderText='<%$ Resources:PRegistration, ItemNature%>'
                                                    HeaderStyle-Width="110px">
                                                    <FilterTemplate>
                                                        <telerik:RadComboBox ID="RadComboBoxCategoryType" Skin="Outlook" DataSource='<%#PopulateGridRadComboBoxCategoryType() %>'
                                                            DataValueField="CategoryType" DataTextField="CategoryType" AppendDataBoundItems="true"
                                                            SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("CategoryType").CurrentFilterValue %>'
                                                            runat="server" OnClientSelectedIndexChanged="RadComboBoxCategoryTypeIndexChanged"
                                                            Width="100%">
                                                        </telerik:RadComboBox>
                                                        <telerik:RadScriptBlock ID="RadCodeBlock3" runat="server">

                                                            <script language="javascript" type="text/javascript">
                                                                function RadComboBoxCategoryTypeIndexChanged(sender, args) {
                                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                                    var filterVal = args.get_item().get_value();
                                                                    if (filterVal == "All") {
                                                                        tableView.filter("CategoryType", filterVal, "DoesNotContain");
                                                                    }
                                                                    else {
                                                                        tableView.filter("CategoryType", args.get_item().get_value(), "EqualTo");
                                                                    }
                                                                }
                                                            </script>

                                                        </telerik:RadScriptBlock>
                                                    </FilterTemplate>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn UniqueName="Select" HeaderText='<%$ Resources:PRegistration, ItemSubCategory%>'
                                                    HeaderStyle-Width="75px" AllowFiltering="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSelect" runat="server" Text='<%$ Resources:PRegistration, ItemSubCategory%>'
                                                            CausesValidation="false" CommandName="Select" CommandArgument='<%# Eval("ItemCategoryId") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    
                    
                                             
                                            
                
                
                
                </div>
                
                
                
                
                
                
                
                
                
                
                
                <div class="col-md-6"> 
               <div class="row margin_bottom">
                <h5 class="h5  overflo">
                <asp:Label ID="Label7" runat="server"  Text='<%$ Resources:PRegistration, ItemCategory%>' />
                               
                                    <asp:Label ID="lblItemcategoryname" runat="server" SkinID="label" Text="" Font-Bold="true" />
                  </h5>
                  </div>
                  
                                    
                      <div class="row form-group">              
                        <div class="col-md-6">
                                <div class="col-md-5">
                                    <asp:Label ID="label11" runat="server" SkinID="label" ToolTip='<%$ Resources:PRegistration, ItemSubCategory%>'
                                        Text='<%$ Resources:PRegistration, ItemSubCategory%>' />
                                    <span style='color: Red'>*</span></div>
                                <div class="col-md-7"><asp:TextBox ID="txtName" SkinID="textbox" runat="server" Width="100%" MaxLength="100" /></div>
                        </div>
                        <div class="col-md-6">
                                <div class="col-md-5">
                                    <asp:Label ID="label12" runat="server" SkinID="label" Text="Short&nbsp;Name" />
                                    <span style='color: Red'>*</span></div>
                                <div class="col-md-7"><asp:TextBox ID="txtShortName" SkinID="textbox" runat="server" Width="100%" Style="text-transform: uppercase;"
                                        MaxLength="20" /></div>
                        </div>
                </div>
                
                      
                        <div class="row form-group">
                            
                            <div class="col-md-6">
                                <div class="col-md-5"><asp:Label ID="Label4" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' /></div>
                                <div class="col-md-7"><telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Active" Value="1" />
                                            <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                        </Items>
                                    </telerik:RadComboBox></div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="col-md-5">
                                                <asp:Label ID="lblSearch" runat="server" Text='<%$ Resources:PRegistration, ItemSubCategory%>'></asp:Label></div>
                                <div class="col-md-7">
                                    <asp:Panel ID="pnltxtSearchValue" runat="server" DefaultButton="btnSearchField">
                                                    <asp:TextBox ID="txtSearchValue" runat="Server" SkinID="textbox" Columns="27" MaxLength="50"></asp:TextBox>
                                                </asp:Panel>
                                </div>
                            </div>
                           
                                            
                        </div>
                                    
                           <div class="row form-group" style="display:none;">
                            <div class="col-md-12">
                                <div class="col-md-6">  <asp:Label ID="lblIsubCat" runat="server" SkinID="label" Text="Is Under Sub Group" />
                            </div>
                                <div class="col-md-6"><span id="trIsSubCat" runat="server" visible="true" class="" style="display:inline-flex">
                               
                                    
                              
                                    <asp:RadioButtonList ID="rblIsSubCategory" runat="server" OnSelectedIndexChanged="rblIsSubCategory_OnSelectedIndexChanged"
                                        AutoPostBack="true" RepeatDirection="horizontal" RepeatLayout="Flow" CssClass="flex">
                                        <asp:ListItem Text="Yes" Value="1" />
                                        <asp:ListItem Text="No" Value="0" Selected="True" />
                                    </asp:RadioButtonList>
                                
                            </span></div>
                            </div>
                                
                      </div> 
                      
                          
                         <div class="col-md-12  margin_bottom">
                             <asp:Button ID="btnSearchField" runat="server" Text="Search" CssClass="btn btn-primary pull-right" OnClick="btnSearchField_OnClick"
                                                    ValidationGroup="SearchField" ToolTip="Search Fields" />
                         
                         </div>       
                             
                            
                            
                            
                           
                                    
                              
                                    
                                
                                
                                
                                           
                                           
                                           
                                                
                                            
                                            
                                            
                                            
                                                
                                            
                                            
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="340px" BorderWidth="1px"
                                                BorderColor="LightBlue">
                                                <telerik:RadTreeList ID="RTLDetails" runat="server" ParentDataKeyNames="ParentId"
                                                    DataKeyNames="ItemSubCategoryId" AllowPaging="true" PageSize="10" AutoGenerateColumns="false"
                                                    AllowSorting="false" OnNeedDataSource="RTLDetails_NeedDataSource" OnPageIndexChanged="RTLDetails_PageIndexChanged"
                                                    OnItemCommand="RTLDetails_OnItemCommand" Skin="Office2007">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <Columns>
                                                        <telerik:TreeListTemplateColumn UniqueName="ItemSubCategoryId" DefaultInsertValue=""
                                                            HeaderText="ItemSubCategoryId" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemSubCategoryId" runat="server" Text='<%#Eval("ItemSubCategoryId") %>'
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                        <telerik:TreeListTemplateColumn UniqueName="MainParentId" DefaultInsertValue="" HeaderText="ItemSubCategoryId"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMainParentId" runat="server" Text='<%#Eval("MainParentId") %>'> </asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                        <telerik:TreeListTemplateColumn UniqueName="ParentId" DefaultInsertValue="" HeaderText="ParentID"
                                                            Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblParentId" runat="server" Text='<%#Eval("ParentId") %>'> </asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                        <telerik:TreeListTemplateColumn UniqueName="ItemSubCategoryName" HeaderText='<%$ Resources:PRegistration, ItemSubCategory%>'>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemSubCategoryName" runat="server" Text='<%#Eval("ItemSubCategoryName")%>' />
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                        <telerik:TreeListTemplateColumn UniqueName="ItemSubCategoryShortName" HeaderText="Short&nbsp;Name"
                                                            HeaderStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblItemSubCategoryShortName" runat="server" Text='<%#Eval("ItemSubCategoryShortName")%>' />
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                        <telerik:TreeListTemplateColumn UniqueName="Status" HeaderText='<%$ Resources:PRegistration, status%>'
                                                            HeaderStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                        <telerik:TreeListTemplateColumn HeaderStyle-Width="50px">
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="SelectButton" runat="server" Text="Select" CommandName="Select"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </telerik:TreeListTemplateColumn>
                                                    </Columns>
                                                </telerik:RadTreeList>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
           </div>   </div>  
                
            
            
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:Button ID="btnFindClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                OnClick="OnClientFindClose_OnClick" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
