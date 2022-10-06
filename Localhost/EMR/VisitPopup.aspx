<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master" CodeFile="VisitPopup.aspx.cs" Inherits="EMRBILLING_VisitPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <head runat="server">--%>
    <title>Visit/Consultation Order</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <style>
        @media (min-width: 768px) and (max-width: 1024px) {
            .patientName {
                padding: 0 0 0 2em !important;
                float: left;
            }
        }
    </style>

    <script language="javascript" type="text/javascript">

        function checkValidVisit() {
            var cmbDoctor = document.getElementById('cmbDoctor')
            if (cmbDoctor.value == '...Select...') {
                alert('Please! Fill Provider Name')
                return false
            }
            var cmbOpVisit = document.getElementById('cmbOpVisit')
            if (cmbOpVisit.value == '....Select....') {
                alert('Please! Fill Visit Type Name')
                return false
            }
        }

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = document.getElementById("hdnXmlString").value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>

    <%--   </head>--%>


    <%--<body>
    <form id="form1" runat="server">--%>

    <%--<asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>--%>
    <div>
        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
    </div>
    <asp:UpdatePanel ID="upMainPannel" runat="server">
        <ContentTemplate>

            <div id="tblMain" runat="server">

                <div class="container-fluid header_main margin_bottom" id="Table1" runat="server">
                    <div class="col-md-5 col-sm-5 col-xs-5">
                        <asp:TextBox ID="txtRegNo" runat="server" Width="80px" SkinID="textbox" Visible="false" />
                        <asp:TextBox ID="txtRegID" Visible="false" runat="server" Width="80px" SkinID="textbox" />
                        <asp:TextBox ID="txtEncNo" runat="server" Width="80px" SkinID="textbox" Visible="false" />
                        <asp:TextBox ID="txtEncId" Visible="false" runat="server" Width="80px" SkinID="textbox" />
                        <asp:HiddenField ID="hdnConsultantId" runat="server" />
                        <asp:HiddenField ID="hdhSpecialzationId" runat="server" />
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                        <asp:HiddenField ID="hdnCompanyCode" runat="server" />
                        <asp:HiddenField ID="hdnInsCode" runat="server" />
                        <asp:HiddenField ID="hdnExternalReg" runat="server" Value="" />
                    </div>

                    <div class="col-md-7 col-sm-7 col-xs-7 text-right">
                        <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-default" Text="Close (Ctrl+F8)" ToolTip="Close" OnClientClick="window.close();" />
                        <asp:Button ID="ibtnSave" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Save (Ctrl+F3)" ToolTip="Save Data..." ValidationGroup="save" OnClick="ibtSave_OnClick" />

                        <script language="javascript" type="text/javascript">
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
                                        $get('<%=ibtnSave.ClientID%>').click();
                                        break;
                                    case 119:  // F8
                                        $get('<%=ibtnClose.ClientID%>').click();
                                        break;

                                }
                                evt.returnValue = false;
                                return false;
                            }
                        </script>
                    </div>

                </div>




                <%--  <div class="container-fluid">
                    <div class="row">
                        <div class="table-responsive">
                            <table class="table table-small-font table-bordered table-striped margin_bottom01">

                                <tr align="center">

                                    <td colspan="1" align="left">
                                        <asp:Label ID="lblRegNoInfo" runat="server" Font-Bold="true" Text="Reg.#"></asp:Label>
                                        <asp:Label ID="RegNo" runat="server" SkinID="label" Font-Bold="true" ForeColor="#990066"></asp:Label>
                                    </td>

                                    <td colspan="1" align="left">
                                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                    </td>

                                    <td colspan="1" align="left">
                                        <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                                    </td>

                                    <td colspan="1" align="left">
                                        <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>

                                        <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" SkinID="label" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label"></asp:Label>
                                    </td>


                                </tr>

                            </table>
                        </div>
                    </div>
                </div>--%>





                <div class="container-fluid margin_bottom">
                    <div class="row" id="tblVisit" runat="server">
                        <div class="col-sm-6 col-md-6 col-xs-6">
                            <div class="row form-group">
                                <div class="col-md-2 col-sm-3 col-xs-5 label1">
                                    <asp:Label ID="Label1" runat="server" Text="Specialisation"></asp:Label>
                                </div>
                                <div class="col-md-10 col-sm-9 col-xs-7">
                                    <telerik:RadComboBox ID="cmbSpecial" runat="server" EnableLoadOnDemand="true"
                                        Width="100%" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="cmbSpecial_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>


                        <div class="col-sm-6 col-md-6 col-xs-6">
                            <div class="row form-group">
                                <div class="col-md-2 col-sm-3 col-xs-4 label1">
                                    <asp:Label ID="lblDoctor" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label>
                                    <span style="color: #CC0000">*</span>
                                </div>
                                <div class="col-md-10 col-sm-9 col-xs-8">
                                    <telerik:RadComboBox ID="cmbDoctor" runat="server" SkinID="dropDown" EnableLoadOnDemand="true"
                                        Width="100%" DropDownCssClass="100%" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="cmbDoctor_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </div>
                            </div>

                            <asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </div>

                    </div>
                </div>




                <div class="container-fluid">
                    <div class="row form-group" id="tdVisit">

                        <asp:UpdatePanel ID="updIpVisit" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnMorningRs" runat="server" />
                                <asp:HiddenField ID="hdnEveningRs" runat="server" />
                                <asp:HiddenField ID="hdnEmergencyRs" runat="server" />
                                <asp:HiddenField ID="HdnEmergencyRs2" runat="server" />
                                <asp:HiddenField ID="hdnServiceId1" runat="server" />
                                <asp:HiddenField ID="hdnServiceId2" runat="server" />
                                <asp:HiddenField ID="hdnServiceId3" runat="server" />
                                <asp:HiddenField ID="hdnServiceId4" runat="server" />
                                <telerik:RadGrid ID="gvIpVisit" ShowFooter="True" GridLines="None" Width="100%" AllowSorting="false"
                                    runat="server" AutoGenerateColumns="false" Height="350px" OnItemDataBound="gvIpVisit_OnItemDataBound">
                                    <MasterTableView Width="98%">
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitId" runat="server" Text='<%# Convert.ToString(Eval("Id")) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="0px" />
                                                <ItemStyle Width="0px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn UniqueName="VisitDate" DataField="VisitDate" HeaderText="Visit Date">
                                                <HeaderStyle Width="50%" />
                                                <ItemStyle Width="50%" />
                                            </telerik:GridBoundColumn>

                                            <telerik:GridTemplateColumn UniqueName="col1">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblMorningCharges" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk1" runat="server" />
                                                    <%--<asp:LinkButton ID="lnkChk1" Visible="false" OnClick="lnkChk1_OnClick" CommandName="Cancel"
                                                                runat="server">Cancel</asp:LinkButton>--%>
                                                    <asp:HiddenField ID="hdChk1" runat="server" />
                                                    <%--<ajax:ConfirmButtonExtender ID="cbsave1" runat="server" ConfirmOnFormSubmit="true"
                                                                ConfirmText="Are you sure you want to cancel visit ? " TargetControlID="lnkChk1">
                                                            </ajax:ConfirmButtonExtender>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="col2">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblEveningCharges" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk2" runat="server" />
                                                    <%--<asp:LinkButton ID="lnkChk2" Visible="false" OnClick="lnkChk2_OnClick" CommandName="Cancel"
                                                                runat="server">Cancel</asp:LinkButton>--%>
                                                    <asp:HiddenField ID="hdChk2" runat="server" />
                                                    <%-- <ajax:ConfirmButtonExtender ID="cbsave2" runat="server" ConfirmOnFormSubmit="true"
                                                                ConfirmText="Are you sure you want to cancel visit ? " TargetControlID="lnkChk2">
                                                            </ajax:ConfirmButtonExtender>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="col3">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblEmergencyCharges" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk3" runat="server" />
                                                    <%-- <asp:LinkButton ID="lnkChk3" Visible="false" OnClick="lnkChk3_OnClick" CommandName="Cancel"
                                                                runat="server">Cancel</asp:LinkButton>--%>
                                                    <asp:HiddenField ID="hdChk3" runat="server" />
                                                    <%-- <ajax:ConfirmButtonExtender ID="cbsave3" runat="server" ConfirmOnFormSubmit="true"
                                                                ConfirmText="Are you sure you want to cancel visit ? " TargetControlID="lnkChk3">
                                                            </ajax:ConfirmButtonExtender>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="col4">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblEmergencyCharges2nd" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chk4" runat="server" />
                                                    <%--<asp:LinkButton ID="lnkChk4" Visible="false" OnClick="lnkChk4_OnClick" CommandName="Cancel"
                                                                runat="server">Cancel</asp:LinkButton>--%>
                                                    <asp:HiddenField ID="hdChk4" runat="server" />
                                                    <%-- <ajax:ConfirmButtonExtender ID="cbsave4" runat="server" ConfirmOnFormSubmit="true"
                                                                ConfirmText="Are you sure you want to cancel visit ? " TargetControlID="lnkChk4">
                                                            </ajax:ConfirmButtonExtender>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True"></Scrolling>
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="cmbDoctor" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="updOpVisit" runat="server">
                            <ContentTemplate>

                                <div class="container-fluid form-group">
                                    <div class="col-md-2 col-sm-2 col-xs-3 row">
                                        <asp:Label ID="lblVisitType" runat="server" Text="Op Visit Type"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-xs-8 row">
                                        <telerik:RadComboBox ID="cmbOpVisit" runat="server" Width="100%" EnableLoadOnDemand="true" Height="100px" Filter="Contains"></telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-xs-1 text-right">
                                        <asp:Button ID="btnAddDoctor" runat="server" Text="Add To Grid" CssClass="btn btn-primary" OnClick="btnAddDoctor_Click" OnClientClick="return checkValidVisit()" />
                                    </div>
                                </div>





                                <telerik:RadGrid ID="gVConsultation" runat="server" Width="100%" AutoGenerateColumns="false"
                                    SkinID="gridview">
                                    <MasterTableView Width="98%">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, Doctor%>'>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctor" runat="server" SkinID="label" Text='<%#Eval("DoctorName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="200px" />
                                                <ItemStyle Width="200px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctorId" runat="server" SkinID="label" Text='<%#Eval("DoctorId") %>'
                                                        Width="0px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="0px" />
                                                <ItemStyle Width="0px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Visit Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitType" runat="server" SkinID="label" Text='<%#Eval("ServiceName") %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="300px" />
                                                <ItemStyle Width="300px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Visit Id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVisitId" runat="server" SkinID="label" Text='<%#Eval("serviceid") %>'
                                                        Width="100px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRate" runat="server" SkinID="label" Text='<%#Eval("Charge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                        Width="100px">
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True"></Scrolling>
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gVConsultation" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="row form-group">
                        <div class="col-sm-12 col-md-12">
                            <asp:UpdatePanel ID="uphidden" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                </div>


                <div class="container-fluid">

                    <div class="row" id="trVisitDetail" runat="server">
                        <div class="col-md-12 col-sm-12">
                            <div class="table-responsive">

                                <table class="table table-small-font table-bordered table-striped margin_bottom01">

                                    <tr align="center">
                                        <td colspan="1">
                                            <asp:Label ID="lblvisit" runat="server" Text="Last Visited : " CssClass="BoldText" />
                                            <asp:Label ID="lblVisitDate" runat="server" CssClass="BoldText-Spacing" />
                                        </td>

                                        <td colspan="1">
                                            <asp:Label ID="lblByService" runat="server" Text=" For : " CssClass="BoldText" />
                                            <asp:Label ID="lblService" runat="server" CssClass="BoldText-Spacing" />
                                        </td>

                                        <td colspan="1">
                                            <asp:Label ID="lblbyDoc" runat="server" Text="By Doctor :" CssClass="BoldText" />
                                            <asp:Label ID="lblByDoctor" runat="server" CssClass="BoldText-Spacing" />
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>

                    </div>
                </div>



            </div>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gVConsultation" />
        </Triggers>
    </asp:UpdatePanel>

    <%--  </form>
</body>--%>
</asp:Content>
