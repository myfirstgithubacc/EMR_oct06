<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true"
    CodeFile="discharge.aspx.cs" Inherits="EMR_ATD_discharge" Title="Discharge" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <link rel="stylesheet" type="text/css" href="../../Include/EMRStyle.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" />

        <script type="text/javascript" language="javascript">
            //                document.attachEvent('onmouseover', getFire); 
            //                document.attachEvent('onkeypress', getFire);
            document.attachEvent('onkeyup', KeysShortcut);

            function KeysShortcut() {

                if (event.keyCode == 119) // f8  Save 
                {
                    document.getElementById('<%=btnsave.ClientID %>').click();
                }
            }
        </script>

        <script language="javascript" type="text/javascript">

            function openWin() {
                // var oWnd = radopen("/EMR/ATD/popupbeddetail.aspx?hdnTextBoxId=" + document.getElementById('<%=hdnregno.ClientID %>').value, "RadWindow1");
                var oWnd = radopen("/EMR/ATD/popupbeddetail.aspx?hdnTextBoxId=C", "RadWindow1");
                oWnd.setSize(800, 610)
                oWnd.center();
            }

            function openWin1() {
                var a, b;
                a = document.getElementById('<%=txtregno.ClientID %>').value;
                b = document.getElementById('<%=hdnregno.ClientID %>').value;

                var oWnd = radopen("/PRegistration/PatientSearch.aspx?TextBoxId=a,&hdnTextBoxId=b,&Mode=R", "RadWindow2");
                oWnd.setSize(1000, 600);
                oWnd.Center();
                //var oWnd = radopen("/PRegistration/PatientSearch.aspx?TextBoxId=" + txtregno.ClientID + "&hdnTextBoxId=" + hdnregno.ClientID + "&Mode=R", "RadWindow2");

            }


            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    var EncounterNo = arg.EncounterNo;
                    var EncounterId = arg.EncounterId;

                    $get('<%=txtregno.ClientID%>').value = RegistrationNo;
                    $get('<%=txtipno.ClientID%>').value = EncounterNo;
                }
                $get('<%=btnfilter.ClientID%>').click();
            }

            function OnClientClose(oWnd) {
                //alert("san");
                $get('<%=btnCheck.ClientID%>').click();
            }


        </script>

    </telerik:RadCodeBlock>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        Discharge Information
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Panel ID="Panelfilter" runat="server" CssClass="row" ScrollBars="None" DefaultButton="btnfilter">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Literal36" runat="server" Text="<%$ Resources:PRegistration, Regno%>" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:TextBox ID="txtregno" runat="server" MaxLength="10" Columns="10" SkinID="textbox"
                                ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:LinkButton ID="lnkipno" runat="server" Font-Bold="true" Text="Enc. No." OnClick="lnkipno_OnClick" />
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                               <asp:TextBox ID="txtipno" runat="server" MaxLength="15" Width="100%" Columns="10" SkinID="textbox">
                            </asp:TextBox>
                            <asp:Button ID="btnfilter" runat="server" Text="Filter" Width="10px" SkinID="Button"
                                OnClick="btnfilter_Click" CausesValidation="false" Style="visibility: hidden;display:none;" />
                                            </div>
                                        </div>
                                    </div>
                        </asp:Panel>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4 text-right">
                        <asp:Button ID="btnnew" runat="server" Text="New" CssClass="btn btn-primary" OnClick="btnew_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnsave_Click"
                            ValidationGroup="Save" AccessKey="S" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary" CausesValidation="false"
                            OnClientClick="window.close();" />
                        <%--onclick="btnClose_Click"--%>
                        <asp:Button ID="btnpasshelp" runat="server" Text="?" CausesValidation="false" SkinID="Button"
                            AccessKey="H" OnClick="btnpasshelp_Click" Visible="False" />
                    </div>
                </div>

                <div class="row" style="background: #F5DEB3;border-top:1px solid #808080;border-bottom:1px solid #808080;">
                    <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                        Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblPatientName1" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                        Font-Bold="true"></asp:Label>
                                    <asp:Label ID="Label23" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                                    <asp:Label ID="Label24" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>
                                    <asp:Label ID="Label25" runat="server" Text="IP No:" SkinID="label" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                        Font-Bold="true"></asp:Label>
                                    <asp:Label ID="Label26" runat="server" Text="Admission Date:" SkinID="label" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-6 m-t">
                        <h2 style="background:#dfe4ec;padding:3px 10px;font-size:14px;font-weight:bold;margin:0px;">
                                    <asp:Label ID="lbldisdetails" runat="server" Text="Discharge Detail"></asp:Label>
                                </h2>
                                <div class="col-md-12 col-sm-12 col-xs-12 border-all">
                                     <asp:Panel ID="Panel1" runat="server">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                 <asp:Label ID="ltrldischargedate" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, dischargedate %>"></asp:Label>
                                                    <span style="color: Red">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                 <telerik:RadDateTimePicker ID="dtptransferdate" runat="server" MinDate="01/01/1900 00:00"
                                                        Calendar-DayNameFormat="FirstLetter" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                                        DateInput-DateDisplayFormat="dd/MM/yyyy HH:mm" Calendar-EnableAjaxSkinRendering="True"
                                                        OnSelectedDateChanged="dtptransferdate_SelectedDateChanged" AutoPostBackControl="Both">
                                                    </telerik:RadDateTimePicker>
                                                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="(DD/MM/YYYY HH:MM)" ForeColor="#CC3300"></asp:Label>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-5 col-sm-5 col-xs-4 text-nowrap">
                                                <asp:Label ID="ltrldischargestatus" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, dischargestatus %>"></asp:Label>
                                                    <span style="color: Red">*</span>
                                            </div>
                                            <div class="col-md-7 col-sm-7 col-xs-8">
                                                <asp:DropDownList ID="ddldischargestatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddldischargestatus_SelectedIndexChanged"
                                                        SkinID="DropDown" Width="100%">
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="btnshowdeath" runat="server" Height="20" ImageUrl="~/Images/ICONS.jpg"
                                                        OnClick="btnshowdeath_Click" Width="20" Visible="False" />
                                            </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label6" runat="server" SkinID="label" Text="Reason"></asp:Label>
                                                    <span id="sp1" runat="server" style="color: Red">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:DropDownList ID="ddlreason" runat="server" AutoPostBack="false" SkinID="DropDown">
                                                        <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Patient Recoverd"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Patient Convenience"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="By Doctor Order"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="Wrong Entry"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="Service Not Performed"></asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-12 col-sm-12 col-xs-12 text-nowrap box-col-checkbox">
                                                <asp:CheckBox ID="chkbed" runat="server" Checked="false" />
                                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="Patient Still on Bed"></asp:Label>
                                            </div>
                                            </div>
                                        </div>
                                    </div>
                                         <div class="row">
                                             <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label27" runat="server" SkinID="label" Text="Facility"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:DropDownList ID="ddlFacility" runat="server" AutoPostBack="false" Width="100%" SkinID="DropDown">
                                                        <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Patient Recoverd" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Patient Convenience" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="By Doctor Order" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="Wrong Entry" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="Service Not Performed" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                            </div>
                                            </div>
                                        </div>
                                             <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="lblDischargeDelayReason" runat="server" SkinID="label" Text="Reason For Delay" Visible="false"></asp:Label>
                                                    <asp:Label runat="server" Style="color: Red" Text="*" ID="lblStarReason" Visible="false"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                 <asp:DropDownList ID="ddlreasonDelay" runat="server" Visible="false" SkinID="DropDown">
                                                    </asp:DropDownList>
                                            </div>
                                            </div>
                                        </div>
                                         </div>
                                         <div class="row">
                                             <div class="col-md-12">
                                                 <asp:RequiredFieldValidator ID="Requiredfield1" runat="server" ControlToValidate="ddlreason"
                                                        InitialValue="0" ErrorMessage="Please Select Reason" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddldischargestatus"
                                                        InitialValue="0" ErrorMessage="Please Select Status" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                             </div>
                                         </div>
                                         </asp:Panel>
                                    </div>
                    </div>

                    <div class="col-md-6 col-sm-6 col-xs-6 m-t">
                        <h2 style="background:#dfe4ec;padding:3px 10px;font-size:14px;font-weight:bold;margin:0px;">
                                    Death Detail
                                </h2>
                                <div class="col-md-12 col-sm-12 col-xs-12 border-all">
                                     <asp:Panel ID="pnldeathdetails" runat="server" Visible="False">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                 <asp:Label ID="ltrldeathdatetime" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, deathdatetime %>"></asp:Label>
                                                    <span style="color: Red">*</span>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                 <telerik:RadDateTimePicker ID="dtpdeathdatetime" runat="server" MinDate="01/01/1900 00:00"
                                                        Calendar-DayNameFormat="FirstLetter" DateInput-DateFormat="dd/MM/yyyy HH:mm"
                                                        DateInput-DateDisplayFormat="dd/MM/yyyy HH:mm" Calendar-EnableAjaxSkinRendering="True">
                                                    </telerik:RadDateTimePicker>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-5 col-sm-5 col-xs-4 text-nowrap">
                                                <asp:Label ID="ltrlbodydisposion" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, disposbody %>"></asp:Label>
                                            </div>
                                            <div class="col-md-7 col-sm-7 col-xs-8">
                                               <asp:DropDownList ID="ddldepositionofbody" runat="server" SkinID="DropDown">
                                                        <asp:ListItem Value="0" Text="Select" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Discgarge Home"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Against Medical Advice"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Transfer To There Care"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="Died More Than 48 hours after Admission"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="Died Post Operative"></asp:ListItem>
                                                        <asp:ListItem Value="6" Text="Died Less Than 48 hours after Admission"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--       <asp:TextBox ID="txtdepositionofbody" runat="server" MaxLength="100" 
                                                        SkinID="textbox" Width="210px"></asp:TextBox>--%>
                                            </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="Mode of Transfer of Body"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                 <asp:TextBox ID="txtmodeoftransfer" runat="server" MaxLength="100" SkinID="textbox"
                                                        Width="210px"></asp:TextBox>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="ltrlbodyrecby" runat="server" SkinID="label" Text="<%$ Resources:PRegistration, bodyrecby %>"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:TextBox ID="txtbodyreceviedby" runat="server" MaxLength="100" SkinID="textbox"
                                                        Width="210px"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                         <div class="row">
                                             <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                <asp:Label ID="Label5" runat="server" SkinID="label" Text="Authorised Burial Permission"></asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                 <asp:TextBox ID="txtauthorised" runat="server" MaxLength="100" SkinID="textbox" Width="210px"></asp:TextBox>
                                            </div>
                                            </div>
                                        </div>
                                             <div class="col-md-6 col-sm-6 col-xs-12">
                                                 <asp:Panel ID="pnlCommonRekarks" runat="server" Visible="False">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                                Remarks
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-xs-8">
                                                <asp:TextBox ID="txtCommonRekarks" runat="server" MaxLength="2000" TextMode="MultiLine" Enabled="false"
                                                        onkeyup="return MaxLenTxt(this, 2000);" Style="min-height: 100px; max-height: 100px; min-width: 450px; max-width: 450px;" />
                                            </div>
                                            </div>
                                                      </asp:Panel>
                                        </div>
                                         </div>
                                         <div class="row">
                                             
                                         </div>
                                         </asp:Panel>
                                    </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:Panel ID="pnlPaymentDetail" runat="server" ScrollBars="None">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="grddeathcause" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                        OnItemCreated="grddeathcause_ItemCreated" OnItemDataBound="grddeathcause_ItemDataBound"
                                        ShowFooter="true" Visible="false">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <MasterTableView TableLayout="Auto">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Doctor Name">
                                                    <HeaderStyle Width="100px" />
                                                    <ItemStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddldoctor" runat="server" CssClass="gridInput" DataSourceID="SQLMode"
                                                            DataTextField="Name" DataValueField="DoctorId" Style="width: 100%;">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <%--  <FooterTemplate>
                                                                <asp:LinkButton ID="lnkAddRow" runat="server" OnClick="lnkAddRow_Click" Text="&lt;strong&gt;+&lt;/strong&gt;"></asp:LinkButton>
                                                            </FooterTemplate>--%>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Death Cause">
                                                    <HeaderStyle Width="200px" />
                                                    <ItemStyle Width="200px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="gridInput" MaxLength="150"
                                                            Width="100%" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                    <asp:SqlDataSource ID="SQLMode" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                                        EnableCaching="true" SelectCommand="SELECT 0 AS DoctorId, 'Select' As Name , '' as sort UNION SELECT Id as DoctorId, isnull(FirstName,'') + ' ' + isnull(MiddleName,'') + ' ' + isnull(Lastname,'') as Name, 'x' as sort  FROM employee WHERE (Employeetype in(1,17)) and Active=1 ORDER BY sort,name"
                                        SelectCommandType="Text"></asp:SqlDataSource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                    <asp:HiddenField ID="hdncWardId" runat="server" />
                        <asp:HiddenField ID="hdncBedcategoryId" runat="server" />
                   
                        <asp:HiddenField ID="hdncBillingCategoryId" runat="server" />
                </div>

                <div class="row">
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:Button ID="btnCheck" Style="visibility: hidden;" Text="Check" runat="server"
                                        OnClick="btnCheck_Click" Height="10" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Label ID="lblmsg" runat="server" Font-Bold="true" ForeColor="Green" Font-Size="10pt"
                                        Font-Names="verdana"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <asp:Panel ID="pnlfunction" runat="server">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Following Fields are mandatory."
                                            ShowMessageBox="True" ShowSummary="False" ValidationGroup="Save" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:RequiredFieldValidator ID="RequiredFieldValreg" runat="server" ControlToValidate="txtregno"
                                        ErrorMessage="Please Enter Registration Id" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValenco" runat="server" ControlToValidate="txtipno"
                                        ErrorMessage="Please Enter IP Number" Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                             </asp:Panel>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <input id="hdnregno" enableviewstate="true" runat="server" type="text" style="visibility: hidden" />
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <telerik:RadWindowManager ID="RadwindMamnager2" Behaviors="Close" VisibleStatusbar="false"
                                    EnableViewState="false" runat="server" Skin="Office2007">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow2" Behaviors="Close" runat="server" OnClientClose="OnClientClose">
                                            <%--NavigateUrl="/PRegistration/PatientSearch.aspx?TextBoxId=a,&hdnTextBoxId=b,&Mode=R"--%>
                                        </telerik:RadWindow>
                                        <telerik:RadWindow ID="RadWindow1" Behaviors="Close" runat="server" OnClientClose="OnClientClose">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4"></div>
                            <div class="col-md-8 col-sm-8 col-xs-8"></div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <asp:Panel ID="patientdetailspnl" runat="server" ScrollBars="None">
                            <%--    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>--%>
                            <div id="dvLogOut" runat="server" visible="false" style="width: 550px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 130px; left: 200px; top: 300px">
                                <table cellpadding="2" cellspacing="2" width="100%">
                                    <tr runat="server" id="tryesno">
                                        <td colspan="4">
                                            <asp:Label ID="lblerrormsg" runat="server" ForeColor="#336600" Font-Bold="True"></asp:Label>
                                            <%--     <asp:Button ID="btnYes" runat="server" Text="Yes" SkinID="Button" CausesValidation="false"
                                        OnClick="btnYes_Click" />&nbsp;--%>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trwardno">
                                        <td>
                                            <asp:Label ID="lblwardno" runat="server" Text="Ward No" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlwardno" runat="server" SkinID="DropDown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="Bed Category" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbedcategory" runat="server" SkinID="DropDown" AutoPostBack="true"
                                                Width="150px" OnSelectedIndexChanged="ddlbedcategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trbedno">
                                        <td>
                                            <asp:Label ID="Label8" runat="server" Text="Bed No" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbedno" runat="server" SkinID="DropDown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Text="Billing Category" SkinID="label"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbillingcategory" runat="server" SkinID="DropDown" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="center">
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                                            <asp:Button ID="btnhide" runat="server" Text="Close" SkinID="Button" CausesValidation="false"
                                                OnClick="btnhide_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--</ContentTemplate>
                       </asp:UpdatePanel>--%>
                        </asp:Panel>
                </div>
                <div class="row">
                    <asp:Panel ID="Paneldivconfirmation" runat="server" ScrollBars="None">
                            <div id="dvConfirm" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                                <table width="100%" cellspacing="2">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Bed Category and Billing Category are diffrent. Do You want to Save ?"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"></td>
                                        <td align="center">
                                            <asp:Button ID="btnYes" SkinID="Button" runat="server" Text="Yes" OnClick="btnYes_OnClick" />
                                            &nbsp;
                                            <asp:Button ID="Button1" SkinID="Button" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" />
                                        </td>
                                        <td align="center"></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="dvConfirmPrint" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000; border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                                <table width="100%" cellspacing="2">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="Label10" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to Print ?"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center"></td>
                                        <td align="center">
                                            <asp:Button ID="btnPrintDischarge" SkinID="Button" runat="server" Text="Yes" OnClick="btnPrintDischarge_OnClick" />
                                            &nbsp;
                                            <asp:Button ID="btnPrintCancel" SkinID="Button" runat="server" Text="Cancel" OnClick="btnPrintCancel_OnClick" />
                                        </td>
                                        <td align="center"></td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnshowdeath" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnsave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnpasshelp" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
