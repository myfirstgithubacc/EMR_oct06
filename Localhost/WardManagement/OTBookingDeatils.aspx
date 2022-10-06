<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="OTBookingDeatils.aspx.cs" Inherits="OTScheduler_OTBookingDeatils" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="../Include/Components/LegendV1.ascx" TagName="Legend" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/bootstrap.min.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/css/mainNew.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/EMRStyle.css" type="text/css" rel="Stylesheet" />
    <link href="../Include/Style.css" type="text/css" rel="Stylesheet" />


    <style type="text/css">
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { background:#c1e5ef !important;border:1px solid #98abb1 !important; border-top:none !important; color: #333 !important; outline:none !important;}
        /*.RadGrid_Office2007 td.rgPagerCell { border: 1px solid #5d8cc9 !important; background: #c1e5ef !important; outline:none !important;}
        .RadGrid .rgPager .rgStatus {border: none !important;}
        .RadGrid .rgPager .rgStatus { width: 0px !important; padding: 0 !important; margin: 0 !important; float: left !important;}
        .RadGrid_Office2007 .rgGroupHeader { background: #c1e5ef !important; color: #171717 !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol { background:none !important;}*/
     </style>

    <script language="javascript" type="text/jscript">
        function OnClientClose(oWnd) {
            $get('<%=btnfind.ClientID%>').click();
        }
    </script>





    <asp:UpdatePanel ID="updpanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-2 col-sm-3">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="&nbsp;OT&nbsp;Booking&nbsp;Details"></asp:Label>
                        <asp:Button ID="btnfind" runat="server" Text="F" OnClick="btnfind_Click" Style="visibility: hidden;" Width="1px" />
                    </h2>
                </div>
                <div class="col-md-3 col-sm-4 text-center"><asp:Label ID="lblMessage" runat="server"></asp:Label></div>
                <div class="col-md-7 col-sm-5 text-right">
                    <asp:Panel ID="pnlAllButtons" runat="server">
                        <asp:Button ID="btnTagPatient" runat="server" Text="Tag Patient" CssClass="btn btn-default" OnClick="btnTagPatient_OnClick" />
                        <asp:Button ID="btnClinicaldetails" runat="server" Text="Clinical Details" CssClass="btn btn-default" OnClick="btnClinicaldetails_Click" />
                        <asp:Button ID="btnChecklist" runat="server" Text="OT Check List" SkinID="Button" OnClick="btnChecklist_Click" />
                        <asp:Button ID="btnDetails" runat="server" Text="Details" CssClass="btn btn-default" OnClick="btnDetails_Click" />
                        <asp:Button ID="btnInvestigationchart" runat="server" Text="Investigation Chart" Visible="false" CssClass="btn btn-default" OnClick="btnInvestigationchart_Click" />
                        <asp:Button ID="btnServiceRequisition" runat="server" Text="Service Requisition" CssClass="btn btn-default" OnClick="btnServiceRequisition_Click" />
                        <asp:Button ID="btnBloodRequest" runat="server" Text="Blood Requesition" CssClass="btn btn-primary"  OnClick="btnBloodRequest_Click" />
                    </asp:Panel>
                </div>
            </div>



            <div class="container-fluid">
                
                <div class="row form-group">
                    <div class="col-md-3 col-sm-3">
                        <div class="row">
                            <div class="col-md-3 col-sm-4"><asp:Label ID="Label1" runat="server" Text="OT&nbsp;Name"></asp:Label></div>
                            <div class="col-md-9 col-sm-8"><telerik:radcombobox id="ddlotname" runat="server" autopostback="false" width="100%"></telerik:radcombobox></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-4">
                        <div class="row">
                            <div class="col-md-2 col-sm-2"><asp:Label ID="Label2" runat="server" Text="From"></asp:Label></div>
                            <div class="col-md-10 col-sm-10">
                                <div class="row">
                                    <div class="col-md-5 col-sm-5 PaddingRightSpacing"><telerik:RadDatePicker ID="dtpdate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                                    <div class="col-md-2 col-sm-2 label2"><asp:Label ID="Label5" runat="server" Text="To"></asp:Label></div>
                                    <div class="col-md-5 col-sm-5 PaddingLeftSpacing"><telerik:RadDatePicker ID="toDate" runat="server" MinDate="01/01/1900" Width="100%" AutoPostBack="true" OnSelectedDateChanged="toDate_SelectedDateChanged" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-1 col-sm-1 PaddingLeftSpacing"><asp:Button ID="btnRefresh" runat="server" Text="Filter" CssClass="btn btn-primary"  OnClick="btnRefresh_Click" /></div>

                    <div class="col-md-5 col-sm-3">
                        <div class="row">
                            <div class="col-md-2 col-sm-3"><asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label></div>
                            <div class="col-md-10 col-sm-9"><telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" runat="server" Width="100%" TabIndex="0" Filter="Contains"></telerik:RadComboBox></div>
                        </div>
                    </div>
                </div>




                <div class="row form-group">
                    <div class="col-md-5 col-sm-5">
                        <div class="row">
                            <div class="col-md-2 col-sm-3"><asp:Label ID="Label3" runat="server" Text="Remarks"></asp:Label></div>
                            <div class="col-md-10 col-sm-9 PaddingLeftSpacing"><asp:TextBox ID="txtClearanceRemarks" runat="server" TextMode="MultiLine" Width="100%" Height="40px"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3">
                        <asp:Button ID="btnPACClear" runat="server" Text="PAC Clearance" ToolTip="Pre-Anesthesia Check Clearance" CssClass="btn btn-primary" OnClick="btnPACClear_Click" />
                        <asp:Button ID="btnBillClearance" runat="server" Text="Bill Clearance" CssClass="btn btn-primary" OnClick="btnBillClearance_Click" />
                    </div>

                    
                    <div class="col-md-4 col-sm-4 pull-right"><uc1:Legend ID="Legend1" runat="server" /></div>

                </div>
            </div>



            <div class="container-fluid">
                <div class="row">
                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="100%">
                        <telerik:radgrid id="gvDetails" runat="server" autogeneratecolumns="false" allowmultirowselection="false"
                            skin="Office2007" Width="100%" height="460px" showfooter="false" gridlines="None" allowpaging="false"
                            pagesize="10" onitemcommand="gvDetails_ItemCommand" onitemdatabound="gvDetails_ItemDataBound">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true" EnableRowHoverStyle="true">
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="OTBookingID" Width="100%" TableLayout="Fixed">
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
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn  Visible="false">
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
                                    <telerik:GridTemplateColumn HeaderText="Theatre Name" UniqueName="TheatreName" HeaderStyle-Width="100px">
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
                                    <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration,UHID %>" UniqueName="RegistrationNo"
                                        HeaderStyle-Width="65px">
                                        <ItemTemplate>
                                            <asp:Label ID="txRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="<%$ Resources:PRegistration,IpNo %>" UniqueName="IpNo"
                                        HeaderStyle-Width="50px">
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
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
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
                                    <telerik:GridTemplateColumn HeaderText="Order Date" UniqueName="OrderDate" HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
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
                                    <telerik:GridTemplateColumn HeaderText="Age / Gender" UniqueName="AgeGender" HeaderStyle-Width="60px"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="txtAgeGender" runat="server" Text='<%#Eval("AgeGender")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Ward" UniqueName="Ward" HeaderStyle-Width="100px"
                                        Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="txtWard" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Company" UniqueName="Company" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="txtCompany" runat="server" Text='<%#Eval("Company")%>'></asp:Label>
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
                                    <telerik:GridTemplateColumn HeaderText="Status Color" UniqueName="StatusColor" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatusColor" runat="server" Text='<%#Eval("StatusColor")%>'></asp:Label>
                                            <asp:HiddenField id="hdngvPayerType" Value='<%#Eval("PayerType")%>' runat="Server" ></asp:HiddenField>
                                            <asp:HiddenField id="hdngvPayerId" runat="Server" Value='<%#Eval("PayorId")%>' ></asp:HiddenField>
                                            <asp:HiddenField id="hdngvSponsorId" runat="Server" Value='<%#Eval("SponsorId")%>' ></asp:HiddenField>
                                            <asp:HiddenField id="hdngvCardId" runat="Server" Value='<%#Eval("InsuranceCardId")%>' ></asp:HiddenField>
                                            <asp:HiddenField id="hdngvStatusCode" runat="Server" Value='<%#Eval("Code")%>' ></asp:HiddenField>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:radgrid>
                    </asp:Panel>
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
                    <telerik:radwindowmanager id="RadWindowManager" enableviewstate="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:radwindowmanager>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
