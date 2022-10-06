<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="Default_WithGridView.aspx.cs" Inherits="EMR_Default" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/style_patient_dt.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../Include/css/jquery.mCustomScrollbar.css" />
    <style>
        body {
            background-color: #ecf0f5;
            font-size: 12px;
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
    </style>
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

    </script>
    <div class="container-fluid content-wrapper">

        <div class="col-md-3">
            <h2>Demographics Details</h2>
            <div class="border-box" style="max-height: 190px; min-height: 190px; padding-top: 15px;">

                <div class="col-md-4">
                    <%-- <img class="max-width-thumb" alt="" src="../img/MensEyewearThumb.jpg">--%>
                    <asp:Image ID="imgPatient" runat="server" CssClass="image_width" />
                </div>
                <div class="col-md-8">
                    <div class="caption">
                        <div class="row line-height">
                            <div class="col-md-6">Reg Id</div>
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblUHID" runat="server" /></strong>
                            </div>
                            <div class="col-md-6">Name:</div>
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblName" runat="server" /></strong>
                            </div>

                            <div class="col-md-6">Age:</div>
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblAge" runat="server" /></strong>
                            </div>
                            <div class="col-md-6">Allergic To:</div>
                            <div class="col-md-6">
                                <strong class="text-danger">
                                    <asp:Label ID="lblAllergy" Text="" runat="server" /></strong>
                            </div>
                            <div class="col-md-6">contact:</div>
                            <div class="col-md-6">
                                <strong>
                                    <asp:Label ID="lblContact" runat="server" /></strong>
                            </div>
                            <%--<div class="col-md-6">Attachment</div><div class="col-md-6"><span class="glyphicon glyphicon-paperclip"></span></div>--%>
                        </div>


                    </div>
                </div>
            </div>



        </div>

        <div class="col-md-9">
            <div class="row">
                <div class="col-md-5">
                    <div id="divLastPatientVisit" runat="server">
                        <h2>Last  Visit</h2>
                        <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">


                            <asp:UpdatePanel ID="upnlCategory" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="rpfCategory" runat="server" OnItemDataBound="rpfCategory_ItemDataBound" ShowHeader="false" >

                                        <Columns>
                                            <asp:TemplateField >
                                                <ItemTemplate>

                                                    <table class="table firstTable">
                                                        <tr>
                                                            <td>
                                                                <ul>
                                                                    <li>
                                                                        <asp:LinkButton ID="lbtnDoctorName" runat="server" Text='<%# Bind("DoctorName") %>' CommandName="DoctorId" CommandArgument='<%# Bind("DoctorId") %>' Font-Underline="false" ForeColor="Navy" Enabled="false"></asp:LinkButton></li>
                                                                </ul>
                                                            </td>

                                                            <td>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("VisitDate") %>'></asp:Label>

                                                            </td>



                                                            <td class="text-center">
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("VisitType") %>'></asp:Label></td>

                                                            <td class="text-center">
                                                                <asp:ImageButton ID="PatientType" runat="server" ImageUrl='<%# Bind("img") %>' Width="25px" Height="25px" ToolTip='<%# Bind("EncounterNo") %>' OnClick="PatientType_Click" Visible="false" />

                                                                <asp:ImageButton ID="ibtnLabResult" runat="server" ImageUrl="~/Images/attachment.png" ToolTip="Lab Result" OnClick="ibtnLabResult_Click" />
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEncounterid" runat="server" Text='<%# Bind("EncounterId") %>'></asp:Label>

                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblEncounterNo" runat="server" Text='<%# Bind("EncounterNo") %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDoctorId" runat="server" Text='<%# Bind("DoctorId") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>



                                        <%--  <FooterTemplate>
                                        </FooterTemplate>--%>
                                    </asp:GridView>
                                    <%--<asp:GridView ID="GridView1"  runat="server"></asp:GridView>--%>
                                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew"
                                                runat="server" Behaviors="Close,Move" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </ContentTemplate>
                            </asp:UpdatePanel>


                            <asp:UpdatePanel ID="rptDoctorVisitPage" runat="server">
                                <%--   <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rptPages" EventName="ItemCommand" />
                                </Triggers>--%>
                                <ContentTemplate>
                                    <asp:GridView ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand" OnItemDataBound="rptPages_ItemDataBound" ShowHeader="false" >
                                        <%--<asp:Repeater ID="rptPages" runat="server" OnItemCommand="rptPages_ItemCommand">--%>


                                        <Columns>
                                            <asp:TemplateField>
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
                                                <%--   <FooterTemplate>
                                            </td>
                    </tr>
                    </table></h6>
                                        </FooterTemplate>--%>
  
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>


                        </div>
                    </div>
                </div>
                <div class="col-md-3">
                    <div id="divViatalsGraph" runat="server">
                        <h2>Vitals Parameter Graph</h2>
                        <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">
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
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />

                                <Columns>

                                    <asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Registration Id">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <%--<asp:Label ID="lblHT" runat="server" Text='<%# Bind("HT") %>'></asp:Label>--%>
                                                <asp:Label ID="lblRegistrationId" runat="server" Text='<%# Bind("RegistrationId") %>' />
                                                <%--  <asp:HyperLinkField DataTextField="HT" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Details.aspx?Id={0}" />--%>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Encounter Id">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <%-- <asp:Label ID="lblWT" runat="server" Text='<%# Bind("WT") %>'></asp:Label>--%>
                                                <asp:Label ID="lblEncounterId" runat="server" Text='<%# Bind("EncounterId") %>' />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <asp:Label ID="lblVitalEntryDate" runat="server" Text='<%# Bind("VitalEntryDate") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="VitalId">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <asp:Label ID="lblVitalId" runat="server" Text='<%# Bind("VitalId") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Vital">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <asp:Label ID="lblDisplayName" runat="server" Text='<%# Bind("DisplayName") %>'></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value">
                                        <ItemTemplate>
                                            <div class="media-body">
                                                <asp:HyperLink ID="hplVitalValue" runat="server" Text='<%# Bind("VitalValue") %>'></asp:HyperLink>
                                            </div>
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












                <div class="col-md-4">
                    <div id="divDoctorsNotification" runat="server">
                        <h2>Doctors Notification</h2>
                        <div class="border-box" style="max-height: 190px; min-height: 190px; padding: 0px 10px">

                            <div class="col-md-12">
                                <p>This is demo notification</p>
                            </div>

                        </div>

                    </div>
                </div>




            </div>
        </div>
    </div>

    <div class="content-wrapper container-fluid">

        <div class="col-md-4" id="divProblemList" runat="server">
            <h2>Problems list</h2>
            <div class="border-box  mCustomScrollbar" data-mcs-theme="minimal" style="max-height: 190px; min-height: 190px">

                <asp:GridView ID="rptProblemList" runat="server" ShowHeader="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>


                                <div class="rd_test">
                                    <div class="media-body" style="width: 50%">

                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("EntryDate") %>'></asp:Label>

                                    </div>

                                    <div class="media-body">
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("Id") %>' />


                                        <asp:Label ID="lblProblemDescription" runat="server" Text='<%# Bind("ProblemDescription") %>'></asp:Label>

                                    </div>

                                </div>

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="rptPaging" runat="server" OnItemCommand="rptPaging_ItemCommand" ShowHeader="false">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnPage"
                                    Style="padding: 8px; margin: 2px; background: #ffa100; border: solid 1px #666; font: 8pt tahoma;"
                                    CommandName="Page" CommandArgument="<%# Container.DataItem %>"
                                    runat="server" ForeColor="White" Font-Bold="True"><%# Container.DataItem %>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>



            </div>
        </div>

        <div class="col-md-8">
            <div class="row">
                <div class="col-md-8" id="divMedicinesGiven" runat="server">
                    <h2>Latest Medicines Given</h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">

                        <asp:GridView  ID="rptPrescribedMedicine" runat="server" ShowHeader="false">
                           <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="rd_test">
                                    <div class="media-left">

                                        <asp:ImageButton ID="ibtnPrescribedMedicine" runat="server" ImageUrl="~/Img/drug.png" ToolTip="Medicine Details" OnClick="ibtnPrescribedMedicine_Click" />
                                    </div>

                                    <div class="media-body">
                                        <asp:HiddenField ID="hdnDetailsId" runat="server" Value='<%# Bind("DetailsId") %>' />
                                        <asp:HiddenField ID="hdnItemId" runat="server" Value='<%# Bind("ItemId") %>' />
                                        <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%# Bind("IndentId") %>' />

                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>

                                    </div>
                                    <div class="media-body">

                                        <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%# Bind("PrescriptionDetail") %>'></asp:Label>

                                    </div>
                                    <div class="media-body">

                                        <asp:Label ID="lblIndentDate" runat="server" Text='<%# Bind("IndentDate") %>'></asp:Label>

                                    </div>

                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>


                        </asp:GridView>



                    </div>
                </div>


                <div class="col-md-4" id="divLabTest" runat="server">
                    <h2>Recent Lab Test</h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">

                        <%--<div class="rd_test">
								<div class="media-left"> <a href="#"> <img alt="64x64" data-src="holder.js/64x64" class="media-object" style="width:24px; height: 24px;" src="../img/flask.png" data-holder-rendered="true"> </a> </div>
								<div class="media-body"> <h4 class="media-heading">Media heading</h4><p>Media heading Cras sit amet nibh liberou.</p></div>
							</div>--%>
                    </div>
                </div>


            </div>
        </div>

    </div>
    <div class="content-wrapper container-fluid">

        <div class="col-md-8">
            <div class="row">
                <div class="col-md-4" id="divSugarGraph" runat="server">
                    <h2>Blood Sugar Graph </h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">


                        <%--							<div class="rd_test">
								<div class="media-left"> <a href="#"> <img alt="64x64" data-src="holder.js/64x64" class="media-object" style="width:24px; height: 24px;" src="../img/flask.png" data-holder-rendered="true"> </a> </div>
								<div class="media-body"> <h4 class="media-heading">Media heading</h4><p>Media heading Cras sit amet nibh liberou.</p></div>
							</div>
							<div class="rd_test border-btm">
								<div class="media-left"> <a href="#"> <img alt="64x64" data-src="holder.js/64x64" class="media-object" style="width:24px; height: 24px;" src="../img/flask.png" data-holder-rendered="true"> </a> </div>
								<div class="media-body"> <h4 class="media-heading">Media heading</h4><p>Media heading Cras sit amet nibh liberou.</p></div>
							</div>--%>
                    </div>
                </div>
                <div class="col-md-4" id="divRadiologyTest" runat="server">
                    <h2>Radiology Test</h2>
                    <div class="border-box mCustomScrollbar" data-mcs-theme="minimal">

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


    </div>




    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.4/jquery.js"></script>

    <script src="../../Include/js/jquery.mCustomScrollbar.concat.min.js"></script>
    <triggers>
        <asp:AsyncPostBackTrigger ControlID="ibtnLabResult" EventName="CLICK" />
    </triggers>
</asp:Content>

