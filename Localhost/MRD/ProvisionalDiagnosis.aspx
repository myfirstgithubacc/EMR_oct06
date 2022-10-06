<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ProvisionalDiagnosis.aspx.cs" Inherits="EMR_Assessment_ProvisionalDiagnosis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">

        function AutoChange() {
            var txt = $get('<%=txtProvisionalDiagnosis.ClientID%>');
            if (txt.value.length > 500) {
                alert("Text length should not be greater then 500 characters.");
                txt.value = txt.value.substring(0, 500);
                txt.focus();
            }
        }

        function OnClientClose(oWnd, args) {
            $get('<%=btnGetDiagnosisSearchCodes.ClientID%>').click();
        }
        
    </script>

    <style type="text/css">
        .Gridheader
        {
            font-family: Verdana; /*background-image: url(/Images/header.gif);*/
            height: 24px;
            color: black;
            font-weight: normal;
            position: relative;
        }
        .blink
        {
            text-decoration: blink;
        }
        .blinkNone
        {
            text-decoration: none;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
        <ContentTemplate>
            <table width="100%" class="clsheader">
                <tr>
                    <td>
                        Patient Provisional Diagnosis
                    </td>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                    </td>
                    <td align="right">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;<asp:Label ID="lblDiagnosis" runat="server" Text="Provisional Diagnosis" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProvisionalDiagnosis" runat="server" SkinID="textbox" MaxLength="100"
                            onkeyup="return AutoChange();" Width="500px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;<asp:Label ID="Label1" runat="server" SkinID="label" 
                            Text="Diagnosis Search Keyword"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlDiagnosisSearchCodes" runat="server" 
                            AutoPostBack="false" DropDownWidth="250px" Skin="Outlook" Width="250px">
                        </telerik:RadComboBox>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="height: 30px" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;<asp:Label ID="Label2" runat="server" Text="Encoded By" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEncodedBy" runat="server" SkinID="label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;<asp:Label ID="Label3" runat="server" Text="Encoded Date" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEncodedDate" runat="server" SkinID="label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px" colspan="2">
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;<asp:Label ID="Label5" runat="server" Text="Last Changed By" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLastChangedBy" runat="server" SkinID="label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        &nbsp;<asp:Label ID="Label7" runat="server" Text="Last Changed Date" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLastChangedDate" runat="server" SkinID="label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnGetDiagnosisSearchCodes" runat="server" CausesValidation="false" Style="visibility: hidden;"
                            OnClick="btnGetDiagnosisSearchCodes_Click" />
                    </td>
                </tr>
            </table>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
