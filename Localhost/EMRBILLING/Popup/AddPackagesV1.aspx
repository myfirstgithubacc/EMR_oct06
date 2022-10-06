<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPackagesV1.aspx.cs" Inherits="EMRBILLING_Popup_AddPackagesV1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Package Order</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadComboBox table td.rcbArrowCell {
            right: auto !important;
        }

        td label {
            margin-bottom: 0px;
        }

        .table td {
            padding: 4px;
        }

        .RadForm.rfdTextbox .RadComboBox .rcbInput, .RadForm.rfdTextbox .RadComboBox .rcbInput.rfdDecorated, {
            border: 1px solid #cccccc !important;
        }

        td.rcInputCell {
            position: relative !important;
        }

        td {
            position: relative !important;
            top: auto !important;
            right: auto !important;
        }

        a.rcCalPopup {
            position: absolute !important;
            right: 18px !important;
            top: 0px !important;
        }

        a.rcTimePopup{
            position: absolute!important;
            right: 0 !important;
            margin: 0!important;
            padding: 0!important;
            top: 0!important;
        }

        }
    </style>
    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.xmlString = document.getElementById("hdnXmlString").value;
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
    <form id="form1" runat="server" style="overflow: hidden;">

        <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>


        <div class="container-fluid header_main">
            <div class="row">
                <div class="col-md-4 ">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtRegNo" runat="server" Width="1px" SkinID="textbox" Visible="false" />
                            <asp:TextBox ID="txtRegID" Visible="false" runat="server" SkinID="textbox" Width="1px" />
                            <asp:TextBox ID="txtEncNo" runat="server" Width="1px" SkinID="textbox" Visible="false" />
                            <asp:TextBox ID="txtEncId" Visible="false" runat="server" SkinID="textbox" Width="1px" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="col-md-4  text-center">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMsg" runat="server" Text="" Font-Bold="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="col-md-4  text-right">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnNew" runat="server" AccessKey="N" CssClass="btn btn-primary" Text="New" ToolTip="Click to refresh window" ValidationGroup="save" OnClick="btnNew_OnClick" />

                            <asp:Button ID="ibtnClose" runat="server" AccessKey="C" CssClass="btn btn-primary" Text="Close" ToolTip="Close" OnClientClick="window.close();" />
                            <asp:Button ID="ibtnSave" runat="server" AccessKey="S" CssClass="btn btn-primary" Text="Proceed" ToolTip="Click to proceed to bill" ValidationGroup="save" OnClick="ibtSave_OnClick" />
                            <%--<ajax:ConfirmButtonExtender ID="cbsave" runat="server" ConfirmOnFormSubmit="true" ConfirmText="Are You Sure That You Want To Save ? " TargetControlID="ibtnSave"></ajax:ConfirmButtonExtender>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>


        <div class="container-fluid">
            <div class="row">

                <div class="table-responsive">
                    <table class="table table-small-font table-bordered table-striped margin_bottom01">
                        <tr align="center">

                            <td colspan="1" align="left">
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:"></asp:Label>
                                        <asp:Label ID="lblPatientName" runat="server" Text="" CssClass="IPTable-Tr" SkinID="label" ForeColor="#990066" Font-Bold="true"></asp:Label>

                                        <asp:Label ID="Label51" runat="server" Text="DOB:"></asp:Label>
                                        <asp:Label ID="lblDob" runat="server" Text="" SkinID="label" CssClass="IPTable-Tr"></asp:Label>

                                        <asp:Label ID="Label15" runat="server" Text="Mobile No:"></asp:Label>
                                        <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label" CssClass="IPTable-Tr"></asp:Label>

                                        <asp:Label ID="lblInfoEncNo" runat="server" Text="IP No:"></asp:Label>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" CssClass="IPTable-Tr" ForeColor="#990066" Font-Bold="true"></asp:Label>

                                        <asp:Label ID="lblInfoAdmissionDt" runat="server" Text="Admission Date:"></asp:Label>
                                        <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label" CssClass="IPTable-Tr"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </td>
                        </tr>
                    </table>
                </div>

            </div>
        </div>
        <div class="container-fluid">
            <div class="row">


                <asp:Panel ID="IdPanel" runat="server" Style="width: 100%;">
                    <div class="table-responsive">
                        <table class="table table-small-font table-bordered table-striped margin_bottom01">
                            <tr align="center">

                                <td colspan="1" align="left">

                                    <asp:RadioButtonList ID="rdoIncision" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Single Incision" Value="0" Selected="True" />
                                        <asp:ListItem Text="Multi Incision" Value="1" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>

            </div>
        </div>



        <div class="container-fluid">
            <div class="row">
                <div class="col-6 col-md-6">
                    <div class="row form-group">
                        <div class="col-md-3 col-sm-3">
                            <asp:Label ID="lblDept" runat="server" Text="<%$ Resources:PRegistration, department %>"></asp:Label>
                        </div>
                        <div class="col-md-9 col-sm-9">
                            <asp:UpdatePanel ID="up1" runat="server">
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="cmbDept" runat="server" Width="100%" Filter="Contains"
                                        MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="cmbDept_OnSelectedIndexChanged"
                                        Skin="Office2010Black">
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <div class="col-6 col-md-6">
                    <div class="row form-group">
                        <div class="col-md-3 col-sm-3 PaddingRightSpacing">
                            <asp:Label ID="lblSubDept" runat="server" Text="<%$ Resources:PRegistration, SubDepartment %>"></asp:Label>
                        </div>
                        <div class="col-md-9 col-sm-9">
                            <asp:UpdatePanel ID="up2" runat="server">
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="cmbSubDept" runat="server" Width="100%" Filter="Contains"
                                        MarkFirstMatch="true" Skin="Office2010Black" AutoPostBack="true" OnSelectedIndexChanged="cmbSubDept_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>


                <div class="col-12 col-md-6">
                    <div class="row form-group">
                        <div class="col-md-3 col-sm-3 PaddingRightSpacing">
                            <asp:Label ID="Label1" runat="server" Text="Select Package"></asp:Label>
                        </div>
                        <div class="col-md-9 col-sm-9">


                            <asp:UpdatePanel ID="up3" runat="server">
                                <ContentTemplate>
                                    <%-- <telerik:RadComboBox ID="cmbPackages" runat="server" Width="300px" Skin="Metro" Filter="Contains" MarkFirstMatch="true" AutoPostBack="false" DropDownWidth="500px"></telerik:RadComboBox>--%>
                                    <asp:Panel ID="pnlAddService" runat="server" DefaultButton="btnAddToGrid">

                                        <div class="col-6 float-left pl-0">
                                            <telerik:RadComboBox ID="cmbPackages" runat="server" Width="100%" Height="350px" EmptyMessage="[Select Package]" AllowCustomText="true" ShowMoreResultsBox="true" EnableLoadOnDemand="true" OnItemsRequested="ddlService_OnItemsRequested" DataTextField="ServiceName" DataValueField="ServiceId" EnableVirtualScrolling="true" Skin="Office2010Black" EnableItemCaching="false"></telerik:RadComboBox>
                                            </span>
                                        </div>
                                        <div class="col-6 float-right">
                                            <asp:CheckBox ID="cbMainPackage" runat="server" Enabled="true" Checked="true" Text="Main" /></span>
                                             <asp:Button ID="btnAddToGrid" runat="server" Text="Get Charges" CssClass="btn btn-primary" OnClick="btnAddToGrid_Click" OnClientClick="return checkValidVisit()" /></span>
                                        </div>

                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>


                <div class="col-12 col-md-6">
                    <div class="row form-group">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" style="width: 100%;">
                            <ContentTemplate>
                                <div class="col-md-3 col-sm-3 pull-left">
                                    <asp:Label ID="Label2" runat="server" Text="Order Date"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-9">
                                    <div class="row">
                                        <div class="col-md-8 col-8">
                                            <telerik:RadDateTimePicker ID="dtOrderDate" runat="server" CssClass="inlin-bl1" Width="100%" Skin="Metro" AutoPostBackControl="Both" OnSelectedDateChanged="dtOrderDate_OnSelectedDateChanged"></telerik:RadDateTimePicker>
                                        </div>
                                        <div class="col-md-4 col-4">
                                            <telerik:RadComboBox ID="ddlOrderMinutes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlOrderMinutes_SelectedIndexChanged" Width="100%" Skin="Office2010Black" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                                            <asp:Label ID="lblInfoBillCategory" runat="server" ForeColor="Green"></asp:Label>
                                        </div>
                                    </div>


                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>

                    <asp:CheckBox ID="chkUnClean" runat="server" SkinID="checkbox" Text="UnClean" Visible="false" />
                </div>
            </div>
        </div>



        <div class="container-fluid" style="overflow:hidden;">
            <div class="row">
                <div class="col-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate>
                            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Office2010Silver" SelectedIndex="0"
                                MultiPageID="RadMultiPage1" Height="100%">
                                <Tabs>
                                    <telerik:RadTab Text="Package List" ToolTip="Package List"></telerik:RadTab>
                                    <telerik:RadTab Text="Package Breakup" ToolTip="Package Breakup"></telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>

                            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="400px"
                                ScrollBars="Auto">
                                <telerik:RadPageView ID="rpvPackageList" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="Textbox" runat="server"
                                                DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                                            <div id="dvZone1">
                                                <div class="container-fluid header_main form-group">
                                                    <div class="row">
                                                        <div class="col-md-4 col-sm-4 col-4 text-center">
                                                            <h2 style="color: #333;">Operation Theater Date And Time</h2>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-4 text-center">
                                                            <h2 style="color: #333;">Anesthesia Date And Time</h2>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-4 text-center">
                                                            <h2 style="color: #333;">Billing Category</h2>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="container-fluid">

                                                    <div class="row form-group" style="border-bottom: solid 1px #d2d2d2; padding: 0 0 5px 0;">
                                                        <div class="col-md-4 col-sm-4 col-4">
                                                            <div class="row">
                                                                <div class="col-md-2 col-sm-2 PaddingRightSpacing">
                                                                    <asp:Label ID="Label10" runat="server" Text="Start"></asp:Label>
                                                                    <span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-10 col-sm-10">
                                                                    <telerik:RadDateTimePicker ID="rdtpOtStartTime" runat="server" CssClass="inlin-bl1" Skin="Metro"></telerik:RadDateTimePicker>
                                                                    <telerik:RadComboBox ID="radCmbOtStartTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbOtStartTimeM_SelectedIndexChanged" Width="50px" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Skin="Metro"></telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-4">
                                                            <div class="row">
                                                                <div class="col-md-2 col-sm-2 PaddingRightSpacing">
                                                                    <asp:Label ID="Label9" runat="server" Text="Start"></asp:Label>
                                                                    <span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-10 col-sm-10">
                                                                    <telerik:RadDateTimePicker ID="rdtpAstartTime" runat="server" CssClass="inlin-bl1" Skin="Metro"></telerik:RadDateTimePicker>
                                                                    <telerik:RadComboBox ID="radCmbAstartTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbAstartTime_SelectedIndexChanged" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Width="50px" Skin="Metro"></telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-4 mt-3 m-md-0">
                                                            <div class="row">
                                                                <div class="col-md-12 col-sm-12">
                                                                    <telerik:RadComboBox ID="radCmbBedCategory" runat="server" MarkFirstMatch="true" Filter="Contains" Width="100%" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Skin="Metro" ForeColor="Black"></telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="row form-group">
                                                        <div class="col-md-4 col-sm-4 col-4">
                                                            <div class="row">
                                                                <div class="col-md-2 col-sm-2 PaddingRightSpacing">
                                                                    <asp:Label ID="Label12" runat="server" Text="End"></asp:Label>
                                                                    <span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-10 col-sm-10">
                                                                    <telerik:RadDateTimePicker ID="rdtpOtEndTime" runat="server" CssClass="inlin-bl1" Skin="Metro"></telerik:RadDateTimePicker>
                                                                    <telerik:RadComboBox ID="radCmbOtEndTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbOtEndTimeM_SelectedIndexChanged" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Width="50px" Skin="Metro"></telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-4">
                                                            <div class="row">
                                                                <div class="col-md-2 col-sm-2 PaddingRightSpacing">
                                                                    <asp:Label ID="Label11" runat="server" Text="End"></asp:Label>
                                                                    <span style="color: red;">*</span>
                                                                </div>
                                                                <div class="col-md-10 col-sm-10">
                                                                    <telerik:RadDateTimePicker ID="rdtpAEndTime" runat="server" CssClass="inlin-bl1" Skin="Metro"></telerik:RadDateTimePicker>
                                                                    <telerik:RadComboBox ID="radCmbAEndTimeM" runat="server" AutoPostBack="True" OnSelectedIndexChanged="radCmbAEndTimeM_SelectedIndexChanged" BorderColor="#cccccc" BorderStyle="Solid" BorderWidth="1px" Width="50px" Skin="Metro"></telerik:RadComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4">
                                                            <div class="row">
                                                                <div class="col-md-2 col-sm-2 PaddingRightSpacing"></div>
                                                                <div class="col-md-10 col-sm-10"></div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>


                                                <div class="container-fluid header_main form-group">
                                                    <div class="col-md-12 col-sm-12 text-left">
                                                        <h2 style="color: #333;">Operation Theater Date And Time</h2>
                                                    </div>
                                                </div>


                                                <div class="container-fluid">
                                                    <div class="row form-group">
                                                        <div class="col-md-12 col-sm-12">Added Package Details</div>
                                                    </div>

                                                <div class="row">
                                                    <telerik:RadGrid ID="gvAddedSurgery" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                                        Skin="Metro" ShowFooter="false" Width="100%" AutoGenerateColumns="false" runat="server"
                                                        HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="LightGray"
                                                        OnItemDataBound="gvAddedSurgery_OnItemDataBound" OnItemCommand="gvAddedSurgery_OnItemCommand">
                                                        <ClientSettings AllowColumnsReorder="false">
                                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                        </ClientSettings>
                                                        <MasterTableView DataKeyNames="ServiceId" TableLayout="Fixed" AllowFilteringByColumn="false">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn HeaderText="SNo" HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <%#Container.ItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Surgery" HeaderStyle-Width="280px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                                                        <asp:HiddenField ID="hdnPackageId" runat="server" Value='<%#Eval("PackageId") %>' />
                                                                        <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                                        <asp:HiddenField ID="hdnResourceId" runat="server" Value='<%#Eval("ResourceId") %>' />
                                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                                        <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%#Eval("ServiceType") %>' />
                                                                        <asp:HiddenField ID="hdnMainSurgeryId" runat="server" Value='<%#Eval("MainSurgeryId") %>' />
                                                                        <asp:HiddenField ID="hdnIsMainSurgery" runat="server" Value='<%#Eval("IsMainSurgery") %>' />
                                                                        <asp:HiddenField ID="hdnIsSurgeryService" runat="server" Value='<%#Eval("IsSurgeryService") %>' />
                                                                        <asp:HiddenField ID="hdnSurgeonType" runat="server" Value='<%#Eval("SurgeonType") %>' />
                                                                        <asp:HiddenField ID="hdnDoctorRequired" runat="server" Value='<%#Eval("DoctorRequired") %>' />
                                                                        <asp:HiddenField ID="hdnDepartmentTypeId" runat="server" Value='<%#Eval("DepartmentTypeId") %>' />
                                                                        <asp:HiddenField ID="hdnIsMainPackage" runat="server" Value='<%#Eval("IsMainPackage") %>' />
                                                                        <asp:HiddenField ID="hdnPriceEditable" runat="server" Value='<%#Eval("PriceEditable") %>' />
                                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' ToolTip='<%#Eval("ServiceName") %>' />
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Resource Type" UniqueName="ResourceType">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblResourceType" runat="server" Text='<%#Eval("ResourceType") %>'
                                                                            ToolTip='<%#Eval("ResourceType") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Resource Name" UniqueName="ResourceName"
                                                                    HeaderStyle-Width="130px">
                                                                    <ItemTemplate>
                                                                        <telerik:RadComboBox ID="ddlResourceName" runat="server" Filter="Contains" MarkFirstMatch="true"
                                                                            Width="120px" Skin="Metro" Height="250px" DropDownWidth="300px" ForeColor="Black">
                                                                        </telerik:RadComboBox>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Service Charge" UniqueName="ServiceCharge">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServiceAmt" runat="server" Text='<%#Eval("ServiceActualCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("ServiceActualCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Charge%" UniqueName="ChargePercentage" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblChargePercentage" runat="server" Text='<%#Eval("ChargePercentage","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("ChargePercentage","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Gross Amt" UniqueName="ServiceCharge">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtServiceCharge" runat="server" MaxLength="10" Text='<%#Eval("ServiceCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("ServiceCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>' Width="98%" Style="text-align: right; color: Black;"
                                                                            autocomplete="off"></asp:TextBox>
                                                                        <ajax:FilteredTextBoxExtender ID="FTBEtxtServiceCharge" runat="server" ValidChars="."
                                                                            FilterType="Custom,Numbers" TargetControlID="txtServiceCharge">
                                                                        </ajax:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Disc%" UniqueName="ServiceDiscountPerc">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServiceDiscountPerc" runat="server" Text='<%#Eval("ServiceDiscountPerc","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("ServiceDiscountPerc","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Disc Amt" UniqueName="ServiceDiscountAmt">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServiceDiscountAmt" runat="server" Text='<%#Eval("ServiceDiscountAmt","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("ServiceDiscountAmt","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Net Amt" UniqueName="NetCharge">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblNetCharge" runat="server" Text='<%#Eval("NetCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("NetCharge","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Payable By Patient" UniqueName="PayableByPatient">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPayableByPatient" runat="server" Text='<%#Eval("PayableByPatient","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("PayableByPatient","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Payable By Payer" UniqueName="PayableByPayer">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPayableByPayer" runat="server" Text='<%#Eval("PayableByPayer","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                            ToolTip='<%#Eval("PayableByPayer","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <HeaderStyle HorizontalAlign="Right" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-Width="40px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbtnSelect" runat="server" Text="Select" CommandName="Select"
                                                                            ToolTip="Click to view package breakup"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click to remove this and related item"
                                                                            ImageUrl="../../Images/DeleteRow.png" CommandName="Delete" HeaderStyle-Width="40px"
                                                                            CommandArgument='<%#Eval("Id") %>' OnClientClick="return confirm('Are you sure, you want to remove?');"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <%-- <telerik:GridButtonColumn Text="Select" CommandName="Select" HeaderText="Select">
                                                                <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center" Width="7%" />
                                                                <ItemStyle HorizontalAlign="Center" Width="7%" />
                                                            </telerik:GridButtonColumn>--%>
                                                                <%--<telerik:GridClientDeleteColumn ButtonType="ImageButton" ImageUrl="../../Images/DeleteRow.png"
                                                                HeaderStyle-Width="40px" CommandName="Delete" ConfirmDialogType="Classic" ConfirmText="Are you sure, you want to delete?"
                                                                ConfirmTitle="Delete" />--%>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                            </div>


                                            <script language="javascript" type="text/javascript">
                                                var sPaymentType;
                                                function GetPaymentType() {
                                                    var PaymentType = $get('<%=hdnPaymentTypePyr.ClientID%>').value;
                                                    return PaymentType;
                                                }

                                                function CalculateChargesOnModifyServiceCharge(txtServiceCharge, lblServiceDiscountPerc, lblServiceDiscountAmt, lblNetCharge, lblPayableByPatient, lblPayableByPayer) {
                                                    var DecimalPlaces = document.getElementById('<%= hdnDecimalPlaces.ClientID%>').value;
                                                    sPaymentType = GetPaymentType();
                                                    var discountPercent = Number(document.getElementById(lblServiceDiscountPerc).innerHTML);
                                                    var scharge = Number(document.getElementById(txtServiceCharge).value);
                                                    var discountamount = Number(0);
                                                    var netcharge = Number(0);
                                                    if (discountPercent > 0) {
                                                        discountamount = (((discountPercent * 1) / 100) * (scharge * 1)).toFixed(DecimalPlaces);
                                                    }
                                                    else {
                                                        discountamount = (0 * 1).toFixed(DecimalPlaces);
                                                    }
                                                    netcharge = ((scharge * 1) - (discountamount * 1)).toFixed(DecimalPlaces);

                                                    document.getElementById(lblNetCharge).innerHTML = netcharge.toString();
                                                    document.getElementById(lblServiceDiscountAmt).innerHTML = discountamount.toString();
                                                    if (sPaymentType == 'C') {
                                                        document.getElementById(lblPayableByPatient).innerHTML = ((scharge * 1) - (discountamount * 1)).toFixed(DecimalPlaces)
                                                    }
                                                    if (sPaymentType == 'B') {
                                                        document.getElementById(lblPayableByPayer).innerHTML = ((scharge * 1) - (discountamount * 1)).toFixed(DecimalPlaces)
                                                    }
                                                }

                                            </script>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </telerik:RadPageView>

                            <telerik:RadPageView ID="rpvBreakup" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                    <ContentTemplate>

                                        <div class="container-fluid header_main">
                                            <div class="col-md-12 col-sm-12 text-left">
                                                <h2 style="color: #333;">
                                                    <asp:Label ID="Label4" runat="server" Text="Package Details" /></h2>
                                            </div>
                                        </div>

                                        <div class="container-fluid">
                                            <div class="row">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadGrid ID="gvPackageDetails" Width="100%" runat="server" AutoGenerateColumns="false"
                                                            Height="50px" Skin="Metro">
                                                            <MasterTableView Width="98%">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="Visits">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVisits" runat="server" SkinID="label" Text='<%#Eval("TotalVisitsIncluded") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Medicine Limit">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMedLimit" runat="server" SkinID="label" Text='<%#Eval("MedicineLimit") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Material Limit">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMaterLimit" runat="server" SkinID="label" Text='<%#Eval("MaterialLimit") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="10%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Days Limit">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalDaysLimit" runat="server" SkinID="label" Text='<%#Eval("TotalDaysLimit") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="ICU Days">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalICUDaysLimit" runat="server" SkinID="label" Text='<%#Eval("TotalICUDaysLimit") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                        <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True"></Scrolling>
                                                            </ClientSettings>
                                                        </telerik:RadGrid>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>



                                        <div class="container-fluid header_main">
                                            <div class="col-md-12 col-sm-12 text-left">
                                                <h2 style="color: #333;">
                                                    <asp:Label ID="Label3" runat="server" Text="Service" /></h2>
                                            </div>
                                        </div>

                                        <div class="container-fluid">
                                            <div class="row">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadGrid ID="gvPackService" Width="100%" runat="server" AutoGenerateColumns="false"
                                                            Height="125px" Skin="Metro">
                                                            <MasterTableView Width="100%">
                                                                <Columns>
                                                                    <telerik:GridTemplateColumn HeaderText="ServiceName">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPackServiceName" runat="server" SkinID="label" Text='<%#Eval("ServiceName") %>'
                                                                                Width="100%"></asp:Label>
                                                                            <asp:HiddenField ID="hdnlblPackServiceId" runat="server" Value='<%#Eval("serviceid") %>' />
                                                                            <asp:HiddenField ID="hdnlblMPackId" runat="server" Value='<%#Eval("PackageId") %>' />
                                                                            <asp:HiddenField ID="hdnlblServiceDtlID" runat="server" Value='<%#Eval("Id") %>' />
                                                                            <asp:HiddenField ID="hdnDoctorRequired" runat="server" Value='<%#Eval("DoctorRequired") %>' />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="70%" />
                                                                        <ItemStyle Width="70%" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Unit Limit">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUnitLimit" runat="server" SkinID="label" Text='<%#Eval("UnitLimit") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                    <telerik:GridTemplateColumn HeaderText="Amount Limit">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAmountLimit" runat="server" SkinID="label" Text='<%#Eval("AmountLimit","{0:f2}") %>'
                                                                                Width="100%">
                                                                            </asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="15%" HorizontalAlign="Right" />
                                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                            <ClientSettings>
                                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True"></Scrolling>
                                                            </ClientSettings>
                                                        </telerik:RadGrid>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>


                                            <div class="container-fluid header_main">
                                                <div class="row">
                                                    <div class="col-md-6 col-sm-6 text-left">
                                                        <h2 style="color: #333;">
                                                            <asp:Label ID="lbl" runat="server" Text="Department" /></h2>
                                                    </div>
                                                    <div class="col-md-6 col-sm-6 text-left">
                                                        <h2 style="color: #333;">
                                                            <asp:Label ID="lblMediLabel" runat="server" Text="Medicine" /></h2>
                                                    </div>
                                                </div>
                                            </div>


                                        <div class="container-fluid">
                                            <div class="row">
                                                <div class="col-md-6 col-sm-6 PaddingSpacing">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>
                                                            <telerik:RadGrid ID="gvPackDeptLimit" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                Height="125px" Skin="Metro">
                                                                <MasterTableView Width="100%">
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn HeaderText="DeptDtlID">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDeptDtlID" runat="server" SkinID="label" Text='<%#Eval("Id") %>'
                                                                                    Width="0px">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="0px" />
                                                                            <ItemStyle Width="0px" />
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="PackageId">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDeptPackId" runat="server" SkinID="label" Text='<%#Eval("PackageId") %>'
                                                                                    Width="0px">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="0px" />
                                                                            <ItemStyle Width="0px" />
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="DepartmentId">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDepartmentId" runat="server" SkinID="label" Text='<%#Eval("DepartmentId") %>'
                                                                                    Width="0px">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="0px" />
                                                                            <ItemStyle Width="0px" />
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Department Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDeptName" runat="server" SkinID="label" Text='<%#Eval("Departmentname") %>'
                                                                                    Width="100%">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="65%" />
                                                                            <ItemStyle Width="65%" />
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="% Limit">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPercentageLimit" runat="server" SkinID="label" Text='<%#Eval("PercentageLimit","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                                    Width="100%">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="Amt. Limit">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmountLimit" runat="server" SkinID="label" Text='<%#Eval("AmountLimit","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                                    Width="100%">
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="20%" HorizontalAlign="Right" />
                                                                            <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                        </telerik:GridTemplateColumn>
                                                                    </Columns>
                                                                </MasterTableView>
                                                                <ClientSettings>
                                                                    <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True"></Scrolling>
                                                                </ClientSettings>
                                                            </telerik:RadGrid>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>

                                                    <div class="col-md-6 col-sm-6 PaddingSpacing">
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <telerik:RadGrid ID="gvPackMedicineLimit" Width="100%" runat="server" AutoGenerateColumns="false"
                                                                    Height="125px" Skin="Metro">
                                                                    <MasterTableView Width="100%">
                                                                        <Columns>
                                                                            <telerik:GridTemplateColumn HeaderText="MediDtlID">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblMediDtlID" runat="server" SkinID="label" Text='<%#Eval("Id") %>'
                                                                                        Width="0px">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="0px" />
                                                                                <ItemStyle Width="0px" />
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="PackageId">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblMedServiceId" runat="server" SkinID="label" Text='<%#Eval("PackageId") %>'
                                                                                        Width="0px">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="0px" />
                                                                                <ItemStyle Width="0px" />
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="StoreId">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreId" runat="server" SkinID="label" Text='<%#Eval("StoreId") %>'
                                                                                        Width="0px">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="0px" />
                                                                                <ItemStyle Width="0px" />
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Store Name">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreName" runat="server" SkinID="label" Text='<%#Eval("Store") %>'
                                                                                        Width="100%">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="75%" />
                                                                                <ItemStyle Width="75%" />
                                                                            </telerik:GridTemplateColumn>
                                                                            <telerik:GridTemplateColumn HeaderText="Medicine Limit">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStoreName" runat="server" SkinID="label" Text='<%#Eval("MedicineLimit","{0:n"+common.myStr(hdnDecimalPlaces.Value)+"}") %>'
                                                                                        Width="100%">
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="25%" HorizontalAlign="Right" />
                                                                                <ItemStyle Width="25%" HorizontalAlign="Right" />
                                                                            </telerik:GridTemplateColumn>
                                                                        </Columns>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True"></Scrolling>
                                                                    </ClientSettings>
                                                                </telerik:RadGrid>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>




        <asp:UpdatePanel ID="uphidden" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnXmlString" runat="server" Value="" />
                <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="0" />
                <asp:HiddenField ID="hdnPaymentTypePyr" runat="server" Value="" />
                <asp:HiddenField ID="hdnOTRoomId" runat="server" Value="0" />
                <asp:HiddenField ID="hdnOTServiceId" runat="server" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>

</body>
</html>
