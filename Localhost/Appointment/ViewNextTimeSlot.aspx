<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewNextTimeSlot.aspx.cs"
    Inherits="Appointment_ViewNextTimeSlot" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="aspl" TagName="PatientQView" Src="~/Include/Components/PatientQView.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>&nbsp;</title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />


    <style type="text/css">
        span {
            font-size: 12px !important;
        }

        .ltraptstyle p {
            display: inline !important;
            font-size: 12px !important;
        }

        .btn {
            font-size: 12px;
            padding: 1px 4px;
        }

        label {
            font-size: 12px;
            font-weight: 600;
            padding-right: 3px;
        }

        th {
            padding: 2px 2px 0px 8px !important;
            font-size: 13px !important;
        }

        .clsGridRoworder td {
            padding: 2px 0px 2px 6px !important;
            font-size: 13px !important;
        }

        .BtnClose {
            background: #007bff;
            float: none;
            color: #fff;
            font-size: 12px;
            font-weight: 500;
            border: none;
            margin: 0px;
            padding: 3px 6px !important;
            border-radius: 3px;
        }

        #lblDoctor {
            float: none !important;
            padding: 0px !important;
            margin: 0 !important;
            color: #292828 !important;
            font-size: 12px;
            text-align: left;
            font-style: normal;
            font-weight: normal !important;
        }

        td.rcbInputCell.rcbInputCellLeft .rcbInput {
            padding: 2px 8px !important;
        }
    </style>

    <script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>
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
                myButton.className = "BtnClose";
                myButton.value = "Processing...";
            }
            return true;
        }
  </script>
    <script type="text/javascript" language="javascript">
        function OnClientClose(oWnd) {

        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow)
                oWindow = window.RadWindow; //Will work in Moz in all cases, including clasic dialog      
            else if (window.frameElement.radWindow)
                oWindow = window.frameElement.radWindow; //IE (and Moz as well)      
            return oWindow;
        }

        function Close() {
            GetRadWindow().Close();
        }          
    </script>

