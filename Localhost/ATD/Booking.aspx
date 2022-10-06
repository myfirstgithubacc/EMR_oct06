<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Booking.aspx.cs" Inherits="ATD_Booking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript" language="javascript">
            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    if (RegistrationNo != "") {
                        $get('<%=txtRegNo.ClientID%>').value = RegistrationNo;
                        $get('<%=btnfind.ClientID%>').click();
                    }
                }

            }
            function SearchBookingOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var BookingNo = arg.BookingNo;
                    if (BookingNo != "") {
                        $get('<%=txtBookingNo.ClientID%>').value = BookingNo;
                        $get('<%=btnSearchByBookingNo.ClientID%>').click();
                    }
                }
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
                        $get('<%=ibtnNew.ClientID%>').click();
                        break;
                    case 114:  // F3
                        $get('<%=btnsave.ClientID%>').click();
                        break;


                }
                evt.returnValue = false;
                return false;

            }
        </script>

    </telerik:RadCodeBlock>
        <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />


        <asp:UpdatePanel ID="mainupd" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                 <div class="container-fluid">
                     <div class="row header_main">
                <asp:Panel ID="panel4" runat="server" DefaultButton="btnfind">
                        
                        <div class="col-md-2 col-sm-2 col-xs-3 text-nowrap">
                            <h2>Patient Booking</h2>
                        </div>
                        <div class="col-md-2 col-sm-2 col-xs-3">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-xs-4">
                                    <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                                Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click"></asp:LinkButton>
                                </div>
                                <div class="col-md-8 col-sm-8 col-xs-8">
                                    <asp:TextBox ID="txtRegNo" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>
                            <asp:Button ID="btnfind" runat="server" Text="Select" SkinID="Button" OnClick="btnfind_Click"
                                ValidationGroup="Search" Width="1px" Style="visibility: hidden;display:none;" />
                                </div>
                            </div>
                        </div>

                            <div class="col-md-3 col-sm-3 col-xs-6">
                                 <asp:Panel ID="panel1" runat="server" DefaultButton="btnSearchByBookingNo">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-5 text-nowrap">
                                   <asp:LinkButton ID="lbtnSearchBookings" runat="server" Text="Admission Request No."
                                    Font-Underline="false" ToolTip="Click to search booking" OnClick="lbtnSearchBookings_OnClick"></asp:LinkButton>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-7">
                                   <asp:TextBox ID="txtBookingNo" runat="server" Width="100%" SkinID="textbox"></asp:TextBox>

                                <asp:Button ID="btnSearchByBookingNo" runat="server" Text="Select" SkinID="Button"
                                    OnClick="btnSearchByBookingNo_Click" ValidationGroup="SearchBB" Width="1px" Style="visibility: hidden;display:none;" />
                                </div>
                            </div>
                                     </asp:Panel>
                        </div>

                        <div class="col-md-5 col-sm-5 col-xs-12 text-right">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRegNo"
                                ErrorMessage="Enter Reg No." SetFocusOnError="True" ValidationGroup="Search"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtRegNo"
                                Display="Static" ErrorMessage="Select Patient" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBookingNo"
                                ErrorMessage="Enter Booking Slip No." SetFocusOnError="True" ValidationGroup="SearchBB"></asp:RequiredFieldValidator>


                            <asp:Button ID="ibtnNew" runat="server" Text="New (Ctrl+F2)" CssClass="btn btn-primary" OnClick="ibtnNew_Click"
                                AccessKey="N" />
                            <asp:Button ID="btnsave" runat="server" Text="Save (Ctrl+F3)" CssClass="btn btn-primary" OnClick="btnsave_Click"
                                ValidationGroup="Save" AccessKey="S" />
                            <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
                        </div>
                   
                </asp:Panel>
                          </div>



                <div class="row text-center">
                    <asp:Label ID="lblmsg" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="9pt"
                        Font-Names="verdana"></asp:Label>
                </div>

                <div class="row">
                    <div id="trInfo1" runat="server" visible="false" class="margin_bottom">

                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                            Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblPatientName1" runat="server" Text="" SkinID="label" ForeColor="#990066"
                            Font-Bold="true"></asp:Label>
                        <asp:Label ID="Label23" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                        <asp:Label ID="Label24" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>

                    </div>
                    <div id="trInfo2" runat="server" visible="false">

                        <asp:Label ID="Label27" runat="server" Text="Address:" SkinID="label" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblAddress" runat="server" Text="" SkinID="label"></asp:Label>
                        <asp:Label ID="Label28" runat="server" Text="City:" SkinID="label" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblCity" runat="server" Text="" SkinID="label"></asp:Label>
                        <asp:Label ID="Label30" runat="server" Text="State:" SkinID="label" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblState" runat="server" Text="" SkinID="label"></asp:Label>
                        <asp:Label ID="Label32" runat="server" Text="Country:" SkinID="label" Font-Bold="true"></asp:Label>
                        <asp:Label ID="lblCountry" runat="server" Text="" SkinID="label"></asp:Label>

                    </div>
                    <div id="trInfo3" runat="server" visible="false">
                        <aspl1:UserDetail ID="pd1" runat="server" />
                        <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>--%>
                                   
                    </div>
                </div>

                

                <div class="row">

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label2" runat="server" Text="Booking Date" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadDatePicker ID="dtpbookingdate" runat="server" Width="100%"
                                    Enabled="false">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>

                   <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label3" runat="server" Text="Booking Type" SkinID="label"></asp:Label><span><font
                                    color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlBookingType" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="[Select]" Value="" />
                                        <telerik:RadComboBoxItem Text="Surgery" Value="S" />
                                        <telerik:RadComboBoxItem Text="Delivery" Value="D" />
                                        <telerik:RadComboBoxItem Text="Medical Management" Value="G" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label4" runat="server" Text="Booking source" SkinID="label"></asp:Label><span><font
                                    color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlBookingSource" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>


                </div>
                <div class="row">
                   <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label5" runat="server" Text="Booking Status" SkinID="label"></asp:Label>
                                <span id="spnBookingStatus" runat="server" class="red">*</span></h2>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlBookingStatus" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Wait Listed" Value="WL" Selected="true" />
                                        <telerik:RadComboBoxItem Text="Confirmed" Value="CO" />
                                        <telerik:RadComboBoxItem Text="Admitted" Value="AD" />
                                        <telerik:RadComboBoxItem Text="Cancelled" Value="CN" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label6" runat="server" Text="First Bed Cat" SkinID="label"></asp:Label>
                                <span id="FirstBedCategoryStar" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlFirstBedCategory" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlFirstBedCategory_OnSelectedIndexChanged">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                   <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label7" runat="server" Text="Second Bed Cat" SkinID="label"></asp:Label>
                                <span id="spnSecondBedCategory" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlSecondBedCategory" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>



                <div class="row">

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label8" runat="server" Text="Third Bed Cat" SkinID="label"></asp:Label>
                                <span id="spnThirdBedCategory" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlThirdBedCategory" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label10" runat="server" Text="Booking Doctor" SkinID="label"></asp:Label>
                                <span><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlBookingDoctor" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label11" runat="server" Text="Exp. Admt. Date" SkinID="label"></asp:Label>
                                <span id="spnExpecAdmtDate" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadDatePicker ID="dtpExpecAdmtDate" runat="server" Width="100%">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label9" runat="server" Text="Probable Stay (Days)" SkinID="label"></asp:Label>
                                <span id="spnProbableStayInDays" runat="server"><font color="red">*</font></span>

                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtProbableStayInDays" runat="server" SkinID="textbox" Width="100%" MaxLength="2"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                    TargetControlID="txtProbableStayInDays" FilterType="Custom, Numbers"
                                    ValidChars="1234567890">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label17" runat="server" Text="Bed No" SkinID="label"></asp:Label>
                                <%--<span id="Span1" runat="server"><font color="red">*</font></span>--%>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadComboBox ID="ddlBedNo" runat="server" Filter="Contains" MarkFirstMatch="true"
                                    Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label18" runat="server" Text="Exp. Disch. Date" SkinID="label"></asp:Label>
                                <%--<span id="Span2" runat="server"><font color="red">*</font></span>--%>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <telerik:RadDatePicker ID="dtpExpDischDate" runat="server" Width="100%">
                                </telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label12" runat="server" Text="Reason For Admission" SkinID="label"></asp:Label>
                                <span id="spnReasonForAdmission" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtReasonForAdmission" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label13" runat="server" Text="Procedure Name" SkinID="label"></asp:Label>
                                <span id="spnProcedureName" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtProcedureName" runat="server" SkinID="textbox" TextMode="MultiLine"
                                    Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label15" runat="server" Text="Procedure Duration" SkinID="label"></asp:Label>
                                <span id="spnProcedureDuration" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtProcedureDuration" runat="server" SkinID="textbox" TextMode="MultiLine"
                                    Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label16" runat="server" Text="Implant Details" SkinID="label"></asp:Label>
                                <span id="spnImplantDetails" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtImplantDetails" runat="server" SkinID="textbox" TextMode="MultiLine"
                                    Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label14" runat="server" Text="Anaesthesia Type" SkinID="label"></asp:Label>
                                <span id="spnAnaesthesiaType" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtAnaesthesiaType" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-xs-4 text-normal">
                                <asp:Label ID="Label1" runat="server" Text="Remarks" SkinID="label"></asp:Label>
                                <span id="spnRemarks" runat="server"><font color="red">*</font></span>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-8">
                                <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" TextMode="MultiLine"
                                    Width="100%" Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>

                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnPatientAgeInYrs" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnTaggedEmpNo" runat="server" Value="" />
                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value="" />
                        <asp:HiddenField ID="hdnBookingId" runat="server" Value="0" />
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move,Maximize,Minimize">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
  </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ibtnNew" />
                <asp:AsyncPostBackTrigger ControlID="btnsave" />
            </Triggers>
          
        </asp:UpdatePanel>
</asp:Content>
