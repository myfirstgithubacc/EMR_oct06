<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="MRDFilesReturnRequest.aspx.cs" Inherits="MRD_MRDFilesStatus" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

    <script type="text/javascript">
        function showMenu(e, menu, requestId) {
            $get('<%=hdnGRequestId.ClientID%>').value = $get(requestId).value;
            var menu = $find(menu);
            menu.show(e);
        }
        function validateMaxLength() {
            var txt = $get('<%=txtRegNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more then 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-3 col-sm-3"><h2><asp:Label ID="lblTitle" runat="server" /></h2></div>
                <div class="col-md-7 col-sm-7 text-center"><asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" /></div>
                <div class="col-md-2 col-sm-2 text-right">
                    <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_OnClick" />
                </div>
            </div>
           
                <div class="row">
                    <div class="col-md-2 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4"><span class="textName"><asp:Label runat="server" Text="File Status" /></span></div>
                            <div class="col-md-8 col-sm-8 col-xs-8"><telerik:RadComboBox ID="ddlFileStatus" Width="100%" runat="server" EmptyMessage="[ Select ]" /></div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4"><span class="textName"><asp:Label ID="Label3" runat="server" Text="Patient Type" /></span></div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlPatientType" Width="100%" runat="server">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="" Selected="true" />
                                        <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                        <telerik:RadComboBoxItem Text="IPD" Value="I" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-xs-12">
                        <div class="row">
                            <div class="col-md-3 col-sm-3 col-xs-4 p-t-b-5"><asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" /></div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <div class="row p-t-b-5">
                                    <div class="col-md-6 col-sm-6 col-xs-5">
                                        <telerik:RadComboBox ID="ddlName" runat="server" AppendDataBoundItems="true"
                                            Width="100%" AutoPostBack="true" OnTextChanged="ddlName_OnTextChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Reg No" Value="RN" />
                                                <telerik:RadComboBoxItem Text="Patient Name" Value="NM" />
                                                <telerik:RadComboBoxItem Text="Encounter No." Value="EN" />
                                                <telerik:RadComboBoxItem Text="Mobile No." Value="MN" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-7">
                                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="30" Width="100%" CssClass="drapDrowHeight" />
                                        <asp:TextBox ID="txtRegNo" Width="100%" runat="server" Text="" MaxLength="13" CssClass="drapDrowHeight" Visible="false" onkeyup="return validateMaxLength();" />
                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-2 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4"><asp:Label ID="Label2" runat="server" Text="Date" /></div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlTime" runat="server" Width="100%" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Today" Value="Today" Selected="true" />
                                        <telerik:RadComboBoxItem Text="Last Week" Value="LastWeek" />
                                        <telerik:RadComboBoxItem Text="Last Two Weeks" Value="LastTwoWeeks" />
                                        <telerik:RadComboBoxItem Text="Last One Month" Value="LastOneMonth" />
                                        <telerik:RadComboBoxItem Text="Last Three Months" Value="LastThreeMonths" />
                                        <telerik:RadComboBoxItem Text="Last Year" Value="LastYear" />
                                        <telerik:RadComboBoxItem Text="Date Range" Value="DateRange" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row" id="tblDate" runat="server" visible="false">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <asp:Label ID="Label17" runat="server" Text="From" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <div class="row p-t-b-5">
                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                        <asp:Label ID="Label18" runat="server" Text="To" />
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                 
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <asp:Panel ID="Panel2" runat="server" Height="480px" Width="100%" ScrollBars="Auto"
                        BorderWidth="0px" BorderColor="LightBlue">
                        <asp:GridView ID="gvData" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="False"
                            Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false"
                            OnRowDataBound="gvData_RowDataBound" OnRowCommand="gvData_OnRowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText='Discharge Date' ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisChargeDate" runat="server" Text='<%#Eval("DisChargeDate")%>' />
                                        <asp:HiddenField ID="hdnRequestId" runat="server" Value='<%#Eval("RequestId")%>' />
                                        <asp:HiddenField ID="hdnIssueId" runat="server" Value='<%#Eval("IssueID")%>' />
                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        <asp:HiddenField ID="hdnStatusColor" runat="server" Value='<%#Eval("StatusColor")%>' />
                                        <asp:HiddenField ID="hdnStatusCode" runat="server" Value='<%#Eval("StatusCode")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Returned Date' ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReturnedDate" runat="server" Text='<%#Eval("ReturnedDate")%>' />
                                            </ItemTemplate>
                                </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText='Received Date' ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceivedDate" runat="server" Text='<%#Eval("ReceivedDate")%>' />
                                            </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' ItemStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' ItemStyle-Width="35px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, PatientName%>' ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Age / Gender' ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientAgeGender" runat="server" Text='<%#Eval("PatientAgeGender")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Department' ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentName" runat="server" Text='<%#Eval("DepartmentName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Requested By' ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedBy" runat="server" Text='<%#Eval("RequestedBy")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Ward No' ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWardNo" runat="server" Text='<%#Eval("WardNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Bed No' ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Returned By' ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReturnedBy" runat="server" Text='<%#Eval("ReturnedBy")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Received By' ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceivedBy" runat="server" Text='<%#Eval("ReceivedBy")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Return File Request" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblAck" runat="server" CommandName="RetrunFileRequest" Text="Return File Request"></asp:LinkButton>
                                        <telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true"
                                            Visible="false" EnableShadows="true" Width="100px" OnItemClick="menuStatus_ItemClick" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
                    </div>
           
                        <div id="dvConfirm" runat="server" visible="false" style="width: 400px; z-index: 200;
                            border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000;
                            border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute;
                            bottom: 0; height: 75px; left: 300px; top: 150px">
                            <table width="100%" cellspacing="2">
                                <tr>
                                    <td colspan="3" align="center">
                                        <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to Acknowledge?"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                    </td>
                                    <td align="center">
                                        <asp:Button ID="btnYes" CssClass="btn btn-primary"  runat="server" Text="Yes" OnClick="btnYes_OnClick" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" CssClass="btn btn-default" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" />
                                    </td>
                                    <td align="center">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <asp:Label ID="lblStatus1" runat="server" CssClass="LegendColor" BackColor="Violet"></asp:Label>&nbsp;&nbsp;
                        <asp:Label ID="lblStatus2" runat="server" Text="&nbsp;" Width="20px" Visible="false"></asp:Label>
                        <asp:Label ID="lblStatus3" runat="server" CssClass="LegendColor" BackColor="LightSteelBlue"></asp:Label>
                        <asp:Label ID="lblStatus4" runat="server" Text="&nbsp;" Width="20px" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>




            <table>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnGRequestId" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
