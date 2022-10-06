<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="OTCheckListMaster.aspx.cs" Inherits="OTScheduler_OTCheckListMaster" Title="OT Standard checkList Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="../Include/EMRStyle.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/bootstrap.min.css" media="all" />
    <link rel="stylesheet" type="text/css" href="../../Include/css/mainNew.css" media="all" />

        <script language="javascript" type="text/javascript">
            function AutoChange() {
                //
                var txt = $get('<%=txtDescription.ClientID%>');
                //alert(txt.value.length);
                if (txt.value.length > 200) {
                    alert("Text length should not be greater then 250.");

                    txt.value = txt.value.substring(0, 250);
                    txt.focus();
                }
            }
        </script>
    </telerik:RadCodeBlock>

    <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server" DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
    <div id="dvZone1" style="width: 100%">
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row header_main">
                    <div class="col-md-3 col-sm-3 col-xs-12">OT Standard CheckList</div>
                    <div class="col-md-6 col-sm-6 col-xs-12 text-center"><asp:Label ID="lblMassage" runat="server" Font-Bold="true"></asp:Label></div>
                    <div class="col-md-3 col-sm-3 col-xs-12 text-right">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Refresh" OnClick="btnClear_Click" />
                    </div>
                </div>

                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-2 col-sm-2 col-xs-3"><asp:Label ID="Label1" runat="server" Text="Description"></asp:Label></div>
                                <div class="col-md-10 col-sm-10 col-xs-9"><asp:TextBox ID="txtDescription" runat="server" MaxLength="250" Style="height: 60px; width: 100%;" TextMode="MultiLine" onkeyup="return AutoChange();"></asp:TextBox></div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-2 col-sm-2 col-xs-3"><asp:Label ID="Label2" runat="server" Text="Type"></asp:Label></div>
                                <div class="col-md-10 col-sm-10 col-xs-9">
                                    <telerik:RadComboBox ID="ddlType" runat="server" CssClass="drapDrowHeight" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="Y/N" Text="Y/N" Selected="true" />
                                            <telerik:RadComboBoxItem Value="Date-Time" Text="Date-Time" />
                                            <telerik:RadComboBoxItem Value="Text" Text="Text" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-12">
                            <div class="row p-t-b-5">
                                <div class="col-md-2 col-sm-2 col-xs-3"><asp:Label ID="Label3" runat="server" Text="Status"></asp:Label></div>
                                <div class="col-md-10 col-sm-10 col-xs-9">
                                    <telerik:RadComboBox ID="ddlStatus" runat="server" CssClass="drapDrowHeight" Width="100%">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="1" Text="Active" Selected="true" />
                                            <telerik:RadComboBoxItem Value="0" Text="InActive" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12 col-sm-12 gridview">
                        <telerik:RadGrid ID="gvDetails" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                            Width="100%" Height="380" ShowFooter="false" CssClass="table table-condensed" GridLines="Both" AllowPaging="true"
                            PageSize="10" ForeColor="Black" OnPreRender="gvDetails_PreRender" OnItemCommand="gvDetails_ItemCommand">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true" Scrolling-AllowScroll="true"
                                Scrolling-UseStaticHeaders="true" Scrolling-SaveScrollPosition="true">
                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                            </ClientSettings>
                            <MasterTableView DataKeyNames="Id" Width="100%" TableLayout="Fixed">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Id" UniqueName="Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="txtId" runat="server" Text='<%#Eval("Id")%>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Description" UniqueName="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="txtDescription" runat="server" Text='<%#Eval("Description")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Type" UniqueName="ValueType" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="txtValueType" runat="server" Text='<%#Eval("ValueType")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Status" AllowFiltering="false" ItemStyle-Wrap="false"
                                        HeaderStyle-Wrap="false" HeaderText='<%$ Resources:PRegistration, status%>' HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="txtStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Active" Visible="false" HeaderText="Active"
                                        HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblActive" runat="server" Text='<%#Eval("Active")%>' Width="100px" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn Text="Edit" CommandName="Select" HeaderStyle-Width="50px"
                                        HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                        </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>