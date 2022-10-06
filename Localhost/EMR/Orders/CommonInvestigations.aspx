<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="CommonInvestigations.aspx.cs" Inherits="EMR_Orders_CommonInvestigations"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- Bootstrap -->
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function addDiagnosisSerchOnClientClose(oWnd, args) {
            $get('<%=btnAddOrderSetClose.ClientID%>').click();
     }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>



            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="VisitHistoryDivText">
                                <h2>Order Sets</h2>
                            </div>
                        </div>
                        <div class="col-md-8"></div>
                        <div class="col-md-1">
                            <asp:UpdatePanel ID="updFrm" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnFormSave" runat="server" CssClass="AdminBtn01" Text="Save" OnClick="btnFormSave_Click" /></ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvSelectedServices" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>





            <div class="AuditTrailDiv">

                <asp:Panel ID="pnl" runat="server" ScrollBars="Auto" Height="480px">
                    <div class="AuditTrailDiv">
                        <div class="container-fluid">

                            <div class="row">


                                <div class="col-md-5">

                                    <div class="row">
                                        <div class="col-md-3"><span class="DepartmentText">Department</span></div>
                                        <div class="col-md-9">
                                            <span class="orderDepartment">
                                                <asp:UpdatePanel ID="updInvSetName" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadComboBox ID="ddlInvSetName" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlInvSetName_SelectedIndexChanged" AppendDataBoundItems="true">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="All" Value="0" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvServices" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-md-3"></div>
                                        <div class="col-md-9">
                                            <span class="orderDepartment01">

                                                <asp:UpdatePanel ID="updSearch" runat="server">
                                                    <ContentTemplate>

                                                        <telerik:RadComboBox ID="ddlSearchCriteria" CssClass="orderDepartmentDropdown" runat="server">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="Anywhere" Value="1" />
                                                                <telerik:RadComboBoxItem Text="Starts With" Value="2" />
                                                                <telerik:RadComboBoxItem Text="Ends With" Value="3" />
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                        <asp:TextBox ID="txtSearch" CssClass="orderDepartmentInput" runat="server" />
                                                        <asp:Button ID="btnSearch" CssClass="OrderSearchBtn" Text="Search" runat="server" OnClick="btnSearch_Click" />



                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="gvServices" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </span>
                                        </div>

                                    </div>


                                    <div class="row">

                                        <div class="col-md-12"><span class="Order-ASText">
                                            <asp:Literal ID="ltrAllServices" runat="server" Text="All Services"></asp:Literal></span><br />
                                        </div>

                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="updService" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlServices" runat="server" Width="100%" ScrollBars="Auto">

                                                        <asp:GridView ID="gvServices" SkinID="gridviewOrderNew" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                                            DataKeyNames="ServiceID" ShowHeader="true" PageSize="10" Width="100%" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" PageIndex="0" ShowFooter="false" PagerSettings-Visible="true"
                                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                            BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px"
                                                            OnRowDataBound="gvServices_OnRowDataBound" OnPageIndexChanging="gvServices_OnPageIndexChanging"
                                                            OnSelectedIndexChanged="gvServices_SelectedIndexChanged">
                                                            <Columns>
                                                                <asp:BoundField DataField="ServiceID" HeaderText="ServiceID" Visible="true" ReadOnly="true" />
                                                                <asp:BoundField DataField="CPTCode" HeaderText="ServiceID" Visible="true" ReadOnly="true" />
                                                                <asp:TemplateField HeaderText="Service Name" ItemStyle-Wrap="true" ItemStyle-Width="92%" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServiceName" Text='<%#Eval("ServiceName")%>' runat="server" /></ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:CommandField HeaderText="Select"
                                                                    ControlStyle-Font-Underline="true" SelectText="Select" CausesValidation="false"
                                                                    ShowSelectButton="true">
                                                                    <ControlStyle Font-Underline="True" CssClass="text-center OrderSelectBtn" />
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
                                </div>



                                <div class="col-md-7">

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="updMessage" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblMessage" runat="server"></asp:Label></ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                        <div class="col-md-12">
                                            <asp:Panel ID="pnlInvestigationOptions" runat="server">
                                                <span class="Order-ASText">
                                                    <asp:Literal ID="ltrlInvestigationSet" runat="server" Text="Order Set"></asp:Literal></span>
                                                <telerik:RadComboBox ID="ddlInvestigationSet" CssClass="Order-ASInput" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvestigationSet_SelectedIndexChanged" EmptyMessage="/Select"></telerik:RadComboBox>
                                                <asp:ImageButton ID="ibtnAddOrderSet" runat="server" ImageUrl="~/Images/PopUp.jpg" CssClass="Order-ASIcon" ToolTip="Add Order Set" Height="17px" Width="17px" OnClick="ibtnAddOrderSet_Click" CausesValidation="false" />
                                            </asp:Panel>
                                        </div>


                                        <div class="col-md-12">
                                            <asp:Label ID="ltrlSelectedServices" runat="server" CssClass="orderDepartment-SS" Text="Selected Services" /></div>

                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="updSelectedservices" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="None" Height="580px">
                                                        <asp:GridView ID="gvSelectedServices" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                                            OnRowDataBound="gvSelectedServices_RowDataBound" Width="100%" ShowHeader="true"
                                                            DataKeyNames="ID" OnRowCommand="gvSelectedServices_RowCommand" PageSize="20"
                                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px"
                                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0"
                                                            BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                                                            AllowPaging="true" PageIndex="0" PagerSettings-Mode="NumericFirstLast" OnPageIndexChanging="gvSelectedServices_PageIndexChanging"
                                                            OnSelectedIndexChanged="gvSelectedServices_SelectedIndexChanged">

                                                            <Columns>
                                                                <asp:BoundField DataField="ID" />
                                                                <asp:BoundField DataField="ServiceID" />
                                                                <asp:BoundField DataField="ServiceName" HeaderText="Service Name" ItemStyle-Wrap="true" ItemStyle-Width="95%" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png" CommandName="Del" CommandArgument='<%#Eval("ServiceID")%>' /></ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>



                                        <div class="col-md-12">
                                            <asp:Button ID="btnAddOrderSetClose" runat="server" Style="visibility: hidden;" OnClick="btnAddOrderSetClose_OnClick" />
                                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server" Width="1200" Height="600" Left="10" Top="10">
                                                <Windows>
                                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin" Width="900" Height="600" />
                                                </Windows>
                                            </telerik:RadWindowManager>
                                        </div>

                                    </div>






                                </div>







                            </div>

                        </div>
                    </div>

                </asp:Panel>




            </div>


        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
