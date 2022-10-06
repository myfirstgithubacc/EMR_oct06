<%@ Page Language="C#" Theme="DefaultControls" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="providertimings.aspx.cs" Inherits="MPages_providertimings"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    


    <script language="javascript" type="text/javascript">
        function NextTab() {
            if (event.keyCode == 13) {
                event.keyCode = 9;
            }
        }

        function Tab_SelectionChanged(sender) {
            //alert('Chandan');
            var tabIndx;
            if (sender == 1) {
                tabIndx = 1;
            }
            else {
                tabIndx = sender.get_activeTabIndex();
            }
            //sender.set_activeTabIndex(tabIndx);
            //alert(tabIndx);
            if (tabIndx == 0) {
                var ctrlsave = document.getElementById('<%=btnDoctorVisitDuration.ClientID %>');
                //alert(ctrlsave);
                ctrlsave.style.visibility = 'hidden';
                ctrlsave.style.position = 'absolute';
                var ctrlsave = document.getElementById('<%=btnsavedoctortime.ClientID %>');
                ctrlsave.style.visibility = 'visible';
                ctrlsave.style.position = 'static';
            }
            else {
                var ctrlsave = document.getElementById('<%=btnsavedoctortime.ClientID %>');
                // alert(ctrlsave);
                ctrlsave.style.visibility = 'hidden';
                ctrlsave.style.position = 'absolute';
                var ctrlupdate = document.getElementById('<%=btnDoctorVisitDuration.ClientID %>');
                ctrlupdate.style.visibility = 'visible';
                ctrlupdate.style.position = 'static';
            }
        }
    </script>

    <div style="overflow-y: hidden; overflow-x: hidden;">

    <div class="container-fluid header_main form-group">
        <div class="col-md-3"><h2>Appointment Template</h2></div>
        <div class="col-md-9 text-right">
            
            <span id="DivMenu" runat="server">
                <asp:LinkButton ID="lnkEmployee" runat="server" CausesValidation="false" Text="Employee"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployee_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkEmployeeLookup" runat="server" CausesValidation="false" Text="Employee Look Up"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    OnClick="lnkEmployeeLookup_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkProviderDetails" runat="server" CausesValidation="false"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Provider Details" OnClick="lnkProviderDetails_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkProviderProfile" runat="server" CausesValidation="false"
                    Text="Employee Profile" CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);"
                    onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkProviderProfile_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkClassification" runat="server" CausesValidation="false" CssClass="btnNew"
                    onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Classification" OnClick="lnkClassification_OnClick"></asp:LinkButton>
                <asp:LinkButton ID="lnkPeriodicTemplate" runat="server" CausesValidation="false"
                    CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                    Text="Periodic Template" OnClick="lnkPeriodicTemplate_OnClick"></asp:LinkButton>

                <script language="JavaScript" type="text/javascript">
                    function LinkBtnMouseOver(lnk) {
                        document.getElementById(lnk).style.color = "SteelBlue";
                    }
                    function LinkBtnMouseOut(lnk) {
                        document.getElementById(lnk).style.color = "SteelBlue";
                    }
                </script>
            </span>

            <span class="pull-right">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdnNextAppDate" runat="server" />
                        <asp:Button ID="btndoctortimenew" CssClass="btn btn-primary" runat="server" OnClick="btndoctortimenew_Click"
                            Text="New" />
                        <asp:Button ID="btnsavedoctortime" CssClass="btn btn-primary" runat="server" OnClick="btnsavedoctortime_Click"
                            ValidationGroup="DT" />
                        <asp:Button ID="btnDoctorVisitDuration" Style="position: absolute; visibility: hidden;"
                            CssClass="btn btn-primary" runat="server" ValidationGroup="PT" Text="Save" OnClick="SaveDoctorVisitDuration_OnClick" />
                        <asp:ValidationSummary ID="valPageError" runat="server" ShowMessageBox="true" ShowSummary="false"
                            ValidationGroup="PT" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </span>
        </div>

    </div>


    <div class="container-fluid">
        <div class="row form-group">
            <div class="col-md-12 col-sm-12 text-center">
                <asp:UpdatePanel ID="updatemsg" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" ForeColor="Green"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>


    <div class="container-fluid">
        <div class="row">
            
                <AJAX:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnClientActiveTabChanged="Tab_SelectionChanged" Width="100%">
                    <AJAX:TabPanel ID="TabVisitMaster" runat="server" TabIndex="0" Width="100%">
                        <HeaderTemplate>
                            Provider Timings
                        </HeaderTemplate>
                        <ContentTemplate>
                            
                            <div class="row form-group">
                                
                                <div class="col-md-4">
                                    <div class="row form-group">
                                        <div class="col-md-3"><asp:Label ID="Label1" runat="server" Text="Provider" /></div>
                                        <div class="col-md-9">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <telerik:RadComboBox ID="ddlDoctor" runat="server" MarkFirstMatch="true" Filter="Contains"
                                                        TabIndex="30" Width="100%" AppendDataBoundItems="true"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3"><asp:Label ID="Label2" runat="server" Text="Facility" /></div>
                                        <div class="col-md-9">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <telerik:RadComboBox ID="ddlfacility" Width="100%" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlfacility_OnSelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <span id="Tr1" runat="server">
                                            <div class="col-md-3" runat="server">From</div>
                                            <div class="col-md-9">
                                                <div class="row">
                                                    <div class="col-md-5 PaddingRightSpacing" runat="server">
                                                        <asp:UpdatePanel ID="uptoDate" runat="server">
                                                            <ContentTemplate>
                                                                <telerik:RadDatePicker ID="rdpFromDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true">
                                                                </telerik:RadDatePicker>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                        </span>
                                                    <div class="col-md-2">To</div>
                                                    <div class="col-md-5 PaddingLeftSpacing">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <telerik:RadDatePicker ID="rdpToDate" runat="server" MinDate="01/01/1900" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" Width="100%">
                                                                </telerik:RadDatePicker>
                                                                
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3"></div>
                                        <div class="col-md-9">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>

                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <div class="PD-TabRadioNew01 margin_z">
                                                                <asp:ValidationSummary ID="ValidationSummary2" ShowMessageBox="true" ShowSummary="false"
                                                                    ValidationGroup="DT" runat="server" />
                                                                <asp:CheckBox ID="chkNoEnd" runat="server" Text="No End Date" OnCheckedChanged="chkNoEnd_OnCheckedChanged" AutoPostBack="true" />
                                                                 <asp:RequiredFieldValidator ID="revAppointmentSlot" runat="server" ControlToValidate="txtslottiming"
                                                                    ValidationGroup="DT" Display="None" ErrorMessage="Enter Appointment Slot Timings"></asp:RequiredFieldValidator>
                                                                 <asp:Button ID="btndoctortimeedit" Visible="false" SkinID="Button" ValidationGroup="DT"
                                                                    runat="server" OnClick="btndoctortimeedit_Click" Text="Find" />
                                                            </div>
                                                                
                                                         </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-8 label2"><asp:Label ID="lblapp" Text="Appointment Slots Timing" runat="server"></asp:Label></div>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtslottiming" Columns="2" MaxLength="2" Text="10" runat="server"></asp:TextBox>
                                                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" Enabled="True"
                                                                TargetControlID="txtslottiming" FilterType="Numbers">
                                                            </AJAX:FilteredTextBoxExtender>
                                                        </div>
                                                    </div>
                                                    
                                                  
                                                    
                                                    
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="row form-group">
                                        <div class="col-md-3"></div>
                                        <div class="col-md-9"></div>
                                    </div>


                                </div>


                                
                                <div class="col-md-4"></div>

                                <div class="col-md-4">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvDoctorTime" Width="100%" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                                OnSelectedIndexChanged="gvDoctorTime_OnSelectedIndexChanged" ShowFooter="false"
                                                OnRowDataBound="gvDoctorTime_RowDataBound" ShowHeader="true" AllowPaging="false"
                                                AllowSorting="false" RowStyle-HorizontalAlign="Left">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:BoundField DataField="DoctorDate" HeaderText="Doctor Timings" />
                                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" SelectText="Edit"
                                                        CausesValidation="false" ShowSelectButton="true" />
                                                    <asp:BoundField DataField="DateFrom" HeaderText="DateFrom" />
                                                    <asp:BoundField DataField="DateTo" HeaderText="DateTo" />
                                                    <%-- <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRowEdit" runat="server" Text="Edit" CommandName="RowEdit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> --%>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>



                            <div class="row form-group">
                                <div class="col-md-12">
                                    <asp:UpdatePanel ID="update" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvTiming" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="false"
                                                OnSelectedIndexChanged="gvTiming_SelectedIndexChanged" Width="60%" OnRowDataBound="gvTiming_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="day" HeaderText="Days" />
                                                    <asp:TemplateField HeaderText="From [hh:mm]">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFromTime" runat="server" Columns="8" MaxLength="80"
                                                                Text='<%#Eval("shiftfirstfrom") %>'></asp:TextBox>
                                                            <AJAX:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtFromTime"
                                                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                                                AcceptNegative="Left" AcceptAMPM="true" ErrorTooltipEnabled="True">
                                                            </AJAX:MaskedEditExtender>
                                                            <asp:RegularExpressionValidator ID="rngto26" runat="server" ControlToValidate="txtFromTime"
                                                                SetFocusOnError="true" ValidationGroup="gvTiming" Text="*" ErrorMessage="Invalid From Time"
                                                                ValidationExpression="^[01][0-9][:][0-9][0-9]\s*(AM|PM)$"></asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="To [hh:mm]">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtToTime" runat="server" Columns="8" MaxLength="5"
                                                                Text='<%#Eval("shiftfirstto") %>'></asp:TextBox>
                                                            <AJAX:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtToTime"
                                                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                                                AcceptNegative="Left" AcceptAMPM="true" ErrorTooltipEnabled="True">
                                                            </AJAX:MaskedEditExtender>
                                                            <asp:RegularExpressionValidator ID="rngto27" runat="server" ControlToValidate="txtToTime"
                                                                SetFocusOnError="true" ValidationGroup="gvTiming" Text="*" ErrorMessage="Invalid To Time"
                                                                ValidationExpression="^[01][0-9][:][0-9][0-9]\s*(AM|PM)$"></asp:RegularExpressionValidator>
                                                            <asp:ValidationSummary ID="vs2" ValidationGroup="gvTiming" runat="server" ShowMessageBox="true"
                                                                ShowSummary="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exclude">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkIsActive" runat="server"  Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem,"IsActive"))%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" CausesValidation="false"
                                                                CommandName="Select"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                
                            </div>


                            <div class="row">
                                <div class="col-md-4">
                                    <div class="row">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <div class="col-md-4"><asp:Label ID="lblOpdCardString" runat="server" Text="OPD Card string"></asp:Label></div>
                                                <div class="col-md-8"><asp:TextBox ID="txtOpdCardString" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="col-md-3"><span style="color: Red">*</span> Mandatory/Invalid Entry</div>

                            </div>
                        </ContentTemplate>
                    </AJAX:TabPanel>


                    <AJAX:TabPanel ID="TabPanel1" runat="server" TabIndex="0" Width="100%">
                        <HeaderTemplate>
                            Provider Slot
                        </HeaderTemplate>
                        <ContentTemplate>

                            <div class="container-fluid">
                                <div class="row form-group">
                                    
                                    <div class="col-md-4">
                                        <div class="row form-group">
                                            <div class="col-md-3">Provider</div>
                                            <div class="col-md-9">
                                                <telerik:RadComboBox ID="ddlProvider" runat="server" MarkFirstMatch="true" Filter="Contains"
                                                    TabIndex="30" Skin="Outlook" Width="100%" AppendDataBoundItems="true"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_OnSelectedIndexChanged">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                                <asp:RequiredFieldValidator ID="rfvddlProvider" runat="server" ControlToValidate="ddlProvider"
                                                    Display="None" InitialValue="0" ErrorMessage="Select Provider" ValidationGroup="PT"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-4">Set Default Time</div>
                                            <div class="col-md-8">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlSetDefaultTime" runat="server" Width="100%"
                                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlSetDefaultTime_OnSelectedIndexChanged"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                                            <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                                            <asp:ListItem Text="60" Value="60"></asp:ListItem>
                                                            <asp:ListItem Text="75" Value="75"></asp:ListItem>
                                                            <asp:ListItem Text="90" Value="90"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3 PaddingLeftSpacing">Minutes</div>
                                                </div>
                                                
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4"></div>

                                </div>


                                <div class="row form-group">
                                    <asp:UpdatePanel ID="UPDgvDoctorVisitDuration" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvDoctorVisitDuration" />
                                            <asp:AsyncPostBackTrigger ControlID="btnDoctorVisitDuration" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlProvider" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:GridView ID="gvDoctorVisitDuration" SkinID="gridviewOrderNew" CellPadding="4" runat="server"
                                                AutoGenerateColumns="false" DataKeyNames="ID" ShowHeader="true" Width="100%" GridLines="Both"
                                                PageSize="13" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" ShowFooter="false"
                                                PagerSettings-Visible="true" HeaderStyle-HorizontalAlign="Left" PageIndex="0"
                                                OnRowDataBound="gvDoctorVisitDuration_OnRowDataBound" OnRowCommand="gvDoctorVisitDuration_OnRowCommand"
                                                OnRowCancelingEdit="gvDoctorVisitDuration_OnRowCancelingEdit" OnRowUpdating="gvDoctorVisitDuration_OnRowUpdating"
                                                OnPageIndexChanging="gvDoctorVisitDuration_OnPageIndexChanging" OnRowEditing="gvDoctorVisitDuration_OnRowEditing">
                                                <HeaderStyle Font-Size="11px" Font-Bold="false" />
                                                <Columns>
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="true" ReadOnly="true" />
                                                    <%-- <asp:BoundField DataField="SerialNo" HeaderText="S&nbsp;No" Visible="true" ReadOnly="true"
                                                            ItemStyle-Width="40px" HeaderStyle-Width="40px" />--%>
                                                    <asp:TemplateField HeaderText="S&nbsp;No" Visible="true" HeaderStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex +1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Visit Type">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltrlGridVisitType" runat="server" Text='<%#Eval("Type")%>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlVisitGrid" runat="server" DataSourceID="sqlImmunization"
                                                                SkinID="DropDown" DataTextField="Name" DataValueField="VisitTypeID" SelectedValue='<%#Eval("VisitTypeID")%>'
                                                                Width="249px" Font-Size="9">
                                                            </asp:DropDownList>
                                                            <asp:SqlDataSource ID="sqlImmunization" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                                                                SelectCommand="Select 0 as VisitTypeID, ' [Select ]' as Name Union all select VisitTypeID,Type as Name from emrvisittype order by Name">
                                                            </asp:SqlDataSource>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duration (Minutes)" HeaderStyle-Width="180px">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="ltrlGridDuration" runat="server" Text='<%#Eval("Duration")%>'></asp:Literal>
                                                            <asp:TextBox ID="txtDoctorVisitDurationGrid1" SkinID="textbox" runat="server" Columns="6"
                                                                MaxLength="3" Text='<%#Eval("Duration")%>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="RFVtxtDoctorVisitDuration1" runat="server" ErrorMessage="Duration Time Cannot Be Blank..."
                                                                    SetFocusOnError="true" ControlToValidate="txtDoctorVisitDurationGrid1" Display="None"
                                                                    ValidationGroup="SaveVisitDuration"></asp:RequiredFieldValidator>--%>
                                                            <AJAX:FilteredTextBoxExtender ID="FTEtxtDoctorVisitDuration1" runat="server" Enabled="True"
                                                                TargetControlID="txtDoctorVisitDurationGrid1" FilterType="Numbers, Custom">
                                                            </AJAX:FilteredTextBoxExtender>
                                                            <%-- <asp:RegularExpressionValidator ID="REVtxtDoctorVisitDuration1" runat="server" ControlToValidate="txtDoctorVisitDurationGrid1"
                                                                SetFocusOnError="true" ErrorMessage="Invalid Format" Display="None" ValidationExpression="^((0?[1-9]|1[012])(:[0-5]\d){0,2})$|^([01]\d|2[0-3])(:[0-5]\d){1,2}$">
                                                            </asp:RegularExpressionValidator>--%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtDoctorVisitDurationGrid" SkinID="textbox" runat="server" Columns="6"
                                                                MaxLength="3" Text='<%#Eval("Duration")%>'></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RFVtxtDoctorVisitDuration" runat="server" ErrorMessage="Duration Time Cannot Be Blank..."
                                                                SetFocusOnError="true" ControlToValidate="txtDoctorVisitDurationGrid" Display="None"
                                                                ValidationGroup="Update"></asp:RequiredFieldValidator>
                                                            <AJAX:FilteredTextBoxExtender ID="FTEtxtDoctorVisitDuration" runat="server" Enabled="True"
                                                                TargetControlID="txtDoctorVisitDurationGrid" FilterType="Numbers, Custom">
                                                            </AJAX:FilteredTextBoxExtender>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="70px" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGridActive1" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblGridActive2" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                            <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="50px" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                                CommandName="DeActivate" CommandArgument='<%#Eval("ID")%>' />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="40px" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server">Edit</asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdate" CommandName="Update" runat="server" Text="Update"
                                                                CausesValidation="true" ValidationGroup="Update"></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkCancel" CommandName="Cancel" runat="server">Cancel</asp:LinkButton>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Active" HeaderText="Active" ReadOnly="true" />
                                                    <asp:BoundField DataField="VisitTypeID" HeaderText="VisitTypeID" ReadOnly="true" />
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>

                        </ContentTemplate>
                    </AJAX:TabPanel>
                </AJAX:TabContainer>
        </div>
            
        </div>
    </div>
</asp:Content>
