<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PatientHistory.aspx.cs" Inherits="LIS_Phlebotomy_PatientHistory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <style type="text/css">
        #ctl00_ContentPlaceHolder1_Label2{
            margin: 0em 7px 0 0!important;
        }
    </style>
    <script language="javascript" type="text/javascript">

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



    <div class="container-fluid">
        <div class="row header_main">
        <div class="col-md-6 col-sm-6 col-xs-6">
            <h2>
                <asp:Label ID="lblHeader" runat="server" Text="Patient Lab Result History" /></h2>
        </div>
       
        <div class="col-md-6 col-sm-6 col-xs-6 text-right">
            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" Visible="false" />
            <asp:Button ID="btnPrint" runat="server" ToolTip="Print" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_OnClick" />


        </div>
    </div>
         <div class="row text-center">
            <%--<asp:UpdatePanel ID="upErrorMessage" runat="server"><ContentTemplate>--%>
            <asp:Label ID="lblMessage" runat="server" />
            <%-- </ContentTemplate></asp:UpdatePanel>--%>
        </div>

    <div class="row">
        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" ControlsToSkip="Buttons" runat="server" DecorationZoneID="dvSearchZone" Skin="Office2007" />
    </div>



    <div id="dvSearchZone" runat="server">
        <%--<asp:UpdatePanel ID="updSearchZone" runat="server" UpdateMode="Conditional"><ContentTemplate>--%>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label4" runat="server" Text="Facility" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlFacility" CssClass="drapDrowHeight" Width="100%" runat="server" AppendDataBoundItems="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lblReportType" runat="server" Text="For&nbsp;Station" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlReportFor" runat="server" CssClass="drapDrowHeight" Width="100%" AppendDataBoundItems="true" />
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-7">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-7 text-nowrap">
                                <asp:Label ID="Label5" runat="server" Text="From" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-5">
                                 <telerik:RadDatePicker ID="dtpFromDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-5">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                <asp:Label ID="Label6" runat="server" Text="To" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-9">
                                 <telerik:RadDatePicker ID="dtpToDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">
                        <asp:Label ID="LblSearch" runat="server" Text="Search&nbsp;By" />
                        <span style="color: Red;">*</span>
                    </div>
                    <div class="col-md-9 col-sm-9 col-xs-8">
                        <div class="row">
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <div class="row">
                                    <div class="col-md-5 col-sm-5 col-xs-5 no-p-l no-p-r">
                                        <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" CssClass="drapDrowHeight" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchCriteria_OnSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, ipno%>' Value="1" />
                                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="2" Selected="true" />
                                            </Items>
                                        </telerik:RadComboBox>
                                        
                                    </div>
                                    <div class="col-md-7 col-sm-7 col-xs-7">
                                        <asp:TextBox ID="txtRegNo" runat="server" MaxLength="13" CssClass="drapDrowHeight" Width="100%" AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged" onkeyup="return validateMaxLength();" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                        <asp:TextBox ID="txtEncNo" runat="server" MaxLength="10" CssClass="drapDrowHeight" Width="100%" AutoPostBack="true" OnTextChanged="txtRegNo_TextChanged" Visible="false" />
                                        <%--&nbsp; &nbsp;<asp:Label ID="Label3" runat="server" Text="Status" SkinID="label"></asp:Label>
                                            &nbsp;<telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" EmptyMessage="All"
                                                Width="165px"  />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 no-p-l">
                                <asp:Button ID="btnSearch" runat="server" Text="Refresh" ToolTip="Click here to refresh grid" Font-Size="8.0pt"
                                    CssClass="btn btn-primary" OnClick="btnSearch_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lblServiceName" runat="server" Text="Patient Lab Service" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlServiceName" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            EmptyMessage="Select Service" AppendDataBoundItems="true" DropDownWidth="300px" CssClass="drapDrowHeight" Width="100%" Height="400px" />
                        <%--OnClientDropDownClosing="ddlServiceNameOnClientItemChecked"--%>
                        <telerik:RadComboBox ID="ddlField" runat="server" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            EmptyMessage="Select Service" AppendDataBoundItems="true" DropDownWidth="300px" CssClass="drapDrowHeight" Width="100%" Height="400px" Visible="false" />
                    </div>
                </div>
            </div>
           <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label7" runat="server" Text="Result&nbsp;View" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadComboBox ID="ddlView" runat="server" CssClass="drapDrowHeight" Width="100%"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlView_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Grid View" Value="GV" />
                                <telerik:RadComboBoxItem Text="Test (X-axis) and Date (Y-axis)" Value="XA" />
                                <telerik:RadComboBoxItem Text="Test (Y-axis) and Date (X-axis)" Value="YA" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-12 col-sm-12 col-xs-12 text-nowrap box-col-checkbox">
                    <asp:CheckBox ID="chkAbnormalValue" runat="server" />
                    <asp:Label ID="Label2" runat="server" Text="Abnormal&nbsp;Result(s)" ForeColor="DarkViolet" />
                
                    <asp:CheckBox ID="chkCriticalValue" runat="server" />
                    <asp:Label ID="Label1" runat="server" Text="Critical&nbsp;Result(s)" ForeColor="Red" />
                </div>
            </div>
                </div>
            <div class="col-md-3 col-sm-3 col-xs-12 p-t">
                <asp:Label ID="lblPatientName" runat="server" Font-Bold="true" SkinID="label" />
                <asp:Button ID="btnCustomView" runat="server" SkinID="Button" Text="Customized View" OnClick="btnCustomView_OnClick" Visible="false" />
                <%--<telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Behaviors="Close">
                        <Windows><telerik:RadWindow ID="RadWindow1" runat="server" /></Windows>
                    </telerik:RadWindowManager>--%>
            </div>
        </div>
    </div>
       
    <div class="row">
        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Skin="Metro" Behaviors="Close">
        <Windows>
            <telerik:RadWindow ID="RadWindowPopup" runat="server" Top="10px" Left="40px" />
        </Windows>
    </telerik:RadWindowManager>
    </div>

   

    
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <%--<asp:Panel ID="pnl" runat="server" BorderWidth="1px" Width="1300px" Height="450px"
                            ScrollBars="Auto">--%>
                    <div id="divScroll" style="overflow: auto; width: 100%; height: 450px; border: solid; border-width: 1px;">

                        <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" Height="430px"
                            HeaderStyle-Font-Size="8pt" ItemStyle-Font-Size="8pt" AlternatingItemStyle-Font-Size="8pt" BorderWidth="0" CellPadding="0"
                            CellSpacing="0" AllowPaging="true" AllowCustomPaging="true" PageSize="15" AutoGenerateColumns="false"
                            ShowStatusBar="true" OnItemDataBound="gvResultFinal_OnItemDataBound" OnItemCommand="gvResultFinal_OnItemCommand"
                            OnPageIndexChanged="gvResultFinal_OnPageIndexChanged" Visible="false">
                            <ClientSettings AllowColumnsReorder="false" Scrolling-FrozenColumnsCount="1" Scrolling-AllowScroll="true"
                                Scrolling-UseStaticHeaders="true" EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <GroupingSettings CaseSensitive="false" />
                            <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No&nbsp;Record&nbsp;Found.
                                    </div>
                                </NoRecordsTemplate>
                                <GroupHeaderItemStyle Font-Bold="true" />
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="Source" DefaultInsertValue="" HeaderText="Source"
                                        Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                            <asp:HiddenField ID="hdnResultHTML" runat="server" Value='<%#Eval("ResultHTML") %>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                            <asp:HiddenField ID="hdnReviewedStatus" runat="server" Value='<%#Eval("ReviewedStatus") %>' />
                                            <asp:HiddenField ID="hdnReviewedComments" runat="server" Value='<%#Eval("ReviewedComments") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="OrderDate" DefaultInsertValue="" HeaderText="Order Date"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LabNo" DefaultInsertValue="" Visible="true">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblLabHeader" runat="server" Text='<%$ Resources:PRegistration, LABNO%>' />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ManualLabNo" Visible="true">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblManualLabHeaer" runat="server" Text="Manual Lab No" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblManualLabNo" runat="server" Text='<%#Eval("ManualLabNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, ipno%>'
                                        Visible="True">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, regno%>'
                                        Visible="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultDate" runat="server" Text='<%#Eval("RegistrationNo") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Provider" DefaultInsertValue="" HeaderText='<%$ Resources:PRegistration, Provider%>'
                                        Visible="true">
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
                                    <telerik:GridTemplateColumn UniqueName="ServiceName" DefaultInsertValue="" HeaderText="Investigation"
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
                                    <telerik:GridTemplateColumn UniqueName="Result" DefaultInsertValue="" HeaderText="Result"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblresult" runat="server" Visible="false" Text='<%#Eval("Result") %>' />
                                            <asp:LinkButton ID="lnkResult" runat="server" Text='<%#Eval("Result") %>' CommandName="Result"
                                                CommandArgument="None" Visible="true" ForeColor="Black" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="print" DefaultInsertValue="" HeaderText="Print"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkprint" runat="server" CommandName="Print" CommandArgument="None"
                                                Text="Print" ForeColor="Black" />
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gvLabDetailsXaxis" EventName="ItemDataBound" />
                    <asp:PostBackTrigger ControlID="gvResultFinal" />
                </Triggers>
            </asp:UpdatePanel>
                </div>
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


</asp:Content>
