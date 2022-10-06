<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="WardDetailsDashBoard.aspx.cs" Inherits="WardManagement_WardDetailsDashBoard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <%--    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />--%>


    <style>
        .textColor {
            border: solid 1px #d2d2d2;
            outline: none;
        }

        .textColor {
            border: solid 1px #d2d2d2;
            outline: none;
        }

        .small-box {
            border-radius: 0px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
            display: block;
            margin-bottom: 20px;
            position: relative;
            text-align: center;
        }

        .bg-aqua, .callout.callout-info, .alert-info, .label-info, .modal-info .modal-body {
            background-color: #eab7c9 !important;
        }

        .small-box > .inner {
            padding: 10px;
        }

        .small-box h3, .small-box p {
            z-index: 5;
        }

        .small-box h3 {
            font-size: 38px;
            font-weight: bold;
            margin: 0 0 0px;
            padding: 0;
            white-space: nowrap;
        }

        .small-box p {
            font-size: 15px;
            margin: 0px;
        }

        .small-box .icon {
            color: rgba(0, 0, 0, 0.15);
            font-size: 90px;
            position: absolute;
            right: 10px;
            top: -10px;
            transition: all 0.3s linear 0s;
            z-index: 0;
        }

        .small-box > .small-box-footer {
            background: rgba(0, 0, 0, 0.1) none repeat scroll 0 0;
            color: rgba(255, 255, 255, 0.8);
            display: block;
            padding: 3px 0;
            position: relative;
            text-align: center;
            text-decoration: none;
            z-index: 10;
        }

        .small-box .icon {
            color: rgba(0, 0, 0, 0.15);
            font-size: 90px;
        }

        .bg-green, .callout.callout-success, .alert-success, .label-success, .modal-success .modal-body {
            background-color: #eab7c9 !important;
        }

        .bg-red, .callout.callout-danger, .alert-danger, .alert-error, .label-danger, .modal-danger .modal-body {
            background-color: #eab7c9 !important;
        }

        .bg-yellow, .callout.callout-warning, .alert-warning, .label-warning, .modal-warning .modal-body {
            background-color: #eab7c9 !important;
        }

        #gvWardDashBoardDetails th {
            background-color: #3C8DBC;
            display: none;
            border: 1px solid #fff;
            color: #fff;
            font-size: 1em;
            font-weight: bold;
            height: 20px;
            margin-bottom: 1em;
            padding: 10px 18px !important;
        }

        .mg_tp {
            margin-top: 15px;
        }

        h5.blue, h5.green, h5.yellow, h5.red {
            color: #fff;
            margin: 0px;
            padding: 5px 0px;
            text-align: center;
        }

        h5.blue {
            background: #d5ffc0;
        }

        h5.green {
            background: #009551;
        }

        h5.yellow {
            background: #DA8C10;
        }

        h5.red {
            background: #C64333;
        }

        h5 a {
            color: black !important;
        }

        .clsGridheaderorderNew {
            margin-bottom: 1em;
        }

        .small-box a {
            color: black !important;
        }

        #gvWardDashBoardDetails {
            border: 0px;
        }

            #gvWardDashBoardDetails tr {
                margin-top: 1em;
                clear: both;
            }

            #gvWardDashBoardDetails td {
                border: 0px;
                width: 25%;
                margin: 0px;
                padding: 0px;
            }

        .rgGroupItem {
            display: none;
        }

        .RadGrid_Default .rgGroupHeader {
            background: #88cee1;
            font-size: 13px;
            line-height: 21px;
            color: #000;
            padding: 12px;
            font-weight: bold;
        }

        .mg_tp {
            margin-top: 0;
        }

        .small-box {
            border-radius: 0px;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
            display: block;
            margin-bottom: 4px;
            position: relative;
            text-align: center;
        }

        #ctl00_ContentPlaceHolder1_gvWardDashBoardDetails_GroupPanel_TB {
            display: none;
        }

        h4 {
            margin: 0 !important;
        }

        .RadGrid_Default .rgAltRow {
            background: white;
        }
    </style>


    <div class="container-fluid header_main">
        <div class="col-md-2 col-sm-2">
            <h4 style="margin: 5px">
                <asp:Label ID="lblHeader" runat="server" Text="Nursing Dashboard" />
            </h4>
        </div>
        <div class="col-md-3 col-sm-3 text-center">
            <asp:Label ID="Label1" runat="server" Text="Ward" />
            <telerik:RadComboBox ID="ddlWard" runat="server" Width="200px" DropDownWidth="250px" Height="300px"
                AutoPostBack="true" OnSelectedIndexChanged="ddlWard_OnSelectedIndexChanged"
                Style="padding-left: 5px; padding-right: 2px;" />
        </div>
        <div class="col-md-3 col-sm-3 text-center">
            <asp:Label ID="Label2" runat="server" Text="Station" />
                <telerik:RadComboBox ID="ddlStation" runat="server" Width="200px" DropDownWidth="250px" Height="300px"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlStation_OnSelectedIndexChanged"
                                                Style="padding-left: 5px; padding-right: 2px;" />
        </div>
        <div class="col-md-3 col-sm-3 text-center">
            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
        </div>
        <div class="col-md-1 col-sm-1 text-right">
            <asp:Button ID="BtnClose" CssClass="btn btn-primary" Width="30%" Height="25px" runat="server" Text="Close" OnClientClick="window.close();" />
        </div>
    </div>

    <asp:UpdatePanel ID="update1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div class="container-fluid">

                <div class="row">

                    <telerik:RadGrid ID="gvWardDashBoardDetails" SortExpression="id" AllowSorting="true" runat="server" ShowGroupPanel="True" SkinID="gridviewOrderNew" OnItemDataBound="gvWardDashBoardDetails_ItemDataBound" AutoGenerateColumns="false" Width="100%">

                        <MasterTableView TableLayout="Auto">

                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="false" />
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="Id" SortOrder="Descending" />
                            </SortExpressions>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <div class="col-lg-12 col-xs-12 mg_tp" id="divlnkNotification1" runat="server">
                                            <h5 class="blue">

                                                <asp:LinkButton ID="lnkNotification1" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification1") %>' OnClick="lnkNotification1_OnClick"></asp:LinkButton>
                                            </h5>
                                            <!-- small box -->
                                            <div class="small-box bg-aqua">
                                                <div class="inner">
                                                    <h4>
                                                        <asp:LinkButton ID="lnkNotification1Count" ForeColor="Black" Font-Bold="true" runat="server" CommandArgument='<%#Eval("Notification1") %>' Text='<%#Eval("Notification1Count") %>' OnClick="lnkNotification1Count_OnClick"></asp:LinkButton>
                                                        <asp:HiddenField ID="hdnFromDate" runat="server" Value='<%#Eval("FromDate") %>' />
                                                    </h4>
                                                </div>
                                                <div class="icon">
                                                </div>

                                            </div>
                                        </div>



                                        <%--<asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                    <ItemTemplate>

                                        <div class="col-lg-12 col-xs-12 mg_tp" id="divlnkNotification2" runat="server">

                                            <h5 class="blue">
                                                <asp:LinkButton ID="lnkNotification2" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification2") %>' OnClick="lnkNotification2_OnClick"></asp:LinkButton>
                                            </h5>
                                            <!-- small box -->
                                            <div class="small-box bg-green">
                                                <div class="inner">
                                                    <%--  <p>                                    <asp:LinkButton ID="lnkNotification2" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification2") %>' OnClick="lnkNotification2_OnClick"></asp:LinkButton>
