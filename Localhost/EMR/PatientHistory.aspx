<%@ Page Title="Patient Diagnostic History" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PatientHistory.aspx.cs"
    Inherits="EMR_PatientHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_txtRegNo {
            height: 26px !important;
        }

        body#ctl00_Body1 {
            overflow: hidden !important;
        }

      
        input#ctl00_ContentPlaceHolder1_dtpFromDate_dateInput {
            padding-bottom: 5px !important;
        }

        input#ctl00_ContentPlaceHolder1_dtpToDate_dateInput {
            padding-bottom: 5px !important;
        }

        span#ctl00_ContentPlaceHolder1_dtpFromDate_dateInput_display {
            padding-top: 3px !important;
        }

        span#ctl00_ContentPlaceHolder1_dtpToDate_dateInput_display {
            padding-top: 3px !important;
        }

        input#ctl00_ContentPlaceHolder1_chkFavoriteService {
            margin-top: 2px !important;
            margin-right: 2px !important;
        }

        span#ctl00_ContentPlaceHolder1_Label2 {
            margin-top: 0px !important;
        }

        span#ctl00_ContentPlaceHolder1_lblMessage {
            margin: 0 !important;
            width: 100% !important;
        }
          @media screen and (min-width: 800px) {
                        .heightauto {
                           overflow-y:scroll;
                           height:560px;
                              
                        }
                    }
           .heightauto{
               overflow-x:hidden!important;
           }
        div#ctl00_ContentPlaceHolder1_gvResultFinal_GridData {
            overflow: visible !important;
            width: 100vw !important;
        }

        table#ctl00_ContentPlaceHolder1_gvResultFinal_ctl00_Header {
            width: 100vw!important;
            table-layout: fixed!important;
            overflow: visible!important;
            empty-cells: show!important;
            white-space: nowrap!important;
        }

        div#ctl00_ContentPlaceHolder1_gvResultFinal_GridHeader {
             overflow: visible!important; 
            width: 100vw!important;
        }
        div#ctl00_ContentPlaceHolder1_gvResultFinalP{
            overflow:auto!important;
        }
        div#divScroll{
            overflow:auto!important;
        }

        div#ctl00_ContentPlaceHolder1_UpdatePanel2 {
            overflow: hidden;
            width:97vw;
        }
    </style>
    
    <div class="container-fluid">
        <div class="row heightauto">

            <div class="container-fluid header_main form-group">
                <div class="row">
                    <div class="col-md-3">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="Patient Diagnostic History" /></h2>
                    </div>
                    <div class="col-md-6 ">
                        <%--<asp:UpdatePanel ID="upErrorMessage" runat="server"><ContentTemplate>--%>
                        <asp:Label ID="lblMessage" runat="server" />
                        <%-- </ContentTemplate></asp:UpdatePanel>--%>
                    </div>
                    <div class="col-md-3 text-right">
                        <asp:Button ID="btnOutsideLab" runat="server" Text="Outside Lab Result" CssClass="btn btn-primary" OnClick="btnOutsideLab_Click" />
                        <asp:Button ID="btnPrint" runat="server" ToolTip="Print" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_OnClick" />
                        <asp:Button ID="btnSearch" runat="server" Text="Refresh" ToolTip="Click here to refresh grid" Font-Size="8.0pt" Width="64px"
                            CssClass="btn btn-primary" OnClick="btnSearch_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />
                        <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" Visible="false" />
                    </div>
                </div>
            </div>
            <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" ControlsToSkip="Buttons" runat="server" DecorationZoneID="dvSearchZone" Skin="Office2007" />

    <%--<asp:UpdatePanel ID="updSearchZone" runat="server" UpdateMode="Conditional"><ContentTemplate>--%>
   <%-- sanyamtanwar--%>
    <div class="container-fluid" id="dvSearchZone">
        <div class="row form-groupTop01">

            <div class="col-lg-3 col-md-4 col-6 form-group">
                <div class="row">
                    <div class="col-md-3 label2 ">
                        <asp:Label ID="Label4" runat="server" Text="Facility" />
                    </div>
                    <div class="col-md-9 exSpace">
                        <telerik:RadComboBox ID="ddlFacility" CssClass="drapDrowHeight" Width="100%" runat="server" AppendDataBoundItems="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-4 col-6 form-group">
                <div class="row">
                    <div class="col-md-4 label2">
                        <asp:Label ID="lblReportType" runat="server" Text="Department" />
                    </div>
                    <div class="col-md-8 exSpace">
                        <telerik:RadComboBox ID="ddlReportFor" runat="server" CssClass="drapDrowHeight" Width="100%" AppendDataBoundItems="true" />
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-4 col-6 form-group">
                <div class="row">
                    <div class="col-md-3 label2">
                        <asp:Label ID="Label3" runat="server" Text="Result" />
                    </div>
                    <div class="col-md-9 exSpace">
                        <telerik:RadComboBox ID="ddlSearchFlag" runat="server" CssClass="drapDrowHeight" Width="100%">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0" Selected="true" />
                                <telerik:RadComboBoxItem Text="Results" Value="1" />
                                <telerik:RadComboBoxItem Text="Pending" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>

            <div class="col-lg-3 col-md-4 col-6 form-group">
                <div class="row">
                    <div class="col-md-6 col-6">
                        <div class="row">
                            <div class="col-md-3 mb-2 mb-md-0" style="margin-top: 3px;">
                                <asp:Label ID="Label5" runat="server" Text="From" />
                            </div>
                            <div class="col-md-9">
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                            </div>
                        </div>


                    </div>
                    <div class="col-md-6 col-6 form-group">
                        <div class="row">
                            <div class="col-md-3 mb-2 mb-md-0">
                                <asp:Label ID="Label6" runat="server" Text="To" />
                            </div>
                            <div class="col-md-9">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3 col-md-4 form-group">
                <div class="row">
                    <div class="col-md-3 col-2 label2">
                        <asp:Label ID="LblSearch" runat="server" Text="Search&nbsp;By" />
                    </div>
                    <div class="col-md-9 col-10">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-5 ">
                                        <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" CssClass="drapDrowHeight" Width="95%"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSearchCriteria_OnSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, ipno%>' Value="1" />
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="2" Selected="true" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        <span style="color: Red; float: right; display: none;">*</span>
                                    </div>
                                    <div class="col-7  " style="padding-left: 7px!important; s">
                                        <asp:TextBox ID="txtRegNo" runat="server" MaxLength="13" CssClass="drapDrowHeight" Width="100%" AutoPostBack="true" Font-Size="8.0pt" OnTextChanged="txtRegNo_TextChanged" onkeyup="return validateMaxLength();" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                        <asp:TextBox ID="txtEncNo" runat="server" MaxLength="10" CssClass="drapDrowHeight" Width="100%" AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged" Visible="false" />
                                        <%--&nbsp; &nbsp;<asp:Label ID="Label3" runat="server" Text="Status" SkinID="label"></asp:Label>
                                            &nbsp;<telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" EmptyMessage="All"
                                                Width="165px"  />--%>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row form-groupTop01">
            <div class="col-md-3 col-6 ">
                <div class="row">
                    <div class="col-md-6 col-7 label2 check-custom">
                        <asp:CheckBox ID="chkFavoriteService" runat="server" Text="Favorite Service" onclick="onclick_chkFavoriteService()" />
                        <%--<asp:Label ID="lblServiceName" runat="server" Text="Favorite" />--%>
                    </div>
                    <div class="col-md-6 col-5">
                        <telerik:RadComboBox ID="ddlServiceName" runat="server" CheckBoxes="true" CssClass="drapDrowHeight" EnableCheckAllItemsCheckBox="true"
                            EmptyMessage="Select Service" AppendDataBoundItems="true" Width="100%" DropDownWidth="400px" />
                        <%--OnClientDropDownClosing="ddlServiceNameOnClientItemChecked"--%>
                        <telerik:RadComboBox ID="ddlField" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            EmptyMessage="Select Service" AppendDataBoundItems="true" Width="100%" Height="400px" Visible="false" />
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-6">
                <div class="row">
                    <div class="col-md-4 col-4 label2">
                        <asp:Label ID="Label7" runat="server" Text="Result&nbsp;View" />
                    </div>
                    <div class="col-md-8 col-8 exSpace">
                        <telerik:RadComboBox ID="ddlView" runat="server" CssClass="drapDrowHeight" Width="100%"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlView_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Test (Y-axis) and Date (X-axis)" Value="YA" />
                                <telerik:RadComboBoxItem Text="Test (X-axis) and Date (Y-axis)" Value="XA" />
                                <telerik:RadComboBoxItem Text="Grid View" Value="GV" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-6 pt-1">
                <div class="PD-TabRadioNew01 margin_z">
                    <asp:CheckBox ID="chkAbnormalValue" runat="server" />
                    <asp:Label ID="Label2" runat="server" Text="Abnormal&nbsp;Result(s)" ForeColor="DarkViolet" />
                </div>
                <div class="PD-TabRadioNew01 margin_z">
                    <asp:CheckBox ID="chkCriticalValue" runat="server" />
                    <asp:Label ID="Label1" runat="server" Text="Critical&nbsp;Result(s)" ForeColor="Red" />
                </div>
            </div>
            <%-- sanyamtanwar--%>
            <div class="col-md-3 col-6" style="margin-top: 4px!important;">
                <asp:Label ID="lblPatientName" runat="server" Font-Bold="true" SkinID="label" />
                <asp:Button ID="btnCustomView" runat="server" SkinID="Button" Text="Customized View" OnClick="btnCustomView_OnClick" Visible="false" />
                <%--  <asp:Button ID="btnOutsideLab" runat="server" Text="Outside Lab Result"  OnClientClick="btnPatientLabResult();"/> --%>


                <%--<telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Behaviors="Close">
                        <Windows><telerik:RadWindow ID="RadWindow1" runat="server" /></Windows>
                    </telerik:RadWindowManager>--%>
            </div>
        </div>

    </div>
    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Skin="Office2007" Behaviors="Close,Maximize" ShowContentDuringLoad="false" VisibleStatusbar="false" ReloadOnShow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindowPopup" runat="server" Top="10px" Left="40px" Skin="Office2007" />
        </Windows>
    </telerik:RadWindowManager>

        <%--</ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlView" />
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
        </asp:UpdatePanel>--%>
        <%--<div id="dvGridZone" runat="server">--%>


    <div class="container-fluid" style="overflow:auto;">
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                        <%--<asp:Panel ID="pnl" runat="server" BorderWidth="1px" Width="1300px" Height="450px"
                            ScrollBars="Auto">--%>
                        <div id="divScroll" style="height: 400px; border: solid; border-width: 1px;">

                            <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" Height="400px" Skin="Office2007"
                                HeaderStyle-Font-Size="8pt" ItemStyle-Font-Size="8pt" AlternatingItemStyle-Font-Size="8pt" BorderWidth="0" CellPadding="0"
                                CellSpacing="0" AllowPaging="true" AllowCustomPaging="true" PageSize="50" AutoGenerateColumns="false"
                                ShowStatusBar="true" OnItemDataBound="gvResultFinal_OnItemDataBound" OnItemCommand="gvResultFinal_OnItemCommand"
                                OnPageIndexChanged="gvResultFinal_OnPageIndexChanged" Visible="false">
                                <ClientSettings AllowColumnsReorder="false" Scrolling-FrozenColumnsCount="1" Scrolling-AllowScroll="true"
                                    Scrolling-UseStaticHeaders="true" EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                    <%-- <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />--%>
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="true" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <GroupingSettings CaseSensitive="false" />
                            <MasterTableView TableLayout="auto" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No&nbsp;Record&nbsp;Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <GroupHeaderItemStyle Font-Bold="true" />
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="Source" DefaultInsertValue="" HeaderText="Source" HeaderStyle-Width="75px"
                                        Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                <asp:HiddenField ID="hdnPrintStatus" runat="server" Value='<%#Eval("PrintStatus") %>' />
                                                <asp:HiddenField ID="hdnResultHTML" runat="server" Value='<%#Eval("ResultHTML") %>' />
                                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                <asp:HiddenField ID="hdnReviewedStatus" runat="server" Value='<%#Eval("ReviewedStatus") %>' />
                                                <asp:HiddenField ID="hdnReviewedComments" runat="server" Value='<%#Eval("ReviewedComments") %>' />
                                                <asp:HiddenField ID="hdnIsOutsideResult" runat="server" Value='<%#Eval("IsOutsideResult") %>' />
                                                <asp:HiddenField ID="hdnIsAllowPrint" runat="server" Value='<%#Eval("IsAllowPrint") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="OrderDate" HeaderStyle-Width="110px" DefaultInsertValue="" HeaderText="Order Date"
                                        Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" Width="100px" Text='<%#Eval("OrderDate") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" Visible="true" HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblLabHeader" runat="server" Text='<%$ Resources:PRegistration, LABNO%>' />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ManualLabNo" Visible="false">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblManualLabHeaer" runat="server" Text="Manual Lab No" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblManualLabNo" runat="server" Text='<%#Eval("ManualLabNo") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ipno%>'
                                        HeaderStyle-Width="100px" Visible="True">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                        Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Provider" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, Provider%>'
                                        Visible="true" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="PatientName" DefaultInsertValue="" HeaderText="Patient Name"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="SubDepartmentName" DefaultInsertValue="" HeaderText="Sub Department"
                                        Visible="true" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDepartmentName" runat="server" Text='<%#Eval("SubDepartmentName") %>' Width="100%" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation" HeaderStyle-Width="150px"
                                        Visible="true">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnFlag" Value='<%#Eval("FlagName") %>' runat="server" />
                                                <asp:LinkButton ID="lnkServiceName" runat="server" Text='<%#Eval("ServiceName") %>'
                                                    CommandName="Investigation" CommandArgument='<%#Eval("ServiceName") %>' ToolTip="click here to view lab result in graph"
                                                    ForeColor="Black" />
                                                <%--<asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' Visible="false" />--%>
                                                &nbsp;&nbsp;
                                                <asp:HiddenField ID="hdnAccessionNo" runat="server" Value='<%#Eval("AccessionNo")%>' />
                                                <asp:ImageButton ID="imgViewImage" runat="server" ImageUrl="~/Icons/RIS.jpg" ToolTip="View Scan Image"
                                                    OnClick="imgViewImage_Click" Visible="false" CommandName='<%#Eval("AccessionNo")+"|"+ Eval("RegistrationNo")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result" HeaderStyle-Width="50px"
                                        Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                                <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result"
                                                    CommandArgument="None" Visible="true" ForeColor="Black" />
                                                <asp:HiddenField ID="hdnDescription" runat="server" Value='<%#Eval("Description") %>' />
                                                <asp:HiddenField ID="hdnDocumentName" runat="server" Value='<%#Eval("DocumentName") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="print" DefaultInsertValue="" HeaderText="Print"  HeaderStyle-Width="50px"
                                        Visible="true">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkprint" runat="server" CommandName="Print" CommandArgument="None"
                                                    Text="Print" ForeColor="Black"  />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName1" DefaultInsertValue="" HeaderText="RegistrationId"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="AbnormalValue" DefaultInsertValue="" HeaderText="AbnormalValue"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAbnormalValue" runat="server" Text='<%#Eval("AbnormalValue") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="CriticalValue" DefaultInsertValue="" HeaderText="CriticalValue"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCriticalValue" runat="server" Text='<%#Eval("CriticalValue") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName7" DefaultInsertValue="" HeaderText="Age/Gender"
                                            AllowFiltering="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName10" DefaultInsertValue="" HeaderText="Status&nbsp;Color"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName11" DefaultInsertValue="" HeaderText="Sample&nbsp;ID"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                                <asp:Label ID="LblFormatID" runat="server" Text='<%#Eval("FormatID") %>' Visible="false" />
                                                <asp:Label ID="LblWinReportType" runat="server" Text='<%#Eval("ReportType") %>' Visible="false" />
                                                <asp:Label ID="LblSubDeptCode" runat="server" Text='<%#Eval("SubDeptId") %>' Visible="false" />
                                                <asp:Label ID="LblDepartmentCode" runat="server" Text='<%#Eval("DepartmentId") %>' Visible="false" />
                                                <asp:Label ID="LblIsArchive" runat="server" Text='<%#Eval("IsArchive") %>' Visible="false" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName12" DefaultInsertValue="" HeaderText="Status&nbsp;ID"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusID" runat="server" Text='<%#Eval("StatusID") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName15" DefaultInsertValue="" HeaderText="StationId"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStationId" runat="server" Text='<%#Eval("StationId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UniqueName14" DefaultInsertValue="" HeaderText="ServiceId"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ResultRemarksId" DefaultInsertValue="" HeaderText="ResultRemarksId"
                                            AllowFiltering="false" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultRemarksId" runat="server" Text='<%#Eval("ResultRemarksId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="StatusCode" DefaultInsertValue="" HeaderText="StatusCode"
                                            AllowFiltering="false" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                            <%--<asp:GridView ID="gvLabDetailsXaxis" runat="server" AutoGenerateColumns="true" SkinID="gridview2"
                                HeaderStyle-Font-Size="7px" OnRowDataBound="gvLabDetailsXaxis_OnRowDataBound"
                                RowStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                </asp:GridView>--%>
                            <telerik:RadGrid ID="gvLabDetailsXaxis" runat="server" Width="1280px" Height="430px" Skin="Office2007"
                                AutoGenerateColumns="true" HeaderStyle-Font-Size="8pt" ItemStyle-Font-Size="8pt" AlternatingItemStyle-Font-Size="8pt"
                                BorderWidth="0" CellPadding="0" CellSpacing="0" HeaderStyle-HorizontalAlign="Center"
                                OnItemDataBound="gvLabDetailsXaxis_OnRowDataBound">
                                <ClientSettings AllowColumnsReorder="false" Scrolling-FrozenColumnsCount="1" Scrolling-AllowScroll="true"
                                    Scrolling-UseStaticHeaders="true" EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <GroupingSettings CaseSensitive="false" />
                                <MasterTableView TableLayout="Auto" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No&nbsp;Record&nbsp;Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <GroupHeaderItemStyle Font-Bold="true" />
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                        <%--</asp:Panel>--%>

                        <div class="row form-group">
                            <div class="col-md-12">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label8" runat="server" CssClass="LegendColor" BackColor="#FFFF99" Text="Note: ** Outside lab result" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" CssClass="LegendColor" BackColor="#7dcea0" Text="Printed" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvLabDetailsXaxis" EventName="ItemDataBound" />
                        <asp:PostBackTrigger ControlID="gvResultFinal" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:Button ID="btnFavoriteService" runat="server" Style="visibility: hidden;" OnClick="btnFavoriteService_OnClick" />
                <asp:Button ID="btnCallcombo" runat="server" Style="visibility: hidden;" OnClick="btnCallcombo_OnClick" />
                <asp:Button ID="btnCallPopup" runat="server" Style="visibility: hidden;" OnClick="btnCallPopup_OnClick" />
                <asp:Button ID="btnPrintPopup" runat="server" Style="visibility: hidden;" OnClick="btnPrintPopup_OnClick" />
                <asp:Button ID="btnFilterView" runat="server" Style="visibility: hidden;" OnClick="btnFilterView_OnClick" />
                <asp:HiddenField ID="hdnsStatusCode" runat="server" />
                <asp:HiddenField ID="hdnsDiagSampleId" runat="server" />
                <asp:HiddenField ID="hdnsServiceId" runat="server" />
                <asp:HiddenField ID="hdnsSource" runat="server" />
                <asp:HiddenField ID="hdnsServiceName" runat="server" />
                <asp:HiddenField ID="hdnsAgeGender" runat="server" />
                <asp:HiddenField ID="hdnsFieldId" runat="server" />
                <asp:HiddenField ID="hdnsType" runat="server" />
                <asp:HiddenField ID="hdnsCellId" runat="server" />
                <asp:HiddenField ID="hdnsLabNoEncIdRegId" runat="server" />
            </div>
        </div>
  </div>
    </div>
        <script language="javascript" type="text/javascript">

            function btnPatientLabResult() {
                //alert("check");
                var x = screen.width / 2 - 1300 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;

                popup = window.open("/LIS/Phlebotomy/OutSource.aspx?From=POPUP&CloseButtonShow=Yes", "Popup", "height=550,width=1300,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
                popup.focus();
                document.getElementById("mainDIV").style.opacity = "0.5";
            <%--$get('<%=btnDisableControl.ClientID%>').click();--%>
                popup.onunload = function () {

                <%--$get('<%=btnBindOrderPriscriptionPlaneOfCare.ClientID%>').click();--%>
                  //document.getElementById("mainDIV").style.opacity = "";
              }

                return false
            }

            function ClientSideClick(myButton) {

                // Client side validation
                if (typeof (Page_ClientValidate) == 'function') {
                    if (Page_ClientValidate() == false) {
                        return false;
                    }
                }

                //make sure the button is not of type "submit" but "button"
                if (myButton.getAttribute('type') == 'button') {
                    // disable the button
                    myButton.disabled = true;
                    myButton.className = "btn-inactive";
                    myButton.className = "btn btn-primary";
                    myButton.value = "Processing...";
                }
                return true;
            }


            function validateMaxLength() {
                var txt = $get('<%=txtRegNo.ClientID%>');
              if (txt.value > 9223372036854775807) {
                  alert("Value should not be more then 9223372036854775807.");
                  txt.value = txt.value.substring(0, 12);
                  txt.focus();
              }
          }
          function ddlServiceNameOnClientItemChecked(sender, eventArgs) {
              $get('<%=btnCallcombo.ClientID%>').click();
        }

        function onclick_chkFavoriteService() {
            $get('<%=btnFavoriteService.ClientID%>').click();
        }

        function showResultPopup(sDiagSampleId, sServiceId, sSource, sStatusCode, sServiceName, sAgeGender, sFieldId, sType, sCellId) {

            $get('<%=hdnsDiagSampleId.ClientID%>').value = sDiagSampleId;
            $get('<%=hdnsServiceId.ClientID%>').value = sServiceId;
            $get('<%=hdnsSource.ClientID%>').value = sSource;
            $get('<%=hdnsStatusCode.ClientID%>').value = sStatusCode;
            $get('<%=hdnsServiceName.ClientID%>').value = sServiceName;
            $get('<%=hdnsAgeGender.ClientID%>').value = sAgeGender;
            $get('<%=hdnsFieldId.ClientID%>').value = sFieldId;
            $get('<%=hdnsType.ClientID%>').value = sType;
            $get('<%=hdnsCellId.ClientID%>').value = sCellId;

            //       
            //        var oWnd = $find("<%= RadWindowPopup.ClientID %>");
            //        oWnd.show();
            //        oWnd.setSize(800, 600);
            //       
            //        oWnd.setUrl("/LIS/Phlebotomy/LabResultGraph.aspx?SOURCE=" + sSource + "&DIAG_SAMPLEID=" + sDiagSampleId + "&DIAG_SAMPLEID_S=" + sDiagSampleId
            //                                                           + "&SERVICEID=" + sServiceId + "&AgeInDays=" + sAgeGender + "&StatusCode=" + sStatusCode
            //                                                           + "&ServiceName=" + sServiceName
            //                                                           + "&FieldId=" + sFieldId + "&RegNo=" + $get('<%=txtRegNo.ClientID%>').value);
            //        oWnd.minimize();
            //        oWnd.maximize();
            //        oWnd.restore();
            $get('<%=btnCallPopup.ClientID%>').click();

        }

        function SearchOnClientClose(oWnd) {
            $get('<%=btnFilterView.ClientID%>').click();
        }

        function SearchOnClientClose1(oWnd) {
            $get('<%=btnSearch.ClientID%>').click();
            }

            function printPopup(e, sDiagSampleId, sServiceId, sSource, sStatusCode, sServiceName, sAgeGender, sFieldId, sType, sCellId, sLabId) {
                $get('<%=hdnsDiagSampleId.ClientID%>').value = sDiagSampleId;
            $get('<%=hdnsServiceId.ClientID%>').value = sServiceId;
            $get('<%=hdnsSource.ClientID%>').value = sSource;
            $get('<%=hdnsStatusCode.ClientID%>').value = sStatusCode;
            $get('<%=hdnsServiceName.ClientID%>').value = sServiceName;
            $get('<%=hdnsAgeGender.ClientID%>').value = sAgeGender;
            $get('<%=hdnsFieldId.ClientID%>').value = sFieldId;
            $get('<%=hdnsType.ClientID%>').value = sType;
            $get('<%=hdnsCellId.ClientID%>').value = sCellId;
            $get('<%=hdnsLabNoEncIdRegId.ClientID%>').value = sLabId;

            $get('<%=btnPrintPopup.ClientID%>').click();
        }
        </script>
</asp:Content>
