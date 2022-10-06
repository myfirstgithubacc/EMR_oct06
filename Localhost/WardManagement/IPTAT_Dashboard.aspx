<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="IPTAT_Dashboard.aspx.cs" Inherits="WardManagement_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" />
   
        <div class="container-fluid">
            <div class="row header_main">
                <div class="col-md-4 col-sm-4 col-xs-4">
                        <h2>
                            <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Discharge Dashboard" /></h2>
                    </div>

                <div class="col-md-8 col-sm-8 col-xs-8 text-center">
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" Font-Size="Larger" />
                </div>

            </div>

            <div class="row">
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Ward Name" />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" DropDownWidth="250px" Height="300px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlWard_SelectedIndexChanged"
                                                CheckBoxes="true" EnableCheckAllItemsCheckBox="true" />
                        </div>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Station Name" />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                             <telerik:RadComboBox ID="ddlStation" runat="server" Width="100%" DropDownWidth="250px" Height="300px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlStation_SelectedIndexChanged"  />
                        </div>
                    </div>
                </div>
                
                 <div class="col-md-3 col-sm-3 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Filter" />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                             <telerik:RadComboBox ID="ddlfilter" runat="server" Width="100%">
                             <Items>
                                 <telerik:RadComboBoxItem Text="All" Selected="true" Value="A" />
                                <telerik:RadComboBoxItem Text="Pharmacy clearance" Value="PC" />
                                 <telerik:RadComboBoxItem Text="Bill Preparation" Value="BP" />
                                 <telerik:RadComboBoxItem Text="Financial clearance" Value="FC" />
                                 <telerik:RadComboBoxItem Text="Financial clearance (insurance)" Value="FCI" />
                                   <telerik:RadComboBoxItem Text="Discharge Approval" Value="DV" />
                                 <telerik:RadComboBoxItem Text="Bed vacate" Value="BV" />
                                
                             </Items>
                            </telerik:RadComboBox>

                        </div>
                    </div>
                </div>

                <div class="col-md-3 col-sm-3 col-xs-12 p-t-b-5">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Duration" />
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                             <telerik:RadComboBox ID="ddlDuration" runat="server" Width="100%" >
                             <Items>
                                 <telerik:RadComboBoxItem Text="" Selected="true" Value="0" />
                                <telerik:RadComboBoxItem Text="Greater then 30" Value="30" />
                                 <telerik:RadComboBoxItem Text="Less then 30" Value="-30" />
                                 <telerik:RadComboBoxItem Text="Greater then 120" Value="120" />
                                 <telerik:RadComboBoxItem Text="Less then 120" Value="-120" />
                             </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <asp:Button ID="btnGo" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnGo_Click" />
                </div>

            </div>
        
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
            <div style="height:480px;width:99%;">
            <telerik:RadGrid ID="gvEncounterStatusList" Skin="Vista" CssClass="table table-condensed" runat="server" BorderWidth="0" RenderMode="Lightweight"
        AutoGenerateColumns="False" GridLines="Both" AllowPaging="false" Height="480px" OnItemDataBound="gvEncounterStatusList_ItemDataBound"
        OnPageIndexChanged="gvEncounterStatusList_PageIndexChanged" HorizontalAlign="Center">
         <ClientSettings>
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
                </ClientSettings>
        <GroupingSettings CaseSensitive="false" />
        <%--<PagerStyle Mode="NextPrevAndNumeric" />--%>
        <MasterTableView AllowFilteringByColumn="false" TableLayout="Auto" Width="100%">
            <NoRecordsTemplate>
                <div style="font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size: 11px;">
                    Nothing to display
                </div>
            </NoRecordsTemplate>
            <Columns>
                <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Font-Bold="true" AllowFiltering="false" >
                    <ItemTemplate>
                        <asp:Label ID="lbsrno" runat="server" Text='<%# Container.DataSetIndex+1 %>' />

                    </ItemTemplate>
                    <HeaderStyle Width="8%" HorizontalAlign="Left"/>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn UniqueName="PatientName" DataField="PatientName"
                    HeaderText="Patient Name" SortExpression="PatientName" AutoPostBackOnFilter="true" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPatientName" runat="server"
                            Text='<%# Eval("PatientName").ToString().Length>20 ? (Eval("PatientName") as string).Substring(0,20) : Eval("PatientName")  %>' SkinID="label" />
                    </ItemTemplate>
                    <HeaderStyle Width="30%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="UHID" DataField="UHID"
                    HeaderText="UHID" SortExpression="UHID" AutoPostBackOnFilter="true" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:Label ID="lblUHID" runat="server" Text='<%#Eval("UHID")%>' SkinID="label" />
                    </ItemTemplate>
                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                  <telerik:GridTemplateColumn UniqueName="EncounterNo" DataField="EncounterNo"
                    HeaderText='<%$ Resources:PRegistration, EncounterNo%>' SortExpression="EncounterNo" AutoPostBackOnFilter="true" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' SkinID="label" />
                    </ItemTemplate>
                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="DoctorName" DataField="DoctorName"
                    HeaderText="Doctor" SortExpression="DoctorName" AutoPostBackOnFilter="true" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>' SkinID="label" />
                    </ItemTemplate>
                    <HeaderStyle Width="30%"  HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="Companytype" HeaderText="Channel" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:Label ID="lblCompanytype" runat="server" Text='<%#Eval("Companytype") %>' SkinID="label" />
                    </ItemTemplate>
                    <HeaderStyle Width="30%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="BedNo" HeaderText="Bed #" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo") %>' SkinID="label" />
                    </ItemTemplate>
                    <HeaderStyle Width="20%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="PharmacyClearance" HeaderText="Pharmacy Clearance" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnIsPharmacyClearance" runat="server" Value='<%#Eval("IsPharmacyClearance") %>' />
                        <%--  <asp:Image ID="Image1" runat="server" />--%>
                        <asp:Image ID="imgPCGreen" runat="server" ImageUrl="~/Images/green-tick.png" Visible="false" />
                        <asp:Image ID="imgPCOrange" runat="server" ImageUrl="~/Images/circle-orange.png" Visible="false" />
                        <asp:Image ID="imgPCRed" runat="server" ImageUrl="~/Images/circle-red.png" Visible="false" />
                        <asp:Label ID="lblPharmacyClearanceDueMin" runat="server" Text='<%#Eval("PharmacyClearanceDueMin") %>' SkinID="label" Visible="false" />
                        <asp:Label ID="lblPCHourMin" runat="server" Text='<%#Eval("PharmacyClearanceDueHourMin") %>' SkinID="label" Visible="false" />

                    </ItemTemplate>
                    <HeaderStyle Width="22%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="BillPrepared" HeaderText="Final Bill Preparation" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnIsBillPrepared" runat="server" Value='<%#Eval("IsBillPrepared") %>' />
                        <asp:Image ID="imgBPGreen" runat="server" ImageUrl="~/Images/green-tick.png" Visible="false" />
                        <asp:Image ID="imgBPOrange" runat="server" ImageUrl="~/Images/circle-orange.png" Visible="false" />
                        <asp:Image ID="imgBPRed" runat="server" ImageUrl="~/Images/circle-red.png" Visible="false" />
                        <asp:Label ID="lblBillPreparedDueMin" runat="server" Text='<%#Eval("BillPreparedDueMin") %>' SkinID="label" Visible="false" />
                        <asp:Label ID="lblBPHourMin" runat="server" Text='<%#Eval("BillPreparedDueHourMin") %>' SkinID="label" Visible="false" />
                    </ItemTemplate>
                    <HeaderStyle Width="22%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="BillPaidFIC" HeaderText="Bill paid / FIC" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnIsBillPaidFIC" runat="server" Value='<%#Eval("IsBillPaidFIC") %>' />
                        <asp:Image ID="imgBPFGreen" runat="server" ImageUrl="~/Images/green-tick.png" Visible="false" />
                        <asp:Image ID="imgBPFOrange" runat="server" ImageUrl="~/Images/circle-orange.png" Visible="false" />
                        <asp:Image ID="imgBPFRed" runat="server" ImageUrl="~/Images/circle-red.png" Visible="false" />
                        <asp:Label ID="lblBillPaidFICDueMin" runat="server" Text='<%#Eval("BillPaidFICDueMin") %>' SkinID="label" Visible="false" />
                        <asp:Label ID="lblBPFHourMin" runat="server" Text='<%#Eval("BillPaidFICHourMin") %>' SkinID="label" Visible="false" />
                    </ItemTemplate>
                    <HeaderStyle Width="18%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="DischargeAppr" HeaderText="Discharge Appr." AllowFiltering="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnDischargeApproval" runat="server" Value='<%#Eval("DischargeApproval") %>' />
                        <asp:Image ID="imgDAGreen" runat="server" ImageUrl="~/Images/green-tick.png" Visible="false" />
                        <asp:Image ID="imgDAOrange" runat="server" ImageUrl="~/Images/circle-orange.png" Visible="false" />
                        <asp:Image ID="imgDARed" runat="server" ImageUrl="~/Images/circle-red.png" Visible="false" />
                        <asp:Label ID="lblDADueMin" runat="server" Text='<%#Eval("DischargeApprovalDueMin") %>' SkinID="label" Visible="false" />
                        <asp:Label ID="lblDAHourMin" runat="server" Text='<%#Eval("DischargeApprovalDueHourMin") %>' SkinID="label" Visible="false" />
                    </ItemTemplate>
                    <HeaderStyle Width="18%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn UniqueName="BedRelease" HeaderText="Bed vacated" AllowFiltering="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnIsBedRelease" runat="server" Value='<%#Eval("IsBedRelease") %>' />
                        <asp:Image ID="imgBVGreen" runat="server" ImageUrl="~/Images/green-tick.png" Visible="false" />
                        <asp:Image ID="imgBVOrange" runat="server" ImageUrl="~/Images/circle-orange.png" Visible="false" />
                        <asp:Image ID="imgBVRed" runat="server" ImageUrl="~/Images/circle-red.png" Visible="false" />
                        <asp:Label ID="lblBedVacantDueMin" runat="server" Text='<%#Eval("BedVacantDueMin") %>' SkinID="label" Visible="false" />
                        <asp:Label ID="lblBRHourMin" runat="server" Text='<%#Eval("BedVacantDueHourMin") %>' SkinID="label" Visible="false" />
                    </ItemTemplate>
                    <HeaderStyle Width="18%" HorizontalAlign="Left" />
                </telerik:GridTemplateColumn>
                  
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
        </div>
    </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                    <table cellpadding="0" cellspacing="4" visible="false" class="table-condensed">
        <tr visible="false">
            <td>
                <asp:Image ID="imgGreen" runat="server" ImageUrl="~/Images/green-tick.png" />
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" Height="14px" Text="Activity done" />&nbsp;&nbsp;
            </td>
            <td>
                <asp:Image ID="imgOrange" runat="server" ImageUrl="~/Images/circle-orange.png" />
            </td>
            <td>
                <asp:Label ID="Label9" runat="server" Height="14px" Text="Activity work in progress and under TAT (of 30 min)" />&nbsp;&nbsp;
            </td>

            <td>
                <asp:Image ID="imgRed" runat="server" ImageUrl="~/Images/circle-red.png" />
            </td>
            <td>
                <asp:Label ID="Label12" runat="server" Height="14px" Text="Activity not done and crossed TAT (of 30 min)" />&nbsp;&nbsp;
            </td>

        </tr>
    </table>
                </div>
            </div>

    </div>
     <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Timer ID="Timer1" runat="server" OnTick="AotoRefresh_OnClick" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Timer1" />
                                </Triggers>
                            </asp:UpdatePanel>
    
</asp:Content>

