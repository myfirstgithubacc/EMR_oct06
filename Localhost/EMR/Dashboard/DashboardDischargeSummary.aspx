<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DashboardDischargeSummary.aspx.cs" Inherits="EMR_Dashboard_DashboardDischargeSummary" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <%--<link href="library/styles/speech-input-sdk.css" rel="stylesheet" />--%>

    <style type="text/css">
        #RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow2 {
            position: absolute !important;
        }

        .EMR-HealthCheck h2 {
            width: 126px !important;
        }

        .orderText {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }

        .VisitHistoryDivText div#ctl00_ContentPlaceHolder1_RTF1 {
            height: auto !important;
            min-height: inherit !important;
            overflow: auto;
        }

        .back_bg {
            bottom: 0 !important;
        }

        div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow2 {
            height: auto !important;
        }
    </style>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

       <div class="WordProcessorDiv">
                <div class="container-fluid">
                    <div class="row">
                           <div class="col-md-2">
                            <div class="LabbgTopText-Message01">
                                <h2>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMessage" ForeColor="Green" runat="server" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </h2>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-2 text-right">
                           
                             <asp:Button ID="btnPrintPdf" runat="server" CssClass="btn btn-primary" Text="Print (Ctrl+F9)" OnClick="btnPrintPdf_Click" />
                            </div>
                   </div>
               </div>
      </div>
      
     <div class="VisitHistoryDivText">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblTemp" runat="server" />
                                <%--    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                      <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                               </ContentTemplate>
                            </asp:UpdatePanel>--%>

                                
                                <telerik:RadEditor runat="server" ID="RTF1" EnableTextareaMode="false" Skin="Office2007" disable="true" Width="100%" AutoResizeHeight="false" ToolsFile="~/Include/XML/DischargeSummary.xml">
                                    <CssFiles>
                                        <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                    </CssFiles>
                                    <SpellCheckSettings DictionaryPath="~/App_Data/RadSpell" AjaxUrl="~/Telerik.Web.UI.SpellCheckHandler.axd" />
                                    <ImageManager ViewPaths="~/medical_illustration" />
                                </telerik:RadEditor>
                                 

                            </div>
                        </div>
                    </div>
                </div>
    
                    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableViewState="false" Style="height: 100vh !important;">
                        <Windows>
                                 <telerik:RadWindow ID="RadWindow2" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                         <%--   <telerik:RadWindow ID="RadWindow2" CssClass="HealthCheckupPage" OpenerElementID="btnPrintPdf" runat="server" Style="height: 100vh !important;" />--%>
                        </Windows>
                    </telerik:RadWindowManager>

              </ContentTemplate>
    </asp:UpdatePanel>
              
</asp:Content>

