<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComponentReturnList.aspx.cs"
    Inherits="ComponentReturnList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search Component Return List</title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
   
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div#upd1{
            overflow-x:hidden!important;
        }
    </style>
    <script type="text/javascript">
        function returnToParent() {
            var oArg = new Object();
            //oArg.CrossMatchDetailsId = document.getElementById("hdnCrossMatchId").value;            
            oArg.Requisition = document.getElementById("hdnRequisition").value;
            oArg.ComponentID = document.getElementById("hdnComponentID").value;
            oArg.CrossMatchNo = document.getElementById("hdnCrossMatchNo").value;
            oArg.ComponentIssue = document.getElementById("hdnComponentIssueNo").value;
            oArg.ReturnId = document.getElementById("hdnReturnId").value;
            oArg.TransfusionId = document.getElementById("hdnTransfusionId").value;
            oArg.Issue = document.getElementById("hdnIssue").value;
            oArg.CrossMatchId = document.getElementById("hdnCrossMatchId").value;
            oArg.ComponentIssueId = document.getElementById("HHdnComponentIssueId").value;
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-sm-8 col-xs-8 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                    <div class="col-sm-4 col-xs-4 text-right"><asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" /></div>
                </div>
                    <div class="row">
                       <div class="col-md-6 col-sm-6 col-xs-6">
                           <div class="row p-t-b-5">
                             <div class="col-md-4 col-sm-4 col-xs-4 label2"><asp:Label ID="lblfacility" runat="server"  Text="Facility"></asp:Label></div>
                                <div class="col-md-8 col-sm-8 col-xs-8"><telerik:RadComboBox ID="ddlLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" Width="100%"></telerik:RadComboBox></div>
                            </div>
                            </div>
                         
                        </div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid OnItemCommand="gvBagEntryDetailsList_OnItemCommand" ID="gvBagEntryDetailsList"
                                    runat="server" CssClass="table table-condensed" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false"
                                    AllowMultiRowSelection="false" AutoGenerateColumns="False" EnableLinqExpressions="false"
                                    GridLines="Both" AllowPaging="true" AllowAutomaticDeletes="false" ShowFooter="false"
                                    PageSize="18" OnPageIndexChanged="gvBagEntryDetailsList_OnPageIndexChanged">
                                    <ClientSettings AllowColumnsReorder="false">
                                        <Resizing AllowRowResize="false" AllowColumnResize="false" />
                                    </ClientSettings>
                                    <PagerStyle ShowPagerText="true" />
                                    <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <ItemStyle Wrap="true" />
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                AllowFiltering="false" HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BagNo" HeaderText="BagNo" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="BagNo" SortExpression="BagNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBagNo" runat="server" Text='<%#Eval("BagNo")%>'></asp:Label>
                                                    <%--                                                         
                                                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value='<%#Eval("CrossMatchId")%>' />
                                                                    --%>
                                                    <asp:HiddenField ID="hdnRequisition" runat="server" Value='<%#Eval("BookingNo")%>' />
                                                    <asp:HiddenField ID="hdnComponentID" runat="server" Value='<%#Eval("CrossMatchComponent")%>' />
                                                    <asp:HiddenField ID="hdnCrossMatchNo" runat="server" Value='<%#Eval("CrossMatchNo")%>' />
                                                    <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value='<%#Eval("CrossIssueNo")%>' />
                                                    <asp:HiddenField ID="hdnReturnId" runat="server" Value='<%#Eval("ReturnID")%>' />
                                                    <asp:HiddenField ID="hdnTransfusionId" runat="server" Value='<%#Eval("TransfusionID")%>' />
                                                    <asp:HiddenField ID="hdnIssue" runat="server" Value='<%#Eval("Issued")%>' />
                                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value='<%#Eval("CrossMatchId")%>' />
                                                    <asp:HiddenField ID="hdnComponentIssueId" runat="server" Value='<%#Eval("ComponentIssueId")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchComponent" runat="server" Text='<%#Eval("ComponentName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchQty" HeaderText="Qty (ml)" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CrossMatchQty" SortExpression="CrossMatchQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchQty" runat="server" Text='<%#Eval("Qty")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchedUnits" HeaderText="Units" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CrossMatchedUnits" SortExpression="CrossMatchedUnits">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchedUnits" runat="server" Text='<%#Eval("Units")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="PatientName" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="PatientName" SortExpression="PatientName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Gender" HeaderText="Gender" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="Gender" SortExpression="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%#Eval("Gender")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ActualCrossMatchComponent_Group" HeaderText="Blood Group"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="ActualCrossMatchComponent_Group"
                                                SortExpression="ActualCrossMatchComponent_Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualCrossMatchComponent_Group" runat="server" Text='<%#Eval("ActualCrossMatchComponent_Group")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchDate" HeaderText="CrossMatchDate"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="CrossMatchDate" SortExpression="CrossMatchDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchDate" runat="server" Text='<%#Eval("CrossMatchDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>

                                <script type="text/javascript">
                                     
                                </script>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                            </div>
                        <asp:HiddenField ID="hdnCrossMatchId" runat="server" />
                        <asp:HiddenField ID="hdnRequisition" runat="server" />
                        <asp:HiddenField ID="hdnComponentID" runat="server" />
                        <asp:HiddenField ID="hdnCrossMatchNo" runat="server" />
                        <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value='<%#Eval("ComponentIssueNo")%>' />
                        <asp:HiddenField ID="hdnRegistrationID" runat="server"  />
                        <asp:HiddenField ID="hdnRegistrationNo" runat="server"  />
                        <asp:HiddenField ID="hdnEncounterID" runat="server" />
                        <asp:HiddenField ID="hdnEncounterNo" runat="server" />
                        <asp:HiddenField ID="hdnStatus" runat="server" />
                        <asp:HiddenField ID="hdnReturnId" runat="server" />
                        <asp:HiddenField ID="hdnTransfusionId" runat="server" />
                        <asp:HiddenField ID="hdnIssue" runat="server" />
                        <asp:HiddenField ID="HHdnComponentIssueId" runat="server" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="container-fluid">
            <div class="row">
                <asp:UpdatePanel ID="aa14" runat="server">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                        <asp:TextBox ID="lblPharmacyId" runat="server" Text="0" Width="0" Style="visibility: hidden;"></asp:TextBox>
                        <asp:TextBox ID="txtPatientImageId" runat="server" Style="visibility: hidden;" Text=""></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

    </form>
</body>
</html>