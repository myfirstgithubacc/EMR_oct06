<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucCaseSheetNew.ascx.cs"
    Inherits="Include_Components_MasterComponent_ucCaseSheetNew" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <%: Styles.Render("~/bundles/EMRMasterWithTopDetailsCss") %>
        <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../../css/mainNew.css" rel="Stylesheet" type="text/css" />
        <link href="../../EMRStyle.css" rel="Stylesheet" type="text/css" />
        <style type="text/css">
            span#ctl00_ContentPlaceHolder1_VisitHostory_Label1 {
                padding-left: 4px;
            }

            input#ctl00_ContentPlaceHolder1_VisitHostory_chk_DoctorTemplet {
                margin-top: 2px !important;
            }
          
        </style>
        <div id="dis" runat="server" style="vertical-align: top; height: 100%">
            <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>

                    <div class="container-fluid">
                        <div class="row header_main">

                            <div class="col-md-7 col-sm-12" id="tblDateRange" runat="server">
                                <div class="row">
                                    <div class="col-md-3 col-sm-3 col-4">
                                        <div class="row">
                                            <div class="col-md-2 col-sm-3 col-3">
                                                <asp:Label ID="Label1311" runat="server" Text="Date" SkinID="label" />
                                            </div>
                                            <div class="col-md-10 col-sm-9 col-9">
                                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-4">
                                        <div class="row">
                                            <div class="col-md-2 col-sm-3 col-3">
                                                <asp:Label ID="Label2" runat="server" Text="To&nbsp;" SkinID="label" />
                                            </div>
                                            <div class="col-md-10 col-sm-9 col-9">
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" />
                                            </div>
                                        </div>
                                    </div>
                                    <%--Add By HImashu On date 22/03/2022--%>
                                    <div class="col-md-3 col-sm-3 col-4">

                                        <asp:Label ID="Label1" runat="server" Text="Doctor Templets" SkinID="label" />
                                        <asp:CheckBox ID="chk_DoctorTemplet" runat="server" AutoPostBack="true" OnCheckedChanged="chk_DoctorTemplet_CheckedChanged" />
                                    </div>

                                    <%--End--%>
                                    <div class="col-md-3 col-sm-3 col-6">
                                    </div>
                                    <div class="col-md-2 col-sm-3 col-6">
                                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close" InitialBehaviors="Maximize" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-5 col-sm-12 col-xs-6 text-right mt-2 mt-md-0">
                                <asp:LinkButton ID="lnkBtnViewCaseSheet" runat="server" Text="View Full Case Sheet" ForeColor="Navy"
                                    Font-Underline="false" Font-Size="10pt" Font-Bold="true" OnClick="lnkBtnViewCaseSheet_OnClick" />
                                <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" Text="Refresh"
                                    OnClick="btnRefresh_OnClick" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />
                                <asp:Button ID="btnViewSelectedTemplate" runat="server" CssClass="btn btn-primary"
                                    Text="View Selected Template" OnClick="btnViewSelectedTemplate_OnClick" OnClientClick="ClientSideClick(this)"
                                    UseSubmitBehavior="false" />
                                <asp:Button ID="ibtnClose" runat="server" Visible="false" CssClass="SearchKeyBtnRight btn btn-primary" Text="Close" ToolTip="Close" OnClientClick="window.close();" />
                                <asp:Button ID="btnPrintPdf" runat="server" CssClass="SearchKeyBtnRight btn btn-primary" Text="Print"
                                    OnClick="btnPrintPDF_Click" OnClientClick="ClientSideClick(this)" UseSubmitBehavior="false" />
                            </div>
                        </div>

                    <div class="row" id="tblPatientDetails">
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    </div>
                        <div class="row text-center">
                            <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text="" />
                        </div>

                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-sm-12 col-6">
                                   
                                    <div class="row gridview">
                                        <div id="divGrid" style="overflow: auto; height: 250px; border-color: skyblue; border-width: 1px;">
                                                <asp:GridView ID="gvDetails" runat="server" Width="100%" AutoGenerateColumns="false"
                                                    OnRowCommand="gvDetails_RowCommand" ShowHeader="false">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkRow" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Template(s)">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkVisitDetail" runat="server" Font-Size="Larger" CommandName="VisitDetail"
                                                                    CommandArgument='<%#Eval("TemplateId") %>' Text='<%#Eval("TemplateName") %>'
                                                                    ToolTip="Click here to view details" />
                                                                <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId") %>' />
                                                                <asp:HiddenField ID="hdnTemplateType" runat="server" Value='<%#Eval("TemplateType") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                    </div>
                                            </div>
                                         <div class="col-sm-12 col-6">
                                    <div class="row m-t gridview">
                                        <div id="divLabResult" style="overflow: auto; height: 160px; border-color: skyblue; border-width: 1px;">
                                            <asp:GridView ID="gvAttachments" runat="server" SkinID="gridview2" Width="100%" AutoGenerateColumns="false"
                                                OnRowCommand="gvAttachments_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="EMR / Lab Attachment(s)" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkServiceName" runat="server" Font-Size="Larger" CommandName="VisitDetail"
                                                                CommandArgument='<%#Eval("DocumentId") %>' Text='<%#Eval("ServiceName") %>' ToolTip="Click here to view external center lab attachment(s)" />
                                                            <asp:HiddenField ID="hdnAttachmentOption" runat="server" Value='<%#Eval("AttachmentOption") %>' />
                                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                                            <asp:HiddenField ID="hdnDiagSampleId" runat="server" Value='<%#Eval("DiagSampleId") %>' />
                                                            <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                                            <asp:HiddenField ID="hdnDocumentName" runat="server" Value='<%#Eval("DocumentName") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                             </div>
                                         </div>
                                </div>
                            </div>

                            <div class="col-md-8 col-sm-8 col-xs-12 m-t">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row gridview">
                                        <telerik:RadEditor ID="RTF1" runat="server" EnableTextareaMode="false" Height="510px"
                                    Skin="Office2007" AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage"
                                    Width="99%" ToolsFile="~/Include/XML/BlankXML.xml" OnClientLoad="OnClientLoad">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                    </CssFiles>
                                    <SpellCheckSettings AllowAddCustom="true" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>
                                    </div>
                                    </div>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:HiddenField ID="hdnDoctorImage" runat="server" />
                            </div>
                        </div>
                    </div>
                  
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="updtProgress" DisplayAfter="2000" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                DynamicLayout="true">
                <ProgressTemplate>
                    <div style="position: absolute; background-color: #FAFAFA; z-index: 2147483647 !important; opacity: 0.8; overflow: hidden; text-align: center; top: 0; left: 0; height: 100%; width: 100%; padding-top: 20%;">
                        <img alt="progress" src="../../../Images/ajax-loader3.gif">
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
              <script language="javascript" type="text/javascript">
            // This Script is used to maintain Grid Scroll on Partial Postback
            var scrollTop;
            //Register Begin Request and End Request 
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            //Get The Div Scroll Position
            function BeginRequestHandler(sender, args) {
                var m = document.getElementById('divGrid');
                scrollTop = m.scrollTop;
            }
            //Set The Div Scroll Position
            function EndRequestHandler(sender, args) {
                var m = document.getElementById('divGrid');
                m.scrollTop = scrollTop;
            }

            function ClientSideClick(myButton) {

                // Client side validation
                if (typeof (Page_ClientValidate) == 'function') {
                    if (Page_ClientValidate() == false) {
                        return false;
                    }
                }

                //make sure the button is not of type "submit" but "button"
                if (myButton.getAttribute('type') == 'button') {
                    // disable the button
                    myButton.disabled = true;
                    myButton.className = "btn-inactive";
                    myButton.value = "Processing...";
                }

                return true;
            }

            function OnClientLoad(sender, args) {
                if (document.getElementById('<%= hdnIsCopyCaseSheetAuthorized.ClientID %>').value == "False") {
                    $telerik.addExternalHandler(sender.get_contentArea(), "copy", function myfunction(ev) {
                        alert("This content cannot be copied!");
                        $telerik.cancelRawEvent(ev);
                    });

                    // Disable copying from HTML mode
                    $telerik.addExternalHandler(sender.get_textArea(), "copy", function myfunction(ev) {
                        alert("This content cannot be copied!");
                        $telerik.cancelRawEvent(ev);
                    });
                }

                var mode = sender.get_mode();

                switch (mode) {
                    case 4:
                        setTimeout(function () {
                            var ImageEditor = sender.getToolByName("ImageEditor");
                            var MedicalIllustration = sender.getToolByName("MedicalIllustration");
                            var ExportToRtf = sender.getToolByName("ExportToRtf");

                            ImageEditor.setState(0);
                            MedicalIllustration.setState(0);
                            ExportToRtf.setState(0);
                        }, 0);
                        break;
                }
            }
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
