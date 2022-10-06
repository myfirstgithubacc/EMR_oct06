<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="OTBookingDeatils.aspx.cs" Inherits="OTScheduler_OTBookingDeatils" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../Include/Components/LegendV1.ascx" TagName="Legend" TagPrefix="uc1" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/bootstrap.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../Include/css/mainNew.css" media="all" />
    <style type="text/css">
        .rgAltRow.rgHoveredRow td:hover {
            border-style: solid !important;
            border-width: 0 0 1px 1px !important;
            border-color: #d0d7e5 !important;
        }
       

        tr#ctl00_ContentPlaceHolder1_gvDetails_ctl00__0 td {
            white-space: nowrap!important;
        }
    </style>



    <script language="javascript" type="text/jscript">
        function OnClientClose(oWnd) {
            $get('<%=btnfind.ClientID%>').click();
        }
    </script>

    <script language="javascript" type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtSearchN.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }


    </script>



    <asp:UpdatePanel ID="updpanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-3 col-sm-3 col-xs-12">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="OT&nbsp;Booking&nbsp;Details"></asp:Label>
                        <asp:Button ID="btnfind" runat="server" Text="F" OnClick="btnfind_Click" Style="visibility: hidden;" Width="1px" />
                    </h2>
                </div>
               
                <div class="col-md-9 col-sm-9 col-xs-12 text-right">
                    <asp:Panel ID="pnlAllButtons" runat="server">
                        <asp:Button ID="btnTagPatient" runat="server" Text="Tag Patient" CssClass="btn btn-primary" OnClick="btnTagPatient_OnClick" />
                        <asp:Button ID="btnClinicaldetails" runat="server" Text="Clinical Details" CssClass="btn btn-primary" OnClick="btnClinicaldetails_Click" />

                        <asp:Button ID="btnCaseSheet" runat="server" Text="Case Sheet" CssClass="btn btn-primary" OnClick="btnCaseSheet_Click" />
                        <asp:Button ID="btnMedicalIllustration" runat="server" Text="Medical Illustration" CssClass="btn btn-primary" OnClick="btnMedicalIllustration_Click" />
                        <asp:Button ID="btnConsentForm" runat="server" Text="Consent Form" CssClass="btn btn-primary" OnClick="btnConsentForm_Click" />
                        <asp:Button ID="btnChecklist" runat="server" Text="OT Check List" CssClass="btn btn-primary" OnClick="btnChecklist_Click" />
                        <asp:Button ID="btnDetails" runat="server" Text="Resource Details" CssClass="btn btn-primary" OnClick="btnDetails_Click" />
                        <asp:Button ID="btnInvestigationchart" runat="server" Text="Investigation Chart" Visible="false" CssClass="btn btn-primary" OnClick="btnInvestigationchart_Click" />
                        <asp:Button ID="btnServiceRequisition" runat="server" Text="Service Requisition" CssClass="btn btn-primary" OnClick="btnServiceRequisition_Click" />
                        <asp:Button ID="btnBloodRequest" runat="server" Text="Blood Requisition" CssClass="btn btn-primary" OnClick="btnBloodRequest_Click" />
                        <asp:Button ID="btnBloodReturn" runat="server" Text="Blood Return" CssClass="btn btn-primary" OnClick="btnBloodReturn_Click" />
                    </asp:Panel>
                </div>
            </div>
                <div class="row text-center">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
          
                <div class="row">
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label1" runat="server" Text="OT&nbsp;Name"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlotname" runat="server" AutoPostBack="false" Width="100%"></telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="row p-t-b-5">
                        <div class="col-md-3 col-sm-3 col-xs-4">
                            <asp:Label ID="Label2" runat="server" Text="From"></asp:Label>
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-8">
                            <telerik:RadDatePicker ID="dtpdate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                        </div>
                    </div>
                </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3 col-xs-4">
                                        <asp:Label ID="Label5" runat="server" Text="To"></asp:Label></div>
                                    <div class="col-md-9 col-sm-9 col-xs-8">
                                        <telerik:RadDatePicker ID="toDate" runat="server" MinDate="01/01/1900" Width="100%" AutoPostBack="true" OnSelectedDateChanged="toDate_SelectedDateChanged" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 co-xs-4 text-nowrap">
                                <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <telerik:RadComboBox ID="ddlName" CssClass="findPatientSelect-Mobile" runat="server"
                                    AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged" Width="110px" DropDownWidth="140px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" Selected="true" />
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                        <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                        <telerik:RadComboBoxItem Text="Ventilator Required" Value="V" />
                                    </Items>
                                </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <telerik:RadComboBox ID="rcbVentilator" CssClass="findPatientSelect-Mobile" runat="server"
                                    AppendDataBoundItems="true" Visible="false" Width="100%">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                        <telerik:RadComboBoxItem Text="No" Value="0" />
                                    </Items>
                                </telerik:RadComboBox>
                                        <asp:TextBox ID="txtSearch" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                    Visible="false" Style="width: 100%; display: inline-block; border: 1px solid #ccc; border-radius: 4px; padding: 3px;" />
                                <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile wd-100" runat="server" Text=""
                                    MaxLength="13" Visible="false" onkeyup="return validateMaxLength();"
                                    Style="width: 100%; display: inline-block; border: 1px solid #ccc; border-radius: 4px; padding: 3px;" />
                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                    FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label4" runat="server" Text="Company"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlcompany" runat="server" AppendDataBoundItems="true" DropDownWidth="300px"
                                    MarkFirstMatch="true" Filter="Contains" Style="width:100%" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                 <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                 <telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" runat="server" Width="100%" TabIndex="0" Filter="Contains"></telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                     
                    
                    
                     
                   <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-8 col-sm-8 col-xs-8 text-nowrap">
                                 <asp:CheckBox ID="chkReExploration" runat="server" /> 
								 <asp:Label ID="Label6" runat="server" Text="Re-exploration"></asp:Label>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Button ID="btnRefresh" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnRefresh_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-2 col-sm-2 col-xs-2 text-nowrap">
                                 <asp:Label ID="Label8" runat="server" Text='Status'></asp:Label>
                            </div>
                            <div class="col-md-10 col-sm-10 col-xs-10">
                                <telerik:RadComboBox ID="rdOTStatus" CssClass="findPatientSelect-Mobile" runat="server"
                                        AppendDataBoundItems="true" Width="120px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Out" Value="0" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Pre" Value="1" />
                                             <telerik:RadComboBoxItem Text="Operation" Value="2" />
                                            <telerik:RadComboBoxItem Text="Post" Value="3" />
                                           
                                        </Items>
                                    </telerik:RadComboBox>
