<%@ Page Title="Compounded Medications" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PrescribeMedicationCM.aspx.cs" Inherits="EMR_Medication_PrescribeMedicationCM" %>

<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%: Styles.Render("~/bundles/OrderStyle") %>
   
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />


 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-2 col-sm-2 col-xs-3">
                    <h2><asp:Label ID="Label1" runat="server" Text="Compounded&nbsp;Drug&nbsp;Order" /></h2>
                </div>
                        <div class="col-md-2 col-sm-2 col-xs-6">
                            <div class="row">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="Label7" runat="server" style="margin:0px!important;padding:0px!important;line-height:18px;" Text="Store" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:DropDownList ID="ddlStore" runat="server" SkinID="DropDown" Width="100%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-2 col-xs-6">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-5">
                                    <asp:Label ID="Label18" runat="server" Text="Advising&nbsp;Doctor" />
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-7">
                                    <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" EmptyMessage="[ Select ]" DropDownWidth="200px"
                                        Width="100%" Height="300px" Filter="Contains" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-2 col-sm-2 col-xs-12">
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-xs-4">
                                    <asp:Label ID="Label2" runat="server" style="margin:0px!important;padding:0px!important;line-height:18px;" Text="Order&nbsp;Priority" />
                                </div>
                                <div class="col-md-7 col-sm-7 col-xs-8">
                                    <asp:DropDownList ID="ddlIndentType" runat="server" SkinID="DropDown" Width="100%" />
                                </div>
                            </div>
                        </div>
                    
                <div class="col-md-4 col-sm-4 col-xs-12 text-right">
                        <button id="liAllergyAlert" runat="server" class="btn btn-primary" visible="false" style="background: #fff; border: 0px;">
                            <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" Width="18px" Height="18px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                        </button>
                        <button id="liMedicalAlert" runat="server" visible="false" class="btn btn-primary" style="background: #fff; border: 0px;">
                            <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif" OnClick="imgMedicalAlert_OnClick" CssClass="iconEMRimg" Width="18px" Height="18px" Visible="false" ToolTip="Patient Alert" />
                        </button>
                    
                        <asp:LinkButton ID="btnPreviousMedications" runat="server" CssClass="btn btn-primary"
                                        OnClick="btnPreviousMedications_Click" Text="Previous Medications" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print (Ctrl+F9)" CssClass="btn btn-primary"
                            OnClick="btnPrint_Click" Visible="false" CausesValidation="false" />
                        <asp:Button ID="btnSave" runat="server" CausesValidation="false" Text="Save (Ctrl+F3)"
                            CssClass="btn btn-primary" OnClick="btnSave_Onclick" OnClientClick="ClientSideClick(this)"
                            UseSubmitBehavior="False" />
                        <asp:HiddenField ID="hdnIsSelectEncounterDoctorId" runat="server" Value="0" />
                        <asp:Button ID="btnclose1" runat="server" Text="Close (Ctrl+F8)" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        
                    
                </div>
            </div>

              
            <div class="row">
                <asplUD:UserDetails ID="asplUD" runat="server" />
                <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" Visible="false"></asp:Label>
                
            </div>

                <div class="row ">
                    <div class="col-md-12">
                            <asp:Label ID="lblMessage" style="position:relative;width:100%;margin:0px;" runat="server" Text="" />
                    </div>
                    <div id="Td1" runat="server" visible="false">
                        <asp:Button ID="btnPrintF9" runat="server" SkinID="Button" Text="Print (Ctrl+F9)" OnClick="btnPrint_Click"
                            Visible="false" CausesValidation="false" />
                    </div>
                </div>
            
                <div class="row" id="Div1" runat="server">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                <asp:Label ID="Label19" runat="server" Text="Prov.&nbsp;Diagnosis" />
                            </div>
                            <div class="col-md-10 col-sm-10 col-xs-9">
                                <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" TextMode="MultiLine" ReadOnly="true"
                                    Style="min-height: 21px; max-height: 21px; min-width: 100%; max-width: 540px; line-height: 1.0em;" />
                            </div>
                        </div>
                    </div>


                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                <asp:Label ID="Label11" runat="server" Text="Height&nbsp;(Cm):&nbsp;" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                                <asp:Label ID="txtHeight" runat="server" ForeColor="Maroon" Width="30px" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                <asp:Label ID="Label12" runat="server" Text="Weight&nbsp;(Kg):&nbsp;" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                                <asp:Label ID="lbl_Weight" runat="server" ForeColor="Maroon" Width="30px" />
                            </div>
                        </div>
                    </div>
                    </div>
                <div class="row">
                    <div id="dvCompoundedDrugName" runat="server">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                <asp:Label ID="Label5" runat="server" Text="Comp. Drug" ToolTip="Compounded Drug Name" />&nbsp;<span class="redStar3" id="Span1" runat="server">*</span>
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                               <asp:TextBox ID="txtCompoundedDrugName" runat="server" Width="100%" MaxLength="250" />
                            </div>
                        </div>
                    </div>


                    <div class="col-md-2 col-sm-2 col-xs-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                <asp:Label ID="Label8" runat="server" Text="Dose" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                                <asp:TextBox ID="txtCompoundedDose" TabIndex="13" runat="server" Width="100%" MaxLength="5"  />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    FilterType="Custom, Numbers" TargetControlID="txtCompoundedDose" ValidChars="." />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                               <asp:Label ID="Label10" runat="server" Text="Unit" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                               <telerik:RadComboBox ID="ddlCompoundedUnitId" runat="server" SkinID="DropDown" Height="200px"
                                    Width="100%" DropDownWidth="160px" />
                            </div>
                        </div>
                    </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5" id="trDrugs" runat="server">
                            <div class="col-md-2 col-sm-2 col-xs-3">
                                <asp:Label ID="lblInfoBrand" runat="server" Text="Brand" />
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6 ">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlBrand" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="ddlBrand" TabIndex="1" runat="server" HighlightTemplatedItems="true"
                                            Width="100%" DropDownWidth="450px" Height="300px" EmptyMessage=" "
                                            AllowCustomText="true" EnableLoadOnDemand="true" ShowMoreResultsBox="true" Skin="Office2007" EnableVirtualScrolling="true"
                                            MarkFirstMatch="false" ZIndex="50000" OnItemsRequested="ddlBrand_OnItemsRequested"
                                            OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged"
                                            OnClientDropDownClosed="ddlBrandOnClientDropDownClosedHandler">
                                            <HeaderTemplate>
                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 100%" align="left">
                                                            <asp:Label ID="Label28" runat="server" Text="Item Name" />
                                                        </td>
                                                        <td style="width: 10%" align="right" runat="server" id="tdClosingBalance">
                                                            <asp:Label ID="lblClosingBalance" runat="server" Text="QTY" />
                                                        </td>
                                                        <%--<td style="width: 100%" align="left" runat="server" id="td2">
                                                                <asp:Label ID="Label20" runat="server" Text="XYZ" />
                                                            </td>--%>
                                                        <%--<td style="width: 10%" align="left" visible="false"><asp:Label ID="Label29" runat="server" Text="Stock" /></td>--%>
                                                        <td style="width: 0%" align="right" visible="false">
                                                            <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />
                                                        </td>
                                                        <td style="width: 0%" align="right" visible="false">
                                                            <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                        </td>
                                                        <td style="width: 0%" align="right" visible="false">
                                                            <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
                                                        </td>

                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="width: 100%" align="left">
                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                        </td>
                                                        <td style="width: 10%" align="right" id="tdItemClosingBalance" runat="server">
                                                            <%--    <%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>--%>
                                                            <asp:Label ID="lblClosingBalanceItem" runat="server" Text='<%# DataBinder.Eval(Container, "Attributes[\"ClosingBalance\"]")%>' />

                                                        </td>
                                                        <%-- <td>
                                                                <asp:Label ID="lblClosingBalance1" runat="server" Text='<%# DataBinder.Eval(Container, "Attributes[\"ClosingBalance\"]")%>' />

                                                            </td>--%>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>

                                        <asp:Button ID="btnGetInfo" runat="server" Text="" CausesValidation="false" SkinID="button"
                                            Style="display: none;" OnClick="btnGetInfo_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-3">
                                <asp:Button ID="btnBrandDetailsViewOnItemBased" CssClass="btn btn-primary" runat="server"
                                    Text="View Brand Details" Visible="true" OnClick="btnBrandDetailsView_OnClick" />
                                <asp:Button ID="btnMonographViewOnItemBased" CssClass="btn btn-primary" runat="server"
                                    Text="View Monograph" Visible="true" OnClick="btnMonographView_OnClick" />
                               
                            </div>
                        </div>
                    </div>
                    </div>
                   
                <div class="row">
                        <asp:Label ID="lblGenericName" runat="server" Text="" />
                    </div>

                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-12 m-t">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                            <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                ScrollBars="Auto" Height="200px" Width="100%">
                                <asp:GridView ID="gvItem" runat="server" Width="100%" AllowPaging="false"
                                    AutoGenerateColumns="False" OnRowCreated="gvItem_OnRowCreated" OnRowDataBound="gvItem_OnRowDataBound"
                                    OnRowCommand="gvItem_OnRowCommand" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                    HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None"
                                    BorderWidth="0px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Drug Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" SkinID="label" Text='<%# Eval("ItemName") %>'
                                                    Width="100%" Font-Size="Smaller" />
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                <asp:HiddenField ID="hdnGenericName" runat="server" Value='<%# Eval("GenericName") %>' />
                                                <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                                <asp:HiddenField ID="hdnIsInjection" runat="server" Value='<%#Eval("IsInjection") %>' />
                                                <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%# Eval("StrengthValue") %>' />
                                                <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugAllergy" runat="server" Value='<%# Eval("OverrideComments") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugToDrug" runat="server" Value='<%# Eval("OverrideCommentsDrugToDrug") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugHealth" runat="server" Value='<%# Eval("OverrideCommentsDrugHealth") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dose" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDoseGD" TabIndex="13" runat="server" Width="100%" MaxLength="5"
                                                    Style="text-align: right; height: 22px;" Text='<%#Eval("Dose") %>' />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                    FilterType="Custom, Numbers" TargetControlID="txtDoseGD" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <telerik:RadComboBox ID="ddlUnitGD" runat="server" SkinID="DropDown" Height="150px"
                                                    Width="100%" DropDownWidth="160px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Strength" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                            <ItemTemplate>
                                                <telerik:RadComboBox ID="ddlStrengthValueGD" runat="server" SkinID="DropDown" Height="150px"
                                                    Width="100%" DropDownWidth="160px" AllowCustomText="true" EmptyMessage=" " Filter="StartsWith" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty." HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQtyGD" runat="server" Width="100%" MaxLength="5"
                                                    Style="text-align: right;" Text='<%#Eval("Qty") %>' />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                    FilterType="Custom, Numbers" TargetControlID="txtQtyGD" ValidChars="." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label51" runat="server" Text="BD" ToolTip="Brand Details" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details"
                                                    CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label52" runat="server" Text="MG" ToolTip="Monograph" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                                    CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label53" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                                    CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                    Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label43" runat="server" Text="DI" ToolTip="Duplicate Ingredient" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDIInteractionCIMS" runat="server" ToolTip="Click here to view cims Duplicate Ingredient"
                                                    CommandName="DIInteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                    Width="17px" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label54" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                    CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label55" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction"
                                                    CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label56" runat="server" Text="BD" ToolTip="Brand Details" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details"
                                                    CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label57" runat="server" Text="MG" ToolTip="Monograph" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                                    CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label58" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                                    CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                    Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label59" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                                    CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                            <HeaderTemplate>
                                                <asp:Label ID="Label60" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction"
                                                    CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA"
                                                    Text="&nbsp;" Width="100%" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="13px" HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                    CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                    ImageUrl="~/Images/DeleteRow.png" Width="16px" Height="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                            </div>
                    </div>

                    <div class="col-md-6 col-sm-6 col-xs-12 m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12 border-all">
                        <asp:Panel ID="Panel5" runat="server" CssClass="row" Style="background:#A8D9E6;">
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                <asp:Label ID="Label22" runat="server" Text="&nbsp;Drug Attributes" />
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                <asp:CheckBox ID="chkShowDetails" runat="server" Font-Size="12px" Text="Show Details"
                                        AutoPostBack="true" OnCheckedChanged="chkShowDetails_OnCheckedChanged" CssClass="pull-left" />
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                 <asp:CheckBox ID="chkFractionalDose" runat="server" Text="Fractional Dose" Visible="false" />
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                <asp:LinkButton ID="lnkDrugAllergy" runat="server" BackColor="#82CAFA" Font-Size="10px"
                                        Text="Drug&nbsp;Allergy" ToolTip="Drug Allergy" Font-Bold="true" OnClick="lnkDrugAllergy_OnClick"
                                        Visible="false" />
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="Panel4" runat="server" CssClass="row" BorderStyle="Solid" BorderWidth="0px">

                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label4" runat="server" Text="Frequency" /><span class="redStar3" id="spnFrequency" runat="server">*</span>
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <telerik:RadComboBox ID="ddlFrequencyId" runat="server" TabIndex="15" Width="100%" MarkFirstMatch="true" Filter="Contains"
                                            EmptyMessage="[ Select ]" Height="250px" DropDownWidth="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlFrequency_OnSelectedIndexChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label6" runat="server" Text="Duration" /><span class="redStar3" id="spnDuration" runat="server">*</span>
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <div class="row">
                                                    <div class="col-md-7 col-sm-7 col-xs-7">
                                                         <asp:TextBox ID="txtDuration" runat="server" TabIndex="16" Height="22px" Text='<%#Eval("Duration") %>' Width="100%"
                                            MaxLength="2" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDuration" ValidChars="" />
                                                    </div>
                                                    <div class="col-md-5 col-sm-5 col-xs-5 no-p-l">
                                                        <telerik:RadComboBox ID="ddlPeriodType" runat="server" Width="100%" DropDownWidth="100px" TabIndex="17"
                                            OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Minute(s)" Value="N" />
                                                <telerik:RadComboBoxItem Text="Hour(s)" Value="H" />
                                                <telerik:RadComboBoxItem Text="Day(s)" Value="D" Selected="true" />
                                                <telerik:RadComboBoxItem Text="Week(s)" Value="W" />
                                                <telerik:RadComboBoxItem Text="Month(s)" Value="M" />
                                                <telerik:RadComboBoxItem Text="Year(s)" Value="Y" />
                                                <telerik:RadComboBoxItem Text="To Be Continued" Value="C" />
                                                <telerik:RadComboBoxItem Text="As Instructed" Value="V" />
                                            </Items>
                                        </telerik:RadComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label3" runat="server" Text="Form" />
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <telerik:RadComboBox ID="ddlFormulation" runat="server" TabIndex="19" MarkFirstMatch="true" Filter="Contains"
                                            EmptyMessage="[ Select ]" AutoPostBack="true" OnSelectedIndexChanged="ddlFormulation_OnSelectedIndexChanged"
                                            Width="100%" Height="250px" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label9" runat="server" Text="Route" />&nbsp;<span id="spnRoute" runat="server" class="redStar3">*</span>
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <telerik:RadComboBox ID="ddlRoute" runat="server" MarkFirstMatch="true" TabIndex="20" Filter="Contains"
                                            EmptyMessage="[ Select ]" Width="100%" Height="250px" />
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label17" runat="server" ToolTip="Food&nbsp;Relation" Text="Food&nbsp;Relation" />
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <telerik:RadComboBox ID="ddlFoodRelation" TabIndex="21" runat="server" Filter="Contains" EmptyMessage="[ Select ]"
                                            Width="100%" Height="200px" ToolTip="Relationship to Food" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                 <asp:Label ID="Label14" runat="server" Text="Start Date" /><span class="redStar3" id="spnStartdate" runat="server" visible="false">*</span>
                                            </div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <div class="row">
                                                    <div class="col-md-5 col-sm-5 col-xs-5">
                                                        <telerik:RadDatePicker ID="txtStartDate" runat="server" TabIndex="22" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" DbSelectedDate='<%#Eval("StartDate")%>'>
                                            <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" />
                                        </telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <asp:Label ID="Label21" runat="server" Text="Type" ToolTip="Order Type" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4 no-p-l">
                                                        <telerik:RadComboBox ID="ddlDoseType" runat="server" TabIndex="23" ToolTip="Order Type" Width="100%" EmptyMessage="[ Select ]"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlDoseType_OnSelectedIndexChanged" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label35" runat="server" Text="Flow Rate" />
                                            </div>
                                           <div class="col-md-9 col-sm-9 col-xs-9">
                                                <div class="row">
                                                    <div class="col-md-7 col-sm-7 col-xs-7">
                                                         <asp:TextBox ID="txtFlowRateUnit" runat="server" Height="22px" Text="" Width="100%" MaxLength="5" Style="text-align: left" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtFlowRateUnit" ValidChars="." />
                                                    </div>
                                                    <div class="col-md-5 col-sm-5 col-xs-5 no-p-l">
                                                        <telerik:RadComboBox ID="ddlFlowRateUnit" Width="100%" runat="server" MarkFirstMatch="true"
                                            EmptyMessage="[ Select ]" ToolTip="Flow Rate Unit" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap"><asp:Label ID="Label33" runat="server" Text="Time" /></div>
                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                <div class="row">
                                                    <div class="col-md-7 col-sm-7 col-xs-7">
                                                        <asp:TextBox ID="txtTimeInfusion" runat="server" Text="" Width="100%" Height="22px" MaxLength="5" Style="text-align: left; margin: 0 9px 0 0px;" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtTimeInfusion" ValidChars="." />
                                                    </div>
                                                    <div class="col-md-5 col-sm-5 col-xs-5 no-p-l">
                                                        <telerik:RadComboBox ID="ddlTimeUnit" Width="100%" runat="server" MarkFirstMatch="true"
                                            EmptyMessage="[ Select ]" ToolTip="Infusion Time unit">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="" Selected="true" />
                                                <telerik:RadComboBoxItem Value="H" Text="Hour(s)" />
                                                <telerik:RadComboBoxItem Value="M" Text="Minute(s)" />
                                                <%--<telerik:RadComboBoxItem Value="S" Text="Second (S)" />--%>
                                            </Items>
                                        </telerik:RadComboBox>
                                                    </div>
                                                    </div>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                <div class="row p-t-b-5">
                                            <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                                <asp:Label ID="Label61" runat="server" Text="Instructions" />
                                            </div>
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <asp:TextBox ID="txtInstructions" runat="server" TabIndex="24" MaxLength="2000" TextMode="MultiLine"
                                            onkeyup="return MaxLenTxt(this, 2000);" Style="width: 100% !important; height: 40px; font-size: 12px; text-align: left; padding: 3px 5px;" />
                                            </div>
                                        </div>
                            </div>
                        </asp:Panel>
                    </div>
                        </div>
                </div>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderWidth="1px" BorderColor="SkyBlue" Width="100%" Height="200px"
                        ScrollBars="Auto">
                        <asp:GridView ID="gvPreviousItems" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                            AutoGenerateColumns="False" OnRowCreated="gvPreviousItems_OnRowCreated" OnRowDataBound="gvPreviousItems_OnRowDataBound"
                            OnRowCommand="gvPreviousItems_OnRowCommand" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                            HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None"
                            BorderWidth="1px">
                            <Columns>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" HeaderText="Reorder">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnReorder" runat="server" ToolTip="Click here to reorder prescription"
                                            CommandName="REORDER" CausesValidation="false" CommandArgument='<%#Eval("IndentId")%>'
                                            Text="Reorder" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Indent No" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%# Eval("IndentNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Prescription Detail">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("CompoundedPrescriptionDetail") %>'
                                            SkinID="label" Font-Size="Smaller" />
                                        <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                        <asp:HiddenField ID="hdnCompoundedItemId" runat="server" Value='<%# Eval("CompoundedItemId") %>' />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%# Eval("EncounterId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instructions" HeaderStyle-Width="220px" ItemStyle-Width="220px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInstructionRemarks" runat="server" SkinID="label" Text='<%# Eval("InstructionRemarks") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Indent Date" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Text='<%# Eval("IndentDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStartDate" runat="server" SkinID="label" Text='<%# Eval("StartDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" HeaderText="Add Another Drug">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnAddNormalDrug" runat="server" ToolTip="Click here to add another drug"
                                            CommandName="ADDNORMALDRUG" CausesValidation="false" CommandArgument='<%#Eval("IndentId")%>'
                                            Text="Add Another Drug" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Cancel">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click here to cancel this drug"
                                            CommandName="ItemCancel" CausesValidation="false"
                                            ImageUrl="~/Images/close_new-old.jpg" Width="15px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Stop">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnStop" runat="server" ToolTip="Click here to stop this drug"
                                            CommandName="ItemStop" CausesValidation="false"
                                            ImageUrl="~/Icons/Critical.gif" Height="17px" Width="17px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
                    </div>
                <div class="row">
                    <asp:HiddenField ID="hdnButtonId" runat="server" />
                        <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                        <asp:HiddenField ID="hdnReturnIndentOPIPSource" runat="server" />
                        <asp:HiddenField ID="hdnReturnIndentDetailsIds" runat="server" />
                        <asp:HiddenField ID="hdnReturnIndentIds" runat="server" />
                        <asp:HiddenField ID="hdnReturnItemIds" runat="server" />
                        <asp:HiddenField ID="hdnReturnGenericIds" runat="server" />
                        <asp:HiddenField ID="hdnStoreId" runat="server" />
                        <asp:HiddenField ID="hdnGenericId" runat="server" />
                        <asp:HiddenField ID="hdnGenericName" runat="server" />
                        <asp:HiddenField ID="hdnItemId" runat="server" />
                        <asp:HiddenField ID="hdnItemName" runat="server" />
                        <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                        <asp:HiddenField ID="hdnCIMSType" runat="server" />
                        <asp:HiddenField ID="hdnVIDALItemId" runat="server" />
                        <asp:HiddenField ID="hdnTotalQty" runat="server" />
                        <asp:HiddenField ID="hdnInfusion" runat="server" />
                        <asp:HiddenField ID="hdnIsInjection" runat="server" />

                        <asp:Button ID="btnRefresh" runat="server" Style="display: none;" OnClick="btnRefresh_OnClick" />
                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close,Move" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <asp:HiddenField ID="hdnControlId" runat="server" />
                        <asp:HiddenField ID="hdnControlType" runat="server" Value="M" />
                        <asp:HiddenField ID="hdnTemplateFieldId" runat="server" />
                        <asp:HiddenField ID="hdnCtrlValue" runat="server" />
                        <asp:HiddenField ID="hdnXmlVariableDoseString" runat="server" Value="" />
                        <asp:HiddenField ID="hdnvariableDoseDuration" runat="server" Value="" />
                        <asp:HiddenField ID="hdnvariableDoseFrequency" runat="server" Value="" />
                        <asp:HiddenField ID="hdnVariabledose" runat="server" Value="" />
                        <asp:HiddenField ID="hdnXmlFrequencyTime" runat="server" Value="" />
                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                        <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                            Style="display: none;" OnClick="btnIsValidPasswordClose_OnClick" />
                        <asp:HiddenField ID="hdneclaimWebServiceLoginID" runat="server" />
                        <asp:HiddenField ID="hdnEpresActive" runat="server" />
                        <asp:HiddenField ID="hdneclaimWebServicePassword" runat="server" />
                        <asp:HiddenField ID="hdnCopyLastPresc" runat="server" />
                        <asp:HiddenField ID="hdnIsRouteMandatory" runat="server" />
                        <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                </div>
          

            
            
                        <div id="dvConfirmPrint" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 410px; top: 240px">
                            <table width="100%" cellspacing="2" cellpadding="0">
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to print the prescription ?" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"></td>
                                    <td align="center">
                                        <asp:Button ID="btnPrintYes" SkinID="Button" runat="server" Font-Size="Smaller" Text="Yes"
                                            Width="80px" OnClick="btnPrintYes_OnClick" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="btnPrintNo" SkinID="Button" runat="server" Font-Size="Smaller" Text="No"
                                            Width="80px" OnClick="btnPrintNo_OnClick" />
                                    </td>
                                    <td align="center"></td>
                                </tr>
                            </table>
                        </div>
                    
            <div id="dvConfirmAlreadyExistOptions" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 150px; left: 270px; top: 200px;">
                <table cellspacing="2" cellpadding="2" width="400px">
                    <tr>
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="lblSn" runat="server" Text="Drug Name :" ForeColor="#990066" />
                        </td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblItemName" runat="server" ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="Label62" runat="server" Text="Already Order By :" ForeColor="#990066" />
                        </td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr style="border-bottom-style: solid; border-bottom-width: 1px;">
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="Label63" runat="server" Text="Order date :" ForeColor="#990066" />
                        </td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            <asp:Label ID="lblAlertMsg" runat="server" Font-Size="12px" Text="Do you wish to continue...?"
                                ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            <asp:Button ID="btnAlredyExistProceed" runat="server" Text="Proceed" Font-Size="Smaller"
                                OnClick="btnAlredyExistProceed_OnClick" SkinID="Button" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnAlredyExistCancel" runat="server" Text="Cancel" Font-Size="Smaller"
                                OnClick="btnAlredyExistCancel_OnClick" SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divConfirmation" runat="server" style="width: 450px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 60px; left: 270px; top: 200px;">
                <table cellspacing="2" cellpadding="2">
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Label ID="Label69" runat="server" Text="Selected Item is blocked for this company. Do you Want to Continue?" ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Button ID="btnProceed" runat="server" Text="Yes" OnClick="btnProceed_OnClick"
                                SkinID="Button" />
                            &nbsp;&nbsp;
                                        <asp:Button ID="btnProceedCancel" runat="server" Text="No" OnClick="btnProceedCancel_OnClick"
                                            SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>

            <div id="dvConfirm" runat="server" visible="false" style="width: 240px; z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed; bottom: 35%; height: 110px; left: 38%;">
                <table width="100%" cellspacing="2" border="0">
                    <tr>
                        <td style="width: 60px"></td>
                        <td colspan="2">
                            <asp:Label ID="lblConfirmHighValue" Style="font-size: 12px; font-weight: bold; margin: 0.5em 0 0; padding: 0; width: 100%; float: left;"
                                runat="server" Text="This is high value item do you want to proceed" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnYes" CssClass="ICCAViewerBtn" runat="server" Text="Yes" OnClick="btnYes_OnClick" />
                            &nbsp;
                            <asp:Button ID="btnCancel" CssClass="ICCAViewerBtn" runat="server" Text="No" OnClick="btnCancel_OnClick" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            
                        <div id="dvInteraction" runat="server" visible="false" style="width: 700px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 340px; left: 320px; top: 190px">
                            <table width="100%" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label25" runat="server" SkinID="label" Font-Size="11px" Font-Bold="true"
                                            Text="Following drug interaction(s) found !" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<asp:Label ID="lblInteractionBetweenMessage" runat="server" SkinID="label" Font-Size="10px" Font-Bold="true" ForeColor="Maroon" Text="This drug has interaction with prescribed medicines !" />--%>
                                        <asp:TextBox ID="txtInteractionBetweenMessage" runat="server" Font-Size="10px" Font-Bold="true"
                                            ForeColor="Maroon" Text="This drug has interaction with prescribed medicines !"
                                            TextMode="MultiLine" Style="min-height: 56px; max-height: 56px; min-width: 690px; max-width: 690px;"
                                            ReadOnly="true" BackColor="#FFFFCC" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnBrandDetailsView" SkinID="Button" runat="server" Font-Size="Smaller"
                                                        Text="View Brand Details" Width="150px" OnClick="btnBrandDetailsView_OnClick" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnMonographView" SkinID="Button" runat="server" Font-Size="Smaller"
                                                        Text="View Monograph" Width="150px" OnClick="btnMonographView_OnClick" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnInteractionView" SkinID="Button" runat="server" Font-Size="Smaller"
                                                        Text="View Drug Interaction(s)" Width="150px" OnClick="btnInteractionView_OnClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblIntreactionMessage" runat="server" SkinID="label" ForeColor="Red" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label26" runat="server" Text="Reason to continue for Drug Allergy Interaction"
                                                        ForeColor="Gray" />
                                                    <span id="spnCommentsDrugAllergy" runat="server" style="color: Red; font-size: large;"
                                                        visible="false">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtCommentsDrugAllergy" runat="server" SkinID="textbox" MaxLength="500"
                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label37" runat="server" Text="Reason to continue for Drug To Drug Interaction"
                                                        ForeColor="Gray" />
                                                    <span id="spnCommentsDrugToDrug" runat="server" style="color: Red; font-size: large;"
                                                        visible="false">*</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtCommentsDrugToDrug" runat="server" SkinID="textbox" MaxLength="500"
                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" />
                                                </td>
                                            </tr>
                                            <tr id="Tr2" runat="server" visible="false">
                                                <td>
                                                    <asp:Label ID="Label38" runat="server" Text="Reason to continue for Drug Health Interaction"
                                                        ForeColor="Gray" />
                                                    <span id="spnCommentsDrugHealth" runat="server" style="color: Red; font-size: large;"
                                                        visible="false">*</span>
                                                </td>
                                            </tr>
                                            <tr id="Tr3" runat="server" visible="false">
                                                <td>
                                                    <asp:TextBox ID="txtCommentsDrugHealth" runat="server" SkinID="textbox" MaxLength="500"
                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this, 500);" Style="min-height: 44px; max-height: 44px; min-width: 650px; max-width: 650px;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnInteractionContinue" SkinID="Button" runat="server" Font-Size="Smaller"
                                            Text="Continue" Width="150px" OnClick="btnInteractionContinue_OnClick" />
                                        &nbsp;
                                        <asp:Button ID="btnInteractionCancel" SkinID="Button" runat="server" Font-Size="Smaller"
                                            Text="Cancel" Width="150px" OnClick="btnInteractionCancel_OnClick" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="middle">
                                        <table cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td valign="middle">
                                                    <asp:Image ID="Image1" ImageUrl="~/CIMSDatabase/CIMSLogo.PNG" Height="30px" Width="120px"
                                                        runat="server" />
                                                </td>
                                                <td valign="bottom">
                                                    <asp:Label ID="Label40" runat="server" SkinID="label" Font-Size="14px" Font-Bold="true"
                                                        ForeColor="Red" Text="(Powered by CIMS. Copyright MIMS Pte Ltd. All rights reserved.)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    
            <div id="dvPrescriptionDetailConfirm" runat="server" visible="false" style="width: 700px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 340px; left: 320px; top: 190px">
                <table cellspacing="4" cellpadding="4" width="400px">
                    <tr>
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Compounded Prescription Detail :" ForeColor="#882244" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPrescriptionDetailConfirm" runat="server" ReadOnly="true" BackColor="#FFFFCC" ForeColor="Maroon"
                                Text="" TextMode="MultiLine" Height="220px" Width="690px" />
                        </td>
                    </tr>

                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Label ID="Label27" runat="server" Font-Size="12px" Text="Once prescription saved, can't be edited. Do you wish to continue...?"
                                ForeColor="#882244" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Button ID="btnPrescriptionDetailConfirmYes" runat="server" Text="Proceed"
                                OnClick="btnPrescriptionDetailConfirmYes_OnClick" SkinID="Button" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnPrescriptionDetailConfirmNo" runat="server" Text="Cancel"
                                OnClick="btnPrescriptionDetailConfirmNo_OnClick" SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>

            
                        <div id="dvConfirmStop" runat="server" visible="false" style="width: 410px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 140px; left: 500px; top: 200px">
                            <table width="100%" cellspacing="2" cellpadding="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblCancelStopMedicationRemarks" Font-Size="12px" runat="server" Font-Bold="true"
                                            Text="Cancel Medication Remarks" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:TextBox ID="txtStopRemarks" SkinID="textbox" runat="server" TextMode="MultiLine"
                                            Style="min-height: 65px; max-height: 65px; min-width: 390px; max-width: 390px;"
                                            MaxLength="200" onkeyup="return MaxLenTxt(this, );" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center"></td>
                                    <td align="center">
                                        <asp:Button ID="btnStopMedication" SkinID="Button" runat="server" Text="Cancel" Width="80px" OnClick="btnStopMedication_OnClick" />
                                        &nbsp;
                                            <asp:Button ID="btnStopClose" SkinID="Button" runat="server" Text="Close" Width="80px" OnClick="btnStopClose_OnClick" />
                                    </td>
                                    <td align="center"></td>
                                </tr>
                            </table>
                        </div>
                   
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="dvProcess" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="3000" DynamicLayout="true">
        <ProgressTemplate>
            <center>
                <div style="width: 154; position: absolute; bottom: 0; height: 60; left: 500px; top: 300px">
                    <img id="Img1" src="~/Images/loading.gif" alt="loading" runat="server" />
                </div>
            </center>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div id="dverx" runat="server" style="position: absolute; top: 20%; width: 100%; opacity: 0.9; background-color: Gray; height: 200px;"
        visible="false">
        <b>There is Validation Error. Please Correct</b><br />
        <div id="dvInfo" runat="server" />
        <br />
        <asp:Button ID="btnOkay" runat="server" Text="Okay" Font-Size="Smaller" OnClick="btnOkay_Click" />
        <asp:GridView ID="dgError" runat="server" SkinID="gridview" />
    </div>
    <asp:Button ID="btnMedicationOverride" runat="server" Text="" CausesValidation="true"
        SkinID="button" Style="display: none;" OnClick="btnMedicationOverride_OnClick" />
    <asp:HiddenField ID="hdnIsOverride" runat="server" Value="" />
    <asp:HiddenField ID="hdnOverrideComments" runat="server" Value="" />
    <asp:HiddenField ID="hdnDrugAllergyScreeningResult" runat="server" Value="" />

    <script language="javascript" type="text/javascript">
        function OnClientSelectedIndexChanged(sender, eventArgs) {
            var item = eventArgs.get_item();

            if (item.get_text() == "To Be Continued") {
                //  alert(item.get_text());

                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').readOnly = true;
                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').value = '--'
                //  alert(item.get_text());
            }

            else if (item.get_text() == "As Instructed") {
                //  alert(item.get_text());

                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').readOnly = true;
                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').value = '--'
                //  alert(item.get_text());
            }
            else {
                // alert(item.get_text());

                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').readOnly = false;
                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').value = ''
                //  alert(item.get_text());
            }
        }
        function ddlFrequency_OnClientSelectedIndexChanged(sender, eventArgs) {
            var item = eventArgs.get_item();

            if (item.get_text() == "SOS") {
                alert(item.get_text());
                document.getElementById('ctl00_ContentPlaceHolder1_Label6').style.display = 'none'; ctl00_ContentPlaceHolder1_spnDuration

                //  alert(item.get_text());
            }

            else {
                alert(item.get_text());

                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').readOnly = false;
                document.getElementById('ctl00_ContentPlaceHolder1_txtDuration').value = ''
                //  alert(item.get_text());
            }
        }
    </script>
    <script language="javascript" type="text/javascript">

        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false) {
                    return false;
                }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.className += " btn btn-primary";
                myButton.value = "Processing...";
            }
            return true;
        }

        function OpenCIMSWindow() {
            var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

            var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(ReportContent.value);
            WindowObject.document.close();
            WindowObject.focus();
        }

        function OnClientMedicationOverrideClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsOverride = arg.IsOverride;
                var OverrideComments = arg.OverrideComments;
                var DrugAllergyScreeningResult = arg.DrugAllergyScreeningResult;

                $get('<%=hdnIsOverride.ClientID%>').value = IsOverride;
                $get('<%=hdnOverrideComments.ClientID%>').value = OverrideComments;
                $get('<%=hdnDrugAllergyScreeningResult.ClientID%>').value = DrugAllergyScreeningResult;
            }

            $get('<%=btnMedicationOverride.ClientID%>').click();
        }

        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }



        function OnChangeCheckboxRemoveFavourite(checkbox) {
            if (checkbox.checked) {
            }
            else {
            }
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }

        function executeCode(evt) {
            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);

            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                    break;
                    <%--    case 115:  // F4
                    $get('<%=btnAddItem.ClientID%>').click();
                    break;--%>
                case 119:  // F8
                    $get('<%=btnclose1.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrint.ClientID%>').click();
                    break;

            }
            evt.returnValue = false;
            return false;
        }

        function ddlBrand_OnClientSelectedIndexChanged(sender, args) {

            var item = args.get_item();
            $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnItemName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";


            $get('<%=btnGetInfo.ClientID%>').click();
        }

        function ddlBrandOnClientDropDownClosedHandler(sender, args) {

            if (sender.get_text().trim() == "") {

                $get('<%=hdnItemId.ClientID%>').value = "";
                $get('<%=hdnItemName.ClientID%>').value = "";

                $get('<%=hdnCIMSItemId.ClientID%>').value = "";
                $get('<%=hdnCIMSType.ClientID%>').value = "";
                $get('<%=hdnVIDALItemId.ClientID%>').value = "";

                $get('<%=btnGetInfo.ClientID%>').click();
            }
        }

        function OnClientClose(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnReturnIndentOPIPSource.ClientID%>').value = arg.IndentOPIPSource;
            $get('<%=hdnReturnIndentDetailsIds.ClientID%>').value = arg.IndentDetailsIds;
            $get('<%=hdnReturnIndentIds.ClientID%>').value = arg.IndentIds;
            $get('<%=hdnReturnItemIds.ClientID%>').value = arg.ItemIds;
            $get('<%=hdnReturnGenericIds.ClientID%>').value = arg.GenericIds;
            $get('<%=btnRefresh.ClientID%>').click();
        }

        function MaxLenTxt(TXT, MAX) {
            if (TXT.value.length > MAX) {
                alert("Text length should not be greater then " + MAX + " ...");

                TXT.value = TXT.value.substring(0, MAX);
                TXT.focus();
            }
        }

        function OnClientCloseVariableDose(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnXmlVariableDoseString.ClientID%>').value = arg.xmlVariableDoseString;
           $get('<%=hdnvariableDoseDuration.ClientID%>').value = arg.DoseDuration;
           $get('<%=hdnvariableDoseFrequency.ClientID%>').value = arg.DoseFrequency;
           $get('<%=hdnVariabledose.ClientID%>').value = arg.DoseValue;
       }
       function OnClientCloseFrequencyTime(oWnd, args) {
           var arg = args.get_argument();
           $get('<%=hdnXmlFrequencyTime.ClientID%>').value = arg.xmlFrequencyString;
        }

        function clickEnterKey(obj, event) {
            var keyCode;
            if (event.keyCode > 0) {
                keyCode = event.keyCode;
            }
            else if (event.which > 0) {
                keyCode = event.which;
            }
            else {
                keycode = event.charCode;
            }
            if (keyCode == 13) {
                document.getElementById(obj).focus();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
