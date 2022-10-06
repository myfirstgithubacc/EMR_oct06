<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientDiagnosisEntry.aspx.cs" Inherits="MRD_PatientDiagnosisEntry"
    Title="Patient Diagnosis Entry" %>

<%@ Register Src="../Include/Components/PatientQView.ascx" TagName="PatientQView" TagPrefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link rel="stylesheet" href="../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../Include/css/mainNew.css" />
    <link rel="stylesheet" href="../Include/EMRStyle.css" />

    <style>
        table#ctl00_ContentPlaceHolder1_patientQV_tblPatientDet {
    width: 100%;
}

        a#ctl00_ContentPlaceHolder1_lbtnSearchPatient:after {
    content: " :";
    white-space: nowrap;
}
    </style>

    <script language="javascript" type="text/javascript">

        function SearchPatientOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                $get('<%=txtAccountNo.ClientID%>').value = RegistrationNo;             

            }
            $get('<%=btnGetInfo.ClientID%>').click();
        }

         function OnClientClose(oWnd, args) {
            $get('<%=btnGetInfoIP.ClientID%>').click();
        }
         function SearchPatientOnClientCloseIP(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                var EncounterNo = arg.EncounterNo;

                 $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                $get('<%=txtAccountNo.ClientID%>').value = RegistrationNo;
              
                $get('<%=txtIPNo.ClientID%>').value = EncounterNo;

            }
            $get('<%=btnGetInfoIP.ClientID%>').click();
         }

         function SearchPatientOnClientCloseMRDNo(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                var EncounterNo = arg.EncounterNo;

                 $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                $get('<%=txtAccountNo.ClientID%>').value = RegistrationNo;
              
                $get('<%=txtIPNo.ClientID%>').value = EncounterNo;

            }
            $get('<%=btnGetInfoMRD.ClientID%>').click();
        }

        function validateMaxLength() {
            var txt = $get('<%=txtAccountNo.ClientID%>');
            var txtIPNo = $get('<%=txtIPNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more than 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
            if (txtIPNo.value > 9223372036854775807) {
                alert("Value should not be more than 9223372036854775807.");
                txtIPNo.value = txtIPNo.value.substring(0, 12);
                txtIPNo.focus();
            }
        }
        function openRadWindow(strPageNameWithQueryString) {
            var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
        }
    </script>

    <asp:UpdatePanel ID="updpnl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close,Move,Maximize,Minimize" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move,Maximize,Minimize" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow4" runat="server" Behaviors="Close,Move,Maximize,Minimize" />
                </Windows>
            </telerik:RadWindowManager>
           <div class="container-fluid" id="tblMain">
               <div class="row header_main">
                   <div class="col-md-2 col-sm-2 col-xs-12 text-nowrap">
                       <asp:Label SkinID="label" runat="server" ID="lblHeader" Text="Patient Diagnosis Entry" />
                   </div>
                   <div class="col-md-5 col-sm5 col-xs-12 text-center">
                       <asp:Label ID="lblMsg" runat="server" SkinID="label" Text="" />
                   </div>
                   <div class="col-md-5 col-sm5 col-xs-12 text-right">
                       <asp:Button ID="btnCaseSheet" runat="server" Text="Case Sheet" CssClass="button"
                                                    SkinID="Button" ToolTip="Back to Case Sheet" OnClick="btnCaseSheet_Click" Visible="False" />
                                           
                                                <asp:Button ID="btnDischargeSummary" runat="server" CssClass="button" SkinID="Button"
                                                    CausesValidation="True" Text="Discharge Summary" ToolTip="Discharge Summary"
                                                    OnClick="btnDischargeSummary_Click" Visible="False" />
                                            
                                                <asp:Button ID="btnOperationNotes" runat="server" CssClass="button" SkinID="Button"
                                                    CausesValidation="True" Text="Operation Notes" ToolTip="Operation Notes" OnClick="btnOperationNotes_Click"
                                                    Visible="false" />
                                           
                                                <asp:Button ID="btnHistory" runat="server" CssClass="button" SkinID="Button" CausesValidation="false"
                                                    Text="Diagnosis(s) History" ToolTip="Diagnosis(s) History" OnClick="btnHistory_Click"
                                                    Visible="False" />
                                            
                                                <asp:Label ID="lblRackNumber" runat="server" Text="Rack Number" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtRackNumber" CssClass="Textbox" runat="server" Visible="false"></asp:TextBox>
                                                <asp:Button ID="btnclose" runat="server" Text="Close File" CssClass="button"
                                                    SkinID="Button"  Visible="false" OnClick="btnclose_Click"/>
                                                <asp:LinkButton ID="lnlMRDTemplate" ToolTip="MRD Template Entry" OnClick="lnlMRDTemplate_Click" runat="server">MRD Template</asp:LinkButton>
                   </div>
               </div>

               <div class="row">
                    <div class="col-md-5 col-sm-5 col-xs-12 m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #dedede;">
                            <div class="row">
                                <div class="col-md-8 col-sm-8 col-xs-12">
                                   <div style="width:100%;float:left;overflow:hidden;">
                                        <uc1:PatientQView ID="patientQV" runat="server" />
                                   </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                                        <div class="col-md-3 col-sm-3 col-xs-4">
                                            
                                                    <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>' 
                                                        Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-xs-8">
                                           <asp:TextBox ID="txtAccountNo" SkinID="textbox" runat="server" Width="100px" ForeColor="Maroon"
                                                        TabIndex="0" MaxLength="13" onkeyup="return validateMaxLength();" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtAccountNo" ValidChars="0123456789" />
                                                    <asp:Button ID="btnGetInfo" runat="server" CausesValidation="false" Enabled="true"
                                                        OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden;" TabIndex="103"
                                                        Text="Assign" Width="10px" />
                                        </div>
                                             </asp:Panel>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <asp:Panel ID="pnlSearchIP" runat="server" DefaultButton="btnGetInfoIP">
                                        <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">
                                           <asp:LinkButton ID="lbtnSearchPatientIP" runat="server" Text="  IP #   : "
                                                        Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatientIP_Click"></asp:LinkButton>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-xs-8">
                                           <asp:TextBox ID="txtIPNo" SkinID="textbox" runat="server" Width="100px" ForeColor="Maroon"
                                                        TabIndex="0" MaxLength="13" onkeyup="return validateMaxLength();" />
                                                   <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtIPNo" ValidChars="0123456789/-" />--%>
                                                     <asp:Button ID="btnGetInfoIP" runat="server" CausesValidation="false" Enabled="true"
                                                        OnClick="btnGetInfoIP_Click" SkinID="button" Style="visibility: hidden;" TabIndex="103"
                                                        Text="Assign" Width="10px" />
                                        </div>
                                            </asp:Panel>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <asp:Panel ID="pnlSearchMRDNo" runat="server" DefaultButton="btnGetInfoMRD">
                                        <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap">
                                            <asp:LinkButton ID="lbtnSearchPatientMRD" runat="server" Text="  MRD #  :"
                                                        Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatientMRD_Click" ></asp:LinkButton>
                                        </div>
                                        <div class="col-md-9 col-sm-9 col-xs-8">
                                          <asp:TextBox ID="TxtMrdNo" SkinID="textbox" runat="server" Width="100px" ForeColor="Maroon"
                                                        TabIndex="0" MaxLength="13" onkeyup="return validateMaxLength();" />
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtMrdNo" ValidChars="0123456789/-" />
                                                     <asp:Button ID="btnGetInfoMRD" runat="server" CausesValidation="false" Enabled="true"
                                                        OnClick="btnGetInfoMRD_Click"  SkinID="button" Style="visibility: hidden;" TabIndex="103"
                                                        Text="Assign" Width="10px" />
                                        </div>
                                             </asp:Panel>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <asp:Label ID="lblEnc" runat="server" Text="Enc#" Visible="false" />
                                            
                                                <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="Maroon" Visible="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="row m-t" style="border-top:1px solid #dedede;border-bottom:1px solid #dedede;">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="lblEDt" runat="server" Text="Enc. Dt." />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                             <asp:Label ID="lblEncDate" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                       <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label5" runat="server" Text="Discharge On" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                           <asp:Label ID="lblDischargeDate" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label2" runat="server" Text="Discharge Status" />
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                           <asp:Label ID="lblDischargeStatus" runat="server" Text="" ForeColor="Maroon" />
                                        </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label1" runat="server" Text="Bed#/Category" />
                                           </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:Label ID="lblBedCategory" runat="server" Text="" ForeColor="Maroon" />
                                            </div>
                                    </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label6" runat="server" Text="Payer" />
                                            </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblPayer" runat="server" ForeColor="Maroon" Text="" />
                                            </div>
                                        </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="Label7" runat="server" Text="Sponsor" />
                                            </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblSponsor" runat="server" ForeColor="Maroon" Text="" />
                                            </div>
                                        </div>

                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                            <asp:Label ID="lblCaptionStatus" runat="server"  Visible="false" Text="File Status" />
                                            </div>
                                        <div class="col-md-8 col-sm-8 col-xs-8">
                                            <asp:Label ID="lblStatus" runat="server" Visible="false" ForeColor="Maroon" Text="" />
                                            </div>
                                        </div>
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                    <asp:LinkButton ID="lnlUpdateDischargeInfo" Visible="false" ToolTip="Update Discharge Information" OnClick="lnlUpdateDischargeInfo_Click" runat="server">Update Discharge Information</asp:LinkButton>
                                            </div>
                                        </div>
                                </div>
                                
                            </div>
                            <div class="row m-t">
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Width="100%" Height="300px"
                                        BorderWidth="0">
                                        <asp:GridView ID="gvVisists" runat="server" SkinID="gridview" AutoGenerateColumns="false"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Enc#" ItemStyle-Width="15%" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkShowDetails" runat="server" Text='<%#Eval("EncounterNo") %>'
                                                            OnClick="lnkShowDetails_OnClick" Width="98%" />
                                                        <asp:HiddenField ID="hdnEncId" runat="server" Value='<%#Eval("ID") %>' />
                                                        <asp:HiddenField ID="hdnRegId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                        <asp:HiddenField ID="hdnPayer" runat="server" Value='<%#Eval("Payer") %>' />
                                                        <asp:HiddenField ID="hdnSponsor" runat="server" Value='<%#Eval("Sponsor") %>' />
                                                        <asp:HiddenField ID="hdnEncounterDate" runat="server" Value='<%#Eval("EncounterDate") %>' />
                                                        <asp:HiddenField ID="hdnDischargeDate" runat="server" Value='<%#Eval("DischargeDate") %>' />
                                                        <asp:HiddenField ID="hdnBedNo" runat="server" Value='<%#Eval("BedNo") %>' />
                                                        <asp:HiddenField ID="hdnBedCategory" runat="server" Value='<%#Eval("BedCategory") %>' />
                                                        <asp:HiddenField ID="hdnDischargeStatus" runat="server" Value='<%#Eval("DischargeStatus") %>' />
                                                        <asp:HiddenField ID="hdnIsExpired" runat="server" Value='<%#Eval("IsExpired") %>' />
                                                        <asp:HiddenField ID="hdnIsNewBorn" runat="server" Value='<%#Eval("IsNewBorn") %>' />
                                                        <asp:HiddenField ID="hdnEncounterStatus" runat="server" Value='<%#Eval("EncounterStatus") %>' />
                                                        <asp:HiddenField ID="hdnMLC" runat="server" Value='<%#Eval("MLC") %>' />
                                                        <asp:HiddenField ID="hdnAcknowledmentStatus" runat="server" Value='<%#Eval("AcknowledmentStatus") %>' />
                                                        <asp:HiddenField ID="hdnRackNumber" runat="server" Value='<%#Eval("RackNumber") %>' />
                                                        <asp:HiddenField ID="hdnStausName" runat="server" Value='<%#Eval("StausName") %>' />
                                                         
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dated On" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncDate" runat="server" Font-Size="9px" Text='<%#Eval("EncDate") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="OP/IP" ItemStyle-Width="5%" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOPIP" runat="server" Font-Size="9px" Text='<%#Eval("OPIP") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doctor" HeaderStyle-Width="60%" ItemStyle-Width="60%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoc" runat="server" Font-Size="9px" Text='<%#Eval("Doctor") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MRDNo" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMRDNo" runat="server" Font-Size="9px" Text='<%#Eval("MRDFileNo") %>'
                                                            Width="98%" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                    </div>
                        </div>
                    </div>

                    <div class="col-md-7 col-sm-7 col-xs-12 m-t">
                        <asp:Panel ID="pnlMain" runat="server" ScrollBars="None" Width="100%" Height="550px"
                            BorderWidth="0">
                            <iframe id="ifrmDiag" runat="server" width="100%" height="100%" frameborder="1">
                            </iframe>
                        </asp:Panel>
                    </div>

                </div>

           </div>
                        
                   
           
            <asp:HiddenField ID="hdnRegistrationId" runat="server" />
            <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
            <asp:HiddenField ID="hdnOPIP" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lbtnSearchPatient" />
            <asp:AsyncPostBackTrigger ControlID="lbtnSearchPatientIP" />
            <asp:AsyncPostBackTrigger ControlID="btnGetInfo" />
             <asp:AsyncPostBackTrigger ControlID="btnGetInfoIP" />
            <asp:AsyncPostBackTrigger ControlID="gvVisists" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
