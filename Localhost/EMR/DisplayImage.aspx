<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisplayImage.aspx.cs" Inherits="EMR_DisplayImage"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />

    <script src="/Include/JS/Common.js" type="text/javascript"></script>

    <title></title>
</head>
<body style="margin: 0px 0px 0px 0px;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="vertical-align: top;">
        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td>                   
                    <asp:Panel ID="pnlimg" runat="server" Height="520px" ScrollBars="Auto">
                      <iframe id="frm" runat="server" height="500px" width="90%"></iframe>
                          <asp:Image ID="imgdetail" runat="server" AlternateText="Thumbnail not available please download to view" Visible="false" />
                        <br />
                        <asp:LinkButton ID="lnkDownLoad" runat="server" OnClick="lnkDownLoad_Click" Text="Download" ForeColor="DodgerBlue"></asp:LinkButton>
                    </asp:Panel>
                    <asp:Label ID="lblmsg" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="90px">
                                Staus&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlreview" runat="server" SkinID="DropDown" Width="110px">
                                </asp:DropDownList>
                                &nbsp;&nbsp;
                            </td>
                            <td>
                                Date&nbsp;Time&nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updatepanel1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadDateTimePicker ID="RadDateTimePicker1" MinDate="01/01/1900" runat="server" AutoPostBackControl="Both"
                                            TabIndex="37" Width="170px" >
                                        </telerik:RadDateTimePicker>
                                        <telerik:RadComboBox ID="RadComboBox1" runat="server" Width="50px" OnSelectedIndexChanged="RadComboBox1_OnSelectedIndexChanged"
                                                        Skin="Outlook" AutoPostBack="True" >
                                                    </telerik:RadComboBox>
                                        <%--<telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="300px"
                                            OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Skin="Outlook" Width="50px">
                                        </telerik:RadComboBox>--%>
                                        &nbsp;<asp:Literal ID="ltDateTime" runat="server" Text="HH   MM"></asp:Literal>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top" width="90px">
                                Comments&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtremarks" TextMode="MultiLine" Rows="4" Columns="100" runat="server"
                                    MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="90px">
                                Reviewed By &nbsp;&nbsp;
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlprovider" runat="server" SkinID="DropDown" Width="200px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnUpdate" SkinID="Button" runat="server" OnClick="btnUpdate_Click"
                                    Text="Save" />
                            </td>
                            <td>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnclose" SkinID="Button" runat="server" Width="120px" Text="Close"
                                    OnClick="btnclose_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
