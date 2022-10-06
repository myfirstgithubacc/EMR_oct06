<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true" CodeFile="POCOutSource.aspx.cs" Inherits="LIS_Phlebotomy_OutSource" %>

<%@ Register TagPrefix="ucl" TagName="legend" Src="~/Include/Components/Legend.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Src="~/Include/Components/TopPanelNew.ascx" TagPrefix="ucl" TagName="TopPanelNew" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

        <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/InvestigationNew.css" rel="stylesheet" type="text/css" />
        <script src="../../Include/JS/scripts.js" type="text/javascript" />
        <style>
            /*#ctl00_ContentPlaceHolder1_gvSelectedServices_ctl18_txtM,#ctl00_ContentPlaceHolder1_gvSelectedServices_ctl03_txtM { margin:10px 0 0 0;}*/
            .txtMClass {
                margin: 10px 0 0 0;
            }
            /*#ctl00_ContentPlaceHolder1_gvSelectedServices_ctl02_txtW { height:344px !important; border:solid 1px lightblue !important; width:100% !important; border-right:solid 2px lightblue !important; position: relative; overflow-y: scroll;}*/
            .txtwnew {
                min-height: 344px !important;
                border: solid 1px lightblue !important;
                width: 100% !important;
                border-right: solid 2px lightblue !important;
                position: relative;
                overflow-y: scroll;
            }


            /*.resultLeft {margin:9% 0 0 0 !important; padding:0; float:left;}*/

            /*.resultLeft01 {margin:0% 0 0 0 !important; padding:0; float:left;}*/
            #ctl00_ContentPlaceHolder1_gvSelectedServices_ctl03_lblFieldName,
            #ctl00_ContentPlaceHolder1_gvSelectedServices_ctl29_lblFieldName,
            #ctl00_ContentPlaceHolder1_gvSelectedServices_ctl31_lblFieldName {
                margin: 0% 0 0 0 !important;
                padding: 0;
                float: left;
            }

            .HistopathologyInput {
                float: left !important;
                width: 170px !important;
                margin: 0 0 0 -1.7em !important;
            }

            #ctl00_ContentPlaceHolder1_gvSelectedServices_ctl29_lblTM {
                margin: -2em 0 0 16em !important;
                padding: 0;
                float: left;
                height: auto !important;
                width: 139% !important;
            }

            .RadComboBox .rcbInputCell .rcbInput {
                border: 1px solid #ABABAB !important;
                border-left: none !important;
            }

            .RadComboBoxDropDown .rcbScroll {
                background-color: #fff;
            }

            .SensitivityBtn {
                float: left !important;
                margin: -1.5em 0 0 0 !important;
            }

            .EnzymeDropDown {
                float: left !important;
                margin: -1.5em 0 0 0 !important;
            }

            .SensitivityInput {
                float: left !important;
                margin: 0em 0 0 1.2em !important;
            }
            /*.MarginSpacing { margin:0px 0 0 0;}*/
            /*.titleText {margin:-2.4em 0 0 0 !important;}*/
            Table.rfdTable tr td {
                margin: 0em 0 0 0 !important;
                padding: 0;
                line-height: 2.2em !important;
            }

           .leftTextname { vertical-align: middle;}
            .header_main { margin: 0;}
            div#ctl00_ContentPlaceHolder1_TopPanelNew_UpdatePanel400 { display: none;}
        </style>


        <div>
            <script type="text/javascript" src="/Include/JS/Functions.js" language="javascript"></script>
            <script type="text/javascript">

                function ValidateTextAreaMaxLenth(txt, MaxLength) {
                    if (txt.value.length > MaxLength) {
                        txt.value = txt.value.substring(0, MaxLength);
                        txt.focus();
                        alert("Text length should not be more then " + MaxLength + " characters.");
                    }
                }

                function OnClientSelectionChange(editor, args) {
                    var tool = editor.getToolByName("RealFontSize");
                    if (tool && !$telerik.isIE) {
                        setTimeout(function () {
                            var value = tool.get_value();

                            switch (value) {
                                case "8px":
                                    value = value.replace("8px", "9pt");
                                    break;
                                case "9px":
                                    value = value.replace("9px", "9pt");
                                    break;
                                case "10px":
                                    value = value.replace("10px", "10pt");
                                    break;
                                case "11px":
                                    value = value.replace("11px", "10pt");
                                    break;
                                case "12px":
                                    value = value.replace("12px", "10pt");
                                    break;
                                case "13px":
                                    value = value.replace("13px", "10pt");
                                    break;
                                case "14px":
                                    value = value.replace("14px", "11pt");
                                    break;
                                case "15px":
                                    value = value.replace("15px", "11pt");
                                    break;
                                case "16px":
                                    value = value.replace("16px", "12pt");
                                    break;
                                case "18px":
                                    value = value.replace("18px", "14pt");
                                    break;
                                case "19px":
                                    value = value.replace("19px", "14pt");
                                    break;
                                case "20px":
                                    value = value.replace("20px", "14pt");
                                    break;
                                case "22px":
                                    value = value.replace("22px", "14pt");
                                    break;
                                case "24px":
                                    value = value.replace("24px", "18pt");
                                    break;
                                case "26px":
                                    value = value.replace("26px", "20pt");
                                    break;
                                case "27px":
                                    value = value.replace("27px", "20pt");
                                    break;
                                case "28px":
                                    value = value.replace("28px", "20pt");
                                    break;
                                case "32px":
                                    value = value.replace("32px", "24pt");
                                    break;
                                case "34px":
                                    value = value.replace("34px", "26pt");
                                    break;
                                case "35px":
                                    value = value.replace("35px", "26pt");
                                    break;
                                case "36px":
                                    value = value.replace("36px", "26pt");
                                    break;
                                case "48px":
                                    value = value.replace("48px", "36pt");
                                    break;
                                case "72px":
                                    value = value.replace("72px", "48pt");
                                    break;
                            }
                            tool.set_value(value);

                        }, 0);
                    }
                }

            </script>

            <script type='text/javascript'>
                function OnClientEditorLoad(editor, args) {
                    var tool = editor.getToolByName("FontName");
                    tool.set_value("Verdana");
                    editor.get_contentArea().style.fontFamily = 'Verdana'
                    editor.get_contentArea().style.fontSize = '12pt'
                }
            </script>

            <script type="text/javascript">
                var ilimit = 40;
                function AutoChange(txtRemarks) {
                    var txt = document.getElementById(txtRemarks);
                    if (txt.value.length >= 10) {
                        if (txt.value.length >= 40 * txt.rows) {
                            txt.rows = txt.rows + 1;
                            ilimit = 0;
                        }
                        else if (txt.value.length < 40 * (txt.rows - 1)) {
                            txt.rows = Math.round(txt.value.length / 40) + 1;
                        }
                        else if (txt.value.length >= 500) {
                            txt.value.length = txt.value.substring(0, 500)
                            return false;
                        }
                        else {
                            if (txt.value.length <= ilimit * txt.rows && txt.rows >= ilimit) {
                                txt.cols = (txt.cols * 1) + 1;
                            }
                        }
                    }
                    return true;
                }
                function maxLength(that, intMax) {
                    if (that.value.length > intMax) {
                        that.value = that.value.substr(0, intMax);
                        alert("Maximum Length is " + intMax + " characters only.");
                    }
                }

                function openRadWindowW(ID, ControlType, RowIndex, LabId, Lab) {
                    //var lbl = document.getElementById(hdnSelCell); santosh
                    $get('<%=hdnSelCell.ClientID%>').value = RowIndex;
                var oWnd = radopen("/EMR/Templates/SentanceGallery.aspx?ID=" + ID + "&ControlType=" + ControlType + "&LabId=" + LabId + "&Lab=" + Lab, "Radwindow1");
                oWnd.Center();
            }

            //OnClientCloseComments
            
            if (window.captureEvents) {
                window.captureEvents(Event.KeyUp);
                window.onkeyup = executeCode;
            }
            else if (window.attachEvent) {
                document.attachEvent('onkeyup', executeCode);
            }

            function OnCloseSentenceGalleryRadWindow(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var Sentence = arg.Sentence;
                    var ctrl = arg.ControlId;
                    var ctrltype = arg.ControlType;
                    ctrl = document.getElementById(ctrl);
                    if (ctrltype == "W") {

                        $get('<%=btnCheck.ClientID%>').click();
                    }
                    else {
                        ctrl.value = Sentence;
                    }
                }
            }
            function OnCloseFieldValueRadWindow(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var ValueId = arg.Sentence;
                    var ctrl = arg.ControlId;
                    var ctrltype = arg.ControlType;
                    ctrl = document.getElementById(ctrl);
                    if (ctrltype == "W") {
                        $get('<%=btnCheck.ClientID%>').click();
                    }
                    else if (ctrltype == "T" || ctrltype == "M") {
                        ctrl.value = ValueId;
                    }

                    $get('<%=hdnControlType.ClientID%>').value = ctrltype;
                }
            }
            function onFocusTxtF(btnF) {
                $get(btnF).click();
            }
            function OnClientIsValidPasswordClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;
                }
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }

            function OnClientTextFormatClose(oWnd, args) {
                $get('<%=btnTextFormatClose.ClientID%>').click();
            }
            </script>

            <script type="text/javascript">
                var editorList = new Object();
                var editorLengthArray = [3000, 3000, 3000, 3000, 3000, 3000];
                var counter = 0;

                function isAlphaNumericKey(keyCode) {
                    if ((keyCode > 47 && keyCode < 58) || (keyCode > 64 && keyCode < 91)) {
                        return true;
                    }
                    return false;
                }

                function LimitCharacters(editor) {

                    editorList[editor.get_id()] = editorLengthArray[counter];
                    counter++;
                    var mode = editor.get_mode();
                    editor.attachEventHandler("onkeydown", function (e) {
                        e = (e == null) ? window.event : e;
                        if (isAlphaNumericKey(e.keyCode)) {
                            var maxTextLength = editorList[editor.get_id()];
                            if (mode == 2) {
                                textLength = editor.get_textArea();
                            }
                            else {
                                textLength = editor.get_text().length;
                            }
                            if (textLength >= maxTextLength) {
                                alert('We are not able to accept more than ' + maxTextLength + ' symbols!');
                                e.returnValue = false;
                            }
                        }
                    });
                }
                function CalculateLength(editor, value) {
                    var textLength = editor.get_text().length;
                    var clipboardLength = value.length;
                    textLength += clipboardLength;
                    return textLength;
                }
                function OnClientPasteHtml(editor, args) {
                    var maxTextLength = editorList[editor.get_id()];
                    var commandName = args.get_commandName();
                    var value = args.get_value();

                    if (commandName == "PasteFromWord"
                        || commandName == "PasteFromWordNoFontsNoSizes"
                        || commandName == "PastePlainText"
                        || commandName == "PasteAsHtml"
                        || commandName == "Paste") {
                        var textLength = CalculateLength(editor, value);
                        if (textLength >= maxTextLength) {
                            alert('We are not able to accept more than ' + maxTextLength + ' symbols!');
                            args.set_cancel(true);

                        }
                    }
                }
            </script>

            <script type="text/javascript">
                function LinkBtnMouseOver(lnk) {
                    document.getElementById(lnk).style.color = "SteelBlue";
                }
                function LinkBtnMouseOut(lnk) {
                    document.getElementById(lnk).style.color = "SteelBlue";
                }
                function checkRange(txtboxCtrl, minVal, maxVal) {
                    var numVal = document.getElementById(txtboxCtrl);

                    if (minVal <= parseFloat(numVal.value) && parseFloat(numVal.value) <= maxVal) {
                        numVal.style.color = '#000000';
                    } else {
                        numVal.style.color = '#ff0000';
                    }
                }
            </script>

            <script type="text/javascript">
                function searching() {
                    var h = document.getElementById('ctl00_ContentPlaceHolder1_hdnTotalTabIndexValue');

                    if (document.activeElement.tabIndex == (h.value - 1)) {
                        $get('<%=btnGetInfo.ClientID%>').click();
                }
            }

            </script>
            <asp:HiddenField ID="hdnFontName" runat="server" />
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="650" Height="580"
                EnableViewState="false" VisibleStatusbar="false" Behaviors="Close,Move" OnClientClose="OnCloseFieldValueRadWindow"
                VisibleOnPageLoad="false" ReloadOnShow="false">
            </telerik:RadWindowManager>


            <asp:UpdatePanel ID="valMain" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                        Behaviors="Close">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowPopup" runat="server" />
                        </Windows>
                    </telerik:RadWindowManager>
                    <div style="overflow-y: hidden; overflow-x: hidden;">

                        <div class="container-fluid header_main form-group">
                            <div class="col-md-3 col-sm-3">
                                <h2>
                                    <asp:Label ID="lblHeader" runat="server" Text="Outsource Investigation Result Entry" /></h2>
                            </div>

                        </div>
                        <div class="container-fluid">
                            <div class="row">
                                <ucl:TopPanelNew runat="server" ID="TopPanelNew" />
                            </div>
                        </div>
                         <div class="text-center">
                            <asp:Label ID="lblMessage" runat="server" Style="font-size: large" Text="" />
                        </div>
                        <div class="container-fluid">
                            <div class="row">
                                <div class="col-md-12">

                                    <div class="col-md-4" >
                                        <div class="col-md-12" style="display:none;">
                                            <div class="col-md-4">
                                                <label>Result Date</label>
                                            </div>
                                            <div class="col-md-8">
                                                <telerik:RadDateTimePicker ID="txtFromDate" runat="server" DateInput-DateFormat="dd/MM/yyyy HH:mm:ss" Width="150px" DatePopupButton-Visible="false" ShowPopupOnFocus="true" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="col-md-12" style="display:none;">
                                            <div class="col-md-4">
                                            <label id="lblServiceName" runat="server">Service Name</label>
                                            </div>
                                            <div class="col-md-8">
                                                <telerik:RadComboBox ID="ddlService" runat="server" Width="100%" DropDownWidth="350px" Height="300px" EmptyMessage="Select Service Name" Filter="Contains" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 text-right">

                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add" ToolTip="Add" OnClick="btnAdd_Click" />
                                        <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Visible="false" Text="Save" CssClass="btn btn-primary" Width="80px" />
                                        <asp:LinkButton ID="lnkFinalRelease" runat="server" Visible="false" Font-Bold="true" Text="Final&nbsp;Release" OnClick="lnkFinalRelease_OnClick"></asp:LinkButton>
                                                        <asp:Button ID="btnViewResult" CssClass="btn btn-default" runat="server" Text="View Result" ToolTip="View Result" OnClick="btnViewResult_Click" />
                                       


                                    </div>

                                </div>

                            </div>
                        </div>
                     
                        <div class="col-md-5 col-sm-5 text-right PaddingLeftSpacing">
                            <span class="pull-left">
                                <asp:Label ID="lblResultStatus" runat="server" ForeColor="DodgerBlue"></asp:Label></span>
                            <asp:UpdatePanel ID="upd1" runat="server">
                                <ContentTemplate>
                                    <div style="display: none;">
                                        <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                                        <asp:HiddenField ID="hdnIsAddendumReleased" Value="0" runat="server" />
                                        <asp:HiddenField ID="hdnUseAddendum" Value="0" runat="server" />
                                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server" Behaviors="Close,Move,Pin,Resize,Maximize">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                        <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false"
                                            Style="visibility: hidden; float: left; margin: 0; padding: 0; height: 1px;" Width="1px" />
                                        <asp:HiddenField ID="hdnIsCallFromLab" runat="server" Value="1" />
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>

                    </div>
                    </div>

                


                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-3 col-sm-4" id="Printtab" runat="server">
                                <asp:Label ID="lblServceName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Panel ID="Panel3" runat="server" SkinID="Panel" Height="500px" Width="100%"
                                            ScrollBars="Auto" BorderWidth="1px" BorderColor="LightBlue" Visible="false">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TreeView ID="tvCategory" runat="server" ImageSet="Msdn" Font-Size="6pt" NodeIndent="10"
                                                        OnSelectedNodeChanged="tvCategory_SelectedNodeChanged" NodeWrap="true">
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <HoverNodeStyle Font-Underline="True" BorderColor="#888888" BorderStyle="Solid" BorderWidth="0px" />
                                                        <SelectedNodeStyle BackColor="Gray" ForeColor="Black" Font-Underline="False" HorizontalPadding="3px"
                                                            VerticalPadding="1px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                                        <NodeStyle Font-Names="Verdana" Font-Size="7pt" ForeColor="Black" HorizontalPadding="5px"
                                                            NodeSpacing="1px" VerticalPadding="2px" />
                                                    </asp:TreeView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row form-group" style="display: block;">
                                    <div class="col-md-4 col-sm-4">
                                        <asp:Label ID="Label13" runat="server" Text="Remarks" Visible="false"/>
                                    </div>
                                    <div class="col-md-8 col-sm-8">
                                        <telerik:RadComboBox ID="ddlRemarks" runat="server" EmptyMessage="[ Select Remarks ]"
                                            MarkFirstMatch="true" Width="100%" Visible="false">
                                        </telerik:RadComboBox>
                                    </div>
                                </div>

                                <div class="row form-group" id="trReviewRemark" runat="server" visible="false">
                                    <asp:Label ID="lblOldReviewRemark" runat="server" />
                                    <div class="col-md-4 col-sm-4">
                                        <span class="textName">
                                            <asp:Label ID="lblReviewRemark" runat="server" Text="Review Remark" /></span>
                                    </div>
                                    <div class="col-md-8 col-sm-8">
                                        <asp:TextBox ID="txtReviewRemark" runat="server" Width="100%" TextMode="MultiLine" onkeyup="return ValidateTextAreaMaxLenth(this, 400);" />
                                    </div>
                                </div>

                            </div>


                            <div class="col-md-9 col-sm-8">

                                <div class="row form-group">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Panel ID="pnlMultipleResult" runat="server" SkinID="Panel" ScrollBars="None"
                                            Height="28px" Style="padding-top: 5px;">
                                            <asp:Label ID="lblInfoResult" runat="server" SkinID="label" Text="Result Stage" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <telerik:RadComboBox ID="ddlResultId" runat="server" MarkFirstMatch="true" CausesValidation="false"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlResultId_OnSelectedIndexChanged"
                                                Width="50px" Skin="Outlook">
                                            </telerik:RadComboBox>
                                            <telerik:RadComboBox ID="ddlMultiStageResultFinalized" runat="server" MarkFirstMatch="true" CausesValidation="false"
                                                Width="85px" Skin="Outlook">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Interim" Value="0" Selected="true" />
                                                    <telerik:RadComboBoxItem Text="Final" Value="1" />
                                                </Items>
                                            </telerik:RadComboBox>
                                            <asp:Label ID="lblInfoMultipleResultCount" runat="server" SkinID="label" />
                                        </asp:Panel>
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="Update">
                                            <ContentTemplate>
                                                <telerik:RadMenu ID="menuTemplate" runat="server" Style="padding-top: 0px; padding-bottom: 0px; z-index: 100"
                                                    EnableShadows="true" Skin="Windows7" OnItemClick="menuTemplate_OnItemClick"
                                                    OnItemDataBound="menuTemplate_OnItemDataBound" Width="100%" Visible="false" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Label ID="lblGrid" runat="server" />
                                    </div>
                                </div>

                                <div class="row form-group" align="left">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlNonTabularFormat" Width="100%" runat="server">
                                                    <asp:GridView ID="gvSelectedServices" GridLines="None" runat="server" ShowHeader="false"
                                                        ShowFooter="false" AutoGenerateColumns="False" Width="100%" CssClass="table-condensed table-striped" AllowPaging="false"
                                                        PagerSettings-Visible="true" PagerSettings-Mode="NumericFirstLast" OnRowDataBound="gvSelectedServices_RowDataBound">
                                                        <RowStyle Font-Size="9pt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="FieldID" HeaderText="ID" />
                                                            <asp:TemplateField ItemStyle-Width="1%" HeaderStyle-Width="1%" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgShowImage" runat="server" ImageUrl="~/Images/close_new.jpg"
                                                                        Visible="false" Width="13px" Height="13px" ToolTip="Show Result After Finalization" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Property Name" ItemStyle-Width="13%" ControlStyle-CssClass="resultLeft" ItemStyle-CssClass="leftTextname" HeaderStyle-Width="13%"
                                                                HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFieldName" CssClass="label2" runat="server" Text='<%#Eval("FieldName") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="FieldType" HeaderText="PropertyType" ItemStyle-Width="0%"
                                                                HeaderStyle-Width="0%" />
                                                            <asp:TemplateField HeaderText="Values" ItemStyle-Width="85%" ItemStyle-CssClass="" HeaderStyle-Width="80%"
                                                                HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <table id="tbl1" runat="server" cellpadding="2" cellspacing="0" border="0" width="100%">
                                                                        <tr align="left">
                                                                            <td width="100%">
                                                                                <div class="row">
                                                                                    <div class="col-md-6 PaddingLeftSpacing">
                                                                                        <div class="row">
                                                                                            <div class="col-md-9">
                                                                                                <div class="row">
                                                                                                    <div class="col-md-1">
                                                                                                        <asp:TextBox ID="txtT" Width="246px" Columns='<%#Convert.ToInt32(Eval("MaxLength"))%>'
                                                                                                            Visible="false" CssClass="SensitivityInput" MaxLength='<%#Convert.ToInt32(Eval("MaxLength"))%>' runat="server"
                                                                                                            Height="22px" autocomplete="off" />
                                                                                                    </div>
                                                                                                    <div class="col-md-9 PaddingSpacing">
                                                                                                        <telerik:RadComboBox ID="ddlMultilineFormats" runat="server" CssClass="HistopathologyInput" Visible="false" OnSelectedIndexChanged="ddlMultilineFormats_OnSelectedIndexChanged"
                                                                                                            AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                                                                                            MarkFirstMatch="true">
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:Label ID="lblTM" runat="server" Height="22px" Visible="false" />
                                                                                                        <telerik:RadToolTip runat="server" ID="ttMSpecialReferenceRange" TargetControlID="lblTM"
                                                                                                            IsClientID="false" ShowEvent="OnMouseOver" HideEvent="Default" Position="BottomRight"
                                                                                                            RelativeTo="Mouse" Width="100%" Height="250px" Title="Special Reference Range" />

                                                                                                        <telerik:RadComboBox ID="ddlTemplateFieldFormats" CssClass="HistopathologyInput" runat="server" Visible="false"
                                                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateFieldFormats_OnSelectedIndexChanged"
                                                                                                            Width="100%" AppendDataBoundItems="true" MarkFirstMatch="true" />
                                                                                                    </div>

                                                                                                    <div class="col-md-1 PaddingLeftSpacing">
                                                                                                        <asp:Button ID="btnHelp" Text="H" Visible="false" ToolTip="Sentence Gallery" runat="server" CssClass="btn btn-primary" />
                                                                                                    </div>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>

                                                                                <div class="row form-groupTop01">
                                                                                    <%--<div class="col-md-4">--%>
                                                                                    <asp:Label ID="lblM" runat="server" Height="22px" Visible="false" />
                                                                                    <div class="col-md-12">
                                                                                        <asp:TextBox ID="txtM" runat="server" Width="100%" Height="60px"
                                                                                            MaxLength="250" onkeyup="return maxLength(this,250);" TextMode="MultiLine" Visible="false"
                                                                                            autocomplete="off" />
                                                                                    </div>

                                                                                    <div class="col-md-12">
                                                                                        <telerik:RadEditor ID="txtW" runat="server" EnableEmbeddedSkins="true" EnableResize="true" CssClass="txtwnew txtwnew01"
                                                                                            Skin="Vista" EditModes="Design" ToolsFile="~/Include/XML/BasicTools.xml"
                                                                                            OnClientSelectionChange="OnClientSelectionChange" BorderColor="LightBlue" OnClientLoad="OnClientEditorLoad">
                                                                                            <CssFiles>
                                                                                                <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                                                                            </CssFiles>
                                                                                        </telerik:RadEditor>
                                                                                    </div>

                                                                                    <asp:LinkButton ID="lnkTextFormat" runat="server" Text="Text Format" Visible="false"
                                                                                        onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);" />
                                                                                    <%--</div>--%>
                                                                                    <div class="col-md-8">
                                                                                        <asp:TextBox ID="txtF" Columns="20" MaxLength="20" Width="125px" Visible="false"
                                                                                            runat="server" Height="22px" autocomplete="off" />
                                                                                        <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                            TargetControlID="txtF" FilterType="Custom" ValidChars="0123456789." />
                                                                                        <asp:TextBox ID="TxtFRemarks" Columns='<%#Convert.ToInt32(Eval("MaxLength"))%>'
                                                                                            MaxLength="20" Visible="false" Width="125px" runat="server" Height="22px" autocomplete="off" />
                                                                                        <asp:Button ID="btnF" CssClass="btn btn-primary" runat="server" Text="Calculate" Visible="false"
                                                                                            OnClick="btnF_Click" CommandArgument='<%#Eval("FieldID") %>' />
                                                                                        <asp:Label ID="lblF" runat="server" Height="22px" Visible="false" />
                                                                                    </div>
                                                                                </div>



                                                                                <div class="row">
                                                                                    <div class="col-md-4">
                                                                                        <asp:TextBox ID="txtN" Columns="20" Visible="false" MaxLength="25" runat="server" Height="22px" autocomplete="off" />
                                                                                    </div>
                                                                                    <div class="col-md-3 PaddingLeftSpacing">
                                                                                        <asp:Label ID="lblLocation" runat="server" Visible="false" Text="Machine" CssClass="margin_Top03"></asp:Label>
                                                                                        <asp:Label ID="lblN" runat="server" CssClass="titleText" Height="22px" Visible="false" />
                                                                                        <telerik:RadToolTip runat="server" ID="ttSpecialReferenceRange" TargetControlID="lblN" IsClientID="false" ShowEvent="OnMouseOver" HideEvent="Default" Position="BottomRight" RelativeTo="Mouse" Width="100%" Height="250px" Title="Special Reference Range" />
                                                                                    </div>
                                                                                    <div class="col-md-3 PaddingLeftSpacing">
                                                                                        <asp:TextBox ID="txtFinalizedDislpayMachine" runat="server" Visible="false" Width="100%" autocomplete="off"></asp:TextBox>
                                                                                        <telerik:RadComboBox ID="ddlRange" runat="server" Visible="false" AutoPostBack="true" Width="100%" AppendDataBoundItems="true" MarkFirstMatch="true" OnSelectedIndexChanged="ddlRange_OnSelectedIndexChanged" EnableEmbeddedSkins="false"></telerik:RadComboBox>
                                                                                    </div>
                                                                                </div>




                                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                                                                    FilterType="Custom" TargetControlID="txtN" ValidChars="0123456789.=><+-*/\," />
                                                                                <asp:HiddenField ID="hdnUnitName" runat="server" Value='<%#Eval("UnitName")%>' />
                                                                                <asp:HiddenField ID="hdnUnitId" runat="server" Value='<%#Eval("UnitId")%>' />
                                                                                <asp:HiddenField ID="hdnMinValue" runat="server" Value='<%#Eval("MinValue")%>' />
                                                                                <asp:HiddenField ID="hdnMaxValue" runat="server" Value='<%#Eval("MaxValue")%>' />
                                                                                <asp:HiddenField ID="hdnSymbol" runat="server" Value='<%#Eval("Symbol")%>' />
                                                                                <asp:HiddenField ID="hdnMinPanicValue" runat="server" Value='<%#Eval("MinPanicValue")%>' />
                                                                                <asp:HiddenField ID="hdnMaxPanicValue" runat="server" Value='<%#Eval("MaxPanicValue")%>' />
                                                                                <asp:HiddenField ID="hdnEditFormulaField" runat="server" Value='<%#Eval("EditFormulaField")%>' />
                                                                                <asp:HiddenField ID="hdnScale" runat="server" Value='<%#Eval("Scale")%>' />
                                                                                <asp:HiddenField ID="hdnSpecialReferenceRange" runat="server" Value='<%#Eval("SpecialReferenceRange")%>' />
                                                                                <asp:HiddenField ID="hdnIsAddendum" runat="server" Value='<%#Eval("IsAddendum")%>' />
                                                                            </td>
                                                                        </tr>
                                                                    </table>




                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <asp:TextBox ID="txtFinalizedDislpay" runat="server" Visible="false" Width="100%"
                                                                                autocomplete="off"></asp:TextBox>
                                                                            <telerik:RadComboBox ID="D" runat="server" OnSelectedIndexChanged="D_OnClick" AutoPostBack="true"
                                                                                Visible="false" Width="100%" Skin="Outlook" AppendDataBoundItems="true" MarkFirstMatch="true">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                                                                </Items>
                                                                            </telerik:RadComboBox>
                                                                            <telerik:RadComboBox ID="O" runat="server" Visible="false" Width="100%"
                                                                                AppendDataBoundItems="true" MarkFirstMatch="true">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Text="" Value="0" />
                                                                                </Items>
                                                                            </telerik:RadComboBox>
                                                                        </div>
                                                                        <div class="col-md-6"></div>
                                                                    </div>


                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <asp:Button ID="btnSN" CssClass="btn btn-primary SensitivityBtn" runat="server" Text="Sensitivity" Visible="false" />
                                                                            <telerik:RadComboBox ID="E" runat="server" Visible="false" Width="100%"
                                                                                AppendDataBoundItems="true" MarkFirstMatch="true">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Text="[Select]" Value="0" />
                                                                                </Items>
                                                                            </telerik:RadComboBox>
                                                                        </div>
                                                                        <div class="col-md-6 PaddingLeftSpacing"></div>
                                                                    </div>



                                                                    <table id="tblOrganism" runat="server" visible="false">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="ddlMultilineFormatsOrganism" runat="server" OnSelectedIndexChanged="ddlMultilineFormatsOrganism_OnSelectedIndexChanged"
                                                                                    AutoPostBack="true" Width="200px" Skin="Outlook" AppendDataBoundItems="true"
                                                                                    Visible="false" MarkFirstMatch="true" BorderColor="Blue" BorderStyle="Solid"
                                                                                    BorderWidth="1">
                                                                                </telerik:RadComboBox>
                                                                                <asp:Label ID="lblMultilineOrg" runat="server" SkinID="label" Height="16px" />
                                                                                <asp:TextBox ID="txtMOrg" SkinID="textbox" runat="server" Width="550px" Height="60px"
                                                                                    onkeyup="return maxLength(this,500);" TextMode="MultiLine" Visible="false" BorderColor="Blue"
                                                                                    BorderStyle="Solid" BorderWidth="1" autocomplete="off" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>



                                                                    <asp:Repeater ID="C" runat="server" Visible="false">
                                                                        <HeaderTemplate>
                                                                            <table width="100%" cellpadding="2" cellspacing="0">
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr align="left">
                                                                                <td valign="top">
                                                                                    <asp:HiddenField ID="hdnCV" runat="server" Value='<%#Eval("ValueId")%>' />
                                                                                    <asp:CheckBox ID="C" SkinID="checkbox" Font-Size="10pt" runat="server" Text=' <%#Eval("ValueName")%>' />
                                                                                </td>
                                                                                <td align="right" visible="false" valign="top">
                                                                                    <textarea id="CT" class="Textbox" visible="false" runat="server" onkeypress="AutoChange()"
                                                                                        rows="1" cols="40" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            </table>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                    <telerik:RadComboBox ID="B" runat="server" Visible="false" OnSelectedIndexChanged="D_OnClick"
                                                                        AutoPostBack="true" Width="200px" CssClass="EnzymeDropDown" AppendDataBoundItems="true"
                                                                        MarkFirstMatch="true">
                                                                        <Items>
                                                                            <telerik:RadComboBoxItem Value="-1" Text="[Select]" />
                                                                            <telerik:RadComboBoxItem Value="0" Text="No" />
                                                                            <telerik:RadComboBoxItem Value="1" Text="Yes" />
                                                                        </Items>
                                                                    </telerik:RadComboBox>
                                                                    <asp:Label ID="lblD" runat="server" SkinID="label" Height="16px" Visible="false" />
                                                                    <table id="tblDate" runat="server" visible="false" cellpadding="2" cellspacing="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox ID="txtDate" SkinID="textbox" Font-Size="13px" Text="" Width="64px"
                                                                                    runat="server" autocomplete="off" />
                                                                            </td>
                                                                            <td>
                                                                                <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                    vspace="0" border="0" id="imgFromDate" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <AJAX:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate"
                                                                                    PopupButtonID="imgFromDate" />
                                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="_/" />
                                                                                <AJAX:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="True" TargetControlID="txtDate" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                    AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                    ErrorTooltipEnabled="false" InputDirection="LeftToRight" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table id="Tabletm" runat="server" visible="false">
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="ddlHr" runat="server" Width="40px" Skin="Outlook" AppendDataBoundItems="true"
                                                                                    MarkFirstMatch="true">
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem Value="0" Text="" />
                                                                                        <telerik:RadComboBoxItem Value="1" Text="1" />
                                                                                        <telerik:RadComboBoxItem Value="2" Text="2" />
                                                                                        <telerik:RadComboBoxItem Value="3" Text="3" />
                                                                                        <telerik:RadComboBoxItem Value="4" Text="4" />
                                                                                        <telerik:RadComboBoxItem Value="5" Text="5" />
                                                                                        <telerik:RadComboBoxItem Value="6" Text="6" />
                                                                                        <telerik:RadComboBoxItem Value="7" Text="7" />
                                                                                        <telerik:RadComboBoxItem Value="8" Text="8" />
                                                                                        <telerik:RadComboBoxItem Value="9" Text="9" />
                                                                                        <telerik:RadComboBoxItem Value="10" Text="10" />
                                                                                        <telerik:RadComboBoxItem Value="11" Text="11" />
                                                                                        <telerik:RadComboBoxItem Value="12" Text="12" />
                                                                                        <telerik:RadComboBoxItem Value="13" Text="13" />
                                                                                        <telerik:RadComboBoxItem Value="14" Text="14" />
                                                                                        <telerik:RadComboBoxItem Value="15" Text="15" />
                                                                                        <telerik:RadComboBoxItem Value="16" Text="16" />
                                                                                        <telerik:RadComboBoxItem Value="17" Text="17" />
                                                                                        <telerik:RadComboBoxItem Value="18" Text="18" />
                                                                                        <telerik:RadComboBoxItem Value="19" Text="19" />
                                                                                        <telerik:RadComboBoxItem Value="20" Text="20" />
                                                                                        <telerik:RadComboBoxItem Value="21" Text="21" />
                                                                                        <telerik:RadComboBoxItem Value="22" Text="22" />
                                                                                        <telerik:RadComboBoxItem Value="23" Text="23" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                                <asp:Label ID="Label2" runat="server" SkinID="label" Width="20px" Text="Hr" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="ddlMin" runat="server" Width="40px" Skin="Outlook" AppendDataBoundItems="true"
                                                                                    MarkFirstMatch="true">
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem Value="0" Text="" />
                                                                                        <telerik:RadComboBoxItem Value="1" Text="1" />
                                                                                        <telerik:RadComboBoxItem Value="2" Text="2" />
                                                                                        <telerik:RadComboBoxItem Value="3" Text="3" />
                                                                                        <telerik:RadComboBoxItem Value="4" Text="4" />
                                                                                        <telerik:RadComboBoxItem Value="5" Text="5" />
                                                                                        <telerik:RadComboBoxItem Value="6" Text="6" />
                                                                                        <telerik:RadComboBoxItem Value="7" Text="7" />
                                                                                        <telerik:RadComboBoxItem Value="8" Text="8" />
                                                                                        <telerik:RadComboBoxItem Value="9" Text="9" />
                                                                                        <telerik:RadComboBoxItem Value="10" Text="10" />
                                                                                        <telerik:RadComboBoxItem Value="11" Text="11" />
                                                                                        <telerik:RadComboBoxItem Value="12" Text="12" />
                                                                                        <telerik:RadComboBoxItem Value="13" Text="13" />
                                                                                        <telerik:RadComboBoxItem Value="14" Text="14" />
                                                                                        <telerik:RadComboBoxItem Value="15" Text="15" />
                                                                                        <telerik:RadComboBoxItem Value="16" Text="16" />
                                                                                        <telerik:RadComboBoxItem Value="17" Text="17" />
                                                                                        <telerik:RadComboBoxItem Value="18" Text="18" />
                                                                                        <telerik:RadComboBoxItem Value="19" Text="19" />
                                                                                        <telerik:RadComboBoxItem Value="20" Text="20" />
                                                                                        <telerik:RadComboBoxItem Value="21" Text="21" />
                                                                                        <telerik:RadComboBoxItem Value="22" Text="22" />
                                                                                        <telerik:RadComboBoxItem Value="23" Text="23" />
                                                                                        <telerik:RadComboBoxItem Value="24" Text="24" />
                                                                                        <telerik:RadComboBoxItem Value="25" Text="25" />
                                                                                        <telerik:RadComboBoxItem Value="26" Text="26" />
                                                                                        <telerik:RadComboBoxItem Value="27" Text="27" />
                                                                                        <telerik:RadComboBoxItem Value="28" Text="28" />
                                                                                        <telerik:RadComboBoxItem Value="29" Text="29" />
                                                                                        <telerik:RadComboBoxItem Value="30" Text="30" />
                                                                                        <telerik:RadComboBoxItem Value="31" Text="31" />
                                                                                        <telerik:RadComboBoxItem Value="32" Text="32" />
                                                                                        <telerik:RadComboBoxItem Value="33" Text="33" />
                                                                                        <telerik:RadComboBoxItem Value="34" Text="34" />
                                                                                        <telerik:RadComboBoxItem Value="35" Text="35" />
                                                                                        <telerik:RadComboBoxItem Value="36" Text="36" />
                                                                                        <telerik:RadComboBoxItem Value="37" Text="37" />
                                                                                        <telerik:RadComboBoxItem Value="38" Text="38" />
                                                                                        <telerik:RadComboBoxItem Value="39" Text="39" />
                                                                                        <telerik:RadComboBoxItem Value="40" Text="40" />
                                                                                        <telerik:RadComboBoxItem Value="41" Text="41" />
                                                                                        <telerik:RadComboBoxItem Value="42" Text="42" />
                                                                                        <telerik:RadComboBoxItem Value="43" Text="43" />
                                                                                        <telerik:RadComboBoxItem Value="44" Text="44" />
                                                                                        <telerik:RadComboBoxItem Value="45" Text="45" />
                                                                                        <telerik:RadComboBoxItem Value="46" Text="46" />
                                                                                        <telerik:RadComboBoxItem Value="47" Text="47" />
                                                                                        <telerik:RadComboBoxItem Value="48" Text="48" />
                                                                                        <telerik:RadComboBoxItem Value="49" Text="49" />
                                                                                        <telerik:RadComboBoxItem Value="50" Text="50" />
                                                                                        <telerik:RadComboBoxItem Value="51" Text="51" />
                                                                                        <telerik:RadComboBoxItem Value="52" Text="52" />
                                                                                        <telerik:RadComboBoxItem Value="53" Text="53" />
                                                                                        <telerik:RadComboBoxItem Value="54" Text="54" />
                                                                                        <telerik:RadComboBoxItem Value="55" Text="55" />
                                                                                        <telerik:RadComboBoxItem Value="56" Text="56" />
                                                                                        <telerik:RadComboBoxItem Value="57" Text="57" />
                                                                                        <telerik:RadComboBoxItem Value="58" Text="58" />
                                                                                        <telerik:RadComboBoxItem Value="59" Text="59" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                                <asp:Label ID="Label14" runat="server" SkinID="label" Text="Min" />
                                                                            </td>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="ddlSec" runat="server" Width="40px" Skin="Outlook" AppendDataBoundItems="true"
                                                                                    MarkFirstMatch="true">
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem Value="0" Text="" />
                                                                                        <telerik:RadComboBoxItem Value="1" Text="1" />
                                                                                        <telerik:RadComboBoxItem Value="2" Text="2" />
                                                                                        <telerik:RadComboBoxItem Value="3" Text="3" />
                                                                                        <telerik:RadComboBoxItem Value="4" Text="4" />
                                                                                        <telerik:RadComboBoxItem Value="5" Text="5" />
                                                                                        <telerik:RadComboBoxItem Value="6" Text="6" />
                                                                                        <telerik:RadComboBoxItem Value="7" Text="7" />
                                                                                        <telerik:RadComboBoxItem Value="8" Text="8" />
                                                                                        <telerik:RadComboBoxItem Value="9" Text="9" />
                                                                                        <telerik:RadComboBoxItem Value="10" Text="10" />
                                                                                        <telerik:RadComboBoxItem Value="11" Text="11" />
                                                                                        <telerik:RadComboBoxItem Value="12" Text="12" />
                                                                                        <telerik:RadComboBoxItem Value="13" Text="13" />
                                                                                        <telerik:RadComboBoxItem Value="14" Text="14" />
                                                                                        <telerik:RadComboBoxItem Value="15" Text="15" />
                                                                                        <telerik:RadComboBoxItem Value="16" Text="16" />
                                                                                        <telerik:RadComboBoxItem Value="17" Text="17" />
                                                                                        <telerik:RadComboBoxItem Value="18" Text="18" />
                                                                                        <telerik:RadComboBoxItem Value="19" Text="19" />
                                                                                        <telerik:RadComboBoxItem Value="20" Text="20" />
                                                                                        <telerik:RadComboBoxItem Value="21" Text="21" />
                                                                                        <telerik:RadComboBoxItem Value="22" Text="22" />
                                                                                        <telerik:RadComboBoxItem Value="23" Text="23" />
                                                                                        <telerik:RadComboBoxItem Value="24" Text="24" />
                                                                                        <telerik:RadComboBoxItem Value="25" Text="25" />
                                                                                        <telerik:RadComboBoxItem Value="26" Text="26" />
                                                                                        <telerik:RadComboBoxItem Value="27" Text="27" />
                                                                                        <telerik:RadComboBoxItem Value="28" Text="28" />
                                                                                        <telerik:RadComboBoxItem Value="29" Text="29" />
                                                                                        <telerik:RadComboBoxItem Value="30" Text="30" />
                                                                                        <telerik:RadComboBoxItem Value="31" Text="31" />
                                                                                        <telerik:RadComboBoxItem Value="32" Text="32" />
                                                                                        <telerik:RadComboBoxItem Value="33" Text="33" />
                                                                                        <telerik:RadComboBoxItem Value="34" Text="34" />
                                                                                        <telerik:RadComboBoxItem Value="35" Text="35" />
                                                                                        <telerik:RadComboBoxItem Value="36" Text="36" />
                                                                                        <telerik:RadComboBoxItem Value="37" Text="37" />
                                                                                        <telerik:RadComboBoxItem Value="38" Text="38" />
                                                                                        <telerik:RadComboBoxItem Value="39" Text="39" />
                                                                                        <telerik:RadComboBoxItem Value="40" Text="40" />
                                                                                        <telerik:RadComboBoxItem Value="41" Text="41" />
                                                                                        <telerik:RadComboBoxItem Value="42" Text="42" />
                                                                                        <telerik:RadComboBoxItem Value="43" Text="43" />
                                                                                        <telerik:RadComboBoxItem Value="44" Text="44" />
                                                                                        <telerik:RadComboBoxItem Value="45" Text="45" />
                                                                                        <telerik:RadComboBoxItem Value="46" Text="46" />
                                                                                        <telerik:RadComboBoxItem Value="47" Text="47" />
                                                                                        <telerik:RadComboBoxItem Value="48" Text="48" />
                                                                                        <telerik:RadComboBoxItem Value="49" Text="49" />
                                                                                        <telerik:RadComboBoxItem Value="50" Text="50" />
                                                                                        <telerik:RadComboBoxItem Value="51" Text="51" />
                                                                                        <telerik:RadComboBoxItem Value="52" Text="52" />
                                                                                        <telerik:RadComboBoxItem Value="53" Text="53" />
                                                                                        <telerik:RadComboBoxItem Value="54" Text="54" />
                                                                                        <telerik:RadComboBoxItem Value="55" Text="55" />
                                                                                        <telerik:RadComboBoxItem Value="56" Text="56" />
                                                                                        <telerik:RadComboBoxItem Value="57" Text="57" />
                                                                                        <telerik:RadComboBoxItem Value="58" Text="58" />
                                                                                        <telerik:RadComboBoxItem Value="59" Text="59" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                                <asp:Label ID="Label7" runat="server" SkinID="label" Text="Sec" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblTimeString" runat="server" SkinID="label" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks" Visible="false" ItemStyle-VerticalAlign="Top"
                                                                ItemStyle-Width="5%" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <textarea id="txtRemarks" class="Textbox" runat="server" onkeypress="AutoChange()"
                                                                        rows="1" cols="40" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ServiceId" HeaderText="Service ID" ItemStyle-Width="0%"
                                                                HeaderStyle-Width="0%" />
                                                            <asp:BoundField DataField="ShowAfterResultFinalization" HeaderText="" ItemStyle-Width="0%"
                                                                HeaderStyle-Width="0%" />
                                                            <asp:BoundField DataField="ShowTextFormatInPopupPage" HeaderText="" ItemStyle-Width="0%"
                                                                HeaderStyle-Width="0%" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="tvCategory" EventName="SelectedNodeChanged" />

                                            </Triggers>
                                        </asp:UpdatePanel>



                                    </div>

                                    <div class="col-md-6 col-sm-6">
                                        <asp:TextBox ID="hdnSelCell" runat="server" Text="0" Style="visibility: hidden;"
                                            autocomplete="off" />

                                    </div>
                                    <div class="col-md-6 col-sm-6">
                                        <asp:Label ID="hdnSentence" runat="server" Text="" Style="visibility: hidden;" />
                                        <asp:Button ID="btnCheck" Text="" SkinID="Button" OnClick="btnCheck_Onclick" runat="server"
                                            BackColor="Transparent" BorderColor="Transparent" BorderWidth="0" Height="0"
                                            Width="0" />
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-12 col-sm-8">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
                                            <ContentTemplate>
                                                <div class="row" id="tblSampleType" runat="server" visible="false">
                                                    <div class="col-md-12">
                                                        <div class="col-md-3 PaddingLeftSpacing01">
                                                            <asp:Label ID="Label10" runat="server" Text="Sample Type"></asp:Label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:DropDownList ID="ddlSampleType" runat="server" Width="100%"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2 text-left PaddingLeftSpacing">
                                                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>
                                </div>


                                <div class="row">
                                    <%--<div class="col-md-12 col-sm-12">
                                        <asp:UpdatePanel ID="update1" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server">
                                                    <Windows>
                                                        <telerik:RadWindow ID="rwPrintLabReport" runat="server" Behaviors="Close,Move" />
                                                    </Windows>
                                                </telerik:RadWindowManager>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>--%>
                                </div>

                                <div class="row">
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Button ID="btnTextFormatClose" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                            OnClick="btnTextFormatClose_OnClick" Width="1px" />
                                        <asp:HiddenField ID="hdnTextFormatData" runat="server" />
                                        <asp:HiddenField ID="hdnTotalTabIndexValue" runat="server" />
                                        <asp:Button ID="btnGetInfo" runat="server" Enabled="true" OnClick="btnGetInfo_Click"
                                            SkinID="button" Style="visibility: hidden;" Text="Assign" Width="10px" />
                                        <asp:HiddenField ID="hdnControlType" runat="server" />
                                    </div>

                                </div>


                            </div>


                        </div>
                    </div>



                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

</asp:Content>
