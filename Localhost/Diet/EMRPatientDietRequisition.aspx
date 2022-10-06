<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EMRPatientDietRequisition.aspx.cs"
    MasterPageFile="~/Include/Master/EMRMaster.master" Inherits="Diet_EMRPatientDietRequisition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
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
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSave.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }

    function OnClientIsValidPasswordClose(oWnd, args) {

        var arg = args.get_argument();
        if (arg) {
            var IsValidPassword = arg.IsValidPassword;

            $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
            }
            $get('<%=btnIsValidPasswordClose.ClientID%>').click();
        }
    </script>

    <style type="text/css">
        .patientDetail {
            background: #F5DEB3;
          
            padding-top: 0px;
            border-style: solid none solid none;
            border-width: 1px;
            border-color: #808080;
        }

        .header {
            background: #b4cdf9;
        }

        span#ctl00_ContentPlaceHolder1_Label6 {
            margin: 0;
            float: none;
        }

        span#ctl00_ContentPlaceHolder1_lblMessage {
            float: none;
            margin: 0;
            position: relative;
            width: 100%;
        }
       
    </style>

    <telerik:RadWindowManager ID="RadWindowManager" Skin="Office2007" EnableViewState="false" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindowForNew" Skin="Office2007" runat="server" Behaviors="Close">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <div class="container-fluid">
        <div class="row header">
            <div class="col-12">
                <div class="row">
                    <div class="col-md-3" style="padding: 6px;">
                        <asp:Label ID="Label6" runat="server" Text="&nbsp;Diet&nbsp;Order" SkinID="label" />
                    </div>
                    <div class="col-md-5 text-center">
                        <button id="liAllergyAlert" runat="server" class="btn btn-default" visible="false" style="background: #fff; border: 0px;">
                            <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" Width="18px" Height="18px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                        </button>
                        <button id="liMedicalAlert" runat="server" visible="false" class="btn btn-default" style="background: #fff; border: 0px;">
                            <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif" OnClick="imgMedicalAlert_OnClick" CssClass="iconEMRimg" Width="18px" Height="18px" Visible="false" ToolTip="Patient Alert" />
                        </button>
                    </div>
                    <div class="col-md-4 text-right " style="margin-top: -4px;margin-bottom:4px;">
                        <asp:Button ID="btnNew" CssClass="btn btn-primary" runat="server" Text="New" OnClick="btnNew_Click" />
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" ToolTip="Save(Ctrl+F3)" Font-Bold="true"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnPrev" CssClass="btn btn-primary" runat="server" Visible="false" Text="Prev. Diet" />
                        <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClientClick="window.close();" />
                        <asp:HiddenField ID="hdnIsPasswordRequired" runat="server" />
                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                        <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                            Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                    </div>
                </div>
            </div>
        </div>
 
   

    <div class="row patientDetail">
        <div class="col-12 text-center">
            <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
        </div>
    </div>
    <div class="row">
        <div class="col-12 text-center">
            <asp:UpdatePanel ID="uplmsg" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Font-Size="15px" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="row mt-2 mb-2">
        <asp:UpdatePanel ID="update1" runat="server" class="col-12">
            <ContentTemplate>
                <div class="row">

                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-5">
                                <asp:Label ID="Label5" runat="server" Text="Diet Type Category:" SkinID="label" />
                                <span style="color: Red">*</span>
                            </div>
                            <div class="col-md-7">
                                <telerik:RadComboBox ID="ddlDietTypeCategory" SkinID="DropDown" runat="server" Width="100%"
                                    Height="350px" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlDietTypeCategory_OnSelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-5">
                                <asp:Label ID="Label3" runat="server" Text="NPO:" SkinID="label" Style="float: inherit;" />
                                <asp:Label ID="strnpo" runat="server" Text="*" Visible="false" Style="color: red"></asp:Label>

                            </div>
                            <div class="col-md-7">
                                <telerik:RadComboBox ID="ddlNPO" SkinID="DropDown" Width="100%"  runat="server" OnSelectedIndexChanged="ddlNPO_OnSelectedIndexChanged"
                                    Height="350px" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-5">
                                <asp:Label ID="Label1" runat="server" Text="Mode of Feeding:" SkinID="label" />

                            </div>
                            <div class="col-md-7">
                                <telerik:RadComboBox ID="ddlModeofFeeding" SkinID="DropDown" runat="server"
                                    Height="350px" Width="100%" Filter="Contains" MarkFirstMatch="true" />
                            </div>
                        </div>
                    </div>

                    <div class="col-4" style="display: none;">
                        <div class="row">
                            <div class="col-md-5">
                                <asp:Label ID="Label4" runat="server" Text="International:" SkinID="label" />
                            </div>
                            <div class="col-md-7">
                                <telerik:RadComboBox ID="ddlInternational" SkinID="DropDown" runat="server"
                            Height="350px" Filter="Contains" Width="100%" MarkFirstMatch="true" AutoPostBack="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" Class="col-12">
            <ContentTemplate>
                <div class="row">


                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-5 ">
                                <asp:Label ID="lblRemars" runat="server" SkinID="label" Text="Remarks" />

                            </div>
                            <div class="col-md-7 ">
                                <asp:TextBox ID="txtRemakrs" runat="server" MaxLength="200" Width="100%"
                                    SkinID="textbox" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-6">
                        <div class="row">
                            <div class="col-md-5 ">
                                <asp:Label ID="lbldiagnosis" runat="server" SkinID="label" Text="Diagnosis" />
                                <span style="color: Red">*</span>
                            </div>
                            <div class="col-md-7 ">
                                <asp:TextBox ID="txtdiagnosis" runat="server" MaxLength="200" SkinID="textbox" Width="100%"  TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                    <script type="text/javascript">
                        var textarea = document.querySelector('textarea');

                        textarea.addEventListener('keydown', autosize);

                        function autosize() {
                            var el = this;
                            setTimeout(function () {
                                el.style.cssText = 'height:auto; padding:0';

                                el.style.cssText = 'height:' + el.scrollHeight + 'px';
                            }, 0);
                        }
                    </script>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="row">

        <div class="col-md-5">
            <table>
                <tr>

                    <td rowspan="5" width="20%" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <table cellpadding="2" cellspacing="2" width="100%" style="border: 1px solid #6F6FFF; background-color: #CFE7E7;">
                                    <tr>
                                        <td>
                                            <strong>Food Precaution</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="gvPrecaution" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                                AllowFilteringByColumn="false" runat="server" Width="98%" AutoGenerateColumns="False"
                                                Height="200px" PageSize="10" EnableLinqExpressions="False" AllowPaging="false"
                                                CellSpacing="0" OnItemDataBound="gvPrecaution_OnItemDataBound">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                                    Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <PagerStyle ShowPagerText="False" />
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.
                                                        </div>
                                                    </NoRecordsTemplate>
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="Id" AllowFiltering="false" ShowFilterIcon="false"
                                                            AutoPostBackOnFilter="false" HeaderText="Id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="false" ShowFilterIcon="false"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                            HeaderText=" Name" FilterControlWidth="99%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDepartment" runat="server" />
                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                                <FilterMenu EnableImageSprites="False">
                                                </FilterMenu>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <strong>Food Habit</strong>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="GvFoodHabit" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                                AllowFilteringByColumn="false" runat="server" Width="98%" AutoGenerateColumns="False"
                                                Height="202px" PageSize="10" EnableLinqExpressions="False" AllowPaging="false"
                                                CellSpacing="0" OnItemDataBound="GvFoodHabit_OnItemDataBound">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                                    Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <PagerStyle ShowPagerText="False" />
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.
                                                        </div>
                                                    </NoRecordsTemplate>
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="Id" AllowFiltering="false" ShowFilterIcon="false"
                                                            AutoPostBackOnFilter="false" HeaderText="Id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="false" ShowFilterIcon="false"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                            HeaderText=" Name" FilterControlWidth="90%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDepartment" runat="server" />
                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                                <FilterMenu EnableImageSprites="False">
                                                </FilterMenu>
                                            </telerik:RadGrid>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>

                    <td rowspan="5" width="20%" valign="top">

                        <table cellpadding="2" cellspacing="2" width="100%" style="border: 1px solid #6F6FFF; background-color: #CFE7E7;">

                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>

                                    <telerik:RadGrid ID="GvDietTypeSubCategoryDetail" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                        AllowFilteringByColumn="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                        Height="240px" PageSize="10" EnableLinqExpressions="False" AllowPaging="false"
                                        CellSpacing="0">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                            Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                        </ClientSettings>
                                        <PagerStyle ShowPagerText="False" />
                                        <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red;">
                                                    No Record Found.
                                                </div>
                                            </NoRecordsTemplate>
                                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                            </RowIndicatorColumn>
                                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                            </ExpandCollapseColumn>
                                            <Columns>
                                                <telerik:GridTemplateColumn UniqueName="Id" AllowFiltering="false" ShowFilterIcon="false"
                                                    AutoPostBackOnFilter="false" HeaderText="Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Name" AllowFiltering="false" ShowFilterIcon="false"
                                                    AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                    HeaderText=" Name" FilterControlWidth="99%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDepartment" runat="server" />
                                                        <asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <EditFormSettings>
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                            </EditFormSettings>
                                        </MasterTableView>
                                        <FilterMenu EnableImageSprites="False">
                                        </FilterMenu>
                                    </telerik:RadGrid></td>
                               
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td rowspan="5" width="20%" valign="top">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="gvDietList" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                                AllowFilteringByColumn="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                                PageSize="10" EnableLinqExpressions="False" AllowPaging="false"
                                                CellSpacing="0">

                                                <PagerStyle ShowPagerText="False" />
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.
                                                        </div>
                                                    </NoRecordsTemplate>
                                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                                    </RowIndicatorColumn>
                                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                                    </ExpandCollapseColumn>
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="CurrentDietDetailID" AllowFiltering="false" ShowFilterIcon="false"
                                                            AutoPostBackOnFilter="false" HeaderText="Id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("CurrentDietDetailID")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn UniqueName="CurrentDietDetail" AllowFiltering="false" ShowFilterIcon="false"
                                                            AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                            HeaderText=" Current Diet Detail" FilterControlWidth="99%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkDepartment" runat="server" />
                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("CurrentDietDetail")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                    <EditFormSettings>
                                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                        </EditColumn>
                                                    </EditFormSettings>
                                                </MasterTableView>
                                            </telerik:RadGrid>



                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                        </table>

                    </td>

                </tr>


            </table>
        </div>
        <div class="col-md-7">
            <table cellpadding="4" cellspacing="4" width="100%" border="0">
                <tr>

                    <td width="80%" valign="top">
                        <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>--%>
                            <asp:Label ID="Label2" runat="server" Text="Ordered Diet" ForeColor="Red" Font-Size="15px" />
                            <telerik:RadGrid ID="gvDietDetail" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                AllowFilteringByColumn="false" runat="server" AutoGenerateColumns="False"
                                Height="250px" PageSize="10" EnableLinqExpressions="False" AllowPaging="false" Width="100%"
                                OnItemDataBound="gvDietDetail_ItemDataBound" OnItemCommand="gvDietDetail_OnItemCommand" CellSpacing="0">
                                <GroupingSettings CaseSensitive="false" />
                                <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                    Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                </ClientSettings>
                                <PagerStyle ShowPagerText="False" />
                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True" />
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True" />
                                    <Columns>
                                        <telerik:GridTemplateColumn UniqueName="DietRequestID" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="DietRequestID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%#Eval("DietRequestID")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="OrderNo" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Order No" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderNo" runat="server" Text='<%#Eval("OrderNo")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="RequestedDate" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Order Date" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestedDate" runat="server" Text='<%#Eval("RequestedDate")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Requestedby" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Order By" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRequestedBy" runat="server" Text='<%#Eval("EncodedBy")%>' />
                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedbyId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="AcknowledgeBy" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Ack. By" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcknowledgeBy" runat="server" Text='<%#Eval("AcknowledgeBy")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Acktime" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Ack. Date" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcktime" runat="server" Text='<%#Eval("Acktime")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="DietName" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="DietName" HeaderStyle-Width="50%" ItemStyle-Width="50%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDietName" runat="server" Text='<%#Eval("DietName")%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Edit" AllowFiltering="false"
                                            ShowFilterIcon="false" AutoPostBackOnFilter="false" HeaderText="Edit" HeaderStyle-Width="5%" ItemStyle-Width="5%" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn UniqueName="Delete" AllowFiltering="false"
                                            ShowFilterIcon="false" AutoPostBackOnFilter="false" HeaderText="Cancel" Visible="false" HeaderStyle-Width="10%" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                              <asp:ImageButton ID="imbtndelete" runat="server" CommandName="Cancel" ImageUrl="../Images/Cancel.jpg" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                           <telerik:GridTemplateColumn UniqueName="Status" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Status" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                
                                                <asp:HiddenField ID="hdndietorderstatus" runat="server" Value='<%#Eval("dietorderstatus") %>' />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                         <telerik:GridTemplateColumn UniqueName="Cancel" AllowFiltering="false" ShowFilterIcon="false"
                                            AutoPostBackOnFilter="false" HeaderText="Cancel" HeaderStyle-Width="7%" ItemStyle-Width="5%">
                                            <ItemTemplate>  
                                                <asp:ImageButton ID="imbtnCancel" runat="server" CommandName="Cancel" ImageUrl="../Images/close_new.jpg"  
                                                    OnClientClick="return confirm('Are you sure to cancel this diet order?');" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                        </telerik:RadGrid>
                        <%--</ContentTemplate></asp:UpdatePanel>--%>
                    </td>
                </tr>
                <tr>

                    <td>
                        <asp:UpdatePanel ID="updapnlgrid" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" BorderWidth="2" BorderColor="AliceBlue" BorderStyle="Solid"
                                    ScrollBars="None" Height="60%">

                                        <telerik:RadGrid ID="gvDiet" Skin="Office2007" BorderWidth="1px" PagerStyle-ShowPagerText="true"
                                            AllowFilteringByColumn="false" runat="server" Width="100%" AutoGenerateColumns="False"
                                            Height="200px" PageSize="10" EnableLinqExpressions="False" AllowPaging="false"
                                            CellSpacing="0">
                                            <GroupingSettings CaseSensitive="false" />
                                            <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" ReorderColumnsOnClient="true"
                                                Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle ShowPagerText="False" />
                                            <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="DietId" AllowFiltering="false" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="false" HeaderText="Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("DietId")%>' />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DietName" AllowFiltering="false" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                        HeaderText="Diet" ItemStyle-Width="30%" HeaderStyle-Width="30%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDietName" runat="server" Text='<%#Eval("DietName")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="DietValue" AllowFiltering="false" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" DataField="Name"
                                                        HeaderText="Value" ItemStyle-Width="70%" HeaderStyle-Width="70%">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDietValue" runat="server" MaxLength="19" Text='<%#Eval("DietValue") %>' />
                                                            <asp:Label ID="lblDietQty" runat="server" Text='<%#Eval("Unit")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                        <asp:HiddenField ID="hdnRegId" runat="server" />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" />
                                        <asp:HiddenField ID="hdnBedId" runat="server" />

                                    </asp:Panel>
                                    <div id="Div1" runat="server" style="width: 420px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 100px; left: 350px; top: 150px">
                                        <table cellpadding="2" cellspacing="2" border="0" width="99%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="Label18" runat="server" Font-Size="12pt" Font-Bold="true" Font-Names="Arial"
                                                        Text="Do you want Delete it" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border-bottom-style: solid; border-bottom-width: 1px; border-bottom-color: #00CCFF"></td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="middle">
                                                    <asp:Button ID="btnDelete" SkinID="Button" runat="server" Text="Yes" ToolTip="Click to Delete"
                                                        OnClick="btnDelete_OnClick" Width="100px" />
                                                    <asp:Button ID="btnClosepopup" SkinID="Button" runat="server" Text="No" OnClick="btnClosepopup_OnClick"
                                                        Width="100px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                    </td>
                </tr>
            </table>
        </div>

    </div>

    <table cellpadding="2" cellspacing="2" width="100%" border="0">
        <tr>
            <td align="center" colspan="3">
                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" ForeColor="#008A2D" />
            </td>

        </tr>
       
    </table>
</div>
</asp:Content>
