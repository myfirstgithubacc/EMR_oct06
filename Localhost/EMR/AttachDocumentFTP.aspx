<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" Trace="false"
    AutoEventWireup="true" CodeFile="AttachDocumentFTP.aspx.cs" Inherits="EMR_AttachDocumentFTP"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelRegAttachDocument.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />

    <link href="../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>--%>

    <%--    <style type="text/css">
        .myClass:hover { background-color: #a1da29 !important;}
        .txt { border: 0px !important; background: #eeeeee !important; color: Black !important; margin-left: 25%; margin-right: auto; width: 100%; /* IE's opacity*/ /* filter: alpha(opacity=50); opacity: 0.50; */ text-align: center;}
    </style>
    --%>
    <style type="text/css">
        @media only screen and (max-width: 650px) {
            div#RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder1_contentPane {
                width: 400px !important;
                min-width: 400px;
            }

            div#RAD_SPLITTER_PANE_CONTENT_ctl00_ContentPlaceHolder1_navigationPane {
                width: 150px;
                min-width: 150px;
            }
        }

        input#ctl00_ContentPlaceHolder1_chkAll {
            margin-top: 2px;
        }

            input#ctl00_ContentPlaceHolder1_chkAll + label {
                margin-left: 8px !important;
                margin-bottom: 5px;
            }
            body{
                overflow-x:hidden;
            }
            p{
                margin-bottom:0px!important;
            }
            #ctl00_ContentPlaceHolder1_lblMessage{
                position:relative;
                width:100%;
                margin:0px!important;
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

        function OnClientClose(oWnd, args) {
            $get('<%=btnrefreshCategory.ClientID%>').click();
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
        function downLoadFile() {
            $get('<%=btnDownLoadFile.ClientID%>').click();
        }
        function LinkBtnMouseOver(lnk) {
            document.getElementById(lnk).style.color = "red";
        }
        function LinkBtnMouseOut(lnk) {
            document.getElementById(lnk).style.color = "blue";
        }
    </script>





    <div class="VisitHistoryDiv">
        <div class="container-fluid">
            <div class="row">

                <div class="col-md-2">
                    <div class="WordProcessorDivText" id="tdName" runat="server">
                        <h2>Patient Documents</h2>                        
                    </div>
                </div>

                <div class="col-md-3">
                    <div class="EMRUHID" id="tdSer" runat="server">
                        <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnGetInfo">
                            <asp:LinkButton ID="lbtnSearchPatient" CssClass="EMRUHIDBtn" runat="server" Text='<%$ Resources:PRegistration, Regno%>' ToolTip="Click to search patient" OnClick="lbtnSearchPatient_Click"></asp:LinkButton>
                            <asp:TextBox ID="txtAccountNo" runat="server" TabIndex="0" MaxLength="10" onkeyup="return validateMaxLength();" />

                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtAccountNo" ValidChars="0123456789" />
                            <asp:TextBox ID="txtRegNo" SkinID="textbox" runat="server" Width="10px" Style="visibility: hidden; display: none;" TabIndex="0"></asp:TextBox>
                            <asp:TextBox ID="txt_hdn_PName" Width="10px" Style="visibility: hidden;" SkinID="textbox" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="hdnregno" runat="server" Value="" />
                            <asp:HiddenField ID="hdnEMRPatientDetails" runat="server" Value="" />
                            <asp:Button ID="btnGetInfo" runat="server" Text="Assign" CausesValidation="false" Enabled="true" SkinID="button" Style="visibility: hidden; display: none;" OnClick="btnGetInfo_Click" TabIndex="103" />
                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                            <asp:Button ID="btnDownLoadFile" runat="server" Style="visibility: hidden; position: absolute; display: none;" Text="" SkinID="Button" CausesValidation="false" OnClick="btnDownLoadFile_OnClick" />
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-md-4"></div>

                <div class="col-md-3 text-right "style="margin-bottom: 5px!important;margin-top: 2px;}">
                    <asp:Button ID="ibtnClose" runat="server" CssClass="btn btn-primary" Text="Close" ToolTip="Close" OnClientClick="window.close();" Visible="false" />
                    <asp:Button ID="btnUpload" runat="server" CssClass="btn btn-primary" Text="Upload " ToolTip="Upload(Ctrl+F3)" ValidationGroup="save" OnClick="btnUpload_Click" />
                    <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary" ToolTip="New" Visible="false" Text="New" OnClick="btnNew_Click" />

                    <asp:Button ID="btnNo" runat="server" ToolTip="Close (Ctrl+F8)" Text="Close" CssClass="btn btn-primary" CausesValidation="false" Visible="false" OnClick="btnNo_OnClick" />
                    <asp:ValidationSummary DisplayMode="BulletList" ShowMessageBox="true" ShowSummary="false" ValidationGroup="save" ID="ValidationSummary1" runat="server" />
                </div>

            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
        <ContentTemplate>

            <div class="row">

                <div id="pdetails" class="col-12" runat="server" style="overflow:hidden;">
                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                    <%--<asplNewEMR:UserDetailsHeader ID="asplHeaderUDEMR" runat="server" /> 
                            <asplNurse:UserDetailsHeader ID="asplHeaderUDNurse" runat="server" />
                            <%--<asp:Xml ID="xmlPatientInfo" runat="server"></asp:Xml>--%>
                </div>

            </div>



            <div class="ImmunizationDOB-Div">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblMessage" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <div class="col-md-12" id="reg" runat="server">
                            <span id="tddetails" runat="server">
                                <asp:LinkButton ID="lnkDemographics" runat="server" CausesValidation="false" Text="Demographics" Visible="false" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkDemographics_OnClick" />
                                <asp:LinkButton ID="lnkAllergies" runat="server" CausesValidation="false" Text="Allergies" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" Visible="false" onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkAllergies_OnClick" />
                                <asp:LinkButton ID="lnkPatientRelation" runat="server" CausesValidation="false" Text="Contacts" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" Visible="false" onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkPatientRelation_OnClick" />
                                <asp:LinkButton ID="lnkOtherDetails" runat="server" CausesValidation="false" Font-Bold="true" Visible="false" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" Text="Other Details" OnClick="lnkOtherDetails_OnClick" />
                                <asp:LinkButton ID="lnkResponsibleParty" runat="server" CausesValidation="false" Visible="false" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" Text="Kin&nbsp;Details" OnClick="lnkResponsibleParty_OnClick" />
                                <asp:LinkButton ID="lnkPayment" runat="server" CausesValidation="false" Text="Payer" Visible="false" Font-Bold="true" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkPayment_OnClick" />
                                <asp:Label ID="LblLabelname" runat="server" Text="Attach Documents" Font-Bold="true" Visible="false" />
                            </span>
                        </div>

                        <div class="col-md-12" style="text-align:center;">
                            <asp:Literal ID="ltrlMessage" runat="server" Mode="Transform"></asp:Literal>
                        </div>
                    </div>
                </div>
            </div>






            <div class="ImmunizationDD-Div overflow-hidden">
                <div class="container-fluid">
                    <div class="row">

                        <div class="col-md-12">
                            <telerik:RadSplitter ID="RadSplitter1" Width="100%" runat="server" Height="510">

                                <telerik:RadPane ID="navigationPane" runat="server" Width="18%">
                                    <span class="EMRDocuments"><strong>Documents</strong></span>

                                    <asp:CheckBox ID="chkAll" runat="server" CssClass="EMRDocuments" Text="All" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />

                                    <asp:TreeView ID="TreeView1" runat="server" ImageSet="Msdn" CssClass="EMRList" NodeIndent="10" ShowExpandCollapse="true" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                                        <ParentNodeStyle Font-Bold="False" />
                                        <HoverNodeStyle Font-Underline="True" BackColor="#CCCCCC" BorderColor="#888888" BorderStyle="Solid" BorderWidth="0px" />
                                        <SelectedNodeStyle BackColor="gray" ForeColor="White" Font-Underline="False" HorizontalPadding="3px" VerticalPadding="1px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="1px" VerticalPadding="2px" />
                                    </asp:TreeView>
                                    <br />
                                </telerik:RadPane>




                                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward" />

                                <telerik:RadPane ID="contentPane" runat="server" Width="80%">
                                    <asp:Panel ID="UploadDocument" runat="server" Width="99%">

                                        <div class="VitalHistory-Div">
                                            <div class="container-fluid">

                                                <div class="row">
                                                    <div class="col-md-4 form-group">
                                                        <div class=" row">
                                                            <div class="col-md-4 col-4">
                                                                <p>File Upload <span class="red">*</span></p>
                                                            </div>
                                                            <div class="col-md-8 col-8">
                                                                <asp:FileUpload ID="_FileUpload" runat="server" Width="100%" CssClass="button" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="_FileUpload" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Select File!"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-5 form-group">
                                                        <div class=" row">
                                                            <div class="col-md-3 col-4">
                                                                <p>Category <span class="red">*</span></p>
                                                            </div>
                                                            <div class="col-md-5 col-5  pl-md-0">
                                                                <asp:DropDownList ID="ddlCategory" CssClass="form-control" runat="server" Width="100%">
                                                                    <asp:ListItem Text="Select" Value="" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3 col-3 pl-0">
                                                                <asp:Button ID="btnDocumentCategory" runat="server" Text="New Category" ToolTip="New Category" Visible="true" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnDocumentCategory_Click" />

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlCategory" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Select Category!"></asp:RequiredFieldValidator>
                                                        </div>

                                                    </div>


                                                </div>

                                                    <div class="col-md-3 form-group">
                                                        <div class=" row">
                                                            <div class="col-md-5 col-4">
                                                                <p>File Name <span class="red">*</span></p>
                                                            </div>
                                                            <div class="col-md-7 col-8">
                                                                <asp:TextBox ID="txtDescription" Width="100%" CssClass="form-control" runat="server" MaxLength="100" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDescription" Display="None" ValidationGroup="save" SetFocusOnError="true" runat="server" ErrorMessage="Please Enter File Name!"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-4  form-group">
                                                        <div class=" row">
                                                            <div class="col-md-4 col-4">
                                                                <p>Remarks</p>
                                                            </div>
                                                            <div class="col-md-8 col-8">
                                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" MaxLength="200" Width="100%" Text='<%#Eval("Remarks") %>' />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-5  form-group">
                                                        <div class=" row">
                                                            <div class="col-6">
                                                                <div class="row">
                                                                    <div class="col-5">
                                                                        <p>Date</p>
                                                                    </div>
                                                                    <div class="col-7">
                                                                        <asp:Button ID="btnrefreshCategory" runat="server" Style="visibility: hidden; position: absolute;" Text="New Category" ToolTip="New Category" CausesValidation="false" OnClick="btnrefreshCategory_Click" />
                                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" Width="100%" Columns="10"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="valDateOfBirth" runat="server" ValidationExpression="^([1-9]|0[1-9]|1[012])[- /.]([1-9]|0[1-9]|[12][0-9]|3[01])[- /.][0-9]{4}$" ControlToValidate="txtDate" SetFocusOnError="true" Display="Dynamic" ErrorMessage="Invalid Date."></asp:RegularExpressionValidator>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="col-6 ">
                                                                <div class="row">
                                                                    <div class="col-md-4 col-5">
                                                                        <p>Visit No</p>
                                                                    </div>
                                                                    <div class="col-md-8 col-7">
                                                                        <asp:DropDownList ID="ddlvisit" Width="100%" CssClass="form-control" runat="server"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-7">
                                                            <div class="EMRUploadDiv01">
                                                                <h4>
                                                                    <asp:Label ID="lblShowInEMRCaseSheet" runat="server" SkinID="label" Text="Show In EMR Case Sheet" />
                                                                    <asp:CheckBox ID="chkShowInEMRCaseSheet" runat="server" />
                                                                </h4>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" id="divRiscticted" runat="server" visible="false">

                                                    <div class="col-md-6">
                                                         <div class="EMRUploadDiv01">
                                                             <h4>
                                                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Restrict outside MRD" />
                                                                    <asp:CheckBox ID="chkFileRestricted" runat="server" />
                                                                </h4>
                                                            
                                                          </div>
                                                   </div>
                                                     <div class="col-md-6">
                                                     </div>
                                                 </div>


                                            </div>
                                        </div>


                                    </asp:Panel>



                                    <asp:Panel ID="pnlImages" runat="server" ScrollBars="None">
                                        <telerik:RadListView runat="server" ID="RadListView1" OnPageIndexChanged="RadListView1_PageIndexChanged" AllowPaging="true" DataKeyNames="Id" Width="98%">

                                            <LayoutTemplate>
                                                <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="RadListView1" Width="100%" PageSize="6">
                                                    <Fields>
                                                        <telerik:RadDataPagerTemplatePageField Visible="true"></telerik:RadDataPagerTemplatePageField>
                                                        <telerik:RadDataPagerButtonField FieldType="FirstPrev" />
                                                        <telerik:RadDataPagerButtonField FieldType="Numeric" />
                                                        <telerik:RadDataPagerButtonField FieldType="NextLast" />
                                                        <telerik:RadDataPagerGoToPageField CurrentPageText="Page: " TotalPageText="of" SubmitButtonText="Go" TextBoxWidth="20" />
                                                        <telerik:RadDataPagerTemplatePageField>
                                                            <PagerTemplate></PagerTemplate>
                                                        </telerik:RadDataPagerTemplatePageField>
                                                    </Fields>
                                                </telerik:RadDataPager>
                                                <fieldset runat="server" id="itemPlaceholder" />
                                            </LayoutTemplate>

                                            <EmptyItemTemplate>
                                                <div class="VitalHistory-Div">
                                                    <div class="container-fluid">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="EMRUploadDiv">
                                                                    <h3>No item to display here.</h3>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </EmptyItemTemplate>


                                            <EmptyDataTemplate>
                                                <div class="VitalHistory-Div">
                                                    <div class="container-fluid">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="EMRUploadDiv">
                                                                    <h3>No records for patient available.</h3>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </EmptyDataTemplate>



                                            <ItemTemplate>
                                                <fieldset style="float: left; margin: 5px; padding: 2px; background: #eeeeee" class="myClass">
                                                    <table>
                                                        <tr>
                                                            <td colspan="2">
                                                                <%--DataValue='<%#Eval("Data") %>'--%>
                                                                <telerik:RadBinaryImage Style="cursor: pointer;" runat="server" ID="RadBinaryImage1" OnClick='<%#CreateWindowScript(DataBinder.Eval(Container.DataItem, "id")) %>' ImageUrl='<%#Bind("ImagePath") %>' Height='<%#ImageHeight%>' Width="<%#ImageWidth %>" ResizeMode="Fit" AutoAdjustImageControlSize="false" AlternateText="Thumbnail not available please download to view" ToolTip='<%#Bind("Description")%>' />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:TextBox ID="txtRemarks" Width="400px" ReadOnly="true" Rows="3" TextMode="MultiLine" runat="server" Text='<%#Bind("Remarks") %>'></asp:TextBox></td>
                                                        </tr>

                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button ID="btnDelete" CssClass="SearchKeyBtn03" runat="server" Text="Delete" CommandArgument='<%#Eval("Id") %>' OnClick="btnDelete_Click" /></td>
                                                            <td align="right">
                                                                <asp:LinkButton ID="lbtnDownload" runat="server" Visible="false" Text="Download" Font-Bold="true" Font-Size="14px" ForeColor="Red" CommandArgument='<%#Eval("ImageName") %>' OnClick="lbtnDownload_OnClick"></asp:LinkButton></td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </ItemTemplate>

                                        </telerik:RadListView>

                                        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" EnableViewState="false" Width="550px" Height="450px">
                                            <Windows>
                                                <telerik:RadWindow runat="server" ID="Details" NavigateUrl="DisplayImage.aspx" Behaviors="Close,Move" Modal="true"></telerik:RadWindow>
                                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow>
                                            </Windows>
                                        </telerik:RadWindowManager>

                                    </asp:Panel>
                                </telerik:RadPane>

                            </telerik:RadSplitter>








                        </div>
                    </div>
                </div>


            </div>




            <%--  <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
      <td align="right">
          <asp:LinkButton ID="lbtnDownloadTest" runat="server" Text="Download" Font-Bold="true" Font-Size="14px" ForeColor="Red" OnClick="lbtnDownloadTest_Click">

          </asp:LinkButton></td>

                </tr>
            </table>--%>
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>