</p>--%>
                                                    <h4>
                                                        <asp:LinkButton ID="lnkNotification2Count" ForeColor="Black" Font-Bold="true" runat="server" CommandArgument='<%#Eval("Notification2") %>' Text='<%#Eval("Notification2Count") %>' OnClick="lnkNotification2Count_OnClick"></asp:LinkButton>

                                                    </h4>


                                                </div>
                                                <div class="icon">
                                                    <i class="ion ion-stats-bars"></i>
                                                </div>

                                            </div>
                                        </div>
                                        <!-- ./col -->

                                        <%--<asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                    <ItemTemplate>

                                        <div class="col-lg-12 col-xs-12 mg_tp" id="divlnkNotification3" runat="server">
                                            <h5 class="blue">
                                                <asp:LinkButton ID="lnkNotification3" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification3") %>' OnClick="lnkNotification3_OnClick"></asp:LinkButton>
                                            </h5>
                                            <!-- small box -->
                                            <div class="small-box bg-yellow">
                                                <div class="inner">
                                                    <%-- <p>                                    <asp:LinkButton ID="lnkNotification3" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification3") %>' OnClick="lnkNotification3_OnClick"></asp:LinkButton>
</p>--%>
                                                    <h4>
                                                        <asp:LinkButton ID="lnkNotification3Count" ForeColor="Black" Font-Bold="true" runat="server" CommandArgument='<%#Eval("Notification3") %>' Text='<%#Eval("Notification3Count") %>' OnClick="lnkNotification3Count_OnClick"></asp:LinkButton>
                                                    </h4>


                                                </div>
                                                <div class="icon">
                                                    <i class="ion ion-person-add"></i>
                                                </div>

                                            </div>
                                        </div>
                                        <!-- ./col -->


                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                    <ItemTemplate>

                                        <div class="col-lg-12 col-xs-12 mg_tp" id="divlnkNotification4" runat="server">
                                            <h5 class="blue">
                                                <asp:LinkButton ID="lnkNotification4" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification4") %>' OnClick="lnkNotification4_OnClick"></asp:LinkButton>

                                            </h5>
                                            <!-- small box -->
                                            <div class="small-box bg-red">
                                                <div class="inner">
                                                    <%--  <p>                                    <asp:LinkButton ID="lnkNotification4" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification4") %>' OnClick="lnkNotification4_OnClick"></asp:LinkButton>
