<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VariableDose.aspx.cs" Inherits="EMR_Medication_VariableDose" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Variable Dose</title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />

        <script language="javascript" type="text/javascript">
            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.xmlVariableDoseString = document.getElementById("hdnXmlString").value;
                oArg.DoseDuration = document.getElementById("hdnvariableDoseDuration").value;
                oArg.DoseFrequency = document.getElementById("hdnvariableDoseFrequency").value;
                oArg.DoseValue = document.getElementById("hdnVariabledose").value;

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }
        </script>

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table border="0" width="100%" class="clsheader" bgcolor="#e3dcco" cellpadding="0"
                    cellspacing="0">
                    <tr>
                        <td style="width: 70%"></td>
                        <td align="right" style="width: 30%">
                            <asp:Button ID="btnSave" Text="Save" runat="server" CausesValidation="false" ToolTip="Add multiple dose  to main page..."
                                SkinID="Button" OnClick="btnSave_OnClick" />
                            <asp:Button ID="btnClose" runat="server" OnClientClick="window.close();" SkinID="Button"
                                Text="Close" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0" width="99%" cellpadding="0" cellspacing="5">
                    <tr>
                        <td>
                            <asp:Label ID="lblItemName" Font-Bold="true" runat="server" Text="" SkinID="label" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" Text="" SkinID="label" />
                        </td>
                    </tr>
                </table>
                </br>
            <table border="0" width="99%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" CellSpacing="0"
                            CellPadding="5" AllowPaging="false" SkinID="gridview" PagerSettings-Visible="true">
                            <EmptyDataTemplate>
                                No Data Found.
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" Style="text-align: left" runat="server" Text='<%#Eval("Date") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 1" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose1" runat="server" SkinID="textbox" Style="text-align: right"
                                            Autocomplete="off" Width="40px" MaxLength="5" Text='<%#Eval("Dose1") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose1" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 2" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose2" runat="server" Style="text-align: right" SkinID="textbox"
                                            Autocomplete="off" Width="40px" MaxLength="5" Text='<%#Eval("Dose2") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose2" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 3" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose3" runat="server" Style="text-align: right" SkinID="textbox"
                                            Autocomplete="off" Width="40px" MaxLength="5" Text='<%#Eval("Dose3") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose3" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 4" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose4" runat="server" Style="text-align: right" Width="40px"
                                            SkinID="textbox" Autocomplete="off" MaxLength="5" Text='<%#Eval("Dose4") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose4" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 5" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose5" runat="server" Width="40px" Style="text-align: right"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose5") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose5" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 6" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose6" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose6") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose6" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 7" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose7" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose7") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose7" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 8" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose8" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose8") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose8" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 9" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose9" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose9") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose9" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 10" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose10" runat="server" Width="40px" Style="text-align: right"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose10") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose10" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 11" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose11" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose11") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose11" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 12" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose12" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose12") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose12" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 13" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose13" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose13") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose13" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 14" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose14" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose14") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose14" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 15" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose15" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose15") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose15" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 16" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose16" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose16") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose16" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 17" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose17" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose17") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose17" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 18" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose18" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose18") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose18" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 19" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose19" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose19") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose19" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 20" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose20" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose20") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose20" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 21" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose21" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose21") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose21" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 22" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose22" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose22") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose22" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 23" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose23" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose23") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose23" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dose 24" ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDose24" runat="server" Style="text-align: right" Width="40px"
                                            Autocomplete="off" SkinID="textbox" MaxLength="5" Text='<%#Eval("Dose24") %>' />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" TargetControlID="txtDose24" ValidChars="." />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                        <asp:HiddenField ID="hdnvariableDoseDuration" runat="server" Value="" />
                        <asp:HiddenField ID="hdnvariableDoseFrequency" runat="server" Value="" />
                        <asp:HiddenField ID="hdnVariabledose" runat="server" Value="" />
                        <asp:GridView ID="gvDisplayVariableDose" runat="server" CellPadding="5" AllowPaging="false"
                            SkinID="gridview" PagerSettings-Visible="true">
                            <EmptyDataTemplate>
                                <h1>No record found.</h1>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
