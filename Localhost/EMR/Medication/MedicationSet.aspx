<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="MedicationSet.aspx.cs" Inherits="EMR_Medication_MedicationSet" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Bootstrap -->
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function addDiagnosisSerchOnClientClose(oWnd, args) {
            $get('<%=btnAddOrderSetClose.ClientID%>').click();
        }

        function MaxLenTxt(TXT, MAX) {
            if (TXT.value.length > MAX) {
                alert("Text length should not be greater then " + MAX + " ...");

                TXT.value = TXT.value.substring(0, MAX);
                TXT.focus();
            }
        }

        function OnClientItemsRequesting(sender, eventArgs) {
            var ddlgeneric = $find('<%=ddlGeneric.ClientID %>');
                    var value = ddlgeneric.get_value();
                    var context = eventArgs.get_context();
                    context["GenericId"] = value;
                }

                function ddlGeneric_OnClientSelectedIndexChanged(sender, args) {

                    var ddlbrand = $find("<%=ddlDrugs.ClientID%>");
            if (ddlbrand != null) {
                ddlbrand.clearItems();
                ddlbrand.set_text("");
                ddlbrand.get_inputDomElement().focus();
            }
            var item = args.get_item();
            $get('<%=hdnGenericId.ClientID%>').value = item != null ? item.get_value() : sender.value();
            $get('<%=hdnGenericName.ClientID%>').value = item != null ? item.get_text() : sender.text();

            $get('<%=hdnCIMSItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSItemId") : "";
            $get('<%=hdnCIMSType.ClientID%>').value = item != null ? item.get_attributes().getAttribute("CIMSType") : "";

            $get('<%=hdnVIDALItemId.ClientID%>').value = item != null ? item.get_attributes().getAttribute("VIDALItemId") : "";

            $get('<%=btnGetInfoGeneric.ClientID%>').click();
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnDrugName" runat="server" />
            <div class="VisitHistoryDiv ">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="VisitHistoryDivText">
                                <h2>Medication Sets</h2>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <asp:Panel ID="pnlMedicationOptions" runat="server">
                                <span class="MedicationDIv">
                                    <asp:Literal ID="ltrlMedicationSet" runat="server" Text="Medication Set"></asp:Literal>
                                    <telerik:RadComboBox ID="ddlMedicationSet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMedicationSet_SelectedIndexChanged"
                                        EmptyMessage="Select" Width="200px" Height="300px" DropDownWidth="300px" />
                                </span>
                                <span class="MedicationDIv">
                                    <asp:ImageButton ID="ibtnAddOrderSet" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                        CssClass="Order-ASIcon" ToolTip="Add Medication Set" Height="17px" Width="17px"
                                        OnClick="ibtnAddOrderSet_Click" CausesValidation="false" />
                                    <asp:TextBox ID="txtMedicationSet" CssClass="AdminBtn" runat="server" MaxLength="100"></asp:TextBox>&nbsp;&nbsp;
                                                <asp:Button ID="btnSave" CssClass="AdminBtn" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    <%--<asp:Button ID="btnNew" CssClass="AdminBtn" runat="server" Text="New" OnClick="btnNew_Click" />
                                                <asp:Button ID="btnDelete" CssClass="AdminBtn" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                                                <asp:Button ID="btnUndo" CssClass="AdminBtn" runat="server" Text="Undo" OnClick="btnUndo_Click" />--%>
                                </span>
                            </asp:Panel>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnFormSave" runat="server" CssClass="AdminBtn01" Text="Save" OnClick="btnFormSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="VitalHistory-Div">
                <div class="container-fluid">
                    <div class="row">
                        <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Height="580px">
                            <div class="col-md-12">
                                <div class="row form-group">
                                    <div class="col-md-1">
                                        <span class="">
                                            <asp:Label ID="Label6" runat="server" Text="Generic" Font-Size="12px"></asp:Label>
                                            <span class="red">*</span></span>
                                    </div>
                                    <div class="col-md-4">
                                        <span class="">
                                            <telerik:RadComboBox ID="ddlGeneric" runat="server" Skin="Office2007" DataTextField="GenericName"
                                                DataValueField="GenericId" HighlightTemplatedItems="true" Height="300px" Width="100%"
                                                ZIndex="50000" EmptyMessage="[ Search Generics ]" AllowCustomText="true"
                                                MarkFirstMatch="true" EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                OnItemsRequested="ddlGeneric_OnItemsRequested" OnClientSelectedIndexChanged="ddlGeneric_OnClientSelectedIndexChanged">
                                                <HeaderTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 100%" align="left">
                                                                <asp:Label ID="Label28" runat="server" Text="Generic Name" />
                                                            </td>
                                                            <td style="width: 0%" align="right" visible="false">
                                                                <asp:HiddenField ID="hdnAttCIMSItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSItemId']")%> />
                                                            </td>
                                                            <td style="width: 0%" align="right" visible="false">
                                                                <asp:HiddenField ID="hdnAttCIMSType" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['CIMSType']")%> />
                                                            </td>
                                                            <td style="width: 0%" align="right" visible="false">
                                                                <asp:HiddenField ID="hdnAttVIDALItemId" runat="server" Value=<%# DataBinder.Eval(Container, "Attributes['VIDALItemId']")%> />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 100%" align="left">
                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:RadComboBox>
                                        </span>
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-4">
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-1">
                                        <span class="">
                                            <asp:Label ID="Label1" runat="server" Text="Drugs" Font-Size="12px"></asp:Label>
                                            <span class="red">*</span></span>
                                    </div>
                                    <div class="col-md-4">
                                        <span class="">
                                            <telerik:RadComboBox ID="ddlDrugs" Filter="Contains" runat="server" HighlightTemplatedItems="true"
                                                Width="100%" EmptyMessage="Select" EnableLoadOnDemand="true" OnItemsRequested="ddlDrugs_OnItemsRequested"
                                                OnClientItemsRequesting="OnClientItemsRequesting"
                                                Skin="Office2007" ShowMoreResultsBox="true" Height="300px" EnableVirtualScrolling="true">
                                                <HeaderTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 80%" align="left">
                                                                <asp:Literal ID="Literal2" runat="server" Text="Item Name" />
                                                            </td>
                                                            <td style="width: 10%" align="center">Stock
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="width: 80%" align="left">
                                                                <%# DataBinder.Eval(Container, "Text")%>
                                                            </td>
                                                            <td style="width: 10%" align="left">
                                                                <%# DataBinder.Eval(Container, "Attributes['ClosingBalance']")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </telerik:RadComboBox>
                                        </span>
                                    </div>

                                    <div class="col-md-1">
                                    </div>

                                    <div class="col-md-1">
                                        <asp:Label ID="lblForm" runat="server" Text="Form" Font-Size="12px" />
                                        <%--<span style="color: Red; font-weight: bold;">*</span>--%>
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadComboBox ID="ddlFormulation" runat="server" Width="100%" Height="300px"
                                            EmptyMessage="Select" AllowCustomText="true" Filter="Contains" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlFormulation_SelectedIndexChanged" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-1">
                                        <asp:Label ID="lblDoseDtl" runat="server" Text="Dose Detail" Font-Size="12px" />
                                        <span style="color: Red; font-weight: bold;">*</span>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row  form-group">
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtDoseDtl" runat="server" Width="100%" MaxLength="3" CssClass="inline_bl" />
                                            </div>
                                            <div class="col-md-6">
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtDoseDtl" FilterType="Custom" ValidChars="1234567890." />
                                                <telerik:RadComboBox ID="ddlDoseUnits" runat="server" DropDownWidth="190px" EmptyMessage="[ Select ]"
                                                    MarkFirstMatch="true" Width="100%" Height="300px" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Label ID="lblRoot" runat="server" Text="Route" Font-Size="12px" />
                                        <%--<span style="color: Red; font-weight: bold;">*</span>--%>
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadComboBox ID="ddlRoute" runat="server" Width="100%" Height="300px" EmptyMessage="Select"
                                            AllowCustomText="true" Filter="Contains" />
                                    </div>


                                </div>
                                <!-- frequency -->
                                <div class="row form-group">
                                    <div class="col-md-1">
                                        <asp:Label ID="Label2" runat="server" Text="Frequency" Font-Size="12px"></asp:Label>
                                        <span class="red">*</span>
                                    </div>
                                    <div class="col-md-4">
                                        <span class="">
                                            <telerik:RadComboBox ID="ddlFrequency" runat="server" Width="100%" Height="300PX"
                                                EmptyMessage="Select" AllowCustomText="true" Filter="Contains" />
                                        </span>
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Label ID="Label4" runat="server" Text="Food Relation" Font-Size="12px" />
                                        <%--<span style="color: Red; font-weight: bold;">*</span>--%>
                                    </div>
                                    <div class="col-md-4">
                                        <telerik:RadComboBox ID="ddlfoodrelationship" runat="server" Width="100%" EmptyMessage="Select"
                                            AllowCustomText="true" Filter="Contains" />
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-1">
                                        <asp:Label ID="Label3" runat="server" Text="Duration" Font-Size="12px"></asp:Label>
                                        <span class="red">*</span>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtDuration" runat="server" MaxLength="3"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                    TargetControlID="txtDuration" FilterType="Custom" ValidChars="1234567890">
                                                </cc1:FilteredTextBoxExtender>
                                            </div>
                                            <div class="col-md-6">
                                                <telerik:RadComboBox ID="ddlPeriodType" Width="100%" runat="server" EmptyMessage="Select">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Text="Day(s)" Value="D" Selected="true" />
                                                        <telerik:RadComboBoxItem Text="Week(s)" Value="W" />
                                                        <telerik:RadComboBoxItem Text="Month(s)" Value="M" />
                                                        <telerik:RadComboBoxItem Text="Year(s)" Value="Y" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                    </div>
                                    <div class="col-md-1">
                                        <asp:Label ID="Label5" runat="server" Text="Instructions" Font-Size="12px" />
                                        <%--<span style="color: Red; font-weight: bold;">*</span>--%>
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:TextBox ID="txtInstructions" runat="server" MaxLength="1000" TextMode="MultiLine"
                                            onkeyup="return MaxLenTxt(this, 1000);" Style="width: 100% !important; font-size: 12px; text-align: left; padding: 3px 5px;" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-12">
                                        <span class="">
                                            <asp:Label ID="ltrlSelectedServices" runat="server" Text="Added Drug Lists" /></span>

                                        <div class="pull-right">
                                            <asp:Button ID="btnAddToList" CssClass="AdminBtn03" runat="server" Text="Add to List"
                                                OnClick="btnAddToList_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:GridView ID="gvMedication" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                            Width="100%" ShowHeader="true" DataKeyNames="ItemID" OnRowCommand="gvMedication_RowCommand" Visible="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Generic Name" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGenericName" runat="server" SkinID="label" Text='<%#Eval("GenericName") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnGenericId" runat="server" Value='<%# Eval("GenericId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Drug Name" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" SkinID="label" Text='<%#Eval("ItemName") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnSetId" runat="server" Value='<%# Eval("SetID") %>' />
                                                        <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# Eval("ItemID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dose" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDosedtls" runat="server" SkinID="label" Text='<%# Eval("Dose") %>'
                                                            HeaderStyle-HorizontalAlign="Right" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDoseUnit" runat="server" SkinID="label" Text='<%# Eval("DoseUnit") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnDoseUnitId" runat="server" Value='<%# Eval("DoseUnitId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Frequency" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" SkinID="label" Text='<%#Eval("FrequencyName") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnFrequencyId" runat="server" Value='<%# Eval("FrequencyId") %>' />
                                                        <asp:HiddenField ID="hdnExistItem" runat="server" Value='<%# Eval("ExistItem") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Duration" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDuration" runat="server" SkinID="label" Text='<%#Eval("Duration") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDType" runat="server" SkinID="label" Text='<%#Eval("DType") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnDtypeId" runat="server" Value='<%# Eval("DTypeId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Form" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFormulation" runat="server" SkinID="label" Text='<%# Eval("FormulationName") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnFormulationId" runat="server" Value='<%# Eval("FormulationId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Route" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRouteName" runat="server" SkinID="label" Text='<%# Eval("RouteName") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnRouteId" runat="server" Value='<%# Eval("RouteId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Food Relation" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFoodName" runat="server" SkinID="label" Text='<%# Eval("FoodName") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnFoodID" runat="server" Value='<%# Eval("FoodID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Instructions" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstructions" runat="server" SkinID="label" Text='<%# Eval("InstructionRemarks") %>'
                                                            HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:HiddenField ID="hdnInstructions" runat="server" Value='<%# Eval("InstructionRemarks") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Images/edit.png" CausesValidation="false"
                                                            CommandName="ed" ToolTip="Click here to edit this record" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                            CommandName="Del" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-md-12">
                                        <asp:Button ID="btnAddOrderSetClose" runat="server" Style="visibility: hidden;" OnClick="btnAddOrderSetClose_OnClick" />
                                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                                            Width="1200" Height="600" Left="10" Top="10">
                                            <Windows>
                                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                                    Width="900" Height="600" />
                                            </Windows>
                                        </telerik:RadWindowManager>
                                    </div>
                                </div>
                                <!-- medication set -->
                            </div>
                            <div class="col-md-12">
                                <asp:HiddenField ID="hdnGenericId" runat="server" />
                                <asp:HiddenField ID="hdnGenericName" runat="server" />
                                <asp:HiddenField ID="hdnCIMSItemId" runat="server" />
                                <asp:HiddenField ID="hdnCIMSType" runat="server" />
                                <asp:HiddenField ID="hdnVIDALItemId" runat="server" />

                                <asp:Button ID="btnGetInfoGeneric" runat="server" Text="" CausesValidation="false"
                                    SkinID="button" Style="visibility: hidden;" OnClick="btnGetInfoGeneric_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnAddOrderSet" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
