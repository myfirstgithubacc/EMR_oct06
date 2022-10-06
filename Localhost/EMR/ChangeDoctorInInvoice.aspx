<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeDoctorInInvoice.aspx.cs"
    MasterPageFile="~/Include/Master/EMRMaster.master" Inherits="EMRBILLING_ChangeDoctorInInvoice" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="../Include/Components/PatientQView.ascx" TagName="PatientQView"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



     <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>




    <script language="javascript" type="text/javascript">
        function openRadWindow(strQueryString) {
            var oWnd = radopen(strQueryString, "RadWindow1");
        }
        function SearchPatientOnClientClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                var EncounterNo = arg.EncounterNo;
                var EncounterId = arg.EncounterId;
                var AgeGender = arg.AgeGender;
                var PhoneHome = arg.PhoneHome;
                var MobileNo = arg.MobileNo;
                var PatientName = arg.PatientName;
                var DOB = arg.DOB;
                var Address = arg.Address;

                $get('<%=txtInvoicenoNo.ClientID%>').value = RegistrationNo;
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnEncounterId.ClientID%>').value = EncounterId;
                $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;
                $get('<%=hdnAgeGender.ClientID%>').value = AgeGender;
                $get('<%=hdnPhoneHome.ClientID%>').value = PhoneHome;
                $get('<%=hdnMobileNo.ClientID%>').value = MobileNo;
                $get('<%=hdnPatientName.ClientID%>').value = PatientName;
                $get('<%=hdnDOB.ClientID%>').value = DOB;
                $get('<%=hdnAddress.ClientID%>').value = Address;
            }
            $get('<%=btnSearchPatient.ClientID%>').click();
        }

        function wndAddService_OnClientClose(oWnd, args) {

        }
        function CalculateFromUnit(txtServiceAmount, txtDoctorAmount, txtUnits, txtDiscountPercent, txtDiscountAmt, hdnAmountPayableByPayer, hdnAmountPayableByPatient, txtNetCharge) {

            if (document.getElementById(txtUnits).value < 1) {
                alert('Unit should be atleast one !');
                document.getElementById(txtUnits).focus();
                document.getElementById(txtUnits).value = 1;
                return false;
            }
            var unit = document.getElementById(txtUnits).value;
            var discPer = document.getElementById(txtDiscountPercent).value;

            if (discPer > 0) {
                document.getElementById(txtDiscountAmt).value = (((discPer * 1) / 100) * ((unit * 1) * ((document.getElementById(txtServiceAmount).value * 1) + (document.getElementById(txtDoctorAmount).value * 1)))).toFixed(2);
            }
            else {
                document.getElementById(txtDiscountAmt).value = (0 * 1).toFixed(2);
            }

            //-----------------

            if (document.getElementById(hdnAmountPayableByPatient).value > 0) {
                document.getElementById(hdnAmountPayableByPatient).value = ((((document.getElementById(txtServiceAmount).value * 1) + (document.getElementById(txtDoctorAmount).value * 1)) * (unit * 1)) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2)
            }
            if (document.getElementById(hdnAmountPayableByPayer).value > 0) {
                document.getElementById(hdnAmountPayableByPayer).value = ((((document.getElementById(txtServiceAmount).value * 1) + (document.getElementById(txtDoctorAmount).value * 1)) * (unit * 1)) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2)
            }

            document.getElementById(txtNetCharge).value = ((((document.getElementById(txtServiceAmount).value * 1) + (document.getElementById(txtDoctorAmount).value * 1)) * (unit * 1)) - (document.getElementById(txtDiscountAmt).value * 1)).toFixed(2)

            var ftrTotalUnit = 0;
            var ftrTotalPatient = 0;
            var totDisc = 0;
            var gridview = document.getElementById('<%=gvService.ClientID %>');
            var length = gridview.rows.length;
            var rowidx = '';
            for (i = 0; i < length; i++) {
                if (i > 1) {
                    if (i < 10) {
                        rowidx = '0' + i.toString();
                    }
                    else {
                        rowidx = i.toString();
                    }
                    var IsPackageService = 0;
                    IsPackageService = $get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_hdnIsPackageService').value;
                    if (IsPackageService == 0) {

                        var Discount = $get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtDiscountAmt').value;
                        totDisc += parseInt(Discount);

                        var ptamt = $get('ctl00_ContentPlaceHolder1_gvService_ctl' + rowidx.toString() + '_txtNetCharge').value;
                        ftrTotalPatient += parseInt(ptamt);

                    }
                }
            }
            var footerrow = gridview.rows.length;
            var footerrowidx = '';
            if (footerrow < 10) {
                footerrowidx = '0' + footerrow.toString();
            }
            else {
                footerrowidx = footerrow.toString();
            }
            $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotDiscountAmt').value = totDisc.toFixed(2);
            $get('ctl00_ContentPlaceHolder1_gvService_ctl' + footerrowidx.toString() + '_txtTotNetCharge').value = ftrTotalPatient.toFixed(2);


        }
        function BindInsuranceOnclose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var payerId = arg.payerId;
                var SponsorId = arg.SponsorId;
                var CardId = arg.CardId;
                var CardValidDate = arg.CardValidDate;

                $get('<%=hdnPayer.ClientID%>').value = payerId;
                $get('<%=hdnSponsor.ClientID%>').value = SponsorId;
                $get('<%=hdnCardId.ClientID%>').value = CardId;
                $get('<%=hdnCardValidDate.ClientID%>').value = CardValidDate;

                document.getElementById('<%=btnFillInsurance.ClientID%>').click();

            }

        }
        
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
  
                       
                     <div class="container-fluid header_main ">
                 <div class="col-md-3">
                  <h2> <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Change Doctor In Invoice"
                            Font-Bold="true" /></h2>
                 </div>
				 
                 <div class="col-md-5 text-center form-group">
                      <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByUHID">
                                        <asp:DropDownList ID="ddlSelect" runat="server" SkinID="DropDown" Width="50%">
                                            <asp:ListItem Text="InvoiceNo" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="OrderNo" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtInvoicenoNo" runat="server" SkinID="textbox" Width="100px" TabIndex="0"
                                            MaxLength="25" Style="padding-left: 2px;" Enabled="false"  />
                                        <asp:Button ID="btnSearchByUHID" runat="server" OnClick="btnSearchByUHID_OnClick"
                                            SkinID="Button" Text="" CausesValidation="false" Style="visibility: hidden;"
                                            Width="1px" />
                                        <asp:Button ID="btnSearchPatient" runat="server" OnClick="btnSearchPatient_OnClick"
                                            SkinID="Button" CausesValidation="false" Style="visibility: hidden;" Width="1" />
                                    </asp:Panel>

                 </div>
                 
                 <div class="col-md-3 text-right pull-right">
                      <asp:Button ID="btnNew" runat="server" ToolTip="New" cssClass="btn btn-default" Text="New" OnClick="btnNew_OnClick" />
                        <asp:Button ID="btnSave" runat="server" ToolTip="Save" cssClass="btn btn-primary" Text="Save"
                            OnClick="btnSave_OnClick" />

                 </div>
                </div>   
                        
                        
                        
                        
                        
                        
                        
                        
                        














            <div class="container-fluid text-center">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" />
                            </ContentTemplate>
                        </asp:UpdatePanel>

            </div>

            










            <asp:Panel ID="Panel1" runat="server" Width="99%">
                <table cellpadding="2" cellspacing="2" border="0" width="100%">
                    <tr>
                        <td style="border: 1px solid #000000; background-color: #FFFFFF; width: 15%; height: 110px;">
                            <%-- <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>--%>
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label6" runat="server" Text="<font size=1 color=Crimson>Patient Details</font>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc1:PatientQView ID="PatientQView1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                        </td>






                        <td style="border: 1px solid #000000; background-color: #FFFFFF; vertical-align: top;
                            width: 30%;">

                            <div class="container-fluid">
                                <div class="row">
            <div class="col-md-12 form-group">
                <div class="row">
                    <div class="col-md-4"> <asp:Label ID="Label8" runat="server" Text="Type" SkinID="label" />
                                        <font color="Red">*</font> <asp:Label ID="Label3" runat="server" SkinID="label" Text=" : "></asp:Label></div>
                    <div class="col-md-8">  <telerik:RadComboBox ID="ddlPayParty" runat="server" AutoPostBack="true" Width="95%"
                                            SkinID="DropDown" Font-Size="10px" OnSelectedIndexChanged="ddlPayParty_OnSelectedIndexChanged"
                                            DropDownWidth="320px" TabIndex="1">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="Direct Patient" />
                                                <telerik:RadComboBoxItem Value="1" Text="Company" />
                                                <telerik:RadComboBoxItem Value="2" Text="Insurance" />
                                            </Items>
                                        </telerik:RadComboBox>
