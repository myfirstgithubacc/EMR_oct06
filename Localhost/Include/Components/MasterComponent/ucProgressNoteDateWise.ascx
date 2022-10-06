<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucProgressNoteDateWise.ascx.cs" Inherits="Include_Components_MasterComponent_ucCaseSheet" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNewEMR" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelRegAttachDocument.ascx" %>

<asp:UpdatePanel ID="UpdatePanel" runat="server">


    <ContentTemplate>

        <meta http-equiv="Page-Enter" content="blendTrans(Duration=0.2)">
        <meta http-equiv="Page-Exit" content="blendTrans(Duration=0.2)">
        <title></title>
        <%--<link rel="shortcut icon" type="image/ico" href="" />--%>
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
        
        <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />


        <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
        <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
        <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
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

        <div id="dis" runat="server">

            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-7">
                            <div class="WordProcessorDivText">
                                <h2>Past Clinical Notes</h2>
                            </div>
                        </div>
                        <div class="col-md-5 text-right">
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
                </div>
            </div>

            <div class="VitalHistory-Div">
                <div class="container-fluid">

                    <div class="row">

                        <div class="col-md-3">
                            <div class="pastClinicalRadio">
                                <asp:RadioButtonList ID="rdoTreeExpandCollapse" CssClass="PCNViewText01" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoTreeExpandCollapse_OnSelectedIndexChanged">
                                    <asp:ListItem Text="-" Value="E" />
                                    <asp:ListItem Text="+" Value="C" Selected="True" />
                                </asp:RadioButtonList>

                                <asp:Label ID="Label1" CssClass="PCNViewText" runat="server" Text="View" />
                                <span class="PCNText">
                                    <asp:RadioButtonList ID="rdoViewType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoViewType_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Visit" Value="V" Selected="True" />
                                        <asp:ListItem Text="Template" Value="T" />
                                    </asp:RadioButtonList>
                                </span>
                            </div>
                        </div>

                        <div class="col-md-9">
                            <div class="row">
                                <div class="col-md-3 PCN-EMRDiv">
                                    <span class="ImmunizationDD-Div">
                                        <asp:Label ID="Label111" CssClass="VitalText" runat="server" Text='<%$Resources:PRegistration,Doctor %>'></asp:Label>
                                        <telerik:RadComboBox ID="ddlDoctor" runat="server" Width="170px" DropDownWidth="250" AllowCustomText="true" Filter="Contains" />
                                    </span>
                                </div>

                                <div class="col-md-3 PCN-EMRDiv">
                                    <span class="ImmunizationDD-Div">
                                        <asp:Label ID="Label1211" runat="server" Text="Template"></asp:Label>
                                        <telerik:RadComboBox ID="ddlTemplate" SkinID="DropDown" runat="server" Width="140px" DropDownWidth="280" AllowCustomText="true" Filter="Contains" />
                                    </span>
                                </div>

                                <div class="col-md-2 PCN-EMRDiv">
                                    <span class="ImmunizationDD-Div">
                                        <asp:Label ID="Label1311" runat="server" Text="Date"></asp:Label>
                                        <asp:HiddenField ID="hdnRegid" runat="server" />
                                        <asp:HiddenField ID="hdnRegNo" runat="server" />
                                        <asp:HiddenField ID="hdnEncId" runat="server" />
                                        <asp:HiddenField ID="hdnEncNo" runat="server" />
                                        <asp:HiddenField ID="hdnFromWard" runat="server" />
                                        <asp:HiddenField ID="hdnOP_IP" runat="server" />
                                        <asp:HiddenField ID="hdnCategory" runat="server" />
                                        <asp:HiddenField ID="hdnCloseButtonShow" runat="server" />
                                        <asp:HiddenField ID="hdnFromEMR" runat="server" />

                                        <telerik:RadComboBox ID="ddlDateRange" SkinID="DropDown" runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlDateRange_OnSelectedIndexChanged">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Select All" Value="" />
                                                <telerik:RadComboBoxItem Text="Last Three Months" Value="LTM" />
                                                <telerik:RadComboBoxItem Text="Last Six Months" Value="LSM" />
                                                <telerik:RadComboBoxItem Text="Last One Year" Value="LOY" />
                                                <telerik:RadComboBoxItem Text="Date Range" Value="DR" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </span>
                                </div>


                                <div class="col-md-2 PCN-EMRDiv">
                                    <span class="ImmunizationDD-Div" id="tblDateRange" runat="server" visible="false">
                                        <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="90px" />
                                        &nbsp;To&nbsp;
                                        <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="90px" />
                                    </span>

                                </div>

                                <div class="col-md-2">
                                    <asp:Button ID="btnRefresh" runat="server" CssClass="SearchKeyBtn02" Text="Refresh" OnClick="btnRefresh_OnClick" />
                                    <asp:Button ID="btnPrintReport" runat="server" CssClass="SearchKeyBtn02" Text="Print" OnClick="btnPrintReport_OnClick" />
                                </div>

                            </div>

                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-3"></div>
                        <div class="col-md-9">
                            <div class="row">

                                <div class="col-md-2 PCN-EMRDiv">
                                    <asp:ImageButton ID="imgPharmacy" runat="server" ToolTip="Prescription" Width="20px" Height="20px" OnClick="imgPharmacy_Onclick" ImageUrl="~/Icons/pillred.png" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="imgVital" runat="server" ToolTip="Vitals" Width="20px" Height="20px" OnClick="imgVital_Onclick" ImageUrl="~/Icons/Vital.jpg" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="imgDiagnosis" runat="server" ToolTip="Diagnosis" Width="20px" Height="20px" OnClick="imgDiagnosis_Onclick" ImageUrl="~/Icons/Diagnosis.jpg" />&nbsp;&nbsp; &nbsp;&nbsp;
                                </div>
                                <div class="col-md-3 PCN-EMRDiv">
                                    <asp:Label ID="Label9" runat="server" CssClass="PCNViewText" Text="Doctor Name :" Visible="false" />
                                    <asp:Label ID="lblDoctorName" runat="server" CssClass="PCNViewText" Visible="false" />
                                </div>
                                <div class="col-md-3 PCN-EMRDiv">
                                    <asp:Label ID="Label10" runat="server" Text="Visit Date :" CssClass="PCNViewText" Visible="false" />
                                    <asp:Label ID="lblVisitDate" runat="server" CssClass="PCNViewText" Visible="false" />
                                </div>
                                <div class="col-md-3 PCN-EMRDiv">
                                    <asp:Label ID="Label11" runat="server" Text="Visit Type :" CssClass="PCNViewText" Visible="false" />
                                    <asp:Label ID="lblVisitType" runat="server" CssClass="PCNViewText" Visible="false" />
                                </div>
                                <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text=""></asp:Label>
                            </div>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-3 PCN-EMRDivLeft">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTree" runat="server" BorderColor="SkyBlue" BorderWidth="1px" Height="460px" ScrollBars="Both">
                                                    <asp:TreeView ID="tvCategory" runat="server" ImageSet="Msdn" Font-Size="8pt" NodeIndent="10" OnSelectedNodeChanged="tvCategory_SelectedNodeChanged" Width="300px">
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

                        <div class="col-md-9 PCN-EMRDivRight">
                            <span class="ImmunizationDD-Div">
                                <telerik:RadEditor ID="RTF1" runat="server" CssClass="PCNDiv" EnableTextareaMode="false" Height="460px" Skin="Office2007"
                                    AutoResizeHeight="false" StripFormattingOptions="NoneSupressCleanMessage" ToolsFile="~/Include/XML/BlankXML.xml"
                                    OnClientLoad="OnClientLoad">
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
            <asp:HiddenField ID="hdnDoctorImage" runat="server" />
            <asp:HiddenField ID="hdnReportContent" runat="server" />
            <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                <Windows>
                    <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
