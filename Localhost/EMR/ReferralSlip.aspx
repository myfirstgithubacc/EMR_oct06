<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master"
    CodeFile="ReferralSlip.aspx.cs" Inherits="EMR_ReferralSlip" Title="Referral Request" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
   <%-- <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            tr#ctl00_ContentPlaceHolder1_gvDetails_ctl00__0 td{
                white-space:nowrap!important;
                padding: 6px 8px!important;
            }
            table#ctl00_ContentPlaceHolder1_gvDetails_ctl00 th{
                white-space:nowrap!important;
                padding: 6px 8px!important;
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

                    myButton.value = "Processing....";
                }
                return true;
            }

            function AutoChange() {
                //
                var txt = $get('<%=txtReason.ClientID%>');
                //alert(txt.value.length);
                if (txt.value.length > 2000) {
                    alert("Text length should not be greater then 2000.");

                    txt.value = txt.value.substring(0, 2000);
                    txt.focus();
                }
            }

            function OnClientIsValidPasswordClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                }
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }


            function returnToParent() {
                var oArg = new Object();

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
                    evt = window.Event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {
                    case 113: //F2
                        $get('<%=btnNew.ClientID%>').click();
                        break;
                    case 114:  // F3
                        $get('<%=btnSave.ClientID%>').click();
                        break;
                    case 119:  // F8
                        $get('<%=btnClose.ClientID%>').click();
                        break;
                }
                evt.returnValue = false;
                return false;
            }
        </script>

    </telerik:RadCodeBlock>

    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>

            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2 col-4">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="Label5" runat="server" Text="&nbsp;Referral Request"></asp:Label>
                                </h2>
                            </div>
                        </div>
                        <div class="col-md-5 col-8">
                            <div class="WordProcessorDivText">
                                <h4>
                                    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" Text="&nbsp;" /></h4>
                            </div>
                        </div>
                        <div class="col-md-5 text-right mb-2 mt-md-2">
                            
                            <asp:Button ID="btnNew" runat="server" Text="New(Ctrl+F2)" ToolTip="New(Ctrl+F2)" CssClass="btn btn-primary" OnClick="btnNew_Click" />
                            <asp:Button ID="btnSave" runat="server"  Text="Save (Ctrl+F3)" ToolTip="Save(Ctrl+F3)" OnClientClick="ClientSideClick(this)"
                                UseSubmitBehavior="False" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                             <asp:Button ID="btnAcknowledge" runat="server"  Text="Acknowledge" CssClass="btn btn-primary" OnClick="btnAcknowledge_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close(Ctrl+F3)" ToolTip="Close(Ctrl+F3)" Visible="true" CssClass="btn btn-primary" OnClick="btnClose_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
            <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>

            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </div>
                </div>
            </div>
            <div id="dvZone1">
                <div class="VitalHistory-Div02">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-5">
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label4" runat="server" Text="Type" />
                                    </div>
                                    <div class="col-md-8">
                                        <asp:RadioButtonList ID="rdoUrgent" runat="server" RepeatDirection="Horizontal" CssClass="radio-custom">
                                            <asp:ListItem Value="0" Text="Routine" Selected="True" />
                                            <asp:ListItem Value="1" Text="Urgent" />
                                            <asp:ListItem Value="2" Text="Stat" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-7"></div>
                        </div>
                        <div class="row">
                            <div class="col-md-5 col-6">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label6" runat="server" Text="Request Type" />
                                    </div>
                                    <div class="col-md-8">
                                        <%-- <asp:DropDownList ID="ddlRequestType" runat="server">
                                        </asp:DropDownList>--%>
                                        <telerik:RadComboBox ID="ddlRequestType" runat="server" AllowCustomText="true" EnableLoadOnDemand="true" Filter="Contains" AutoPostBack="true" EnableItemCaching="false" MarkFirstMatch="true" TabIndex="2" Width="230px" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label1" runat="server" Text="Specialization" /><span id="snpSpecialization" runat="server" class="redStar3">*</span>
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlspecilization" runat="server" AllowCustomText="true" EnableLoadOnDemand="true" Filter="Contains" AutoPostBack="true" EnableItemCaching="false" MarkFirstMatch="true" OnSelectedIndexChanged="ddlspecilization_SelectedIndexChanged" TabIndex="2" Width="230px" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                    <asp:Label ID="ltrltodoctor" runat="server" Text="Refer To Doctor" />
                                        <span id="spnReferToDoctor" runat="server" class="redStar3">*</span>
									</div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlRefertodoctor" runat="server" SkinID="DropDown" TabIndex="3" AutoPostBack="true" 
                                            EnableLoadOnDemand="true" Filter="Contains" Width="230px" OnSelectedIndexChanged="ddlRefertodoctor_SelectedIndexChanged" />
                                    
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:Label ID="lblActive" runat="server" Text="<%$ Resources:PRegistration, status%>" Visible="false" />
                                    </div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlActive" runat="server" SkinID="DropDown" Visible="false" Width="230px">
                                            <Items>
                                                <telerik:RadComboBoxItem Selected="true" Text="Active" Value="1" />
                                                <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-7 col-6">
                                
                                <div class="ReferralRequestBox01" id="tblConclusion" runat="server">
                                    <h2>
                                        <asp:Label ID="Label2" runat="server" Text="Reply To Referral"></asp:Label>
                                        <span style="color: #ff0000; font-size: 10px; float: left; width: 100%;">(2000 Charactors)</span>
                                    </h2>
                                    <h4>
                                        <asp:TextBox ID="txtConclusion" runat="server" onkeyup="return AutoChange();" Style="height: 40px; width: 500px;" TextMode="MultiLine"></asp:TextBox></h4>
                                    <asp:Button ID="btnMutipleReply" runat="server" CssClass="PatientBtn01" Text="Multiple Reply" OnClick="btnMutipleReply_OnClick" />
                                    <h5>
                                        <asp:CheckBox ID="chkFinalized" Text="Finalized" runat="server" />
                                    </h5>
                                </div>
                             
                                <div class="ReferralRequestBox01a">
                                    <div class="col-md-4">
                                       <asp:Label ID="Label7" runat="server" Text="Refer From Doctor" /><sup id="Span1" runat="server" class="redStar3">*</sup>

									</div>
                                    <div style="border-bottom: 0; margin-top:8px;" class="col-md-8">
                                       
									 <telerik:RadComboBox ID="ddlReferFromDoctor" runat="server" SkinID="DropDown" TabIndex="3" AutoPostBack="true" EnableLoadOnDemand="true"
                                            Filter="Contains" Width="100%" Enabled="false" />
									</div>
                                    
                                        <asp:Label ID="lblReferToDoctorMobile" runat="server" Text="" Width="100%" />
                                    
                                </div>
                                   <div class="ReferralRequestBox01">
                                    <div class="col-md-4">
                                        <asp:Label ID="Label3" runat="server" Text="Reason For Referral" /><sup class="redStar3">*</sup> <span style="color: #ff0000; font-size: 10px;">(2000 Charactors)</span>
                                    </div>
                                    <div style="border-bottom: 0;" class="col-md-8">
                                        <asp:TextBox ID="txtReason" runat="server" onkeyup="return AutoChange();" Style="height: 40px; width: 100%;" TextMode="MultiLine" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtReason" Display="None" ErrorMessage="Please Enter Reason For Referral" InitialValue="" ValidationGroup="Save" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-5"></div>
                            <div class="col-md-7">
                                <asp:LinkButton ID="lnkBtnDoctorReferralHistory" Visible="false" CssClass="PatientBtn01" runat="server" Text="Doctor Referral History" OnClick="lnkBtnDoctorReferralHistory_OnClick" ToolTip="Doctor Referral History" />
                                <asp:LinkButton ID="lbVisitHistory" runat="server" Visible="false" CssClass="PatientBtn01" Text="Visit History" OnClick="lbVisitHistory_OnClick" ToolTip="Visit History of the Selected Patient"></asp:LinkButton>
                                <asp:LinkButton ID="lbLabHistory" runat="server" Visible="false" CssClass="PatientBtn01" Text="Lab History" OnClick="lbLabHistory_OnClick" ToolTip="Lab History of the Selected Patient"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="VisitHistoryTopBorder">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="ReferralRequestBox01b">
                                        <h2>
                                            <asp:Label ID="lblRegistrationNoCaption" Visible="false" runat="server" Text="Registration No:"></asp:Label>
                                        </h2>
                                        <h3>
                                            <asp:Label ID="lblRegistrationNumber" Visible="true" runat="server" Text="" ForeColor="#990066"></asp:Label>
                                        </h3>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="ReferralRequestBox01b">
                                        <h2>
                                            <asp:Label ID="lblPatNameCaption" Visible="false" runat="server" Text="Patient Name:"></asp:Label>
                                        </h2>
                                        <h3>
                                            <asp:Label ID="lblPatName" Visible="true" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                                        </h3>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="ReferralRequestBox01b">
                                        <h2>
                                            <asp:Label ID="lblFromDoctorCaption" Visible="false" runat="server" Text="From Doctor:"></asp:Label></h2>
                                        <h3>
                                            <asp:Label ID="lblFromDoctor" Visible="true" runat="server" Text="" ForeColor="#990066"></asp:Label></h3>
                                    </div>
                                </div>
                                <div class="col-md-4" style="display: none;">
                                    <div class="ReferralRequestBox01a">
                                        <h2>
                                            <asp:Label ID="lable1" Visible="false" runat="server" Text="Date" />
                                        </h2>
                                        <h3>
                                            <telerik:RadDateTimePicker ID="dtpdate" Visible="false" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm" DateInput-DateDisplayFormat="dd/MM/yyyy HH:mm" Calendar-DayNameFormat="FirstLetter" TabIndex="0" AutoPostBackControl="Both" Calendar-EnableAjaxSkinRendering="True" PopupDirection="BottomRight" Enabled="true" />
                                        </h3>
                                    </div>
                                </div>
                                <div class="col-md-4" style="display: none;">
                                    <div class="ReferralRequestBox01a">
                                        <h2>
                                            <asp:Label ID="lblWardNameCaption" Visible="false" runat="server" Text="Ward:"></asp:Label>
                                        </h2>
                                        <h3>
                                            <asp:Label ID="lblWardName" Visible="true" runat="server" Text="" ForeColor="#990066"></asp:Label>
                                        </h3>
                                    </div>
                                </div>
                                <div class="col-md-4" style="display: none;">
                                    <div class="ReferralRequestBox01a">
                                        <h2>
                                            <asp:Label ID="lblBedCaption" Visible="false" runat="server" Text="Bed No:"></asp:Label>
                                        </h2>
                                        <h3>
                                            <asp:Label ID="lblBed" Visible="true" runat="server" Text="" ForeColor="#990066"></asp:Label>
                                        </h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="VitalHistory-Div02">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12 table-responsive border-0" >
                                    <telerik:RadGrid ID="gvDetails" Skin="Office2007" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false" 
                                        ShowFooter="false" GridLines="Both" AllowPaging="true" PageSize="10" OnItemCommand="gvDetails_ItemCommand" 
                                        OnItemDataBound="gvDetails_OnItemDataBound" OnPageIndexChanged="gvDetails_PageIndexChanged" CssClass="table-custom ">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                            <%-- <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />--%>
                                            <Selecting UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                        </ClientSettings>
                                        <MasterTableView DataKeyNames="ReferralId" Width="100%" TableLayout="auto">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblSelect" runat="server" CommandName="Select" Text="Select"></asp:LinkButton>
                                                        <asp:HiddenField ID="hdnReferralReplyId" runat="server" Value='<%#Eval("ReferralReplyId")%>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnReferralId" runat="server" Value='<%#Eval("ReferralId")%>'></asp:HiddenField>
                                                     <asp:HiddenField ID="hdnAcknowledge" runat="server" Value='<%#Eval("Acknowledge")%>'></asp:HiddenField>
                                                          <asp:HiddenField ID="hdnSpecialisationId" runat="server" Value='<%#Eval("SpecialisationId")%>'></asp:HiddenField>

                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Source" UniqueName="Source" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Patient Name" UniqueName="PatientName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="UHID" UniqueName="RegistrationNo" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Referral Date" UniqueName="ReferralDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtReferralDate" runat="server" Text='<%#Eval("ReferralDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Refer From Dr." UniqueName="FromDoctorName" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtFromDoctorName" runat="server" Text='<%#Eval("FromDoctorName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Refer To Dr." UniqueName="DoctorName" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Referral Notes" UniqueName="Note" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtNote" runat="server" Text='<%#Eval("Note")%>' ToolTip='<%#Eval("Note")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Reply To Referral" UniqueName="DoctorRemark" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txDoctorRemark" runat="server" Text='<%#Eval("DoctorRemark")%>' ToolTip='<%#Eval("DoctorRemark")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="200" HeaderText="Replied By" UniqueName="DoctorRemark" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReplyBy" runat="server" Text='<%#Eval("ReplyBy")%>' ToolTip='<%#Eval("ReplyBy")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Before Conclusion Date" UniqueName="BeforeFinalizedDate" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBeforeFinalizedDate" runat="server" Text='<%#Eval("BeforeFinalizedDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Conclusion Date" UniqueName="ReferralToDate" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtConclusionDate" runat="server" Text='<%#Eval("ReferralConclusionDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Type" UniqueName="Urgent" HeaderStyle-Width="60" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtUrgent" runat="server" Text='<%#Eval("Urgent")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Status" Visible="false" UniqueName="ConcludeReferral" HeaderStyle-Width="80" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txConcludeReferral" runat="server" Text='<%#Eval("ConcludeReferral")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ReferToDoctorIds" UniqueName="ReferToDoctorId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txReferToDoctorId" runat="server" Text='<%#Eval("ReferToDoctorId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="EncodedId" UniqueName="EncodedId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txEncodedId" runat="server" Text='<%#Eval("EncodedId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="SpecialisationId" UniqueName="SpecialisationId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSpecialisationId" runat="server" Text='<%#Eval("SpecialisationId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="EncounterId" UniqueName="EncounterId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txEncounterId" runat="server" Text='<%#Eval("EncounterId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="CompareId" UniqueName="CompareId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCompareId" runat="server" Text='<%#Eval("CompareId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="RegistrationId" UniqueName="RegistrationId" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationId" runat="server" Text='<%#Eval("RegistrationId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="WardName" UniqueName="WardName" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="BedNo" UniqueName="BedNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="EncounterNo" UniqueName="EncounterNo" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="AdmissionDate" UniqueName="AdmissionDate" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmissionDate" runat="server" Text='<%#Eval("AdmissionDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Request Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestType" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Request Type StatusId">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestTypeStatusId" runat="server" Text='<%#Eval("StatusId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="VitalHistory-Div02">
                        <div class="container-fluid">
                            <div class="row">

                                <div class="col-md-12">
                                    <asp:TextBox ID="txtStat" runat="server" Text="" BorderWidth="0" BorderColor="LightGreen" ReadOnly="true" Enabled="false" BackColor="LightGreen" Width="20px"></asp:TextBox>
                                    Stat Referral
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Behaviors="Close,Move,Pin,Resize,Maximize">
                                                <Windows>
                                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                                </Windows>
                                            </telerik:RadWindowManager>
                                            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                <asp:HiddenField ID="hdnRefferToDoctorID" runat="server" />
                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                    </Windows>
                </telerik:RadWindowManager>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
