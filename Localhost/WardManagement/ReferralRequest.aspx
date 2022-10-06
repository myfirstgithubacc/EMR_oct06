<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master"
    CodeFile="ReferralRequest.aspx.cs" Inherits="WardManagement_ReferralRequest" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />


    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            th.rgHeader {
                padding: 6px 10px !important;
            }
            tr#ctl00_ContentPlaceHolder1_gvDetails_ctl00__0 td {
                padding: 6px 10px !important;
                white-space: nowrap !important;
            }



            textarea#ctl00_ContentPlaceHolder1_txtRemarks {
                height: 94px;
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
                var txt = $get('<%=txtRemarks.ClientID%>');
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

    <asp:UpdatePanel ID="Updatepanel1" runat="server" style="overflow:hidden;">
        <ContentTemplate>

            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2 col-sm-4 col-5">
                            <div class="WordProcessorDivText">
                                <h2>
                                    <asp:Label ID="Label5" runat="server" Text="&nbsp;Cross consultation (Referral)"></asp:Label>
                                </h2>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-4 col-7">
                            <div class="WordProcessorDivText">
                                <h4>
                                    <asp:Label ID="lblmsg" runat="server" Font-Bold="True" Text="&nbsp;" />
                                </h4>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 text-right mb-2 mt-2">
                            <asp:LinkButton ID="lnkBtnDoctorReferralHistory" Visible="false" CssClass="PatientBtn01" runat="server" Text="Doctor Referral History" OnClick="lnkBtnDoctorReferralHistory_OnClick" ToolTip="Doctor Referral History" />

                            <asp:Button ID="btnNew" runat="server" Text="New " ToolTip="New(Ctrl+F2)" CssClass="btn btn-primary" OnClick="btnNew_Click" />
                            <asp:Button ID="btnSave" runat="server" Text="Save " ToolTip="Save(Ctrl+F3)" OnClientClick="ClientSideClick(this)"
                                UseSubmitBehavior="False" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close " ToolTip="Close(Ctrl+F8)" Visible="true" CssClass="btn btn-primary" OnClick="btnClose_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
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
                            <div class="col-sm-4 col-6">
                                <div class="row form-group">
                                    <div class="col-md-2">
                                        <asp:Label ID="Label1" runat="server" Text="Type" />
                                    </div>
                                    <div class="col-md-10">
                                        <asp:RadioButtonList ID="rdoRequestType" runat="server" RepeatDirection="Horizontal" CssClass="radio-custom">
                                            <asp:ListItem Value="0" Text="Routine" Selected="True" />
                                            <asp:ListItem Value="1" Text="Urgent" />
                                            <asp:ListItem Value="2" Text="Stat" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-6">
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-4 text-nowrap">
                                        <asp:Label ID="Label2" runat="server" Text="Specialization" /><sup id="snpSpecialization" runat="server" class="redStar3">*</sup>
                                    </div>
                                    <div class="col-lg-9 col-md-8">
                                        <telerik:RadComboBox ID="ddlspecilization" runat="server" AllowCustomText="true" EnableLoadOnDemand="true" Filter="Contains" AutoPostBack="true" EnableItemCaching="false" MarkFirstMatch="true" TabIndex="2" Width="100%" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-6">
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-4">
                                        <asp:Label ID="ltrltodoctor" runat="server" Text="Resident" />
                                        <sup id="spnReferToDoctor" runat="server" class="redStar3">*</sup>
                                    </div>
                                    <div class="col-lg-9 col-md-8">
                                        <telerik:RadComboBox ID="ddlRequestToDoctor" runat="server" SkinID="DropDown" TabIndex="3"
                                            EnableLoadOnDemand="true" Filter="Contains" Width="100%" Height="300px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-4 col-sm-6 col-6">
                                <div class="row form-group">
                                    <div class="col-lg-4 col-md-4">
                                        <asp:Label ID="Label7" runat="server" Text="Refer From Doctor" /><sup id="Span1" runat="server" class="redStar3">*</sup>
                                    </div>
                                    <div class="col-lg-8 col-md-8">
                                        <telerik:RadComboBox ID="ddlReferFromDoctor" runat="server" SkinID="DropDown" TabIndex="3" AutoPostBack="true" EnableLoadOnDemand="true"
                                            Filter="Contains" Width="100%" Enabled="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-6">
                                <div class="row form-group">
                                    <div class="col-lg-3 col-md-4">
                                        <asp:Label ID="lblActive" runat="server" Text="<%$ Resources:PRegistration, status%>" />
                                    </div>
                                    <div class="col-lg-9 col-md-8">
                                        <telerik:RadComboBox ID="ddlActive" Width="100%" runat="server" SkinID="DropDown">
                                            <Items>
                                                <telerik:RadComboBoxItem Selected="true" Text="Active" Value="1" />
                                                <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-md-3">
                                        <asp:Label ID="Label3" runat="server" Text="Remarks" /><sup class="redStar3">*</sup> <span style="color: #ff0000; font-size: 10px; width: 100%;">(2000 Charactors)</span>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:TextBox ID="txtRemarks" runat="server" onkeyup="return AutoChange();" Style="width: 100%;" TextMode="MultiLine" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRemarks" Display="None" ErrorMessage="Please Enter Reason For Referral" InitialValue="" ValidationGroup="Save" />
                                    </div>
                                </div>
                            </div>
                           
                        </div>
                    </div>
                    <div class="VitalHistory-Div02">
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">
                                    <telerik:RadGrid ID="gvDetails" Skin="Office2007" runat="server" AutoGenerateColumns="false"
                                        AllowMultiRowSelection="false" ShowFooter="false" GridLines="Both" AllowPaging="true" PageSize="10"
                                        OnItemCommand="gvDetails_ItemCommand" OnItemDataBound="gvDetails_OnItemDataBound"
                                        OnPageIndexChanged="gvDetails_PageIndexChanged" CssClass="table-responsive">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="false" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                            <%-- <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />--%>
                                            <Selecting UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                        </ClientSettings>
                                        <MasterTableView Width="100%" TableLayout="auto">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Type" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestTypeName" runat="server" Text='<%#Eval("RequestTypeName")%>' />
                                                        <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Eval("RequestId")%>' />
                                                        <asp:HiddenField ID="hdnRequestType" runat="server" Value='<%#Eval("RequestType")%>' />
                                                        <asp:HiddenField ID="hdnRequestToDoctorId" runat="server" Value='<%#Eval("RequestToDoctorId")%>' />
                                                        <asp:HiddenField ID="hdnRequestStatusCode" runat="server" Value='<%#Eval("RequestStatusCode")%>' />
                                                        <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("Active")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Request Date" ItemStyle-Width="130px" HeaderStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestDate" runat="server" Text='<%#Eval("RequestDate")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Resident" ItemStyle-Width="250px" HeaderStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestToDoctorName" runat="server" Text='<%#Eval("RequestToDoctorName")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestRemarks" runat="server" Text='<%#Eval("RequestRemarks")%>' ToolTip='<%#Eval("RequestRemarks")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nurse" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestedByName" runat="server" Text='<%#Eval("RequestedByName")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Status" ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestStatusName" runat="server" Text='<%#Eval("RequestStatusName")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblEdit" runat="server" CommandName="Select" Text="Edit" />
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

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
