<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientVitalHistory.aspx.cs"
    Inherits="ICM_PatientVitalHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vital</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <%--<link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />--%>
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

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
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" style="overflow:hidden">
                <ContentTemplate>

                    <div class="container-fluid">
                        <div class="row header_main mb-2">
                            <div class="col-md-4 text-left">
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="true" SkinID="label"></asp:Label>
                            </div>
                            <div class="col-md-4 text-center">
                                <asp:Label ID="Label2" runat="server" />
                                <asp:HiddenField ID="hdnLabData" runat="server" Value="" />
                                <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                                <asp:HiddenField ID="hdnVitalName" runat="server" />
                            </div>
                            <div class="col-md-4 text-right">
                                <asp:Button ID="btnUpdateSummary" CssClass="btn btn-primary" Text="Add Vitals" runat="server" CausesValidation="false"
                                    ToolTip="Update Dicharge Summary" OnClick="btnUpdateSummary_OnClick" />
                                <asp:Button ID="imgOkPrevValue" Text="Filter" CssClass="btn btn-primary" runat="server" OnClick="imgOkPrevValue_Click" />
                                <asp:Button ID="Button3" Text="Close" CssClass="btn btn-primary" runat="server" ToolTip="Close"
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
                                            <telerik:RadTab Text="Vital History" ToolTip="Vital History" />
                                            <telerik:RadTab Text="Added Vital History ( Discharge Summary )" ToolTip="Added Vital History ( Discharge Summary ) " />
                                        </Tabs>
                                    </telerik:RadTabStrip>
                                    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0"
                                         Width="100%" Style="background: #e0ebfd;">
                                        <telerik:RadPageView ID="rpvItem" runat="server" Style="background: #e0ebfd; border-width: 1px">

                                            <%--     <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>--%>
                                         
                                            <div class="row pr-1 pl-1 pt-2">
                                                <div class="col-md-4 col-6 form-group">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Lablevitalvalue" runat="server"  Text="Vital Sign Type"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                            
                                                                <telerik:RadComboBox ID="ddlVitalSigntype" runat="server" Width="100%" AutoPostBack="false"></telerik:RadComboBox>
                                                          
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-6 form-group">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label1" runat="server"  Text="Date Range"></asp:Label>
                                                        </div>
                                                        <div class="col-md-8">
                                                           
                                                                <telerik:RadComboBox ID="ddldateRange" Width="100%" runat="server" OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged" AutoPostBack="true">
                                                                    <Items>
                                                                        <telerik:RadComboBoxItem Text="Select All" Value="" runat="server" />
                                                                        <telerik:RadComboBoxItem Text="Today" Value="DD0" runat="server" />
                                                                        <telerik:RadComboBoxItem Text="Last Week" Value="WW-1" runat="server" />
                                                                        <telerik:RadComboBoxItem Text="Last Month" Value="MM-1" runat="server" />
                                                                        <telerik:RadComboBoxItem Text="Last Six Months" Value="MM-6" runat="server" />
                                                                        <telerik:RadComboBoxItem Text="Last Year" Value="YY-1" runat="server" />
                                                                        <telerik:RadComboBoxItem Text="Date Range" Value="6" runat="server" />
                                                                    </Items>
                                                                </telerik:RadComboBox>
                                                           
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-6 form-group">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <asp:CheckBox ID="chkAbNormal" runat="server" Text="Abnormal Values" />
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-6 form-group">
                                                    <div class="row">
                                                        <div class="col-12">
                                                            <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                                                                <asp:Label ID="lblFrom" runat="server"  Text="From"></asp:Label>
                                                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00" Width="90px"></telerik:RadDatePicker>
                                                                <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="90px"></telerik:RadDatePicker>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                            </div>

                                           
                                            <%-- </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                            <asp:GridView ID="gvPrevious" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                                ShowHeader="true" Width="100%" Height="100%" OnRowDataBound="gvPrevious_RowDataBound"
                                                HeaderStyle-Height="3px" OnRowCommand="gvPrevious_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Vital Date" HeaderStyle-CssClass="text-center emrWidth">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDetails" CssClass="IHwidth04-bottom" runat="server" Text='<%#Eval("Vital Date")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label29" runat="server" Text="HT (cm)" ToolTip="Height" CssClass="heightEMR" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHT" runat="server" Text='<%#Eval("HT")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label30" runat="server" Text="WT (kg)" HeaderStyle-CssClass="text-center"
                                                                ToolTip="Weight" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWT" runat="server" Text='<%#Eval("WT")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label31" runat="server" Text="HC(cm)" ToolTip="Head Circumference"
                                                                ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHC" runat="server" Text='<%#Eval("HC")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label32" runat="server" Text="T" ToolTip="Temperature" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblT" runat="server" Text='<%#Eval("T")%>' />
                                                            <asp:HiddenField ID="hdnT_ABNORMAL_VALUE" runat="server" Value='<%#Eval("T_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label33" runat="server" Text="R" ToolTip="Respiration" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblR" runat="server" Text='<%#Eval("R")%>' />
                                                            <asp:HiddenField ID="hdnR_ABNORMAL_VALUE" runat="server" Value='<%#Eval("R_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label34" runat="server" Text="P" ToolTip="Pulse" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblP" runat="server" Text='<%#Eval("P")%>' />
                                                            <asp:HiddenField ID="hdnP_ABNORMAL_VALUE" runat="server" Value='<%#Eval("P_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label35" runat="server" Text="BPS" ToolTip="BP Systolic" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBPS" runat="server" Text='<%#Eval("BPS")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label36" runat="server" Text="BPD" ToolTip="BP Diastolic" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBPD" runat="server" Text='<%#Eval("BPD")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label37" runat="server" Text="MAC" ToolTip="Mid Arm Circumference"
                                                                ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMAC" runat="server" Text='<%#Eval("MAC")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label38" runat="server" Text="SpO2" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSpO2" runat="server" Text='<%#Eval("SpO2")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label39" runat="server" Text="BMI" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBMI" runat="server" Text='<%#Eval("BMI")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label40" runat="server" Text="BSA" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBSA" runat="server" Text='<%#Eval("BSA")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" Visible="false" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label41" runat="server" Text="Entered By" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEnteredBy" runat="server" Text='<%#Eval("Entered BY")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" HeaderText="" ItemStyle-CssClass="text-center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAddList" runat="server" Text="Add To List"
                                                                CommandName="AddList" ToolTip="click here Add To List" ForeColor="Black" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <span class="VitalHistory-Div01">
                                                <asp:Label ID="lblNoOfRows" runat="server" Text="" Font-Bold="true"></asp:Label></span>
                                        </telerik:RadPageView>
                                        <telerik:RadPageView ID="rpvToAddVital" runat="server" Style="background: #e0ebfd;border-width: 1px; ">

                                            <asp:GridView ID="gvVitaltoadd" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                                ShowHeader="true" Width="100%" Height="100%"
                                                HeaderStyle-Height="3px"  OnRowCommand="gvVitaltoadd_RowCommand" >
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Vital Date" HeaderStyle-CssClass="text-center emrWidth">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltovitaldate" CssClass="IHwidth04-bottom" runat="server" Text='<%#Eval("VitalDate")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label4" runat="server" Text="HT (cm)" ToolTip="Height" CssClass="heightEMR" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label5" runat="server" Text='<%#Eval("HT")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label6" runat="server" Text="WT (kg)" HeaderStyle-CssClass="text-center"
                                                                ToolTip="Weight" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label7" runat="server" Text='<%#Eval("WT")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label8" runat="server" Text="HC(cm)" ToolTip="Head Circumference"
                                                                ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label9" runat="server" Text='<%#Eval("HC")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label10" runat="server" Text="T" ToolTip="Temperature" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label11" runat="server" Text='<%#Eval("T")%>' />
                                                            <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("T_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label12" runat="server" Text="R" ToolTip="Respiration" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label13" runat="server" Text='<%#Eval("R")%>' />
                                                            <asp:HiddenField ID="HiddenField3" runat="server" Value='<%#Eval("R_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label14" runat="server" Text="P" ToolTip="Pulse" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label15" runat="server" Text='<%#Eval("P")%>' />
                                                            <asp:HiddenField ID="HiddenField4" runat="server" Value='<%#Eval("P_ABNORMAL_VALUE")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label16" runat="server" Text="BPS" ToolTip="BP Systolic" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label17" runat="server" Text='<%#Eval("BPS")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label18" runat="server" Text="BPD" ToolTip="BP Diastolic" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label19" runat="server" Text='<%#Eval("BPD")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label20" runat="server" Text="MAC" ToolTip="Mid Arm Circumference"
                                                                ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label21" runat="server" Text='<%#Eval("MAC")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label22" runat="server" Text="SpO2" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label23" runat="server" Text='<%#Eval("SpO2")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label24" runat="server" Text="BMI" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label25" runat="server" Text='<%#Eval("BMI")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label26" runat="server" Text="BSA" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label27" runat="server" Text='<%#Eval("BSA")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-CssClass="text-center" Visible="false" ItemStyle-CssClass="text-center">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="Label28" runat="server" Text="Entered By" ToolTip="Oxygen Saturation" ForeColor="Black" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label42" runat="server" Text='<%#Eval("EnteredBY")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center">                                                       
                                                        <ItemTemplate>
                                                             <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete"
                                                                    CommandName="Remove" ToolTip="Delete" ForeColor="Black" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>

                                        </telerik:RadPageView>
                                    </telerik:RadMultiPage>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>

