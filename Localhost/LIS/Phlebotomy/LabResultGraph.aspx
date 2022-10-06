<%@ Page Language="C#" Title="Lab Result" AutoEventWireup="true" CodeFile="LabResultGraph.aspx.cs" Inherits="LIS_Phlebotomy_LabResultGraph" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Charting" TagPrefix="telerik" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />

    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
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
</head>
<body>


    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdateProgress ID="updtProgress" DisplayAfter="10" runat="server" AssociatedUpdatePanelID="update"
            DynamicLayout="true">
            <ProgressTemplate>
                <div style="position: absolute; background-color: #FAFAFA; z-index: 2147483647 !important; opacity: 0.8; overflow: hidden; text-align: center; top: 0; left: 0; height: 100%; width: 100%; padding-top: 20%;">
                    <img alt="progress" src="../../Images/ajax-loader3.gif">
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="update" runat="server">
            <Triggers>
                <%-- <asp:AsyncPostBackTrigger ControlID="ddldateRange" />--%>
                <asp:AsyncPostBackTrigger ControlID="btnFilter" />
                <%-- <asp:AsyncPostBackTrigger ControlID="ddlgraphtype" />--%>
                <%--<asp:AsyncPostBackTrigger ControlID="ddlField" />--%>
            </Triggers>
            <ContentTemplate>


                <div class="container-fluid header_main form-group">
                    <div class="col-md-3 col-sm-3">
                        <h2 style="color: #333;">Lab Result Graph</h2>
                    </div>
                    <div class="col-md-6 col-sm-6 text-center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" /></div>
                    <div class="col-md-3 col-sm-3 text-right">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Print" OnClientClick="printChartClient(); return false;" />
                        <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Close Page" OnClientClick="window.close();" CssClass="btn btn-default" />
                    </div>
                </div>


                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-md-3 col-sm-3">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2">
                                    <asp:Label ID="Label1" Text="Field(s)" runat="server"></asp:Label></div>
                                <div class="col-md-8 col-sm-8">
                                    <telerik:RadComboBox ID="ddlField" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlField_OnSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-sm-3">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><span class="textName">
                                    <asp:Label ID="Label2" Text="Chart Type" runat="server"></asp:Label></span></div>
                                <div class="col-md-8 col-sm-8">
                                    <telerik:RadComboBox ID="ddlgraphtype" runat="server" OnSelectedIndexChanged="ddlgraphtype_OnSelectedIndexChanged" AutoPostBack="true" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Line" Value="Line" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Bar" Value="Bar" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-sm-3">
                            <div class="row">
                                <div class="col-md-4 col-sm-4 label2"><span class="textName">
                                    <asp:Label ID="Label3" runat="server" Text="Date Range"></asp:Label></span></div>
                                <div class="col-md-8 col-sm-8">
                                    <telerik:RadComboBox ID="ddldateRange" runat="server" Width="100%"
                                        OnSelectedIndexChanged="ddldateRange_OnSelectedIndexChanged" AutoPostBack="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Select All" Value="" runat="server" />
                                            <telerik:RadComboBoxItem Text="Today" Value="DD0" runat="server" />
                                            <telerik:RadComboBoxItem Text="Last Week" Value="WW-1" runat="server" />
                                            <telerik:RadComboBoxItem Text="Last Month" Value="MM-1" runat="server" />
                                            <telerik:RadComboBoxItem Text="Last Three Month" Value="MM-3" runat="server" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Last Six Months" Value="MM-6" runat="server" />
                                            <telerik:RadComboBoxItem Text="Last Year" Value="YY-1" runat="server" />
                                            <telerik:RadComboBoxItem Text="Date Range" Value="6" runat="server" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 col-sm-3">
                            <div class="row">
                                <asp:UpdatePanel ID="updaterng" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlDatarng" runat="server" Visible="false">
                                            <div class="col-md-3 col-sm-3 label2">
                                                <asp:Label ID="lblFrom" runat="server" Text="From"></asp:Label></div>
                                            <div class="col-md-9 col-sm-9">
                                                <div class="row">
                                                    <div class="col-md-5 col-sm-5 PaddingRightSpacing">
                                                        <telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                                    </div>
                                                    <div class="col-md-2 col-sm-2 label2">
                                                        <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label></div>
                                                    <div class="col-md-5 col-sm-5 PaddingLeftSpacing">
                                                        <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" />
                            </Windows>
                        </telerik:RadWindowManager>
                    </div>

                </div>
                <div id="chartContainer">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <table cellspacing="0" class="table table-bordered table-striped" style="width: 100%; height: 40px; font-size: 18px;">
                                <tbody>
                                    <tr align="center">
                                        <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                                            <asp:Label ID="Label4" runat="server" Text="Patient Name:"></asp:Label>
                                            <asp:Label ID="lblPatientName" runat="server"></asp:Label>
                                        </td>
                                        <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2">UHID:
                                            <asp:Label ID="lblUHID" runat="server"></asp:Label>
                                        </td>
                                        <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3">Age/Gender:
                                            <asp:Label ID="lblAgeGender" runat="server"></asp:Label>
                                        </td>

                                    </tr>
                                </tbody>
                            </table>

                            <div style="width: 100%; margin-bottom: 100px; font-size: 12px!important">
                                <asp:Literal ID="litItem" runat="server" />
                            </div>
                            <div class="container-fluid">
                                <div class="row">
                                    <telerik:RadChart ID="RadChart1" Legend-Visible="false" runat="server" Width="800px" Height="520px" AlternateText="DrillDown RadChart" Skin="Office2007" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
