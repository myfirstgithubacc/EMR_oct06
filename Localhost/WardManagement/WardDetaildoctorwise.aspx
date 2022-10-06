<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="WardDetaildoctorwise.aspx.cs" Inherits="WardManagement_WardDetaildoctorwise"
    Title="Ward Details" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

        <script type="text/javascript">

            function showMenu2(e, wardno, bedcat, bedno, regno, encno, currentEncStatusId, hdnIsCritical111, hdnUnperformedServices111) {

                var menu = $find("<%=RadContextMenu2.ClientID %>");

                var lblwardno = document.getElementById('ctl00_ContentPlaceHolder1_lblwardno');
                var lblbedcetogry = document.getElementById('ctl00_ContentPlaceHolder1_lblbedcetogry');
                var lblbeno = document.getElementById('ctl00_ContentPlaceHolder1_lblbeno');
                var lblregno = document.getElementById('ctl00_ContentPlaceHolder1_lblregno');
                var lblencno = document.getElementById('ctl00_ContentPlaceHolder1_lblencno');

                var lblCriticalIndication1 = document.getElementById(hdnIsCritical111).value;

                var hdnUnperformedServices1 = document.getElementById(hdnUnperformedServices111).value;
                var AcknoledgeEncStatusId = $get('<%=hdnOpenEncStatusId.ClientID%>').value;

                lblwardno.value = wardno;
                lblbedcetogry.value = bedcat;
                lblbeno.value = bedno;
                lblregno.value = regno;
                lblencno.value = encno;

                $get('<%=hdnCurrentEncStatusId.ClientID%>').value = currentEncStatusId;
                var OpenEncStatusId = $get('<%=hdnOpenEncStatusId.ClientID%>').value;
                var AcknoledgeEncStatusId = $get('<%=hdnAcknoledgeEncStatusId.ClientID%>').value;
                var MarkedForDischargedEncStatusId = $get('<%=hdnMarkedForDischargedEncStatusId.ClientID%>').value;
                $get('<%=hdnDisableDashboardControl.ClientID%>').value = 0;


                var items = menu.get_items();
                for (var idx = 0; idx < items.get_count(); idx++) {

                    var VAL = items.getItem(idx).get_value();
                    $get('<%=hdnDisableDashboardControl.ClientID%>').value = 1;
                    if (VAL == "DG" || VAL == "DR" || VAL == "SV" || VAL == "DV" || VAL == "BT") {
                        items.getItem(idx).disable();
                        if (currentEncStatusId == OpenEncStatusId || currentEncStatusId == MarkedForDischargedEncStatusId) {
                            items.getItem(idx).enable();
                        }
                    }
                    if (VAL == "VCT") {
                        items.getItem(idx).disable();
                    }
                    if (VAL == "VCT" && lblCriticalIndication1 == "1") {
                        items.getItem(idx).enable();
                    }
                    else if (VAL == "VCT" && lblCriticalIndication1 == "0") {
                        items.getItem(idx).disable();
                    }

                    if (VAL == "VUS") {
                        items.getItem(idx).disable();
                    }
                    if (VAL == "VUS" && hdnUnperformedServices1 == "1") {
                        items.getItem(idx).enable();
                    }
                    else if (VAL == "VUS" && hdnUnperformedServices1 == "0") {
                        items.getItem(idx).disable();
                    }
                    if (VAL != "VC") {
                        if (currentEncStatusId == AcknoledgeEncStatusId) {
                            items.getItem(idx).disable();
                        }
                    }

                }


                menu.show(e);
            }

            function OnClientClose(oWnd) {
                $get('<%=btnfind.ClientID%>').click();
            }

            function SearchPatientOnClientClose(oWnd, args) {
            }

            function ddlProblem_OnClientSelectedIndexChanged(sender, args) {
                var item = args.get_item();
                var AccountNo = item.get_attributes().getAttribute("Account");
                $get('<%=hdnregID.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=hdnregno.ClientID%>').value = AccountNo;
                $get('<%=btnRegNoFind.ClientID%>').click();
            }

            function OnClearClientClose(oWnd) {
                $get('<%=btnClear.ClientID%>').click();
            }
        </script>

    </telerik:RadCodeBlock>
    <div oncontextmenu="return false">
        <asp:UpdatePanel ID="updatepanelmain" runat="server">
            <ContentTemplate>
                <table border="0" class="clsheader" cellpadding="0" cellspacing="0" width="100%">
                    <tr class="clsheader">
                        <td>
                            <asp:Label ID="warddetails" runat="server" Text="&nbsp;Ward&nbsp;Details" ToolTip="Ward Details Employee Wise"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label18" runat="server" SkinID="label" Text="Bed Status" />
                            <telerik:RadComboBox ID="ddlBedStatus" runat="server" Width="110px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlbedstatus_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" Font-Bold="true" ForeColor="Black" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td id="tdWard" runat="server" visible="false">
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Ward&nbsp;" />
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlWard" runat="server" Width="150px" AutoPostBack="true"
                                DropDownWidth="250px" OnSelectedIndexChanged="ddlWard_OnSelectedIndexChanged" />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text="Legend&nbsp;" />
                            <telerik:RadComboBox ID="ddlEncounterStatus" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlEncounterStatus_OnSelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text='<%# Eval("StatusColor") %>' Value="" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="Label12" runat="server" SkinID="label" Text="Column(s)" />
                            <telerik:RadComboBox ID="ddlColumns" runat="server" MarkFirstMatch="true" Width="40px"
                                OnSelectedIndexChanged="ddlColumns_OnSelectedIndexChanged" AutoPostBack="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="6" Value="6" />
                                    <telerik:RadComboBoxItem Text="7" Value="7" />
                                    <telerik:RadComboBoxItem Text="8" Value="8" />
                                    <telerik:RadComboBoxItem Text="9" Value="9" />
                                    <telerik:RadComboBoxItem Text="10" Value="10" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" Text="Entry Sites"></asp:Label>&nbsp;
                            <telerik:RadComboBox ID="ddlEntrySitesActual" SkinID="DropDown" runat="server" Width="120px"
                                DropDownWidth="250px" EmptyMessage="[ Select ]" MarkFirstMatch="true" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlEntrySitesActual_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr class="clsheader" >
                        <td colspan="7" style="padding-left: 10px;">
                            <asp:Label ID="Label11" runat="server" SkinID="label" Text="Patient&nbsp;" />
                            <telerik:RadComboBox ID="ddlPatient" runat="server" Width="300px" Height="400px"
                                Skin="Metro" EnableLoadOnDemand="true" ZIndex="50000" HighlightTemplatedItems="true"
                                EmptyMessage="Search by regno, ipno, name..." DropDownWidth="650px" OnItemsRequested="ddlPatient_ItemsRequested"
                                ShowMoreResultsBox="true" AllowCustomText="true" EnableVirtualScrolling="true"
                                DataValueField="RegistrationId" DataTextField="PatientName" OnClientSelectedIndexChanged="ddlProblem_OnClientSelectedIndexChanged">
                                <HeaderTemplate>
                                    <table style="width: 600px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 80px">
                                                <asp:Literal ID="Literal1" runat="server" Text='<%$ Resources:PRegistration, Regno%>'></asp:Literal>
                                            </td>
                                            <td style="width: 80px">
                                                <asp:Literal ID="Literal2" runat="server" Text="IP No"></asp:Literal>
                                            </td>
                                            <td style="width: 200px">
                                                Name
                                            </td>
                                             <td style="width: 200px">
                                                Father Name
                                            </td>
                                            <td style="width: 110px">
                                                Age/Gender
                                            </td>
                                            <td style="width: 110px">
                                                Ward / Bed
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 600px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 80px">
                                                <%# DataBinder.Eval(Container,"Attributes['Account']" )%>
                                            </td>
                                            <td style="width: 80px">
                                                <%# DataBinder.Eval(Container,"Attributes['EncounterNo']" )%>
                                            </td>
                                            <td style="width: 200px;">
                                                <%# DataBinder.Eval(Container, "Text")%>
                                            </td>
                                             <td style="width: 200px;">
                                                <%# DataBinder.Eval(Container, "Attributes['FatherName']")%>
                                            </td>
                                            <td style="width: 110px;">
                                                <%# DataBinder.Eval(Container, "Attributes['AgeGender']")%>
                                            </td>
                                            <td style="width: 110px;">
                                                <%# DataBinder.Eval(Container, "Attributes['Ward/Bed']")%>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadComboBox>
                            &nbsp;
                            <asp:Button ID="btnClear" runat="server" SkinID="Button" Text="Clear" CausesValidation="false"
                                OnClick="btnClear_OnClick" />
                            &nbsp;
                            <asp:LinkButton ID="lnkbloodRe" Font-Size="11px" Visible="true" runat="server" Text="Blood Request"
                                OnClick="lnkbloodRe_OnClick"></asp:LinkButton>
                            <asp:Label ID="lblNoOfRequest" Font-Size="11px" Visible="true" runat="server"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="lnkunperformedSer" Font-Size="11px" Visible="true" runat="server"
                                Text="Unperformed Services" OnClick="lnkunperformedSer_Click"></asp:LinkButton>
                            <asp:Label ID="lblunperformedSer" Font-Size="11px" Visible="true" runat="server"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="lnkdrugordercount" Font-Size="11px" Visible="true" runat="server"
                                Text="Drug Order" OnClick="lnkdrugordercount_Click"></asp:LinkButton>
                            <asp:Label ID="lbldrugordercount" Font-Size="11px" Visible="true" runat="server"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="lnknondrugordercount" Font-Size="11px" Visible="true" runat="server"
                                Text="Non Drug Order" OnClick="lnknondrugordercount_Click"></asp:LinkButton>
                            <asp:Label ID="lblnondrugordercount" Font-Size="11px" Visible="true" runat="server"></asp:Label>
                            &nbsp;
                            <asp:Label ID="lblRejectedSampleStart" runat="server" ForeColor="Maroon" />
                            &nbsp;
                            <asp:LinkButton ID="lnkBtnRejectedSampleIPCount" runat="server" ToolTip="View details of rejected sample for IP"
                            Font-Underline="false" OnClick="lnkBtnRejectedSampleIPCount_OnClick" />
                            <asp:Label ID="lblRejectedSampleEnd" runat="server" ForeColor="Maroon" />
                            <asp:LinkButton ID="lnkBedClearance" runat="server" ToolTip="ICU Clearance" Text="ICU Clearance" 
                            OnClick="lnkBedClearance_OnClick"/>
                            <asp:Label ID="lblBedClearanceCount" runat="server" ForeColor="Maroon" />
                            
                  
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="updatpanel1" runat="server">
                                <ContentTemplate>
                                    <%-- <asp:Panel ID="PanelBeds" runat="server" Height="472px" ScrollBars="auto" Width="100%">--%>
                                    <asp:GridView ID="grvBedStatus" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvBedStatus_RowDataBound"
                                        GridLines="None" ShowHeader="False" OnSelectedIndexChanged="grvBedStatus_SelectedIndexChanged">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div1" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label1" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc1" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton1" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno1" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno1" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical1" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices1" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication1" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices1" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert1" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert1" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory1" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC1" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div2" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label2" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc2" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton2" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno2" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno2" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical2" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices2" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication2" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices2" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert2" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert2" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory2" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName2" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill2" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype2" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC2" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div3" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label3" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc3" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton3" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno3" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno3" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical3" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices3" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication3" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices3" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert3" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert3" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory3" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName3" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill3" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype3" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC3" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div4" runat="server" style="border-style: solid; border-width: 1px; width: 120px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label4" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc4" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton4" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno4" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno4" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical4" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices4" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication4" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices4" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert4" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert4" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory4" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName4" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill4" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype4" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC4" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div5" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label5" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc5" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton5" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno5" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno5" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical5" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices5" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication5" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices5" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert5" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert5" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory5" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName5" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill5" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype5" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC5" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div6" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label6" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc6" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton6" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno6" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno6" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical6" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices6" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication6" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices6" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert6" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert6" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory6" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName6" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill6" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype6" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC6" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div7" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label7" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc7" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton7" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno7" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno7" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical7" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices7" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication7" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices7" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert7" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert7" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory7" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName7" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill7" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype7" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC7" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div8" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label8" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc8" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton8" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno8" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno8" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical8" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices8" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication8" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices8" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert8" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert8" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory8" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName8" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill8" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype8" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC8" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div9" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label9" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc9" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton9" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno9" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno9" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical9" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices9" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication9" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices9" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert9" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert9" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory9" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName9" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill9" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype9" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC9" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div id="div10" runat="server" style="border-style: solid; border-width: 1px; width: 130px;
                                                        height: 100px;">
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label10" runat="server" />
                                                                </td>
                                                                <td align="left" style="width: 10px">
                                                                    <asp:Label ID="lblEnc10" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr valign="middle">
                                                                            <td>
                                                                                <asp:ImageButton ID="Imagebutton10" runat="server" />
                                                                                <asp:HiddenField ID="hdnregno10" runat="server" />
                                                                                <asp:HiddenField ID="hdnencounterno10" runat="server" />
                                                                                <asp:HiddenField ID="hdnIsCritical10" runat="server" />
                                                                                <asp:HiddenField ID="hdnUnperformedServices10" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblCriticalIndication10" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                                    Font-Size="X-Large" ForeColor="Red" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblUnperformedServices10" runat="server" Text="*" Visible="false"
                                                                                    Font-Bold="True" Font-Size="X-Large" ForeColor="Blue" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgAllergyAlert10" runat="server" ImageUrl="~/Icons/allergy.gif" Width="16px"
                                                                                    Height="16px" ToolTip="Allergy Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgMedicalAlert10" runat="server" ImageUrl="~/Icons/MedicalAlert.gif"
                                                                                    Width="16px" Height="16px" ToolTip="Patient Alert" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgReffHistory10" runat="server" ImageUrl="~/Icons/letter_referral.png"
                                                                                    Width="16px" Height="20px" ToolTip="Referral History" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblPatientName10" runat="server" Font-Size="9px" Width="115px" />
                                                                    <asp:Image ID="imgbill10" runat="server" Width="15px" Height="15px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblCompanytype10" runat="server" Font-Size="9px" />
                                                                    &nbsp;&nbsp;&nbsp;<asp:Label ID="lblMLC10" runat="server" Font-Size="9px" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <%--</asp:Panel>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro">
                                <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server">--%>
                                <%-- <ContentTemplate>--%>
                                <telerik:RadContextMenu ID="RadContextMenu2" runat="server" EnableShadows="true"
                                    OnItemClick="RadContextMenu2_ItemClick">
                                    <DefaultGroupSettings Height="470px" />
                                </telerik:RadContextMenu>
                                <%-- </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="RadContextMenu2" />
                                    </Triggers>
                                </asp:UpdatePanel>--%></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRegResource" runat="server" Text="<%$ Resources:PRegistration, Regno%>"
                                Style="visibility: hidden;" SkinID="label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnfncall" runat="server" Text="callfn" Style="visibility: hidden" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblwardno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblbedcetogry" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblbeno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="lblregno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="lblencno" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBedstatus" runat="server" Style="visibility: hidden;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                                        <asp:Button ID="btnfindad" runat="server" Text="Find" Style="visibility: hidden;" />
                                        <asp:HiddenField ID="hdnCurrentEncStatusId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdnOpenEncStatusId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdnMarkedForDischargedEncStatusId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdnDisableDashboardControl" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdnAcknoledgeEncStatusId" runat="server" Value="0" />
                                    </td>
                                    <td>
                                        <input id="hdnregno" type="hidden" runat="server" />
                                        <input id="hdnregID" type="hidden" runat="server" />
                                        <asp:Button ID="btnRegNoFind" Style="visibility: hidden; width: 0%; height: 0%" runat="server"
                                            SkinID="Button" CausesValidation="false" OnClick="btnRegNoFind_OnClick" />
                                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="850" Height="500"
                                            Left="10" Top="10" VisibleStatusbar="false" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                            Modal="true" OnClientClose="SearchPatientOnClientClose">
                                        </telerik:RadWindowManager>
                                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                        
                                        <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                                        </Windows>
                                        </telerik:RadWindowManager>
                                        </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
