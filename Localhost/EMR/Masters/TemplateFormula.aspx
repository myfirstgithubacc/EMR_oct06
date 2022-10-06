<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TemplateFormula.aspx.cs"
    Inherits="EMR_Masters_TemplateFormula" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript">        
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr class="clsheader">
                        <td id="tdHeader" align="left" style="padding-left: 10px; width: 300px" runat="server">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Template&nbsp;Formula"
                                Font-Bold="true" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                SkinID="Button" ValidationGroup="SaveData" Text="Save" />
                            &nbsp;
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                        </td>
                    </tr>
                </table>
                <table border="0" style="margin-left: 10px;" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Field(s) List" />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Formula Details [ Formula example: A*(B+C) ]" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 270px;">
                            <asp:Panel ID="Panel2" runat="server" Height="400px" ScrollBars="Auto">
                                <asp:GridView ID="gvTemplateField" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                    Width="100%" AllowPaging="false" OnRowDataBound="gvTemplateField_RowDataBound"
                                    OnRowCommand="gvTemplateField_OnRowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Field&nbsp;Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFieldName" runat="server" SkinID="label" Text='<%#Eval("FieldName") %>' />
                                                <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add to list" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSelectFieldId" runat="server" CommandName="AddToList" CommandArgument='<%#Eval("FieldId") %>'
                                                    Text="Add to list" ToolTip="Add to create formula" CausesValidation="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td style="width: 550px;">
                            <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Auto">
                                <asp:GridView ID="gvSelectedFields" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                    Width="100%" AllowPaging="false" OnRowDataBound="gvSelectedFields_RowDataBound"
                                    OnRowCommand="gvSelectedFields_OnRowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Field&nbsp;Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFieldName" runat="server" SkinID="label" Text='<%#Eval("FieldName") %>' />
                                                <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reference Name" HeaderStyle-Width="95px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReferenceName" runat="server" SkinID="label" Text='<%#Eval("ReferenceName") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Formula Definition" HeaderStyle-Width="110px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFormulaDefinition" runat="server" SkinID="textbox" Text='<%#Eval("FormulaDefinition") %>'
                                                    Width="100%" MaxLength="50" Style="text-transform: uppercase" />
                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    FilterType="Custom, LowercaseLetters, UppercaseLetters" TargetControlID="txtFormulaDefinition"
                                                    ValidChars="+-*/().0123456789" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Calc" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="55px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkTotalCalc" runat="server" SkinID="checkbox" Checked='<%#Eval("TotalCalc").ToString().Equals("1")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" CommandName="Delete1"
                                                    CommandArgument='<%#Eval("FieldId") %>' ImageUrl="/images/DeleteRow.png" ToolTip="Click here to delete a row" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
