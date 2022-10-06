<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ConsumableOrderHistory.aspx.cs" Inherits="EMR_Medication_ConsumableOrderHistory" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2 col-sm-3">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="Label1" runat="server" Text="Consumable Order History" /></h2>
                            </div>
                        </div>
                        <div class="col-md-4 checkbox">
                        </div>
                        <div class="col-md-3 col-sm-4 text-center">
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" Font-Size="Larger" />
                        </div>
                        <div class="col-md-2 col-sm-3 text-right pull-right">
                            &nbsp;
                                <asp:Button ID="btnClose" Text="Close" runat="server" CausesValidation="false" Width="70px" CssClass="btn btn-primary pull-right"
                                    OnClientClick="window.close();" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="VitalHistory-Div02">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3 col-sm-3">
                            <div class="PreviousMedicationsBox01">
                                <h2>

                                    <h3>
                                        <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="185px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlSearchOn_OnSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Date Range" Value="D" />
                                                <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </h3>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3">
                            <div class="PreviousMedicationsBox01" id="tblDateRange" runat="server" visible="false">
                                <h2>
                                    <asp:Label ID="Label4" runat="server" Text="From" /></h2>
                                <h3>
                                    <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                </h3>
                                <h2>
                                    <asp:Label ID="Label6" runat="server" Text="To" /></h2>
                                <h3>
                                    <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                </h3>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-3">
                            <div class="PreviousMedicationsBox01 radioo">
                                
                                    <asp:Label ID="Label3" runat="server" Text="Item Name" />
                                
                                    <asp:TextBox ID="txtDrugName" runat="server" Width="150px" MaxLength="50" />&nbsp;&nbsp;
                               
                                
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" Width="60px"
                                        OnClick="btnFilter_Onclick" />
                                <asp:CheckBox ID="Chktoprinter" runat="server" Text="Direct to printer" class="pull-right"  Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="VitalHistory-Div02">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12 col-sm-12">
                            <asp:Panel ID="Panel2" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="gvPrevious" runat="server" Width="100%" Height="100%" AllowPaging="True"
                                    PageSize="60" SkinID="gridviewOrder" AutoGenerateColumns="False" OnRowCommand="gvPrevious_OnRowCommand"
                                    OnPageIndexChanging="gvPrevious_OnPageIndexChanging" HeaderStyle-ForeColor="#15428B"
                                    HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee"
                                    HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                    BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                    <Columns>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="8px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Eval("DetailsId") %>' />
                                                 <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%# Eval("EncounterId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentNo" runat="server" Text='<%# Eval("IndentNo") %>' Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentDate" runat="server" Text='<%# Eval("IndentDate") %>' Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Consumable Item" ItemStyle-Width="300px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Indent Type" ItemStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentType" runat="server" Text='<%# Eval("IndentType") %>' Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="65px" ItemStyle-HorizontalAlign="Right"
                                            Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks"  HeaderStyle-Width="200px" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>'
                                                    Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Indent Store"  HeaderStyle-Width="200px" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentStore" runat="server" Text='<%#Eval("DepartmentName") %>'
                                                    Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Encoded By" HeaderStyle-Width="300px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncodedBy" runat="server" Text='<%# Eval("EncodedBy") %>' Width="100%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" HeaderText="Print">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnPrint" runat="server" ToolTip="Click here to print prescription"
                                                    CommandName="PRINT" CausesValidation="false" CommandArgument='<%#Eval("IndentId")%>'
                                                    Text="Print" Font-Bold="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Cancel">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click here to cancel this drug"
                                                    CommandName="ItemCancel" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                    ImageUrl="~/Images/close_new-old.jpg" Width="15px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <table border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td>
                            <div id="dvConfirmStop" runat="server" visible="false" style="width: 410px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 140px; left: 500px; top: 200px">
                                <table width="100%" cellspacing="2" cellpadding="0">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblCancelStopMedicationRemarks" Font-Size="12px" runat="server" Font-Bold="true"
                                                Text="Cancelation Remarks" />
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
                    </td>
                </tr>
            </table>
              <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

