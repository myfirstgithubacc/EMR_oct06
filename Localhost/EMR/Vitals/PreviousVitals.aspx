<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="PreviousVitals.aspx.cs" Inherits="EMR_Vitals_PreviousVitals" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
   
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">

        <script type="text/javascript">

            function setValue(val, valName, DisplayInGraph) {

                
                $get('<%=hdnVitalvalue.ClientID%>').value = val;
                $get('<%=hdnVitalName.ClientID%>').value = valName;
                if (DisplayInGraph == 1) { 
                var oWnd = radopen("/EMR/Vitals/Vitalgraph.aspx?Value=" + $get('<%=hdnVitalvalue.ClientID%>').value +
                                    "&Name=" + $get('<%=hdnVitalName.ClientID%>').value, "RadWindowForNew");
                }
                oWnd.setSize(1000, 650)
                oWnd.center();
                oWnd.VisibleStatusbar = "false";
                oWnd.set_status(""); // would like to remove statusbar, not just blank it
            }

        </script>

    </telerik:RadCodeBlock>
    <div class="container-fluid">

        <div class="row header_main">
            <div class="col-md-4"></div>
            <div class="col-md-8 text-right"><asp:Button ID="btnVitalChart" runat="server" Text="Vital Chart" ToolTip="Vital Chart"
                        OnClick="btnVitalChart_Click" CssClass="btn btn-primary" Visible="false"  />
                    <asp:Button ID="btnVital" runat="server" Visible="false" Text="New Vital" ToolTip="New Vital"
                        OnClick="btnVital_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnBack_Click" />
                    <asp:Button ID="btnClose" CssClass="btn btn-primary hidden" runat="server" Text="Close" OnClientClick="window.close();" /></div>
            </div>
            <div class="row text-center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel></div>


               <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <div class="row">
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="colmd-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Lablevitalvalue" runat="server" Text="Vital Sign Type" SkinID="label"></asp:Label>
                            </div>
                            <div class="colmd-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddlVitalSigntype" runat="server" Width="100%" MaxHeight="150px" AutoPostBack="false"></telerik:RadComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-3 col-xs-12">
                        <div class="row p-t-b-5">
                            <div class="colmd-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="Label1" runat="server" Text="Date Range" SkinID="label"></asp:Label>
                            </div>
                            <div class="colmd-8 col-sm-8 col-xs-8">
                                <telerik:RadComboBox ID="ddldateRange" runat="server" Width="100%" MaxHeight="150px" OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged" AutoPostBack="true">
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
                    <div class="col-md-4 col-sm-4 col-xs-12">
                        <asp:UpdatePanel ID="updaterng" runat="server" UpdateMode="Conditional">
                           <ContentTemplate>
                          <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                       <div class="col-md-6 col-sm-6 col-xs-6">
                            <div class="row p-t-b-5">
                            <div class="colmd-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="lblFrom" runat="server" SkinID="label" Text="From"></asp:Label>
                            </div>
                            <div class="colmd-8 col-sm-8 col-xs-8">
                                <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00" Width="100%"></telerik:RadDatePicker>
                            </div>
                        </div>
                       </div>
                              <div class="col-md-6 col-sm-6 col-xs-6">
                            <div class="row p-t-b-5">
                            <div class="colmd-4 col-sm-4 col-xs-4 text-nowrap">
                                <asp:Label ID="lblTo" runat="server" SkinID="label" Text="To" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="colmd-8 col-sm-8 col-xs-8">
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100%">
                                                                                        </telerik:RadDatePicker>
                            </div>
                        </div>
                       </div>
                              </asp:Panel>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-12 p-t-b-5">
                    <asp:CheckBox ID="chkAbNormal" runat="server" Text="Abnormal Values" />
                    <asp:Button ID="imgOkPrevValue" Text="Filter" CssClass="btn btn-xs btn-primary pull-right" runat="server" OnClick="imgOkPrevValue_Click" />
                </div>
                </div>
            
                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel1" runat="server">
                                            <div class="table-auto">
                                            <asp:GridView ID="gvPrevious" AutoGenerateColumns="true" BorderColor="#336666"
                                                OnRowDataBound="gvPrevious_RowDataBound" runat="server" AllowPaging="true" PageSize="15"
                                                RowStyle-Wrap="false" OnPageIndexChanging="gvPrevious_PageIndexChanging" CellPadding="2"
                                                Width="100%">
                                                <%-- <Columns>
                                                 <asp:BoundField DataField="Vital Date" HeaderText="Vital Date" />
                                                <asp:BoundField DataField="WT" HeaderText="WT" />
                                                <asp:BoundField DataField="BMI" HeaderText="BMI" />
                                                <asp:BoundField DataField="BSA" HeaderText="BSA" />
                                                <asp:BoundField DataField="Entered By" HeaderText="Enterd By" />
                                            </Columns>--%>
                                            </asp:GridView>
                                                </div>
                                            <br />
                                            <asp:Label ID="lblNoOfRows" runat="server" Text="" Font-Bold="true"></asp:Label>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5">
                        <telerik:RadWindowManager ID="RadWindowManager" Skin="Metro" runat="server" Title="Vital Graph" AutoSize="true" AutoSizeBehaviors="HeightProportional">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Maximize,Resize">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                    </div>
                     <asp:HiddenField ID="hdnVitalvalue" runat="server" />
                                <asp:HiddenField ID="hdnVitalName" runat="server" />
                </div>

            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
  
</asp:Content>
