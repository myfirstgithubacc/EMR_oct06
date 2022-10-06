<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrugFavourate.aspx.cs" Inherits="EMR_Dashboard_DrugFavourate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sets / Favourites  </title>
    
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" />
    <link href="../../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/FavoriteSet.css" rel="stylesheet" type="text/css" />
    
    
    
    <script type="text/javascript" language="javascript">
        function returnToParentPage() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.Message = document.getElementById("lblMessage").value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    
    </script>

    <style type="text/css">
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
        .clsGridheader
        {
            border: 1px solid #9DC9F1;
        }
        .clsexta
        {
        }
        .clsGridheader th, .clsGridRowfooter td
        {
            background: transparent url(/Images/extended-button.png) repeat-x scroll 0 0;
            border-bottom: 2px solid #6593CF;
            border-right: 1px solid #6593CF;
            color: #15428B;
            height: 20px;
            cursor: default;
           /* font-family: Arial,Helvetica,Tahoma,Sans-Serif,Monospace; */
            font-size: 18px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            padding: 1px 2px;
        }
        .clsGridheader a
        {
            display: block;
            font-size: 12px;
        }
        .clsGridheader a, .clsGridRow a
        {
            text-decoration: none;
        }
        .clsGridheader a:hover, .clsGridRow a:hover
        {
            text-decoration: underline;
        }
        .clsGridRow > td, .clsGridRow-alt > td
        {
            border-bottom: 1px solid #E5ECF9;
            border-right: 1px solid #E5ECF9;
            color: #000000;
            padding: 2px 8px;
        }
        .clsGridRow:hover, .clsGridRow-alt:hover
        {
            background: transparent url(/Images/gridview-gradient.png) repeat-x scroll 0 0;
        }
        .clsGridRow-alt
        {
            background-color: #F5F5F5;
        }
        .clsGridRow-selected
        {
            background-color: #FAFAD2;
        }
        .clsGridRow-edit td
        {
            background-color: #E5ECF9;
        }
    </style>
    <%--<style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
           
            <asp:HiddenField ID="hdnItemId" runat="server" />
            <asp:HiddenField ID="hdnItemName" runat="server" />
            <asp:HiddenField ID="hdnAllergyType" runat="server" />
          
            <asp:Label ID="lblPlanofcaretext" runat="server" />
            <div class="ALPTop EMRLineBorder">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-10 col-xs-11">
                            <span class="EMRfeaturesLeft">
                                <asp:Label ID="lblMessage_2" Visible="false" runat="server" Text=""></asp:Label>
                                <h2>
                                    <asp:Label ID="lblDept" runat="server" Visible="false" Text="Set Name " /></h2>
                                  
                                <asp:HiddenField ID="hdnPlanOfCareRecordId" runat="server" />
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </span>
                        </div>
                        <div class="col-md-2 col-xs-1">
                            <span class="EMRfeatures">
                                <asp:Button ID="btnSave" ToolTip="Submit" CssClass="btnSave" runat="server" ValidationGroup="Submit" CausesValidation="true" Text="Submit" OnClick="btnSave_OnClick" />
                                <asp:Button ID="btnClose" runat="server" CssClass="btnSave" Text="Close" 
                                OnClientClick="window.close();"  />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="emrPart">
                <div class="container-fluid">
                    <div class="row">
                     <div class="col-md-6">
                            <%-- <asp:Panel ID="pnlGrd" runat="server" ScrollBars="Horizontal" Width="100%">--%>
                            <span class="MPSpacingDiv01">
                                <h2>Favourites </h2>
                                
                                <asp:GridView ID="gvProblemDetails" runat="server" OnRowDataBound="gvProblemDetails_RowDataBound"
                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" 
                                    HeaderStyle-BorderWidth="0" AutoGenerateColumns="False" Width="100%" BackColor="White"
                                    BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"  onrowcommand="gvProblemDetails_RowCommand">
                                    <RowStyle BackColor="White" ForeColor="#EEEEEE" />
                                    <Columns>
                                        <asp:TemplateField >
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkboxgvTreatmentPlan" runat="server" Checked="false" OnCheckedChanged="chkboxgvTreatmentPlan_CheckedChanged" />
                                                <asp:HiddenField ID="hdnItemID" runat="server" Value='<% #Eval("ItemID") %>' />
                                                  <asp:HiddenField ID="hdnDose" runat="server" Value='<% #Eval("Dose") %>' />
                                                    <asp:HiddenField ID="hdnDoseUnitID" runat="server" Value='<% #Eval("DoseUnitID") %>' />
                                                    <asp:HiddenField ID="hdnFrequency" runat="server" Value='<% #Eval("Frequency") %>' />
                                                  <asp:HiddenField ID="hdnFrequencyID" runat="server" Value='<% #Eval("FrequencyID") %>' />
                                                <asp:HiddenField ID="hdnFoodNameID" runat="server" Value='<% #Eval("FoodNameID") %>' />
                                                 <asp:HiddenField ID="hdnFoodName" runat="server" Value='<% #Eval("FoodName") %>' />
                                                
                                                 <asp:HiddenField ID="hdnDays" runat="server" Value='<% #Eval("Days") %>' />
                                                 <asp:HiddenField ID="hdnDaysType" runat="server" Value='<% #Eval("DaysType") %>' />
                                                 <asp:HiddenField ID="hdnDTypeId" runat="server" Value='<% #Eval("DTypeId") %>' />
                                                   <asp:HiddenField ID="hdnRemarks" runat="server" Value='<% #Eval("Remarks") %>' />
                                               
                                               
                                                <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnNotToPharmcy" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnStartDate" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnGenericId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnFormulationId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnRouteId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnStrengthId" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnStrengthValue" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnXMLData" runat="server" Value="0" />
                                                <asp:Label ID="lblPrescriptionDetail" runat="server" Visible="false" />
                                                <asp:TextBox ID="txtTotalQty" runat="server" Visible="false" />
                                                <asp:HiddenField ID="hdnDetailsId" runat="server" Value="0" />
                                         
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2px"></ItemStyle>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Brand Name" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' /></ItemTemplate>
                                            <%--<ItemStyle Width="90px"></ItemStyle>--%>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prescription Details" ItemStyle-Width="180px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDose" runat="server" Text='<%#Eval("prescriptionDetail")%>' />
                                              
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                       
                                           <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="13px" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                        CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("ItemID")%>'
                                                        ImageUrl="~/Images/DeleteRow.png" Width="13px" Height="13px"  />
                                                  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                     
                                       
                                    </Columns>
                                </asp:GridView>
                            </span>
                        </div>
                        
                        <div class="col-md-6">
                            <span class="MPSpacingDiv001">
                                <h2>
                                    Set  <span class="RedText">*</span>&nbsp;
                                    <telerik:RadComboBox ID="ddlPlanTemplates" runat="server" MarkFirstMatch="true" Filter="Contains"
                                    EnableLoadOnDemand="true" EmptyMessage="[Select Set Name]" Width="180px"
                                    Height="150px" DropDownWidth="200px" EnableVirtualScrolling="true" OnSelectedIndexChanged="ddlPlanTemplates_SelectedIndexChanged"
                                    AutoPostBack="true" />
                                    
                                    
                                    <h2>
                                        <span class="MPSpacingDiv001">
                                        <h2>
                                            <asp:GridView ID="gvService" runat="server" AutoGenerateColumns="false" 
                                                HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" 
                                                HeaderStyle-BorderWidth="0" HeaderStyle-ForeColor="#15428B" 
                                                HeaderStyle-Height="25px" HeaderStyle-HorizontalAlign="Right" 
                                                HeaderStyle-Wrap="false" OnRowCommand="gvService_RowCommand" 
                                                OnRowDataBound="gvService_RowDataBound" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="Center" 
                                                        ItemStyle-Width="5px">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkboxgvSet" runat="server" Checked="false" />
                                                            <asp:HiddenField ID="hdnSetIdSet" runat="server" Value='<% #Eval("SetId") %>' />
                                                            <asp:HiddenField ID="hdnItemIDSet" runat="server" 
                                                                Value='<% #Eval("itemid") %>' />
                                                            <asp:HiddenField ID="hdnFrequencyIDSet" runat="server" 
                                                                Value='<% #Eval("FrequencyId") %>' />
                                                            <asp:HiddenField ID="hdnFrequencySet" runat="server" 
                                                                Value='<% #Eval("Frequency") %>' />
                                                            <asp:HiddenField ID="hdnDTypeIdSet" runat="server" 
                                                                Value='<% #Eval("DTypeId") %>' />
                                                            <asp:HiddenField ID="hdnDaysSet" runat="server" 
                                                                Value='<% #Eval("Duration") %>' />
                                                            <asp:HiddenField ID="hdnDaysDurationSet" runat="server" 
                                                                Value='<% #Eval("DType") %>' />
                                                            <asp:HiddenField ID="hdnUnitIDSET" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnFoodRelationSET" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnAttCIMSItemIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnAttCIMSTypeSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnAttVIDALItemIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnIndentIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnNotToPharmcySet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnStartDateSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnGenericIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnFormulationIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnRouteIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnStrengthIdSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnStrengthValueSet" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hdnXMLDataSet" runat="server" Value="0" />
                                                            <asp:Label ID="lblPrescriptionDetailSet" runat="server" Visible="false" />
                                                            <asp:TextBox ID="txtTotalQtySet" runat="server" Visible="false" />
                                                            <asp:HiddenField ID="hdnDetailsIdSet" runat="server" Value="0" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Brand Name" ItemStyle-Width="120px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("itemName")%>' />
                                                        </ItemTemplate>
                                                        <%--<ItemStyle Width="120px"></ItemStyle>--%></asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Prescription Details" ItemStyle-Width="250px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDetailSET" runat="server" 
                                                                Text='<%#Eval("PrescriptionDetail")%>' />
                                                        </ItemTemplate>
                                                        <%--<ItemStyle Width="130px"></ItemStyle>--%></asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:TextBox ID="txtWPlanOfCare" runat="server" Visible="false" />
                                        </h2>
                                        </span>
                                        <h2>
                                        </h2>
                                    </h2>
                                    
                                    
                                    </h2>
                            </span>
                        </div>
                        
                       
                        <div id="dvConfirmCancelOptions" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                        
                        <table width="100%" cellspacing="2">
                            <tr><td colspan="3" align="center"><asp:Label ID="lblConfirm" Font-Size="12px" Font-Bold="true" runat="server" Text="Do you want to delete ?"></asp:Label></td></tr>
                            <tr><td colspan="3">&nbsp;</td></tr>
                            
                            <tr>
                                <td align="center"></td>
                                <td align="center">
                                    <asp:Button ID="ButtonOk" SkinID="Button" runat="server" Text="Yes" OnClick="ButtonOk_OnClick" />&nbsp;
                                    <asp:Button ID="ButtonCancel" SkinID="Button" runat="server" Text="Cancel" OnClick="ButtonCancel_OnClick" />
                                </td>
                                <td align="center"></td>
                            </tr>
                        </table>
                    </div>
                       
                    </div>
                </div>
            </div>
            <div class="emrPart">
                <div class="container-fluid">
                    <div class="row">
                        
                        
                        
                        
                        
                        
                        
                    </div>
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
