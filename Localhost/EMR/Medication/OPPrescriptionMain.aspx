<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OPPrescriptionMain.aspx.cs"
    Inherits="EMR_OPPrescription_OPPrescriptionMain" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
    <script language="javascript" type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.IndentIds = document.getElementById("hdnPageIndentIds").value;
            oArg.ItemIds = document.getElementById("hdnPageItemIds").value;

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


        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="container-fluid header_main form-group">
                    <div class="col-md-3"><h2><asp:Image ID="Image1" ImageUrl="/Images/user.jpg" runat="server" /><asp:Label ID="Label1" runat="server" Text="Previous&nbsp;Medications" /></h2></div>
                    <div class="col-md-6 text-center"><asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" /></div>
                    <div class="col-md-3 text-right">
                        <asp:Button ID="btnReOrder" Text="Re Order" runat="server" OnClick="btnReOrder_Onclick" CssClass="btn btn-primary" />
                        <asp:Button ID="btnClose" Text="Close" runat="server" CausesValidation="false" CssClass="btn btn-default" OnClientClick="window.close();" />
                    </div>
                </div>



                <div class="container-fluid">
                    <div class="row">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvPrevious" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="430px" Width="100%" ScrollBars="Auto"
                                    BorderWidth="1px" BorderColor="LightBlue">
                                    <asp:GridView ID="gvPrevious" runat="server" Width="100%" Height="100%" AllowPaging="false"
                                        SkinID="gridviewOrderNew" AutoGenerateColumns="False" OnRowDataBound="gvPrevious_OnRowDataBound"
                                        OnRowCommand="gvPrevious_OnRowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="8px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                    <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                    <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                    <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                    <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                    <%--<asp:HiddenField ID="hdnUnitId" runat="server" Value='<%# Eval("UnitId") %>' />--%>
                                                    <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                    <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                    <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                    <%-- <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%#Eval("FrequencyId") %>' />--%>
                                                    <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%#Eval("RouteId") %>' />
                                                    <asp:HiddenField ID="hdnStrengthId" runat="server" Value='<%#Eval("StrengthId") %>' />
                                                    <asp:HiddenField ID="hdnXMLData" runat="server" />
                                                    <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Eval("DetailsId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="15px" >
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentNo" runat="server" Text='<%# Eval("IndentNo") %>'
                                                        Width="40px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentDate" runat="server" Text='<%# Eval("IndentDate") %>'
                                                        Width="60px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Drug Name" HeaderStyle-Width="280px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="280px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'
                                                        Width="280px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Indent Type" HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIndentType" runat="server" Text='<%# Eval("IndentType") %>'
                                                        Width="90px" />
                                                    <asp:HiddenField ID="hdnIndentTypeId" runat="server" Value='<%#Eval("IndentTypeId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Qty." HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalQty" runat="server" Text='<%#Eval("Qty") %>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prescription Detail" HeaderStyle-Width="450px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="450px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("PrescriptionDetail") %>'
                                                        Width="450px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Not Shown to Pharmacy" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNotShownToPharmacy" runat="server" Text='<%#Eval("NotShownToPharmacy") %>'
                                                        Width="50px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                
                                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>'
                                                        Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="End Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>'
                                                        Width="50px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" HeaderText="Cancel">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnCancel" runat="server" ToolTip="Click here to cancel this drug"
                                                        CommandName="ItemCancel" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'
                                                        ImageUrl="~/Images/close_new-old.jpg" Width="15px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>




                
                <table cellpadding="0" cellspacing="4" visible="false">
                    <tr visible="false">
                        <td>
                            <asp:Label ID="Labell1" runat="server" SkinID="label" Text="&nbsp;Reason for stop order" visible="false"/>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlStopOrder" runat="server" EmptyMessage="[ Select ]" Width="120px" visible="false"/>
                        </td>
                        <td>
                            <asp:Button ID="btnStopOrder" runat="server" Text="Stop Order" SkinID="Button" OnClick="btnStopOrder_Click" visible="false"/>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="2">
                    <tr>
                        <td>
                            <div id="dvConfirmStop" runat="server" visible="false" style="width: 400px; z-index: 200;
                                border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                                border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute;
                                bottom: 0; height: 100px; left: 520px; top: 220px">
                                <table width="100%" cellspacing="2" cellpadding="0">
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="Label31" Font-Size="12px" runat="server" Font-Bold="true" Text="Cancel Medication Remarks" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtStopRemarks" SkinID="textbox" runat="server" TextMode="MultiLine"
                                                Style="min-height: 45px; max-height: 45px; min-width: 390px; max-width: 390px;"
                                                MaxLength="200" onkeyup="return MaxLenTxt(this, );" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnStopMedication" SkinID="Button" runat="server" Text="Cancel" OnClick="btnStopMedication_OnClick" />
                                            &nbsp;
                                            <asp:Button ID="btnStopClose" SkinID="Button" runat="server" Text="Close" OnClick="btnStopClose_OnClick" />
                                        </td>
                                        <td align="center">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                        
                    <asp:HiddenField ID="hdnPageIndentIds" runat="server" />
                    <asp:HiddenField ID="hdnPageItemIds" runat="server" />
                
                            <asp:HiddenField ID="hdnItemId" runat="server" />
                            <asp:HiddenField ID="hndItemName" runat="server" />
                            <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>
