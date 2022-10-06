<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ICPMaster.aspx.cs" Inherits="MRD_ICPMaster" Title="Procedure Cause Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

 <script language="javascript" type="text/javascript">

        function AutoChange() {
            //
            var txt = $get('<%=txtName.ClientID%>');
         
            if (txt.value.length > 250) {
                alert("Text length should not be greater then 250.");

                txt.value = txt.value.substring(0, 250);
                txt.focus();             
            }
        }
        
      </script>
      
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr class="clsheader" bgcolor="#e3dcco">
            <td>
                <asp:Label ID="Label1" runat="server" Text="Procedure Cause Master"></asp:Label>
            </td>
            <td align="right">
                <asp:Button ID="ibtnNew" runat="server" Text="New" SkinID="Button" OnClick="ibtnNew_Click"
                    AccessKey="N" CausesValidation="false" />
                <asp:Button ID="btnsave" runat="server" Text="Save" SkinID="Button" OnClick="btnsave_Click"
                    ValidationGroup="Save" AccessKey="S" />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMassage" runat="server"></asp:Label>
                <asp:HiddenField ID="hdndId" runat="server" />
            </td>
        </tr>
    </table>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <td valign="top">
                <asp:Label ID="Label2" runat="server" Text="Code"></asp:Label>&nbsp;
                <asp:TextBox ID="txtCode" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>&nbsp;&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCode"
                    ErrorMessage="Enter Code." Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </td>
            <td valign="top" rowspan="2">
                <asp:Label ID="Label3" runat="server" Text="Name"></asp:Label>&nbsp;
            </td>
            <td valign="top" rowspan="2">
                <asp:TextBox ID="txtName" runat="server" SkinID="textbox" TextMode="MultiLine" Style="min-height: 50px;
                    max-height: 50px; min-width: 300px; max-width: 300px;" onkeyup="return AutoChange();" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtName"
                    ErrorMessage="Enter Name." Display="None" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="Label4" runat="server" Text="Status"></asp:Label>&nbsp;
                <asp:DropDownList ID="ddlStatus" runat="server" SkinID="DropDown" Width="112px">
                    <asp:ListItem Value="1" Text="Active" Selected="True"></asp:ListItem>
                    <asp:ListItem Value="0" Text="In-Active"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>
    <table cellpadding="2" cellspacing="2">
        <tr>
            <td valign="top">
                <telerik:RadGrid ID="gvProcedure" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                    AllowFilteringByColumn="false" AllowMultiRowSelection="false" runat="server"
                    Width="550" Height="400" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                    OnItemCommand="gvProcedure_ItemCommand" OnPreRender="gvProcedure_PreRender" GridLines="Both"
                    AllowPaging="false">
                    <GroupingSettings CaseSensitive="false" />
                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                        <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                            AllowColumnResize="false" />
                    </ClientSettings>
                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                        <NoRecordsTemplate>
                            <div style="font-weight: bold; color: Red;">
                                No Record Found.</div>
                        </NoRecordsTemplate>
                        <ItemStyle Wrap="true" />
                        <Columns>
                            <telerik:GridTemplateColumn UniqueName="Code" DefaultInsertValue="" HeaderText="Code"
                                SortExpression="Code" ShowFilterIcon="false" AllowFiltering="false" FilterControlWidth="99%"
                                HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCode" runat="server" Text='<%#Eval("DiseaseCode")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Name" DefaultInsertValue="" HeaderText="Name"
                                DataField="Name" SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" AllowFiltering="false" FilterControlWidth="99%">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("DiseaseName")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                HeaderStyle-Width="100px" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                AllowFiltering="false" FilterControlWidth="99%">
                                <ItemTemplate>
                                    <asp:Label ID="lblActive" runat="server" Text='<%#Eval("Active")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                HeaderStyle-Width="100px" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                AllowFiltering="false" FilterControlWidth="99%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn DefaultInsertValue="" HeaderText="Id" HeaderStyle-Width="100px"
                                CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false"
                                FilterControlWidth="99%" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Status" DefaultInsertValue="" HeaderText="Status"
                                HeaderStyle-Width="70px" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                                AllowFiltering="false" FilterControlWidth="99%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnbselect" runat="server" Text="Select" CommandName="Select"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings>
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="True">
                        </Scrolling>
                    </ClientSettings>
                </telerik:RadGrid>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="Save" />
            </td>
        </tr>
    </table>
</asp:Content>
