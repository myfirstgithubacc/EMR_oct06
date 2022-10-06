<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ICMNONDrugOrder.aspx.cs" Inherits="ICM_ICMNONDrugOrder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/scrollbar.css" rel="stylesheet" />

    <style type="text/css">
        ul.reToolbar.Default {
            margin-left: 12rem !important;
            margin-top: 2px !important;
        }

        div#ctl00_ContentPlaceHolder1_txtPrescription {
            min-width: 100% !important;
        }

        div#ctl00_ContentPlaceHolder1_Updatepanel1 {
            overflow-x: hidden;
        }

        td#ctl00_ContentPlaceHolder1_txtPrescriptionTop {
            background: #c1e5ef;
        }

        div#ctl00_ContentPlaceHolder1_ddlDoctor {
            border: none;
        }
        .PD-TabRadio{
            padding:0 15px!important;
        }
        #ctl00_ContentPlaceHolder1_lblMessage{
           margin:0!important;
           padding:0!important;
           position:relative!important;
        }
    </style>

    <script type="text/javascript">

        function returnToParent() {
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
    </script>
    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnorderAppovedStatus" runat="server" />

            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2>
                        <asp:Label ID="Label2" runat="server" Text="NON Drug Order" ToolTip="NON Drug Order" /></h2>
                </div>

                <div class="col-md-5">
                    <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text="" Width="100%" />
                </div>

                <div class="col-md-3 text-right pull-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New Record" SkinID="Button" Text="New" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnSave" runat="server" Text="Save (Ctrl+F3)" SkinID="Button" OnClick="btnSave_OnClick" />

                </div>
            </div>

            <div class="container-fluid">
                <div class="row">
                    <aspl1:UserDetail ID="pd1" runat="server" />
                    <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />--%>
                </div>
            </div>

            <div class="container-fluid" style="margin-top: 10px;">
                <div class="row">
                    <div class="col-md-3 mb-3 m-md-0">
                       
                        <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Width="100%">
                            <asp:GridView ID="gvFav" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                AlternatingRowStyle-BackColor="Beige" OnRowCommand="gvFav_OnRowCommand" OnRowDataBound="gvFav_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Favourite Non Drug Orders" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFavName" runat="server" Font-Size="12px" Font-Bold="false"
                                                CommandName="FAVLIST" Text='<%#Eval("NonDrugOrders")%>' />
                                            <asp:HiddenField ID="hdnFavId" runat="server" Value='<%#Eval("FavouriteId")%>' />
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
                    <div class="col-md-9">

                        <asp:Panel ID="pnlDoctor" runat="server">

                            <div class=" form-group" style="position: absolute; z-index: 99999; top: 6px; left: 40px;">
                                <strong>Drug Orders <span style="color: Red">*</span></strong>
                            </div>
                            <div class="col-md-12  form-group">
                                <telerik:RadEditor runat="server" ID="txtPrescription" EnableTextareaMode="false"
                                    Width="100%" Height="110px" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                    ToolsFile="~/Include/XML/PrescriptionRTF.xml" EditModes="Design">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                    </CssFiles>
                                    <SpellCheckSettings AllowAddCustom="true" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>
                            </div>
                            <div style="position: absolute; top: 6px; right: 40px">
                                <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favourite" OnClick="btnAddToFavourite_Click" CssClass="btn btn-primary" />
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-4 col-4">
                                        <div class="row">
                                            <div class="col-md-2 ">Date</div>
                                            <div class="col-md-10 ">
                                                <asp:UpdatePanel ID="udpdateoforder" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadDateTimePicker ID="dtpdate" runat="server" DateInput-DateFormat="dd/MM/yyyy hh:mm tt"
                                                            DateInput-DateDisplayFormat="dd/MM/yyyy hh:mm tt" Calendar-DayNameFormat="FirstLetter"
                                                            TabIndex="0" AutoPostBackControl="Calendar" Calendar-EnableAjaxSkinRendering="True"
                                                            Width="100%" PopupDirection="BottomRight" Enabled="false" Calendar-Enabled="False"
                                                            DateInput-Enabled="False">
                                                        </telerik:RadDateTimePicker>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-4">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label ID="Label1" runat="server" Text="Order Type" />
                                                <span style="color: Red">*</span>
                                            </div>
                                            <div class="col-md-8">
                                                <telerik:RadComboBox ID="ddlOrderType" SkinID="DropDown" runat="server" Width="100%"
                                                    AutoPostBack="false">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="Routine" Value="R" />
                                                        <telerik:RadComboBoxItem Text="Urgent" Value="U" />
                                                        <telerik:RadComboBoxItem Text="Stat" Value="S" />
                                                        <telerik:RadComboBoxItem Text="SOS" Value="O" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4 col-4">
                                        <div class="row">
                                            <div class="col-md-3">
                                                <asp:Label ID="lblDoctor" runat="server" Text="Doctor" />
                                                <span style="color: Red">*</span>
                                            </div>
                                            <div class="col-md-9 ">
                                                <telerik:RadComboBox ID="ddlDoctor" Width="100%" MarkFirstMatch="true" runat="server"
                                                    SkinID="DropDown" EmptyMessage="Select">
                                                </telerik:RadComboBox>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="col-md-12 PD-TabRadio margin_z">
                                        <div class="row">
                                            <div class="col-lg-2 col-4">

                                                <asp:CheckBox ID="chkApprovalRequired" Visible="false" runat="server" TextAlign="Right" AutoPostBack="true" Text="Verbal/Telephonic"
                                                    OnCheckedChanged="chkApprovalRequired_OnCheckedChanged" />
                                            </div>
                                            <div class="col-md-2 col-3">
                                                <asp:CheckBox ID="chkIsReadBack" runat="server" TabIndex="43" Text="Read Back" TextAlign="Right" Visible="false" />

                                            </div>
                                            <div class="col-md-6 col-5 mb-2">
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblReadBackNote" runat="server" Text="Read&nbsp;Back&nbsp;Note" Visible="false"></asp:Label>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:TextBox ID="txtIsReadBackNote" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Visible="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblStatus" runat="server" Text="Status" />
                                            </div>
                                            <div class="col-md-8">
                                                <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" Width="80px" MarkFirstMatch="true"
                                                    runat="server" AutoPostBack="false">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="1" Text="Active" Selected="true" />
                                                        <telerik:RadComboBoxItem Value="0" Text="In-Active" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="col-md-12">
                                        <asp:Panel ID="pnlgvdata" runat="server" Height="200px" ScrollBars="Vertical">
                                            <telerik:RadGrid ID="gvData" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                                                Width="99%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                AllowPaging="false" PageSize="5" OnItemCommand="gvData_OnItemCommand" OnItemDataBound="gvData_OnItemDataBound">
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Auto">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red;">
                                                            No Record Found.
                                                        </div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn HeaderText="Id" CurrentFilterFunction="Contains" AllowFiltering="False"
                                                            ShowFilterIcon="false" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNonDrugOrderId" runat="server" SkinID="label" Text='<%#Eval("NonDrugOrderId")%>' />
                                                                <asp:HiddenField ID="hdnAcknowledge" runat="server" Value='<%#Eval("Acknowledge")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Order Date" CurrentFilterFunction="Contains"
                                                            UniqueName="Name" AllowFiltering="False" AutoPostBackOnFilter="False" ShowFilterIcon="false"
                                                            FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderDate" runat="server" SkinID="label" Text='<%#Eval("OrderDate")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="130px" />
                                                            <ItemStyle Width="130px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Non Drug Orders" CurrentFilterFunction="Contains"
                                                            UniqueName="Prescription" AllowFiltering="False" AutoPostBackOnFilter="False"
                                                            ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPrescription" runat="server" SkinID="label" Text='<%#Eval("Prescription")%>' />
                                                                <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById") %>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Order Type" CurrentFilterFunction="Contains"
                                                            AllowFiltering="False" AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOrderType" runat="server" SkinID="label" Text='<%#Eval("OrderTypeName")%>' />
                                                                <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%#Eval("OrderType")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="60px" />
                                                            <ItemStyle Width="60px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Doctor Name" CurrentFilterFunction="Contains"
                                                            AllowFiltering="False" AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>' />
                                                                <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Acknowledge By" CurrentFilterFunction="Contains"
                                                            UniqueName="AcknowledgeBy" AllowFiltering="False" AutoPostBackOnFilter="False"
                                                            ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAcknowledgeBy" runat="server" SkinID="label" Text='<%#Eval("AcknowledgeBy")%>' />
                                                                <asp:HiddenField ID="hdnNurseId" runat="server" Value='<%#Eval("NurseId")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="120px" />
                                                            <ItemStyle Width="120px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Acknowledge Date" CurrentFilterFunction="Contains"
                                                            UniqueName="AcknowledgeDate" AllowFiltering="False" AutoPostBackOnFilter="False"
                                                            ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAcknowledgeDate" runat="server" SkinID="label" Text='<%#Eval("AcknowledgeDate")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="100px" />
                                                            <ItemStyle Width="100px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Acknowledge Remarks" CurrentFilterFunction="Contains"
                                                            UniqueName="AcknowledgeRemarks" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAcknowledgeRemarks" runat="server" SkinID="label" Text='<%#Eval("AcknowledgeRemarks")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="180px" />
                                                            <ItemStyle Width="180px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Encoded By" CurrentFilterFunction="Contains"
                                                            UniqueName="EncodedBy" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEncodedBy" runat="server" SkinID="label" Text='<%#Eval("EncodedBy")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="150px" />
                                                            <ItemStyle Width="150px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Encoded Date" CurrentFilterFunction="Contains"
                                                            UniqueName="EncodedDate" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEncodedDate" runat="server" SkinID="label" Text='<%#Eval("EncodedDate")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="130px" />
                                                            <ItemStyle Width="130px" />
                                                        </telerik:GridTemplateColumn>

                                                        <telerik:GridTemplateColumn HeaderText="Approval" CurrentFilterFunction="Contains"
                                                            UniqueName="ApprovalStatus" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblApprovalStatus" runat="server" SkinID="label" />
                                                                <asp:HiddenField ID="hdnApprovalStatus" Value='<%# Eval("IsApprovalReqd") %>' runat="server" />
                                                                <asp:HiddenField ID="hdnIsApproved" Value='<%# Eval("IsApproved") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="130px" />
                                                            <ItemStyle Width="130px" />
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Modify By" Visible="false" CurrentFilterFunction="Contains"
                                                            UniqueName="ModifyBy" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblModifyBy" runat="server" SkinID="label" Text='<%#Eval("ModifyBy")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Modify Date" Visible="false" CurrentFilterFunction="Contains"
                                                            UniqueName="ModifyDate" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblModifyDate" runat="server" SkinID="label" Text='<%#Eval("ModifyDate")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-HorizontalAlign="Center"
                                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbnSelect" runat="server" Text="Edit" CommandName="Select" />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="40px" />
                                                            <ItemStyle Width="40px" />
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </asp:Panel>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-4"></div>
                                            <div class="col-md-8">
                                                <asp:Label ID="lblModBy" Visible="false" runat="server" Text="Modify By" />&nbsp;
                                                                <asp:Label ID="lblModifyBy" Visible="false" Font-Bold="true" runat="server" Text="" />&nbsp;
                                                                <asp:Label ID="lblModDate" Visible="false" runat="server" Text="Modify Date" />&nbsp;
                                                                <asp:Label ID="lblModifyDate" Visible="false" Font-Bold="true" runat="server" Text="" />
                                                <asp:Label ID="lblAck" Visible="false" runat="server" Text="Acknowledge By" />&nbsp;
                                                                <asp:Label ID="lblAcknowledgeBy" Visible="false" Font-Bold="true" runat="server"
                                                                    Text="" />&nbsp;
                                                                <asp:Label ID="lblAckDate" Visible="false" runat="server" Text="Acknowledge Date" />&nbsp;
                                                                <asp:Label ID="lblAcknowledgeDate" Visible="false" Font-Bold="true" runat="server"
                                                                    Text="" />&nbsp;
                                                                <asp:Label ID="lblAckRem" Visible="false" runat="server" Text="Acknowledge Remark" />&nbsp;
                                                                <asp:Label ID="lblAckRemark" Visible="false" Font-Bold="true" runat="server" Text="" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:TextBox ID="txtack" runat="server" Text=" " Width="20" BorderColor="LightGreen"
                                    BorderWidth="0" BackColor="LightGreen" />
                                Acknowledge Order
                                    <asp:Panel ID="pnlNurse" runat="server" Visible="false">
                                        <table>
                                            <tr>
                                                <td>Acknowledge By <span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlNurse" Width="200px" MarkFirstMatch="true" runat="server"
                                                        SkinID="DropDown" EmptyMessage="Select">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Acknowledge Date & Time
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <telerik:RadDateTimePicker ID="dtpAckdate" runat="server" DateInput-DateFormat="dd/MM/yyyy hh:mm tt"
                                                                DateInput-DateDisplayFormat="dd/MM/yyyy hh:mm tt" Calendar-DayNameFormat="FirstLetter"
                                                                TabIndex="0" AutoPostBackControl="Both" Calendar-EnableAjaxSkinRendering="True"
                                                                Width="200px" PopupDirection="BottomRight" Enabled="false" SkinID="DropDown">
                                                            </telerik:RadDateTimePicker>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Acknowledge Remark <span style="color: Red">*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAckRemark" runat="server" SkinID="textbox" Width="250px" TextMode="MultiLine"
                                                        Text="" MaxLength="200" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                        </asp:Panel>

                    </div>
                </div>

            </div>


            <asp:HiddenField ID="hdnNonDrugOrder" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
