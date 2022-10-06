<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucCaseSheet.ascx.cs" Inherits="Include_Components_MasterComponent_ucCaseSheet" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNewEMR" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelRegAttachDocument.ascx" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server">
    <ContentTemplate>
        <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)">
        <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)">
        <title></title>
        <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
        <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
        <link href="../Include/css/mainStyle.css" rel="stylesheet" />
        <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
        <link href="../Include/css/emr_new.css" rel="stylesheet" />
        <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
        <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />

        <style>
          

            input#ctl00_ContentPlaceHolder1_VisitHostory_rdoViewType_1 {
                margin-top: 3px !important;
            }

            input#ctl00_ContentPlaceHolder1_VisitHostory_rdoViewType_0 {
                margin-top: 3px !important;
            }

            span#ctl00_ContentPlaceHolder1_VisitHostory_Label1 {
                margin-top: 4px;
            }

            table#ctl00_ContentPlaceHolder1_VisitHostory_rdoTreeExpandCollapse {
                width: 100%;
            }

            div#ctl00_ContentPlaceHolder1_VisitHostory_RTF1 {
                height: 41.7vw !important;
            }

            div#ctl00_ContentPlaceHolder1_VisitHostory_pnlTree {
                height: 90vh !important;
                 overflow: auto !important;
            }
            td.rcbInputCell.rcbInputCellLeft .rcbInput{
                height:24px!important;
            }
            span{
                font-size:13px!important;
            }
        </style>

        <div id="dis" runat="server" style="overflow-x:hidden;">

            <div class="VisitHistoryDiv hidden">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-11">
                            <div class="WordProcessorDivText">
                                <h2>Past Clinical Notes</h2>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnClose" runat="server" CssClass="SearchKeyBtn01a" Text="Close" OnClientClick="window.close();" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="VisitHistoryBorderNew" id="tblPatientDetails">
                <div class="container-fluid">
                    <div class="row">
                        <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                        <asplNewEMR:UserDetailsHeader ID="asplHeaderUDEMR" runat="server" />
                        <div class="col-md-12"></div>
                    </div>
                    <div class="row header_main">
                        <div class="col-6 text-right">
                             <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text=""></asp:Label>
                        </div>
                        <div class="col-6 text-right">
                            <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-primary" Text="Refresh" OnClick="btnRefresh_OnClick" />
                            <asp:Button ID="btnPrintReport" runat="server" CssClass="btn btn-primary" Text="Print" OnClick="btnPrintReport_OnClick" />
                            <asp:LinkButton ID="lnkSendToPoliceStation" runat="server" Text="Send To Police Station"
                                Font-Underline="false" Font-Size="Larger" OnClick="lnkSendToPoliceStation_Click" Visible="false" />




                        </div>
                    </div>
                </div>
            </div>

            <div class="VitalHistory-Div">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-2" style="padding-right: 0;">
                            <div class="row">
                                <div class="col-md-12 col-12">
                                    <div class="row ">
                                        <div class="col-md-6 col-3 box-col-checkbox" style="padding-right: 0px;">
                                            <asp:RadioButtonList ID="rdoTreeExpandCollapse" CssClass="PCNViewText01" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoTreeExpandCollapse_OnSelectedIndexChanged">
                                                <asp:ListItem Text="-" Value="E" />
                                                <asp:ListItem Text="+" Value="C" Selected="True" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="col-md-6 col-3 PCN-EMRDiv">
                                            <asp:ImageButton ID="imgPharmacy" runat="server" ToolTip="Prescription" Width="20px" Height="20px" OnClick="imgPharmacy_Onclick" ImageUrl="~/Icons/pillred.png" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="imgVital" runat="server" ToolTip="Vitals" Width="20px" Height="20px" OnClick="imgVital_Onclick" ImageUrl="~/Icons/Vital.jpg" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="imgDiagnosis" runat="server" ToolTip="Diagnosis" Width="20px" Height="20px" OnClick="imgDiagnosis_Onclick" ImageUrl="~/Icons/Diagnosis.jpg" />&nbsp;&nbsp; &nbsp;&nbsp;
                                        </div>
                                        <div class="col-md-12 PCN-EMRDiv" style="display: none;">
                                            <asp:Label ID="Label9" runat="server" CssClass="PCNViewText" Text="Doctor Name :" Visible="false" />
                                            <asp:Label ID="lblDoctorName" runat="server" CssClass="PCNViewText" Visible="false" />
                                        </div>
                                        <div class="col-md-12 PCN-EMRDiv" style="display: none;">
                                            <asp:Label ID="Label10" runat="server" Text="Visit Date :" CssClass="PCNViewText" Visible="false" />
                                            <asp:Label ID="lblVisitDate" runat="server" CssClass="PCNViewText" Visible="false" />
                                        </div>
                                        <div class="col-md-12 PCN-EMRDiv" style="display: none;">
                                            <asp:Label ID="Label11" runat="server" Text="Visit Type :" CssClass="PCNViewText" Visible="false" />
                                            <asp:Label ID="lblVisitType" runat="server" CssClass="PCNViewText" Visible="false" />
                                        </div>
                                       


                                        <div class="col-md-12 col-6 box-col-checkbox ml-md-4 m-0" style="padding-right: 0; display: inline-table;">
                                            <asp:Label ID="Label1" CssClass="PCNViewText" runat="server" Text="View" />
                                            <span class="PCNText">
                                                <asp:RadioButtonList ID="rdoViewType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoViewType_OnSelectedIndexChanged">
                                                    <asp:ListItem Text="Visit" Value="V" Selected="True" />
                                                    <asp:ListItem Text="Template" Value="T" />
                                                </asp:RadioButtonList>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-md-12 col-4">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Label ID="Label111" CssClass="VitalText" runat="server" Text='<%$Resources:PRegistration,Doctor %>'></asp:Label>
                                                </div>
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <telerik:RadComboBox ID="ddlDoctor" runat="server" Width="100%" DropDownWidth="250" AllowCustomText="true" Filter="Contains" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-4">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Label ID="Label1211" runat="server" Text="Template"></asp:Label>
                                                </div>
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <telerik:RadComboBox ID="ddlTemplate" SkinID="DropDown" runat="server" Width="100%" DropDownWidth="280" AllowCustomText="true" Filter="Contains" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-4">
                                            <div class="row">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <asp:Label ID="Label1311" runat="server" Text="Date" />
                                                    <asp:HiddenField ID="hdnRegid" runat="server" />
                                                    <asp:HiddenField ID="hdnRegNo" runat="server" />
                                                    <asp:HiddenField ID="hdnEncId" runat="server" />
                                                    <asp:HiddenField ID="hdnEncNo" runat="server" />
                                                    <asp:HiddenField ID="hdnFromWard" runat="server" />
                                                    <asp:HiddenField ID="hdnOP_IP" runat="server" />
                                                    <asp:HiddenField ID="hdnCategory" runat="server" />
                                                    <asp:HiddenField ID="hdnCloseButtonShow" runat="server" />
                                                    <asp:HiddenField ID="hdnFromEMR" runat="server" />
                                                </div>
                                                <div class="col-md-12 col-sm-12 col-xs-12">

                                                    <telerik:RadComboBox ID="ddlDateRange" SkinID="DropDown" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlDateRange_OnSelectedIndexChanged">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="Select All" Value="" />
                                                            <telerik:RadComboBoxItem Text="Last Three Month(s)" Value="LTM" />
                                                            <telerik:RadComboBoxItem Text="Last Six Month(s)" Value="LSM" />
                                                            <telerik:RadComboBoxItem Text="Last One Year" Value="LOY" />
                                                            <telerik:RadComboBoxItem Text="Last Three Year(s)" Value="LTY" Selected="true" />
                                                            <telerik:RadComboBoxItem Text="Date Range" Value="DR" />
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="col-md-12 col-xs-12 form-group"  id="tblDateRange"  runat="server" visible="false">
                                            <div class="row">
                                                <div class="col-md-12 col-6">
                                                    <div class="row">
                                                        <div class="col-md-12 col-4">
                                                             <span class="ImmunizationDD-Div">From</span>
                                                        </div>
                                                         <div class="col-md-12 col-8">
                                                             <telerik:RadDatePicker ID="dtpFromDate" Width="100%" runat="server" />
                                                         </div>

                                                    </div>
                                                </div>
                                                 <div class="col-md-12 col-6">
                                                     <div class="row">
                                                          <div class="col-md-12 col-3">
                                                              <span>To</span>
                                                          </div>
                                                          <div class="col-md-12 col-8">
                                                              <telerik:RadDatePicker ID="dtpToDate" Width="100%" runat="server" />
                                                          </div>
                                                     </div>
                                                 </div>
                                            </div>
                                           

                                        </div>

                                    </div>

                                </div>

                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlTree" runat="server" BorderColor="SkyBlue" BorderWidth="1px" Height="460px" ScrollBars="Both">
                                                            <asp:TreeView ID="tvCategory" runat="server" ImageSet="Msdn" Font-Size="8pt" NodeIndent="10" OnSelectedNodeChanged="tvCategory_SelectedNodeChanged" Width="100%">
                                                                <ParentNodeStyle Font-Bold="False" />
                                                                <HoverNodeStyle Font-Underline="True" BackColor="#CCCCCC" BorderColor="#888888" BorderStyle="Solid" BorderWidth="0px" />
                                                                <SelectedNodeStyle BackColor="gray" ForeColor="White" Font-Underline="False" HorizontalPadding="0px" VerticalPadding="0px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" NodeSpacing="1px" HorizontalPadding="0px" VerticalPadding="0px" />
                                                            </asp:TreeView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                            </div>
                        </div>
                        <div class="col-sm-7">
                            <div class="row">


                                <div class="col-md-12 col-sm-12 col-xs-12" style="padding: 0px;">
                                    <span class="ImmunizationDD-Div">
                                        <telerik:RadEditor ID="RTF1" runat="server" CssClass="PCNDiv" EnableTextareaMode="false" Width="100%" Height="460px" Skin="Office2007"
                                            AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage" ToolsFile="~/Include/XML/BlankXML.xml"
                                            OnClientLoad="OnClientLoad" Style="min-width: inherit !important; height: 10vh !important;">
                                            <CssFiles>
                                                <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                            </CssFiles>
                                            <SpellCheckSettings AllowAddCustom="true" />
                                            <ImageManager ViewPaths="~/medical_illustration" />
                                        </telerik:RadEditor>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>






                </div>
            </div>
            <asp:HiddenField ID="hdnDoctorImage" runat="server" />
            <asp:HiddenField ID="hdnReportContent" runat="server" />
            <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </div>

        <div style="width: 50px">
            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close" />
                </Windows>
            </telerik:RadWindowManager>
        </div>
        <script type="text/javascript">
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
                //Added by Ujjwal 28April2015 to give access of copy for casesheet end 
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
