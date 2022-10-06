<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="CommonInvestigations.aspx.cs" Inherits="EMR_Orders_CommonInvestigations"
    Title="" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function addDiagnosisSerchOnClientClose(oWnd, args) {
            $get('<%=btnAddOrderSetClose.ClientID%>').click();
        }            
    </script>
       
    
         
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-3"><h2>Order Sets</h2></div>
                <div class="col-md-8 col-sm-7 text-center">
                    <asp:UpdatePanel ID="updMessage" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                
                <div class="col-md-1 col-sm-2 text-right">
                    <asp:UpdatePanel ID="updFrm" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnFormSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnFormSave_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvSelectedServices" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>


            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-4 col-sm-4">
                        <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Height="580px">
                            <div class="row form-group">
                                <div class="col-md-3 col-sm-3">Department</div>
                                <div class="col-md-9 col-sm-9 PaddingLeftSpacing">
                                    <asp:UpdatePanel ID="updInvSetName" runat="server">
                                        <ContentTemplate>
                                            <telerik:RadComboBox ID="ddlInvSetName" runat="server" AutoPostBack="true"
                                                Width="100%" OnSelectedIndexChanged="ddlInvSetName_SelectedIndexChanged" AppendDataBoundItems="true">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="All" Value="0" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvServices" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                            <div class="row form-group">
                                <asp:UpdatePanel ID="updSearch" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-3 col-sm-5">
                                            <telerik:RadComboBox ID="ddlSearchCriteria" runat="server" Width="100%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Text="Anywhere" Value="1" />
                                                    <telerik:RadComboBoxItem Text="Starts With" Value="2" />
                                                    <telerik:RadComboBoxItem Text="Ends With" Value="3" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-md-6 col-sm-4 PaddingSpacing"><asp:TextBox ID="txtSearch" Width="100%" runat="server" /></div>
                                        <div class="col-md-3 col-sm-3 text-left"><asp:Button ID="btnSearch" CssClass="btn btn-primary" Font-Size="10px" Text="Search" runat="server" OnClick="btnSearch_Click" /></div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvServices" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12 col-sm-12"><asp:Literal ID="ltrAllServices" runat="server" Text="All Services"></asp:Literal></div>
                            </div>

                            <div class="row form-group">
                                <div class="col-md-12 col-sm-12">
                                    <asp:UpdatePanel ID="updService" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlServices" runat="server" Width="100%" ScrollBars="Auto">
                                                <asp:GridView ID="gvServices" SkinID="gridviewOrderNewP" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                                    DataKeyNames="ServiceID" ShowHeader="true" PageSize="10" Width="100%" AllowPaging="true"
                                                    PagerSettings-Mode="NumericFirstLast" PageIndex="0" ShowFooter="false" PagerSettings-Visible="true"
                                                    OnRowDataBound="gvServices_OnRowDataBound" OnPageIndexChanging="gvServices_OnPageIndexChanging"
                                                    OnSelectedIndexChanged="gvServices_SelectedIndexChanged">
                                                    <Columns>
                                                        <asp:BoundField DataField="ServiceID" HeaderText="ServiceID" Visible="true" ReadOnly="true" />
                                                        <asp:BoundField DataField="CPTCode" HeaderText="ServiceID" Visible="true" ReadOnly="true" />
                                                        <asp:TemplateField HeaderText="Service Name" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblServiceName" Text='<%#Eval("ServiceName")%>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField HeaderText="Select" ButtonType="Link" ControlStyle-ForeColor="Blue"
                                                            ControlStyle-Font-Underline="true" SelectText="Select" CausesValidation="false"
                                                            ShowSelectButton="true">
                                                            <ControlStyle Font-Underline="True" ForeColor="Blue" />
                                                        </asp:CommandField>
                                                    </Columns>
                                                    <PagerSettings PageButtonCount="6" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvSelectedServices" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>


                    <div class="col-md-8 col-sm-8">
                        <div class="row form-group">
                            <asp:Panel ID="pnlInvestigationOptions" runat="server">
                                <div class="col-md-3 col-sm-3"><asp:Literal ID="ltrlInvestigationSet" runat="server" Text="Order Set"></asp:Literal></div>
                                <div class="col-md-6 col-sm-7">
                                    <telerik:RadComboBox ID="ddlInvestigationSet" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlInvestigationSet_SelectedIndexChanged" EmptyMessage="/Select"
                                        Width="100%">
                                    </telerik:RadComboBox>
                                </div>
                                <div class="col-md-3 col-sm-2 text-left">
                                    <asp:ImageButton ID="ibtnAddOrderSet" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                        ToolTip="Add Order Set" Height="17px" Width="17px" OnClick="ibtnAddOrderSet_Click"
                                    CausesValidation="false" />
                                </div>
                            </asp:Panel>
                        </div>

                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12"><asp:Label ID="ltrlSelectedServices" runat="server" Text="Selected Services" /></div>
                        </div>

                        <div class="row form-group">
                            <div class="col-md-12 col-sm-12">
                                <asp:UpdatePanel ID="updSelectedservices" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="None" Height="580px">
                                            <asp:GridView ID="gvSelectedServices" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                                OnRowDataBound="gvSelectedServices_RowDataBound" Width="100%" ShowHeader="true"
                                                DataKeyNames="ID" OnRowCommand="gvSelectedServices_RowCommand" PageSize="20"
                                                AllowPaging="true" PageIndex="0" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="gvSelectedServices_PageIndexChanging"
                                                OnSelectedIndexChanged="gvSelectedServices_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:BoundField DataField="ID" />
                                                    <asp:BoundField DataField="ServiceID" />
                                                    <asp:BoundField DataField="ServiceName" HeaderText="Service Name" ItemStyle-Wrap="true"
                                                        HeaderStyle-HorizontalAlign="Left" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                                CommandName="Del" CommandArgument='<%#Eval("ServiceID")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                    </div>



                </div>

                <div class="row">
                    <asp:Button ID="btnAddOrderSetClose" runat="server" Style="visibility: hidden;"
                        OnClick="btnAddOrderSetClose_OnClick" />
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server"
                        Width="1200" Height="600" Left="10" Top="10">
                        <Windows>
                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin"
                                Width="900" Height="600" />
                        </Windows>
                    </telerik:RadWindowManager>
                </div>

            </div>
    
    </ContentTemplate>     
    </asp:UpdatePanel> 
</asp:Content>