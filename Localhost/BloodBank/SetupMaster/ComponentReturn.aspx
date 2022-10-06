<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ComponentReturn.aspx.cs" Inherits="BloodBank_SetupMaster_ComponentReturn" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link rel="stylesheet" type="text/css" href="~/Include/EMRStyle.css" media="all" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    
    <link rel="stylesheet" type="text/css" href="~/Include/css/mainNew.css" media="all" />
        <style type="text/css">
             .btnicon {
                position: absolute;
                left: 18px;
                top: 3px;
            }
             input#ctl00_ContentPlaceHolder1_txtIssueNo{
                 padding-left:25px;
             }
             table#ctl00_ContentPlaceHolder1_gvData_ctl00{
                 table-layout:auto!important;
             }
             div#ctl00_ContentPlaceHolder1_gvData{
                 overflow-x:auto;
             }
        </style>
        <script language="JavaScript" type="text/javascript">
            function FillDetailsForComponentRequisitionOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RequisitionId = arg.Requisition;
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                }

                $get('<%=btnGetRequisitionInfo.ClientID%>').click();
            }

            function SearchComponentIssueDetailsOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var CrossMatchNo = arg.CrossMatchNo;
                    var RequisitionId = arg.Requisition;
                    var ComponentID = arg.ComponentID;
                    var ComponentIssue = arg.ComponentIssue;
                    var TransfusionId = arg.TransfusionId;
                    var ReturnId = arg.ReturnId;
                    var Issue = arg.Issue;
                    var CrossMatchId = arg.CrossMatchId;
                    var ComponentIssueId = arg.ComponentIssueId;
                    //  alert(ComponentIssueId);
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                    $get('<%=hdnCrossMatchNo.ClientID%>').value = CrossMatchNo;
                    $get('<%=hdnComponentID.ClientID%>').value = ComponentID;
                    $get('<%=hdnComponentIssueNo.ClientID%>').value = ComponentIssue;
                    $get('<%=hdnTransfusionId.ClientID%>').value = TransfusionId;
                    $get('<%=hdnReturnId.ClientID%>').value = ReturnId;
                    $get('<%=hdnIssue.ClientID%>').value = Issue;
                    $get('<%=hdnCrossMatchId.ClientID%>').value = CrossMatchId;
                    $get('<%=hdnComponentIssueId.ClientID%>').value = ComponentIssueId;

                }
                $get('<%=btnGetCrossMatchInfo.ClientID%>').click();
            }

            function OnClientIsValidPasswordClose(oWnd, args) {

            var arg = args.get_argument();
            if (arg) {
                var IsValidPassword = arg.IsValidPassword;

                $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
        </script>
        <script type="text/javascript">
            if (window.captureEvents) {
                window.captureEvents(Event.KeyUp);
                window.onkeyup = executeCode;
            }
            else if (window.attachEvent) {
                document.attachEvent('onkeyup', executeCode);
            }

            function executeCode(evt) {
                if (evt == null) {
                    evt = window.event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {
                    case 113:  // F2
                        $get('<%=btnNew.ClientID%>').click();
                         break;
                     case 114: //F3
                         $get('<%=btnSaveData.ClientID%>').click();
                         break;

                     case 119:  // F8
                         $get('<%=btnclose.ClientID%>').click();
                         break;



                 }
                 evt.returnValue = false;
                 return false;
             }
        </script>
    </telerik:RadScriptBlock>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-3 col-sm-3 col-xs-12" id="Td1" runat="server">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="Component Return" ToolTip="" /></h2>
                </div>
                <div class="col-md-6 col-sm-5 col-xs-12 text-center">
                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="" /></div>
                <div class="col-md-3 col-sm-4 col-xs-12 text-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New Record (F2)" CssClass="btn btn-primary" Text="New " OnClick="btnNew_OnClick" CausesValidation="false" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data (F3)" OnClick="btnSaveData_OnClick" ValidationGroup="Save" CssClass="btn btn-primary" Text="Save " />
                    <asp:Button ID="btnclose" Text="Close  " runat="server" ToolTip="Close (F8)" CausesValidation="false" CssClass="btn btn-primary" OnClientClick="window.close();" />
                </div>
            </div>
            <asp:Button ID="btnGetCrossMatchInfo" runat="server" CausesValidation="false" Enabled="true" OnClick="btnGetCrossMatchInfo_Click" SkinID="button" Style="visibility: hidden; float: left; margin: 0; display:none; padding: 0; height: 1px;" Text="Assign" Width="10px" />

                <div class="row">
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label3" runat="server" Text="UHID"></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtUHID" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox>

                            </div>
                        </div>
                    </div>
         

                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label33" runat="server" Text="Issue No."></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <div class="row">
                                    <div class="col-md-12 " style="position:relative">
                                        <asp:TextBox ID="txtIssueNo" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:TextBox></div>
                                    <div class="btnicon PaddingLeftSpacing">
                                        <asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="../../Images/search icon.png" ToolTip="Open Search Window" Width="18px" Height="18px" OnClick="btnAddItem_Click" /></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label2" runat="server" Text="Issue Date"></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtIssueDate" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label1" runat="server" Text="Manual No."></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtManualNo" Enabled="false" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:TextBox></div>
                        </div>
                    </div>
                  
                    
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label4" runat="server" Text="IP No."></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtIPNo" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                                   
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label5" runat="server" Text="Paient Name"></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtPatientName" Enabled="false" CssClass="drapDrowHeight" Width="100%" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label6" runat="server" Text="Referred By"></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtReferredBy" Enabled="false" CssClass="drapDrowHeight" Width="100%" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
               
                        
                    
                 <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label9" runat="server" Enabled="false" Text="Ward No."></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtWardNo" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label10" runat="server" Text="Bed No."></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtBedNo" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName">
                                <asp:Label ID="Label15" runat="server" Text="Patient Blood Group"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtPatientBloodGroup" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
               

               
                    
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName">
                                <asp:Label ID="Label16" runat="server" Text="Mother Blood Group"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtMotherBloodGroup" CssClass="drapDrowHeight" Width="100%" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label11" runat="server" Text="Return Date"></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <telerik:RadDatePicker ID="dtpReturnDate" runat="server" Enabled="false" CssClass="drapDrowHeight" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label17" runat="server" Text="Return By"></asp:Label><span style="color: Red">&nbsp;*</span></div>
                            <div class="col-md-8 col-sm-7">
                                <telerik:RadComboBox ID="ddlReturnBy" runat="server" CssClass="drapDrowHeight" Width="100%" EmptyMessage="[ Select ]">
                                    <%--   <Items>
                                    <telerik:RadComboBoxItem Text="1" Value="1" />
                                    <telerik:RadComboBoxItem Text="2" Value="2" />
                                    <telerik:RadComboBoxItem Text="3" Value="3" />
                                    </Items>--%>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    


                
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label7" runat="server" Text="Reason"></asp:Label><span style="color: Red">*</span></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtReason" Width="100%" Height="40px" TextMode="MultiLine" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2">
                                <asp:Label ID="Label18" runat="server" Text="Remarks"></asp:Label></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtRemarks" Width="100%" Height="40px" TextMode="MultiLine" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName">
                                <asp:Label ID="Label8" runat="server" Text="Blood Discard Quantity"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:TextBox ID="txtDiscardQuantity" Width="100%" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-12 gridview">
                    <telerik:RadGrid ID="gvData" CssClass="table table-condensed" BorderWidth="0" PagerStyle-ShowPagerText="false"
                        AllowFilteringByColumn="false" ShowHeader="true" AllowMultiRowSelection="false"
                        runat="server" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                        GridLines="Both" AllowPaging="false">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True" AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" ShowHeadersWhenNoRecords="true">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="SrNo" HeaderStyle-Width="25px" ItemStyle-Width="25px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="SrNo" HeaderText="Sr. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%# Container.ItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="UnitNumber" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="UnitNumber" HeaderText="Bag Number">
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="ddlUnitNumber" AutoPostBack="true" Width="100px" Enabled="false"
                                            DropDownWidth="100px" runat="server" EmptyMessage="[ Select ]">
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%-- 
                                <telerik:GridTemplateColumn UniqueName="SegmentNo" ShowFilterIcon="false" HeaderStyle-Width="120px"
                                    ItemStyle-Width="120px" DefaultInsertValue="" DataField="SegmentNo" HeaderText="Segment No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSegmentNo" SkinID="label" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                --%>
                                <telerik:GridTemplateColumn UniqueName="Quantity" ShowFilterIcon="false" HeaderStyle-Width="60px"
                                    ItemStyle-Width="60px" DefaultInsertValue="" DataField="Quantity" HeaderText="Quantity (ml)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" SkinID="label" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="CrossMatchDate" HeaderStyle-Width="140px"
                                    ItemStyle-Width="140px" ShowFilterIcon="false" DefaultInsertValue="" DataField="CrossMatchDate"
                                    HeaderText="CrossMatchDate">
                                    <ItemTemplate>
                                        <telerik:RadDatePicker ID="dtpCrossMatchDate" Enabled="false" DateInput-Enabled="false"
                                            runat="server" Width="120px">
                                        </telerik:RadDatePicker>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%-- 
                                <telerik:GridTemplateColumn UniqueName="TestResult" HeaderStyle-Width="130px" ItemStyle-Width="130px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="TestResult" HeaderText="Test Result">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTestResult" SkinID="label" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                --%>
                                <telerik:GridTemplateColumn UniqueName="Issued" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="Issued" HeaderText="Issued" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chbIssued" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Component" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="ComponentName" HeaderText="Component">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComponent" SkinID="label" runat="server" Text='<%# Eval("ComponentName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="BloodGroup" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="PatientBloodGroup" HeaderText="Blood Group">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBloodGroup" SkinID="label" runat="server" Text='<%# Eval("PatientBloodGroup") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Return" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="Issued" HeaderText="Return">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chbReturn" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Cancel" HeaderStyle-Width="25px" ItemStyle-Width="25px"
                                    ShowFilterIcon="false" DefaultInsertValue="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server"
                                            CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%# Bind("ReturnId") %>'
                                            ImageUrl="~/Images/DeleteRow.png" Width="16px" OnClick="ibtnDelete_OnClick" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadInputManager ID="RadInputManager1" runat="server">
                        <telerik:NumericTextBoxSetting>
                            <TargetControls>
                                <telerik:TargetInput ControlID="txtCurrentCrossMatchUnits" />
                            </TargetControls>
                        </telerik:NumericTextBoxSetting>
                    </telerik:RadInputManager>
                </div>
                    </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 gridview">
                        <%-- <asp:UpdatePanel ID="aa14" runat="server">
                                <ContentTemplate>--%>
                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" runat="server" EnableViewState="false">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" Skin="Office2007" runat="server" Behaviors="Close,Move" InitialBehaviors="Maximize">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                        <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>

                        <asp:HiddenField ID="hdnComponentIssueId" runat="server" />
                        <asp:HiddenField ID="hdnRequisition" runat="server" />
                        <asp:HiddenField ID="hdnCrossMatchId" runat="server" />
                        <asp:HiddenField ID="hdnCrossMatchNo" runat="server" />
                        <asp:HiddenField ID="hdnComponentID" runat="server" />
                        <asp:HiddenField ID="hdnComponentIssueNo" runat="server" />
                        <asp:HiddenField ID="hdnTransfusionId" runat="server" />
                        <asp:HiddenField ID="hdnReturnId" runat="server" />
                        <asp:HiddenField ID="hdnIssue" runat="server" />
                        <asp:Button ID="btnGetRequisitionInfo" runat="server" CausesValidation="false" Enabled="true" OnClick="btnGetRequisitionInfo_Click" SkinID="button" Style="visibility: hidden;" Text="Assign" Width="10px" />
                        <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                        <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                            Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
