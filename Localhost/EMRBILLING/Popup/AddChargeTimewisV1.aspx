<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddChargeTimewisV1.aspx.cs"
    Inherits="EMRBILLING_Popup_AddChargeTimewisV1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Time wise charge</title>


     <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <%-- <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        tr.clsGridheader:nth-of-type(odd) {
            background: #eee!important;
        }

       tr.clsGridheader th {
            background: #3498db!important;
            color: white!important;
            font-weight: bold!important;
        }

      table#gvTimeBaseService td, th {
            padding: 6px 10px!important;
            border: 1px solid #ccc!important;
            text-align: left!important;
            font-size: 14px!important;
            white-space:nowrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="overflow:hidden;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="row">
                <div class="col-md-6 col-xs-6">
                    <h2 style="color: #333;">
                        <asp:Label ID="Label3" runat="server" Text="Charge Type"></asp:Label></h2>
                </div>
                <div class="col-md-6 col-xs-6 text-right">
                    <asp:Button ID="btnsave" OnClick="btnsave_OnClick" runat="server" CssClass="btn btn-primary" ValidationGroup="save" Text="Save" />
                    <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-primary" Text="Close" ToolTip="Close" OnClientClick="window.close();" />
                    <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                </div>
            </div>

        </div>


        <div class="container-fluid">
            <div class="row form-group">
                <div class="col-md-12 col-xs-12 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>


        <div class="container-fluid">

            <div class="row form-group">
                <div class="col-lg-4 col-6 form-group">
                    <div class="row">
                        <div class="col-lg-2 col-md-3 col-xs-2">
                            <asp:Label ID="lblService" runat="server" Text="Service"></asp:Label>
                        </div>
                        <div class="col-lg-10 col-md-9 col-xs-10">
                            <telerik:RadComboBox ID="ddlService" runat="server" Width="100%" EmptyMessage="/Select" Filter="Contains" OnSelectedIndexChanged="ddlService_OnSelectedIndexChanged" AutoPostBack="true"></telerik:RadComboBox>
                        </div>
                    </div>

                </div>
                <div class="col-lg-3 col-6">
                    <div class="row">
                        <div class="col-md-4 col-xs-2">
                            <asp:Label ID="Label4" runat="server" Text="Order Date"></asp:Label>
                        </div>
                        <div class="col-md-8 col-xs-10">
                            <telerik:RadDateTimePicker ID="dtOrderDate" runat="server" CssClass="inlin-bl1" Skin="Metro" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%" />

                        </div>
                    </div>
                </div>


                <div class="col-lg-5 col-6">
                    <div class="row form-group">
                        <div class="col-6">
                            <div class="row">
                                <div class="col-md-3 col-xs-4">
                                    <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
                                </div>
                                <div class="col-md-9 col-xs-8">
                                    <telerik:RadDateTimePicker ID="dtpfromdate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" OnSelectedDateChanged="dtpfromdate_SelectedDateChanged" AutoPostBackControl="Both" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <TimeView CellSpacing="-1"></TimeView>
                                        <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth="" AutoPostBack="True"></DateInput>
                                    </telerik:RadDateTimePicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-6">
                            <div class="row">
                                <div class="col-md-3 col-xs-4">
                                    <asp:Label ID="Label1" runat="server" Text="To"></asp:Label>
                                </div>
                                <div class="col-md-9 col-xs-8">
                                    <telerik:RadDateTimePicker ID="dtpTodate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy HH:mm" OnSelectedDateChanged="dtpTodate_SelectedDateChanged" AutoPostBackControl="Both" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <TimeView CellSpacing="-1"></TimeView>
                                        <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth="" AutoPostBack="True"></DateInput>
                                    </telerik:RadDateTimePicker>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-2 col-6 form-group">
                    <div class="row">
                        <div class="col-md-6 col-xs-4">
                            <asp:Label ID="Label2" runat="server" Text="Sr. Charge"></asp:Label>
                        </div>
                        <div class="col-md-6 col-xs-8">
                            <asp:TextBox ID="txtCharge" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2 col-6 form-group">
                    <div class="row">
                        <div class="col-md-6 col-xs-4 PaddingRightSpacing">
                            <asp:Label ID="lblDrCharge" runat="server" Text="Dr. Charge"></asp:Label>
                        </div>
                        <div class="col-md-6 col-xs-8">
                            <asp:TextBox ID="txtDrCharge" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>



                <div class="col-lg-3 col-6 form-group">
                    <div class="row">
                        <div class="col-md-4 col-xs-2">
                            <asp:Label ID="lblDoctor" runat="server" Text="Doctor"></asp:Label>
                            <asp:Label ID="lblStarDoctor" ForeColor="Red" runat="server" Text="*"></asp:Label>
                        </div>
                        <div class="col-md-8 col-xs-10">
                            <telerik:RadComboBox ID="ddlDoctor" runat="server" Width="100%" Filter="Contains" MarkFirstMatch="true" AutoPostBack="false"></telerik:RadComboBox>
                        </div>
                    </div>
                </div>



                <div class="col-lg-5 col-6 form-group">
                    <div class="row">
                        <div class="col-lg-2 col-md-3 col-xs-2">
                            <asp:Label ID="lblPeriod" Text="Period" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-10 col-md-9 col-xs-4">
                            <asp:Label ID="lblUnitDescription" runat="server" Font-Bold="true"></asp:Label>
                        </div>

                    </div>
                </div>
                <div class="col-md-4 col-6">
                    <div class="row">
                        <div class="col-md-3 col-xs-3  PaddingRightSpacing">
                            <asp:Label ID="lblUnit" Text="Unit Calculated" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-9 col-xs-3">
                            <asp:TextBox ID="txtUnit" runat="server" Width="100%"></asp:TextBox>
                        </div>
                    </div>
                </div>


                <div class="col-md-8 col-12 form-group">
                    <div class="row">
                        <div class="col-md-2 col-xs-2">
                            <asp:Label ID="lblRemark" runat="server" Text="Remarks"></asp:Label>
                        </div>
                        <div class="col-md-10 col-xs-10">
                            <asp:TextBox ID="txtRemark" MaxLength="100" Width="100%" runat="server" Text="" TextMode="SingleLine"></asp:TextBox>
                        </div>
                    </div>
                </div>


                <div>
                    <div class="col-md-12 col-xs-12 form-group">
                        <div class="row">
                            <div class="col-md-12 col-xs-12">
                                <asp:HiddenField ID="hdnDoctorRequired" runat="server" Value="" />
                                <asp:HiddenField ID="hdnChargeType" runat="server" Value="" />
                                <asp:HiddenField ID="hdnDiscount" runat="server" Value="" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 col-xs-12 form-group">
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            Time Bases Services
                           
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <div class="col-md-12 col-xs-12 form-group">
            <div class="row">
                <div class="col-md-12 col-xs-12 table-responsive">
                    <asp:GridView ID="gvTimeBaseService" runat="server" SkinID="gridview" AllowFilteringByColumn="false" AllowPaging="false"
                        AutoGenerateColumns="False"
                        PagerStyle-ShowPagerText="false" Width="100%">

                        <EmptyDataTemplate>
                            <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">No Field Found.</div>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="Sl no" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order No" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnServiceid" runat="server" Value='<%#Eval("Serviceid")%>' />

                                    <asp:Label ID="lblOrderNO" runat="server" Text='<%#Eval("OrderNO")%>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Order Date" ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Service Name(s)" HeaderStyle-Width="50%" ItemStyle-Width="40%">
                                <ItemTemplate>

                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                <ItemTemplate>

                                    <asp:Label ID="lblServiceAmount" runat="server" Text='<%#Eval("ServiceAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}")%>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        </div>

           
    </form>
</body>
</html>
