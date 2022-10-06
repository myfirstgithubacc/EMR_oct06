<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceActivityDetails.aspx.cs"
    Inherits="WardManagement_ServiceActivityDetails" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
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
            height: 20px !important;
        }

        .RadGrid_Office2007 .rgFilterRow {
            background: #c1e5ef !important;
        }

        .RadGrid_Office2007 .rgPager {
            background: #c1e5ef 0 -7000px repeat-x !important;
            color: #00156e !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-xs-6 col-md-6">
                <h2>Service Activity Details</h2>
            </div>
            <div class="col-xs-6 col-md-6 text-right">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnReport" runat="server" Text="Print Investigation List" OnClick="btnReport_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.close();" CssClass="btn btn-primary" />
                        <telerik:RadWindowManager ID="RadWindowManager2" ShowContentDuringLoad="false" EnableViewState="false"
                            VisibleStatusbar="false" ReloadOnShow="true" DestroyOnClose="true" runat="server">
                            <Windows>
                                <telerik:RadWindow runat="server" ID="RadWindow2">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="container-fluid" style="background-color: #fff !important;">
            <div class="row form-group">
                <div class="">
                    <%--<asp:Label ID="lblPatientDetail" runat="server" Text=""></asp:Label>--%>
                    <asplUD:UserDetails ID="asplUD" runat="server" />
                </div>
            </div>
        </div>

        <%--        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                           
                               <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            </td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2">
                                <asp:Label ID="Label5" runat="server" Text="DOB:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblDob" runat="server" Text=""></asp:Label>
                            </td>
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3">
                                <asp:Label ID="Label4" runat="server" Text="Mobile No:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                            </td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-5">
                                <asp:Label ID="Label3" runat="server" Text="IP No:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            </td>                         
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6">
                                <asp:Label ID="Label6" runat="server" Text="Admission Date:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblAdmissionDate" runat="server" Text=""></asp:Label>
                            </td>
                            <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6">
                                <asp:HiddenField ID="hdnRegId" runat="server" />
                                <asp:Label ID="lblDept" runat="server" Text='<%$ Resources:PRegistration, department%>' Font-Bold="true" />
                            </td>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </tr>
            </tbody>
        </table>--%>


        <div class="container-fluid">
            <div class="row form-groupTop01">
                <div class="col-xs-3 col-md-3">
                    <div class="row">
                        <div class="col-xs-5 col-md-4 label2">
                            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                        </div>
                        <div class="col-xs-7 col-md-8">
                            <telerik:RadComboBox ID="ddlDepartment" runat="server" MarkFirstMatch="true" EmptyMessage="[Select Department]" CssClass="drapDrowHeight" Width="100%" />
                        </div>
                    </div>
                </div>
                <div class="col-xs-3 col-md-3">
                    <div class="row">
                        <div class="col-xs-5 col-md-4 label2 PaddingRightSpacing">
                            <asp:Label ID="lblService" runat="server" Text="Service Name"></asp:Label>
                        </div>
                        <div class="col-xs-7 col-md-8">
                            <telerik:RadComboBox ID="ddlService" DropDownWidth="300px" Filter="Contains" CssClass="drapDrowHeight" Width="100%" runat="server"></telerik:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="col-xs-3 col-md-3">
                    <div class="row">
                        <div class="col-xs-5 col-md-4 label2">
                            <asp:Label ID="lblStatus" runat="server" Text="Lab Status"></asp:Label>
                        </div>
                        <div class="col-xs-7 col-md-8">
                            <telerik:RadComboBox ID="ddlLabStatus" runat="server" CssClass="drapDrowHeight" Width="100%" Filter="Contains"></telerik:RadComboBox>
                        </div>
                    </div>
                </div>

                <div class="col-xs-3 col-md-3">
                    <div class="row">
                        <div class="col-xs-5 col-md-4 label2"><asp:Label ID="lblBillingStatus" runat="server" Text="Status"></asp:Label></div>
                        <div class="col-xs-7 col-md-8">
                            <telerik:RadComboBox ID="ddlStatus" runat="server" CssClass="drapDrowHeight" Width="100%" Filter="Contains">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="2" />
                                    <telerik:RadComboBoxItem Text="Active" Value="1" />
                                    <telerik:RadComboBoxItem Text="InActive" Value="0" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                </div>


            </div>

            <div class="row form-groupTop01">
                <div class="col-xs-offset-10 col-xs-2 col-md-offset-10 col-md-2 text-right">
                    <asp:Button ID="BtnRefresh" runat="server" Text="Refresh" CssClass="btn btn-primary" OnClick="BtnRefresh_OnClick" />
                </div>
            </div>
        </div>

        <div class="container-fluid">
            <div class="row margin_Top">
                <telerik:RadGrid ID="gvServiceDetails" runat="server" Width="100%" Skin="Office2007"
                    BorderWidth="0px" AllowPaging="true" AllowCustomPaging="false" Height="600px"
                    AllowMultiRowSelection="True" AutoGenerateColumns="False" ShowStatusBar="True"
                    EnableLinqExpressions="False" GridLines="Both" PageSize="1000" CellSpacing="0"
                    ShowFooter="true" OnItemDataBound="gvServiceDetails_OnItemDataBound" OnEditCommand="gvServiceDetails_EditCommand">
                    <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                        Scrolling-SaveScrollPosition="true">
                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                            AllowColumnResize="false" />
                    </ClientSettings>
                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                        <NoRecordsTemplate>
                            <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                        </NoRecordsTemplate>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="OrderNo" DefaultInsertValue="" HeaderText="Order No"
                                FilterControlWidth="99%" HeaderStyle-Width="7%" ItemStyle-Width="7%" Visible="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblOrderNo" runat="server" Text='<%#Eval("OrderNo") %>' ForeColor="Black"
                                        OnClick="btnPrint_OnClick" CausesValidation="false" />
                                    <asp:HiddenField ID="hdnIssueDetailsId" runat="server" Value='<%#Eval("DetailId") %>' />
                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderId") %>' />
                                    <asp:HiddenField ID="hdnServiceID" runat="server" Value='<%#Eval("ServiceID") %>' />
                                    <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<%#Eval("DiagSampleId") %>' />
                                    <asp:HiddenField ID="hdnLabNo" runat="server" Value='<%#Eval("LabNo") %>' />
                                    <asp:HiddenField ID="hdnColorCode" runat="server" Value='<%#Eval("StatusColor") %>' />
                                    <asp:HiddenField ID="hdnResultHTML" runat="server" Value='<%#Eval("ResultHTML") %>' />
                                     <asp:HiddenField ID="hdnServiceCode" runat="server" Value='<%#Eval("ServiceCode") %>' />
                                     <asp:HiddenField ID="hdnLabSampleNotes" runat="server" Value='<%#Eval("LabSampleNotes") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="OrderDate" DataField="OrderDate" HeaderText="Order Date"
                                HeaderStyle-Width="8%" ItemStyle-Width="8%">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="ServiceName"  HeaderText="Service Name"
                                HeaderStyle-Width="25%" ItemStyle-Width="25%">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("ServiceName") %>'></asp:Label>
                                       <asp:ImageButton ID="ibtnForNotes" runat="server" ImageUrl="~/Images/NotesNew.png"
                                                    ToolTip="Click to show patient notes." Visible="false" CommandName="sel" OnClick="ibtnForNotes_Click" CommandArgument='<%#Eval("OrderId")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn UniqueName="Units" DataField="Units" HeaderText="Units"
                                HeaderStyle-Width="5%" ItemStyle-Width="5%">
                            </telerik:GridBoundColumn>
                            <telerik:GridNumericColumn Aggregate="Sum" DataType="System.Decimal" HeaderText="Gross Amt."
                                DataField="GrossAmt" FooterText=" " HeaderStyle-Width="6%" ItemStyle-Width="6%" UniqueName="GrossAmt"
                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" DataFormatString="{0:F2}">
                            </telerik:GridNumericColumn>
                            <telerik:GridNumericColumn Aggregate="Sum" DataType="System.Decimal" HeaderText="Net Amt."
                                DataField="NetAmount" FooterText=" " HeaderStyle-Width="6%" ItemStyle-Width="6%"
                                UniqueName="NetAmount" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right"
                                DataFormatString="{0:F2}">
                            </telerik:GridNumericColumn>
                            <telerik:GridBoundColumn UniqueName="DoctorName" DataField="DoctorName" HeaderText="Doctor Name"
                                HeaderStyle-Width="14%" ItemStyle-Width="14%">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="EncodedBy" DataField="EncodedBy" HeaderText="Encoded By"
                                HeaderStyle-Width="13%" ItemStyle-Width="13%">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="ServiceStatus" DefaultInsertValue="" HeaderText="Service Status"
                                DataField="ServiceStatus" SortExpression="ServiceStatus" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                FilterControlWidth="99%" HeaderStyle-Width="9%" ItemStyle-Width="9%" Visible="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("ServiceStatus") %>'
                                        OnClick="lnkResult_OnClick" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                DataField="ServiceBillingStatus" SortExpression="ServiceBillingStatus" AutoPostBackOnFilter="true"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true"
                                FilterControlWidth="99%" HeaderStyle-Width="5%" ItemStyle-Width="5%" Visible="true">
                                <ItemTemplate>
                                    <asp:Label ID="lnkServiceBillingStatus" runat="server" Text='<%#Eval("ServiceBillingStatus") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>
        </div>
    </form>
</body>
</html>
