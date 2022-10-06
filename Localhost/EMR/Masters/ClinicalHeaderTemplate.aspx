<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ClinicalHeaderTemplate.aspx.cs" Inherits="Include_Master_Default" Theme="DefaultControls" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <asp:UpdatePanel ID="updHeader1" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2>Clinical Header Template</h2>
                </div>
                <div class="col-md-5">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="relativ text-center text-success alert_new" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-3 text-right pull-right">
                    <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary" Text="New" ToolTip="New" OnClick="btnNew_Click"
                        CausesValidation="false" />&nbsp;
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" ToolTip="Save"
                            OnClick="btnsave_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updHeader" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="pnlHeader" runat="server">
                <br />
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3 form-group">
                            <div class="col-md-4">Name</div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtHeaderName" runat="server" MaxLength="30" SkinID="textbox" Width="100%" />
                                <asp:RequiredFieldValidator ID="reqVal" runat="server" ControlToValidate="txtHeaderName"
                                    Display="None" ErrorMessage="Please enter Header Name" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-md-3 form-group">
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-6">Colomn(s)</div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlColomns" runat="server" SkinID="dropdown">
                                            <asp:ListItem Text="1" Value="1" />
                                            <asp:ListItem Selected="True" Text="2" Value="2" />
                                            <asp:ListItem Text="3" Value="3" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-6">Row(s)</div>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlRows" runat="server" SkinID="dropdown">
                                            <asp:ListItem Text="1" Value="1" />
                                            <asp:ListItem Selected="True" Text="2" Value="2" />
                                            <asp:ListItem Text="3" Value="3" />
                                            <asp:ListItem Text="4" Value="4" />
                                            <asp:ListItem Text="5" Value="5" />
                                            <asp:ListItem Text="6" Value="6" />
                                            <asp:ListItem Text="7" Value="7" />
                                            <asp:ListItem Text="8" Value="8" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 form-group">
                            <div class="col-md-4">Type</div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlType" runat="server" SkinID="dropdown">
                                    <asp:ListItem Text="Diagnostic Reports" Value="DR" />
                                    <asp:ListItem Selected="True" Text="Discharge Summery" Value="DS" />
                                    <asp:ListItem Text="Death Summary" Value="DE" />
                                    <asp:ListItem Text="Health Check Up" Value="HC" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3 pull-right">
                            <div class="col-md-6">
                                <asp:CheckBox ID="chkBorder" runat="server" Text="Show&nbsp;Border" />
                            </div>
                            <div class="col-md-6">
                                <asp:CheckBox ID="chkDate" runat="server" Text="Show&nbsp;Date" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3 form-group">
                            <div class="col-md-4">
                                <asp:Label ID="Label3" runat="server" Text="Status" />
                            </div>
                            <div class="col-md-8">
                                <asp:DropDownList ID="ddlActive" runat="server" SkinID="dropdown">
                                    <asp:ListItem Text="Active" Value="1" />
                                    <asp:ListItem Text="In-Active" Value="0" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" CssClass="btn btn-primary pull-right"
                                Text="Submit" ValidationGroup="Save" Width="100" />
                        </div>
                    </div>
                </div>
                <hr class="hr" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="updGrid" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvHeaderTemplate" AutoGenerateColumns="false" runat="server" SkinID="gridview"
                OnRowDataBound="gvHeaderTemplate_RowDataBound">
                <SelectedRowStyle BackColor="LightPink" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="col-md-12 form-group">
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtName0" Width="120px" SkinID="textbox" MaxLength="20" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlHTemplate0" SkinID="DropDown" runat="server" Width="180px" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="col-md-12 form-group">
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtName1" Width="120px" SkinID="textbox" MaxLength="20" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlHTemplate1" SkinID="DropDown" runat="server" Width="180px" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="col-md-12 form-group">
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtName2" Width="120px" SkinID="textbox" MaxLength="20" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ddlHTemplate2" SkinID="DropDown" runat="server" Width="180px" />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <hr class="hr" />
    <asp:UpdatePanel ID="updGridHeader" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvHeader" SkinID="gridview" Caption="Header Template(s)" AutoGenerateColumns="false"
                runat="server" OnRowDataBound="gvHeader_RowDataBound" DataKeyNames="HeaderId"
                OnSelectedIndexChanged="gvHeader_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="Sr No" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" Text='<%#Container.DataItemIndex + 1 %>' SkinID="label" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="250px">
                        <ItemTemplate>
                            <asp:Label ID="lblHeader" Text='<%#Eval("HeaderName")%>' SkinID="label" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblHeaderId" Text='<%#Eval("HeaderId")%>' SkinID="label" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="65px">
                        <ItemTemplate>
                            <asp:Label ID="lblActive1" Text='<%#Eval("Active")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField HeaderText="Edit" ButtonType="Link" ControlStyle-ForeColor="Blue"
                        ControlStyle-Font-Underline="true" SelectText="Edit" CausesValidation="false"
                        ShowSelectButton="true" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvHeaderTemplate" />
            <asp:AsyncPostBackTrigger ControlID="ddlActive" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
