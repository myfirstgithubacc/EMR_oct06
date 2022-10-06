<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ProgressNote.aspx.cs" Inherits="WardManagement_ProgressNote"
    Title="" %>

<%@ Register TagPrefix="aspl1" TagName="UserDetail" Src="~/Include/Components/TopPanelNew.ascx" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        #ctl00_ContentPlaceHolder1_ddlrange_Input {
            width: 100% !important;
        }

        .heightBox {
            min-height: 300px !important;
            max-height: 300px !important;
        }
        /*#ctl00_ContentPlaceHolder1_txtWProgressNote {min-height:350px !important; max-height:300px !important;height:350px !important;}*/
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
    </script>

    <script type='text/javascript'>
        function OnClientEditorLoad(editor, args) {
            var style = editor.get_contentArea().style;
            style.fontFamily = 'Tahoma';
            style.fontSize = 11 + 'pt';
        }
    </script>
      <script language="javascript" type="text/javascript">
            function nocopy() {
                if (document.getElementById('<%= hdnIsCopyCaseSheetAuthorized.ClientID %>').value == "False") {
                    alert("This content cannot be copied!");
                    return false;
                }
            }
        </script>



      <asp:HiddenField ID="hdnIsCopyCaseSheetAuthorized" runat="server" />


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid header_main">
                <div class="col-md-3">
                    <h2>Nursing&nbsp;Notes</h2>
                </div>
                <div class="col-md-5 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" />
                </div>
                <div class="col-md-3 text-right pull-right"></div>
            </div>




            <div class="form-group">
                <aspl1:UserDetail ID="pd1" runat="server" />
            </div>
            <%--<asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>--%>

            <div class="clearfix"></div>

            <div class="container-fluid" style="margin-top: 10px;">

                <div class="row form-group">
                    <div class="col-md-4">
                        <div class="row">
                            <div class="col-md-2 label2">
                                <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' CssClass="margin_z" />
                            </div>
                            <div class="col-md-10">
                                <telerik:RadComboBox ID="ddlProvider" runat="server" TabIndex="0" Font-Size="11px"
                                    Filter="Contains" Width="100%" Height="250px" />
                            </div>
                        </div>
                    </div>


                    <div class="col-md-5">
                        <div class="row">

                            <div class="col-md-5">
                                <div class="row">
                                    <div class="col-md-3 label2">
                                        <asp:Label ID="lblDate" runat="server" Text="Date"></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <telerik:RadComboBox ID="ddlrange" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-7">
                                <div class="row" id="tblDateRange" runat="server">
                                    <div class="col-md-3 label2">
                                        <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="row">
                                            <div class="col-md-5 PaddingRightSpacing">
                                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                            </div>
                                            <div class="col-md-2 label2">
                                                <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                                            </div>
                                            <div class="col-md-5 PaddingLeftSpacing">
                                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100%" DateInput-ReadOnly="false" DatePopupButton-Visible="false" ShowPopupOnFocus="true"></telerik:RadDatePicker>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="col-md-3">
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Width="80px" Text="Filter"
                            OnClick="btnFilter_Click" />&nbsp;
                        <asp:LinkButton ID="lnkAlerts" runat="server" Text="Patient Alert" Visible="false"
                            OnClick="lnkAlerts_OnClick" />
                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="New" Font-Bold="true" OnClick="btnNew_OnClick" />
                        <asp:Button ID="btnSaveProgressNote" runat="server" CssClass="btn btn-primary" Text="Save (Ctrl+F3)" Font-Bold="true" OnClick="btnSaveProgressNote_OnClick" />

                    </div>
                </div>

            </div>

            <div class="form-group">
                <div class="col-md-8">
                    <div class="row form-group">
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
                                                    <%--<asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId")%>' />
                                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId")%>' />
                                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                                        <asp:HiddenField ID="hdnHospitalLocationId" runat="server" Value='<%#Eval("HospitalLocationId") %>' />
                                                        <asp:HiddenField ID="hdnEncodedDate" runat="server" Value='<%#Eval("EncodedDate") %>' />--%>
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
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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

                <div class="col-md-4">

                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <strong>
                                <asp:Label ID="Label7" runat="server" Text="Nursing Notes" /></strong>

                            <telerik:RadEditor ID="txtWProgressNote" ToolbarMode="ShowOnFocus" EnableResize="true"
                                runat="server" CssClass="heightBox" Width="100%" ToolsFile="~/Include/XML/PrescriptionRTF.xml"
                                OnClientLoad="OnClientEditorLoad" EditModes="Design" Height="300px">
                            </telerik:RadEditor>
                            <br />

                        </ContentTemplate>
                    </asp:UpdatePanel>


                    <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                            </telerik:RadWindow>
                        </Windows>
                    </telerik:RadWindowManager>
                </div>
            </div>

            <%--Progress Note--%>
            <div id="dvProgressNoteView" runat="server" class="container-fluid" visible="false" style="width: 510px; height: 290px; left: 200px; top: 95px; bottom: 0; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background: #f2f6f8 repeat-x; position: absolute;">
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
                    <div id="divScroll" style="overflow: auto; width: 500px; height: 250px; border: solid; border-width: 1px;">
                        <asp:Label ID="lblProgressNoteView" runat="server" Text=""  oncopy="return nocopy()" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
