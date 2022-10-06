<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="PatientDashboard.aspx.cs" Inherits="EMR_Dashboard_Default" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="PatientParts/Problems.ascx" TagName="Problems" TagPrefix="uc1" %>
<%@ Register Src="PatientParts/Allergies.ascx" TagName="Allergies" TagPrefix="uc2" %>
<%@ Register Src="PatientParts/Diagnosis.ascx" TagName="Diagnosis" TagPrefix="uc5" %>
<%@ Register Src="PatientParts/Orders.ascx" TagName="Orders" TagPrefix="uc6" %>
<%@ Register Src="PatientParts/Medications.ascx" TagName="Medications" TagPrefix="uc7" %>
<%@ Register Src="PatientParts/Appointments.ascx" TagName="Appointments" TagPrefix="uc8" %>
<%@ Register Src="PatientParts/Vitals.ascx" TagName="Vital" TagPrefix="uc9" %>
<%@ Register Src="PatientParts/Notes.ascx" TagName="Notes" TagPrefix="uc10" %>
<%@ Register Src="PatientParts/Tasks.ascx" TagName="Tasks" TagPrefix="uc11" %>
<%@ Register Src="PatientParts/CurrentMedications.ascx" TagName="CurrentMedications" TagPrefix="uc12" %>
<%@ Register Src="~/EMR/Dashboard/PatientParts/PatientLabDashboard.ascx" TagName="LabResults" TagPrefix="uc13" %>
<%--<%@ Register Src="PatientParts/PatientLabResult.ascx" TagName="LabResults" TagPrefix="uc13" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register Src="PatientParts/NonDrugOrder.ascx" TagName="NonDrugOrder" TagPrefix="uc14" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/scrollbar.css" rel="stylesheet" />

    <style type="text/css">
        .rmScrollWrap.rmRootGroup.rmRoundedCorners.rmHorizontal {
            width: 97vw !important;
        }
        body{
            overflow-x:hidden;
            overflow-y:auto!important;
        }
        .VisitHistoryDivText {
            overflow-y:hidden!important;
}
    </style>

    <script language="javascript" type="text/javascript">
        function OnClientSelectedIndexChangedEventHandler(sender, args) {
            var item = args.get_item();
            $get('<%=txtEncounterNumber.ClientID%>').value = item != null ? item.get_value() : sender.value();

        }
        function ShowError(sender, args) {
            alert("Enter a Valid Date");
            sender.focus();
        }
        function OnClientDockPositionChanged(obj, args) {
            alert(obj);
            var dockZoneID = obj.get_DockZoneID();

            if (dockZoneID == "RadDock8") {
                if (obj.get_Collapsed()) {
                    obj.set_Collapsed(true);
                }
            }
            else {
                if (obj.get_Collapsed()) {
                    obj.set_Collapsed(false);
                }
            }
        }  
    </script>

    <asp:ScriptManagerProxy ID="scrp" runat="server"></asp:ScriptManagerProxy>
    
    
    
    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
    
    
    
       
    
    
       
    
    
    <div id="dvZone1" style="width: 100%">
        <div class="WordProcessorDiv">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-1">
                        <div class="WordProcessorDivText">
                            <h2>Dashboard</h2>
                        </div>
                    </div>
                    <div class="col-md-6 ">
                        <div class="WordProcessorDivText01">
                            <h2>Date</h2>
                            <h3>
                                <asp:UpdatePanel ID="upnlMenu1" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="ddlTime" runat="server" Width="130px" AutoPostBack="true" OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Complete History" Value="" />
                                                <telerik:RadComboBoxItem Text="Today" Value="DD0" />
                                                <telerik:RadComboBoxItem Text="Last Week" Value="WW-1" />
                                                <telerik:RadComboBoxItem Text="Last Month" Value="MM-1" />
                                                <telerik:RadComboBoxItem Text="Last Year" Value="YY-1" />
                                                <telerik:RadComboBoxItem Text="Date Range" Value="4" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </h3>
                         </div>   
                        <div class="WordProcessorDivText" id="tdDateRange" runat="server">   
                            <h2>From:</h2>
                            <h3>
                                <telerik:RadDatePicker ID="dtpFromDate" Width="100px" runat="server" MinDate="01/01/1900">
                                    <DateInput ID="DateInput1" runat="server">
                                        <ClientEvents OnError="ShowError" />
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </h3>
                            <h2>To:</h2>
                            <h3>
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100px" MinDate="01/01/1900">
                                    <DateInput ID="DateInput2" runat="server">
                                        <ClientEvents OnError="ShowError" />
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </h3>
                        </div>
                    </div>

                    <div class="col-md-5 ">
                        <asp:Button ID="btnAttachment" runat="server" Text="Attachment(s)" CssClass="PatientLabBtn01" OnClick="btnAttachment_Click" />
                        <asp:Button ID="btnSearchDateRange" runat="server" Text="Filter" CssClass="PatientLabBtn01" OnClick="btnSearchDateRange_Click" />
                        
                        <div class="WordProcessorDivText02">
                            <asp:UpdatePanel ID="upnlMenu" runat="server">
                                <ContentTemplate>
                                    <telerik:RadComboBox runat="server" ID="ComboPatientSearch" Width="200px" Height="150px" EnableLoadOnDemand="true" HighlightTemplatedItems="true" EmptyMessage="Encounter No, Provider name" DropDownWidth="460px" OnItemsRequested="RadComboBoxProduct_ItemsRequested">
                                        <HeaderTemplate>
                                            <table style="width: 450px" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="width: 150px" align="left">Date</td>
                                                    <td style="width: 150px" align="left">Encounter No</td>
                                                    <td style="width: 150px" align="left">Provider Name</td>
                                                    <td style="width: 150px" align="left">Facility</td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <table style="width: 450px" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td style="width: 150px;" align="left"><%# DataBinder.Eval(Container, "Text")%></td>
                                                    <td style="width: 150px;" align="left"><%# DataBinder.Eval(Container, "Attributes['EncounterNo']")%></td>
                                                    <td style="width: 150px"><%# DataBinder.Eval(Container, "Attributes['DcotorName']")%></td>
                                                    <td style="width: 150px"><%# DataBinder.Eval(Container, "Attributes['FacilityName']")%></td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        
                        
                        <asp:Button ID="btnClearFilter" Visible="false" runat="server" Text="Clear Filter" CssClass="PatientLabBtn01" OnClick="btnClearFilter_Click" />
                        <asp:Button ID="btnSaveLayout" runat="server" Visible="false" Text="Save Layout" CssClass="PatientLabBtn01" />
                        <asp:Button ID="btnprint" runat="server" CssClass="PatientLabBtn01" Text="Print" ToolTip="Print" CausesValidation="false" OnClick="btnprint_Click" Visible="false" />
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close"></telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                        <asp:Label ID="Label1" runat="server" Text="Report:" CssClass="PatientLabBtn01" Visible="false" />
                        <telerik:RadComboBox ID="cboReport" runat="server" Width="100px" Skin="Vista" Visible="false">
                            <Items>
                                <telerik:RadComboBoxItem Text="Select" Value="0" />
                            </Items>
                        </telerik:RadComboBox>
                    </div>
                
                </div>
            </div>
        </div>
    
    
        <div class="VisitHistoryBorder">
            <div class="container-fluid">
                <div class="row">
                     <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>--%>
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                
                    <%--<div class="HealthBox"></div>--%>
                </div>
            </div>    
        </div>
        
        
        <div class="VisitHistoryDivText">
            <div class="container-fluid">
            
                <div class="row"> 
                    <div class="col-md-12">
                        <%-- <asp:UpdatePanel ID="upnlWebParts" runat="server"><ContentTemplate>--%>
                        <telerik:RadDockLayout runat="server" EnableViewState="false" ID="RadDockLayout1" Visible="false">
                            <telerik:RadDockZone runat="server" ID="LeftZoneUpper" Orientation="vertical" FitDocks="true" Docks="1" Style="width: 100%; min-height: 20px; float: left; margin-right: 10px;" MinHeight="150px">
                                <telerik:RadDock runat="server" ID="RadDock8" EnableDrag="true" Title="<b>Encounters</b>" Skin="Vista" Collapsed="true" Visible="false">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc8:Appointments ID="Appointments" runat="server" Title="Appointments" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <%-- <telerik:RadDock runat="server" ID="RadDock6" EnableDrag="true" Title="<b>Notes</b>" Skin="Vista">
                                        <Commands><telerik:DockExpandCollapseCommand /></Commands>
                                        <ContentTemplate><uc10:Notes ID="Notes" runat="server" /></ContentTemplate>
                                    </telerik:RadDock>--%>
                                <telerik:RadDock runat="server" ID="RadDock4" EnableDrag="true" Title="<b>Diagnosis</b>" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc5:Diagnosis ID="Diagnosis" runat="server" Title="Diagnosis" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="RadDock1" EnableDrag="true" Visible="false" Title="<b>Current Medications</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc12:CurrentMedications ID="CurrentMedications1" runat="server" Title="Current Medications" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                            </telerik:RadDockZone>
                            
                            <telerik:RadDockZone runat="server" ID="CenterZoneUpper" Orientation="vertical" Style="width: 32%; min-height: 20px; float: left" MinHeight="150px">
                                <telerik:RadDock runat="server" ID="RadDock13" EnableDrag="true" Title="<b>Vital</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc9:Vital ID="Vital" runat="server" Title="Vital" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="RadDock5" EnableDrag="true" Title="<b>Allergies</b>" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc2:Allergies ID="Allergies" runat="server" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="RadDock15" EnableDrag="true" Title="<b>Ordered Medications</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc7:Medications ID="Medications" runat="server" Title="Prescription" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <%--<telerik:RadDockZone runat="server" ID="RightZoneMiddle" Orientation="vertical" FitDocks="true" Docks="3" Style="width: 28%; min-height: 20px; float: left;" MinHeight="150px">
                                        <telerik:RadDock runat="server" ID="RadDock7" EnableDrag="true" Title="<b>Tasks</b>" Skin="Vista">
                                            <Commands><telerik:DockExpandCollapseCommand /></Commands>
                                            <ContentTemplate><uc11:Tasks ID="Tasks" runat="server"></uc11:Tasks></ContentTemplate>
                                        </telerik:RadDock>--%>
                                <telerik:RadDock runat="server" EnableDrag="true" ID="RadDock3" Title="<b>Problems</b>" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc1:Problems ID="Problems" runat="server" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="RadDock14" EnableDrag="true" Title="<b>Orders</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc6:Orders ID="Orders" runat="server" Title="Orders" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="RadDock24" EnableDrag="true" Title="<b>Lab results</b>"
                                    Text="" Skin="Vista" EnableAnimation="true" EnableRoundedCorners="true" Visible="true">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc13:LabResults ID="LabResults1" runat="server" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                            </telerik:RadDockZone>
                        </telerik:RadDockLayout>

                        <div class="row">
                            <telerik:RadDockLayout runat="server" EnableViewState="false" ID="RadDockLayout2" Visible="true">
                                <div class="col-md-6">
                                    <telerik:RadDockZone runat="server" ID="RadDockZone1" Orientation="vertical" Style="width: 100%; min-height: 20px; float: left;" MinHeight="150px">
                                        <telerik:RadDock runat="server" ID="RadDock17" EnableDrag="true" Title="<b>Encounters</b>" Skin="Vista" Visible="false">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc8:Appointments ID="Appointments1" runat="server" Title="Appointments" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <%--<telerik:RadDock runat="server" ID="RadDock11" EnableDrag="true" Title="<b>Tasks</b>" Skin="Vista">
                                        <Commands><telerik:DockExpandCollapseCommand /></Commands>
                                        <ContentTemplate><uc11:Tasks ID="Tasks1" runat="server"></uc11:Tasks></ContentTemplate>
                                    </telerik:RadDock>--%>
                                        <telerik:RadDock runat="server" ID="RadDock9" EnableDrag="true" Title="<b>Allergies</b>" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc2:Allergies ID="Allergies1" runat="server" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <telerik:RadDock runat="server" ID="RadDock16" EnableDrag="true" Title="<b>Diagnosis</b>" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc5:Diagnosis ID="Diagnosis1" runat="server" Title="Diagnosis" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <telerik:RadDock runat="server" ID="RadDock12" EnableDrag="true" Title="<b>Ordered Medications</b>" Text="" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc7:Medications ID="Medications1" runat="server" Title="Prescription" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <telerik:RadDock runat="server" ID="RadDock6" EnableDrag="true" Title="<b>NonDrugOrders</b>" Text="" Skin="Vista" EnableAnimation="true" EnableRoundedCorners="true" Visible="true">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc14:NonDrugOrder ID="NonDrugOrder1" runat="server" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </div>
                                <div class="col-md-6">
                                    <telerik:RadDockZone runat="server" ID="RadDockZone2" Orientation="vertical" Style="width: 100%; min-height: 20px; float: left; margin-right: 10px;" MinHeight="150px">
                                        <telerik:RadDock runat="server" ID="RadDock18" EnableDrag="true" Title="<b>Vital</b>" Text="" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc9:Vital ID="Vital1" runat="server" Title="Vital" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <%-- <telerik:RadDock runat="server" ID="RadDock10" EnableDrag="true" Title="<b>Notes</b>" Skin="Vista">
                                    <Commands><telerik:DockExpandCollapseCommand /></Commands>
                                    <ContentTemplate><uc10:Notes ID="Notes1" runat="server" /></ContentTemplate>
                                </telerik:RadDock>--%>
                                        <telerik:RadDock runat="server" ID="RadDock2" EnableDrag="true" Title="<b>Problems</b>" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc1:Problems ID="Problems1" runat="server" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <telerik:RadDock runat="server" ID="RadDock19" EnableDrag="true" Title="<b>Orders</b>" Text="" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc6:Orders ID="Orders1" runat="server" Title="Orders" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <telerik:RadDock runat="server" ID="RadDock20" EnableDrag="true" Visible="false" Title="<b>Current Medications</b>" Text="" Skin="Vista">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc12:CurrentMedications ID="CurrentMedications2" runat="server" Title="Current Medications" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                        <telerik:RadDock runat="server" ID="RadDock22" EnableDrag="true" Title="<b>Lab results</b>" Text="" Skin="Vista" EnableAnimation="true" EnableRoundedCorners="true" Visible="true">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc13:LabResults ID="LabResults2" runat="server" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </div>
                            </telerik:RadDockLayout>

                        </div>
                        <telerik:RadDockLayout runat="server" EnableViewState="false" ID="RadDockLayout3" Visible="false">
                            <telerik:RadDockZone runat="server" ID="RadDockZone4" Orientation="Vertical" MinHeight="150px">
                                <telerik:RadDock runat="server" ID="R3adDock27" EnableDrag="true" Title="<b>Encounters</b>" Skin="Vista" Visible="false">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc8:Appointments ID="Appointments2" runat="server" Title="Appointments" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="R3adDock28" EnableDrag="true" Title="<b>Vital</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc9:Vital ID="Vital2" runat="server" Title="Vital" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <%--<telerik:RadDock runat="server" ID="R3adDock24" EnableDrag="true" Title="<b>Tasks</b>" Skin="Vista">
                                    <Commands><telerik:DockExpandCollapseCommand /></Commands>
                                    <ContentTemplate><uc11:Tasks ID="Tasks2" runat="server"></uc11:Tasks></ContentTemplate>
                                </telerik:RadDock>--%>
                                <%-- <telerik:RadDock runat="server" ID="R3adDock23" EnableDrag="true" Title="<b>Notes</b>" Skin="Vista">
                                    <Commands><telerik:DockExpandCollapseCommand /></Commands>
                                    <ContentTemplate><uc10:Notes ID="Notes2" runat="server" /></ContentTemplate>
                                </telerik:RadDock>--%>
                                <telerik:RadDock runat="server" ID="R3adDock22" EnableDrag="true" Title="<b>Allergies</b>" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc2:Allergies ID="Allergies2" runat="server" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="R3adDock21" EnableDrag="true" Title="<b>Problems</b>" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc1:Problems ID="Problems2" runat="server" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="R3adDock26" EnableDrag="true" Title="<b>Diagnosis</b>" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc5:Diagnosis ID="Diagnosis2" runat="server" Title="Diagnosis" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="R3adDock29" EnableDrag="true" Title="<b>Orders</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc6:Orders ID="Orders2" runat="server" Title="Orders" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="R3adDock25" EnableDrag="true" Title="<b>Ordered Medications<b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc7:Medications ID="Medications2" runat="server" Title="Prescription" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="R3adDock30" EnableDrag="true" Visible="false" Title="<b>Current Medications</b>" Text="" Skin="Vista">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc12:CurrentMedications ID="CurrentMedications3" runat="server" Title="Current Medications" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                                <telerik:RadDock runat="server" ID="RadDock23" EnableDrag="true" Title="<b>Lab Results</b>" Text="" Skin="Vista" EnableAnimation="true" EnableRoundedCorners="true" Visible="true">
                                    <Commands>
                                        <telerik:DockExpandCollapseCommand />
                                    </Commands>
                                    <ContentTemplate>
                                        <uc13:LabResults ID="LabResults3" runat="server" />
                                    </ContentTemplate>
                                </telerik:RadDock>
                            </telerik:RadDockZone>
                        </telerik:RadDockLayout>
                        
                        <%--</ContentTemplate>
                            
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="radddlView" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTime" />
                                <asp:AsyncPostBackTrigger ControlID="btnAttachment" />
                                <asp:AsyncPostBackTrigger ControlID="cboReport" />
                                <asp:AsyncPostBackTrigger ControlID="btnprint" />
                                <asp:AsyncPostBackTrigger ControlID="btnSaveLayout" />
                                <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearchDateRange" />
                                <asp:AsyncPostBackTrigger ControlID="ComboPatientSearch" />
                                <asp:AsyncPostBackTrigger ControlID="ddlTime" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </div>
                
                </div>
                 
            </div>
        </div>            
        
        
    
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr style="display:none;">
                <td align="left" valign="top">
                    <%-- <asp:UpdatePanel ID="upnlMenu" runat="server"><ContentTemplate>--%>
                    
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td align="left" id="tdfalse" runat="server" style="background-color: #C6AEC7;"></td>
                            <td colspan="5" style="background-color: #C6AEC7; display:none;" align="center">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblPatientName1" runat="server" Text="" SkinID="label" Font-Bold="true"></asp:Label>
                                <asp:Label ID="Label23" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                                <asp:Label ID="Label24" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>
                                <asp:Label ID="lblmsg" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                            <td style="background-color: #C6AEC7;" align="right">
                                <%-- <asp:Label ID="lbl3" Text="Dashboard Column(s)" runat="server"></asp:Label>--%> &nbsp;
                                <%-- <telerik:RadComboBox ID="radddlView" runat="server" Width="30px" AutoPostBack="true" OnSelectedIndexChanged="radddlView_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="1" Value="1" Selected="true" />
                                            <telerik:RadComboBoxItem Text="2" Value="2" />
                                            <telerik:RadComboBoxItem Text="3" Value="3" />
                                        </Items>
                                    </telerik:RadComboBox>--%>
                            </td>
                        </tr>

                        <tr>
                            <td align="left" colspan="5">
                                <asp:Label ID="lblEncouterDetails" runat="server" SkinID="label"></asp:Label></td>
                        </tr>
                    </table>
                    
                    
                    <%--  </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="radddlView" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTime" />
                            <asp:AsyncPostBackTrigger ControlID="btnAttachment" />
                            <asp:AsyncPostBackTrigger ControlID="cboReport" />
                            <asp:AsyncPostBackTrigger ControlID="btnprint" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveLayout" />
                            <asp:AsyncPostBackTrigger ControlID="btnClearFilter" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearchDateRange" />
                            <asp:AsyncPostBackTrigger ControlID="ComboPatientSearch" />
                            <asp:AsyncPostBackTrigger ControlID="ddlTime" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
            
        </table>
        
        
    </div>
    
    
    <asp:TextBox ID="txtEncounterNumber" Columns="6" CssClass="Textbox" Style="visibility: hidden;" runat="server"></asp:TextBox>
</asp:Content>