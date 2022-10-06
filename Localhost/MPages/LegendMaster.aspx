<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="LegendMaster.aspx.cs" Inherits="EMR_Masters_LegendMaster" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />  




    <div>
        <asp:UpdatePanel ID="updsave" runat="server">
            <ContentTemplate>

                <div class="container-fluid header_main form-group">
                    <div class="col-md-2 col-sm-3"><h2>Color Legend</h2></div>
                    <div class="col-md-8 col-sm-7 text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="Error msg" Font-Bold="true" ForeColor="Green" />
                        <asp:ValidationSummary ID="VS" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="SaveUpdate" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
                            ShowSummary="false" ValidationGroup="SaveColor" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="true"
                            ShowSummary="false" ValidationGroup="SaveStatus" />
                    </div>
                    <div class="col-md-2 col-sm-2 text-right">
                        <asp:Button ID="ibtnLegendColorSave" runat="server" CausesValidation="true" OnClick="ibtnLegendColorSave_OnClick"
                            ToolTip="Save" CssClass="btn btn-primary" ValidationGroup="SaveUpdate" Text="Save" />
                    </div>
                </div>



                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3 col-sm-4">
                            <div class="row form-group">
                                <div class="col-md-4 col-sm-4">Type</div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="ddlType" runat="server" Width="100%" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-4 col-sm-4">Status</div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="ddlLegendType" runat="server" DataTextField="FieldDescription"
                                        DataValueField="FieldID" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row form-group">
                                <div class="col-md-4 col-sm-4 PaddingRightSpacing">Select Color</div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="ddlColorList" runat="server" Width="100%"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvColorList" runat="server" ControlToValidate="ddlColorList"
                                        InitialValue="Select" ErrorMessage="Please select color list." Display="None"
                                        ValidationGroup="SaveUpdate"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>


                        <div class="col-md-5 col-sm-4">
                            <div class="row" id="trCustomStatus" runat="server" visible="false">
                                <div class="col-md-4 col-sm-4">
                                    <asp:LinkButton ID="lnkbAddCustomColor" runat="server" Text="Add Custom Color" OnClick="lnkbAddCustomColor_OnClick" />
                                    <asp:LinkButton ID="lnkbAddStatus" runat="server" Text="Add Appointment Status" OnClick="lnkbAddStatus_OnClick" />
                                </div>
                                <div class="col-md-8 col-sm-8">

                                </div>
                            </div>



                            <span id="trAddCustomColor" runat="server" visible="false">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4">Color Name</div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtColorName" Width="100%" runat="server" MaxLength="40" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtColorName"
                                            ValidationGroup="SaveColor" Display="None" runat="server" ErrorMessage="Enter Color Name !"
                                            Text="" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4">Color Code</div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtColorCode" Width="100%" runat="server" MaxLength="10" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtColorCode"
                                            ValidationGroup="SaveColor" Display="None" runat="server" ErrorMessage="Enter Color Code !"
                                            Text="" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4"></div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:Button ID="btnSaveColor" runat="server" CausesValidation="true" OnClick="btnSaveColor_OnClick"
                                            ToolTip="Save Color" CssClass="btn btn-primary" ValidationGroup="SaveColor" Text="Save Color" />&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="true" OnClick="btnCancel_OnClick"
                                            ToolTip="Cancel" CssClass="btn btn-primary" Text="Cancel" />
                                    </div>
                                </div>
                            </span>


                            <span id="trAddStatus" runat="server" visible="false">
                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4">Status</div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtstatus" Width="100%" runat="server" MaxLength="40" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtstatus"
                                            ValidationGroup="SaveStatus" Display="None" runat="server" ErrorMessage="Enter Status !"
                                            Text="" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4">Status Color</div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:DropDownList ID="ddlStatusColor" runat="server" Width="100%"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlStatusColor"
                                            InitialValue="Select" ErrorMessage="Please Select Status color." Display="None"
                                            ValidationGroup="SaveStatus"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4">Status Code</div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtStatusCode" Width="100%" runat="server" MaxLength="10" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtStatusCode"
                                            ValidationGroup="SaveStatus" Display="None" runat="server" ErrorMessage="Enter Status Code !"
                                            Text="" />
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-4 col-sm-4">Active</div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:DropDownList ID="ddlStatusActive" runat="server">
                                            <asp:ListItem Text="Active" Value="1" Selected="True" />
                                            <asp:ListItem Text="In-Active" Value="0" />
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Button ID="btnSaveStatus" runat="server" CausesValidation="true" OnClick="btnSaveStatus_OnClick"
                                            ToolTip="Save Status" CssClass="btn btn-primary" ValidationGroup="SaveStatus" Text="Save Status" />&nbsp;&nbsp;
                                        <asp:Button ID="btnCancelStatus" runat="server" CausesValidation="true" OnClick="btnCancelStatus_OnClick"
                                            ToolTip="Cancel Status" CssClass="btn btn-primary" Text="Cancel" />
                                    </div>
                                </div>
                            </span>

                        </div>


                        <div class="col-md-4 col-sm-4">
                            <asp:Label ID="lblOrders" runat="server" Text="Color Legends" Font-Size="10"
                                Font-Bold="true"></asp:Label><br />
                            <asp:Panel ID="pnlgvSelectedLegends" runat="server" ScrollBars="Auto" Height="200px"
                                Width="100%">
                                <asp:GridView SkinID="gridviewOrderNew" ID="gvSelectedLegends" CellPadding="1" CellSpacing="0"
                                    runat="server" AutoGenerateColumns="false" DataKeyNames="StatusId" ShowHeader="true"
                                    Width="100%" ForeColor="#333333" GridLines="None" PageSize="10" AllowPaging="false"
                                    ShowFooter="false" PageIndex="0" PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true"
                                    OnRowDataBound="gvSelectedLegends_OnRowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="StatusId" HeaderText="ServiceID" Visible="true" ReadOnly="true" />
                                        <asp:BoundField DataField="Status" HeaderText="Description" Visible="true" ReadOnly="true" />
                                        <asp:BoundField DataField="StatusColor" HeaderStyle-HorizontalAlign="Center" HeaderText="Color"
                                            Visible="true" ReadOnly="true" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>




                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>