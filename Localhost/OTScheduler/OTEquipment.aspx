<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTEquipment.aspx.cs" Inherits="EMR_OTEquipment" Title="Akhil Systems Pvt. Ltd." %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />

        <asp:UpdatePanel ID="updatepanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid header_main form-group">
                    <div class="col-md-3"><h2>OT Equipment Master</h2></div>
                    <div class="col-md-6 text-center"><asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" /></div>
                    <div class="col-md-3 text-right">
                        <asp:Button ID="btnNewOT" runat="server" CausesValidation="true" ToolTip="New OTEquipment" CssClass="btn btn-default" Text="New" OnClick="btnNewOT_Click" />
                        <asp:Button ID="btnOTSave" runat="server" CausesValidation="true" ToolTip="Save OT" CssClass="btn btn-primary" ValidationGroup="SaveUpdate" Text="Save" OnClick="btnOTEquipmentsSave_Click" />
                    </div>
                </div>

                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-2 label2">Facility</div>
                                <div class="col-md-10">
                                    <asp:DropDownList ID="ddlFacility" runat="server" AppendDataBoundItems="true" CssClass="drapDrowHeight" Width="100%">
                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="SaveUpdate" ControlToValidate="ddlFacility" InitialValue="0" Display="None" ErrorMessage="Select Facility Name!"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="row">
                                <div class="col-md-4 label2">Equipments Name</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtOTEquipmentsName" runat="server" CssClass="drapDrowHeight" Width="100%" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="SaveUpdate" ControlToValidate="txtOTEquipmentsName" Display="None" ErrorMessage="Enter Equipments Name!"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="row">
                                <div class="col-md-5 label2"><asp:Label ID="lblQty" runat="server" Font-Bold="true"  Text="Total Qty"></asp:Label></div>
                                <div class="col-md-7">
                                    <asp:TextBox ID="txtTotalQty" runat="server" CssClass="drapDrowHeight" Width="100%" MaxLength="3"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtTotalQty" ValidChars="1234567890"> </cc1:FilteredTextBoxExtender>  
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="SaveUpdate" ControlToValidate="txtTotalQty" Display="None" ErrorMessage="Enter Total Qty.!"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="row">
                                <div class="col-md-5 label2"><strong>Status</strong></div>
                                <div class="col-md-7">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="drapDrowHeight" Width="100%">
                                        <asp:ListItem Value="1" Text="Active" />
                                        <asp:ListItem Value="0" Text="InActive" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-group">
                        <asp:GridView SkinID="gridviewOrderNew" ID="gvOTEquipments" CellPadding="1" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="EquipmentId" Width="100%" ForeColor="#333333" GridLines="None"
                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" OnRowDataBound="gvOTEquipments_OnRowDataBound"
                            OnRowCommand="gvOTEquipments_RowCommand" AllowPaging="True" OnPageIndexChanging="gvOTEquipments_PageIndexChanging" PageSize="20">
                            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                            <Columns>
                                <asp:BoundField DataField="EquipmentName" HeaderText="OT Equipment Name" />
                                    <asp:BoundField DataField="TotalQty" HeaderText="Total Qty" />
                                <asp:BoundField DataField="Facility" HeaderText="Facility Name" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="linkSelect" runat="server" CausesValidation="False" CommandName="btnEdit" Text="Select"  CommandArgument='<%#Bind("EquipmentId")%>' ></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle ForeColor="Blue" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="SaveUpdate" />
                <asp:HiddenField ID="hdnEquipmentId" runat="server" />
                <asp:HiddenField ID="hdnUserID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>