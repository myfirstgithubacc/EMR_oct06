<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PACDashboard.aspx.cs" Inherits="EMR_PACEMR_PACDashboard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="/Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="/Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />

    <style>
        /*.RadGrid ,.RadGrid * { overflow: inherit !important; margin: 0 !important; padding: 0 !important}
        div#ctl00_ContentPlaceHolder1_gvEncounter_GridHeader {
            margin-right: 0 !important;
        }
        colgroup col { width: auto !important;}*/

        .hidden-pacdashboard { display: none;}
        .form-pac .label-text {
            color: #333 !important;
            font-size: 12px !important;
            font-weight: normal !important;
        }

        table#ctl00_ContentPlaceHolder1_rblPACStatus td { padding-top: 0;}

        colgroup col + col { width: auto !important;}

        div#ctl00_ContentPlaceHolder1_gvEncounter_GridData { overflow: inherit !important;}
}
    </style>
    <script type="text/javascript">
        function showMenu(e, menu, RegId, EncId, RegNo, EncNo, PatName, DocName, DoctorId, OTRequestID, RefDoctorId, RefEncounterId, rowIdx) {
            $get('<%=hdnMnuRegId.ClientID%>').value = RegId;
            $get('<%=hdnMnuEncId.ClientID%>').value = EncId;
            $get('<%=hdnMnuRegNo.ClientID%>').value = RegNo;
            $get('<%=hdnMnuEncNo.ClientID%>').value = EncNo;
            $get('<%=hdnMnuPatName.ClientID%>').value = PatName;
            $get('<%=hdnMnuDocName.ClientID%>').value = DocName;
            $get('<%=hdnMnuDoctorId.ClientID%>').value = DoctorId;
            $get('<%=hdnMnuOTRequestID.ClientID%>').value = OTRequestID;
            $get('<%=hdnMnuRefDoctorId.ClientID%>').value = RefDoctorId;
            $get('<%=hdnMnuRefEncounterId.ClientID%>').value = RefEncounterId;

            var menu = $find("<%=menuStatus.ClientID %>");

            menu.show(e);
            var grid = $find("<%=gvEncounter.ClientID%>").get_masterTableView();
            var Index = rowIdx;
            grid.selectItem(Index);
            var row = grid.get_dataItems()[Index];

        }
    </script>
    <div class="container-fluid header_main form-group">
        <div class="col-md-2 col-sm-3">
            <h2>
                <asp:Label ID="lblHeader" runat="server" Text="PAC Lists"></asp:Label>
            </h2>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row form-pac">
            <div class="col-md-2">
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label ID="lblSpecilization" runat="server" Text="Specilization" />
                    </div>
                    <div class="col-md-8">
                        <telerik:RadComboBox ID="ddlSpecilization" runat="server" AppendDataBoundItems="true"
                            Filter="Contains" Height="300px" AutoPostBack="true" DropDownWidth="300px"
                            OnSelectedIndexChanged="ddlSpecilization_SelectedIndexChanged" Width="100%" />
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <div class="row">
                    <div class="col-md-4">
                        <asp:Label ID="lblDoctor" runat="server" Text="Doctor" CssClass="label-text" />
                    </div>
                    <div class="col-md-8">
                        <telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" Filter="Contains"
                            runat="server" ItemsPerRequest="10" EnableVirtualScrolling="true" TabIndex="0"
                            AutoPostBack="false" MaxHeight="120px" Width="100%" />
                    </div>
                </div>
            </div>
            <div class="col-md-2">
                <div class="row">
                    <div class="col-md-3">
                        <asp:Label ID="lblPACStatus" runat="server" Text="PACStatus" />
                    </div>
                    <div class="col-md-9">
                        <asp:RadioButtonList ID="rblPACStatus" runat="server" RepeatDirection="Horizontal" CssClass="table-condensed table-noborder">

                            <asp:ListItem Text="Pending" Value="0" Selected="True" />
                            <asp:ListItem Text="Done" Value="1" />
                        </asp:RadioButtonList>
                    </div>
                </div>
            </div>
            <div class="col-md-2" runat="server" id="dvDateRange">
                <div class="row">
                    <div class="col-md-12">
                        <div class="pull-left" style="width: 50%;">
                        <telerik:RadComboBox ID="ddlName" runat="server" AppendDataBoundItems="true" 
                            Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Reg No" Value="R" />
                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                <telerik:RadComboBoxItem Text="Mobile No." Value="MN" />
                            </Items>
                        </telerik:RadComboBox>
                            </div>

                        <div class="pull-left" style="width: 50%;">
                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="100%" SkinID="textbox"
                            Visible="false" />
                        <asp:TextBox ID="txtSearchN" SkinID="textbox" Width="100%" runat="server" Text=""
                            MaxLength="10" onkeyup="return validateMaxLength();" />
                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                            FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                    </div>
                    
                </div>
            </div>
            </div>

            <div class="col-md-3">
                <div class="row">
                    <div class="col-md-6">
                        <asp:Label ID="lblFromDate" runat="server" Text="From" />
                        <telerik:RadDatePicker ID="dtpFromDate" runat="server" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                            Width="80px" DateInput-ReadOnly="false" Style="float: none;" />
                    </div>
                  
                    <div class="col-md-6">
                        <asp:Label ID="lblToDate" runat="server" Text="To" />
                         <telerik:RadDatePicker ID="dtpToDate" runat="server" DatePopupButton-Visible="false" ShowPopupOnFocus="true"
                            Width="80px" DateInput-ReadOnly="false" Style="float: none;" />
                    </div>
                </div>
            </div>
            <div class="col-md-1">
                <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" OnClick="btnFilter_Click" Text="Filter" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">

                <div style="width: 98%;">

                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                    </Windows>
                </telerik:RadWindowManager>

                    
                <telerik:RadGrid ID="gvEncounter" runat="server" RenderMode="Lightweight" Skin="Office2007"
                    PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                    AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                    AllowPaging="true" AllowAutomaticDeletes="false" Height="480px"
                    ShowFooter="false" PageSize="20" AllowCustomPaging="true" AllowSorting="true"
                    OnPageIndexChanged="gvEncounter_PageIndexChanged" OnItemDataBound="gvEncounter_ItemDataBound"
                    OnPreRender="gvEncounter_PreRender" OnItemCommand="gvEncounter_ItemCommand">
                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                            AllowColumnResize="false" />
                        <Scrolling AllowScroll="True" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    </ClientSettings>
                    <PagerStyle ShowPagerText="true" />
                    <MasterTableView TableLayout="Auto" GroupLoadMode="Client">
                        <SortExpressions>
                            <telerik:GridSortExpression FieldName="PatientName" SortOrder="Descending" />
                        </SortExpressions>
                        <NoRecordsTemplate>
                            <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                        </NoRecordsTemplate>
                        <ItemStyle Wrap="true" />
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="Select" HeaderText="" HeaderStyle-Width="40px"
                                AllowFiltering="false">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnSelect" runat="server" Text="Select" CausesValidation="false"
                                        CommandName="Select" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" ShowFilterIcon="false"
                                DefaultInsertValue="" DataField="PatientName" SortExpression="PatientName"
                                HeaderStyle-Width="17%" ItemStyle-Width="17%">
                                <ItemTemplate>
                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                    <asp:HiddenField ID="hdnOTRequestID" runat="server" Value='<%#Eval("OTRequestID")%>' />
                                    <asp:HiddenField ID="hdnIsEmergency" runat="server" Value='<%#Eval("IsEmergency")%>' />
                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                    <asp:HiddenField ID="hdnOTREncounterId" runat="server" Value='<%#Eval("OTREncounterId")%>' />
                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                    <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%#Eval("EncounterNo")%>' />
                                    <asp:HiddenField ID="hdnOTREncounterNo" runat="server" Value='<%#Eval("OTREncounterNo")%>' />
                                    <asp:HiddenField ID="hdnDoctorName" runat="server" Value='<%#Eval("DoctorName")%>' />
                                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                    <asp:HiddenField ID="hdnRefDoctorId" runat="server" Value='<%#Eval("RefDoctorId")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="AgeGender" HeaderText="Age/Gender" ShowFilterIcon="false"
                                DefaultInsertValue="" DataField="AgeGender" AllowFiltering="false" SortExpression="AgeGender"
                                HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("AgeGender")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo" SortExpression="RegistrationNo"
                                HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="RefDoctorName" HeaderText="RefDoctorName" ShowFilterIcon="false"
                                DefaultInsertValue="" DataField="RefDoctorName" SortExpression="RefDoctorName"
                                HeaderStyle-Width="13%" ItemStyle-Width="13%">
                                <ItemTemplate>
                                    <asp:Label ID="lblRefDoctorName" runat="server" Text='<%#Eval("RefDoctorName")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="ServiceName" HeaderText="Service Name"
                                ShowFilterIcon="false" DefaultInsertValue="" DataField="ServiceName" AllowFiltering="false" SortExpression="ServiceName"
                                HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="ProblemDescription" HeaderText="Chief Complaints" ShowFilterIcon="false"
                                DefaultInsertValue="" DataField="ProblemDescription" SortExpression="ProblemDescription"
                                HeaderStyle-Width="22%" ItemStyle-Width="22%">
                                <ItemTemplate>
                                    <asp:Label ID="lblProblemDescription" runat="server" Text='<%#Eval("ProblemDescription")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Diagnosis" HeaderText="Diagnosis" ShowFilterIcon="false"
                                DefaultInsertValue="" DataField="Diagnosis" AllowFiltering="false" SortExpression="Diagnosis"
                                HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label ID="lblDiagnosis" runat="server" Text='<%#Eval("Diagnosis")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="OTRequestDate" HeaderText="OT Req Date"
                                ShowFilterIcon="false" DefaultInsertValue="" DataField="OTRequestDate" SortExpression="OTRequestDate"
                                HeaderStyle-Width="7%" ItemStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label ID="lblOTRequestDate" runat="server" Text='<%#Eval("OTRequestDate")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>

                    </div>
                <telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true"
                    EnableShadows="true" Width="100px" OnItemClick="menuStatus_ItemClick" />
                <asp:HiddenField ID="hdnMnuRegId" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuEncId" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuRegNo" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuEncNo" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuPatName" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuDocName" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuDoctorId" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuOTRequestID" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuRefDoctorId" runat="server" Value="" />
                <asp:HiddenField ID="hdnMnuRefEncounterId" runat="server" Value="" />
            </div>
        </div>
    </div>

</asp:Content>

