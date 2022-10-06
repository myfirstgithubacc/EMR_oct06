<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Allergy.aspx.cs" Inherits="EMR_Allergy_Allergy"
    MasterPageFile="~/Include/Master/EMRMaster.master" Title="Allergy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" />
    <div>

        <%-- <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js">
        </script>--%>
        <script src="../../Include/JS/jquery-1.8.2.js" type="text/javascript"></script>
        <style type="text/css">
            span#ctl00_ContentPlaceHolder1_lblMessage {
                width: 100% !important;
                margin: 0px !important;
            }

            .gridview table tr td {
                padding: 0px 0px !important;
            }

            span#ctl00_ContentPlaceHolder1_tbcAllergy_tbpnlDrugAllergy_Label5 {
                vertical-align: sub !important;
                margin-left: 10px;
            }

            table#ctl00_ContentPlaceHolder1_tbcAllergy_tbpnlDrugAllergy_gvDrugAllergy {
                text-align: center;
            }

            #ctl00_ContentPlaceHolder1_ddlBrand {
                margin: 0px !important;
            }

            .box-col-checkbox input {
                width: auto!important;
                margin-right: 4px!important;
               
            }
            
        </style>
        <script type="text/javascript">

            <%--  //By using jQuery
//        $(window).bind('onbeforeunload', function() {
//        return 'please save your setting before leaving the page.';
//    });


//    var reconnect = false;
//    window.onfocus = function() {
//        if (reconnect) {
//            reconnect = false;
//            alert("Perform an auto-login here!");
//        }
//    };

//    window.onbeforeunload = function() {
//        var msg = "Are you sure you want to leave?";
//        reconnect = true;
//        return msg;
    //    };

//    var myEvent = window.attachEvent || window.addEventListener;
//    var chkevent = window.attachEvent ? 'onbeforeunload' : 'beforeunload'; /// make IE7, IE8 compitable

//    myEvent(chkevent, function(e) { // For >=IE7, Chrome, Firefox
//        var confirmationMessage = 'Are you sure to leave the page?';  // a space
//        (e || window.event).returnValue = confirmationMessage;
//        return confirmationMessage;
    //    });

//    $(window).bind('onbeforeunload', function() {
//        callSomeFunction();
//        return;
//    });


