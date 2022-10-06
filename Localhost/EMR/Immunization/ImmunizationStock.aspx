<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ImmunizationStock.aspx.cs" Inherits="EMR_Immunization_ImmunizationStock" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  <script language="javascript" type="text/javascript">
        function NextTab() {
            if (event.keyCode == 13) {
                event.keyCode = 9;
            }
        }
       

        function cmbDrugList_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();

            var GenPId = item.get_attributes().getAttribute("DRUG_SYN_ID");
            //var DrugSId = item.get_attributes().getAttribute("DRUG_ID");

            $get('<%=hdn_DRUG_SYN_ID.ClientID%>').value = GenPId;
            //$get('<%=hdn_DRUG_ID.ClientID%>').value = DrugSId;

            $get('<%=hdn_GENPRODUCT_ID.ClientID%>').value = item != null ? item.get_value() : sender.value();

        }


    </script>
<%--//            oWnd.close();--%>
  
     <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
     
                                    <ContentTemplate>--%>
                                   
                                 
        <table width="100%" cellpadding="0" cellspacing="0" style="background-color: White;">
            <tr>
                  <td style="height: 20px;" colspan="3">
                    <table width="100%" class="clsheader">
                        <tr>                           
                            <td style=" padding-left: 20px;">
                                Medical Stock
                            </td>
                            <td align="right">
                              <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <div >
                                         
                                            <asp:Button ID="btnNew" runat="server" SkinID="Button"  CausesValidation="true"
                                                OnClick="btnNew_OnClick" ToolTip="New" Width="50px" Text="New" Visible="true" />
                                            <asp:Button ID="btnSaveImmunizationStock" 
                                                runat="server"  SkinID="Button" CausesValidation="false" OnClick="SaveImmunizationStock_OnClick"
                                                ToolTip="Save" Width="50px" ValidationGroup="Save" Text="Save" />
                                          
                                            <asp:Button ID="btnUpdateImmunizationStock" runat="server" SkinID="Button" 
                                                CausesValidation="true"  ToolTip="Update"
                                                Width="50px" ValidationGroup="Save" Text="Update" Visible="false" />
                                                  <asp:ValidationSummary ID="VSibtnSaveImmunizationStock" runat="server" ShowMessageBox="True"
                                                ShowSummary="False" ValidationGroup="Save" />
                                                
                                               <%-- <asp:Button ID="btnShow" runat="server" Width="50px" Text="View" SkinID="Button" OnClick="btnShow_OnClick" ValidationGroup="Save"  />--%>
                                                
                                                                                                                                              
                                        </div>                                        
                                    </ContentTemplate>
                                 
                                </asp:UpdatePanel>
                                 
                            </td>
                            <td>
                                <%--<asp:UpdatePanel ID="updatepanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate >
                                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                    <Windows>                                    
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                    </Windows>
                                    </telerik:RadWindowManager>
                                    </ContentTemplate>
                                   
                                    </asp:UpdatePanel>
                                    
                                     <asp:UpdatePanel ID="updatepanelclose" runat="server" UpdateMode="Conditional" >
                                     <ContentTemplate>    
                                     <asp:Button ID="btnclose" runat="server" CausesValidation="false"  style="visibility:hidden; " OnClick="btnclose_OnClick" />
                                            </ContentTemplate>
                                     </asp:UpdatePanel> --%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" colspan="1" style="height: 13px; color: green; font-size: 12px;
                    font-weight: bold;">
                      <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" Text="" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="left" width="60%" valign="top">
                    <table width="100%" cellpadding="0" cellspacing="0" border="1">
                        <tr >
                            <td align="left" style="width: 100%" valign="top">
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">     
                                          <ContentTemplate>
                                <table width="100%" cellpadding="3" cellspacing="0" border="0" align="left" >
                                    <tr>
                                        <td >
                                            <asp:Literal ID="ltrlImmunizationName" runat="server" Text="Name"></asp:Literal><span
                                                style="color: Red">*</span>
                                        </td>
                                        <td align="left">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">     
                                          <ContentTemplate>
                                          
                                           <%-- <asp:DropDownList ID="ddlImmunizationName" SkinID="DropDown" runat="server" Width="200px"
                                                AutoPostBack="True">
                                            </asp:DropDownList>--%>
                                            <telerik:RadComboBox ID="ddlImmunizationName" runat="server" Height="300px" Width="200px"
                                                EmptyMessage="Search NDC by TradeName or Id" DataTextField="ImmunizationName" 
                                                DataValueField="ImmunizationID" EnableLoadOnDemand="true" HighlightTemplatedItems="true"
                                                ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnItemsRequested="ddlImmunizationName_OnItemsRequested"
                                                OnSelectedIndexChanged="ddlImmunizationName_OnSelectedIndexChanged" CausesValidation="false"
                                                AutoPostBack="true">
                                            </telerik:RadComboBox>
                                            </ContentTemplate>
                                           
                                            </asp:UpdatePanel>
                                        </td>
                                        <td align="left">
                                           <asp:Literal ID="ltrlLot" runat="server" Text="Lot"></asp:Literal><span
                                                style="color: Red;">*</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtLot" runat="server" Width="100px" SkinID="textbox"></asp:TextBox>
                                        </td>
                           
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="Literal5" runat="server" Text="Medication"></asp:Literal><span style="color: Red">*</span>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            
                                                <ContentTemplate>
                                                
                                            <telerik:RadComboBox ID="RadCmbMedication" runat="server" Height="300px" Width="200px"
                                                EmptyMessage="Search NDC by TradeName or Id" DataTextField="generic_product_name"
                                                DataValueField="genproduct_id" EnableLoadOnDemand="true" HighlightTemplatedItems="true"
                                                ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnItemsRequested="RadCmbMedication_OnItemsRequested"
                                                OnSelectedIndexChanged="RadCmbMedication_OnSelectedIndexChanged" CausesValidation="false"
                                                AutoPostBack="true">
                                            </telerik:RadComboBox>
                                            </ContentTemplate>
                                            <Triggers>
                                            <asp:AsyncPostBackTrigger  ControlID="btnaddtolist" />
                                            </Triggers>
                                            </asp:UpdatePanel>
                                        </td>                                        
                                        <td align="left">
                                            <asp:Literal ID="ltrlExpdate" runat="server" Text="Expiry Date"></asp:Literal><span
                                                style="color: Red;">*</span>
                                        </td>
                                        <td align="left">
                                            <telerik:RadDatePicker ID="RadExpiryDate" runat="server" MinDate="01/01/1900" Width="100px">
                                            </telerik:RadDatePicker>
                                        </td>      
                                       
                                    </tr>
                                    <tr>
                                        <td>
                                            NDC Code<span style="color: Red">*</span>
                                        </td>
                                        <td width="200px">
                                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSaveImmunizationStock" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnaddtolist"  />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <telerik:RadComboBox ID="cmbNDCList" runat="server" Height="300px" Width="200px"
                                                        EmptyMessage="Search NDC by TradeName or Id" DataTextField="DISPLAY_NAME" DataValueField="PKG_PRODUCT_ID"
                                                        EnableLoadOnDemand="true" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                        EnableVirtualScrolling="true" OnItemsRequested="cmbNDCList_OnItemsRequested"
                                                        OnSelectedIndexChanged="cmbNDCList_OnSelectedIndexChanged" CausesValidation="false"
                                                        AutoPostBack="true">
                                                        <HeaderTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        Drug Display Name
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <%# DataBinder.Eval(Container, "Text")%>
                                                                    </td>
                                                                    <td id="Td1" visible="false" runat="server">
                                                                        <%# DataBinder.Eval(Container, "Attributes['PKG_PRODUCT_ID']")%>
                                                                    </td>
                                                                    <td id="Td2" visible="false" runat="server">
                                                                        <%# DataBinder.Eval(Container, "Attributes['GENERIC_PRODUCT_NAME']")%>
                                                                    </td>
                                                                    <td id="Td3" visible="false" runat="server">
                                                                        <%# DataBinder.Eval(Container, "Attributes['JCODE']")%>
                                                                    </td>
                                                                    <td id="Td4" visible="false" runat="server">
                                                                        <%# DataBinder.Eval(Container, "Attributes['LABELER_NAME']")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>                                        
                                        <td>
                                            <asp:Literal ID="ltrlQty" runat="server" Text="Quantity"></asp:Literal><span style="color: Red;">*</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtqty" runat="server" Width="50px" MaxLength="5"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" Enabled="True"
                                                TargetControlID="txtqty" FilterType="Numbers">
                                            </cc1:FilteredTextBoxExtender>
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            
                                            <asp:Literal ID="Literal2" runat="server" Text="Unit"></asp:Literal><span style="color: Red">*</span>
                                        </td>
                                        <td>
                                        
                                            <asp:DropDownList ID="ddlunit" SkinID="DropDown" runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center" colspan="2">
                                              </td>
                                    </tr>
                                    <tr><td colspan="4" align="center">
                                     <%--<asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                     <Triggers>
                                     <asp:PostBackTrigger ControlID="ddlunit" />
                                     <asp:AsyncPostBackTrigger ControlID="RadExpiryDate" />
                                     </Triggers>
                                                <ContentTemplate>--%>
                                    <asp:Button ID="btnaddtolist" runat="server" CausesValidation="false" ValidationGroup="Save"
                                                Text="Add To List" SkinID="Button" OnClick="btnaddtolist_OnClick" />
                                            <asp:Button ID="btnupdatelist" runat="server" Visible="false" CausesValidation="true"
                                                ValidationGroup="Save" Text="Update To List" SkinID="Button" OnClick="btnupdatelist_OnClick" />
                                     <%-- </ContentTemplate></asp:UpdatePanel>--%>
                                    </td></tr>
                                    <%--<tr>
                                      <td align="left">
                                            &nbsp;&nbsp;<asp:Literal ID="Literal1" runat="server" Text="Manufacturer"></asp:Literal><span
                                                style="color: Red;">*</span>
                                        </td>
                                      <td>
                                            <asp:DropDownList ID="ddlmanufacturer" Width="100px" runat="server" SkinID="DropDown">
                                            </asp:DropDownList>
                                        </td>
                                      <td>
                                            <asp:Literal ID="Literal4" runat="server" Text="Dose"></asp:Literal>
                                        </td>
                                      <td>
                                            <asp:DropDownList ID="ddldose" SkinID="DropDown" runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                      <td>
                                            <asp:Literal ID="Literal3" runat="server" Text="Route"></asp:Literal>
                                        </td>
                                      <td>
                                            <asp:DropDownList ID="ddlrout" SkinID="DropDown" runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    
                                    
                                </table>
                                </ContentTemplate></asp:UpdatePanel>
                            </td>
                        </tr>
                      
                    </table>
                   
                                 <asp:UpdatePanel ID="UpdImmunizationStockList" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                       <asp:AsyncPostBackTrigger ControlID="btnupdatelist" />
                                        <asp:AsyncPostBackTrigger ControlID="btnaddtolist" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSaveImmunizationStock" />
                                       <asp:AsyncPostBackTrigger ControlID="gvImmunizationStock" />
                                                
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:GridView ID="GvStockList"  SkinID="gridview" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                            ShowHeader="true" Width="200px"  OnSelectedIndexChanged="GvStockList_OnSelectedIndexChanged"
                                            ShowFooter="false">
                                            <EmptyDataTemplate>
                                                <div style="font-weight: bold; color: Red; width: 200px">
                                                    No Record Found.</div>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField Visible="True" HeaderText="S No">
                                                    <ItemTemplate>
                                                        <%--<asp:Label ID="lblsno" SkinID="label" runat="server" Text=' <%#Eval("Sno")%>'></asp:Label>--%>
                                                        <%# Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" SkinID="label" runat="server" Text=' <%#Eval("Id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ImmuId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImmuId" SkinID="label" runat="server" Text=' <%#Eval("ImmunizationId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Immunization Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImmunName" SkinID="label" runat="server" Text=' <%#Eval("ImmunizationName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LotNo">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLotNo" SkinID="label" runat="server" Text=' <%#Eval("LotNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MedicationId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGENProdID" SkinID="label" runat="server" Text=' <%#Eval("GENPRODUCT_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Medication">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDISPLAYNAME" SkinID="label" runat="server" Text=' <%#Eval("Medication")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UnitId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitId" SkinID="label" runat="server" Text=' <%#Eval("UnitId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitName" SkinID="label" runat="server" Text=' <%#Eval("UnitName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <%-- <asp:TemplateField HeaderText="RouteId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRouteId" SkinID="label" runat="server" Text=' <%#Eval("RouteId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route name" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRouteName" SkinID="label" runat="server" Text=' <%#Eval("RoutName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <%--<asp:TemplateField HeaderText="DoseFormId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoseFormId" SkinID="label" runat="server" Text=' <%#Eval("DoseFormId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dose Name" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoseName" SkinID="label" runat="server" Text=' <%#Eval("DoseName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="NDCCode" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNDCCode" SkinID="label" runat="server" Text=' <%#Eval("NDCCode")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="NDC Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNDCName" SkinID="label" runat="server" Text=' <%#Eval("NDCName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              <%--  <asp:TemplateField HeaderText="Manufacturer" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManufacturer" SkinID="label" runat="server" Text=' <%#Eval("Manufacturer")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ManufacturerId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblManufacturerId" SkinID="label" runat="server" Text=' <%#Eval("ManufacturerId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Expiry Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblexpirydate" SkinID="label" runat="server" Text=' <%#Eval("ExpiryDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblquantity" SkinID="label" runat="server" Text=' <%#Eval("QtyReceived")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Update">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ibtnUpdate" runat="server" Text="Update" CausesValidation="true"
                                                            CommandName="Select" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnListDelete" runat="server" ToolTip="Delete" ImageUrl="/Images/DeleteRow.png"
                                                            OnClick="ibtnListDelete_OnClick" />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False"
                                            ValidationGroup="Edit" />--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                   
                </td>
                 <td>&nbsp;&nbsp;</td>
                <td  valign="top" width="40%">  
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate>
                 <asp:Label ID="lblImmunizationName" runat="server" Font-Bold="true" Text=""></asp:Label>
               <%-- <b>Status</b>--%> <asp:DropDownList ID="ddlstatus" Visible="false" SkinID ="DropDown" AutoPostBack="true" Width="100px"  
                runat="server" onselectedindexchanged="ddlstatus_SelectedIndexChanged">
                    <asp:ListItem Text="Open" Value="O"></asp:ListItem>
                    <asp:ListItem Text="Post" Value="P"></asp:ListItem>
                 
            </asp:DropDownList>
                 <asp:GridView ID="gvImmunizationStock" SkinID="gridview" runat="server"
            AutoGenerateColumns="False" DataKeyNames="Id" 
            Width="200px" PageSize="15"
            AllowPaging="True" PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" 
            OnPageIndexChanging="gvImmunizationStock_OnPageIndexChanging" OnRowCancelingEdit="gvImmunizationStock_OnRowCancelingEdit"
            OnSelectedIndexChanged="gvImmunizationStock_SelectedIndexChanged" 
            OnRowDataBound="gvImmunizationStock_RowDataBound" 
            >
            <%--OnRowDataBound="gvImmunizationStock_OnRowDataBound"   OnRowEditing="gvImmunizationStock_OnRowEditing" OnRowUpdating="gvImmunizationStock_OnRowUpdating"--%>
            <EmptyDataTemplate>
                <div style="font-weight: bold; color: Red; width: 100%">
                    No Record Found.</div>
            </EmptyDataTemplate>
            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
            <Columns>
                
                <asp:TemplateField Visible="True" HeaderText="S No">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Id" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblId" SkinID="label" runat="server" Text=' <%#Eval("Id")%>'></asp:Label>
                        <%--<asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                             ToolTip="DeActivate" OnClick="btngo_OnClick"
                                                            ValidationGroup="Cancel" CausesValidation="true" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="HospitalLocationId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblHLocationId" SkinID="label" runat="server" Text='<%#Eval("HospitalLocationId")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DocumentNo" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDocumentNo" SkinID="label" runat="server" Text='<%#Eval("DocumentNo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Lot No">
                    <ItemTemplate>
                        <asp:Label ID="lblLotno" SkinID="label" runat="server" Text='<%#Eval("LotNo")%>'></asp:Label>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Received Date">
                    <ItemTemplate>
                        <asp:Label ID="lblReceivedDate" SkinID="label" runat="server" Text='<%#Eval("ReceivedDate")%>'></asp:Label>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:Label ID="lblQtyReceived" SkinID="label" runat="server" Text='<%#Eval("QtyReceived")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnview" Text="Edit" runat="server" Width="100%" Font-Underline="false"
                            CommandName="Select"></asp:LinkButton>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Post">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnStockPost" runat="server" SkinID="Button" Text="Stock Post" Font-Underline="false" ToolTip="Stock Post"
                            OnClick="btnStockPost_OnClick" CausesValidation="true" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField >
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                            ToolTip="Delete" OnClick="btngo_OnClick"   ValidationGroup="Cancel" CausesValidation="true" />

                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="DeTailId" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblDetailid" SkinID="label" runat="server" Text=' <%#Eval("Detailid")%>'></asp:Label>
                       
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
        </asp:GridView>
        </ContentTemplate></asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                            <td align="center" colspan="3">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <%--<asp:Button ID="cmdSave" runat="server" SkinID="Button" Text="Save" OnClick="cmdSave_OnClick"
                                            CausesValidation="false" Visible="false" />--%>
                                        <asp:HiddenField ID="hdn_DRUG_SYN_ID" runat="server" />
                                        <asp:HiddenField ID="hdn_DRUG_ID" runat="server" />
                                        <asp:HiddenField ID="hdn_GENPRODUCT_ID" runat="server" />
                                        
                                        <asp:HiddenField ID="hdnServiceAmount" runat="server" />
                                        <asp:HiddenField ID="hdnDoctorAmount" runat="server" />
                                        <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" />
                                        <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
        </table>
        
       
       
       <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
   
</asp:Content>

