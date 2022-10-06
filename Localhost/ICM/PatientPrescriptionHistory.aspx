<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientPrescriptionHistory.aspx.cs" Inherits="ICM_PatientPrescriptionHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Prescription</title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" />
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .btn {
            font-size: 12px;
        }

        .checkbox {
            float: none;
            width: 100% !important;
            margin: 0px !important;
        }
        .checkbox input{
            float:none!important;
        }
        a#btnDrugOrder {
    font-size: 14px!important;
}
        div#UpdatePanel2{
            overflow-x:hidden!important;
        }
    </style>
    <script type="text/javascript">
        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.LabData = document.getElementById("hdnLabData").value;

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
        <div>
            <asp:ScriptManager ID="scriptmgr1" runat="server">
            </asp:ScriptManager>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="container-fluid">
                        <div class="row header_main">
                            <div class="col-md-3">
                                <asp:Label ID="lblPatientName" runat="server" Font-Bold="true" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <asp:Label ID="lblMessage" runat="server" />
                                <asp:HiddenField ID="hdnLabData" runat="server" Value="" />
                            </div>
                            <div class="col-md-6 text-right">
                                <asp:CheckBox ID="chkStopedMedicine" runat="server" Text="Add Stoped Medicine" SkinID="checkbox" OnCheckedChanged="chkStopedMedicine_CheckedChanged" AutoPostBack="true" />
                                <asp:LinkButton ID="btnDrugOrder" runat="server" CausesValidation="false" Font-Size="Medium" Text="Drug Order"
                                    ToolTip="Add new drug" Font-Underline="false" OnClick="btnDrugOrder_OnClick" />
                                <asp:Button ID="btnUpdateSummary" Text="Update Summary" runat="server" CausesValidation="false"
                                    ToolTip="Update Dicharge Summary" CssClass="btn btn-primary" OnClick="btnUpdateSummary_OnClick" />
                                <asp:Button ID="btnclose" Text="Close" runat="server" CssClass="btn btn-primary" ToolTip="Close"
                                    OnClientClick="window.close();" />
                            </div>
                        </div>
                    </div>

                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td align="left">
                                <div id="dvGridZone" runat="server">
                                    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Office2007" SelectedIndex="0"
                                        MultiPageID="RadMultiPage1">
                                        <Tabs>
                                            <telerik:RadTab Text="Prescriptions" ToolTip="Prescriptions" />
                                            <telerik:RadTab Text="Added Prescriptions ( Discharge Summary ) " ToolTip="Added Prescriptions ( Discharge Summary ) " />
                                        </Tabs>
                                    </telerik:RadTabStrip>
                                    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0"
                                        ScrollBars="Auto" Width="100%" Style="background: #e0ebfd;">
                                        <telerik:RadPageView ID="rpvItem" runat="server" Style="background: #e0ebfd; border-width: 1px">
                                            <telerik:RadGrid ID="gvResultFinal" runat="server" Skin="Office2007" Width="99%" Height="430px"
                                                BorderWidth="0" AllowMultiRowSelection="false" AutoGenerateColumns="false" OnItemDataBound="gvResultFinal_OnItemDataBound"
                                                OnItemCommand="gvResultFinal_OnItemCommand">
                                                <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                                    EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No&nbsp;Record&nbsp;Found.
                                                        </div>
                                                    </NoRecordsTemplate>
                                                    <GroupHeaderItemStyle Font-Bold="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="IndentType" HeaderText="Order Priority"
                                                            HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%#Eval("IndentType") %>' />
                                                                <asp:HiddenField ID="hdnIndentDetailId" runat="server" Value='<%#Eval("IndentDetailId") %>' />
                                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId") %>' />
                                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="IndentDate" HeaderText="Indent Date"
                                                            HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Text='<%#Eval("IndentDate") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="IndentNo" HeaderText="Indent No"
                                                            HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%#Eval("IndentNo") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DrugName" HeaderText="Drug Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDrugName" runat="server" SkinID="label" Text='<%#Eval("DrugName") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DoseUnit" HeaderText="Dose"
                                                            HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDoseUnit" runat="server" SkinID="label" Text='<%#Eval("DoseUnit") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FrequencyDescription" HeaderText="Frequency"
                                                            HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFrequencyDescription" runat="server" SkinID="label" Text='<%#Eval("FrequencyDescription") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DurationType" HeaderText="Duration"
                                                            HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDurationType" runat="server" SkinID="label" Text='<%#Eval("DurationType") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FoodRelationship" HeaderText="Food Relationship"
                                                            HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFoodRelationship" runat="server" SkinID="label" Text='<%#Eval("FoodRelationship") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="InstructionRemarks" HeaderText="Instruction"
                                                            HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInstructionRemarks" runat="server" SkinID="label" Text='<%#Eval("InstructionRemarks") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="AddToList" HeaderText="Add To List"
                                                            HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkAddList" runat="server" Text="Add To List"
                                                                    CommandName="AddList" ToolTip="click here Add To List" ForeColor="Black" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="rpvPayment" runat="server" Style="background: #e0ebfd; border-width: 1px">
                                            <telerik:RadGrid ID="gvLabHistoryCart" runat="server" Skin="Office2007" Width="99%" Height="430px"
                                                BorderWidth="0" AllowMultiRowSelection="false" AutoGenerateColumns="false" OnItemCommand="gvLabHistoryCart_OnItemCommand">
                                                <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                                    EnablePostBackOnRowClick="false" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" EnableDragToSelectRows="false" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <GroupingSettings CaseSensitive="false" />
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No&nbsp;Record&nbsp;Found.
                                                        </div>
                                                    </NoRecordsTemplate>
                                                    <GroupHeaderItemStyle Font-Bold="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="IndentType" HeaderText="Order Priority"
                                                            HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentType" runat="server" SkinID="label" Text='<%#Eval("IndentType") %>' />
                                                                <asp:HiddenField ID="hdnIndentDetailId" runat="server" Value='<%#Eval("IndentDetailId") %>' />
                                                                <asp:HiddenField ID="hdnIndentId" runat="server" Value='<%#Eval("IndentId") %>' />
                                                                <asp:HiddenField ID="hdnItemId" runat="server" Value='<%#Eval("ItemId") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="IndentDate" HeaderText="Indent Date"
                                                            HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentDate" runat="server" SkinID="label" Text='<%#Eval("IndentDate") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="IndentNo" HeaderText="Indent No"
                                                            HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndentNo" runat="server" SkinID="label" Text='<%#Eval("IndentNo") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DrugName" HeaderText="Drug Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDrugName" runat="server" SkinID="label" Text='<%#Eval("DrugName") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DoseUnit" HeaderText="Dose"
                                                            HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDoseUnit" runat="server" SkinID="label" Text='<%#Eval("DoseUnit") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FrequencyDescription" HeaderText="Frequency"
                                                            HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFrequencyDescription" runat="server" SkinID="label" Text='<%#Eval("FrequencyDescription") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="DurationType" HeaderText="Duration"
                                                            HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDurationType" runat="server" SkinID="label" Text='<%#Eval("DurationType") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="FoodRelationship" HeaderText="Food Relationship"
                                                            HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFoodRelationship" runat="server" SkinID="label" Text='<%#Eval("FoodRelationship") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="InstructionRemarks" HeaderText="Instruction"
                                                            HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInstructionRemarks" runat="server" SkinID="label" Text='<%#Eval("InstructionRemarks") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Delete" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete"
                                                                    CommandName="Delete" ToolTip="Delete" ForeColor="Black" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </telerik:RadPageView>

                                    </telerik:RadMultiPage>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadWindowManager ID="RadWindowManager2" Skin="Office2007" runat="server" EnableViewState="false">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowPopup" Skin="Office2007" runat="server" />
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_OnClick" Style="visibility: hidden;" />
                                <script type="text/javascript">
                                    function OnClientRefreshButton() {

                                        $get('<%=btnRefresh.ClientID%>').click();
                                    }
                                </script>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
