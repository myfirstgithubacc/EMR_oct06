<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="AddendumProgressNote.aspx.cs" Inherits="LIS_Phlebotomy_AddendumProgressNote" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script src="../../Scripts/jquery-1.7.1.min.js"></script>

    <!-- Bootstrap -->
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />

    <style type="text/css">
        .btnCls {
            text-align: right;
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

        div#ctl00_ContentPlaceHolder1_gvAddendum_GridData {
            height: 85vh !important;
            
        }
        /*#ctl00_ContentPlaceHolder1_ddlformat_Input {
            width: 100% !important;
        }*/
        .heightBox {
            /*min-height: 300px !important;
            max-height: 300px !important;*/
            height: 700px !important;
        }

        div#ctl00_ContentPlaceHolder1_RTF1 {
            height: 87.6vh !important;
        }

        table#ctl00_ContentPlaceHolder1_RTF1Wrapper {
            height: 87vh !important;
        }

        td#ctl00_ContentPlaceHolder1_txtWProgressNoteCenter {
            height: 86vh !important;
        }

        div#divScroll {
            height: 87.6vh !important;
        }

        div#ctl00_ContentPlaceHolder1_Panel2 {
            border: none !important;
        }

        tr.clsGridheaderorderNew th {
            background: #428bca;
            color: #fff;
            padding-left: 5px;
        }

        .header {
            background: #25a0da80;
            padding: 5px 0px;
            margin-bottom: 12px;
        }

        span#ctl00_ContentPlaceHolder1_lblMessage {
            width: 100% !important;
            float: none !important;
            margin: 0px !important;
        }

        .Outlook.reWrapper {
            border: solid 1px black !important;
        }

        div#RadWindowWrapper_ctl00_ContentPlaceHolder1_txtWProgressNote_toolbarMode {
            width: 84px !important;
            position: absolute;
            left: 120px !important;
            top: 32px !important;
            bottom: 0px !important;
            right: auto !important;
        }

            div#RadWindowWrapper_ctl00_ContentPlaceHolder1_txtWProgressNote_toolbarMode table {
                height: 0px !important;
            }

        tr.rwTitleRow {
            display: none !important;
        }

        td.rwCorner.rwBodyLeft {
            display: none !important;
        }

        td.rwCorner.rwBodyRight {
            display: none !important;
        }

        tr.rwStatusbarRow {
            display: none !important;
        }

        tr.rwFooterRow {
            display: none !important;
        }

        .RadWindow_Outlook td.rwWindowContent {
            background-color: #fff !important;
         }



        /*#ctl00_ContentPlaceHolder1_txtWProgressNote {min-height:350px !important; max-height:300px !important;height:350px !important;}*/
    </style>


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

                    //stopAutoSaveWhileWorking();
                });


            }, 0);
        }
        function clearProgressNote() {
            var editor = $find("<%=txtWProgressNote.ClientID%>"); //get a reference to RadEditor client object
            editor.set_html(""); //reset the content
        }
        function OnClientLoad(sender, args) {

            // Disable copying from HTML mode
            $telerik.addExternalHandler(sender.get_textArea(), "copy", function myfunction(ev) {
                alert("This content cannot be copied!");
                $telerik.cancelRawEvent(ev);
            });


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


    <%--<asp:ScriptManager ID="scriptmgr1" runat="server">
        </asp:ScriptManager>--%>
    <div class="container-fluid">
        <div class="row header">
            <div class="col-xs-4">
                <asp:UpdatePanel ID="UpdatePanel21" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />--%>
                        <asp:AsyncPostBackTrigger ControlID="gvAddendum" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-4">
                <asp:Label ID="lblPatientDetails" runat="server" SkinID="label" ForeColor="White" />
            </div>
            <div class="col-xs-4 text-right">
                <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary" ToolTip="New" OnClick="btnNew_OnClick"
                    Text="New" />
                <asp:Button ID="btnSaveData" runat="server" CssClass="btn btn-primary" ToolTip="Save"
                    Text="Save" OnClick="btnSaveData_Click" />
                <asp:Button ID="btnUpdateData" runat="server" CssClass="btn btn-primary" ToolTip="Update"
                    Text="Update" OnClick="btnSaveData_Click" Visible="false" />
                <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary"
                    OnClientClick="window.close();" />

            </div>
        </div>


    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-5">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">

                        <ContentTemplate>
                            <asp:Label ID="lbltypenm" runat="server" Text="Current Addendum" Font-Bold="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvAddendum" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />--%>
                    </Triggers>
                    <ContentTemplate>
                        <asp:HiddenField runat="server" ID="hdnAddendumID" />
                        <asp:HiddenField runat="server" ID="hdnProgressNoteID" />
                        <telerik:RadEditor ID="txtWProgressNote" ToolbarMode="ShowOnFocus" Skin="Outlook" EnableResize="true"
                            runat="server" Width="100%" ToolsFile="~/Include/XML/PrescriptionRTF.xml"
                            OnClientLoad="OnClientEditorLoad" EditModes="Design">
                        </telerik:RadEditor>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-5">
                <div>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">

                        <ContentTemplate>
                            <asp:Label ID="Label3" runat="server" Text="Previous Addendum" Font-Bold="true" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvAddendum" />
                        <%--<asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />--%>
                    </Triggers>
                    <ContentTemplate>
                        <div id="divScroll" style="overflow: auto; border: solid; border-width: 1px; background-color: #f9f9f9;">
                            <asp:Label ID="lblNoteView" runat="server" Text="" oncopy="return nocopy()" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-xs-2" style="margin-top:15px;">

                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" Width="99%" >

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" Width="100%" Height="100%" 
                                BorderWidth="1px" ScrollBars="Auto">
                                <asp:GridView ID="gvAddendum" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="False"
                                    Width="100%" Height="100%" AllowPaging="true" PageSize="10" OnItemCommand="gvAddendum_OnItemCommand"
                                    OnRowCommand="gvAddendum_OnRowCommand" BorderColor="Black" OnRowDataBound="OnRowDataBound">
                                    <EmptyDataTemplate>
                                        <div style="font-weight: bold; color: Red; vertical-align: top">
                                            No Record Found.
                                        </div>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText='Provider' HeaderStyle-Width="130px" >
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnAddendumId" runat="server" Value='<%#Eval("AddendumId") %>' />
                                                <asp:HiddenField ID="lblAddendumDate" runat="server" Value='<%#Eval("EncodedDate") %>' />
                                                <asp:HiddenField ID="lblAddendum" runat="server" Value='<%#Eval("Addendum") %>' />
                                                <%--<asp:LinkButton ID="lnkSelect" ToolTip="Click here to view this record" CommandName="Select" Text='<%#Eval("EncodedDate") %>' runat="server"></asp:LinkButton>--%>
                                                <asp:Label ID="lblEncodedBy" runat="server" Text='<%#Eval("EncodedBy")%>' />
                                                <span style="float: left;">&nbsp; </span>
                                                <br />
                                                <asp:Label ID="lblDate"  Text='<%#Eval("EncodedDate") %>' runat="server"></asp:Label>
                                                <span style="float: left;">&nbsp; </span>
                                                
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

                <%--  <div id="dvConfirmCancelOptions" runat="server" visible="false" style="width: 400px; z-index: 200; border: 1px solid #60AFC3; background-color: #A8D9E6; position: fixed; bottom: 35%; height: 85px; left: 38%;">
                                <table width="100%" cellspacing="2">
                                    <tr>
                                        <td colspan="3" align="center">
                                            <asp:Label ID="Label19" Style="font-size: 12px; font-weight: bold; margin: 0.5em 0 0; padding: 0; width: 100%; float: left;"
                                                runat="server" Text="Do you want to delete addendum?"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">&nbsp;
                                                </td>
                                    </tr>
                                    <tr>
                                        <td align="center"></td>
                                        <td align="center">
                                            <asp:Button ID="ButtonOk" CssClass="ICCAViewerBtn" runat="server" Text="Yes" OnClick="ButtonOk_OnClick" />
                                            &nbsp;
                                                               
                                                <asp:Button ID="ButtonCancel" CssClass="ICCAViewerBtn" runat="server" Text="No" OnClick="ButtonCancel_OnClick" />
                                        </td>
                                        <td align="center"></td>
                                    </tr>
                                </table>
                            </div>--%>
            </div>
        </div>
    </div>


</asp:Content>
