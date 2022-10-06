<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewDocument.aspx.cs" Inherits="Pharmacy_SaleIssue_ViewDocument" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Details</title>
    
        <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlDocString = document.getElementById("hdnDocXmlString").value;
            oArg.IssueId = document.getElementById("hdnIssueId").value;
            oArg.InvoiceId = document.getElementById("hdnGlobleInvoiceId").value;
            
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
    <style>
        .tble_mk td:nth-child(1){width:70px;}
        .tble_mk td:nth-child(2){width:90px;}
        .tble_mk td:nth-child(3){width:90px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
               
                
                <div class="container-fluid header_main  margin_bottom">
                    
				 
                     <div class="col-md-1 PaddingRightSpacing">
                      <h2>
                          <span id="tdHeader"  runat="server"><asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Issue History" ToolTip="" /></span>
                          
                      </h2>
                     </div>
                   
                     <div class="col-sm-3 text-center">  <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" /></div>
                 
                     <div class="col-sm-4 text-right pull-right"><asp:Button ID="btnSearch" runat="server" cssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server" cssClass="btn btn-default" Text="Clear Filter"
                                OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" CausesValidation="false" ToolTip="Close"
                                cssClass="btn btn-default" OnClientClick="window.close();" /> </div>
                </div>


                <div class="container-fluid">
                    
 <div class="row form-group">
                    <div class="col-sm-3">
                        <div class="row">
                            <div class="col-sm-6"> <asp:Label ID="Label2" runat="server" SkinID="label" Text="Search On" /></div>
                            <div class="col-sm-6"><telerik:RadComboBox ID="ddlSearchOn" SkinID="DropDown" runat="server" Width="80%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="Return No" Value="1" Selected="true" />
                                    <telerik:RadComboBoxItem Text="Issue No" Value="2" />
                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, UHID%>" Value="3" />
                                    <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, PatientName%>" Value="4" />
                                    <telerik:RadComboBoxItem Text="Encounter No" Value="5" />
                                    <telerik:RadComboBoxItem Text="Current Bed No" Value="6" />
                                   <%-- <telerik:RadComboBoxItem Text="Status" Value="7" />--%>
                                </Items>
                            </telerik:RadComboBox></div>
                        </div>
                    </div>

                    <div class="col-sm-3">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                <asp:TextBox ID="txtDocNo" runat="server" SkinID="textbox" Width="30%" MaxLength="20" />
                                <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="30%" MaxLength="20"
                                    Visible="false" />
                                <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                            </asp:Panel>
                    </div>
     <div class="col-sm-5">

         <asp:Label ID="Label3" runat="server" SkinID="label" Font-Bold="true" Font-Size="18px" Text="Store Name :" /><asp:Label ID="lblStoreName" runat="server" Font-Size="18px" SkinID="label"  Font-Bold="true" Text="" ToolTip="" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lblTotalReturn" runat="server" Font-Bold="true" ForeColor="Red" ></asp:Label>

     </div>
                   
                </div>

 <div class="row form-group">
                   
     <div id="tblDate" runat="server">
      <div class="col-sm-4">
                        <div class="row">
                            <div class="col-sm-6"><asp:Label ID="lblIssueFromDate" runat="server" SkinID="label" Text="Issue&nbsp;From&nbsp;Date" /></div>
                            <div class="col-sm-6"> <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                        </div>
                    </div>

                    <div class="col-sm-4">
                        <div class="row">
                            <div class="col-sm-6"> <asp:Label ID="lblIssueToDate" runat="server" SkinID="label" Text="Issue&nbsp;To&nbsp;Date" /></div>
                            <div class="col-sm-6"> <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                        </div>
                    </div>
         </div>


                    <div class="col-sm-4">
                        <div class="row">
                            <div class="col-sm-6"><asp:Label ID="Label1" runat="server" SkinID="label" Text="Status" /></div>
                            <div class="col-sm-6">
                         <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="100%"
                                AutoPostBack="false" >
                                <Items>
                                    <telerik:RadComboBoxItem Text="Open" Value="O" Selected="true" />
                                    <telerik:RadComboBoxItem Text="Post" Value="P" />
                                    <telerik:RadComboBoxItem Text="Cancel" Value="C" />
                                    
                                </Items>
                            </telerik:RadComboBox></div>
                        </div>
                    </div>
                </div>
                </div>

                
                <div class="container-fluid">
                            <asp:Panel ID="pnlgrid" runat="server" Height="400px" Width="100%" ScrollBars="Auto" CssClass="tble tble_mk">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvItemDetails" runat="server"  AutoGenerateColumns="False" CssClass="table tble_mk"
                                            Width="100%" PageSize="15" onpageindexchanging="gvItemDetails_OnPageIndexChanged" AllowPaging="true" OnSelectedIndexChanged="gvItemDetails_SelectedIndexChanged" OnRowDataBound="gvItemDetails_RowDataBound">
                                            <Columns>
                                                <asp:CommandField ControlStyle-ForeColor="DodgerBlue" SelectText="Select" ShowSelectButton="true" ItemStyle-CssClass="widt"></asp:CommandField>
                                                <asp:TemplateField HeaderText='Return No'>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReturnNo" runat="server" Width="100%" SkinID="label" Text='<%# Eval("ReturnNo") %>' />
                                                        <asp:HiddenField ID="hdnInvoiceId" runat="server" Value='<%# Eval("InvoiceId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Return Date'>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReturnDate" runat="server" Width="100%" SkinID="label" Text='<%# Eval("ReturnDate") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Issue No'  HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueNo" runat="server" Width="100%" SkinID="label" Text='<%# Eval("IssueNo") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Issue Date' HeaderStyle-Width="120px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueDate" runat="server" Width="100%" SkinID="label" Text='<%# Eval("IssueDate") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RegistrationNo" HeaderText='<%$ Resources:PRegistration, UHID%>'
                                                    ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle Width="60px" />
                                                </asp:BoundField>
                                               <%-- <asp:BoundField DataField="EncounterNo"  HeaderText='<%$ Resources:PRegistration, ipno%>'
                                                    ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle Width="40px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CurrentBedNo"   HeaderText="Bed No" ItemStyle-Width="40px"
                                                    HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle Width="40px" />
                                                </asp:BoundField>--%>
                                                
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ipno%>' HeaderStyle-Width="30px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Width="100%" SkinID="label" Text='<%# Eval("EncounterNo") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Bed No" HeaderStyle-Width="30px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrentBedNo" runat="server" Width="100%" SkinID="label" Text='<%# Eval("CurrentBedNo") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:BoundField DataField="PName" HeaderText="<%$ Resources:PRegistration,PatientName%>"
                                                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="120px"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Net Amount" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtnetamount" runat="server" SkinID="textbox" Style="width: 100%;
                                                            text-align: right;" Enabled="false" Text='<%#Eval("NetAmount")%>' ForeColor="#421887"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EncodedBy" HeaderText="Encoded By" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="140px" />
                                                <asp:BoundField DataField="EncodedDate" Visible="false" HeaderText="Encoded Date"  HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="120px" />
                                                <asp:BoundField DataField="LoginStoreName" HeaderText="Store Name" HeaderStyle-HorizontalAlign="Left">
                                                </asp:BoundField>
                                                <asp:BoundField DataField="IssueId" HeaderText="IssueId" />
                                                <%--<asp:BoundField DataField="SaleSetupId" HeaderText="SaleSetupId" />--%>
                                                <asp:BoundField DataField="id" HeaderText="id" />
                                                <%--<asp:BoundField DataField="LoginStoreId" HeaderText="LoginStoreId" />--%>
                                                 
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProcessStatus" runat="server" SkinID="label" Text='<%#Eval("ProcessStatus")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Indent No" HeaderStyle-Width="60px" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%#Eval("IndentNo")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Ward Name" HeaderStyle-Width="60px" >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardName" runat="server" SkinID="label" Text='<%#Eval("WardName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                 <asp:TemplateField HeaderText="Order Enrty" HeaderStyle-Width="60px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderEnrty" runat="server" SkinID="label" Text='<%#Eval("OrderEnrty")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                       </div>
               
                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                <asp:HiddenField ID="hdnDocXmlString" runat="server" />
                <asp:HiddenField ID="hdnIssueId" runat="server" Value="" />
                <asp:HiddenField ID="hdnGlobleInvoiceId" runat="server" Value="" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
