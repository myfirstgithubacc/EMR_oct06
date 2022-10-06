<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="BreakAndBlock.aspx.cs" Inherits="Appointment_BreakAndBlock" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>

<asp:Content ID="cntBreakAndBlock" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .red1 {
            color: #f00;
            float: none !important;
        }

        .RecurrenceEditor .rsAdvChkWrap input,
        .RecurrenceEditor .rsRecurrenceOptionList input,
        .RecurrenceEditor .rsAdvRadio input,
        .RecurrenceEditor .rsAdvWeekly_WeekDays input {
            margin-top: 5px !important;
        }

        @media only screen and (max-width: 680px) {
            #ctl00_ContentPlaceHolder1_RadSchedulerRecurrenceEditor1_RecurrenceFrequencyPanel {
                width: 100%!important;
                height:40px!important;
            }

            ul.rsRecurrenceOptionList {
                display: flex!important;
                justify-content: space-around!important;
            }
            div#ctl00_ContentPlaceHolder1_RadSchedulerRecurrenceEditor1_RecurrencePatternPanel{
                padding-left:0px!important;
            }
            .RecurrenceEditor_Default ul.rsRecurrenceOptionList{
                border-right:none!important;
                border-bottom:1px solid #ababab!important;
            }
            
        }
        @media only screen and (max-width: 945px) {
         ul.rsAdvWeekly_WeekDays{
            display:block!important;
        }
        }
        ul.rsAdvWeekly_WeekDays{
            display:inline-flex
        }
    </style>
    
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">


  <script type="text/javascript">
        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.value = "Processing...";
            }
            return true;
        }
  </script>
        <script type="text/javascript">
            function pageLoad() {
                var recurrenceEditor = $find('<%=RadSchedulerRecurrenceEditor1.ClientID %>');
                if (recurrenceEditor.get_recurrenceRule() == null) {
                    $telerik.$(".rsAdvWeekly_WeekDays li span input").each(function(index, item) {
                        item.checked = false;
                    });
                }
                $telerik.$(".rsRecurrenceOptionList li:first-child").hide();
            }
        </script>

        <script type="text/javascript">
            function CloseScreen() {
                window.close();
            }
        </script>

    </telerik:RadScriptBlock>

    <asp:UpdatePanel ID="updBlock" runat="server">
        <ContentTemplate>


            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-4">
                        <h2>
                            <asp:Label ID="lblHeader" Text="" runat="server" /></h2>
                    </div>
                    <div class="col-md-8 col-sm-8 col-8 text-right">
                        <asp:Button ID="btnSave" class="btn btn-primary BBSaveBtn" role="button" runat="server" Text="Save" OnClick="btnSave_Click" />

                        <asp:Button ID="btnUpdate" class="btn btn-primary" role="button" runat="server" Text="Update Break" OnClick="btnUpdate_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        <asp:Button ID="btnUpdateAll" class="btn btn-primary" role="button" runat="server" Text="Update All Break" OnClick="btnUpdateAll_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        <asp:Button ID="btnDeleteAllBreak" class="btn btn-primary" role="button" runat="server" Text="Delete All" OnClick="btnDeleteAllBreak_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        <asp:Button ID="btnDeleteBreak" class="btn btn-primary" role="button" runat="server" Text="Delete This" OnClick="btnDeleteBreak_Click" />

                        <asp:Button ID="btnNew" class="btn btn-primary" role="button" runat="server" Text="New" OnClick="btnNew_Click" />
                        <asp:Button ID="btnClose" class="btn btn-primary" role="button" Text="Close" runat="server" ToolTip="Close" OnClientClick="window.close();" />
                    </div>
                </div>
                <div class="row text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                </div>
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-12">
                        <div class="BBGreen">
                            <h2>
                                <img src="../Images/Break-And-Block.png" border="0" width="24" height="24"></h2>
                            <h3>Break And Block</h3>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12 border-all">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 ">
                                            Facility <span class="red1">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 ">
                                            <telerik:RadComboBox ID="ddlFacility" runat="server" AllowCustomText="False" Width="100%" MarkFirstMatch="False" SkinID="DropDown"></telerik:RadComboBox>
                                            <asp:RequiredFieldValidator ID="rfvFacility" runat="server" ControlToValidate="ddlFacility" ErrorMessage="Please Select Facility" ValidationGroup="BreakAndBlock" Display="None"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-5 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 ">
                                            <asp:Label ID="lblProvider" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' />
                                            <span class="red1">&nbsp;*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 ">
                                            <telerik:RadComboBox ID="ddlProvider" runat="server" Width="100%" MarkFirstMatch="true" AllowCustomText="true" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged" />
                                            <telerik:RadComboBox ID="ddlTheater" runat="server" EmptyMessage="[ Select ]" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-3 col-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-12 col-sm-12 ">
                                            <asp:RadioButtonList ID="rdoIsBlock" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Break" Value="0" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Block" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="BBGreen">
                            <h2>
                                <img src="../Images/InsuranceDetails.png" border="0" width="23" height="24"></h2>
                            <h3>Details</h3>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12 border-all">
                            <div class="row">
                                <div class="col-lg-4 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 ">
                                            Name <span class="red1">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8 ">
                                            <asp:TextBox ID="txtBreakName" Width="100%" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvBreakName" runat="server" ControlToValidate="txtBreakName" ErrorMessage="Please enter Break Name" ValidationGroup="BreakAndBlock" Display="None"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-6">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 col-sm-4 text-nowrap">
                                            Break/Block Date <span class="red1">*</span>
                                        </div>
                                        <div class="col-md-8 col-sm-8">
                                            <telerik:RadDatePicker ID="dtpDate" runat="server" Width="100%" DateInput-DateFormat="dd/MM/yyyy" MinDate="01/01/1901"></telerik:RadDatePicker>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-8 col-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-lg-4 col-md-3 col-sm-3 col-4 text-nowrap">
                                            Break/Block Time <span class="red1">*</span>
                                        </div>
                                        <div class="col-lg-8 col-md-9 col-sm-8 col-8">
                                            <div class="row">
                                                <div class="col-md-5 col-sm-5 col-5">
                                                    <telerik:RadTimePicker ID="RadTimeFrom" DateInput-ReadOnly="true" AutoPostBack="true" OnSelectedDateChanged="RadTimeFrom_SelectedDateChanged" TimeView-Interval="15" PopupDirection="BottomLeft" TimeView-Columns="6" runat="server" Width="100%" />
                                                </div>
                                                <div class="col-md-2 col-sm-2 col-2 text-center">To &nbsp;</div>

                                                <div class="col-md-5 col-sm-5 col-5">
                                                    <telerik:RadTimePicker ID="RadTimeTo" runat="server" AutoPostBack="true" TimeView-Interval="15" DateInput-ReadOnly="true" OnSelectedDateChanged="RadTimeTo_SelectedDateChanged" PopupDirection="BottomLeft" TimeView-Columns="6" Width="100%" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12">
                                    <telerik:RadSchedulerRecurrenceEditor ID="RadSchedulerRecurrenceEditor1" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-xs-12 col-xs-12 gridview" id="Table1" runat="server">
                        <asp:GridView ID="gvBreakAndBlockDetails" runat="server" Width="100%"
                            AllowPaging="true" PageSize="15" GridLines="None" DataKeyNames="ID" AutoGenerateColumns="false"
                            OnRowDataBound="gvBreakAndBlockDetails_RowDataBound" OnRowCommand="gvBreakAndBlockDetails_RowCommand"
                            OnSelectedIndexChanged="gvBreakAndBlockDetails_SelectedIndexChanged" OnPageIndexChanging="gvBreakAndBlockDetails_PageIndexChanging">

                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctorId" runat="server" Text='<%#Eval("DoctorId")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFacilityId" runat="server" Text='<%#Eval("FacilityId")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Facility&nbsp;Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBreakName" runat="server" Text='<%#Eval("BreakName")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Break&nbsp;Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBreakDate" runat="server" Text='<%#Eval("BreakDate")%>'></asp:Label>
                                        <asp:HiddenField ID="hdBdate" runat="server" Value='<%#Eval("BDate")%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Start&nbsp;Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStartTime" runat="server" Text='<%#Eval("StartTime")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="End&nbsp;Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEndTime" runat="server" Text='<%#Eval("EndTime")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Recurring">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrence" runat="server" Text='<%#Eval("RecurrenceRule")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Recurring ParentID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrenceParentId" runat="server" Text='<%#Eval("RecurrenceParentId")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Recurring">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecurrenceRuleType" runat="server" Text='<%#Eval("RecurrenceRuleType")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="Edit" ButtonType="Link" ControlStyle-ForeColor="Blue"
                                    ControlStyle-Font-Underline="true" SelectText="Edit" CausesValidation="false"
                                    ShowSelectButton="true">
                                    <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                </asp:CommandField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                            ToolTip="Delete" Width="13px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                </div>
            </div>
    	</div>
    	      
	<!-- Footer Icon Part Ends -->
 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
