<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchResourceAppointment.aspx.cs"  MasterPageFile="~/Include/Master/EMRMaster.master" Inherits="Appointment_ResourceAppointment_SearchResourceAppointment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />  
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />


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
                    
                    <div class="container-fluid header_main form-group">
                        <div class="col-md-3"><h2 style="color:#333;"><asp:Label ID="Label8" runat="server" Text="Search Resource Appointment" /></h2></div>
                        <div class="col-md-6 text-center"><asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" /></div>
                        <div class="col-md-3 text-right">
                            <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary" CausesValidation="false" Text="Export" OnClick="btnExport_OnClick" />
                            <asp:Button ID="btnCloseW" Text="Close" Visible="false" runat="server" ToolTip="Close" CssClass="btn btn-default" OnClientClick="window.close();" />
                        </div>
                    </div>




                    <div class="container-fluid">
                        <div class="row form-groupTop01">
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="lblLocation" runat="server" Text="Facility" /></div>
                                    <div class="col-md-8"><telerik:RadComboBox ID="ddlLocation" runat="server" Width="100%" AutoPostBack="false" Enabled="false" /></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" /></div>
                                    <div class="col-md-8">
                                        <div class="row">
                                            <div class="col-md-7">
                                                <telerik:RadComboBox ID="ddlName" runat="server" AppendDataBoundItems="true"
                                                    Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="Reg No" Value="R" />
                                                        <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                                        <telerik:RadComboBoxItem Text="Mobile No." Value="MN" />
                                                        <telerik:RadComboBoxItem Text="IP No." Value="IP" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                            <div class="col-md-5 PaddingLeftSpacing">
                                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnRefresh">
                                                    <asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="100%" Visible="false" />
                                                    <asp:TextBox ID="txtSearchN" Width="100%" runat="server" Text="" MaxLength="13" onkeyup="return validateMaxLength();" />
                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="Label1" runat="server" Text="Ward" /></div>
                                    <div class="col-md-8"><telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" Height="300px" DropDownWidth="250px" EmptyMessage="[ Select ]" Filter="Contains" /></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"></div>
                                    <div class="col-md-8">
                                        <div class="PD-TabRadioNew01 margin_z">
                                            <asp:RadioButtonList ID="rdbActive" AutoPostBack="true" OnSelectedIndexChanged="rdbActive_SelectedIndexChanged" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Active" Selected="True" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                           
                        </div>

                        <div class="row form-groupTop01">
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2 PaddingRightSpacing"><span class="textName"><asp:Label ID="Label2" runat="server" Text="Sub Department" /></span></div>
                                    <div class="col-md-8"><telerik:RadComboBox ID="ddlSubDepartment" runat="server" AppendDataBoundItems="true"
                                    Filter="Contains" Height="300px" Width="100%" DropDownWidth="280px" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSubDepartment_SelectedIndexChanged" /></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="Label5" runat="server" Text="Resource&nbsp;Name" /></div>
                                    <div class="col-md-8"><telerik:RadComboBox ID="ddlResource" runat="server" CheckBoxes="true" Width="100%" EnableCheckAllItemsCheckBox="true" /></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="lblAppointmentStatus" runat="server" Text="Status" /></div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlAppointmentStatus" runat="server" Width="100%" AutoPostBack="false"
                                            ShowMoreResultsBox="false" AppendDataBoundItems="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="0" Text="All" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3" style="display:none;">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="lblSource" runat="server" Text="Source" /></div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlSource" runat="server" AppendDataBoundItems="true" Width="100%" AutoPostBack="true">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="All" Value="A" Selected="true" />
                                                <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                                <telerik:RadComboBoxItem Selected="true" Text="IPD" Value="I" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row form-groupTop01">
                             <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"><asp:Label ID="lblDate" runat="server" Text="Date" /></div>
                                    <div class="col-md-8">
                                        <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged" Width="100%" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>
                              <div class="col-md-3">
                                <div class="row" id="tblDateRange" runat="server">
                                    <div class="col-md-5 PaddingRightSpacing"><telerik:RadDatePicker ID="dtpfromDate"  MinDate="01/01/1900" DisplayDateFormat="dd/MM/yyyy"
                                                        DateFormat="MM/dd/yyyy" ForeColor="Black" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                    <div class="col-md-2 label2">To</div>
                                    <div class="col-md-5 PaddingLeftSpacing"><telerik:RadDatePicker ID="dtpToDate"  MinDate="01/01/1900"  DisplayDateFormat="dd/MM/yyyy"
                                                        DateFormat="MM/dd/yyyy" ForeColor="Black" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="row">
                                    <div class="col-md-4 label2"></div>
                                    <div class="col-md-8"></div>
                                </div>
                            </div>
                            <div class="col-md-3 text-right">
                                <asp:Button ID="btnRefresh" runat="server" CausesValidation="true" CssClass="btn btn-primary" OnClick="btnRefresh_OnClick" Text="Refresh" />
                                <asp:Button ID="btnResetFilter" runat="server" CausesValidation="false" CssClass="btn btn-default" OnClick="btnResetFilter_OnClick" Text="Reset Filter" />
                            </div>
                        </div>


                    </div>


                    <div class="container-fluid">
                        <div class="row form-group">
                            <div class="col-md-12"><asp:Label ID="lblGridStatus" runat="server" Font-Bold="true" /></div>
                        </div>

                        <div class="row form-group">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlgrid" runat="server" Height="490px" Width="100%" BorderWidth="1"
                                        BorderColor="SkyBlue" ScrollBars="Both">
                                        <asp:GridView ID="gvAppointmentList" runat="server" AutoGenerateColumns="False" Width="2700px"
                                            Height="100%" SkinID="gridviewOrderNew" AllowPaging="True" PageSize="25" OnRowCreated="gvAppointmentList_OnRowCreated"
                                            OnPageIndexChanging="gvAppointmentList_PageIndexChanging" OnRowCommand="gvAppointmentList_OnRowCommand"
                                            OnRowDataBound="gvAppointmentList_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Source" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSource" runat="server" Text='<%#Eval("Source") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Appointment Date" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppointmentDate" runat="server" Text='<%#Eval("AppointmentDate")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Time Slot" HeaderStyle-Width="120px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAppointmentTimeSlot" runat="server" Text='<%#Eval("ShowTime")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblregistrationno" runat="server" Text='<%#Eval("registrationno")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Encounter No.' HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Patient Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobile No." HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Age/Gender" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bed No." HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbedno" runat="server" Text='<%#Eval("Bedno")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ward" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Service Name" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resource Name" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResourceName" runat="server" Text='<%#Eval("ResourceName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Precautions" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrecaution" runat="server" Text='<%#Eval("Precaution")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Booked By" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbookedby" runat="server" Text='<%#Eval("Bookedby")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Booked Date" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbookeddate" runat="server" Text='<%#Eval("BookedDate")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Check In By" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCheckinby" runat="server" Text='<%#Eval("CheckInby")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Check In Date" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCheckinDate" runat="server" Text='<%#Eval("CheckInDate")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ordered By" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderedBy" runat="server" Text='<%#Eval("Orderedby")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ordered Date" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrdereddate" runat="server" Text='<%#Eval("OrderedDate")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="1px" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnStatusColor" runat="server" Value='<%#Eval("StatusColor")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                    <asp:AsyncPostBackTrigger ControlID="gvAppointmentList" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>

                        <div class="row form-group">
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>