<asp:Button ID="btnShowHideInDashBoard" runat="server" Text="OT Display Board" ToolTip="Show In OT Display" CssClass="btn btn-primary" OnClick="btnShowHideInDashBoard_Click"  />                                </div>
                        </div>
                    </div>
                   <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label3" runat="server" Text="Remarks"></asp:Label>
                                <span style="font: bold; color: red;">*</span>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                               <asp:TextBox ID="txtClearanceRemarks" runat="server" TextMode="MultiLine" Width="100%" Height="40px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12 col-sm-3 col-xs-12  text-right" style="padding-bottom:5px;">
                        <asp:Button ID="btnPACClear" runat="server" Text="PAC Clearance" ToolTip="Pre-Anesthesia Check Clearance" CssClass="btn btn-primary" OnClick="btnPACClear_Click" />
                                <asp:Button ID="btnBillClearance" runat="server" Text="Bill Clearance" CssClass="btn btn-primary" OnClick="btnBillClearance_Click" />
                        </div>
                </div>
                <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 text-right">
                                <uc1:Legend ID="Legend1" runat="server" />
                            </div>
                        </div>
                   

           
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview m-t">
                    <asp:UpdatePanel ID="updGrid" runat="server">
                        <ContentTemplate>

                            <asp:Panel ID="Panel1" runat="server"  Width="1300px">
                                <telerik:RadGrid ID="gvDetails" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                    Skin="Office2007" Width="100%" Height="380px" ShowFooter="false" GridLines="None" AllowPaging="false"
                                    OnItemCommand="gvDetails_ItemCommand" OnItemDataBound="gvDetails_ItemDataBound">
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                        Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true" EnableRowHoverStyle="true">
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="OTBookingID" Width="100%">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="45px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" CommandName="ItemSelect" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="45px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Eval("BillClearance").ToString().Equals("1")%>'
                                                        ToolTip="Check for bill clearance" Visible="false" />
                                                    <asp:HiddenField ID="htnBillClearance" runat="server" Value='<%#Eval("BillClearance")%>' />
                                                     <asp:HiddenField ID="hdnOTDashBoardStatus" runat="server" Value='<%#Eval("OTDashBoardStatus")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtOTBookingID" runat="server" Text='<%#Eval("OTBookingID")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Booking No" UniqueName="BookingNo" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtBookingNo" runat="server" Text='<%#Eval("BookingNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Theatre Name" UniqueName="TheatreName" HeaderStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtTheatreName" runat="server" Text='<%#Eval("TheatreName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <%--<telerik:GridTemplateColumn HeaderText="Booking Date" Visible="true" HeaderStyle-Width="68" >
                                        <ItemTemplate>
                                            <asp:Label ID="txtBookingDate" runat="server" Text='<%#Eval("BookingDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                            <telerik:GridTemplateColumn HeaderText="Start Time" UniqueName="StartTime" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtStartTime" runat="server" Text='<%#Eval("StartTime")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="End Time" UniqueName="EndTime" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtEndTime" runat="server" Text='<%#Eval("EndTime")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>


                                            <telerik:GridTemplateColumn HeaderText="Check In" UniqueName="OTCheckintime" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtOTCheckintime" runat="server" Text='<%#Eval("OTCheckintime")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Check Out" UniqueName="OTCheckouttime" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtOTCheckouttime" runat="server" Text='<%#Eval("OTCheckouttime")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <%--palendra--%>
                                             <telerik:GridTemplateColumn HeaderText="OT Dashboard" UniqueName="DashBoardStatus" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDashBoardStatus" runat="server" Text='<%#Eval("DashBoardStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                             <%--palendra--%>
                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderStyle-Width="70px" ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
                                                <HeaderTemplate>
                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Session["RegistrationLabelName"]%>' />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="txRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration,IpNo %>" UniqueName="IpNo"
                                                HeaderStyle-Width="80px" ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txIpNo" runat="server" Text='<%#Eval("IpNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Patient Name" UniqueName="Patient" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txPatient" runat="server" Text='<%#Eval("Patient")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="250px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="WardName"  UniqueName="WardName" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="BedNo" UniqueName="BedNo" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtBedNo" runat="server" Text='<%#Eval("BedNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Admitting Doctor" UniqueName="AdmittingDoctor"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtDoctor" runat="server" Text='<%#Eval("AdmittingDoctor")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <%--palendra--%>
                                             <telerik:GridTemplateColumn HeaderText="Advance Amt" UniqueName="AdvanceAmt"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtAdvanceAmt" runat="server" Text='<%#Eval("AdvanceAmt")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Surgery" UniqueName="Surgery" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtSurgery" runat="server" Text='<%#Eval("Surgery")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Booking Status" UniqueName="BookingStatus"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookingStatus" runat="server" Text='<%#Eval("BookingStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Order No" UniqueName="OrderNo" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtOrderNo" runat="server" Text='<%#Eval("OrderNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Posted Date" UniqueName="OrderDate" HeaderStyle-Width="110px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Posted Date" UniqueName="PostedDate" HeaderStyle-Width="110px" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPostedDate" runat="server" Text='<%#Eval("surgeryPostedDate") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Bill Clearance" UniqueName="BillClearance"
                                                HeaderStyle-Width="105px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBillClearance" runat="server" Text='<%#Eval("BillClearance")%>'
                                                        ToolTip='<%#Eval("BillClearanceDet")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="PAC Clearance" UniqueName="PACClearance"
                                                HeaderStyle-Width="105px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPACClearance" runat="server" Text='<%#Eval("PACClearance")%>' ToolTip='<%#Eval("PACClearanceDet")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Ventilator Required" UniqueName="IsVentilator" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="IsVentilator" runat="server" Text='<%#Eval("IsVentilator")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Age / Gender" UniqueName="AgeGender" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtAgeGender" runat="server" Text='<%#Eval("AgeGender")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Ward" UniqueName="Ward" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"
                                                Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtWard" runat="server" visible="false" Text='<%#Eval("WardName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Company"  UniqueName="Company" ItemStyle-Wrap="false" HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtCompany" visible="false" runat="server" Text='<%#Eval("Company")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="RegistrationId" UniqueName="RegistrationID"
                                                Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtRegistrationID" runat="server" Text='<%#Eval("RegistrationID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="EncounterId" UniqueName="EncounterID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtEncounterID" runat="server" Text='<%#Eval("EncounterID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="BedId" UniqueName="CurrentBedId" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtCurrentBedId" runat="server" Text='<%#Eval("CurrentBedId")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Estimate" HeaderStyle-HorizontalAlign="Center"
                                                AllowFiltering="false" HeaderText="Estimate" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="IbtnEstimate" runat="server" Text="View" CommandName="Estimate" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>          
                                            <telerik:GridTemplateColumn HeaderText="Status Color" UniqueName="StatusColor" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvPayerType" Value='<%#Eval("PayerType")%>' runat="Server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hdngvPayerId" runat="Server" Value='<%#Eval("PayorId")%>'></asp:HiddenField>
                                                    <asp:HiddenField ID="hdngvSponsorId" runat="Server" Value='<%#Eval("SponsorId")%>'></asp:HiddenField>
                                                    <asp:HiddenField ID="hdngvCardId" runat="Server" Value='<%#Eval("InsuranceCardId")%>'></asp:HiddenField>
                                                    <asp:HiddenField ID="hdngvStatusCode" runat="Server" Value='<%#Eval("Code")%>'></asp:HiddenField>
                                                    <asp:HiddenField ID="hdnEncounterDate" runat="Server" Value='<%#Eval("EncounterDate")%>'></asp:HiddenField>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </asp:Panel>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                    </div>

                <div class="row">
                    <asp:HiddenField ID="hdnIpno" runat="server" />
                    <asp:HiddenField ID="hdnWardno" runat="server" />
                    <asp:HiddenField ID="hdnPatientname" runat="server" />
                    <asp:HiddenField ID="hdnSurgeryname" runat="server" />
                    <asp:HiddenField ID="hdnBookinId" runat="server" />
                    <asp:HiddenField ID="hdnSurgeryId" runat="server" />
                    <asp:HiddenField ID="hdnBedId" runat="server" />
                    <asp:HiddenField ID="hdnPACClearance" runat="server" />
                    <asp:HiddenField ID="hdnBookingStatus" runat="server" />
                    <asp:HiddenField ID="hdnpayerType" runat="Server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnpayerId" runat="Server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnSponsorId" runat="Server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnCardId" runat="Server"></asp:HiddenField>
                    <asp:HiddenField ID="hdnCode" runat="Server"></asp:HiddenField>
                </div>

                <div class="row">
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
