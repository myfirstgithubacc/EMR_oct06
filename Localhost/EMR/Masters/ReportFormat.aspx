<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportFormat.aspx.cs" Inherits="EMR_Masters_ReportFormat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Discharge Summry Report Format</title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <style>
        /*#txtReportFooterText {
            margin: 0 0 0 -5px !important;
        }*/

        .auto-style1 {
            margin-bottom: 0;
        }


        /*<%------------------------------------Yogesh------------1/04/2022----------------%>*/

        .Background {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .Popup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 350px;
        }

        div#Panel1 {
            width: 70%;
            height: 80%;
            left: 200px !important;
        }

       

        /*<%------------------------------------Yogesh------------1/04/2022----------------%>*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container-fluid header_main margin_bottom">
            <div class="col-sm-3"></div>
            <div class="col-sm-5">
                <asp:Label ID="lblMessage" runat="server" Text="" CssClass="relativ alert_new text-center text-success" />
            </div>
            <div class="col-sm-4 text-right">
                <%------------------------------------Yogesh------------1/04/2022----------------%>
                <asp:Button ID="btnHospitalSettings" runat="server" Text="Image Settings" CssClass="btn btn-primary" />


                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panl1" TargetControlID="btnHospitalSettings"
                    CancelControlID="btnCloseHS" BackgroundCssClass="Background">
                </cc1:ModalPopupExtender>


                <%------------------------------------Yogesh------------1/04/2022----------------%>

                <asp:Button ID="btnHospitalSetup" runat="server" CssClass="btn btn-primary" Text="Hospital Setup" ToolTip="Hospital Setup" />
                <cc1:ModalPopupExtender ID="ModalPopupExtenderHospitalSetup" runat="server" PopupControlID="Panel1" TargetControlID="btnHospitalSetup"
                    CancelControlID="Button1" BackgroundCssClass="Background">
                </cc1:ModalPopupExtender>

                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Clear" ToolTip="Clear"
                    OnClick="btnClear_OnClick" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_OnClick" />
                <asp:Button ID="ibtnClose" runat="server" CssClass="btn btn-primary" Text="Close" ToolTip="Close"
                    OnClientClick="window.close();" />

            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <div class="container-fluid" style="overflow-x: hidden;">
                    <div class="row">
                        <div class="col-sm-6">
                            <telerik:RadGrid ID="gvReport" runat="server" Skin="Office2007" Width="100%" PagerStyle-ShowPagerText="false"
                                AllowSorting="False" AllowMultiRowSelection="False" EnableLinqExpressions="false"
                                ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                GridLines="none" Height="480px" OnItemCommand="gvReport_ItemCommand">
                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                        AllowColumnResize="false" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="307px" />
                                </ClientSettings>
                                <MasterTableView Width="100%">
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.
                                        </div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                        <%-- <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, serialno %>'
                                HeaderStyle-Width="0%"  ItemStyle-Width="0%">
                                <ItemTemplate>
                                    <%# Container.ItemIndex+1 %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                                        <telerik:GridTemplateColumn HeaderText="ReportId" HeaderStyle-Width="0px" Visible="false"
                                            ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReportId" runat="server" Text='<%#Eval("ReportId") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Report Name" HeaderStyle-Width="200px" ItemStyle-Width="450px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReportName" runat="server" Text='<%#Eval("ReportName")%>' />
                                                <asp:HiddenField ID="hdnHeaderId" runat="server" Value='<%#Eval("HeaderId")%>' />
                                                <asp:HiddenField ID="hdnIsPrintHospitalHeader" runat="server" Value='<%#Eval("IsPrintHospitalHeader")%>' />
                                                <asp:HiddenField ID="hdnIsPrintDoctorSignature" runat="server" Value='<%#Eval("IsPrintDoctorSignature")%>' />
                                                <asp:HiddenField ID="hdnIsShowFilledTemplates" runat="server" Value='<%#Eval("IsShowFilledTemplates")%>' />
                                                <asp:HiddenField ID="hdnIsCheckListRequired" runat="server" Value='<%#Eval("IsCheckListRequired")%>' />
                                                <asp:HiddenField ID="hdnIsDefaultForOp" runat="server" Value='<%#Eval("IsDefaultForOp")%>' />
                                                <asp:HiddenField ID="hdnPageSize" runat="server" Value='<%#Eval("PageSize")%>' />
                                                <%--change palendra--%>
                                                <asp:HiddenField ID="hdnPrintHeaderImage" runat="server" Value='<%#Eval("PrintHeaderImage")%>' />
                                                <asp:HiddenField ID="hdnPrintFooterImage" runat="server" Value='<%#Eval("PrintFooterImage")%>' />
                                                <asp:HiddenField ID="hdnPrintHeaderImageUrl" runat="server" Value='<%#Eval("PrintHeaderImagePath")%>' />
                                                <asp:HiddenField ID="hdnPrintFooterImageURL" runat="server" Value='<%#Eval("PrintFooterImagePath")%>' />
                                                <asp:HiddenField ID="hdnPrintVersionCode" runat="server" Value='<%#Eval("PrintVersionCode")%>' />
                                                <%--change palendra--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Heading Name" HeaderStyle-Width="180px" ItemStyle-Width="350px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblHeadingName" runat="server" Text='<%#Eval("HeadingName") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Show Page No" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShowPageNoInPageFooter" runat="server" Text='<%#Eval("ShowPageNoInPageFooter") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Show Print By" HeaderStyle-Width="100px"
                                            ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShowPrintByInPageFooter" runat="server" Text='<%#Eval("ShowPrintByInPageFooter") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Show Print Date" HeaderStyle-Width="100px"
                                            ItemStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShowPrintDateInPageFooter" runat="server" Text='<%#Eval("ShowPrintDateInPageFooter") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="56px" ItemStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>' />
                                                <asp:HiddenField ID="hdnActive" runat="server" Value='<%#Eval("Active") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Report Type" HeaderStyle-Width="88px" ItemStyle-Width="90px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReportType" runat="server" Text='<%#Eval("ReportType") %>' />
                                                <asp:HiddenField ID="hdnMarginLeft" runat="server" Value='<%#Eval("MarginLeft") %>' />
                                                <asp:HiddenField ID="hdnMarginRight" runat="server" Value='<%#Eval("MarginRight") %>' />
                                                <asp:HiddenField ID="hdnMarginTop" runat="server" Value='<%#Eval("MarginTop") %>' />
                                                <asp:HiddenField ID="hdnMarginBottom" runat="server" Value='<%#Eval("MarginBottom") %>' />
                                                <asp:HiddenField ID="hdnReportFooterText" runat="server" Value='<%#Eval("ReportFooterText") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>



                                        <%-- yogesh --%>

                                        <telerik:GridTemplateColumn HeaderText="SignDoctorHeight" HeaderStyle-Width="450px" Visible="false"
                                            ItemStyle-Width="450px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="lbldocheight" runat="server" Value='<%#Eval("SignDoctorHeight") %> ' />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>



                                        <telerik:GridTemplateColumn HeaderText="SignDoctorWidth" HeaderStyle-Width="0px" Visible="false"
                                            ItemStyle-Width="0px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="docWidth" runat="server" Value='<%#Eval("SignDoctorWidth") %>' />

                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="" HeaderTooltip="Delete a row">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnSelect" Text="Select" CommandName="Select" runat="server" OnClick="lbtnSelect_Click" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <%--,MarginLeft,MarginRight,MarginTop,MarginBottom--%>
                                        <%--  <telerik:GridTemplateColumn HeaderText="Margin Left" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportType" runat="server" Text='<%#Eval("MarginLeft") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                   <telerik:GridTemplateColumn HeaderText="Margin Right" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportType" runat="server" Text='<%#Eval("MarginRight") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                   <telerik:GridTemplateColumn HeaderText="Margin Top" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportType" runat="server" Text='<%#Eval("MarginTop") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                   <telerik:GridTemplateColumn HeaderText="Margin Bottom" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportType" runat="server" Text='<%#Eval("MarginBottom") %>' />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>

                        <div class="col-sm-6">
                            <div class="row">

                                <div class="col-sm-6">
                                    <div class="row">
                                        <div class="col-sm-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="label3" runat="server" SkinID="label" Text="Report Name" />&nbsp;<span
                                                        style='color: Red'>*</span>
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:TextBox ID="txtReportName" SkinID="textbox" runat="server" Text="" Width="100%"
                                                        MaxLength="100" AutoCompleteType="None" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="label1" runat="server" SkinID="label" Text="Report Heading" />
                                                </div>


                                                <div class="col-sm-12">
                                                    <asp:TextBox ID="txtReportHead" SkinID="textbox" runat="server" Text="" Width="100%"
                                                        MaxLength="100" AutoCompleteType="None" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="label4" runat="server" SkinID="label" Text="Report Type" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <telerik:RadComboBox ID="ddlReportType" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="" Value="" />
                                                            <telerik:RadComboBoxItem Text="Discharge Summary" Value="DI" />
                                                            <telerik:RadComboBoxItem Text="Death Summary" Value="DE" />
                                                            <telerik:RadComboBoxItem Text="Health Checkup Summary" Value="HC" />
                                                            <telerik:RadComboBoxItem Text="Medical Summary" Value="MS" />
                                                            <telerik:RadComboBoxItem Text="Patient Report" Value="PR" />
                                                            <telerik:RadComboBoxItem Text="Print EMR Template" Value="PT" />
                                                            <telerik:RadComboBoxItem Text="Immunization Report" Value="IM" />
                                                            <telerik:RadComboBoxItem Text="Case Summary" Value="CS" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="label5" runat="server" SkinID="label" Text="Report Header" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <telerik:RadComboBox ID="ddlClinicalHeaderId" runat="server" Width="100%">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="" Value="" />
                                                            <telerik:RadComboBoxItem Text="Discharge Summary" Value="DI" />
                                                            <telerik:RadComboBoxItem Text="Death Summary" Value="DE" />
                                                            <telerik:RadComboBoxItem Text="Health Checkup Summary" Value="HC" />
                                                            <telerik:RadComboBoxItem Text="Medical Summary" Value="MS" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="label2" runat="server" SkinID="label" Text="Status" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <telerik:RadComboBox ID="ddlStatus" runat="server" Width="100%">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Active" Value="1" />
                                                            <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12 form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:Label ID="label6" runat="server" SkinID="label" Text="Page Size" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <telerik:RadComboBox ID="ddlPageSize" runat="server" Width="100%">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="A3" Value="A3" />
                                                            <telerik:RadComboBoxItem Text="A4" Value="A4" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="A5" Value="A5" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-sm-12 form-group">

                                                    <asp:Label ID="lblMarginLeft" runat="server" Text="Margin Left" />
                                                </div>
                                                <div class="col-sm-12 form-group">
                                                    <asp:TextBox ID="txtMarginLeft" runat="server" MaxLength="3" Width="50%" />
                                                    <span id="spMarginLeft" runat="server">pixels</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-sm-12 form-group">
                                                    <asp:Label ID="lblMarginRight" runat="server" Text="Margin Right" />
                                                </div>
                                                <div class="col-sm-12 form-group">
                                                    <asp:TextBox ID="txtMarginRight" runat="server" MaxLength="3" Width="50%" />
                                                    <span id="spMarginRight" runat="server">pixels</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-sm-12 form-group">
                                                    <asp:Label ID="lblMarginTop" runat="server" Text="Margin Top" />
                                                </div>
                                                <div class="col-sm-12 form-group">
                                                    <asp:TextBox ID="txtMarginTop" runat="server" MaxLength="3" Width="50%" />
                                                    <span id="spMarginTop" runat="server">pixels</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-sm-12 form-group">
                                                    <asp:Label ID="lblMarginBottom" runat="server" Text="Margin Bottom" />
                                                </div>
                                                <div class="col-sm-12 form-group">
                                                    <asp:TextBox ID="txtMarginBottom" runat="server" MaxLength="3" Width="50%" />
                                                    <span id="spMarginBottom" runat="server">pixels</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="row">
                                                <div class="col-sm-12 form-group">
                                                    <asp:Label ID="lblReportFooterText" runat="server" Text="Report Footer Text" />
                                                </div>
                                                <div class="col-sm-12 form-group">
                                                    <asp:TextBox ID="txtReportFooterText" runat="server" MaxLength="250" Width="100%" />
                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="row">
                                                <div class="col-sm-12 form-group">
                                                    <asp:Label ID="lblPrintVersionCode" runat="server" Text="Report Print Version Code" />
                                                </div>
                                                <div class="col-sm-12 form-group">
                                                    <asp:TextBox ID="txtPrintVersionCode" runat="server" MaxLength="250" Width="100%" />
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtMarginLeft" ValidChars="0123456789" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtMarginRight" ValidChars="0123456789" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtMarginTop" ValidChars="0123456789" />
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtMarginBottom" ValidChars="0123456789" />
                                </div>


                                <div class="col-sm-6">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkShowPageNoInPageFooter" runat="server" Text="Show Page No In Page Footer" />
                                                </div>

                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkCheckListRequired" runat="server" Enabled="false" Text="Check List Required" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkShowPrintByInPageFooter" runat="server" Text="Show Print By In Page Footer" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkIsShowFilledTemplates" runat="server" Enabled="false" Text="Show Filled Templates Only" />
                                                </div>

                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkShowPrintDateInPageFooter" runat="server" Text="Show Print Date In Page Footer" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkDefaultForOP" runat="server" Text="Default For OP" />
                                                </div>



                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkPrintHospitalHeader" runat="server" Enabled="false" Text="Print Hospital Header" />
                                                </div>
                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkPrintDoctorSignature" runat="server" Enabled="false" Text="Print Doctor Signature" OnCheckedChanged="chkPrintDoctorSignature_CheckedChanged" />
                                                </div>


                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkPrintHeaderImage" runat="server" Text="Print Header Images" />
                                                </div>
                                                <div class="col-sm-12" style="margin-bottom: 8px!important">
                                                    <asp:FileUpload ID="_ImageFileUploadHeader" runat="server" Width="250px" CssClass="button" />
                                                </div>

                                                <div class="col-sm-12">
                                                    <asp:CheckBox ID="chkPrintFooterImage" runat="server" Text="Print Footer Images" />
                                                </div>
                                                <div class="col-sm-12" style="margin-bottom: 8px!important">
                                                    <asp:FileUpload ID="_ImageFileUploadFooter" runat="server" Width="250px" CssClass="button" />
                                                </div>

                                                &nbsp;&nbsp;&nbsp;&nbsp;
                        <%------------------------------------Yogesh------------1/04/2022----------------%>
                                                <asp:Panel ID="Panl1" runat="server" CssClass="Popup" Style="display: none">
                                                    <asp:Button ID="btnCloseHS" CssClass="btn " Style="float: right; margin: -7px 5px 0px 0px; border: 1px solid blue; padding: 0px 3px!important;" runat="server" Text="X" /><br />
                                                    <iframe id="Iframe1" src="HospitalSetupSettingsPopUp.aspx" style="width: 98%; height: 90%;" runat="server"></iframe>


                                                </asp:Panel>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="Popup" Style="display: none">
                                                    <asp:Button ID="Button1" CssClass="btn " Style="float: right; margin: -7px 5px 0px 0px; border: 1px solid blue; padding: 0px 3px!important;" runat="server" Text="X" /><br />
                                                    <iframe id="Iframe2" src="EMRHSetup.aspx" style="width: 98%; height: 90%;" runat="server"></iframe>
                                                    <br />

                                                </asp:Panel>

                                                <%------------------------------------Yogesh------------1/04/2022----------------%>

                                                <div class="col-sm-12 form-group">
                                                    <div class="row">
                                                        <div class="col-sm-5" style="padding-right: 0px;">
                                                            <asp:Label ID="label7" runat="server" SkinID="label" Text="Upload Image Text" />
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <asp:FileUpload ID="FileUpLoad1" runat="server" />
                                                            <asp:RegularExpressionValidator
                                                                ID="FileUpLoadValidator" runat="server"
                                                                ErrorMessage="Upload Jpegs and Gifs only."
                                                                ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.jpg|.JPG|.gif|.GIF)$"
                                                                ControlToValidate="FileUpload1">  
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                    </div>
                                                </div>


                                            </div>
                                            <div class="row">



                                                <%-- Yogesh--%>
                                                <%--yogesh 4/4/22 rollback--%>

                                                <%-- <div class="col-sm-4 form-group">
                                <asp:Label ID="signheight" runat="server" Text="Signature Height" />
                                <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </div>
                            <div class="col-sm-8 form-group">
                                <asp:TextBox ID="signheighttxtbox" runat="server"  MaxLength="3" Width="75%" />
                                <span id="Span1" runat="server">pixels</span>
                            </div>
                            <div class="col-sm-4 form-group">
                                <asp:Label ID="signwidth" runat="server" Text="Signature Width" />
                                <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
                            </div>
                            <div class="col-sm-8 form-group">
                                <asp:TextBox ID="signwidthtxtbox" runat="server"  MaxLength="3" Width="75%" />
                                <span id="Span2" runat="server">pixels</span>
                            </div>--%>
                                                <%-- Yogesh--%>
                                            </div>








                                        </div>
                                    </div>

                                </div>

                            </div>
                        </div>



                    </div>



                </div>
                </div>

                </div>


                </div>
                </div>
               </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdnPrintHeaderImagePath" runat="server" />
        <asp:HiddenField ID="hdnPrintFooterImagePath" runat="server" />
    </form>
</body>
</html>
