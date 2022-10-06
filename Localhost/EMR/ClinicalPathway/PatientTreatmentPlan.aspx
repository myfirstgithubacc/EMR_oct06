<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="PatientTreatmentPlan.aspx.cs" Inherits="EMR_ClinicalPathway_PatientTreatmentPlan" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript">
        window.onload = function () {
            var div = document.getElementById("dvScroll");
            var div_position = document.getElementById("div_position");
            var position = parseInt('<%=Request.Form["div_position"] %>');

            if (isNaN(position)) {
                position = 0;
            }
            div.scrollTop = position;
            div.onscroll = function () {
                div_position.value = div.scrollTop;
            };
        };
    </script>
    <script type="text/javascript">
        function OnClientClose(oWnd) {
            $get('<%=btnSearch.ClientID%>').click();
        }
    </script>
    <script type="text/javascript">

        function OnClientEditorLoad(editor, args) {
            var style = editor.get_contentArea().style;
            style.fontFamily = 'Tahoma';
            style.fontSize = 11 + 'pt';
        }



    </script>
    <script type="text/javascript">
        function OnClientSelectionChange(editor, args) {
            var tool = editor.getToolByName("RealFontSize");
            if (tool && !$telerik.isIE) {
                setTimeout(function () {
                    var value = tool.get_value();

                    switch (value) {
                        case "11px":
                            value = value.replace("11px", "9pt");
                            break;
                        case "12px":
                            value = value.replace("12px", "9pt");
                            break;
                        case "14px":
                            value = value.replace("14px", "11pt");
                            break;
                        case "16px":
                            value = value.replace("16px", "12pt");
                            break;
                        case "15px":
                            value = value.replace("15px", "11pt");
                            break;
                        case "18px":
                            value = value.replace("18px", "14pt");
                            break;
                        case "24px":
                            value = value.replace("24px", "18pt");
                            break;
                        case "26px":
                            value = value.replace("26px", "20pt");
                            break;
                        case "32px":
                            value = value.replace("32px", "24pt");
                            break;
                        case "34px":
                            value = value.replace("34px", "26pt");
                            break;
                        case "48px":
                            value = value.replace("48px", "36pt");
                            break;
                    }
                    tool.set_value(value);
                }, 0);
            }
        }
    </script>

    <style>
        body {
            overflow: hidden;
        }
    </style>
    <input type="hidden" id="div_position" name="div_position" />
    <div id="dvScroll" style="overflow-y: scroll; height: 600px; width: 99%">

        <asp:UpdatePanel ID="updMain" runat="server">
            <ContentTemplate>

                <style>
                    .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
                        border: 1px solid #ddd !important;
                    }

                    .form-scroller .panel.panel-default {
                        margin-bottom: 5px;
                    }

                        .form-scroller .panel.panel-default .panel-heading {
                            padding: 5px 15px;
                        }

                        .form-scroller .panel.panel-default .panel-body {
                            padding: 5px 15px;
                        }

                            .form-scroller .panel.panel-default .panel-body .table {
                                margin-bottom: 5px;
                            }

                    .main-info {
                        text-align: center;
                    }

                        .main-info span {
                            float: none !important;
                            margin: 0 5px 0 0 !important;
                            font-size: 13px !important;
                            font-weight: bold;
                        }

                    .ajax__tab_header {
                        padding: 0 16px;
                    }

                    .ajax__tab_xp .ajax__tab_body {
                        border: 0;
                    }

                    .check-panel label {
                        padding: 2px 0 0 5px;
                        font-weight: normal;
                    }

                    .check-panel td {
                        display: inline-block;
                        width: 50%;
                    }

                        .check-panel td span {
                            display: flex;
                        }

                    .clsGridRow table {
                        width: 100%;
                        border: 0 !important;
                    }

                    tr.clsGridRow {
                        background: none !important;
                    }

                    .head-greybg {
                        background: #f5f5f5;
                    }

                        .head-greybg td {
                            padding: 6px 10px;
                        }

                    .ajax__tab_xp .ajax__tab_header {
                        background: none;
                    }

                    .panl {
                        border: 1px solid #dddddd;
                        padding: 8px 14px;
                        background: whitesmoke;
                    }
                </style>


                <div class="container-fluid" style="margin: 5px;">
                    <div class="row">
                        <div class="col-md-4" style="padding: 0;">
                            <asp:GridView ID="gvTreatmentPatientList" runat="server"
                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                HeaderStyle-Wrap="false" HeaderStyle-BackColor="#d9edf7" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                AutoGenerateColumns="False" Width="100%" CellPadding="4" OnRowDataBound="gvTreatmentPatientList_RowDataBound"
                                CssClass="table table-bordered" HeaderStyle-CssClass="bg-info">

                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="10px">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>'>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                            <asp:HiddenField ID="hdnConsultingDoctorId" runat="server" Value='<%#Eval("ConsultingDoctorId")%>' />
                                            <asp:HiddenField ID="hdnPayerType" runat="server" Value='<%#Eval("PayerType")%>' />
                                            <asp:HiddenField ID="hdnInsuranceCardId" runat="server" Value='<%#Eval("InsuranceCardId")%>' />
                                            <asp:HiddenField ID="hdnCurrentBillCategory" runat="server" Value='<%#Eval("CurrentBillCategory")%>' />
                                            <asp:HiddenField ID="hdnSponsorId" runat="server" Value='<%#Eval("SponsorId")%>' />
                                            <asp:HiddenField ID="hdnPayorId" runat="server" Value='<%#Eval("PayorId")%>' />
                                            <asp:HiddenField ID="hdnOPIP" runat="server" Value='<%#Eval("OPIP")%>' />
                                            <asp:HiddenField ID="hdnDoctorName" runat="server" Value='<%#Eval("DoctorName")%>' />
                                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value='<%#Eval("EncounterNo")%>' />
                                            <asp:HiddenField ID="hdnAdmissionDate" runat="server" Value='<%#Eval("AdmissionDate")%>' />
                                            <asp:HiddenField ID="hdnGender" runat="server" Value='<%#Eval("Gender")%>' />
                                            <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                            <asp:HiddenField ID="hdnTemplateTypeID" runat="server" Value='<%#Eval("TemplateTypeID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Patient Name" HeaderStyle-Width="190px" ItemStyle-Width="190px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Age/Gender">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeAndGender" runat="server" Text='<%#Eval("AgeAndGender")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" OnClick="lnkSelect_Click"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="col-md-8" style="padding: 0;">
                            <div id="divDetail" runat="server">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12 main-info">
                                            <div class="bg-primary well-sm">
                                                <asp:Label ID="Label2" runat="server" Text="Encounter No: " />
                                                <asp:Label ID="lblEncounterNo" runat="server" Text="" Font-Bold="true" />

                                                <asp:Label ID="Label4" runat="server" Text="Provider: " />
                                                <asp:Label ID="lblProvider" runat="server" Text="" Font-Bold="true" />

                                                <asp:Label ID="Label6" runat="server" Text="Admission Date: " />
                                                <asp:Label ID="lblAdmissionDate" runat="server" Text="" Font-Bold="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="container-fluid">
                                <%-- <asp:Panel ID="Panel1" runat="server" Style="max-height: 600px; overflow-y: auto;">--%>


                                <div class="emrPart">
                                    <div class="container-fluid">

                                        <div class="row">
                                            <!--Right Side Starts-->
                                            <div class="col-md-12 form-scroller" style="margin-top: 5px; width: 100%;">
                                                <div class="panel-group">


                                                    <div class="row" style="padding: 16px;">

                                                        <div class="col-12 panl">
                                                            <div class="row">

                                                                <div class="col-8">
                                                                    <div class="row">
                                                                        <div class="col-5">
                                                                            <label>
                                                                                <asp:Label ID="lblDept" runat="server" Text="Plan Name:" />&nbsp;&nbsp;<asp:Label ID="lblPlanName" runat="server" Text="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:HiddenField ID="hdnPlanId" runat="server" Value="" />
                                                                                <asp:Label ID="Label1" runat="server" Text="Days: " />&nbsp;&nbsp;
                                                                            </label>
                                                                        </div>
                                                                        <div class="col-7">
                                                                            <telerik:RadComboBox ID="ddlPlanDayDetail" runat="server"
                                                                                EmptyMessage="[Select Care Plan]" OnSelectedIndexChanged="ddlPlanDayDetail_SelectedIndexChanged"
                                                                                AutoPostBack="true" Width="100%" />
                                                                        </div>
                                                                    </div>


                                                                </div>
                                                                <div class="col-4">
                                                                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="top-error" />
                                                                    <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Preview" CssClass="btn btn-primary btn-sm pull-right" />&nbsp;&nbsp;&nbsp;&nbsp;
                                                                     <asp:Button ID="btnVariationReport" runat="server" OnClick="btnVariationReport_Click" Text="Variation Report" CssClass="btn btn-primary btn-sm pull-right" />
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row">


                                                        <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                                                            <cc1:TabPanel ID="TabSubDepartment" runat="server" TabIndex="1">
                                                                <HeaderTemplate>
                                                                    Templates
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div class="col-md-12">
                                                                        <%-- Chief Complaints Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>Chief Complaints</b>

                                                                                <asp:Button ID="btnChiefComplaints" runat="server" OnClick="btnChiefComplaints_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="form-group">
                                                                                    <asp:TextBox ID="txtChiefComplaints" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px"
                                                                                        Width="100%"></asp:TextBox>


                                                                                </div>


                                                                            </div>
                                                                        </div>
                                                                        <%-- Chief Complaints Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- History Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>History</b>
                                                                                <asp:Button ID="btnHistory" runat="server" OnClick="btnHistory_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="form-group">
                                                                                    <asp:TextBox ID="txtHistory" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px"
                                                                                        Width="100%"></asp:TextBox>

                                                                                </div>


                                                                            </div>
                                                                        </div>
                                                                        <%-- History Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- Examination Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>Examination</b>
                                                                                <asp:Button ID="btnExamination" runat="server" OnClick="btnExamination_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="form-group">
                                                                                    <asp:TextBox ID="txtExamination" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px"
                                                                                        Width="100%"></asp:TextBox>

                                                                                </div>


                                                                            </div>
                                                                        </div>
                                                                        <%-- Examination Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- Plan Of Care Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>Plan Of Care</b>
                                                                                <asp:Button ID="btnPlanOfCare" runat="server" OnClick="btnPlanOfCare_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="form-group">
                                                                                    <asp:TextBox ID="txtPlanOfCare" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px"
                                                                                        Width="100%"></asp:TextBox>

                                                                                </div>


                                                                            </div>
                                                                        </div>
                                                                        <%-- Plan Of Care Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- Instructions Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>Instructions</b>
                                                                                <asp:Button ID="btnInstruction" runat="server" OnClick="btnInstruction_Click" Text="Add" CssClass="btn btn-primary btn-sm" />
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="form-group">
                                                                                    <asp:TextBox ID="txtFreeInstruction" SkinID="textbox" runat="server" TextMode="MultiLine" Height="40px"
                                                                                        Width="100%"></asp:TextBox>


                                                                                </div>


                                                                            </div>
                                                                        </div>
                                                                        <%-- Instructions Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- Consultation Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading"><b>Consultation</b></div>
                                                                            <div class="panel-body">

                                                                                <asp:GridView ID="gvSpecialsation" runat="server" SkinID="gridviewOrderNew"
                                                                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                                                    AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                                                                                    CssClass="table table-bordered">

                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Specialisation Name" ItemStyle-Font-Size="Small">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblSpecialisation" runat="server" Text='<%#Eval("SpecialisationName")%>' />
                                                                                                <asp:HiddenField ID="hdnSpecialisationId" runat="server" Value='<% #Eval("SpecialisationId") %>' />
                                                                                                <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                                                                <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                                                                <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Font-Size="Small" ItemStyle-Width="150px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </div>
                                                                        <%-- Specialisation Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- Investigations Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>Investigations</b>
                                                                                <asp:Button ID="btnAddService" runat="server" OnClick="btnSaveService_Onclick" Text="Service Order" CssClass="btn btn-primary btn-sm pull-right" />
                                                                            </div>
                                                                            <div class="panel-body">
                                                                                <div class="row">
                                                                                    <div class="col-md-12">
                                                                                        <asp:GridView ID="gvService" runat="server" SkinID="gridviewOrderNew" OnRowDataBound="gvService_RowDataBound"
                                                                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                                                            AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-bordered">

                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:CheckBox ID="chkAllService" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllService_CheckedChanged" />
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkService" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px">
                                                                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                                                                </asp:TemplateField>

                                                                                                <asp:TemplateField HeaderText="Service Name" ItemStyle-Font-Size="Small" ItemStyle-Width="800px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                                                                        <asp:HiddenField ID="hdnServiceId" runat="server" Value='<% #Eval("ServiceId") %>' />
                                                                                                        <asp:HiddenField ID="hdnId" runat="server" Value='<% #Eval("Id") %>' />
                                                                                                        <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                                                                        <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                                                                        <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />

                                                                                                        <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<% #Eval("DiagSampleId") %>' />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Font-Size="Small" ItemStyle-Width="150px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLabStatus" runat="server" Text='<%#Eval("LabStatus")%>' />
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>

                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <%-- Investigations Ends --%>
                                                                    </div>
                                                                    <div class="col-md-12">
                                                                        <%-- Medicines Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading">
                                                                                <b>Drug Order</b>
                                                                                <asp:Button ID="btnAddPrescription" runat="server" OnClick="btnAddPrescription_Onclick" CssClass="btn btn-sm btn-primary pull-right" Text="Drug Order" Style="vertical-align: top;" />
                                                                            </div>
                                                                            <div class="panel-body">

                                                                                <asp:GridView ID="gvPrescription" runat="server" SkinID="gridviewOrderNew" OnRowDataBound="gvPrescription_RowDataBound"
                                                                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                                                    AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-bordered">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px">
                                                                                            <HeaderTemplate>
                                                                                                <asp:CheckBox ID="chkAllPrescription" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllPrescription_CheckedChanged" />
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkPrescription" runat="server" />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Generic/Brand Name" ItemStyle-Font-Size="Small" ItemStyle-Width="240px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblGenericName" runat="server" Text='<%#Eval("GenericName")%>' />&nbsp&nbsp;
                                                                                <asp:Label ID="lblItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                                                                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# Eval("Id") %>' />
                                                                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Eval("ItemId") %>' />
                                                                                                <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                                                                <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                                                                <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />

                                                                                                <asp:HiddenField ID="hdnGenericId" runat="server" Value='<% #Eval("GenericId") %>' />
                                                                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<% #Eval("IndentId") %>' />
                                                                                                <asp:HiddenField ID="hdnQty" runat="server" Value='<% #Eval("Qty") %>' />
                                                                                                <asp:HiddenField ID="hdnRouteId" runat="server" Value='<% #Eval("RouteId") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Dose" ItemStyle-Font-Size="Small" ItemStyle-Width="70px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDose" runat="server" Text='<%#Eval("Dose")%>' />
                                                                                                <asp:Label ID="lblDoseUnit" runat="server" Text='<%#Eval("DoseUnit")%>' />
                                                                                                <asp:HiddenField ID="hdnDoseUnitID" runat="server" Value='<% #Eval("DoseUnitID") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Frequency" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFrequency" runat="server" Text='<%#Eval("Frequency")%>' />
                                                                                                <asp:HiddenField ID="hdnFrequencyID" runat="server" Value='<% #Eval("FrequencyID") %>' />
                                                                                                <asp:HiddenField ID="hdnFrequencyValue" runat="server" Value='<% #Eval("FrequencyValue") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Duration" ItemStyle-Font-Size="Small" ItemStyle-Width="50px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days")%>' />
                                                                                                <asp:Label ID="lblDaysType" runat="server" Text='<%#Eval("DaysType")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Route" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRouteName" runat="server" Text='<%#Eval("RouteName")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Food Relation" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFoodName" runat="server" Text='<%#Eval("FoodName")%>' />
                                                                                                <asp:HiddenField ID="hdnFoodRelationId" runat="server" Value='<% #Eval("FoodRelationId") %>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Instructions" ItemStyle-Font-Size="Small" ItemStyle-Width="110px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblIntructions" runat="server" Text='<%#Eval("Intructions")%>' />

                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Font-Size="Small" ItemStyle-Width="60px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />

                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                                                                </asp:GridView>

                                                                            </div>

                                                                        </div>
                                                                        <%-- Medicines Ends --%>
                                                                    </div>

                                                                    <div class="col-md-12">
                                                                        <%-- Check List Start --%>
                                                                        <div class="panel panel-default">
                                                                            <div class="panel-heading"><b>Template List</b></div>
                                                                            <div class="form-group">
                                                                                <asp:GridView ID="gvTemplateList" runat="server" SkinID="gridviewOrderNew"
                                                                                    HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                                                    AutoGenerateColumns="False" Width="100%" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" CssClass="table table-bordered" Style="margin-top: 15px;" ShowHeaderWhenEmpty="true">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>'
                                                                                            ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Template Name" ItemStyle-Font-Size="Small">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkTemplateName" runat="server" Text='<%#Eval("TemplateName")%>' OnClick="lnkTemplateName_Click" />
                                                                                                <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<% #Eval("TemplateId") %>' />
                                                                                                <asp:HiddenField ID="hdnPlanId" runat="server" Value='<% #Eval("PlanId") %>' />
                                                                                                <asp:HiddenField ID="hdnDayId" runat="server" Value='<% #Eval("DayId") %>' />
                                                                                                <asp:HiddenField ID="hdnDayDetailId" runat="server" Value='<% #Eval("DayDetailId") %>' />

                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Status" ItemStyle-Font-Size="Small" ItemStyle-Width="120px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>No Template Available</EmptyDataTemplate>
                                                                                </asp:GridView>
                                                                            </div>

                                                                        </div>
                                                                        <%-- Check List Ends --%>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="TabPanel1" runat="server" TabIndex="1">
                                                                <HeaderTemplate>
                                                                    Others
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <div class="col-md-12">
                                                                        <%-- Dynamic Template Start --%>
                                                                        <div class="panel panel-default checkbox-panel">

                                                                            <div class="panel-body" style="padding: 8px 0;">
                                                                                <div class="">
                                                                                    <div class="row1">
                                                                                        <div class="col-md-4">
                                                                                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" CssClass="select-all" />
                                                                                        </div>
                                                                                        <div class="col-md-8 text-right">
                                                                                            <asp:Button ID="btnAddTemplateField" runat="server" Text="Save" CssClass="btn btn-primary btn-sm" OnClick="btnAddTemplateField_Click" />
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="form-group">

                                                                                    <asp:GridView ID="gvSelectedServices" SkinID="gridview" runat="server" HeaderStyle-Wrap="false"
                                                                                        AutoGenerateColumns="False" ShowHeader="false" CellSpacing="0" CellPadding="0"
                                                                                        Width="100%" AllowPaging="false" PagerSettings-Visible="true" OnRowDataBound="gvSelectedServices_RowDataBound" CssClass="gv-selected-service">
                                                                                        <EmptyDataTemplate>
                                                                                            No Data Found.
                                                                                        </EmptyDataTemplate>
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Values" ItemStyle-Width="100%" HeaderStyle-Width="100%"
                                                                                                HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top">
                                                                                                <ItemTemplate>
                                                                                                    <table id="tblFieldName" cellpadding="0" cellspacing="1" border="0" runat="server" width="100%" class="head-greybg">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName")%>' Font-Bold="true"></asp:Label>
                                                                                                                <asp:HiddenField ID="hdnFieldType" runat="server" Value='<%#Eval("FieldType")%>' />
                                                                                                                <asp:HiddenField ID="hdnSectionId" runat="server" Value='<%#Eval("SectionId")%>' />
                                                                                                                <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldID")%>' />


                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <table id="tbl1" cellpadding="0" cellspacing="1" border="0" runat="server">
                                                                                                        <tr valign="top">
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtT" SkinID="textbox" Columns='<%#common.myInt(Eval("MaxLength"))%>'
                                                                                                                    Visible="false" MaxLength='<%#common.myInt(Eval("MaxLength"))%>' runat="server"
                                                                                                                    Width="100%" />
                                                                                                            </td>

                                                                                                            <td colspan="2">
                                                                                                                <asp:TextBox ID="txtM" SkinID="textbox" runat="server" TextMode="MultiLine" Style="min-height: 50px; min-width: 250px; width: auto !important;"
                                                                                                                    MaxLength="5000" onkeyup="return MaxLenTxt(this,5000);"
                                                                                                                    Visible="false" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="ddlTemplateFieldFormats" Font-Size="10pt" runat="server" OnSelectedIndexChanged="ddlTemplateFieldFormats_OnSelectedIndexChanged"
                                                                                                                    SkinID="DropDown" Width="200px" AutoPostBack="true" Visible="false" />
                                                                                                                <telerik:RadEditor ID="txtW" ToolbarMode="ShowOnFocus" OnClientSelectionChange="OnClientSelectionChange"
                                                                                                                    EnableResize="true" runat="server" Skin="Outlook"
                                                                                                                    ToolsFile="~/Include/XML/PrescriptionRTF.xml" EditModes="Design" OnClientLoad="OnClientEditorLoad" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <table id="Table1" cellpadding="0" cellspacing="1" border="0" runat="server">
                                                                                                        <tr valign="top">
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="D" SkinID="DropDown"
                                                                                                                    Visible="false" runat="server" Width="227px" Font-Size="10pt" AppendDataBoundItems="true">
                                                                                                                    <asp:ListItem Text="Select" Value="0" />
                                                                                                                </asp:DropDownList>
                                                                                                                <telerik:RadComboBox ID="IM" Visible="false" runat="server" Width="227px" Font-Size="10pt"
                                                                                                                    AppendDataBoundItems="true" Skin="Default" EnableAutomaticLoadOnDemand="True"
                                                                                                                    EnableVirtualScrolling="true">
                                                                                                                    <Items>
                                                                                                                        <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                                    </Items>
                                                                                                                </telerik:RadComboBox>

                                                                                                                <asp:DataList ID="C" runat="server" Visible="false" CellPadding="10" CellSpacing="10" CssClass="check-panel">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:HiddenField ID="hdnCV" runat="server" Value='<%#Eval("ValueId")%>' />
                                                                                                                        <asp:CheckBox ID="C" Font-Size="10pt" runat="server" Text='<%#Eval("ValueName")%>' Font-Bold="false" />
                                                                                                                        <textarea id="CT" class="Textbox" visible="false" runat="server" onkeypress="AutoChange()"
                                                                                                                            rows="1" cols="40"></textarea>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:DataList>
                                                                                                            </td>

                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <asp:RadioButtonList ID="B" Font-Size="10pt" Width="100px" runat="server" Visible="false" RepeatDirection="Horizontal">
                                                                                                        <asp:ListItem Value="1" Text="Yes" />
                                                                                                        <asp:ListItem Value="0" Text="No" />
                                                                                                    </asp:RadioButtonList>
                                                                                                    <asp:RadioButtonList ID="R" Font-Size="10pt" Width="100%" CssClass="FormatRadioButtonList"
                                                                                                        runat="server" Visible="false" RepeatDirection="Horizontal" RepeatLayout="Flow" />
                                                                                                    <table id="tblDate" runat="server" visible="false" cellpadding="0" cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txtDate" SkinID="textbox" Font-Size="13px" Text="" Width="67px"
                                                                                                                    runat="server" MaxLength="10" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                    vspace="0" border="0" id="imgFromDate" runat="server" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate"
                                                                                                                    Format="dd/MM/yyyy" PopupButtonID="imgFromDate">
                                                                                                                </cc1:CalendarExtender>
                                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                                    TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                                <cc1:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                                                                                    CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                    Enabled="True" TargetControlID="txtDate" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                    AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                    ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                </cc1:MaskedEditExtender>
                                                                                                                <asp:CustomValidator ID="CustomValidator" runat="server" ClientValidationFunction="isValidateDate"
                                                                                                                    ControlToValidate="txtDate" ErrorMessage="Invalid date format." />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <%-- Outcome Template Ends --%>
                                                                    </div>
                                                                    </div>
                                                <!--row ends-->
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                        </cc1:TabContainer>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <%-- </asp:Panel>--%>
                            </div>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:Button ID="btnSearch" runat="server" Width="0px" OnClick="btnSearch_Click" Height="0px" BackColor="Transparent" Style="visibility: hidden; height: 0px; float: left;" />
                        </div>
                        <asp:HiddenField ID="hdnEncId" runat="server" />
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

