<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OPPrescriptionMainNew.aspx.cs"
    Inherits="EMR_Medication_OPPrescriptionMainNew" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>

        <script language="javascript" type="text/javascript">
            function OpenCIMSWindow() {
                var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

                var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
                WindowObject.document.writeln(ReportContent.value);
                WindowObject.document.close();
                WindowObject.focus();
            }

            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();

                oArg.IndentOPIPSource = document.getElementById("hndPageOPIPSource").value;
                oArg.IndentDetailsIds = document.getElementById("hdnPageDetailsIds").value;
                oArg.IndentIds = document.getElementById("hdnPageIndentIds").value;
                oArg.ItemIds = document.getElementById("hdnPageItemIds").value;
                oArg.GenericIds = document.getElementById("hdnPageGenericIds").value;

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

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row header_main">
                            <div class="col-md-2 col-sm-3 col-12 text-nowrap">
                               <asp:Label ID="Label1" runat="server" Text="Medication&nbsp;Lists" />
                            </div>
                            <div class="col-md-7 col-sm-9 col-12 box-col-checkbox-table">
                                <asp:RadioButtonList ID="rdoPreviousCurrent" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rdoPreviousCurrent_OnSelectedIndexChanged">
                                    <asp:ListItem Text="Previous Medications&nbsp;" Value="P" Selected="True" />
                                    <asp:ListItem Text="Current Medications&nbsp;" Value="C" />
                                    <asp:ListItem Text="Stop Medications&nbsp;" Value="S" />
                                    <asp:ListItem Text="Partial Indent&nbsp;" Value="PI" />
                                    <asp:ListItem Text="Cancel Medications" Value="CL" />
                                </asp:RadioButtonList>
                            </div>
                            
                            <div class="col-md-3 col-sm-12 col-12 text-right">
                            <%-- Add By Himanshu On Date 17/02/2022--%>
                            <asp:Button ID="btnPrint" Text="Print Narcotic Sheet" runat="server" OnClick="btnPrint_Click"
                                 CssClass="btn btn-primary" Visible="true" />
                           
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Onclick"
                                     CssClass="btn btn-primary" Visible="true" />
                               
                                <asp:Button ID="btnReOrder" Text="Re-Order" runat="server" OnClick="btnReOrder_Onclick"
                                    CssClass="btn btn-primary"  Visible="true" />
                              
                                <asp:Button ID="btnClose" Text="Close" runat="server" CausesValidation="false"  CssClass="btn btn-primary pull-right"
                                    OnClientClick="window.close();" />
                            </div>
                        </div>
                        <div class="row text-center">
                                <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" Font-Size="Larger" />
                            </div>
                    
                        <div class="row">
                            <div class="col-md-3 col-sm-5 col-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3  text-nowrap">
                                        <asp:Label ID="Label2" runat="server" Text="Search On" />

                                    </div>
                                    <div class="col-md-9 col-sm-9 ">
                                        <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="100%" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSearchOn_OnSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Date Range" Value="D" />
                                                <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-5 col-sm-3 col-6" id="tblDateRange" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12">
                                    <asp:Label ID="Label4" runat="server" Text="From" />
                                </div>
                                <div class="col-md-8 col-sm-8 col-12">
                                   <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                </div>
                            </div>
                                </div>
                                    <div class="col-md-6 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12">
                                    <asp:Label ID="Label6" runat="server" Text="To" />
                                </div>
                                <div class="col-md-8 col-sm-8 col-12">
                                   <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                </div>
                            </div>
                                </div>
                                    
                                </div>
                            </div>

                            <div class="col-md-3 col-sm-7 col-6">
                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3 col-12 text-nowrap">
                                        <asp:Label ID="Label3" runat="server" Text="Drug Name" />
                                        </div>
                                    <div class="col-md-9 col-sm-9 col-12">
                                        <div class="row">
                                            <div class="col-md-8 col-sm-8 col-9">
                                                <asp:TextBox ID="txtDrugName" runat="server" Width="100%" MaxLength="50" />
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-3">
                                                <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter"
                                            OnClick="btnFilter_Onclick" />
                                            </div>
                                        </div>
                                </div>
                            </div>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-3 text-nowrap">
                                        <asp:Label ID="lblPendingIndent" runat="server" Text="Partial Indent" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-9">
                                        <telerik:RadComboBox ID="ddlPartialPendingIndent" runat="server" EmptyMessage="[ Select Partial Indent ]" Width="200px"
                                            Visible="false" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                                        <asp:Label ID="lblPendingIndentRemarks" runat="server" Text="Remarks" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-12">
                                        <asp:TextBox ID="txtPartialIndentRemarks" runat="server" Width="300px" MaxLength="500" />
                                    </div>
                                    <div class="col-md-2 col-sm-2 col-xs-12">
                                        <asp:Button ID="btnSavePartialPendingIndent" runat="server" Text="Indent Close" Visible="false" OnClick="btnSavePartialPendingIndent_Click" CssClass="btn btn-sm btn-primary" />
                                    </div>
                                </div>
                            </div>
                        </div>
                       <div id="dvAntibiotics" runat="server" style="width: 620px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 200px; left: 400px; top: 200px;" visible="false">
                <table cellspacing="2" cellpadding="2" width="400px">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="Label19" runat="server" Text="You have prescribing following Details:" ForeColor="#990066" />
                        </td>

                    </tr>
                    <tr>
                        <td>  
                          <asp:GridView ID="Gridantibiotics" runat="server"  Width="600px" AllowPaging="false" SkinID="gridview"
                                    AutoGenerateColumns="False">
                            
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sno">
                                            <ItemTemplate>
                                                  <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Issue No">
                                            <ItemTemplate>
                                               <asp:Label ID="OrderDate" runat="server" Text='<%# Bind("IssueNo") %>' ForeColor="#990066" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Date">
                                            <ItemTemplate>
                                                <asp:Label ID="OrderDate" runat="server" Text='<%# Bind("Orderdate") %>' ForeColor="#990066" />
                                                 
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Issues Date">
                                            <ItemTemplate>
                                               <asp:Label ID="OrderDate" runat="server" Text='<%# Bind("Issuesdate") %>' ForeColor="#990066" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acknowledge Date">
                                            <ItemTemplate>
                                               <asp:Label ID="OrderDate" runat="server" Text='<%# Bind("Aknowdate") %>' ForeColor="#990066" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Issue Qty">
                                            <ItemTemplate>
                                               <asp:Label ID="OrderDate" runat="server" Text='<%# Bind("IssuesQty") %>' ForeColor="#990066" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                             
                                        
                                    </Columns>
                              </asp:GridView>
                                        </td>

                      
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            
                            <asp:Button ID="btnAntibioticsCancel" OnClick="btnAntibioticsCancel_Click" runat="server" Text="Close" Font-Size="Smaller"
                                 SkinID="Button" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                  
                </table>
            </div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvPrevious" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                                            <asp:GridView ID="gvPrevious" runat="server" Width="100%" Height="100%" AllowPaging="True"
                                                PageSize="60"  AutoGenerateColumns="False" OnRowCreated="gvPrevious_OnRowCreated"
                                                OnRowDataBound="gvPrevious_OnRowDataBound" OnRowCommand="gvPrevious_OnRowCommand"
                                                OnPageIndexChanging="gvPrevious_OnPageIndexChanging" HeaderStyle-ForeColor="#15428B"
                                                HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee"
                                                HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                                BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table table-condensed table-bordered">
                                                <Columns>
                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="8px"
                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                            <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                            <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                            <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                                            <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                            <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                            <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                            <%-- <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId") %>' />--%>
                                                            <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                                            <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                                            <asp:HiddenField ID="hdnXMLData" runat="server" />
                                                            <asp:HiddenField ID="hdnCommentsDrugAllergy" runat="server" Value='<%# Eval("OverrideComments") %>' />
                                                            <asp:HiddenField ID="hdnCommentsDrugToDrug" runat="server" Value='<%# Eval("OverrideCommentsDrugToDrug") %>' />
                                                            <asp:HiddenField ID="hdnCommentsDrugHealth" runat="server" Value='<%# Eval("OverrideCommentsDrugHealth") %>' />
                                                            <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Eval("DetailsId") %>' />
                                                            <asp:HiddenField ID="hdnCustomMedication" runat="server" Value='<%# Eval("CustomMedication") %>' />
                                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%# Eval("EncounterId") %>' />
                                                            <asp:HiddenField ID="hdnIsStop" runat="server" Value='<%# Eval("IsStop") %>' />
                                                            <asp:HiddenField ID="hdnIsCancel" runat="server" Value='<%# Eval("IsCancel") %>' />
                                                            <asp:HiddenField ID="hdnApprovalStatusColor" runat="server" Value='<%# Eval("ApprovalStatusColor") %>' />
                                                            <asp:HiddenField ID="hdnIsCompoundedDrugOrder" runat="server" Value='<%# Eval("IsCompoundedDrugOrder") %>' />

                                                            <asp:HiddenField ID="hdnItemFlagName" runat="server" Value='<%# Eval("ItemFlagName") %>' />
                                                            <asp:HiddenField ID="hdnItemFlagCode" runat="server" Value='<%# Eval("ItemFlagCode") %>' />
                                                            <asp:HiddenField ID="hdnIndentClose" runat="server" Value='<%# Eval("IndentClose") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="true" OnCheckedChanged="chkRow_OnCheckedChanged2"/>
                                                            <%--AutoPostBack="true" OnCheckedChanged="chkRow_OnCheckedChanged"--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Store Name" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStoreName" runat="server" Text='<%# Eval("DepartmentName") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <%--<asp:Label ID="lblIndentNo" runat="server" Text='<%# Eval("IndentNo") %>' Width="100%" />--%>
                                                       <asp:LinkButton ID="BtnIndentNo" Text='<%# Eval("IndentNo") %>' Width="100%" CommandName="BindDetail"  runat="server"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentDate" runat="server" Text='<%# Eval("IndentDate") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Issued Item Name" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblindItemName" runat="server" Text='<%# Eval("IssuedItemName") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Requested Item Name" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Indent Type" ItemStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentType" runat="server" Text='<%# Eval("IndentType") %>' Width="100%" />
                                                            <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Right"
                                                        Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Prescription Detail">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>'
                                                                Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>' Width="100%" />
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
                                                            <asp:Label ID="LabelDuplicate" runat="server" Text="DI" ToolTip="Duplicate Ingredient" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnDIInteractionCIMS" runat="server" ToolTip="Click here to view cims Duplicate Ingredient"
                                                                CommandName="DIInteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                                Width="19px" Visible="false" />
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
                                                                CommandName="BrandDetailsVIDAL" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
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
                                                    <asp:TemplateField HeaderText="Visit" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSource" runat="server" Text='<%# Eval("Source") %>' Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Issue Quantity" ItemStyle-Width="15px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIssueQty" runat="server" Text='<%#Eval("IssueQty")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Approval" ItemStyle-Width="15px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApproved" runat="server" Text='<%#Eval("Approved")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" HeaderText="Print">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkBtnPrint" runat="server" ToolTip="Click here to print prescription"
                                                                CommandName="PRINT" CausesValidation="false" CommandArgument='<%#Eval("IndentId")%>'
                                                                Text="Print" Font-Bold="true" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" HeaderText="Stop By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStopBy" runat="server" Text='<%#Eval("StopBy")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" HeaderText="Stop Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStopDateTime" runat="server" Text='<%#Eval("StopDateTime")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" HeaderText="Stop Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStopRemarks" runat="server" Text='<%#Eval("StopRemarks")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Cancel">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click here to cancel this drug"
                                                                CommandName="ItemCancel" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                                ImageUrl="~/Images/close_new-old.jpg" Width="15px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Stop">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnStop" runat="server" ToolTip="Click here to stop this drug"
                                                                CommandName="ItemStop" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                                ImageUrl="~/Icons/Critical.gif" Height="17px" Width="17px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Indent Close" ItemStyle-Width="90px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndentClose" runat="server" Width="100%" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                   
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 box-color-col m-t">
                        <p class="mb-2 mb-md-0">
                            <asp:Label ID="Label5" runat="server" CssClass="box-colorbox" BorderWidth="1px" BackColor="Teal" />
                            <asp:Label ID="Label7" runat="server" Height="14px" Text="Custom Medication" />
                        </p>
                        <p class="mb-2 mb-md-0">
                            <asp:Label ID="Label8" runat="server" CssClass="box-colorbox" BorderWidth="1px" BackColor="Aqua"/>
                       <asp:Label ID="Label9" runat="server" Height="14px" Text="Stop Medication" />
                        </p>
                        <p class="mb-2 mb-md-0">
                            <asp:Label ID="Label10" runat="server" CssClass="box-colorbox" BorderWidth="1px" BackColor="LightGray" />
                                <asp:Label ID="Label12" runat="server" Height="14px" Text="LASA" />
                        </p>
                        <p>
                            <asp:Label ID="Label13" runat="server" CssClass="box-colorbox" BorderWidth="1px" BackColor="Yellow"  />
                                <asp:Label ID="Label14" runat="server" Height="14px" Text="Schedule H1, High value, High alert" />
                        </p>
                            <%--Add By Himanshu 17/02/2022--%>
                            <p>
                                <asp:Label ID="Label15" runat="server" CssClass="box-colorbox" BorderWidth="1px" BackColor="#009933" />
                                <asp:Label ID="Label16" runat="server" Height="14px" Text="Narcotic" />
                            </p>
                        <ucl:legend ID="LegendAR" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <table cellpadding="0" cellspacing="4" visible="false">
                    <tr visible="false">
                        <td>
                            <asp:Label ID="Label11" runat="server" SkinID="label" Text="&nbsp;Reason for stop order"
                                Visible="false" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlStopOrder" runat="server" EmptyMessage="[ Select ]" Width="120px"
                                Visible="false" />
                        </td>
                        <td>
                            <asp:Button ID="btnStopOrder" runat="server" Text="Stop Order" SkinID="Button" OnClick="btnStopOrder_Click"
                                Visible="false" />
                        </td>
                    </tr>
                </table>
                        </div>
                    </div>
                
              
                            <div id="dvConfirmStop" runat="server" visible="false" style="width: 410px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 140px; left: 500px; vertical-align:middle">
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
                       
                
                            <div id="dvInteraction" runat="server" visible="false" style="width: 700px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 340px; left: 320px; top: 150px">
                                <table width="100%" cellspacing="0" cellpadding="2">
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="Label25" runat="server" SkinID="label" Font-Size="11px" Font-Bold="true"
                                                Text="Following drug interaction(s) found !" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
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
                                                        <asp:Button ID="btnBrandDetailsView" SkinID="Button" runat="server" Text="View Brand Details"
                                                            Width="150px" OnClick="btnBrandDetailsView_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnMonographView" SkinID="Button" runat="server" Text="View Monograph"
                                                            Width="150px" OnClick="btnMonographView_OnClick" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnInteractionView" SkinID="Button" runat="server" Text="View Drug Interaction(s)"
                                                            Width="150px" OnClick="btnInteractionView_OnClick" />
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
                                                <tr id="Tr1" runat="server" visible="false">
                                                    <td>
                                                        <asp:Label ID="Label38" runat="server" Text="Reason to continue for Drug Health Interaction"
                                                            ForeColor="Gray" />
                                                        <span id="spnCommentsDrugHealth" runat="server" style="color: Red; font-size: large;"
                                                            visible="false">*</span>
                                                    </td>
                                                </tr>
                                                <tr id="Tr2" runat="server" visible="false">
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
                                            <asp:Button ID="btnInteractionContinue" SkinID="Button" runat="server" Text="Continue"
                                                Width="150px" OnClick="btnInteractionContinue_OnClick" />
                                            &nbsp;
                                        <asp:Button ID="btnInteractionCancel" SkinID="Button" runat="server" Text="Cancel"
                                            Width="150px" OnClick="btnInteractionCancel_OnClick" />
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
                       
                        <div class="row">
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" Skin="Metro" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Maximize" />
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:HiddenField ID="hndPageOPIPSource" runat="server" />
                            <asp:HiddenField ID="hdnPageDetailsIds" runat="server" />
                            <asp:HiddenField ID="hdnPageIndentIds" runat="server" />
                            <asp:HiddenField ID="hdnPageItemIds" runat="server" />
                            <asp:HiddenField ID="hdnPageGenericIds" runat="server" />
                            <asp:HiddenField ID="hdnItemId" runat="server" />
                            <asp:HiddenField ID="hdnItemName" runat="server" />
                            <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                            <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                            <asp:HiddenField ID="hdnGenericId" runat="server" />
                            <asp:HiddenField ID="hdnGenericName" runat="server" />
                            <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                            <asp:HiddenField ID="hdnCIMSType" runat="server" />
                            <asp:HiddenField ID="hdnVIDALItemId" runat="server" />
                        </div>
                    
                        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
