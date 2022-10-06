<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PaitentDashboard.aspx.cs" Inherits="EMR_PaitentDashboard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelPatientDashBoard.ascx" %>
<%@ Import Namespace="System.Web.Optimization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/style_patient_dt.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Include/css/jquery.mCustomScrollbar.css" />--%>

    <style>
        body {
            background-color: #ecf0f5;
            font-size: 12px;
        }

        .patientDiv-Table table tr td {
            background: #e0ecf9 none repeat scroll 0 0 !important;
            border: 1px solid #90c0f9;
            color: #4d4d4d;
            font-size: 12px;
            font-weight: 500;
            margin: 0;
            padding: 0.6em 0.2em 0.5em;
        }

        #ctl00_ContentPlaceHolder1_dvVitals td {
            width: 50%;
            padding: 5px;
            border-bottom: 1px solid #eee;
        }

        #ctl00_ContentPlaceHolder1_upnlCategory td, #ctl00_ContentPlaceHolder1_upnlCategory tr {
            padding: 5px;
            border-bottom: 1px solid #eee;
        }

            #ctl00_ContentPlaceHolder1_upnlCategory td input[type=image] {
                width: 20px;
            }

        .redme {
            background: #DD4B39 !important;
            color: #fff !important;
            border-color: #AD281A !important;
        }

        .blueme {
            background: #00C0EF !important;
            color: #fff !important;
            border-color: #06A5C9 !important;
        }

        .greenme {
            background: #00A65A !important;
            color: #fff !important;
            border-color: #047A43 !important;
        }

        .yellowme {
            background: #F39C12 !important;
            color: #fff !important;
            border-color: #BC7407 !important;
        }

        .orangeme {
            background: #F96F2A !important;
            color: #fff !important;
            border-color: #B74005 !important;
        }

        .dgreenme {
            background: #53656F !important;
            color: #fff !important;
            border-color: #31444F !important;
        }
    </style>

    <div>
        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
    </div>

    <div class="container-fluid content-wrapper">

        <div class="col-md-12">
            <div id="divLastPatientVisit" runat="server">
                <h2 class="yellowme"><i class="fa fa-arrow-circle-o-left" aria-hidden="true"></i>Previous  Visit</h2>
                <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px; overflow: auto;">
                    <%--<div >--%>

                    <asp:UpdatePanel ID="upnlCategory" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="rpfCategory" AutoGenerateColumns="false" GridLines="None" runat="server" OnRowDataBound="rpfCategory_RowDataBound" CssClass="table table-bordered">
                                <Columns>
                                    <asp:TemplateField HeaderText="VisitDate">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("VisitDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EncounterNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%# Bind("EncounterNo") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%# Bind("DoctorId") %>' />
                                            <asp:HiddenField ID="hdnAppointmentID" runat="server" Value='<%# Bind("AppointmentID") %>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%# Bind("RegistrationId") %>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%# Bind("EncounterId") %>' />
                                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%# Bind("EncounterNo") %>' />
                                            <asp:HiddenField ID="hdnVisitDate" runat="server" Value='<%# Bind("VisitDate") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DoctorName">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnDoctorName" runat="server" Text='<%# Bind("DoctorName") %>' CommandName="DoctorId" CommandArgument='<%# Bind("DoctorId") %>' Font-Underline="false" ForeColor="Navy" Enabled="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Diagnosis">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiagnosis" runat="server" Text='<%# Bind("Diagnosis") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chief Complaint">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProblem" runat="server" Text='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("Problem"))) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VisitType">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Text='<%# Bind("VisitType") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Case Sheet">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnCaseSheet" runat="server" ImageUrl="~/imagesHTML/news.png" ToolTip="CaseSheet" OnClick="ibtnLabResult_Click" Width="20px" Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Past&nbsp;Clinical Notes">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnClinicalNotes" runat="server" ImageUrl="~/imagesHTML/notepad.png" ToolTip="Past Clinical Notes" OnClick="imgPastClinicalNote_Click" Width="20px" Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OP Summary">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnOPSummary" runat="server" ImageUrl="~/Icons/registration.png" ToolTip="OP Summary" OnClick="btnPrintReport_OnClick" Width="20px" Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discharge Summary">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDischargeSummary" runat="server" ImageUrl="~/Icons/registration.png" ToolTip="Discharge Summary" OnClick="btnPrintPDFNew_Click" Width="20px" Height="20px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%--<asp:GridView ID="GridView1"  runat="server"></asp:GridView>--%>
                            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                                    <telerik:RadWindow ID="RadWindowPopup" runat="server" Top="10px" Left="40px" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hdnSummaryID" runat="server" />
                    <asp:UpdatePanel runat="server" ID="UpdatePanel7" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Timer ID="Timer1" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer1" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="rptDoctorVisitPage" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rptPages" EventName="ItemCommand" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand" OnItemDataBound="rptPages_ItemDataBound">
                                <%--<asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">--%>

                                <HeaderTemplate>
                                    <h6>
                                        <table>
                                            <tr class="text">
                                                <td></td>
                                                <td>

                                                    <%--                             <h6>
                                <ul class="pagination">
                                    <li><a href="#" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>
                                    <li class="active"><a href="#">1 <span class="sr-only">(current)</span></a></li>
                                    <li><a href="#">2</a></li>
                                    <li><a href="#">3</a></li>
                                    <li><a href="#" aria-label="Next"><span aria-hidden="true">»</span></a></li>
                                </ul>
                           </h6>  --%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<div style="background-color:skyblue; color:white; border:1px solid;"> ForeColor="#3399ff"--%>
                                    <%-- <asp:Panel ID="pnl" runat="server" Height="20px" Width="25px">--%>

                                    <%--         <asp:LinkButton ID="lbtnPage" runat="server" Text="<%# Container.DataItem %>" CommandArgument="<%# Container.DataItem %>" 
                         CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>--%>


                                    <asp:LinkButton ID="lbtnPage"
                                        CommandName="Page"
                                        CommandArgument="<%#
                         Container.DataItem %>"
                                        CssClass='page_disabled'
                                        runat="server" OnClick="lbtnPage_Click"
                                        Text="<%# Container.DataItem %>">
                                    </asp:LinkButton>


                                    <%--<asp:LinkButton ID="lbtnPage" 
                         CommandName="Page"
                         CommandArgument="<%#
                         Container.DataItem %>"
                         CssClass="text"
                         runat="server" OnClick="lbtnPage_Click"
                              Text="<%# Container.DataItem %>" OnClientClick="return valid(this);">
                     </asp:LinkButton>&nbsp;--%>
                                    <%-- </asp:Panel>--%>

                                    <%--</div>--%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </td>
                    </tr>
                    </table></h6>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ContentTemplate>
                    </asp:UpdatePanel>


                </div>
            </div>
        </div>



    </div>
    <div class="content-wrapper container-fluid">
        <div class="col-md-4">
            <h2 class="redme"><i class="fa fa-buysellads" aria-hidden="true"></i>Allergic To</h2>
            <div class="border-box" style="max-height: 190px; min-height: 190px; overflow: auto;">

                <%-- <div class="col-md-4">--%>
                <%-- <img class="max-width-thumb" alt="" src="../img/MensEyewearThumb.jpg">--%>
                <%--<asp:Image ID="imgPatient" runat="server" CssClass="image_width" />--%>
                <%--</div>--%>
                <div class="col-md-12">
                    <div class="caption">
                        <div class="row line-height">

                            <asp:GridView ID="gvAllergy" runat="server" Width="100%"
                                Height="100%" ShowHeader="false" AutoGenerateColumns="False" AllowPaging="false" CssClass="table table-bordered">
                                <Columns>
                                    <asp:TemplateField HeaderText="Allergy Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAllergyName" runat="server" Text='<%#Eval("AllergyName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                                <HeaderStyle HorizontalAlign="Left" />
                                <RowStyle Wrap="False" />
                            </asp:GridView>



                        </div>


                    </div>
                </div>
            </div>



        </div>
        <div class="col-md-8" id="divMedicinesGiven" runat="server">
            <h2 class="blueme"><i class="fa  fa-medkit" aria-hidden="true"></i>Latest Medicines Given</h2>
            <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px; overflow: auto;">
                <asp:GridView ID="gvPrescribedMedicine" runat="server" AutoGenerateColumns="false" GridLines="None" AllowPaging="True"
                    ShowHeader="false" CssClass="table table-bordered" PageSize="10"
                    OnPageIndexChanging="gvPrescribedMedicine_PageIndexChanging" OnRowDataBound="gvPrescribedMedicine_DataBound">
                    <PagerStyle />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="30px">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtnPrescribedMedicine" Enabled="false" runat="server" ImageUrl="~/Img/drug.png" ToolTip="Medicine Details" OnClick="ibtnPrescribedMedicine_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Label ID="lblIndentDate" runat="server" Text='<%# Bind("IndentDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Label ID="lblDoctorName" runat="server" Text='<%# Bind("DoctorName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>' />

                                <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Bind("DetailsId") %>' />
                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Bind("ItemId") %>' />
                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Bind("IndentId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%# Bind("PrescriptionDetail") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:CheckBox ID="chkIsNoFollowUpRequired" Text="No follow-up required" Visible="false" CssClass="checkbox" runat="server" />
    <telerik:RadComboBox ID="DpLanguageMasterPrint" OnSelectedIndexChanged="DpLanguageMasterPrint_SelectedIndexChanged" AutoPostBack="true" runat="server" Visible="false" DataValueField="Abbreviation" DataTextField="Language" Width="80px" />
    <asp:HiddenField ID="hdnRegistrationID" runat="server" Value="" />
    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="" />
    <asp:HiddenField ID="hiddenencounterid" runat="server" Value="" />
    <asp:HiddenField ID="hiddenencounterno" runat="server" Value="" />
    <asp:HiddenField ID="hdnDoctorImage" runat="server" />
    <asp:HiddenField ID="hdnReportContent" runat="server" />
    <div class="content-wrapper container-fluid">

        <div class="col-md-4">
            <div id="divViatalsGraph" runat="server">
                <h2 class="greenme"><i class="fa  fa-bar-chart" aria-hidden="true"></i>Vitals Parameter Graph</h2>
                <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px; overflow: auto;">
                    <asp:GridView ID="dvVitals" runat="server"
                        AutoGenerateColumns="false"
                        GridLines="None" AllowPaging="True" PageSize="10"
                        OnPageIndexChanging="dvVitals_PageIndexChanging"
                        ShowHeader="false"
                        OnRowDataBound="dvVitals_DataBound">
                        <%-- <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />--%>
                        <%-- <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />--%>
                        <%-- <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />--%>
                        <%-- <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />--%>
                        <PagerStyle />

                        <Columns>

                            <asp:TemplateField HeaderText="Id">
                                <ItemTemplate>

                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Registration Id">
                                <ItemTemplate>

                                    <asp:Label ID="lblRegistrationId" runat="server" Text='<%# Bind("RegistrationId") %>' />

                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Encounter Id">
                                <ItemTemplate>

                                    <%-- <asp:Label ID="lblWT" runat="server" Text='<%# Bind("WT") %>'></asp:Label>--%>
                                    <asp:Label ID="lblEncounterId" runat="server" Text='<%# Bind("EncounterId") %>' />

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>

                                    <asp:Label ID="lblVitalEntryDate" runat="server" Text='<%# Bind("VitalEntryDate") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VitalId">
                                <ItemTemplate>

                                    <asp:Label ID="lblVitalId" runat="server" Text='<%# Bind("VitalId") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Vital">
                                <ItemTemplate>

                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# Bind("DisplayName") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Value">
                                <ItemTemplate>

                                    <asp:HyperLink ID="hplVitalValue" runat="server" Text='<%# Bind("VitalValue") %>'></asp:HyperLink>

                                </ItemTemplate>
                            </asp:TemplateField>



                        </Columns>

                        <%--  <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                    </asp:GridView>
                    <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                    <asp:HiddenField ID="hdnVitalName" runat="server" />
                </div>
            </div>

        </div>

        <div class="col-md-8" id="divLabTest" runat="server">
            <h2 class="orangeme"><i class="fa  fa-flask" aria-hidden="true"></i>Recent Lab Investigations</h2>
            <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px; overflow: auto;">

                <asp:GridView ID="gvLabTest" runat="server" CssClass="table  table-bordered" Width="100%"
                    Height="100%" ShowHeader="false" AutoGenerateColumns="False" AllowPaging="false">
                    <Columns>
                        <asp:TemplateField HeaderText="OrderDate">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ServiceName">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DoctorName">
                            <ItemTemplate>
                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("DoctorName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="status" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="header-center"
                            ItemStyle-Width="180px">
                            <ItemTemplate>
                                <%--<asp:LinkButton ID="lblStatus" runat="server" CssClass="text-center" Text='<%#Eval("status")%>' CommandName="Result" CommandArgument="None"  />--%>

                                <asp:HyperLink ID="hdnresult" Text='<%#Eval("status")%>' runat="server" Target="_blank"
                                    Enabled='<%#Eval("StatusCode").Equals("RF") || Eval("StatusCode").Equals("RP") ? true:false %>'
                                    NavigateUrl='<%# "/LIS/Phlebotomy/previewResult.aspx?SOURCE="+ Eval("Source") 
                                        + "&DIAG_SAMPLEID="+ Server.UrlEncode(Eval("DiagSampleID").ToString())
                                        + "&SERVICEID=" + Server.UrlEncode(Eval("ServiceId").ToString())
                                        + "&AgeInDays=" + Server.UrlEncode(Eval("AgeGender").ToString())
                                        + "&StatusCode=" + Server.UrlEncode(Eval("StatusCode").ToString())
                                        + "&ServiceName=" + Server.UrlEncode(Eval("ServiceName").ToString())
                                         + "&LabNo=" + Server.UrlEncode(Eval("LabNo").ToString())%>' />

                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" Visible="FALSE">
                            <ItemTemplate>
                                <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle HorizontalAlign="Left" />
                    <RowStyle />
                </asp:GridView>


                <%--<div class="rd_test">
								<div class="media-left"> <a href="#"> <img alt="64x64" data-src="holder.js/64x64" class="media-object" style="width:24px; height: 24px;" src="../img/flask.png" data-holder-rendered="true"> </a> </div>
								<div class="media-body"> <h4 class="media-heading">Media heading</h4><p>Media heading Cras sit amet nibh liberou.</p></div>
							</div>--%>
            </div>
        </div>




    </div>
    <div class="content-wrapper container-fluid">

        <div class="col-md-12">
            <div class="row">
                <%--  <div class="col-md-4" id="divSugarGraph" runat="server">
                    <h2>Blood Sugar Graph </h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">


                    </div>
                </div>--%>
                <%-- <div class="col-md-4" id="divRadiologyTest" runat="server">
                    <h2>Radiology Test</h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">--%>
                <div class="col-md-12" id="divRadiologyTest" runat="server">
                    <h2 class="dgreenme"><i class="fa fa-search-plus" aria-hidden="true"></i>Radiology Investigations</h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px; overflow: auto;">

                        <asp:GridView ID="gvRadiologyTest" runat="server" Width="100%" CssClass="table table-bordered"
                            Height="100%" ShowHeader="false" AutoGenerateColumns="False" AllowPaging="false">
                            <Columns>
                                <asp:TemplateField HeaderText="OrderDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ServiceName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="status" ItemStyle-VerticalAlign="Top" HeaderStyle-CssClass="header-center"
                                    ItemStyle-Width="180px">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="lblStatus" runat="server" CssClass="text-center" Text='<%#Eval("status")%>' CommandName="Result" CommandArgument="None"  />--%>
                                        <asp:HyperLink ID="hdnresult" Text='<%#Eval("status")%>' runat="server" Target="_blank"
                                            Enabled='<%#Eval("StatusCode").Equals("RF") || Eval("StatusCode").Equals("RP") ? true:false %>'
                                            NavigateUrl='<%# "/LIS/Phlebotomy/previewResult.aspx?SOURCE="+ Eval("Source") 
                                        + "&DIAG_SAMPLEID="+ Server.UrlEncode(Eval("DiagSampleID").ToString())
                                        + "&SERVICEID=" + Server.UrlEncode(Eval("ServiceId").ToString())
                                        + "&AgeInDays=" + Server.UrlEncode(Eval("AgeGender").ToString())
                                        + "&StatusCode=" + Server.UrlEncode(Eval("StatusCode").ToString())
                                        + "&ServiceName=" + Server.UrlEncode(Eval("ServiceName").ToString())
                                         + "&LabNo=" + Server.UrlEncode(Eval("LabNo").ToString())%>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender") %>' />
                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#Eval("StatusCode") %>' />
                                        <asp:Label ID="lblDiagSampleID" runat="server" Text='<%#Eval("DiagSampleID") %>' />
                                        <asp:Label ID="lblServiceId" runat="server" Text='<%#Eval("ServiceId") %>' />
                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                        <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                            <RowStyle Wrap="False" />
                        </asp:GridView>

                        <%--		<div class="rd_test">
								<div class="media-left"> <a href="#"> <img alt="64x64" data-src="holder.js/64x64" class="media-object" style="width:24px; height: 24px;" src="../img/flask.png" data-holder-rendered="true"> </a> </div>
								<div class="media-body"> <h4 class="media-heading">Media heading</h4><p>Media heading Cras sit amet nibh liberou.</p></div>
							</div>--%>

                        <%--	<div class="rd_test border-btm">
								<div class="media-left"> <a href="#"> <img alt="64x64" data-src="holder.js/64x64" class="media-object" style="width:24px; height: 24px;" src="../img/flask.png" data-holder-rendered="true"> </a> </div>
								<div class="media-body"> <h4 class="media-heading">Media heading</h4><p>Media heading Cras sit amet nibh liberou.</p></div>
							</div>--%>
                    </div>
                </div>
            </div>
        </div>
        <%-- < Start EMR_PaitentDashboard Board Graph >--%>

        <%--  <div class="col-md-6">


            <h2 class="blueme"><i class="fa  fa-flask" aria-hidden="true"></i>&nbsp; 
                <asp:Label runat="server" ID="lblLabHeader" Text="Haemoglobin"></asp:Label>
            </h2>


            <div class="border-box" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px;">

                   <div id="myCarousel" class="carousel slide" data-ride="carousel">
                    <ol class="carousel-indicators">
                        <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                        <li data-target="#myCarousel" data-slide-to="1"></li>
                    </ol>
                    <div class="carousel-inner">
                        <div class="item active" runat="server" id="dvSlider1">

                <telerik:RadChart ID="RadChart1" runat="server" Width="700px" Height="190px" Skin="Office2007" data-toggle="modal"
                    data-target="#myModal11">
                    <PlotArea>
                        <YAxis AutoScale="false" MinValue="2" MaxValue="20" Step="2"></YAxis>
                          <XAxis AutoScale="false" IsZeroBased="false"  DataLabelsColumn="EncodedDate">
                                                <Appearance ValueFormat="ShortDate" CustomFormat="dd/MM/yyyy"></Appearance>
                                            </XAxis>
                    </PlotArea>
                    <Legend>
                        <Appearance Position-AlignedPosition="Bottom">
                        </Appearance>
                    </Legend>

                </telerik:RadChart>

                                            <div class="container">
                                <div class="carousel-caption">
                                </div>
                            </div>
                        </div>

                        <div class="item" runat="server" id="dvSlider2">
                            <telerik:RadChart ID="RadChart3" runat="server" Width="600px" Height="190px" Skin="Telerik" data-toggle="modal" data-target="#myModal11">
                                <PlotArea>

                                    <YAxis AutoScale="false" MinValue="2" MaxValue="20" Step="2"></YAxis>
                                    <Appearance>
                                        <FillStyle MainColor="YellowGreen" />
                                    </Appearance>
                                </PlotArea>
                                <Legend>
                                    <Appearance Position-AlignedPosition="Bottom">
                                    </Appearance>
                                </Legend>

                            </telerik:RadChart>


                            <div class="container">
                                <div class="carousel-caption">
                                </div>
                            </div>

                        </div>
                    </div>


                    <a class="left carousel-control" href="#myCarousel" data-slide="prev"><span class="glyphicon glyphicon-chevron-left"></span></a>
                    <a class="right carousel-control" href="#myCarousel" data-slide="next"><span class="glyphicon glyphicon-chevron-right"></span></a>
                </div>
            </div>

        </div>--%>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="hdnTemplateData" runat="server" />
                <asp:HiddenField ID="hdnDoctorSignID" runat="server" />
                <asp:HiddenField ID="hdnSignJuniorDoctorID" runat="server" />

                <asp:HiddenField ID="hdnFinalize" runat="server" />
                <asp:HiddenField ID="hdnEncodedBy" runat="server" />
                <asp:HiddenField ID="hdnFrom" runat="server" />
                <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                <asp:HiddenField ID="DischargeSummaryCode" runat="server" />

            </ContentTemplate>
        </asp:UpdatePanel>
        <%--   <div class="col-md-4" id="div1" runat="server">
            <h2 class="blueme"><i class="fa  fa-flask" aria-hidden="true"></i>&nbsp; 
                <asp:Label runat="server" ID="Label1" Text=""></asp:Label></h2>
            <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px;">
                <telerik:RadChart ID="RadChart4" runat="server" Width="450px" Height="190px" Skin="Office2007" data-toggle="modal"
                    data-target="#myModal12">
                    <PlotArea>
                        <YAxis AutoScale="false" MinValue="0" MaxValue="2" Step=".5"></YAxis>

                    </PlotArea>
                    <Legend>
                        <Appearance Position-AlignedPosition="Bottom">
                        </Appearance>
                    </Legend>

                </telerik:RadChart>

            </div>
        </div>--%>

        <%--   <div class="col-md-4" id="div2" runat="server">
            <h2 class="blueme"><i class="fa  fa-flask" aria-hidden="true"></i>&nbsp; 
                <asp:Label runat="server" ID="Label22" Text=""></asp:Label></h2>
            <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px;">
                <telerik:RadChart ID="RadChart5" runat="server" Width="450px" Height="190px" Skin="Office2007" data-toggle="modal"
                    data-target="#myModal13">
                    <PlotArea>
                        <YAxis AutoScale="false" MinValue="50" MaxValue="150" Step="20"></YAxis>
                    </PlotArea>
                    <Legend>
                        <Appearance Position-AlignedPosition="Bottom">
                        </Appearance>
                    </Legend>

                </telerik:RadChart>

            </div>
        </div>--%>

        <%--<div class="col-md-4" id="div3" runat="server">
            <h2 class="blueme"><i class="fa  fa-flask" aria-hidden="true"></i>&nbsp; 
                 <asp:Label runat="server" ID="Label33" Text=""></asp:Label></h2>
            <div class="border-box mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px;">
                <telerik:RadChart ID="RadChart6" runat="server" Width="450px" Height="190px" Skin="Office2007" data-toggle="modal"
                    data-target="#myModal14">
                    <PlotArea>
                        <YAxis AutoScale="false" MinValue="50" MaxValue="200" Step="20"></YAxis>
                    </PlotArea>
                    <Legend>
                        <Appearance Position-AlignedPosition="Bottom">
                        </Appearance>
                    </Legend>

                </telerik:RadChart>

            </div>
        </div>--%>
        <%-- END DashBoard Graph--%>
    </div>
    <%-- <script src="../Include/JS/jquery1.6.4.min.js"></script>
    <script src="../../Include/js/jquery.mCustomScrollbar.concat.min.js"></script>
    --%>
    <triggers>
        <asp:AsyncPostBackTrigger ControlID="ibtnLabResult" EventName="CLICK" />
    </triggers>

    <script type="text/javascript" language="javascript">
        function setVitalValue(val, valName) {
            $get('<%=hdnVitalvalue.ClientID%>').value = val;
            $get('<%=hdnVitalName.ClientID%>').value = valName;
            var oWnd = window.open("/EMR/Vitals/Vitalgraph.aspx?Value=" + $get('<%=hdnVitalvalue.ClientID%>').value +
                                    "&Name=" + $get('<%=hdnVitalName.ClientID%>').value, "RadWindowForNew");
            oWnd.setSize(500, 320)
            oWnd.center();
            oWnd.VisibleStatusbar = "false";
            oWnd.set_status(""); // would like to remove statusbar, not just blank it
        }
        function btnPastClinicalNote() {

            var hdnRegistrationID = document.getElementById('<%=hdnRegistrationID.ClientID%>').value;
            var hdnRegistrationNo = document.getElementById('<%=hdnRegistrationNo.ClientID%>').value;
            var hdnEncounterid = document.getElementById('<%=hiddenencounterid.ClientID%>').value;
            var hdnEncounterNo = document.getElementById('<%=hiddenencounterno.ClientID%>').value;

            var x = screen.width / 2 - 1300 / 2;
            var y = screen.height / 2 - 550 / 2;
            var popup;

            popup = window.open("/WardManagement/VisitHistory.aspx?Regid=" + hdnRegistrationID + "&RegNo=" + hdnRegistrationNo + "&EncId=" + hdnEncounterid + "&EncNo=" + hdnEncounterNo + "&FromWard=Y&OP_IP=I&Category=PopUp&CloseButtonShow=Yes&FromEMR=1", "Popup", "height=550,width=1300,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
            popup.focus();

            document.getElementById("mainDIV").style.opacity = "0.5";





            popup.onunload = function () {


                document.getElementById("mainDIV").style.opacity = "";

            }

            return false
        }

        function ShowPAgePrint(Url) {

            var x = screen.width / 2 - 1300 / 2;
            var y = screen.height / 2 - 550 / 2;
            var popup;

            popup = window.open(Url, "Popup", "height=550,width=1300,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

            popup.focus();
            document.getElementById("mainDIV").style.opacity = "0.5";
            popup.onunload = function () {


                document.getElementById("mainDIV").style.opacity = "";
            }

            return false
        }
    </script>
</asp:Content>

