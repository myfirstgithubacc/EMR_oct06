<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="BloodTransfusionDetails.aspx.cs" Inherits="BloodBank_SetupMaster_BloodTransfusionDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link href="../../Include/EMRStyle.css" rel="stylesheet" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" />
        <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>
        <script src="../../Include/bootstrap4/js/bootstrap.min.js"></script>

        <style type="text/css">
            th.rgHeader {
                padding: 2px 5px !important;
            }

            div#ctl00_ContentPlaceHolder1_gvData {
                overflow-x: auto;
                width: 100% !important;
            }

            table#ctl00_ContentPlaceHolder1_gvData_ctl00 {
                table-layout: auto !important;
                white-space: nowrap !important;
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
                    var ComponentIssueId = arg.ComponentIssueId;
                    var CrossMatchId = arg.CrossMatchId;
                    var TransfusionId = arg.TransfusionId;
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                    $get('<%=hdnCrossMatchId.ClientID%>').value = CrossMatchId;
                    $get('<%=hdnCrossMatchNo.ClientID%>').value = CrossMatchNo;
                    $get('<%=hdnComponentID.ClientID%>').value = ComponentID;
                    $get('<%=hdnComponentIssueNo.ClientID%>').value = ComponentIssue;
                    $get('<%=hdnComponentIssueId.ClientID%>').value = ComponentIssueId;
                    $get('<%=hdnPatientTransfusionID.ClientID%>').value = TransfusionId;

                }
                $get('<%=btnGetCrossMatchInfo.ClientID%>').click();
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


    <div style="overflow-y: hidden !important; overflow-x: hidden !important; background: #fff !important;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                        <div class="col-md-4 col-sm-12 col-xs-12" id="Td1" runat="server">
                            <h2>
                                <asp:Label ID="lblHeader" runat="server" Text="Blood Transfusion Details" ToolTip="" /></h2>
                        </div>
                        <div class="col-md-4 col-sm-3 col-xs-3 text-center">
                            <asp:LinkButton ID="lbtnSearchrBloodTransfusion" runat="server" CssClass="btn" Text='Search Blood Transfusion'
                                Font-Underline="false" ToolTip="Bag List" Font-Bold="true" OnClick="lbtnSearchrBloodTransfusion_Click"></asp:LinkButton>
                        </div>
                        <div class="col-md-4 col-sm-9 col-xs-9 text-right">
                            <asp:Button ID="btnNew" runat="server" ToolTip="New Record (F2)" CssClass="btn btn-primary" Text="New " OnClick="btnNew_OnClick" CausesValidation="false" />
                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data (F3)" OnClick="btnSaveData_OnClick" ValidationGroup="Save" CssClass="btn btn-primary" Text="Save " />
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close  (F8)" CausesValidation="false" CssClass="btn btn-primary" OnClientClick="window.close();" />
                            <button type="button" class="btn btn-primary" data-toggle="collapse" data-target="#Ensure">Ensure</button>
                        </div>
                    </div>

            <asp:Button ID="btnGetCrossMatchInfo" runat="server" CausesValidation="false" Enabled="true"
                OnClick="btnGetCrossMatchInfo_Click" SkinID="button" Style="visibility: hidden; height:1px; float:left;"
                Text="Assign" Width="10px" />


                <div class="row text-center">
                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
            </div>


                    <div class="container-fluid">
                        <div class="row form-groupTop01">
                            <div class="col-md-3 col-sm-6 col-6">
                                <div class="row">
                                    <div class="col-md-4 col-sm-4 col-4 label2 ">
                                        <asp:Label ID="Label33" runat="server" Text="Issue No."></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-8">
                                        <div class="row">
                                            <div class="col-sm-12 " style="position: relative;">
                                                <asp:TextBox ID="txtIssueNo" Enabled="false" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:TextBox>
                                            </div>
                                            <div style="position: absolute; left: 19px; top: 2px;">
                                                <asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="../../Images/search%20icon.png" ToolTip="Open Search Window" Width="18px" Height="18px" OnClick="btnAddItem_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-6">
                                <div class="row">
                                    <div class="col-md-4 col-4 label2 PaddingRightSpacing">
                                        <asp:Label ID="Label2" runat="server" Text="Issue Date"></asp:Label>
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtIssueDate" Enabled="false" runat="server" CssClass="drapDrowHeight" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-6">
                                <div class="row">
                                    <div class="col-md-4 col-4 label2 PaddingRightSpacing">
                                        <span class="textName">
                                            <asp:Label ID="Label5" runat="server" Text="Paient Name"></asp:Label></span>
                                    </div>
                                    <div class="col-md-8 col-8">
                                        <asp:TextBox ID="txtPatientName" Enabled="false" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3 col-6">
                                <div class="row">
                                    <div class="col-md-5 col-5 label2">
                                        <span class="textName">
                                            <asp:Label ID="Label6" runat="server" Text="Patient Blood Group"></asp:Label></span>
                                    </div>
                                    <div class="col-md-7 col-7">
                                        <asp:TextBox ID="txtPatientBloodGroup" Enabled="false" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6 col-sm-12">
                                <div class="row">
                                    <div class="col-md-2 col-2 label2">
                                        <asp:Label ID="Label1" runat="server" Text="Remarks"></asp:Label>
                                    </div>
                                    <div class="col-md-10 col-10">
                                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="100%" Columns="50"></telerik:RadTextBox>
                                    </div>
                                </div>
                            </div>

                            <div id="Ensure" class="collapse col-md-6 col-sm-12 PaddingRightSpacing">
                                <div class="row form-groupTop01">
                                    <div class="PD-TabRadioNew01 margin_z">
                                        <asp:CheckBox ID="chkEnsureTransfusion" runat="server" Text="Ensure Transfusion notes are completed" />
                                    </div>
                                </div>
                                <div class="row form-groupTop01">
                                    <div class="PD-TabRadioNew01 margin_z">
                                        <asp:CheckBox ID="chkUnitsReceived" runat="server" Text="Ensure units received have been checked for all details" />
                                    </div>
                                </div>
                                <div class="row form-groupTop01">
                                    <div class="PD-TabRadioNew01 margin_z">
                                        <asp:CheckBox ID="chkBaseLineVitals" runat="server" Text="Ensure baseline vitals of patients are taken and recorded" />
                                    </div>
                                </div>
                                <div class="row form-groupTop01">
                                    <div class="PD-TabRadioNew01 margin_z text-left">
                                        <asp:CheckBox ID="chkUnitsChecked" runat="server" Text="Ensure every unit is checked any kind of leakage/Clot/Haemolysis or any other abnormalty" />
                                    </div>
                                </div>
                            </div>


                </div>

                        <div class="row" style="margin-top: 5px;">
                            <telerik:RadComboBox ID="ddlComponent" runat="server" Width="100%" AutoPostBack="True"
                                Visible="false" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" EmptyMessage="[ Select ]">
                            </telerik:RadComboBox>
                            <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" Width="100%" PagerStyle-ShowPagerText="false"
                                AllowFilteringByColumn="false" ShowHeader="true" AllowMultiRowSelection="false"
                                runat="server" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                GridLines="Both" AllowPaging="false" OnItemDataBound="gvData_ItemDataBound">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                    <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
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
                                                <asp:HiddenField ID="hdnComponentID" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnBloodGroupID" runat="server" Value="0" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="ComponentName" ShowFilterIcon="false" HeaderStyle-Width="100px"
                                            ItemStyle-Width="100px" DefaultInsertValue="" DataField="ComponentName" HeaderText="Component Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComponentName" SkinID="label" runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="UnitNumber" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                            ShowFilterIcon="false" DefaultInsertValue="" DataField="UnitNumber" HeaderText="Bag Number">
                                            <ItemTemplate>
                                                <%-- <telerik:RadComboBox ID="ddlUnitNumber" AutoPostBack="true" Width="100px" Enabled="false"
                                            DropDownWidth="100px" runat="server" EmptyMessage="[ Select ]">
                                        </telerik:RadComboBox>--%>
                                        <asp:Label ID="lblUnitNumber" SkinID="label" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Qty" ShowFilterIcon="false" HeaderStyle-Width="50px"
                                    ItemStyle-Width="50px" DefaultInsertValue="" DataField="Quantity" HeaderText="Qty (ml)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuantity" SkinID="label" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ActualCrossMatchComponent_Group" ShowFilterIcon="false"
                                    HeaderStyle-Width="70px" ItemStyle-Width="60px" DefaultInsertValue="" DataField="ActualCrossMatchComponent_Group"
                                    HeaderText="Blood Group">
                                    <ItemTemplate>
                                        <asp:Label ID="lblActualCrossMatchComponent_Group" SkinID="label" runat="server" Visible ="false"></asp:Label>
                                        <asp:Label ID="lblBloodGroup" SkinID="label" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="TransfusionStart" HeaderStyle-Width="120px"
                                    ItemStyle-Width="100px" ShowFilterIcon="false" DefaultInsertValue="" DataField="TransfusionStart"
                                    HeaderText="Transfusion Start">
                                    <ItemTemplate>
                                        <telerik:RadComboBox ID="ddlTransfusionStart" AutoPostBack="true" Width="80px"
                                            DropDownWidth="80px" runat="server">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                <telerik:RadComboBoxItem Text="No" Value="0" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="StartTime" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" HeaderText="Start Time">
                                    <ItemTemplate>
                                        <telerik:RadTimePicker ID="dtpStartTime" runat="server" Width="100px">
                                        </telerik:RadTimePicker>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="EndTime" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" HeaderText="End Time">
                                    <ItemTemplate>
                                        <telerik:RadTimePicker ID="dtpEndTime" runat="server" Width="100px">
                                        </telerik:RadTimePicker>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Reaction" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" HeaderText="Reaction">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtReaction" TextMode="MultiLine" Width="110px" runat="server">
                                        </telerik:RadTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                        
                                <telerik:GridTemplateColumn UniqueName="Reason" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" HeaderText="Reason">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtReason" TextMode="MultiLine" Width="110px" runat="server">
                                        </telerik:RadTextBox>
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


                <div class="row">
                    <div class="col-sm-12 col-md-12">
                        <%-- <asp:UpdatePanel ID="aa14" runat="server">
                                <ContentTemplate>--%>
                                    <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" runat="server" EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" Skin="Office2007" runat="server" Behaviors="Close" InitialBehaviors="Maximize">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                                    <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCrossMatchNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnComponentID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnComponentIssueNo" runat="server" />
                                    <asp:HiddenField ID="hdnRegistrationID" runat="server"  />
                                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                                    <asp:HiddenField ID="hdnEncounterID" runat="server"  />
                                    <asp:HiddenField ID="hdnEncounterNo" runat="server"  />
                                    
                                    
                                    <asp:HiddenField ID="hdnPatientBloodGroupID" runat="server" />
                                    <asp:HiddenField ID="hdnPatientTransfusionID" runat="server" />
                                    
                                    <asp:HiddenField ID="hdnComponentIssueId" runat="server" />
                                    
                                    <asp:Button ID="btnGetRequisitionInfo" runat="server" CausesValidation="false" Enabled="true"
                                        OnClick="btnGetRequisitionInfo_Click" SkinID="button" Style="visibility: hidden;"
                                        Text="Assign" Width="10px" />
                    </div>
                </div>


            </div>



        </ContentTemplate>
    </asp:UpdatePanel>

</div>
</asp:Content>
