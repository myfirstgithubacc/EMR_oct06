<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Vitalgraph.aspx.cs" Inherits="EMR_Vitals_Vitalgraph" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

     <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>

        <script type="text/javascript">
            function OnClientClose(oWnd, args) {
                $get('<%=btnFilter.ClientID%>').click();
            }
        </script>
        <div class="container-fluid">
            <div class="row header_main">
                <div class="col-md-5 col-sm-5 col-xs-5">

                </div>
                <div class="col-md-7 col-sm-7 col-xs-7 text-right">
                    <asp:Button ID="btnFilter" CssClass="btn btn-sm btn-primary" runat="server" Text="Filter" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnPrint" runat="server" style="display:none" CssClass="btn btn-primary" Text="Print" OnClick="btnPrint_OnClick" />
                  
                    <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text="Close" ToolTip="Close Page" OnClientClick="window.close();" />
                </div>
                </div>
                <div class="row text-center" style="height: 13px; color: green; font-size: 12px;font-weight: bold;">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>
            <div class="row">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label1" Text="Vital" SkinID="label" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddlVital" runat="server" AutoPostBack="false"
                                    Width="100%" MaxHeight="250px">
                                </telerik:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label2" Text="Chart Type" SkinID="label" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddlgraphtype" runat="server" OnSelectedIndexChanged="ddlgraphtype_OnSelectedIndexChanged"
                                    AutoPostBack="true" Width="100%" MaxHeight="250px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Line" Value="Line" Selected="true" />
                                        <telerik:RadComboBoxItem Text="Bar" Value="Bar" />
                                    </Items>
                                </telerik:RadComboBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="Label3" runat="server" Text="Date Range" SkinID="label"></asp:Label>
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadComboBox ID="ddldateRange" runat="server" Width="100%" MaxHeight="250px"
                                    OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged" AutoPostBack="true">
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
                </div>
            <div class="row">
                <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:UpdatePanel ID="updaterng" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4">
                            <asp:Label ID="lblFrom" runat="server" SkinID="label" Text="From"></asp:Label>
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00"
                                                            Width="100%"></telerik:RadDatePicker>
                        </div>
                    </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="row p-t-b-5">
                        <div class="col-md-4 col-sm-4 col-xs-4"><asp:Label ID="lblTo" runat="server" SkinID="label" Text="To"></asp:Label></div>
                        <div class="col-md-8 col-sm-8 col-xs-8">
                            <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100%">
                                                        </telerik:RadDatePicker>
                        </div>
                    </div>
                        </div>
                </div>
                                            </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
            </div>
                </div>
            <div class="row">
                <div class="col-md-12">
                    <telerik:RadWindowManager ID="RadWindowManager1" Skin="Metro" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow1" runat="server" />
                                    </Windows>
                                </telerik:RadWindowManager>
                    </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlRadcahrt1" runat="server">
                                <telerik:RadChart ID="RadChart1" runat="server" Width="900px" Height="500px" AlternateText="DrillDown RadChart"
                                    Skin="Office2007">
                                    <%--<ChartTitle> 
                                    <TextBlock Text="Vital">
                                    </TextBlock>
                                </ChartTitle>--%><PlotArea>
                                    <EmptySeriesMessage Visible="True">
                                        <Appearance Visible="True">
                                        </Appearance>
                                    </EmptySeriesMessage>
                                    <XAxis>
                                        <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134">
                                            <MajorGridLines Color="134, 134, 134" Width="0" />
                                            <TextAppearance TextProperties-Color="Black">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="Black">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </XAxis>
                                    <YAxis>
                                        <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134" MinorTick-Color="134, 134, 134">
                                            <MajorGridLines Color="134, 134, 134" />
                                            <MinorGridLines Color="134, 134, 134" />
                                            <TextAppearance TextProperties-Color="Black">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="Black">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </YAxis>
                                    <Appearance>
                                        <FillStyle FillType="Solid" MainColor="">
                                        </FillStyle>
                                    </Appearance>
                                </PlotArea>
                                    <Appearance>
                                        <Border Color="134, 134, 134" />
                                    </Appearance>
                                    <ChartTitle>
                                        <Appearance>
                                            <FillStyle MainColor="">
                                            </FillStyle>
                                        </Appearance>
                                        <TextBlock>
                                            <Appearance TextProperties-Color="Black" TextProperties-Font="Arial, 18px">
                                            </Appearance>
                                        </TextBlock>
                                    </ChartTitle>
                                    <Legend>
                                        <Appearance Dimensions-Margins="15%, 2%, 1px, 1px" Dimensions-Paddings="2px, 8px, 6px, 3px">
                                            <ItemTextAppearance TextProperties-Color="Black">
                                            </ItemTextAppearance>
                                            <ItemMarkerAppearance Figure="Square">
                                            </ItemMarkerAppearance>
                                        </Appearance>
                                    </Legend>
                                </telerik:RadChart>
                            </asp:Panel>
                            <asp:Panel ID="pnlRadChartDynamic" runat="server" CssClass="chart-fluid">
                                <telerik:RadChart ID="RadChartDynamic" runat="server" Width="900px" Height="500px"
                                    AutoTextWrap="true" AutoLayout="true" Skin="Default" PlotArea-YAxis-Appearance-LabelAppearance-Visible="true"
                                    IntelligentLabelsEnabled="false" OnBeforeLayout="RadChartDynamic_BeforeLayout">
                                    <PlotArea>
                                        <EmptySeriesMessage TextBlock-Text="No&nbsp;records&nbsp;found&nbsp;!">
                                        </EmptySeriesMessage>
                                        <XAxis DataLabelsColumn="Height" AutoScale="false">                                           
                                            <AxisLabel>
                                                <TextBlock Visible="true" Appearance-Dimensions-AutoSize="true" Appearance-Dimensions-Width="60">
                                                        <Appearance TextProperties-Font="Verdana, 10pt, style=Bold" TextProperties-Color="Black">
                                                        </Appearance>
                                                    </TextBlock>                                               
                                            </AxisLabel>
                                        </XAxis>
                                        <YAxis AutoScale="false">
                                            <Appearance Color="Red">
                                                <MajorGridLines Color="DimGray" Width="1" />
                                            </Appearance>
                                            <AxisLabel Appearance-Position-Auto="true" Visible="false">
                                            </AxisLabel>
                                        </YAxis>
                                    </PlotArea>
                                </telerik:RadChart>
                                <%--    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Skin="Telerik"
                                        Width="200px" Animation="Slide" Position="TopCenter" EnableShadow="true" ToolTipZoneID="RadChartDynamic"
                                        AutoTooltipify="true" />--%>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            </div>
    </form>
</body>
</html>
