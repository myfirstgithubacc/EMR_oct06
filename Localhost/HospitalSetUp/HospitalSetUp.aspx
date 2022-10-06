<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="HospitalSetUp.aspx.cs" Inherits="HospitalSetUp_HospitalSetUp" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" /> 
    
    <style>
        .textName { width:150px; float:left;}
    </style>
    
    <div class="container-fluid">

        <script language="javascript" type="text/javascript">
            function Tab() {
                if (event.keyCode == 13)
                    event.keyCode = 9;
            }
            function MultilineMaxNumber(txt, maxLen) {
                try {
                    if (txt.value.length > (maxLen - 1)) {
                        alert("Only  " + maxLen + " Characters Alowed");
                        return false;
                    }
                } catch (e) {
                    alert(e);
                }
            }
        </script>
    </div>    
    

    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="chkAccept" EventName="CheckedChanged" />
        </Triggers>
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="col-md-3 col-sm-3"><h2>Organization SetUp</h2></div>
                <div class="col-md-6 col-sm-6 text-center">
                    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Green" Font-Bold="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-3 col-sm-3 text-right">
                    <span class="pull-right">
                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="lnkResetPassword" runat="server" CausesValidation="false" Text="Reset Password"
                                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                    OnClick="lnkResetPassword_OnClick" Font-Underline="false"></asp:LinkButton>
                                <script language="JavaScript" type="text/javascript">
                                    function LinkBtnMouseOver(lnk) {
                                        document.getElementById(lnk).style.color = "SteelBlue";
                                    }
                                    function LinkBtnMouseOut(lnk) {
                                        document.getElementById(lnk).style.color = "SteelBlue";
                                    }
                                </script>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </span>
                    

                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="Save_OnClick" />
                    <asp:ValidationSummary ID="VSHospital" runat="server" ShowMessageBox="True" ShowSummary="False" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 text-center"><asp:Label ID="lblTemplateMessage" runat="server" /></div>
        </div>
    </div>

    
    
    <div class="container-fluid header_mainGray margin_Top01">
        <div class="col-md-12 col-sm-12"><h2>Organization Details</h2></div>
    </div>

    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-8 col-sm-8">
                <div class="row">
                    <div class="col-md-2 col-sm-2 label2"><asp:Literal ID="ltrlAccountName" runat="server" Text="Organization"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-10 col-sm-10">
                        <asp:UpdatePanel ID="updAccountName" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtAccountName" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVtxtAccountName" runat="server" ErrorMessage="Enter Organization Name"
                                    SetFocusOnError="true" ControlToValidate="txtAccountName" Display="None">
                                </asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="ltrlNPI" runat="server" Text="NPI"></asp:Literal></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtNPI" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-4 label2"><asp:Literal ID="ltrlEIN" runat="server" Text="EIN"></asp:Literal></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtEIN" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2 PaddingRightSpacing"><asp:Literal ID="ltrlMainNo" runat="server" Text="Phone"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtMainNo" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FTEtxtMainNo" runat="server" Enabled="True" TargetControlID="txtMainNo"
                                    FilterType="Custom, Numbers" ValidChars="()- _">
                                </AJAX:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter Phone No"
                                    SetFocusOnError="true" ControlToValidate="txtMainNo" Display="None">
                                </asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="ltrlFaxNo" runat="server" Text="Fax"></asp:Literal></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtFaxNo" runat="server" Width="100%"  MaxLength="20"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FTEtxtFaxNo" runat="server" Enabled="True" TargetControlID="txtFaxNo"
                                    FilterType="Custom, Numbers" ValidChars="()- _">
                                </AJAX:FilteredTextBoxExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-8 col-sm-8">
                <div class="row">
                    <div class="col-md-2 col-sm-2 label2"><asp:Literal ID="ltrlMainAddress" runat="server" Text="Address"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-10 col-sm-10">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="250" Width="100%"
                                    onkeypress="return MultilineMaxNumber(this,250);"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RFVtxtAddress" runat="server" ErrorMessage="Enter Organization Address1"
                                    SetFocusOnError="true" ControlToValidate="txtAddress" Display="None">
                                </asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <asp:Literal ID="Literal1" runat="server" Text="Address2" Visible="false"></asp:Literal>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional" Visible="false">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:TextBox ID="txtAddress2" runat="server" SkinID="textbox" MaxLength="250" Width="95%"
                        onkeypress="return MultilineMaxNumber(this,250);"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="ltrlCountry" runat="server" Text="Country"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="updCountry" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server"
                                        AppendDataBoundItems="true" onkeydown="Tab();" Width="100%"
                                        OnSelectedIndexChanged="ddlCountry_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RFVddlCountry" runat="server" ErrorMessage="Enter Country"
                                        SetFocusOnError="true" ControlToValidate="ddlCountry" Display="None" InitialValue="0">
                                    </asp:RequiredFieldValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-4 label2"><asp:Literal ID="ltrlState" runat="server" Text="State"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="updState" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCountry" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlState" AutoPostBack="true" runat="server"
                                    AppendDataBoundItems="true" onkeydown="Tab();" Width="100%"
                                    OnSelectedIndexChanged="ddlState_OnSelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVddlState" runat="server" ErrorMessage="Enter State"
                                    SetFocusOnError="true" ControlToValidate="ddlState" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="ltrlCity" runat="server" Text="City"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="updCity" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlState" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCity" AutoPostBack="true" runat="server" AppendDataBoundItems="true" onkeydown="Tab();" Width="100%" OnSelectedIndexChanged="ddlCity_OnSelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVddlCity" runat="server" ErrorMessage="Enter City"
                                    SetFocusOnError="true" ControlToValidate="ddlCity" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="ltrlZip" runat="server" Text="ZIP"></asp:Literal><span style="color: Red">&nbsp;*</span></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="updZip" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCity" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlZip" AutoPostBack="false" runat="server" Width="100%"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVddlZip" runat="server" ErrorMessage="Enter Zip"
                                    SetFocusOnError="true" ControlToValidate="ddlZip" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group" id="trposcode" runat="server">
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblPOS" runat="server" Text="POS"></asp:Label><span style="color: #FF0000">&nbsp;*</span></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:DropDownList ID="ddlPOS" runat="server" AppendDataBoundItems="true" TabIndex="19" Width="100%">
                            <asp:ListItem Text="[Select]" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="ddlPOS"
                            InitialValue="0" Display="None" runat="server" ErrorMessage="Select POS"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-3"></div>
                    <div class="col-md-8 col-sm-9"></div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-3"></div>
                    <div class="col-md-8 col-sm-9"></div>
                </div>
            </div>
        </div>
    </div>



    <div class="container-fluid header_mainGray margin_Top01" id="tbltimezonehd" runat="server">
        <div class="col-md-6 col-sm-6"><h2>Timing Zone</h2></div>
        <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="Label5" runat="server" Font-Bold="true" ForeColor="Green" /></div>
    </div>

    <div class="container-fluid" id="tbltimezone" runat="server">
        <div class="row form-group">
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-4 label2"><asp:Label ID="lblTimeZone" runat="server" Text="Time Zone"></asp:Label><span style="color: Red;">&nbsp;*</span></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlTimeZone" runat="server" AppendDataBoundItems="true" TabIndex="19" Width="100%"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="TimeZone_RequiredFieldValidator" ControlToValidate="ddlTimeZone" InitialValue="0"
                                        Display="None" runat="server" ErrorMessage="Select Time Zone"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-3"></div>
                    <div class="col-md-8 col-sm-9"></div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-3"></div>
                    <div class="col-md-8 col-sm-9"></div>
                </div>
            </div>
        </div>
    </div>
    



    <div class="container-fluid header_mainGray margin_Top01" id="table1" runat="server">
        <div class="col-md-6 col-sm-6"><h2>Billing Address</h2></div>
        <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="Label4" runat="server" ForeColor="Green" Font-Bold="true" /></div>
    </div>

    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-8 col-sm-8">
                <div class="row">
                    <div class="col-md-2 col-sm-2 label2"><asp:Literal ID="Literal2" runat="server" Text="Address"></asp:Literal></div>
                    <div class="col-md-10 col-sm-10">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="txtBillingaddress" runat="server" MaxLength="250"
                                    Width="100%"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <span style="display:none;">Address2</span>
            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional" Visible="false">
                <ContentTemplate>
                    <asp:TextBox ID="txtBillingaddress2" runat="server" MaxLength="250" Width="100%"></asp:TextBox>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="Literal5" runat="server" Text="Country"></asp:Literal></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlbillingcountery" AutoPostBack="true" runat="server"
                                    AppendDataBoundItems="true" onkeydown="Tab();" Width="100%"
                                    OnSelectedIndexChanged="ddlbillingcountery_OnSelectedIndexChanged">
                                </asp:DropDownList>
                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Enter Country"
                        SetFocusOnError="true" ControlToValidate="ddlbillingcountery" Display="None" InitialValue="0">*</asp:RequiredFieldValidator>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-4 label2"><asp:Literal ID="Literal8" runat="server" Text="State"></asp:Literal></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlbillingcountery" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlbillingstate" AutoPostBack="true" runat="server"
                                    AppendDataBoundItems="true" onkeydown="Tab();" Width="100%"
                                    OnSelectedIndexChanged="ddlbillingstate_OnSelectedIndexChanged">
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Enter State"
                                SetFocusOnError="true" ControlToValidate="ddlbillingstate" Display="None" InitialValue="0">*</asp:RequiredFieldValidator>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="Literal6" runat="server" Text="City"></asp:Literal></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlbillingstate" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlbillingcity" AutoPostBack="true" runat="server"
                                    AppendDataBoundItems="true" onkeydown="Tab();" Width="100%"
                                    OnSelectedIndexChanged="ddlbillingcity_OnSelectedIndexChanged">
                                    <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                                <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Enter City"
                                    SetFocusOnError="true" ControlToValidate="ddlCity" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 label2"><asp:Literal ID="Literal9" runat="server" Text="Zip"></asp:Literal></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlbillingcity" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlbillingzip" AutoPostBack="false" runat="server" Width="100%"></asp:DropDownList>
                                <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Enter Zip"
                                    SetFocusOnError="true" ControlToValidate="ddlZip" Display="None" InitialValue="0">
                                </asp:RequiredFieldValidator>--%>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-4 col-sm-4 lable2"><span class="textName"><asp:Literal ID="Literal4" runat="server" Text="End of Year Date"></asp:Literal><span style="color: Red">&nbsp;*</span></span></div>
                    <div class="col-md-8 col-sm-8">
                        <asp:UpdatePanel ID="updendofyear" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddlmonth" runat="server" AutoPostBack="True" Width="100%"
                                                OnSelectedIndexChanged="ddlmonth_SelectedIndexChanged">
                                                <asp:ListItem Value="00" Text="[ Month ]"></asp:ListItem>
                                                <asp:ListItem Value="01" Text="January"></asp:ListItem>
                                                <asp:ListItem Value="02" Text="February"></asp:ListItem>
                                                <asp:ListItem Value="03" Text="March"></asp:ListItem>
                                                <asp:ListItem Value="04" Text="April"></asp:ListItem>
                                                <asp:ListItem Value="05" Text="May"></asp:ListItem>
                                                <asp:ListItem Value="06" Text="June"></asp:ListItem>
                                                <asp:ListItem Value="07" Text="July"></asp:ListItem>
                                                <asp:ListItem Value="08" Text="August"></asp:ListItem>
                                                <asp:ListItem Value="09" Text="September"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="October"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="November"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="December"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddldate" runat="server" Style="visibility: hidden; height: 1px; float: left; margin: 0; padding: 0 !important; width: 1px;">
                                                <asp:ListItem Value="0" Text="[ Date ]"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Select End Of Year Date"
                                                SetFocusOnError="true" ControlToValidate="ddlmonth" Display="None" InitialValue="00">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlmonth" />
                                <asp:AsyncPostBackTrigger ControlID="ddldate" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 lable2"><asp:Literal ID="Literal3" runat="server" Text="Phone"></asp:Literal></div>
                    <div class="col-md-9 col-sm-9">
                        <asp:TextBox ID="txtBillingphone" runat="server" width="100%" MaxLength="20"></asp:TextBox>
                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                            TargetControlID="txtBillingphone" FilterType="Custom, Numbers" ValidChars="()- _">
                        </AJAX:FilteredTextBoxExtender>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div class="row">
                    <div class="col-md-3 col-sm-3 lable2"></div>
                    <div class="col-md-9 col-sm-9"></div>
                </div>
            </div>
        </div>

    </div>



    <div class="container-fluid header_mainGray margin_Top01" id="tableUserDetails" runat="server">
        <div class="col-md-6 col-sm-6"><h2>User Details</h2></div>
        <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="Label2" runat="server" Font-Bold="true" ForeColor="Green" /></div>
    </div>


    <div class="container-fluid" id="trnewuser" runat="server" visible="true">
        <asp:UpdatePanel ID="Availpanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="row form-group">
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2">Prefix</div>
                            <div class="col-md-8 col-sm-8"><asp:DropDownList ID="ddltitle" runat="server" Width="100%" DataTextField="Name" DataValueField="TitleId"></asp:DropDownList></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2"></div>
                            <div class="col-md-9 col-sm-9"></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2"></div>
                            <div class="col-md-9 col-sm-9"></div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2">Last Name<span style="color: Red;">&nbsp;*</span></div>
                            <div class="col-md-8 col-sm-8">
                                <asp:TextBox ID="txtlastname" Width="100%" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="None" ControlToValidate="txtlastname"
                                    runat="server" ErrorMessage="Enter Last Name"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2"><span class="textName">First Name<span style="color: Red;">&nbsp;*</span></span></div>
                            <div class="col-md-9 col-sm-9">
                                <asp:TextBox ID="txtfirstname" Width="100%" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="None" ControlToValidate="txtfirstname"
                                    runat="server" ErrorMessage="Enter First Name"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2"><span class="textName">Middle Name</span></div>
                            <div class="col-md-9 col-sm-9"><asp:TextBox ID="txtmiddlename" Width="100%" runat="server" MaxLength="50"></asp:TextBox></div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"><span class="textName"><asp:Literal ID="Literal7" runat="server" Text="Employee Type"></asp:Literal></span></div>
                            <div class="col-md-8 col-sm-8"><asp:DropDownList ID="ddlemployeetype" runat="server" AutoPostBack="false" Width="100%"></asp:DropDownList></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2">
                                <span class="textName"><asp:Label ID="lblEmploymentStatus" runat="server" Text="Employment Status"></asp:Label><span style="color: Red">&nbsp;*</span></span>
                            </div>
                            <div class="col-md-9 col-sm-9">
                                <asp:DropDownList ID="ddlEmploymentstatus" runat="server" Width="100%"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="employmentvalidation" runat="server" ControlToValidate="ddlEmploymentstatus"
                                    InitialValue="0" Display="None" ErrorMessage="Select Employment Status">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2"></div>
                            <div class="col-md-9 col-sm-9"></div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2">User Name<span style="color: Red;">&nbsp;*</span></div>
                            <div class="col-md-8 col-sm-8">
                                <asp:UpdatePanel ID="up1" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnAvailability" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-4">
                                                <asp:TextBox ID="txtusername" runat="server" Width="100%" Columns="30" MaxLength="15"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="None" ControlToValidate="txtusername"
                                                    runat="server" ErrorMessage="Enter User Name"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-6 col-sm-8 PaddingLeftSpacing">
                                                <asp:Button ID="btnAvailability" runat="server" CssClass="btn btn-primary" Text="Check User Availability"
                                                    ValidationGroup="username" OnClick="btnAvailability_Click" Font-Size="10px" Height="20px" />
                                                <asp:Label ID="lblAvailabilityMessage" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2">Password<span style="color: Red;">&nbsp;*</span></div>
                            <div class="col-md-9 col-sm-9">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-md-10 col-sm-9 PaddingRightSpacing">
                                                <asp:TextBox ID="txtpassword" MaxLength="30" TextMode="Password" Width="100%"
                                            runat="server" ToolTip="Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."></asp:TextBox>
                                            </div>
                                            <div class="col-md-2 col-sm-2">
                                                <asp:Button ID="btnpasshelp" runat="server" Text="?" CausesValidation="false" CssClass="btn btn-primary" 
                                                    ToolTip="  Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."
                                                    OnClick="btnpasshelp_Click" />
                                            </div>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="None" ControlToValidate="txtpassword"
                                            runat="server" ErrorMessage="Enter Password"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="regexpName" Display="None" runat="server" ErrorMessage="Password is not in write format."
                                            ControlToValidate="txtpassword" ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^*&+=]).*$"></asp:RegularExpressionValidator>
                                        <telerik:RadToolTip ID="RadToolTip1" runat="server" EnableShadow="true" RelativeTo="Element"
                                            Height="40px" Width="300px" TargetControlID="txtpassword" BackColor="#FFFFD5"
                                            Position="TopCenter">
                                        </telerik:RadToolTip>
                                        <AJAX:PasswordStrength ID="PasswordStrength1" DisplayPosition="BelowLeft" CalculationWeightings="50;15;15;20"
                                            TextStrengthDescriptions="Weak;Average;Strong;Excellent" TargetControlID="txtpassword"
                                            MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumSymbolCharacters="1"
                                            MinimumUpperCaseCharacters="1" runat="server">
                                        </AJAX:PasswordStrength>
                                        <telerik:RadToolTip ID="RadToolTip5" runat="server" EnableShadow="true" RelativeTo="Element"
                                            Height="40px" Width="300px" TargetControlID="btnpasshelp" BackColor="#FFFFD5"
                                            Position="TopCenter">
                                        </telerik:RadToolTip>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2"><span class="textName">Confirm Password<span style="color: Red;">&nbsp;*</span></span></div>
                            <div class="col-md-9 col-sm-9">
                                <asp:TextBox ID="txtConfirmPassword" MaxLength="30" runat="server" Width="100%"
                                    ToolTip="Passwords must be alphanumeric, a minimum of 8 characters, and should include both lower and upper case  one number one Special Character @#$%^&amp;=*' letters."
                                    TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Display="None" ControlToValidate="txtConfirmPassword"
                                    runat="server" ErrorMessage="Enter Confirm Password"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Password and Confirm Password Should be Same"
                                    Display="None" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"
                                    SetFocusOnError="true" Operator="Equal"></asp:CompareValidator>
                                <telerik:RadToolTip ID="RadToolTip2" runat="server" EnableShadow="true" RelativeTo="Element"
                                    Height="40px" Width="300px" TargetControlID="txtConfirmPassword" BackColor="#FFFFD5"
                                    Position="TopCenter">
                                </telerik:RadToolTip>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row form-group">
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2">EMail</div>
                            <div class="col-md-8 col-sm-8">
                                <asp:TextBox ID="txtemailid" MaxLength="70" CssClass="Textbox" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtemailid"
                                    runat="server" ErrorMessage="Not a valid e-Mail." Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2">Work</div>
                            <div class="col-md-9 col-sm-9">
                                <asp:TextBox ID="txtworkPhone" CssClass="Textbox" Columns="15" runat="server" MaxLength="20"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    TargetControlID="txtworkPhone" FilterType="Custom, Numbers" ValidChars="()- _">
                                </AJAX:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 label2">Mobile</div>
                            <div class="col-md-9 col-sm-9">
                                <asp:TextBox ID="txtmobileno" CssClass="Textbox" Columns="15" runat="server"></asp:TextBox>
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                    TargetControlID="txtmobileno" FilterType="Custom, Numbers" ValidChars="()- _">
                                </AJAX:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAvailability" />
            </Triggers>
        </asp:UpdatePanel>
    </div>




    <div class="container-fluid header_mainGray margin_Top01">
        <div class="col-md-6 col-sm-6"><h2>Terms & Conditions</h2></div>
        <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="Label3" runat="server" Font-Bold="true" ForeColor="Green" /></div>
    </div>


    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-12 col-sm-12">
                Please read the 
                <asp:Label ID="lblTerms" runat="server" Text="Terms of V-Care Service" CssClass="btnNew" Font-Underline="true" Font-Bold="true" Style="cursor: pointer; color:blue;"></asp:Label>
                and the
                <asp:Label ID="lblPrivacyPolicy" runat="server" Text="Privacy Policy" CssClass="btnNew" Font-Underline="true" Font-Bold="true" Style="cursor: pointer;color:blue;"></asp:Label>.
                If you agree with these terms and conditions, please accept the term below.
            </div>
        </div>

        <div class="row form-group">
            <div class="col-md-12 col-sm-12">
                <div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkAccept" Font-Bold="true" runat="server" AutoPostBack="true" Text="I Accept" OnCheckedChanged="chkAccept_OnCheckedChanged" /></div>
                Please read this standard
                <asp:Label ID="Label1" runat="server" Text="Business Associate Agreement" CssClass="btnNew" Font-Underline="true" Font-Bold="true" ForeColor="Blue" Style="cursor: pointer; color:blue;"></asp:Label>.<br />
                If you agree with these terms and conditions,please print,sign and attach the document. If you have your Business Associate Agreement, please attach it or fax it to XXX-XXX-XXXX. We will review the agreement and execute it or contact you with concerns. This can be completed at any time.
            </div>
        </div>

    </div>
    
    



</asp:Content>