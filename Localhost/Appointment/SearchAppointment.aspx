<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchAppointment.aspx.cs"
    Inherits="Appointment_SearchAppointment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appointment List</title>
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />

    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" />

    <style type="text/css">
        .FacilityText {
            /* float: left; */
            margin: 0px;
            /* padding: 2px 0px 5px 22px; */
            width: 100% !important;
            font-size: 12px !important;
            font-weight: 600;
        }

        td.rcbInputCell.rcbInputCellLeft .rcbInput {
            padding: 2px 8px !important;
        }
        td{
            background:none!important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript">
        function showMenu(e, menu) {
            var menu = $find(menu);
            menu.show(e);
        }

        function OnClientClose(oWnd) {
            $get('<%=btnRefresh.ClientID%>').click();
        }
        
    </script>

    

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                
                <!-- Search Appointment Part -->


                <div class="container-fluid header_main margin_bottom" style="padding-right: 15px!important; padding-left: 15px!important; z-index:99999;">
                    <div class="row">


                        <div class="col-md-3">
                            <h2>
                                <asp:Label ID="Label8" runat="server" SkinID="ListDetailsText" Text="Search Appointment" />
                            </h2>
                        </div>

                        <div class="col-md-5 text-center">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </div>

                        <div class="col-md-4 text-right ">

                            <asp:Button ID="btnRefresh" runat="server" CausesValidation="true" OnClick="btnRefresh_OnClick" Text="Refresh" CssClass="btn btn-primary" />
                            <asp:Button ID="btnResetFilter" runat="server" CausesValidation="false" OnClick="btnResetFilter_OnClick" Text="Reset Filter" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" OnClientClick="window.close();" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>

                <!-- Search Appointment Part -->

                <div class="row form-group" style="margin-right: 0px!important;">

                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-4 col-12">
                                <asp:Label ID="lblLocation" runat="server" CssClass="FacilityText" Text="Facility" />
                            </div>
                            <div class="col-md-8 col-12">
                                <telerik:RadComboBox ID="ddlLocation" runat="server" Width="100%" AutoPostBack="false" Skin="Office2010Black" />
                            </div>
                        </div>

                    </div>

                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-4 col-12">
                                <asp:Label ID="lblPatient" runat="server" CssClass="FacilityText" Text="Search&nbsp;On" />
                            </div>
                            <div class="col-md-4 col-6">
                                <telerik:RadComboBox ID="ddlName" runat="server" Skin="Office2010Black" AppendDataBoundItems="true"
                                    Width="100%" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlName_OnTextChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Reg No" Value="R" />
                                        <%--<telerik:RadComboBoxItem Text="Encounter No." Value="ENC" />--%>
                                        <telerik:RadComboBoxItem Text="Old Reg No" Value="O" />
                                        <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                        <telerik:RadComboBoxItem Text="Enrolle No." Value="EN" />
                                        <telerik:RadComboBoxItem Text="Mobile No." Value="MN" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                            <div class="col-md-4 col-6">
                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnRefresh">
                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" SkinID="textbox" Visible="false" />
                                    <asp:TextBox ID="txtSearchN" CssClass="searchOnInput01" Width="100%" runat="server" Text="" MaxLength="10" onkeyup="return validateMaxLength();" />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                </asp:Panel>
                            </div>

                        </div>
                    </div>

                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-3 col-12">
                                <asp:Label ID="lblDate" runat="server" CssClass="FacilityText" Text="Date" />
                            </div>
                            <div class="col-md-8 col-12">
                                <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True" Skin="Office2010Black" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged" Width="100%" CausesValidation="false" />
                            </div>
                        </div>
                    
                    </div>




                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-4 col-12">
                                <asp:Label ID="Label2" runat="server" CssClass="FacilityText" Text='<%$ Resources:PRegistration, specialisation%>' />
                            </div>
                            <div class="col-md-8 col-12">
                                <telerik:RadComboBox ID="ddlSpecilization" Skin="Office2010Black" runat="server" AppendDataBoundItems="true" Filter="Contains" Width="100%" DropDownWidth="280px" AutoPostBack="true" OnSelectedIndexChanged="ddlSpecilization_SelectedIndexChanged" />
                            </div>
                        </div>
                    </div>



                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-4 col-12">
                                <asp:Label ID="lblProvider" runat="server" CssClass="FacilityText" Text='<%$ Resources:PRegistration, Doctor%>' />
                            </div>
                            <div class="col-md-8 col-12">
                                <telerik:RadComboBox ID="ddlProvider" Skin="Office2010Black" MarkFirstMatch="true" Filter="Contains" runat="server" ItemsPerRequest="10" EnableVirtualScrolling="true" TabIndex="0" AutoPostBack="false" Width="100%" DropDownWidth="300px" />
                            </div>
                        </div>


                    </div>




                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-3 col-12">
                                <asp:Label ID="lblAppointmentStatus" runat="server" CssClass="FacilityText" Text="Status" />
                            </div>
                            <div class="col-md-8 col-12">
                                <telerik:RadComboBox ID="ddlAppointmentStatus" runat="server" Width="100%" AutoPostBack="false" ShowMoreResultsBox="false" AppendDataBoundItems="true" Skin="Office2010Black">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="0" Text="All" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-12" id="tblDateRange" runat="server">
                    <div class="row">
                        <div class="col-md-5 col-5">
                            <telerik:RadDatePicker ID="dtpfromDate" runat="server" CssClass="FacilityText" Width="100%" />

                        </div>
                        <div class="col-md-1 col-2">
                            <p>To</p>
                        </div>
                        <div class="col-md-5 col-5">
                            <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" />
                        </div>
                    </div>
                </div>

                <div class="container-fluid form-group">
                    <div class="row">

                        <div class="col-md-12">
                            <asp:Label ID="lblGridStatus" runat="server" Font-Size="12px" Font-Bold="true" />
                        </div>
                    </div>
                </div>








                <div class="GeneralDiv">
    	            <div class="container-fluid">
                        <div class="row">
                        
    	                    <div class="col-md-12">
    	                    
                                <table id="Table1" cellpadding="0" cellspacing="0" runat="server" border="0" class="tableRecurrence">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlgrid" runat="server"  Width="100%" BorderWidth="1" BorderColor="SkyBlue" ScrollBars="Auto">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvAppointmentList" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            Height="100%" SkinID="gridviewOrder" AllowPaging="True" PageSize="15" OnRowCreated="gvAppointmentList_OnRowCreated"
                                                            OnPageIndexChanging="gvAppointmentList_PageIndexChanging" OnRowCommand="gvAppointmentList_OnRowCommand"
                                                            OnRowDataBound="gvAppointmentList_RowDataBound">
                                                            
                                                            
                                                            
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="60px" HeaderStyle-Height="25px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Appointment&nbsp;Date" HeaderStyle-Width="110px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAppointmentDate" runat="server" Text='<%#Eval("AppointmentDate")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' HeaderStyle-Width="90px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblregistrationno" runat="server" Text='<%#Eval("registrationno")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="AgeGender" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Facility" HeaderStyle-Width="110px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Doctor%>' HeaderStyle-Width="200px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>' />
                                                                        <asp:HiddenField ID="hdnEncounterStatus" runat="server" Value='<% #Eval("EncounterStaus") %>' />
                                                                        <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%#Eval("PackageId") %>' />
                                                                        <asp:HiddenField ID="hdnPackageName" runat="server" Value='<%#Eval("PackageName") %>' />
                                                                        <asp:HiddenField ID="hdnVisitType" runat="server" Value='<%#Eval("VisitType") %>' />
                                                                        <asp:HiddenField ID="hdnStatusCode" runat="server" Value='<%#Eval("StatusCode") %>' />
                                                                        <asp:HiddenField ID="hdnMedicalAlert" runat="server" Value='<%#Eval("MedicalAlert")%>' />
                                                                        <asp:HiddenField ID="hdnAllergiesAlert" runat="server" Value='<%#Eval("AllergiesAlert")%>' />
                                                                        <asp:HiddenField ID="hdnRoomNo" runat="server" Value='<%#Eval("RoomNo")%>' />
                                                                        <asp:HiddenField ID="hdnAppointmentID" runat="server" Value='<%#Eval("AppointmentID")%>' />
                                                                        <asp:HiddenField ID="hdnDoctorID" runat="server" Value='<%#Eval("DoctorID")%>' />
                                                                        <asp:HiddenField ID="hdnStatusId" runat="server" Value='<%#Eval("StatusId")%>' />
                                                                        <asp:HiddenField ID="hdnGender" runat="server" Value='<%#Eval("Gender")%>' />
                                                                        <asp:HiddenField ID="hdnRoomID" runat="server" Value='<%#Eval("RoomID")%>' />
                                                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                                        <asp:HiddenField ID="hdnFacilityID" runat="server" Value='<%#Eval("FacilityID")%>' />
                                                                        <asp:HiddenField ID="hdnStatusColor" runat="server" Value='<%#Eval("StatusColor")%>' />
                                                                        <asp:HiddenField ID="hdnVisitTypeId" runat="server" Value='<%#Eval("VisitTypeId")%>' />
                                                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                                        <asp:HiddenField ID="hdnRecurrenceParentID" runat="server" Value='<%#Eval("RecurrenceParentID")%>' />
                                                                        <asp:HiddenField ID="hdnFromTime" runat="server" Value='<%#Eval("FromTime")%>' />
                                                                        <asp:HiddenField ID="hdnToTime" runat="server" Value='<%#Eval("ToTime")%>' />
                                                                        <asp:HiddenField ID="hdnPatientDOB" runat="server" Value='<%#Eval("PatientDOB")%>' />
                                                                        <asp:HiddenField ID="hdnDuration" runat="server" Value='<%#Eval("Duration")%>' />
                                                                        <asp:HiddenField ID="hdnPackageType" runat="server" Value='<%#Eval("PackageType")%>' />
                                                                        <asp:HiddenField ID="hdnRecurrenceRule" runat="server" Value='<%#Eval("RecurrenceRule")%>' />
                                                                        <asp:HiddenField ID="hdnNoPAForInsurancePatient" runat="server" Value='<%#Eval("NoPAForInsurancePatient")%>' />
                                                                        <asp:HiddenField ID="hdnNoBillForPrivatePatient" runat="server" Value='<%#Eval("NoBillForPrivatePatient")%>' />
                                                                        <asp:HiddenField ID="hdnIsDoctor" runat="server" Value='<%#Eval("IsDoctor")%>' />
                                                                        <asp:HiddenField ID="hdnShiftFirstFrom" runat="server" Value='<%#Eval("ShiftFirstFrom")%>' />
                                                                        <asp:HiddenField ID="hdnShiftFirstTo" runat="server" Value='<%#Eval("ShiftFirstTo")%>' />
                                                                        <asp:HiddenField ID="hdnTimeTaken" runat="server" Value='<%#Eval("TimeTaken")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderText="Company Type" HeaderStyle-Width="120px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAccountCategory" runat="server" Text='<%#Eval("AccountCategory")%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                                <asp:TemplateField HeaderText="Options" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnCategory" runat="server" ImageUrl="../Images/icon/mynewoption.png" Width="22px" />
                                                                        <%--<telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true" EnableShadows="true" Width="100px" DataTextField="Status" DataValueField="Code" AppendDataBoundItems="true" OnItemClick="menuStatus_ItemClick" />--%>
                                                                        <telerik:RadContextMenu ID="menuStatus" runat="server" EnableRoundedCorners="true" EnableShadows="true" Width="100px" OnItemClick="menuStatus_ItemClick" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>

                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvAppointmentList" />
                                                    </Triggers>

                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                
                
                            </div>
                        </div>
                    </div>
                </div>
                
                
                
                
                
                
                
                <table>
                    <tr>
                        <td>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close" InitialBehaviors="Maximize"></telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </td>
                    </tr>
                </table>
                
                
                <div id="dvDelete" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 150px; left: 450px; top: 300px">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <table>

                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblDeleteApp" runat="server" Text="Delete Appointment ?"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblDeleteEncounterMessage" runat="server" ForeColor="Red"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Reason : "></asp:Label><font color='Red'>*</font></td>
                                    <td>
                                        <asp:DropDownList ID="ddlRemarkss" runat="server" AppendDataBoundItems="true" Width="200px" SkinID="DropDown"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Remarks :"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtCancel" runat="server" SkinID="textbox" TextMode="MultiLine" Style="max-height: 70px; min-height: 70px; max-width: 200px; min-width: 200px;"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnDeleteApp" SkinID="Button" runat="server" Text="Cancel" OnClick="btnDeleteApp_Click" />
                                        <asp:Button ID="btnCancelApp" SkinID="Button" runat="server" Text="Close" OnClick="btnCancelApp_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                
                
                <div id="dvDeleteRecurring" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 150px; left: 450px; top: 300px">
                    
                    <asp:UpdatePanel ID="updDelete" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lblDeleteMessage" runat="server" Text="Delete ?"></asp:Label></td>
                                </tr>
                                
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblRecurringDeleteMessage" runat="server" ForeColor="Red"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Reason : "></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlRemarks" runat="server" AppendDataBoundItems="true" SkinID="DropDown"></asp:DropDownList></td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Remarks :"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtRecurRemark" runat="server" SkinID="textbox" TextMode="MultiLine" Style="max-height: 70px; min-height: 70px; max-width: 200px; min-width: 200px;"></asp:TextBox></td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDeleteAllApp" SkinID="Button" runat="server" Text="Cancel All" OnClick="btnDeleteAllApp_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnDeleteThisApp" SkinID="Button" runat="server" Text="Cancel This" OnClick="btnDeleteThisApp_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnCancelRecurrApp" SkinID="Button" runat="server" Text="Close" OnClick="btnCancelRecurrApp_OnClick" /></td>
                                </tr>
                                
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    
    </form>
</body>
</html>
