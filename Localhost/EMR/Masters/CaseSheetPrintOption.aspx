<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="CaseSheetPrintOption.aspx.cs" Inherits="EMR_Masters_CaseSheetPrintOption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" class="clsheader">
        <tr>
            <td>
                Casesheet Print Group Option
            </td>
            <td align="right">
         <%--       <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPrintGroup" />
                        <asp:AsyncPostBackTrigger ControlID="btnGetDetails" />
                    </Triggers>
                    <ContentTemplate>--%>
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" ValidationGroup="CreateGroup"
                            ToolTip="Save Letter" Text="Save" SkinID="Button" />
                        <asp:Button ID="btnEdit" runat="server" SkinID="Button" Text="Edit" OnClick="btnEdit_Click" />    
                        <asp:Button ID="btnNew" runat="server" SkinID="Button" Text="New" OnClick="btnNew_Click" />
                        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" ValidationGroup="CreateGroup"
                            ToolTip="Update" Text="Update" SkinID="Button" />
                   <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
            <td>
                <asp:Label ID="lblPatientInfo" runat="server" SkinID="label" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="400" cellpadding="3" cellspacing="3">
        <tr>
            <td>
                <asp:Label ID="Label2" Text="Print Group" SkinID="label" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlPrintGroup" runat="server" SkinID="DropDown"></asp:DropDownList><asp:TextBox ID="txtPrintGroup" runat="server" SkinID="textbox" MaxLength="50"></asp:TextBox>&nbsp;<asp:Button
                    ID="btnGetDetails" runat="server" Text="Get Details" OnClick="btnGetDetails_Click"
                    SkinID="Button" />
            </td>
        </tr>
    </table>
    <table border="0">
        <tr>
            <td>
                <asp:Label ID="lblTemplateList" Text="Template(s) List" SkinID="label" runat="server" />
            </td>
            <td>
            </td>
            <td>
                <asp:Label ID="Label1" Text="Assigned Template(s)" SkinID="label" runat="server" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lstMasterTemplate" SelectionMode="Multiple" runat="server" SkinID="listbox"
                    Height="300px" Width="150px" />
            </td>
            <td style="width: 30px" align="center">
                <asp:Button ID="btnShift" Text=">>" Width="24px" Font-Bold="true" OnClick="btnShift_OnClick"
                    ToolTip="Move to" runat="server" SkinID="Button" Style="color: Black; border: 1px outset #AFAC9C;
                    font-family: Arial; font-weight: bold; font-size: 11px;" />
            </td>
            <td>
                <asp:UpdatePanel ID="updGridView" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:ListBox ID="lstTemplate" SelectionMode="Multiple" runat="server" SkinID="listbox"
                            Height="300px" Width="150px" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnShift" />
                        <asp:AsyncPostBackTrigger ControlID="lnkRemove" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td valign="top">
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <asp:Button ID="lnkRemove" Text="Remove" Width="75px" OnClick="lnkRemove_OnClick"
                                SkinID="Button" ToolTip="Click to Remove" runat="server" Style="color: Black;
                                border: 1px outset #AFAC9C; font-family: Arial; font-weight: bold; font-size: 11px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--<asp:Button ID="btnUp" Text="Move Up" Width="75px" OnClick="btnUp_OnClick" ToolTip="Click to Remove"
                        runat="server" SkinID="Button" Style="color: Black; border: 1px outset #AFAC9C;
                        font-family: Arial; font-weight: bold; font-size: 11px;" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%-- <asp:Button ID="btnDown" Text="Move Down" Width="75px" OnClick="btnDown_OnClick" SkinID="Button"
                        ToolTip="Click to Remove" runat="server" Style="color: Black; 
                        border: 1px outset #AFAC9C; font-family: Arial; font-weight: bold; font-size: 11px;" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>