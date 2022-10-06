<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SearchPatient.aspx.cs" Inherits="EMRBILLING_Popup_SearchPatient" Title="Patient Details" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="shortcut icon" type="image/ico" href="~/Images/Logo/HealthHub.ico" />
    <link href="/Include/css/bootstrap.css" type="text/css" rel="Stylesheet" />
    <link href="/Include/css/mainNew.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.RegistrationId = document.getElementById("hdnRegistrationId").value;
            oArg.RegistrationNo = document.getElementById("hdnRegistrationNo").value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        $(document).keypress(function (e) {
            if (e.keyCode === 13) {
            }

        });
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main1">
                <div class="row well-sm bg-info">
                    <div class="col-md-2 col-sm-2">
                        <asp:Label ID="lblfacility" runat="server" Text="Facility" SkinID="label" />
                    </div>
                    <div class="col-md-2 col-sm-2">
                        <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="form-control input-sm"
                            Width="170px" />
                    </div>
                    <div class="col-md-8 col-sm-8  pull-right text-right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-xs btn-warning" Text="Filter" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-xs btn-primary" Text="Clear Filter" OnClick="btnClearSearch_Click" />
                        <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-xs btn-primary"
                            OnClientClick="window.close();" />
                    </div>
                </div>
                <div class="row well-sm">
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="lblRegNo" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, Regno%>" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtRegistrationNo" runat="server" CssClass="form-control input-sm" />
                    </div>
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="lblPatientName" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, PatientName%>" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtPatientName" runat="server" CssClass="form-control input-sm" />
                    </div>
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Company" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <%--<asp:TextBox ID="txtCompany" runat="server" CssClass="form-control input-sm" />--%>
                        <telerik:RadComboBox ID="ddlcompany" runat="server" AppendDataBoundItems="true" DropDownWidth="300px"
                            MarkFirstMatch="true" Filter="Contains" Style="width: auto !important;">
                        </telerik:RadComboBox>
                    </div>
                </div>
                <div class="row well-sm">
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="lblmobile" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, mobile%>" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control input-sm" />
                    </div>

                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Address" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control input-sm" />
                    </div>
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="OldRegNo" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtOldRegNo" runat="server" CssClass="form-control input-sm" />
                    </div>
                </div>
                <div class="row well-sm">
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="DOB" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control input-sm" />
                    </div>
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="Guardian&nbsp;Name" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtGName" runat="server" CssClass="form-control input-sm" />
                    </div>
                    <div class="col-md-1 col-sm-2 form-group">
                        <asp:Label ID="Label6" runat="server" SkinID="label" Text="EMP #" />
                    </div>
                    <div class="col-md-3 col-sm-4 form-group">
                        <asp:TextBox ID="txtEmpNo" runat="server" CssClass="form-control input-sm" />
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvRegistration" runat="server" CssClass="table table-bordered" Width="100%" GridLines="None" AutoGenerateColumns="false"
                OnRowCommand="gvRegistration_RowCommand" HeaderStyle-CssClass="bg-primary">
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:PRegistration, Regno%>" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                        <ItemTemplate>
                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                            <asp:HiddenField ID="hdnRegId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:PRegistration, PatientName%>" HeaderStyle-Width="17%" ItemStyle-Width="17%">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DOB" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                        <ItemTemplate>
                            <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Age/Gender" HeaderStyle-Width="12%" ItemStyle-Width="12%">
                        <ItemTemplate>
                            <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("CompanyName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:PRegistration, mobile%>" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Patient Address" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Label ID="lblPatientAddress" runat="server" Text='<%#Eval("PatientAddress")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Guardian Name" HeaderStyle-Width="10%" ItemStyle-Width="12%">
                        <ItemTemplate>
                            <asp:Label ID="lblGuardianName" runat="server" Text='<%#Eval("GuardianName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EMP #" HeaderStyle-Width="5%" ItemStyle-Width="5%" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblTaggedEmpNo" runat="server" Text='<%#Eval("TaggedEmpNo")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="OLD&nbsp;Reg" HeaderStyle-Width="6%" ItemStyle-Width="6%" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblRegistrationNoOld" runat="server" Text='<%#Eval("RegistrationNoOld")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="SD" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkSD" runat="server" Text="SD" CommandName="SD" ToolTip="View Scanned Document" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>










