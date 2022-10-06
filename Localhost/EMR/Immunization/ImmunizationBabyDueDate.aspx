<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImmunizationBabyDueDate.aspx.cs"
    Inherits="EMR_Immunization_ImmunizationBabyDueDate" MasterPageFile="~/Include/Master/EMRMaster.master"
    Title="" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
    <asp:HiddenField ID="hdnIsMedicalAlert" runat="server" Value="0" />
    <!-- Bootstrap -->
       <%: Styles.Render("~/bundles/EMRMainStyle") %>
    
   <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <asp:UpdatePanel ID="upRad" runat="server">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false" Skin="Metro">
                <Windows>
                    <telerik:RadWindow ID="RadWindow2" CssClass="HealthCheckupPage" OpenerElementID="btnPrintPdf" runat="server" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <telerik:RadWindow ID="oWnd" runat="server" Width="100%" ReloadOnShow="true" Modal="true" Skin="Web20" Behaviors="Maximize"></telerik:RadWindow>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-3 col-sm-3 col-xs-4 text-nowrap"><h2>Immunization Due Date</h2></div>
                    <div class="col-md-4 col-sm-4 col-xs-5">
                        <div class="row">
                            <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                            <div class="col-md-4 col-sm-4 col-xs-3">
                                <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>' Font-Underline="false" CssClass="lbtnSearchPatient" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:TextBox ID="txtAccountNo" CssClass="txtAccountNo" runat="server" Width="100%" MaxLength="10" onkeyup="return validateMaxLength();" />
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-5 text-nowrap">
                                <asp:Button ID="btnGetInfo" runat="server" Enabled="true" OnClick="btnGetInfo_Click" SkinID="button" Style="visibility: hidden; display: none;" Text="Assign" Width="10px" />
                              
                                <asp:LinkButton ID="lnkDailyInjection" runat="server" Text="Daily Injection" Font-Underline="false" Font-Bold="true" ToolTip="Click to give Daily Injection" OnClick="lnkDailyInjection_OnClick" />
                            </div>
                                 </asp:Panel>
                        </div>
                    </div>
                     <div class="col-md-5 col-sm-5 col-xs-3 text-right no-p-l">
                         <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" />
                                <asp:Button ID="btnPrint" CssClass="btn btn-primary" runat="server" Text="Print" OnClick="btnPrint_Click" />
                            <asp:Button ID="btnClosePage" CssClass="btn btn-primary" Visible="false" runat="server" Text="Close" OnClientClick="window.close();" />
                        </div>
                </div>
                 <div class="row text-center">
                            <asp:HiddenField ID="hdnCurrentDate" runat="server" />
                            <asp:Label ID="lblmsg" runat="server" style="position:relative;margin:0px;width:100%;" Font-Bold="true" ForeColor="Green"></asp:Label>
                        </div>
                <div class="row" style="background:#e8e8e8;">
                    <div class="col-md-12 p-t-b-5">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-3 col-sm-3 col-xs-4">
                                <asp:Label ID="Label1" runat="server" CssClass="ImmunizationDOB" Text="DOB"></asp:Label>
                            </div>
                            <div class="col-md-9 col-sm-9 col-xs-8">
                                <asp:Label ID="lbldob" CssClass="ImmunizationDOB01" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                 <asp:LinkButton ID="lnkAlerts" runat="server" Text="Patient Alert" OnClick="lnkAlerts_OnClick" Visible="false" />
                                <span class="ImmunizationDOB">Cancellation Remarks</span>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtcancel" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ValidationGroup="Cancel" ControlToValidate="txtcancel" Display="None" runat="server" ErrorMessage="Enter Cancellation Remarks."></asp:RequiredFieldValidator>
                                <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="Cancel" ShowMessageBox="true" ShowSummary="false" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:Label ID="lblImmunizationDD" runat="server" Visible="false" Text="Label"></asp:Label>
                        <div style="width:97vw;overflow:auto;height:350px;">
                                <asp:GridView ID="gvDue" CellPadding="0" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                                    Width="100%" AllowPaging="false" PagerSettings-Mode="NumericFirstLast" ShowFooter="false" PagerSettings-Visible="true" 
                                    HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDue_RowCommand" OnRowDataBound="gvDue_OnDataBound" HeaderStyle-ForeColor="#15428B" 
                                    HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" 
                                    HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                    <HeaderStyle />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Age" HeaderStyle-Width="55px" ItemStyle-Width="55px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAge" Text='<%#Eval("Age")%>' runat="server"></asp:Label>
                                                <asp:HiddenField ID="lblScheduleID" Value='<%#Eval("ScheduleID")%>' runat="server" />
                                                <asp:HiddenField ID="hdnGivenStatus" Value='<%#Eval("GivenStatus")%>' runat="server" />
                                                <asp:HiddenField ID="hdnDaysFrom" Value='<%#Eval("DaysFrom")%>' runat="server" />
                                                <asp:HiddenField ID="hdnDaysTo" Value='<%#Eval("DaysTo")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Immunization Name" HeaderStyle-Width="220px" ItemStyle-Width="220px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImmunizationName" Text='<%#Eval("ImmunizationName")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblImmunizationId" Text='<%#Eval("ImmunizationId")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Due Date" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" Text='<%#Eval("ImmunizationDueDate")%>' runat="server">
                                                </asp:Label>
                                                <telerik:RadDatePicker ID="RadExpiryDate" runat="server" DbSelectedDate='<%# Bind("Duedate") %>' MinDate="01/01/1900" Width="140px" AutoPostBackControl="Both">
                                                    <DateInput runat="server" DisplayDateFormat="dd/MM/yyyy " DateFormat="dd/MM/yyyy " />
                                                </telerik:RadDatePicker>
                                                <%--<asp:CheckBox ID="chkDueDate" runat="server" AutoPostBack="true" OnCheckedChanged ="onchkDueDate" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Given Date" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGivenDate" Text='<%#Eval("GivenDate")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										
                                        <asp:TemplateField HeaderText="Details" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="SearchKeyBtn" OnClick="btngo_OnClick" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Batch No." HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBatchNo" Text='<%#Eval("BatchNo")%>' runat="server"></asp:Label>
                                                <asp:HiddenField ID="lblID" Value='<%#Eval("Id")%>' runat="server" />
                                                <asp:HiddenField ID="hdnDaysDifference" Value='<%#Eval("DaysDifference")%>' runat="server" />
                                                <asp:HiddenField ID="hdnImmunizationDueDateCount" Value='<%#Eval("ImmunizationDueDateCount")%>' runat="server" />
                                                <asp:HiddenField ID="hdnRejectedByPatient" Value='<%#Eval("RejectedByPatient")%>' runat="server" />
                                                <asp:HiddenField ID="hdnVaccineGivenByOutsider" Value='<%#Eval("VaccineGivenByOutsider")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Brand" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrand" runat="server" Text='<%#Eval("ItemBrandName") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnBrandId" runat="server" Value='<%#Eval("BrandId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name of Nurse" HeaderStyle-Width="145px" ItemStyle-Width="145px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNurseName" Text='<%#Eval("NurseName")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" CssClass="ImmunizationRemark" Text='<%#Eval("Remarks")%>' ReadOnly="true" runat="server" Wrap="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DeActivate" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png" CommandName="DeActivate" CommandArgument='<%#Eval("ID")%>' ToolTip="DeActivate" ValidationGroup="Cancel" CausesValidation="true" />
                                                <asp:ImageButton ID="ibtnDeleteDueDate" runat="server" ImageUrl="/Images/DeleteRow.png" CommandName="DeActivateDueDate" CommandArgument='<%#Eval("DueId")%>' ToolTip="DeActivate" ValidationGroup="Cancel" CausesValidation="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DueId" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueId" Text='<%#Eval("DueId")%>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                    </div>
                        </div>
                </div>
                <div class="row">

                        <div class="col-md-12 p-t-b-5">
                            <span class="ImmunizationDOB">Color Legends</span>

                            <asp:Label ID="lblDueColor" runat="server" CssClass="ImmunizationGreen" Text="Due" />
                            <asp:Label ID="lblGivenColor" runat="server" CssClass="ImmunizationSkyBlue" Text="Given" />
                            <asp:Label ID="lblRejected" runat="server" CssClass="ImmunizationSkyGreen" Text="Refused" />

                            <asp:HiddenField ID="hdnCountDueDate" runat="server" />
                            <asp:HiddenField ID="hdnDueDateColor" runat="server" />
                            <asp:HiddenField ID="hdnGivenDateColor" runat="server" />
                            <asp:HiddenField ID="hdnRejected" runat="server" />

                            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server" Skin="Metro">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Maximize,Minimize,Pin"></telerik:RadWindow>
                                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move,Maximize,Minimize,Pin"></telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:Button ID="btnclose" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnclose_OnClick" />

                        </div>

                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        <script language="javascript" type="text/javascript">
        function Validation(CtrltxtDate, CtrlddlGivenBy, CtrlHdnCurrDate) {

            var valCtrltxtDate = $get(CtrltxtDate).value;
            var SelIndexCtrlddlGivenBy = $get(CtrlddlGivenBy).selectedIndex;

            var totFromDays = 0;
            var totToDays = 0;

            if (valCtrltxtDate == "") {
                alert("Please Select The Given Date");
                $get(CtrltxtDate).focus();
                return false;
            }
            if (SelIndexCtrlddlGivenBy == "0") {
                alert("Please Select Who Has Given The Immunization..");
                $get(CtrlddlGivenBy).focus();
                return false;
            }
            return ValidateDate(CtrltxtDate, CtrlHdnCurrDate);
        }

        function ValidateDate(userdate, CCurDate) {
            var DtTxt = $get(userdate).value;
            var DtCurr = $get(CCurDate).value;



            var str1 = DtTxt;
            var str2 = DtCurr;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);

            //            var indate = new Date(DtTxt);
            //            var cdate = new Date(DtCurr);

            if (date1 > date2) {
                alert("Date Cannot Be Greater Than Current Date");
                //                $get(CDate).focus();

                return false;
            }
            else {
                return true;
            }
        }
        function openRadWindow(ID, ControlType, name, ScheduleId) {

            var oWnd = radopen("ImmunizationTrackingDialog.aspx?ImmId=" + ID + "&ImmName=" + name + "&SchId=" + ScheduleId);
            // oWnd.Center();
            oWnd.setSize(750, 500);
            oWnd.Center();

        }
        function OnClientClose(oWnd, args) {
            $get('<%=btnclose.ClientID%>').click();
        }


        function SearchPatientOnClientClose(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {

                var RegistrationId = arg.RegistrationId;
                var RegistrationNo = arg.RegistrationNo;
                var EncounterNo = arg.EncounterNo;
                var IsMedicalAlert = arg.IsMedicalAlert;
                $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                $get('<%=hdnEncounterNo.ClientID%>').value = EncounterNo;
                $get('<%=hdnIsMedicalAlert.ClientID%>').value = IsMedicalAlert;
            }
            $get('<%=btnGetInfo.ClientID%>').click();
        }

        function validateMaxLength() {
            var txt = $get('<%=txtAccountNo.ClientID%>');
            if (txt.value > 9223372036854775807) {
                alert("Value should not be more than 9223372036854775807.");
                txt.value = txt.value.substring(0, 12);
                txt.focus();
            }
        }
    </script>
</asp:Content>