//    function callSomeFunction() {
//        alert("please save your setting before leaving the page.");
//    }--%>
    
        </script>

        <script type="text/javascript">
            function OnClientItemsRequesting(sender, eventArgs) {
                var ddlgeneric = $find('<%= ddlGeneric.ClientID %>');
                var value = ddlgeneric.get_value();
                var context = eventArgs.get_context();
                context["GenericId"] = value;
            }

            <%--window.onbeforeunload = function (evt) {
                var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
                if (IsUnsave == 0) {
                    return false;
                }
            }--%>

            function ddlGeneric_OnClientSelectedIndexChanged(sender, args) {
                var ddlbrand = $find("<%=ddlBrand.ClientID%>");
                ddlbrand.clearItems();
                ddlbrand.set_text("");
                ddlbrand.get_inputDomElement().focus();

                var item = args.get_item();
                $get('<%=hdnGenericId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=hdnGenericName.ClientID%>').value = item != null ? item.get_text() : sender.text();

                $get('<%=btnGetInfoGeneric.ClientID%>').click();
            }

            function ddlGenericOnClientDropDownClosedHandler(sender, args) {
                if (sender.get_text().trim() == "") {
                    $get('<%=hdnGenericId.ClientID%>').value = "";
                    $get('<%=hdnGenericName.ClientID%>').value = "";

                    $get('<%=btnGetInfoGeneric.ClientID%>').click();
                }
            }

            function ddlBrand_OnClientSelectedIndexChanged(sender, args) {
                var item = args.get_item();
                $get('<%=hdnItemId.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=hdnItemName.ClientID%>').value = item != null ? item.get_text() : sender.text();
                $get('<%=btnGetInfo.ClientID%>').click();
            }

            function ddlBrandOnClientDropDownClosedHandler(sender, args) {
                if (sender.get_text().trim() == "") {
                    $get('<%=hdnItemId.ClientID%>').value = "";
                    $get('<%=hdnItemName.ClientID%>').value = "";

                    $get('<%=btnGetInfo.ClientID%>').click();
                }
            }

            function MaxLenTxt(TXT, LEN) {
                if (TXT.value.length > LEN) {
                    alert("Text length should not be greater then " + LEN + " ...");

                    TXT.value = TXT.value.substring(0, LEN);
                    TXT.focus();
                }
            }

            //            function CancelValidation(CtrlCancelRemarks) {
            //                if ($get(CtrlCancelRemarks).value == "") {
            //                    alert("De-Activation Remarks Cannot Be Blank");
            //                    $get(CtrlCancelRemarks).focus();
            //                    return false;
            //                }
            //            }
            function ShowError(sender, args) {
                alert("Enter a Valid Date");
                sender.focus();
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
                    evt = window.event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {
                    case 113:  // F2
                        $get('<%=btnNew.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }
        </script>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-6 hidden">
                            <h2>
                                <asp:Label ID="lblHeader" runat="server" Text="&nbsp;Allergies" />
                                <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                            </h2>
                        </div>

                    </div>
                </div>

                <%-- <div class="container-fluid text-center bg-warning form-group">
                    <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                </div>--%>
                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-md-3 col-sm-4 col-12">
                            <asp:CheckBox ID="chkNKA" runat="server" Text="Not Known Allergies" ForeColor="Red"
                                Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkNKA_OnCheckedChanged" />
                        </div>



                        <div class="col-md-9 col-sm-8 col-12 text-right">

                            <asp:LinkButton ID="lnkDemographics" runat="server" Text="Demographics" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" OnClick="lnkDemographics_Click"
                                Visible="false" />
                            &nbsp;
                            <asp:Label ID="lblAllergies" runat="server" Text="Allergies" Font-Bold="true" Visible="false" />
                            &nbsp;
                            <asp:LinkButton ID="lnkPatientRelation" runat="server" Text="Contacts" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" OnClick="lnkPatientRelation_OnClick"
                                Visible="false" />
                            &nbsp;
                            <asp:LinkButton ID="lnkOtherDetails" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Text="Other&nbsp;Details" Font-Bold="true"
                                OnClick="lnkOtherDetails_OnClick" Visible="false" />
                            &nbsp;
                            <asp:LinkButton ID="lnkResponsibleParty" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Text="Kin&nbsp;Details" Font-Bold="true"
                                OnClick="lnkResponsibleParty_OnClick" Visible="false" />
                            &nbsp;
                            <asp:LinkButton ID="lnkPayment" runat="server" Text="Payer" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Font-Bold="true" OnClick="lnkPayment_OnClick"
                                Visible="false" />
                            &nbsp;
                            <asp:LinkButton ID="lnkAttachDocument" runat="server" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" Text="Attach&nbsp;Document" Font-Bold="true"
                                OnClick="lnkAttachDocument_OnClick" Visible="false" />
                            &nbsp;
                            <asp:LinkButton ID="lnkAlerts" runat="server" Text="Patient Alert" OnClick="lnkAlerts_OnClick"
                                Visible="false" />
                            <asp:Button ID="btnupdate" runat="server" CssClass="btn btn-primary" Text="Update Not Known Allergies"
                                OnClick="btnUpdate_Click" />
                            <asp:LinkButton ID="lnkAllergyMaster" runat="server" Text="Allergy&nbsp;Master"
                                onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                                Font-Bold="false" CssClass="btn btn-primary" OnClick="lnkAllergyMaster_OnClick" />

                            <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record (F2)" CssClass="btn btn-primary"
                                Text="New " OnClick="btnNew_OnClick" />
                            <asp:Button ID="btnSave" Visible="false" runat="server" Text="Save" CssClass="btn btn-primary"
                                OnClick="btnSave_Click" CausesValidation="false" />
                            <asp:Button ID="btnClose" CssClass="btn btn-primary" Visible="false" runat="server" Text="Close"
                                OnClientClick="window.close();" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 ">
                            <div class="row text-center" style="margin-bottom: 13px;">
                                <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4 col-sm-12 col-xs-12 m-t">
                            <div class="col-md-12 col-sm-12 col-xs-12 bg-gray" style="border: 1px solid #ccc;">
                                <div class="row p-t-b-5">
                                    <div class="col-md-12 col-sm-12 col-xs-12 box-col-checkbox">
                                        <asp:RadioButtonList ID="rdoAllergyType" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rdoAllergyType_SelectedIndexChanged">
                                            <asp:ListItem Text="Drug" Value="1" Selected="True" />
                                            <%--<asp:ListItem Text="CIMS" Value="2" Enabled="false"  />--%>
                                            <%--<asp:ListItem Text="VIDAL" Value="3" Enabled="false" />--%>
                                            <asp:ListItem Text="Other&nbsp;" Value="0" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <div class="row p-t-b-5">
                                    <div class="col-md-12 col-sm-12 col-xs-12 box-col-checkbox" id="tdFormularyType" runat="server">
                                        <asp:RadioButtonList ID="rdoFormularyType" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="rdoFormularyType_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Text="Hospital Formulary" Value="0" />
                                            <asp:ListItem Text="NHIS Drugs List&nbsp;" Value="1" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 col-sm-6">
                                        <div class="row p-t-b-5" id="trGeneric" runat="server">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:Label ID="lblGeneric" runat="server" SkinID="label" Text="Generic" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlGeneric" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlGeneric" runat="server" Width="100%" DropDownWidth="300px" Height="300px"
                                                            EmptyMessage="" AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true"
                                                            OnItemsRequested="ddlGeneric_OnItemsRequested" DataTextField="GenericName" DataValueField="GenericId"
                                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged"
                                                            OnClientDropDownClosed="ddlGenericOnClientDropDownClosedHandler" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6">
                                        <div class="row p-t-b-5" id="trDrugs" runat="server">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:Label ID="lblBrand" runat="server" SkinID="label" Text="Drug" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlBrand" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlBrand" runat="server" HighlightTemplatedItems="true"
                                                            Width="100%" DropDownWidth="300px" EmptyMessage="[ Search Drugs By Typing Minimum 2 Characters ]"
                                                            AllowCustomText="true" MarkFirstMatch="true" EnableLoadOnDemand="true" OnItemsRequested="ddlBrand_OnItemsRequested"
                                                            OnClientItemsRequesting="OnClientItemsRequesting" ShowMoreResultsBox="true"
                                                            OnClientSelectedIndexChanged="ddlBrand_OnClientSelectedIndexChanged" OnClientDropDownClosed="ddlBrandOnClientDropDownClosedHandler"
                                                            Height="300px" EnableVirtualScrolling="true">
                                                            <HeaderTemplate>
                                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 80%" align="left">
                                                                            <asp:Literal ID="Literal2" runat="server" Text="Item Name" />
                                                                        </td>
                                                                        <%--<td style="width:
                                                                10%" align="center"> Stock </td>--%>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="width: 80%" align="left">
                                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                                        </td>
                                                                        <%--<td style="width:
                                                                    10%" align="left"> <%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>
                                                                    </td>--%>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </telerik:RadComboBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="Onset&nbsp;Date" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6 col-6">
                                                        <telerik:RadDatePicker ID="dtpAllergyDate" runat="server" MinDate="01/01/1900" Width="100%">
                                                            <DateInput ID="DateInput1" runat="server">
                                                                <ClientEvents OnError="ShowError" />
                                                            </DateInput>
                                                        </telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 col-6 box-col-checkbox">
                                                        <asp:CheckBox ID="chkIntolerance" runat="server" Text="Intolerance" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:Label ID="Label2" runat="server" SkinID="label" Text="Reaction/Any Adverse Drug Event" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <asp:TextBox ID="txtReaction" runat="server" Width="100%" SkinID="textbox" MaxLength="500" TextMode="MultiLine"
                                                    onkeyup="return MaxLenTxt(this, 500);" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="Remarks" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <asp:TextBox ID="txtRemarks" runat="server" SkinID="textbox" MaxLength="500" TextMode="MultiLine" Width="100%" onkeyup="return MaxLenTxt(this, 500);" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-6">
                                        <div class="row p-t-b-5" id="trInteractionPlausibility" runat="server" visible="true">
                                            <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                                <asp:Label ID="Label3" runat="server" SkinID="label" Text="Interaction Severity" />
                                                <span style="color: #FF0000">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <telerik:RadComboBox ID="ddlAllergyPlausibility" runat="server" MarkFirstMatch="true"
                                                    EmptyMessage="[ Select ]" Width="100%">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="" Value="0" Selected="true" />
                                                        <telerik:RadComboBoxItem Text="Major" Value="1" />
                                                        <telerik:RadComboBoxItem Text="Moderate" Value="2" />
                                                        <telerik:RadComboBoxItem Text="Minor" Value="3" />
                                                        <telerik:RadComboBoxItem Text="No Allert" Value="4" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-4">
                                                <asp:Label ID="lblshow" runat="server" Visible="false" Text="Show in Note" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-8">
                                                <div class="row">
                                                    <div class="col-md-7 col-sm-7 col-7">
                                                        <asp:RadioButtonList ID="rblShowNote" Visible="false" runat="server" RepeatColumns="2">
                                                            <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                                            <asp:ListItem Text="No" Value="0" />
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div class="col-md-5 col-sm-5 col-5 text-right">
                                                        <asp:Button ID="btnAddtogrid" runat="server" CssClass="btn btn-primary" Font-Bold="true"
                                                            Text="Add to List" OnClick="btnAddtogrid_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-8 col-sm-12 col-xs-12 m-t gridview">

                    <AJAX:TabContainer ID="tbcAllergy" runat="server" ActiveTabIndex="0" Width="100%">
                        <AJAX:TabPanel ID="tbpnlDrugAllergy" runat="server" HeaderText="Drug Allergies(0)">
                            <ContentTemplate>





                                <asp:Panel ID="pnlDrugAllergy" runat="server" Height="330px" CssClass="panel_x_y"
                                    Width="100%">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvDrugAllergy" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:GridView ID="gvDrugAllergy" runat="server" AutoGenerateColumns="False" SkinID="gridview"
                                                OnSelectedIndexChanged="gvDrugAllergy_OnSelectedIndexChanged" OnRowDataBound="gvDrugAllergy_OnRowDataBound"
                                                Width="100%" OnRowCommand="gvDrugAllergy_RowCommand" ShowHeaderWhenEmpty="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Onset Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldaAllergyDate" runat="server" SkinID="label" Text='<%#Eval("AllergyDate")%>' />
                                                            <asp:HiddenField ID="hdndaAllergyDate" runat="server" Value='<%#Eval("WitoutFormatEncodedDate")%>' />
                                                            <asp:HiddenField ID="hdndaId" runat="server" Value='<%#Eval("Id")%>' />
                                                            <asp:HiddenField ID="hdndaAllergyID" runat="server" Value='<%#Eval("AllergyID")%>' />
                                                            <asp:HiddenField ID="hdndaGENERIC_ID" runat="server" Value='<%#Eval("GENERIC_ID")%>' />
                                                            <asp:HiddenField ID="hdndsGeneric_Name" runat="server" Value='<%#Eval("Generic_Name")%>' />
                                                            <asp:HiddenField ID="hdndaTYPE" runat="server" Value='<%#Eval("AllergyType") %>' />
                                                            <asp:HiddenField ID="hdndaRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                            <asp:HiddenField ID="hdnAllergySeverity" runat="server" Value='<%#Eval("DrugSeverity")%>' />
                                                            <asp:HiddenField ID="hdndaIntolerance" runat="server" Value='<%#Eval("Intolerance")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Allergy">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldaAllergyName" runat="server" SkinID="label" Text='<%#Eval("AllergyName")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reaction/Any Adverse Drug Event" ItemStyle-Width="120px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldaReaction" runat="server" SkinID="label" Text='<%#Eval("Reaction")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Interaction Severity" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAllergySeverity" runat="server" SkinID="label" Text='<%#Eval("AllergySeverity")%>'
                                                                Width="50px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True"
                                                        HeaderText="" ItemStyle-Width="30px">
                                                        <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                    </asp:CommandField>
                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                                CommandArgument='<%#Eval("Id")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                        <EmptyDataTemplate><span class="col-md-12 m-sm-0 m-md-3  text-center" style="background-color: #f8f9fa!important;">Record not found</span></EmptyDataTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>


                                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="De-Activation&nbsp;Remarks" Style="vertical-align: top;" />

                                        <asp:TextBox ID="txtdaCancellationRemarks" runat="server" CssClass="form-group" SkinID="textbox" MaxLength="500"
                                            TextMode="MultiLine" Style="min-height: 35px; max-height: 35px; min-width: 350px; max-width: 350px; margin-bottom: 10px;"
                                            Width="350px" Height="35px" onkeyup="return MaxLenTxt(this, 500);" />

                            </ContentTemplate>
                        </AJAX:TabPanel>
                        <AJAX:TabPanel ID="tbpnlCIMSAllergy" runat="server" HeaderText="CIMS Allergies(0)" Visible="false">
                            <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlCIMSAllergy" runat="server" Height="380px" CssClass="panel_x_y"
                                                Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvCIMSAllergy" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvCIMSAllergy" runat="server" AutoGenerateColumns="False" SkinID="gridview"
                                                            OnSelectedIndexChanged="gvCIMSAllergy_OnSelectedIndexChanged" Width="100%" OnRowCommand="gvCIMSAllergy_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Onset Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaAllergyDate" runat="server" SkinID="label" Text='<%#Eval("AllergyDate")%>' />
                                                                        <asp:HiddenField ID="hdndaAllergyDate" runat="server" Value='<%#Eval("WitoutFormatEncodedDate")%>' />
                                                                        <asp:HiddenField ID="hdndaId" runat="server" Value='<%#Eval("Id")%>' />
                                                                        <asp:HiddenField ID="hdndaAllergyID" runat="server" Value='<%#Eval("AllergyID")%>' />
                                                                        <asp:HiddenField ID="hdndaGENERIC_ID" runat="server" Value='<%#Eval("GENERIC_ID")%>' />
                                                                        <asp:HiddenField ID="hdndaTYPE" runat="server" Value='<%#Eval("AllergyType") %>' />
                                                                        <asp:HiddenField ID="hdndaRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                                        <asp:HiddenField ID="hdnAllergySeverity" runat="server" Value='<%#Eval("DrugSeverity")%>' />
                                                                        <asp:HiddenField ID="hdndaIntolerance" runat="server" Value='<%#Eval("Intolerance")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allergy">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaAllergyName" runat="server" SkinID="label" Text='<%#Eval("AllergyName")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reaction/Any Adverse Drug Event" ItemStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaReaction" runat="server" SkinID="label" Text='<%#Eval("Reaction")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True"
                                                                    HeaderText="" ItemStyle-Width="30px">
                                                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                                            CommandArgument='<%#Eval("Id")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 140px">
                                            <asp:Label ID="Label8" runat="server" SkinID="label" Text="De-Activation&nbsp;Remarks" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCIMSCancellationRemarks" runat="server" SkinID="textbox" MaxLength="500"
                                                TextMode="MultiLine" Style="min-height: 40px; max-height: 40px; min-width: 350px; max-width: 350px;"
                                                Width="350px" Height="40px" onkeyup="return MaxLenTxt(this, 500);" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </AJAX:TabPanel>
                        <AJAX:TabPanel ID="tbpnlVIDALAllergy" runat="server" HeaderText="VIDAL Allergies(0)" Visible="false">
                            <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlVIDALAllergy" runat="server" Height="380px" CssClass="panel_x_y"
                                                Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvVIDALAllergy" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvVIDALAllergy" runat="server" AutoGenerateColumns="False" SkinID="gridview"
                                                            OnSelectedIndexChanged="gvVIDALAllergy_OnSelectedIndexChanged" Width="100%" OnRowCommand="gvVIDALAllergy_RowCommand">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Onset Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaAllergyDate" runat="server" SkinID="label" Text='<%#Eval("AllergyDate")%>' />
                                                                        <asp:HiddenField ID="hdndaAllergyDate" runat="server" Value='<%#Eval("WitoutFormatEncodedDate")%>' />
                                                                        <asp:HiddenField ID="hdndaId" runat="server" Value='<%#Eval("Id")%>' />
                                                                        <asp:HiddenField ID="hdndaAllergyID" runat="server" Value='<%#Eval("AllergyID")%>' />
                                                                        <asp:HiddenField ID="hdndaGENERIC_ID" runat="server" Value='<%#Eval("GENERIC_ID")%>' />
                                                                        <asp:HiddenField ID="hdndaTYPE" runat="server" Value='<%#Eval("AllergyType") %>' />
                                                                        <asp:HiddenField ID="hdndaRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                                        <asp:HiddenField ID="hdnAllergySeverity" runat="server" Value='<%#Eval("DrugSeverity")%>' />
                                                                        <asp:HiddenField ID="hdndaIntolerance" runat="server" Value='<%#Eval("Intolerance")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allergy">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaAllergyName" runat="server" SkinID="label" Text='<%#Eval("AllergyName")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reaction/Any Adverse Drug Event" ItemStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaReaction" runat="server" SkinID="label" Text='<%#Eval("Reaction")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True"
                                                                    HeaderText="" ItemStyle-Width="30px">
                                                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                                            CommandArgument='<%#Eval("Id")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 140px">
                                            <asp:Label ID="Label7" runat="server" SkinID="label" Text="De-Activation&nbsp;Remarks" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVIDALCancellationRemarks" runat="server" SkinID="textbox" MaxLength="500"
                                                TextMode="MultiLine" Style="min-height: 40px; max-height: 40px; min-width: 350px; max-width: 350px;"
                                                Width="350px" Height="40px" onkeyup="return MaxLenTxt(this, 500);" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </AJAX:TabPanel>
                        <AJAX:TabPanel ID="tbpnlOtherAllergy" runat="server" HeaderText="Other Allergies(0)">
                            <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlOtherAllergy" runat="server" Height="380px" CssClass="panel_x_y"
                                                Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel234" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvOtherAllergy" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvOtherAllergy" Width="100%" runat="server" AutoGenerateColumns="False"
                                                            SkinID="gridview" OnSelectedIndexChanged="gvOtherAllergy_OnSelectedIndexChanged"
                                                            OnRowCommand="gvOtherAllergy_RowCommand" OnRowDataBound="gvOtherAllergy_RowDataBound" ShowHeaderWhenEmpty="true">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Onset Date" ItemStyle-Width="60px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbloaAllergyDate" runat="server" SkinID="label" Text='<%#Eval("AllergyDate")%>' />
                                                                        <asp:HiddenField ID="hdnoaAllergyDate" runat="server" Value='<%#Eval("WitoutFormatEncodedDate")%>' />
                                                                        <asp:HiddenField ID="hdnoaId" runat="server" Value='<%#Eval("Id") %>' />
                                                                        <asp:HiddenField ID="hdnoaAllergyID" runat="server" Value='<%#Eval("AllergyID") %>' />
                                                                        <asp:HiddenField ID="hdnoaGENERIC_ID" runat="server" Value='<%#Eval("GENERIC_ID") %>' />
                                                                        <asp:HiddenField ID="hdnoaTYPE" runat="server" Value='<%#Eval("AllergyType")%>' />
                                                                        <asp:HiddenField ID="hdnoaRemarks" runat="server" Value='<%#Eval("Remarks")%>' />
                                                                        <asp:HiddenField ID="hdnoaIntolerance" runat="server" Value='<%#Eval("Intolerance")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allergy">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbloaAllergyName" runat="server" SkinID="label" Text='<%#Eval("AllergyName")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Reaction/Any Adverse Drug Event" ItemStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbloaReaction" runat="server" SkinID="label" Text='<%#Eval("Reaction")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True"
                                                                    ItemStyle-Width="30px">
                                                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                                </asp:CommandField>
                                                                <asp:TemplateField ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtnoaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                                            CommandArgument='<%#Eval("Id")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                    <EmptyDataTemplate><span class="col-md-12 well-sm text-center" style="background-color: #f8f9fa!important;">Record not found</span></EmptyDataTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" style="width: 140px">
                                            <asp:Label ID="Label6" runat="server" SkinID="label" Text="De-Activation&nbsp;Remarks" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtoaCancellationRemarks" runat="server" SkinID="textbox" MaxLength="500"
                                                TextMode="MultiLine" Style="min-height: 40px; max-height: 40px; min-width: 350px; max-width: 350px;"
                                                Width="350px" Height="40px" onkeyup="return MaxLenTxt(this, 500);" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </AJAX:TabPanel>
                        <AJAX:TabPanel ID="tbpnlDeactivateAllergy" runat="server" HeaderText="De-Activate Allergies(0)">
                            <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlDeactivateAllergy" runat="server" Height="380px" CssClass="panel_x_y"
                                                Width="100%">
                                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvDeactivateAllergy" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvDeactivateAllergy" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                                            SkinID="gridview" Width="100%" CssClass="allergy-table">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Onset Date" ItemStyle-Width="60px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaAllergyDate" runat="server" SkinID="label" Text='<%#Eval("AllergyDate") %>' />
                                                                        <asp:HiddenField ID="hdndaAllergyDate" runat="server" Value='<%#Eval("WitoutFormatEncodedDate") %>' />
                                                                        <asp:HiddenField ID="hdndaId" runat="server" Value='<%#Eval("Id") %>' />
                                                                        <asp:HiddenField ID="hdndaAllergyID" runat="server" Value='<%#Eval("AllergyID") %>' />
                                                                        <asp:HiddenField ID="hdndaGENERIC_ID" runat="server" Value='<%#Eval("GENERIC_ID") %>' />
                                                                        <asp:HiddenField ID="hdndaTYPE" runat="server" Value='<%#Eval("AllergyType") %>' />
                                                                        <asp:HiddenField ID="hdndaReaction" runat="server" Value='<%#Eval("Reaction")%>' />
                                                                        <asp:HiddenField ID="hdndaIntolerance" runat="server" Value='<%#Eval("Intolerance")%>' />
                                                                        <asp:HiddenField ID="hdndaRemarks" runat="server" Value='<%#Eval("Remarks") %>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Allergy">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldaAllergyName" runat="server" SkinID="label" Text='<%#Eval("AllergyName")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="De-Activation Remarks" ItemStyle-Width="190px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCancelRemarks" runat="server" SkinID="label" Text='<%#Eval("CancellationRemarks")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                                    <EmptyDataTemplate><span class="col-md-12 well-sm  text-center" style="background-color: #f8f9fa!important;">Record not found</span></EmptyDataTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </AJAX:TabPanel>
                    </AJAX:TabContainer>

                    <asp:Label ID="lblUserName" runat="server" SkinID="label" Visible="false" />
                </div>
                    </div>



                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:HiddenField ID="hdnStoreId" runat="server" />
                            <asp:HiddenField ID="hdnGenericId" runat="server" />
                            <asp:HiddenField ID="hdnGenericName" runat="server" />
                            <asp:HiddenField ID="hdnItemId" runat="server" />
                            <asp:HiddenField ID="hdnItemName" runat="server" />
                            <asp:Literal ID="ltrhdnCurrentRowId" Visible="false" runat="server" />
                            <asp:Literal ID="lblhdn_GENERIC_ID" Visible="false" runat="server" />
                            <asp:Literal ID="lblhdn_ALLERGYID" Visible="false" runat="server" />
                            <asp:Literal ID="lblhdn_TYPE" Visible="false" runat="server" />
                            <asp:TextBox ID="txteditno" Style="visibility: hidden; position: absolute;" runat="server" />
                            <asp:Button ID="btnGetInfo" runat="server" Text="" CausesValidation="false" SkinID="button"
                                Style="visibility: hidden;" OnClick="btnGetInfo_Click" />
                            <asp:Button ID="btnGetInfoGeneric" runat="server" Text="" CausesValidation="false"
                                SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfoGeneric_Click" />
                            <asp:Button ID="btnAlert" runat="server" Text="" CausesValidation="false" SkinID="button"
                                Style="visibility: hidden;" OnClick="btnAlert_Click" />
                            <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize" />
                                </Windows>
                            </telerik:RadWindowManager>
                        </td>
                        <td>
                            <div id="divDelete" runat="server" visible="false" style="width: 250px; z-index: 100; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; background-color: #FFF8DC; border-right: 4px solid
                #CCCCCC; border-top: 4px solid #CCCCCC; position: absolute; bottom: 0; height: 75px; left: 470px; top: 355px">
                                <table width="100%" border="0">
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblTitle" runat="server" Text="Do you want to delete ?" SkinID="label"
                                                Font-Bold="true" Font-Size="Small" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_OnClick" CssClass="btn btn-primary"
                                                Width="60px" />
                                        </td>
                                        <td>&nbsp;
                                            <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_OnClick" CssClass="btn btn-primary"
                                                Width="60px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