</div>
                </div>

            </div>
             <div class="col-md-12  form-group">
                <div class="row">
                    <div class="col-md-4"><asp:Label ID="Label9" runat="server" Text="Payer" SkinID="label" />
                                        <font color="Red">*</font>                                        <asp:Label ID="Label12" runat="server" SkinID="label" Text=" : "></asp:Label>
</div>
                    <div class="col-md-8"><telerik:RadComboBox ID="ddlCompany" AutoPostBack="true" runat="server" Width="95%"
                                            Font-Size="10px" SkinID="DropDown" OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged"
                                            Filter="Contains" DropDownWidth="320px" TabIndex="2">
                                        </telerik:RadComboBox></div>
                </div>

            </div>
            <div class="col-md-12  form-group">
                <div class="row">
                    <div class="col-md-4"> <asp:Label ID="Label10" runat="server" Text="Sponsor" SkinID="label" />
                                        <font color="Red">*</font>                                        <asp:Label ID="Label11" runat="server" SkinID="label" Text=" : "></asp:Label>
</div>
                    <div class="col-md-8">  <telerik:RadComboBox ID="ddlSponsor" AutoPostBack="false" runat="server" Width="95%"
                                            Filter="Contains" DropDownWidth="320px" TabIndex="3" Font-Size="10px">
                                        </telerik:RadComboBox></div>
                </div>

            </div>












 <div class="col-md-12  form-group">
     <div class="row">
                    <div class="col-md-4"> <asp:Label ID="lblCardId" runat="server" Text='<%$ Resources:PRegistration, Card%>'
                                            SkinID="label"></asp:Label><asp:Label ID="Label4" runat="server" SkinID="label" Text=" : "></asp:Label></div>
                    <div class="col-md-8"> <telerik:RadComboBox ID="ddlCardId" runat="server" Width="95%" Font-Size="10px" SkinID="DropDown"
                                            Filter="Contains" DropDownWidth="220px" />
                                        <asp:TextBox ID="txtCard" SkinID="textbox" runat="server" Visible="false" Width="90%"></asp:TextBox>
                                        <asp:LinkButton ID="lnkInsuranceDetails" runat="server" Visible="false">Insurance&nbsp;Details</asp:LinkButton></div>
                </div>
     </div>
            </div>



    </div>

                            </div>



                         
            </asp:Panel>
            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="pnlgvService" runat="server" Width="99%">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvService" TabIndex="7" runat="server" AutoGenerateColumns="False"
                                        ShowFooter="false" SkinID="gridview2" OnRowDataBound="gvService_RowDataBound"
                                        Width="100%" OnRowCommand="gvService_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltrId" runat="server" Text='<%#Eval("SNO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Literal ID="LblOrderNo" runat="server" Text='<%#Eval("OrderNo") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Literal ID="LblOrderDate" runat="server" Text='<%#Eval("OrderDate") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service" HeaderStyle-Width="25%" ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnid" runat="server" Value='<%#Eval("id") %>' />
                                                    <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("Orderid") %>' />
                                                    <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId")%>' />
                                                    <asp:Literal ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Literal>
                                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                                    <asp:HiddenField ID="hdnINVOICEID" runat="server" Value='<%#Eval("INVOICEID")%>' />
                                                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value='<%# Convert.ToString(Eval("RegistrationNo")) %>' />
                                                    <asp:HiddenField ID="hdnNewDoctor" runat="server" Value='<%# Convert.ToString(Eval("NewDoctor")) %>' />
                                                    <asp:HiddenField ID="hdnolddoctor" runat="server" Value='<%# Convert.ToString(Eval("olddoctorid")) %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblInfoTotal" runat="server" Font-Bold="true" SkinID="label" Width="99%"
                                                        ForeColor="White"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Department Type" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Literal ID="LblDepartmentType" runat="server" Text='<%#Eval("DepartmentType") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            
                                            <asp:TemplateField HeaderText="Doctor Name" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Literal ID="LblDoctorName" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New Doctor" HeaderStyle-Width="10%" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblDoctorID" runat="server" Text='<%#Eval("NewDoctor") %>'></asp:Label>--%>
                                                    <telerik:RadComboBox ID="ddlDoctor" runat="server" Filter="Contains" MarkFirstMatch="true"
                                                        Width="98%" Skin="Metro" Height="250px" DropDownWidth="300px" AutoPostBack="true">
                                                    </telerik:RadComboBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                          
                                            <asp:TemplateField HeaderText="SOC" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtSoc" runat="server" Text='<%#Eval("SOS_AMOUNT") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Service Amount" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtServiceAmount" runat="server" Text='<%#Eval("ServiceAmount") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Provider Percent" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtProviderPercent" runat="server" Text='<%#Eval("ProviderPercent") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <FooterStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%--   <asp:TemplateField HeaderText="Payable Amt" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNetCharge" runat="server" Enabled="false" SkinID="textbox" Style="text-align: right;"
                                                        Width="98%" Text='<%#Eval("NetCharge","{0:f2}") %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtTotNetCharge" SkinID="textbox" ReadOnly="true" runat="server"
                                                        Width="98%" Style="text-align: right;"></asp:TextBox>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Right" />
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>--%>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:UpdatePanel ID="updDivConfirm" runat="server">
                            <ContentTemplate>
                                <div id="dvConfirmPrintingOptions" runat="server" style="width: 400px; z-index: 200;
                                    border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC;
                                    border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute;
                                    bottom: 0; height: 140px; left: 270px; top: 200px;">
                                    <table cellspacing="2" cellpadding="2" width="400px">
                                        <tr>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:Label ID="lblSn" runat="server" Text="Service name :" ForeColor="#990066" />
                                            </td>
                                            <td style="width: 70%; text-align: left;">
                                                <asp:Label ID="lblServiceName1" runat="server" ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%; text-align: left;">
                                                <asp:Label ID="Label5" runat="server" Text="Already posted by :" ForeColor="#990066" />
                                            </td>
                                            <td style="width: 70%; text-align: left;">
                                                <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr style="border-bottom-style: solid; border-bottom-width: 1px;">
                                            <td style="width: 30%; text-align: left;">
                                                <asp:Label ID="Label7" runat="server" Text="Posted date :" ForeColor="#990066" />
                                            </td>
                                            <td style="width: 70%; text-align: left;">
                                                <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%; text-align: center;">
                                                <asp:Label ID="lblAlertMsg" runat="server" Font-Size="12px" Text="Do you wish to continue...?"
                                                    ForeColor="#990066" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width: 100%; text-align: center;">
                                                <asp:Button ID="btnYes" runat="server" Text="Proceed" OnClick="btnYes_OnClick" SkinID="Button" />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_OnClick"
                                                    SkinID="Button" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="850" Height="500"
                                    InitialBehaviors="Maximize" Left="10" Top="10" VisibleStatusbar="false" Behaviors="Close,Minimize,Maximize"
                                    Modal="true" OnClientClose="SearchPatientOnClientClose">
                                </telerik:RadWindowManager>
                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:HiddenField ID="hdnOPIP" runat="server" Value="O" />
                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnEncounterDate" runat="server" Value="" />
                                <asp:HiddenField ID="hdnAgeGender" runat="server" Value="" />
                                <asp:HiddenField ID="hdnPhoneHome" runat="server" Value="" />
                                <asp:HiddenField ID="hdnMobileNo" runat="server" Value="" />
                                <asp:HiddenField ID="hdnPatientName" runat="server" Value="" />
                                <asp:HiddenField ID="hdnDOB" runat="server" Value="" />
                                <asp:HiddenField ID="hdnAddress" runat="server" Value="" />
                                <asp:HiddenField ID="hdnCompanyCode" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnSponsorName" runat="server" Value="" />
                                <asp:HiddenField ID="hdnInsCode" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnPayerName" runat="server" Value="" />
                                <asp:HiddenField ID="hdnOrderNo" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnCompanyType" runat="server" Value="C" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="2" />
                                <asp:HiddenField ID="hdnPayer" runat="server" />
                                <asp:HiddenField ID="hdnSponsor" runat="server" />
                                <asp:HiddenField ID="hdnCardId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnCardValidDate" runat="server" Value="" />
                                <asp:Button ID="btnFillInsurance" runat="server" Enabled="true" OnClick="btnFillInsurance_Click"
                                    SkinID="button" Style="visibility: hidden;" Text="" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
