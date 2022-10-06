<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="DiagnosoMaster.aspx.cs" Inherits="EMR_Assessment_DiagnosoMaster" Title="Diagnosis Master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr class="clsheader">
                    <td style="padding-left: 10px; width: 250px;">
                        Diagnosis Master
                    </td>
                    <td align="right">
                        <asp:Button ID="btnNew" runat="server" ToolTip="Clear Data" SkinID="Button" Text="New"
                            CausesValidation="false" OnClick="btnNew_Click" />
                        <asp:Button ID="btnSave" runat="server" ToolTip="Save" SkinID="Button" Text="Save"
                            ValidationGroup="Save" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" height="18">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Group" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGroup" runat="server" SkinID="textbox" ReadOnly="true" Width="300px"></asp:TextBox>
                        <asp:HiddenField ID="hdngroupid" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Sub Group" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubgroup" runat="server" SkinID="textbox" ReadOnly="true" Width="300px"></asp:TextBox>
                        <asp:HiddenField ID="hdnsubgroupid" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Disease" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDisease" runat="server" SkinID="textbox" ReadOnly="true" Width="300px"></asp:TextBox>
                        <asp:HiddenField ID="hdndiseaseid" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="ICD Code" SkinID="label"></asp:Label>
                        <span id="span1" runat="server" style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtIcdCode" runat="server" SkinID="textbox" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtIcdCode"
                            SetFocusOnError="true" Display="None" ErrorMessage="Please Enter ICDCode !" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        &nbsp; &nbsp;
                        <asp:Label ID="Label6" runat="server" Text="Status" SkinID="label"></asp:Label>
                        &nbsp; &nbsp;
                        <telerik:RadComboBox ID="ddlStatus" runat="server" Width="115px">
                            <Items>
                                <telerik:RadComboBoxItem Value="1" Text="Active" Selected="true" />
                                <telerik:RadComboBoxItem Value="0" Text="In-Active" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Disease" SkinID="label"></asp:Label>
                        <span id="span2" runat="server" style="color: Red;">*</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSubDisease" runat="server" SkinID="textbox" MaxLength="200" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSubDisease"
                            SetFocusOnError="true" Display="None" ErrorMessage="Please Enter Description !"
                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                            ShowSummary="False" ValidationGroup="Save" />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="gvDetails" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                            Height="330" ShowFooter="false" GridLines="Both" AllowPaging="false" PageSize="10"
                            Skin="Office2007" Width="70%" AllowFilteringByColumn="true" OnPreRender="gvDetails_PreRender"
                            OnItemCommand="gvDetails_ItemCommand">
                            <HeaderStyle HorizontalAlign="Center" />
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                <Scrolling AllowScroll="True" UseStaticHeaders="true" />
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="ICDId" TableLayout="Fixed">
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Sl. No" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Container.DataSetIndex + 1 %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ICDCode" UniqueName="ICDCode" HeaderStyle-Width="100px"
                                        ItemStyle-Width="100px" AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblICDCode" Text='<%#Eval("ICDCode")%>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdnICDId" Value='<%#Eval("ICDId")%>' runat="server" />
                                            <asp:HiddenField ID="hdnGroupId" Value='<%#Eval("GroupId")%>' runat="server" />
                                            <asp:HiddenField ID="hdnSubGroupId" Value='<%#Eval("SubGroupId")%>' runat="server" />
                                            <asp:HiddenField ID="hdnDiseaseId" Value='<%#Eval("DiseaseId")%>' runat="server" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Disease" ItemStyle-Width="330px" HeaderStyle-Width="330px"
                                        ShowFilterIcon="false" FilterControlWidth="285px" DataField="Description" SortExpression="Description"
                                        AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" Text='<%#Eval("Description")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="100px" ItemStyle-Width="100px"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnActive" Value='<%#Eval("Active")%>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="60">
                                    </telerik:GridButtonColumn>
                                    <%--  <asp:CommandField HeaderText='Edit' ControlStyle-ForeColor="Blue" SelectText="Edit"
                                    ShowSelectButton="true" ItemStyle-Width="30px">
                                    <ControlStyle ForeColor="Blue" />
                                </asp:CommandField>--%>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
