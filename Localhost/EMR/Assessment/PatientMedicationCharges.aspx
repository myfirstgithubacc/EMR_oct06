<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientMedicationCharges.aspx.cs"
    EnableViewState="true" Inherits="EMR_Assessment_PatientMedicationCharges" Title="" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">
            function ShowICDPanel(ctrlPanel, txt1) {
                var ICDarr = new Array();
                var txt = document.getElementById('<%=txtICDCode.ClientID%>');
                ICDarr = txt.value.split(',');

                var ICDCodes = '';
                var tt = document.getElementById(ctrlPanel);
                tt.style.visibility = 'visible';
                var tableElement = document.getElementById('rptrICDCodes');
                if (tableElement != null) {
                    for (var i = 0; i < tableElement.rows.length; i++) {
                        var rowElem = tableElement.rows[i];
                        var col = rowElem.cells[0].childNodes[0];
                        col.checked = false;
                    }

                    for (var i = 0; i < tableElement.rows.length; i++) {
                        var rowElem = tableElement.rows[i];
                        var col = rowElem.cells[0].childNodes[0];
                        var chklabel = rowElem.cells[0].childNodes[1];
                        for (var j = 0; j < ICDarr.length; j++) {
                            if (chklabel.innerText == ICDarr[j]) {
                                col.checked = true;
                            }
                        }
                    }
                }
            }

            function HideICDPanel(ctrlname) {
                var tt = document.getElementById(ctrlname);
                tt.style.visibility = 'hidden';
            }

            function cmbDrugList_OnClientSelectedIndexChanged(sender, args) {
                var item = args.get_item();

                var GenPId = item.get_attributes().getAttribute("DRUG_SYN_ID");
                //var DrugSId = item.get_attributes().getAttribute("DRUG_ID");

                $get('<%=hdn_DRUG_SYN_ID.ClientID%>').value = GenPId;
                //$get('<%=hdn_DRUG_ID.ClientID%>').value = DrugSId;

                $get('<%=hdn_GENPRODUCT_ID.ClientID%>').value = item != null ? item.get_value() : sender.value();
                $get('<%=lbl_DISPLAY_NAME.ClientID%>').value = item != null ? item.get_text() : sender.value();
            }
            
        </script>

        <style type="text/css">
            .style1
            {
                width: 100%;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smDiagnosis" runat="server">
    </asp:ScriptManager>
    <div>
        <table cellpadding="1" cellspacing="1" border="0" width="100%">
            <tr>
                <td width="750px" class="clssubtopicbar" align="right" valign="Middle" style="background-color: #E0EBFD;
                    padding-right: 10px;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lbl_Msg" runat="server" ForeColor="Green" Font-Bold="true" Style="padding-right: 10px;"></asp:Label>
                            <asp:LinkButton ID="lnkENMCodes" runat="server" CausesValidation="false" Text="E&M Codes"
                                Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkENMCodes_OnClick"></asp:LinkButton>&nbsp;|&nbsp;
                            <asp:LinkButton ID="lnkDiagnosis" runat="server" CausesValidation="false" Text="CPT Coding"
                                Font-Bold="true" Font-Underline="true" onmouseover="LinkBtnMouseOver(this.id);"
                                onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkDiagnosis_OnClick"></asp:LinkButton>

                            <script language="JavaScript" type="text/javascript">
                                function LinkBtnMouseOver(lnk) {
                                    document.getElementById(lnk).style.color = "red";
                                }
                                function LinkBtnMouseOut(lnk) {
                                    document.getElementById(lnk).style.color = "blue";
                                }
                            </script>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="rboName" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvDrug" />
                        </Triggers>
                        <ContentTemplate>
                            <div style="float: left">
                                <asp:RadioButtonList TabIndex="1" ID="rbonamefilter" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbonamefilter_SelectedIndexChanged" RepeatDirection="Horizontal"
                                    Font-Bold="true" CausesValidation="false">
                                    <asp:ListItem Selected="True" Text="Both" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Brand" Value="60"></asp:ListItem>
                                    <asp:ListItem Text="Generic" Value="59"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div style="float: right;">
                               <div style="float:right;">
                         <asp:Button ID="btnNew" runat="server" SkinID="Button" Text="New" TabIndex="6" OnClick="btnNew_Click" CausesValidation="false" />
                         <button id="img1" runat="server" class="buttonBlue" onclick="javascript:window.close();">
                                    Close</button>
                    </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td width="400px">
                            <asp:UpdatePanel ID="upRx" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvDrug" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button ID="btn_AllRx" TabIndex="2" runat="server" SkinID="Button" OnClick="btnAllRx_OnClick"
                                        Text="All Rx" CausesValidation="false" />
                                    <asp:Button ID="btn_PastRx" TabIndex="3" runat="server" SkinID="Button" OnClick="btnPastRx_OnClick"
                                        Text="Past Rx" CausesValidation="false" />
                                    <asp:Button ID="btn_PastPatientRx" TabIndex="4" runat="server" SkinID="Button" OnClick="btnPastPatientRx_OnClick"
                                        Text="Past Patient Rx" CausesValidation="false" />
                                    <asp:Button ID="btn_FavouriteRx" TabIndex="5" runat="server" SkinID="Button" OnClick="btn_FavouriteRx_OnClick"
                                        Text="Favorite Rx" CausesValidation="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvDrug" />
                                </Triggers>
                                <ContentTemplate>
                                    <telerik:RadComboBox ID="ddlOption" TabIndex="8" runat="server" AllowCustomText="True"
                                        MarkFirstMatch="true" Width="100px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Anywhere" Value="0" Selected="True" />
                                            <telerik:RadComboBoxItem Text="Starts With" Value="1" Selected="False" />
                                            <telerik:RadComboBoxItem Text="Ends With" Value="2" Selected="False" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <asp:TextBox ID="txtSearch" runat="server" Columns="26" CssClass="Textbox" SkinID="textbox"></asp:TextBox>&nbsp;
                                    <asp:Button ID="btnSearch" TabIndex="10" Text="Search" Width="64px" runat="server"
                                        OnClick="btnSearch_Click" SkinID="Button" CausesValidation="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width:99%">
                    <asp:UpdatePanel ID="upDrug" runat="server">
                        <ContentTemplate>
                            <telerik:RadGrid ID="gvDrug" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                PageSize="5" Skin="Office2007" Width="99%" AllowSorting="False" AllowMultiRowSelection="False"
                                AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                GridLines="none" OnItemDataBound="gvDrug_OnRowDataBound" OnPageIndexChanged="gvDrug_PageIndexChanging"
                                OnSelectedIndexChanged="gvDrug_SelectedIndexChanged">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                </ClientSettings>
                                <MasterTableView Width="100%">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Master List of Drug(s)" HeaderStyle-Width="500">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_DISPLAY_NAME" ToolTip='<%#Eval("DISPLAY_NAME") %>' runat="server"
                                                    Text='<%#Eval("DISPLAY_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnJCode" runat="server" Value='<%#Eval("JCode") %>' />
                                                <asp:HiddenField ID="hdnDRUG_ID" runat="server" Value='<%#Eval("DRUG_ID") %>' />
                                                <asp:HiddenField ID="hdnGENPRODUCT_ID" runat="server" Value='<%#Eval("GENPRODUCT_ID") %>' />
                                                <asp:HiddenField ID="hdnDRUG_SYN_ID" runat="server" Value='<%#Eval("DRUG_SYN_ID") %>' />
                                                <asp:HiddenField ID="hdnGENERIC_NAME" runat="server" Value='<%#Eval("GENERIC_NAME") %>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_RX_OTC_STATUS" ToolTip='<%#Eval("RX_OTC_STATUS") %>' runat="server"
                                                    Text='<%#Eval("RX_OTC_STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridButtonColumn Text="Select" CommandName="Select" ItemStyle-Width="100px"
                                            HeaderText="Select">
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
                                    <Selecting AllowRowSelect="True"></Selecting>
                                    <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                                        ResizeGridOnColumnResize="False"></Resizing>
                                </ClientSettings>
                            </telerik:RadGrid>
                            <%--<asp:GridView ID="gvDrug" runat="server" SkinID="gridview" Width="100%" ShowFooter="false"
                                ShowHeader="true" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                                AllowSorting="false" OnPageIndexChanging="gvDrug_PageIndexChanging" OnRowDataBound="gvDrug_OnRowDataBound"
                                OnSelectedIndexChanged="gvDrug_SelectedIndexChanged">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="No Record Found." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DRUG_ID" runat="server" Text='<%#Eval("DRUG_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_GENPRODUCT_ID" runat="server" Text='<%#Eval("GENPRODUCT_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DRUG_SYN_ID" runat="server" Text='<%#Eval("DRUG_SYN_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Master List of Drug(s)">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DISPLAY_NAME" ToolTip='<%#Eval("DISPLAY_NAME") %>' runat="server"
                                                Text='<%#Eval("DISPLAY_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RX_OTC_STATUS" ToolTip='<%#Eval("RX_OTC_STATUS") %>' runat="server"
                                                Text='<%#Eval("RX_OTC_STATUS") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_GENERIC_NAME" Visible="false" runat="server" Text='<%#Eval("GENERIC_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" SelectText="Select"
                                        CausesValidation="false" ShowSelectButton="true" />
                                    <asp:BoundField DataField="JCode" HeaderText="JCode" />
                                </Columns>
                            </asp:GridView>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td class="DetailHeader">
                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblInfoDrug" Text="Drug :" runat="server" Visible="false"></asp:Label>
                            <asp:TextBox Style="visibility: hidden;" ID="lbl_DISPLAY_NAME" Width="30" runat="server"></asp:TextBox>&nbsp;<asp:Label
                                ID="lblInfoTopGebericName" Visible="false" Text="Generic :" runat="server"></asp:Label>
                            <asp:Label ID="lbl_GENERIC_NAME" Width="313px" runat="server" Font-Bold="true"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="2" width="100%">
                        <tr>
                            <td width="80px">
                                Drug Name<span class="style1">*</span>
                            </td>
                            <td colspan="7">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                       <telerik:RadComboBox ID="cmbDrugList" runat="server" Height="300px" Width="385px"
                                            EmptyMessage="Search Drug by Text" DataTextField="DISPLAY_NAME" DataValueField="GENPRODUCT_ID"
                                            HighlightTemplatedItems="true" ShowMoreResultsBox="true" EnableLoadOnDemand="true"
                                            EnableVirtualScrolling="true" OnItemsRequested="cmbDrugList_OnItemsRequested"
                                            CausesValidation="false" OnDataBound="cmbDrugList_DataBound"
                                            OnClientSelectedIndexChanged="cmbDrugList_OnClientSelectedIndexChanged"
                                            
                                            AutoPostBack="true">
                                            <HeaderTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            Drug Display Name
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
                                                            <%# DataBinder.Eval(Container, "Attributes['hdnDRUG_SYN_ID']")%>
                                                        </td>
                                                        <td id="Td2" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['DRUG_ID']")%>
                                                        </td>
                                                        <td id="Td3" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['SYNONYM_TYPE_ID']")%>
                                                        </td>
                                                        <td id="Td4" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['ROUTE_ID']")%>
                                                        </td>
                                                        <td id="Td5" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['ROUTE_DESCRIPTION']")%>
                                                        </td>
                                                        <td id="Td6" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['DOSEFORM_ID']")%>
                                                        </td>
                                                        <td id="Td7" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['DOSEFORM_DESCRIPTION']")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                A total of
                                                <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                items
                                            </FooterTemplate>
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                NDC Code<span class="style1">*</span>
                            </td>
                            <td colspan="7">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbDrugList" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="cmbNDCList" runat="server" Height="300px" Width="385px"
                                            EmptyMessage="Search NDC by TradeName or Id" DataTextField="DISPLAY_NAME" DataValueField="PKG_PRODUCT_ID"
                                            EnableLoadOnDemand="true" HighlightTemplatedItems="true" ShowMoreResultsBox="true"
                                            EnableVirtualScrolling="true" OnItemsRequested="cmbNDCList_OnItemsRequested"
                                         
                                            CausesValidation="false" AutoPostBack="true">
                                            <HeaderTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            Drug Display Name
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
                                                            <%# DataBinder.Eval(Container, "Attributes['PKG_PRODUCT_ID']")%>
                                                        </td>
                                                        <td id="Td2" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['GENERIC_PRODUCT_NAME']")%>
                                                        </td>
                                                        <td id="Td3" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['JCODE']")%>
                                                        </td>
                                                        <td id="Td4" visible="false" runat="server">
                                                            <%# DataBinder.Eval(Container, "Attributes['LABELER_NAME']")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ICD<span class="style1">*</span>
                            </td>
                            <td colspan="7">
                                <asp:UpdatePanel ID="UpdatePanelICDCodes" runat="server">
                                    <ContentTemplate>
                                        <input id="hdnICDCode" value='<%# Eval("ICDCodes")%>' type="hidden" runat="server" />
                                        <input id="hdnExitOrNot" value='<%# Eval("ExitOrNot")%>' type="hidden" runat="server" />
                                        <asp:TextBox ID="txtICDCode" TabIndex="14" runat="server" SkinID="textbox" Wrap="true"
                                             Width="380px"></asp:TextBox>
                                             
                                        <AJAX:PopupControlExtender ID="PopUnit" runat="server" TargetControlID="txtICDCode"
                                            PopupControlID="pnlICDCodes" Position="Bottom" OffsetX="5">
                                        </AJAX:PopupControlExtender>
                                        <asp:Label ID="lblJCode" runat="server" Text="JCode"></asp:Label>
                                        <asp:Label ID="lblCodeJ" runat="server"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                       
                        <tr>
                            <td>
                                Mode
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                                    <ContentTemplate>
                                        <telerik:RadComboBox ID="ddlModeType" runat="server" AllowCustomText="False" MarkFirstMatch="False"
                                            Width="80px">
                                            <Items>
                                                <telerik:RadComboBoxItem Text="Administer" Value="A" />
                                                <telerik:RadComboBoxItem Text="Dispense" Value="D" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Units<span class="style1">*</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtUnits" runat="server" SkinID="textbox" Width="60px" MaxLength="5"
                                            Text="1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtUnits"
                                            Text="*" ErrorMessage="Units" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <AJAX:FilteredTextBoxExtender ID="ftb1" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                            TargetControlID="txtUnits" ValidChars="+-_">
                                        </AJAX:FilteredTextBoxExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Unit Charge<span class="style1">*</span>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updateUnitCharges" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtUnitCharge" runat="server" SkinID="textbox" Width="60px" MaxLength="8"
                                            Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="txtUnitCharge"
                                            Text="*" ErrorMessage="Unit Charge" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <AJAX:FilteredTextBoxExtender ID="ftb2" runat="server" Enabled="True" FilterType="Custom, Numbers"
                                            TargetControlID="txtUnitCharge" ValidChars="$0123456789.">
                                        </AJAX:FilteredTextBoxExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="cbBillPatient" runat="server" SkinID="checkbox" Text="Billable"
                                            Enabled="True" Checked="true" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:ValidationSummary ID="vs1" runat="server" DisplayMode="BulletList" HeaderText="Following fields are mandatory."
                                            ShowMessageBox="true" ShowSummary="false" />
                                        <asp:Button ID="cmdAddtoList" runat="server" SkinID="Button" Text="Add to List" OnClick="cmdAddtoList_OnClick" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <Triggers>
                            <%-- <asp:AsyncPostBackTrigger ControlID="txtNDC" />--%>
                            <asp:AsyncPostBackTrigger ControlID="txtUnits" />
                            <asp:AsyncPostBackTrigger ControlID="txtUnitCharge" />
                            <asp:AsyncPostBackTrigger ControlID="txtICDCode" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:HiddenField ID="ltrId" runat="server" />
                            <telerik:RadGrid ID="gvAddedDrug" runat="server" ClientSettings-EnablePostBackOnRowClick="true"
                                PageSize="5" Skin="Office2007" Width="99%" AllowSorting="False" AllowMultiRowSelection="False"
                                AllowPaging="True" ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                GridLines="none" OnItemDataBound="gvAddedDrug_RowDataBound" OnItemCommand="gvAddedDrug_RowCommand"
                                OnSelectedIndexChanged="gvAddedDrug_SelectedIndexChanged">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <MasterTableView Width="100%">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Medication" HeaderStyle-Width="250">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDISPLAY_NAME" runat="server" Text='<%#Eval("DISPLAY_NAME") %>'
                                                    ToolTip='<%#Eval("DISPLAY_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="ltrId" runat="server" Value='<%#Eval("PrescriptionId") %>' />
                                                <asp:HiddenField ID="lblGENPRODUCT_ID" runat="server" Value='<%#Eval("GENPRODUCT_ID") %>' />
                                                <asp:HiddenField ID="lblDRUG_ID" runat="server" Value='<%#Eval("DRUG_ID") %>' />
                                                <asp:HiddenField ID="lblGENERIC_NAME" runat="server" Value='<%#Eval("GENERIC_NAME") %>' />
                                                <asp:HiddenField ID="lblPrescriptionMode" runat="server" Value='<%#Eval("PrescriptionMode") %>' />
                                                <asp:HiddenField ID="lblDRUG_SYN_ID" runat="server" Value='<%#Eval("DRUG_SYN_ID") %>'>
                                                </asp:HiddenField>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ICDCode" HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="NDCCode" HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNDCCode" runat="server" Text='<%#Eval("NDCCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Units" HeaderStyle-Width="40">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyAmount" runat="server" Text='<%#Eval("QtyAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Unit Charge" HeaderStyle-Width="80">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnitCharge" runat="server" Text='<%#Eval("UNIT_CHARGE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Bill Patient" HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBillPatient" runat="server" Text='<%# Billable(Eval("IsBillable").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" CausesValidation="false" runat="server" Text="Edit"
                                                    CommandName="Select"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="60">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" CausesValidation="false" CommandName="Del"
                                                    ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="13px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                          
                            </telerik:RadGrid>
                            <%--<asp:GridView ID="gvAddedDrug" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvAddedDrug_SelectedIndexChanged"
                                SkinID="gridview" Width="100%" OnRowCommand="gvAddedDrug_RowCommand" OnRowDataBound="gvAddedDrug_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrId" runat="server" Text='<%#Eval("Id") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Medication">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDISPLAY_NAME" runat="server" Text='<%#Eval("DISPLAY_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NDC">
                                        <ItemTemplate>
                                            <asp:Label ID="lblINDC" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ICDCode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblICDCode" runat="server" Text='<%#Eval("ICDCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GENPRODUCT_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGENPRODUCT_ID" runat="server" Text='<%#Eval("GENPRODUCT_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DRUG_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDRUG_ID" runat="server" Text='<%#Eval("DRUG_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="DRUG_SYN_ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDRUG_SYN_ID" runat="server" Text='<%#Eval("DRUG_SYN_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GENERIC_NAME">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGENERIC_NAME" runat="server" Text='<%#Eval("GENERIC_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Units">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQtyAmount" runat="server" Text='<%#Eval("QtyAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Charge">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitCharge" runat="server" Text='<%#Eval("UNIT_CHARGE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bill Patient">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBillPatient" runat="server" Text='<%# Billable(Eval("IsBillable").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" CausesValidation="false" runat="server" CommandName="Del"
                                                ImageUrl="~/Images/DeleteRow.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField SelectText="Edit" ShowSelectButton="True" CausesValidation="false">
                                        <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                    </asp:CommandField>
                                    <asp:BoundField DataField="PrescriptionMode" />
                                </Columns>
                            </asp:GridView>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="Center">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="cmdSave" runat="server" SkinID="Button" Text="Save" OnClick="cmdSave_OnClick"
                                            CausesValidation="false" Visible="false" />
                                        <asp:HiddenField ID="hdn_DRUG_SYN_ID" runat="server" />
                                        <asp:HiddenField ID="hdn_DRUG_ID" runat="server" />
                                        <asp:HiddenField ID="hdn_GENPRODUCT_ID" runat="server" />
                                        
                                        <asp:HiddenField ID="hdnServiceAmount" runat="server" />
                                        <asp:HiddenField ID="hdnDoctorAmount" runat="server" />
                                        <asp:HiddenField ID="hdnDoctorDiscountAmount" runat="server" />
                                        <asp:HiddenField ID="hdnServiceDiscountAmount" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
         <asp:Panel BorderStyle="Solid" BorderWidth="1px" ID="pnlICDCodes" Style="visibility: hidden;
            position: absolute;" BackColor="#E0EBFD" runat="server" Height="180px" ScrollBars="Auto"
            Width="400">
            <table width="100%" border="0">
                <tr>
                    <td>
                    <asp:UpdatePanel ID="update" runat="server">
                    <ContentTemplate>
                     <aspl:ICD ID="icd" runat="server" width="400" PanelName="pnlICDCodes" ICDTextBox="txtICDCode"></aspl:ICD>
                           <asp:HiddenField ID="hdnGridClientId" runat="server" />
                     </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
    

</body>
</html>