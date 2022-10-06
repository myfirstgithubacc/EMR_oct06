<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ItemBrandMaster.aspx.cs" Inherits="Pharmacy_ItemBrandMaster" Title="Item Brand Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

        <script type="text/javascript">

            function OnClientClose(oWnd, args) {
                $get('<%=btnGetGeneric.ClientID%>').click();
            }
            function OnClientCloseBrand(oWnd, args) {
                $get('<%=btnBrandMst.ClientID%>').click();
            }
            function OnClientCloseStrength(oWnd, args) {
                $get('<%=btnStrengthMst.ClientID%>').click();
            }

            function OnClientClosePacking(oWnd, args) {
                $get('<%=btnPackingMst.ClientID%>').click();
            }

            function OnClientCIMSClose(oWnd, args) {
                $get('<%=btnGetCIMS.ClientID%>').click();
            }

            function OnClientManufactureClose(oWnd, args) {
                $get('<%=btnGetManufacture.ClientID%>').click();
            }
            function OnClientItemUnitClose(oWnd, args) {
                $get('<%=btnItemUnitClose.ClientID%>').click();
            }

            function OnClientFindClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var ItemId = arg.ItemId;

                    $get('<%=hdnItemID.ClientID%>').value = ItemId;
                }
                $get('<%=btnFindClose.ClientID%>').click();

            }
            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }

            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }

            function MaxLenTxt(TXT) {
                if (TXT.value.length > 500) {
                    alert("Text length should not be greater then 500 ...");

                    TXT.value = TXT.value.substring(0, 500);
                    TXT.focus();
                }
            }

            function SelectOnlyOne(spanChk) {
                var IsChecked = spanChk.checked;

                var CurrentRdbID = spanChk.id;
                var Chk = spanChk;
                Parent = document.getElementById("<%=gvItemUnit.ClientID%>");
                var items = Parent.getElementsByTagName('input');
                for (i = 0; i < items.length; i++) {
                    if (items[i].id != CurrentRdbID && items[i].type == "radio") {
                        if (items[i].checked) {
                            items[i].checked = false;
                        }
                    }
                }
            }
            function SelectOnlyOneChk(spanChk) {
                var IsChecked = spanChk.checked;

                var CurrentRdbID = spanChk.id;
                var Chk = spanChk;
                Parent = document.getElementById("<%=gvItemUnit.ClientID%>");
                var items = Parent.getElementsByTagName('input');
                var Row = 0;
                for (i = 0; i < items.length; i++) {
                    if (items[i].id != CurrentRdbID && items[i].type == "checkbox") {
                        if (items[i].checked) {
                            items[i].checked = false;
                        }
                    }
                    else if (items[i].id == CurrentRdbID && items[i].type == "checkbox") {
                        $get('<%=btnItemchk.ClientID%>').click();
                    }

                }
            }
            function SelectAllItemUnit(id) {
                //get reference of GridView control
                var grid = document.getElementById("<%=gvItemUnit.ClientID%>");
                //variable to contain the cell of the grid
                var cell;

                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (ridx = 1; ridx < grid.rows.length; ridx++) {
                        //get the reference of first column
                        cell = grid.rows[ridx].cells[0];

                        //loop according to the number of childNodes in the cell
                        for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                            //if childNode type is CheckBox
                            if (cell.childNodes[cIdx].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell checkbox within the grid
                                cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }
            }

            function OnClientSelectedIndexChangingHandlerddlStatus(sender, eventArgs) {

                var selectedValue = eventArgs.get_item().get_value();

                var chkPurchaseClose = document.getElementById("<%=chkPurchaseClose.ClientID%>");
                var chkSaleClose = document.getElementById("<%=chkSaleClose.ClientID%>");

                if (selectedValue == 1) {
                    chkPurchaseClose.checked = false;
                    chkPurchaseClose.disabled = false;

                    chkSaleClose.checked = false;
                    chkSaleClose.disabled = false;
                }
                else {
                    chkPurchaseClose.checked = true;
                    chkPurchaseClose.disabled = true;

                    chkSaleClose.checked = true;
                    chkSaleClose.disabled = true;
                }
            }
            
        </script>

        <div>
            <asp:UpdatePanel ID="upItemBrandMaster" runat="server">
                <ContentTemplate>
                   
                   
                   <div class="container-fluid header_main">
	                <div class="col-md-3" id="tdHeader" runat="server">
		                <h2> <asp:Label ID="lblHeader" runat="server"  Text="Item&nbsp;Master" ToolTip="Item&nbsp;Master"/></h2>
	                </div>
	                
	                
	                <div class="col-md-5 text-right pull-right">
                	         <asp:HiddenField ID="hdnItemID" runat="server" />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" ToolTip="Search"  CssClass="btn btn-primary"
                                    CausesValidation="false" OnClick="btnSearch_OnClick" />
                                <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record"  CssClass="btn btn-primary"
                                    Text="New" OnClick="btnNew_OnClick" />
                                <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                     CssClass="btn btn-primary" ValidationGroup="SaveData" Text="Save" />
                                <asp:Button ID="btnItemchk" runat="server" OnClick="btnItemchk_OnClick" Style="visibility: hidden;" />
                                &nbsp;
	                </div>

                </div>
                   
                   
                   <div class="container-fluid subheading_main">
                        <div class="col-md-3"><asp:Label ID="lblIBD" runat="server" Text="Item&nbsp;Details"></asp:Label></div>
                        <div class="col-md-7"> <asp:Label ID="lblMessage"  CssClass="relativ alert_new text-center" runat="server" Text="&nbsp;" /></div>
                        <div class="col-md-2  text-right"> <asp:HyperLink ID="HyperLink1" Style="text-decoration: none;" onmouseover="LinkBtnMouseOver(this.id);"
                                    onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" NavigateUrl="/Pharmacy/ItemMasterDetails.aspx"
                                    runat="server">Item Master Details</asp:HyperLink></div>
                   </div>
                   
               
                    
                    
                    
                    
                    <div class="container-fluid form-group">
                        <div class="row form-group">
                                <div class="col-md-4">
                                    <div class="col-md-4"><asp:Label ID="lblMajorGroup" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, ItemCategory%>'></asp:Label></div>
                                    <div class="col-md-8"><telerik:RadComboBox ID="ddlItemCategory" SkinID="DropDown" runat="server" Width="98%"
                                                EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlItemCategory_OnSelectedIndexChanged" /></div>
                                </div>
                                <div class="col-md-6"><asp:LinkButton ID="lnkItemSubDetail" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                                onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" Text="Sub Group" OnClick="lnkItemSubDetail_OnClick" />
                                            <span style='color: Red'>*</span></div>
                        </div>
                    
                        <div class="row form-group">
                                
                                <asp:Label ID="lblSubGroupName" SkinID="label" runat="server" />
                                    <asp:Label ID="lblSubGroupShortName" SkinID="label" runat="server" Visible="false" />
                                <div class="col-md-12">
                                     <div class="row">
                                     
                                     <span id="trRTLDetail" runat="server" visible="false">
                                       
                                            <div class="col-md-4">
                                                <div class="col-md-4"><asp:Label ID="lblSearch" runat="server" Text='<%$ Resources:PRegistration, ItemSubCategory%>'></asp:Label></div>
                                                <div class="col-md-8"> <asp:Panel ID="pnltxtSearchValue" runat="server" DefaultButton="btnSearchField">
                                                                        <asp:TextBox ID="txtSearchValue" runat="Server" SkinID="textbox" Columns="27" MaxLength="50" Width="100%"></asp:TextBox>
                                                                    </asp:Panel></div>
                                            </div>
                                            <div class="col-md-1">
                                                 <asp:Button ID="btnSearchField" runat="server" Text="Search" CssClass="btn btn-block btn-primary" OnClick="btnSearchField_OnClick"
                                                                        ValidationGroup="SearchField" ToolTip="Search Fields" />
                                            </div>                        
                                              <div class="col-md-7">
                                                         
                                                    <span id="tdGrid" runat="server" valign="top">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="125px" BorderWidth="1px"
                                                                    BorderColor="LightBlue">
                                                                    <telerik:RadTreeList ID="RTLDetails" runat="server" Skin="Office2007" ParentDataKeyNames="ParentId"
                                                                        Height="99%" DataKeyNames="ItemSubCategoryId" AllowPaging="false" AutoGenerateColumns="false"
                                                                        AllowSorting="false" OnNeedDataSource="RTLDetails_NeedDataSource" OnPageIndexChanged="RTLDetails_PageIndexChanged"
                                                                        OnItemCommand="RTLDetails_OnItemCommand" OnItemDataBound="RTLDetails_OnItemDataBound">
                                                                        <NoRecordsTemplate>
                                                                        </NoRecordsTemplate>
                                                                        <Columns>
                                                                            <telerik:TreeListTemplateColumn UniqueName="ItemSubCategoryId" DefaultInsertValue=""
                                                                                HeaderText="ItemSubCategoryId" Visible="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemSubCategoryId" runat="server" Text='<%#Eval("ItemSubCategoryId") %>'
                                                                                        Visible="false" />
                                                                                    <asp:HiddenField ID="hdnIsLeafCat" runat="server" Value='<%#Eval("IsLeafCat") %>' />
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
                                                                             <telerik:TreeListTemplateColumn UniqueName="ItemSubCategoryShortName" HeaderText="ShortName" HeaderStyle-Width="15%" ItemStyle-Width="15%" >
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemSubCategoryShortName" runat="server" Text='<%#Eval("ItemSubCategoryShortName")%>' />
                                                                                </ItemTemplate>
                                                                            </telerik:TreeListTemplateColumn>
                                                                            <telerik:TreeListTemplateColumn HeaderStyle-Width="50px" HeaderText="Select">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkBtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                                                </ItemTemplate>
                                                                            </telerik:TreeListTemplateColumn>
                                                                        </Columns>
                                                                    </telerik:RadTreeList>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="ddlItemCategory" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </span>
                                              
                                              </div>                  
                                               
                                    </span>
                                </div>
                                </div>
                        </div>
                    
                    <div class="row form-group">
                     <div class="col-md-12">
                     <div   class="row">
                     <span  id="trBrand" runat="server">
                     <div class="col-md-4">
                            <div class="col-md-4"> <asp:Label ID="label7" runat="server" SkinID="label" ToolTip="Brand" Text="Brand" /></div>
                            <div class="col-md-7">
                                <telerik:RadComboBox ID="ddlBrand" SkinID="DropDown" runat="server" Width="100%" EmptyMessage="[ Select ]" ShowMoreResultsBox="true" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlBrand_OnSelectedIndexChanged" CssClass="margin_z" />
                                
                             </div>
                            <div class="inline-bl"> <asp:ImageButton ID="imgBtnBrand" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                                            ToolTip="Add&nbsp;New&nbsp;Brand&nbsp;Master" Height="18px" Visible="true" CausesValidation="false"
                                                            OnClick="imgBtnBrand_Click" /></div>
                        </div>
                        
                        
                        <div class="col-md-4">
                                <div class="col-md-4"><asp:Label ID="label8" runat="server" SkinID="label" ToolTip="Strength" Text="Strength" /></div>
                                <div class="col-md-7"><telerik:RadComboBox ID="ddlStrength" SkinID="DropDown" runat="server" Width="98%"
                                                            ShowMoreResultsBox="true" DropDownWidth="500px" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlStrength_OnSelectedIndexChanged" /></div>
                                <div class="inline-bl"><asp:ImageButton ID="imgBtnStrength" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                                            ToolTip="Add&nbsp;New&nbsp;Strength&nbsp;Master" Height="19px" Visible="true"
                                                            CausesValidation="false" OnClick="imgBtnStrength_Click" /></div>
                        </div>
                             <div class="col-md-4"  id="trGeneric" runat="server">
                                <div class="col-md-4"><asp:Label ID="label32" runat="server" SkinID="label" ToolTip="Generic" Text="<%$ Resources:PRegistration, Generic%>" /></div>
                                <div class="col-md-7"> <telerik:RadComboBox ID="ddlGeneric" SkinID="DropDown" runat="server" Width="98%"
                                                            ShowMoreResultsBox="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlGeneric_OnSelectedIndexChanged" />
                                                    </div>
                                <div class="inline-bl"> <asp:ImageButton ID="imgBtnGeneric" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                                            ToolTip="Add&nbsp;New&nbsp;Generic&nbsp;Master" Height="18px" Visible="true"
                                                            CausesValidation="false" OnClick="imgBtnGeneric_Click" /></div>
                        </div>
                        </span>
                        </div>
                     </div>
                    
                    </div>
                    <!-- subgroup ends -->
                    
                    <div class="row form-group">
                        
                        <div  id="trCIMSCategory" runat="server">
                        
                        <div class="col-md-4"> 
                            <div class="col-md-4"><asp:Label ID="Label2" runat="server" SkinID="label" Text="CIMS Category" /> </div>
                            <div class="col-md-8"> <asp:TextBox ID="txtCIMSCategory" ReadOnly="true" runat="server" SkinID="textbox"
                                                Width="98%"></asp:TextBox> </div>
                        </div>
                        
                        <div class="col-md-4"> 
                            <div class="col-md-4"> <asp:Label ID="Label3" runat="server" SkinID="label" Text="Sub Category" /></div>
                            <div class="col-md-8"> <asp:TextBox ID="txtCIMSSubCategory" ReadOnly="true" runat="server" SkinID="textbox"
                                                Width="98%"></asp:TextBox></div>
                        </div>
                        
                        </div>
                        
                        <div class="col-md-4"> 
                            <div class="col-md-4"><asp:Label ID="Label20" runat="server" SkinID="label" ToolTip="Manufacture" Text="<%$ Resources:PRegistration, Manufacture%>" /> </div>
                            <div class="col-md-7">  <telerik:RadComboBox ID="ddlManufacture" SkinID="DropDown" runat="server" Width="100%"
                                                            EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlOnSelectedIndexChanged" />
                                                        <asp:HiddenField ID="hdnManufactureShortName" runat="server" /></div>
                            <div class="inline-bl"><asp:ImageButton ID="imgBtnManufacture" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                                            ToolTip="Add&nbsp;New&nbsp;Manufacture" Height="18px" Visible="true" CausesValidation="false"
                                                            OnClick="imgBtnManufacture_Click" /> </div>
                        </div>
                        
                    </div>
                    
                    
                    <div class="row form-group">
                          <div class="col-md-4" id="tdPacking" runat="server">
                                <div class="col-md-4"><asp:Label ID="lblPacking" runat="server" SkinID="label" ToolTip="Packing" Text="Packing" /></div>
                                <div class="col-md-7"><telerik:RadComboBox ID="ddlPacking" SkinID="DropDown" runat="server" Width="98%"
                                                            ShowMoreResultsBox="true" DropDownWidth="500px" EmptyMessage="[ Select ]" MarkFirstMatch="true"  AutoPostBack="true" OnSelectedIndexChanged="ddlOnSelectedIndexChanged" /></div>
                                <div class="inline-bl"><asp:ImageButton ID="iBtnPackingMst" runat="server" Enabled="true" ImageUrl="~/Images/PopUp.jpg"
                                                            ToolTip="Add&nbsp;New&nbsp;Packing&nbsp;Master" Height="18px" Visible="true"
                                                            CausesValidation="false" OnClick="iBtnPackingMst_Click" /></div>
                          </div>  
                    <div class="col-md-4">
                                <div class="col-md-4"> <asp:Label ID="label5" runat="server" SkinID="label" ToolTip="Item Name" Text="Item Name" />
                                            <span style='color: Red'>*</span></div>
                                <div class="col-md-8">
                                            <asp:TextBox ID="txtItemName" SkinID="textbox" runat="server" Style="
                                                 width:100%;" MaxLength="500" onkeyup="return MaxLenTxt(this);"
                                                 /></div>
                            </div>
                            
                            <div class="col-md-4">
                                <div class="col-md-4">
                                            <asp:Label ID="label36" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, Specification%>' /></div>
                                <div class="col-md-8">
                                            <asp:TextBox ID="txtSpecification" SkinID="textbox" runat="server" Style="
                                                width:100%;" MaxLength="500" onkeyup="return MaxLenTxt(this);"
                                                 />
                                        </div>
                            </div>
                            
                          
                    </div>
                    
                    
                    
                    <div class="row form-group">
                            
                           
                            <div class="col-md-4">
                                <div class="col-md-4"> <asp:Label ID="Label4" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' /></div>
                                <div class="col-md-8">
                                                        <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="100%"
                                                            OnClientSelectedIndexChanging="OnClientSelectedIndexChangingHandlerddlStatus">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="Active" Value="1" />
                                                                <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                                            </Items>
                                                        </telerik:RadComboBox></div>
                            </div>
                             <div class="col-md-8">
                         
                                        
                                    <span id="Tr1" visible="false" runat="server">
                                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Create As" />
                                    </span>
                                    
                    <div class="col-md-4"><asp:CheckBox ID="chkPurchaseClose" runat="server" Text="Close For Purchase"  CssClass="checkboxes" /> </div>  
                    <div class="col-md-4"><asp:CheckBox ID="chkSaleClose" runat="server" Text="Close For Sale"   CssClass="checkboxes"/></div>  
                                           
                              
                              
                            <span id="tdIsItem1" valign="top" runat="server" visible="false" width="1%"></span>
                            <span id="tdIsItem2" valign="top" runat="server" visible="false" width="1%"></span>
                    </div>
                    </div>
                   
                    <div class="row">
                    
                        <div class="col-md-7">
                                <div class="col-md-2"> <asp:Label ID="Label6" runat="server" SkinID="label" Text="Item Units" /></div>
                                <div class="col-md-9"><asp:Panel ID="Panel4" runat="server" Height="110px" Width="98%" BorderWidth="1"
                                                            BorderColor="SkyBlue" ScrollBars="Auto">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvItemUnit" SkinID="gridview2" runat="server" AutoGenerateColumns="False"
                                                                        AllowMultiRowSelection="false" Width="100%" OnRowDataBound="gvItemUnit_RowDataBound">
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>'
                                                                                        onclick="javascript:SelectOnlyOneChk(this);" />
                                                                                    <asp:HiddenField ID="hdnIsDefault" runat="server" Value='<%#Eval("IsDefault")%>' />
                                                                                    <asp:HiddenField ID="hdnItemUnitId" runat="server" Value='<%#Eval("ItemUnitId")%>' />
                                                                                    <asp:HiddenField ID="hdnIssueUnitId" runat="server" Value='<%#Eval("IssueUnitId")%>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Item Unit(s)">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblUnitName" runat="server" SkinID="label" Text='<%#Eval("ItemUnitName") %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="PurchaseUnit1" HeaderText="Pur. Unit 1" />
                                                                            <asp:BoundField DataField="PurchaseUnit2" HeaderText="Pur. Unit 2" />
                                                                            <asp:BoundField DataField="IssueUnit" HeaderText="Issue Unit" />
                                                                            <asp:BoundField DataField="PackingName" HeaderText="Packing" Visible="false" />
                                                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top" HeaderText="Default">
                                                                                <ItemTemplate>
                                                                                    <asp:RadioButton ID="rdoDefault" runat="server" Checked='<%#Eval("IsDefault").ToString().Equals("1")%>'
                                                                                        onclick="javascript:SelectOnlyOne(this);" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </asp:Panel></div>
                                 <div class="inline-bl"><asp:ImageButton ID="ibtnNewItemUnit" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                            ToolTip="Add&nbsp;New&nbsp;Item&nbsp;Unit" Height="18px" Width="16px" OnClick="ibtnNewItemUnit_Click"
                                                            Visible="true" CausesValidation="false" /></div>
                          </div>
                        
                    </div>
                    
                    
                   
                   
                   
                    
                                 
                                   
                                           
                                       
                                       
                                       
                                        
                                        
                                        
                                  
                       
                       
                       
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload, Close" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <asp:Button ID="btnFindClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        OnClick="OnClientFindClose_OnClick" />
                    <asp:Button ID="btnBrandMst" runat="server" Text="GetInfo" CausesValidation="true"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnBrandMst_Click" />
                    <asp:Button ID="btnStrengthMst" runat="server" Text="GetInfo" CausesValidation="true"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnStrengthMst_Click" />
                    <asp:Button ID="btnPackingMst" runat="server" Text="GetInfo" CausesValidation="true"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnPackingMst_Click" />
                    <asp:Button ID="btnGetGeneric" runat="server" Text="GetInfo" CausesValidation="true"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnGetGeneric_Click" />
                    <asp:Button ID="btnGetManufacture" runat="server" Text="GetInfo" CausesValidation="true"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnGetManufacture_Click" />
                    <%-- <asp:Button ID="btnPacking" runat="server" Text="GetInfo" CausesValidation="true"
                        SkinID="button" Style="visibility: hidden;" OnClick="btnPacking_Click" />--%>
                    <asp:Button ID="btnItemUnitClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                        OnClick="btnItemUnitClose_OnClick" />
                    <asp:Button ID="btnGetCIMS" runat="server" Style="visibility: hidden;" OnClick="btnGetCIMS_Click" />
                    <asp:ValidationSummary ID="vs1" runat="server" ShowMessageBox="true" ShowSummary="false" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
