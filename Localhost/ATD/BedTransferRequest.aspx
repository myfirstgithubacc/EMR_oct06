<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="BedTransferRequest.aspx.cs" Inherits="ATD_BedTransferRequest" Title="Transfer Requisition" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function ValidateMaxLenth() {
            var txt = $get('<%=txtRemarks.ClientID%>');
            if (txt.value.length > 255) {
                alert("Text length should not be more then 255 characters.");
                txt.value = txt.value.substring(0, 255);
                txt.focus();
            }
        }
    </script>

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr class="clsheader">
            <td align="left" style="width: 50%; padding-left: 10px;">
                <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Transfer Requisition"
                    Font-Bold="true" />
            </td>
            <td align="center" style="font-size: 12px; width: 25%">
                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="overflow: scroll; height: 340px">
                <asp:GridView ID="gvBedTransferRequest" runat="server" SkinID="gridview" AutoGenerateColumns="false"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" OnClick="lnkSelect_OnClick"></asp:LinkButton>
                                <asp:HiddenField ID="hdnToward" runat="server" Value='<%#Eval("Toward") %>' />
                                <asp:HiddenField ID="hdnToBedCategory" runat="server" Value='<%#Eval("ToBillingCategory") %>' />
                                <asp:HiddenField ID="hdnToBillingCat" runat="server" Value='<%#Eval("ToBedCategory") %>' />
                                <asp:HiddenField ID="hdnToBedNo" runat="server" Value='<%#Eval("ToBedNo") %>' />
                                <asp:HiddenField ID="hdnTowardId" runat="server" Value='<%#Eval("ToWardId") %>' />
                                <asp:HiddenField ID="hdnToBedCategoryId" runat="server" Value='<%#Eval("ToBedCategoryId") %>' />
                                <asp:HiddenField ID="hdnToBedNoId" runat="server" Value='<%#Eval("ToBedId") %>' />
                                <asp:HiddenField ID="hdnFromBillingCategory" runat="server" Value='<%#Eval("FromBillingCategory") %>' />
                                <asp:HiddenField ID="hdnFromBillingCategoryId" runat="server" Value='<%#Eval("FromBillingCategoryId") %>' />
                                <asp:HiddenField ID="hdnToBillingCategory" runat="server" Value='<%#Eval("ToBillingCategory") %>' />
                                <asp:HiddenField ID="hdnToBillingCategoryId" runat="server" Value='<%#Eval("ToBillingCategoryId") %>' />
                                <asp:HiddenField ID="hdnFromBedCategoryId" runat="server" Value='<%#Eval("FromBedCategoryId") %>' />
                                <asp:HiddenField ID="hdnFromBedId" runat="server" Value='<%#Eval("FromBedId") %>' />
                                <asp:HiddenField ID="hdnFromWardId" runat="server" Value='<%#Eval("FromWardId") %>' />
                                <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                <asp:HiddenField ID="hdnTransferRequistionId" runat="server" Value='<%#Eval("id") %>' />
                                <asp:HiddenField ID="hdnrequestDate" runat="server" Value='<%#Eval("requestDate") %>' />
                                <asp:HiddenField ID="hdnCode" runat="server" Value='<%#Eval("Code") %>' />
                                <asp:HiddenField ID="hdnTemporaryBed" runat="server" Value='<%#Eval("TemporaryBed") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Reg# No." DataField="RegistrationNo" />
                        <asp:BoundField HeaderText="IP No." DataField="EncounterNo" />
                        <asp:BoundField HeaderText="Name" DataField="PatientName" />
                        <asp:BoundField HeaderText="Age/Gender" DataField="AgeGender" />
                        <asp:BoundField HeaderText="From Billing Category" DataField="FromBillingCategory" />
                        <asp:BoundField HeaderText="From Bed Category" DataField="FromBedCategory" />
                        <asp:BoundField HeaderText="From Ward" DataField="Fromward" />
                        <asp:BoundField HeaderText="To Ward" DataField="Toward" Visible="true" />
                        <asp:BoundField HeaderText="From Bed No" DataField="FromBedNo" />
                        <asp:BoundField HeaderText="Request Remarks" DataField="RequestRemarks"  />
                        <asp:BoundField HeaderText="Current Bed Status" DataField="Bedstatus" />
                        <asp:BoundField HeaderText="Transfer Request Date" DataField="RequestDt" />
                    </Columns>
                </asp:GridView>
            </div>
            <table width="100%">
                <tr>
                    <td colspan="2" class="txt06" align="left">
                        To Bed Detail
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ltrltoward" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, ward %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbltowardname" runat="server" SkinID="label" Width="200px"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblTransferdate" runat="server" SkinID="label" Text="Transfer Request Date"></asp:Label>
                        <telerik:raddatetimepicker id="dtpTransferDate" runat="server" mindate="01/01/1999"
                            dateinput-dateformat="dd/MM/yyyy HH:mm" dateinput-displaydateformat="dd/MM/yyyy HH:mm" Enabled ="false">
                                            </telerik:raddatetimepicker>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" SkinID="label" Text="Actual Transfer Date"></asp:Label>
                        <telerik:raddatetimepicker id="dtpActualTransferDate" runat="server" mindate="01/01/1999"
                            dateinput-dateformat="dd/MM/yyyy HH:mm" dateinput-displaydateformat="dd/MM/yyyy HH:mm">
                                            </telerik:raddatetimepicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bedcategory %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbedcategory" runat="server" SkinID="DropDown" Width="200px"
                            Enabled="false">
                        </asp:DropDownList>
                    </td>
                    <td rowspan="3">
                        <asp:Label ID="lblInfoRemarks" runat="server" Text="Notes*" SkinID="label"></asp:Label>
                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" SkinID="textbox"
                            Width="250px" Height="70px" onkeyup="return ValidateMaxLenth();"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ltrltobedno" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bedno %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVacantBed" runat="server" SkinID="DropDown" Width="200px"
                            Enabled="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="ltrltobillingcategory" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, billingcategory %>"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBillingCat" runat="server" SkinID="DropDown" Width="200px"
                            >
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Button ID="btnTransfer" runat="server" OnClick="btnTransfer_OnClick" SkinID="Button"
                            Text="Transfer" />
                        <asp:Button ID="BtnCancelRequest" runat="server" OnClick="BtnCancelRequest_Click"
                            SkinID="Button" Text="Cancel " />
                        <asp:Button ID="BtnClear" runat="server" OnClick="BtnClear_OnClick" SkinID="Button"
                            Text="Clear" />
                        <ajax:ConfirmButtonExtender ID="cbCancel" runat="server" ConfirmOnFormSubmit="true"
                            ConfirmText="Are you sure that you want to cancel ? " TargetControlID="BtnCancelRequest" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvBedTransferRequest" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
