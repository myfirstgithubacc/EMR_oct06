<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DoctorProgressNote.aspx.cs" Inherits="EMR_Templates_DoctorProgressNote"
    Title="" %>

<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/TopPanelNew.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" type="text/css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/scrollbar.css" rel="stylesheet" />

    <script src="../../Scripts/jquery-1.7.1.min.js"></script>

    <!-- Bootstrap -->
    <script src="https://cdn.jsdelivr.net/npm/jquery@3.6.0/dist/jquery.slim.min.js"></script>
    <script src="../../Include/bootstrap4/js/bootstrap.min.js"></script>



    <style type="text/css">
        .btnCls {
            text-align:right;
        }
        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        #ctl00_ContentPlaceHolder1_ddlrange_Input {
            width: 100% !important;
        }
        /*#ctl00_ContentPlaceHolder1_ddlformat_Input {
            width: 100% !important;
        }*/
        .heightBox {
            min-height: 300px !important;
            max-height: 300px !important;
        }
        /*#ctl00_ContentPlaceHolder1_txtWProgressNote {min-height:350px !important; max-height:300px !important;height:350px !important;}*/

        #ctl00_ContentPlaceHolder1_lblMessage {
            float: none;
            margin: 0;
            padding: 0;
            width: 100%;
            position: relative;
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

        function executeCode(evt) {
            if (evt == null) {
                evt = window.event;
            }
            var theKey = parseInt(evt.keyCode, 10);
            switch (theKey) {
                case 114:  // F3
                    $get('<%=btnSaveProgressNote.ClientID%>').click();
                break;
        }
        evt.returnValue = false;
        return false;
    }

    function ShowPopup() {
        $("#MyPopup").modal("show");
    }
    function openModal() {
        $('#MyPopupAdendum').modal('show');
    }

    </script>

    <script type='text/javascript'>
        function OnClientEditorLoad(editor, args) {
            var style = editor.get_contentArea().style;
            style.fontFamily = 'Tahoma';
            style.fontSize = 11 + 'pt';
            var range = editor.getSelection().getRange(true); //returns an object that represents a restore point.
            editor.getSelection().selectRange(range);



            setTimeout(function () {
                var editor = $find("<%=txtWProgressNote.ClientID%>")

                var element = document.all ? editor.get_document().body : editor.get_document();
                $telerik.addExternalHandler(element, "keydown", function (e) {

                    stopAutoSaveWhileWorking();
                });


            }, 0);
        }
    </script>
    <script language="javascript" type="text/javascript">
        function nocopy() {
            if (document.getElementById('<%= hdnIsCopyCaseSheetAuthorized.ClientID %>').value == "False") {
                alert("This content cannot be copied!");
                return false;
            }
        }

        function clearProgressNote() {
            var editor = $find("<%=txtWProgressNote.ClientID%>"); //get a reference to RadEditor client object
            editor.set_html(""); //reset the content
        }
    </script>
    <script type="text/javascript">
        var editorObject;
        function OnClientModeChange(editor) {
            editorObject = editor;
            if ($telerik.isIE) {
                if (editor.get_mode() == 2) {
                    editor.get_textArea().attachEvent("onkeydown", trapTab);
                }
            }
            else editor.get_textArea().addEventListener("keydown", trapTab, false);
        }
        function trapTab(e) {

            if (e.keyCode == 9) {
                editorObject.pasteHtml(" ");
                $telerik.cancelRawEvent(e);
                return false;
            }
        }
    </script>

    <script type="text/javascript">
        var delay;
        function startDelay() {
            delay = setTimeout(callFinalAutoSave, 20000);
        }
        function resetDelay() {
            clearTimeout(delay);
        }
        function callFinalAutoSave() {

            if ('<%=common.GetKeyValue("AutoSave")%>' == '1') {
                setTimeout(function () {
                    $get('<%=btnAutoSave.ClientID%>').click();
                    console.log('Progress Note Saved');
                }, 1000);
            }

        }
        function stopAutoSaveWhileWorking() {
            resetDelay();
            startDelay();
        }

        //window.onload = function () {
        //    document.onchange = stopAutoSaveWhileWorking;

        //}

    </script>




    <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-lg-2 col-md-3 col-4 order-lg-1 order-1">
                        <h2>Doctor&nbsp;Progress&nbsp;Note</h2>
                    </div>
                    <div class="col-lg-2 col-md-3 col-6 order-lg-2 order-2 ">
                        <div class="row">
                            <div class="col-md-8 col-8">
                                <telerik:RadComboBox ID="ddlformat" runat="server" Width="100%" AutoPostBack="True" Text="Select Format" OnSelectedIndexChanged="ddlformat_SelectedIndexChanged" DropDownWidth="230px" />
                            </div>
                            <div class="col-md-4 col-4 no-p-l">

                                <asp:ImageButton runat="server" ImageUrl="../../Img/new-format.svg" Style="width: 20px; float: left; height: 20px; margin-right: 10px;" alt="" OnClick="btnShowPopup_Click1" data-toggle="modal" data-target="#MyPopup" ToolTip="Create New Progress Note Format" />




                                <%--<asp:Button ID="btnformatsave1" runat="server" Text="Save Format" CssClass="btn btn-primary" OnClick="btnformatsave_Click"
                                Width="95px" />--%>
                                <%--<img src="../../Img/save-format.svg" style="width: 20px;float:left;height: 20px;margin-right: 10px;" alt="" />--%> 
                                <asp:ImageButton runat="server" ImageUrl="../../Img/save-format.svg" style="width: 20px;height: 20px;float:left;" alt="Update Progress Note Format"  OnClick="UpdateFormat_Click" ToolTip="Update Format" />
