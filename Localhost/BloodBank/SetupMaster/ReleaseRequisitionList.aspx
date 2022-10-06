<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReleaseRequisitionList.aspx.cs"
    Inherits="BloodBank_SetupMaster_ReleaseRequisitionList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Release Requisition List</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />

    <script type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.ReleaseID = document.getElementById("hdnRelease").value;
            oArg.RequisitionId = document.getElementById("hdnRequisitionId").value;
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
                        <div class="col-md-3 col-sm-4 col-12">
                            <h2 style="color: #333;">Release Requisition List</h2>
                        </div>
                        <div class="col-md-5 col-sm-4 col-12 text-center">
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                        <div class="col-md-4 col-sm-4 col-12 text-right">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                            <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>
                    <div class="row">
                        <asp:Panel ID="pnl" runat="server" CssClass="col-md-12 col-sm-12 col-xs-12" DefaultButton="btnSearch">
                            <div class="row">
                                <div class="col-md-4 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-5">
                                            <asp:Label ID="Label4324" runat="server" style="white-space:nowrap;" Text="Requisition No" /></div>
                                        <div class="col-md-8 col-sm-8 col-7">
                                            <asp:TextBox ID="txtRequisitionNo" runat="server" MaxLength="20" Width="100%" /></div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-5">
                                            <asp:Label ID="Label1" runat="server" style="white-space:nowrap;" Text="Patient Name" /></div>
                                        <div class="col-md-8 col-sm-8 col-7">
                                            <asp:TextBox ID="txtPatientName" runat="server" MaxLength="30" Width="100%" /></div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-6 p-t-b-5 box-legendcolor" style="display:flex;">
                                    <asp:Table ID="tblLegendUrgent" runat="server" border="0" />
                                    <asp:Label ID="lblLegendUrgent" runat="server" Text="Urgent" style="margin-right:12px;"/>
                                    <asp:Table ID="tblLegendImmediate" runat="server" border="0" />
                                    <asp:Label ID="lblLegendImmediate" runat="server" Text="Immediate" />
                                </div>
                            </div>
                </asp:Panel>
                        </div>



                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                            <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="99%" BorderWidth="1"
                                BorderColor="SkyBlue" ScrollBars="Auto">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadGrid ID="gvEncounter" runat="server" BorderWidth="0"
                                            PagerStyle-ShowPagerText="false" PageSize="16" AllowFilteringByColumn="false"
                                            AllowMultiRowSelection="false" Width="100%" AutoGenerateColumns="False" ShowStatusBar="true"
                                            EnableLinqExpressions="false" GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                            AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" AllowSorting="true"
                                            OnItemCommand="gvEncounter_OnItemCommand" OnPageIndexChanged="gvEncounter_OnPageIndexChanged">
                                            <FilterMenu EnableImageSprites="False">
                                            </FilterMenu>
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="true" />
                                            <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false"
                                                Width="100%">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.
                                                    </div>
                                                </NoRecordsTemplate>
                                                <EditFormSettings>
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                </EditFormSettings>
                                                <ItemStyle Wrap="true" />
                                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                        AllowFiltering="false" HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                        <ItemStyle Width="50px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RequisitionNo" HeaderText='<%$ Resources:PRegistration, reqno%>'
                                                        ShowFilterIcon="false" DefaultInsertValue="" DataField="RequisitionNo" SortExpression="REGID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RequisitionNo")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnReleaseID" runat="server" Value='<%#Eval("ReleaseID")%>' />
                                                            <asp:HiddenField ID="hdnRequisitionId" runat="server" Value='<%#Eval("RequisitionId")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ReleaseRequisitionNo" HeaderText='Release Requisition Id'
                                                        ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReleaseRequisitionNo" runat="server" Text='<%#Eval("Releaseid")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="PatientName" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                        SortExpression="PatientName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="150px" />
                                                        <ItemStyle Width="150px" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Gender" HeaderText="Gender" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGender" runat="server" Text='<%#Eval("Gender")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="RequestType" HeaderText="Request Type" ShowFilterIcon="false"
                                                        DefaultInsertValue="" DataField="RequestType" SortExpression="RequestType">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRequestType" runat="server" Text='<%#Eval("RequestType")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ReleaseDate" HeaderText="Release Date" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReleaseDate" runat="server" Text='<%#Eval("ReleaseDate")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="ReleaseBy" HeaderText="ReleaseBy" ShowFilterIcon="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReleaseBy" runat="server" Text='<%#Eval("ReleaseBy")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                <asp:HiddenField ID="hdnReleaseID" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRequisitionId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRelease" runat="server" Value='0' />
                <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                    </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
