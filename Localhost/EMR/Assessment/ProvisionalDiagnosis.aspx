<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ProvisionalDiagnosis.aspx.cs" Inherits="EMR_Assessment_ProvisionalDiagnosis" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        span#ctl00_ContentPlaceHolder1_lblMessage {
            width: auto !important;
            margin: 0px !important;
            font-size: 12px !important;
        }

       div#ctl00_ContentPlaceHolder1_dvConfirmCancelOptions{
            width: 190px!important;
            border: 1px solid #cccccc!important;
            background: #c1e5ef!important;
            text-align: center!important;
            margin-top: -11px!important;
            margin-left: 10%!important;
        }

        input#ctl00_ContentPlaceHolder1_ButtonOk {
            margin-right: 10px !important;
        }

       table.table-primary th.rgHeader, td {
           
            white-space: nowrap!important;
        }
       table#ctl00_ContentPlaceHolder1_gvData_ctl00{
           table-layout:auto!important;
           width:100vw!important;
       }
       div#ctl00_ContentPlaceHolder1_gvData{
           width:100%!important;
       }
       .autosize{
           resize:none;
    overflow: hidden;
    height: 70px!important;
    margin-bottom: 5px;
}
    </style>
    <script language="javascript" type="text/javascript">

        function AutoChange() {
            var txt = $get('<%=txtProvisionalDiagnosis.ClientID%>');
            if (txt.value.length > 500) {
                alert("Text length should not be greater then 500 characters.");
                txt.value = txt.value.substring(0, 500);
                txt.focus();
            }
        }

        function OnClientClose(oWnd, args) {
            $get('<%=btnGetDiagnosisSearchCodes.ClientID%>').click();
        }

    </script>

    <script type="text/javascript">
        if (window.captureEvents) {
            window.captureEvents(Event.KeyUp);
            window.onkeyup = executeCode;

        }
        else if (window.attachEvent) {
            document.attachEvent('onkeyup', executeCode);
        }
        function executeCode(evt) {
            if (evt == null) {
                evt = window.Event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114: //F3
                    $get('<%=btnSave.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;

    }
    function returnToParent() {
        //create the argument that will be returned to the parent page
        var oArg = new Object();
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

    <style type="text/css">
        .Gridheader {
            font-family: Verdana; /*background-image: url(/Images/header.gif);*/
            height: 24px;
            color: black;
            font-weight: normal;
            position: relative;
        }
        /*sanyamtanwar*/
        input#ctl00_ContentPlaceHolder1_btnSearchF {
            position: absolute;
            left: 18px;
            top: 3px;
        }

        input#ctl00_ContentPlaceHolder1_txtSearchFavriouteDiag {
            padding: 3px 28px;
            height: 24px;
            font-size: 15px;
            margin-bottom: 10px;
        }
        /*sanyamtanwar*/
        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        .table-primary table, .table-primary table th, .table-primary table td {
            border-collapse: collapse !important;
            ;
        }

            .table-primary table tr th, .table-primary table tr td {
                border: 1px solid #ccc !important;
               
            }

            .table-primary table tr th {
                color: #fff !important;
                background: #337ab7 !important;
                padding:6px 10px!important;
            }

        .box-col-checkbox input {
            width: auto !important;
        }
        #ctl00_ContentPlaceHolder1_lblMessage {
            position:relative!important;
        }
        table#ctl00_ContentPlaceHolder1_gvFav th{
                color: #fff !important;
                background: #337ab7 !important;
                padding:6px 10px!important;
            }
         table#ctl00_ContentPlaceHolder1_gvFav td{
             padding:4px 10px!important;
         }
    </style>

    <%--   <asp:UpdatePanel ID="UpdatePanel23" runat="server">
        <ContentTemplate>--%>

    <div class="container-fluid" style="overflow:hidden;">
        <div class="row header_main">
            <div class="col-md-8 col-sm-8 col-xs-8 text-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
            </div>
            <div class="col-md-4 col-sm-4 col-xs-4 text-right">
                <%--  <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>--%>
                <%--  <asp:Button ID="btnSave" runat="server" CssClass="button" SkinID="Button" CausesValidation="false"
                                    Text="Save (F3)"cssClass="btn btn-primary" ToolTip="Save Provisional Diagnosis" OnClick="btnSave_Click" />--%>
                <asp:Button ID="btnSave" runat="server" CausesValidation="false" Text="Save "
                    CssClass="btn btn-primary" Font-Bold="true" ToolTip="Save Diagnosis(F3)" OnClick="btnSave_Click" />


                <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favourite" CssClass="btn btn-primary" OnClick="btnAddToFavourite_Click" />

                <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
            </div>
        </div>

        <table id="tblProviderDetails" runat="server" visible="false">
            <tr>
                <td>
                    <asp:Label ID="lblProvider" runat="server" SkinID="label" Text="Provider" />
                    <asp:Label ID="lblProviderStart" runat="server" SkinID="label" Text="*" ForeColor="Red" />
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlRendringProvider" runat="server" EmptyMessage="[ Select ]"
                        Height="250px" Width="250px" DropDownWidth="250px" Filter="Contains" />
                </td>
                <td>
                    <asp:Label ID="lblChangeDate" runat="server" SkinID="label" Text="Date" />
                    <asp:Label ID="lblChangeDateStar" runat="server" SkinID="label" Text="*" ForeColor="Red" />
                </td>
                <td>
                    <telerik:RadDatePicker ID="dtpChangeDate" runat="server" MinDate="01/01/1870" DateInput-ReadOnly="true" Width="168px"></telerik:RadDatePicker>
                    &nbsp;<asp:Literal ID="Literal1" runat="server" Text="Time"></asp:Literal>
                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="*" ForeColor="Red" />
                    <telerik:RadTimePicker ID="RadTimeFrom" runat="server" DateInput-ReadOnly="true"
                        PopupDirection="BottomLeft" TimeView-Columns="10" Width="95px" />
                    <telerik:RadComboBox ID="ddlMinute" runat="server" AutoPostBack="True" Skin="Outlook" Width="50px"
                        OnSelectedIndexChanged="ddlMinute_SelectedIndexChanged" Style="max-height: 500px;">
                    </telerik:RadComboBox>
                    &nbsp;<asp:Literal ID="ltDateTime" runat="server" Text="HH   MM"></asp:Literal>&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
                <td colspan="2">
                    <asp:Label ID="lblRange" runat="server" SkinID="label" Text="*" Font-Bold="true" ForeColor="Red" />
                </td>
            </tr>
        </table>

        <div class="row">
            <div class="col-md-12 m-t">
                <div class="row">
                    <div class="col-md-3 col-sm-4">

                        <%--//yogesh 08/07/2022--%>
                        <asp:TextBox ID="txtSearchFavriouteDiag" runat="server" OnTextChanged="txtSearchFavriouteDiag_TextChanged" Width="100%" ToolTip="Search Favourite Diagnosis" AutoPostBack="true" />
                        <asp:ImageButton ID="btnSearchF" runat="server" ImageUrl="../../Images/search icon.png" Width="18px" ToolTip="Search Favourite Diagnosis" Height="18px" OnClick="btnSearchFavDiag_Click" />
                    </div>

                    <div class="col-md-5 col-sm-8">
                        <div class="row">
                            <div class="col-3">
                                <asp:Label ID="lblDiagnosis" runat="server" Text="Diagnosis" Font-Bold="true" SkinID="label"></asp:Label>
                                &nbsp;<span style="color: #FF0000">*</span>
                            </div>
                            <div class="col-9">
                                <asp:TextBox ID="txtProvisionalDiagnosis" runat="server"  CssClass="form-control autosize" 
                                    onkeyup="return AutoChange();"  TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-6 box-col-checkbox m-sm-0 mt-2 " style="font-weight: normal!important; font-size: 11px!important;">
                        <%--<asp:CheckBox ID="chkPrimarys" runat="server" Text="Primary" />
                        <asp:CheckBox ID="chkChronics" runat="server" Text="Chronic" />--%>
                        <asp:CheckBox ID="chkQuery" runat="server" Text="Provisional" />
                        <%--<asp:CheckBox ID="chkResolve" runat="server" Text="Resolved" />--%>
                        <asp:CheckBox ID="chkIsFinalDiagnosis" runat="server" Text="Final Diagnosis" />
                    </div>
                </div>
            </div>
            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-4 mt-1 mb-3">
                        <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Width="100%">
                            <asp:GridView ID="gvFav" runat="server" Width="100%" AutoGenerateColumns="false"
                                AlternatingRowStyle-BackColor="Beige" OnRowCommand="gvFav_OnRowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Favourite Diagnosis" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFavName" runat="server" Font-Size="12px" Font-Bold="false"
                                                CommandName="FAVLIST" Text='<%#Eval("Description")%>'></asp:LinkButton>
                                            <asp:HiddenField ID="hdnFavId" runat="server" Value='<%#Eval("ID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete1" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                ToolTip="Delete" Width="16px" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                    <div class="col-md-8">
                        <telerik:RadGrid ID="gvData" Skin="Office2007" CssClass="table-primary table-responsive" BorderWidth="0" PagerStyle-ShowPagerText="false"
                            AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                            AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                            GridLines="Both" AllowPaging="True" Height="350px" PageSize="10" OnPageIndexChanged="gvData_OnPageIndexChanged"
                            OnItemCommand="gvData_OnItemCommand" OnPreRender="gvData_PreRender" OnItemDataBound="gvData_ItemDataBound">
                            <GroupingSettings CaseSensitive="false" />
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                    AllowColumnResize="false" />
                            </ClientSettings>
                            <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="false">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.
                                    </div>
                                </NoRecordsTemplate>
                                <ItemStyle Wrap="true" />
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="ProvisionalDiagnosis" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="ProvisionalDiagnosis" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="StartsWith" HeaderText=" Diagnosis" FilterControlWidth="99%"
                                        ItemStyle-Wrap="true" HeaderStyle-Wrap="false" HeaderStyle-Width="30%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProvisionalDiagnosis" runat="server" Text='<%#Eval("ProvisionalDiagnosis")%>'
                                                ToolTip='<%#Eval("ProvisionalDiagnosis")%>' />
                                            <asp:HiddenField ID="hdnProvisionalDiagnosisID" runat="server" Value='<%#Eval("Id")%>' />
                                            <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EnteredBy") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DiagnosisSearchCode" ShowFilterIcon="false"
                                        DefaultInsertValue="" DataField="DiagnosisSearchCode" AutoPostBackOnFilter="true"
                                        CurrentFilterFunction="StartsWith" HeaderText="Diagnosis Search" FilterControlWidth="99%"
                                        ItemStyle-Wrap="true" HeaderStyle-Wrap="false" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiagnosisSearchCode" runat="server" Text='<%#Eval("DiagnosisSearchCode")%>' />
                                            <asp:HiddenField ID="hdnDiagnosisSearchId" runat="server" Value='<%#Eval("DiagnosisSearchId")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="EncodedBy" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="EncodedBy" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="Entered By" FilterControlWidth="99%" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"
                                        ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="EncodedDate" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="EncodedDate" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="Entered Date" FilterControlWidth="99%" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"
                                       ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEncodedDate" runat="server" Text='<%#Eval("EncodedDate")%>' />
                                            <asp:HiddenField ID="hdnProviderId" runat="server" Value='<%#Eval("ProviderId")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--<telerik:GridTemplateColumn UniqueName="LastChangedBy" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="LastChangedBy" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="Changed By" FilterControlWidth="99%" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"
                                        HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastChangedBy" runat="server" Text='<%#Eval("LastChangedBy")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="LastChangedDate" ShowFilterIcon="false" DefaultInsertValue=""
                                        DataField="LastChangedDate" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                        HeaderText="Changed Date" FilterControlWidth="99%" ItemStyle-Wrap="false" HeaderStyle-Wrap="false"
                                        HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastChangedDate" runat="server" Text='<%#Eval("LastChangedDate")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    <%--   <telerik:GridBoundColumn HeaderText="Status" DataField="ActiveStatus" AllowFiltering="false"
                                        HeaderStyle-Width="10%" />--%>
                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" UniqueName="EditData"
                                        HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    <telerik:GridButtonColumn UniqueName="BtnCol" Text="Delete" ButtonType="ImageButton"
                                        CommandName="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        ImageUrl="~/Images/DeleteRow.png"  />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                </div>

            </div>
            <div class="col-md-8 col-sm-8 col-xs-12">
                <div class="col-md-12">



                    <div class="row">
                        <div class="col-md-3  hidden">
                            <asp:Label ID="Label1" runat="server" Text="Diagnosis Search Keyword" SkinID="label" Font-Bold="true"></asp:Label>&nbsp;
                                          <span style="color: #FF0000">*</span>
                        </div>

                        <div class="col-md-9 hidden">
                            <telerik:RadComboBox ID="ddlDiagnosisSearchCodes" runat="server" Width="250px" Skin="Outlook"
                                DropDownWidth="250px" AutoPostBack="false">
                            </telerik:RadComboBox>
                            &nbsp;
                                    <asp:ImageButton ID="ibtnDiagnosisSearchCode" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                        ToolTip="Add New Search Keyword" Height="18px" Width="18px" OnClick="ibtnDiagnosisSearchCode_Click"
                                        Visible="true" CausesValidation="false" />
                            <asp:Button ID="btnGetDiagnosisSearchCodes" runat="server" CausesValidation="false"
                                Style="visibility: hidden;" OnClick="btnGetDiagnosisSearchCodes_Click" />
                            &nbsp;
                                    
                        </div>

                    </div>




                    <div id="dvConfirmCancelOptions" runat="server" class="row p-t-b-5">
                        <div class="">
                            <strong>Do You Want To Delete?</strong>
                        </div>
                        <div>
                            <asp:Button ID="ButtonOk" runat="server" Text="Yes" CssClass="btn btn-xs btn-success" OnClick="ButtonOk_OnClick" />
                            <asp:Button ID="ButtonCancel" runat="server" Text="No" CssClass="btn btn-xs btn-danger" OnClick="ButtonCancel_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowForNew" runat="server" InitialBehaviors="Maximize" Behaviors="Close">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
