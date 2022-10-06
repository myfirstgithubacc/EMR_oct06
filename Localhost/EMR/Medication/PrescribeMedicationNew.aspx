<%@ Page Title="Drug Order" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PrescribeMedicationNew.aspx.cs" Inherits="EMR_Medication_PrescribeMedicationNew" %>

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

    <style>


        #ctl00_ContentPlaceHolder1_AccordionPane1_content {
            overflow: hidden !important;
        }

        .hiddencol {
            display: none;
        }
     span#ctl00_ContentPlaceHolder1_lblRequestFromOtherWard {vertical-align: top;}
        div#ctl00_ContentPlaceHolder1_ddlReqestFromOtherWard { position: relative; margin-top: -5px;}
        .legends-row td span {
    font-size: 11px;
    vertical-align: middle;
            white-space: nowrap;
}
        .box-col-checkbox-table {
            white-space: nowrap;
    padding-right: 0;
        }
        #ctl00_ContentPlaceHolder1_ddlBrand {
            margin: 0 !important;
        }

        input#ctl00_ContentPlaceHolder1_ddlAdvisingDoctor_Input {
            padding: 1px 8px !important;
        }

        td.rcbArrowCell.rcbArrowCellRight {
            top: -2px;
        }

        a#ctl00_ContentPlaceHolder1_btnPreviousMedications {
            padding: 2px 4px !important;
        }

        input#ctl00_ContentPlaceHolder1_btnCopyLastPrescription {
            padding: 2px 4px !important;
        }

        tr {
            vertical-align: top;
        }

        input#ctl00_ContentPlaceHolder1_btnRemoveItem {
            margin-left: 12px;
            margin-top: -5px;
        }

        span#ctl00_ContentPlaceHolder1_Label13 {
            margin: 0px !important;
        }

       input#ctl00_ContentPlaceHolder1_chkTaperingDose{
            margin: 1px 3px 0 0;
        }
       input[type="radio"]{
               margin: 1px 2px 0 0!important;
       }
       div#ctl00_ContentPlaceHolder1_ddlAdvisingDoctor{
           margin-top:-2px!important;
       }
       select#ctl00_ContentPlaceHolder1_ddlIndentType{
           text-transform:uppercase!important;
       }
    </style>

    
        <div class="container-fluid">
            <div class="row">
                <asplUD:UserDetails ID="asplUD" runat="server" />
                <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" Visible="false"></asp:Label>
                <div class="col-md-12">
                </div>
            </div>
        </div>
    
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-1 col-sm-1 col-xs-3">
                        <h2>
                            <asp:Label ID="Label1" runat="server" Text="&nbsp;Drug&nbsp;Order" /></h2>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-12">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <asp:Label ID="Label7" runat="server" style="margin:0px!important;padding:0px!important;line-height:18px;" Text="Store" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <asp:DropDownList ID="ddlStore" runat="server" SkinID="DropDown" Width="100%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 col-xs-4">
                                <asp:Label ID="Label18" runat="server" Text="Advising&nbsp;Doctor" />
                            </div>
                            <div class="col-md-8 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlAdvisingDoctor" runat="server" EmptyMessage="[ Select ]" DropDownWidth="200px"
                                    Width="100%" Filter="Contains" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-md-4 col-sm-6 col-xs-4">
                                <asp:Label ID="Label2" runat="server" Style="margin: 0px!important; padding: 0px!important; line-height: 18px;" Text="Order&nbsp;Priority" />
                            </div>
                            <div class="col-md-8 col-sm-6 col-xs-8">
                                <asp:DropDownList ID="ddlIndentType" runat="server" SkinID="DropDown" Width="100%" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 ">
                    <div style="float: left; text-align: left">
                        <button id="liAllergyAlert" runat="server" class="btn btn-default" visible="false" style="background: #fff; border: 0px;">
                            <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" Width="18px" Height="18px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                        </button>
                        <button id="liMedicalAlert" runat="server" visible="false" class="btn btn-default" style="background: #fff; border: 0px;">
                            <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif" OnClick="imgMedicalAlert_OnClick" CssClass="iconEMRimg" Width="18px" Height="18px" Visible="false" ToolTip="Patient Alert" />
                        </button>
                    </div>
                    <div class="text-right">
                        <asp:Button ID="btnOrderSet" runat="server" Text="Add Med. Set" CssClass="btn btn-primary" OnClick="btnOrderSet_OnClick" />
                        <asp:Button ID="btnAddOrderSetClose" runat="server" Style="visibility: hidden;" OnClick="btnAddOrderSetClose_OnClick" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" ToolTip="(Ctrl+F9)" CssClass="btn btn-primary"
                                OnClick="btnPrint_Click" Visible="false" CausesValidation="false" />
                            <asp:Button ID="btnSave" runat="server" CausesValidation="false" ToolTip="(Ctrl+F3)" Text="Save "
                                CssClass="btn btn-primary" OnClick="btnSave_Onclick" OnClientClick="ClientSideClick(this)"
                                UseSubmitBehavior="False" />
                            <asp:Button ID="btnclose" Text="Close" ToolTip="(Ctrl+F8)" runat="server" CssClass="btn btn-primary"
                                Visible="false" OnClick="btnClose_OnClick" />
                            <asp:HiddenField ID="hdnIsSelectEncounterDoctorId" runat="server" Value="0" />
                            <asp:Button ID="btnclose1" Visible="false" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>

                </div>
                <div class="row text-center">
                            <asp:Label ID="lblMessage" style="position:relative;width:100%;margin:0px;" runat="server" Text="" />
                    </div>
                <div class="row">
                    

                    <div id="Td1" runat="server" visible="false">
                        <asp:Label ID="Label65" runat="server" Text="Prescription No" SkinID="label" />&nbsp;
                            <telerik:RadComboBox ID="ddlPrescription" runat="server" EmptyMessage="[ Select ]"
                                Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlPrescription_SelectedIndexChanged" />
                        <%--<aspNewControls:NewDropDownList ID="ddlPrescription" runat="server" SkinID="DropDown" class="chosen-select" Font-Size="10px" data-placeholder="[ Select ]" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlPrescription_SelectedIndexChanged" />--%>&nbsp;
                            <asp:Button ID="Button1" runat="server" SkinID="Button" Text="Print" ToolTip="(Ctrl+F9)" OnClick="btnPrint_Click"
                                Visible="false" CausesValidation="false" />
                    </div>
                </div>
            
                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="Label19" runat="server" Text="Prov.&nbsp;Diagnosis" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" TextMode="MultiLine" ReadOnly="true" Width="100%" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5" id="trGeneric" runat="server">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="Label16" runat="server" Text="Generic" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlGeneric" runat="server" DataTextField="GenericName"
                                    DataValueField="GenericId" HighlightTemplatedItems="true" Height="300px" Width="100%"
                                    DropDownWidth="450px" ZIndex="50000" EmptyMessage=" " AllowCustomText="true"
                                    MarkFirstMatch="true" EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                    OnItemsRequested="ddlGeneric_OnItemsRequested" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged">
                                    <HeaderTemplate>
                                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td style="width: 100%" align="left">
                                                    <asp:Label ID="Label28" runat="server" Text="Generic Name" />
                                                </td>
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
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </div>
                        </div>

                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-6 text-nowrap">
                                        <asp:Label ID="Label11" runat="server" Text="Height&nbsp;(Cm):&nbsp;" />
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <asp:Label ID="txtHeight" runat="server" ForeColor="Maroon" Width="30px" Style="margin-left: 18px;" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-sm-3 col-xs-6">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-6 text-nowrap">
                                        <asp:Label ID="Label12" runat="server" Text="Weight&nbsp;(Kg):&nbsp;" />
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <asp:Label ID="lbl_Weight" runat="server" ForeColor="Maroon" Width="30px" Style="margin-left: 18px;" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:Button ID="btnBrandDetailsViewOnItemBased" CssClass="btn btn-primary" runat="server"
                                    Text="View Brand Details" Visible="false" OnClick="btnBrandDetailsView_OnClick" />
                                <asp:Button ID="btnMonographViewOnItemBased" CssClass="btn btn-primary" runat="server"
                                    Text="View Monograph" Visible="false" OnClick="btnMonographView_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="Label30" runat="server" Text="Medication&nbsp;List" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox-table">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" class="radio-outer">
                                        <ContentTemplate>
                                            <asp:RadioButtonList ID="rdoSearchMedication" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rdoSearchMedication_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Favourite" Value="F" Selected="True" />
                                                <asp:ListItem Text="Current" Value="C" />
                                                <asp:ListItem Text="Medication Set" Value="OS" />
                                                <%-- <asp:ListItem Text="Previous" Value="P" />--%>
                                            </asp:RadioButtonList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5" id="trDrugs" runat="server">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="lblInfoBrand" runat="server" Text="Brand" />
                            </div>
                           <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlBrand" />
                                        </Triggers>
                                        <ContentTemplate>

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
                                            <telerik:RadComboBox ID="ddlBrand" TabIndex="1" runat="server" HighlightTemplatedItems="true"
                                                Width="100%" DropDownWidth="450px" Height="300px" EmptyMessage=" "
                                                AllowCustomText="true" EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                MarkFirstMatch="false" ZIndex="50000" OnItemsRequested="ddlBrand_OnItemsRequested"
                                                OnClientItemsRequesting="OnClientItemsRequesting" OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged"
                                                OnClientDropDownClosed="ddlBrandOnClientDropDownClosedHandler">
                                                <HeaderTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 70%" align="left">
                                                                <asp:Label ID="Label28" runat="server" Text="Item Name" />
                                                            </td>
                                                            <td style="width: 10%" align="right" runat="server" id="tdClosingBalance">
                                                                <asp:Label ID="lblClosingBalance" runat="server" Text="QTY" />
                                                            </td>
                                                            <td style="width: 20%" align="right" id="tdmargin" runat="server">
                                                                <asp:Label ID="Label8" runat="server" Text="Margin %" visible="false" />
                                                            </td>
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
                                                            <td style="width: 70%" align="left">
                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                            </td>
                                                            <td style="width: 10%" align="right" id="tdItemClosingBalance" runat="server">
                                                                <asp:Label ID="lblClosingBalanceItem" runat="server" Text='<%# DataBinder.Eval(Container, "Attributes[\"ClosingBalance\"]")%>' />
                                                            </td>
                                                            <td style="width: 20%" align="right">
                                                             <asp:Label id="lblProfitPercentage" runat="server" Text='<%# DataBinder.Eval(Container, "Attributes[\"ProfitPercentage\"]")%>'/>
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
                            </div>
                        </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:LinkButton ID="lnkStopMedication" runat="server" SkinID="label" CssClass="PrescriptionsLink"
                                    Font-Size="12px" OnClick="lnkStopMedication_OnClick" Text="Stopped Medication"
                                    ToolTip="Click to see stoped medication" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:LinkButton ID="lnkCompoundedDrugOrder" runat="server" SkinID="label" CssClass="PrescriptionsLink"
                                    Font-Size="12px" OnClick="lnkCompoundedDrugOrder_OnClick" Text="Compounded Drug Order"
                                    ToolTip="Compounded Drug Order" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Button ID="btnAddtoFav" runat="server" Text="Add To Favourite" ToolTip="Add To Favourite"
                                    CssClass="btn btn-primary pull-right" OnClick="btnAddtoFav_Click" OnClientClick="ClientSideClick(this)"
                                    UseSubmitBehavior="False" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5" id="tblFavouriteSearch" runat="server">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="Label24" runat="server" Text="Search&nbsp;Drug&nbsp;Name" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Panel ID="Panel6" runat="server" DefaultButton="btnSearchFavourite">
                                    <asp:TextBox ID="txtFavouriteItemName" runat="server" Width="100%" />
                                </asp:Panel>
                                <div style="float:right;position:absolute;right:15px;top:0px;">
                                <asp:ImageButton ID="btnProceedFavourite" runat="server" ToolTip="Click here to proceed selected favourite"
                                    ImageUrl="~/Images/Login/orrange-arrow.GIF" style="width:18px;height:21px!important;" OnClick="btnProceedFavourite_OnClick" />
                            </div>
                            </div>
                            
                        </div>

                        <div class="row p-t" id="tblCurrentSearch" runat="server" visible="false">
                             <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="Label27" runat="server" Text="Search Drug Name" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Panel ID="Panel7" runat="server" DefaultButton="btnSearchCurrent">
                                    <asp:TextBox ID="txtCurrentItemName" runat="server" Width="100%" />
                                </asp:Panel>
                            <div style="float:right;position:absolute;right:15px;top:0px;">
                                <asp:ImageButton ID="btnProceedCurrent" runat="server" ToolTip="Click here to proceed selected current medication"
                                    ImageUrl="~/Images/Login/orrange-arrow.GIF" Width="18px" OnClick="btnProceedCurrent_OnClick" />
                            </div>
                        </div>
                            </div>

                        <div class="row p-t" id="tblOrderSetSearch" runat="server" visible="false">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Label ID="Label68" runat="server" SkinID="label" Text="Search Medication Set" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearchOrderSet">
                                    <asp:TextBox ID="txtOrderSetName" runat="server" />
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkCustomMedication" runat="server" AutoPostBack="true" OnCheckedChanged="chkCustomMedication_OnCheckedChanged"
                                                Text="Custom&nbsp;Medication" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                            </div>

                            <div class="col-md-6 col-sm-6 col-xs-6">
                                <asp:CheckBox ID="chkApprovalRequired" runat="server" Visible="false"  AutoPostBack="true"  Text="Verbal/Telephonic" TextAlign="Right"
                                        OnCheckedChanged="chkApprovalRequired_OnCheckedChanged" />
                            </div>                          
                        </div>
                    </div>

                      <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-6 col-sm-7 col-xs-7">
                                <asp:CheckBox ID="ChkReqestFromOtherWard" OnCheckedChanged="ChkReqestFromOtherWard_CheckedChanged" Visible="true" AutoPostBack="true" runat="server" />
                                <asp:Label ID="lblRequestFromOtherWard" runat="server" Visible="true" Text="Request From..." ToolTip="Request From Other Ward" />
                                <telerik:RadComboBox ID="ddlReqestFromOtherWard" runat="server" DropDownWidth="100px" Width="100px"
                                    EmptyMessage="[ Select ]" AutoPostBack="true" Visible="false" />
                            </div>
                            <div class="col-md-6 col-sm-5 col-xs-5 text-right">
                                <asp:LinkButton ID="btnPreviousMedications" runat="server" CssClass="btn btn-primary"
                                    OnClick="btnPreviousMedications_Click" Text="Previous Medi." ToolTip="Previous Medications" />
                                <asp:LinkButton ID="lnkDrugAllergy" runat="server" BackColor="#82CAFA" Font-Size="10px"
                                    Text="Drug&nbsp;Allergy" ToolTip="Drug Allergy" Font-Bold="true" OnClick="lnkDrugAllergy_OnClick"
                                    Visible="false" />
                                <asp:Button ID="btnCopyLastPrescription" runat="server" CssClass="btn btn-primary"
                                    Text="Copy Last Rx" ToolTip="Copy Last Prescription" OnClick="btnCopyLastPrescription_Click" OnClientClick="ClientSideClick(this)"
                                    UseSubmitBehavior="False" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-12" id="dvReason" runat="server" visible="false">
                          <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Label10" runat="server" Text="Reason" /><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <telerik:RadComboBox ID="ddlReason" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                </div>
                            </div>
                                </div>

                            <div class="col-md-12 col-sm-12 col-xs-12" id="trCustomMedication" runat="server" visible="false">
                                <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:Label ID="Label23" runat="server" Text="Custom Medication" />
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:TextBox ID="txtCustomMedication" runat="server" MaxLength="1000" TextMode="MultiLine"
                                        onkeyup="return MaxLenTxt(this, 1000);" Style="width: 100%; height: 22px; line-height: 1.0em" />
                                </div>
                            </div>
                                </div>
                        </div>
                    </div>
                    <div class="col-md-7 PaddingRightSpacing">
                    <asp:CheckBox ID="chkShowDetails" runat="server" Font-Size="12px" Text="Show Details"
                                    AutoPostBack="true" OnCheckedChanged="chkShowDetails_OnCheckedChanged" CssClass="pull-left" />
                                
                    </div>
                </div>
                


                <div class="row">
                    
                    <div class="col-md-3 col-sm-3 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-12">
                                         <asp:CheckBox ID="chkIsReadBack" runat="server" TabIndex="43" Text="Read Back" TextAlign="Right" Visible="false" />

                                        <asp:Label ID="lblReadBackNote" runat="server" Text="Read&nbsp;Back&nbsp;Note" Visible="false"  ></asp:Label>

                                        <asp:TextBox ID="txtIsReadBackNote" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Visible="false" CssClass="pull-left"></asp:TextBox>
                                    </div>
                                    
                                </div>
                                </div>
                     <div class="col-md-5 col-sm-5 col-xs-12 p-t-b-5">
                                <asp:Label ID="lblGenericName" runat="server" Text="" />
                            </div>
                </div>
           
                <div class="row">
                    <div class="col-md-4 col-sm-4 colxs-12 m-t">
                        <div class="row">
                            <div class="col-md-12 gridview">
                                <asp:Panel ID="pnlFavouriteDetails" runat="server" BorderStyle="Solid" BorderWidth="0px"
                                    Height="440px" ScrollBars="Auto">
                                    <asp:GridView ID="lstFavourite" runat="server" Width="100%" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="15" OnPageIndexChanging="lstFavourite_PageIndexChanging"
                                        OnRowDataBound="lstFavourite_OnRowDataBound" OnRowCommand="lstFavourite_OnRowCommand"
                                        HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                        HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                        BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" SkinID="label" ForeColor="Red" Font-Bold="true" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right"><ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate></asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Favourite Drug(s)">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkItemName" runat="server" ToolTip="Click here to add new prescription"
                                                        CausesValidation="false" CommandName='selectItem'
                                                        Text='<%#Eval("ItemNameWithAttributes")%>' Font-Size="Smaller" />
                                                    <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%#Eval("GenericId")%>' />
                                                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                    <asp:HiddenField ID="hdnFavItemName" runat="server" Value='<%#Eval("ItemName")%>' />
                                                    <asp:HiddenField ID="hdnDose" runat="server" Value='<%#Eval("Dose")%>' />
                                                    <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId")%>' />
                                                    <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId")%>' />
                                                    <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%#Eval("StrengthValue")%>' />
                                                    <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%#Eval("FormulationId")%>' />
                                                    <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId")%>' />
                                                    <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId")%>' />
                                                    <asp:HiddenField ID="hdnDuration" runat="server" Value='<%#Eval("Duration")%>' />
                                                    <asp:HiddenField ID="hdnDurationType" runat="server" Value='<%#Eval("DurationType")%>' />
                                                    <asp:HiddenField ID="hdnFoodRelationshipId" runat="server" Value='<%#Eval("FoodRelationshipId")%>' />
                                                    <asp:HiddenField ID="hdnFCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                    <asp:HiddenField ID="hdnFCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                    <asp:HiddenField ID="hdnFVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                    <asp:HiddenField ID="hdnInstructions" runat="server" Value='<%#Eval("Instructions") %>' />
                                                    <asp:HiddenField ID="hdnPrescription" runat="server" Value='<%#Eval("Prescription") %>' />   <%-- Akshay_13072022_Evercare_Hyderabad--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                        CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                        ImageUrl="~/Images/DeleteRow.png" Height="16px" Width="16px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnlCurrentDetails" runat="server" Visible="false" BorderStyle="Solid"
                                    BorderWidth="0px" Height="440px" ScrollBars="Auto" >
                                    <asp:GridView ID="gvPrevious" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                                        AutoGenerateColumns="False" OnRowCreated="gvPrevious_OnRowCreated" OnRowDataBound="gvPrevious_OnRowDataBound"
                                        OnRowCommand="gvPrevious_OnRowCommand">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%# Eval("IndentNo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Text='<%# Eval("IndentDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current Drug(s)">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkItemName" runat="server" ToolTip="Click here to add new prescription"
                                                        CausesValidation="false" CommandName='selectItem' CommandArgument='<%#Eval("ItemId")%>'
                                                        Text='<%#Eval("ItemName")%>' Font-Size="Smaller" />
                                                    <asp:Label ID="lblGenericName" runat="server" Style="visibility: hidden" Text='<%#Eval("GenericName")%>' />
                                                    <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                    <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                    <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />
                                                    <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                    <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                    <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                    <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId") %>' />
                                                    <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                                    <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                                    <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%#Eval("StrengthValue")%>' />
                                                    <asp:HiddenField ID="hdnXMLData" runat="server" />
                                                    <asp:HiddenField ID="hdnDose" runat="server" Value='<%#Eval("Dose") %>' />
                                                    <asp:HiddenField ID="hdnDays" runat="server" Value='<%#Eval("Days") %>' />
                                                    <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                                    <asp:HiddenField ID="hdnIndentDetailsId" runat="server" Value='<%#Eval("DetailsId") %>' />
                                                    <asp:HiddenField ID="hdnDuration" runat="server" Value='<%#Eval("Duration")%>' />
                                                    <asp:HiddenField ID="hdnInstructions" runat="server" Value='<%#Eval("Instructions") %>' />
                                                    <asp:HiddenField ID="hdnFoodRelationshipId" runat="server" Value='<%#Eval("FoodRelationshipId")%>' />
                                                    <asp:HiddenField ID="hdnStartDate" runat="server" Value='<%# Eval("StartDate") %>' />
                                                    <asp:HiddenField ID="hdnType" runat="server" Value='<%# Eval("Type") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%# Eval("IndentType") %>' />
                                                    <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="40px"
                                                        SkinID="label" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>'
                                                        SkinID="label" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" SkinID="label" Text='<%# Eval("StartDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" SkinID="label" Text='<%# Eval("EndDate") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label41" runat="server" Text="BD" ToolTip="Brand Details" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view cims brand details"
                                                        CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label42" runat="server" Text="MG" ToolTip="Monograph" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                                        CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label43" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                                        CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                        Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="LabelDuplicate" runat="server" Text="DI" ToolTip="Duplicate Ingredient" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnDIInteractionCIMS" runat="server" ToolTip="Click here to view cims Duplicate Ingredient"
                                                        CommandName="DIInteractionCIMS" CausesValidation="false" Text="&nbsp;"
                                                        Width="17px" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label44" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                        CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label45" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnDAInteractionCIMS" runat="server" ToolTip="Click here to view cims drug allergy interaction"
                                                        CommandName="DAInteractionCIMS" CausesValidation="false" BackColor="#82CAFA"
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label46" runat="server" Text="BD" ToolTip="Brand Details" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnBrandDetailsVIDAL" runat="server" ToolTip="Click here to view vidal brand details"
                                                        CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label47" runat="server" Text="MG" ToolTip="Monograph" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnMonographVIDAL" runat="server" ToolTip="Click here to view vidal monograph"
                                                        CommandName="MonographVIDAL" CausesValidation="false" CommandArgument='<%#Eval("VIDALItemId")%>'
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label48" runat="server" Text="DD" ToolTip="Drug to Drug Interaction" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug to drug interaction"
                                                        CommandName="InteractionVIDAL" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                        Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label49" runat="server" Text="DH" ToolTip="Drug Health Interaction" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnDHInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug health interaction"
                                                        CommandName="DHInteractionVIDAL" CausesValidation="false" BackColor="#82AB76"
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15px">
                                                <HeaderTemplate>
                                                    <asp:Label ID="Label50" runat="server" Text="DA" ToolTip="Drug Allergy Interaction" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnDAInteractionVIDAL" runat="server" ToolTip="Click here to view vidal drug allergy interaction"
                                                        CommandName="DAInteractionVIDAL" CausesValidation="false" BackColor="#82CAFA"
                                                        Text="&nbsp;" Width="100%" Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stop Remarks" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" Height="20px" Width="100%"
                                                        TextMode="MultiLine" Text="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" HeaderText="Stop">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnStop" runat="server" ToolTip="Click here to stop this drug"
                                                        CommandName="ItemStop" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                        ImageUrl="~/Images/Redtick71.GIF" Height="16px" Width="16px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnlOrderSet" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="0px"
                                    Height="440px" ScrollBars="Auto">
                                    <asp:GridView ID="gvOrderSet" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                                        AutoGenerateColumns="False" OnRowCommand="gvOrderSet_OnRowCommand" OnRowDataBound="gvOrderSet_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Medication Set(s)">
                                                <ItemTemplate>
                                                    <%--ToolTip="Click here to add new medication set"--%>
                                                    <asp:LinkButton ID="lnkSetName" runat="server" 
                                                        CausesValidation="false" CommandName='SelectOrderSet' CommandArgument='<%#Eval("SetId")%>'
                                                        Text='<%#Eval("SetName")%>' Font-Size="Smaller" />
                                                    <asp:HiddenField ID="hdnDrugName" runat="server" Value='<%# Eval("DrugName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                        <div class="col-md-12 col-sm12 col-xs-12 border-all">
                             <asp:Panel ID="Panel5" runat="server">
                            <div class="row p-t-b-5" style="background:#9caabe;">
                                <div class="col-md-4 col-sm-4 col-xs-5">
                                    <asp:Label ID="Label22" runat="server" Text="Drug Attributes" />
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-7">
                                    <asp:CheckBox ID="chkFractionalDose" runat="server" Text="Fractional Dose" />
                                    <asp:LinkButton ID="lnkPharmacistInstruction" runat="server" Font-Bold="true" OnClick="lnkPharmacistInstruction_OnClick"
                                        Text="Instruction For Patient" ToolTip="Instruction For Patient" Font-Size="Smaller" CssClass="btn btn-primary pull-right"
                                        Visible="false" />
                                </div>
                            </div>
                                 </asp:Panel>
                        
                        

                            <asp:Panel ID="Panel4" runat="server">
                                <div class="row p-t-b-5">
                                    <div class="col-md-2 col-sm-3 col-xs-3" style="margin-top: 8px;">
                                        <%--Add By Himanshu In Text properties Dose to Dose/Qty On Date 17/03/2022 For Sanar Hospital --%>
                                        <asp:Label ID="lblDose" runat="server" Text="Dose/Qty" />&nbsp;<span class="redStar3" id="spnDose" runat="server">*</span>
                                    </div>
                                    <div class="col-md-10 col-sm-9 col-xs-9">
                                        <div class="row" style="padding-top: 5px">
                                            <div class="col-md-8 col-sm-9 col-xs-8">
                                                <div class="row ">
                                                    <div class="col-md-5 col-sm-4 col-xs-4 no-p-r">
                                                        <asp:TextBox ID="txtDose" TabIndex="13" runat="server" Text='<%#Eval("Dose") %>' Width="100%" MaxLength="5"
                                                            Style="text-align: left; height: 22px;" />
                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                            FilterType="Custom, Numbers" TargetControlID="txtDose" ValidChars="." />
                                                    </div>
                                                    <div class="col-md-2 col-sm-3 col-xs-3 text-nowrap" style="margin-right: 5px;">
                                                        <asp:Label ID="Label13" runat="server" Text="Unit" ToolTip="Unit Preparation" /><span class="redStar3" id="spnUnit" runat="server">*</span>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4 no-p-l">
                                                        <telerik:RadComboBox ID="ddlUnit" runat="server" TabIndex="14" CssClass="UnitSelect" MarkFirstMatch="true" DropDownWidth="120%"
                                                            Filter="Contains" Width="100%" Height="250px" EmptyMessage="[ Select ]" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-3 col-xs-3" style="padding-top: 6px!important; padding-left: 10px!important;">
                                                <asp:Button ID="btnRemoveItem" runat="server" CssClass="btn btn-primary" Text="Remove"
                                                    OnClick="btnRemoveItem_OnClick" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="Label4" runat="server" Text="Frequency" />&nbsp;<span class="redStar3" id="spnFrequency" runat="server">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <div class="row">
                                       <div class="col-md-8 col-sm-8 col-xs-8">
                                                    <telerik:RadComboBox ID="ddlFrequencyId" runat="server" TabIndex="15" Width="100%" DropDownWidth="220px" MarkFirstMatch="true" Filter="Contains"
                                                    EmptyMessage="[ Select ]" Height="250px" AutoPostBack="true" OnSelectedIndexChanged="ddlFrequency_OnSelectedIndexChanged" />
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4 no-p-l">
                                                    <asp:LinkButton ID="lbtnFrequencyTime" runat="server" CssClass="btn btn-primary"
                                                    OnClick="lbtnFrequencyTime_OnClick" Text="Dosage Time" />
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                  <asp:Label ID="Label6" runat="server" Text="Duration" />&nbsp;<span class="redStar3" id="spnDuration" runat="server">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <div class="row">
                                       <div class="col-md-7 col-sm-7 col-xs-7">
                                                    <asp:TextBox ID="txtDuration" runat="server" TabIndex="16" Height="22px" Text='<%#Eval("Duration") %>' Width="100%"
                                                    MaxLength="2" />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    FilterType="Custom, Numbers" TargetControlID="txtDuration" ValidChars="" />
                                                </div>
                                                <div class="col-md-5 col-sm-5 col-xs-5">
                                                    <telerik:RadComboBox ID="ddlPeriodType" runat="server" Width="100%" TabIndex="17" OnClientSelectedIndexChanged="OnClientSelectedIndexChanged">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="Minute(s)" Value="N" />
                                                        <telerik:RadComboBoxItem Text="Hour(s)" Value="H" />
                                                        <telerik:RadComboBoxItem Text="Day(s)" Value="D" Selected="true" />
                                                        <telerik:RadComboBoxItem Text="Week(s)" Value="W" />
                                                        <telerik:RadComboBoxItem Text="Month(s)" Value="M" />
                                                        <telerik:RadComboBoxItem Text="Year(s)" Value="Y" />
                                                        <telerik:RadComboBoxItem Text="To Be Continued" Value="C" />
                                                        <telerik:RadComboBoxItem Text="Till next visit" Value="V" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                  <asp:Label ID="lblStrength" runat="server" Text="Strength" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <div class="row">
                                       <div class="col-md-5 col-sm-5 col-xs-5">
                                                   <telerik:RadComboBox ID="ddlStrengthValue" runat="server" SkinID="DropDown" TabIndex="17" Height="100px"
                                                    Width="100%" DropDownWidth="200px" AllowCustomText="true" EmptyMessage=" " Filter="StartsWith" />
                                                </div>
                                                <div class="col-md-7 col-sm-7 col-xs-7 no-p-l">
                                                  <asp:CheckBox ID="chkSubstituteNotAllow" runat="server" TabIndex="18" Text="Substitute&nbsp;Not&nbsp;Allowed" />
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="Label3" runat="server" Text="Form" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadComboBox ID="ddlFormulation" runat="server" TabIndex="19" MarkFirstMatch="true" Filter="Contains"
                                            EmptyMessage="[ Select ]" AutoPostBack="true" OnSelectedIndexChanged="ddlFormulation_OnSelectedIndexChanged"
                                            Width="100%" Height="220px" />
                                </div>
                                </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                     <asp:Label ID="Label9" runat="server" Text="Route" />&nbsp;<span id="spnRoute" runat="server" class="redStar3">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadComboBox ID="ddlRoute" runat="server" MarkFirstMatch="true" TabIndex="20" Filter="Contains"
                                            EmptyMessage="[ Select ]" Width="100%" Height="220px" />
                                </div>
                                </div>
                            <div class="row p-t-b-5" id="trDiagnosis" runat="server" visible="false">
                                <div class="col-md-3 col-sm-3 col-xs-3 ">
                                    <asp:Label ID="label64" runat="server" SkinID="label" Text="Diagnosis" /><span style="color: Red; font-weight: bold;">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:UpdatePanel ID="indicationsel" runat="server">
                                            <ContentTemplate>
                                                <input id="hdnICDCode" value='<%# Eval("ICDCodes")%>' type="hidden" runat="server" />
                                                <input id="hdnExitOrNot" value='<%# Eval("ExitOrNot")%>' type="hidden" runat="server" />
                                                <asp:TextBox ID="txtICDCode" runat="server" SkinID="textbox" Wrap="true" MaxLength="200"
                                                    Visible="true" Width="100px" AutoPostBack="True" OnTextChanged="txtICDCode_TextChanged" />
                                                <asp:Panel ID="pnlICDCodes" BorderStyle="Solid" BorderWidth="1px" Style="visibility: hidden; position: relative;"
                                                    BackColor="#E0EBFD" runat="server" Height="100px" ScrollBars="Auto"
                                                    Width="100%">
                                                    <asp:UpdatePanel ID="update" runat="server">
                                                        <ContentTemplate>
                                                            <aspl:ICD ID="icd" runat="server" width="400px" PanelName="ctl00_ContentPlaceHolder1_pnlICDCodes"
                                                                ICDTextBox="ctl00_ContentPlaceHolder1_txtICDCode" />
                                                            <asp:HiddenField ID="hdnGridClientId" runat="server" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                                <AJAX:PopupControlExtender ID="PopUnit" runat="server" TargetControlID="txtICDCode"
                                                    PopupControlID="pnlICDCodes" Position="Right" OffsetX="5" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                </div>
                                </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="Label17" runat="server" Text="Food&nbsp;Relation" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadComboBox ID="ddlFoodRelation" TabIndex="21" runat="server" Filter="Contains" EmptyMessage="[ Select ]"
                                            Width="100%" Height="200px" ToolTip="Relationship to Food" />
                                </div>
                                </div>
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="Label14" runat="server" Text="Start Date" /><span class="redStar3" id="spnStartdate" runat="server" visible="false">*</span>
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4 no-p-r">
                                                <%--AutoPostBack="true" OnSelectedDateChanged="txtStartDate_OnSelectedDateChanged"--%>
                                                <telerik:RadDatePicker ID="txtStartDate" runat="server" TabIndex="22" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" DbSelectedDate='<%#Eval("StartDate")%>'>
                                                    <DateInput ID="DateInput1" runat="server" DateFormat="dd/MM/yyyy" />
                                                </telerik:RadDatePicker>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <asp:Label ID="Label15" runat="server" Text="End Date" Visible="false" />
                                                <asp:Label ID="Label21" runat="server" Text="Order Type" />
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4 no-p-l">
                                                <telerik:RadComboBox ID="ddlDoseType" runat="server" TabIndex="23" Width="100%" EmptyMessage="[ Select ]"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlDoseType_OnSelectedIndexChanged" />
                                                <%--<asp:DropDownList ID="ddlDoseType" runat="server" SkinID="DropDown" Width="110px" AutoPostBack="true" OnSelectedIndexChanged="ddlDoseType_OnSelectedIndexChanged" />--%>
                                                <telerik:RadDatePicker ID="txtEndDate" runat="server" Width="100px" Enabled="false"
                                                    DbSelectedDate='<%#Eval("EndDate")%>' Visible="false">
                                                    <DateInput ID="DateInput2" runat="server" DateFormat="dd/MM/yyyy" />
                                                </telerik:RadDatePicker>
                                            </div>
                                        </div>
                                </div>
                                </div>
                            
                                <div id="maindivofControl" runat="server">
                                        <AJAX:Accordion ID="Accordion1" runat="server" Width="100%" FadeTransitions="true" TransitionDuration="500" AutoSize="None" SelectedIndex="-1"
                                            RequireOpenedPane="false">
                                            <Panes>
                                                <AJAX:AccordionPane ID="AccordionPane1" runat="server">
                                                   
                                                        
                                                            <Header>
                                                                <div style="background:#60AFC3;padding:3px 10px;">
                                                                    <b>Click here for linked item details</b>
                                                                </div>
                                                            </Header>
                                                       
                                                   
                                                    
                                                    <Content>
                                                        <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="Label5" runat="server" Text="Linked Item" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <telerik:RadComboBox ID="ddlReferanceItem" runat="server" Width="100%" EmptyMessage="[ Select ]"
                                                                    AppendDataBoundItems="true">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="" Value="-1" Selected="true" />
                                                                        <telerik:RadComboBoxItem Text="Diluents" Value="0" />
                                                                        <telerik:RadComboBoxItem Text="Normal Saline" Value="100001" />
                                                                        <telerik:RadComboBoxItem Text="Dextrose 5%" Value="100002" />
                                                                        <telerik:RadComboBoxItem Text="Dextrose 10%" Value="100005" />
                                                                        <telerik:RadComboBoxItem Text="Dextrose 12%" Value="100006" />
                                                                        <telerik:RadComboBoxItem Text="Dextrose 15%" Value="100007" />
                                                                        <telerik:RadComboBoxItem Text="Dextrose 20%" Value="100008" />
                                                                        <telerik:RadComboBoxItem Text="DNS" Value="100003" />
                                                                        <telerik:RadComboBoxItem Text="Others" Value="100004" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                </div>
                                </div>
                                                      
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-3 col-sm-3 col-xs-3">
                                                                        <asp:Label ID="Label29" runat="server" Text="Volume" />
                                                                    </div>

                                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                                <div class="row">
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <asp:TextBox ID="txtVolume" runat="server" Text="" Width="100%" Height="22px" MaxLength="5" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                            FilterType="Custom, Numbers" TargetControlID="txtVolume" ValidChars="." />
                                                                    </div>
                                                                     <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <asp:Label ID="Label34" runat="server" Text="Volume Unit" />
                                                                    </div>
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <telerik:RadComboBox ID="ddlVolumeUnit" runat="server" MarkFirstMatch="true" EmptyMessage="[ Select ]"
                                                                            ToolTip="Volume unit" Width="100%" DropDownWidth="150px" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-3 col-sm-3 col-xs-3">
                                                                <asp:Label ID="Label35" runat="server" Text="Flow Rate" />
                                                                </div>
                                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                                <div class="row">
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <asp:TextBox ID="txtFlowRateUnit" runat="server" Height="22px" Text="" Width="100%" MaxLength="5" Style="text-align: left" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                            FilterType="Custom, Numbers" TargetControlID="txtFlowRateUnit" ValidChars="." />
                                                                    </div>
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <asp:Label ID="Label36" runat="server" Text="Rate&nbsp;Unit" />
                                                                    </div>
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                         <telerik:RadComboBox ID="ddlFlowRateUnit" Width="100%" runat="server" MarkFirstMatch="true"
                                                                            EmptyMessage="[ Select ]" ToolTip="Flow Rate Unit" DropDownWidth="150px" />
                                                                    </div>
                                                            </div>
                                                                </div>
                                                            </div>
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-3 col-sm-3 col-xs-3">
                                                                <asp:Label ID="Label32" runat="server" Text="Total Volume" Visible="false"
                                                                    Font-Size="Smaller" />
                                                                <asp:Label ID="Label33" runat="server" Text="Time" />
                                                                </div>
                                                            <div class="col-md-9 col-sm-9 col-xs-9">
                                                                <div class="row">
                                                                    <div class="col-md-7 col-sm-7 col-xs-7">
                                                                        <asp:TextBox ID="txtTotalVolumn" runat="server" Text="" Height="22px" Width="100%"
                                                                            MaxLength="50" Visible="false" />
                                                                        <asp:TextBox ID="txtTimeInfusion" runat="server" Text="" Width="100%" Height="22px" MaxLength="5" Style="text-align: left; margin: 0 9px 0 0px;" />
                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                            FilterType="Custom, Numbers" TargetControlID="txtTimeInfusion" ValidChars="." />
                                                                    </div>
                                                                    <div class="col-md-5 col-sm-5 col-xs-5">
                                                                        <telerik:RadComboBox ID="ddlTimeUnit" Width="100%" runat="server" MarkFirstMatch="true"
                                                                            EmptyMessage="[ Select ]" ToolTip="Infusion Time unit">
                                                                            <Items>
                                                                                <telerik:RadComboBoxItem Value="0" Text="" Selected="true" />
                                                                                <telerik:RadComboBoxItem Value="H" Text="Hour(s)" />
                                                                                <telerik:RadComboBoxItem Value="M" Text="Minute(s)" />
                                                                                <%--<telerik:RadComboBoxItem Value="S" Text="Second (S)" />--%>
                                                                            </Items>
                                                                        </telerik:RadComboBox>
                                                                        <%--<asp:DropDownList ID="ddlTimeUnit" Width="80px" runat="server" SkinID="DropDown"
                                                                    ToolTip="Infusion Time unit">
                                                                    <asp:ListItem Text="" Value="0" Selected="true" />
                                                                    <asp:ListItem Text="Hour(s)" Value="H" />
                                                                    <asp:ListItem Text="Minute(s)" Value="M" />
                                                                    <asp:ListItem Text="Second (S)" Value="S" />
                                                                </asp:DropDownList>--%>
                                                                    </div>
                                                            </div>
                                                                </div>
                                                            </div>
                                                        
                                                    </Content>
                                                </AJAX:AccordionPane>
                                            </Panes>
                                        </AJAX:Accordion>

                                </div>

                                <div class="row form-group" runat="server" id="divgenerateInstruction" visible="false">
                                    <div class="col-lg-12" style="padding-right: 0;">
                                        <asp:Label ID="lblm" runat="server" Text="M"></asp:Label>
                                        <asp:TextBox ID="txtmonday" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtmonday" ValidChars="123456789." />
                                        &nbsp;

                                                    <asp:Label ID="lblT" runat="server" Text="T"></asp:Label>
                                        <asp:TextBox ID="txttuesday" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txttuesday" ValidChars="123456789." />

                                        &nbsp;

                                                    <asp:Label ID="lblW" runat="server" Text="W"></asp:Label>
                                        <asp:TextBox ID="txtW" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtW" ValidChars="123456789." />


                                        &nbsp;

                                                    <asp:Label ID="lblth" runat="server" Text="Th"></asp:Label>
                                        <asp:TextBox ID="txtth" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtth" ValidChars="123456789." />

                                        &nbsp;

                                                    <asp:Label ID="lblF" runat="server" Text="F"></asp:Label>
                                        <asp:TextBox ID="txtF" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtF" ValidChars="123456789." />

                                        &nbsp;

                                                    <asp:Label ID="lblsa" runat="server" Text="Sa"></asp:Label>
                                        <asp:TextBox ID="txtsa" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtsa" ValidChars="123456789." />

                                        &nbsp;

                                                    <asp:Label ID="lblsu" runat="server" Text="Su"></asp:Label>
                                        <asp:TextBox ID="txtsu" CssClass="text-right" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtsu" ValidChars="123456789." />


                                    </div>

                                </div>

                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                        <asp:Button ID="btngenerateInstruction" runat="server" CssClass="btn btn-primary" TabIndex="26" Text="Generate Inst." ToolTip="Generate Instruction"
                                            UseSubmitBehavior="False" Visible="false" OnClick="btngenerateInstruction_Click" />
                                        <br />

                                        <asp:Label ID="Label61" runat="server" Text="Instructions" />
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-xs-9">
                                        <asp:TextBox ID="txtInstructions" runat="server" TabIndex="24" MaxLength="2000" TextMode="MultiLine"
                                            onkeyup="return MaxLenTxt(this, 2000);" Style="width: 100% !important; height: 40px; font-size: 12px; text-align: left; padding: 3px 5px;" />
                                    </div>
                                </div>
                                <div class="row p-t-b-5">
                                    <div class="col-md-5 col-sm-5 col-xs-6 box-col-checkbox">
                                            <asp:CheckBox ID="chkNotToPharmacy" runat="server" TabIndex="25" Text="Own Med." ToolTip="Own Medication" />
                                            <asp:CheckBox ID="chkTaperingDose" runat="server" TabIndex="25" Text="TD" ToolTip="Tapering Dose" />
                                        </div>
                                    <div class="col-md-7 col-sm-7 col-xs-6 no-p-l">
                                        <asp:Button ID="btnVariableDose" runat="server" Text="Variable Dose" CssClass="btn btn-primary"
                                            OnClick="btnVariableDose_OnClick" Visible="true" />
                                        
                                        <asp:Button ID="btnAddItem" runat="server" CssClass="btn btn-primary" TabIndex="26" Text="Add To List (Ctrl+F7)"
                                            OnClick="btnAddItem_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                                    </div>
                                </div>
                           
                            <div class="row p-t-b-5" id="Table1" runat="server" visible="false">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="label66" runat="server" SkinID="label" Font-Size="Smaller" Text="Total&nbsp;Qty" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:TextBox ID="txtTotQty" runat="server" SkinID="textbox" Text="0.00" Width="70px"
                                        MaxLength="10" Style="text-align: right" autocomplete="off" Enabled="false" />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                        FilterType="Custom,Numbers" TargetControlID="txtTotQty" ValidChars="0123456789." />
                                </div>
                            </div>
                            <div class="row p-t-b-5" id="Tr1" runat="server" visible="false">
                                <div class="col-md-3 col-sm-3 col-xs-3">
                                    <asp:Label ID="label67" runat="server" SkinID="label" Font-Size="Smaller" Text="Special Instrucation" />
                                </div>
                                <div class="col-md-9 col-sm-9 col-xs-9">
                                    <asp:TextBox ID="txtSpecialInstrucation" runat="server" ReadOnly="true" TextMode="MultiLine"
                                        Width="320px" Height="30px" />
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                        </div>
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                        <div class="row">
                            <div class="col-md-12 gridview">
                            <asp:Panel ID="Panel3" runat="server" BorderStyle="Solid" BorderWidth="0px" BorderColor="SkyBlue" Width="100%" style="overflow-y: auto; max-height:180px;" class="custom-scroller custom-scroller-light">
                                <asp:GridView ID="gvItem" runat="server" Width="100%" AllowPaging="false" SkinID="gridview"
                                    AutoGenerateColumns="False" OnRowCreated="gvItem_OnRowCreated" OnRowDataBound="gvItem_OnRowDataBound"
                                    OnRowCommand="gvItem_OnRowCommand" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                    HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None"
                                    BorderWidth="1px">
                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                <asp:HiddenField ID="hdnGenericName" runat="server" Value='<%# Eval("GenericName") %>' />
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                                <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                <asp:HiddenField ID="hdnIsInfusion" runat="server" Value='<%#Eval("IsInfusion") %>' />
                                                <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                                <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                                <asp:HiddenField ID="hdnStrengthValue" runat="server" Value='<%#Eval("StrengthValue")%>' />
                                                <asp:HiddenField ID="hdnXMLData" runat="server" Value='<%#Eval("XMLData") %>' />
                                                <asp:HiddenField ID="hdnCustomMedication" runat="server" Value='<%#Eval("CustomMedication") %>' />
                                                <asp:HiddenField ID="hdnNotToPharmcy" runat="server" Value='<%#Eval("NotToPharmacy") %>' />
                                                <asp:HiddenField ID="hdnStartDate" runat="server" Value='<%# Eval("StartDate") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugAllergy" runat="server" Value='<%# Eval("OverrideComments") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugToDrug" runat="server" Value='<%# Eval("OverrideCommentsDrugToDrug") %>' />
                                                <asp:HiddenField ID="hdnCommentsDrugHealth" runat="server" Value='<%# Eval("OverrideCommentsDrugHealth") %>' />
                                                <asp:HiddenField ID="hdnUnAppPrescriptionId" runat="server" Value='<%#Eval("UnAppPrescriptionId") %>' />


                                                <asp:HiddenField ID="hdnDose" runat="server" Value='<%#Eval("Dose") %>' />
                                                <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId") %>' />
                                                <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%# Eval("FrequencyId") %>' />
                                                <asp:HiddenField ID="hdnDuration" runat="server" Value='<%# Eval("Duration") %>' />
                                                <asp:HiddenField ID="hdnDurationType" runat="server" Value='<%# Eval("DurationType") %>' />
                                                <asp:HiddenField ID="hdnFoodRelationshipId" runat="server" Value='<%# Eval("FoodRelationshipId") %>' />
                                                <asp:HiddenField ID="hdnRemarks" runat="server" Value='<%# Eval("InstructionRemarks") %>' />
                                                <asp:HiddenField ID="hdnItemFlagName" runat="server" Value='<%# Eval("ItemFlagName")%>'  />
                                                <asp:HiddenField ID="hdnItemFlagCode" runat="server" Value='<%# Eval("ItemFlagCode") %>' />

                                                <%--<asp:HiddenField ID="hdnCustomId" runat="server" Value='<%#Eval("Id") %>'/>
                                                         <asp:HiddenField ID="hdnVolume" runat="server" Value='<%#Eval("Volume") %>' />
                                                         <asp:HiddenField ID="hdnVolumeUnitId" runat="server" Value='<%#Eval("VolumeUnitId") %>' />
                                                         <asp:HiddenField ID="hdnInfusionTime" runat="server" Value='<%#Eval("InfusionTime") %>' />
                                                         <asp:HiddenField ID="hdnTimeUnit" runat="server" Value='<%#Eval("TimeUnit") %>' />
                                                         <asp:HiddenField ID="hdnTotalVolume" runat="server" Value='<%#Eval("TotalVolume") %>' />
                                                         <asp:HiddenField ID="hdnFlowRateUnit" runat="server" Value='<%#Eval("FlowRateUnit") %>' />--%>
                                                <%-- <asp:HiddenField ID="hdnXmlVariableDose" runat="server" Value='<%#Eval("XmlVariableDose") %>' />--%>
                                                <asp:HiddenField ID="hdnXmlVariableDose" runat="server" Value='<%#Eval("XmlVariableDose") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Drug Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" SkinID="label" Text='<%# Eval("ItemName") %>'
                                                    Width="100%" Font-Size="Smaller" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="60px" ItemStyle-Width="60px"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStartDate" runat="server" SkinID="label" Text='<%# Eval("StartDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndDate" runat="server" SkinID="label" Text='<%# Eval("EndDate") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%# Eval("IndentType") %>' />
                                                <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="60px" Visible="false" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotalQty" runat="server" SkinID="textbox" Width="100%" MaxLength="3"
                                                    Text='<%#Eval("Qty") %>' />
                                                <asp:HiddenField ID="hdnClosingBalanceQty" runat="server" Value='<%#Eval("ClosingBalanceQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>'
                                                    SkinID="label" Font-Size="Smaller" />
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
                                                <asp:ImageButton ID="ibtnEdit" runat="server" CommandName="Select" CausesValidation="false"
                                                    CommandArgument='<%#Eval("ItemId")%>' ImageUrl="~/Images/edit.png" Width="16px"
                                                    Height="16px" ToolTip="Click here to edit this record" />
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
                        <div class="col-md-12">
                            <asp:Label ID="lblLegend" runat="server" Visible="false" CssClass="LegendColor LegendColor1" BackColor="LightGreen" Height="" Text="Qty available in Dest. Store" />

                            <table cellpadding="0" cellspacing="4" class="legends-row">
                                <tr >

                                        <td>
                                            <asp:Label ID="Label73" runat="server" BorderWidth="1px" BackColor="LightGreen" Width="14px"
                                                Height="14px" />&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="Label74" runat="server" Height="14px" Text="Qty available" />&nbsp;&nbsp;
                                        </td>

                                        <td>
                                            <asp:Label ID="Label20" runat="server" BorderWidth="1px" BackColor="LightGray" Width="14px"
                                                Height="14px" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label70" runat="server" Height="14px" Text="LASA" />&nbsp;&nbsp;
                                        </td>
                                        &nbsp;&nbsp;
                                    <td>
                                        <asp:Label ID="Label71" runat="server" BorderWidth="1px" BackColor="Yellow" Width="14px"
                                            Height="14px" />&nbsp;&nbsp;
                                    </td>
                                        <td>
                                            <asp:Label ID="Label72" runat="server" Height="14px" Text="Schedule H1, High value, High alert" />&nbsp;&nbsp;
                                        </td>
                                        <%--Add by Himanshu on Date 17/02/2022--%>
                                        <td>
                                            <asp:Label ID="Label75" runat="server" BorderWidth="1px" BackColor="#009933" Width="14px"
                                                Height="14px" />&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="Label76" runat="server" Height="14px" Text="Narcotic" />&nbsp;&nbsp;
                                        </td>
                                </tr>
                            </table>
                        </div>
                    </div>


            </div>

            </div>
          <div class="row">
                        <div id="dvPharmacistInstruction" runat="server" visible="false" style="width: 520px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 260px; left: 400px; top: 250px">
                            <table width="100%" cellspacing="0" cellpadding="2">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label39" runat="server" SkinID="label" Font-Size="12px" Font-Bold="true"
                                            Text="Instruction For Patient" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txtPharmacistInstruction" runat="server" Font-Size="12px" Font-Bold="true"
                                            ForeColor="Navy" TextMode="MultiLine" Style="min-height: 200px; max-height: 200px; min-width: 510px; max-width: 510px;"
                                            ReadOnly="true" BackColor="#FFFFCC" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnPharmacistInstructionClose" SkinID="Button" runat="server" Font-Size="Smaller"
                                            Text="Close" OnClick="btnPharmacistInstructionClose_OnClick" />
                                    </td>
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
   
                        <div id="dvConfirmStop" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 100px; left: 520px; top: 220px">
                            <table width="100%" cellspacing="2" cellpadding="0">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="Label31" Font-Size="12px" runat="server" Font-Bold="true" Text="Stop Medication Remarks" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtStopRemarks" SkinID="textbox" runat="server" TextMode="MultiLine"
                                            Style="min-height: 45px; max-height: 45px; min-width: 390px; max-width: 390px;"
                                            MaxLength="200" onkeyup="return MaxLenTxt(this, );" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"></td>
                                    <td align="center">
                                        <asp:Button ID="btnStopMedication" SkinID="Button" runat="server" Text="Stop" Font-Size="Smaller"
                                            OnClick="btnStopMedication_OnClick" />
                                        &nbsp;
                                        <asp:Button ID="btnStopClose" SkinID="Button" runat="server" Text="Close" Font-Size="Smaller"
                                            OnClick="btnStopClose_OnClick" />
                                    </td>
                                    <td align="center"></td>
                                </tr>
                            </table>
                        </div>
                    
           
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
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
                        <asp:HiddenField ID="hdnClosingBalance" runat="server" />
                        <asp:HiddenField ID="hdnTotalQty" runat="server" />
                        <asp:HiddenField ID="hdnInfusion" runat="server" />
                        <asp:HiddenField ID="hdnIsInjection" runat="server" />
                        <asp:HiddenField ID="hdnItemFlagName" runat="server" />
                        <asp:HiddenField ID="hdnItemFlagCode" runat="server" />

                        <asp:Button ID="btnSearchFavourite" runat="server" CausesValidation="false" Style="display: none;"
                            OnClick="btnSearchFavourite_OnClick" />
                        <asp:Button ID="btnSearchCurrent" runat="server" CausesValidation="false" Style="display: none;"
                            OnClick="btnSearchCurrent_OnClick" />
                        <asp:Button ID="btnSearchOrderSet" runat="server" CausesValidation="false" Style="display: none;"
                            OnClick="btnSearchOrderSet_OnClick" />
                        <asp:Button ID="btnGetInfoGeneric" runat="server" Text="" CausesValidation="false"
                            SkinID="button" Style="display: none;" OnClick="btnGetInfoGeneric_Click" />
                        <asp:Button ID="btnRefresh" runat="server" Style="display: none;" OnClick="btnRefresh_OnClick" />
                        <asp:Button ID="btnGetFavourite" runat="server" Style="display: none;" OnClick="btnGetFavourite_OnClick" />
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
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
                        <asp:Button ID="btnCalc" runat="server" Style="display: none;" OnClick="btnCalc_OnClick" />
                        <asp:HiddenField ID="hdneclaimWebServiceLoginID" runat="server" />
                        <asp:HiddenField ID="hdnEpresActive" runat="server" />
                        <asp:HiddenField ID="hdneclaimWebServicePassword" runat="server" />
                        <asp:HiddenField ID="hdnCopyLastPresc" runat="server" />
                        <asp:HiddenField ID="hdnMainUnAppPrescriptionId" runat="server" />
                        <asp:HiddenField ID="hdnIsRouteMandatory" runat="server" />
                        <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                    </td>
                </tr>
            </table>
            
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


                    <div id="dvExpensiveDrugs" runat="server" style="width: 400px; z-index: 99999; border: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 400px; left: 470px; top: 75px;">
                        <table style="width: 100%">
                            <tr>
                                <p><span style="color:red">Alert:</span> There are some expensive drugs are: </p>
                            </tr>
                            <tr>
                                <td style="width: 100%; text-align: left;">
                                    <asp:GridView ID="gvExpensiveDrugs" runat="server" Width="100%" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="15" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                        HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                        BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Drug Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDrugName" runat="server" Text='<%#Eval("ItemName") %>' Font-Size="Smaller" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <footer style="  position: absolute !important; bottom: 2px !important;left: 4em !important;">
                            <div class="row">
                                <div class="col-sm-12" style="text-align:center;">
                                    <asp:Label ID="lblContinueMessage" runat="server" Font-Size="12px" Text="Do you wish to continue...?"  ForeColor="#990066" /></td>
                                </div>
                                <div class="col-sm-12"  style="text-align:center;">
                                    <asp:Button ID="btnContinue" runat="server" Text="Proceed" CssClass="btn btn-primary" Font-Size="Smaller" OnClick="btnContinue_Click" />
                                    &nbsp;&nbsp;
                            <asp:Button ID="btnCancels" runat="server" Text="Cancel" Font-Size="Smaller" CssClass="btn btn-primary" OnClick="btnCancels_Click" />
                                </div>
                            </div>
                        </footer>
                    </div>


                    <div id="dvNarcoticDrugsPopup" runat="server" style="width: 400px; z-index: 99999; border: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 400px; left: 470px; top: 75px;">
                        <table style="width: 100%">
                            <tr>
                                <p><span style="color:red">Alert:</span> There are some Narcotic Drugs which can't order more than maximum quantity</p>
                            </tr>
                            <tr>
                                <td style="width: 100%; text-align: left;">
                                    <asp:GridView ID="gvNarcoticDrugs" runat="server" Width="100%" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="15" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                        HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                        BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Drug Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDrugName" runat="server" Text='<%#Eval("ItemName") %>' Font-Size="Smaller" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ordered Quantiy">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaxNarcoticDrugsQuantity" runat="server" Text='<%#Eval("OrderedQty") %>' Font-Size="Smaller" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Quantiy">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMaxNarcoticDrugsQuantity" runat="server" Text='<%#Eval("MaxNarcoticDrugsQty") %>' Font-Size="Smaller" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <footer style="  position: absolute !important; bottom: 2px !important;left: 4em !important;">
                            <div class="row">
                                <div class="col-sm-12"  style="text-align:center; margin-left: 9rem; margin-bottom: 3px;">
                                    <asp:Button ID="btnCloses" runat="server" Text="Close" Font-Size="Smaller" CssClass="btn btn-primary" OnClick="btnClose_Click"/>
                                </div>
                            </div>
                        </footer>
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

            else if (item.get_text() == "Till next visit") {
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

        function AddOrderSet_OnClientClose(oWnd, args) {
            $get('<%=btnAddOrderSetClose.ClientID%>').click();
        }

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
                function ShowICDPanel(ctrlPanel, txt1) {
                    var ICDarr = new Array();
                    var txt = document.getElementById('<%=txtICDCode.ClientID%>');
                    ICDarr = txt.value.split(',');

                    var ICDCodes = '';
                    var tt = document.getElementById(ctrlPanel);
                    tt.style.visibility = 'visible';
                    var tableElement = document.getElementById('rptrICDCodes');
                    if (tableElement != null) {
                        for (var i = 0; i < tableElement.rows.length; i++) {
                            var rowElem = tableElement.rows[i];
                            var col = rowElem.cells[0].childNodes[0];
                            col.checked = false;
                        }

                        for (var i = 0; i < tableElement.rows.length; i++) {
                            var rowElem = tableElement.rows[i];
                            var col = rowElem.cells[0].childNodes[0];
                            var chklabel = rowElem.cells[0].childNodes[1];
                            for (var j = 0; j < ICDarr.length; j++) {
                                if (chklabel.innerText == ICDarr[j]) {
                                    col.checked = true;
                                }
                            }
                        }
                    }
                }

                function CalcChange() {
                    $get('<%=btnCalc.ClientID%>').click();
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
        function SelectAllFavourite(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=lstFavourite.ClientID%>");
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

        function SelectAllCurrent(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvPrevious.ClientID%>");
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

        function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }

        function returnToParent() {
            var oArg = new Object();

            oArg.ControlId = $get('<%=hdnControlId.ClientID%>').value;
            oArg.ControlType = $get('<%=hdnControlType.ClientID%>').value;
            oArg.TemplateFieldId = $get('<%=hdnTemplateFieldId.ClientID%>').value;
            oArg.Sentence = $get('<%=hdnCtrlValue.ClientID%>').value;

            //var oWnd = GetRadWindow();
            //oWnd.close(oArg);

            window.close();
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
                case 118:  // F7
                    $get('<%=btnAddItem.ClientID%>').click();
                    break;
                case 119:  // F8
                    $get('<%=btnclose.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrint.ClientID%>').click();
                    break;

            }
            evt.returnValue = false;
            return false;
        }

        function OnClientItemsRequesting(sender, eventArgs) {
            var ddlgeneric = $find('<%=ddlGeneric.ClientID %>');
            var value = ddlgeneric.get_value();
            var context = eventArgs.get_context();
            context["GenericId"] = value;
        }

        function ddlGeneric_OnClientSelectedIndexChanged(sender, args) {

            var ddlbrand = $find("<%=ddlBrand.ClientID%>");
            if (ddlbrand != null) {
                ddlbrand.clearItems();
                ddlbrand.set_text("");
                ddlbrand.get_inputDomElement().focus();
            }
            var item = args.get_item();
            $get('<%=hdnGenericId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnGenericName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";

            $get('<%=btnGetInfoGeneric.ClientID%>').click();
        }

        function ddlBrand_OnClientSelectedIndexChanged(sender, args) {

            var item = args.get_item();
            $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnItemName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";
            $get('<%=hdnClosingBalance.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ClosingBalance") : "";
            $get('<%=hdnItemFlagName.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ItemFlagName") : "";
            $get('<%=hdnItemFlagCode.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ItemFlagCode") : "";

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
        function OnClientCloseFavourite(oWnd, args) {
            var arg = args.get_argument();
            $get('<%=hdnItemId.ClientID%>').value = arg.ItemIds;
            $get('<%=btnGetFavourite.ClientID%>').click();
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
    </script>
</asp:Content>
