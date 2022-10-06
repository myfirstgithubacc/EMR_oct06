<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartmentConsumptionList.aspx.cs"
    Inherits="Pharmacy_SaleIssue_DepartmentConsumptionList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Department Cosumption</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" />

    <style type="text/css">
        input#cbShowCancelled + label {
            margin-top: 2px;
            margin-left:4px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>

        <script type="text/javascript">

            function returnToParent() {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.DocId = document.getElementById("hdnDocId").value;
                oArg.DocNo = document.getElementById("hdnDocNo").value;

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
                    <div class="container-fluid header_main">
                        <div class="row">
                            <div class="col-md-3 col-4">
                                <asp:Label ID="Label4" runat="server" SkinID="label" Font-Bold="true" Font-Size="13px" Text="Store Name :" />
                                <asp:Label ID="lblStoreName" runat="server" Font-Size="16px" SkinID="label" Font-Bold="true" Text="" ToolTip="" />
                            </div>
                            <div class="col-md-5 col-8">
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                            </div>
                            <div class="col-md-4 col-12 text-right">
                                <asp:HiddenField ID="hdnDocId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnDocNo" runat="server" Value="" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" ToolTip="Filter" Text="Filter"
                                    OnClick="btnSearch_OnClick" />
                                <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" ToolTip="Clear Filter"
                                    Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                                <asp:Button ID="btnCloseW" Text="Close" runat="server" CssClass="btn btn-primary" ToolTip="Close"
                                    OnClientClick="window.close();" />
                            </div>
                        </div>
                    </div>

                    <div class="container-fluid">
                        <div class="row mt-1">
                            <div class="col-md-3 col-6">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Label ID="label3" runat="server" Font-Bold="true" SkinID="label" ToolTip="Status"
                                            Text="<%$ Resources:PRegistration, status%>" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlStatus" Width="100%" runat="server" Skin="Office2007"
                                            EmptyMessage="[ Select ]" MarkFirstMatch="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="" Value="" Selected="true" />
                                                <telerik:RadComboBoxItem Text="Open" Value="O" />
                                                <telerik:RadComboBoxItem Text="Post" Value="P" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-md-7 col-6">
                                <div class="row">
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-lg-5 col-md-5">
                                                <asp:Label ID="Label2" runat="server" Font-Bold="true" SkinID="label" Text="From Date" />
                                            </div>
                                            <div class="col-lg-7 col-md-7">
                                                <telerik:RadDatePicker ID="dtFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-lg-5 col-md-5">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="true" SkinID="label" Text="To Date" />
                                            </div>
                                            <div class="col-lg-7 col-md-7">
                                                <telerik:RadDatePicker ID="dtToDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 ">
                                <asp:CheckBox ID="cbShowCancelled" runat="server" Checked="false" Text="Show Cancelled" />
                            </div>
                        </div>
                    </div>

                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlgrid" runat="server" Height="430px" BorderWidth="1" BorderColor="SkyBlue"
                                    ScrollBars="Auto">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gv" runat="server" SkinID="gridview" HeaderStyle-Wrap="false" AutoGenerateColumns="False"
                                                Width="100%" AllowPaging="true" PageSize="20" OnPageIndexChanging="gv_OnPageIndexChanging"
                                                OnSelectedIndexChanged="gv_SelectedIndexChanged" OnRowDataBound="gv_RowDataBound">
                                                <Columns>
                                                    <asp:CommandField ControlStyle-ForeColor="Blue" SelectText="Select" ShowSelectButton="true"
                                                        ItemStyle-Width="30px">
                                                        <ControlStyle ForeColor="Blue" />
                                                    </asp:CommandField>
                                                    <asp:TemplateField HeaderText="Document No" HeaderStyle-Width="30px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocNo" runat="server" Text='<%# Eval("ConsumptionNo") %>'></asp:Label>
                                                            <asp:HiddenField ID="hdnDocId" runat="server" Value='<%# Eval("ConsumptionId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Document Date" HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConsumptionDate" runat="server" Text='<%# Eval("ConsumptionDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bill Amount" HeaderStyle-Width="100px" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBillAmount" runat="server" Text="" SkinID="label"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, status%>' HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProcessStatus" runat="server" Text='<%# Eval("ProcessStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Encoded By" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncodedBy" runat="server" Text='<%# Eval("EncodedBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Encoded Date" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncodedDate" runat="server" Text='<%# Eval("EncodedDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gv" />
                                        </Triggers>
                                    </asp:UpdatePanel>
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
