<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true" CodeFile="CancelDrugReturn.aspx.cs" Inherits="WardManagement_CancelDrugReturn" Title="Cancel Return" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" /> 
    <link href="../Include/css/mainNew.css" rel='stylesheet' type='text/css'>   
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    
    <div class="container-fluid header_main">
        <div class="col-xs-6" id="tdHeader" runat="server"><h2><asp:Label ID="lblHeader" runat="server" ToolTip="Item Return Cancel" Text="Item Return Cancel" /></h2></div>
        <div class="col-xs-6 text-right">
            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-default" OnClientClick="window.close();" />
            <asp:Button ID="btnCancel" runat="server" ToolTip="Cancel Return" CssClass="btn btn-primary" Text="Cancel Return" OnClick="btnCancel_OnClick" />
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
    <div class="container-fluid">
        <div class="row form-groupTop01">
            <asp:GridView ID="grvReturnData" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="false"
                Width="100%"  OnRowCommand="grvReturnData_OnRowCommand">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" CommandName="Select"
                                CommandArgument='<%# Eval("IssueId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IssueNo" HeaderText="Return No." HeaderStyle-Width="70px" />
                    <asp:BoundField DataField="IssueDate" HeaderText="Return Date" HeaderStyle-Width="70px" />
                    <asp:BoundField DataField="RegistrationNo" HeaderText="Registration No." HeaderStyle-Width="90px" />
                    <asp:BoundField DataField="PatientName" HeaderText="Patient Name" />
                    <asp:BoundField DataField="EncounterNo" HeaderText="Encounter No" HeaderStyle-Width="80px" />
                    <asp:BoundField DataField="EncodedBy" HeaderText="Encoded By" HeaderStyle-Width="130px" />
                    <asp:BoundField DataField="EncodedDate" HeaderText="Encoded Date" HeaderStyle-Width="130px" />
                    <asp:BoundField DataField="CancelledBy" HeaderText="Cancelled By" HeaderStyle-Width="120px" />
                    <asp:BoundField DataField="CancelledDate" HeaderText="Cancelled Date" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="ProcessStatus" HeaderText="Status" HeaderStyle-Width="50px" />
                      <asp:TemplateField HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPrint" runat="server" Text="Print" CommandName="Print"
                                CommandArgument='<%# Eval("IssueId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="row form-groupTop01">
            <asp:GridView ID="gvItemReturnDetails" runat="server" Width="100%" Height="100%"
                AllowPaging="false" SkinID="gridviewOrderNew" AutoGenerateColumns="False" OnRowDataBound="gvItemReturnDetails_OnRowDataBound">
                <Columns>
                    <asp:BoundField HeaderText='Issue Date' DataField="IssueDate" HeaderStyle-Width="70px" />
                    <asp:TemplateField HeaderText="Item">
                        <ItemTemplate>
                            <asp:Label ID="lblItemName" runat="server" Width="100%" Text='<%# Eval("ItemName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='Batch No' HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lblBatchNo" runat="server" Width="100%" Text='<%# Eval("BatchNo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='MRP' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblMRP" runat="server" Width="100%" Text='<%# Eval("MRP") %>' />
                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                            <asp:HiddenField ID="hdnBatchId" runat="server" Value='<%# Eval("BatchId") %>' />
                            <asp:HiddenField ID="hdnIssueId" runat="server" Value='<%# Eval("IssueId") %>' />
                            <asp:HiddenField ID="hdnItemExpiryDate" runat="server" Value='<%# Eval("ItemExpiryDate") %>' />
                            <asp:HiddenField ID="hdnCostPrice" runat="server" Value='<%# Eval("CostPrice") %>' />
                            <asp:HiddenField ID="hdnSaleTaxPerc" runat="server" Value='<%# Eval("SaleTaxPerc") %>' />
                            <asp:HiddenField ID="hdnBalanceQty" runat="server" Value='<%# Eval("BalanceQty") %>' />
                            <asp:HiddenField ID="hdnMRP" runat="server" Value='<%# Eval("MRP") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='Disc %' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblDiscPerc" runat="server" Width="100%" Text='<%# Eval("DiscPerc") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText='Issue Qty' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblQty" runat="server" Width="100%" SkinID="label" Text='<%# Eval("Qty") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText='Issued Qty' ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblReturnRequestQty" runat="server" Width="100%" Text='<%# Eval("ReturnRequestQty") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Returned Net Amt" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNetAmt" runat="server" Width="100%" Style="text-align: right"
                                CssClass="gridInput" Text='<%# Eval("NetAmt") %>' Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='Ack. Qty' ItemStyle-HorizontalAlign="Right"
                        HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblAckQty" runat="server" Width="100%" Text='<%# Eval("Qty") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='Ack. By' HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblERReturnAckBy" runat="server" Width="100%" Text='<%# Eval("ERReturnAckBy") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='Ack. Date' HeaderStyle-Width="130px">
                        <ItemTemplate>
                            <asp:Label ID="lblERReturnAckDate" runat="server" Width="100%" Text='<%# Eval("ERReturnAckDate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>