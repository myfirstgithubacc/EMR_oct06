<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BookingList.aspx.cs" Inherits="ATD_BookingList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Patient Booking List</title>
        <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.BookingNo = document.getElementById("hdnBookingNo").value;
            oArg.RegNo = document.getElementById("hdnRegNo").value;

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
    <asp:ScriptManager ID="scrpmgr" runat="server"></asp:ScriptManager>
    
        

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-9 col-sm-9 col-xs-10 text-center">
                        <asp:UpdatePanel ID="up5" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-2 text-right">
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_OnClick" />
                        <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                    </div>
                </div>


                    <div class="row form-group">
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-4 p-t-b-5"><asp:Label ID="Label4" runat="server" Text="Search By"></asp:Label></div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-7 col-sm-7 col-xs-7">
                                            <telerik:RadComboBox ID="ddlSearchBy" runat="server" Width="100%" DropDownWidth="100px">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="[Select]" Value="" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="Booking No" Value="BN" />
                                                    <telerik:RadComboBoxItem Text="Reg No" Value="RN" />
                                                    <telerik:RadComboBoxItem Text="Booking Doctor" Value="BD" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-5 col-sm-5 col-xs-5 no-p-l"><asp:TextBox ID="txtKeyword" runat="server" Width="100%"></asp:TextBox></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-4 p-t-b-5"><asp:Label ID="Label3" runat="server" Text="Booking Type"></asp:Label></div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-7 col-sm-7 col-xs-7">
                                            <telerik:RadComboBox ID="ddlBookingType" runat="server" Width="100%" DropDownWidth="100px">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="[Select]" Value="" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="Surgery" Value="S" />
                                                    <telerik:RadComboBoxItem Text="Delivery" Value="D" />
                                                    <telerik:RadComboBoxItem Text="General" Value="G" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-5 col-sm-5 col-xs-5 no-p-l">
                                            <telerik:RadComboBox ID="ddlReportType" runat="server" Width="100%" DropDownWidth="100px">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Exp Ad Date" Value="A" />
                                                    <telerik:RadComboBoxItem Text="Booking Date" Value="B" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadDatePicker ID="dtpFromdate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12 p-t-b-5">
                            <asp:HiddenField ID="hdnBookingNo" runat="server" />
                            <asp:HiddenField ID="hdnRegNo" runat="server" />
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_OnClick" />
                        </div>
                    </div>

                    <div class="row form-group">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvBooking" Skin="Metro" Width="100%" BorderWidth="0" AllowFilteringByColumn="false"
                                    Height="500px" AllowMultiRowSelection="false" runat="server" AutoGenerateColumns="false"
                                    ShowStatusBar="true" EnableLinqExpressions="false" GridLines="None" AllowPaging="true"
                                    PageSize="8" AllowCustomPaging="false" OnPageIndexChanged="gvBooking_PageIndexChanged"
                                    OnItemCommand="gvBooking_OnItemCommand" OnItemDataBound="gvBooking_ItemDataBound"
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
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="BookingId" DefaultInsertValue="" HeaderText="Id"
                                                AllowFiltering="false" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingId" runat="server" Text='<%#Eval("BookingId") %>' />
                                                    <asp:HiddenField ID="hdnBookingtype"  runat="server" Value='<%#Eval("Bookingtype") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BookingNo" DefaultInsertValue="" HeaderText="Req.No"
                                                AllowFiltering="false" HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingNo" runat="server" Text='<%#Eval("BookingNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BookingDate" DefaultInsertValue="" HeaderText="Req. Date"
                                                AllowFiltering="false" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingDate" runat="server" Text='<%#Eval("BookingDateF") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                                
                                            <telerik:GridTemplateColumn UniqueName="BookingAmt" DefaultInsertValue="" HeaderText="Booking Amt"
                                                AllowFiltering="false" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingName" runat="server" Text='<%#Eval("BookingAmt") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                                
                                                
                                            <telerik:GridTemplateColumn UniqueName="ExpectedAdmissiondate" DefaultInsertValue=""
                                                HeaderText="Expected Adm Date" AllowFiltering="false" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExpectedAdmissiondateF" runat="server" Text='<%#Eval("ExpectedAdmissiondateF") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                                                AutoPostBackOnFilter="false" ShowFilterIcon="false" AllowFiltering="false" FilterControlWidth="99%"
                                                HeaderStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BedCategoryName1" DefaultInsertValue="" HeaderText="Bed Category First Pref."
                                                AllowFiltering="false" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBedCategoryName1" runat="server" Text='<%#Eval("BedCategoryName1") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BedCategoryName2" DefaultInsertValue="" HeaderText="Bed Category Second Pref."
                                                AllowFiltering="false" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBedCategoryName2" runat="server" Text='<%#Eval("BedCategoryName2") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BedCategoryName3" DefaultInsertValue="" HeaderText="Bed Category Third Pref."
                                                AllowFiltering="false" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBedCategoryName3" runat="server" Text='<%#Eval("BedCategoryName3") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="DoctorName" DefaultInsertValue="" HeaderText="Doctor"
                                                AllowFiltering="false" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                                AllowFiltering="false" HeaderStyle-Width="8%" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingstatusName" runat="server" Text='<%#Eval("BookingstatusName") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Remarks" DefaultInsertValue="" HeaderText="Remarks"
                                                AllowFiltering="false" HeaderStyle-Width="12%" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn> 
                                            <telerik:GridTemplateColumn UniqueName="InternalRemarks" DefaultInsertValue="" HeaderText="Internal Remarks"
                                                AllowFiltering="false" HeaderStyle-Width="12%" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInternalRemarks" runat="server" Text='<%#Eval("InternalRemarks") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Delete" AllowFiltering="false" ShowFilterIcon="false"
                                                HeaderText="Delete" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                        CommandName="BookingDelete" CausesValidation="false" CommandArgument='<%#Eval("BookingId")%>'
                                                        ImageUrl="~/Images/DeleteRow.png" Height="14px" Width="14px" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridButtonColumn Text="Select" CommandName="Select" HeaderStyle-Width="8%"
                                                HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                             <telerik:GridButtonColumn Text="Print" CommandName="Print" HeaderStyle-Width="8%"
                                                HeaderText="Print" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>