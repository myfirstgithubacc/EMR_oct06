<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" 
    CodeFile="ICCAChartView.aspx.cs" Inherits="EMR_ICCAChartView"  %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function closeDiv() {
            dvchart.style.display = 'none';
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td colspan="3" class="clsheader">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblheader" runat="server" Text="Chart"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnClosex" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <aspl1:UserDetail ID="asplUD" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:GridView ID="gvchartdata" runat="server" SkinID="gridview" AutoGenerateColumns="false"
                            AlternatingRowStyle-BackColor="Desktop">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <%--<asp:Button ID="btnXview" runat ="server" Text ="View" OnClick ="btnXview_Onclock" CommandName='<%#Eval("parameterID")+"$"+Eval("Parameter")%>' />--%>
                                        <asp:Button ID="btnXview" runat="server" Text='<%#Eval("Parameter") %>' ToolTip="Click here to View Graph  "
                                            OnClick="btnXview_Onclock" CommandName='<%#Eval("parameterID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="Parameter" HeaderText="Parameter" />--%>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td style="vertical-align: top;">
                        <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <asp:Timer ID="timer" runat="server" Interval="1000" OnTick="timmer_tick" Enabled="false"></asp:Timer>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="timer" EventName="tick" />
                            </Triggers>
                            <ContentTemplate>
                                <telerik:RadChart ID="Chart" runat="server" Width="900px" Height="500px" AlternateText="DrillDown RadChart"
                                    Skin="Office2007" OnItemDataBound="ItemDataBound">
                                    <PlotArea>
                                        <EmptySeriesMessage Visible="True">
                                            <Appearance Visible="True">
                                            </Appearance>
                                        </EmptySeriesMessage>
                                        <XAxis>
                                            <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134" MinorTick-Color="134, 134, 134">
                                                <MajorGridLines Color="134, 134, 134" Width="0" />
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
                                        </XAxis>
                                        <YAxis AutoScale="true" MinValue="0" MaxValue="300" AxisMode="Extended">
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="vertical-align: top;">Chart Interval:
                            <asp:DropDownList ID="ddltimeinterv" runat="server">
                                <asp:ListItem Text="Auto" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="15 Min" Value="15"></asp:ListItem>
                                <asp:ListItem Text="10 Min" Value="30"></asp:ListItem>
                                <asp:ListItem Text="1 Hrs" Value="60"></asp:ListItem>
                            </asp:DropDownList>
                        <asp:Button ID="btnPlayChart" runat="server" Text="Play" CssClass="btn btn-primary" OnClick="BtnPlay_Onclick" />
                        <asp:Button ID="btnPlStop" runat="server" Text="Stop" CssClass="btn btn-primary" OnClick="BtnPlay_Onclick" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="BtnPlay_Onclick" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

