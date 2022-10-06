<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ICCAViewer.aspx.cs" Inherits="EMR_ICCAViewer" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function closeDiv() {

            dvchart.style.display = 'none';

        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" cellspacing="0" cellpadding="0" border="0" class="clsheader">
                <tr>
                    <td>
                         <asp:Label ID="lblheader" runat="server" Text="ICCA View"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblmessage" runat="server"></asp:Label>
                    </td>
                    <td align="right" style="height: 13px; color: green; font-size: 13px; font-weight: bold;">
                        <asp:Button ID="btnviechartm" runat="server" Text="Chart View" CssClass="btn btn-primary" OnClick="btnviechartm_OnClick" />
                        <asp:Button ID="btnImportData" runat="server" Text="Import Selected Data" CssClass="btn btn-primary"
                            OnClick="btnImportData_OnClick" />
                        <%-- <asp:Button ID="Button1" runat="server" Text="Close" SkinID="Button" OnClientClick="window.close();" />--%>
                    </td>
                </tr>
            </table>
            <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <aspl1:UserDetail ID="asplUD" runat="server" />
                        <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>--%>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>From Date:
                    </td>
                    <td>
                        <telerik:RadDateTimePicker ID="dtpfrom" Width="130px" runat="server" DateInput-ReadOnly="false"
                            DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm">
                        </telerik:RadDateTimePicker>
                    </td>
                    <td>To Date:
                    </td>
                    <td>
                        <telerik:RadDateTimePicker ID="dtpTodate" Width="130px" runat="server" DateInput-ReadOnly="false"
                            DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm">
                        </telerik:RadDateTimePicker>
                    </td>
                     <td>Parameters:
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlParameters" CssClass="drapDrowHeight" Width="100%" runat="server" AppendDataBoundItems="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                       
                    </td>
                    <td>
                        &nbsp;<asp:Button ID="btnView" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnView_click" />
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr valign="top">
                    
                    <td>
                        <br />
                        <asp:GridView ID="gvICCAdata" AutoGenerateColumns="false" runat="server" SkinID="gridview"
                            OnRowDataBound="gvICCAdata_OnRowDataBound" AllowPaging="true" PageSize="25" OnPageIndexChanging="gvICCAdata_PageIndexChanging"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="25px">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" Visible='<%# Convert.ToBoolean(Eval("IsMapped")) ? Convert.ToBoolean(Eval("IsTransmitted"))? false : true : false %>' />
                                        <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("Id")%>' />
                                        <asp:HiddenField ID="hdnImport" runat="server" Value='<%#Eval("Import") %>' />
                                        <asp:HiddenField ID="hdnIsTransmitted" runat="server" Value='<%#Eval("IsTransmitted") %>' />
                                        <asp:HiddenField ID="hdnIsMapped" runat="server" Value='<%# Eval("IsMapped") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="RegistrationNo" ItemStyle-Width="100px" HeaderText='<%$ Resources:PRegistration, regno%>' />
                                <asp:BoundField DataField="EncounterNo" ItemStyle-Width="100px" HeaderText='<%$ Resources:PRegistration, IpNo%>' />
                                <asp:TemplateField HeaderText="Output Date" HeaderStyle-Width="180px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutputDate" runat="server" Text='<%#Eval("OutputDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ParameterName" HeaderText="Parameter" />
                                <asp:BoundField DataField="OBX" ItemStyle-Width="100px" HeaderText="Result" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                         <asp:Label ID="Label1" runat="server" BorderWidth="1px" BackColor="#D0ECBB" SkinID="label" Width="22px" Height="14px" />&nbsp;&nbsp;<asp:Label ID="lblColor1" runat="server" SkinID="label" Text="Unmapped&nbsp;Vitals" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblColor" runat="server" BorderWidth="1px" BackColor="#ECBBBB" SkinID="label" Width="22px" Height="14px" />&nbsp;&nbsp;<asp:Label ID="Label11" runat="server" SkinID="label" Text="Data&nbsp;Transmitted" />
                       
                    </td>
                </tr>
            </table>
             <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Behaviors="Close">
            <Windows>
                <telerik:RadWindow ID="RadWindowPopup" runat="server" Top="10px" Left="40px" />
            </Windows>
        </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
