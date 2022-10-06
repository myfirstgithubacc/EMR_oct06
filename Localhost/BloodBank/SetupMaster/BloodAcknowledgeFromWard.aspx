<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="BloodAcknowledgeFromWard.aspx.cs" Inherits="BloodBank_SetupMaster_BloodAcknowledgeFromWard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" />
        <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

        <style type="text/css">
            table.rwTable {
                height: 100vh !important;
            }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow1 {
                height: 100vh !important;
                width: 100vw !important;
                top: 0 !important;
            }

            .btnicon {
                position: absolute;
                left: 18px;
                top: 3px;
            }

            div#ctl00_ContentPlaceHolder1_gvData {
                width: 100% !important;
                overflow-x: auto;
            }
        </style>
        <script lang="JavaScript" type="text/javascript">
            function FillDetailsForComponentRequisitionOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var CrossMatchNo = arg.CrossMatchNo;
                    var RequisitionId = arg.Requisition;
                    var ComponentID = arg.ComponentID;
                    var ComponentIssue = arg.ComponentIssue;
                    var CrossMatchId = arg.CrossMatchId;
                    var Status = arg.status;
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                    $get('<%=hdnCrossMatchNo.ClientID%>').value = CrossMatchNo;
                    $get('<%=hdnComponentID.ClientID%>').value = ComponentID;
                    $get('<%=hdnComponentIssueNo.ClientID%>').value = ComponentIssue;
                    $get('<%=hdnCrossMatchId.ClientID%>').value = CrossMatchId;
                    $get('<%=hdnstatus.ClientID%>').value = Status;
                    
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
                    var CrossMatchId = arg.CrossMatchId;
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                    $get('<%=hdnCrossMatchNo.ClientID%>').value = CrossMatchNo;
                    $get('<%=hdnComponentID.ClientID%>').value = ComponentID;
                    $get('<%=hdnComponentIssueNo.ClientID%>').value = ComponentIssue;
                    $get('<%=hdnCrossMatchId.ClientID%>').value = CrossMatchId;
                                                    
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


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-xs-4" id="Td1" runat="server">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="Component Acknowledge" ToolTip="" /></h2>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-3 text-center">
                        <asp:LinkButton ID="lbtnSearchrBloodAcknowledge" runat="server" Text='Search Blood Acknowledge' CssClass="btn" Font-Bold="true" Font-Underline="false" ToolTip="Bag List" OnClick="lbtnSearchrBloodAcknowledge_Click"></asp:LinkButton>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-5 text-right">
                        <asp:Button ID="btnNew" runat="server" ToolTip="New Record (F2)" CssClass="btn btn-primary" Text="New "
                            OnClick="btnNew_OnClick" CausesValidation="false" />
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data (F3)" OnClick="btnSaveData_OnClick"
                            ValidationGroup="Save" CssClass="btn btn-primary" Text="Save " />
                        <asp:Button ID="btnclose" Text="Close " runat="server" ToolTip="Close (F8)" CausesValidation="false"
                            CssClass="btn btn-primary" OnClientClick="window.close();" />
                    </div>
                </div>

                <div class="row">
                    <asp:Button ID="btnGetCrossMatchInfo" runat="server" CausesValidation="false" Enabled="true"
                        OnClick="btnGetCrossMatchInfo_Click" SkinID="button" Style="visibility: hidden; float: left; height: 0px;"
                        Text="Assign" Width="10px" />
                </div>

                <div class="row text-center">
                    <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
                </div>


                <div class="row">
                    <div class="col-lg-3 col-6 ">
                        <div class="row p-t-b-5">

                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap center">
                                <asp:Label ID="Label33" runat="server" Text="Issue No."></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <div class="row">
                                    <div class="col-md-12" style="position: relative;">
                                        <asp:TextBox ID="txtIssueNo" Enabled="false" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                    <div class="btnicon">
                                        <asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="../../Images/search icon.png" ToolTip="Open Search Window" Width="18px" Height="18px" OnClick="btnAddItem_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-6 ">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap ">
                                <asp:Label ID="Label2" runat="server" Text="Issue Date"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtIssueDate" Enabled="false" Width="100%" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-6 ">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap center">
                                <asp:Label ID="Label5" runat="server" Text="Paient Name"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtPatientName" Enabled="false" Width="100%" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap ">
                                <asp:Label ID="Label6" runat="server" Text="Referred By"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:TextBox ID="txtReferredBy" Enabled="false" Width="100%" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <telerik:RadComboBox ID="ddlComponent" runat="server" Width="100%"
                            AutoPostBack="True" Visible="false" OnSelectedIndexChanged="ddlComponent_SelectedIndexChanged" EmptyMessage="[ Select ]">
                        </telerik:RadComboBox>

                        <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                            AllowFilteringByColumn="false" ShowHeader="true" AllowMultiRowSelection="false"
                            runat="server" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                            GridLines="Both" AllowPaging="false"
                            OnItemDataBound="gvData_ItemDataBound">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView TableLayout="auto" ShowHeadersWhenNoRecords="true">
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
                                            <asp:Label ID="lblBagNo" SkinID="label" runat="server" Text='<%# Eval("BagNumber") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnIssue" runat="server" Value='<%# Eval("Issued") %>' />
                                            <%--<telerik:RadComboBox ID="ddlUnitNumber" AutoPostBack="true" Width="100px" Enabled="false"
                                            DropDownWidth="100px" runat="server" EmptyMessage="[ Select ]">
                                        </telerik:RadComboBox>--%>
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
                                        <asp:Label ID="lblQuantity" SkinID="label" runat="server" Text = '<%#Eval("qty") %>' ></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="CrossMatchDate" HeaderStyle-Width="140px"
                                    ItemStyle-Width="140px" ShowFilterIcon="false" DefaultInsertValue="" DataField="CrossMatchDate"
                                    HeaderText="CrossMatchDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCrossMatchDate" SkinID="label" runat="server" Text = '<%#Eval("CrossMatchDate") %>' ></asp:Label>
                                        <telerik:RadDatePicker ID="dtpCrossMatchDate" Enabled="false" DateInput-Enabled="false"
                                            runat="server" Width="120px" Visible = "false" >
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
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="Issued" HeaderText="Issued">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chbIssued" runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                        
                                  <telerik:GridTemplateColumn UniqueName="Issued" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="Issued" HeaderText="Acknowledged">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAcknowledged" runat="server" />
                                        <asp:HiddenField ID="hdnAcknowledge" runat="server" Value = '<%# Eval("bloodAcknowledged") %>'/>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                      </div>
                    <telerik:RadInputManager ID="RadInputManager1" runat="server">
                        <telerik:NumericTextBoxSetting>
                            <TargetControls>
                                <telerik:TargetInput ControlID="txtCurrentCrossMatchUnits" />
                            </TargetControls>
                        </telerik:NumericTextBoxSetting>
                    </telerik:RadInputManager>

                </div>
            
            
            <div class="row m-t">
                  <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" Skin="Office2007" EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                   </div>
                                    <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCrossMatchNo" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnComponentID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value="0" />
                                    
                                    
                                    <asp:HiddenField ID="hdnRegistrationID" runat="server" />
                                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                                    <asp:HiddenField ID="hdnEncounterID" runat="server" />
                                    <asp:HiddenField ID="hdnEncounterNo" runat="server"  />
                                    
                                    <asp:HiddenField ID="hdnstatus" runat="server" Value = "0" />
                                    
                                    <asp:Button ID="btnGetRequisitionInfo" runat="server" CausesValidation="false" Enabled="true"
                                        OnClick="btnGetRequisitionInfo_Click" SkinID="button" Style="visibility: hidden;"
                                        Text="Assign" Width="10px" />
                               </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
