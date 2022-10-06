<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTMaster.aspx.cs" Inherits="EMR_Masters_OT" Title="Akhil Systems Pvt. Ltd." %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/bootstrap.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/mainNew.css" media="all" />   
        
        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-3 col-sm-3 col-xs-12"><h2>Theater</h2></div>
                    <div class="col-md-6 col-sm-6 col-xs-12 text-center"><asp:Label ID="lblMessage" runat="server" Font-Bold="true" ForeColor="Green" /></div>
                    <div class="col-md-3 col-sm-3 col-xs-12 text-right">
                        <asp:Button ID="btnNewOT" runat="server" CausesValidation="true" ToolTip="New OT" CssClass="btn btn-primary" Text="New" OnClick="btnNewOT_Click" />
                        <asp:Button ID="btnOTSave" runat="server" CausesValidation="true" ToolTip="Save OT" CssClass="btn btn-primary" ValidationGroup="SaveUpdate" Text="Save" OnClick="btnOTSave_Click" />
                    </div>
                </div>

               
                    <div class="row">
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-4">Facility</div>
                                <div class="col-md-9 col-sm-9 col-xs-8">
                                    <asp:DropDownList ID="ddlFacility" runat="server" AppendDataBoundItems="true" CssClass="drapDrowHeight" Width="100%">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="SaveUpdate" ControlToValidate="ddlFacility" InitialValue="0" Display="None" ErrorMessage="Select Facility Name!"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-4">Name</div>
                                <div class="col-md-9 col-sm-9 col-xs-8"><asp:TextBox ID="txtOTName" runat="server" CssClass="drapDrowHeight" Width="100%" MaxLength="50"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-8 col-sm-8 col-xs-8 box-col-checkbox">
                                    <asp:CheckBox ID="chkIsOTAdvance" runat="server" AutoPostBack="true" /> OT Advance Mandatory
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="SaveUpdate" ControlToValidate="txtOTName" Display="None" ErrorMessage="Enter Theater Name!"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDown" Width="100%">
                                <asp:ListItem Value="1" Text="Active" />
                                <asp:ListItem Value="0" Text="InActive" />
                            </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">Freeze Time</div>
                                <div class="col-md-9 col-sm-9 col-xs-8">
                                    <telerik:RadTimePicker ID="RadFreezeTime" Skin="Metro" runat="server" AutoPostBack="true"
                                     DateInput-ReadOnly="false"  PopupDirection="BottomLeft" TimeView-Columns="4" Width="70%" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                            <asp:GridView ID="gvOT" CellPadding="1" CssClass="table table-condensed" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="TheatreId" Width="100%" ForeColor="#333333" GridLines="None"
                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" OnRowDataBound="gvOT_OnRowDataBound"
                            OnRowDeleting="gvOT_RowDeleting" OnSelectedIndexChanged="gvOT_SelectedIndexChanged">
                            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="TheatreId" HeaderText="ID" />
                                <asp:BoundField DataField="TheatreName" HeaderText="OT Name" />
                                <asp:BoundField DataField="Facility" HeaderText="Facility Name" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:BoundField DataField="Active" HeaderText="Status" />
                                <asp:BoundField DataField="OTFreezeTime" HeaderText="Freeze Time" />
                                <%--  <asp:ButtonField CommandName="Delete" Text="Delete" ControlStyle-ForeColor="Blue" />--%>
                                <asp:CommandField ShowSelectButton="true" ControlStyle-ForeColor="Blue" />
                            </Columns>
                        </asp:GridView>
                        </div>
                    </div>
                </div>
                <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="SaveUpdate" />
                <asp:HiddenField ID="hdnOTID" runat="server" />
                <asp:HiddenField ID="hdnUserID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
