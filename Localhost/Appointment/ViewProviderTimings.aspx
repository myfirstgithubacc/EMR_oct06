<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewProviderTimings.aspx.cs"
    Inherits="Appointment_ViewProviderTimings" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work Hours</title>

    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        td.rcbInputCell.rcbInputCellLeft .rcbInput {
            padding: 2px 8px !important;
        }
        #updProvider{
            width:100%!important;
        }
    </style>
</head>


<body>
    
    <form id="frmProviderTimings" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="ALPTop01">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-12 col-xs-12 features02 text-right">

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass=" btn btn-primary mt-1 mb-1" OnClientClick="window.close(); return false;" />

                    </div>

                </div>
            </div>
        </div>




        <div class="AppointmentWhite">
            <div class="row ALP-Spacing">

                <div class="col-md-4 col-6">
                    <div class="row">
                        <div class="col-md-3 col-4">
                            <asp:Label ID="lblProvider" CssClass="FromDateText" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label>
                        </div>
                        <div class="col-8">
                            <asp:UpdatePanel ID="updProvider" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlFacility" />
                                </Triggers>
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="ddlProvider" runat="server" SkinID="DropDown" AutoPostBack="true" Filter="Contains" Width="100%" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged"></telerik:RadComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                </div>

                <div class="col-md-4 col-6">
                    <div class="row">
                        <div class="col-md-3 col-4">
                            <asp:Label ID="Label1" runat="server" CssClass="ToDateText" Text="Facility"></asp:Label>
                        </div>
                        <div class="col-8">
                            <telerik:RadComboBox ID="ddlFacility" runat="server" SkinID="DropDown" AutoPostBack="true" Filter="Contains" Width="100%" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"></telerik:RadComboBox>
                        </div>
                    </div>


                </div>



            </div>
        </div>



        <div class="GeneralDiv">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-12 table-responsive">

                        <table cellpadding="0" cellspacing="0" runat="server" border="0" class="tableRecurrence">

                            <tr>
                                <td>

                                    <asp:UpdatePanel ID="updProviderTiming" runat="server">
                                        <ContentTemplate>
                                            <br />
                                            <telerik:RadGrid ID="gvProviderTiming" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="False" AllowSorting="False" ShowGroupPanel="false" GridLines="none" Width="100%" Skin="Office2007">

                                                <ClientSettings>
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
                                                </ClientSettings>

                                                <MasterTableView Width="100%">
                                                    <Columns>
                                                        <telerik:GridBoundColumn HeaderText="Facility Name" DataField="FacilityName"></telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderText="From Date" DataField="DateFrom"></telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn HeaderText="To Date" DataField="DateTo"></telerik:GridBoundColumn>

                                                        <telerik:GridTemplateColumn HeaderText="Day">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDay" runat="server" Text='<%#Eval("Day") %>' Width="70px"></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn HeaderText="From Time">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFrom" runat="server" Text='<%#Eval("FromTime") %>' Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn HeaderText="To Time">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTo" runat="server" Text='<%#Eval("ToTime") %>' Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>

                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlProvider" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlFacility" />
                                        </Triggers>

                                    </asp:UpdatePanel>

                                </td>
                            </tr>
                        </table>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
