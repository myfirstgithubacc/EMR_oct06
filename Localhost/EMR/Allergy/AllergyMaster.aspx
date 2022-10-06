<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AllergyMaster.aspx.cs" Inherits="EMR_Allergy_AllergyMaster"
    MasterPageFile="~/Include/Master/EMRMaster.master" Title="Allergy Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">

        function pageLoad() {
        }        
    </script>
    <style type="text/css">
        div.RadWindow_Metro .rwTitlebarControls em {
            width: 100% !important;
        }

        td.rwCorner.rwBodyLeft {
            width: 0px !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function Tab_SelectionChanged(sender) {
            var tabIndx = sender.get_activeTabIndex();
            //alert(tabIndx);
            if (tabIndx == 0) {
                var ctrlsave = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveAllergyMaster');
                //alert(ctrlsave);
                ctrlsave.style.display = 'block';
                var ctrlsave2 = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveAllergyTypeMaster');
                ctrlsave2.style.display = 'none';
            }
            else {
                var ctrlsave = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveAllergyMaster');
                // alert(ctrlsave);
                ctrlsave.style.display = 'none';
                var ctrlsave2 = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveAllergyTypeMaster');
                ctrlsave2.style.display = 'block';
            }
        }
    </script>
    <style>
        select#ctl00_ContentPlaceHolder1_tbMaster_tbpnlAllergyMaster_ddlAllergyType {
            height: 25px !important;
            border-radius: 3px !important;
        }
        span#ctl00_ContentPlaceHolder1_tbMaster_tbpnlAllergyMaster_ltrlAllergyName{
            font-size:12px!important;
        }
        span#ctl00_ContentPlaceHolder1_tbMaster_tbpnlAllergyMaster_ltrlAllergyType {
              font-size:12px!important;
        }
        .ajax__tab_xp .ajax__tab_body{
            border:none!important;
            padding:0px!important;
        }
     
    </style>
    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-6 col-sm-6 col-6">
                <div class="hidden">
                    <asp:Label ID="lblAllergyName" SkinID="label" runat="server" Text="Allergy Master "
                        Font-Bold="true"></asp:Label>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-6 text-right">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="ibtnSaveAllergyMaster" runat="server" CausesValidation="true"
                            OnClick="SaveAllergyMaster_OnClick" ToolTip="Save Allergy" ValidationGroup="Save"
                            Text="Save Allergy Name" CssClass="btn btn-xs btn-primary pull-right" />
                        <asp:ValidationSummary ID="VSAllergyMaster" runat="server" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="Save" CssClass="btn btn-xs btn-primary pull-right" />
                        <asp:Button ID="ibtnSaveAllergyTypeMaster" runat="server" CausesValidation="true"
                            OnClick="SaveAllergyTypeMaster_OnClick" Style="display: none;"
                            ToolTip="Save Allergy Type" ValidationGroup="SaveT" CssClass="btn btn-xs btn-primary pull-right" Text="Save Allergy Type" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="SaveT" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12 col-12">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMsg" runat="server" Text="" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyMaster" />
                                    <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyTypeMaster" />
                                </Triggers>
                            </asp:UpdatePanel>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12 col-12 m-t">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyMaster" />
                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyTypeMaster" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="pnltbMaster" runat="server" Width="100%">
                            <div class="row">
                                <AJAX:TabContainer ID="tbMaster" runat="server" ActiveTabIndex="0" OnClientActiveTabChanged="Tab_SelectionChanged"
                                    Width="100%">
                                    <AJAX:TabPanel ID="tbpnlAllergyMaster" runat="server" HeaderText="Allergy Master">
                                        <ContentTemplate>
                                            <div class="col-md-12 col-sm-12 col-12">
                                                <div class="row" style="margin-top: 5px;">
                                                    <div class="col-md-4 col-sm-5 col-6">
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-3 col-sm-4 col-4" style="margin-top: 3px;">
                                                                <asp:Label ID="ltrlAllergyType" runat="server" Text="Allergy&nbsp;Type"></asp:Label>
                                                            </div>
                                                            <div class="col-md-8 col-sm-8 col-8">
                                                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:DropDownList ID="ddlAllergyType" SkinID="DropDown" runat="server" OnSelectedIndexChanged="ddlAllergyType_OnSelectedIndexChanged"
                                                                            AutoPostBack="true" Width="100%">
                                                                        </asp:DropDownList>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="gvAllergy" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                                <asp:UpdatePanel ID="updHdn" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hdnAllergyType" runat="server" />
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlAllergyType" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-7 col-6">
                                                        <div class="row p-t-b-5">
                                                            <div class="col-md-4 col-sm-4 col-4" style="margin-top: 3px;">
                                                                <asp:Label ID="ltrlAllergyName" runat="server" Text="Add New Allergy"></asp:Label>
                                                            </div>
                                                            <div class="col-md-8 col-sm-8 col-8">
                                                                <asp:UpdatePanel ID="updtxtAllergyName" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtAllergyName" SkinID="textbox" Width="100%" runat="server" Columns="50" MaxLength="100"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RFVtxtAllergyName" runat="server" ErrorMessage="Allergy Description Cannot Be Blank..."
                                                                            SetFocusOnError="true" ControlToValidate="txtAllergyName" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyMaster" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12 col-sm-12 col-xs-12 gridview p-t-b-5">
                                                                <asp:UpdatePanel ID="updgvAllergy" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvAllergy" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                                                            DataKeyNames="AllergyID" ShowHeader="true" PageSize="13" AllowPaging="true"
                                                                            PagerSettings-Mode="NumericFirstLast" ShowFooter="false" PagerSettings-Visible="true"
                                                                            PageIndex="0" OnRowDataBound="gvAllergy_OnRowDataBound" OnRowCommand="gvAllergy_OnRowCommand"
                                                                            OnRowCancelingEdit="gvAllergy_OnRowCancelingEdit" OnRowUpdating="gvAllergy_OnRowUpdating"
                                                                            OnPageIndexChanging="gvAllergy_OnPageIndexChanging" OnRowEditing="gvAllergy_OnRowEditing" CssClass="table table-bordered">
                                                                            <PagerSettings Mode="NumericFirstLast" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="AllergyID" HeaderText="AllergyID" Visible="true" ReadOnly="true" />
                                                                                <asp:TemplateField HeaderText="Sl&nbsp;No" Visible="true">
                                                                                    <ItemTemplate>
                                                                                        <%# Container.DataItemIndex+1 %>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Description">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltrlGridDescription" runat="server" Text='<%#Eval("AllergyName")%>'></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:TextBox ID="txtGridAllergyName" SkinID="textbox" runat="server" Text='<%#Eval("AllergyName")%>'
                                                                                            MaxLength="100" Width="98%"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="RFVtxtGridAllergyName" runat="server" ControlToValidate="txtGridAllergyName"
                                                                                            ValidationGroup="Update" ErrorMessage="Name Cannot Be Blank" ForeColor="White"
                                                                                            Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-Width="90px"
                                                                                    HeaderStyle-Width="90px" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ddlAllergyType" />
                                                                        <asp:AsyncPostBackTrigger ControlID="gvAllergy" />
                                                                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyMaster" />
                                                                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyTypeMaster" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </AJAX:TabPanel>
                                            <AJAX:TabPanel ID="tbpnlAllergyTypeMaster" runat="server" HeaderText="Allergy Type Master">
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-sm-12 col-xs-12">
                                                        <div class="row">
                                                    <div class="col-md-5 col-sm-7 col-xs-12">
                                                                <div class="row p-t-b-5" style="margin-top:5px;">
                                                                    <div class="col-md-4 col-sm-4 col-xs-5 text-nowrap" style="font-size: 12px;margin-top: 3px;" >
                                                                        <asp:Literal ID="Literal2" runat="server" Text="Add New Allergy Type"></asp:Literal>
                                                                    </div>
                                                                    <div class="col-md-8 col-sm-8 col-xs-7">
                                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyTypeMaster" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <asp:TextBox ID="txtAddNewAllergyType" SkinID="textbox" runat="server" Columns="50"
                                                                            MaxLength="100"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Allergy Type Description Cannot Be Blank"
                                                                            SetFocusOnError="true" ControlToValidate="txtAddNewAllergyType" Display="None"
                                                                            ValidationGroup="SaveT"></asp:RequiredFieldValidator>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                    </div>
                                                                    </div>
                                                                </div>
                                                            <div class="col-md-4 col-sm-4 col-xs-12">
                                                                <div class="row p-t-b-5">
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                    </div>
                                                                    <div class="col-md-8 col-sm-8 col-xs-8"></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        <div class="row">
                                                            <div class="col-md-12 col-sm-12 col-xs-12 gridview p-t-b-5">
                                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="gvAllergyType" />
                                                                        <asp:AsyncPostBackTrigger ControlID="ibtnSaveAllergyTypeMaster" />
                                                                    </Triggers>
                                                                    <ContentTemplate>
                                                                        <asp:GridView ID="gvAllergyType" CellPadding="4" runat="server"
                                                                            AutoGenerateColumns="false" DataKeyNames="TypeId" ShowHeader="true"
                                                                            PageSize="13" AllowPaging="False" PagerSettings-Mode="NumericFirstLast" ShowFooter="false"
                                                                            PagerSettings-Visible="true" PageIndex="0" OnRowDataBound="gvAllergyType_OnRowDataBound"
                                                                            OnRowCommand="gvAllergyType_OnRowCommand" OnRowCancelingEdit="gvAllergyType_OnRowCancelingEdit"
                                                                            OnRowUpdating="gvAllergyType_OnRowUpdating" OnPageIndexChanging="gvAllergyType_OnPageIndexChanging"
                                                                            OnRowEditing="gvAllergyType_OnRowEditing" CssClass="table table-bordered">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="TypeId" HeaderText="TypeId" Visible="true" ReadOnly="true" />
                                                                                <asp:TemplateField HeaderText="Sl&nbsp;No" Visible="true">
                                                                                    <ItemTemplate>
                                                                                        <%# Container.DataItemIndex+1 %>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="30px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Description">
                                                                                    <ItemTemplate>
                                                                                        <asp:Literal ID="ltrlGridDescription" runat="server" Text='<%#Eval("TypeName")%>'></asp:Literal>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:TextBox ID="txtGridAllergyName" SkinID="textbox" runat="server" Text='<%#Eval("TypeName")%>'
                                                                                            MaxLength="100" Width="98%"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="RFVtxtGridAllergyName" runat="server" ControlToValidate="txtGridAllergyName"
                                                                                            ValidationGroup="Update" ErrorMessage="Name Cannot Be Blank" ForeColor="White"
                                                                                            Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <EditItemTemplate>
                                                                                        <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </EditItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:CommandField ShowEditButton="true" ValidationGroup="Update" ItemStyle-Width="90px"
                                                                                    HeaderStyle-Width="90px" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                </ContentTemplate>
                                            </AJAX:TabPanel>
                                        </AJAX:TabContainer>
                                    </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
            </div>
    </div>

    
</asp:Content>
