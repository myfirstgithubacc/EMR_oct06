<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="MedicalOncologySchedule.aspx.cs" Inherits="ICM_MedicalOncologySchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="Left" Src="~/Include/Components/ucTree.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Include/JS/jquery1.6.4.min.js"></script>
    <script src="../Include/JS/jquery1.11.3.min.js"></script>
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <script src="../../jsHTML/bootstrap.min.js"></script>
    <%--<script src="../../jsHTML/autosize.js"></script>--%>

    <style type="text/css">
        .Gridheader {
            font-family: Verdana;
            background-image: url(/Images/header.gif);
            height: 24px;
            color: black;
            font-weight: normal;
            position: relative;
        }

        .blink {
            text-decoration: blink;
        }

        .z-ind {
            z-index: 1 !important;
        }

        .blinkNone {
            text-decoration: none;
        }
    </style>

    <script type="text/javascript">

        function BasalEnergyExpenditure() {
            var Height = $('#<%=txtHeight.ClientID%>').val();
            var Weight = $('#<%=txtWeight.ClientID %>').val();
            var gender = $('#<%=hdnGender.ClientID%>').val();
            if (gender == "MALE") {
                if (eval(Height) > 0 && eval(Weight) > 0) {
                    var BSA = (0.007184 * Math.pow(Height, 0.725) * Math.pow(Weight, 0.425)).toFixed(2);
                    $('#<%=txtBSA.ClientID%>').val(BSA);
                }
            }
            else if (gender == "FEMALE") {
                if (eval(Height) > 0 && eval(Weight) > 0) {
                    var BSA = (0.007184 * Math.pow(Height, 0.725) * Math.pow(Weight, 0.425)).toFixed(2);
                    $('#<%=txtBSA.ClientID%>').val(BSA);
                }
            }


    }
    var prm = Sys.WebForms.PageRequestManager.getInstance();


    function FillDayCalculation(sender, e) {
        var checkInDate = '';
        var checkInDate = sender.get_selectedDate().format("MM/dd/yyyy");
        $('#<%=hdnCurrentDate.ClientID%>').val(checkInDate);
        $get('<%=btnCalender.ClientID%>').click();
    }

    $(document).ready(function () {
        getIndexorPrieviousDate();
    });
    function getIndexorPrieviousDate() {
        $("[id*=grvOncologySchedule] [id*=imgFromDate]").click(function () {

            var rowindex = $(this).closest('td').parent()[0].sectionRowIndex;
            var row = $(this).closest("tr");
            var DateValue = row.find("[id*=txtDate]").val();
            var cycleValue = row.find("[id*=ddlCycle]").val();

            $('#<%=hdnPreviousDate.ClientID%>').val(DateValue);
                    $('#<%=hdnRowIndex.ClientID%>').val(rowindex);
                    $('#<%=hdnCycle.ClientID%>').val(cycleValue);
                });
            }
            prm.add_endRequest(function () {
                getIndexorPrieviousDate();
            });

            function isValidateDate(sender, args) {
                var dateString = document.getElementById(sender.controltovalidate).value;

                var regex = /((0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[012])\/((19|20)\d\d))$/;

                if (regex.test(dateString)) {
                    var parts = dateString.split("/");
                    var dt = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
                    args.IsValid = (parseInt(dt.getDate()) == parseInt(parts[0]) && parseInt((dt.getMonth() + 1)) == parseInt(parts[1]) && parseInt(dt.getFullYear()) == parseInt(parts[2]));

                } else {
                    args.IsValid = false;
                }

                if (!args.IsValid) {
                    if (dateString == "__/__/____" || dateString == "") {
                        args.IsValid = true;
                    }
                    else {
                        alert("Invalid date format.");
                        document.getElementById(sender.controltovalidate).value = '__/__/____';
                    }
                }
            }

            function MaxLenTxt(TXT, intMax) {
                if (TXT.value.length > intMax) {
                    TXT.value = TXT.value.substr(0, intMax);
                    alert("Maximum length is " + intMax + " characters only.");
                }
            }



    </script>
    <asp:HiddenField ID="hdnCycle" runat="server" />
    <div class="container-fluid header_main form-group">
        <div class="col-md-4">
            <h2>
                <asp:Label ID="lblHeading" runat="server" Text="Medical OnCology Schedule"></asp:Label>
            </h2>
        </div>
        <div class="col-md-6">
            <div id="pagepopup" title="ifrmpage.Page.Title" style="display: none; background-color: White; width: 100% !important; height: 1000px; margin: 0 !important; padding: 0 !important;">
                <iframe id="ifrmpage" runat="server" width="100%" height="100%" style="border-bottom-style: none; margin: 0 !important; padding: 0 !important; width: 100% !important; float: left !important;" frameborder="0"></iframe>
            </div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" CssClass="mrgn_header z-ind" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-4 pull-right text-right">
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <div>
                                <asp:Panel ID="pnlSearch" runat="server" Width="100%">

                                    <div class="row form-group">
                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtName" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1 text-left PaddingRightSpacing">
                                            <asp:Label ID="Label1" runat="server" Text="Admitting Doctor"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtAdmittingDoctor" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>


                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="Label2" runat="server" Text="Weight(Kg)"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtWeight" runat="server" onchange="BasalEnergyExpenditure();" onblur="BasalEnergyExpenditure();"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtWeight" ValidChars="." />
                                        </div>

                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="lblMRDNo" runat="server" Text="MRD NO."></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtMRDNo" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="lblAgeSex" runat="server" Text="Age/Sex"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtAgeSex" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>


                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="lblHeight" runat="server" Text="Height(cms)"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtHeight" runat="server" onchange="BasalEnergyExpenditure();" onblur="BasalEnergyExpenditure();"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtHeight" ValidChars="." />
                                        </div>

                                    </div>


                                    <div class="row form-group">
                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="lblIPNo" runat="server" Text="IP NO."></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtIPNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                                        </div>

                                        <div class="col-md-1 text-left PaddingRightSpacing">
                                            <asp:Label ID="lblAdmissionDate" runat="server" Text="Admission Date"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtAdmissionDate" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>


                                        <div class="col-md-1 text-left PaddingRightSpacing">
                                            <asp:Label ID="lblBSA" runat="server" Text="BSA"></asp:Label>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            <asp:TextBox ID="txtBSA" runat="server"></asp:TextBox>
                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                FilterType="Custom, Numbers" TargetControlID="txtBSA" ValidChars="." />
                                        </div>
                                    </div>


                                    <div class="form-group row">
                                        <span class="hrBorder" style="margin: 5px!important;"></span>
                                    </div>


                                    <div class="row form-group">
                                        <div class="col-md-6">
                                            <div class="col-md-2 text-left PaddingLeftSpacing">
                                                <asp:Label ID="lblDiagnosis" runat="server" Text="Diagnosis"></asp:Label>
                                            </div>
                                            <div class="col-md-10 text-left PaddingLeftSpacing01">
                                                <asp:TextBox ID="txtDiagnosis" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="col-md-3 text-left PaddingRightSpacing">
                                                <asp:Label ID="lbltxtChemoProtocol" runat="server" Text="Chemotherapy Protocol"></asp:Label>
                                                <asp:Label ID="lblChemo" runat="server" CssClass="red">*</asp:Label>
                                            </div>
                                            <div class="col-md-9 text-left PaddingRightSpacing">
                                                <asp:TextBox ID="txtChemoProtocol" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>



                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-1 text-left">
                                            <asp:Label ID="lblCycle" runat="server" Text="Cycle"></asp:Label>
                                        </div>
                                        <div class="col-md-2 text-left PaddingRightSpacing">
                                            <div class="row">
                                                <div class="col-md-4 PaddingRightSpacing01">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="chkAll" EventName="CheckedChanged" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <telerik:RadComboBox ID="cmbCycle" runat="server" Width="100%" AutoPostBack="true" Filter="Contains" OnSelectedIndexChanged="cmbCycle_SelectedIndexChanged" CausesValidation="false"></telerik:RadComboBox>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>
                                                <div class="col-md-8 PaddingLeftSpacing">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbCycle" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:CheckBox runat="server" ID="chkAll" Text='All' AutoPostBack="true" Checked="true" OnCheckedChanged="chkAll_CheckedChanged" CausesValidation="false" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>


                                        </div>

                                        <div class="col-md-2 text-left">
                                            <asp:Button ID="btnAddrow" runat="server" Text="Add Row" CssClass="btn btn-primary" OnClick="btnAddrow_Click" ValidationGroup="g1" />

                                        </div>
                                        <div class="col-md-2 text-right"></div>
                                        <div class="col-md-5 text-right PaddingLeftSpacing">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="g1" />&nbsp;&nbsp;
                                        <asp:Button ID="btnAppointment" runat="server" Text="Appointment Register" CssClass="btn btn-primary" OnClick="btnAppointment_Click" CausesValidation="false" />
                                            <asp:Button ID="btnPrintPreview" runat="server" Text="View Report" CssClass="btn btn-primary" OnClick="btnPrintData_OnClick" ToolTip="Click to Print Preview" CausesValidation="false" />
                                        </div>


                                    </div>

                                </asp:Panel>
                            </div>
                        </div>


                    </div>

                </div>
            </div>
        </div>
    </div>



    <div class="container-fluid">
        <div class="row form-group">
            <div style="max-height: 325px;">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCalender" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddrow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />


                    </Triggers>
                    <ContentTemplate>
                        <asp:GridView ID="grvOncologySchedule" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="#C1E5EF" BackColor="White" BorderColor="#eeeeee" HeaderStyle-Height="25px"
                            CellPadding="2" CellSpacing="0" Width="100%" Style="text-align: left; padding: 5px" OnRowDataBound="grvOncologySchedule_RowDataBound" OnRowCommand="grvOncologySchedule_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Cycle" HeaderStyle-Width="8%" ItemStyle-Width="8%" ItemStyle-VerticalAlign="middle">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlCycle" runat="server" Width="90%"></asp:DropDownList>

                                        <asp:Label ID="lblCycle" runat="server" Text='<%#Eval("Cycle") %>' Visible="false"></asp:Label>
                                        <asp:RequiredFieldValidator ID="rqvfddlCycle" runat="server" ControlToValidate="ddlCycle"
                                            ErrorMessage="*" InitialValue="Select"></asp:RequiredFieldValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Day" HeaderStyle-Width="4%" ItemStyle-Width="4%" ItemStyle-VerticalAlign="middle">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDay" runat="server" MaxLength="3" Text='<%#Eval("DAY") %>' Enabled="false"></asp:TextBox>

                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" HeaderStyle-Width="10%" ItemStyle-Width="10%" ItemStyle-VerticalAlign="middle">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDate" runat="server" Width="70%" Height="20" Text='<%#Eval("Date") %>'></asp:TextBox>
                                        <asp:Image ImageUrl="~/Images/calendar.gif" alt="Click here to get date" Width="19" Height="20" vspace="0" border="0" ID="imgFromDate" runat="server" SkinID="textbox" />
                                        <AJAX:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate" Format="dd/MM/yyyy" PopupButtonID="imgFromDate" OnClientDateSelectionChanged="FillDayCalculation" PopupPosition="TopRight"></AJAX:CalendarExtender>

                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="_/">
                                        </AJAX:FilteredTextBoxExtender>
                                        <AJAX:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                            CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                            Enabled="True" TargetControlID="txtDate" MessageValidatorTip="false" AcceptAMPM="true"
                                            AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                            ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                        </AJAX:MaskedEditExtender>
                                        <asp:CustomValidator ID="CustomValidator" runat="server" ClientValidationFunction="isValidateDate"
                                            ControlToValidate="txtDate" />

                                        <asp:RequiredFieldValidator ID="rqvfDate" runat="server" ControlToValidate="txtDate" ValidationGroup="g1"
                                            ErrorMessage="*" SetFocusOnError="True" InitialValue="__/__/____"></asp:RequiredFieldValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chemo Schedule" HeaderStyle-Width="65%" ItemStyle-Width="65%" ItemStyle-VerticalAlign="middle">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtChemoSchedule" runat="server" TextMode="MultiLine" Style="min-height: 100px; max-height: 600px; min-width: 800px; max-width: 1000px; background-color: #fff !important;"
                                            MaxLength="5000" onkeyup="return MaxLenTxt(this,5000);" Text='<%#Eval("Chemoshedule") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" Visible="false" ItemStyle-VerticalAlign="middle">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSheduleDetailId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnPreviousDate" runat="server" Value='<%#Eval("Date") %>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cancel" HeaderStyle-Width="4%" ItemStyle-Width="4%" ItemStyle-VerticalAlign="middle">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkCancel" runat="server" Visible="false" />
                                        <asp:Button ID="btnRemoveRow" runat="server" CommandName="RemoveRow" Text="X" CssClass="btn btn-primary" Visible="false" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row form-group" style="display: none">
            <asp:Button ID="btnCalender" runat="server" Text="" Style="visibility: hidden;" OnClick="btnCalender_Click" />
        </div>

        <div class="row form-group" style="display: none">
            <asp:HiddenField ID="hdnGender" runat="server" />
            <asp:HiddenField ID="hdnCurrentDate" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdnPreviousDate" runat="server" />
            <asp:HiddenField ID="hdnRowIndex" runat="server" />
            <asp:HiddenField ID="hdnEncounterId" runat="server" />
            <asp:HiddenField ID="hdnRegistrationId" runat="server" />
            <asp:HiddenField ID="hdnButtonId" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>

        </div>


    </div>


</asp:Content>

