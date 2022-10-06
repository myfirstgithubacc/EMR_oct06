<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Facility.aspx.cs" Inherits="EMR_Masters_Facility" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    
    <asp:UpdatePanel ID="update" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvFacility" />
            <asp:PostBackTrigger ControlID="btnSave" />
            <asp:PostBackTrigger ControlID="btnUpdate" />
        </Triggers>



        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-2 col-sm-2"><h2>Facility</h2></div>
                <div class="col-md-7 col-sm-7 text-center"><asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Green" Font-Bold="true" /></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:Button ID="btnNew" CssClass="btn btn-default"  runat="server" Text="New" OnClick="btnNew_OnClick" CausesValidation="false" />
                    <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="SaveFacility_OnClick" />
                    <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="UpdateLocation_OnClick" />                                
                </div>
            </div>



            <div class="container-fluid">
                <asp:Panel ID="pnlFacility" runat="server" DefaultButton="btnSave">
                    <div class="row form-groupTop01">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblFacility" runat="server" Text="Name"></asp:Label><span style="color: Red;">*</span></div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:TextBox ID="txtFacilityName" runat="server" MaxLength="250" CssClass="drapDrowHeight" Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFacilityName" Display="None" runat="server" ErrorMessage="Enter Facility Name"></asp:RequiredFieldValidator>
                                    <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="true" ShowSummary="false" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="Label1" runat="server" Text="Report To"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:DropDownList ID="ddlReportto" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:DropDownList></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-12 col-sm-12">
                                    <div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkmain" runat="server" Text="Main Facility" /></div>
                                    <div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkActive" runat="server" Text="Active" /></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-groupTop01">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblAddress" runat="server" Text="Address1"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtAddress" runat="server" CssClass="drapDrowHeight" MaxLength="249" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblAddress2" runat="server" Text="Address2"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtAddress2" runat="server" CssClass="drapDrowHeight" MaxLength="249" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label></div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="dropLCountry" runat="server" CssClass="drapDrowHeight" AutoPostBack="true" CausesValidation="false"
                                        OnSelectedIndexChanged="LocalCountry_SelectedIndexChanged" TabIndex="16" Width="100%">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-groupTop01">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblState" runat="server" Text="State"></asp:Label></div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="dropLState" runat="server" CssClass="drapDrowHeight" AppendDataBoundItems="true" AutoPostBack="true"
                                        OnSelectedIndexChanged="LocalState_SelectedIndexChanged" TabIndex="17" Width="100%">
                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblCity" runat="server" Text="City"></asp:Label></div>
                                <div class="col-md-8 col-sm-8">
                                    <%--<asp:DropDownList ID="ddlCity" runat="server" SkinID="DropDown" Width="220px">
                                        </asp:DropDownList>--%>
                                    <asp:DropDownList ID="dropLCity" runat="server" Width="100%" CssClass="drapDrowHeight"
                                        AppendDataBoundItems="true" TabIndex="18" AutoPostBack="true" OnSelectedIndexChanged="LocalCity_OnSelectedIndexChanged">
                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblZip" runat="server" Text="Zip"></asp:Label></div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="ddlZip" runat="server" Width="100%" CssClass="drapDrowHeight" AppendDataBoundItems="true" TabIndex="19">
                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-groupTop01">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblTelephone" runat="server" Text="Phone"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtTelephone" Width="100%" CssClass="drapDrowHeight" runat="server" Columns="10"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblFax" runat="server" Text="Fax"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtFax" Width="100%" CssClass="drapDrowHeight" runat="server" Columns="10"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblTimeZone" runat="server" Text="Time Zone"></asp:Label><span style="color: Red;">*</span></div>
                                <div class="col-md-8 col-sm-8">
                                    <asp:DropDownList ID="ddlTimeZone" runat="server" CssClass="drapDrowHeight" AppendDataBoundItems="true" TabIndex="19" Width="100%"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="TimeZone_RequiredFieldValidator" ControlToValidate="ddlTimeZone"
                                        InitialValue="0" Display="None" runat="server" ErrorMessage="Select Time Zone"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-groupTop01">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblNPI" runat="server" Text="NPI"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtNPI" Width="100%" CssClass="drapDrowHeight" runat="server" MaxLength="20" Columns="20"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><asp:Label ID="Label2" runat="server" Text="SMS Sender"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtSmsSender" CssClass="drapDrowHeight" runat="server" Width="100%" MaxLength="10"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2 PaddingRightSpacing"><asp:Label ID="lblSmsAdd" runat="server" Text="SMSServer Link"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtLinkServer" CssClass="drapDrowHeight" runat="server" MaxLength="500" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2 PaddingRightSpacing"><asp:Label ID="Label3" runat="server" Text="CaseNote Folder"></asp:Label></div>
                                <div class="col-md-8 col-sm-8"><asp:TextBox ID="txtCaseNoteFolder" CssClass="drapDrowHeight" runat="server" MaxLength="250" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><%--<asp:Label ID="lblPOS" runat="server" Text="POS"></asp:Label><span style="color: #FF0000">*</span>--%></div>
                                <div class="col-md-8 col-sm-8">
                                    <%--<asp:DropDownList ID="ddlPOS" runat="server" AppendDataBoundItems="true" TabIndex="19" Width="100%">
                                        <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlPOS" InitialValue="0"
                                        Display="None" runat="server" ErrorMessage="Select POS"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"></div>
                                <div class="col-md-8 col-sm-8"></div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>
            </div>

            <div class="container-fluid">
                <div class="row">
                    <asp:GridView ID="gvFacility" SkinID="gridviewOrderNew" CellPadding="4" runat="server" AutoGenerateColumns="false"
                        DataKeyNames="FacilityID" ShowHeader="true" Width="100%" PageSize="13" AllowPaging="true"
                        PagerSettings-Mode="NumericFirstLast" ShowFooter="false" PagerSettings-Visible="true"
                        PageIndex="0" OnRowDataBound="gvFacility_OnRowDataBound" OnRowCommand="gvFacility_OnRowCommand"
                        OnPageIndexChanging="gvFacility_OnPageIndexChanging" HeaderStyle-HorizontalAlign="Left"
                        OnSelectedIndexChanged="gvFacility_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="FacilityID" HeaderText="FacilityID" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="Name" HeaderText="Name" Visible="true" ReadOnly="true"
                                HeaderStyle-Width="15%" />
                            <asp:BoundField DataField="NPI" HeaderText="NPI" Visible="true" ReadOnly="true" HeaderStyle-Width="10%" />
                            <asp:BoundField DataField="POSCode" HeaderText="POSId" Visible="false" ReadOnly="true" />
                            <asp:BoundField DataField="POSName" HeaderText="POS" Visible="false" ReadOnly="true" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" Visible="true" ReadOnly="true"
                                HeaderStyle-Width="8%" />
                            <asp:BoundField DataField="Fax" HeaderText="Fax" Visible="true" ReadOnly="true" HeaderStyle-Width="8%" />
                            <asp:BoundField DataField="Address1" HeaderText="Address1" Visible="false" ReadOnly="true" />
                            <asp:BoundField DataField="CityId" HeaderText="CityId" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="CityName" HeaderText="CityName" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="StateId" HeaderText="StateId" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="StateName" HeaderText="StateName" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="CountryId" HeaderText="CountryId" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="CountryName" HeaderText="CountryName" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="PinNo" HeaderText="PinNo" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="Address2" HeaderText="Address2" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="TimeZoneId" HeaderText="TimeZoneId" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="TimeZoneOffSetMinutes" HeaderText="Time Zone OffSet Minutes" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="ReferToFacility" HeaderText="Report To Facility" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="MainFacility" HeaderText="Main Facility" Visible="true" ReadOnly="true" />
                            <asp:BoundField DataField="Status" HeaderText="Status" Visible="true" ReadOnly="true" HeaderStyle-Width="8%" />
                            <asp:TemplateField HeaderText="SMS SenderName" Visible="true" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblSmsSender" runat="server" Text='<%#Eval("SmsSender") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SMSServer Link" Visible="true" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblSmsServer" runat="server" Text='<%#Eval("SmsServer") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CaseNote Folder" Visible="true" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lblCaseNote" runat="server" Text='<%#Eval("CaseNotePath") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnActive" runat="server" Value='<% #Eval("Active") %>' />
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                        CausesValidation="false" CommandName="DeActivate" CommandArgument='<%#Eval("FacilityID")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" HeaderStyle-Width="5%" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>                