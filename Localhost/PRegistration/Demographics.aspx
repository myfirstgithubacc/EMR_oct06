<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Demographics.aspx.cs" MasterPageFile="~/Include/Master/EMRMaster.master" Inherits="PRegistration_Demographics" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="aspNewControls" Namespace="NewControls" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%--<!DOCTYPE html>
<html lang="en">
<head runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    
    <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)">
    <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)">
    <title>:: New Registration ::</title>
    <link rel="shortcut icon" type="image/ico" href="" />  
    
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/registrationNew.css" rel="stylesheet" runat="server" />
    
    <link ref="~/Include/EMRStyle.css" rel="stylesheet" runat="server" /> 
    <link href="~/Include/Style.css" rel="stylesheet" type="text/css" runat="server" />    
        
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->   

    <script src="../Include/JS/jquery-1.8.2.js" type="text/javascript"></script>
    <script src="../Include/JS/bootstrap.js" type="text/javascript"></script>

    <script type="text/javascript">
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
                myButton.className = "btn-inactive";
                myButton.value = "Processing...";
            }
            return true;
        }
    </script>
    <script type="text/javascript">
              function BindPatientOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.hdnRegistrationNo;
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;

            }
            $get('<%=btnGetSinglePatientInfo.ClientID%>').click();
        }
    

        function OpenPatienDetails() {
            var popup;
            var txtac = document.getElementById("txtAccountNo");
            txtac.value = '';
            popup = window.open("/Pharmacy/Saleissue/PatientDetailsNew.aspx?OPIP=O&RegEnc=0&CloseTo=Reg", "Popup", "height=600,width=800,left=10,top=10, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }

        
        function SearchPatientOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;

            }
            $get('<%=btnGetInfo.ClientID%>').click();
        }

        function validateMobileNo() {
            var txt = document.getElementById('txtSearchMobile').value;
           // alert(txt);
            if(txt!="") {
                var pattern = /^\d{10}$/;
                if (pattern.test(txt)) {
                    // alert("Mobile number should not be less than 10 digits.");
                return true;
            }
            {
                alert("Mobile number must be 10 digits long.");
                document.getElementById('txtSearchMobile').focus();        
                return false;
            }
            }
            else
            {
                alert("Please! Enter 10 digit Mobile No.");
                return false;
            }
        }

       
        
        
        function OpenMotherDetails() {
            var popup;
            var txtact = document.getElementById("txtAccountNo");
            var txtMotRegNo = document.getElementById("txtMothergNo");
            txtact.value = '';
            txtMotRegNo.value = '';
            popup = window.open("/Pharmacy/SaleIssue/PatientDetailsNew.aspx?RegEnc=1&Gender=1&PageFrom=Demographics", "Popup", "height=600,width=600,left=10,top=30, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }
        function OpenCustomField() {
            var popup;
            var regId = document.getElementById("txtRegNo").value;
            if (regId <= 0) {
                alert('Please Select Patient. ! ');
                return false;
            }
            else {
                popup = window.open("/ATD/AdditionalfieldsNew.aspx?RegID=" + regId + "&Source=Demographics", "Popup", "height=500,width=700,left=100,top=30, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
                popup.focus();
                return true;
            }
        }
        function OpenIdentity() {
            var popup;
            popup = window.open("/PRegistration/StatusNew.aspx?CtrlDesp=Iden", "Popup", "height=300,width=600,left=100,top=30, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }
        function OpenRefered() {
            var popup;
            var reftype = document.getElementById("dropReferredType").value;
            if (reftype == 'C') {
                popup = window.open("/PRegistration/StatusNew.aspx?CtrlDesp=Camp&CountId=" + reftype, "Popup", "height=300,width=600,left=100,top=30, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
                popup.focus();
                return false
            }
            if (reftype == "D") {

                popup = window.open("/PRegistration/StatusNew.aspx?CtrlDesp=ExternalDoctor&CountId=" + reftype, "Popup", "height=300,width=600,left=100,top=30, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
                popup.focus();
                return false
            }
        }
        function OpenInsurence() {
            var popup;
            var txtregno = document.getElementById("txtAccountNo").value;
            if (txtregno > 0) {
                var compid = document.getElementById("ddlCompany").value;
                popup = window.open("/PRegistration/InsuranceDetailNew.aspx?OPIP=O&RegistrationNo=" + txtregno + "&PayerId=" + compid, "Popup", "height=600,width=1000,left=100,top=40, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");

                popup.focus();
                return false
            }
            else {
                alert('Please Select The Patient');
                return false;
            }
        }
        function OpenInsurencePopUp() {
            var dvRedirect = document.getElementById("dvRedirect");
            dvRedirect.style.visibility = "none";
            var popup;
            var txtregno = document.getElementById("txtAccountNo").value;
            if (txtregno > 0) {
                var compid = document.getElementById("ddlCompany").value;
                popup = window.open("/PRegistration/InsuranceDetailNew.aspx?OPIP=O&RegistrationNo=" + txtregno + "&PayerId=" + compid + "&openfrom=aftersave", "Popup", "height=600,width=1000,left=100,top=40, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");

                popup.focus();
                return false
            }
            else {
                alert('Please Select The Patient');
                return false;
            }
        }
        function OpenAdmission() {

            var popup;
            var txtac = document.getElementById("txtAccountNo");
            var dvRedirect = document.getElementById("dvRedirect");
            dvRedirect.style.visibility = "none";
            popup = window.open("/ATD/Admission.aspx?NewRegNo=" + txtac.value + "&Source=Demographics&openfrom=Reg", "Popup", "height=600,width=1000,left=100,top=40, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }


        function ValidateDob() {
            if (document.getElementById("<%=dtpDateOfBirth.ClientID%>").value != "__/__/____") {
                document.getElementById('<%=imgCalYear.ClientID%>').click();
            }

            }
            //onkeyup="myFunction(event)"
//            function myFunction(event) {

//                var x = event.which || event.keyCode;
//               
//                if (x == 8 || x==46) {


//                    document.getElementById("<%=dtpDateOfBirth.ClientID%>").value ="__/__/____";
//                }
//                
//                
//            }
       
    </script>


    <script type="text/javascript">
        function validateMaxLength() {
            var txt = $get('<%=txtAccountNo.ClientID%>');
            if (txt.value > 2147483647) {
                alert("Value should not be more than 2147483647.");
                txt.value = txt.value.substring(0, 9);
                txt.focus();
            }
        }       
            
//            if (txt.value.length<10  ) {
//                alert("Mobile number should not be less than 10 digits.");
//                txt.value = txt.value.substring(0, 9);
//                txt.focus();
//                return false;
//            }
      //  }
        
         function OpenPatienDetailsOnMobile() {
            //validateMobileNo();
            var popup;
            var txtac = document.getElementById("txtSearchMobile");
            txtac.value = '';
            popup = window.open("/Pharmacy/Saleissue/PatientDetailsNewMob.aspx?OPIP=O&RegEnc=0&CloseTo=Reg", "Popup", "height=600,width=800,left=10,top=10, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }
        function CheckMobNo()
        {
          var txt = $get('<%=txtAccountNo.ClientID%>');
        }
        
//        function onCheckBoxload() {

//        }
        function onCheckBoxChange() {
           return true;
        }
        function onCheckBoxChangeVIP() {
                return true;
            }
            else {
                txtVIPNarration.value = '';
                txtVIPNarration.disabled = true;
                return true;
            }
        }
        function divClose(d, c) {
            var div = document.getElementById(d);
            var btn = document.getElementById(c);
            div.style.display = "none";
        }
        var popup;
        function OpenCheckList() {
            var regid = $get('<%=hdnRegistrationId.ClientID%>').value;
            var regno = $get('<%=txtAccountNo.ClientID%>').value;
            popup = window.open("/PRegistration/ChecklistNew.aspx?From=R&RegistrationId=" + regid + "&RegistrationNo=" + regno, "Popup", "height=300,width=600,left=100,top=30, status=no, resizable= no, scrollbars= no, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }         
    </script>

    <script language="JavaScript" type="text/javascript">
        function change(id) {
            identity = document.getElementById(id);
            identity.style.border = "2px solid #FF0000";
        }
        function CalCulateDOB(sender, e) {
      
                document.getElementById('<%=imgCalYear.ClientID%>').click();
         
        }
       
        function ShowError(sender, args) {
            alert("Enter a Valid Date");
            sender.focus();
        }
        function CalCulateAge() {
            var txtYear = document.getElementById('<%=txtYear.ClientID %>').value;
            var txtMonth = document.getElementById('<%=txtMonth.ClientID %>').value;
            var txtDays = document.getElementById('<%=txtDays.ClientID %>').value;

            if (txtYear != "") {
                if (txtYear > 120) {
                    alert("Year Cannot Be Greater Than 120");
                    document.getElementById('<%=txtYear.ClientID %>').value = '';
                    document.getElementById('<%=txtYear.ClientID %>').focus()
                    return false;
                }
            }
            if ((txtYear != "") || (txtMonth != "") || (txtDays != "")) {
                //document.getElementById('btnCalAge').click();
                document.getElementById('txtParentof').focus();
            }
        }
        function LocalAddressLostFocus() {
            document.getElementById('<%=dropLCountry.ClientID %>').focus();
        }
        function dropLCountryLostFocus() {
            document.getElementById('<%=dropLState.ClientID %>').focus();
        }
        function dropLStateLostFocus() {
            document.getElementById('<%=dropLCity.ClientID %>').focus();
        }
        function dropLCityLostFocus() {
            document.getElementById('<%=txtLPin.ClientID %>').focus();
        }
        function txtLPinLostFocus() {
            document.getElementById('<%=txtLPhoneRes.ClientID %>').focus();
        }
        function Tab() {
            if (event.keyCode == 13)
                event.keyCode = 9;
        }
        function OnClientSelectedIndexChangedEventHandler(sender, args) {
            var item = args.get_item();
            $get('<%=txtRegNo.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=txt_hdn_PName.ClientID%>').value = item != null ? item.get_text() : sender.value();
            $get('<%=btnGetInfo.ClientID%>').click();
        }
        function BindCombo(oWnd, args) {
            document.getElementById('<%=btnFillCombo.ClientID%>').click();
        }
    </script>

    <script type="text/javascript">

        function AutoChange() {
            var txt = $get('<%=txtLAddress1.ClientID%>');
            if (txt.value.length > 200) {
                alert("Text length should not be greater then 148.");

                txt.value = txt.value.substring(0, 148);
                txt.focus();
            }
        } 
    </script>

    <script type="text/javascript" language="javascript">

        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "red";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "blue";
        }
    </script>

    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>                       
                    
                    
                    
            <div class="wrapper">
                 
                
                <!-- Patient Icon Part Start -->
                <div class="Regpatient-Top">
                    <div class="container-fluid">
                        <div class="row">
                            
                            <div class="col-md-2 col-xs-12 features01">
                                <div class="RegNew-Patient">
                                    <h2>
                                        <asp:Label ID="lblRegistration" Visible="false" runat="server"></asp:Label>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                                    </h2>
                                </div>
                            </div>
                            
                            <div class="col-md-10 col-xs-12 features02">
                                <div class="RegNew-Patient-Left">
                                    <h3>
                                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                                            <asp:Button ID="btnGetSinglePatientInfo" runat="server" Enabled="true" OnClick="btnGetSinglePatientInfo_Click" SkinID="button" Style="visibility: hidden; display:none;" Text="Assign" Width="10px" />
                                            <asp:Button ID="btnGetInfo" runat="server" Enabled="true" OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden; display:none;" Text="Assign" Width="10px" />
                                            <asp:Button ID="btnGetMotherInfo" runat="server" Enabled="true" OnClick="btnGetMotherInfo_Click" SkinID="button" Style="visibility: hidden; display:none;" Text="Assign" Width="10px" />
                                            <asp:Button ID="btnFillInsurance" runat="server" Enabled="true" OnClick="btnFillInsurance_Click" SkinID="button" Style="visibility: hidden; display:none;" Text="Assign" Width="2px" />
                                            <asp:Button ID="btnFindchecklis" runat="server" Enabled="true" OnClick="btnFindchecklis_Click" SkinID="button" Style="visibility: hidden; display:none;" Text="Assign" Width="2px" />
                                            <asp:HiddenField ID="hdnPayer" runat="server" />
                                            <asp:HiddenField ID="hdnSponsor" runat="server" />
                                            <asp:HiddenField ID="hdnchecklistid" runat="server" />
                                            <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>' Font-Underline="false" CssClass="lbtnSearchPatient" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click"></asp:LinkButton>          
                                            <asp:TextBox ID="txtAccountNo" CssClass="txtAccountNo" runat="server" Width="100px" MaxLength="10" onkeyup="return validateMaxLength();" />
                                            <asp:LinkButton ID="lnkUHID" CssClass="btnSearchMob" runat="server" Font-Underline="false" ToolTip="Click to search patient" OnClientClick="return validateMobileNo();" onclick="lnkUHID_Click" ></asp:LinkButton>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtAccountNo" ValidChars="0123456789" />
                                            <asp:TextBox ID="TextBox1" runat="server" Style="visibility: hidden; display:none;"></asp:TextBox>
                                            <asp:TextBox ID="txtRegNo" runat="server" Style="visibility: hidden; display:none;"></asp:TextBox>
                                        </asp:Panel>
                                    </h3>
                                    
                                    <h3>  
                                        <asp:LinkButton ID="lnkchecklist" runat="server" CausesValidation="false" CssClass="RegSaveBtn" OnClientClick="OpenCheckList()" Text="Check List" Visible="false"></asp:LinkButton>
                                        <asp:Label ID="lblMobileSearch" runat="server" CssClass="lblMobileSearch" Text="Mobile No"></asp:Label>
                                        <div class="input-group joinDiv">
                                            <asp:TextBox ID="txtSearchMobile" runat="server" CssClass="form-control searchDiv txtSearchMobile" MaxLength="10"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderMob" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchMobile" ValidChars="0123456789" />
                                            <asp:LinkButton ID="btnSearchMob" runat="server" CssClass="btnSearchMob" Font-Underline="false" OnClick="btnSearchMob_Click" OnClientClick="return validateMobileNo();" ToolTip="Click to search patient"></asp:LinkButton>
                                        </div>
                                    </h3>      

                                    <h5>
                                        <div class="RegNew-Patient-TopRight text-center">
                                            <h4>
                                                <asp:LinkButton ID="linkcustomfields" runat="server" CausesValidation="false" OnClientClick="OpenCustomField()" onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" Text="Custom Fields" Visible="false"></asp:LinkButton>
                                                <asp:Button ID="btnNew" runat="server" CssClass="RegNewBtn" OnClick="btnNew_OnClick" Text="New" ToolTip="Create New Registration" />
                                                <asp:Button ID="btnSave" runat="server" CssClass="RegNewBtn" OnClick="btnSave_OnClick" Text="Save" ToolTip="Save" ValidationGroup="Save" onclientclick="ClientSideClick(this)"   UseSubmitBehavior="False" />
                                                <asp:Button ID="btnprint" runat="server" CausesValidation="false" CssClass="RegNewBtn" OnClick="btnprint_Click" Text="Print" ToolTip="Print" onclientclick="ClientSideClick(this)"   UseSubmitBehavior="False" />
                                                <asp:Button ID="btnprintlabel" runat="server" CausesValidation="false" CssClass="RegNewBtn" onclick="btnprintlabel_Click" Text="Print Card" Visible="true" onclientclick="ClientSideClick(this)"   UseSubmitBehavior="False" />
                                            </h4>
                                        </div>
                                    </h5>
                                    
                                        
                                   </div>
                                </div> 
                                                                             
                        </div>
                    </div>
                </div>
                <!-- Patient Icon Part Ends -->
                    
                    
                <div class="GeneralMessage">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate><asp:Label ID="lblMessage" CssClass="GeneralMessageText" runat="server" /></ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="aa14" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="lblPharmacyId" runat="server" Style="visibility: hidden; display:none;"></asp:TextBox>
                                        <asp:TextBox ID="txtPatientImageId" runat="server" Style="visibility: hidden; display:none;" Text=""></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                
                
                
                <div class="GeneralDiv">
                    <div class="container-fluid">
                        <!-- Left Part EMR Template Start -->
                        <div class="col-md-2 fullDiv">
                            <div class="row RegfullDiv-left">
                                <div class="RegPart-Green">
                                    <h2><img border="0" height="18" src="../Images/Chief-Complaints.png" width="18" /></h2>
                                    <h3>Photo</h3>
                                </div>
                                <div class="RegTemplate">
                                    <h2><asp:Image ID="PatientImage" runat="server" alt="Patient" BorderColor="Gray" ImageUrl="/Images/patientLeft.jpg" Width="100%" />
                                    </h2>
                                    <h3>
                                        <asp:Button ID="ibtnUpload" runat="server" CausesValidation="false" CssClass="RegUploadBtn" OnClick="ibtnUpload_Click" Text="Upload" ToolTip="Upload Image" />
                                        <asp:Button ID="ibtnRemove" runat="server" CausesValidation="false" CssClass="RegUploadBtn" OnClick="RemoveImage_OnClick" Text="Remove" ToolTip="Remove Image" />
                                        <cc1:ModalPopupExtender ID="ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" CancelControlID="ibtnClose" DropShadow="true" PopupControlID="pnlUpload" TargetControlID="ibtnUpload" />
                                    </h3>
                                </div>
                            </div>
                        </div>
                        <!-- Left Part EMR Template Ends -->
                        
                        
                        <div class="col-md-10">
                            <div class="row">
                               
                                <!-- Patient Demographics Start -->
                                <div class="Registration-Right">
                                    <div class="RegGreen">
                                        <h2><i class="fa fa-envelope"></i></h2>
                                        <h3>Patient Demographics</h3>
                                        <h4>
                                            <span>Registration Date :</span>
                                            <asp:TextBox ID="txtRegistrationDate" runat="server" MaxLength="10" ReadOnly="true" TabIndex="42"></asp:TextBox>
                                        </h4>
                                    </div>
                                    
                                    <div class="RegWhiteDiv">
                                        <div class="RegistrationDiv">
                                            
                                            <div class="col-md-4 Reg-personalInfomation">
                                                <div class="Reg-PI"><h3>Personal Information</h3></div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>Title <asp:Label ID="lblTitle" runat="server" ForeColor="Red" Text="*" /></h2>
                                                    <div class="RegText"><aspNewControls:NewDropDownList ID="dropTitle" runat="server" AutoPostBack="true" CssClass="RegText" MarkFirstMatch="true" OnSelectedIndexChanged="Title_SelectedIndexChanged" TabIndex="0" Width="100%"></aspNewControls:NewDropDownList></div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:PRegistration, FIRSTNAME%>"></asp:Literal><span class="red">*</span></h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtFname" runat="server" CssClass="RegText" MaxLength="50" TabIndex="1" Width="100%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqFName" runat="server" ControlToValidate="txtFname" Display="None" ErrorMessage="First Name Cannot Be Blank" SetFocusOnError="true" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom, LowercaseLetters , UppercaseLetters" TargetControlID="txtFname" ValidChars="1234567890 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:PRegistration, MIDDLENAME%>" />
                                                        <asp:Label ID="lblMiddleNameStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtMName" runat="server" CssClass="RegText" MaxLength="50" TabIndex="2" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom, LowercaseLetters , UppercaseLetters" TargetControlID="txtMName" ValidChars="123467890"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="LtrlLastName" runat="server" Text="<%$ Resources:PRegistration, LASTNAME%>" />
                                                        <asp:Label ID="lblLastNameStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtLame" runat="server" CssClass="RegText" MaxLength="50" TabIndex="3" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" FilterType="Custom, LowercaseLetters , UppercaseLetters" TargetControlID="txtLame" ValidChars="123467890"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>

                                                <div class="Reg-Name">
                                                    <h2><asp:Literal ID="ltrlSex" runat="server" Text="<%$ Resources:PRegistration, SEX%>" /><span class="red">*</span></h2>
                                                    <div class="RegText">
                                                        <asp:UpdatePanel ID="UPDGender" runat="server" UpdateMode="Conditional">
                                                            <Triggers><asp:AsyncPostBackTrigger ControlID="dropTitle" /></Triggers>
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="dropSex" runat="server" CssClass="RegText" Font-Size="11px" Skin="Outlook" TabIndex="4" ValidationGroup="Save" Width="100%">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:PRegistration, MALE%>" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ Resources:PRegistration, FEMALE%>" Value="1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="dropSex" Display="None" ErrorMessage="Select Gender" InitialValue="0" ValidationGroup="Save" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal8" runat="server" Text="DOB" />
                                                        <span class="red">*</span>
                                                        <asp:ImageButton ID="imgCalYear" runat="server" ImageUrl="~/Images/insert_table.gif" OnClick="imgCalYear_Click" ToolTip="Calculate Year, Month, Days" ValidationGroup="DOB" Width="1%" />
                                                        <asp:ImageButton ID="btnCalAge" runat="server" ImageUrl="~/Images/insert_table.gif" OnClick="btnCalAge_Click" ToolTip="Calculate Age" ValidationGroup="GetAge" Width="1%" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="dtpDateOfBirth" runat="server" CssClass="RegText" onblur="ValidateDob()" TabIndex="5" Text="11" Width="100%"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="dtpDOB" runat="server" Animated="true" Format="dd/MM/yyyy" OnClientDateSelectionChanged="CalCulateDOB" TargetControlID="dtpDateOfBirth"></cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" InputDirection="RightToLeft" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" TargetControlID="dtpDateOfBirth" />
                                                        <asp:HiddenField ID="hdnCurrDate" runat="server" Value="" />
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2><asp:Literal ID="Literal1" runat="server" Text="Age(Y-M-D)" /> <span class="red">*</span></h2>
                                                    <div class="RegText-YY"><asp:TextBox ID="txtYear" runat="server" AutoPostBack="true" Columns="1" CssClass="text-center" MaxLength="3" OnTextChanged="txtYear_TextChanged" TabIndex="5" Width="100%"></asp:TextBox></div>
                                                    <div class="RegText-YY"><asp:TextBox ID="txtMonth" runat="server" AutoPostBack="true" Columns="1" CssClass="text-center" MaxLength="2" OnTextChanged="txtMonth_TextChanged" TabIndex="5" Width="100%"></asp:TextBox></div>
                                                    <div class="RegText-DD"><asp:TextBox ID="txtDays" runat="server" AutoPostBack="true" Columns="1" CssClass="text-center" MaxLength="2" OnTextChanged="txtDays_TextChanged" TabIndex="5" Width="100%"></asp:TextBox></div>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtYear" Display="None" ErrorMessage="please enter date of birth or age !" InitialValue="" ValidationGroup="Save" />
                                                    <cc1:FilteredTextBoxExtender ID="FTEtxtYear" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtYear"></cc1:FilteredTextBoxExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTEtxtMonth" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtMonth"></cc1:FilteredTextBoxExtender>
                                                    <cc1:FilteredTextBoxExtender ID="FTEtxtDays" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtDays"></cc1:FilteredTextBoxExtender>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Label ID="lblGardianname" runat="server" Text="Guardian Name"></asp:Label>
                                                        <asp:Label ID="lblGuardianNameStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:DropDownList ID="ddlParentof" runat="server" Font-Size="11px" 
                                                            Skin="Outlook" TabIndex="9" Visible="false" Width="100%">
                                                            <asp:ListItem Text="C/O" Value="C/O"></asp:ListItem>
                                                            <asp:ListItem Text="S/O" Value="S/O"></asp:ListItem>
                                                            <asp:ListItem Text="D/O" Value="D/O"></asp:ListItem>
                                                            <asp:ListItem Text="W/O" Value="W/O"></asp:ListItem>
                                                            <asp:ListItem Text="H/O" Value="H/O"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="txtParentof" runat="server" CssClass="RegText" MaxLength="140" TabIndex="10" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Custom" runat="server" FilterMode="InvalidChars" InvalidChars="!@#$%^&amp;*()~?&gt;&lt;|\';:" TargetControlID="txtParentof"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                            
                                            
                                            <div class="col-md-4 Reg-personalInfomation-Center">
                                                <div class="Reg-PI"><h3>Contact Details</h3></div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal12" runat="server" Text="Address" />
                                                        <asp:Label ID="lblAddressStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtLAddress1" runat="server" Height="61px" MaxLength="150" onkeyup="return AutoChange();" TabIndex="11" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                        <asp:TextBox ID="txtLAddress2" runat="server" MaxLength="100" SkinID="textbox" TabIndex="12" Visible="False" Width="100%"></asp:TextBox>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal26" runat="server" Text="<%$ Resources:PRegistration, country%>" />
                                                        <asp:Label ID="lblCountryStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText"><asp:DropDownList ID="dropLCountry" runat="server" AutoPostBack="true" CssClass="RegText" DropDownWidth="200px" Font-Size="11px" OnSelectedIndexChanged="LocalCountry_SelectedIndexChanged" TabIndex="13" Width="100%"></asp:DropDownList></div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal27" runat="server" Text="<%$ Resources:PRegistration, state%>" />
                                                        <asp:Label ID="lblStateStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:DropDownList ID="dropLState" runat="server" AutoPostBack="true" class="RegText" DropDownWidth="200px" Font-Size="11px" OnSelectedIndexChanged="LocalState_SelectedIndexChanged" Skin="Outlook" TabIndex="14" Width="100%">
                                                            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal28" runat="server" Text="<%$ Resources:PRegistration, City%>" />
                                                        <asp:Label ID="lblCityStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:DropDownList ID="dropLCity" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="RegText" DropDownWidth="200px" Font-Size="11px" OnSelectedIndexChanged="LocalCity_OnSelectedIndexChanged" Skin="Outlook" TabIndex="15" Width="100%"><asp:ListItem Text="Select" Value="-1"></asp:ListItem></asp:DropDownList>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal30" runat="server" Text="Area" />
                                                        <asp:Label ID="lblAreaStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:DropDownList ID="ddlCityArea" runat="server" AppendDataBoundItems="true" AutoPostBack="false" CssClass="RegText" DropDownWidth="200px" Font-Size="11px" OnSelectedIndexChanged="ddlCityArea_SelectedIndexChanged" Skin="Outlook" TabIndex="16" Width="100%">
                                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal31" runat="server" Text="<%$ Resources:PRegistration, Pin%>" />
                                                        <asp:Label ID="lblZipStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtLPin" runat="server" CssClass="RegText" MaxLength="10" TabIndex="16" Text="" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="PinFilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtLPin"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                <div class="clear"></div>
                                            </div>
                                            
                                            
                                            <div class="col-md-4 Reg-personalInfomation-right">
                                                <div class="Reg-PI"><h3>Other Details</h3></div>
                                               
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal29" runat="server" Text="<%$ Resources:PRegistration, phonehome%>" />
                                                        <asp:Label ID="lblPhoneHomeStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtLPhoneRes" runat="server" CssClass="RegText" MaxLength="30" TabIndex="17" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtLPhoneRes" ValidChars="()+-"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal43" runat="server" Text="<%$ Resources:PRegistration, Mobile%>" />
                                                        <asp:Label ID="lblMobileNoStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtPMobile" runat="server" CssClass="RegText" MaxLength="10" TabIndex="17" Text="" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtPMobile" ValidChars="+-"></cc1:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal44" runat="server" Text="<%$ Resources:PRegistration, Email%>" />
                                                        <asp:Label ID="lblEmailStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:TextBox ID="txtPEmailID" runat="server" CssClass="RegText" MaxLength="50" TabIndex="18" Text="" Width="100%"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="REV1" runat="server" ControlToValidate="txtPEmailID" CssClass="displayDiv" ErrorMessage="Invalid Email Id" SetFocusOnError="true" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:PRegistration, NATIONALITY%>" />
                                                        <asp:Label ID="lblNationalityStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:DropDownList ID="dropNationality" runat="server" AppendDataBoundItems="true" CssClass="RegText" Font-Size="11px" Skin="Outlook" TabIndex="19" Width="100%">
                                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal19" runat="server" Text="Identity Type" />
                                                        <asp:Label ID="lblIdentityTypeStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText">
                                                        <asp:DropDownList ID="dropIdentityType" runat="server" AppendDataBoundItems="true" CssClass="RegText" Font-Size="11px" Skin="Outlook" TabIndex="20" Width="100%"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal20" runat="server" Text="Identity #" />
                                                        <asp:Label ID="lblIdentityNoStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText"><asp:TextBox ID="txtIdentityNumber" runat="server" CssClass="RegText" MaxLength="15" TabIndex="37" Width="100%"></asp:TextBox></div>
                                                </div>
                                                
                                                <div class="Reg-Name">
                                                    <h2>
                                                        <asp:Literal ID="Literal24" runat="server" Text="Narration" />
                                                        <asp:Label ID="lblVIPNarrationStar" runat="server" ForeColor="Red" Text="*" />
                                                    </h2>
                                                    <div class="RegText"><asp:TextBox ID="txtVIPNarration" runat="server" CssClass="RegText" MaxLength="100" TabIndex="41" Width="100%"></asp:TextBox></div>
                                                </div>
                                                
                                                <div class="clear"></div>
                                            </div>
                                            
                                        </div>
                                    </div>
                                </div>
                                <!-- Patient Demographics Ends -->
                                
                            </div>
                        </div>
                    </div>
                </div>
                
                
                
                <div style="display: none;">
                    <table ID="testNone" cellpadding="0" cellspacing="0" style="display: none;" 
                        width="100%">
                        <tr>
                            <td class="clssubtopic" width="27%">
                                <asp:Label ID="lblDemographics" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td align="right" class="clssubtopicbar" style="display: none;" valign="bottom">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="LblLabelname" runat="server" Font-Bold="true" 
                                            Text="Demographics" Visible="false"></asp:Label>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkPatientRelation" runat="server" CausesValidation="false" 
                                            Font-Bold="true" OnClick="lnkPatientRelation_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="Permanent Address"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkOtherDetails" runat="server" CausesValidation="false" 
                                            Font-Bold="true" OnClick="lnkOtherDetails_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="Other Details" Visible="false"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkResponsibleParty" runat="server" 
                                            CausesValidation="false" Font-Bold="true" OnClick="lnkResponsibleParty_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="Responsible Party" Visible="false"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkPayment" runat="server" CausesValidation="false" 
                                            Font-Bold="true" OnClick="lnkPayment_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="Payer" Visible="false"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkAttachDocument" runat="server" CausesValidation="false" 
                                            Font-Bold="true" OnClick="lnkAttachDocument_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="Attach Doc."></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkbtnAppointment" runat="server" CausesValidation="false" 
                                            Font-Bold="true" OnClick="lnkbtnAppointment_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="Appointment"></asp:LinkButton>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkbtnBilling" runat="server" CausesValidation="false" 
                                            Font-Bold="true" OnClick="lnkbtnBilling_OnClick" 
                                            onmouseout="LinkBtnMouseOut(this.id);" onmouseover="LinkBtnMouseOver(this.id);" 
                                            Text="OP Order Bill"></asp:LinkButton>
                                        <%-- <asp:LinkButton ID="linkcustomfields" runat="server" CausesValidation="false" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" Text="Custom Fields" OnClientClick="OpenCustomField()"></asp:LinkButton>&nbsp;--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <!--Hidden part Start -->
                    <asp:Xml ID="xmlPatientInfo" runat="server" Visible="false"></asp:Xml>
                    <asp:Literal ID="ltrlSSN" runat="server" Text="UID" Visible="false" />
                    <asp:TextBox ID="txtSSN" runat="server" MaxLength="30" SkinID="textbox" 
                        TabIndex="42" Visible="false" Width="138"></asp:TextBox>
                    <asp:Literal ID="ltrlOtherName" runat="server" Text="Previous Name" 
                        Visible="false" />
                    <asp:TextBox ID="txtPreviousName" runat="server" MaxLength="50" 
                        ReadOnly="False" Visible="false" Width="138px"></asp:TextBox>
                    <asp:HiddenField ID="hdnState" runat="server" Value="" />
                    <asp:HiddenField ID="hdnCity" runat="server" Value="" />
                    <asp:Button ID="btnFillCombo" runat="server" CausesValidation="false" 
                        Enabled="true" OnClick="btnFillCombo_Click" SkinID="button" 
                        Style="visibility: hidden;" Text="Assign" Width="10px" />
                    <asp:Button ID="btnGetRegNoInfo" runat="server" OnClick="btnGetRegNoInfo_Click" 
                        Style="visibility: hidden;" />
                    <asp:TextBox ID="txt_hdn_PName" runat="server" SkinID="textbox" 
                        Style="visibility: hidden;" Width="10px"></asp:TextBox>
                    <asp:HiddenField ID="hdnregno" runat="server" Value="" />
                    <asp:HiddenField ID="hdnCountry" runat="server" Value="" />
                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                     <asp:HiddenField ID="hdnOTBookingId" runat="server" Value="" />
                    <!--Hidden part End -->
                    <!-- Hidden part   -->
                    <asp:Button ID="Button1" runat="server" OnClick="btnGetRegNoInfo_Click" 
                        Style="visibility: hidden;" />
                    <asp:TextBox ID="TextBox2" runat="server" SkinID="textbox" 
                        Style="visibility: hidden;" Width="10px"></asp:TextBox>
                    <asp:HiddenField ID="HiddenField3" runat="server" Value="" />
                    <asp:HiddenField ID="HiddenField4" runat="server" Value="" />
                    <asp:TextBox ID="dtpAgeDateOfBirth" runat="server" Text="" Visible="false"></asp:TextBox>
                    <cc1:CalendarExtender ID="cleAgeDateofBirth" runat="server" 
                        TargetControlID="dtpAgeDateOfBirth">
                    </cc1:CalendarExtender>
                    <asp:HiddenField ID="HiddenField5" runat="server" Value="" />
                    <asp:HiddenField ID="HiddenField6" runat="server" Value="" />
                    <asp:Button ID="Button2" runat="server" CausesValidation="false" Enabled="true" 
                        OnClick="btnFillCombo_Click" SkinID="button" Style="visibility: hidden;" 
                        Text="Assign" Width="10px" />
                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" 
                        ShowMessageBox="true" ShowSummary="False" ValidationGroup="Save" />
                    <!-- Hidden part End -->
                </div>
                <asp:Panel ID="pnlUpload" runat="server" CssClass="modalPopup" 
                    Style="display: none">
                    <div>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <p style="color: Black; font-weight: bold;">
                                                    Add an Image</p>
                                            </td>
                                            <td align="left">
                                                <p style="color: Red; font-weight: bold;">
                                                    <asp:ImageButton ID="ibtnClose" runat="server" CausesValidation="false" 
                                                        ImageUrl="/Images/icon-close.jpg" OnClientClick="return false;" />
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="border: solid 1 gray;">
                                    <asp:FileUpload ID="FileUploader1" runat="server" Width="250px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnUpload" runat="server" Height="22px" 
                                        OnClick="Upload_OnClick" Text="Upload" ValidationGroup="Upload" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" valign="top">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" 
                                        EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <asp:TextBox ID="TextBox3" runat="server" Style="visibility: hidden;" Text="0" 
                                        Width="0"></asp:TextBox>
                                    <asp:TextBox ID="TextBox4" runat="server" Style="visibility: hidden;" Text=""></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <%-- </div>--%>
                <!-- Footer Part Start -->
                <%--  <div class="EMR-Footer">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    Copyright © 2010-2015 Akhil Systems Pvt. Ltd.</p>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <!-- Footer Icon Part Ends -->
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel ID="updDelete" runat="server">
        <ContentTemplate>
            <div id="dvRedirect" runat="server" visible="false" class="DemoPopBox">
                <table cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr align="center">
                        <td><asp:Label ID="Label4" runat="server" Text="Would you like to go?" CssClass="DemoPopBoxText"></asp:Label></td>
                    </tr>
                    <tr align="center">
                        <td>
                            <strong>
                                <asp:LinkButton ID="lnkAppointment" runat="server" OnClick="lnkAppointment_Click" CssClass="DemoPopBoxBtn">Appointment</asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_onClick" CssClass="DemoPopBoxBtn">Cancel</asp:LinkButton>
                            </strong>
                            <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        
        
        
        
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="lnkInsuranceDetailsPopup" />--%>
            <asp:AsyncPostBackTrigger ControlID="lnkAppointment" />
            <%--<asp:AsyncPostBackTrigger ControlID="lnkOpBilling" />--%>
            <asp:AsyncPostBackTrigger ControlID="btnCancel" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnFillInsurancePop" />--%>
        </Triggers>
    </asp:UpdatePanel>
    
    </asp:Content>
    <%--</form>
</body>--%>
<%--</html>
--%>