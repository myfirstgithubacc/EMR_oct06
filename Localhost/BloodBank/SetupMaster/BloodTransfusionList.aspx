<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BloodTransfusionList.aspx.cs"
    Inherits="BloodTransfusionList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Blood Transfusion List</title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
            border: solid #868686 1px !important;
            border-top: none !important;
            border-left: none !important;
            outline: none !important;
            color: #333;
            background: 0 -2300px repeat-x #c1e5ef !important;
        }

        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {
            background-color: #fff !important;
        }

        #ctl00_ContentPlaceHolder1_Panel1 {
            background-color: #c1e5ef !important;
        }

        .RadGrid .rgFilterBox {
            height: 20px;
        }

        .RadGrid_Office2007 .rgFilterRow {
            background: #c1e5ef !important;
        }

        .RadGrid_Office2007 .rgPager {
            background: #c1e5ef 0 -7000px repeat-x !important;
            color: #00156e !important;
        }
    </style>
     
    <script type="text/javascript">
        function returnToParent() {
            var oArg = new Object();
            oArg.CrossMatchId = document.getElementById("hdnCrossMatchId").value;            
            oArg.Requisition = document.getElementById("hdnRequisition").value;
            oArg.ComponentID = document.getElementById("hdnComponentID").value;
            oArg.CrossMatchNo = document.getElementById("hdnCrossMatchNo").value;
            oArg.ComponentIssue = document.getElementById("hdnComponentIssueNo").value;
            oArg.ComponentIssueId = document.getElementById("hdnComponentIssueId").value;
            oArg.TransfusionId = document.getElementById("hdnTransfusionId").value;
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

    <script type="text/css">
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
        
        
        
        
        <div>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <div class="container-fluid header_main form-group">
                        <div class="col-xs-9 text-center">
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                        </div>
                        <div class="col-xs-3 text-right">
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>
                    <div class="container-fluid">
                        <div class="row ">
                            <div class="col-md-4 col-sm-6 ">
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-3 label2">
                                        <asp:Label ID="lblfacility" runat="server" Text="Facility"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-9">
                                        <telerik:RadComboBox ID="ddlLocation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged" Width="100%"></telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                        <div class="col-md-6 col-sm-6 col-xs-6">
                            </div>
                         
                        </div>
                     </div>
                <div class="container-fluid">
                    

                        <div class="row">
                            <div class="col-12 table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>

                                        <telerik:RadGrid OnItemCommand="gvBagEntryDetailsList_OnItemCommand" ID="gvBagEntryDetailsList"
                                            runat="server" Skin="Office2007" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false"
                                            AllowMultiRowSelection="false" AutoGenerateColumns="False" EnableLinqExpressions="false"
                                            GridLines="Both" AllowPaging="true" AllowAutomaticDeletes="false" ShowFooter="false"
                                            OnPageIndexChanged="gvBagEntryDetailsList_OnPageIndexChanged"
                                            PageSize="18">
                                            <ClientSettings AllowColumnsReorder="false">
                                                <Resizing AllowRowResize="false" AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="true" />
                                            <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
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
                                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value='<%#Eval("CrossMatchId")%>' />
                                                    <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value='<%#Eval("CrossIssueNo")%>' />
                                                    <asp:HiddenField ID="hdnComponentIssueId" runat="server" Value='<%#Eval("ComponentIssueId")%>' />
                                                    <asp:HiddenField ID="hdnTransfusionId" runat="server" Value='<%#Eval("TransfusionId")%>' />
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
                                                    <asp:Label ID="lblActualCrossMatchComponent_Group" runat="server" Text='<%#Eval("BloodGroupDescription")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchDate" HeaderText="CrossMatchDate"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="CrossMatchDate" SortExpression="CrossMatchDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchDate" runat="server" Text='<%#Eval("CrossMatchDate","{0:dd/MM/yyyy}")%>'></asp:Label>
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
                        </div>
                    </div>

                
                <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                <asp:HiddenField ID="hdnComponentID" runat="server" Value="0" />
                <asp:HiddenField ID="hdnCrossMatchNo" runat="server" Value="0" />
                <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value='<%#Eval("ComponentIssueNo")%>' />
                <asp:HiddenField ID="hdnRegistrationID" runat="server"  />
                <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                <asp:HiddenField ID="hdnEncounterID" runat="server" />
                <asp:HiddenField ID="hdnEncounterNo" runat="server"  />
                <asp:HiddenField ID="hdnAcknowledge" runat="server" Value="1" />
                <asp:HiddenField ID="hdnStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdnComponentIssueId" runat="server" />
                <asp:HiddenField ID="hdnTransfusionId" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>




        <table cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" valign="top">
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
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