</head>
<body>
   
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <div class="ALPTop01">
            <div class="container-fluid">
                <div class="row pt-1 pb-1">

                    <div class="col-md-8">
                        <div class="ListDetailsText-TopRight">
                            <span>New Appointment</span>
                            <asp:Label ID="lbl_Msg" runat="server" ForeColor="Green" Font-Bold="true" Style="padding-right: 100px;"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:LinkButton ID="lnkViewNextSlot" runat="server" CausesValidation="false" Text="Next Available Slot"
                            Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                            onmoustheout="LinkBtnMouseOut(this.id);" OnClick="lnkViewNextSlot_Click" Visible="false"></asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkRepeatAppointment" runat="server" CausesValidation="false"
                                        Text="Repeat Appointment" Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                                        onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkRepeatAppointment_Click" Visible="false"></asp:LinkButton>
                        <asp:Button ID="btnFindNext" runat="server" Text="Find Next" CssClass="btn btn-primary BtnClose" OnClick="btnFindNext_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="False" />
                        <aspl:PatientQView ID="ucPatientQView" runat="server" />
                        <asp:Button ID="btnclose" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close(); return false;" />

                        <script language="JavaScript" type="text/javascript">
                            function LinkBtnMouseOver(lnk) {
                                document.getElementById(lnk).style.color = "red";
                            }
                            function LinkBtnMouseOut(lnk) {
                                document.getElementById(lnk).style.color = "blue";
                            }
                        </script>
                    </div>
                </div>
            </div>

        </div>









        <div class="container-fluid">

            <div class="row pt-2">


                <div class="" id="trTop" runat="server">
                    <span>Facility</span>
                    <h4>
                        <telerik:RadComboBox ID="ddlFacility" Width="272px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"></telerik:RadComboBox>
                    </h4>
                </div>


                <div class="col-md-5 mb-1">
                    <div class="row">
                        <div class="col-3">
                            <span>Starting From</span>
                        </div>

                        <div class="col-9">
                            <asp:RadioButtonList ID="rblDatePeriod" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblDatePeriod_OnSelectedIndexChanged" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Text="Period" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Date" Value="D"></asp:ListItem>
                            </asp:RadioButtonList>


                            <span id="trDate" runat="server">
                                <telerik:RadDatePicker ID="fromDate" runat="server" Width="110px" MinValue="1">
                                    <DateInput ID="DateInput1" runat="server" DisplayDateFormat="dd/MM/yyyy" DateFormat="MM/dd/yyyy"></DateInput>
                                </telerik:RadDatePicker>
                            </span>

                            <span id="trPeriod" runat="server">
                                <telerik:RadNumericTextBox ID="txtPeriodTime" EmptyMessage="1" Width="40px" MaxLength="2" MinValue="1" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" runat="server" Value="1"></telerik:RadNumericTextBox>
                                <asp:DropDownList ID="ddlPeriodTime" SkinID="DropDown" runat="server">
                                    <asp:ListItem Text="Day(s)" Value="D"></asp:ListItem>
                                    <asp:ListItem Text="Week(s)" Value="W"></asp:ListItem>
                                    <asp:ListItem Text="Month(s)" Value="M"></asp:ListItem>
                                    <asp:ListItem Text="Year(s)" Value="Y"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </div>

                    </div>
                </div>


                <div class="col-md-4 mb-1">
                    <div class="row">
                        <div class="col-3">
                            <span>Show</span>
                        </div>

                        <div class="col-9">

                            <telerik:RadNumericTextBox ID="Radnoofslots" EmptyMessage="1" Width="40px" MinValue="1" MaxLength="2" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" runat="server" Value="1"></telerik:RadNumericTextBox>
                            <span class="vacantProvider">Vacant Slots Per Provider</span>
                        </div>
                    </div>
                </div>


                <div class=" col-md-3 mb-1">
                    <div class="row">
                        <div class="col-md-4 col-3 ltraptstyle">
                            <p>
                                <asp:Literal ID="ltrAppointmentType" runat="server" Text="Appt Type"></asp:Literal>
                            </p>
                            <p style="color: Red;">*</p>
                        </div>
                        <div class="col-8">
                            <telerik:RadComboBox ID="ddlAppointmentType" TabIndex="14" EmptyMessage="" Width="100%" AppendDataBoundItems="true" runat="server">
                                <Items>
                                    <telerik:RadComboBoxItem Text="" Value="0" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                    </div>

                </div>
            </div>
            <div class="row  mt-2">
                <div class="col-12 header_main mb-2">
                    <span><b>Preference:-</b></span>
                </div>



                <div class="col-md-3">
                    <div class="row">
                        <div class="col-md-2 col-3">
                            <span>Time</span>
                        </div>
                        <div class="col-md-10 col-9">
                            <asp:CheckBox ID="chkAm" runat="server" Text="AM" Checked="true" />&nbsp;&nbsp;
                                <asp:CheckBox ID="chkPm" runat="server" Text="PM" Checked="true" />
                        </div>
                    </div>
                </div>

                <div class="col-md-5">
                    <div class="row">
                        <div class="col-md-2 col-3">
                            <span>Days</span>
                        </div>
                        <div class="col-md-10 col-9">
                            <asp:CheckBoxList ID="cblWeekday" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Text="MON" Value="MON"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="TUE" Value="TUE"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="WED" Value="WED"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="THU" Value="THU"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="FRI" Value="FRI"></asp:ListItem>
                                <asp:ListItem Selected="False" Text="SAT" Value="SAT"></asp:ListItem>
                                <asp:ListItem Selected="False" Text="SUN" Value="SUN"></asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="row">
                        <div class="col-3 ltraptstyle">
                            <p>
                                <asp:Label ID="lblDoctor" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label>
                            </p>
                            <p style="color: Red;">*</p>
                        </div>
                        <div class="col-9">
                            <telerik:RadComboBox ID="RadLstDoctor" Width="100%" CheckBoxes="true" runat="server"></telerik:RadComboBox>
                        </div>
                        <%--<telerik:RadListBox CheckBoxes="true" Width="98%" Height="200px" ID="RadLstDoctor" runat="server"></telerik:RadListBox>--%>
                    </div>

                    <div class="findNextDiv03">
                    </div>
                    <div class="findNextDiv03">
                    </div>
                </div>

            </div>

        </div>






        <div class="GeneralDiv">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-12">

                        <table id="Table1" cellpadding="0" cellspacing="0" runat="server" class="tableRecurrence  table-bordered table-hover">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvTimeSlot" SkinID="gridviewOrder" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                        DataKeyNames="DoctorId,FacilityId" ShowHeader="true" PageSize="15" Width="100%"
                                        AllowPaging="true" PagerSettings-Mode="NumericFirstLast" PageIndex="0" ShowFooter="false"
                                        PagerSettings-Visible="true" OnSelectedIndexChanging="gvTimeSlot_SelectedIndexChanging"
                                        OnPageIndexChanging="gvTimeSlot_PageIndexChanging" OnRowDataBound="gvTimeSlot_RowDataBound"
                                        HeaderStyle-ForeColor="#e9ecef" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#494f54" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                        <Columns>
                                            <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" Visible="true" />
                                            <asp:BoundField DataField="Facility" HeaderText="Facility" Visible="true" />
                                            <asp:BoundField DataField="AppDate" HeaderText="Date" Visible="true" />
                                            <asp:BoundField DataField="AppTime" HeaderText="Begin Time" Visible="true" />
                                            <asp:BoundField DataField="DayName" HeaderText="Day" Visible="true" />
                                            <asp:CommandField ShowSelectButton="true" ButtonType="Link" ItemStyle-ForeColor="DodgerBlue" />
                                        </Columns>
                                        <PagerSettings PageButtonCount="10" />
                                    </asp:GridView>

                                    <asp:Label ID="Label1" runat="server" Width="410px"></asp:Label>
                                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>

                                </td>
                            </tr>
                        </table>

                    </div>

                </div>
            </div>
        </div>



        </div>
    </form>
    
</body>
</html>
