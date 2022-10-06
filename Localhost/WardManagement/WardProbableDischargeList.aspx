<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="WardProbableDischargeList.aspx.cs" Inherits="WardManagement_WardProbableDischargeList" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<title>Ward Probable Discharge List</title>    --%>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />


    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main">

                <div class="row form-groupTop01" id="trEdod" runat="server">
                    <div class="col-md-2"></div>
                    <div class="col-md-2">
                        <asp:Label ID="Label1" runat="server" Text="Probable Discharge Date"></asp:Label><span style="color: Red">*</span></div>
                    <div class="col-md-2">
                        <telerik:RadDatePicker ID="dtpEod" Width="100%" runat="server"></telerik:RadDatePicker>
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_OnClick" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_OnClick" />
                    </div>
                    <div class="col-md-2 col-sm-2 text-right">
                        <%--<asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" OnClientClick="window.close();" />--%>
                    </div>
                </div>
            </div>

            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-12 text-center">
                        <asp:Label ID="lblMesaage" runat="server"></asp:Label></div>
                </div>
            </div>
            <br />

            <%--<div class="container-fluid">
         <div class="VisitHistoryBorderNew">
                    
                        <div class="row">
                            <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                        </div>
                    </div>    
                </div><br /> --%>

            <div class="container-fluid">
                <div class="row">
                    <%--<asp:GridView ID="gvProbableDischargeDate" runat="server" SkinID="gridviewOrderNew" Width="100%" HeaderStyle-ForeColor="#333" 
                HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" 
                BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table table-bordered">
                </asp:GridView>--%>
                    
                    <div style="align-content: center;">
                         <style>
                            .table-custom th { background: #C1E5EF;}
                            .table-responsive .table-custom th, .table-responsive .table-custom td { border: 1px solid #ddd !important; font-size: 11px; white-space: nowrap;}
                        </style>
                        <div class="table-responsive">
                        <asp:GridView ID="gvProbableDischargeDate" SkinID="gridviewOrderNew" Width="100%" HeaderStyle-ForeColor="#333" runat="server"
                            HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                            BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table table-bordered table-condensed table-custom" ShowHeader="true"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField HeaderText="Encounter No" HeaderStyle-CssClass="text-center emrWidth" DataField="EncounterNo" />
                                <asp:BoundField HeaderText="Patient Name" HeaderStyle-CssClass="text-center emrWidth" DataField="PatientName" />
                                <asp:BoundField HeaderText="Doctor Name" HeaderStyle-CssClass="text-center emrWidth" DataField="DoctorName" />
                                <asp:BoundField HeaderText="Ward Name" HeaderStyle-CssClass="text-center emrWidth" DataField="WardName" />
                                <asp:BoundField HeaderText="Bed No" HeaderStyle-CssClass="text-center emrWidth" DataField="BedNo" />
                                <asp:BoundField HeaderText="Admission Date" HeaderStyle-CssClass="text-center emrWidth" DataField="AdmissionDate" />
                                <asp:BoundField HeaderText="Probable Discharge Date" HeaderStyle-CssClass="text-center emrWidth" DataField="ProbableDischargeDate" />
                                <asp:BoundField HeaderText="Sponsor Name" HeaderStyle-CssClass="text-center emrWidth" DataField="SponsorName" />
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                    <span class="VitalHistory-Div01">
                        <asp:Label ID="lblNoOfRows" runat="server" Text="" Font-Bold="true"></asp:Label></span>

                </div>
            </div>
            <br />
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
