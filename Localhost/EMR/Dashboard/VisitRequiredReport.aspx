<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="VisitRequiredReport.aspx.cs" Inherits="EMR_Dashboard_VisitRequiredReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/css/font-awesome.css" rel="stylesheet" />


    <script type="text/javascript">
        var CtrlEventKey = "";

        $(function () {
            $(document).pos();
            $(document).on('scan.pos.barcode', function (event) {

                //access `event.code` - barcode data
                //alert(event.code.val);
                var ScannedVal = $('#<%=txtSearch.ClientID %>').val();
                var scanerVal = event.code;
                scanerVal = scanerVal.replace("\r\n", "");
                if (ScannedVal != scanerVal) {
                    $('#<%=txtSearch.ClientID %>').val("");
                    $('#<%=txtSearch.ClientID %>').val(scanerVal);
                    $get('<%=btnSearch.ClientID%>').click();
                }
            });

        });

        document.addEventListener('keydown', function (event) {
            if (event.keyCode == 17 || event.keyCode == 74) {
                if (event.keyCode == 17) {
                    CtrlEventKey = event.keyCode;
                }
                if (CtrlEventKey == 17 && event.keyCode == 74) {
                    CtrlEventKey = "";
                    event.preventDefault();
                }

            }
        });

    </script>

    <script type="text/javascript">
        var CtrlEventKey = "";

        $(function () {
            $(document).pos();
            $(document).on('scan.pos.barcode', function (event) {

                //access `event.code` - barcode data
                //alert(event.code.val);
                var ScannedVal = $('#<%=txtSearchN.ClientID %>').val();
                var scanerVal = event.code;
                scanerVal = scanerVal.replace("\r\n", "");
                if (ScannedVal != scanerVal) {
                    $('#<%=txtSearchN.ClientID %>').val("");
                    $('#<%=txtSearchN.ClientID %>').val(scanerVal);
                    $get('<%=btnSearch.ClientID%>').click();
                }
            });

        });

        document.addEventListener('keydown', function (event) {
            if (event.keyCode == 17 || event.keyCode == 74) {
                if (event.keyCode == 17) {
                    CtrlEventKey = event.keyCode;
                }
                if (CtrlEventKey == 17 && event.keyCode == 74) {
                    CtrlEventKey = "";
                    event.preventDefault();
                }

            }
        });

    </script>
    <div class="WordProcessorDiv" style="background-color: #bde5ee; font-size: small; margin-left: 0;">
        <div class="container-fluid">
            <div class="row">

                <div class="col-md-12 col-sm-12">
                    <div class="WordProcessorDivText">
                        <h5>
                            <asp:Label ID="Label7" runat="server" CssClass="info" Text="Visit Required Report" /></h5>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="update" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="btn_ClearFilter" />
        </Triggers>
        <ContentTemplate>

            <div class="VitalHistory-Div" style="margin-top: 0px;">
                <div class="container-fluid">
                    <div id="pnlsearch" runat="server" visible="true">
                        <div class="row">
                            <div class="col-md-12 text-center">
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text=""></asp:Label>
                            </div>

                        </div>
                        <div class="row">

                            <div class="col-md-3">
                                <span class="findPatientText">
                                    <asp:Label ID="lblPatient" runat="server" Text="Search&nbsp;On" />
                                </span>

                                <telerik:RadComboBox ID="ddlName" CssClass="findPatientSelect-Mobile" runat="server"
                                    AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="R" />
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="ENC" />
                                    </Items>
                                </telerik:RadComboBox>

                                <asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearch">
                                    <asp:TextBox ID="txtSearch" CssClass="findPatientInput-Mobile" runat="server" MaxLength="50"
                                        Visible="false" />
                                    <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" runat="server" Text=""
                                        MaxLength="10" onkeyup="return validateMaxLength();" />
                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                                </asp:Panel>
                            </div>
                            <div class="col-md-3">
                                <span class="findPatientText">From</span>
                                <div id="tblDateRange" runat="server">
                                    <telerik:RadDatePicker ID="dtpfromDate" runat="server" Width="80px" AutoPostBack="false" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                    <span id="spTo" runat="server">&nbsp;To&nbsp;</span>
                                    <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="80px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="Label1" runat="server" Text="Surgery Required" />
                                <telerik:RadComboBox ID="ddlSurgeryRequired" runat="server">
                                    <Items>
                                        <telerik:RadComboBoxItem Text='All' Value="0" />
                                        <telerik:RadComboBoxItem Text='Yes' Value="1" />
                                        <telerik:RadComboBoxItem Text='No' Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                                <asp:Label ID="Label8" runat="server" Text="Admission Required" />
                                <telerik:RadComboBox ID="ddlAdmissionRequired" runat="server">
                                    <Items>
                                        <telerik:RadComboBoxItem Text='All' Value="0" />
                                        <telerik:RadComboBoxItem Text='Yes' Value="1" />
                                        <telerik:RadComboBoxItem Text='No' Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>

                            <div class="col-md-3">
                                <div>
                                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="btn btn-primary btn-chotu"
                                        OnClick="btnSearch_Click" Text="Refresh" Width="95px" UseSubmitBehavior="False" />
                                    <asp:Button ID="btn_ClearFilter" runat="server" CausesValidation="false" CssClass="btn btn-primary btn-chotu"
                                        Text="Reset Filter" Width="95px" OnClick="btn_ClearFilter_Click" UseSubmitBehavior="False" />

                                </div>

                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="VitalHistory-Div01">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:LinkButton ID="lnkbtn_Refresh" runat="server"
                                Text="Refresh" ForeColor="OrangeRed" Visible="false" />
                            <div>
                                <asp:Label ID="Label2" runat="server" Text="Patient Name : " Visible="false"></asp:Label>
                                <asp:Label ID="lblPatienName" runat="server" Font-Bold="true"></asp:Label>

                                <asp:Label ID="Label3" runat="server" Text="Reg No :" Visible="false"></asp:Label>
                                <asp:Label ID="lblRegNo" runat="server" Font-Bold="true"></asp:Label>

                                <asp:Label ID="Label4" runat="server" Text="Enc. No :" Visible="false"></asp:Label>
                                <asp:Label ID="lblEncNo" runat="server" Font-Bold="true"></asp:Label>

                                <asp:Label ID="Label5" runat="server" Text="Bill No :" Visible="false"></asp:Label>
                                <asp:Label ID="lblBillNo" runat="server" Font-Bold="true"></asp:Label>

                                <asp:Label ID="Label6" runat="server" Text="Summary ID :" Visible="false"></asp:Label>
                                <asp:Label ID="lblSummaryId" runat="server" Font-Bold="true"></asp:Label>

                            </div>
                            <asp:Panel ID="pnlGrid" runat="server" Height="530px" ScrollBars="Auto"
                                Width="100%">
                                <asp:HiddenField ID="hdnSummaryIDToDispatch" runat="server" Value="" />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>

                                        <telerik:RadGrid ID="gvEncounter" runat="server" CellFormatting="gvEncounter_CellFormatting" Skin="Office2007" RenderMode="Lightweight" Height="500px" ItemStyle-Height="30px"
                                            AutoGenerateColumns="false" AllowPaging="true" PageSize="20" CellPadding="0" CellSpacing="0" GridLines="None"
                                            AllowMultiRowSelection="false" AllowCustomPaging="false" AllowAutomaticDeletes="false" ShowStatusBar="true" AllowFilteringByColumn="false"
                                            ShowFooter="false" EnableLinqExpressions="false" BorderWidth="0" PagerStyle-ShowPagerText="false" HeaderStyle-Wrap="false"
                                            OnPageIndexChanged="gvEncounter_OnPageIndexChanged" OnPreRender="gvEncounter_OnPreRender"
                                            OnItemDataBound="gvEncounter_OnItemDataBound">
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                                <Scrolling AllowScroll="True" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                                            </ClientSettings>
                                            <MasterTableView AllowFilteringByColumn="false" TableLayout="Fixed" Width="100%">
                                                <SortExpressions>
                                                    <%--<telerik:GridSortExpression FieldName="EncounterId" SortOrder="Descending" />--%>
                                                </SortExpressions>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>'
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText='<%$ Resources:PRegistration, EncounterNo%>'
                                                        HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="EncounterDate" HeaderText="Visit Date" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEncounterDate" runat="server" Text='<%#Eval("EncounterDate") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%#Eval("PatientName") %>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AgeGender" HeaderText="Age/Gender" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="SurgeryRequired" HeaderText="Surgery Required" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSurgeryRequired" runat="server" Text='<%#Eval("SurgeryRequired")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="AdmissionRequired" HeaderText="Admission Required" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAdmissionRequired" runat="server" Text='<%#Eval("AdmissionRequired")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </div>
                    </div>

                </div>
            </div>

            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow runat="server" ID="RadWindow1" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow runat="server" ID="RadWindow2" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

