<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="RequisitionReleaseAcknowledge.aspx.cs" Inherits="BloodBank_SetupMaster_RequisitionReleaseAcknowledge" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            input#ctl00_ContentPlaceHolder1_txtMonth {
                width: 110% !important;
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

            function ReleaseRequisitionList_OnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var ReleaseID = arg.ReleaseID;
                    var RequisitionId = arg.RequisitionId;

                    $get('<%=hdnReleaseID.ClientID%>').value = ReleaseID;
                    $get('<%=hdnRequisition.ClientID%>').value = RequisitionId;
                }

                $get('<%=btnGetReleaseInfo.ClientID%>').click();
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
                    <div class="col-md-4 col-sm-4 col-12" id="Td1" runat="server">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" Text="Blood Release Requisition" ToolTip="" /></h2>
                    </div>
                    <div class="col-md-4 col-sm-4 col-12 text-center">
                        <asp:LinkButton ID="lnkBtnSearchReleaseRequisition" runat="server" Text='Search Release Requisition'
                            Font-Underline="false" ToolTip="Search Release Requisition" Font-Bold="true" OnClick="lnkBtnSearchReleaseRequisition_OnClick" />
                        <asp:Button ID="btnCancel" runat="server" ToolTip="Cancel Record" CssClass="btn" Text="Cancel" OnClick="Cancel_OnClick" />
                    </div>
                    <div class="col-md-4 col-sm-4 col-12 text-right">
                        <asp:Button ID="btnNew" runat="server" ToolTip="New Record (F2)" CssClass="btn btn-primary" Text="New "
                            OnClick="btnNew_OnClick" CausesValidation="false" />
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data (F3)" OnClick="btnSaveData_OnClick"
                            ValidationGroup="Save" CssClass="btn btn-primary" Text="Save " />
                        <asp:Button ID="btnPrint" runat="server" ToolTip="Print Data" OnClick="btnPrint_Click"
                            ValidationGroup="print" CssClass="btn btn-primary" Text="Print" Visible="false" />
                        <asp:Button ID="btnclose" Text="Close " runat="server" ToolTip="Close (F8)" CausesValidation="false"
                            CssClass="btn btn-primary" OnClientClick="window.close();" />
                    </div>
                </div>

                <div class="row text-center">
                        <asp:Label ID="lblMessage" style="position:relative;margin:0px;width:100%!important;" ForeColor="Green" Font-Bold="true" runat="server" Text="&nbsp;" />
                    </div>

                <div class="row">
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 text-nowrap">
                                <asp:Label ID="Label1" runat="server" Text="Request No."></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-12" style="position: relative">
                                        <asp:TextBox ID="txtRequestNo" Enabled="false" runat="server" Width="100%"></asp:TextBox>
                                    </div>
                                    <div style="position: absolute; top: 2px; left: 18px;">
                                        <asp:ImageButton ID="btnAddItem" runat="server" ImageUrl="../../Images/search icon.png" ToolTip="Open Search Window" Width="18px" Height="18px" OnClick="btnAddItem_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 text-nowrap">
                                <asp:Label ID="Label2" runat="server" Text="Request Date"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtRequestDate" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4  text-nowrap">
                                <asp:Label ID="Label3" runat="server" Text='<%$ Resources:PRegistration, Regno%>'></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtUHID" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4  text-nowrap">
                                <asp:Label ID="Label4" runat="server" Text="IP No."></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtIPNo" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    
                
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4  text-nowrap">
                                <asp:Label ID="Label5" runat="server" Text="Paient Name"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtPatientName" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 text-nowrap">
                                <asp:Label ID="Label7" runat="server" Text="Patient Age/Sex"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <div class="row">
                                    <div class="col-3 col-sm-3 col-md-3 equalpaddingone ">
                                        <asp:TextBox ID="txtYear" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-2 col-sm-2 col-md-2 equalpaddingtwo">
                                        <asp:TextBox ID="txtMonth" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-2 col-sm-2 col-md-3 equalpaddingthree">
                                        <asp:TextBox ID="txtDays" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-5 col-sm-5 col-md-4 equalpaddingfour">
                                        <asp:TextBox ID="txtSex" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4  text-nowrap">
                                <asp:Label ID="Label9" runat="server" Enabled="False" Text="Ward Name"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtWardNo" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 text-nowrap">
                                <asp:Label ID="Label10" runat="server" Text="Bed No."></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtBedNo" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    
                
                    <div class="col-md-3 col-sm-6 col-4">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 text-nowrap">
                                <asp:Label ID="lblReleaseId" runat="server" Enabled="False" Text="Release Id"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-8 ">
                                <asp:TextBox ID="txtReleaseId" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-9 col-sm-6 col-12 p-t-b-5">
                        <asp:Label ID="Label6" runat="server" Text="Previous Requests List"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <telerik:RadGrid ID="gvData" Width="100%" BorderWidth="0" PagerStyle-ShowPagerText="false"
                        AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                        GridLines="Both" AllowPaging="True" PageSize="7" OnItemDataBound="gvData_ItemDataBound">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="false">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="ComponentName" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Component Name"
                                    ItemStyle-Wrap="true" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComponentName" runat="server" Text='<%#Eval("ComponentName")%>' />
                                        <asp:HiddenField ID="hdnComponentID" runat="server" Value='<%#Eval("ComponentId") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ComponentDefaultMLQty" ShowFilterIcon="false"
                                    DefaultInsertValue="" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                    HeaderText="Ordered Qty" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderQty" runat="server" Text='<%#Eval("OrderedQty")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="RequestingQty" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Requested Qty."
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestingQty" runat="server" Text='<%#Eval("RequestedQty")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="QtyIssued" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Qty Not Issued"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtyIssued" runat="server" Text='<%#Eval("BalanceQty")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="CountCrossmatch" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Cross Matched Qty"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQtycrossmatched" runat="server" Text='<%#Eval("CountCrossmatch")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="QtytobeReleased" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Qty to be Released"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtQtytobeReleased" SkinID="textbox" Width="100px" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtQtytobeReleased">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
                    </div>
                
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                    <telerik:RadGrid ID="RadGridRequistionReleased" Skin="Office2007" Width="100%" BorderWidth="0" PagerStyle-ShowPagerText="false"
                        AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                        GridLines="Both" AllowPaging="True">
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="false">
                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="true" />
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="ReleaseID" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Released Id"
                                    ItemStyle-Wrap="true" HeaderStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleaseID" runat="server" Text='<%#Eval("ReleaseID")%>' />
                                        <asp:HiddenField ID="hdnReleaseID" runat="server" Value='<%#Eval("ReleaseID") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ReleaseRequestDate" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Release Req. DateTime"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleaseRequestDate" runat="server" Text='<%#Eval("ReleaseRequestDate")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ReleasingQty" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Releasing Qty"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleasingQty" runat="server" Text='<%#Eval("ReleasingQty")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ReleaseRequestBy" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Release Requested By"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleaseRequestBy" runat="server" Text='<%#Eval("ReleaseRequestBy")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ComponentName" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Component"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblComponentName" runat="server" Text='<%#Eval("ComponentName")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ReleaseAcknowledge" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Release Acknowledge(Yes/No)"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleaseAcknowledge" runat="server" Text='<%#Eval("ReleaseAcknowledge")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ReleaseAcknowledgedBy" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Release Acknowledged By"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleaseAcknowledgedBy" runat="server" Text='<%#Eval("ReleaseAcknowledgedBy")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="ReleaseAcknowledgeDatetime" ShowFilterIcon="false" DefaultInsertValue=""
                                    AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith" HeaderText="Release Acknowledge Date"
                                    ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReleaseAcknowledgeDatetime" runat="server" Text='<%#Eval("ReleaseAcknowledgeDatetime")%>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>

                <div class="row form-group">
                    <%-- <asp:UpdatePanel ID="aa14" runat="server">
                        <ContentTemplate>--%>
                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" Skin="Office2007" EnableViewState="false" >
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                    <asp:HiddenField ID="hdnReleaseID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnEncounterNo" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                    <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                    <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                        Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                </div>


                <div class="row form-group">
                        <div class="col-lg-10  col-md-12">
                            <asp:Label ID="lbl_notification" runat="server" Visible="false" Style="font-size: 18px; color: Red; font-weight: bold;" Text="">

                            </asp:Label>

                        </div>
                    </div>




            <div id="dvConfirmCancel" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #fff; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">
                <table width="100%" cellspacing="2" cellpadding="0">
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to Cancel ?" />
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
                            <asp:Button ID="btnNo" SkinID="Button" runat="server" Text="No" OnClick="btnNo_OnClick" />
                        </td>
                        <td align="center"></td>
                    </tr>
                </table>
            </div>

            <div class="container-fluid" style="background-color: #fff;">
                <div class="row">
                    <asp:Button ID="btnGetRequisitionInfo" runat="server" CausesValidation="false" Enabled="true"
                        OnClick="btnGetRequisitionInfo_Click" SkinID="button" Style="visibility: hidden;"
                        Text="Assign" Width="10px" />
                    <asp:Button ID="btnGetReleaseInfo" runat="server" CausesValidation="false" Enabled="true"
                        OnClick="btnGetReleaseInfo_OnClick" SkinID="button" Style="visibility: hidden;"
                        Text="Assign" Width="10px" />
                </div>
            </div>
                
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
