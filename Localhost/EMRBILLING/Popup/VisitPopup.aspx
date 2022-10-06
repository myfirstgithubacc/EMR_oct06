<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisitPopup.aspx.cs" Inherits="EMRBILLING_VisitPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Visit/Consultation Order</title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .header_main{
            overflow:hidden;
        }
    </style>
    <script language="javascript" type="text/javascript">
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

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="upMainPannel" runat="server">
                <ContentTemplate>

                    <div id="tblMain" runat="server">
                        <div class="container-fluid header_main" id="Table1" runat="server">
                            <div class="row">
                                <div class="col-md-3">
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
                                <div class="col-md-6 text-center">
                                    <asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-3 text-right">
                                    <asp:Button ID="ibtnSave" runat="server" CssClass="btn btn-primary" AccessKey="S" Text="Save " ToolTip="Save Data...(Ctrl+F3)"
                                         OnClick="ibtSave_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" ValidationGroup="save" />

                                    <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-primary" Text="Close" ToolTip="Close(Ctrl+F8)" OnClientClick="window.close();" />
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
                        </div>

                        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
                            <tbody>
                                <tr align="center">
                                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2">
                                        <%--<asp:Label ID="lblRegNoInfo" runat="server" Font-Bold="true" Text="Reg.#"></asp:Label>--%>
                                          <asp:Label ID="lblRegNoInfo" runat="server" Font-Bold="true" Text='<%$ Resources:PRegistration, regno%>'></asp:Label>
                                        <asp:Label ID="RegNo" runat="server" Font-Bold="true" ForeColor="#990066"></asp:Label>
                                    </td>
                                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3">
                                        <asp:Label ID="Label5" runat="server" Text="DOB:" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblDob" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-5">
                                        <asp:Label ID="Label4" runat="server" Text="Mobile No:" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6">
                                        <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td data-priority="6" colspan="1" data-columns="tech-companies-1-col-6">
                                        <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblAdmissionDate" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>


                        <div class="container-fluid" id="tblVisit" runat="server">
                            <div class="row form-group">
                                <div class="col-md-3 col-6">
                                    <div class="row">
                                        <div class="col-4 label2">
                                            <asp:Label ID="Label1" runat="server" Text="Specialisation"></asp:Label>
                                        </div>
                                        <div class="col-8">
                                            <telerik:RadComboBox ID="cmbSpecial" runat="server" EnableLoadOnDemand="true"
                                                CssClass="drapDrowHeight" Width="100%" Filter="Contains" AutoPostBack="true" Skin="Office2010Black" OnSelectedIndexChanged="cmbSpecial_OnSelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-6">
                                    <div class="row">
                                        <div class="col-4 label2">
                                            <asp:Label ID="lblDoctor" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label><span style="color: #CC0000">*</span>
                                        </div>
                                        <div class="col-8">
                                            <telerik:RadComboBox ID="cmbDoctor" runat="server" EnableLoadOnDemand="true"
                                                CssClass="drapDrowHeight" Width="100%" Filter="Contains" AutoPostBack="true" Skin="Office2010Black" OnSelectedIndexChanged="cmbDoctor_OnSelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                               
                            </div>

                            <div class="row" id="tdVisit">
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
                                            runat="server" AutoGenerateColumns="false" Height="400px" OnItemDataBound="gvIpVisit_OnItemDataBound">
                                            <MasterTableView Width="100%">
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
                                                        <HeaderStyle Width="100px" />
                                                        <ItemStyle Width="100px" />
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
                                        <asp:Label ID="lblVisitType" runat="server" SkinID="label" Text="Op Visit Type"></asp:Label>
                                        &nbsp; &nbsp; &nbsp;
                                    <telerik:RadComboBox ID="cmbOpVisit" runat="server" Width="400px" EnableLoadOnDemand="true"
                                        Height="100px" Filter="Contains">
                                    </telerik:RadComboBox>
                                        <asp:Button ID="btnAddDoctor" runat="server" Text="Add To Grid" SkinID="Button" OnClick="btnAddDoctor_Click"
                                            OnClientClick="return checkValidVisit()" />
                                        <br />
                                        <br />
                                        <telerik:RadGrid ID="gVConsultation" runat="server" Width="100%" AutoGenerateColumns="false"
                                            SkinID="gridview">
                                            <MasterTableView Width="100%">
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

                            <div class="row">
                                <asp:UpdatePanel ID="uphidden" runat="server">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                                        <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>


                    </div>







                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr id="trVisitDetail" runat="server">
                            <td style="width: 100%; text-align: center;">
                                <table style="width: 100%; text-align: left;">
                                    <tr>
                                        <td style="width: 10%;">
                                            <asp:Label ID="lblvisit" runat="server" Text="Last Visited : " />
                                        </td>
                                        <td style="width: 25%;">
                                            <asp:Label ID="lblVisitDate" runat="server" SkinID="label" />
                                        </td>
                                        <td style="width: 5%;">
                                            <asp:Label ID="lblByService" runat="server" Text=" For : " />
                                        </td>
                                        <td style="width: 33%;">
                                            <asp:Label ID="lblService" runat="server" SkinID="label" />
                                        </td>
                                        <td style="width: 7%;">
                                            <asp:Label ID="lblbyDoc" runat="server" Text="By Doctor" />
                                        </td>
                                        <td style="width: 20%;">
                                            <asp:Label ID="lblByDoctor" runat="server" SkinID="label" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="gVConsultation" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
