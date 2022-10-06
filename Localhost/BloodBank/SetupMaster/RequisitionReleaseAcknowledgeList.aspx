<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequisitionReleaseAcknowledgeList.aspx.cs" Inherits="RequisitionReleaseAcknowledgeList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Blood Release Requisition List</title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <script type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.Requisition = document.getElementById("hdnRequisition").value;
            oArg.ReleaseID = document.getElementById("hdnRelease").value;            
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
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
					<div class="col-md-3"><h2 style="color:#333;">Blood Release Requisition List</h2></div>
                    <div class="col-md-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                    <div class="col-md-3 text-right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                        <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                        <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" OnClick="btnCloseW_Click" />
                    </div>
                </div>

                    <div class="row">
                        <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4 text-nowrap">Request Type</div>
                                <div class="col-md-8 col-sm-8 col-8">
                                    <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%" Height="22px" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value="" />
                                        <asp:ListItem Text="Routine" Value="R" />
                                        <asp:ListItem Text="Urgent" Value="U" />
                                        <asp:ListItem Text="Immediate" Value="I" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4 text-nowrap"><asp:Label ID="lblUHID" runat="server" Text="UHID" /></div>
                                <div class="col-md-8 col-sm-8 col-8"><asp:TextBox ID="txtUHID" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4 text-nowrap"><asp:Label ID="lblEncounter" runat="server" Text="Encounter" /></div>
                                <div class="col-md-8 col-sm-8 col-8"><asp:TextBox ID="txtEncounter" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4 text-nowrap"><asp:Label ID="lblPatient" runat="server" Text="Patient Name" /></div>
                                <div class="col-md-8 col-sm-8 col-8"><asp:TextBox ID="txtPatientName" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                                         
                      <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4 text-nowrap"><asp:Label ID="lblfacility" runat="server"  Text="Facility"></asp:Label></div>
                                <div class="col-md-8 col-sm-8 col-8"><telerik:RadComboBox ID="ddlLocation" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></telerik:RadComboBox></div>
                            
                                </div>
                            </div>
                        <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4 text-nowrap">
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                        <asp:TextBox ID="txtSearch" runat="server" SkinID="textbox" Width="100px" MaxLength="20" Visible="false" />
                                    </asp:Panel>
                                    <%-- <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="100px" MaxLength="20"
                                        Visible="false" />
                                    <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True"
                                        FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />--%>
                                </div>
                                <div class="col-md-8"></div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap"><asp:Label ID="lblTotRecord" runat="server" SkinID="label" ForeColor="Red" Visible="true" Font-Bold="true"></asp:Label></div>
                                <div class="col-md-8"></div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-6" id="tblDate" runat="server" visible="false">
                            <div class="row">
                                <div class="col-md-3 label2"><asp:Label ID="Label17" runat="server" Text="From" /></div>
                                <div class="col-md-9">
                                    <div class="row">
                                        <div class="col-md-5 PaddingRightSpacing"><telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                        <div class="col-md-2 label2"><asp:Label ID="Label18" runat="server" Text="To" /></div>
                                        <div class="col-md-5 PaddingLeftSpacing"><telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" /></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="100%" BorderWidth="0"
                            BorderColor="SkyBlue" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvEncounter" runat="server" BorderWidth="0"
                                        PageSize="18" AllowFilteringByColumn="false" AllowMultiRowSelection="false" Width="100%"
                                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                        GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                        AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" AllowSorting="true"
                                        OnItemCommand="gvEncounter_OnItemCommand" OnPageIndexChanged="gvEncounter_OnPageIndexChanged">
                                        <FilterMenu EnableImageSprites="False">
                                        </FilterMenu>
                                        <PagerStyle ShowPagerText="true" />
                                        <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false"
                                            Width="100%">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <EditFormSettings>
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                            </EditFormSettings>
                                            <ItemStyle Wrap="true" />
                                            <CommandItemSettings ExportToPdfText="Export to PDF" />
                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center"
                                                    AllowFiltering="false" HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RequisitionNo" HeaderText='<%$ Resources:PRegistration, reqno%>'
                                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="RequisitionNo" SortExpression="REGID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RequisitionNo")%>'></asp:Label>
                                                        <asp:HiddenField ID="hdnRequisitionId" runat="server" Value='<%#Eval("RequisitionId")%>' />
                                                        <asp:HiddenField ID="hdnReleaseID" runat="server" Value='<%#Eval("ReleaseID")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="PatientName" HeaderText="Patient Name" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="PatientName" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                                    SortExpression="PatientName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="150px" />
                                                    <ItemStyle Width="150px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText="Registration No"
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
                                                    DefaultInsertValue="" DataField="GenderToShow" SortExpression="Gender">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGender" runat="server" Text='<%#Eval("GenderToShow")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RequestType" HeaderText="Request Type" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="RequestType" SortExpression="RequestType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestType" runat="server" Text='<%#Eval("RequestType")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RequestDate" HeaderText="Request Date" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="RequestDate" SortExpression="RequestDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestDate" runat="server" Text='<%#Eval("RequestDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="PatientBloodGroup" HeaderText="Patient Blood Group"
                                                    ItemStyle-Width="150px" ShowFilterIcon="false" DefaultInsertValue="" DataField="PatientBloodGroup"
                                                    SortExpression="PatientBloodGroup">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientBloodGroup" runat="server" Text='<%#Eval("PatientBloodGroup")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="ConsentTakenBy" HeaderText="Consent Taken By"
                                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="ConsentTakenBy" SortExpression="ConsentTakenBy">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConsentTakenBy" runat="server" Text='<%#Eval("ConsentTakenBy")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />
                <asp:HiddenField ID="hdnRelease" runat="server" Value='0' />
                <asp:HiddenField ID="hdnEncounterId" runat="server" Value="0" />
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>