<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="MRDFileRequest.aspx.cs" Inherits="MRD_MRDFileRequest" Title="Akhil Systems Pvt. Ltd." %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" />
        <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
        <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

        <style type="text/css">
            #ctl00_ContentPlaceHolder1_Label7{
                padding:0!important;
                margin:0!important;
                    
            }
            body#ctl00_Body1{
                overflow-x:hidden;
            }
            #ctl00_ContentPlaceHolder1_Label4, #ctl00_ContentPlaceHolder1_Label6{
                padding:0!important;
                margin:0!important;
            }
            #ctl00_ContentPlaceHolder1_Label3{
                margin:2px 0 0 0!important;
            }
            #ctl00_ContentPlaceHolder1_lblMessage{
                width:100%;
                margin:0!important;
                position:relative;

            }
        </style>


        <script type="text/javascript">
            function validateMaxLength() {
                var txt = $get('<%=txtUHID.ClientID%>');
                var txtIPNo = $get('<%=txtIPNo.ClientID%>');
                if (txt.value > 9223372036854775807) {
                    alert("Value should not be more than 9223372036854775807.");
                    txt.value = txt.value.substring(0, 12);
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


    <%--  <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>--%>
    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-3 col-sm-12 col-12" id="tdReg" runat="server">
                <h2>
                    <asp:Label ID="lblHeader" runat="server" Text="&nbsp;MRD File Request" /></h2>
            </div>
            <div class="col-md-4 col-sm-6 col-6">
                <div class="row">
                    <div class="col-md-3 col-sm-4 col-4">
                        <asp:LinkButton ID="lnkUHID" runat="server" Font-Bold="true" Text='<%$ Resources:PRegistration, Regno%>' OnClick="lnkUHID_OnClick" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-8">
                        <asp:Panel runat="server" DefaultButton="btnClose">
                            <asp:TextBox ID="txtUHID" runat="server" MaxLength="13" onkeyup="return validateMaxLength();" />
                            <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtUHID" ValidChars="0123456789" />
                        </asp:Panel>
                    </div>




                </div>
            </div>

            <div class="col-md-3 col-sm-6 col-6">
                <div class="row">
                    <div class="col-md-3 col-sm-3 col-4">
                        <asp:LinkButton ID="lbtnSearchPatientIP" runat="server" Font-Bold="true" Text="IP#:" OnClick="lbtnSearchPatientIP_Click" />
                    </div>
                    <div class="col-md-9 col-sm-9 col-8">
                        <asp:Panel runat="server" DefaultButton="btnCloseIP">
                            <asp:TextBox ID="txtIPNo" runat="server" MaxLength="10" onkeyup="return validateMaxLength();" />
                            <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtIPNo" ValidChars="0123456789/-" />
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <div class="col-md-2 col-sm-12 col-12 text-right">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary mt-2 mt-md-0" Text="File Request" ToolTip="Save" OnClick="btnSave_OnClick" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12" style="background: #cccccc; border: 1px solid #000000;">
                <div class="row">
                    <div class="col-md-4  col-6" style="border-right: 1px solid #000000;">
                        <div class="row p-t-b-5">
                            <div class="col-4">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-8">
                                <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-6" style="border-right: 1px solid #000000;">
                        <div class="row p-t-b-5">
                            <div class=" col-4">
                                <asp:Label ID="Label5" runat="server" Text="DOB:" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-8">
                                <asp:Label ID="lblDob" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2 col-6" style="border-right: 1px solid #000000;">
                        <div class="row p-t-b-5">
                            <div class="col-4">
                                <asp:Label ID="Label6" runat="server" Text="IP No:" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-8">
                                <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-4 text-nowrap">
                                <asp:Label ID="Label7" runat="server" Text="Admission Date:" Font-Bold="true"></asp:Label>
                            </div>
                            <div class=" col-8">
                                <asp:Label ID="lblAdmissionDate" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row text-center">
            <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
        </div>
        <asp:Panel runat="server">
            <div class="row">
                <div class="col-lg-3 col-md-4 col-6">
                    <div class="row p-t-b-5">
                        <div class=" col-4 text-nowrap">
                            <asp:Label ID="Label2" runat="server" Text="Request For" /><span style='color: Red'>*</span>
                        </div>
                        <div class=" col-8">
                            <telerik:RadComboBox ID="ddlDoctorList" AppendDataBoundItems="true" AutoPostBack="true" Filter="Contains" CssClass="drapDrowHeight" Width="100%" runat="server" OnSelectedIndexChanged="ddlDoctorList_OnSelectedIndexChanged" />
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-4  col-6">
                    <div class="row p-t-b-5">
                        <div class="col-4 text-nowrap">
                            <asp:Label ID="Label4" runat="server" Text="Request By" /><span style='color: Red'>*</span>
                        </div>
                        <div class="col-8">
                            <telerik:RadComboBox ID="ddlRequestBy" AppendDataBoundItems="true" AutoPostBack="true" Enabled="false" Filter="Contains" CssClass="drapDrowHeight" Width="100%" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-4  col-6">
                    <div class="row p-t-b-5">
                        <div class="col-4 text-nowrap">
                            <asp:Label runat="server" Text="Department" /><span style='color: Red'>*</span>
                        </div>
                        <div class="col-8">
                            <telerik:RadComboBox ID="ddlDepartment" runat="server" CssClass="drapDrowHeight" Width="100%" Enabled="false" />
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-4  col-6" id="trRequiredDate" runat="server">
                    <div class="row p-t-b-5">
                        <div class="col-4 text-nowrap">
                            <asp:Label ID="Label1" runat="server" Text="Required Date" />
                        </div>
                        <div class="col-8">
                            <telerik:RadDateTimePicker ID="dtpRequiredDate" runat="server" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                <DateInput DateFormat="dd/MM/yyyy hh:mm tt" />
                            </telerik:RadDateTimePicker>
                        </div>
                    </div>
                </div>
          
                <div class="col-lg-6 col-md-6 col-12">
                    <div class="row p-t-b-5">
                        <div class="col-lg-3 col-md-4 col-3 text-nowrap">
                            <asp:Label ID="Label3" runat="server" Text="Purpose/Remarks" /><span style='color: Red'>*</span>
                        </div>
                        <div class="col-8">
                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Style="height: 45px; width: 100%;" MaxLength="500" onkeyup="return MaxLenTxt(this, 500);" />
                        </div>
                    </div>
                </div>
                <div class="col-lg-9 col-md-8 col-9" id="fileupload" runat="server" visible="false">
                    <div class="row p-t-b-5">
                        <div class="col-4 text-nowrap">
                            <asp:Label ID="lblfileUpload" runat="server" Visible="false" Text="File Upload" /><span style='color: Red'>*</span>
                        </div>
                        <div class=" col-8">
                            <asp:FileUpload ID="_FileUpload" runat="server" Width="230px" Visible="false" CssClass="button" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="_FileUpload" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Select File!"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-4 col-6" id="fileName" runat="server" visible="false">
                    <div class="row p-t-b-5">
                        <div class="col-4 text-nowrap">
                            <asp:Label ID="lblFileNaem" runat="server" Text="File Name" Visible="false" /><span style='color: Red'>*</span>
                        </div>
                        <div class="col-8">
                            <asp:TextBox ID="txtFileName" runat="server" MaxLength="100" Visible="false" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtFileName" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Enter File Name!"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                </div>
            </div>

             <asp:LinkButton ID="ImageButtonDms" runat="server" Text="DMS" ForeColor="Black"
                            data-toggle="tooltip" title="DMS" data-placement="left" OnClientClick="OpenDmsTab()" CssClass="btn btn-primary" ></asp:LinkButton>

        </asp:Panel>
    </div>





        <asp:Panel ID="pnlRecordVisit" runat="server">

            <div class="container-fluid">
            </div>

        <div class="container-fluid">
            <div class="row">
                <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize" />
                    </Windows>
                </telerik:RadWindowManager>
                <asp:Button ID="btnClose" runat="server" SkinID="Button" CausesValidation="false" OnClick="btnClose_OnClick" Style="visibility: hidden;" />
                <asp:Button ID="btnCloseIP" runat="server" SkinID="Button" CausesValidation="false" OnClick="btnCloseIP_Click" Style="visibility: hidden;" />

                    <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                    <asp:HiddenField ID="hdnEncounterId" runat="server" />
                    <asp:HiddenField ID="hdnEncounterNo" runat="server" />
                    <asp:HiddenField ID="hdnGIssueId" runat="server" />
                    <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                        Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                </div>
            </div>

        </asp:Panel>

        <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>

        <asp:UpdatePanel ID="updMain" runat="server">
            <ContentTemplate>

            <div class="container-fluid">
                <div class="row">
                    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Office2007" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow2" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize" />
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </div>

                <asp:GridView ID="gvData" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                    Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false"
                    OnRowCommand="gvData_RowCommand" Visible="false">
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

                        <asp:TemplateField HeaderText='Department' ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Label ID="lblDepartmentName" runat="server" Width="100%" Text='<%#Eval("DepartmentName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='Request By' ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Label ID="lblRequestByName" runat="server" Width="100%" Text='<%#Eval("RequestByName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='Purpose/Remarks' ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Width="100%" Text='<%#Eval("Remarks")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='File Issue Entry Date' ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Label ID="lblDateOffileIssue" runat="server" Width="100%" Text='<%#Eval("TransctionDate")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='File Name' ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Label ID="lblDocumentName" runat="server" Width="100%" Text='<%#Eval("DocumentName")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText='Rack Number' ItemStyle-Width="40px">
                            <ItemTemplate>
                                <asp:Label ID="lblRackNumber" runat="server" Width="100%" Text='<%#Eval("RackNumber")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='Download file' ItemStyle-Width="40px">
                            <ItemTemplate>

                                <asp:LinkButton ID="lblDocumentPath" runat="server" CommandName="Download" CommandArgument='<%# Eval("DocumentPath") %>' Text='Download' ToolTip="click here to dawnload" />

                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="row m-t">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                <asp:UpdatePanel ID="updatepanel3" runat="server" UpdateMode="conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvData1" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Panel ID="panel2" runat="server" Height="400px" Width="100%" ScrollBars="auto"
                            BorderWidth="1px" BorderColor="lightblue">
                            <%--Akshay_202072022_Tirathram--%>
                            <hr />
                            <input type="text" class="form-control" onkeyup="Search_GridviewUnit(this)" placeholder="🔍   Search...."><br />

                            <asp:GridView ID="gvData1" SkinID="gridview" runat="server" AutoGenerateColumns="false"
                                Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false">
                                <Columns>
                                    <asp:TemplateField HeaderText='request date' ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfilerequestdate" runat="server" Text='<%#Eval("filerequestdate")%>' />
                                            <asp:HiddenField ID="hdnrequestid" runat="server" Value='<%#Eval("requestid")%>' />
                                            <asp:HiddenField ID="hdnmrdstatusid" runat="server" Value='<%#Eval("mrdstatusid")%>' />
                                            <asp:HiddenField ID="hdnmrdstatuscode" runat="server" Value='<%#Eval("mrdstatuscode")%>' />
                                            <asp:HiddenField ID="hdnregistrationid" runat="server" Value='<%#Eval("registrationid")%>' />
                                            <asp:HiddenField ID="hdnencounterid" runat="server" Value='<%#Eval("encounterid")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ resources:pregistration, regno%>' ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblregistrationno" runat="server" Width="100%" Text='<%#Eval("registrationno")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ resources:pregistration, encounterno%>' ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblencounterno" runat="server" Width="100%" Text='<%#Eval("encounterno")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ resources:pregistration, patientname%>' ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpatientname" runat="server" Width="100%" Text='<%#Eval("patientname")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='age / gender' ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblpatientagegender" runat="server" Width="100%" Text='<%#Eval("patientagegender")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='department' ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lbldepartmentname" runat="server" Width="100%" Text='<%#Eval("departmentname")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='requested for' ItemStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrequestedfor" runat="server" Width="100%" Text='<%#Eval("requestedfor")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText='requested by' ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblrequestedby" runat="server" Width="100%" Text='<%#Eval("requestedby")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText='encoded by' ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblencodedby" runat="server" Width="100%" Text='<%#Eval("encodedby") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText='required date' ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfilerequireddate" runat="server" Width="100%" Text='<%#Eval("filerequireddate")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='remarks' ItemStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblremarks" runat="server" Width="100%" Text='<%#Eval("remarks")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='rack number' ItemStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblracknumber" runat="server" Width="100%" Text='<%#Eval("racknumber")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="status" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Width="100%" Text='<%#Eval("MRDStatus")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:HiddenField ID="hdngrequestid" runat="server" />
        </div>

        <%--Akshay_20072022_Tirathram--%>
        <script type="text/javascript">
            function Search_GridviewUnit(strKey) {
                debugger;
                var strData = strKey.value.toLowerCase().split(" ");
                var tblData = document.getElementById('<%=gvData1.ClientID%>');
                var rowData;
                for (var i = 1; i < tblData.rows.length; i++) {
                    rowData = tblData.rows[i].innerHTML;
                    var styleDisplay = 'none';
                    for (var j = 0; j < strData.length; j++) {
                        if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                            styleDisplay = '';
                        else {
                            styleDisplay = 'none';
                            break;
                        }
                    }
                    tblData.rows[i].style.display = styleDisplay;
                }
            }

            function OpenDmsTab() {
                debugger;
                var UHID = document.getElementById('<%=txtUHID.ClientID %>').value

              
                 if(UHID !="")
                 {
                     var url = "http://mission.ezeefile.in/patientDataRetrival?valid=cTl6OGk4bXJNd3hqbkNBcTF2T05wMHg4Ujh4VUZrYWVQK0tINFBOS21JOD0ezeeFLie&p=1&mr_no=" + UHID + "";
                     window.open(url)
                 }
                 else
                 {
                     alert("Please Select Patinet");
                 }

                 return false;
                 
             }
        </script>
</asp:Content>
