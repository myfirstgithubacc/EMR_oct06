<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true"
    CodeFile="DashboardPermission.aspx.cs" Inherits="MPages_DashboardPermission"
    Title="Dashboard Permissionj" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px !important;}
        .RadGrid_Office2007 .rgFilterRow {background: #c1e5ef !important;}
        .RadGrid_Office2007 .rgPager { background: #c1e5ef 0 -7000px repeat-x !important; color: #00156e !important;}
    </style>


    <div class="container-fluid header_main form-group">
        <div class="col-sm-4"><h2><asp:Label ID="lblGroupDetails" runat="server"></asp:Label></h2></div>
        <div class="col-sm-6">
            <div class="PD-TabRadioNew01 margin_z">
                <asp:RadioButtonList ID="rdlLIst" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdlLIst_OnSelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Billing/MisDashboard" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Pharmacy" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="col-sm-2 text-right">
            <asp:Button ID="ibtnSave" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Save" ToolTip="Save Permission..." OnClick="ibtnSave_OnClick" />
        </div>
    </div>



    <div class="container-fluid">
        <div class="row">
            <asp:GridView ID="gvPermission" runat="server" AutoGenerateColumns="false" Width="100%" SkinID="gridviewOrderNew">
                <Columns>
                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="65%" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeader1" runat="server" Text=""></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkMisDashBoard" runat="server" Checked='<%#Eval("MISDashBoard")%>' />
                            <asp:HiddenField ID="hdnDashBoardType" runat="server" Value='<%#Eval("DashBoardType")%>' />
                            <asp:HiddenField ID="hdnpermissionId" runat="server" Value='<%#Eval("PermissionId")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblHeader2" runat="server" Text="Billing DashBoard"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkBillingDashBoard" runat="server" Checked='<%#Eval("BillingDashBoard")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>