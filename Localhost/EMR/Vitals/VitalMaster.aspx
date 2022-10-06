<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    Theme="DefaultControls" CodeFile="VitalMaster.aspx.cs" Inherits="EMR_Vitals_VitalMaster"
    Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .LinkUnder
        {
            text-decoration: underline;
        }
        .LinkNone
        {
            text-decoration: none;
        }
    </style>

    <script type="text/javascript">

        function openRadWindow(ID) {
            var oWnd = radopen("TagVitalUnit.aspx?VitalId=" + ID, "Radwindow1");
            oWnd.Height = "900";
            oWnd.Center();
        }
        function openRadWindowVital(ID) {
            var oWnd = radopen("AddVitalValues.aspx?VitalId=" + ID, "Radwindow2");
            oWnd.Height = "900";
            oWnd.Center();
        }
        function openRadWindowVitalType(ID) {
            var oWnd = radopen("TagVitalTypeRange.aspx?VitalId=" + ID, "Radwindow2");
            oWnd.Height = "900";
            oWnd.width = "900";
            oWnd.Center();
        }

    </script>

    <script language="javascript" type="text/javascript">
        function Hide(ctrl) {
            //                var ctrlsave = document.getElementById(ctrl);
            //                ctrlsave.style.visibility = 'hidden';
            alert('sdfsd');

        }

        function Tab_SelectionChanged(sender) {
            var tabIndx = sender.get_activeTabIndex();
            //alert(tabIndx);
            if (tabIndx == 0) {
                var ctrlsave = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveUnit');
                //alert(ctrlsave);
                ctrlsave.style.visibility = 'hidden';
                var ctrlsave = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveVital');
                ctrlsave.style.visibility = 'visible';
            }
            else {
                var ctrlsave = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveVital');
                // alert(ctrlsave);
                ctrlsave.style.visibility = 'hidden';
                var ctrlsave = document.getElementById('ctl00_ContentPlaceHolder1_ibtnSaveUnit');
                ctrlsave.style.visibility = 'visible';
            }

        }
         
    </script>

    <table width="100%" cellpadding="0" cellspacing="0" class="clsheader">
        <tr>
            <td width="30%">
                <table>
                    <tr>
                        <td>
                        </td>
                        <td align="left" style="padding-left: 10px; width: 250px;">
                            Vitals
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right" style="padding-right: 10px">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="ibtnSaveVital" ToolTip="Save Vital" runat="server" SkinID="Button"
                            OnClick="ibtnSaveVital_Click" Text="Save Vital" />
                        <asp:Button ID="ibtnSaveUnit" ToolTip="Save Unit" runat="server" SkinID="Button"
                            OnClick="ibtnSaveUnit_Click" Text="Save Unit" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="right">
                <asp:HyperLink ID="HyperLink1" Style="text-decoration: none;" NavigateUrl="/EMR/Vitals/VitalMaster.aspx"
                    runat="server">Vital Signs</asp:HyperLink>
                |
                <asp:HyperLink ID="HyperLink2" Style="text-decoration: none;" NavigateUrl="/EMR/Vitals/VitalSignTemplate.aspx"
                    runat="server">Vital Templates</asp:HyperLink>
                |
                <asp:HyperLink ID="HyperLink3" Style="text-decoration: none;" NavigateUrl="/EMR/Vitals/LinkDoctorWithVitalTemplate.aspx"
                    runat="server">Provider - Vital Templates </asp:HyperLink>
                &nbsp; &nbsp; &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlTabMaster" runat="server" Width="100%" Height="100%">
                            <cc1:TabContainer ID="TabMaster" runat="server" ActiveTabIndex="0" AutoPostBack="true"
                                OnActiveTabChanged="TabMaster_OnActiveTabChanged">
                                <cc1:TabPanel ID="SubTabVitalMaster" runat="server" TabIndex="0">
                                    <HeaderTemplate>
                                        Vital&nbsp;Master</HeaderTemplate>
                                    <ContentTemplate>
                                        <table cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td colspan="6">
                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="label11" runat="server" SkinID="label" Text="Vital&nbsp;Name" />
                                                                <span style='color: Red'>*</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVitalName" SkinID="textbox" Width="160px" MaxLength="30" runat="server"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtVitalName"
                                                                    Display="None" ErrorMessage="Vital Name" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="label2" runat="server" SkinID="label" Text="Vital&nbsp;Display&nbsp;Name" />
                                                                <span style='color: Red'>*</span>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtShortName" SkinID="textbox" Style="text-transform: uppercase;"
                                                                    MaxLength="5" Columns="5" runat="server" Width="60px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtShortName"
                                                                    Display="None" ErrorMessage="Vital Display Name" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="label3" runat="server" SkinID="label" Text="Vital&nbsp;Type" />
                                                                <span style='color: Red'>*</span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDropType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDropType_SelectedIndexChanged"
                                                                    Width="100px">
                                                                    <asp:ListItem Text="Select" Value="0" />
                                                                    <asp:ListItem Text="TextBox" Value="T" />
                                                                    <asp:ListItem Text="DropDown" Value="D" />
                                                                      <asp:ListItem Text="Date" Value="DT" />
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="label1" runat="server" SkinID="label" Text="Max Length" Visible="False" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaxLength" SkinID="textbox" MaxLength="1" Columns="2" Visible="False"
                                                                    runat="server"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtMaxLength" FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMaxLength"
                                                                    Display="None" ErrorMessage="Max Length" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMaxLength"
                                                                    Display="None" MaximumValue="6" MinimumValue="1" Type="Integer" SetFocusOnError="True"
                                                                    ErrorMessage="Max Length Must Between 1-6." ValidationGroup="Save"></asp:RangeValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="label4" runat="server" SkinID="label" Text="Result&nbsp;Type" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlResultType" SkinID="DropDown" runat="server" Width="160px">
                                                                    <asp:ListItem Selected="True" Value="S" Text="Gender Wise"></asp:ListItem>
                                                                    <asp:ListItem Value="A" Text="Age Wise"></asp:ListItem>
                                                                    <asp:ListItem Value="AS" Text="Age Gender Wise"></asp:ListItem>
                                                                    <asp:ListItem Value="G" Text="General"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="label10" runat="server" SkinID="label" Text="Display In Graph" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlDisplayInGraph" SkinID="DropDown" runat="server" Width="60px">
                                                                    <asp:ListItem Value="True" Text="True"></asp:ListItem>
                                                                    <asp:ListItem Value="False" Text="False" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Label12" runat="server" SkinID="label" Text="Is&nbsp;Mandatory" />
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlIsMandatory" SkinID="DropDown" runat="server" Width="100px">
                                                                    <asp:ListItem Text="True" Value="True" />
                                                                    <asp:ListItem Text="False" Value="False" Selected="True" />
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:GridView ID="gvVitalView" runat="server" AutoGenerateColumns="False" SkinID="gridview2"
                                                        AllowPaging="True" OnRowEditing="gvVitalView_RowEditing" OnRowCancelingEdit="gvVitalView_RowCancelingEdit"
                                                        OnRowUpdating="gvVitalView_RowUpdating" OnPageIndexChanging="gvVitalView_PageIndexChanging"
                                                        OnRowDataBound="gvVitalView_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S No">
                                                                <ItemTemplate>
                                                                    <%#Container .DataItemIndex +1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField HeaderText="S No" DataField="VitalId" ReadOnly="True" />
                                                            <asp:TemplateField HeaderText="Vital">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtVitalName" MaxLength="30" SkinID="textbox" Columns="30" runat="server"
                                                                        Text='<%#Eval("VitalSignName")%>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldVitalName" runat="server" ControlToValidate="txtVitalName"
                                                                        Display="None" ErrorMessage="Vital Name" SetFocusOnError="True" ValidationGroup="UpdateVital">
                                                                    </asp:RequiredFieldValidator>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("VitalSignName")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Display&nbsp;Name">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtShortNameEdit" Style="text-transform: uppercase;" MaxLength="5"
                                                                        SkinID="textbox" Columns="5" runat="server" Text='<%#Eval("DisplayName")%>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldDisplayName" runat="server" ControlToValidate="txtShortNameEdit"
                                                                        Display="None" ErrorMessage="Display Name" SetFocusOnError="True" ValidationGroup="UpdateVital">
                                                                    </asp:RequiredFieldValidator>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("DisplayName")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Max&nbsp;Length">
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtMaxLengthEdit" MaxLength="1" Columns="2" runat="server" SkinID="textbox"
                                                                        Text='<%#Eval("MaxLength")%>'></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="txtfindingsFilteredTextBoxExtender" runat="server"
                                                                        Enabled="True" TargetControlID="txtMaxLengthEdit" FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldMax" runat="server" ControlToValidate="txtMaxLengthEdit"
                                                                        Display="None" ErrorMessage="Max Length" SetFocusOnError="True" ValidationGroup="UpdateVital">
                                                                    </asp:RequiredFieldValidator>--%>
                                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMaxLengthEdit"
                                                                        Display="None" MaximumValue="6" MinimumValue="1" Type="Integer" SetFocusOnError="True"
                                                                        ErrorMessage="Max Length Must Between 1-6." ValidationGroup="UpdateVital"></asp:RangeValidator>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("MaxLength")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Result&nbsp;Type">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlResultTypeEdit" SelectedValue='<%#Eval("ResultTypeId") %>'
                                                                        SkinID="DropDown" runat="server">
                                                                        <asp:ListItem Selected="True" Value="S" Text="Gender Wise"></asp:ListItem>
                                                                        <asp:ListItem Value="A" Text="Age Wise"></asp:ListItem>
                                                                        <asp:ListItem Value="AS" Text="Age Gender Wise"></asp:ListItem>
                                                                        <asp:ListItem Value="G" Text="General"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("ResultType") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vital&nbsp;Type">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlVitalType" SkinID="DropDown" runat="server">
                                                                        <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                                        <asp:ListItem Value="T" Text="TextBox"></asp:ListItem>
                                                                        <asp:ListItem Value="D" Text="DropDown"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVitalType" runat="server" Text='<%#Eval("VitalType") %>'></asp:Label>
                                                                    <a href="" onclick="openRadWindowVital('<%# DataBinder.Eval(Container.DataItem, "VitalId") %>'); return false;">
                                                                        <asp:ImageButton ID="imgVital" runat="server" ImageUrl="/Images/PopUp.jpg" /></a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlStatusEdit" SelectedValue='<%#Eval("Active") %>' runat="server"
                                                                        SkinID="DropDown">
                                                                        <asp:ListItem Selected="True" Value="True" Text="Active"></asp:ListItem>
                                                                        <asp:ListItem Value="False" Text="In-Active"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("Status") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Display In Graph">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlDisplayInGraph" SelectedValue='<%#Eval("DisplayInGraph") %>'
                                                                        runat="server" SkinID="DropDown">
                                                                        <asp:ListItem Value="True" Text="True"></asp:ListItem>
                                                                        <asp:ListItem Value="False" Text="False"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("DisplayInGraph") %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Is Mandatory">
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlIsMandatory" SelectedValue='<%#Eval("IsMandatory") %>' runat="server"
                                                                        SkinID="DropDown">
                                                                        <asp:ListItem Text="True" Value="True" />
                                                                        <asp:ListItem Text="False" Value="False" Selected="True" />
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <ItemTemplate>
                                                                    <%#Eval("IsMandatory")%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ShowEditButton="True" ValidationGroup="UpdateVital" />
                                                            <asp:TemplateField HeaderText="Tag">
                                                                <ItemTemplate>
                                                                    <img alt="Tag Unit" src="/Images/PopUp.jpg" style="cursor: pointer;" onclick="openRadWindow('<%# DataBinder.Eval(Container.DataItem, "VitalId") %>'); return false;" />
                                                                    <%-- <img id="imgTagUnit" runat="server" alt="Tag Unit" src="/Images/PopUp.jpg" style="cursor: pointer;" />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vital Type">
                                                                <ItemTemplate>
                                                                    <img alt="Tag Unit" src="/Images/PopUp.jpg" style="cursor: pointer;" onclick="openRadWindowVitalType('<%# DataBinder.Eval(Container.DataItem, "VitalId") %>'); return false;" />
                                                                    <%-- <img id="imgTagUnit" runat="server" alt="Tag Unit" src="/Images/PopUp.jpg" style="cursor: pointer;" />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Following field(s) are mandtory."
                                                        ValidationGroup="Save" ShowMessageBox="True" ShowSummary="False" />
                                                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" HeaderText="Following field(s) are mandtory."
                                                        ValidationGroup="UpdateVital" ShowMessageBox="True" ShowSummary="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="pnlUnit" runat="server" TabIndex="1">
                                    <%--OnClientClick="Hide('ctl00_ContentPlaceHolder1_TabMaster_pnlUnit_ibtnSaveUnit');"--%>
                                    <HeaderTemplate>
                                        Vital&nbsp;Unit&nbsp;Master</HeaderTemplate>
                                    <ContentTemplate>
                                        <table cellpadding="2" cellspacing="2" width="100%">
                                            <tr>
                                                <td width="120px">
                                                    <asp:Label ID="label5" runat="server" SkinID="label" Text="Unit&nbsp;Name" />
                                                    <span style='color: Red'>*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtunitname" SkinID="textbox" Columns="30" MaxLength="30" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtunitname"
                                                        Display="None" ErrorMessage="Unit Name" SetFocusOnError="True" ValidationGroup="SaveUnit"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="label6" runat="server" SkinID="label" Text="Symbol" />
                                                    <span style='color: Red'>*</span>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSymbol" SkinID="textbox" Columns="2" MaxLength="5" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSymbol"
                                                        Display="None" ErrorMessage="Symbol" SetFocusOnError="True" ValidationGroup="SaveUnit"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="label7" runat="server" SkinID="label" Text="Conversion&nbsp;Formula" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtConversionFormula" SkinID="textbox" Columns="30" MaxLength="30"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="label8" runat="server" SkinID="label" Text="Measurement&nbsp;System" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMeasurementSystem" runat="server" SkinID="DropDown" Style="width: 90px;">
                                                        <asp:ListItem Selected="True" Text="Metric" Value="M"></asp:ListItem>
                                                        <asp:ListItem Text="English" Value="E"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="label9" runat="server" SkinID="label" Text="Converted&nbsp;Unit" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlConvertedId" runat="server" SkinID="DropDown">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="gvUnitView" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                                        OnRowEditing="gvUnitView_RowEditing" OnRowCancelingEdit="gvUnitView_RowCancelingEdit"
                                                        OnRowUpdating="gvUnitView_RowUpdating" OnRowDataBound="gvUnitView_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="S No" DataField="UnitId" ReadOnly="True" />
                                                            <asp:BoundField HeaderText="S No" DataField="Sno" ReadOnly="True" />
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%#Eval("UnitName")%>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtUnitNameEdit" runat="server" SkinID="textbox" MaxLength="30"
                                                                        Columns="30" Text='<%#Eval("UnitName")%>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldUnitName" runat="server" ControlToValidate="txtUnitNameEdit"
                                                                        Display="None" ErrorMessage="Unit Name" SetFocusOnError="True" ValidationGroup="UpdateUnit">
                                                                    </asp:RequiredFieldValidator>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    Unit</HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%#Eval("Symbol")%>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtSymbolEdit" runat="server" SkinID="textbox" MaxLength="10" Columns="10"
                                                                        Text='<%#Eval("Symbol")%>'></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldSymbol" runat="server" ControlToValidate="txtSymbolEdit"
                                                                        Display="None" ErrorMessage="Symbol" SetFocusOnError="True" ValidationGroup="UpdateUnit">
                                                                    </asp:RequiredFieldValidator>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    Symbol</HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%#Eval("ConversionFormula")%>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txtConversionFormulaEdit" MaxLength="30" Columns="30" runat="server"
                                                                        SkinID="textbox" Text='<%#Eval("ConversionFormula")%>'></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    Conversion Formula</HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%#Eval("ConvertedName")%>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlConvertedUnitEdit" SelectedValue='<%#Eval("ConvertedUnitId") %>'
                                                                        SkinID="DropDown" runat="server" DataSourceID="sqDSource" DataTextField="UnitName"
                                                                        DataValueField="UnitID" AppendDataBoundItems="true">
                                                                        <asp:ListItem Text="[Select]" Value=""></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <asp:SqlDataSource ID="sqDSource" EnableCaching="true" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>"
                                                                        SelectCommandType="Text" SelectCommand="select UnitID, UnitName from EMRVitalUnitMaster">
                                                                    </asp:SqlDataSource>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    Converted Unit</HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%#Eval("MS") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlMeasurementSystemEdit" SelectedValue='<%#Eval("MeasurementSystem") %>'
                                                                        runat="server" SkinID="DropDown">
                                                                        <asp:ListItem Selected="True" Text="Metric" Value="M"></asp:ListItem>
                                                                        <asp:ListItem Text="English" Value="E"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    Measurement System</HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%#Eval("Status") %>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddlUnitStatusEdit" SelectedValue='<%#Eval("Active") %>' SkinID="DropDown"
                                                                        runat="server">
                                                                        <asp:ListItem Selected="True" Value="True" Text="Active"></asp:ListItem>
                                                                        <asp:ListItem Value="False" Text="In-Active"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <HeaderTemplate>
                                                                    Status</HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:CommandField ShowEditButton="True" ValidationGroup="UpdateUnit" />
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:GridView>
                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" HeaderText="Following field(s) are mandtory."
                                                        ValidationGroup="SaveUnit" ShowMessageBox="True" ShowSummary="False" />
                                                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" HeaderText="Following field(s) are mandtory."
                                                        ValidationGroup="UpdateUnit" ShowMessageBox="True" ShowSummary="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </asp:Panel>
                        <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                        </telerik:RadWindow>
                        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="800" Height="570"
                            VisibleStatusbar="false" Top="40" Left="200" Behaviors="Close,Move" OnClientClose=""
                            ReloadOnShow="true">
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
