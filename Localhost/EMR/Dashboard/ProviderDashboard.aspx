<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ProviderDashboard.aspx.cs" Inherits="EMR_Dashboard_PDashboard" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/EMR/Dashboard/ProviderParts/Appointments.ascx" TagName="Appointments"
    TagPrefix="uc1" %>
<%@ Register Src="~/EMR/Dashboard/ProviderParts/Notes.ascx" TagName="Notes" TagPrefix="uc2" %>
<%@ Register Src="~/EMR/Dashboard/ProviderParts/Task.ascx" TagName="Task" TagPrefix="uc3" %>
<%@ Register Src="~/EMR/Dashboard/ProviderParts/ProviderLabDashboard.ascx" TagName="LabDashboard"
    TagPrefix="uc4" %>
<%@ Register Src="~/EMR/Dashboard/ProviderParts/Admission.ascx" TagName="Admission"
    TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function OnClientClose(oWnd) {
            $get('<%=ddlProviders.ClientID%>').click();
        }
    </script>

    <asp:UpdatePanel ID="upProvider" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" class="clsheader" style="padding-left: 2px; width: 350px;" align="left">
                        Provider Dashboard
                    </td>
                    <td align="right" valign="top" class="clsheader" style="padding-right: 20px;">
                        <asp:Button ID="btnSaveLayout" Visible="false" runat="server" Text="Save Layout"
                            SkinID="Button" OnClick="btnSaveLayout_OnClick" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" style="background-color: #C6AEC7;">
                        <%--           <asp:UpdatePanel ID="upnlPrvdr" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        Dashboard for:<telerik:RadComboBox ID="ddlProviders" runat="server" Width="160px"
                            Skin="Outlook" AutoPostBack="True" BorderColor="ActiveBorder" BackColor="AliceBlue" 
                            OnSelectedIndexChanged="ddlProviders_SelectedIndexChanged">
                        </telerik:RadComboBox>
                        <%--              </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%" cellpadding="2" cellspacing="2">
                            <tr>
                                <td>
                                    <telerik:RadDockZone runat="server" ID="LeftZoneMiddle" Orientation="vertical" Style="width: 99%;
                                        min-height: 50px; float: left; z-index: 1;">
                                        <telerik:RadDock runat="server" ID="RadDock1" EnableDrag="false" Title="<b>Appointments</b>"
                                            Text="" Skin="Simple" aultCommands="ExpandCollapse">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc1:Appointments ID="Appointments1" runat="server" title="Appointments" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                    <telerik:RadDockZone runat="server" ID="RadDockZone2" Orientation="vertical" Style="width: 99%;
                                        min-height: 50px; float: left; z-index: 1;">
                                        <telerik:RadDock runat="server" ID="RadDock5" EnableDrag="false" Title="<b>Admission</b>"
                                            Text="" Skin="Simple" aultCommands="ExpandCollapse">
                                            <Commands>
                                                <telerik:DockExpandCollapseCommand />
                                            </Commands>
                                            <ContentTemplate>
                                                <uc5:Admission ID="ucAdmission" runat="server" title="Admission" />
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                    <telerik:RadDockLayout runat="server" EnableViewState="false" ID="RadDockLayout1"
                                        OnLoadDockLayout="RadDockLayout1_LoadDockLayout" OnSaveDockLayout="RadDockLayout1_SaveDockLayout"
                                        StoreLayoutInViewState="false">
                                        <telerik:RadDockZone runat="server" ID="LeftZoneUpper" Orientation="vertical" Style="width: 99%;
                                            min-height: 50px; float: left; z-index: 1;">
                                            <telerik:RadDock runat="server" EnableDrag="false" ID="RadDock3" Title="<b>Pending Tasks</b>"
                                                Skin="Simple">
                                                <Commands>
                                                    <telerik:DockExpandCollapseCommand />
                                                </Commands>
                                                <ContentTemplate>
                                                    <uc3:Task ID="Task" runat="server" title="Pending Tasks" />
                                                </ContentTemplate>
                                            </telerik:RadDock>
                                        </telerik:RadDockZone>
                                        <%--<telerik:RadDockZone runat="server" ID="CenterZoneUpper" Orientation="vertical" Style="width: 48%;
                                            min-height: 30px; float: right; z-index: 1;">
                                           
                                            <telerik:RadDock runat="server" ID="RadDock2" EnableDrag="false" Title="<b>Unsigned Patient Notes</b>"
                                                Skin="Simple">
                                                <Commands>
                                                    <telerik:DockExpandCollapseCommand />
                                                </Commands>
                                                <ContentTemplate>
                                                    <uc2:Notes ID="Notes" runat="server" title="In-Progress Notes" />
                                                </ContentTemplate>
                                            </telerik:RadDock>
                                        </telerik:RadDockZone>--%>
                                        <telerik:RadDockZone runat="server" ID="RadDockZone1" Orientation="vertical" Style="width: 99%;
                                            min-height: 20px; float: left; z-index: 1;" CssClass="BorderLightBlue" Visible="false">
                                            <telerik:RadDock runat="server" ID="RadDock4" EnableDrag="false" Title="<b>Lab Results</b>"
                                                Text="" Skin="Simple" EnableAnimation="true" EnableRoundedCorners="true" aultCommands="ExpandCollapse">
                                                <Commands>
                                                    <telerik:DockExpandCollapseCommand />
                                                </Commands>
                                                <ContentTemplate>
                                                    <uc4:LabDashboard ID="LabDashboard1" runat="server" title="Provider Lab Dashboard" />
                                                </ContentTemplate>
                                            </telerik:RadDock>
                                        </telerik:RadDockZone>
                                    </telerik:RadDockLayout>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
