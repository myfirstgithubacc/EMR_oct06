<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ApprovedWardIndent.aspx.cs" Inherits="Pharmacy_SaleIssue_ApprovedWardIndent" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />


    <script src="../../Include/JS/Validate.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">


        function OpenCIMSWindow() {
            var ReportContent = $get('<%=hdnCIMSOutput.ClientID%>')

            var WindowObject = window.open('', 'PrintWindow2', 'width=1250,height=585,top=72,left=30,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
            WindowObject.document.writeln(ReportContent.value);
            WindowObject.document.close();
            WindowObject.focus();
        }

        function validateMaxLength() {


        }

        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;
        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }

        function executeCode(evt) {

            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);


            evt.returnValue = false;
            return false;
        }

        function openRadWindow(strPageNameWithQueryString) {
            var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
        }

        function openRadWindow(e, Purl, wHeight, wWidth) {
            var unicode = e.keyCode ? e.keyCode : e.charCode


            if (unicode == '13') {
                var oWnd = radopen(Purl);
                oWnd.setSize(wHeight, wWidth);
                oWnd.set_modal(true);
                oWnd.set_visibleStatusbar(false);
                oWnd.Center();
            }
        }

        function SearchPatientWardOnClientClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {

                var IndentId = arg.IndentId;
                var IndentNo = arg.IndentNo;
                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                var EncounterId = arg.EncounterId;
                var EncounterNo = arg.EncounterNo;
                var FacilityId = arg.FacilityId;

                $get('<%=hdnIndentId.ClientID%>').value = IndentId;
                $get('<%=hdnIndentNo.ClientID%>').value = IndentNo;
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                $get('<%=hdnEncounterId.ClientID%>').value = EncounterId;
                $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;


                $get('<%=btnRequestedWardItems.ClientID%>').click(null, null);
            }
        }

    </script>
    <script type="text/javascript">

            function checkAll(objRef) {

                var GridView = objRef.parentNode.parentNode.parentNode;

                var inputList = GridView.getElementsByTagName("input");

                for (var i = 0; i < inputList.length; i++) {

                    //Get the Cell To find out ColumnIndex

                    var row = inputList[i].parentNode.parentNode;

                    if (inputList[i].type == "checkbox" && objRef != inputList[i]) {

                        if (objRef.checked) {

                            //If the header checkbox is checked

                            //check all checkboxes

                            //and highlight all rows

                            row.style.backgroundColor = "aqua";

                            inputList[i].checked = true;
                           

                        }

                        else {

                            //If the header checkbox is checked

                            //uncheck all checkboxes

                            //and change rowcolor back to original

                            if (row.rowIndex % 2 == 0) {

                                //Alternating Row Color

                                row.style.backgroundColor = "#C2D69B";

                            }

                            else {

                                row.style.backgroundColor = "white";

                            }

                            inputList[i].checked = false;
                           
                        }

                    }

                }

            }

        </script> 

    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            </telerik:RadCodeBlock>

            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" />
            <div class="container-fluid header_main">
                <div class="col-md-2">
                    <h2>
                        <span id="tdHeader" runat="server">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Prescription Review" />
                        </span>
                    </h2>
                </div>
                <div class="col-md-5">
                    <asp:Button ID="btnRequestFromWard" runat="server" CausesValidation="false" OnClick="btnRequestFromWard_OnClick"
                        CssClass="btn btn-primary" Text="Request From Ward" />
                    <asp:Button ID="btnApproved" runat="server" CausesValidation="false" OnClick="btnbtnApproved_OnClick"
                        CssClass="btn btn-primary" Text="Approved Lists" />
                    <asp:Button ID="btnRejected" runat="server" CausesValidation="false" OnClick="btnRejected_OnClick"
                        CssClass="btn btn-primary" Text="Rejected Lists" />

                          Store
                     <asp:DropDownList ID="ddlStore" runat="server" SkinID="DropDown" Width="150px"
                                        AutoPostBack="false" />

                </div>
                <div class="col-md-5 text-right">
                    <asp:LinkButton ID="lnkBtnCaseSheet" runat="server" Text="Case Sheet" Font-Bold="true" OnClick="OnClick_lnkBtnCaseSheet" />
                    &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkBtnDiagnosticHistory" runat="server" Text="Diagnostic History" Font-Bold="true" OnClick="OnClick_lnkBtnDiagnosticHistory" />
                    &nbsp;&nbsp;&nbsp;&nbsp;                        
                        <asp:Button ID="BtnApproveSelect" runat="server" CausesValidation="false" OnClick="BtnApproveSelect_Click"
                            CssClass="btn btn-primary" Text="Approve Selected" />
                    <asp:Button ID="BtnRejectSelect" runat="server" CausesValidation="false" OnClick="BtnRejectSelect_Click"
                        CssClass="btn btn-primary" Text="Reject Selected" />
                    <asp:Button ID="btnRequestedWardItems" runat="server" CausesValidation="false" OnClick="btnRequestedWardItems_OnClick"
                        Style="visibility: hidden;" />
                </div>

            </div>
            <div class="container-fluid" style="background-color: #f5deb3!important; border-style: solid; border-width: 1px; border-color: darkgoldenrod;">
                <div id="divPatient" style="display: block;">
                    <div class="row form-group" style="margin-left: 0px; margin-right: 0px;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                Patient Name:
                                    <asp:Label ID="jlblPatientname" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                    Age/Gender:
                                    <asp:Label ID="jlblAGe" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                    <%--  MRD No.:--%>
                                <asp:Label ID="jlblUHID" runat="server" Text="<%$ Resources:PRegistration, UHID%>"></asp:Label>
                                <asp:Label ID="jlblMRD" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Enc #.:
                                <asp:Label ID="jlblEnc" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                 Bed No. :
                                <asp:Label ID="jlblBed" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                 Ward Name :
                                <asp:Label ID="jlblWard" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Mobile :
                                <asp:Label ID="jlblMob" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" 
                                    Width="18px" Height="18px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="row form-group" style="margin-left: 0px; margin-right: 0px;">
                        <div class="row">
                            <div class="col-md-12 col-sm-12">
                                Secondary Doctor :
                                <asp:Label ID="jlblDoc" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Company :
                                <asp:Label ID="jlblComp" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Address :
                                <asp:Label ID="jlblAddress" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Height :
                                <asp:Label ID="jlblHeight" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Weight :
                                <asp:Label ID="jlblWeight" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                BMI :
                                <asp:Label ID="jlblBMI" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                BSA :
                                <asp:Label ID="jlblBSA" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                                &nbsp;&nbsp;&nbsp;
                                Diagnosis :
                                <asp:Label ID="jlblDiagnosis" runat="server" Text="" Font-Bold="true" ForeColor="#990066" />
                            </div>
                        </div>                       
                    </div>
                </div>
            </div>
            <div id="tabPaymodedetails" runat="server" class="container-fluid">
                <div class="row">

                    <table border="0" cellpadding="0" cellspacing="0" style="background: #e0ebfd; border-width: 1px"
                        width="100%">
                        <tr>
                            <td id="tdGrid" runat="server" valign="top" style="width: 100%;">
                                <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                    AlternatingRowStyle-BackColor="#E6E6FA" SkinID="gridview2" ShowFooter="true"
                                    Width="100%" OnRowCommand="gvService_RowCommand" OnRowDataBound="gvService_RowDataBound">
                                    <EmptyDataTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" Text="All" ID="chkAll" OnCheckedChanged="chkAll_CheckedChanged" AutoPostBack="true" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkCheck" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="25px">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Item group (Form)" HeaderStyle-Width="100px"
                                            ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFormulationName" runat="server" Width="30%" Text='<%#Eval("FormulationName")%>'
                                                    ToolTip='<%#Eval("FormulationName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PRegistration, ItemName%>" HeaderStyle-Width="200px"
                                            ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lnkItemName" runat="server" Width="100%" Text='<%#Eval("ItemName")%>'
                                                    ToolTip='<%#Eval("ItemName")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnPrescriptionDetailsId" runat="server" Value='<%#Eval("PrescriptionDetailsId")%>' />
                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId")%>' />
                                                <asp:HiddenField ID="hdnCIMSItemId" runat="server" Value='<%#Eval("CIMSItemId") %>' />
                                                <asp:HiddenField ID="hdnCIMSType" runat="server" Value='<%# Eval("CIMSType") %>' />
                                                <asp:HiddenField ID="hdnVIDALItemId" runat="server" Value='<%#Eval("VIDALItemId") %>' />
                                                <asp:HiddenField ID="hdnApprovalStatusColor" runat="server" Value='<%#Eval("ApprovalStatusColor") %>' />
                                                <asp:HiddenField ID="hdnisapproved" runat="server" Value='<%#Eval("isapproved") %>' />
                                                 <asp:HiddenField ID="hdnRequestedItemId" runat="server" Value='<%#Eval("RequestedItemId") %>' />
                                                 <asp:HiddenField ID="hdnTableString" runat="server" Value='<%#Eval("TableString") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Generic Name" HeaderStyle-Width="200px" ItemStyle-Wrap="true"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGenericName" runat="server" SkinID="label" Style="width: 100%; text-align: right; background-color: Transparent;"
                                                    Text='<%#Eval("GenericName")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Unit %>' HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("UnitName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Dose" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDose" runat="server" Text='<%#Eval("Dose")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Frequency Name" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFrequencyName" runat="server" Text='<%#Eval("FrequencyName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Duration" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDays" runat="server" Text='<%#Eval("Days")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Route" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRouteName" runat="server" Text='<%#Eval("RouteName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="InstructionRemarks" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInstructionRemarks" runat="server" Text='<%#Eval("InstructionRemarks")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mon." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnMonographCIMS" runat="server" ToolTip="Click here to view cims monograph"
                                                    CommandName="MonographCIMS" CausesValidation="false" CommandArgument='<%#Eval("CIMSItemId")%>'
                                                    Text="Mon." Width="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Int." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnInteractionCIMS" runat="server" ToolTip="Click here to view cims drug to drug interaction"
                                                    CommandName="InteractionCIMS" CausesValidation="false" BackColor="#ECBBBB" Text="&nbsp;"
                                                    Width="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DH." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkBtnDHInteractionCIMS" runat="server" ToolTip="Click here to view cims drug health interaction"
                                                    CommandName="DHInteractionCIMS" CausesValidation="false" BackColor="#82AB76"
                                                    Text="&nbsp;" Width="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkApproved" CommandName="Select" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'>Approve</asp:LinkButton>
                                                <asp:LinkButton runat="server" ID="lnkCancel" CommandName="WdCancel" CausesValidation="false" CommandArgument='<%#Eval("ItemId")%>'>Reject</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <RowStyle Wrap="False" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>


                </div>

            </div>
            <div class="row">
                <div class="col-md-2 text-center">
                    <ucl:legend ID="Legend1" runat="server" />
                    <%--<asp:Table ID="tblLegend" runat="server" border="0" CellPadding="2" CellSpacing="0" Width="200px">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="Label1" runat="server" BorderWidth="1px" BackColor="Beige" SkinID="label"
                                            Width="15px" Height="15px" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Pending " />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="Label27" runat="server" BorderWidth="1px" BackColor="SpringGreen" SkinID="label"
                                            Width="15px" Height="15px" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="Label28" runat="server" SkinID="label" Text="Approved " />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="Label19" runat="server" BorderWidth="1px" BackColor="YellowGreen" SkinID="label"
                                            Width="15px" Height="15px" />
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label ID="Label24" runat="server" SkinID="label" Text="Rejected " />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>--%>
                </div>
            </div>
            <div id="divAppropriatenessReview" runat="server" visible="false"
                style="width: 510px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; 
                 border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 400px; 
            left: 350px; top: 130px">
                <table cellspacing="2" cellpadding="2">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label26" Font-Size="12px" runat="server" Font-Bold="true" 
                                Text="Prescription Review Parameters"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblAppropriatenessReviewMessage" Font-Size="12px" runat="server" Font-Bold="true" 
                                ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <table cellspacing="1" cellpadding="1" border="1">
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="Panel8" runat="server" ScrollBars="Auto" Width="500px" Height="300px">
                                            <asp:GridView ID="gvPrescriptionReview" runat="server" AutoGenerateColumns="False"
                                                AlternatingRowStyle-BackColor="#E6E6FA" SkinID="gridview2" Width="498px">
                                                <EmptyDataTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.
                                                    </div>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="1%" ItemStyle-Width="1%">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Prescription Review Parameters" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrescriptionReview" runat="server" Text='<%#Eval("PrescriptionReview")%>' />
                                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                           <asp:TextBox ID="txtRemarks" runat="server" Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server"  />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnSavePrescriptionReview" CssClass="btn btn-primary" runat="server" Text="Proceed" OnClick="btnSavePrescriptionReview_OnClick" />
                                        &nbsp;
                                        <asp:Button ID="btnClosePrescriptionReview" CssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnClosePrescriptionReview_OnClick" CommandArgument="No" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <table width="99%" cellpadding="1" cellspacing="1" style="background: #e0ebfd;">
                <tr>
                    <td align="left">
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Pin, Move, Close, Maximize, Resize" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                            Behaviors="Close,Move,Pin,Resize,Maximize">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                        <asp:HiddenField ID="hdnIndentId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnIndentNo" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                        <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                        <asp:HiddenField ID="hdnEncounterId" runat="server" />
                        <asp:HiddenField ID="hdnEncounterNo" runat="server" />
                        <asp:HiddenField ID="hdnCIMSOutput" runat="server" />
                        <asp:HiddenField ID="hdnXMLItems" runat="server" />
                        <asp:HiddenField ID="hdnItemid" runat="server"  />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