<%--                                <img src="../../Img/new-format.svg" style="width: 20px;height: 20px;float:left;" alt="" /> --%>
                            </div>
                        </div>
                        
                    </div>

                    <div class="col-lg-3 order-lg-2  order-4 text-center">
                        <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                    </div>

                    <div class="col-lg-5 col-md-6 order-lg-4 order-3  text-right">
                        <asp:LinkButton ID="lnkBtnClinicalExamination" runat="server" Text="Clinical Examination" Font-Bold="true"
                            OnClick="lnkBtnClinicalExamination_OnClick" />

                        <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favourite" OnClick="btnAddToFavourite_Click"
                            CssClass="btn btn-primary" />



                        <asp:LinkButton ID="lnkAlerts" runat="server" Text="Patient Alert" Visible="false"
                            OnClick="lnkAlerts_OnClick" />

                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="New" OnClick="btnNew_OnClick" />

                        <asp:Button ID="btnSaveProgressNote" runat="server" CssClass="btn btn-primary" Text="Save" ToolTip="Save(Ctrl+F3)"
                            OnClick="btnSaveProgressNote_OnClick" />

                        <%--<asp:Button ID="btnSaveFormat" runat="server" CssClass="btn btn-primary" Text="SaveAs Format (Ctrl+F3)"
                                Font-Bold="true" Width="95px" OnClick="btnSaveFormat_Click" />--%>
                         <%--   <asp:Button ID="btnShowPopup" runat="server" Text="New Format" CssClass="btn btn-primary" OnClick="btnShowPopup_Click1"
                                Width="95px" />--%>

                      
                         <%--   <asp:Button ID="btnformatsave1" runat="server" Text="Save Format" CssClass="btn btn-primary" OnClick="btnformatsave_Click"
                                Width="95px" />--%>
                    </div>

                </div>

                <div class="row">
                    <aspl1:UserDetail ID="pd1" runat="server" />
                </div>
                <div class="row">
                    <div class="col-md-4 m-t">
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-6">
                                <div class="row">
                                    <div class="col-md-3 col-3">
                                        <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' />
                                    </div>
                                    <div class="col-md-9 col-9">
                                        <telerik:RadComboBox ID="ddlProvider" runat="server" TabIndex="0"
                                            Filter="Contains" Width="100%" Height="250px" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 col-6">
                                <div class="row">
                                    <div class="col-md-3 col-3">
                                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-9">
                                        <div class="row">
                                            <div class="col-md-9 col-8">
                                                <telerik:RadComboBox ID="ddlrange" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged" />
                                            </div>
                                            <div class="col-md-3 col-3">
                                                <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter"
                                                    OnClick="btnFilter_Click" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-6 mb-2" id="tblDateRange" runat="server">
                                <div class="row">
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-md-3 col-3">
                                                <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                                            </div>
                                            <div class="col-md-9 col-9">
                                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="row">
                                            <div class="col-md-3 col-3">
                                                <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                                            </div>
                                            <div class="col-md-9 col-9">
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12 col-6">
                                <div class="row">
                                    <div class="col-md-3 col-3" id="tblFavouriteSearch" runat="server">
                                        <asp:Label ID="Label24" runat="server" Text="Search" />
                                    </div>
                                    <div class="col-md-9 col-9">
                                        <asp:Panel ID="Panel6" runat="server" DefaultButton="btnSearchFavourite">
                                            <asp:TextBox ID="txtFavouriteItemName" runat="server" Width="100%" MaxLength="200" />
                                        </asp:Panel>
                                        <asp:Button ID="btnSearchFavourite" runat="server" CausesValidation="false" Style="visibility: hidden; display: none"
                                            Width="1px" OnClick="btnSearchFavourite_OnClick" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row p-t-b-5">
                            <div class="col-md-12 col-sm-12 gridview">
                                <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Height="210px" Width="100%" BorderColor="LightBlue" BorderWidth="1px" BorderStyle="Solid">
                                <asp:GridView ID="gvFav" runat="server" SkinID="gridview" Height="99%" Width="100%" AutoGenerateColumns="false"
                                    AlternatingRowStyle-BackColor="Beige" OnRowCommand="gvFav_OnRowCommand" OnRowDataBound="gvFav_OnRowDataBound"
                                    AllowPaging="true" PageSize="15" OnPageIndexChanging="gvFav_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Favourite Doctor Progress Note" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFavName" runat="server" Font-Size="9pt" Font-Bold="false"
                                                    CommandName="FAVLIST" Text='<%#Eval("Description")%>' />
                                                <asp:HiddenField ID="hdnFavouriteDPNId" runat="server" Value='<%#Eval("FavouriteDPNId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="45px" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete1" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                    ToolTip="Delete" Width="16px" Height="16px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            </div>
                        </div>
                    
                        </div>
                    <div class="col-md-8 m-t">
                        <div class="row">
                           <div class="col-md-12 col-sm-12">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <strong>
                                        <asp:Label ID="Label7" runat="server" Text="Current Progress Notes" /></strong>

                                        <telerik:RadEditor ID="txtWProgressNote" ToolbarMode="ShowOnFocus" Skin="Outlook" EnableResize="true"
                                            runat="server" CssClass="heightBox" Width="100%" ToolsFile="~/Include/XML/PrescriptionRTF.xml"
                                            OnClientLoad="OnClientEditorLoad" EditModes="Design" Height="300px">
                                        </telerik:RadEditor>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindow3" runat="server" Skin="Office2007" Behaviors="Close" InitialBehaviors="Maximize">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row m-t">
                    <div class="col-md-12 col-sm-12 gridview">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%" BorderColor="#ddd"
                                    BorderWidth="1px" ScrollBars="Auto">
                                    <asp:GridView ID="gvDoctorProgressNote" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="False"
                                        Width="100%" Height="100%" AllowPaging="true" PageSize="10" OnRowCommand="gvDoctorProgressNote_OnRowCommand"
                                        OnPageIndexChanging="gvDoctorProgressNote_OnPageIndexChanging" OnRowDataBound="gvDoctorProgressNote_OnRowDataBound">
                                        <EmptyDataTemplate>
                                            <div style="font-weight: bold; color: Red; vertical-align: top">
                                                No Record Found.
                                            </div>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText='Enc. #' HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                    <asp:HiddenField ID="hdnProgressNoteId" runat="server" Value='<%#Eval("ProgressNoteId") %>' />
                                                    <asp:HiddenField ID="hdnEncodedBy" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                    <asp:HiddenField ID="hdnProgressNote" runat="server" Value='<%#Eval("ProgressNote") %>' />
                                                    <asp:HiddenField ID="hdnProviderId" runat="server" Value='<%#Eval("ProviderId") %>' />
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Date' HeaderStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("EncounterDate")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Doctor%>' HeaderStyle-Width="180px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDoctor" runat="server" Width="98%" Text='<%#Eval("DoctorName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Previous Progress Notes'>
                                                <ItemTemplate>
                                                    <%--<telerik:RadEditor ID="editProgressNote" ToolbarMode="ShowOnFocus" EnableResize="true"
                                                        runat="server" Skin="Outlook" Height="70px" Width="100%" EditModes="Preview"
                                                        ToolsFile="~/Include/XML/BasicTools.xml" OnClientLoad="OnClientEditorLoad">
                                                    </telerik:RadEditor>--%>
                                                    <div style="overflow: auto; height: 50px;">
                                                        <asp:Label ID="lblProgressNote" runat="server" Width="98%" Text='<%#Eval("ProgressNote")%>' oncopy="return nocopy()" />
                                                    </div>
                                                    <%--<asp:TextBox ID="txtProgressNote" runat="server" Width="100%" Visible="false" SkinID="textbox"
                                                Text='<%#Eval("ProgressNote")%>' TextMode="MultiLine" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='View'>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDoctorNote" runat="server" Text='View'
                                                        CommandName="NoteView" CommandArgument="None" Visible="true" CausesValidation="false" Font-Underline="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Delete' HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                                        CommandName="PROGRESSNOTEDELETE" CausesValidation="false" CommandArgument='<%#Eval("ProgressNoteId")%>'
                                                        ImageUrl="~/Images/DeleteRow.png" Height="20px" Width="20px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Edit' HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" ToolTip="Click here to edit this record"
                                                        Text='Edit' CausesValidation="false" CommandName="ITEMSELECT" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Ritika(23-09-2022)Added Addendum--%>
                                             <asp:TemplateField HeaderText='Addendum' HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkAddendum" runat="server" ToolTip="Click here to Add Addendum"
                                                        Text='Add' CausesValidation="false" CommandName="ITEMADDENDUM" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText='Print' HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkPrint" runat="server" ToolTip="Click here to Print this record"
                                                        Text='Print' CausesValidation="false" CommandName="ITEMPRINT" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                        
                        <table id="tblProviderDetails" runat="server" visible="false">

                            <tr>
                                <td>
                                    <asp:Label ID="lblProvider" runat="server" Text="Provider" />
                                    <asp:Label ID="lblProviderStart" runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlRendringProvider" runat="server" EmptyMessage="[ Select ]"
                                        Height="250px" Width="250px" DropDownWidth="250px" Filter="Contains" />
                                </td>
                                <td>
                                    <asp:Label ID="lblChangeDate" runat="server" Text="Date" />
                                    <asp:Label ID="lblChangeDateStar" runat="server" Text="*" ForeColor="Red" />
                                </td>
                                <td>

                                    <telerik:RadDatePicker ID="dtpChangeDate" runat="server" MinDate="01/01/1870" DateInput-ReadOnly="true" Width="168px"></telerik:RadDatePicker>
                                    &nbsp;<asp:Literal ID="Literal1" runat="server" Text="Time"></asp:Literal>
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red" />
                                    <telerik:RadTimePicker ID="RadTimeFrom" runat="server" DateInput-ReadOnly="true"
                                        PopupDirection="TopLeft" TimeView-Columns="10" Width="95px" />
                                    <telerik:RadComboBox ID="ddlMinute" runat="server" AutoPostBack="True"
                                        Height="300px" Skin="Outlook" Width="50px"
                                        OnSelectedIndexChanged="ddlMinute_SelectedIndexChanged">
                                    </telerik:RadComboBox>
                                    &nbsp;<asp:Literal ID="ltDateTime" runat="server" Text="HH   MM"></asp:Literal>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                                <td colspan="2">
                                    <asp:Label ID="lblRange" runat="server" Text="*" Font-Bold="true" ForeColor="Red" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            <%--Progress Note--%>
            <div id="dvProgressNoteView" runat="server" class="container-fluid" visible="false" style="width: 1000px; height: 338px; left: 200px; top: 95px; bottom: 0; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background: #f2f6f8 repeat-x; position: absolute;">
                <div class="row">
                    <div class="container-fluid header_main">
                        <div class="col-md-10">
                            <asp:Label ID="lblResultHistoryPatientName" Font-Size="12px" runat="server" Font-Bold="true" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnResultHistoryClose" runat="server" CssClass="btn btn-primary" CausesValidation="false" Text="Close" OnClick="btnResultHistoryClose_OnClick" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div id="divScroll" style="overflow: auto; width: 1000px; height: 300px; border: solid; border-width: 1px;">
                        <asp:Label ID="lblProgressNoteView" runat="server" Text="" oncopy="return nocopy()" />
                    </div>
                </div>
            </div>

            <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindowPrint" Skin="Office2007"  runat="server" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upAutoSave" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnAutoSave" runat="server" Text="Auto Save" OnClick="btnAutoSave_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal Popup -->

    <div id="MyPopup" class="modal" role="dialog" style="z-index: 9999;">
        <div class="modal-dialog">
            <asp:UpdatePanel runat="server" ID="gridUpdate">
                <ContentTemplate>
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <div class="col-12">
                                <div class="row">
                                    <div class="col-md-8 col-sm-8 text-center">
                                        <asp:Label ID="lblsucc" runat="server" Text="&nbsp;" />
                                    </div>
                                    <div class="col-md-4 col-sm-4 text-right">
                                        <asp:Button ID="Button2" runat="server" OnClick="btnformatsave_Click" class="btn btn-info" Text="Save" />
                                        <asp:Button ID="Button3" runat="server" OnClick="btnclose_Click" Text="Close" class="btn btn-danger" data-dismiss="modal" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="modal-body">

                            <div class="col-md-12 col-sm-12" style="padding: 0px;">
                                <div class="row" style="padding: 0px 12px;">
                                    <div class="col-md-7 ">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-4 col-3 text-nowrap">
                                                <label for="name">Format Name:</label>
                                            </div>
                                            <div class="col-md-8 col-9">
                                                <asp:TextBox ID="txtFormatName" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <%--<div class="col-md-5 col-sm-5 box-col-checkbox p-t-b-5">
                                <asp:RadioButtonList ID="rdoSend" runat="server" RepeatDirection="Horizontal" Width="100%">
                                <asp:ListItem Text="For Me" Value="For Me" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="For All" Value="For All"></asp:ListItem>
                            </asp:RadioButtonList>
                            </div>--%>
                        </div>
                        <div class="modal-footer">
               <%-- <asp:Button ID="btnformatsave" runat="server" OnClick="btnformatsave_Click" class="btn btn-info btn-lg" Text="Save" />
                <asp:Button ID="btnclose" runat="server" OnClick="btnclose_Click" Text="Close" class="btn btn-danger" data-dismiss="modal" />--%>
                                    <div class="col-12">
                                        <div class="row p-t-b-5">

                                            <div id="dialog" class="gridview" style="display: block;">

                    <asp:GridView ID="GridViewFormat" runat="server" AutoGenerateColumns="false" Width="100%"
                        PageSize="10" AllowPaging="true" OnRowCommand="GridViewFormat_RowCommand" OnPageIndexChanging="GridViewFormat_PageIndexChanging">

                        <Columns>
                            <asp:BoundField DataField="FormatId" HeaderText="Format Id" ItemStyle-Width="150" Visible="false" />
                            <asp:BoundField DataField="FormatName" HeaderText="Format Name" ItemStyle-Width="98%" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="FormatText" HeaderText="Format Text" ItemStyle-Width="150" Visible="false" />
                            <asp:BoundField DataField="EmployeeId" HeaderText="Employee Id" ItemStyle-Width="150" Visible="false" />
                            <asp:TemplateField HeaderText='Delete' HeaderStyle-Width="5px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                        CommandName="FORMATPROGRESSNOTEDELETE" CausesValidation="false" CommandArgument='<%#Eval("FormatId")%>'
                                        ImageUrl="~/Images/DeleteRow.png" />
                                </ItemTemplate>
                            </asp:TemplateField>

                                                        <asp:TemplateField HeaderText='Edit' ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkformatEdit" runat="server" ToolTip="Click here to edit this record"
                                                                    Text='Edit' CausesValidation="false" CommandName="EDITFORMAT" CommandArgument='<%#Eval("FormatId").ToString()+";"+Eval("FormatName").ToString()+";"+Eval("FormatText").ToString()%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>

                                        </div>
                                    </div>
                                </div>

                                <div class="clearfix"></div>

                                <div class="clearfix"></div>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


</asp:Content>


