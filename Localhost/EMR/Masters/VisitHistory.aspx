<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="VisitHistory.aspx.cs" Inherits="EMR_Masters_VisitHistory" Title="Visit History" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

        <script type="text/javascript">

            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {

                    var hdnRegId = arg.RegistrationId;
                    var hdnRegNo = arg.RegistrationNo;
                    $get('<%=hdnRegId.ClientID%>').value = hdnRegId;
                    $get('<%=txtRegNo.ClientID%>').value = hdnRegNo;

                }
                $get('<%=ibtnShowDetails.ClientID%>').click();
            }
            //            function RunEXE() {
            //               set wshell = CreateObject("WScript.Shell") 
            //                wshell.run "%COMSPEC% /C dir c:\ > c:\dir.txt", 0, TRUE 
            //                set wshell = nothing 
            //             
            //                set fso = CreateObject("Scripting.FileSystemObject") 
            //                set fs = fso.openTextFile("c:\dir.txt", 1, TRUE) 
            //                response.write replace(replace(fs.readall,"<","<"),vbCrLf,"<br>") 
            //                fs.close: set fs = nothing: set fso = nothing 
            //            }
        </script>

    </telerik:RadScriptBlock>
    <style type="text/css">
        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol {
            background: #c1e5ef !important;
            border: 1px solid #98abb1 !important;
            border-top: none !important;
            color: #333 !important;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>

            <div class="container-fluid header_main form-group">
                <div class="col-md-2">
                    <h2>Visit History</h2>
                </div>
                <div class="col-md-5 text-center">
                    <div class="row" style="display: none;">
                        <div class="col-md-2">
                            <asp:Label ID="Label4" runat="server" Text="Templates"></asp:Label></div>
                        <div class="col-md-6">
                            <telerik:RadComboBox ID="ddlTemplates" SkinID="DropDown" runat="server" Width="100%"
                                EmptyMessage="/Select">
                            </telerik:RadComboBox>
                        </div>
                        <div class="col-md-4 row">
                            <asp:Button ID="btnViewAll" runat="server" CssClass="btn btn-default" Text="View All Case Sheet"
                                OnClick="btnViewAll_Click" />

                        </div>
                    </div>
                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                </div>
                <div class="col-md-5 text-right">
                    <div style="display: none;">
                        <asp:Button ID="btniCCa" runat="server" OnClick="btnicca_OnClick" Text="ICCA Client" CssClass="btn btn-default" />
                        <asp:Button ID="btnReferral" runat="server" OnClick="btnReferral_OnClick" Text="Referral Request" CssClass="btn btn-default" />
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download Old CaseNotes" CausesValidation="false" CssClass="btn btn-primary" OnClick="lnkDownload_OnClick"></asp:LinkButton>&nbsp;
                    </div>
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" OnClientClick="window.close();" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-12">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row form-group">
                    <asp:UpdatePanel ID="updpnl" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRegNo" runat="server" Width="100%" DefaultButton="ibtnShowDetails">
                                <div class="col-md-2">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text="<%$ Resources:PRegistration, regno%>"
                                                Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" MaxLength="15" Width="80%"></asp:TextBox>
                                            <asp:HiddenField ID="hdnRegId" runat="server" />
                                            <asp:Button ID="ibtnShowDetails" runat="server" OnClick="ibtnShowDetails_OnClick" Text="Search" Style="visibility: hidden; float: left; width: 1%;" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ibtnShowDetails" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div class="col-md-2">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Label ID="Label3" runat="server" Text="Source" /></div>
                            <div class="col-md-9">
                                <telerik:RadComboBox ID="ddlSource" runat="server" Width="100%" AutoPostBack="false">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="ALL" Value="A" Selected="True" />
                                        <telerik:RadComboBoxItem Text="OPD" Value="O" />
                                        <telerik:RadComboBoxItem Text="IPD" Value="I" />
                                        <telerik:RadComboBoxItem Text="ER" Value="E" />
                                        <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, MHC%>' Value="M" />
                                        <%-- <telerik:RadComboBoxItem Text="Package" Value="P" />--%>
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'></asp:Label></div>
                            <div class="col-md-9">
                                <telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" runat="server"
                                    Width="100%" TabIndex="0" AutoPostBack="false" OnSelectedIndexChanged="ddlProvider_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label></div>
                            <div class="col-md-9">
                                <telerik:RadComboBox ID="ddlrange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged"
                                    SkinID="DropDown" CausesValidation="false" Width="100%">
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4" id="tblDateRange" runat="server">
                        <div class="row">
                            <div class="col-md-2 PaddingRightSpacing">
                                <asp:Label ID="Label1" runat="server" Text="From"></asp:Label></div>
                            <div class="col-md-4">
                                <telerik:RadDatePicker ID="dtpfrmdate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                            </div>
                            <div class="col-md-1 PaddingRightSpacing">
                                <asp:Label ID="Label2" runat="server" Text="To"></asp:Label></div>
                            <div class="col-md-4">
                                <telerik:RadDatePicker ID="dtpTodate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>
                    <%-- <div class="col-md-2">
                        <div class="row">
                            <div class="col-md-3"></div>
                            <div class="col-md-9"></div>
                        </div>
                    </div>--%>
                </div>
                <div class="row form-group">
                    <div class="col-md-12 text-right">
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_Click" />&nbsp;
                        <div style="display: none;">
                            <asp:Button ID="btnView" runat="server" CssClass="btn btn-primary" Text="View" OnClick="btnView_Click" />
                            <asp:LinkButton ID="lnkAlerts" runat="server" CssClass="btn" Text="Patient Alert" OnClick="lnkAlerts_OnClick" Visible="false" />
                            <asp:LinkButton ID="lnkpatientFiles" runat="server" CssClass="btn btn-primary" OnClick="lnkpatientFiles_OnClick" Text="Patient Files"></asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="row form-group">
                    <telerik:RadGrid ID="gvPatientHistory" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                        AllowFilteringByColumn="false" runat="server" Width="100%" AutoGenerateColumns="False"
                        Height="450px" PageSize="18" EnableLinqExpressions="False" AllowPaging="false"
                        AllowMultiRowSelection="true" CellSpacing="0" OnItemCommand="gvPatientHistory_OnItemCommand"
                        OnItemDataBound="gvPatientHistory_OnItemDataBound">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                            Scrolling-SaveScrollPosition="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <PagerStyle ShowPagerText="False" />
                        <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red;">
                                    No Record Found.
                                </div>
                            </NoRecordsTemplate>
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                            </ExpandCollapseColumn>
                            <Columns>

                                <%--<telerik:GridTemplateColumn UniqueName="Check" AllowFiltering="False" ShowFilterIcon="false" 
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderText=" Name"
                                    FilterControlWidth="99%" ItemStyle-Width="10%" HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEncounter" runat="server" Visible="false"/>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllEncounter" OnCheckedChanged="Encounter_CheckedChanged" Font-Bold="true"
                                            runat="server" Text="All" AutoPostBack="true"  Visible="false"/>
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </telerik:GridTemplateColumn>--%>

                                <telerik:GridTemplateColumn UniqueName="OPIP" AllowFiltering="false" ShowFilterIcon="false"
                                    AutoPostBackOnFilter="false" HeaderText="OP/IP" HeaderStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOPIP" runat="server" Text='<%#Eval("OPIP")%>' />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                        <asp:HiddenField ID="hdnOTCount" runat="server" Value='<%#Eval("OTCount") %>' />
                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EncounterNo" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Enc. #" FilterControlWidth="99%" HeaderStyle-Width="8%" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                        <%--<asp:Label ID="lblCriticalIndication" runat="server" Text="*" Visible="false" Font-Bold="True"
                                                                Font-Size="X-Large" ForeColor="Red" />--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Name" FilterControlWidth="99%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EncounterDate" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Date" FilterControlWidth="99%" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("EncounterDate")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="FacilityName" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Facility Name" FilterControlWidth="99%" Visible="true" HeaderStyle-Width="8%"
                                    ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFName" runat="server" Width="98%" Text='<%#Eval("FacilityName")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Doctor" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText='<%$ Resources:PRegistration, Doctor%>' FilterControlWidth="99%" HeaderStyle-Width="15%"
                                    ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctor" runat="server" Width="98%" Text='<%#Eval("DoctorName")%>' />
                                        <asp:HiddenField ID="hdnIsResource" runat="server" Value='<%#Eval("IsResource") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="SeenByDoctor" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Seen By Doctor" FilterControlWidth="99%" HeaderStyle-Width="15%"
                                    ItemStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSeenByDoctor" runat="server" Width="98%" Text='<%#Eval("SeenByDoctor")%>' />
                                        <asp:HiddenField ID="hdnSeenByDoctorId" runat="server" Value='<%#Eval("SeenByDoctorId") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Problem" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="Diagnosis" FilterControlWidth="99%" HeaderStyle-Width="25%" ItemStyle-Width="33%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkProblemName" runat="server" Text='<%#Eval("ProblemDescription")%>'
                                            CommandName="ProblemName" onmouseout="this.style.textDecoration='none';" onmouseover="this.style.textDecoration='underline';"
                                            Font-Strikeout="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="View" AllowFiltering="false" ShowFilterIcon="false"
                                    HeaderText="View" FilterControlWidth="99%" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkview" runat="server" Text="Case sheet" CommandName="Add"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderText="Discharge Summary"
                                    HeaderStyle-Wrap="true" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDischargeSummary" runat="server" Text="Summary" CommandName="DischargeSummary" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center" Visible="false"
                                    ItemStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderText="Health Check up Summary"
                                    HeaderStyle-Wrap="true" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkHealthCheckup" runat="server" Text="Health Checkup" CommandName="HealthCheckup" />
                                        <asp:HiddenField ID="hdnHealthCheckup" runat="server" Value='<%#Eval("HealthCheckup") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="" HeaderStyle-HorizontalAlign="Center" HeaderText="Documents" Visible="false"
                                    ItemStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderStyle-Width="9%"
                                    ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkAttach" runat="server" Text="Documents" CommandName="Attach" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="" ItemStyle-HorizontalAlign="Center" HeaderText="OT Notes" Visible="false"
                                    AllowFiltering="false" HeaderStyle-Width="9%" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOTNotes" runat="server" Text="OT Notes" CommandName="OtNotes" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="" ItemStyle-HorizontalAlign="Center" HeaderText="Images" Visible="false"
                                    AllowFiltering="false" HeaderStyle-Width="80px" ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkImages" runat="server" Text="Images" CommandName="Images" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFilter" />
            <asp:AsyncPostBackTrigger ControlID="ibtnShowDetails" />
            <asp:AsyncPostBackTrigger ControlID="gvPatientHistory" />
            <asp:AsyncPostBackTrigger ControlID="lnkDownload" />
            <asp:AsyncPostBackTrigger ControlID="btnViewAll" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
