<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddTimeBasedService.aspx.cs" Inherits="EMRBILLING_Popup_AddTimeBasedService" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Time wise charge</title>
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-6 col-xs-6">
                <h2 style="color: #333;">
                    <asp:Label ID="Label3" runat="server" Text="Charge Type"></asp:Label></h2>
            </div>
            <div class="col-md-6 col-xs-6 text-right">
                <asp:Button ID="btnStart" OnClick="btnStart_Click" runat="server" CssClass="btn btn-primary" ValidationGroup="save" Text="Start" />
                <asp:Button ID="btnEnd" OnClick="btnEnd_Click" runat="server" CssClass="btn btn-lg" ValidationGroup="save" Text="End" />
                <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-default" Text="Close" ToolTip="Close" OnClientClick="window.close();" />

            </div>
        </div>
        <div class="container-fluid">
            <div class="table-responsive">
                <table class="table table-small-font table-bordered table-striped margin_bottom01">
                    <tr align="center">
                        <td colspan="1" align="left">
                            <asp:Label ID="lblReg" runat="server" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblregNo" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true" />
                        </td>

                        <td colspan="1" align="left">
                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true" />
                        </td>

                        <td colspan="1" align="left">
                            <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" />
                        </td>
                        <td colspan="1" align="left">
                            <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" />

                            <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066" Font-Bold="true" />
                            <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:" SkinID="label" Font-Bold="true" />
                            <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row form-group">
                <div class="col-md-12 col-xs-12 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>


        <div class="container-fluid">
            <div class="row form-group">
                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="col-md-2">
                                    <asp:Label ID="lblService" runat="server" Text="Service"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <telerik:RadComboBox ID="ddlService" runat="server" Width="100%" Filter="Contains" OnSelectedIndexChanged="ddlService_OnSelectedIndexChanged" AutoPostBack="true"></telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-2">
                                    <asp:Label ID="lblDoctor" runat="server" Text="Doctor"></asp:Label>
                                    <asp:Label ID="lblStarDoctor" ForeColor="Red" runat="server" Text="*"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <telerik:RadComboBox ID="ddlDoctor" runat="server" Width="100%" Filter="Contains" MarkFirstMatch="true" AutoPostBack="false"></telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="col-md-4">
                                    <asp:Label ID="lblFrom" runat="server" Text="Start D/T"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <telerik:RadDateTimePicker ID="dtpfromdate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                        OnSelectedDateChanged="dtpfromdate_SelectedDateChanged" AutoPostBackControl="Both" Width="100%"
                                        DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <TimeView CellSpacing="-1"></TimeView>
                                        <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth="" AutoPostBack="True"></DateInput>
                                    </telerik:RadDateTimePicker>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="col-md-4">
                                    <asp:Label ID="Label1" runat="server" Text="End D/T"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <telerik:RadDateTimePicker ID="dtpTodate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                        OnSelectedDateChanged="dtpTodate_SelectedDateChanged" AutoPostBackControl="Both" DateInput-ReadOnly="false"
                                        DatePopupButton-Visible="false" ShowPopupOnFocus="true" Enabled="false">
                                        <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"></Calendar>
                                        <TimeView CellSpacing="-1"></TimeView>
                                        <TimePopupButton ImageUrl="" HoverImageUrl=""></TimePopupButton>
                                        <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                                        <DateInput DisplayDateFormat="dd/MM/yyyy HH:mm" DateFormat="dd/MM/yyyy HH:mm" LabelWidth="" AutoPostBack="True"></DateInput>
                                    </telerik:RadDateTimePicker>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="col-md-4">
                                    <asp:Label ID="lblPeriod" Text="Period" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="lblPeriodDescription" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="col-md-4">
                                    <asp:Label ID="lblUnit" Text="Unit(s)" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="txtUnit" runat="server" Width="100%"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="col-md-4">
                                    <asp:Label ID="Label2" runat="server" Text="Sr. Charge"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtCharge" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="col-md-4">
                                    <asp:Label ID="lblDrCharge" runat="server" Text="Dr. Charge"></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtDrCharge" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-2">
                                    <asp:Label ID="lblRemark" runat="server" Text="Remarks"></asp:Label>
                                </div>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtRemark" MaxLength="100" Width="100%" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <center>
                            <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False"
                                ShowFooter="false" OnRowDataBound="gvService_RowDataBound"
                                Width="98%" OnRowCommand="gvService_RowCommand"
                                HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px"
                                ItemStyle-CssClass="PaddingRightSpacing" HeaderStyle-CssClass="PaddingCenterSpacing">
                                <Columns>
                                    <asp:TemplateField HeaderText="Service Name" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' />
                                            <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                            <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderId")%>' />
                                            <asp:HiddenField ID="hdnStartDateTime" runat="server" Value='<%#Eval("StartDateTime")%>' />
                                            <asp:HiddenField ID="hdnEndDateTime" runat="server" Value='<%#Eval("EndDateTime")%>' />
                                            <asp:HiddenField ID="hdnServiceOrderDetailId" runat="server" Value='<%#Eval("ServiceOrderDetailId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:PRegistration, Provider %>" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start D/T" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDateTime" runat="server" SkinID="label" Text='<%#Eval("StartDT") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End D/T" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndDateTime" runat="server" SkinID="label" Text='<%#Eval("EndDT") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Is Stop" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsStop" runat="server" SkinID="label" Text='<%#Eval("IsStop") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderNo" runat="server" SkinID="label" Text='<%#Eval("OrderNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Units" HeaderStyle-Width="10px" ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitsCalculated" runat="server" SkinID="label" Text='<%#Eval("UnitsCalculated") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Period" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderPeriod" runat="server" SkinID="label" Text='<%#Eval("OrderPeriod") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" SkinID="label" Text='<%#Eval("Remarks") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Amt" HeaderStyle-Width="30px" ItemStyle-Width="30px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblServiceAmount" runat="server" SkinID="label"
                                                Text='<%#Eval("ServiceAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doctor Amt" HeaderStyle-Width="30px" ItemStyle-Width="30px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorAmount" runat="server" Enabled="false" SkinID="label"
                                                Text='<%#Eval("DoctorAmount","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payable Amt" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountPayable" runat="server" Enabled="false" SkinID="label"
                                                Text='<%#Eval("AmountPayable","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtndaDelete" runat="server" CommandName="Del" CausesValidation="false"
                                                CommandArgument='<%#Eval("Id")%>' ToolTip="DeActivate" ImageUrl="~/Images/DeleteRow.png" Visible="false" />
                                            <asp:ImageButton ID="ibtnSel" runat="server" CommandName="Sel" CausesValidation="false"
                                                CommandArgument='<%#Eval("Id")%>' ToolTip="Edit/Stop" ImageUrl="~/Images/edit.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </center>
                    </div>
                </div>
                <div class="row form-group">
                    <div class="col-md-12 col-xs-12">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:HiddenField ID="hdnDoctorRequired" runat="server" Value="" />
                                <asp:HiddenField ID="hdnChargeType" runat="server" Value="" />
                                <asp:HiddenField ID="hdnChargeId" runat="server" Value="" />
                                <asp:HiddenField ID="hdnOrderId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnServiceOrderDetailId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
