<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="AdverseTransfusionReactionWorkupReport.aspx.cs" Inherits="BloodBank_SetupMaster_AdverseTransfusionReactionWorkupReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/EMRStyle.css" rel="stylesheet" />
     <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
   

    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
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
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                    $get('<%=hdnCrossMatchNo.ClientID%>').value = CrossMatchNo;
                    $get('<%=hdnComponentID.ClientID%>').value = ComponentID;
                    $get('<%=hdnComponentIssueNo.ClientID%>').value = ComponentIssue;
                    $get('<%=hdnTransfusionId.ClientID%>').value = TransfusionId;
                    $get('<%=hdnReturnId.ClientID%>').value = ReturnId;
                    $get('<%=hdnIssue.ClientID%>').value = Issue;
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
            <div style="overflow-y: hidden !important; overflow-x: hidden !important;">

            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-4  col-12" id="Td1" runat="server"><h2><asp:Label ID="lblHeader" runat="server" Text="Adverse Transfusion Reaction Workup Report" ToolTip=""  /></h2></div>
                    <div class="col-md-5 col-12">
                         <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" Width-="100%" Text="&nbsp;" />
                    </div>
                <div class="col-md-3 col-12 text-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New Record(F2)" CssClass="btn btn-primary" Text="New " OnClick="btnNew_OnClick" CausesValidation="false" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data(F3)" OnClick="btnSaveData_OnClick" ValidationGroup="Save" CssClass="btn btn-primary" Text="Save " />
                    <asp:Button ID="btnclose" Text="Close  " runat="server" ToolTip="Close(F8)" CausesValidation="false" CssClass="btn btn-primary" OnClientClick="window.close();" />
                    <asp:Button ID="btnGetCrossMatchInfo" runat="server" CausesValidation="false" Enabled="true"
                OnClick="btnGetCrossMatchInfo_Click" SkinID="button" Style="visibility: hidden; float:left; height:0px;" Text="Assign" Width="10px" />

                </div>
            </div>


                <div class="row">
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label33" runat="server" Text="Issue No."></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12">
                                <div class="row">
                                     <div class="col-md-12 col-sm-12 col-12 " style="position:relative;"><asp:TextBox ID="txtIssueNo" runat="server" Width="100%" CssClass="drapDrowHeight"></asp:TextBox></div>
                                     <div style="position:absolute;left: 18px; top: 2px;" ><asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="../../Images/search%20icon.png" ToolTip="Open Search Window" Width="18px" Height="18px" OnClick="btnAddItem_Click" /></div>
                                </div>
                            </div>
                        </div>
                    </div>
                   
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label12" runat="server" Text="Request No"></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtRequestNo" Enabled="false" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label2" runat="server" Text="Issue Date"></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtIssueDate" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
   
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><span class="textName"><asp:Label ID="Label5" runat="server" Text="Paient Name"></asp:Label></span></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtPatientName" Enabled="false" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label3" runat="server" Text="UHID"></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtUHID" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label4" runat="server" Text="IP No."></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtIPNo" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
   
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label9" runat="server" Enabled="false" Text="Ward No."></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtWardNo" Enabled="false" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><asp:Label ID="Label10" runat="server" Text="Bed No."></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtBedNo" Enabled="false" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12">Age / Sex</div>
                             <div class="col-md-8 col-sm-8 col-12">
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-3 no-p-r"><asp:TextBox ID="txtYear" Width="100%" placeholder="Year" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                                    <div class="col-md-3 col-sm-3 col-3"><asp:TextBox ID="txtMonth" Width="100%" placeholder="Month" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                                    <div class="col-md-2 col-sm-2 col-2 no-p-l"><asp:TextBox ID="txtDays" Width="100%" placeholder="Days" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                                    <div class="col-md-1 col-sm-1 col-1 no-p-l no-p-r text-center">/</div>
                                    <div class="col-md-3 col-sm-3 col-3 no-p-l"><asp:TextBox ID="txtGender" Width="100%" placeholder="Gender" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12 text-nowrap"><asp:Label ID="Label20" runat="server" Text="Pretransfusion Sample"></asp:Label></div>
                             <div class="col-md-8 col-sm-8 col-12"><telerik:RadComboBox ID="ddlBloodGroup1" runat="server" Width="100%" CssClass="drapDrowHeight" Filter="Contains" EmptyMessage="[ Select ]" /></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><span class="textName"><asp:Label ID="Label22" runat="server" Text="Post Transfusion Sample"></asp:Label></span></div>
                             <div class="col-md-8 col-sm-8 col-12"><telerik:RadComboBox ID="ddlBloodGroup2" runat="server" Width="100%" CssClass="drapDrowHeight" Filter="Contains" EmptyMessage="[ Select ]" /></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12"><span class="textName"><asp:Label ID="Label23" runat="server" Text="Blood Bag"></asp:Label></span></div>
                            <div class="col-md-8 col-sm-8 col-12"><asp:TextBox ID="txtBloodBag" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>

                    <div class="col-md-6 col-sm-6 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-12 text-nowrap"><asp:Label ID="Label13" runat="server" Text="No. of Blood Units Issued"></asp:Label></div>
                            <div class="col-md-8 col-sm-5 col-12"><asp:TextBox ID="txtNumberBloodUnitsIssued" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-12 text-nowrap"><asp:Label ID="Label14" runat="server" Text="No. of Blood Units Trasfused"></asp:Label></div>
                            <div class="col-md-8 col-sm-7 col-12"><asp:TextBox ID="txtNumberBloodUnitsTransfused" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>

                    <div class="col-md-6 col-sm-6 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-5 col-sm-5 col-12 text-nowrap"><asp:Label ID="Label19" runat="server" Text="Units to which Trasfusion Reaction Occured"></asp:Label></div>
                            <div class="col-md-7 col-sm-7 col-12"><asp:TextBox ID="txtUnitstowhichTrasfusionReactionOccured" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-6">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 col-12 text-nowrap"><asp:Label ID="Label21" runat="server" Text="Volume Transfused"></asp:Label></div>
                            <div class="col-md-8 col-sm-7 col-12"><asp:TextBox ID="txtVolumeTransfused" Width="100%" CssClass="drapDrowHeight" Enabled="false" runat="server"></asp:TextBox></div>
                        </div>
                    </div>
                </div>

           <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-12">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="border:1px solid #ccc;">
                            <div class="row">
                                <h2 style="background: #96bdc7;color: #000;font-size: 13px;padding:3px 10px;margin:0px;float:left;width:100%;">
                                  <asp:Label ID="Label24" runat="server" Text="Compatibility Testing Major"></asp:Label>
                                </h2>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12 gridview p-t-b-5">
                                    <table class="table table-bordered" style="background-color:#fff; margin:0;">
                <thead>
                    <tr>
                        <th>Sample</th>
                        <th>+4°C Saline</th>
                        <th>+22°C Saline</th>
                        <th>+37°C Enzyme</th>
                        <th>+37°C Albumin</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>Pre Transfusion</td>
                        <td><asp:TextBox ID="txtPR4Saline" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtPR22Saline" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtPR37Enzyme" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtPR37Albumin" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                    </tr>

                    <tr>
                        <td>Post Transfusion</td>
                        <td><asp:TextBox ID="txtPO4Saline" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtPO22Saline" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtPO37Enzyme" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                        <td><asp:TextBox ID="txtPO37Albumin" Width="100%" CssClass="drapDrowHeight" runat="server"></asp:TextBox></td>
                    </tr>

                </tbody>
            </table>
                                </div>
                                </div>
                            </div>
                        </div>
               </div>

           <div class="row m-t">
               <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12 text-nowrap">
                                    <asp:Label ID="Label25" runat="server" Text="Minor"></asp:Label><span style="color: Red">*</span>
                                </div>
                             <div class="col-md-8 col-sm-8 col-12">
                                 <telerik:RadComboBox ID="ddlMinor" runat="server" Width="100%" EmptyMessage="[ Select ]">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Compatible" Value="1" />
                                        <telerik:RadComboBoxItem Text="Incompatible" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                             </div>
                        </div>
                    </div>
               <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-12 text-nowrap">
                                    <asp:Label ID="Label18" runat="server" Text="Conclusion"></asp:Label>
                                </div>
                             <div class="col-md-8 col-sm-8 col-12">
                                 <asp:TextBox ID="txtConclusion" runat="server" Width="100%"></asp:TextBox>
                             </div>
                        </div>
                    </div>
           </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <telerik:RadGrid ID="gvDetails" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                         ShowFooter="false" GridLines="Both" AllowPaging="false"
                        AllowSorting="true" AllowFilteringByColumn="false" Width="100%" OnItemDataBound="gvDetails_ItemDataBound">
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                            Scrolling-UseStaticHeaders="true" EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                        </ClientSettings>
                        <PagerStyle ShowPagerText="true" />
                        <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false"
                            Width="100%">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="ParameterName" HeaderText="Parameter Name"
                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="ParameterName" HeaderStyle-Width="300px"
                                    ItemStyle-Width="300px" SortExpression="ParameterName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblParameterName" runat="server" Text='<%#Eval("Parameter_Name")%>'></asp:Label>
                                        <asp:HiddenField ID="hdnParameter_Code" runat="server" Value='<%#Eval("Parameter_Code")%>' />
                                        <asp:HiddenField ID="hdnIdentification" runat="server" Value='<%#Eval("Identification")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Value" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                    HeaderText="Value" ShowFilterIcon="false" DefaultInsertValue="" DataField="Value"
                                    SortExpression="Value">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnRenderControl" Value='<%#Eval("RenderControl")%>' runat="server" />
                                        <telerik:RadTextBox ID="radTxt" Width="150px" runat="server">
                                        </telerik:RadTextBox>
                                        <telerik:RadComboBox ID="radCmb" Width="150px" runat="server" EmptyMessage="[ Select ]">
                                            <%--<Items>
                                                <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                                <telerik:RadComboBoxItem Text="No" Value="0" />
                                            </Items>--%>
                                        </telerik:RadComboBox>
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
                    <%-- <asp:UpdatePanel ID="aa14" runat="server">
                        <ContentTemplate>--%>
                            <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" runat="server" EnableViewState="false">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                            <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                            <asp:HiddenField ID="hdnRequisition" runat="server" />
                            <asp:HiddenField ID="hdnCrossMatchId" runat="server" />
                            <asp:HiddenField ID="hdnCrossMatchNo" runat="server" />
                            <asp:HiddenField ID="hdnComponentID" runat="server" />
                            <asp:HiddenField ID="hdnComponentIssueNo" runat="server" />
                            <asp:HiddenField ID="hdnTransfusionId" runat="server" />
                            <asp:HiddenField ID="hdnReturnId" runat="server" />
                            <asp:HiddenField ID="hdnIssue" runat="server" />
                            <asp:Button ID="btnGetRequisitionInfo" runat="server" CausesValidation="false" Enabled="true" OnClick="btnGetRequisitionInfo_Click" SkinID="button" Style="visibility: hidden; height:auto;" Text="Assign" Width="10px" />
                            <asp:HiddenField ID="hdnDonorRegistrationId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnDonorRegistrationNo" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnExamId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnOPID" runat="server" />
                </div>


            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>