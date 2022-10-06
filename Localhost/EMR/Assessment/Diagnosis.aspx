<%@ Page Language="C#" Theme="DefaultControls" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="Diagnosis.aspx.cs" Inherits="EMR_Diagnosis_Default"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asp1" TagName="Top1" Src="~/Include/Components/TopPanel.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript">
        var ilimit = 60;
        function SearchPatientOnClientClose(oWnd, args) {
            $get('<%=btnfind.ClientID%>').click();
        }
        function AutoChange(txtRemarks) {
            var txt = document.getElementById(txtRemarks);

            if (txt.value.length >= 10) {
                if (txt.value.length >= 60 * txt.rows) {
                    txt.rows = txt.rows + 1;
                    ilimit = 0;
                }
                else if (txt.value.length < 60 * (txt.rows - 1)) {
                    txt.rows = Math.round(txt.value.length / 60) + 1;
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
            var combo = $find("<%= ddlDiagnosisStatus.ClientID %>");
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

            var combo = $find("<%= ddlDiagnosisStatus.ClientID %>");

            //            if (combo == null) {
            //                combo = chk;

            //            }
            //           alert(combo);
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
                    if (j < 3) {
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
                $get('<%=txtstatusIds.ClientID%>').value = values;
            }
            else {
                //all checkboxes are unchecked
                //so reset the controls
                combo.set_text("");
                $get('<%=txtstatusIds.ClientID%>').value = '';
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
            var combo = $find('<%= ddlDiagnosisStatus.ClientID %>');
            combo.hideDropDown();
        }


        function OnTextChange(sender) {
            $get('<%=txtIcdCodes.ClientID%>').value = '';
        }

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            var oWnd = GetRadWindow();
            oWnd.close(oArg);

        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

    </script>

    <script language="javascript" type="text/javascript">
        function ValidateDate(CDate, CCurDate) {
            var arrEnteredDate = new Array();
            arrEnteredDate = $get(CDate).value.split("/");
            var dateEnteredString = arrEnteredDate[2].toString() + "/" + arrEnteredDate[1].toString() + "/" + arrEnteredDate[0].toString();  // yyyy/MM/dd
            var myEnteredDate = new Date(dateEnteredString);

            var arrCurrentDate = new Array();
            arrCurrentDate = $get(CCurDate).value.split("/");
            var dateCurrentString = arrCurrentDate[2].toString() + "/" + arrCurrentDate[1].toString() + "/" + arrCurrentDate[0].toString();  // yyyy/MM/dd
            var myCurrentDate = new Date(dateCurrentString);

            if (myEnteredDate > myCurrentDate) {
                alert("Date Cannot Be Greater Than Current Date");
                $get(CDate).focus();
                return false;
            }
            else {
                return true;
            }
        }

        function OnClientClose(oWnd, args) {

            $get('<%=btnGetCondition.ClientID%>').click();
        }
        function ddlDiagnosiss_OnClientSelectedIndexChanged(sender, args) {
            var item = args.get_item();

            var DiagICDID = item.get_attributes().getAttribute("ICDID");
            var DiagICDCode = item.get_attributes().getAttribute("ICDCode");

            var DiagICDDIscription = item.get_attributes().getAttribute("ICDDescription");

            $get('<%=hdnDiagnosisId.ClientID%>').value = DiagICDID;
            $get('<%=txtIcdCodes.ClientID%>').value = DiagICDCode;

            $get('<%=hdnDiagnosisId.ClientID%>').value = item != null ? item.get_value() : sender.value();

            $get('<%=btnCommonOrder.ClientID%>').click();
            //alert('test');
        }
        //Added By Ashutosh Prashar:13/05/2013
        //Confirmation Message box before Delete Button Press.
        function alertBeforeDelete() {
            confirm('Do you want to delete!');
        }
        //End Here

        function SearchPatientOnClientClose() {
            //window.close();
        }

    </script>

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
                    $get('<%=btnAddtogrid.ClientID%>').click();
                break;

        }
        evt.returnValue = false;
        return false;
    }
    </script>
    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main">
                <div class="col-md-4 hidden"><h2>Patient Diagnosis(s)</h2></div>
                
                <div class="col-md-12 col-sm-12 col-xs-12 text-right">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            
                            <asp:Button ID="btnfind" runat="server" Text="Find" Style="visibility: hidden;" OnClick="btnfind_Click" />
                            <asp:Button ID="btnBackToSuperBill" runat="server" ToolTip="Back To Superbill" Text="Back To Superbill"
                                Width="1px" Visible="false" SkinID="Button" OnClick="btnBackToSuperBill_Click" />
                            <asp:Button ID="btnHistory" runat="server" CssClass="btn btn-primary" CausesValidation="false"
                                Text="Diagnosis(s) History" ToolTip="Diagnosis(s) History" OnClick="btnHistory_Click" />
                                <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                    </div>
                <div class="row text-center">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />

                </div>

           
                <div class="row">
                    <div class="col-md-4 col-sm-4 col-xs-12 m-t">
                        <div class="row p-t-b-5">
                            <div class="col-md-4 col-sm-4 co-xs-4 text-nowrap">
                                <asp:Label ID="Label2" runat="server" Text="Favourite Search" Font-Bold="true" SkinID="label" />
                            </div>
                            <div class="col-md-8">
                                <asp:TextBox ID="txtSearch" SkinID="textbox" Width="100%" runat="server" AutoPostBack="true" 
                                    OnTextChanged="txtSearch_OnTextChanged" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview m-t">
                                <div style="max-height:80vh; overflow-y: auto;">
                                <asp:GridView ID="gvFavourite" runat="server" AutoGenerateColumns="false" Width="100%"
                                    DataKeyNames="ICDID" GridLines="None" OnRowCommand="gvFavourite_RowCommand"
                                    >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Favourite" FooterStyle-Width="82%" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDescription" runat="server" Text='<%#Eval("ICDDescription")%>'
                                                    OnClick="lbtnName_Click" CommandName="Select" CommandArgument='<%#Eval("ICDID")%>' />
                                                <asp:HiddenField ID="hdnICDID" runat="server" Value='<%#Eval("ICDID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                    ToolTip="Delete" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblmsg" Text="No Rows Found." runat="server" />
                                        No Rows Found.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                    </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-8 col-sm-8 col-xs-12 m-t" id="idhistory" runat="server">
                        <div class="col-md-12 col-sm12 col-xs-12 bg-gray p-t-b-5" style="border:1px solid #f1f1f1;">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 col-xs-6">
                            <asp:LinkButton ID="lnkDiagnosisDetails" runat="server" Text="ICD Help" Font-Bold="true"
                                Font-Size="14px" OnClick="lnkDiagnosisDetails_Click" style="display: inline-block; margin: 10px 0 0 5px;" />
                        </div>

                        <div class="col-md-6 col-sm-6 col-xs-6 text-right">
                            <asp:Button ID="btnAddtogrid" runat="server" CssClass="btn btn-primary" Font-Bold="true"
                                                OnClick="btnAddtogrid_Click" Text="Add To List(F3)" style="margin: 10px 0;" />
                        </div>
                        </div>
                        
                        <asp:Panel ID="pnlSearch" runat="server" Width="100%">
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3">
                                            <asp:Literal ID="Literal1" runat="server" Text="Group" /></div>
                                        <div class="col-md-10 col-sm-10 col-xs-9">
                                            <telerik:RadComboBox ID="ddlCategory" runat="server" Width="100%"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3">
                                            <asp:Literal ID="Literal2" runat="server" Text="Sub&nbsp;Group" /></div>
                                        <div class="col-md-10 col-sm-10 col-xs-9">
                                            <telerik:RadComboBox ID="ddlSubCategory" runat="server" Width="100%"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div class="row form-group">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3">Diagnosis</div>
                                        <div class="col-md-10 col-sm-10 col-xs-9">
                                            <telerik:RadComboBox ID="ddlDiagnosiss" runat="server" Height="300px" Width="100%" DropDownWidth="300px"
                                                AutoPostBack="false" EmptyMessage="Search Diagnosis by Text"
                                                DataTextField="DISPLAY_NAME" DataValueField="DiagnosisId" EnableLoadOnDemand="true"
                                                HighlightTemplatedItems="true" ShowMoreResultsBox="true" OnItemsRequested="ddlDiagnosiss_OnItemsRequested"
                                                EnableVirtualScrolling="true" OnClientSelectedIndexChanged="ddlDiagnosiss_OnClientSelectedIndexChanged"
                                                OnClientTextChange="OnTextChange" style="width: 100% !important;">
                                                <HeaderTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>Diagnosis Display Name
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left">
                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                            </td>
                                                            <td id="Td1" visible="false" runat="server">
                                                                <%# DataBinder.Eval(Container, "Attributes['ICDID']")%>
                                                            </td>
                                                            <td id="Td2" visible="false" runat="server">
                                                                <%# DataBinder.Eval(Container, "Attributes['ICDCode']")%>
                                                            </td>
                                                            <td id="Td3" visible="false" runat="server">
                                                                <%# DataBinder.Eval(Container, "Attributes['ICDDescription']")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3"></div>
                                        <div class="col-md-4 col-sm-4 col-xs-4">
                                            <asp:Button ID="btnAddToFavourite" runat="server" Text="Add To Favourite" OnClick="btnAddToFavourite_Click"
                                                CssClass="btn btn-primary" />
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2 text-center">
                                            <asp:ImageButton ID="imgUTD" runat="server" ImageUrl="~/Icons/UpToDate.jpg" ToolTip="Search Diagnosis on UpToDate"
                                                OnClick="imgUTD_OnClick" Width="25px" Height="25px" />
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-xs-4">

                                            <asp:Button ID="btnExtenalEdu" runat="server" OnClick="btnExtenalEdu_Click" ValidationGroup="CreateGroup"
                                                ToolTip="External Education" Text="Education Materials" CssClass="btn btn-primary" />

                                            <asp:Button ID="btnCommonOrder" runat="server" Style="visibility: hidden; display: none" OnClick="btnCommonOrder_OnClick"
                                                Width="1px" />
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </asp:Panel>



                        <asp:Panel ID="pnlDiagnoSearch" runat="server" DefaultButton="btnSearchICDCode">



                            <div class="row">
                                <div id="icdcode" runat="server">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-2 col-sm-2 col-xs-3">ICD&nbsp;Code</div>
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <div class="row">
                                                    <div class="col-md-8 col-sm-8 col-xs-8">
                                                        <asp:TextBox ID="txtIcdCodes" runat="server" CssClass="Textbox" Width="100%" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                         <asp:Button ID="btnSearchICDCode" runat="server" Text="Search By ICD" CssClass="btn btn-primary"
                                                    OnClick="btnSearchICDCode_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="row p-t-b-5">
                                            <div class="col-md-2 col-sm-2 col-xs-3">Onset&nbsp;Date</div>
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <telerik:RadDatePicker ID="rdpOnsetDate" runat="server" MinDate="01/01/1900" Width="100%"
                                                    DateInput-ReadOnly="false">
                                                </telerik:RadDatePicker>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                            </div>
                            <div class="row">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3">Location</div>
                                        <div class="col-md-10 col-sm-10 col-xs-9">
                                            <telerik:RadComboBox ID="ddlSides" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="" Value="0" runat="server" />
                                                    <telerik:RadComboBoxItem Text="Left" Value="1" runat="server" />
                                                    <telerik:RadComboBoxItem Text="Right" Value="2" runat="server" />
                                                    <telerik:RadComboBoxItem Text="Front" Value="3" runat="server" />
                                                    <telerik:RadComboBoxItem Text="Back" Value="4" runat="server" />
                                                    <telerik:RadComboBoxItem Text="All over" Value="5" runat="server" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3">Type</div>
                                        <div class="col-md-10 col-sm-10 col-xs-9">
                                            <telerik:RadComboBox ID="ddlDiagnosisType" runat="server" Width="100%"
                                                AutoPostBack="false">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>
                                </div>





                            </div>
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-2 col-sm-2 col-xs-3">Condition</div>
                                        <div class="col-md-10 col-sm-10 col-xs-9">
                                            <telerik:RadComboBox ID="ddlDiagnosisStatus" runat="server" AllowCustomText="true"
                                                HighlightTemplatedItems="true" OnClientDropDownClosed="On_ddlQualityClosing"
                                                AutoPostBack="false" MarkFirstMatch="True" Width="95%">
                                                <ItemTemplate>
                                                    <div onclick="StopPropagation(event)">
                                                        <asp:CheckBox runat="server" ID="chk1" Text='<%# Eval("Description") %>' onclick="onCheckBoxClick(this)" />
                                                    </div>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="Label1" Width="160px" runat="server" Text="Maximum Three." onclick="CloseMe();" />
                                                </FooterTemplate>
                                            </telerik:RadComboBox>
                                            <asp:ImageButton ID="ibtnReferredBy" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                ToolTip="Add New Condition" Height="18px" Width="15px" OnClick="ibtnReferredBy_Click"
                                                Visible="true" CausesValidation="false" CssClass="add-popup-icon" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-7 col-sm-7 col-xs-12 p-t-b-5 box-col-checkbox " style="font-weight: normal!important; font-size: 11px!important;">
                                    <asp:CheckBox ID="chkPrimarys" runat="server" Text="Primary" />

                                    <asp:CheckBox ID="chkChronics" runat="server" Text="Chronic" />


                                    <asp:CheckBox ID="chkQuery" runat="server" Text="Provisional" />

                                    <asp:CheckBox ID="chkResolve" runat="server" Text="Resolved" />

                                    <asp:CheckBox ID="chkIsFinalDiagnosis" runat="server" Text="Final Diagnosis" />
                                </div>
                               
                            </div>

                                    <div class="row p-t-b-5">
                                        <div class="col-md-1 col-sm-1 col-xs-3">Remarks</div>
                                        <div class="col-md-11 col-sm-11 col-xs-9">
                                            <asp:TextBox ID="txtcomments" runat="server" SkinID="textbox" MaxLength="100" Width="100%"
                                                TextMode="MultiLine" />
                                        </div>
                                    </div>
                                
                            <div class="row form-group">
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <asp:Label ID="Lablefacility" runat="server" SkinID="label" Text="Facility" Visible="false" /></div>
                                        <div class="col-md-8">
                                            <telerik:RadComboBox ID="ddlFacility" runat="server" OnSelectedIndexChanged="ddlFacility_SelectedIndexChanged"
                                                Width="100%" AutoPostBack="true" Visible="false">
                                            </telerik:RadComboBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <asp:Label ID="Lable1" runat="server" Text='<%$ Resources:PRegistration, Doctor%>'
                                                    SkinID="label" Visible="false" />
                                            </div>
                                            <div class="col-md-8">
                                                <telerik:RadComboBox ID="ddlProviders" runat="server" Width="100%"
                                                    DropDownWidth="200px" AutoPostBack="false" Visible="false">
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>

                                </div>


                            </div>
                        </asp:Panel>
                        
                        <div class="row m-t">
                            <AJAX:TabContainer ID="tbDiagnosis" runat="server" ActiveTabIndex="0" Width="100%">
                                <AJAX:TabPanel ID="tbpnlToday" runat="server" HeaderText="Today's Diagnosis">
                                    <HeaderTemplate>
                                        Today's Diagnosis
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlgrid" runat="server" CssClass="gridview" Width="100%">
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvDiagnosisDetails" runat="server" AutoGenerateColumns="false"
                                                        HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDiagnosisDetails_RowCommand"
                                                        OnRowDataBound="gvDiagnosisDetails_RowDataBound" SkinID="gridview" OnSelectedIndexChanged="gvDiagnosisDetails_SelectedIndexChanged"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Diagnosis" ItemStyle-Wrap="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                                    <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                                    <asp:HiddenField ID="hdnIsFinalDiagnosis" runat="server" Value='<%#Eval("IsFinalDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Side">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Primary" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Chronic">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>' />
                                                                    <asp:HiddenField ID="hdnIsQuery" runat="server" Value='<%#Eval("IsQuery") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Condition">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resolved">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Provider">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Onset Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>' />
                                                                    <asp:DropDownList ID="ddlProvider" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                        Visible="false">
                                                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="HdnOnsetDate" runat="server" Value='<%#Eval("OnsetDateWithoutFormat") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                                    <asp:DropDownList ID="ddlLocation" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                        Visible="false">
                                                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                                SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                        ToolTip="Delete" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                    <table border="0" cellpadding="0" cellspacing="2">
                                                        <tr>
                                                            <td>
                                                                <div id="dvConfirmDeletion" runat="server" visible="false" class="popup-delete-data">
                                                                    <table width="100%" cellspacing="2" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="3" align="center">
                                                                                <asp:HiddenField ID="hdnlblId" runat="server" Value="0" />
                                                                                <strong>Delete?</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            
                                                                            <td align="center" colspan="3">
                                                                                <asp:Button ID="btnYes" CssClass="btn btn-success" runat="server" Text="Yes" OnClick="btnDeletion_OnClick" />
                                                                                <asp:Button ID="btnNo" CssClass="btn btn-danger" runat="server" Text="No" OnClick="btnCancelDeletion_OnClick" />
                                                                            </td>
                                                                           
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </AJAX:TabPanel>
                                <AJAX:TabPanel ID="tbpnlChronic" runat="server" HeaderText="Chronic Diagnosis">
                                    <HeaderTemplate>
                                        Chronic Diagnosis
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlgvChronic" runat="server" CssClass="gridview" Width="100%">
                                            <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvChronicDiagnosis" runat="server" AutoGenerateColumns="false"
                                                        HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvChronicDiagnosis_RowCommand"
                                                        OnRowDataBound="gvChronicDiagnosis_RowDataBound" SkinID="gridview" OnSelectedIndexChanged="gvChronicDiagnosis_SelectedIndexChanged"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ICD&nbsp;Code" ItemStyle-Width="80" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Diagnosis">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                                    <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Side">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Primary" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Chronic">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resolved">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Provider">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Onset Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectChronic" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                                SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" />
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                        ToolTip="Delete" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                    <table border="0" cellpadding="0" cellspacing="2">
                                                        <tr>
                                                            <td>
                                                                <div id="dvChronicConfirmDeletion" runat="server" visible="false" style="width: 200px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 70px; left: 430px; top: 170px">
                                                                    <table width="100%" cellspacing="2" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="3" align="center">
                                                                                <asp:HiddenField ID="hdnChroniclblId" runat="server" Value="0" />
                                                                                <strong>Delete?</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center"></td>
                                                                            <td align="center">
                                                                                <asp:Button ID="btnChronicYes" SkinID="Button" runat="server" Text="Yes" OnClick="btnChronicDeletion_OnClick" />
                                                                                &nbsp;
                                                                                                        <asp:Button ID="btnChronicNo" SkinID="Button" runat="server" Text="No" OnClick="btnChronicCancelDeletion_OnClick" />
                                                                            </td>
                                                                            <td align="center"></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </AJAX:TabPanel>

                               <%-- //yogesh provisionAL 28-09-2022--%>
                                <AJAX:TabPanel ID="TabPanel1" runat="server" HeaderText="Provisional Diagnosis">
                                    <HeaderTemplate>
                                        Provisional Diagnosis
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="gridview" Width="100%">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvProvisionalDiag" runat="server" AutoGenerateColumns="false"
                                                        HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDiagnosisDetails_RowCommand"
                                                        OnRowDataBound="gvDiagnosisDetails_RowDataBound" SkinID="gridview" OnSelectedIndexChanged="gvDiagnosisDetails_SelectedIndexChanged"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Provisional Diagnosis" ItemStyle-Wrap="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                                    <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                                    <asp:HiddenField ID="hdnIsFinalDiagnosis" runat="server" Value='<%#Eval("IsFinalDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Side">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Primary" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Chronic">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>' />
                                                                    <asp:HiddenField ID="hdnIsQuery" runat="server" Value='<%#Eval("IsQuery") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Condition">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resolved">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Provider">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Onset Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>' />
                                                                    <asp:DropDownList ID="ddlProvider" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                        Visible="false">
                                                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="HdnOnsetDate" runat="server" Value='<%#Eval("OnsetDateWithoutFormat") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                                    <asp:DropDownList ID="ddlLocation" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                        Visible="false">
                                                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                                SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                        ToolTip="Delete" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                    <table border="0" cellpadding="0" cellspacing="2">
                                                        <tr>
                                                            <td>
                                                                <div id="Div1" runat="server" visible="false" class="popup-delete-data">
                                                                    <table width="100%" cellspacing="2" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="3" align="center">
                                                                                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                                                <strong>Delete?</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            
                                                                            <td align="center" colspan="3">
                                                                                <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Yes" OnClick="btnDeletion_OnClick" />
                                                                                <asp:Button ID="Button2" CssClass="btn btn-danger" runat="server" Text="No" OnClick="btnCancelDeletion_OnClick" />
                                                                            </td>
                                                                           
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </AJAX:TabPanel>

                                <%-- //yogesh Resolved 28-09-2022--%>
                                <AJAX:TabPanel ID="TabPanel2" runat="server" HeaderText="Resolved Diagnosis">
                                    <HeaderTemplate>
                                        Resolved Diagnosis
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel2" runat="server" CssClass="gridview" Width="100%">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView ID="ResolvedGrid" runat="server" AutoGenerateColumns="false"
                                                        HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvDiagnosisDetails_RowCommand"
                                                        OnRowDataBound="gvDiagnosisDetails_RowDataBound" SkinID="gridview" OnSelectedIndexChanged="gvDiagnosisDetails_SelectedIndexChanged"
                                                        Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIcdId" runat="server" Text='<%#Eval("ICDID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ICD Code" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Provisional Diagnosis" ItemStyle-Wrap="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("ICDDescription") %>' />
                                                                    <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedBy") %>' />
                                                                    <asp:HiddenField ID="hdnIsFinalDiagnosis" runat="server" Value='<%#Eval("IsFinalDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Side">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSide" runat="server" Text='<%#Eval("LocationId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Primary" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrimary" runat="server" Text='<%#Eval("PrimaryDiagnosis") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Chronic">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblChronic" runat="server" Text='<%#Eval("IsChronic") %>' />
                                                                    <asp:HiddenField ID="hdnIsQuery" runat="server" Value='<%#Eval("IsQuery") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlType" runat="server" Text='<%#Eval("TypeId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Condition">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlStatus" runat="server" Text='<%#Eval("ConditionIds") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Resolved">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResolved" runat="server" Text='<%#Eval("IsResolved") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Provider">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlProvider" runat="server" Text='<%#Eval("DoctorId") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Onset Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOnsetDate" runat="server" Text='<%#Eval("OnsetDate") %>' />
                                                                    <asp:DropDownList ID="ddlProvider" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                        Visible="false">
                                                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="HdnOnsetDate" runat="server" Value='<%#Eval("OnsetDateWithoutFormat") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblddlLocation" runat="server" Text='<%#Eval("FacilityId") %>' />
                                                                    <asp:DropDownList ID="ddlLocation" runat="server" SkinID="DropDown" AppendDataBoundItems="true"
                                                                        Visible="false">
                                                                        <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComments" runat="server" Text='<%#Eval("Remarks") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ControlStyle-Font-Underline="true"
                                                                SelectText="Edit" CausesValidation="false" ShowSelectButton="true" HeaderText="Edit"
                                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" />
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                                HeaderStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDelete" runat="server" Width="16px" CommandName="Del" ImageUrl="~/Images/DeleteRow.png"
                                                                        ToolTip="Delete" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                    <table border="0" cellpadding="0" cellspacing="2">
                                                        <tr>
                                                            <td>
                                                                <div id="Div2" runat="server" visible="false" class="popup-delete-data">
                                                                    <table width="100%" cellspacing="2" cellpadding="0">
                                                                        <tr>
                                                                            <td colspan="3" align="center">
                                                                                <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                                                                                <strong>Delete?</strong>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            
                                                                            <td align="center" colspan="3">
                                                                                <asp:Button ID="Button3" CssClass="btn btn-success" runat="server" Text="Yes" OnClick="btnDeletion_OnClick" />
                                                                                <asp:Button ID="Button4" CssClass="btn btn-danger" runat="server" Text="No" OnClick="btnCancelDeletion_OnClick" />
                                                                            </td>
                                                                           
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </AJAX:TabPanel>


                            </AJAX:TabContainer>
                            <asp:CheckBox ID="chkPullDiagnosis" runat="server" Text="Pull Forward From Prior Exam"
                                TextAlign="Right" Visible="false" />
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" Skin="Metro" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                            <asp:TextBox ID="hdn_DRUG_SYN_ID" runat="server" Style="visibility: hidden;" Width="30px" />
                            <asp:TextBox ID="hdn_DRUG_ID" runat="server" Style="visibility: hidden;" Width="30px" />
                            <asp:TextBox ID="hdn_GENPRODUCT_ID" runat="server" Style="visibility: hidden;" Width="30px" />
                            <asp:TextBox ID="hdn_SYNONYM_TYPE_ID" runat="server" Style="visibility: hidden;"
                                Width="30px" />
                            <asp:TextBox Style="visibility: hidden;" ID="lbl_DISPLAY_NAME" Width="30" runat="server" />
                            <asp:HiddenField ID="hdnProblems" runat="server" />
                            <asp:HiddenField ID="hdnRowIndex" runat="server" />
                            <asp:TextBox ID="txtid" runat="server" Style="visibility: hidden; position: absolute;" />
                            <asp:TextBox ID="txtIcdId" runat="server" Style="visibility: hidden; position: absolute;" />
                            <asp:HiddenField ID="hdnDiagnosisId" runat="server" />
                            <asp:HiddenField ID="hdnGenericId" runat="server" />
                            <asp:HiddenField ID="hdnCurrentRowId" runat="server" />
                            <asp:TextBox ID="txtstatusIds" runat="server" Style="visibility: hidden; position: absolute;"
                                SkinID="textbox" />
                            <asp:Button ID="btnGetCondition" runat="server" CausesValidation="false" Style="visibility: hidden;"
                                OnClick="btnGetCondition_Click" />

                        </div>
                    </div>
                        </div>
                </div>
            </div>



            <div id="divExcludedService" runat="server" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFF8DC; position: absolute; bottom: 0; height: 140px; left: 270px; top: 200px;">
                <table cellspacing="2" cellpadding="2" width="400px">
                    <tr>
                        <td colspan="2" style="width: 100%; text-align: center;">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblExcludedService" runat="server" Text="Selected Diagnosis is excluded for the payer."
                                            ForeColor="#990066" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="  Do you wish to continue ? " ForeColor="#990066" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; text-align: center;" colspan="2">
                            <asp:Button ID="btnExcludedService" runat="server" Text="Proceed" OnClick="btnExcludedService_OnClick"
                                SkinID="Button" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnExcludedServiceCancel" runat="server" Text="Cancel" OnClick="btnExcludedServiceCancel_OnClick"
                                SkinID="Button" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
