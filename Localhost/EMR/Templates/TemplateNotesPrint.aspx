<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="TemplateNotesPrint.aspx.cs" Inherits="EMR_Templates_TemplateNotesPrint" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanel.ascx" %>
<%@ Register TagPrefix="aspNewControls" Namespace="NewControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <%--<link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css' />--%>
    <%--<link href="../../Include/css/emr.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css' />--%>

    <script src="../../Scripts/wgssSigCaptX.js" type="text/javascript"></script>
    <script src="../../Scripts/base64.js" type="text/javascript"></script>

    <style type="text/css">
        input#ctl00_ContentPlaceHolder1_txtAllTemplateSearch { background: #fff; padding: 2px;}
         div#ctl00_ContentPlaceHolder1_xsign { border: 1px solid #ccc !important; top: 88%; height: auto !important;}
         iframe#ctl00_ContentPlaceHolder1_ifrmx { border: 0;}

         canvas#colors_sketch { width: 100%;}


         input#ctl00_ContentPlaceHolder1_btnClose1 {
    font-size: 0;
    opacity: 0;
    position: absolute;
    right: 0;
    top: 0;
    height: 24px;
}

         div#ctl00_ContentPlaceHolder1_xsign .form-group { margin-top: 10px;}

div#ctl00_ContentPlaceHolder1_xsign .form-group:after {
    content: 'X';
    position: absolute;
    right: 0;
    z-index: 0;
    background: #337ab7;
    font-weight: bold;
    font-family: arial;
    width: 26px;
    text-align: center;
    color: #fff;
    height: 25px;
    top: 0;
    line-height: 26px;
    z-index: -1;
}

 .TelerikModalOverlay { width: 100% !important;}

 div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow2 { width: 98% !important;top:0 !important }
 div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindow2 table.rwTitlebarControls em { width: auto !important;}

 #ctl00_ContentPlaceHolder1_lblMessage { margin: 0;}
    </style>

    <script type="text/javascript">
        window.onbeforeunload = function (evt) {
            var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            if (IsUnsave == 1) {
                return false;
            }
        }
       var newWindow;
       function pagepopup() {
            var hdnpage = $get('<%=hdnpage.ClientID%>').value;
            newWindow = window.open(hdnpage, 'Report', 'positon= absolute, left=80, margin= auto,height=540,width=1224,top=100,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no, status=yes');

       }
       
         function OnClientClose(oWnd, args) {
             
             $get('<%=btnEnableControl.ClientID%>').click();
           
         }
        function OnClientCloseTemplate(oWnd, args) {
              
            $get('<%=btnTemplatetablebind.ClientID%>').click();
         }
         function SearchPatientOnClientClose(oWnd, args) {
                       var arg = args.get_argument();
                        if (arg) {
                            var RegistrationId = arg.RegistrationId;
                            var RegistrationNo = arg.RegistrationNo;
                            $get('<%=hdnRegistrationId.ClientID%>').value = RegistrationId;
                            $get('<%=hdnRegistrationNo.ClientID%>').value = RegistrationNo;
                             $get('<%=txtAccountNo.ClientID%>').value = RegistrationNo;
                            
                        }
                        $get('<%=btnGetInfo.ClientID%>').click();
                    }
    </script>
    
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-4 col-sm-4 col-xs-12" id="tdHeader" runat="server">
                        <asp:HiddenField ID="hdnpage" runat="server" />
                        <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Consent Forms" Font-Bold="true" />
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-12"></div>
                    <div class="col-md-4 col-sm-4 col-xs-12 text-right">
                        <asp:Button ID="btnClose" Visible="false" runat="server" Text="Close" CssClass="btn btn-pr"
                            OnClientClick="window.close();" />
                        <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 bg-info m-t text-center">
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3 col-sm-3">
                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                        <div class="row p-t-b-5">
                            <div class="col-md-2 col-sm-2 no-p-r">
                                
                  <asp:Button ID="btnGetInfo" runat="server" Enabled="true" OnClick="btnGetInfo_Click"
                                        SkinID="button" Style="visibility: hidden; display: none" Text="Assign" Width="10px" />
                           <asp:label ID="lbtnSearchPatient" runat="server" Text='<%$ Resources:PRegistration, Regno%>'
                                        Font-Underline="false" ToolTip="Click to search patient"></asp:label>
                            </div>
                            <div class="col-md-8 col-sm-9 no-p-l">
                                 <asp:TextBox ID="txtRegNo" SkinID="textbox" runat="server" Width="10px" Style="visibility: hidden;display:none;"></asp:TextBox>
                                 <asp:TextBox ID="txtAccountNo" SkinID="textbox" runat="server" placeholder="Search UHID" autocomplete="off" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-2 no-p-l text-right">
                                <asp:LinkButton ID="lnk_PendingQue" runat="server" Text='Search UHID' CssClass="btn btn-xs btn-primary"
                                        Font-Underline="false" ToolTip="Click to search patient" OnClick="lnk_PendingQue_Click"></asp:LinkButton>
                            </div>
                        </div>
                          </asp:Panel>
                    </div>
                    <%--//yogesh--%>
                    <div class="col-md-9 col-sm-9 col-xs-8 text-right">
                        <asp:Button ID="btnUpload" runat="server" Text="Upload"  CssClass="btn btn-xs btn-primary" onclick="btnUpload_Click" />
                    </div>

                </div>

                <div class="row">
                    <div class="col-md-5 col-sm-5" id="Table1" runat="server">
                        <div class="row">
                            <div class="col-md-12">
                            <div class="col-md-3 p-t-b-5" style="border-top:1px solid #ccc;border-left:1px solid #ccc;display:none">
                                <asp:Label ID="Label1" runat="server" SkinID="label" Text="All Template(s)" />
                                    <telerik:RadComboBox ID="ddlTempGroup" Visible="false" Width="250px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTempGroup_OnSelectedIndexChanged" runat="server">
                                    </telerik:RadComboBox>
                            </div>
                            <div class="col-md-10 col-sm-10 p-t-b-5 no-p-r" style="border-top:1px solid #ccc;border-left:1px solid #ccc;">
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAllTemplateSearch">
                                        <asp:TextBox ID="txtAllTemplateSearch" runat="server" placeholder="Search Consent Form" SkinID="textbox" MaxLength="50"
                                         Style="padding:3px 10px;width:100%;" />
                                    </asp:Panel>
                            </div>
                            <div class="col-md-2 col-sm-2 p-t-b-5 text-right" style="border-top:1px solid #ccc;border-right:1px solid #ccc;">
                                <asp:Button ID="btnAllTemplateSearch" runat="server" Text="Search"
                                        OnClick="btnAllTemplateSearch_OnClick" CssClass="btn btn-xs btn-primary"  />
                            </div>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%" ScrollBars="Auto" style="min-height: 380px;">
                                        <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                        <asp:GridView ID="gvAllTemplate" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="True" PageSize="15" OnRowDataBound="gvAllTemplate_OnRowDataBound"
                                            OnRowCommand="gvAllTemplate_RowCommand" OnPageIndexChanging="gvAllTemplate_PageIndexChanging" CssClass="table table-bordered">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Consent Forms">
                                                    <ItemTemplate>
                                                        
                                                        <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName") %>' />
                                                        <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId") %>' />
                                                         <asp:HiddenField ID="hdnCode" runat="server" Value='<%#Eval("Code") %>' />
                                                         <asp:HiddenField ID="hdnSignatureType" runat="server" Value='<%#Eval("SignatureType") %>' />
                                                        <asp:HiddenField ID="hdnFieldStatus" runat="server" Value='<%#Eval("FieldStatus") %>' />

                                                           
                                                         <asp:HiddenField ID="hdnShowPatientSignatureImage" runat="server" Value='<%#Eval("ShowPatientSignatureImage") %>' />
                                                        <%--<asp:HiddenField ID="hdnTemplateTypeID" runat="server" Value='<%#Eval("TemplateTypeID") %>' />
                                                            <asp:HiddenField ID="hdnTemplateTypeCode" runat="server" Value='<%#Eval("TemplateTypeCode") %>' />
                                                            <asp:HiddenField ID="hdnEntryType" runat="server" Value='<%#Eval("EntryType") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                              <%--  <asp:TemplateField HeaderText="Print" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnPrint" runat="server" SkinID="label" CommandName="PRINT" CommandArgument='<%#Eval("TemplateId") %>'
                                                            Text="Print" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                  <%-- <asp:TemplateField HeaderText="Template" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnTemplate" runat="server" SkinID="label" CommandName="TemplateSelect" CommandArgument='<%#Eval("TemplateId") %>'
                                                            Text="Template" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnTablet" runat="server" SkinID="label" CommandName="TABLET"  CommandArgument='<%#Eval("TemplateId") %>'
                                                            Text="Select" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Wacom Enable" ItemStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnSelect" runat="server" SkinID="label" CommandName="SELECT"  CommandArgument='<%#Eval("TemplateId") %>'
                                                            Text="Wacom" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </asp:Panel>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-7 col-sm-7" id="table11" runat="server">
                        <asp:Panel ID="Panel2" runat="server" SkinID="Panel" Width="100%" Height="470px"
                                        ScrollBars="Auto">
                                        <asp:GridView ID="gvContentForm" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="True" PageSize="8" OnRowDataBound="gvContentForm_RowDataBound"
                                            OnRowCommand="gvContentForm_RowCommand" OnPageIndexChanging="gvContentForm_PageIndexChanging" CssClass="table table-bordered">
                                            <Columns>
                                                  <asp:TemplateField HeaderText="Encounter No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Text='<%#Eval("EncounterNo") %>' />
                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Consent Forms">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemplateNameConsent" runat="server" SkinID="label" Text='<%#Eval("TemplateName") %>' />
                                                         <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                                           <asp:HiddenField ID="hdnFinalize" runat="server" Value='<%#Eval("Finalize") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Encoded By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncodedBy" runat="server" SkinID="label" Text='<%#Eval("EncodedBy") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Encoded Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncodedDate" runat="server" SkinID="label" Text='<%#Eval("EncodedDate") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                             
                                             
                                                  <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnEditTemp" runat="server"  SkinID="label" CommandName="EDITTEMP" CommandArgument='<%#Eval("Id") %>'
                                                            Text="Edit" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                    <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnPrintTemp" runat="server" SkinID="label" CommandName="VIEW" CommandArgument='<%#Eval("Id") %>'
                                                            Text="View" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                          <asp:ImageButton ID="lnkDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png" />
                                                        <%--<asp:LinkButton ID="lnkDelete" runat="server"  SkinID="label" CommandName="Del" CommandArgument='<%#Eval("Id") %>'
                                                            Text="Delete" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                      
                                    </asp:Panel>
                    </div>

                </div>

            </div>
            
                
                   
             <div class="container-fluid text-center bg-warning">
              
                
                  
            </div>
              <div class="container-fluid text-center bg-danger">
                   
                </div>
            
           <%-- <div class="VisitHistoryBorderNew">
                <div class="container-fluid">
                    <div class="row">
                        <asplUD:UserDetails ID="asplUD" runat="server" />
                    </div>
                </div>
            </div>--%>
           <%-- <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;"
                cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                    </td>
                </tr>
            </table>--%>
         <div style="margin-top:20px;">
              
         </div>
            <table border="0" width="100%" cellpadding="2" cellspacing="1" align="center">
               
                <tr>
                    <td style="padding-left:1%;width: 50%" align="center">
                        <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                        <table runat="server" border="0" cellpadding="1" cellspacing="1" width="100%" style="margin: 10px 0;">
                            <tr>
                                <td style="width: 100px;">
                                    <asp:Label runat="server" Text="Template Type" SkinID="label" style="display:none" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlTemplateTypeCode" Width="100px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTemplateTypeCode_OnSelectedIndexChanged" style="display:none" runat="server">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Consent Forms" Value="CF" Selected="True" />
                                            <telerik:RadComboBoxItem Text="Forms" Value="FS" />
                                            <telerik:RadComboBoxItem Text="Instructions" Value="IN" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    
                                </td>
                               
                                    <td style="width: 100px;padding-left:20px;">
                                    <asp:Label ID="Label3" runat="server" style="margin-left:10px;display:none"  SkinID="label" Text="Encounter" />
                                </td>
                                <td>
                                    <aspNewControls:NewDropDownList ID="ddlEncounter" runat="server" style="display:none" SkinID="DropDown" Width="100%" Height="22px" AutoPostBack="true" OnSelectedIndexChanged="ddlEncounter_SelectedIndexChanged" />

                                    
                                </td>
                               
                           
                              
                            </tr>
                        </table>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                </tr>
               
            </table>
            <asp:Button ID="btnEnableControl" runat="server" Style="visibility: hidden;"
                            OnClick="btnEnableControl_Click" />
             <asp:Button ID="btnTemplatetablebind" runat="server" Style="visibility: hidden;"
                            OnClick="btnTemplatetablebind_Click" />
            <telerik:RadWindowManager ID="RadWindowManager2" runat="server" EnableViewState="false">
                            <Windows>
                                 <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                    </telerik:RadWindow>
                                
                            </Windows>
                        </telerik:RadWindowManager>
              <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
     
</asp:Content>
