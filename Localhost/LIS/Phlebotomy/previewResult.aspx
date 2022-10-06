<%@ Page Language="C#" Title="Lab Result" AutoEventWireup="true" CodeFile="previewResult.aspx.cs" Inherits="LIS_Phlebotomy_previewResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" cellpadding="1" cellspacing="1" width="100%">
            <tr height="25px" align="right">
                <td class="clssubtopic">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblServiceName" runat="server"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                    OnClientClick="window.close();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <div align="left" style="padding-left: 20px">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="rwPrintLabReport" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadComboBox ID="ddlResultId" runat="server" MarkFirstMatch="true" CausesValidation="false"
                AutoPostBack="true" Width="50px" Skin="Outlook" Visible="false">
            </telerik:RadComboBox>
            <asp:GridView ID="gvSelectedServices" GridLines="None" runat="server" ShowHeader="false"
                ShowFooter="false" HeaderStyle-Wrap="false" AutoGenerateColumns="False" Width="99%"
                AllowPaging="false" PagerSettings-Visible="true" PagerSettings-Mode="NumericFirstLast"
                OnRowDataBound="gvSelectedServices_RowDataBound">
                <EmptyDataTemplate>
                    No Data Found.
                </EmptyDataTemplate>
                <RowStyle Font-Size="9pt" />
                <Columns>
                    <asp:BoundField DataField="FieldID" HeaderText="ID" ItemStyle-Width="0%" HeaderStyle-Width="0%" />
                    <asp:TemplateField HeaderText="Property Name" HeaderStyle-Width="25%" ItemStyle-Width="25%"
                        HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblFieldName" runat="server" Width="200px" Text='<%#Eval("FieldName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FieldType" HeaderText="PropertyType" ItemStyle-Width="0%"
                        HeaderStyle-Width="0%" />
                    <asp:TemplateField HeaderText="Values" ItemStyle-Width="70%" HeaderStyle-Width="70%"
                        HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <table id="tbl1" cellpadding="2" cellspacing="0" border="0" runat="server" width="100%">
                                <tr align="left">
                                    <td width="100%">
                                        <asp:TextBox ID="txtT" SkinID="textbox" Columns='<%#Convert.ToInt32(Eval("MaxLength"))%>'
                                            Visible="false" MaxLength='<%#Convert.ToInt32(Eval("MaxLength"))%>' runat="server"
                                            Height="16" />
                                        <telerik:RadComboBox ID="ddlMultilineFormats" runat="server" Visible="false" AutoPostBack="true"
                                            Width="200px" Skin="Outlook" AppendDataBoundItems="true" MarkFirstMatch="true">
                                        </telerik:RadComboBox>
                                        <asp:Label ID="lblM" runat="server" SkinID="label" Height="16px" Visible="false" />
                                        <asp:TextBox ID="txtM" SkinID="textbox" runat="server" Width="550px" Height="60px"
                                            MaxLength="250" onkeyup="return maxLength(this,250);" TextMode="MultiLine" Visible="false" />
                                        <telerik:RadComboBox ID="ddlTemplateFieldFormats" runat="server" Visible="false"
                                            AutoPostBack="true" Width="200px" Skin="Outlook" AppendDataBoundItems="true"
                                            MarkFirstMatch="true">
                                        </telerik:RadComboBox>
                                        <telerik:RadEditor ID="txtW" runat="server" EditModes="Design" EnableEmbeddedSkins="true" ContentAreaMode="Div"
                                            EnableResize="true" Skin="Vista" Height="360px" Width="700px" ToolbarMode="ShowOnFocus"
                                            ToolsFile="~/Include/XML/EditorTools.xml" OnClientSelectionChange="OnClientSelectionChange"
                                            BorderColor="LightBlue" BorderWidth="1" BorderStyle="Solid"
                                            Style="overflow: auto; overflow-x: hidden; overflow-y: scroll;">
                                            <Modules>
                                                <telerik:EditorModule Visible="false" />
                                            </Modules>
                                        </telerik:RadEditor>
                                        <asp:TextBox ID="txtF" SkinID="textbox" Columns="20" MaxLength="20" Visible="false"
                                            runat="server" Height="16" ReadOnly="true" />
                                        <asp:TextBox ID="TxtFRemarks" SkinID="textbox" Columns='<%#Convert.ToInt32(Eval("MaxLength"))%>'
                                            MaxLength="20" Visible="false" Width="60px" runat="server" Height="16" />
                                        <asp:Button ID="btnF" SkinID="Button" runat="server" Text="Calculate" Visible="false" />
                                        <asp:Label ID="lblF" runat="server" SkinID="label" Height="16px" Visible="false" />
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtN" SkinID="textbox" Columns="20" Visible="false" MaxLength="20"
                                                        runat="server" Height="16px" />
                                                    <asp:Label ID="lblN" runat="server" SkinID="label" Height="16px" Visible="false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLocation" runat="server" Visible="false" SkinID="label" Text="Machine"></asp:Label>
                                                    <asp:TextBox ID="txtFinalizedDislpayMachine" runat="server" Visible="false"></asp:TextBox>
                                                    <telerik:RadComboBox ID="ddlRange" runat="server" Visible="false" AutoPostBack="true"
                                                        Width="130px" Skin="Outlook" AppendDataBoundItems="true" MarkFirstMatch="true"
                                                        EnableEmbeddedSkins="false">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updFU">
                                                                                <ContentTemplate>
                                                                                    <div class="row">
                                                                                        <div class="col-md-3">
                                                                                            <asp:FileUpload ID="iFileUploader" runat="server" Visible="false" />
                                                                                            <asp:HiddenField ID="hdnFileAddress" runat="server" />
                                                                                            <asp:HiddenField ID="hdnFileName" runat="server" />

                                                                                        </div>
                                                                                        <div class="col-md-5">
                                                                                            <asp:Button ID="ibtnUpload" runat="server"  Text="Upload"
                                                                                                Visible="false" />
                                                                                            <asp:Label runat="server" ID="lblStatus" Visible="false" Text="" />
                                                                                            <asp:ImageButton ID="imgRIS" runat="server" OnClick="RenderAttachedImage" ImageUrl="~/Icons/RIS.jpg" BorderWidth="1"
                                                                                                BorderColor="Gray" Width="25px" Height="25px" Visible="false" />
                                                                                            <asp:Button ID="btnRemoveimage" runat="server" Text="Remove Image" CssClass="btn btn-primary"
                                                                                                Visible="false" />
                                                                                            <asp:HiddenField ID="HiddenField1" runat="server" />
                                                                                        </div>
                                                                                    </div>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:PostBackTrigger ControlID="ibtnUpload" />
                                                                                    <%--<asp:PostBackTrigger ControlID="imgRIS" />--%>
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnRemoveimage" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                            FilterType="Custom" TargetControlID="txtN" ValidChars="0123456789.=><+-" />
                                        <asp:HiddenField ID="hdnUnitName" runat="server" Value='<%#Eval("UnitName")%>' />
                                        <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId")%>' />
                                        <asp:HiddenField ID="hdnMinValue" runat="server" Value='<%#Eval("MinValue")%>' />
                                        <asp:HiddenField ID="hdnMaxValue" runat="server" Value='<%#Eval("MaxValue")%>' />
                                        <asp:HiddenField ID="hdnSymbol" runat="server" Value='<%#Eval("Symbol")%>' />
                                        <asp:HiddenField ID="hdnMinPanicValue" runat="server" Value='<%#Eval("MinPanicValue")%>' />
                                        <asp:HiddenField ID="hdnMaxPanicValue" runat="server" Value='<%#Eval("MaxPanicValue")%>' />
                                    </td>
                                </tr>
                            </table>
                            <asp:TextBox ID="txtFinalizedDislpay" runat="server"  Width="80%" Height="80%" Visible="false"></asp:TextBox>
                            <telerik:RadComboBox ID="D" runat="server" AutoPostBack="true" Visible="false" Width="200px"
                                Skin="Outlook" AppendDataBoundItems="true" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                </Items>
                            </telerik:RadComboBox>
                            <telerik:RadComboBox ID="O" runat="server" Visible="false" Width="200px" Skin="Outlook"
                                AppendDataBoundItems="true" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                </Items>
                            </telerik:RadComboBox>
                            <table id="tblOrganism" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <telerik:RadComboBox ID="ddlMultilineFormatsOrganism" runat="server" AutoPostBack="true"
                                            Width="200px" Skin="Outlook" AppendDataBoundItems="true" Visible="false" MarkFirstMatch="true"
                                            BorderColor="Blue" BorderStyle="Solid" BorderWidth="1">
                                        </telerik:RadComboBox>
                                        <asp:Label ID="lblMultilineOrg" runat="server" SkinID="label" Height="16px" />
                                        <asp:TextBox ID="txtMOrg" SkinID="textbox" runat="server" Width="550px" Height="60px"
                                            onkeyup="return maxLength(this,500);" TextMode="MultiLine" Visible="false" BorderColor="Blue"
                                            BorderStyle="Solid" BorderWidth="1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                            <telerik:RadComboBox ID="E" runat="server" Visible="false" Width="200px" Skin="Outlook"
                                AppendDataBoundItems="true" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:Button ID="btnSN" SkinID="Button" runat="server" Text="Sensitivity" Visible="false"
                                OnClick="btnSN_Click" />
                            <asp:Repeater ID="C" runat="server" Visible="false">
                                <HeaderTemplate>
                                    <table width="100%" cellpadding="2" cellspacing="0">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr align="left">
                                        <td valign="top">
                                            <asp:HiddenField ID="hdnCV" runat="server" Value='<%#Eval("ValueId")%>' />
                                            <asp:CheckBox ID="C" SkinID="checkbox" Font-Size="10pt" runat="server" Text=' <%#Eval("ValueName")%>' />
                                        </td>
                                        <td align="right" visible="false" valign="top">
                                            <textarea id="CT" class="Textbox" visible="false" runat="server" onkeypress="AutoChange()"
                                                rows="1" cols="40" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <telerik:RadComboBox ID="B" runat="server" Visible="false" AutoPostBack="true" Width="200px"
                                Skin="Outlook" AppendDataBoundItems="true" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="-1" Text="[Select]" />
                                    <telerik:RadComboBoxItem Value="0" Text="No" />
                                    <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                </Items>
                            </telerik:RadComboBox>
                            <asp:Label ID="lblD" runat="server" SkinID="label" Height="16px" Visible="false" />
                            <table id="tblDate" runat="server" visible="false" cellpadding="2" cellspacing="0">
                                <tr align="left">
                                    <td>
                                        <asp:TextBox ID="txtDate" SkinID="textbox" Font-Size="13px" Text="" Width="64px"
                                            runat="server" />
                                    </td>
                                    <td>
                                        <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                            vspace="0" border="0" id="imgFromDate" runat="server" />
                                    </td>
                                    <td>
                                        <AJAX:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate"
                                            PopupButtonID="imgFromDate" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="_/" />
                                        <AJAX:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                            CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                            Enabled="True" TargetControlID="txtDate" MessageValidatorTip="false" AcceptAMPM="true"
                                            AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                            ErrorTooltipEnabled="false" InputDirection="LeftToRight" />
                                    </td>
                                </tr>
                            </table>
                            <table id="Tabletm" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <telerik:RadComboBox ID="ddlHr" runat="server" Width="40px" Skin="Outlook" AppendDataBoundItems="true"
                                            MarkFirstMatch="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="" />
                                                <telerik:RadComboBoxItem Value="1" Text="1" />
                                                <telerik:RadComboBoxItem Value="2" Text="2" />
                                                <telerik:RadComboBoxItem Value="3" Text="3" />
                                                <telerik:RadComboBoxItem Value="4" Text="4" />
                                                <telerik:RadComboBoxItem Value="5" Text="5" />
                                                <telerik:RadComboBoxItem Value="6" Text="6" />
                                                <telerik:RadComboBoxItem Value="7" Text="7" />
                                                <telerik:RadComboBoxItem Value="8" Text="8" />
                                                <telerik:RadComboBoxItem Value="9" Text="9" />
                                                <telerik:RadComboBoxItem Value="10" Text="10" />
                                                <telerik:RadComboBoxItem Value="11" Text="11" />
                                                <telerik:RadComboBoxItem Value="12" Text="12" />
                                                <telerik:RadComboBoxItem Value="13" Text="13" />
                                                <telerik:RadComboBoxItem Value="14" Text="14" />
                                                <telerik:RadComboBoxItem Value="15" Text="15" />
                                                <telerik:RadComboBoxItem Value="16" Text="16" />
                                                <telerik:RadComboBoxItem Value="17" Text="17" />
                                                <telerik:RadComboBoxItem Value="18" Text="18" />
                                                <telerik:RadComboBoxItem Value="19" Text="19" />
                                                <telerik:RadComboBoxItem Value="20" Text="20" />
                                                <telerik:RadComboBoxItem Value="21" Text="21" />
                                                <telerik:RadComboBoxItem Value="22" Text="22" />
                                                <telerik:RadComboBoxItem Value="23" Text="23" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:Label ID="Label2" runat="server" SkinID="label" Width="20px" Text="Hr" />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlMin" runat="server" Width="40px" Skin="Outlook" AppendDataBoundItems="true"
                                            MarkFirstMatch="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="" />
                                                <telerik:RadComboBoxItem Value="1" Text="1" />
                                                <telerik:RadComboBoxItem Value="2" Text="2" />
                                                <telerik:RadComboBoxItem Value="3" Text="3" />
                                                <telerik:RadComboBoxItem Value="4" Text="4" />
                                                <telerik:RadComboBoxItem Value="5" Text="5" />
                                                <telerik:RadComboBoxItem Value="6" Text="6" />
                                                <telerik:RadComboBoxItem Value="7" Text="7" />
                                                <telerik:RadComboBoxItem Value="8" Text="8" />
                                                <telerik:RadComboBoxItem Value="9" Text="9" />
                                                <telerik:RadComboBoxItem Value="10" Text="10" />
                                                <telerik:RadComboBoxItem Value="11" Text="11" />
                                                <telerik:RadComboBoxItem Value="12" Text="12" />
                                                <telerik:RadComboBoxItem Value="13" Text="13" />
                                                <telerik:RadComboBoxItem Value="14" Text="14" />
                                                <telerik:RadComboBoxItem Value="15" Text="15" />
                                                <telerik:RadComboBoxItem Value="16" Text="16" />
                                                <telerik:RadComboBoxItem Value="17" Text="17" />
                                                <telerik:RadComboBoxItem Value="18" Text="18" />
                                                <telerik:RadComboBoxItem Value="19" Text="19" />
                                                <telerik:RadComboBoxItem Value="20" Text="20" />
                                                <telerik:RadComboBoxItem Value="21" Text="21" />
                                                <telerik:RadComboBoxItem Value="22" Text="22" />
                                                <telerik:RadComboBoxItem Value="23" Text="23" />
                                                <telerik:RadComboBoxItem Value="24" Text="24" />
                                                <telerik:RadComboBoxItem Value="25" Text="25" />
                                                <telerik:RadComboBoxItem Value="26" Text="26" />
                                                <telerik:RadComboBoxItem Value="27" Text="27" />
                                                <telerik:RadComboBoxItem Value="28" Text="28" />
                                                <telerik:RadComboBoxItem Value="29" Text="29" />
                                                <telerik:RadComboBoxItem Value="30" Text="30" />
                                                <telerik:RadComboBoxItem Value="31" Text="31" />
                                                <telerik:RadComboBoxItem Value="32" Text="32" />
                                                <telerik:RadComboBoxItem Value="33" Text="33" />
                                                <telerik:RadComboBoxItem Value="34" Text="34" />
                                                <telerik:RadComboBoxItem Value="35" Text="35" />
                                                <telerik:RadComboBoxItem Value="36" Text="36" />
                                                <telerik:RadComboBoxItem Value="37" Text="37" />
                                                <telerik:RadComboBoxItem Value="38" Text="38" />
                                                <telerik:RadComboBoxItem Value="39" Text="39" />
                                                <telerik:RadComboBoxItem Value="40" Text="40" />
                                                <telerik:RadComboBoxItem Value="41" Text="41" />
                                                <telerik:RadComboBoxItem Value="42" Text="42" />
                                                <telerik:RadComboBoxItem Value="43" Text="43" />
                                                <telerik:RadComboBoxItem Value="44" Text="44" />
                                                <telerik:RadComboBoxItem Value="45" Text="45" />
                                                <telerik:RadComboBoxItem Value="46" Text="46" />
                                                <telerik:RadComboBoxItem Value="47" Text="47" />
                                                <telerik:RadComboBoxItem Value="48" Text="48" />
                                                <telerik:RadComboBoxItem Value="49" Text="49" />
                                                <telerik:RadComboBoxItem Value="50" Text="50" />
                                                <telerik:RadComboBoxItem Value="51" Text="51" />
                                                <telerik:RadComboBoxItem Value="52" Text="52" />
                                                <telerik:RadComboBoxItem Value="53" Text="53" />
                                                <telerik:RadComboBoxItem Value="54" Text="54" />
                                                <telerik:RadComboBoxItem Value="55" Text="55" />
                                                <telerik:RadComboBoxItem Value="56" Text="56" />
                                                <telerik:RadComboBoxItem Value="57" Text="57" />
                                                <telerik:RadComboBoxItem Value="58" Text="58" />
                                                <telerik:RadComboBoxItem Value="59" Text="59" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:Label ID="Label14" runat="server" SkinID="label" Text="Min" />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlSec" runat="server" Width="40px" Skin="Outlook" AppendDataBoundItems="true"
                                            MarkFirstMatch="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="" />
                                                <telerik:RadComboBoxItem Value="1" Text="1" />
                                                <telerik:RadComboBoxItem Value="2" Text="2" />
                                                <telerik:RadComboBoxItem Value="3" Text="3" />
                                                <telerik:RadComboBoxItem Value="4" Text="4" />
                                                <telerik:RadComboBoxItem Value="5" Text="5" />
                                                <telerik:RadComboBoxItem Value="6" Text="6" />
                                                <telerik:RadComboBoxItem Value="7" Text="7" />
                                                <telerik:RadComboBoxItem Value="8" Text="8" />
                                                <telerik:RadComboBoxItem Value="9" Text="9" />
                                                <telerik:RadComboBoxItem Value="10" Text="10" />
                                                <telerik:RadComboBoxItem Value="11" Text="11" />
                                                <telerik:RadComboBoxItem Value="12" Text="12" />
                                                <telerik:RadComboBoxItem Value="13" Text="13" />
                                                <telerik:RadComboBoxItem Value="14" Text="14" />
                                                <telerik:RadComboBoxItem Value="15" Text="15" />
                                                <telerik:RadComboBoxItem Value="16" Text="16" />
                                                <telerik:RadComboBoxItem Value="17" Text="17" />
                                                <telerik:RadComboBoxItem Value="18" Text="18" />
                                                <telerik:RadComboBoxItem Value="19" Text="19" />
                                                <telerik:RadComboBoxItem Value="20" Text="20" />
                                                <telerik:RadComboBoxItem Value="21" Text="21" />
                                                <telerik:RadComboBoxItem Value="22" Text="22" />
                                                <telerik:RadComboBoxItem Value="23" Text="23" />
                                                <telerik:RadComboBoxItem Value="24" Text="24" />
                                                <telerik:RadComboBoxItem Value="25" Text="25" />
                                                <telerik:RadComboBoxItem Value="26" Text="26" />
                                                <telerik:RadComboBoxItem Value="27" Text="27" />
                                                <telerik:RadComboBoxItem Value="28" Text="28" />
                                                <telerik:RadComboBoxItem Value="29" Text="29" />
                                                <telerik:RadComboBoxItem Value="30" Text="30" />
                                                <telerik:RadComboBoxItem Value="31" Text="31" />
                                                <telerik:RadComboBoxItem Value="32" Text="32" />
                                                <telerik:RadComboBoxItem Value="33" Text="33" />
                                                <telerik:RadComboBoxItem Value="34" Text="34" />
                                                <telerik:RadComboBoxItem Value="35" Text="35" />
                                                <telerik:RadComboBoxItem Value="36" Text="36" />
                                                <telerik:RadComboBoxItem Value="37" Text="37" />
                                                <telerik:RadComboBoxItem Value="38" Text="38" />
                                                <telerik:RadComboBoxItem Value="39" Text="39" />
                                                <telerik:RadComboBoxItem Value="40" Text="40" />
                                                <telerik:RadComboBoxItem Value="41" Text="41" />
                                                <telerik:RadComboBoxItem Value="42" Text="42" />
                                                <telerik:RadComboBoxItem Value="43" Text="43" />
                                                <telerik:RadComboBoxItem Value="44" Text="44" />
                                                <telerik:RadComboBoxItem Value="45" Text="45" />
                                                <telerik:RadComboBoxItem Value="46" Text="46" />
                                                <telerik:RadComboBoxItem Value="47" Text="47" />
                                                <telerik:RadComboBoxItem Value="48" Text="48" />
                                                <telerik:RadComboBoxItem Value="49" Text="49" />
                                                <telerik:RadComboBoxItem Value="50" Text="50" />
                                                <telerik:RadComboBoxItem Value="51" Text="51" />
                                                <telerik:RadComboBoxItem Value="52" Text="52" />
                                                <telerik:RadComboBoxItem Value="53" Text="53" />
                                                <telerik:RadComboBoxItem Value="54" Text="54" />
                                                <telerik:RadComboBoxItem Value="55" Text="55" />
                                                <telerik:RadComboBoxItem Value="56" Text="56" />
                                                <telerik:RadComboBoxItem Value="57" Text="57" />
                                                <telerik:RadComboBoxItem Value="58" Text="58" />
                                                <telerik:RadComboBoxItem Value="59" Text="59" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <asp:Label ID="Label7" runat="server" SkinID="label" Text="Sec" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTimeString" runat="server" SkinID="label" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks" Visible="false" ItemStyle-VerticalAlign="Top"
                        ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <textarea id="txtRemarks" class="Textbox" runat="server" onkeypress="AutoChange()"
                                rows="1" cols="40" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ServiceId" HeaderText="Service ID" ItemStyle-Width="0%"
                        HeaderStyle-Width="0%" />
                </Columns>
            </asp:GridView>
            <asp:Literal ID="litResult" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
