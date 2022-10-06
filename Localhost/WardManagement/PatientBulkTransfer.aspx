<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master" CodeFile="PatientBulkTransfer.aspx.cs" Inherits="WardManagement_PatientBulkTransfer" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Import Namespace="System.Web.Optimization" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    <style type="text/css">
        input#ctl00_ContentPlaceHolder1_gvPatients_ctl00_ctl02_ctl00_chkHeader{
            margin-left:5px;
            margin-top:-2px!important;
        }
    </style>
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <asp:UpdatePanel ID="upHeader" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid header_main">
                            <div class="col-md-2">
                                <h4 class="m-0">Assigned Staff Details</h4>
                            </div>
                            <div class="col-md-6 text-center">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
                            </div>
                            <div class="col-md-4 text-right">
                                 <asp:Button ID="btnPrint" runat="server" Text="Nurses Allocation Print" CssClass="btn btn-primary"  OnClick="btnPrint_Click" />
                                  <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upForm" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">
                            <div>
                                 <div class="col-md-3 " style="margin-top:20px;">
                                <div class="col-md-12 form-group">
                                    <div class="col-md-12 form-group">
                                        <asp:Label ID="Label1" runat="server" Text="From Staff :"></asp:Label><span style="color: Red">*</span>

                                    </div>
                                    <div class="col-md-12">
                                        <telerik:RadComboBox ID="ddlFromStaff"  Width="100%" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlFromStaff_SelectedIndexChanged" runat="server" />

                                    </div>
                                </div>
                                <div class="col-md-12 form-group">
                                    <div class="col-md-12 form-group">
                                        <asp:Label ID="Label2" runat="server" Text="To Staff :"></asp:Label><span style="color: Red">*</span>

                                    </div>
                                    <div class="col-md-12">
                                        <telerik:RadComboBox ID="ddlToStaff" Filter="Contains" runat="server"  Width="100%" />

                                    </div>
                                </div>
                                <div class="col-md-12 form-group">
                                    <div class="col-md-12 form-group">
                                        <asp:Label ID="lblentered" runat="server" Text="Remarks :" />

                                    </div>
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" Columns="50" Text="" />

                                    </div>
                                </div>

                            </div>
                            <div class="col-md-9">
                    <div class="form-group" style="margin-top: 10px;" >
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="upGV" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvPatients" runat="server" Skin="Office2007" Width="100%"
                                    PagerStyle-ShowPagerText="false" AllowSorting="False" AllowMultiRowSelection="true"
                                    EnableLinqExpressions="false" ShowGroupPanel="false" AutoGenerateColumns="False"
                                    GroupHeaderItemStyle-Font-Bold="true" GridLines="none">
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="490px" />
                                    </ClientSettings>
                                    <MasterTableView Width="100%">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.
                                            </div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkHeader" runat="server" TextAlign="Left" Text="ALL" AutoPostBack="true" OnCheckedChanged="chkHeader_CheckedChanged" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkItem" runat="server" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Registration No." HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Encounter No." HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Patient Name" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                      
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    
                    </div>
                    
                </div>
                            </div>
                           
                    </ContentTemplate>
                </asp:UpdatePanel>
               

                

                </div>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" Skin="Office2007" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close" />
                            </Windows>
                        </telerik:RadWindowManager>
               
            </ContentTemplate>
        </asp:UpdatePanel>



</asp:content>
