<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="GrowthChart.aspx.cs" Inherits="EMR_Vitals_GrowthChart" %>

<%--<%@ Register TagPrefix="asplNewEMR" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>--%>
<%--<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelRegAttachDocument.ascx" %>--%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <style>
        /* div#ctl00_ContentPlaceHolder1_RadChart1 {
            width: 82% !important;
           
        }
        div#ctl00_ContentPlaceHolder1_RadChart2 {
            width: 82% !important;
            
        }*/
        div#ctl00_ContentPlaceHolder1_RadChart1 img {
            max-width: 100%;
        }

        div#ctl00_ContentPlaceHolder1_RadChart2 img {
            max-width: 100%;
        }

        .VisitHistoryDivText h4 {
            margin: 0;
        }

        .h4, .h5, .h6, h4, h5, h6 {
            margin-top: 2px;
            margin-bottom: 2px;
        }
        div#ctl00_ContentPlaceHolder1_ddlGrowthChartType2 { width: auto !important;}


    </style>
    <%--<div class="VisitHistoryDiv">
        <div class="container-fluid">
            <div class="row">

                
               

            </div>
        </div>
    </div>--%>

    
            <div class="container-fluid">
                <div class="row header_main">

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                    <div class="col-md-3 col-sm-3 col-xs-6">
                     <div class="row">
                        <label class="col-md-3 col-sm-3 col-xs-3"><asp:Label ID="Label4" Text="Type" runat="server" style="margin:0px!important;padding:0px!important;" /></label>
                        <div class="col-md-9 col-sm-9 col-xs-9">
                            <telerik:RadComboBox ID="ddlGrowthChartType" runat="server" EmptyMessage="Select" style="margin:0px!important;" Filter="Contains" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Select" Value="" />
                                            <telerik:RadComboBoxItem Text="23 to 42 weeks" Value="1" />
                                            <telerik:RadComboBoxItem Text="2 weeks to 6 months" Value="2" />
                                            <telerik:RadComboBoxItem Text="0 - 1 Years" Value="3" />
                                            <telerik:RadComboBoxItem Text="1 - 4 Years" Value="4" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadComboBox ID="ddlGrowthChartType2" runat="server" EmptyMessage="Select" AutoPostBack="true" style="margin:0px!important;width:100%;" OnSelectedIndexChanged="ddlGrowthChartType2_SelectedIndexChanged" /></div>
                         </div>
                     </div>

                    <div class="col-md-3 col-sm-3 col-xs-6">
                     <div class="row">
                        <div class="col-md-3 col-sm-3 col-xs-3">
                            <asp:Label ID="Label1" Text="Chart" runat="server" CssClass="GrowthChart-Textlabel" />
                        </div>
                        <div class="col-md-9 col-sm-9 col-xs-9">
                            <telerik:RadComboBox ID="ddlGrowthChart" runat="server" Visible="false" Height="22px" />
                                        <telerik:RadComboBox ID="ddlGrowthChart2" runat="server" CssClass="GrowthChartInput" AutoPostBack="true" OnSelectedIndexChanged="ddlGrowthChart2_SelectedIndexChanged" Width="60%" /></div>
                         </div>
                     </div>

                    <div class="form-group col-md-4 col-xs-4 form-group-custom" style="display: none;">
                     <div class="row">
                        <label class="col-xs-3"><asp:Label ID="Label2" Text="System" SkinID="label" runat="server" Visible="false" /></label>
                        <div class="col-xs-9"><telerik:RadComboBox ID="ddlMsystem" runat="server" Skin="Outlook" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Metric" Value="M" Selected="true" />
                                            <telerik:RadComboBoxItem Text="English" Value="E" />
                                        </Items>
                                    </telerik:RadComboBox></div>
                         </div>
                     </div>

                    <div class="form-group col-md-4 col-xs-4 form-group-custom" style="display: none;">
                     <div class="row">
                        <label class="col-xs-3"><asp:Label ID="Literal3" Text="Skin" SkinID="label" runat="server" Visible="false" />
                                    </label>
                        <div class="col-xs-9"><telerik:RadComboBox ID="ddlSkin" runat="server" Skin="Outlook" Width="80px" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem  Text="Default" Value="Default" />
                                            <telerik:RadComboBoxItem Text="Black" Value="Black" />
                                            <telerik:RadComboBoxItem Text="Hay" Value="Hay" />
                                            <telerik:RadComboBoxItem  Selected="true" Text="Inox" Value="Inox" />
                                            <telerik:RadComboBoxItem Text="Office2007" Value="Office2007" />
                                            <telerik:RadComboBoxItem  Text="Outlook" Value="Outlook" />
                                            <telerik:RadComboBoxItem  Text="Sunset" Value="Sunset" />
                                            <telerik:RadComboBoxItem  Text="Vista" Value="Vista" />
                                            <telerik:RadComboBoxItem  Text="Web20" Value="Web20" />
                                            <telerik:RadComboBoxItem  Text="WebBlue" Value="WebBlue" />
                                            <telerik:RadComboBoxItem  Text="Marble" Value="Marble" />
                                            <telerik:RadComboBoxItem  Text="Metal" Value="Metal" />
                                            <telerik:RadComboBoxItem  Text="Wood" Value="Wood" />
                                            <telerik:RadComboBoxItem  Text="BlueStripes" Value="BlueStripes" />
                                            <telerik:RadComboBoxItem  Text="DeepBlue" Value="DeepBlue" />
                                            <telerik:RadComboBoxItem  Text="DeepGray" Value="DeepGray" />
                                            <telerik:RadComboBoxItem Text="DeepGreen" Value="DeepGreen" />
                                            <telerik:RadComboBoxItem  Text="DeepRed" Value="DeepRed" />
                                            <telerik:RadComboBoxItem   Text="GrayStripes" Value="GrayStripes" />
                                            <telerik:RadComboBoxItem  Text="GreenStripes" Value="GreenStripes" />
                                            <telerik:RadComboBoxItem  Text="LightBlue" Value="LightBlue" />
                                            <telerik:RadComboBoxItem  Text="LightBrown" Value="LightBrown" />
                                            <telerik:RadComboBoxItem Text="LightGreen" Value="LightGreen" />
                                        </Items>
                                    </telerik:RadComboBox></div>
                         </div>
                     </div>

                    <div class="col-md-6 col-sm-6 col-xs-12 text-right">
                        <asp:Button ID="cmdFilter" Text="Filter" Visible="false" CssClass="FilterBtn" runat="server" OnClick="cmdFilter_OnClick" />
                                <asp:Button ID="Button1" Text="Print Graph" CssClass="btn btn-xs btn-primary" OnClientClick="printChartClient(); return false;" runat="server" />
                                    <asp:LinkButton ID="lnkbtn" runat="server" Text="Update Chart" OnClick="lnkbtn_Click" Visible="false" />
                                    <asp:Button ID="btnPrint" Text="Print" runat="server" onclick="btnPrint_OnClick" Visible="false" CssClass="btn btn-xs btn-primary" />
                                    <asp:Button ID="btnHistory" runat="server" Text="History" ToolTip="Vital History" OnClick="btnHistory_Click" CssClass="btn btn-xs btn-primary" />
                                    <asp:Button ID="btnVital" runat="server" Text="New Vital" ToolTip="New Vital" OnClick="btnVital_Click" CssClass="btn btn-xs btn-primary" />
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                         </div>
                    

                                </ContentTemplate>
                        </asp:UpdatePanel>

                </div>
            
                    <div class="row m-t">
                        <div class="col-md-12 col-sm-12 col-xs-12" id="chartContainer">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    
                                    <telerik:RadChart ID="RadChart1" runat="server"
                                        OnDataBound="RadChart1_DataBound"
                                        AutoTextWrap="true" AutoLayout="true" Skin="Default" 
                                        PlotArea-YAxis-Appearance-LabelAppearance-Visible="true"
                                        OnItemDataBound="RadChart1_OnItemDataBound" IntelligentLabelsEnabled="false" 
                                        OnBeforeLayout="RadChart1_BeforeLayout" Width="1250px" Height="400px">
                                       
                                        <PlotArea>
                                            <EmptySeriesMessage TextBlock-Text="No&nbsp;records&nbsp;found&nbsp;!" Visible="True">
                                                <Appearance Visible="True">
                                                </Appearance>
                                                <TextBlock Text="No&nbsp;records&nbsp;found&nbsp;!">
                                                </TextBlock>
                                            </EmptySeriesMessage>
                                            <XAxis DataLabelsColumn="Height" AutoScale="false">
                                                <AxisLabel Visible="true">
                                                    <Appearance Visible="True">
                                                    </Appearance>
                                                    <TextBlock Visible="true"  Appearance-Dimensions-Width="5px" Appearance-Border-Width="2">
                                                        <Appearance TextProperties-Font="Verdana, 6pt, style=Bold" TextProperties-Color="Black">
                                                            <Border Width="2" />
                                                        </Appearance>
                                                    </TextBlock>
                                                </AxisLabel>
                                                
                                            </XAxis>
                                            <YAxis AutoScale="false">
                                                <Appearance Color="Red">
                                                    <MajorGridLines Color="DimGray" Width="5" />
                                                </Appearance>
                                                <AxisLabel Appearance-Position-Auto="true" Visible="true">
                                                    <Appearance Visible="True">
                                                    </Appearance>
                                                </AxisLabel>
                                               
                                            </YAxis>
                                            <Appearance Dimensions-Margins="0%, 0%, 0%, 0%">
                                            </Appearance>
                                        </PlotArea>


                                    </telerik:RadChart>

                                    <telerik:RadChart ID="RadChart2" runat="server" Width="1250px" Height="400px"
                                        AutoTextWrap="true" AutoLayout="true" Skin="Default" PlotArea-YAxis-Appearance-LabelAppearance-Visible="true"
                                        OnItemDataBound="RadChart2_OnItemDataBound" IntelligentLabelsEnabled="false" OnBeforeLayout="RadChart2_BeforeLayout">
                                      
                                        <PlotArea>
                                            
                                            <EmptySeriesMessage TextBlock-Text="No&nbsp;records&nbsp;found&nbsp;!" Visible="True">
                                                <Appearance Visible="True">
                                                </Appearance>
                                                <TextBlock Text="No&nbsp;records&nbsp;found&nbsp;!">
                                                </TextBlock>
                                            </EmptySeriesMessage>
                                            <XAxis DataLabelsColumn="Height" AutoScale="false">
                                                <AxisLabel Visible="true">
                                                    <Appearance Visible="True">
                                                    </Appearance>
                                                    <TextBlock Visible="true" Appearance-Dimensions-Width="5px" Appearance-Border-Width="2">
                                                        <Appearance TextProperties-Font="Verdana, 6pt, style=Bold" TextProperties-Color="Black">
                                                            <Border Width="2" />
                                                        </Appearance>
                                                    </TextBlock>
                                                </AxisLabel>
                                            </XAxis>
                                            <YAxis AutoScale="false">
                                                <Appearance Color="Red">
                                                    <MajorGridLines Color="DimGray" Width="5" />
                                                </Appearance>
                                                <AxisLabel Appearance-Position-Auto="true" Visible="true">
                                                    <Appearance Visible="True">
                                                    </Appearance>
                                                </AxisLabel>
                                            </YAxis>

                                            <Appearance Dimensions-Margins="0%, 0%, 0%, 0%">
                                            </Appearance>

                                        </PlotArea>

                                    </telerik:RadChart>

                                    <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Skin="Telerik" Width="200px" Animation="Slide" Position="TopCenter" EnableShadow="true" ToolTipZoneID="RadChart1" AutoTooltipify="true" />
                                    <telerik:RadToolTipManager ID="RadToolTipManager2" runat="server" Skin="Telerik" Width="200px" Animation="Slide" Position="TopCenter" EnableShadow="true" ToolTipZoneID="RadChart2" AutoTooltipify="true" />

                                    <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow runat="server" ID="RadWindow2" KeepInScreenBounds="true">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>

                                </ContentTemplate>

                            </asp:UpdatePanel>
                        </div>

                    </div>

    </div>
    
    <script type="text/javascript">
        //$telerik.$ is the jQuery that comes with the Telerik UI for ASP.NET AJAX suite


        //It can be assigned to a shorter variable name or external jQuery can be used

        function printChartClient() {
            var chartContainer = $telerik.$("#chartContainer");
            //save the original CSS rules that will be modified
            var chartOriginalCSS = {
                display: chartContainer.css("display"),
                position: chartContainer.css("position"),
                top: chartContainer.css("top"),
                left: chartContainer.css("left"),
                zIndex: chartContainer.css("zIndex"),
                visibility: chartContainer.css("visibility")
            };
            //make sure the chart container is visible
            chartContainer.css({
                display: "block",
                position: "absolute",
                top: "6px",
                left: "1px",
                zIndex: 10000,
                visibility: "visible"
            }
            );
            var body = $telerik.$("body");
            //store the original CSS rules for the body tag that will be modified
            var bodyOriginalCSS = {
                visibility: body.css("visibility"),
                overflow: body.css("overflow"),
                background: body.css("background")
            };
            //hide the body tag
            body.css({
                visibility: "hidden",
                overflow: "hidden",
                background: "transparent"
            }
            );

            //call the browser print() method
            window.print();

            //restore the original CSS properties
            chartContainer.css(chartOriginalCSS);
            body.css(bodyOriginalCSS);
        }
    </script>
</asp:Content>
