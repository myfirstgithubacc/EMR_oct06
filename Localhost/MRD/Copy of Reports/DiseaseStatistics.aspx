<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DiseaseStatistics.aspx.cs" Inherits="MRD_Reports_DiseaseStatistics"
    Title="Disease Statistics" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table id="tblmain" runat="server" cellpadding="1" cellspacing="1" border="0" width="100%">
        <tr>
            <td style="width: 10%; text-align: left;">
                <asp:Label ID="lbltype" runat="server" Text="Visit Type" />
            </td>
            <td style="width: 10%; text-align: left;">
                <telerik:RadComboBox ID="ddlType" runat="server" AppendDataBoundItems="false" Width="80px">
                    <Items>
                        <telerik:RadComboBoxItem Value="O" Text="OPD" Selected="true" />
                        <telerik:RadComboBoxItem Value="I" Text="IPD" />
                        <telerik:RadComboBoxItem Value="B" Text="Both" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td style="width: 10%; text-align: center;">
                <asp:Label ID="Label2" runat="server" Text="Date From" SkinID="label"></asp:Label>
            </td>
            <td style="width: 8%; text-align: left;">
                <telerik:RadDatePicker ID="dtpFromdate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy"
                    DateInput-DisplayDateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td style="width: 2%; text-align: left;">
                <asp:Label ID="Label1" runat="server" Text="To " SkinID="label"></asp:Label>
            </td>
            <td style="width: 10%; text-align: left;">
                <telerik:RadDatePicker ID="dtpTodate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy"
                    DateInput-DisplayDateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
            </td>
            <td style="width: 10%; text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="ICD/ICP" />
            </td>
            <td style="width: 30%; text-align: left;">
                <telerik:RadComboBox ID="ddlICDorICP" runat="server" AppendDataBoundItems="false"
                    Width="80px">
                    <Items>
                        <telerik:RadComboBoxItem Value="D" Text="ICD" />
                        <telerik:RadComboBoxItem Value="P" Text="ICP" />
                        <telerik:RadComboBoxItem Value="A" Text="Both" Selected="true" />
                    </Items>
                </telerik:RadComboBox>
            </td>
            <td style="width: 10%; text-align: right;">
            </td>
        </tr>
        <tr style="vertical-align: top;">
            <td style="width: 10%; text-align: left; vertical-align: middle;">
                <asp:Label ID="lblRptType" runat="server" Text="Report Type" />
            </td>
            <td colspan="7" style="width: 60%; text-align: right; vertical-align: middle;">
                <asp:RadioButtonList ID="rdoRptType" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rdoRptType_OnSelectedIndexChanged">
                    <asp:ListItem Value="D" Text="Diagnosis Wise" Selected="True" />
                    <asp:ListItem Value="P" Text="Doctor Wise" />
                    <asp:ListItem Value="S" Text="Specialisation Wise" />
                </asp:RadioButtonList>
                &nbsp;
            </td>
            <td style="width: 30%; text-align: right; vertical-align: middle;">
                <asp:Button ID="btnPerformanceAnalysis" runat="server" Text="Show Analysis" SkinID="Button"
                    OnClick="btnPerformanceAnalysis_OnClick" />
            </td>
        </tr>
        <tr id="trDiag" runat="server" visible="false">
            <td style="width: 20%; text-align: left;" colspan="2">
                <asp:CheckBox ID="chkAllDiag" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllDiag_OnCheckedChanged"
                    Text="All Diagnosis : " TextAlign="Left" />
            </td>
            <td id="tdCate" runat="server" visible="false" style="width: 10%; text-align: center;">
                <asp:Label ID="lbl1" runat="server" Text="Category" />
            </td>
            <td id="tdddlCate" runat="server" visible="false" colspan="3" style="width: 20%;
                text-align: left;">
                <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="true" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="98%"
                    DropDownWidth="300px" Filter="Contains" EnableLoadOnDemand="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged">
                </telerik:RadComboBox>
            </td>
            <td id="tdDiag" runat="server" visible="false" style="width: 10%; text-align: right;">
                <asp:Label ID="Label5" runat="server" Text="Diagnosis" SkinID="label" />
            </td>
            <td id="tdddlDiag" runat="server" visible="false" style="width: 30%; text-align: left;">
                <telerik:RadComboBox ID="ddlDiagnosis" runat="server" AppendDataBoundItems="true"
                    EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="98%" DropDownWidth="300px"
                    Filter="Contains" EnableLoadOnDemand="true">
                </telerik:RadComboBox>
            </td>
            <td style="width: 10%; text-align: right;">
            </td>
        </tr>
        <tr>
            <td colspan="9" style="width: 100%;">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="true" ShowPrintButton="true"
                    ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowFindControls="False"
                    ShowParameterPrompts="False" ShowPromptAreaButton="False" ShowZoomControl="False"
                    Height="450px" Width="100%">
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
</asp:Content>
