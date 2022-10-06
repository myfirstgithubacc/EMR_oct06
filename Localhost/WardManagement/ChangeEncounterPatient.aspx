<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeEncounterPatient.aspx.cs" Inherits="WardManagement_ChangeEncounterPatient" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Encounter Status</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
</head>

<script type="text/javascript">
    if (window.captureEvents) {
        window.captureEvents(Event.KeyUp);
        window.onkeyup = executeCode;
    }
    else if (window.attachEvent) {
        document.attachEvent('onkeyup', executeCode);
    }

    function MaxLenTxt(TXT, MAX) {
        if (TXT.value.length > MAX) {
            alert("Text length should not be greater then " + MAX + " ...");

            TXT.value = TXT.value.substring(0, MAX);
            TXT.focus();
        }
    }

    function openRadWindow(strPageNameWithQueryString) {
        var oWnd = radopen(strPageNameWithQueryString, "RadWindow1");
    }

    function executeCode(evt) {
        if (evt == null) {
            evt = window.event;
        }
        var theKey = parseInt(evt.keyCode, 10);
        switch (theKey) {

            case 114: //F3
                $get('<%=btnSave.ClientID%>').click();
                break;

            case 119:  // F8
                $get('<%=btnclose.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }

    function Confirm() {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        if (confirm("Have you returned all the medicines ?")) {
            confirm_value.value = "Yes";
        } else {
            confirm_value.value = "No";
        }
        document.forms[0].appendChild(confirm_value);
    }


</script>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrptmanger1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-2">
                <h2>Change Encounter Status</h2>
            </div>
            <div class="col-md-10 text-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Text="&nbsp;" />
            </div>
        </div>

        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <tr align="left">
                    <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>




        <div class="container-fluid" id="Table1" runat="server">
            <div class="row" id="Table2" runat="server">
                <div class="col-md-4 mb-3 mb-md-0">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <div class="row form-groupTop01">
                                <div class="col-md-5 col-4 label2">
                                    <asp:Label ID="Lable1" runat="server" Text="Change&nbsp;Status" />
                                </div>
                                <div class="col-md-7 col-8">
                                    <telerik:RadComboBox ID="ddlStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                </div>
                            </div>
                            <div class="row form-groupTop01" id="dvReason" runat="server" visible="false">
                                <div class="col-md-5 col-4 label2">
                                    <asp:Label ID="Label3" runat="server" Text="Reason" /><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-7 col-8">
                                    <telerik:RadComboBox ID="ddlReason" runat="server" Width="100%" EmptyMessage="[ Select ]" MarkFirstMatch="false" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row form-groupTop01" id="trEdod" runat="server">
                        <div class="col-md-5 col-4 PaddingRightSpacing label2">
                            <asp:Label ID="Label1" runat="server" Text="Date Of Discharge"></asp:Label><span style="color: Red">*</span>
                        </div>
                        <div class="col-md-7 col-8">
                            <telerik:RadDatePicker ID="dtpEod" Width="100%" runat="server"></telerik:RadDatePicker>
                        </div>
                    </div>

                    <div class="row form-groupTop01" id="trStatus" runat="server">
                        <div class="col-md-5 col-4 label2">
                            <asp:Label ID="ltrldischargestatus" runat="server" Text="<%$ Resources:PRegistration, dischargestatus %>"></asp:Label><span style="color: Red">*</span>
                        </div>
                        <div class="col-md-7 col-8">
                            <telerik:RadComboBox ID="ddldischargestatus" runat="server" OnSelectedIndexChanged="ddldischargestatus_SelectedIndexChanged" AutoPostBack="true" Width="100%"></telerik:RadComboBox>
                        </div>
                    </div>
                    <div class="row form-groupTop01" id="trAdmittingDoctor" runat="server" visible="false">
                        <div class="col-md-5 col-4 label2">
                            <asp:Label ID="ltrladmitingdoctor" runat="server" Text="Advising Doctor"></asp:Label>
                            <span style="color: Red">*</span>
                        </div>
                        <div class="col-md-7 col-8">
                            <telerik:RadComboBox ID="ddladmitingdoctor" runat="server" Width="100%" MarkFirstMatch="true" />
                        </div>
                    </div>
                    <div class="row form-groupTop01" id="dvCommonRekarks" runat="server" visible="false">
                        <div class="col-md-5 col-4 label2">
                            <asp:Label ID="Label2" runat="server" Text="Common Remarks" />
                            <span id="spnCommonRekarksMandatory" runat="server" style="color: Red">*</span>
                        </div>
                        <div class="col-md-7 col-8">
                            <asp:TextBox ID="txtCommonRekarks" runat="server" MaxLength="2000" TextMode="MultiLine"
                                onkeyup="return MaxLenTxt(this, 2000);" Style="min-height: 60px; max-height: 60px; min-width: 300px; max-width: 300px;" />
                        </div>
                    </div>
                    <div class="row margin_Top ">
                        <div class="col-md-5 col-4">
                            <asp:Button ID="btnCheckListSFB" runat="server" Text="CheckList (Sent For Billing)" CssClass="btn btn-primary" OnClick="btnCheckListSFB_Click" />
                        </div>
                        <div class="col-md-7 col-8">
                            <asp:Button ID="btnSave" runat="server" Text="Save (Ctrl+F2)" CssClass="btn btn-primary" ValidationGroup="S" OnClick="btnSave_Click" />
                            <asp:Button ID="btnclose" runat="server" Text="Close (Ctrl+F8)" CssClass="btn btn-primary" CausesValidation="false" OnClientClick="window.close();" />
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-6 " runat="server">
                    <asp:Panel ID="pnldeathdetails" runat="server" Visible="false">
                        <div class="row brdr01">
                            <div class="col-12 header_main mb-2">
                                <h2>Death Detail</h2>
                            </div>
                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-4" id="trExpiredReason">
                                        <asp:Label ID="lblExpiredReason" runat="server" SkinID="label" Text="Expired Reason"></asp:Label>
                                        <span id="Span1" runat="server" style="color: Red">*</span>
                                    </div>
                                    <div class="col-8">
                                        <asp:DropDownList ID="ddlExpiredReason" runat="server" AutoPostBack="True"
                                            SkinID="DropDown" Width="100%" OnSelectedIndexChanged="ddlExpiredReason_SelectedIndexChanged" />
                                        <asp:TextBox ID="txtOtherExpiredRemarks" Visible="true" runat="server" AutoPostBack="True" Width="100%" SkinID="textbox" />
                                    </div>

                                </div>
                            </div>
                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-4">
                                        <asp:Label ID="ltrldeathdatetime" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, deathdatetime %>"></asp:Label>
                                        <span style="color: Red">*</span>
                                    </div>
                                    <div class="col-8">
                                        <telerik:RadDateTimePicker ID="dtpdeathdatetime" runat="server" MinDate="01/01/1900 00:00"
                                            Calendar-DayNameFormat="FirstLetter" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                            DateInput-DateDisplayFormat="dd/MM/yyyy HH:mm" Calendar-EnableAjaxSkinRendering="True" Width="100%">
                                        </telerik:RadDateTimePicker>
                                    </div>
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-4">
                                        <asp:Label ID="ltrlbodydisposion" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, disposbody %>"></asp:Label>
                                    </div>
                                    <div class="col-8">
                                        <asp:DropDownList ID="ddldepositionofbody" Width="100%" runat="server" SkinID="DropDown">
                                            <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Discgarge Home"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Against Medical Advice"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Transfer To There Care"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Died More Than 48 hours after Admission"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Died Post Operative"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="Died Less Than 48 hours after Admission"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-4">
                                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="Mode of Transfer of Body"></asp:Label>
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtmodeoftransfer" runat="server" Width="100%" MaxLength="100" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-4">
                                        <asp:Label ID="ltrlbodyrecby" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bodyrecby %>"></asp:Label>
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtbodyreceviedby" runat="server" Width="100%" MaxLength="100" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-12">
                                <div class="row form-group">
                                    <div class="col-4">
                                        <asp:Label ID="Label5" runat="server" SkinID="label" Text="Authorised Burial Permission"></asp:Label>
                                    </div>
                                    <div class="col-8">
                                        <asp:TextBox ID="txtauthorised" runat="server" Width="100%" MaxLength="100" SkinID="textbox"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-md-4 col-6">
                    <table width="100%">
                        <tr>
                            <td valign="top">
                                <asp:Panel ID="pnlPaymentDetail" runat="server" ScrollBars="None">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="grddeathcause" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                                OnItemCreated="grddeathcause_ItemCreated" OnItemDataBound="grddeathcause_ItemDataBound"
                                                ShowFooter="true" Skin="Office2007" Visible="false">
                                                <ItemStyle HorizontalAlign="Left" />
                                                <MasterTableView TableLayout="Auto">
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Doctor Name">
                                                            <HeaderStyle Width="100px" />
                                                            <ItemStyle Width="100px" />
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddldoctor" runat="server" CssClass="gridInput" DataSourceID="SQLMode"
                                                                    DataTextField="Name" DataValueField="DoctorId" Style="width: 100%;">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <%--  <FooterTemplate>
                                                                <asp:LinkButton ID="lnkAddRow" runat="server" OnClick="lnkAddRow_Click" Text="&lt;strong&gt;+&lt;/strong&gt;"></asp:LinkButton>
                                                            </FooterTemplate>--%>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Death Cause">
                                                            <HeaderStyle Width="200px" />
                                                            <ItemStyle Width="200px" />
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="gridInput" MaxLength="150"
                                                                    Width="100%" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                            <asp:SqlDataSource ID="SQLMode" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                                                EnableCaching="true" SelectCommand="SELECT 0 AS DoctorId, 'Select' As Name , '' as sort UNION SELECT Id as DoctorId, isnull(FirstName,'') + ' ' + isnull(MiddleName,'') + ' ' + isnull(Lastname,'') as Name, 'x' as sort  FROM employee WHERE (Employeetype in(1,17)) and Active=1 ORDER BY sort,name"
                                                SelectCommandType="Text"></asp:SqlDataSource>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </td>

                        </tr>

                    </table>
                </div>
            </div>
        </div>
        </div>
        <div id="dvTemplate" runat="server" style="width: 550px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 150px; left: 500px; top: 170px;">
            <table cellspacing="2" cellpadding="2" width="500px">
                <tr>
                    <td>
                        <b>Lists of mandatory templates not fill in EMR. Please fill up following templates.</b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 30%; text-align: left;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvTemplate" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                    ShowFooter="false" Skin="Office2007">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <MasterTableView TableLayout="Auto">
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Template Name">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnCancel_OnClick"
                            SkinID="Button" />
                    </td>
                </tr>
            </table>
        </div>

        <table width="100%" cellpadding="0" cellspacing="0" style="background: #ffffff;">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close" InitialBehaviors="Maximize" />
                                </Windows>
                            </telerik:RadWindowManager>

                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnCompanyCode" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnadmitingdoctor" runat="server" Value="N" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
