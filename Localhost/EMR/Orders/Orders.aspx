<%@ Page Title="Service Orders" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="EMR_Orders_Order" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%: Styles.Render("~/bundles/OrderStyle") %>
    <style type="text/css">
        .taborderbutton {
            background-image: url(/Images/orders.jpg);
            background-repeat: repeat-x;
            height: 22px;
            text-align: center;
        }

        input[type=checkbox], input[type=radio] {
            clear: both;
            float: none;
        }

        .tabmidbuttonactive {
            background-image: url(/Images/Butt.png);
            background-repeat: no-repeat;
            color: Black;
            height: 22px;
            text-align: center;
        }

        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        .Gridheader {
            font-family: Verdana;
            background-image: url(/Images/header.gif);
            height: 24px;
            color: black;
            font-weight: normal;
            position: relative;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>

            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-2 col-sm-2">
                            <div class="WordProcessorDivText">
                                <h2>Orders</h2>
                            </div>
                        </div>
                        <div class="col-md-5 col-sm-5">
                            <div class="WordProcessorDivText">
                                <h4>
                                    <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                                </h4>
                            </div>
                        </div>
                        <div class="col-md-5 col-sm-5">
                            <span class="orderPop">
                                <button id="liAllergyAlert" runat="server" class="btn btn-default" visible="false" style="background: #fff; border: 0px;">
                                    <asp:ImageButton ID="imgAllergyAlert" runat="server" ImageUrl="~/Icons/allergy.gif" Visible="false" CssClass="iconEMRimg" Width="18px" Height="18px" ToolTip="Allergy Alert" OnClick="imgAllergyAlert_Click" />
                                </button>
                                <button id="liMedicalAlert" runat="server" visible="false" class="btn btn-default" style="background: #fff; border: 0px;">
                                    <asp:ImageButton ID="imgMedicalAlert" runat="server" ImageUrl="~/Icons/MedicalAlert.gif" OnClick="imgMedicalAlert_OnClick" CssClass="iconEMRimg" Width="18px" Height="18px" Visible="false" ToolTip="Patient Alert" />
                                </button>
                                <asp:CheckBox ID="chkAllergyReviewed" runat="server" Text="Allergy&nbsp;List&nbsp;Reviewed" Visible="false" />
                            </span>
                            <asp:Button ID="btnClose1" Visible="false" runat="server" Text="Close" CssClass="PatientBtn01" OnClientClick="window.close();" />
                            <asp:Button ID="btnPrint" runat="server" CssClass="PatientBtn01" OnClick="btnPrint_Click" Text="Print (Ctrl+F9)" />
                            <asp:LinkButton ID="lnkAllergyDetails" Visible="false" runat="server" OnClick="lnkAllergyDetails_OnClick" PostBackUrl="#" Text="Allergy&nbsp;Details" ToolTip="Allergy&nbsp;Details" />
                            <asp:LinkButton ID="lnkAlerts" runat="server" Visible="false" CssClass="PatientBtn01" OnClick="lnkAlerts_OnClick" Text="Patient Alert" />
                            <asp:LinkButton ID="lnkLabHistory" runat="server" Visible="false" CssClass="PatientBtn01" OnClick="lnkLabHistory_OnClick" Text="DIAGNOSTIC HISTORY" />
                            <asp:Button ID="btnSave" runat="server" CssClass="PatientBtn01" Text="Save (Ctrl+F3)" ValidationGroup="ORDER" OnClick="btnSave_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />
                            <asp:Button ID="btnOrderHistory" runat="server" Text="Order History" CssClass="PatientBtn01" OnClick="btnOrderHistory_OnClick" />
                            <asp:Button ID="btnOrderSet" runat="server" Text="Add Order Set" CssClass="PatientBtn01" OnClick="btnOrderSet_OnClick" />
                            <asp:Button ID="btnAddOrderSetClose" runat="server" Style="visibility: hidden;" OnClick="btnAddOrderSetClose_OnClick" />
                            <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <asplUD:UserDetails ID="asplUD" runat="server" />
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" Visible="false" />
                    </div>
                </div>
            </div>
            <%--<table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;" cellpadding="2" cellspacing="2" width="100%">
                    <tr><td><asp:Label ID="Label4" runat="server" Text="" Font-Bold="true" Visible="false" /></td></tr>
            </table>--%>
            <asp:Panel ID="pnlAllCtrl" runat="server">
                <div class="ImmunizationDD-Div">
                    <div class="container-fluid">
                        <div class="row">

                            <div class="col-md-4">
                                <asp:Panel ID="Panel2" runat="server">
                                    <div class="orderPopLeftPart">
                                        <h2>
                                            <asp:Label ID="Label4" runat="server" Text="Search" /></h2>
                                        <h3>
                                            <asp:TextBox ID="txtSearchFavrioute" runat="server" OnTextChanged="txtSearchFavrioute_OnTextChanged" AutoPostBack="true" /></h3>
                                        <h4>
                                            <asp:ImageButton ID="btnProceedFavourite" runat="server" ToolTip="Click here to proceed selected favourite" ImageUrl="~/Images/Login/orrange-arrow.GIF" Width="18px" Height="20px" OnClick="btnProceedFavourite_OnClick" /></h4>
                                    </div>
                                    <div class="orderPopLeftPart">
                                        <asp:Panel ID="pnlFavorites" runat="server" ScrollBars="Auto" Width="100%">
                                            <asp:GridView ID="gvFavorites" runat="server" PageSize="10" AutoGenerateColumns="false" DataKeyNames="ServiceId"
                                                Autopostback="true" HeaderStyle-HorizontalAlign="Left" SkinID="gridviewOrder" Width="100%"
                                                Style="margin-bottom: 0px" OnRowCommand="gvFavorites_OnRowCommand" OnRowDataBound="gvFavorites_OnRowDataBound"
                                                HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee"
                                                HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee"
                                                BorderStyle="None" BorderWidth="1px" OnPageIndexChanging="gvFavorites_PageIndexChanging" AllowPaging="True">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Favourite" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkFAV" runat="server" Font-Size="12px" Font-Bold="false" CommandName="FAVLIST" Text='<%#Eval("ServiceName")%>' />
                                                            <asp:HiddenField ID="hdnFAvID" runat="server" Value='<%#Eval("ServiceId")%>' />
                                                            <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                            <asp:HiddenField ID="hdnSubDeptId" runat="server" Value='<%#Eval("SubDeptId")%>' />
                                                            <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("ServiceType")%>' />
                                                            <asp:HiddenField ID="hdnLabType" runat="server" Value='<%#Eval("LabType")%>' />
                                                            <asp:HiddenField ID="hdnStatOrderAllowed" runat="server" Value='<%#Eval("IsStatOrderAllowed")%>' />
                                                            <asp:HiddenField ID="hdngvFavisServiceRemarkMandatory" runat="server" Value='<%#Eval("isServiceRemarkMandatory")%>' />
                                                            <asp:HiddenField ID="hdnServiceDurationId" runat="server" Value='<%#Eval("ServiceDurationId")%>' />
                                                            <asp:HiddenField ID="hdnStationId" runat="server" Value='<%#Eval("StationId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="ibtnAddToList" runat="server" CommandName="AddToList" ToolTip="Add To List" Text="Add" Width="20" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete1" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="13px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--  <asp:TemplateField>
                                                        <ItemTemplate>                                                           
                                                            <asp:Label  ID="lblgvFavisServiceRemarkMandatory" runat="server" CommandName="AddToList" ToolTip="Add To List" Text="Add" Width="20" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                    <asp:Panel ID="gvpanel" runat="server" ScrollBars="Auto" Height="300px" Width="100%">
                                        <asp:GridView ID="gvorder" runat="server" AutoGenerateColumns="false" DataKeyNames="SetID" Width="100%"
                                            Autopostback="true" HeaderStyle-HorizontalAlign="Left" SkinID="gridviewOrder" Style="margin-bottom: 0px"
                                            OnRowCommand="gvorder_OnRowCommand" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff"
                                            HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px"
                                            PageSize="10" OnPageIndexChanging="gvorder_PageIndexChanging" AllowPaging="True">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Order Sets" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkOrder" runat="server" CommandName="OrderLIST" Font-Size="12px" Font-Bold="false" Text='<%#Eval("SetName")%>' />
                                                        <asp:HiddenField ID="hdnProblemId" runat="server" Value='<%#Eval("SetID")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </asp:Panel>
                            </div>
                            <div class="col-md-8">
                                <asp:Panel ID="Panel22" runat="server">
                                    <div class="orderPopCenterPart">
                                        <div class="container-fluid">
                                            <div class="row form-group">
                                                <div class="col-md-2 PaddingRightSpacing">
                                                    <asp:Label ID="Label5" runat="server" Text="Provisional Diagnosis" />
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:TextBox ID="txtProvisionalDiagnosis" Width="100%" runat="server" TextMode="MultiLine" ReadOnly="true" Height="25px" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="orderPopCenterPart01">
                                            <h3>
                                                <asp:RadioButtonList ID="rdoOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdoOrder_OnSelectedIndexChanged" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Laboratory" Value="G" />
                                                    <asp:ListItem Text="Radiology" Value="X" />
                                                    <%-- <asp:ListItem Text="Others" Value="O" />--%>
                                                    <asp:ListItem Text="All Services" Value="O" Selected="True" />
                                                </asp:RadioButtonList>
                                            </h3>

                                            <h4>
                                                <asp:Button ID="btnAddRequest" runat="server" CssClass="PrescriptionsRemoveBtn01" OnClick="btnAddRequest_Click" Text="Req. CT/MRI/Dexo/Spl. Invs." />
                                                <asp:Button ID="btnRequestList" runat="server" CssClass="PrescriptionsRemoveBtn01" OnClick="btnRequestList_Click" Text="View Req. Form" />
                                            </h4>
                                        </div>
                                        <div class="container-fluid">
                                            <asp:HiddenField ID="hdnStatValueContainer" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnIsServiceRemarkMandatory" runat="server" Value="0" />
                                            <div class="row form-group">
                                                <div class="col-md-8">
                                                    <div class="row">
                                                        <div class="col-md-3">
                                                            <asp:Label ID="lblServicenam1" runat="server" Text="Service&nbsp;Name" Font-Bold="true" />
                                                        </div>
                                                        <div class="col-md-9">
                                                            <asp:Panel ID="Panel3" runat="server" Width="100%" DefaultButton="btnUpdate">
                                                                <%--EmptyMessage="Search by Text"--%>
                                                                <telerik:RadComboBox ID="cmbServiceName" runat="server" Skin="Office2007" AutoPostBack="true"
                                                                    DataTextField="ServiceName" DataValueField="ServiceID" EmptyMessage=""
                                                                    EnableLoadOnDemand="true" EnableVirtualScrolling="true" Height="350px"
                                                                    HighlightTemplatedItems="true" OnItemsRequested="cmbServiceName_OnItemsRequested"
                                                                    OnSelectedIndexChanged="cmbServiceName_OnSelectedIndexChanged" ShowMoreResultsBox="true"
                                                                    Width="100%" OnClientSelectedIndexChanged="cmbServiceName_OnClientSelectedIndexChanged"
                                                                    style="border: 1px solid #8c8686;">

                                                                    <HeaderTemplate>
                                                                        <table width="100%" cellpadding="0" cellspacing="1">
                                                                            <tr>
                                                                                <td>Service(s)</td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>

                                                                    <ItemTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left"><%# DataBinder.Eval(Container, "Text")%></td>
                                                                                <td id="Td1" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['CPTCode']")%></td>
                                                                                <td id="Td2" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['LongDescription']")%></td>
                                                                                <td id="Td3" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['ServiceType']")%></td>
                                                                                <td id="Td4" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['DoctorRequired']")%></td>
                                                                                <td id="Td5" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['IsStatOrderAllowed']")%></td>
                                                                                <td id="Td6" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['isServiceRemarkMandatory']")%></td>
                                                                                <td id="Td7" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['IsLinkService']")%></td>
                                                                                <td id="Td8" runat="server" visible="false"><%# DataBinder.Eval(Container, "Attributes['StationId']")%></td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </telerik:RadComboBox>
                                                            </asp:Panel>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favourites" CssClass="btn btn-primary" OnClick="btnAddToFavourite_Click" />

                                                    <asp:CheckBox ID="chkFreeTest" runat="server" Checked="false" Font-Bold="true" Text="Free Service" CssClass="checkboxes" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row form-group">
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="ltrlInvSetName" runat="server" Text="Order&nbsp;Sets" />
                                                            <asp:Label ID="ltrlInvCategory" runat="server" Text="Department" />
                                                        </div>
                                                        <div class="col-md-8">
                                                            <%-- <telerik:RadComboBox ID="ddlDepartment" runat="server" AppendDataBoundItems="true" TabIndex="10" SkinID="DropDown" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged" AutoPostBack="true" Width="300px"  EnableVirtualScrolling="true"  EnableTextSelection="true" EnableLoadOnDemand="true"  ShowMoreResultsBox="true" MarkFirstMatch="true" />--%>
                                                            <telerik:RadComboBox ID="ddlDepartment" runat="server" AppendDataBoundItems="true" AutoPostBack="true" SkinID="DropDown" Width="100%" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged" MarkFirstMatch="true" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="Label1" runat="server" Text="Sub&nbsp;Department" />
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
                                                                    <asp:AsyncPostBackTrigger ControlID="rdoOrder" />
                                                                </Triggers>
                                                                <ContentTemplate>
                                                                    <telerik:RadComboBox ID="ddlSubDepartment" runat="server" AppendDataBoundItems="true" SkinID="DropDown" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged" />
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row form-group">

                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblloinicCode" runat="server" Text="LOINC&nbsp;Code" />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="txtLonicCode" runat="server" ReadOnly="true" Width="100%" />
                                                        </div>
                                                        <div class="col-md-5">
                                                            <div class="row">
                                                                <div class="col-md-3">
                                                                    <asp:Label ID="lblUnit" runat="server" Text="Unit" />
                                                                </div>
                                                                <div class="col-md-9">
                                                                    <asp:TextBox ID="txtUnit" runat="server" MaxLength="5" Text="1" Width="100%" />
                                                                    <AJAX:FilteredTextBoxExtender ID="ftneUnit" runat="server" Enabled="True" TargetControlID="txtUnit" FilterType="Custom,Numbers" ValidChars="."></AJAX:FilteredTextBoxExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <asp:CheckBoxList ID="chkStat" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="Stat" Value="STAT" onclick="UncheckOthers(this);"></asp:ListItem>
                                                                <asp:ListItem Text="Urgent" Value="URGENT" onclick="UncheckOthers(this);"></asp:ListItem>
                                                            </asp:CheckBoxList>
                                                            <%--<asp:CheckBox ID="chkStat" runat="server" Checked="false" Font-Bold="true" SkinID="checkbox"  Text="Stat" />--%>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:Label ID="lblICDCODE" runat="server" Text="ICD&nbsp;Codes" /><span class="red">*</span>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <input id="hdnICDCode" runat="server" type="hidden" value='<%# Eval("ICDCodes")%>' />
                                                            <input id="hdnExitOrNot" runat="server" type="hidden" value='<%# Eval("ExitOrNot")%>' />

                                                            <asp:TextBox ID="txtICDCode" runat="server" EnableViewState="true" Width="100%" />
                                                            <AJAX:PopupControlExtender ID="PopUnit" runat="server" OffsetX="5" PopupControlID="pnlICDCodes" Position="Left" TargetControlID="txtICDCode" />
                                                            <asp:Label ID="lblModifier" runat="server" SkinID="label" Visible="false" />
                                                            <asp:TextBox ID="txtModifier" runat="server" SkinID="textbox" Visible="false" Width="130px" />
                                                            <AJAX:PopupControlExtender ID="pcEtxtModifier" runat="server" OffsetX="5" PopupControlID="pnlModifierCode" Position="Right" TargetControlID="txtModifier" />
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row form-group">

                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-4">Service Duration:</div>
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="ddlServiceDuration" SkinID="DropDown" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:CheckBox ID="chkIsBioHazard" runat="server" TabIndex="43" Text="Is Biohazard" TextAlign="Right" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="container-fluid">
                                            <div class="row form-group">

                                                <!-- Abhishek Goel -->
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-md-4">Investigation Date</div>
                                                        <div class="col-md-8">
                                                            <div class="row">
                                                                <div class="col-md-7">
                                                                    <telerik:RadDateTimePicker ID="RadDateTimePicker1" TabIndex="37" runat="server" AutoPostBackControl="Both" Width="150px" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" TabIndex="38" AutoPostBack="True" Height="300px" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged" Skin="Outlook" Width="50px" />
                                                                </div>
                                                                <div class="col-md-2 PaddingSpacing">
                                                                    <asp:Literal ID="ltDateTime" runat="server" Text="HH MM" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!-- Abhishek Goel -->
                                                <div class="col-md-6">
                                                    <div class="row form-group">
                                                        <div class="col-md-4">
                                                            <asp:Label ID="lblremark1" runat="server" Text="Remarks / Rationale / Clinical Indication" Font-Size="8pt" />
                                                        </div>
                                                        <div class="col-md-8">
                                                            <asp:TextBox ID="txtInstruction" runat="server" TabIndex="40" TextMode="MultiLine" Height="40px" />
                                                            <asp:DropDownList ID="ddlbImgCntr" runat="server" SkinID="DropDown" Visible="false" Width="1px">
                                                                <asp:ListItem Text="Select" Value="0" />
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="ddlOrg" runat="server" SkinID="DropDown" Visible="false" Width="1px">
                                                                <asp:ListItem Text="Select" Value="0" />
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                                    <div class="row form-group">
                                                        <div class="col-md-12">
                                                            <asp:CheckBox ID="chkApprovalRequired" runat="server"   AutoPostBack="true"  Text="Verbal/Telephonic" TextAlign="Right"
                                        OnCheckedChanged="chkApprovalRequired_OnCheckedChanged" />

                                                            <asp:CheckBox ID="chkIsReadBack" runat="server" TabIndex="43" Text="Read Back" TextAlign="Right" Visible="false" />

                                                           
                                            <asp:Label ID="lblReadBackNote" runat="server" Text="Read&nbsp;Back&nbsp;Note" Visible="false"  ></asp:Label>
                                                            <asp:TextBox ID="txtIsReadBackNote" runat="server" SkinID="textbox" TextMode="MultiLine" Width="100%" Visible="false" CssClass="pull-left"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                </div>

                                        <div class="container-fluid">
                                            <div class="row form-group">
                                                <div class="col-md-6">
                                                    <div class="row" id="trAssignToEmp" runat="server">
                                                        <div class="col-md-4">
                                                            <asp:Literal ID="lblAssignToEmpId" runat="server" Text="Result Finalized By" />
                                                        </div>
                                                        <div class="col-md-8">
                                                            <telerik:RadComboBox ID="ddlAssignToEmpId" SkinID="DropDown" runat="server" EmptyMessage="[ Select Employee ]"
                                                                Width="250px" MarkFirstMatch="true" Filter="Contains" Enabled="false" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 pull-right">
                                                    <div class="row form-group pull-right">
                                                        <div class="col-md-12">
                                                            <asp:Button ID="btnAddPackage" Style="float: right;" runat="server" Text="Add Package Details" SkinID="button" OnClick="btnAddPackage_Click" Visible="false" />
                                                            <asp:Label ID="lblPackageId" runat="server" Visible="false" />
                                                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" CssClass="PrescriptionsAddBtnLeft" Text="Add To List" ValidationGroup="ORDER" OnClick="btnUpdate_Click" />

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row form-group" id="trContrast" runat="server" visible="false">
                                                <div class="col-md-2">
                                                    Contrast
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RadioButtonList ID="chklstcnt" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="With" />
                                                        <asp:ListItem Text="Without" />
                                                        <asp:ListItem Text="Both" />
                                                    </asp:RadioButtonList>
                                                </div>
                                                <div class="col-md-7">
                                                    <asp:Label ID="lblAlertMessage" runat="server" Text="" Font-Bold="true" />
                                                </div>
                                            </div>
                                            <div class="row form-group" id="Div1" runat="server" visible="false">
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnCloseResultMeasure" runat="server" CausesValidation="false" Style="visibility: hidden;" />&nbsp;
                                                </div>
                                                <div class="col-md-10">
                                                    <asp:LinkButton ID="lnkbtnSplInfo" runat="server" Text="Special Information Needed" Visible="false" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="orderPopCenterPart02">
                                            <asp:Panel ID="grdpanl" runat="server" ScrollBars="Auto" Height="180px">
                                                <asp:GridView ID="gvPatientServiceDetail" runat="server" AutoGenerateColumns="false" TabIndex="49"
                                                    HeaderStyle-HorizontalAlign="Left" OnRowDataBound="gvPatientServiceDetail_RowDataBound"
                                                    OnRowDeleting="gvPatientServiceDetail_RowDeleting" ShowHeader="true" SkinID="gridviewOrder"
                                                    Style="margin-bottom: 0px" Width="100%" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                    BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px">
                                                    <PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="6" />
                                                    <%--<RowStyle Wrap="false" />--%>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Font-Bold="true" ForeColor="Red" Text="No Record Found." />
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField DataField="CPTCode" HeaderText="RefService Code" Visible="false" />
                                                        <asp:TemplateField HeaderText="Services">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                                <asp:HiddenField ID="hdnExcludedServices" runat="server" Value='<%#Eval("IsExcluded")%>' />
                                                                <asp:HiddenField ID="hdnRequestToDepartment" runat="server" Value='<%#Eval("RequestToDepartment") %>' />
                                                                <asp:HiddenField ID="hdnServiceID" runat="server" Value='<%#Eval("ServiceID") %>' />
                                                                <asp:HiddenField ID="hdnresult" runat="server" Value='<%#Eval("result") %>' />
                                                                <asp:HiddenField ID="hdnStat" runat="server" Value='<%#Eval("Stat") %>' />
                                                                <asp:HiddenField ID="HdnUrgent" runat="server" Value='<%#Eval("Urgent") %>' />
                                                                <asp:HiddenField ID="hdnAlertRequired" runat="server" Value='<%#Eval("AlertRequired") %>' />
                                                                <asp:HiddenField ID="hdnAlertMessage" runat="server" Value='<%#Eval("AlertMessage") %>' />
                                                                <asp:HiddenField ID="HiddenField2" runat="server" Value='<%#Eval("Stat") %>' />
                                                                <asp:HiddenField ID="hdnIsTemplateRequired" runat="server" />
                                                                <asp:HiddenField ID="hdnFreeTest" runat="server" Value='<%#Eval("FreeTest") %>' />
                                                                <%--<asp:HiddenField ID="hdngvIsServiceRemarkMandatory" runat="server" Value='<%#Eval("isServiceRemarkMandatory")%>' />--%>
                                                                <asp:HiddenField ID="hdnServiceDurationId" runat="server" Value='<%#Eval("ServiceDurationId") %>' />
                                                                <asp:HiddenField ID="hdnBiohazard" runat="server" Value='<%#Eval("IsBioHazard") %>' />
                                                                <asp:HiddenField ID="hdnServiceType" runat="server" Value='<%#Eval("ServiceType")%>' />
                                                                <asp:HiddenField ID="hdnStationId" runat="server" Value='<%#Eval("StationId")%>' />
                                                                <asp:HiddenField ID="hdnAssignToEmpId" runat="server" Value='<%#Eval("AssignToEmpId") %>' />
                                                                <asp:HiddenField ID="hdnProviderid" runat="server" Value='<%#Eval("Providerid") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Investigation Date" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTestDate" runat="server" Text='<%# string.Format("{0:dd/MM/yyyy HH:mm tt}",Eval("TestDate")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Units" HeaderStyle-Width="25px" ItemStyle-Width="25px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUnits" runat="server" Text='<%#Eval("Units")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Charges" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnIsPriceEditableFromEMR" runat="server" Value='<%# Eval("IsPriceEditableFromEMR") %>' />
                                                                <asp:TextBox ID="txtcharges" Visible="false" runat="server" Text='<%#Eval("Charges","{0:f2}")%>' />
                                                                <AJAX:FilteredTextBoxExtender ID="ftnecharges" runat="server" Enabled="True" TargetControlID="txtcharges" FilterType="Custom,Numbers" ValidChars="."></AJAX:FilteredTextBoxExtender>
                                                                <asp:Label ID="lblCharges" runat="server" Text='<%#Eval("Charges","{0:f2}")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkAddInvestigationSpecification" runat="server" OnClick="lnkAddInvestigationSpecification_OnClick" CommandName="Template" Text="Request Form" ToolTip="Add Investigation Specification" CommandArgument='<%# Eval("ServiceId") %>' ForeColor="DodgerBlue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="55px" ItemStyle-Width="55px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkServiceName" runat="server" OnClick="lnkServiceName_OnClick" CommandName="Template" Text="Inv. Dt." ToolTip="Investigation Details" CommandArgument='<%# Eval("ServiceId") %>' ForeColor="DodgerBlue" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblgvIsServiceRemarkMandatory" runat="server" Text='<%#Eval("isServiceRemarkMandatory")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:PRegistration, Provider %>" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDoctorID" runat="server" Text='<%#Eval("DoctorID") %>' />
                                                                <telerik:RadComboBox ID="ddlDoctor" runat="server" Filter="Contains" MarkFirstMatch="true"
                                                                    Width="150px" Skin="Metro" Height="250px" DropDownWidth="300px" />
                                                                <asp:HiddenField ID="hdnDocReq" runat="server" Value='<%#Eval("DoctorRequired")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="~/Images/edit.png" OnClick="ibtnEdit_Click" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField ButtonType="Image" ItemStyle-HorizontalAlign="Center" DeleteImageUrl="~/Images/DeleteRow.png" HeaderStyle-Width="20px" ItemStyle-Width="20px" ShowDeleteButton="true" />
                                                    </Columns>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:GridView>
                                                <asp:HiddenField ID="hdnID" runat="server" Value="0" />
                                                <asp:HiddenField ID="hdnServiceType" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnGlobleStationId" runat="server" Value="" />
                                                <asp:HiddenField ID="hdnServiceId" runat="server" />
                                                <asp:HiddenField ID="hdnServiceName" runat="server" />
                                                <asp:HiddenField ID="hdnLabStatus" runat="server" />
                                                <asp:HiddenField ID="hdnOrderId" runat="server" />
                                                <asp:HiddenField ID="hdnEncodedBy" runat="server" />
                                                <asp:HiddenField ID="hdnPatientGender" runat="server" />
                                                <asp:HiddenField ID="hdnLongDescription" runat="server" />
                                                <asp:HiddenField ID="hdnDoctorRequired" runat="server" />
                                                <asp:HiddenField ID="hdnDepartmentRequest" runat="server" />
                                                <asp:HiddenField ID="hdnAlertRequired" runat="server" />
                                                <asp:HiddenField ID="hdnAlertMessage" runat="server" />
                                                <asp:HiddenField ID="hdngvIsServiceRemarkMandatory" runat="server" />
                                                <asp:HiddenField ID="hdnCharges" runat="server" />
                                                <telerik:RadWindowManager ID="RadWindowManager2" ShowContentDuringLoad="false" EnableViewState="false" VisibleStatusbar="false" ReloadOnShow="true" DestroyOnClose="true" runat="server">
                                                    <Windows>
                                                        <telerik:RadWindow runat="server" ID="RadWindow2"></telerik:RadWindow>
                                                    </Windows>
                                                </telerik:RadWindowManager>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="orderPopCenterBottomPart">
                                <div class="orderPopCenterBottomPart02">
                                    <span>
                                        <asp:Button ID="btnAddRequiredTemplate" runat="server" SkinID="button" Text="Add Investigation Specification" OnClick="btnAddRequiredTemplate_Click" Visible="false" />
                                    </span>
                                    <h2>
                                        <asp:Label ID="Label15" runat="server" Text="Total Charges :" />
                                        <asp:Label ID="lblTotCharges" runat="server" Text="0" Font-Size="Large" />
                                    </h2>
                                    <span>
                                        <asp:CheckBox ID="chkPullOrder" runat="server" Text="Pull Forward From Prior Exam" TextAlign="Right" Visible="false" />
                                    </span>
                                    <h2>
                                        <asp:Label ID="lblColorCode" runat="server" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                                        <asp:Label ID="Label3" runat="server" SkinID="label" Text="Excluded Service" />
                                    </h2>
                                    <h2>
                                        <asp:Label ID="lblColorCodeForMandatoryTemplate" runat="server" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                                        <asp:Label ID="Label14" runat="server" SkinID="label" Text="Service Specification(s) Optional" />
                                    </h2>
                                    <h2>
                                        <asp:Label ID="lblColorCodeForTemplateRequired" runat="server" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                                        <asp:Label ID="Label12" runat="server" SkinID="label" Text="Service Specification(s) Mandatory" />
                                    </h2>
                                    <h2>
                                        <asp:Label ID="lblColorStat" runat="server" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                                        <asp:Label ID="Label8" runat="server" SkinID="label" Text="Stat" />
                                    </h2>
                                    <h2>
                                        <asp:Label ID="lblFreeTest" runat="server" BorderWidth="1px" Text="&nbsp;" Width="16px" Height="16px" />
                                        <asp:Label ID="Label9" runat="server" SkinID="label" Text="Free Service" />
                                    </h2>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>


            <asp:Panel BorderStyle="Solid" BorderWidth="1px" ID="pnlICDCodes" Style="visibility: hidden; position: absolute; width: 370px;" BackColor="#E0EBFD" runat="server" Height="120px">
                <table width="100%" border="0" style="margin: 10px 0; padding: 0;">
                    <tr>
                        <td>
                            <%-- <asp:UpdatePanel ID="update" runat="server"> <ContentTemplate>--%>
                            <aspl:ICD ID="icd" runat="server" width="400" PanelName="ctl00_ContentPlaceHolder1_pnlICDCodes" ICDTextBox="ctl00_ContentPlaceHolder1_txtICDCode"></aspl:ICD>
                            <asp:HiddenField ID="hdnGridClientId" runat="server" />
                            <%-- </ContentTemplate></asp:UpdatePanel>--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="Panel1" runat="server">
                <div id="divDelete" runat="server" visible="false" style="width: 250px; z-index: 100; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; background-color: #FFF8DC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; position: absolute; bottom: 0; height: 75px; left: 470px; top: 355px">
                    <table width="100%" border="0">
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="lblTitle" runat="server" Text="Do you want to delete ?" SkinID="label" Font-Bold="true" Font-Size="Small" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_OnClick" SkinID="Button" Width="60px" />
                                <asp:HiddenField ID="hdnUpdateServiceId" runat="server" />
                                <asp:HiddenField ID="hdnUpdateOrderDtlId" runat="server" />
                            </td>
                            <td>&nbsp;<asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_OnClick" SkinID="Button" Width="60px" /></td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel ID="updDelete" runat="server">
        <ContentTemplate>
            <div id="dvRedirect" runat="server" visible="false" style="width: 400px; z-index: 100; border-bottom: 7px solid #ADD8E6; border-left: 7px solid #ADD8E6; background-color: White; border-right: 7px solid #ADD8E6; border-top: 7px solid #ADD8E6; position: absolute; background-color: #FFF8DC; bottom: 0; left: 450px; top: 300px; height: 200px;">
                <table cellpadding="0" cellspacing="0" align="center" width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btnClose" runat="server" ToolTip="Close" SkinID="Button" Text="Close" OnClick="btnClose_Click" /></td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Literal ID="litRequiredInvestigationSpecification" runat="server"></asp:Literal></td>
                    </tr>
                    <tr align="center">
                        <td style="padding-top: 10px;">
                            <asp:Button ID="btnRedirect" runat="server" SkinID="button" Width="200px" Text="Add Investigation Specification" OnClick="btnAddRequiredTemplate_Click" /></td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hdnDecimalPlaces" runat="server" Value="2" />
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel ID="updDivConfirm" runat="server">
        <ContentTemplate>
            <div id="dvConfirmAlreadyExistOptions" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 140px; left: 270px; top: 200px;">
                <table cellspacing="2" cellpadding="2" width="400px">
                    <tr>
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="lblSn" runat="server" Text="Service name :" ForeColor="#990066" /></td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblServiceName" runat="server" ForeColor="#990066" /></td>
                    </tr>
                    <tr>
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="Label2" runat="server" Text="Already posted by :" ForeColor="#990066" /></td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblEnteredBy" runat="server" ForeColor="#990066" /></td>
                    </tr>
                    <tr style="border-bottom-style: solid; border-bottom-width: 1px;">
                        <td style="width: 30%; text-align: left;">
                            <asp:Label ID="Label6" runat="server" Text="Posted date :" ForeColor="#990066" /></td>
                        <td style="width: 70%; text-align: left;">
                            <asp:Label ID="lblEnteredOn" runat="server" ForeColor="#990066" /></td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            <asp:Label ID="lblAlertMsg" runat="server" Font-Size="12px" Text="Do you wish to continue...?" ForeColor="#990066" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            <asp:Button ID="btnAlredyExistProceed" runat="server" Text="Proceed" OnClick="btnAlredyExistProceed_OnClick" SkinID="Button" />&nbsp;&nbsp;
                           
                            <asp:Button ID="btnAlredyExistCancel" runat="server" Text="Cancel" OnClick="btnAlredyExistCancel_OnClick" SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="divConfirmation" runat="server" style="width: 450px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 60px; left: 270px; top: 200px;">
                <table cellspacing="2" cellpadding="2">
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Label ID="Label11" runat="server" Text="Selected service is blocked for this company. Do you Want to Continue?" ForeColor="#990066" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Button ID="btnProceed" runat="server" Text="Yes" OnClick="btnProceed_OnClick"
                                SkinID="Button" />
                            &nbsp;&nbsp;
                           
                            <asp:Button ID="btnProceedCancel" runat="server" Text="No" OnClick="btnProceedCancel_OnClick"
                                SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdPageFlag" runat="server" />

    <script type="text/javascript">

        function AddOrderSet_OnClientClose(oWnd, args) {
            $get('<%=btnAddOrderSetClose.ClientID%>').click();
        }

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
                case 119:  // F8
                    $get('<%=btnClose1.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrint.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }


        function SelectAllFavourite(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvFavorites.ClientID%>");
            //variable to contain the cell of the grid
            var cell;
            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function openWin(inv) {
            oWnd = radopen("/EMR/Orders/PrintOrder.aspx", "RadWindow2");
            oWnd.setSize(1000, 600);

            oWnd.Center();
        }
        function pageLoad() {
        }

        function OnClientCloseResultMeasure(oWnd, args) {
            $get('<%=btnCloseResultMeasure.ClientID%>').click();
        }


        function OnClientClose(oWnd, args) {

        }

        function ShowLeftPnl() {
            //$get("pnlLeft").style.visibility = 'visible't
        }

        function HideLeftPnl() {
            //$get("pnlLeft").style.visibility = 'hidden';
        }

        function CheckEmptyDate() {
            if ($get('txtFrom').value == "" || $get('txtTo').value == "") {
                alert("Please select Date first.");
                return false;
            }
        }

        // Function to show modifier popup window
        function ShowModifierPanelOnChangeDropDown(CtrlDDL, CtrlNewText, ctrlname) {
            var DropdownList = document.getElementById(CtrlDDL);
            var txt = document.getElementById(CtrlNewText);
            var dd = DropdownList.value;
            if (txt.value == null || txt.value == '') {
                txt.value = dd;
            }
            else {
                txt.value = txt.value + ',' + dd;
            }

            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
            DropdownList.tooltip = DropdownList.selecteditem;
        }

        function showModifierPanel(ctrlPanel) {
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
        }



        //         window.onbeforeunload = function(evt) {
        //             var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
        //             if (IsUnsave == 1) {
        //                 return false;
        //             }
        //         }



        function HidePanelOKClick(ctrlPanel, ctrlCheckBox, ctrlLabel, HICDCode) {
            var browserName = navigator.appName;
            var txt = document.getElementById(ctrlLabel);
            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'hidden';
            var tableElement = document.getElementById(ctrlCheckBox);
            if (tableElement != null) {
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                    if (col.checked == true) {
                        if (ICDCodes == '') {
                            if (browserName == "Netscape") {
                                ICDCodes = chklabel.textContent;
                            }
                            else {
                                ICDCodes = chklabel.innerText;
                            }
                        }
                        else {
                            if (browserName == "Netscape") {
                                ICDCodes = ICDCodes + ',' + chklabel.textContent;
                            }
                            else {
                                ICDCodes = ICDCodes + ',' + chklabel.innerText;

                            }
                        }
                    }
                }
            }
            document.getElementById(HICDCode).value = ICDCodes;
            txt.value = ICDCodes;
            tt.style.visibility = 'hidden';
        }




        function ShowICDPanel(ctrlPanel, txt1) {
            var ICDarr = new Array();
            var txt = document.getElementById('<%=txtICDCode.ClientID%>');
            ICDarr = txt.value.split(',');

            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
            var tableElement = document.getElementById('rptrICDCodes');

            if (tableElement != null) {
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    col.checked = false;
                }

                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                    for (var j = 0; j < ICDarr.length; j++) {
                        if (chklabel.innerText == ICDarr[j]) {
                            col.checked = true;
                        }
                    }
                }
            }
        }

        function HideICDPanel(ctrlname) {
            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
        }


        function BindContact(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var ContactId = arg.ContactId;
                var CompanyName = arg.CompanyName;
                //               
            }
            //document.getElementById(HICDCode).value = ICDCodes;
            //alert(cptCode);
        }
        function cmbServiceName_OnClientSelectedIndexChanged(sender, args) {

            var item = args.get_item();
            $get('<%=hdnStatValueContainer.ClientID%>').value = item != null ? item.get_attributes().getAttribute("IsStatOrderAllowed") : " ";
            $get('<%=hdnIsServiceRemarkMandatory.ClientID%>').value = item != null ? item.get_attributes().getAttribute("isServiceRemarkMandatory") : " ";
            $get('<%=hdnServiceType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("ServiceType") : " ";
            $get('<%=hdnGlobleStationId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("StationId") : " ";

        }

        function UncheckOthers(objchkbox) {
            //Get the parent control of checkbox which is the checkbox list
            var objchkList = objchkbox.parentNode.parentNode.parentNode;
            //Get the checkbox controls in checkboxlist
            var chkboxControls = objchkList.getElementsByTagName("input");
            //Loop through each check box controls
            for (var i = 0; i < chkboxControls.length; i++) {
                //Check the current checkbox is not the one user selected
                if (chkboxControls[i] != objchkbox && objchkbox.checked) {
                    //Uncheck all other checkboxes
                    chkboxControls[i].checked = false;
                }
            }
        }

    </script>

    <%--  <script type="text/javascript">

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
                case 119:  // F8
                    $get('<%=btnClose1.ClientID%>').click();
                    break;
                case 120:  // F9
                    $get('<%=btnPrint.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }

        function SelectAllFavourite(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvFavorites.ClientID%>");
            //variable to contain the cell of the grid
            var cell;
            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function openWin(inv) {
            oWnd = radopen("/EMR/Orders/PrintOrder.aspx", "RadWindow2");
            oWnd.setSize(1000, 600);

            oWnd.Center();
        }
        function pageLoad() {
        }

        function OnClientCloseResultMeasure(oWnd, args) {
            $get('<%=btnCloseResultMeasure.ClientID%>').click();
        }


        function OnClientClose(oWnd, args) {

        }

        function ShowLeftPnl() {
            //$get("pnlLeft").style.visibility = 'visible't
        }

        function HideLeftPnl() {
            //$get("pnlLeft").style.visibility = 'hidden';
        }

        function CheckEmptyDate() {
            if ($get('txtFrom').value == "" || $get('txtTo').value == "") {
                alert("Please select Date first.");
                return false;
            }
        }

        // Function to show modifier popup window
        function ShowModifierPanelOnChangeDropDown(CtrlDDL, CtrlNewText, ctrlname) {
            var DropdownList = document.getElementById(CtrlDDL);
            var txt = document.getElementById(CtrlNewText);
            var dd = DropdownList.value;
            if (txt.value == null || txt.value == '') {
                txt.value = dd;
            }
            else {
                txt.value = txt.value + ',' + dd;
            }

            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
            DropdownList.tooltip = DropdownList.selecteditem;
        }

        function showModifierPanel(ctrlPanel) {
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
        }
        window.onbeforeunload = function(evt) {
            var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            if (IsUnsave == 1) {
                return false;
            }
        }
        function HidePanelOKClick(ctrlPanel, ctrlCheckBox, ctrlLabel, HICDCode) {
            var browserName = navigator.appName;
            var txt = document.getElementById(ctrlLabel);
            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'hidden';
            var tableElement = document.getElementById(ctrlCheckBox);
            if (tableElement != null) {
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                    if (col.checked == true) {
                        if (ICDCodes == '') {
                            if (browserName == "Netscape") {
                                ICDCodes = chklabel.textContent;
                            }
                            else {
                                ICDCodes = chklabel.innerText;
                            }
                        }
                        else {
                            if (browserName == "Netscape") {
                                ICDCodes = ICDCodes + ',' + chklabel.textContent;
                            }
                            else {
                                ICDCodes = ICDCodes + ',' + chklabel.innerText;

                            }
                        }
                    }
                }
            }
            document.getElementById(HICDCode).value = ICDCodes;
            txt.value = ICDCodes;
            tt.style.visibility = 'hidden';
        }

        function ShowICDPanel(ctrlPanel, txt1) {
            var ICDarr = new Array();
            var txt = document.getElementById('<%=txtICDCode.ClientID%>');
            ICDarr = txt.value.split(',');

            var ICDCodes = '';
            var tt = document.getElementById(ctrlPanel);
            tt.style.visibility = 'visible';
            var tableElement = document.getElementById('rptrICDCodes');

            if (tableElement != null) {
                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    col.checked = false;
                }

                for (var i = 0; i < tableElement.rows.length; i++) {
                    var rowElem = tableElement.rows[i];
                    var col = rowElem.cells[0].childNodes[0];
                    var chklabel = rowElem.cells[0].childNodes[1];
                    for (var j = 0; j < ICDarr.length; j++) {
                        if (chklabel.innerText == ICDarr[j]) {
                            col.checked = true;
                        }
                    }
                }
            }
        }

        function HideICDPanel(ctrlname) {
            var tt = document.getElementById(ctrlname);
            tt.style.visibility = 'hidden';
        }

        function BindContact(oWnd, args) {
            var arg = args.get_argument();
            if (arg) {
                var ContactId = arg.ContactId;
                var CompanyName = arg.CompanyName;
                //               
            }
            //document.getElementById(HICDCode).value = ICDCodes;
            //alert(cptCode);
        }
    </script>
    --%>

    <script type="text/javascript">
        function ClientSideClick(myButton) {
            // Client side validation
            if (typeof (Page_ClientValidate) == 'function') {
                if (Page_ClientValidate() == false)
                { return false; }
            }

            //make sure the button is not of type "submit" but "button"
            if (myButton.getAttribute('type') == 'button') {
                // disable the button
                myButton.disabled = true;
                myButton.className = "btn-inactive";
                myButton.className += " PatientBtn01";
                myButton.value = "Save (Ctrl+F3)";

                //display message
                //document.getElementById("message-div").style.display = "block";
                //document.getElementById("divbtnSaveConfirm").style.display = "none";

            }
            return true;
        }
    </script>
</asp:Content>
