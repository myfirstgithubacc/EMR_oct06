<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComponentRequisitionList.aspx.cs" Inherits="ComponentRequisitionList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Component Requisition List</title>
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


    <style type="text/css">
        .style1
        {
            width: 213px;
        }
    select{
        height:22px!important;
    }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>


    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-8 col-sm-8 col-7 text-center"><h2><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></h2></div>
                    <div class="col-md-4 col-sm-4 col-5 text-right">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnSearch_OnClick" />
                        <asp:Button ID="btnClearSearch" runat="server" CssClass="btn btn-primary" Text="Clear Filter" OnClick="btnClearSearch_OnClick" />
                        <asp:Button ID="BtnPrintLabel" Text="Print Label" runat="server" ToolTip="Click to Print Label" Visible="false" CssClass="btn btn-primary" onclick="btnPrintLabel_Click" />
                        <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" onclick="btnCloseW_Click" />
                    </div>
                </div>

                    <div class="row">
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4"><asp:Label ID="lblUHID" runat="server" Text='<%$ Resources:PRegistration, Regno%>' /></div>
                                <div class="col-md-8 col-sm-8 col-8"><asp:TextBox ID="txtUHID" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4"><asp:Label ID="lblEncounter" runat="server" Text="Encounter" /></div>
                               <div class="col-md-8 col-sm-8 col-8"><asp:TextBox ID="txtEncounter" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4"><asp:Label ID="lblPatient" runat="server" Text="Patient Name" /></div>
                                <div class="col-md-8 col-sm-8 col-8"><asp:TextBox ID="txtPatientName" runat="server" Width="100%"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4">Request Type</div>
                                <div class="col-md-8 col-sm-8 col-8">
                                    <asp:DropDownList ID="ddlSearchOn" runat="server" Width="100%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                                        <asp:ListItem Text="" Value="" />
                                        <asp:ListItem Text="Routine" Value="R" />
                                        <asp:ListItem Text="Urgent" Value="U" />
                                        <asp:ListItem Text="Immediate" Value="I" />                                
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4">Status</div>
                                <div class="col-md-8 col-sm-8 col-8">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Text="All" Value="ALL" />
                                <asp:ListItem Text="Request Acknowledged" Value="RAK" />
                                  <asp:ListItem Text="Request UnAcknowledged" Value="RUK" />
                                <asp:ListItem Text="Sample Acknowledged" Value="SAK" />
                                <asp:ListItem Text="Sample UnAcknowledged" Value="SUK" />                                
                            </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-6 col-sm-6 col-6">
                                    <asp:DropDownList ID="drpActiveInActive" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="drpActiveInActive_SelectedIndexChanged">                                
                                        <asp:ListItem Text="Active" Value="A" />
                                        <asp:ListItem Text="Cancelled" Value="I" />
                                    </asp:DropDownList>
                        
                                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                                        <asp:TextBox ID="txtSearch" runat="server" SkinID="textbox" Width="100px" MaxLength="20" Visible="false"/>
                                        <%-- <asp:TextBox ID="txtRegNo" runat="server" SkinID="textbox" Width="100px" MaxLength="20"
                                            Visible="false" />
                                        <AJAX:FilteredTextBoxExtender ID="filteredtextboxextender1" runat="server" Enabled="True"
                                            FilterType="Custom" TargetControlID="txtRegNo" ValidChars="0123456789" />--%></asp:Panel>
                                </div>
                                <div class="col-md-6 col-sm-6 col-6">
                                    <asp:Label ID="lblTotRecord" runat="server" ForeColor="Red" Visible="true" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-6">
                            <div class="row p-t-b-5">
                                <div class="col-md-4 col-sm-4 col-4"><asp:Label ID="lblfacility" runat="server"  Text="Facility"></asp:Label></div>
                                <div class="col-md-8 col-sm-8 col-8"><telerik:RadComboBox ID="ddlLocation" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></telerik:RadComboBox></div>
                            </div>
                            </div>
                        </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="100%" BorderWidth="1"
                            BorderColor="SkyBlue" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    
                                    <telerik:RadGrid ID="gvEncounter" runat="server" Skin="Office2007" BorderWidth="0"
                                        PagerStyle-ShowPagerText="false" PageSize="10" 
                                        AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                        Width="100%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                        GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                        AllowAutomaticDeletes="false" Height="99%" ShowFooter="false" 
                                        AllowSorting="true"  OnItemCommand="gvEncounter_OnItemCommand"  
                                        OnPageIndexChanged="gvEncounter_OnPageIndexChanged" 
                                        onselectedindexchanged="gvEncounter_SelectedIndexChanged" >
                                        <FilterMenu EnableImageSprites="False">
                                        </FilterMenu>
                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                        </ClientSettings>
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
                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                Visible="True">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                Visible="True">
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
                                                    
                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, Regno%>' ShowFilterIcon="false"
                                                        >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistration" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn> 
                                                     
                                                <telerik:GridTemplateColumn UniqueName="EncounterNo" HeaderText="Encounter No" ShowFilterIcon="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                                        <asp:HiddenField  ID="hdnEncounterId" runat="server"  Value='<%#Eval("EncounterId")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>  
                                                                                                      
                                                <telerik:GridTemplateColumn UniqueName="Gender" HeaderText="Gender" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="GenderToShow"
                                                    SortExpression="Gender">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGender" runat="server" Text='<%#Eval("GenderToShow")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                                    
                                                <telerik:GridTemplateColumn UniqueName="RequestType" HeaderText="Request Type" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="RequestType"
                                                    SortExpression="RequestType">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestType" runat="server" Text='<%#Eval("RequestType")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="RequestDate" HeaderText="Request Date" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="RequestDate"
                                                    SortExpression="RequestDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRequestDate" runat="server" Text='<%#Eval("RequestDate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="PatientBloodGroup" HeaderText="Patient Blood Group" ItemStyle-Width="150px"
                                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="PatientBloodGroup" 
                                                    SortExpression="PatientBloodGroup">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientBloodGroup" runat="server" Text='<%#Eval("PatientBloodGroup")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </telerik:GridTemplateColumn>   
                                                
                                                  <telerik:GridTemplateColumn UniqueName="CompatibilityStatus" HeaderText="Compatibility Status" ItemStyle-Width="150px"
                                                    ShowFilterIcon="false" DefaultInsertValue="" DataField="CompatibilityStatus" 
                                                    SortExpression="CompatibilityStatus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCompatibilityStatus" runat="server" Text='<%#Eval("CompatibilityStatus")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="150px" />
                                                </telerik:GridTemplateColumn>                                                  
                                                    
                                                <telerik:GridTemplateColumn UniqueName="ConsentTakenBy" HeaderText="Consent Taken By" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="ConsentTakenBy"
                                                    SortExpression="ConsentTakenBy">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConsentTakenBy" runat="server" Text='<%#Eval("ConsentTakenBy")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn> 
                                                    
                                                <telerik:GridTemplateColumn HeaderText="Entered By" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="EncodedBy">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>'></asp:Label>
                                                    </ItemTemplate>                                                        
                                                </telerik:GridTemplateColumn>  
                                                    
                                                <telerik:GridTemplateColumn HeaderText="Encoded Date" ShowFilterIcon="false"
                                                    DefaultInsertValue="" DataField="EncodedDate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                                                    </ItemTemplate>                                                        
                                                </telerik:GridTemplateColumn>  
                                                                                                       
                                                    
                                                <telerik:GridTemplateColumn UniqueName="PrintLabel"  ShowFilterIcon="false"                                                      
                                                    >
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPrint"  runat="server"  Text="Print Label"  CommandName="Print"      ></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>                                                    
                                                    
                                                    <telerik:GridTemplateColumn UniqueName="Acknowledge" HeaderText="Acknowlege" ShowFilterIcon="false"
                                    DefaultInsertValue="">
                                    <ItemTemplate>
                                            
                                        <asp:LinkButton ID="lblAck"  runat="server"  CommandName="Ack"   Text="Ack" ></asp:LinkButton>
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
                    <div class="row">
                        <telerik:RadWindowManager ID="RadWindowManager1"  EnableViewState="false"   runat="server">
                            <Windows>
                            <telerik:RadWindow  ID="RadWindow1" runat="server"  Behaviors="Close,Move" />
                            </Windows> 
                            </telerik:RadWindowManager> 
                    </div>

               


                <table cellpadding="2" cellspacing="2" border="0">
                  
                      <tr>
                        <td>
                            <table id="tblDate" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                    </td>
                                    <td>
                                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                    </td>
                                    <td class="style1">
                                        <telerik:RadDatePicker ID="txtToDate" runat="server" MinDate="01/01/1900" Width="100px"
                                            DateInput-ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </div>
                <asp:HiddenField ID="hdnRequisition" runat="server" Value="0" />   
                <asp:HiddenField ID="hdnRelease" runat="server" Value='0' />
                <asp:HiddenField ID="hdnEncounterId" runat="server" Value = "0" />
                <asp:HiddenField ID="hdn_EncounterID"  runat="server"   Value="0" /> 
            </ContentTemplate>
        </asp:UpdatePanel>



        <asp:UpdatePanel  ID="updv" runat="server" >
        <ContentTemplate >
        
           <div id="dvConfirm" runat="server" visible="false" style="width: 400px; z-index: 200;
                border-bottom: 1px solid #000000; border-left: 1px solid #000000; border-right: 1px solid #000000;
                border-top: 1px solid #000000; background-color: #C9DFFD; position: absolute;
                bottom: 0; height: 75px; left: 300px; top: 150px">
                <table width="100%" cellspacing="2">
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Label ID="lblConfirm" Font-Size="12px" runat="server" Font-Bold="true" Text="Do you want to Acknowledge?"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Button ID="btnYes" SkinID="Button" runat="server" Text="Yes" OnClick="btnYes_OnClick" />
                            &nbsp;
                            <asp:Button ID="btnCancel" SkinID="Button" runat="server" Text="Cancel" OnClick="btnCancel_OnClick" />
                        </td>
                        <td align="center">
                        </td>
                    </tr>
                </table>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel> 
    </div>
    </form>
</body>
</html>
