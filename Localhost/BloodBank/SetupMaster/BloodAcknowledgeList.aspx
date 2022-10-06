<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BloodAcknowledgeList.aspx.cs"
    Inherits="BloodAcknowledgeList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Blood Acknowledge List</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
   
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div#gvBagEntryDetailsList{
            overflow-x:auto!important;
            width:100%!important;
        }
        
    </style>
    <script type="text/javascript">
        function returnToParent() {
            var oArg = new Object();
            //oArg.CrossMatchDetailsId = document.getElementById("hdnCrossMatchId").value;            
            oArg.Requisition = document.getElementById("hdnRequisition").value;
            oArg.ComponentID = document.getElementById("hdnComponentID").value;
            oArg.CrossMatchNo = document.getElementById("hdnCrossMatchNo").value;
            oArg.ComponentIssue = document.getElementById("hdnComponentIssueNo").value;
            oArg.CrossMatchId = document.getElementById("hdnCrossMatchId").value;
            oArg.status = 1;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
    <div>

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>

                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-7 col-sm-7 col-xs-7 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                    <div class="col-md-5 col-sm-5 col-xs-5 text-right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                        <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                        <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                    </div>
                </div>

                
                    <div class="row">
                        <div class="col-6 col-md-4">
                            <div class="row p-t-b-5">
                                <div class="col-md-5 col-sm-5">Request Type</div>
                                <div class="col-md-7 col-sm-7">
                                    <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%" CssClass="drapDrowHeight"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value="" />
                                        <asp:ListItem Text="Routine" Value="R" />
                                        <asp:ListItem Text="Urgent" Value="U" />
                                        <asp:ListItem Text="Immediate" Value="I" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-6 col-md-4">
                            <div class="row p-t-b-5">
                                <div class="col-md-5 col-sm-5"><asp:Label ID="lblUHID" runat="server" Text='<%$ Resources:PRegistration, Regno%>'/></div>
                                <div class="col-md-7 col-sm-7"><asp:TextBox ID="txtUHID" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-5 col-sm-5"><asp:Label ID="lblEncounter" runat="server" Text="Encounter" /></div>
                                <div class="col-md-7 col-sm-7"><asp:TextBox ID="txtEncounter" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>


                        
               

                    
                        <div class="col-md-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-5 col-sm-5"><asp:Label ID="lblPatient" runat="server" Text="Patient Name" /></div>
                                <div class="col-md-7 col-sm-7"><asp:TextBox ID="txtPatientName" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-5 col-sm-5"><asp:Label ID="lblfacility" runat="server"  Text="Facility"></asp:Label></div>
                                <div class="col-md-7 col-sm-7"><telerik:RadComboBox ID="ddlLocation" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></telerik:RadComboBox></div>
                                </div>
                            </div>
                        <div class="col-md-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-5 col-sm-5">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                        <asp:TextBox ID="txtSearch" runat="server" Width="100%" MaxLength="20" Visible="false" />
                                    </asp:Panel>
                                    <%-- <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="100px" MaxLength="20"
                                        Visible="false" />
                                    <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />--%>
                                </div>
                                <div class="col-md-7 col-sm-7">
                                    <asp:Label ID="lblTotRecord" runat="server" ForeColor="Red" Visible="true" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-6">
                            <div class="row p-t-b-5" id="tblDate" runat="server" visible="false">
                                <div class="col-md-5 col-sm-5"><asp:Label ID="Label17" runat="server" Text="From" /></div>
                                <div class="col-md-7 col-sm-7">
                                    <div class="row">
                                        <div class="col-sm-5"><telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                        <div class="col-sm-2 label2"><asp:Label ID="Label18" runat="server" SkinID="label" Text="To" /></div>
                                        <div class="col-sm-5"><telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                  
                         </div>

                    <div class="row m-t">
                  <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <telerik:RadGrid OnItemCommand="gvBagEntryDetailsList_OnItemCommand" ID="gvBagEntryDetailsList"
                                    runat="server" Skin="Office2007" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                    AutoGenerateColumns="False" EnableLinqExpressions="false" GridLines="Both" AllowPaging="true"
                                    PageSize="18" AllowAutomaticDeletes="false" OnPageIndexChanged="gvBagEntryDetailsList_OnPageIndexChanged">
                                    <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <ItemStyle Wrap="true" />
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                AllowFiltering="false" HeaderText="Select">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="CrossMatchNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMtno" runat="server" Text='<%#Eval("CrossMatchNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="BagNo" HeaderText="BagNo" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="BagNo" SortExpression="BagNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBagNo" runat="server" Text='<%#Eval("BagNo")%>'></asp:Label>
                                                    <%--                                                         
                                                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value='<%#Eval("CrossMatchId")%>' />
                                                                    --%>
                                                    <asp:HiddenField ID="hdnRequisition" runat="server" Value='<%#Eval("BookingNo")%>' />
                                                    <asp:HiddenField ID="hdnComponentID" runat="server" Value='<%#Eval("CrossMatchComponent")%>' />
                                                    <asp:HiddenField ID="hdnCrossMatchNo" runat="server" Value='<%#Eval("CrossMatchNo")%>' />
                                                    <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value='<%#Eval("CrossIssueNo")%>' />
                                                    <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value='<%#Eval("CrossMatchId")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchComponent" runat="server" Text='<%#Eval("ComponentName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="CrossIssueNo">
                                                <ItemTemplate>
                                                    <asp:Label ID="lCrossIssueNo" runat="server" Text='<%#Eval("CrossIssueNo") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchQty" HeaderText="Qty (ml)" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CrossMatchQty" SortExpression="CrossMatchQty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchQty" runat="server" Text='<%#Eval("Qty")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchedUnits" HeaderText="Units" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="CrossMatchedUnits" SortExpression="CrossMatchedUnits">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchedUnits" runat="server" Text='<%#Eval("Units")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="PatientName" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="PatientName" SortExpression="PatientName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, Regno%>'
                                                ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistration" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText="Encounter No" ShowFilterIcon="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Gender" HeaderText="Gender" ShowFilterIcon="false"
                                                DefaultInsertValue="" DataField="Gender" SortExpression="Gender">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGender" runat="server" Text='<%#Eval("Gender")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ActualCrossMatchComponent_Group" HeaderText="Blood Group"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="ActualCrossMatchComponent_Group"
                                                SortExpression="ActualCrossMatchComponent_Group">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualCrossMatchComponent_Group" runat="server" Text='<%#Eval("ActualCrossMatchComponent_Group")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="CrossMatchDate" HeaderText="CrossMatchDate"
                                                ShowFilterIcon="false" DefaultInsertValue="" DataField="CrossMatchDate" SortExpression="CrossMatchDate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCrossMatchDate" runat="server" Text='<%#Eval("CrossMatchDate","{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>

                                <script type="text/javascript">
                                     
                                </script>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                      </div>
                        <asp:HiddenField ID="hdnCrossMatchId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnComponentID" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnCrossMatchNo" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnComponentIssueNo" runat="server" Value='<%#Eval("ComponentIssueNo")%>' />
                        <asp:HiddenField ID="hdnRegistrationID" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnRegistrationNo" runat="server" />
                        <asp:HiddenField ID="hdnEncounterID" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnEncounterNo" runat="server" />
                        <asp:HiddenField ID="hdnAck" runat="server" Value="0" />
                    </div>
                </div>

                
            </ContentTemplate>
        </asp:UpdatePanel>


        <table cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" valign="top">
                    <asp:UpdatePanel ID="aa14" runat="server">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:TextBox ID="lblPharmacyId" runat="server" Text="0" Width="0" Style="visibility: hidden;"></asp:TextBox>
                            <asp:TextBox ID="txtPatientImageId" runat="server" Style="visibility: hidden;" Text=""></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
