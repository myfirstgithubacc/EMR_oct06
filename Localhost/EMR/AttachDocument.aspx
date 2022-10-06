<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" Trace="false"
    AutoEventWireup="true" CodeFile="AttachDocument.aspx.cs" Inherits="EMR_AttachDocument"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="ComboPatientSearch" Src="~/Include/Components/PatientSearchCombo.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <body>

        <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
        <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
        <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" /> 


        <style type="text/css">
            .myClass:hover
            {
                background-color: #a1da29 !important;
            }
            .txt
            {
                border: 0px !important;
                background: #eeeeee !important;
                color: Black !important;
                margin-left: 25%;
                margin-right: auto;
                width: 100%; /* IE's opacity*/ /* filter: alpha(opacity=50); 
            opacity: 0.50; */
                text-align: center;
            }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew.RadWindow.RadWindow_Default.rwNormalWindow.rwTransparentWindow{
                left:14px!important;
            }
        </style>

        <script type="text/javascript">


            if (window.captureEvents) {
                window.captureEvents(Event.KeyUp);
                window.onkeyup = executeCode;
            }
            else if (window.attachEvent) {
                document.attachEvent('onkeyup', executeCode);
            }
            function validateMaxLength() {
                var txt = $get('<%=txtAccountNo.ClientID%>');
                if (txt.value > 9223372036854775807) {
                    alert("Value should not be more than 9223372036854775807.");
                    txt.value = txt.value.substring(0, 12);
                    txt.focus();
                }
            }
            function executeCode(evt) {
                if (evt == null) {
                    evt = window.event;
                }

                var theKey = parseInt(evt.keyCode, 10);

                switch (theKey) {
                    case 114:  // F3
                        $get('<%=btnUpload.ClientID%>').click();
                        break;
                    case 119:  // F8
                        $get('<%=btnNo.ClientID%>').click();
                        break;

                }
                evt.returnValue = false;
                return false;
            }

            function SearchPatientOnClientClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var RegistrationId = arg.RegistrationId;
                    var RegistrationNo = arg.RegistrationNo;
                    $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                    $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                    $get('<%=txtAccountNo.ClientID%>').value = RegistrationNo;
                    $get('<%=txtRegNo.ClientID%>').value = RegistrationNo;
                }
                $get('<%=btnGetInfo.ClientID%>').click();
            }
            function CloseAndRebind() {
                GetRadWindow().close(); // Close the window 
            }
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog 
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well) 

                return oWindow;
            }


            function showingSetAsDesktop(sender, args) {
                //Disable the setAsDesktop menu on the desktop image
                if (args.get_targetElement().id == "qsfexDesktop") {
                    args.set_cancel(true);
                }
            }

            function OnClientSelectedIndexChangedEventHandler(sender, args) {
                var item = args.get_item();
                $get('<%=txtRegNo.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=txt_hdn_PName.ClientID%>').value = item != null ? item.get_text() : sender.value();
                //alert("asdads");
                $get('<%=btnGetInfo.ClientID%>').click();
                //alert("2nd");
            }      
        </script>

        <script language="JavaScript" type="text/javascript">


            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }
        </script>



        <div class="container-fluid header_main">
            <div class="col-md-2 col-sm-3"><h2 id="tdName" runat="server">Patient Documents</h2></div>
            <div class="col-md-3 col-sm-3 text-center" id="tdSer" runat="server">
                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                    
                    <asp:LinkButton ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                        Font-Underline="false" ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click"></asp:LinkButton>
                    <asp:TextBox ID="txtAccountNo" SkinID="textbox" runat="server" Width="86px" TabIndex="0"
                        MaxLength="13" onkeyup="return validateMaxLength();" />
                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                        FilterType="Custom" TargetControlID="txtAccountNo" ValidChars="0123456789" />
                    <asp:TextBox ID="txtRegNo" SkinID="textbox" runat="server" Width="12px" Style="visibility: hidden;"
                        TabIndex="0"></asp:TextBox>
                    <asp:TextBox ID="txt_hdn_PName" Width="10px" Style="visibility: hidden;" SkinID="textbox"
                        runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hdnregno" runat="server" Value="" />
                    <asp:Button ID="btnGetInfo" runat="server" Text="Assign" CausesValidation="false"
                        Enabled="true" SkinID="button" Width="10px" Style="visibility: hidden;" OnClick="btnGetInfo_Click"
                        TabIndex="103" />
                   
                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnRegistrationName" runat="server" Value="0" />
                        
                    <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                </asp:Panel>
            </div>
            <div class="col-md-3 col-sm-3"></div>
            <div class="col-md-4 col-sm-3 text-right"style="padding-right:5em;">
             
                <asp:Button ID="btnUpload" SkinID="Button" runat="server" Text="Upload (Ctrl+F3)" ValidationGroup="save"  Visible="false" />
                <asp:Button ID="btnAddDocument" CssClass="btn btn-primary" runat="server" Text="Add Document" Visible="false" />
                <asp:Button ID="btnNo" runat="server" ToolTip="Close" CssClass="btn btn-default" Text="Close (Ctrl+F8)" CausesValidation="false" Visible="false" OnClick="btnNo_OnClick" />
             
                 <asp:ValidationSummary DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" ValidationGroup="save" ID="ValidationSummary1" runat="server" />
                 &nbsp; &nbsp;
                 <asp:LinkButton ID="lnkScan" runat="server" Text="Scan File"  OnClick="lnkScan_Onclick"></asp:LinkButton>
                 <a href="http://ahm.no-ip.org:8081/TwainScanner/publish.htm">Plug-in</a> &nbsp;
                  <asp:Button ID="btnNew" runat="server" ToolTip="New" CssClass="btn btn-primary" Text="New" OnClick="btnNew_Click" />
                 <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Visible="false" Text="Close" OnClientClick="window.close();" />
                  
                
            </div>
        </div>


        <div class="container-fluid subheading_main">
            <div class="form-group">
                <div class="col-md-12" id="pdetails" runat="server"><asp:Xml ID="xmlPatientInfo" runat="server"></asp:Xml></div>
            </div>

            <div class="row form-group">
                <div class="col-md-12 text-center" style="color: green; font-size: 12px; font-weight: bold;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            
            <div class="row" id="reg" runat="server" visible="false">
                <div class="col-md-12" id="tddetails" runat="server" visible="false">
                  
                    <asp:Label ID="LblLabelname" runat="server" Text="Attach Documents" Font-Bold="true" />
                </div>
            </div>

            <div class="row"><div class="col-md-12 text-center" style="left: 0px; top: 0px"><asp:Literal ID="ltrlMessage" runat="server" Mode="Transform"></asp:Literal></div></div>
        </div>

        
        <div class="container-fluid">
            <div class="row">
                   <asp:Panel ID="pnlImages" runat="server" ScrollBars="None" BorderStyle="Solid">
                        <iframe height="515px" width="98%" scrolling="auto" runat="server" id="iFrame1" />
                        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" EnableViewState="false"
                            Width="550px" Height="450px">
                            <Windows>
                                <telerik:RadWindow runat="server" ID="Details" NavigateUrl="DisplayImage.aspx" Behaviors="Close,Move"
                                    Modal="true">
                                </telerik:RadWindow>
                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </asp:Panel>
            </div>
        </div>


    </body>
</asp:Content>
