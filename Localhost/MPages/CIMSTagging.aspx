<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="CIMSTagging.aspx.cs" Inherits="MPages_CIMSTagging" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="/Include/JS/Functions.js" language="javascript">
    </script>

    <script type="text/javascript">
        function OpenCIMSWindow() {
            var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

            var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(ReportContent.value);
            WindowObject.document.close();
            WindowObject.focus();
        }
        function cboItem_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%= hdnItemId.ClientID %>').value = item != null ? item.get_value() : sender.value();
            $get('<%= btnbrandselect.ClientID %>').click();
        }

        function OnClientSelectedIndexChangedEventHandler(sender, args) {
            var item = args.get_item();
            $get('<%= hdnItemDetailId.ClientID %>').value = item != null ? item.get_value() : sender.value();
            $get('<%= btnItemSelect.ClientID %>').click();
        }

        function ValidateRequiredBrandName(source, args) {
            if ($get('<%= hdnItemId.ClientID %>').value == "") {
                args.IsValid = false;
                source.errormessage = "Please select Name.";
            }
        }              
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr class="clsheader">
            <td width="150px">
                <asp:Label ID="lblheader" runat="server" SkinID="label" Text="&nbsp;CIMS Tagging Setup" />
            </td>
            <td align="center">
                <asp:Label ID="lblMessage" runat="server" SkinID="label" />
                <asp:ValidationSummary ID="vsScheduleMaster" DisplayMode="BulletList" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="CIMSTagging" runat="server" />
            </td>
            <td width="100px">
                <asp:Button ID="btnNew" runat="server" Text="New" SkinID="Button" OnClick="btnNew_Click" />
            </td>
            <td width="100px" align="right">
                <table cellpadding="1" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" SkinID="Button" ValidationGroup="CIMSTagging"
                                OnClick="btnSave_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.close()"
                                SkinID="Button" Visible="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table cellpadding="4" cellspacing="0" style="margin-left: 10px">
        <tr>
            <td colspan="2">
                <asp:RadioButtonList ID="rdoUseFor" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                    OnSelectedIndexChanged="rdoUseFor_SelectedIndexChanged">
                    <asp:ListItem Text="Brand" Value="I" Selected="True" />
                    <asp:ListItem Text="Generic" Value="G" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td width="100px">
                <asp:Label ID="lblName" runat="server" Text="Brand Name" SkinID="label" />
                <span style="color: #CC0000">*</span>
            </td>
            <td>
                <telerik:RadComboBox ID="cboItem" runat="server" Width="400px" Height="400px" EnableLoadOnDemand="true"
                    ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="[ Search items by typing minimum 2 characters ]"
                    DropDownWidth="400px" OnClientSelectedIndexChanged="cboItem_OnClientSelectedIndexChanged"
                    OnItemsRequested="cboItem_ItemsRequested" ShowMoreResultsBox="true" EnableVirtualScrolling="true">
                    <HeaderTemplate>
                        <table style="width: 99%" cellspacing="2" cellpadding="0">
                            <tr>
                                <td style="width: 115px" align="left">
                                    Item #
                                </td>
                                <td align="left">
                                    Item Name
                                </td>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="width: 99%" cellspacing="2" cellpadding="0">
                            <tr>
                                <td style="width: 115px" align="left">
                                    <%# DataBinder.Eval(Container,"Attributes['ItemNo']" )%>
                                </td>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                <asp:CustomValidator ID="cvRequiredBrandName" runat="server" Display="None" ValidationGroup="CIMSTagging"
                    ClientValidationFunction="ValidateRequiredBrandName" />
                <asp:HiddenField ID="hdnItemId" runat="server" />
                <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                <asp:Button ID="btnbrandselect" runat="server" Text="Select" SkinID="button" Style="visibility: hidden;"
                    OnClick="btnbrandselect_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <asp:Label ID="lblcimsdetail" runat="server" SkinID="label" />
                        </td>
                        <td>
                            <asp:Label ID="lblcimsdetails" runat="server" SkinID="label" ForeColor="Navy" Font-Bold="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="pnlSelectBrand" runat="server">
            <td colspan="2">
                <asp:RadioButtonList ID="rdoBrandType" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rdoBrandType_SelectedIndexChanged">
                    <asp:ListItem Text="CIMS Brand" Value="I" Selected="True" />
                    <asp:ListItem Text="CIMS Generic" Value="G" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td width="100px">
                <asp:Label ID="lblCIMSName" runat="server" Text="CIMS Brand" SkinID="label" />
            </td>
            <td>
                <telerik:RadComboBox runat="server" ID="RadCmbItemSearch" Width="400px" Height="370px"
                    EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true" EmptyMessage="[ Search items by typing minimum 1 characters ]"
                    DropDownWidth="400px" OnClientSelectedIndexChanged="OnClientSelectedIndexChangedEventHandler"
                    OnItemsRequested="RadComboBoxProduct_ItemsRequested" ShowMoreResultsBox="true"
                    EnableVirtualScrolling="true">
                    <ItemTemplate>
                        <table style="width: 100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%>
                                </td>
                                <td align="left">
                                    <%# DataBinder.Eval(Container, "Text")%>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </telerik:RadComboBox>
                <asp:HiddenField ID="hdnItemDetailId" runat="server" />
                <asp:Button ID="btnShowData" runat="server" Text="Show" SkinID="button" OnClick="btnShowData_OnClick" />
                <asp:Button ID="btnItemSelect" runat="server" Text="Show" SkinID="button" OnClick="btnItemSelect_Click"
                    Style="visibility: hidden;" />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td width="98%" align="center">
                <div style="max-height: 330px; width: 100%; overflow: auto;">
                    <asp:GridView ID="gvItemDetail" TabIndex="3" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="No Record Found" DataKeyNames="CIMSItemId" SkinID="gridview2"
                        Width="100%" OnRowCommand="gvItemDetail_RowCommand">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="82%" ItemStyle-VerticalAlign="Top" HeaderText="CIMS Desc.">
                                <ItemTemplate>
                                    <asp:Label ID="lblcimsitemdesc" runat="server" SkinID="label" Text='<%# Eval("CIMSItemDesc") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Brand Details" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnBrandDetailsCIMS" runat="server" ToolTip="Click here to view vidal brand details"
                                        CommandName="BrandDetailsCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                        Text="Brand Details" Visible='<%# Eval("CIMSItemDesc")==""?false:true %>' />
                                    <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSTYPE") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkselect" runat="server" Text="Select" Visible='<%# Eval("CIMSItemDesc")==""?false:true %>'
                                        CommandName="_SelectItem" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'></asp:LinkButton>
                                    <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%# Eval("CIMSItemId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
