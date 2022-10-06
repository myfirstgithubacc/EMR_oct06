<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="MRDNonReturnable.aspx.cs" Inherits="MRD_MRDNonReturnable" Title="Akhil Systems Pvt. Ltd." %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

        <script type="text/javascript">
            function validateMaxLength() {
                var txt = $get('<%=txtUHID.ClientID%>');
                var txtIPNo = $get('<%=txtIPNo.ClientID%>');
                if (txt.value > 2147483647) {
                    alert("Value should not be more than 2147483647.");
                    txt.value = txt.value.substring(0, 9);
                    txt.focus();
                }
                if (txtIPNo.value > 2147483647) {
                    alert("Value should not be more than 2147483647.");
                    txtIPNo.value = txtIPNo.value.substring(0, 9);
                    txtIPNo.focus();
                }
            }
            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;

                    $get('<%=txtUHID.ClientID%>').value = RegistrationNo;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnEncounterId.ClientID%>').value = EncounterId;
                    $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;

                }
                $get('<%=btnClose.ClientID%>').click();
            }



            function SearchPatientOnClientCloseIP(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;

                    $get('<%=txtUHID.ClientID%>').value = RegistrationNo;
                    $get('<%=txtIPNo.ClientID%>').value = EncounterNo;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnEncounterId.ClientID%>').value = EncounterId;
                    $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;

                }
                $get('<%=btnCloseIP.ClientID%>').click();
            }

            function ShowError(sender, args) {
                alert("Enter a Valid Date");
                sender.focus();
            }

            function MaxLenTxt(TXT, MAX) {
                if (TXT.value.length > MAX) {
                    alert("Text length should not be greater then " + MAX + " ...");

                    TXT.value = TXT.value.substring(0, MAX);
                    TXT.focus();
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

            function ShowPAgePrint(Url) {

                var x = screen.width / 2 - 1300 / 2;
                var y = screen.height / 2 - 550 / 2;
                var popup;
              
                popup = window.open(Url, "Popup", "height=550,width=1300,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");

                popup.focus();
                //document.getElementById("mainDIV").style.opacity = "0.5";
                //popup.onunload = function () {


                //    document.getElementById("mainDIV").style.opacity = "";
                //}

                return false
            }
        </script>

    </telerik:RadScriptBlock>
    

    <div class="container-fluid">
        <div class="row header_main">
        <div class="col-md-3 col-sm-3 col-xs-12" id="tdReg" runat="server">
            <h2><asp:Label ID="lblHeader" runat="server" Text="&nbsp;MRD Non- Returnable" /></h2>
        </div>
        <div class="col-md-2 col-sm-3 col-xs-12">
            <div class="row">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:LinkButton ID="lnkUHID" runat="server" Font-Bold="true" Text='<%$ Resources:PRegistration, Regno%>' OnClick="lnkUHID_Click" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:Panel runat="server" DefaultButton="btnClose">
                        <asp:TextBox ID="txtUHID" runat="server" MaxLength="10" onkeyup="return validateMaxLength();" />
                        <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtUHID" ValidChars="0123456789" />
                    </asp:Panel>
                </div>
            </div>
        </div>
<div class="col-md-2 col-sm-3 col-xs-12">
            <div class="row">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:LinkButton ID="lbtnSearchPatientIP" runat="server" Font-Bold="true" Text="IP#:" OnClick="lbtnSearchPatientIP_Click" />
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:Panel runat="server" DefaultButton="btnCloseIP">
                        <asp:TextBox ID="txtIPNo" runat="server" MaxLength="10" onkeyup="return validateMaxLength();" />
                        <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtIPNo" ValidChars="0123456789/-" />
                    </asp:Panel>
                </div>
            </div>
        </div>

        <div class="col-md-5 col-sm-3 col-xs-12 text-right">
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="File Save" ToolTip="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnnew" runat="server" CssClass="btn btn-default" Text="New" ToolTip="New" OnClick="btnnew_Click" Visible="false" />
        </div>

        
    </div>
        

        <div class="row" style="background:#f2f2f2;">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-2 col-sm-2 col-xs-3">
                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10 col-sm-10 col-xs-9">
                        <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-2 col-sm-2 col-xs-3">
                        <asp:Label ID="Label5" runat="server" Text="DOB:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-10 col-sm-10 col-xs-9">
                        <asp:Label ID="lblDob" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">
                        <asp:Label ID="Label6" runat="server" Text="IP No:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-9 col-sm-9 col-xs-8">
                        <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label7" runat="server" Text="Admission Date:" Font-Bold="true"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:Label ID="lblAdmissionDate" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
            <asp:Label ID="lblMessage" ForeColor="Green" Width="100%" Font-Bold="true" runat="server" Text="&nbsp;" />
        </div>
        </div>
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="Label2" runat="server" Text="Applicant Name" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <div class="row">
                        <div class="col-md-8 col-sm-8 col-xs-8">
                             <asp:TextBox ID="txtoutsidePersoName" runat="server" Width="100%" Visible="false" />
                        <telerik:RadComboBox ID="ddlDoctorList" AppendDataBoundItems="true" Filter="Contains" CssClass="drapDrowHeight" Width="100%" runat="server" />
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4 box-col-checkbox no-p-l">
                            <asp:CheckBox ID="chkoutside" Text="outside" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" />
                        </div>
                    </div>
                   
                        
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="Label4" runat="server" Text="Enter  By" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <telerik:RadComboBox ID="ddlRequestBy" AppendDataBoundItems="true" Filter="Contains" CssClass="drapDrowHeight" Width="100%" runat="server" Enabled="false" />
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label runat="server" Text="Reason/Status" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:TextBox ID="txtreason" runat="server" TextMode="MultiLine" Style="width: 100%;" MaxLength="500" onkeyup="return MaxLenTxt(this, 500);" />
                </div>
            </div>
        </div>
    </div>
        <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="Label3" runat="server" Text="Purpose/Detail" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Style="height: 45px; width: 100%;" MaxLength="500" onkeyup="return MaxLenTxt(this, 500);" />
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="lblfileUpload" runat="server" Text="File Upload" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:FileUpload ID="_FileUpload" runat="server" Width="230px" CssClass="button" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="_FileUpload" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Select File!"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="lblFileNaem" runat="server" Text="File Name" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:TextBox ID="txtFileName" runat="server" MaxLength="100" Width="100%" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtFileName" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Enter File Name!"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
        <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="Label1" runat="server" Text="Mobile No." /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:TextBox ID="txtmobileno" runat="server" Width="100%" />
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                     <asp:Label ID="Label8" runat="server" Text="Type of Document" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                    <asp:TextBox ID="txtDocument" runat="server" Width="100%" />
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Label ID="Label9" runat="server" Text="Request Time" /><span style='color: Red'>*</span>
                </div>
                <div class="col-md-8 col-sm-8 col-xs-8">
                     <telerik:RadDateTimePicker ID="dtpDeliveryDateTime" runat="server"  DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                </div>
            </div>
        </div>
    </div>
        <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4"></div>
                <div class="col-md-8 col-sm-8 col-xs-8"></div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4"></div>
                <div class="col-md-8 col-sm-8 col-xs-8"></div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12">
            <div class="row p-t-b-5">
                <div class="col-md-4 col-sm-4 col-xs-4"></div>
                <div class="col-md-8 col-sm-8 col-xs-8"></div>
            </div>
        </div>
    </div>
       
        <div class="row">
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <asp:Button ID="btnClose" runat="server" SkinID="Button" CausesValidation="false" OnClick="btnClose_Click" Style="visibility: hidden;" />
            <asp:Button ID="btnCloseIP" runat="server" SkinID="Button" CausesValidation="false" OnClick="btnCloseIP_Click" Style="visibility: hidden;" />

            <asp:HiddenField ID="hdnRegistrationId" runat="server" />
            <asp:HiddenField ID="hdnEncounterId" runat="server" />
            <asp:HiddenField ID="hdnEncounterNo" runat="server" />
            <asp:HiddenField ID="hdnGIssueId" runat="server" />
            <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
            <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                Style="visibility: hidden;" Width="1px" />
        </div>
    </div>
     <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
   
            <div class="container-fluid">
                <div class="row">
                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close,Move" />
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </div>

            <asp:GridView ID="gvData" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false"
                OnRowDataBound="gvData_RowDataBound" OnRowCommand="gvData_RowCommand">
                <Columns>

                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblRegistrationNo" runat="server" Width="100%" Text='<%#Eval("RegistrationNo")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblEncounterNo" runat="server" Width="100%" Text='<%#Eval("EncounterNo")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText='Request For' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblRequestForName" runat="server" Width="100%" Text='<%#Eval("RequestForName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText='Applicant Name' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblRequestByName" runat="server" Width="100%" Text='<%#Eval("RequestByName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText='Reason/Status' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblReason" runat="server" Width="100%" Text='<%#Eval("Reason")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText='Purpose/Detail' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblNonReturnableDetails" runat="server" Width="100%" Text='<%#Eval("NonReturnableDetails")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText='Non-Returnable Entry Date' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblDateOfNonReturnable" runat="server" Width="100%" Text='<%#Eval("DateOfNonReturnable")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText='File Name' ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lblDocumentName" runat="server" Width="100%" Text='<%#Eval("DocumentName")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText='Download file' ItemStyle-Width="40px">
                        <ItemTemplate>

                            <asp:LinkButton ID="lblDocumentPath" runat="server" CommandName="Download" CommandArgument='<%# Eval("DocumentPath") %>' Text='Download' ToolTip="click here to dawnload" />

                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText='Delete' ItemStyle-Width="40px">
                        <ItemTemplate>

                            <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"  CommandArgument='<%#Eval("MRDNonReturnableid")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png"  onclientclick="return confirm('Are you sure to Delete this Record?');" />
                        </ItemTemplate>
                    </asp:TemplateField>



                </Columns>
            </asp:GridView>


            </ContentTemplate>
         </asp:UpdatePanel>
    
</asp:Content>

