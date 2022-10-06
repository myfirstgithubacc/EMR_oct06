<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddChargeTimewisOT.aspx.cs"
    Inherits="OTScheduler_AddChargeTimewisOT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Equipment Charges ( Time Based )</title>
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0" class="clsheader" bgcolor="#e3dcco">
        <tr>
            <td style="width: 270px; padding-left: 10px;" align="left">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left">
            </td>
            <td align="right">
                <asp:Button ID="btnsave" OnClick="btnsave_OnClick" runat="server" SkinID="Button"
                    ValidationGroup="save" Text="Save" />
                <asp:Button ID="ibtnClose" runat="server" AccessKey="C" SkinID="Button" Text="Close"
                    ToolTip="Close" OnClientClick="window.close();" />
                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px;
                    border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                    cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblReg" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                                SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblregNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                Font-Bold="true" />
                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                Font-Bold="true" />
                            <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                Font-Bold="true" />
                            <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" />
                            <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" />
                            <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                Font-Bold="true" />
                            <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" SkinID="label"
                                Font-Bold="true" />
                            <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" />
                            <%-- <asp:HiddenField ID="hdnCompanyId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnInsuranceId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnCardId" runat="server" Value="" />
                                    <asp:HiddenField ID="hdnConfirmValue" runat="server" Value="" />--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table width="700px">
        <tr>
            <td>
                <asp:Label ID="lblService" runat="server" Text="Equipment" SkinID="label"></asp:Label>
            </td>
            <td colspan="3">
                <telerik:RadComboBox ID="ddlService" runat="server" SkinID="DropDown" Font-Size="10px"
                    Width="400px" Filter="Contains" OnSelectedIndexChanged="ddlService_OnSelectedIndexChanged"
                    AutoPostBack="true">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label>
            </td>
            <td>
                <telerik:RadDateTimePicker ID="dtpfromdate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                    OnSelectedDateChanged="dtpfromdate_SelectedDateChanged" AutoPostBackControl="Both">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <TimeView CellSpacing="-1">
                    </TimeView>
                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                    <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth=""
                        AutoPostBack="True">
                    </DateInput>
                </telerik:RadDateTimePicker>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="To"></asp:Label>
            </td>
            <td>
                <telerik:RadDateTimePicker ID="dtpTodate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                    OnSelectedDateChanged="dtpTodate_SelectedDateChanged" AutoPostBackControl="Both">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <TimeView CellSpacing="-1">
                    </TimeView>
                    <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                    <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth=""
                        AutoPostBack="True">
                    </DateInput>
                </telerik:RadDateTimePicker>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Sr. Charge"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCharge" SkinID="textbox" ReadOnly="true" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblDrCharge" runat="server" Text="Dr. Charge"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDrCharge" SkinID="textbox" ReadOnly="true" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDoctor" runat="server" Text="Doctor"></asp:Label>
                <asp:Label ID="lblStarDoctor" ForeColor="Red" runat="server" Text="*"></asp:Label>
            </td>
            <td colspan="3">
                <telerik:RadComboBox ID="ddlDoctor" Width="400px" runat="server" SkinID="DropDown"
                    Filter="Contains" MarkFirstMatch="true" AutoPostBack="false">
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPeriod" Text="Period" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblUnitDescription" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblUnit" Text="Unit Calculated" runat="server"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtUnit" SkinID="textbox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRemark" runat="server" Text="Remarks"></asp:Label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtRemark" SkinID="textbox" MaxLength="100" Width="400px" runat="server"
                    Text="" TextMode="SingleLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:HiddenField ID="hdnDoctorRequired" runat="server" Value="" />
                <asp:HiddenField ID="hdnChargeType" runat="server" Value="" />
                <asp:HiddenField ID="hdnDiscount" runat="server" Value="" />
            </td>
        </tr>
        <tr>
            <td align="right" colspan="4">
                <asp:Button ID="btnAddToGrid" OnClick="btnAddToGrid_OnClick" runat="server" SkinID="Button"
                    ValidationGroup="save" Text="Add To Grid" />
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                <asp:GridView ID="gvService" TabIndex="3" Width="100%" runat="server" AutoGenerateColumns="False"
                    ShowFooter="false" SkinID="gridview2" OnRowCommand="gvService_OnRowCommand">
                    <%--OnRowDataBound="gvService_RowDataBound"      Width="100%" OnRowCommand="gvService_RowCommand">--%>
                    <Columns>
                        <%-- <telerik:GridClientSelectColumn UniqueName="chkCollection" HeaderStyle-Width="30px" />--%>
                        <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Literal ID="ltrId" runat="server" Text='<%#Eval("SNo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Equipment Name" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lblEquipmentName" runat="server" SkinID="label" Text='<%#Eval("ServiceName") %>' />
                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderId") %>' />
                                <asp:HiddenField ID="hdnDetailId" runat="server" Value='<%#Eval("DetailsId") %>' />
                                <asp:HiddenField ID="hdnOTEquipmentDetailsId" runat="server" Value='<%#Eval("OTEquipmentDetailsId") %>' />
                                <asp:HiddenField ID="hdnAmountPayableByPatient" runat="server" Value='<%#Eval("AmountPayableByPatient") %>' />
                                <asp:HiddenField ID="hdnAmountPayableByPayer" runat="server" Value='<%#Eval("AmountPayableByPayer") %>' />
                                <asp:HiddenField ID="hdnServiceDiscountPercentage" runat="server" Value='<%#Eval("ServiceDiscountPercentage") %>' />
                                <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" Value='<%#Eval("ServiceDiscountAmount") %>' />
                                
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='From Date' HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Label ID="lblFromDate" runat="server" SkinID="label" Text='<%#Eval("FromDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='To Date' HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Label ID="lblToDate" runat="server" SkinID="label" Text='<%#Eval("ToDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Charge" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblSrCharge" runat="server" SkinID="label" Text='<%#Eval("ServiceAmount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Cal." HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblUnit" runat="server" SkinID="label" Text='<%#Eval("Units") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Charge" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="lblDrCharge" runat="server" SkinID="label" Text='<%#Eval("NetAmount") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkselect" runat="server" SkinID="checkbox" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                    CommandArgument='<%#Eval("ServiceId")%>' ToolTip="Delete" ImageUrl="~/Images/DeleteRow.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
