<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TagPatient.aspx.cs" Inherits="OTScheduler_TagPatient" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tag OT Patient</title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">

        <script language="javascript" type="text/javascript">
            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;

                    var cmbvalue = Number(0);
                    var comboBox1 = $find('<%= ddlSearchOn.ClientID %>');
                    cmbvalue = Number(comboBox1.get_value());
                    if (cmbvalue == 0) {
                        $get('<%=txtPatientNo.ClientID%>').value = RegistrationNo;
                    }
                    if (cmbvalue == 1) {
                        $get('<%=txtPatientNo.ClientID%>').value = EncounterNo;
                    }
                    $get('<%=hdnRegistrationNo.ClientID %>').value = RegistrationNo;
                    $get('<%=hdnEncounterNo.ClientID %>').value = EncounterNo;
                    $get('<%=btnSearchByPatientNo.ClientID%>').click();
                }
            }                                          
        </script>

    </telerik:RadScriptBlock>
    <style type="text/css">
        .style1
        {
            width: 75px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
        DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
    <div id="dvZone1" style="width: 100%">
        <table cellpadding="0" cellspacing="2" width="100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" />&nbsp;
                    <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="window.close();" />&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 50%;" valign="top">
                            <div id="pnl1" runat="server" style="border: solid 1px #dcdcdc; height: 315px;">
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td colspan="5" class="txt06" align="center">
                                            Patient Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="lblInfoRegNo" runat="server" Text="<%$ Resources:PRegistration, regno %>"
                                                SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRegNo" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="Label8" runat="server" Text="First Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td  align="left">
                                            <asp:Label ID="lblFName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="Label9" runat="server" Text="Middle Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label10" runat="server" Text="Last Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblLName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="Label11" runat="server" Text="DOB" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDOB" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label13" runat="server" Text="Age(Y-M-D)" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblAgeY" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>-
                                            <asp:Label ID="lblAgeM" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>-
                                            <asp:Label ID="lblAgeD" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1">
                                            <asp:Label ID="Label1" runat="server" Text="Gender" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGender" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr id="trOtDetails" runat="server">
                                        <td colspan="5" class="txt06" align="center">
                                            <asp:Label ID="lblDetails" Text="OT Details" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trOtDetails1" runat="server">
                                        <td class="style1">
                                            <asp:Label ID="Label2" runat="server" Text="Theater" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTheaterName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Date" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblDate" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trOtDetails2" runat="server">
                                        <td class="style1">
                                            <asp:Label ID="Label4" runat="server" Text="Time From" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTimeFrom" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="To" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <asp:Label ID="lblTimeTo" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trOtDetails3" runat="server">
                                        <td class="style1">
                                            <asp:Label ID="Label5" runat="server" Text="Service" SkinID="label"></asp:Label>
                                        </td>
                                        <td colspan="4">
                                            <asp:Label ID="lblService" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width: 50%;" valign="top">
                            <div id="Div1" runat="server" style="border: solid 1px #dcdcdc; height: 315px;">
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr>
                                        <td colspan="4" class="txt06" align="center">
                                            Tag Patient Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:RadioButtonList ID="rblOPIP" runat="server" RepeatLayout="Table" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblOPIP_OnSelectedIndexChanged">
                                                <asp:ListItem Text="Registration" Value="O" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Admission" Value="I"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearchByPatientNo">
                                                <asp:ImageButton ID="ibtnOpenSearchPatientPopup" ImageUrl="~/Images/Binoculr.ico"
                                                    ToolTip="Click to open search patient window" runat="server" OnClick="ibtnOpenSearchPatientPopup_OnClick"
                                                    ImageAlign="AbsMiddle" />&nbsp;
                                                <telerik:RadComboBox ID="ddlSearchOn" runat="server" Width="100px" Skin="Metro" ForeColor="DarkGray">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="<%$ Resources:PRegistration, UHID%>" Value="0" />
                                                        <telerik:RadComboBoxItem Selected="True" Text="IP No" Value="1" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                <asp:TextBox ID="txtPatientNo" runat="server" Width="100px" Style="padding-left: 1px;"
                                                    ToolTip="Press enter to search" />
                                                <asp:Button ID="btnSearchByPatientNo" runat="server" OnClick="btnSearchByPatientNo_OnClick"
                                                    SkinID="Button" Text="Search" CausesValidation="false" Style="visibility: hidden;"
                                                    Width="1" />
                                                <asp:ImageButton ID="imgCalYear" runat="server" ImageUrl="~/Images/insert_table.gif"
                                                    Style="visibility: hidden;" Width="6%" ToolTip="Calculate Year, Month, Days"
                                                    OnClick="imgCalYear_Click" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblInfoTReg" runat="server" Text="RegNo" SkinID="label" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTRegNo" runat="server" SkinID="label" ForeColor="Maroon" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="First Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:Label ID="lblTFName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label12" runat="server" Text="Middle Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltMName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Last Name" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTLName" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="DOB" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadDatePicker ID="dtpTDOB" Width="100px" runat="server" AutoPostBack="true"
                                                MinDate="01/01/1900" Skin="Metro">
                                            </telerik:RadDatePicker>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="Age(Y-M-D)" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTAgeY" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>&nbsp;<asp:Label
                                                ID="Label16" runat="server" SkinID="label" Text="-" ForeColor="Maroon"></asp:Label>&nbsp;
                                            <asp:Label ID="lblTAgeM" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>&nbsp;<asp:Label
                                                ID="Label21" runat="server" SkinID="label" Text="-" ForeColor="Maroon"></asp:Label>&nbsp;
                                            <asp:Label ID="lblTAgeD" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="Gender" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTGender" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label19" runat="server" Text="Admission date" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTAdmissionDate" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Text="Bed No" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTBedNo" runat="server" SkinID="label" ForeColor="Maroon"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                        Width="1200" Height="600" Left="10" Top="10">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                                Width="900" Height="600" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <asp:HiddenField ID="hdnOTBookingID" Value="0" runat="server" />
                                    <asp:HiddenField ID="hdnOTBookingNO" Value="0" runat="server" />
                                    <asp:HiddenField ID="hdnRegistrationId" Value="0" runat="server" />
                                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnEncounterId" Value="0" runat="server" />
                                    <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
