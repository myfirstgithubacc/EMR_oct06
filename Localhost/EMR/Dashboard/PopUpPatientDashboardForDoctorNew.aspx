<%@ Page Title="Last OPD Summary" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PopUpPatientDashboardForDoctorNew.aspx.cs" Inherits="EMR_Dashboard_PopUpPatientDashboardForDoctorNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%--<%@ Import Namespace="System.Web.Optimization" %>--%>
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
  

    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Maximize" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>



    <div class="container-fluid1">
        <div class="row header_main">
             <div class="col-md-6 col-sm-6 col-xs-6">
                         <asp:Label ID="lblLastEncounterDate" runat="server" Text="" Font-Bold="false" CssClass="visit-date" />
                     </div>
            <div class="col-md-6 col-sm-6 col-xs-6 text-right">
                <asp:Button ID="btnSave" Text="Copy to Current Visit" CssClass="btn btn-primary" runat="server" OnClick="btnSaveDashboard_OnClick" />

                <asp:Button ID="btnClose" runat="server" Text="X" Font-Bold="true" CssClass="btn btn-primary" OnClientClick="returnToParent()" />
            
            </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                         <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <%--SaveToClose--%>
                            <asp:HiddenField ID="hdnSaveToClose" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("ID") %>' />
                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                            <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP") %>' />
                            <asp:HiddenField ID="hdnEncounterStatusName" runat="server" Value='<%#Eval("EncounterStatusName") %>' />
                            <%--<asp:Label ID="lblEncounterDate" runat="server" SkinID="label" Text='<%#Eval("EncounterDate") %>' />--%>
                            <asp:Label ID="lblDoctor" runat="server" Text='<%#Eval("Doctor") %>' />

                            <%--<asp:GridView ID="gvEncounters" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                            Width="100%" ShowHeader="true" AllowPaging="false" PageSize="10" OnRowCommand="gvEncounters_OnRowCommand"
                                            OnRowDataBound="gvEncounters_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEncounterNo" runat="server" CommandName="SelectEncounter"
                                                            CommandArgument='<%#Eval("EncounterNo") %>' Text='<%#Eval("EncounterNo") %>'
                                                            CausesValidation="false" Font-Underline="false" ToolTip="Click to view last OPD details" />
                                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("ID") %>' />
                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                        <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP") %>' />
                                                        <asp:HiddenField ID="hdnEncounterStatusName" runat="server" Value='<%#Eval("EncounterStatusName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterDate%>' HeaderStyle-Width="135px" ItemStyle-Width="135px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterDate" runat="server" SkinID="label" Text='<%#Eval("EncounterDate") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doctor">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoctor" runat="server" SkinID="label" Text='<%#Eval("Doctor") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               
                                            </Columns>
                                        </asp:GridView>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                     </div>
            </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-5 box-col-checkbox p-t-b-5">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkSelectDeselectAll" runat="server" Text="Select / Deselect All"
                            onClick="change();" Style="margin: 0 0 0 5px;" />
                        <asp:UpdatePanel ID="btncheckupdate" runat="server"> 
                            <ContentTemplate> 
                         <asp:Button ID="btnCheck" runat="server" Text="btnCheck" OnClick="btnCheck_Click" Style="display:none;" />
                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                
            </div>

            <div class="col-md-6 col-sm-6 col-xs-7 p-t-b-5">
                <asp:Label ID="lblMessage" runat="server" CssClass="MessageGreen" Text="" />
            </div>


        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="border-radius:5px;border:1px solid #ccc;overflow:hidden;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-gray">
                                <b>
                                    <asp:Label ID="Label20" runat="server" Text="Chief Complaints" />
                                            <asp:Label ID="lblChiefMessage" runat="server" Text="" />
                                </b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 gridview">
                                <asp:Panel ID="Panel1" runat="server">
                                                <div class="emrPWhite-innerBottom01">
                                                    <asp:GridView ID="gvProblemDetails" runat="server" Width="100%"
                                                        Height="100%" ShowHeader="false" OnRowDataBound="gvProblemDetails_RowDataBound"
                                                        AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkProblemSelectAll" runat="server" SkinID="checkbox" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Rows="3" Width="100%"
                                                                        Style="overflow: auto; word-wrap: break-word; font-size: 14px; height: 135px; resize: none;"
                                                                        Text='<%#Eval("ProblemDescription")%>' onkeyup="return MaxLenTxt(this,4000);" />
                                                                    <asp:HiddenField ID="hdnProblemId" runat="server" Value='<%#Eval("Id")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <RowStyle Wrap="False" />
                                                    </asp:GridView>
                                                </div>
                                            </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <div class="emrPart" id="trChiefComplaints" runat="server">
                                <script type="text/javascript">
                                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);
                                </script>

                                <div class="panel-group">
                                    

                    <!-- Chief Complaints Part Ends -->
                    <asp:UpdatePanel ID="UpdatePanel30" runat="server">
                        <ContentTemplate>
                            <div class="panel panel-default emrPart" id="trHistory" runat="server">

                                <!-- History Part Start -->
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="lblHistoryMessage" runat="server" />History of Present Illness
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="Panel2" runat="server">
                                                    <div class="emrHisWhite-innerBottom">
                                                        <asp:Panel ID="Panel3" runat="server">
                                                            <asp:GridView ID="gvHistory" SkinID="APgridview" runat="server" AutoGenerateColumns="false"
                                                                ShowHeader="false" OnRowDataBound="gvHistory_OnDataBinding" Width="100%"
                                                                Height="100%">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkHistorySelectAll" runat="server" SkinID="checkbox" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Rows="3" Width="100%"
                                                                                Style="overflow: auto; word-wrap: break-word; font-size: 14px; height: 135px; resize: none;"
                                                                                Text='<%#Eval("TemplateName")%>' onkeyup="return MaxLenTxt(this,8000);" />
                                                                            <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("DetailId")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <!-- History Part Ends -->

                            </div>
                            <!--paneldefault1-->


                            <div class="panel panel-default emrPart" id="trExamination" runat="server">
                                <!-- Examination Part Start -->
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="lblExamMessage" runat="server" />Examination
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="Panel10" runat="server">
                                                    <div class="emrHisWhite-innerBottom">
                                                        <asp:Panel ID="Panel15" runat="server">
                                                            <asp:GridView ID="gvExamination" SkinID="APgridview" runat="server" AutoGenerateColumns="false"
                                                                ShowHeader="false" OnRowDataBound="gvExamination_OnDataBinding" Width="100%"
                                                                Height="100%">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkExamSelectAll" runat="server" SkinID="checkbox" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Rows="3" Width="100%"
                                                                                Style="overflow: auto; word-wrap: break-word; font-size: 14px; height: 135px; resize: none;"
                                                                                Text='<%#Eval("TemplateName")%>' onkeyup="return MaxLenTxt(this,8000);" />
                                                                            <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("DetailId")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                            <!--paneldefault2-->

                            <div class="panel panel-default emrPart" id="trPlanOfCare" runat="server">
                                <!-- Examination Part Ends -->
                                <!-- Plan Of Care Part Start -->
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="lblPlanOfCareMessage" runat="server" /><asp:Label ID="Label24" runat="server" Text="Plan Of Care" />
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="Panel13" runat="server">
                                                    <div class="emrHisWhite-innerBottom01">
                                                        <asp:Panel ID="Panel17" runat="server">
                                                            <asp:GridView ID="gvPlanOfCare" SkinID="APgridview" runat="server" AutoGenerateColumns="false"
                                                                ShowHeader="false" OnRowDataBound="gvPlanOfCare_OnDataBinding" Width="100%" Height="100%">
                                                                <Columns>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkPlanOfCareSelectAll" runat="server" SkinID="checkbox" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Rows="3" Width="100%"
                                                                                Style="overflow: auto; word-wrap: break-word; font-size: 14px; height: 135px; resize: none;"
                                                                                Text='<%#Eval("TemplateName")%>' onkeyup="return MaxLenTxt(this,8000);" />
                                                                            <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("DetailId")%>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <!--paneldefault3-->


                            <div class="panel panel-default emrPart" id="trProvisionalDiagnosis" runat="server">
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="Label15" runat="server" Text="Provisional&nbsp;Diagnosis" />
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="Panel12" runat="server">
                                                    <div class="emrPart-White">
                                                        <div class="emrNotesWhite-innerBottom01">
                                                            <asp:UpdatePanel ID="UpdatePanel801" UpdateMode="Conditional" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:GridView ID="gvData" SkinID="PDgridview" runat="server" AutoGenerateColumns="False"
                                                                        ShowHeader="false" Width="100%" OnRowDataBound="gvData_RowDataBound" BackColor="White"
                                                                        BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkProvisionalSelectAll" runat="server" SkinID="checkbox" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Rows="3" Width="100%"
                                                                                        Style="overflow: auto; word-wrap: break-word; font-size: 14px; height: 135px; resize: none;"
                                                                                        Text='<%#Eval("ProvisionalDiagnosis")%>' onkeyup="return MaxLenTxt(this,500);" />
                                                                                    <asp:HiddenField ID="hdnProvisionalDiagnosisID" runat="server" Value='<%#Eval("Id")%>' />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <!--paneldefault4-->


                            <div class="panel panel-default emrPart" id="divDiagnosisDetails" runat="server">
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="Label4" runat="server" Text="Diagnosis" />
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="pnlDiagnosis" runat="server">
                                                    <asp:GridView ID="gvDiagnosisDetails" runat="server" AutoGenerateColumns="false"
                                                        Width="100%" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                                        HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                                        BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CssClass="table table-bordered">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkDiagnosisDetails" runat="server" SkinID="checkbox" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="60px" HeaderStyle-Width="60px"
                                                                HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                                    <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true" HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Primary" HeaderStyle-ForeColor="Black">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%# (Convert.ToString(Eval("PrimaryDiagnosis"))!="True")?"Y":"N" %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <!--paneldefault5-->


                            <div class="panel panel-default emrPart" id="trOrdersAndProcedures" runat="server">
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="Label13" runat="server" Text="Orders&nbsp;And&nbsp;Procedures&nbsp;" />

                                                <asp:Label ID="lbladdfavorders" runat="server" Text="" />
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="Panel5" runat="server">
                                                    <asp:GridView ID="gvOrdersAndProcedures" SkinID="PDgridview" runat="server" AutoGenerateColumns="False"
                                                        ShowHeader="true" Width="100%" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                                        HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                                        BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" OnRowDataBound="gvOrdersAndProcedures_RowDataBound" CssClass="table table-condensed table-bordered table-sm">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                                <HeaderTemplate>
                                                                    <%--OnCheckedChanged="chkb1_CheckedChanged-- add by ss for psri--%>

                                                                    <asp:CheckBox ID="chkb1" runat="server" Text="Select All"  OnCheckedChanged="chkb1_CheckedChanged"  
                                                                        AutoPostBack="true" TextAlign="Left" />
                                                                    <%--onclick="changeorder();"--%>
                                                                    <asp:Button ID="btnorder" runat="server" OnClick="btnorder_Click"  Text="btnorder" Style="display:none;" />
                                                                   
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkOrderSelectAll" runat="server" SkinID="checkbox" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service Name" HeaderStyle-CssClass="header-left" HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                                    <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceID")%>' />
                                                                    <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                                    <asp:HiddenField ID="hdnStat" runat="server" Value='<%#Eval("Stat")%>' /> 
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Instruction" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="header-left"
                                                                HeaderStyle-Width="30%" ItemStyle-Width="30%" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Instruction")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <asp:HiddenField ID="hdndtMerge_Order" runat="server" />
                                                    <asp:HiddenField ID="hdnServiceId" runat="server" />
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                            <!--paneldefault6-->


                            <div class="panel panel-default emrPart" id="trPrescriptions" runat="server">
                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
                                        <ContentTemplate>
                                            <div class="panel-heading">
                                                <asp:Label ID="Label16" runat="server" Text="Prescriptions&nbsp;" />

                                                <asp:Label ID="lbladdfav" runat="server" Text="" />
                                            </div>
                                            <div class="panel-body">
                                                <asp:Panel ID="Panel7" runat="server">
                                                    <asp:UpdatePanel ID="UpdatePanel701" runat="server">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="gvAddList" SkinID="PDgridview" runat="server" AutoGenerateColumns="False"
                                                                ShowHeader="true" Width="100%" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false"
                                                                HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White"
                                                                BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" OnRowDataBound="gvAddList_RowDataBound" CssClass="table table-condensed table-bordered table-sm">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top">
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkprescription" runat="server" Text="Select All" OnClick="prescription();"
                                                                                AutoPostBack="true" TextAlign="Left" />
                                                                            <asp:Button ID="btnPrescription" runat="server" OnClick="btnPrescription_Click" Text="btnPrescription" Style="display:none;" />
   
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkPrecriptionAll" runat="server" SkinID="checkbox" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="4%" ItemStyle-Width="4%"
                                                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                      <%--<asp:TemplateField HeaderText="GenericName" ItemStyle-CssClass="header-left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDurationText" runat="server" Text='<%# Eval("GenericName") %>'
                                                                                Width="100%" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="Drug Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' Width="100%" />
                                                                            <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Eval("IndentId") %>' />
                                                                            <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Eval("DetailsId") %>' />
                                                                            <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                                      <%--      <asp:HiddenField ID="hdnPrescriptionDetail" runat="server" Value='<%# Eval("PrescriptionDetail") %>' />
                                                                            <asp:HiddenField ID="hdnGenericName" runat="server" Value='<%# Eval("GenericName") %>' />--%>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField HeaderText="Prescription Detail" ItemStyle-CssClass="header-left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFrequencyName" runat="server" Text='<%# Eval("PrescriptionDetail") %>'
                                                                                Width="100%" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                                    
                                                                    <asp:TemplateField HeaderText="Dose" HeaderStyle-Width="100px" ItemStyle-CssClass="header-left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDose" runat="server" Text='<% # Eval("Dose")  %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Food Relation" ItemStyle-CssClass="header-left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFoodRelationship" runat="server" Text='<%# Eval("FoodRelationship") %>'
                                                                                Width="100%" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Instruction" ItemStyle-CssClass="header-left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInstruction" runat="server" Text='<%# Eval("Instructions") %>' Width="100%" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <!--paneldefault7-->

                        </ContentTemplate>
                    </asp:UpdatePanel>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

         <div class="row">
            <table width="99%" cellpadding="0" cellspacing="0" css="table table-condensed table-bordered table-sm">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel43" runat="server">
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnItemId" runat="server" />
                                <asp:HiddenField ID="hdnItemName" runat="server" />
                                <asp:HiddenField ID="hdnAllergyType" runat="server" />
                                <asp:HiddenField ID="hdnHistoryRecordId" runat="server" />
                                <asp:HiddenField ID="hdnPreviousTreatmentRecordId" runat="server" />
                                <asp:HiddenField ID="hdnExaminationRecordId" runat="server" />
                                <asp:HiddenField ID="hdnNutritionalStatusRecordId" runat="server" />
                                <asp:HiddenField ID="hdnPlanOfCareRecordId" runat="server" />
                                <asp:HiddenField ID="hdnCostAnalysisRecordId" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        </div>

    <script type="text/javascript">
        autosize(document.querySelectorAll('ctl00_ContentPlaceHolder1_gvProblemDetails_ctl02_editorProblem'));
    </script>

    </ContentTemplate>
    </asp:UpdatePanel>

    
    
    <script type="text/javascript">

        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "ICCAViewerBtn";
                myButton.value = "Processing...";
            }
            return true;
        }
        function MaxLenTxt(TXT, intMax) {
            if (TXT.value.length > intMax) {
                TXT.value = TXT.value.Expandstr(0, intMax);
                alert("Maximum length is " + intMax + " characters only.");
            }
        }

    </script>

    <script type="text/javascript">
        var isSplitterResized = false;
        var SplitterResizeModes = ['AdjacentPane', 'Proportional', 'EndPane'];
        var resizeModeInt = 0;
    </script>

    <script type="text/javascript">
        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }

        function executeCode(evt) {
            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                break;

        }
        evt.returnValue = false;
        return false;
        }


        function returnToParent() {
  
            //create the argument that will be returned to the parent page
            var oArg = new Object();
             var oArg = new Object();
             oArg.SaveToClose = $get('<%=hdnSaveToClose.ClientID%>').value;
             var oWnd = GetRadWindow();
             oWnd.close(oArg);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function change()
        {
            $('#<%=btnCheck.ClientID%>').trigger('click');  
            //document.getElementById('ctl00_ContentPlaceHolder1_btnCheck').click();
        }

        function changeorder() {
            document.getElementById('ctl00_ContentPlaceHolder1_gvOrdersAndProcedures_ctl01_btnorder').click();
        }
        function prescription()
        {
            document.getElementById('ctl00_ContentPlaceHolder1_gvAddList_ctl01_btnPrescription').click();
        }

    </script>


</asp:Content>
