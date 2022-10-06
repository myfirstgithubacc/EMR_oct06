<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnacknowledgedServices.aspx.cs"
    Inherits="EMRBILLING_Popup_UnacknowledgedServices" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unperformed Services and drug order</title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        <%--   <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <table cellpadding="0px" border="0" style="background: #e0ebfd; border-width: 1px;
            vertical-align: middle;" width="99%">
            <tr>
                <td  style="display:none">
                    <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                        border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                        cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblRegNoInfo" runat="server" Font-Bold="true" Text="Reg.#"></asp:Label>
                                <asp:Label ID="lblRegNo" runat="server" SkinID="label" Font-Bold="true" ForeColor="#990066"></asp:Label>
                                <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                    Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblBedNo1" runat="server" Text="Bed No:" SkinID="label" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblBedNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                    Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="vertical-align: middle;">
                <td style="text-align: left; width: 30%;">
                    <asp:Label ID="lbl" runat="server" Text="Select : " SkinID="label" style="display:none"></asp:Label>
                    <telerik:RadComboBox ID="ddlFilter" runat="server" Width="250px" AutoPostBack="true"
                        AppendDataBoundItems="true" OnSelectedIndexChanged="ddlFilter_OnSelectedIndexChanged"  style="display:none">
                        <Items>
                            <telerik:RadComboBoxItem Text="All" Value="0" />
                            <telerik:RadComboBoxItem Text="Laboratory" Value="1" />
                            <telerik:RadComboBoxItem Text="Radiology" Value="2" />
                        </Items>
                    </telerik:RadComboBox>
                </td>
                <td style="text-align: right; width: 15%;">
                    <asp:Button ID="btnClose" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />&nbsp;
                </td>
            </tr>
            <tr style="vertical-align: middle;">
                <td colspan="3">
                    <asp:Label ID="lblMesaage" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td style="font-weight: bold; padding-top: 10px; padding-bottom: 10px;">Services :</td>
        </tr>
        </table>
        <asp:GridView ID="gvUnacknowledgedServices" OnPageIndexChanging="gvUnacknowledgedServices_OnPageIndexChanging" AllowPaging="true" PageSize="7" runat="server" SkinID="gridview" Width="99%"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order No">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" SkinID="label" Text='<%#Eval("OrderNo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Date">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderDate" runat="server" SkinID="label" Text='<%#Eval("OrderDate")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Lab No">
                    <ItemTemplate>
                        <asp:Label ID="lblLabNo" runat="server" SkinID="label" Text='<%#Eval("LabNo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Service Name">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceName" runat="server" SkinID="label" Text='<%#Eval("ServiceName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Sub Department">
                    <ItemTemplate>
                        <asp:Label ID="lblSubDepartment" runat="server" SkinID="label" Text='<%#Eval("SubDepartment")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
        <td style="font-weight: bold; padding-top: 10px; padding-bottom: 10px;">Drug Order :</td>
        </tr>
        </table>
        
        
        <asp:GridView ID="gvIPPharmacyStore" OnPageIndexChanging="gvIPPharmacyStore_OnPageIndexChanging" AllowPaging="true" PageSize="7" runat="server" SkinID="gridview" Width="99%"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Department Name">
                    <ItemTemplate>
                        <asp:Label ID="lblDepartment" runat="server" SkinID="label" Text='<%#Eval("DepartmentName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Indent No">
                    <ItemTemplate>
                        <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%#Eval("IndentNo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Indent Date">
                    <ItemTemplate>
                        <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Text='<%#Eval("IndentDate")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Type">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderType" runat="server" SkinID="label" Text='<%#Eval("OrderType")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Pending Satus">
                    <ItemTemplate>
                        <asp:Label ID="lblPendingSatus" runat="server" SkinID="label" Text='<%#Eval("PendingSatus")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Requested By">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedBy" runat="server" SkinID="label" Text='<%#Eval("RequestedBy")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <div style="font-weight: bold; padding-top: 10px; padding-bottom: 10px;">Non Drug Order :</div>
           <asp:GridView ID="gvNonDrugOrder" OnPageIndexChanging="gvNonDrugOrder_OnPageIndexChanging" AllowPaging="true" PageSize="7" runat="server" SkinID="gridview" Width="99%"
            AutoGenerateColumns="false">
            <Columns>
               <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order No">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" SkinID="label" Text='<%#Eval("OrderNo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Date">
                    <ItemTemplate>
                        <asp:Label ID="lblNonDrugOrderDate" runat="server" SkinID="label" Text='<%#Eval("OrderDate")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Prescription">
                    <ItemTemplate>
                        <asp:Label ID="lblPrescription" runat="server" SkinID="label" Text='<%#Eval("Prescription")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Type">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderTypeName" runat="server" SkinID="label" Text='<%#Eval("OrderTypeName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Doctor Name">
                    <ItemTemplate>
                        <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encoded By">
                    <ItemTemplate>
                        <asp:Label ID="lblEncodedBy" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encoded Date">
                    <ItemTemplate>
                        <asp:Label ID="lblEncodedDate" runat="server" SkinID="label" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
