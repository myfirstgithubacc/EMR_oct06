<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PPdoctorsetup.aspx.cs" Inherits="WardManagement_ChangeEncounterDate"   MasterPageFile="~/Include/Master/EMRMaster.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <style>
        div#RAD_SPLITTER_PANE_CONTENT_ctl00_RadPane2 { overflow: inherit !important; }
        .RadComboBox.RadComboBox_Metro {  width: 100% !important;}
        .form-group.row label { display: block; margin-top: 15px;}
        .form-group.row [type="checkbox"] { margin-bottom: 9px;}
        div#ctl00_ContentPlaceHolder1_UpdatePanel1 { width: 99%;}
        .header_main span#ctl00_ContentPlaceHolder1_lblMessage {     width: 80%;
    margin: auto;
    position: inherit;
    color: #ff0000;}
    </style>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                  
                
                
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12 header_main text-right" style="padding: 6px 5px !important;">
                            <h3 style="font-size: 16px; float: left; margin: 0; text-align: left;"><asp:Label ID="lblPPDoctorSetup" runat="server" Text="Patient Portal Doctor Setup"></asp:Label></h3>
                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                <asp:Button ID="btnSave" OnClick="btnSave_Click" CssClass="btn btn-primary" runat="server" Text="Save" style="padding: 0;" />
                            </div>
                        </div>


                     <div class="form-group row">
      <div class="col-sm-3">
        <label><asp:Label ID="lblLocation" runat="server" Text="Facility" /></label>
        <telerik:RadComboBox ID="ddlLocation" runat="server" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" AutoPostBack="true" Skin="Metro" />
      </div>

                         <div class="col-sm-3">
        <label><asp:Label ID="Label2" runat="server" Text="Provider" /></label>
        <telerik:RadComboBox ID="ddlProvider" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged" Skin="Metro" Height="300px" Width="100%" />
      </div>


                          <div class="col-sm-3">
        <label><asp:Label ID="lblTeam" runat="server" Text="Team Member" /></label>
        <telerik:RadComboBox ID="ddlTeam" runat="server" AutoPostBack="false" Skin="Metro" />
      </div>



                         <div class="col-sm-3">
        <label><asp:Label ID="lblDiscountAmount" runat="server" Text="Discount Amount" /></label>
        <asp:TextBox ID="txtDiscountAmount" runat="server"></asp:TextBox>
                              <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtDiscountAmount" ValidChars="0123456789" />
      </div>


                             <div class="col-sm-3">
        <label><asp:Label ID="lblDiscountPercentage" runat="server" Text="Discount Percentage" /></label>
        <asp:TextBox ID="txtDiscountPercentage" runat="server"></asp:TextBox>
                             <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtDiscountPercentage" ValidChars="0123456789" />
        
      </div>


                         <div class="col-sm-3">
        <label><asp:Label ID="lblDiscountAmountOnArrival" runat="server" Text="Discount Amount On Arrival" /></label>
        <asp:TextBox ID="txtDiscountAmountOnArrival" runat="server"></asp:TextBox>
                              <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtDiscountAmountOnArrival" ValidChars="0123456789" />
        
      </div>


                           <div class="col-sm-3">
        <label><asp:Label ID="lblDiscountPercentageOnArrival" runat="server" Text="Discount Percentage On Arrival" /></label>
        <asp:TextBox ID="txtDiscountPercentageOnArrival" runat="server"></asp:TextBox>
                               <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtDiscountPercentageOnArrival" ValidChars="0123456789" />
        
      </div>


                            <div class="col-sm-3">
        <label><asp:Label ID="lblIsOnlinePaymentMandatory" runat="server" Text="IsOnlinePaymentMandatory" /></label>
        <asp:CheckBox ID="chkIsOnlinePaymentMandatory" runat="server" />        
      </div>


                         <div class="col-sm-3">
        <label><asp:Label ID="lblMorningSlots" runat="server" Text="Morning Slots" /></label>
        <asp:TextBox ID="txtMorningSlots" runat="server"></asp:TextBox>
                             <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtMorningSlots" ValidChars="0123456789" />   
      </div>


                         <div class="col-sm-3">
        <label><asp:Label ID="lblMorningExtraChargeAllow" runat="server" Text="Morning Extra Charge Allow" /></label>
        <asp:CheckBox ID="chkMorningExtraChargeAllow" runat="server" />
      </div>


                         <div class="col-sm-3">
        <label><asp:Label ID="lblMorningExtraChargePercent" runat="server" Text="Morning Extra Charge Percent" /></label>
        <asp:TextBox ID="txtMorningExtraChargePercent" runat="server"></asp:TextBox>
                              <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtMorningExtraChargePercent" ValidChars="0123456789" />
      </div>


                         <div class="col-sm-3">
        <label><asp:Label ID="lblMorningExtraChargeAmt" runat="server" Text="Morning Extra Charge Amount" /></label>
        <asp:TextBox ID="txtMorningExtraChargeAmt" runat="server"></asp:TextBox>
                               <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtMorningExtraChargeAmt" ValidChars="0123456789" />
      </div>


                               <div class="col-sm-3">
        <label><asp:Label ID="lblEveningSlots" runat="server" Text="Evening Slots" /></label>
        <asp:TextBox ID="txtEveningSlots" runat="server"></asp:TextBox>
                             <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtEveningSlots" ValidChars="0123456789" />
      </div>


                         <div class="col-sm-3">
        <label><asp:Label ID="lblEveningExtraChargeAllow" runat="server" Text="Evening Extra Charge Allow" /></label>
        <asp:CheckBox ID="chkEveningExtraChargeAllow" runat="server" />
      </div>


                              <div class="col-sm-3">
        <label><asp:Label ID="lblEveningExtraChargePercent" runat="server" Text="Evening Extra Charge Percent" /></label>
        <asp:TextBox ID="txtEveningExtraChargePercent" runat="server"></asp:TextBox>
                              <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtEveningExtraChargePercent" ValidChars="0123456789" />
      </div>

                           <div class="col-sm-3">
        <label><asp:Label ID="lblEveningExtraChargeAmt" runat="server" Text="Evening Extra Charge Amount" /></label>
        <asp:TextBox ID="txtEveningExtraChargeAmt" runat="server"></asp:TextBox>
                             <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtEveningExtraChargeAmt" ValidChars="0123456789" />
      </div>

                           <div class="col-sm-3">
        <label><asp:Label ID="lblMorningSlotCutOffTime" runat="server" Text="Morning Slot Cut Off Time" /></label>
        <telerik:RadTimePicker ID="RadMorningSlotCutOffTime" runat="server" AutoPostBack="True" DateInput-ReadOnly="true" OnSelectedDateChanged="RadMorningSlotCutOffTime_SelectedDateChanged"  PopupDirection="BottomLeft" TimeView-Columns="6" Width="95px" />
      </div>



                         </div>
                  

                      </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Content>
