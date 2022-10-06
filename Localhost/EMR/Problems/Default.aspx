<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="EMR_Problems_Default" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntChiefComplaint" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

      <style>
        div#RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew {
    top: 0 !important;
}
    </style>
    <script language="javascript" type="text/javascript">

        function AutoChange() {
            //
            var txt = $get('<%=txtSentenceGallery.ClientID%>');
            //alert(txt.value.length);
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
            if (txt.value.length > 2000) {
                alert("Text length should not be greater then 2000.");

                txt.value = txt.value.substring(0, 2000);
                txt.focus();
                //return false;

            }
            //txt.focus();
        }

        function GetCode(source, eventArgs) {
            $get('<%=prblmID.ClientID%>').value = eventArgs.get_value()

        }

        function openRadWindow(ID) {
            var oWnd = radopen("/EMR/Templates/SentanceGallery.aspx?ID=" + ID, "Radwindow1");
            oWnd.Height = "900";
            oWnd.Center();
        }
        function OnCloseSentenceGalleryRadWindow(oWnd, args) {
            $get('<%=btnSentence.ClientID%>').click();
            var arg = args.get_argument();
            if (arg) {
                var Sentence = arg.Sentence;
                var ctrl = arg.ControlId;
                ctrl = document.getElementById(ctrl);
                ctrl.value = Sentence;
            }
        }

        function cmbProblemName_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            var SnomedCode = item.get_attributes().getAttribute("SNOMEDCode");
            $get('<%=prblmID.ClientID%>').value = item.get_value();

        }
        function OnTextChange(sender) {
            $get('<%=txtSCTId.ClientID%>').value = '';
        }
        // Code Start for Quality Dropdown with Checkbox ------------------------
        //prevent default selection in the combobox and deselect all checkboxes
        var cancelDropDownClosing = false;
        function StopPropagation(e) {
            //cancel bubbling
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }
        window.onbeforeunload = function (evt) {
            var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            if (IsUnsave == 1) {
                return false;
            }
        }
        function on_ddlQualityLoad() {
            var combo = $find("<%= ddlQuality.ClientID %>");
            combo.clearSelection();
            var items = combo.get_items();
            for (var i = 0; i < items.get_count() ; i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combo.get_id() + "_i" + i + "_chk1");
                chk1.checked = false;
            }
        }
        function On_ddlQualityClosing() {
            cancelDropDownClosing = false;
        }

        function onCheckBoxClick(chk) {
            var combo = $find("<%= ddlQuality.ClientID %>");
            //prevent second combo from closing
            cancelDropDownClosing = true;
            //holds the text of all checked items
            var text = "";
            //holds the values of all checked items
            var values = "";
            //get the collection of all items
            var items = combo.get_items();
            //enumerate all items
            var j = 0;
            for (var i = 0; i < items.get_count() ; i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combo.get_id() + "_i" + i + "_chk1");
                if (chk1.checked) {
                    if (j < 5) {
                        text += item.get_text() + ",";
                        values += item.get_value() + ",";
                        j = j + 1;
                    }
                    else {
                        chk1.checked = false;
                    }
                }
            }
            //remove the last comma from the string
            text = removeLastComma(text);
            values = removeLastComma(values);
            if (text.length > 0) {
                //set the text of the combobox
                combo.set_text(text);
                $get('<%=txtQualityIds.ClientID%>').value = values;
            }
            else {
                //all checkboxes are unchecked
                //so reset the controls
                combo.set_text("");
                $get('<%=txtQualityIds.ClientID%>').value = '';
            }


        }

        //this method removes the ending comma from a string
        function removeLastComma(str) {
            return str.replace(/,$/, "");
        }
        function OnClientDropDownClosingHandler(sender, e) {
            //do not close the second combo if 
            //a checkbox from the first is clicked
            e.set_cancel(cancelDropDownClosing);
        }

        function CloseMe() {
            var combo = $find('<%= ddlQuality.ClientID %>');
            combo.hideDropDown();
        }

        function cmbAdd1_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%=txtAdditionalProblemId1.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
        }
        function cmbAdd2_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%=txtAdditionalProblemId2.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
        }
        function cmbAdd3_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%=txtAdditionalProblemId3.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
        }
        function cmbAdd4_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%=txtAdditionalProblemId4.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
        }
        function cmbAdd5_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();
            $get('<%=txtAdditionalProblemId5.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnIsUnSavedData.ClientID%>').value = 1;
        }

        function OnClientClosepopup(oWnd, args) {
            $get('<%=btnfind.ClientID%>').click();

        }

        function OnClientCloseSearch(oWnd, args) {
            $get('<%=btnGetComplaintSearchCodes.ClientID%>').click();
        }

    </script>

    <style type="text/css">
        body {overflow: hidden;}
        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        .style1 {
            width: 139px;
        }


       .ajax__tab_panel { height: 68vh; overflow-y: auto;} 

      
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row bg-info top-head-panel">
                <div class="col-xs-4 hidden"><h2>Patient&nbsp;Complaint(s)</h2></div>
                <div class="col-xs-4 text-center"></div>
                <div class="col-xs-8 text-right">
                    <asp:UpdatePanel ID="updSave" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary" CausesValidation="false"
                                Text="New" OnClick="btnNew_Click" />

                            <asp:Button ID="btnSentence" runat="server" OnClick="btnSentence_click" CausesValidation="false"
                                Text="SentenceGallary" Width="1px" Style="visibility: hidden; display: none" />
                            <asp:Button ID="btnSave" Visible="false" runat="server" OnClick="btnSave_Click" ValidationGroup="CreateGroup"
                                Text="Save" SkinID="button" />
                            <asp:TextBox ID="txtShowNote" runat="server" Width="50px" Style="visibility: hidden;" />
                            <asp:Button ID="btnClose" CssClass="btn btn-primary" Visible="false" runat="server" Text="Close"
                                OnClientClick="window.close();" />
                            <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                            <asp:Button ID="btnfind" runat="server" OnClick="btnfind_click" CausesValidation="false"
                                Text="Find" Width="1px" Style="visibility: hidden" /><asp:Button ID="btnHistory" runat="server" CssClass="btn btn-primary" CausesValidation="false"
                                    Text="Complaint(s) History" ToolTip="Complaint(s) History" OnClick="btnHistory_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                </div>
            </div>
            <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 well-sm  bg-warning">
                <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true" />
                    </div>
                </div>
            </div>
            <div class="text-center">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
            </div>

            <asp:RadioButtonList ID="rblBTN" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                OnSelectedIndexChanged="rblBTN_SelectedIndexChanged" Enabled="false" Visible="false">
                <asp:ListItem Text="All Problems" Value="ALL" Selected="True" />
                <asp:ListItem Text="Favorites" Value="FAV" />
            </asp:RadioButtonList>

            <asp:LinkButton ID="lnkAlerts" runat="server" Text="Patient Alert" OnClick="lnkAlerts_OnClick"
                Visible="false" />
            <div class="contentscroll-auto">
            <div id="dispage" runat="server">


                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3">

                            <div class="row form-group" style="margin-top: 10px;">
                             
                                <div class="col-xs-12">

                                    <div class="input-group">
    <span class="input-group-addon"><asp:Label ID="Label2" runat="server" Font-Bold="true" SkinID="label" Text="Search " /></span>
    <asp:Panel ID="pnlSearchFav" runat="server" DefaultButton="btnSearchFav">
                                        <asp:TextBox ID="txtSearchFav" runat="server" CssClass="form-control" style="border-radius: 0;" />
                                        <asp:Button ID="btnSearchFav" runat="server" Text="F" CssClass="btn btn-primary hide" OnClick="btnSearchFav_OnClick"
                                            Style="visibility: hidden;" Width="1px" />
                                    </asp:Panel>
  </div>

                             
      
 



                                
                                    
                                </div>
                            </div>
                            <div class="">
                                <asp:Panel ID="pnl" runat="server">
                                    <asp:GridView ID="gvFav" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                        AlternatingRowStyle-BackColor="Beige" OnRowCommand="gvFav_OnRowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Favourite Problems" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkFavName" runat="server" Font-Size="12px" Font-Bold="false"
                                                        CommandName="FAVLIST" Text='<%#Eval("ProblemDescription")%>' />
                                                    <asp:HiddenField ID="hdnProblemId" runat="server" Value='<%#Eval("ProblemId")%>' />
                                                    <asp:HiddenField ID="hdnSNOMEDCode" runat="server" Value='<%#Eval("SNOMEDCode") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete1" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                        ToolTip="Delete" Width="13px" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>

                            </div>


                        </div>
                        <hr />
                        <div class="col-md-5">

                            <table border="0" width="100%" style="margin-left: 5px" cellpadding="0" cellspacing="2">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProblem" SkinID="label" Text="Problem" Font-Bold="true" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="cmbProblemName" runat="server" MaxHeight="200px" AutoPostBack="false"
                                             Width="100%" EmptyMessage="Search by Text" DataTextField="ProblemDescription"
                                            DataValueField="ProblemId" EnableLoadOnDemand="true" HighlightTemplatedItems="true"
                                            ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnItemsRequested="cmbProblemName_OnItemsRequested"
                                            OnClientSelectedIndexChanged="cmbProblemName_OnClientSelectedIndexChanged">
                                            <HeaderTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>Condition/Symptom
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <%# DataBinder.Eval(Container, "Text")%>
                                                        </td>
                                                        <td id="Td1" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favourite" OnClick="btnAddToFavourite_Click"
                                            CssClass="btn btn-primary" style="margin-left: 2px;" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgUTD" runat="server" ImageUrl="~/Icons/UpToDate.jpg" ToolTip="Search Problem on UpToDate"
                                            Width="40px" Height="35px" OnClick="imgUTD_OnClick" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnRemovefromFavorites" runat="server" Text="Remove Favourite" CssClass="btn btn-primary"
                                            OnClick="btnRemovefromFavorites_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="hidden">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" Text="Complaint Search Keyword" SkinID="label" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlComplaintSearchCodes" runat="server" Width="170px" Skin="Outlook" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="ibtnComplaintSearchCode" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                        ToolTip="Add New Search Keyword" Height="18px" Width="18px" OnClick="ibtnComplaintSearchCode_Click"
                                                        Visible="true" CausesValidation="false" style="margin-left: 5px;" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnGetComplaintSearchCodes" runat="server" CausesValidation="false"
                                                        Style="visibility: hidden;" OnClick="btnGetComplaintSearchCodes_OnClick" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>

                            <asp:Panel ID="pnlDuration" runat="server" GroupingText="Duration" CssClass="head2-style">

                                <telerik:RadComboBox ID="ddlDuration" runat="server" AllowCustomText="true"
                                    MarkFirstMatch="true" Width="200px" MaxLength="50" Visible="false" />

                                <asp:ImageButton ID="ibtnDuration" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                    ToolTip="Add New Duration" Height="15px" Width="15px" OnClick="ibtnDuration_Click"
                                    Visible="false" CausesValidation="false" />


                                <asp:RadioButtonList ID="rdoDurationList" CssClass="radioo" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="1" Value="1" />
                                    <asp:ListItem Text="2" Value="2" />
                                    <asp:ListItem Text="3" Value="3" />
                                    <asp:ListItem Text="4" Value="4" />
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="6" Value="6" />
                                    <asp:ListItem Text="7" Value="7" />
                                    <asp:ListItem Text="8" Value="8" />
                                    <asp:ListItem Text="9" Value="9" />
                                    <asp:ListItem Text="10" Value="10" />
                                </asp:RadioButtonList>

                                <asp:RadioButtonList ID="ddlDurationType" runat="server" RepeatDirection="Horizontal"
                                    Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlDurationType_SelectedIndexChanged">
									<asp:ListItem Text="Hour(s)" Value="H" />
                                    <asp:ListItem Text="Day(s)" Value="D" />
                                    <asp:ListItem Text="Week(s)" Value="W" />
                                    <asp:ListItem Text="Month(s)" Value="M" />
                                    <asp:ListItem Text="Year(s)" Value="Y" />
                                    <asp:ListItem Text="Other(s)" Value="O" />
                                </asp:RadioButtonList>

                                <asp:CheckBox ID="chkChronics" runat="server" Text="Chronic" TextAlign="Right" Font-Bold="true" />
                                <asp:CheckBox ID="chkPrimarys" runat="server" Text="Primary" TextAlign="Right" Style="visibility: hidden" />


                                <asp:TextBox ID="txtDuration" runat="server" MaxLength="20" SkinID="textbox" Width="150px"
                                    Visible="false" />
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                    TargetControlID="txtDuration" FilterType="Custom, LowercaseLetters , UppercaseLetters"
                                    ValidChars="1234567890 " />

                            </asp:Panel>

                            <asp:LinkButton ID="lnkModeDetails" runat="server" Text="More Details" Font-Bold="true" OnClick="lnkModeDetails_OnClick" />

                            <asp:Button ID="btnAddtogrid" runat="server" CssClass="btn btn-primary pull-right" Font-Bold="true"
                                ValidationGroup="Problem" Text="Add To List" OnClick="btnAddtogrid_Click" />

                        </div>

                        <hr />
                        <div class="col-md-4">
                            <cc1:TabContainer ID="tbProblem" runat="server" ActiveTabIndex="0" Width="100%">
                                <cc1:TabPanel ID="tbpnlToday" runat="server" HeaderText="Today's Problems">
                                    <HeaderTemplate>
                                        Today's&nbsp;Problems
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlgrid" runat="server" Height="100%" Width="100%">
                                            <asp:GridView ID="gvProblemDetails" runat="server" AutoGenerateColumns="False" OnRowCommand="gvProblemDetails_RowCommand"
                                                OnRowDataBound="gvProblemDetails_RowDataBound" OnSelectedIndexChanged="gvProblemDetails_SelectedIndexChanged"
                                                SkinID="gridview" Width="100%" CssClass="text-normal">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%#Container .DataItemIndex +1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                                    <asp:TemplateField HeaderText="Problem Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProblemId" runat="server" Font-Bold="true" SkinID="label" Text='<%#Eval("ProblemId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Problem Name" ItemStyle-Wrap="true">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblProblem" runat="server" CommandName="HPI" Text='<%#Eval("ProblemDescription")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Is HPI">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblHPI" runat="server" CommandName="ISHPI" SkinID="label" Text='<%#Eval("IsHPI")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocation" runat="server" SkinID="label" Text='<%#Eval("Location")%>'
                                                                Visible="false" /><asp:Label ID="lblLocationId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("LocationId")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnset" runat="server" SkinID="label" Text='<%#Eval("Onset")%>'
                                                                Visible="false" /><asp:Label ID="lblOnsetId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("OnsetId")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duration">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDuration" runat="server" SkinID="label" Text='<%#Eval("Duration")%>' /><asp:Label
                                                                ID="lblDurationId" runat="server" SkinID="label" Text='<%#Eval("DurationId")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuality" runat="server" SkinID="label" Text='<%#Eval("Quality")%>'
                                                                Visible="false" /><asp:Label ID="lblQualityId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("QualityId1")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblContext" runat="server" SkinID="label" Text='<%#Eval("Context")%>'
                                                                Visible="false" /><asp:Label ID="lblContextId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("ContextId")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSeverity" runat="server" SkinID="label" Text='<%#Eval("Severity")%>'
                                                                Visible="false" /><asp:Label ID="lblSeverityId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("SeverityID")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID1" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID1")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem1" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem1")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID2" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID2")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem2" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem2")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID3" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID3")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem3" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem3")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID4" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID4")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem4" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem4")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID5" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID5")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem5" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem5")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProvider" runat="server" SkinID="label" Text='<%#Eval("DoctorId")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFacility" runat="server" SkinID="label" Text='<%#Eval("FacilityId")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrimary" runat="server" SkinID="label" Text='<%#Eval("IsPrimary")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChronic" runat="server" SkinID="label" Text='<%#Eval("IsChronic")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSCTId" runat="server" SkinID="label" Text='<%#Eval("SNOMEDCode")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSideDescription" runat="server" SkinID="label" Text='<%#Eval("SideDescription")%>'
                                                                Visible="false" />
                                                            <asp:Label ID="lblSide" runat="server" SkinID="label" Text='<%#Eval("Side")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCondition" runat="server" SkinID="label" Text='<%#Eval("Condition")%>'
                                                                Visible="false" /><asp:Label ID="lblConditionID" runat="server" SkinID="label"
                                                                    Text='<%#Eval("ConditionID")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True">
                                                        <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                    </asp:CommandField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" Width="13px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPercentage" runat="server" SkinID="label" Text='<%#Eval("Percentage")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" SkinID="label" Text='<%#Eval("ID")%>' Visible="false" />
                                                            <asp:HiddenField ID="hdnPDurations" runat="server" Value='<%#Eval("Durations") %>' />
                                                            <asp:HiddenField ID="hdnPDurationType" runat="server" Value='<%#Eval("DurationType") %>' />
                                                            <asp:HiddenField ID="hdnTemplateFieldId" runat="server" Value='<%#Eval("TemplateFieldId") %>' />
                                                            <asp:HiddenField ID="hdnComplaintSearchId" runat="server" Value='<%#Eval("ComplaintSearchId") %>' />
                                                            <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <RowStyle Wrap="False" />
                                            </asp:GridView>
                                        </asp:Panel>

                                     
                                        <div id="dvConfirmCancelOptions" runat="server" class="popup-delete-data">
                                            <strong>Delete?</strong>
                                            <br />
                                            <br />
                                            <div style="text-align: center;">
                                                <asp:Button ID="ButtonOk" runat="server" Text="Yes" CssClass="btn btn-success" OnClick="ButtonOk_OnClick" />
                                                <asp:Button ID="ButtonCancel" runat="server" Text="No"  CssClass="btn btn-danger" OnClick="ButtonCancel_OnClick" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tbpnlChronic" runat="server" HeaderText="Chronic Problems">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlChronicProblemDetails" runat="server" Width="100%" Height="100%">
                                            <asp:GridView ID="gvChronicProblemDetails" runat="server" AutoGenerateColumns="False"
                                                SkinID="gridview" Width="100%" OnRowDataBound="gvChronicProblemDetails_RowDataBound"
                                                OnRowCommand="gvChronicProblemDetails_RowCommand" OnSelectedIndexChanged="gvChronicProblemDetails_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%#Container .DataItemIndex +1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Id" />
                                                    <asp:TemplateField HeaderText="ProblemId">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProblemId" runat="server" Font-Bold="true" SkinID="label" Text='<%#Eval("ProblemId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Problem Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProblem" runat="server" SkinID="label" Text='<%#Eval("ProblemDescription")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLocation" runat="server" SkinID="label" Text='<%#Eval("Location")%>'
                                                                Visible="false" /><asp:Label ID="lblLocationId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("LocationId")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOnset" runat="server" SkinID="label" Text='<%#Eval("Onset")%>'
                                                                Visible="false" /><asp:Label ID="lblOnsetId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("OnsetId")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duration">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDuration" runat="server" SkinID="label" Text='<%#Eval("Duration")%>' /><asp:Label
                                                                ID="lblDurationId" runat="server" SkinID="label" Text='<%#Eval("DurationId")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quality">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuality" runat="server" SkinID="label" Text='<%#Eval("Quality")%>'
                                                                Visible="false" /><asp:Label ID="lblQualityId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("QualityId1")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Context">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblContext" runat="server" SkinID="label" Text='<%#Eval("Context")%>'
                                                                Visible="false" /><asp:Label ID="lblContextId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("ContextId")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Severity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSeverity" runat="server" SkinID="label" Text='<%#Eval("Severity")%>'
                                                                Visible="false" /><asp:Label ID="lblSeverityId" runat="server" SkinID="label"
                                                                    Text='<%#Eval("SeverityID")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID1" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID1")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem1" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem1")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID2" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID2")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem2" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem2")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID3" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID3")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem3" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem3")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID4" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID4")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem4" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem4")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssociatedProblemID5" runat="server" SkinID="label" Text='<%#Eval("AssociatedProblemID5")%>'
                                                                Visible="false" /><asp:Label ID="lblAssociatedProblem5" runat="server"
                                                                    SkinID="label" Text='<%#Eval("AssociatedProblem5")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProvider" runat="server" SkinID="label" Text='<%#Eval("DoctorId")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFacility" runat="server" SkinID="label" Text='<%#Eval("FacilityId")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrimary" runat="server" SkinID="label" Text='<%#Eval("IsPrimary")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblChronic" runat="server" SkinID="label" Text='<%#Eval("IsChronic")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectChronic" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSCTId" runat="server" SkinID="label" Text='<%#Eval("SNOMEDCode")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSideDescription" runat="server" SkinID="label" Text='<%#Eval("SideDescription")%>'
                                                                Visible="false" />
                                                            <asp:Label ID="lblSide" runat="server" SkinID="label" Text='<%#Eval("Side")%>' Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCondition" runat="server" SkinID="label" Text='<%#Eval("Condition")%>'
                                                                Visible="false">
                                                            </asp:Label><asp:Label ID="lblConditionID" runat="server" SkinID="label" Text='<%#Eval("ConditionID")%>'
                                                                Visible="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:CommandField CausesValidation="False" SelectText="Edit" ShowSelectButton="True">
                                                        <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                    </asp:CommandField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                ToolTip="Delete" Width="13px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPercentage" runat="server" SkinID="label" Text='<%#Eval("Percentage")%>'
                                                                Visible="false" />
                                                            <asp:HiddenField ID="hdnCDurations" runat="server" Value='<%#Eval("Durations") %>' />
                                                            <asp:HiddenField ID="hdnCDurationType" runat="server" Value='<%#Eval("DurationType") %>' />
                                                            <asp:HiddenField ID="hdnTemplateFieldId" runat="server" Value='<%#Eval("TemplateFieldId") %>' />
                                                            <asp:HiddenField ID="hdnComplaintSearchId" runat="server" Value='<%#Eval("ComplaintSearchId") %>' />
                                                            <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <RowStyle Wrap="False" />
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlbtnChronicToday" runat="server" Width="100%" HorizontalAlign="Right">                                           
                                        </asp:Panel>
                                        <div id="dvChronic" runat="server" class="popup-delete-data">
                                            <strong>Delete ?</strong>
                                            <br />
                                            <br />
                                            <div style="text-align: center;">
                                                <asp:Button ID="ButtonOk1" runat="server" Text="Yes" CssClass="btn btn-success" OnClick="ButtonOk1_OnClick" />
                                                <asp:Button ID="ButtonCancel1" runat="server" Text="No"  CssClass="btn btn-danger" OnClick="ButtonCancel1_OnClick" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                            <table border="0" width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td width="90%">
                                        <table runat="server" cellpadding="1" cellspacing="1" visible="false">
                                            <tr>
                                                <td>
                                                    <b>Remarks</b>&nbsp;<span style="color: Red;"> (Maximum character length is 2000.)</span>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSentenceGallery" Text="H" ToolTip="Sentence Gallery" runat="server"
                                                        CssClass="btn btn-primary" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtSentenceGallery" onkeyup="return AutoChange();" SkinID="textbox"
                                                        runat="server" TextMode="MultiLine" MaxLength="2000" Width="98%" Height="200px" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkPullForward" runat="server" Text="Pull Forward for Next Visit"
                                                        TextAlign="Right" Visible="false" />
                                                    &nbsp;<asp:Button ID="btnSaveRemarks" Text="Save&nbsp;Remarks" OnClick="btnSaveRemarks_OnClick"
                                                        ToolTip="Save Remarks" runat="server" BackColor="#FFCBA4" Font-Bold="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" align="left">
                                        <br />
                                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="650" Height="270"
                                            VisibleStatusbar="false" Top="0" Left="200" Behaviors="Close,Move" OnClientClose="OnCloseSentenceGalleryRadWindow"
                                            ReloadOnShow="true">
                                        </telerik:RadWindowManager>
                                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindowForNew" Skin="Metro"  runat="server" Behaviors="Close,Pin,Minimize,Maximize,Resize,Move" CssClass="medium-popup" style="width: 50vw !important;">
                                                </telerik:RadWindow>

                                            </Windows>
                                        </telerik:RadWindowManager>
                                        <asp:Label ID="lblUserName" runat="server" Visible="false" />
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </div>


            </div>
            <div id="divProblemDetails" runat="server" visible="false" style="width: 480px; z-index: 100; bottom:5%; border-bottom: 0px solid #000000; border-left: 0px solid #000000; background-color: White; border-right: 0px solid #000000; border-top: 0px solid #000000; position: absolute; height: 350px; left: 0; right: 0; margin: auto;  padding: 0;">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="border: 1px solid #ccc;">
                                    <table width="100%" cellpadding="1" cellspacing="1" class="table-popup" style="display:block;">
                                        <tr class="clsheader" style="height: 20px;">
                                            <td colspan="2">Problem Details
                                            </td>
                                            <td>
                                                <asp:Button ID="btnCloseDetail" runat="server" Text="Proceed" CssClass="btn btn-primary" OnClick="btnCloseDetail_OnClick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Location
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlLocation" runat="server" Skin="Outlook" AllowCustomText="true"
                                                    MarkFirstMatch="true" MaxLength="50" Width="200px">
                                                </telerik:RadComboBox>
                                               <asp:ImageButton ID="ibtnLocation" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                    ToolTip="Add New Location" Height="18px" Width="15px" OnClick="ibtnLocation_Click"
                                                    Visible="true" CausesValidation="false" />
                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Side
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlSide" runat="server" Skin="Outlook" AllowCustomText="true"
                                                    MarkFirstMatch="true" MaxLength="50" Width="200px">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="on the left" Value="L" />
                                                        <telerik:RadComboBoxItem Text="on the right" Value="R" />
                                                        <telerik:RadComboBoxItem Text="bilaterally" Value="B" />
														<telerik:RadComboBoxItem Text="on the center" Value="C" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Onset
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlOnset" runat="server" Skin="Outlook" AllowCustomText="true"
                                                    MarkFirstMatch="true" MaxLength="50" Width="200px">
                                                </telerik:RadComboBox>
                                                <asp:ImageButton ID="ibtnOnset" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                    ToolTip="Add New Onset" Height="18px" Width="15px" OnClick="ibtnOnset_Click"
                                                    Visible="true" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Quality
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlQuality" runat="server" Skin="Outlook" AllowCustomText="True"
                                                    HighlightTemplatedItems="true" OnClientDropDownClosed="On_ddlQualityClosing"
                                                    MarkFirstMatch="True" Width="200px">
                                                    <ItemTemplate>
                                                        <div onclick="StopPropagation(event)">
                                                            <asp:CheckBox runat="server" ID="chk1" Text='<%# Eval("Description") %>' onclick="onCheckBoxClick(this)" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="Label1" Width="300px" runat="server" Text="Select maximum five Quality"
                                                            onclick="CloseMe();" />
                                                    </FooterTemplate>
                                                </telerik:RadComboBox>
                                               
                                                <asp:ImageButton ID="ibtnQuality" runat="server" ImageUrl="~/Images/PopUp.jpg" ToolTip="Add New Quality"
                                                    Height="18px" Width="15px" OnClick="ibtnQuality_Click" Visible="true" CausesValidation="false" />
                                                <asp:TextBox ID="txtQualityIds" runat="server" Style="visibility: hidden;" Width="80"
                                                    SkinID="textbox" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Context
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlContext" runat="server" Skin="Outlook" Width="200px"
                                                    AllowCustomText="true" MarkFirstMatch="true" MaxLength="50">
                                                </telerik:RadComboBox>
                                               <asp:ImageButton ID="ibtnContext" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                    ToolTip="Add New Context" Height="18px" Width="15px" OnClick="ibtnContext_Click"
                                                    Visible="true" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Pain Score
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlSeverity" runat="server" Skin="Outlook" AllowCustomText="true"
                                                    MarkFirstMatch="true" MaxLength="50" Width="200px">
                                                </telerik:RadComboBox>
                                                &nbsp;<asp:ImageButton ID="ibtnSeverity" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                    ToolTip="Add New Severity" Height="18px" Width="15px" OnClick="ibtnSeverity_Click"
                                                    Visible="false" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Condition&nbsp;
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="ddlCondition" runat="server" Skin="Outlook" AllowCustomText="true"
                                                    MarkFirstMatch="true" MaxLength="50" Width="200px">
                                                </telerik:RadComboBox>
                                               <asp:ImageButton ID="ibtnCondition" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                    ToolTip="Add New Condition" Height="18px" Width="15px" OnClick="ibtnCondition_Click"
                                                    Visible="true" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>%age
                                            </td>
                                            <td align="left" colspan="2">
                                                <asp:TextBox ID="TxtPercentage" runat="server" SkinID="textbox" Font-Bold="true"
                                                    MaxLength="5" />
                                                <cc1:FilteredTextBoxExtender ID="percentage" runat="server" Enabled="True" TargetControlID="TxtPercentage"
                                                    FilterType="Custom, Numbers" ValidChars="." />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <%-- <asp:Label ID="Label3" runat="server" Text="  Associated Problems" />--%>
                                                <span style="font-weight: bold; color: #003399">Associated Problems</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Problemdsff
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="cmbAdd1" runat="server" Height="200px" Skin="Outlook" Width="250px"
                                                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                                                    EnableLoadOnDemand="true" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                    EnableVirtualScrolling="true" MaxLength="50" OnItemsRequested="cmbAdd1_OnItemsRequested"
                                                    OnClientSelectedIndexChanged="cmbAdd1_OnClientSelectedIndexChanged">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>Condition/Symptom
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                </td>
                                                                <td id="Td1" visible="false" runat="server">
                                                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnAdd1" runat="server" Text="+" OnClick="btnAdd1_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:TextBox ID="txtAdditionalProblemId1" runat="server" Style="visibility: hidden;"
                                                    Width="1px" SkinID="textbox" Text="0" />
                                            </td>
                                        </tr>
                                        <tr id="tr2" runat="server">
                                            <td>Problem
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="cmbAdd2" runat="server" Skin="Outlook" Height="200px" Width="250px"
                                                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                                                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                    EnableVirtualScrolling="true" OnItemsRequested="cmbAdd2_OnItemsRequested" OnClientSelectedIndexChanged="cmbAdd2_OnClientSelectedIndexChanged">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>Condition/Symptom
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                </td>
                                                                <td id="Td1" visible="false" runat="server">
                                                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnAdd2" runat="server" Text="+" OnClick="btnAdd2_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:Button ID="btnRemove2" runat="server" Text="-" OnClick="btnRemove2_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:TextBox ID="txtAdditionalProblemId2" runat="server" Style="visibility: hidden;"
                                                    Width="10" SkinID="textbox" Text="0" />
                                            </td>
                                        </tr>
                                        <tr id="tr3" runat="server">
                                            <td>Problem
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="cmbAdd3" runat="server" Skin="Outlook" Height="200px" Width="250px"
                                                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                                                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                    EnableVirtualScrolling="true" OnItemsRequested="cmbAdd3_OnItemsRequested" OnClientSelectedIndexChanged="cmbAdd3_OnClientSelectedIndexChanged">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>Condition/Symptom
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                </td>
                                                                <td id="Td1" visible="false" runat="server">
                                                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnAdd3" runat="server" Text="+" OnClick="btnAdd3_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:Button ID="btnRemove3" runat="server" Text="-" OnClick="btnRemove3_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:TextBox ID="txtAdditionalProblemId3" runat="server" Style="visibility: hidden;"
                                                    Width="10" SkinID="textbox" Text="0" />
                                            </td>
                                        </tr>
                                        <tr id="tr4" runat="server">
                                            <td>Problem
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="cmbAdd4" runat="server" Skin="Outlook" Height="200px" Width="250px"
                                                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                                                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                    EnableVirtualScrolling="true" OnItemsRequested="cmbAdd4_OnItemsRequested" OnClientSelectedIndexChanged="cmbAdd4_OnClientSelectedIndexChanged">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>Condition/Symptom
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                </td>
                                                                <td id="Td1" visible="false" runat="server">
                                                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnAdd4" runat="server" Text="+" OnClick="btnAdd4_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:Button ID="btnRemove4" runat="server" Text="-" OnClick="btnRemove4_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:TextBox ID="txtAdditionalProblemId4" runat="server" Style="visibility: hidden;"
                                                    Width="10" SkinID="textbox" Text="0" />
                                            </td>
                                        </tr>
                                        <tr id="tr5" runat="server">
                                            <td>Problem
                                            </td>
                                            <td align="left">
                                                <telerik:RadComboBox ID="cmbAdd5" runat="server" Skin="Outlook" Height="200px" Width="250px"
                                                    EmptyMessage="Search by Text" DataTextField="ProblemDescription" DataValueField="ProblemId"
                                                    EnableLoadOnDemand="true" MaxLength="50" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                                    EnableVirtualScrolling="true" OnItemsRequested="cmbAdd5_OnItemsRequested" OnClientSelectedIndexChanged="cmbAdd5_OnClientSelectedIndexChanged">
                                                    <HeaderTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>Condition/Symptom
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left">
                                                                    <%# DataBinder.Eval(Container, "Text")%>
                                                                </td>
                                                                <td id="Td1" visible="false" runat="server">
                                                                    <%# DataBinder.Eval(Container, "Attributes['SNOMEDCode']")%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </telerik:RadComboBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnRemove5" runat="server" Text="-" OnClick="btnRemove5_Click" CssClass="btn btn-primary"
                                                    Font-Bold="true" />
                                                <asp:TextBox ID="txtAdditionalProblemId5" runat="server" Style="visibility: hidden;"
                                                    Width="10" SkinID="textbox" Text="0" />
                                            </td>
                                        </tr>
                                        <tr id="trSnamedCode" runat="server" visible="false">
                                            <td>Snomed&nbsp;Code:&nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSCTId" runat="server" SkinID="textbox" Font-Bold="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Lablefacility" runat="server" SkinID="label" Text="Facility" Visible="false" />
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'
                                                    SkinID="label" Visible="false" />&nbsp;
                                                <telerik:RadComboBox ID="ddlProviders" runat="server" Skin="Outlook" Width="150px"
                                                    Visible="false" />
                                                <asp:RequiredFieldValidator ID="rfvddlProvider" runat="server" ControlToValidate="ddlProviders"
                                                    Display="None" ErrorMessage="Please select Provider." Text="*" ValidationGroup="Problem" />
                                                <asp:ValidationSummary ID="vsPageErrors" runat="server" ShowMessageBox="true" ShowSummary="false"
                                                    ValidationGroup="Problem" />
                                                <asp:TextBox ID="txtedit" runat="server" Style="visibility: hidden;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left" visible="false">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblshow" runat="server" Visible="false" Text="Show in Note" />
                                                        </td>
                                                        <td>
                                                            <asp:RadioButtonList ID="rblShowNote" Visible="false" runat="server" RepeatColumns="2">
                                                                <asp:ListItem Text="Yes" Value="1" Selected="True" />
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:HiddenField ID="hdnProblems" runat="server" />
            <asp:HiddenField ID="hdnRowIndex" runat="server" />
            <asp:HiddenField ID="prblmID" runat="server" />
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
