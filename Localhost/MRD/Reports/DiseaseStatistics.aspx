<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DiseaseStatistics.aspx.cs" Inherits="MRD_Reports_DiseaseStatistics"
    Title="Disease Statistics" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />


    <div class="container-fluid" id="tblmain" runat="server" >
        <div class="row header_main">
            <div class="col-md-5 col-sm-5 col-xs-12">&nbsp;&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lbltype" runat="server" Text="Visit Type" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlType" runat="server" AppendDataBoundItems="false" Width="100%">
                    <Items>
                        <telerik:RadComboBoxItem Value="I" Text="IPD" Selected="true" />
                        <telerik:RadComboBoxItem Value="O" Text="OPD" />
                        <%--   <telerik:RadComboBoxItem Value="B" Text="Both" />--%>
                    </Items>
                </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label2" runat="server" Text="Date From" SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadDatePicker ID="dtpFromdate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy"
                    DateInput-DisplayDateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label1" runat="server" Text="To " SkinID="label"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadDatePicker ID="dtpTodate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy"
                    DateInput-DisplayDateFormat="dd/MM/yyyy">
                </telerik:RadDatePicker>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-7 col-sm-7 col-xs-7 box-col-checkbox">
                        <asp:RadioButtonList ID="rdoICDCPT" runat="server" Width="100%" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoICDCPT_OnSelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Value="D" Text="ICD" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="S" Text="CPT"></asp:ListItem>
                </asp:RadioButtonList>
                    </div>
                    <div class="col-md-5 col-sm-5 col-xs-5">
                        <asp:CheckBox ID="chkSummary" runat="server" Text="Details" TextAlign="Left" SkinID="checkbox" />
                    </div>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label3" runat="server" Text="ICD/CPT" />&nbsp;
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                <asp:TextBox ID="txtICDCPT" runat="server" SkinID="textbox" MaxLength="15" Width="100%"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-7 col-sm-7 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                        <asp:Label ID="lblRptType" runat="server" Text="Report Type" />
                    </div>
                    <div class="col-md-10 col-sm-10 col-xs-9 box-col-checkbox-table">
                        <asp:RadioButtonList ID="rdoRptType" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" OnSelectedIndexChanged="rdoRptType_OnSelectedIndexChanged">
                    <asp:ListItem Value="D" Text="Diagnosis Wise" Selected="True" />
                    <asp:ListItem Value="P" Text="Doctor Wise" />
                    <asp:ListItem Value="S" Text="Specialisation Wise" />
                    <asp:ListItem Value="AW" Text="Age Wise" />
                    <asp:ListItem Value="DS" Text="Disease Statis detail" />
                </asp:RadioButtonList>
                <asp:RadioButtonList ID="rdoRptType1" runat="server" RepeatDirection="Horizontal"
                    Visible="false" AutoPostBack="True" OnSelectedIndexChanged="rdoRptType1_OnSelectedIndexChanged">
                    <asp:ListItem Value="D" Text="Surgery Wise" Selected="True" />
                    <asp:ListItem Value="P" Text="Doctor Wise" />
                    <asp:ListItem Value="AW" Text="Age Wise" />
                </asp:RadioButtonList>                
                <asp:LinkButton ID="lnkeditAgeRange" runat="server" Text="Edit Age Group" OnClick="lnkeditAgeRange_OnClick"
                    Visible="false" />
                    </div>
                </div>
            </div>
            <div class="col-md-5 col-sm-5 col-xs-12 p-t-b-5">
                <asp:Button ID="btnPerformanceAnalysis" runat="server" Text="Show Analysis" CssClass="btn btn-primary"
                    OnClick="btnPerformanceAnalysis_OnClick" />
            </div>
        </div>
        <div id="trDiag" runat="server" visible="true" >
            <div class="row">
                <div class="col-md-8 col-sm-8 col-xs-12">
                <div class="row">
                    <div class="col-md-2 col-sm-2 col-xs-10 text-nowrap">
                        <asp:CheckBox ID="chkAllDiag" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllDiag_OnCheckedChanged"
                    Text="All Diagnosis : " TextAlign="Left" />
                    </div>
                    <div class="col-md-10 col-sm-10 col-xs-10 griview">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="true" ShowPrintButton="true"
                    ShowCredentialPrompts="False" ShowDocumentMapButton="False" ShowFindControls="False"
                    ShowParameterPrompts="False" ShowPromptAreaButton="False" ShowZoomControl="False"
                     Width="100%">
                </rsweb:ReportViewer>
                    </div>
                </div>
            </div>
            </div>
            <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12" id="trDoctors" runat="server" visible="false">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lblDoctor" runat="server" Text="Doctor" SkinID="label" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlRenderingProvider" runat="server" MarkFirstMatch="true"
                    EmptyMessage="Select" Filter="Contains" DropDownWidth="300px" TabIndex="30" Skin="Outlook"
                    Width="100%" CheckBoxes="true" AppendDataBoundItems="true">
                </telerik:RadComboBox>
                <asp:CheckBox ID="ChkAllprovider" runat="server" Text="All" AutoPostBack="true" OnCheckedChanged="ChkAllprovider_CheckedChanged"
                    SkinID="checkbox" />
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap" id="tdCate" runat="server" visible="false">
                        <asp:Label ID="lbl1" runat="server" Text="Category" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8" id="tdddlCate" runat="server" visible="false">
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AppendDataBoundItems="true"
                    AutoPostBack="true" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="100%"
                    DropDownWidth="300px" Filter="Contains" EnableLoadOnDemand="true" OnSelectedIndexChanged="ddlCategory_OnSelectedIndexChanged">
                </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap" id="tdDiag" runat="server" visible="false">
                        <asp:Label ID="Label5" runat="server" Text="Diagnosis" SkinID="label" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8" id="tdddlDiag" runat="server" visible="false">
                        <telerik:RadComboBox ID="ddlDiagnosis" runat="server" AppendDataBoundItems="true"
                    EnableCheckAllItemsCheckBox="true" CheckBoxes="true" Width="100%" DropDownWidth="300px"
                    Filter="Contains" EnableLoadOnDemand="true">
                </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
            </div>
        </div>
    </div>
    
    
</asp:Content>
