<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="TemplateNotesPrint1.aspx.cs" Inherits="EMR_Templates_TemplateNotesPrint" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

       <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>

    <script type="text/javascript">
        window.onbeforeunload = function (evt) {
            var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            if (IsUnsave == 1) {
                return false;
            }
        }
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr class="clsheader">
                    <td id="tdHeader" align="left" style="padding-left: 10px; width: 160px" runat="server">
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Print Form" Font-Bold="true" />
                    </td>
                    <td align="right">
                        <asp:Button ID="btnClose" Visible="false" runat="server" Text="Close" SkinID="Button"
                            OnClientClick="window.close();" />
                        <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                    </td>
                    <td>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow2" OpenerElementID="btnPrintPdf" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <asplUD:UserDetails ID="asplUD" runat="server" />
                    </div>
                </div>
            </div>
           <%-- <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                    </td>
                </tr>
            </table>--%>
            <table width="100%">
                <tr>
                    <td align="center" style="font-size: 12px;">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table border="0" width="100%" cellpadding="2" cellspacing="1" align="center">
                <tr>
                    <td align="center" style="width: 550px">
                        <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                        <table runat="server" border="0" cellpadding="1" cellspacing="1" width="550px">
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label runat="server" Text="Template Type" SkinID="label" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlTemplateTypeCode" Width="200px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTemplateTypeCode_OnSelectedIndexChanged" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Consent Forms" Value="CF" Selected="True" />
                                            <telerik:RadComboBoxItem Text="Forms" Value="FS" />
                                            <telerik:RadComboBoxItem Text="Instructions" Value="IN" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top" style="width: 550px">
                        <table id="tblAll" runat="server" border="0" cellpadding="2" cellspacing="1" width="550px">
                            <tr>
                                <td colspan="2" class="clsheader" valign="middle" style="height: 20px;">
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="All Template(s)" />
                                    <telerik:RadComboBox ID="ddlTempGroup" Visible="false" Width="250px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTempGroup_OnSelectedIndexChanged" runat="server">
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAllTemplateSearch">
                                        <asp:TextBox ID="txtAllTemplateSearch" runat="server" SkinID="textbox" MaxLength="50"
                                            Width="100%" />
                                    </asp:Panel>
                                </td>
                                <td style="width: 65px" align="right">
                                    <asp:Button ID="btnAllTemplateSearch" runat="server" Text="Search"
                                        OnClick="btnAllTemplateSearch_OnClick"  CssClass="btn btn-primary" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%" Height="430px"
                                        ScrollBars="Auto">
                                        <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                        <asp:GridView ID="gvAllTemplate" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="True" PageSize="15" OnRowDataBound="gvAllTemplate_OnRowDataBound"
                                            OnRowCommand="gvAllTemplate_RowCommand" OnPageIndexChanging="gvAllTemplate_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Template Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName") %>' />
                                                        <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId") %>' />
                                                        <%--<asp:HiddenField ID="hdnTemplateTypeID" runat="server" Value='<%#Eval("TemplateTypeID") %>' />
                                                            <asp:HiddenField ID="hdnTemplateTypeCode" runat="server" Value='<%#Eval("TemplateTypeCode") %>' />
                                                            <asp:HiddenField ID="hdnEntryType" runat="server" Value='<%#Eval("EntryType") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Print" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnPrint" runat="server" SkinID="label" CommandName="PRINT" CommandArgument='<%#Eval("TemplateId") %>'
                                                            Text="Print" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
