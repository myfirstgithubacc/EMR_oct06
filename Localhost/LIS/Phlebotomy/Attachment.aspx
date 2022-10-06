<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Attachment.aspx.cs" Inherits="LIS_Phlebotomy_Attachment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>External Report Attachment</title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main form-group">
            <div class="col-xs-12">
                <asp:Label ID="lblSource" runat="server"></asp:Label>
                <asp:Label ID="lblPatientDetails" runat="server"></asp:Label>
            </div>
        </div>

        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-12 text-center"><asp:Label ID="lblMessage" runat="server"></asp:Label></div>
            </div>
        </div>
        
        <asp:Panel ID="pnlExternalReport" runat="server" BorderColor="#77c9e0" BorderWidth="1"
            BorderStyle="Solid" Width="100%" ScrollBars="None">
                
            <div class="container-fluid header_main">
                <div class="col-xs-12">
                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Investigation: " Font-Bold="true" />&nbsp;
                    <asp:Label ID="lblInvestigation" runat="server" SkinID="label" />&nbsp;
                    <asp:Label ID="lblRefServiceCode" runat="server" SkinID="label" />
                </div>
            </div>

            <div class="container-fluid">
                <div class="row form-groupTop01">
                    <div class="col-xs-3"><asp:Label ID="Label17" runat="server" Text="External Center" /></div>
                    <div class="col-xs-9"><asp:Label ID="lblExternalName" runat="server" Font-Bold="True" /></div>
                </div>

                <div class="row form-groupTop01">
                    <div class="col-xs-3 label2">
                        <asp:Label ID="Label14" runat="server" Text="File Name" />
                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtFileName" ErrorMessage="Required Field - File Name" SetFocusOnError="true" Text="*"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-xs-9"><asp:TextBox ID="txtFileName" runat="server" Width="100%" /></div>
                </div>

                <div class="row form-groupTop01">
                    <div class="col-xs-3 label2"><asp:Label ID="Label16" runat="server" Text="Select File" /></div>
                    <div class="col-xs-9"><asp:FileUpload ID="fUpload2" runat="server" Height="22px" Width="200px" /></div>
                </div>

                <div class="row form-groupTop01">
                    <div class="col-xs-3 label2"><asp:Label ID="Label15" runat="server" Text="Uploaded File Name" /></div>
                    <div class="col-xs-6"><telerik:RadComboBox ID="ddlfilename" runat="server" AutoPostBack="true" Width="100%" CausesValidation="false" OnSelectedIndexChanged="ddlfilename_SelectedIndexChanged"></telerik:RadComboBox></div>
                    <div class="col-xs-3 text-right PaddingLeftSpacing">
                        <%--<img id="imgDownload" runat="server" alt="" src="../../Images/d_arrow.gif" />--%>
                        <asp:LinkButton ID="lbtnDownload" runat="server" Text="Download" Font-Size="8pt" CssClass="btn btn-primary" OnClick="lbtnDownload_OnClick" CausesValidation="false"></asp:LinkButton>
                        <%--<img id="img1" runat="server" alt="" src="../../Images/d_arrow.gif" />   --%>
                        <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="btn btn-primary" Font-Size="8pt" Text="Delete" OnClick="lbtnDelete_OnClick" />
                    </div>
                </div>

                <div class="row form-groupTop01">
                    <div class="col-xs-12 label2"><asp:TextBox ID="txtDocumentId" Width="5px" Text="0" runat="server" Style="visibility: hidden; height:1px; float:left;" /></div>
                </div>

            </div>
        </asp:Panel>
       
        <div class="container-fluid margin_Top">
            <div class="row">
                <div class="col-xs-12 text-center">
                    <asp:Button ID="cmdUpload" runat="server" Text="Upload And Save" CssClass="btn btn-primary" OnClick="cmdUpload_OnClick" />
                    <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-default" OnClientClick="window.close();" />
                    <asp:ValidationSummary ID="vs1" runat="server" ShowMessageBox="false" ShowSummary="true" /> 
                </div>
            </div>
        </div>

    </form>
</body>
</html>