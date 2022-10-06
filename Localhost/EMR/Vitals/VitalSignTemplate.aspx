<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    Theme="DefaultControls" CodeFile="VitalSignTemplate.aspx.cs" Inherits="EMR_Vitals_VitalSignTemplate"
    Title="" %>
    
 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function GetVitalID(VitalID) {
            alert(VitalID);
        }
    </script>

    <table width="100%" cellpadding="0" cellspacing="0">
        <tr class="clsheader">
            <td>
            </td>
            <td align="left" style="width: 270px; padding-left: 10px;">
                Vital Sign Template
            </td>
            <td align="right" style="padding-right: 15px">
                <%--<asp:ImageButton ID="ibtnVitalSignTemplate" runat="server" OnClick="SaveVitalSignTemplate_OnClick"
                    ImageUrl="~/Images/save.gif" />--%>
                <asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_OnClick" SkinID="Button" />
                <asp:Button ID="ibtnVitalSignTemplate" runat="server" Text="Save" OnClick="SaveVitalSignTemplate_OnClick"
                    SkinID="Button" />
                <asp:HiddenField ID="hdnVitalID" runat="server" Value="" />
                <asp:HiddenField ID="hdnEditMode" runat="server" Value="" />
                <asp:HiddenField ID="hdnNew" runat="server" Value="" />
            </td>
            <td>
                &nbsp;
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
    </table>
    <table width="90%" cellpadding="2" cellspacing="2">
        <tr>
            <td align="center" colspan="2" style="height: 13px; color: green; font-size: 12px;
                font-weight: bold;">
                <asp:UpdatePanel ID="updmessage" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlVitalSet" runat="server">
                            <table cellpadding="2" cellspacing="2">
                                <tr align="left">
                                    <td>
                                        <asp:Literal ID="ltrlVitalSet" runat="server" Text="Template"></asp:Literal><span style="color: red">*</span>
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList  ID="ddlVitalTemplateSet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVitalTemplateSet_OnSelectedIndexChanged" Width="250px">
                                </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtVitalTemplateSet" SkinID="textbox" runat="server" Text=""></asp:TextBox>
                                        <%--<asp:Button ID="btnNew" SkinID="Button" runat="server" Text="New" OnClick="btnNew_OnClick" />
                                <asp:Button ID="btnEdit" SkinID="Button" runat="server" Text="Edit" OnClick="btnEdit_OnClick"
                                    Visible="false" />
                                <asp:Button ID="btnDelete" SkinID="Button" runat="server" Text="Delete" OnClick="btnDelete_OnClick" />
                                <asp:ValidationSummary ID="VSSave" runat="server" ShowMessageBox="True" ShowSummary="False"
                                    ValidationGroup="Save" /> --%>
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltrlDescription" runat="server" Text="Description"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" SkinID="textbox" runat="server" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="ltrlMeasurement" runat="server" Text="Measurement"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMeasurement" runat="server" AutoPostBack="false" SkinID="DropDown">
                                            <asp:ListItem Text="English" Value="E" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Metric" Value="M" Selected="False"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVtxtVitalTemplateSet" runat="server" ErrorMessage="Template Name Cannot Be Blank..."
                                            SetFocusOnError="true" ControlToValidate="txtVitalTemplateSet" Display="None"
                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        <%--<asp:Button ID="btnSave" SkinID="Button" runat="server" Text="Save" ToolTip="Save New Vital Template"
                                    OnClick="btnSave_OnClick" ValidationGroup="Save" />
                                
                                <asp:Button ID="btnUndo" SkinID="Button" runat="server" Text="Undo" OnClick="btnUndo_OnClick" />--%>
                                    </td>
                                    <td>
                                        Make&nbsp;Default
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkDefault" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <%--<tr>
            <td>
                <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnVitalSearch">
                    <asp:TextBox ID="txtVitalSearch" SkinID="textbox" runat="server" Columns="40"></asp:TextBox>&nbsp;
                    <asp:Button ID="btnVitalSearch" SkinID="Button" runat="server" Text="Search" OnClick="btnVitalSearch_OnClick" />
                </asp:Panel>
            </td>
            <td>
            </td>
        </tr>--%>
        <tr>
            <td valign="top" style="width: 35%">
                <asp:Literal ID="ltrAllVitals" runat="server" Text="<strong>All Vitals</strong>"
                    Visible="false"></asp:Literal><br />
                <asp:Panel ID="pnlVitals" runat="server" Width="350px" Height="265px">
                    <asp:UpdatePanel ID="updVitals" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        
                            <asp:GridView ID="gvVitals" SkinID="gridview" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                ShowHeader="true" Width="100%" PageSize="12" AllowPaging="True" PagerSettings-Mode="NumericFirstLast"
                                OnSelectedIndexChanged="gvVitals_SelectedIndexChanged" OnRowDataBound="gvVitals_RowDataBound"
                                OnPageIndexChanging="gvVitals_PageIndexChanging" PageIndex="0" 
                                AllowSorting="True">
                                <PagerSettings Mode="NumericFirstLast" />
                                <Columns>
                                    <asp:BoundField DataField="VitalID" />
                                    <asp:BoundField DataField="VitalSignName" HeaderText="All Vitals" 
                                        HeaderStyle-Font-Bold="true" >
                                        <HeaderStyle Font-Bold="True" />
                                    </asp:BoundField>
                                    <asp:CommandField ButtonType="Link" ControlStyle-ForeColor="Blue" ItemStyle-Width="70px"
                                        ControlStyle-Font-Underline="true" SelectText="Select" CausesValidation="false"
                                        ShowSelectButton="true">
                                        <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                        <ItemStyle Width="70px" />
                                    </asp:CommandField>
                                </Columns>                             
                            </asp:GridView>
                           <%-- <asp:Literal ID="Literal1" runat="server" Text="(Double Click To Add)" Visible="true"></asp:Literal>--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvSelectedVitals" />
                            <asp:AsyncPostBackTrigger ControlID="gvVitals" />
                            <asp:AsyncPostBackTrigger ControlID="gvTemplate" />
                            <asp:AsyncPostBackTrigger ControlID="ibtnVitalSignTemplate" />
                        </Triggers>
                    </asp:UpdatePanel>
             </asp:Panel>
            </td>
            <td valign="top" style="width: 45%">
                <asp:Literal ID="ltrlSelectedVitals" runat="server" Text="<strong>Selected Vitals</strong>"></asp:Literal><br />
                <asp:Panel ID ="pnlSelectedGrid" runat ="server" Width ="350px" ScrollBars ="None">
                <asp:UpdatePanel ID="updSelected" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvSelectedVitals" SkinID="gridview" runat="server" AutoGenerateColumns="false" 
                            DataKeyNames="VitalID" AllowPaging="true" PageIndex="0" PageSize="20" ShowHeader="true"
                            PagerSettings-Mode="NumericFirstLast" Width="100%" OnRowDataBound="gvSelectedVitals_OnRowDataBound"
                            OnRowCommand="gvSelectedVitals_OnRowCommand" OnPageIndexChanging="gvSelectedVitals_OnPageIndexChanging">
                            <PagerSettings Mode="NumericFirstLast" />
                            <Columns>
                           
                                <asp:BoundField DataField="TemplateId" HeaderText="TemplateId" />
                                <asp:BoundField DataField="VitalID" HeaderText="VitalID" />
                                <asp:TemplateField HeaderText="Vital Name">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtVitalNameGrid" CssClass="gridInput" runat="server" Text='<%#Eval("Vital")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SequenceNo" HeaderText="Sequence" HeaderStyle-Width="68px"
                                    ItemStyle-Width="68px" >
                                    <HeaderStyle Width="68px" />
                                    <ItemStyle Width="68px" />
                                </asp:BoundField>
                                                             
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnInActive" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                            CommandName="InActive" CommandArgument='<%#Eval("VitalID")%>' ToolTip="InActive" />
                                        <asp:ImageButton ID="ibtnRemoveFromGrid" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                            CommandName="Remove" CommandArgument='<%#Eval("VitalID")%>' 
                                            ToolTip="Remove From Grid" Width="16px" />
                                        <asp:ImageButton ID="ibtnActivate" runat="server" ImageUrl="~/Images/Down.jpg" CommandName="Activate"
                                            CommandArgument='<%#Eval("VitalID")%>' ToolTip="Activate" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Active" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvSelectedVitals" />
                        <asp:AsyncPostBackTrigger ControlID="gvVitals" />
                        <asp:AsyncPostBackTrigger ControlID="gvTemplate" />
                        <asp:AsyncPostBackTrigger ControlID="ibtnVitalSignTemplate" />
                    </Triggers>
                </asp:UpdatePanel>
                </asp:Panel>
                
            </td>
        </tr>
       
        <tr>
            <td colspan="2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvTemplate" SkinID="gridview" runat="server" AutoGenerateColumns="false"
                            Width="100%" OnRowDataBound="gvTemplate_RowDataBound" OnSelectedIndexChanged="gvTemplate_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="TemplateName" HeaderText="Template" />
                                <asp:BoundField DataField="TemplateDescription" HeaderText="Description" />
                                <asp:BoundField DataField="Measurement" HeaderText="Measurement " />
                                <asp:BoundField DataField="MeasurementSystem" HeaderText="MeasurementSystem " />
                                <asp:BoundField DataField="DefaultTemplate" HeaderText="Default" />
                                <asp:BoundField DataField="DefaultValue" HeaderText="Default" />
                                <asp:CommandField ShowSelectButton="true" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
