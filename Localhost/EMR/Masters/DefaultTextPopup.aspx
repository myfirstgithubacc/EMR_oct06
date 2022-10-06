<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/BlankMaster.master" CodeFile="DefaultTextPopup.aspx.cs" Inherits="EMR_Masters_DefaultTextPopup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

       <%-- <asp:ScriptManager ID="_ScriptManager" runat="server" EnablePageMethods="true" />--%>
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />


         <asp:UpdatePanel ID="UpdatePanel6"  runat="server">
                <ContentTemplate>
                    <div class="container-fluid header_main  margin_bottom"  runat="server">
                        <div class="col-sm-3">
                            <h2>
                                <asp:Label ID="lblHeader" runat="server" SkinID="label" ToolTip="Default Text"
                                    Text="Default Text" Font-Bold="true" /></h2>
                        </div>
                        <div class="col-sm-5">
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="relativ alert_new text-center text-success" />
                        </div>
                        <div class="col-sm-3 text-right pull-right">
                                <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                CssClass="btn btn-primary"  Text="Save" />
                            <asp:Button ID="btnClose" runat="server"  Text="Close"  CssClass="btn btn-primary"
                                OnClientClick="window.close();" />
                        </div>

                    </div>

                    <div class="col-sm-12  form-group">
                        <div class="col-sm-2">
                            <asp:Label ID="lblDefaultText" runat="server" SkinID="label" Text="Default Text" />
                        </div>
                        <div class="col-sm-10">
                            <asp:TextBox ID="txtDefaultText" runat="server" Text=""  TextMode="MultiLine" Height="250PX" />
                        </div>
                    </div>
                   
                </ContentTemplate>
                 
            </asp:UpdatePanel>

     <%--   <asp:UpdatePanel  runat="server">
            <ContentTemplate>
                <table>

                    <tr>
                        <td>
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" ToolTip="Default text"
                                Text="Default text" Font-Bold="true" />
                        </td>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="relativ alert_new text-center text-success" />

                        </td>
                        <td>

                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                Text="Save" />
                            <asp:Button ID="btnClose" runat="server" SkinID="button" Text="Close"
                                OnClientClick="window.close();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblDefaultText" runat="server" SkinID="label" Text="Default Text" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:TextBox ID="txtDefaultText" runat="server" Text="" AutoPostBack="true" TextMode="MultiLine" Height="400PX" />

                        </td>
                    </tr>

                </table>

            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSaveData" />
            </Triggers>
        </asp:UpdatePanel>--%>
       
  </asp:Content>