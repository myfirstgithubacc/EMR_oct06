<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EMROrderExceptionsMaster.aspx.cs" Inherits="WardManagement_EMROrderExceptionsMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <script src="https://code.jquery.com/jquery-1.11.3.js"></script>

     <%--<script type="text/javascript">
          function Confirm() {
              var confirm_value = document.createElement("INPUT");
              confirm_value.type = "hidden";
              confirm_value.name = "confirm_value";
              if (confirm("Do you want to delete record?")) {
                  
                  confirm_value.value = "Yes";
              } else {
                  confirm_value.value = "No";
              }
              document.forms[0].appendChild(confirm_value);
              if (confirm_value.value == "No")
                  return false;
          }
    </script>--%>
    <script type="text/javascript">
          function Confirm() {
              return confirm("Are you sure you want to delete this?");
          }
    </script>
    <script type="text/javascript">
        function MaxLenTxt(textBox, maxLength) {
            if (parseInt(textBox.value.length) >= parseInt(maxLength)) {

                alert("Max characters allowed are " + maxLength);

                textBox.value = textBox.value.substr(0, maxLength);
            }
        }
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
                       

            <div id="Table1" runat="server">
                <div class="container-fluid header_main">

                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server"  Text="Order Exclusion Setup" Font-Bold="true" />

                    </div>
                    <div class="col-lg-4 text-center">
                        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" ForeColor="Green" />
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4 text-right pull-right">
                         <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save"    OnClick="btnSave_Click" />
                        &nbsp;
                        <asp:Button ID="btnClose" runat="server" Text="Close" Visible="false" CssClass="btn btn-default"
                            OnClientClick="window.close();" />
                        &nbsp;

                    </div>


                </div>
            </div>
           <table border="0" width="99%" cellpadding="2" cellspacing="1" align="center">
                <tr>
                    <td>
                      
                        <div class="col-md-12 col-sm-12" id="div1" runat="server">
                            <div class="row form-group">
                                <div class="col-md-1 col-sm-1 label2">
                                    <asp:Literal ID="Literal5" runat="server" Text="Type"></asp:Literal><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <telerik:RadComboBox ID="ddlType" runat="server"  Width="100%"  OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Value="0" Text="Service" />
                                            <telerik:RadComboBoxItem Value="1" Text="Formulary" />
                                        </Items>

                                    </telerik:RadComboBox>
                                </div>
                                   <div class="col-md-1 col-sm-1 label2" style="padding:0">
                                    <asp:Literal ID="Literal1" runat="server" Text="Groups"></asp:Literal><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <telerik:RadComboBox ID="ddlGroups" runat="server" EmptyMessage="Select Group Name" Filter="Contains" Width="100%"></telerik:RadComboBox>
                                </div>

                                <div class="col-md-1 col-sm-1 label2">
                                    <asp:Literal ID="Literal2" runat="server" Text="Employee"></asp:Literal><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2">
                                    <telerik:RadComboBox ID="ddlEmployee" EmptyMessage="Select Employee" runat="server" Filter="Contains" Width="100%"></telerik:RadComboBox>
                                </div>


                            </div>
                            <div class="row">
                            </div>
                        </div>

                    </td>
                </tr>
            </table>
            <table border="0" width="99%" cellpadding="2" cellspacing="1" align="center">
                <tr>
                    <td>
                       
                        <div class="col-md-12 col-sm-12" id="divWardMenu" runat="server">
                            <div class="row form-group">
                             

                                <div class="col-md-1 col-sm-1 label2" id="dprtmntlbl" runat="server">
                                    <asp:Literal ID="Literal3" runat="server" Text="Department"></asp:Literal><span style="color: Red">*</span>
                                    
                                </div>

                           
                                <div class="col-md-2 col-sm-2"  id="dprtmnt" runat="server">
                                    <telerik:RadComboBox ID="ddlDepartment" EmptyMessage="Select Department"  runat="server" Filter="Contains" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true" Width="100%"></telerik:RadComboBox>
                                </div>


                                <div class="col-md-1 col-sm-1 label2 "  id="dprtsubdeparmentlbl" runat="server" style="padding-right: 0; padding:0">
                                    <asp:Literal ID="Literal4" runat="server" Text="Sub Deparment"></asp:Literal><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2"  id="dprtsubdeparment" runat="server">
                                    <telerik:RadComboBox ID="ddlSubDeparment" EmptyMessage="Select Sub Deparment" runat="server" Filter="Contains" Width="100%"></telerik:RadComboBox>
                                </div>

                                     <div class="col-md-1 col-sm-1 label2" id="lblcategory" runat="server" visible="false">
                                    <asp:Literal ID="Literal6" runat="server" Text="Category"></asp:Literal><span style="color: Red">*</span>
                                    
                                </div>
                                <div class="col-md-2 col-sm-2"  id="divCategory" runat="server" visible="false">
                                    <telerik:RadComboBox ID="ddlCategory" EmptyMessage="Select Category" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"  runat="server" Filter="Contains" Width="100%"></telerik:RadComboBox>
                                </div>                                
                                <div class="col-md-1 col-sm-1 label2 "  id="lblSubCategory" runat="server" visible="false" style="padding-right: 0; padding:0">
                                    <asp:Literal ID="Literal7" runat="server" Text="Sub Category"></asp:Literal><span style="color: Red">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2"  id="duvsubcategory" visible="false" runat="server">
                                    <telerik:RadComboBox ID="ddlsubCategory" EmptyMessage="Select Sub Category" runat="server" Filter="Contains" Width="100%"></telerik:RadComboBox>
                                </div>


                            </div>
                            <div class="row">
                            </div>
                        </div>

                    </td>
                </tr>
            </table>

            <div class="container-fluid form-groupTop">
                <div class="row">

                    <telerik:RadGrid ID="gvOrderApproval" runat="server" RenderMode="Lightweight" Skin="Office2007"
                        AutoGenerateColumns="false" ItemStyle-Height="30px"
                        BorderWidth="0" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="true"
                        ShowStatusBar="true" EnableLinqExpressions="false" AllowPaging="true" AllowAutomaticDeletes="false"
                        ShowFooter="false" PageSize="50" AllowCustomPaging="false"
                        AllowSorting="true" OnPreRender="gvOrderApproval_PreRender" OnItemCommand="gvOrderApproval_ItemCommand">
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" />

                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                            <Scrolling AllowScroll="True" ScrollHeight="450px" UseStaticHeaders="true" FrozenColumnsCount="6" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Auto">

                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="false" />
                            <Columns>                                

                                <telerik:GridTemplateColumn HeaderText="Group Name" SortExpression="GroupName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>


                                    </ItemTemplate>
                                    <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Employee Name" SortExpression="EmployeeName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>


                                    </ItemTemplate>
                                    <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Sub Deparment" SortExpression="SubName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubName" runat="server" Text='<%# Eval("SubName") %>'></asp:Label>

                                    </ItemTemplate>
                                    <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Delete" SortExpression="SubName">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                            CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("id")%>'
                                            ImageUrl="~/Images/DeleteRow.png"  OnClientClick="return Confirm(); "  Width="16px" Height="16px" />

                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>


                    <telerik:RadGrid ID="gvOrderFormularyby" runat="server" RenderMode="Lightweight" Skin="Office2007"
                        AutoGenerateColumns="false" ItemStyle-Height="30px"
                        BorderWidth="0" PagerStyle-ShowPagerText="false" AllowFilteringByColumn="false" AllowMultiRowSelection="true"
                        ShowStatusBar="true" EnableLinqExpressions="false" AllowPaging="true" AllowAutomaticDeletes="false"
                        ShowFooter="false" PageSize="50" AllowCustomPaging="false"
                        AllowSorting="true" OnPreRender="gvOrderFormularyby_PreRender" OnItemCommand="gvOrderFormularyby_ItemCommand" Visible="false">
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                            <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="false" />

                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                AllowColumnResize="false" />
                            <Scrolling AllowScroll="True" ScrollHeight="450px" UseStaticHeaders="true" FrozenColumnsCount="6" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Auto">

                            <NoRecordsTemplate>
                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size: 11px;">No Record Found.</div>
                            </NoRecordsTemplate>
                            <ItemStyle Wrap="false" />
                            <Columns>                                

                                <telerik:GridTemplateColumn HeaderText="Group Name" SortExpression="GroupName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:Label>


                                    </ItemTemplate>
                                    <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Employee Name" SortExpression="EmployeeName">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>'></asp:Label>


                                    </ItemTemplate>
                                    <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Sub Category" SortExpression="SubCategory">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSubName" runat="server" Text='<%# Eval("ItemSubCategoryName") %>'></asp:Label>

                                    </ItemTemplate>
                                    <HeaderStyle Width="250px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Delete" SortExpression="SubName">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDelete" runat="server" ToolTip="Click here to delete this record"
                                            CommandName="ItemDelete" CausesValidation="false" CommandArgument='<%#Eval("id")%>'
                                            ImageUrl="~/Images/DeleteRow.png"  OnClientClick="return Confirm(); "  Width="16px" Height="16px" />

                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" HorizontalAlign="Left"></HeaderStyle>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