</p>--%>
                                                    <h4>
                                                        <asp:LinkButton ID="lnkNotification4Count" ForeColor="Black" Font-Bold="true" runat="server" CommandArgument='<%#Eval("Notification4") %>' Text='<%#Eval("Notification4Count") %>' OnClick="lnkNotification4Count_OnClick"></asp:LinkButton>
                                                    </h4>


                                                </div>
                                                <div class="icon">
                                                    <i class="ion ion-pie-graph"></i>
                                                </div>

                                            </div>
                                        </div>
                                        <!-- ./col -->

                                        <%--<asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                    <ItemTemplate>

                                        <div class="col-lg-12 col-xs-12 mg_tp" id="divlnkNotification5" runat="server">
                                            <h5 class="blue">
                                                <asp:LinkButton ID="lnkNotification5" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification5") %>' OnClick="lnkNotification5_Click"></asp:LinkButton>

                                            </h5>
                                            <!-- small box -->
                                            <div class="small-box bg-red">
                                                <div class="inner">
                                                    <%--  <p>                                    <asp:LinkButton ID="lnkNotification4" ForeColor="Black" Font-Bold="true" runat="server" Text='<%#Eval("Notification4") %>' OnClick="lnkNotification4_OnClick"></asp:LinkButton>
</p>--%>
                                                    <h4>
                                                        <asp:LinkButton ID="lnkNotification5Count" ForeColor="Black" Font-Bold="true" runat="server" CommandArgument='<%#Eval("Notification5") %>' Text='<%#Eval("Notification5Count") %>' OnClick="lnkNotification5Count_OnClick"></asp:LinkButton>
                                                    </h4>


                                                </div>
                                                <div class="icon">
                                                    <i class="ion ion-pie-graph"></i>
                                                </div>

                                            </div>
                                        </div>
                                        <!-- ./col -->

                                        <%--<asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <GroupByExpressions>
                                <telerik:GridGroupByExpression>
                                    <GroupByFields>
                                        <telerik:GridGroupByField FieldName="id" SortOrder="None" FieldAlias=":" />
                                    </GroupByFields>
                                    <SelectFields>
                                        <telerik:GridGroupByField FieldName="NotificationType" SortOrder="None" HeaderText=" " />
                                    </SelectFields>
                                </telerik:GridGroupByExpression>
                            </GroupByExpressions>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>
            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                </Windows>
            </telerik:RadWindowManager>
            <div class="container-fluid header_mainGray">

                <asp:HiddenField ID="hdnMinFromDate" runat="server" />
                <%--<div class="col-md-12 col-sm-12"><h2>To Bed Detail</h2></div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvWardDashBoardDetails" />

        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
