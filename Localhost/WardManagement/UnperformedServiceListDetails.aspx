<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnperformedServiceListDetails.aspx.cs"
    Inherits="WardManagement_UnperformedServiceListDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Unperformed Services</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        tr.clsGridheaderorderNew th {
            color: #fff !important;
            background: #337ab7 !important;
            padding: 6px 10px !important;
            white-space: nowrap !important;
        }

        tr.clsGridRoworderNew td {
            padding: 6px 10px !important;
            white-space: nowrap !important;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server" defaultbutton="btnFilter" style="overflow-x: hidden;">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <script type="text/javascript" language="javascript">
            function validateMaxLength() {
                var txt = $get('<%=txtSearchRegNo.ClientID%>');
                if (txt.value > 9223372036854775807) {
                    alert("Value should not be more then 9223372036854775807.");
                    txt.value = txt.value.substring(0, 12);
                    txt.focus();
                }
            }
            function OnCollectClientClose(oWnd) {
                $get('<%=btnFilter.ClientID%>').click();
            }
        </script>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="container-fluid header_main form-group pl-2 pr-2">
                    <div class="row">
                        <div class="col-md-4">
                            <h2>
                                <asp:Label ID="Label1" runat="server" ForeColor="Navy" Text="No of Patient " />
                                <asp:Label ID="lblNoOfPatient" runat="server" ForeColor="DarkRed" Text="" />
                                &nbsp;
                            &nbsp;
                            <asp:Label ID="lblRecord" runat="server" ForeColor="Red" Text="No of Total Record" />
                                <asp:Label ID="lbitotalRecord" runat="server" ForeColor="DarkRed" Text="" />
                            </h2>
                        </div>
                        <div class="col-md-6 text-center">
                            <asp:Label ID="lblMessage" runat="server" Text="" />
                        </div>
                        <div class="col-md-2 text-right">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                        </div>
                    </div>
                </div>

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-4 col-sm-6">
                            <div class="row">
                                <div class="col-md-7 col-6">
                                    <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" Width="100%" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Ward" Value="W" Selected="true" />
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, UHID%>' Value="R" />
                                            <telerik:RadComboBoxItem Text="Encounter No" Value="ENC" />
                                            <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                                <div class="col-md-5 col-6 PaddingLeftSpacing">
                                    <asp:TextBox ID="txtSearchRegNo" Width="100%" runat="server" Text="" MaxLength="13" onkeyup="return validateMaxLength();" Visible="false" />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchRegNo" ValidChars="0123456789" />
                                    <asp:TextBox ID="txtSearch" runat="server" Width="100%" MaxLength="30" Visible="false" />
                                    <telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" Height="350px" Filter="Contains" EmptyMessage="[ All Selected ]" />
                                </div>
                            </div>

                        </div>
                        <div class="col-md-4 col-sm-6">
                            <div class="row">
                                <div class="col-md-2 col-sm-3 col-2 label2">
                                    <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                </div>
                                <div class="col-md-10 col-sm-9 col-10">
                                    <div class="row">
                                        <div class="col-md-6 col-6">
                                            <telerik:RadDatePicker ID="dtpDatefrom" runat="server" MinDate="01/01/1990" Width="100%" TabIndex="5" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                        </div>
                                        <div class="col-md-6 col-6">
                                            <telerik:RadDatePicker ID="dtpDateto" runat="server" MinDate="01/01/1990" Width="100%" TabIndex="5" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 text-right mb-2 mt-2 m-md-0">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                            <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" CssClass="btn btn-primary" OnClick="btnClearFilter_Click" />
                        </div>
                        
                    </div>
                </div>

                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" Skin="Office2007" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close" />
                    </Windows>
               </telerik:RadWindowManager>

                <div class="container-fluid form-groupTop" id="tblunperformesServices" runat="server">
                    <div class="row">
                        <div class="col-12 table-responsive">
                            <asp:GridView ID="gvUnacknowledgedServices" OnPageIndexChanging="gvUnacknowledgedServices_OnPageIndexChanging"
                                AllowPaging="true" PageSize="15" runat="server" SkinID="gridviewOrderNew" Width="100%"
                                AutoGenerateColumns="false"
                                OnRowDataBound="gvUnacknowledgedServices_OnRowDataBound"
                                OnRowCommand="gvUnacknowledgedServices_RowCommand">
                                <EmptyDataTemplate>
                                    <p style="color: Red;">
                                        No Record(s) Found
                                    </p>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='<%$ Resources:PRegistration, UHID%>'
                                        ItemStyle-Width="85px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRegistrationNo" runat="server" CommandArgument='<%#Eval("RegistrationNo")%>' CommandName="Collect" Text='<%#Eval("RegistrationNo")%>'></asp:LinkButton>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Patient Name" ItemStyle-Width="160px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientname" runat="server" Text='<%#Eval("PatientName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encounter No" ItemStyle-Width="75px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Ward Name/BedNo" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardBed")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Bed No." ItemStyle-Width="60px" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order No" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderNo" runat="server" Text='<%#Eval("OrderNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Date" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%#Eval("OrderDate")%>' />
                                        <asp:HiddenField ID="hdnEncounterDate" runat="server" Value='<%#Eval("EncounterDate") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Lab No" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLabNo" runat="server" Text='<%#Eval("LabNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Service Name" ItemStyle-Width="200px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                        <asp:HiddenField ID="hdnLabSampleNotes" runat="server"  Value='<%# Eval("LabSampleNotes") %>'/>
                                          <asp:ImageButton ID="ibtnForNotes" runat="server" ImageUrl="~/Images/NotesNew.png"
                                                    ToolTip="Click to show patient notes." Visible="false" CommandName="sel" OnClick="ibtnForNotes_Click" CommandArgument='<%#Eval("LabNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Sub Department" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubDepartment" runat="server" Text='<%#Eval("SubDepartment")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Sample Status" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSampleStatus" runat="server" Text='<%#Eval("Status")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Date to be Performed" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateToBePerformed" runat="server" Text='<%#Eval("DateToBePerformed")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Remarks" ItemStyle-Width="250px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label runat="server" ID="lblFutureDate" BackColor="Aqua" Text="Future Date"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="container-fluid" id="tbldrugorder" runat="server">
                    <div class="row form-group">
                        <div class="col-12 table-responsive">
                            <asp:GridView ID="gvIPPharmacyStore" OnPageIndexChanging="gvIPPharmacyStore_OnPageIndexChanging"
                                AllowPaging="true" PageSize="8" runat="server" SkinID="gridviewOrderNew" Width="100%"
                                AutoGenerateColumns="false" OnRowCommand="gvIPPharmacyStore_RowCommand">
                                <EmptyDataTemplate>
                                    <p style="color: Red;">
                                        No Record(s) Found
                                    </p>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='<%$ Resources:PRegistration, UHID%>'
                                        ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Patient Name" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encounter No" ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncounterno" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Ward Name" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Bed No." ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Department Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepartment" runat="server" Text='<%#Eval("DepartmentName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Indent No" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndentNo" runat="server" Text='<%#Eval("IndentNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Indent Date" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndentDate" runat="server" Text='<%#Eval("IndentDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Type" ItemStyle-Width="95px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderType" runat="server" Text='<%#Eval("OrderType")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Pending Satus"
                                        ItemStyle-Width="105px">
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblStatus" runat="server" Text='<%#Eval("PendingStatus")%>'>'></asp:Label>--%>
                                            <asp:Label ID="lblstat" runat="server" Text='<%#Eval("PendingSatus") %>'>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Requested By" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestedBy" runat="server" Text='<%#Eval("RequestedBy")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="View Details" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDetails" CommandName="Details" CommandArgument='<%#Eval("IndentId")%>' runat="server">Select</asp:LinkButton>
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>


                <div class="container-fluid" id="tblDrugOrderDetails" runat="server" visible="false">
                    
                        <div class="row form-group">
                            <div class="col-md-12"><strong>Drug Order Details</strong></div>
                        </div>

                    <div class="row form-group">
                        <div class="col-12 table-responsive">
                            <asp:GridView ID="gvDetails" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="False"
                                Height="100%" Width="100%" CellPadding="0" CellSpacing="0" OnRowDataBound="gvDetails_RowDataBound">
                                <Columns>

                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="10px" ItemStyle-Width="10px"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indent No" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIndentno" runat="server" Width="100%" Text='<%# Eval("IndentNo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Generic Name" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGenericName" runat="server" Width="100%" Text='<%# Eval("GenericName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Item Name" ItemStyle-Width="300px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblItemName" runat="server" Width="100%" Text='<%# Eval("ItemName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Substitute Allowed" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnQuantity" runat="server" Value='<%#Eval("IssueQty")%>' />
                                            <asp:Label ID="lblSubstituteAllowed" runat="server" Text='<%#Eval("SubstituteAlloweded")%>'
                                                Width="80px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrescriptionDetail" runat="server" Text='<%#Eval("Remarks") %>'
                                                Width="300px"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="row">
                        <asp:Label ID="lblIssuedColor" runat="server" BorderWidth="1px" BackColor="DarkSeaGreen" SkinID="label" Width="22px" Height="14px" />
                        <asp:Label ID="Label11" runat="server" SkinID="label" Text="Issued" />

                        <asp:Label ID="lblPendingColor" runat="server" BorderWidth="1px" BackColor="#FFFBC7" SkinID="label" Width="22px" Height="14px" />
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Pending" />
                    </div>

                </div>


                <div class="container-fluid" id="tblnondrugorder" runat="server">
                    <div class="row form-group">
                        <div class="col-12 table-responsive">
                            <asp:GridView ID="gvNonDrugOrder" OnPageIndexChanging="gvNonDrugOrder_OnPageIndexChanging"
                                AllowPaging="true" PageSize="15" runat="server" SkinID="gridviewOrderNew" Width="100%"
                                AutoGenerateColumns="false">
                                <EmptyDataTemplate>
                                    <p style="color: Red;">
                                        No Record(s) Found
                                    </p>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='<%$ Resources:PRegistration, UHID%>'
                                        ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Patient Name" ItemStyle-Width="160px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encounter No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNonDrugOrderDate" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Ward Name" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Bed No." ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Date" ItemStyle-Width="130px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNonDrugOrderDate" runat="server" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Prescription">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrescription" runat="server" Text='<%#Eval("Prescription")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Type" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderTypeName" runat="server" Text='<%#Eval("OrderTypeName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Doctor Name" ItemStyle-Width="170px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                </div>
                <div class="container-fluid" id="divStopMedication" runat="server">
                    <div class="row form-group">
                        <div class="col-12 table-responsive">
                            <asp:GridView ID="gvStopMedication" OnPageIndexChanging="gvStopMedication_OnPageIndexChanging"
                                AllowPaging="true" PageSize="15" runat="server" SkinID="gridviewOrderNew" Width="100%"
                                AutoGenerateColumns="false">
                                <EmptyDataTemplate>
                                    <p style="color: Red;">
                                        No Record(s) Found
                                    </p>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='<%$ Resources:PRegistration, UHID%>'
                                        ItemStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Patient Name" ItemStyle-Width="160px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encounter No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNonDrugOrderDate" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Ward Name" ItemStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Bed No." ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Order Date" ItemStyle-Width="130px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNonDrugOrderDate" runat="server" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Prescription">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrescription" runat="server" Text='<%#Eval("Prescription")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Presc. Type" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderTypeName" runat="server" Text='<%#Eval("OrderTypeName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Stop Date&Time" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStopDateTime" runat="server" Text='<%#Eval("StopDateTime")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Stop By" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStopBy" runat="server" Text='<%#Eval("StopBy")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Doctor Name" ItemStyle-Width="170px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
</body>
</html>