<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTOrderList.aspx.cs" Inherits="OTScheduler_OTOrderList" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" media="all" />
   
    <div class="container-fluid">
        <div class="row header_main">
        <div class="col-md-3 col-sm-3 colxs-12"><h2><asp:Label ID="lblHeader" runat="server" Text="OT Order List"></asp:Label></h2></div>
        <div class="col-md-6 col-sm-6 col-xs-12 text-center">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" SkinID="label"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-3 col-sm-3 col-xs-12 text-right">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnCancelOrder" runat="server" Text="Cancel Order" CssClass="btn btn-primary" OnClick="btnCancelOrder_OnClick" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

        <div class="row">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3 col-xs-4">
                                        <asp:Label ID="Label4" runat="server" Text="from" />
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-xs-8">
                                        <telerik:RadDatePicker ID="dtFromDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3 col-xs-4">
                                        <asp:Label ID="Label6" runat="server" Text="to" />
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-xs-8">
                                        <telerik:RadDatePicker ID="dtToDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12 p-t-b-5">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_OnClick" />
                    </div>
                    
        </ContentTemplate>
    </asp:UpdatePanel>
</div>


        <div class="row m-t">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvOTOrder" Skin="Metro" Width="100%" BorderWidth="0" AllowFilteringByColumn="false"
                        Height="260px" AllowMultiRowSelection="false" runat="server" AutoGenerateColumns="false"
                        ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" AllowPaging="true"
                        PageSize="8" AllowCustomPaging="false" OnPageIndexChanged="gvOTOrder_PageIndexChanged"
                        OnItemCommand="gvOTOrder_OnItemCommand" OnItemDataBound="gvOTOrder_ItemDataBound"
                        HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="LightGray">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="false"
                            Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="false" ResizeGridOnColumnResize="false"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderText="Cancel"  />
                                <telerik:GridTemplateColumn UniqueName="OrderNo" HeaderText="Order No" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderId" runat="server" Text='<%#Eval("Id") %>' Visible="false" />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        <asp:Label ID="lblOrderNo" runat="server" Text='<%#Eval("OrderNo") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="OrderDate" HeaderText="Order Date" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="RegistrationNo" AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false">
                                     <HeaderTemplate>
                                     <asp:Label ID="lbl1" runat="server" Text='<%#Session["RegistrationLabelName"]%>'/>
                                     </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" AllowFiltering="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="InvoiceNo" HeaderText="Invoice No" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" AllowFiltering="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EnteredBy" HeaderText="Entered By" AllowFiltering="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblEnteredBy" runat="server" Text='<%#Eval("EnteredBy") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EnteredDate" HeaderText="Entered Date" AllowFiltering="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblEnteredDate" runat="server" Text='<%#Eval("EnteredDate") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn Text="Select" CommandName="Select" 
                                    HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
            </div>
        <div class="row m-t">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <telerik:RadGrid ID="gvService" CssClass="table table-condensed" Width="100%" BorderWidth="0" AllowFilteringByColumn="false"
                        Height="180px" AllowMultiRowSelection="false" runat="server" AutoGenerateColumns="false"
                        ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" AllowPaging="false"
                        PageSize="8" AllowCustomPaging="false" OnItemDataBound="gvService_ItemDataBound"
                        HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="LightGray">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                            Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="OrderId" DefaultInsertValue="" HeaderText="OrderId"
                                    AllowFiltering="false" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderId" runat="server" Text='<%#Eval("OrderId") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="SodId" DefaultInsertValue="" HeaderText="SOD Id"
                                    AllowFiltering="false" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSodId" runat="server" Text='<%#Eval("SodId") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Service Name"
                                    AllowFiltering="false" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ProviderType" DefaultInsertValue="" HeaderText="Provider Type"
                                    AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProviderType" runat="server" Text='<%#Eval("ProviderType") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ProviderName" DefaultInsertValue="" HeaderText="Provider Name"
                                    CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                    HeaderStyle-Width="12%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProviderName" runat="server" Text='<%#Eval("ProviderName") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ProviderId" DefaultInsertValue="" HeaderText="ProviderId"
                                    Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProviderId" runat="server" Text='<%#Eval("ProviderId") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ServiceAmount" DefaultInsertValue="" HeaderText="Service Amount"
                                    AllowFiltering="false" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceAmount" runat="server" Text='<%#Eval("ServiceAmount") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ServiceDiscount" DefaultInsertValue="" HeaderText="ServiceDiscount"
                                    AllowFiltering="false" HeaderStyle-Width="10%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceDiscount" runat="server" Text='<%#Eval("ServiceDiscount") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="NetAmount" DefaultInsertValue="" HeaderText="Net Amount"
                                    AllowFiltering="false" HeaderStyle-Width="10%" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNetAmount" runat="server" Text='<%#Eval("NetAmount") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
</div>
  



    <table cellpadding="2" cellspacing="2" width="100%" border="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <div id="dvConfirmCancel" runat="server" style="width: 300px; z-index: 200; border-bottom: 4px solid #CCCCCC;
                            border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC;
                            background-color: #FFF8DC; position: absolute; bottom: 0; height: 170px; left: 450px;
                            top: 150px">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <p style="color: Black; font-weight: bold;">
                                            Are you sure, You want to cancel this order?
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <p style="color: Black;">
                                            Cancellation remarks<span style="color: Red;">*</span>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txtCancelRemarks" runat="server" Width="250px" Height="40px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="height: 40px;">
                                    <td align="center">
                                        <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_Click" CssClass="btn btn-primary" CausesValidation="true" ValidationGroup="cancel" />
                                        <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_Click" CssClass="btn btn-default" CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;<asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtCancelRemarks"
                                            Text="Enter Cancellation Remarks" ErrorMessage="Enter Cancellation Remarks" SetFocusOnError="true"
                                            ValidationGroup="cancel">
                                        </asp:RequiredFieldValidator>
                                        <asp:ValidationSummary ID="vs1" runat="server" ShowMessageBox="true" ShowSummary="false"
                                            ValidationGroup="cancel" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
        </div>
                
</asp:Content>
