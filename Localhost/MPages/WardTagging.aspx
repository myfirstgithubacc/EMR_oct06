<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="WardTagging.aspx.cs" Inherits="MPages_WardTagging" Title="Ward Tagging" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />   

    <style>
        .RadGrid_Office2007 .rgHeader, .RadGrid_Office2007 th.rgResizeCol { border: solid #868686 1px !important; border-top:none !important; outline:none !important; color:#333; background: 0 -2300px repeat-x #c1e5ef !important;}
        .RadGrid_Office2007 td.rgGroupCol, .RadGrid_Office2007 td.rgExpandCol {background-color:#fff !important;}
        #ctl00_ContentPlaceHolder1_Panel1 { background-color:#c1e5ef !important;}
        .RadGrid .rgFilterBox {height:20px;}
    </style>


    <asp:UpdatePanel ID="Updatepanel1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-2 col-sm-3"><h2><asp:Label ID="Label2" runat="server" Text="Ward Tagging" ToolTip="Ward Tagging"></asp:Label></h2></div>
                <div class="col-md-7 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New Record" Text="New" CssClass="btn btn-default" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" OnClick="btnSaveData_OnClick" CssClass="btn btn-primary" Text="Save" />
                </div>
            </div>


            <div class="container-fluid">
                <div class="row form-group">

                    <div class="col-md-3 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Literal ID="Literal1" runat="server" Text="Tagging"></asp:Literal></span></div>
                            <div class="col-md-8 col-sm-7">
                                <asp:DropDownList ID ="ddlTagging" runat="server" OnSelectedIndexChanged="ddlTagging_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Ward" Value="0"/>
                                        <asp:ListItem Text="OT" Value="1" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-3 col-sm-4">
                        <div class="row">
                            <div class="col-md-4 col-sm-5 label2"><span class="textName"><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:PRegistration, employeetype %>"></asp:Literal></span></div>
                            <div class="col-md-8 col-sm-7"><telerik:RadComboBox ID="ddlemployeetype" runat="server" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlemployeetype_SelectedIndexChanged" Width="100%"></telerik:RadComboBox></div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-5">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 label2"><span class="textName"><asp:Label ID="Label1" runat="server" Text="Employee Name" ToolTip="Doctor And Nurse Name"></asp:Label>&nbsp;<span style='color: Red'>*</span></span></div>
                            <div class="col-md-8 col-sm-8"><telerik:RadComboBox ID="ddlEmployee" MarkFirstMatch="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged" Width="100%"></telerik:RadComboBox></div>
                        </div>
                    </div>
                </div>





                <div class="row form-group main_box02">
                    <div class="col-md-5 col-sm-6">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 PaddingRightSpacing">
                                <div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkAllEmp" runat="server" Font-Bold="true" Text="All&nbsp;Select&nbsp;/&nbsp;Unselect&nbsp;Employee(s)" AutoPostBack="true" OnCheckedChanged="chkAllEmp_OnCheckedChanged" /></div>
                            </div>
                            <div class="col-md-6 col-sm-6 PaddingLeftSpacing"><div class="PD-TabRadioNew01 margin_z"><asp:CheckBox ID="chkUnchk" runat="server" Font-Bold="true" Text="All&nbsp;Select&nbsp;/&nbsp;Unselect&nbsp;Ward(s)" AutoPostBack="true" OnCheckedChanged="chkUnchk_OnCheckedChanged" /></div></div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-5"><strong> <asp:Label ID ="lblTag" runat="server" /> Tagging</strong></div>
                </div>

                <div class="row form-group">
                    <div class="col-md-8 col-sm-6">
                        <div class="row">
                            <div class="col-md-6 col-sm-6 PaddingRightSpacing">
                                <asp:Panel ID="Panel3" runat="server" Height="400px" Width="100%" ScrollBars="Auto">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                                <telerik:RadGrid ID="gvEmployee" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                                AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                GridLines="Both" AllowPaging="false" Height="99%" >
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="chkEmpid" HeaderText="" HeaderStyle-Width="30px"
                                                            AllowFiltering="false">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkEmpid" runat="server"  />
                                                            </ItemTemplate>
                                                            <%--  Checked='<%#Eval("IsChk").ToString().Equals("1")%>'--%>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="EMP Id" CurrentFilterFunction="Contains"
                                                            AllowFiltering="False" DataField="ID" ShowFilterIcon="false" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmpId" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Employee Name" CurrentFilterFunction="Contains"
                                                            SortExpression="EmployeeName" UniqueName="EmployeeName" AllowFiltering="False" DataField="EmployeeName"
                                                            AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("EmployeeName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                         </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>

                               
                            </div>
                            <div class="col-md-6 col-sm-6 PaddingLeftSpacing">
                                <asp:Panel ID ="panel4" runat="server" Height="400px" Width="100%" ScrollBars="Auto" Visible="false" >
                                   <asp:UpdatePanel ID ="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                       <ContentTemplate>
                                              <telerik:RadGrid ID="gvOTList" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                                AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                GridLines="Both" AllowPaging="false" Height="99%">
                                                  <GroupingSettings CaseSensitive="false" />
                                                  <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                      <telerik:GridTemplateColumn UniqueName="chkTheatreid" HeaderText="" HeaderStyle-Width="30px"
                                                            AllowFiltering="false">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkTheatreid" runat="server" />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                         <telerik:GridTemplateColumn HeaderText="Theater Id" CurrentFilterFunction="Contains"
                                                            AllowFiltering="False" DataField="TheaterId" ShowFilterIcon="false" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTheatreId" runat="server" Text='<%#Eval("TheatreId")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="List of OT" CurrentFilterFunction="Contains"
                                                            SortExpression="TheatreName" UniqueName="TheatreName" AllowFiltering="False" DataField="TheatreName"
                                                            AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTheatreName" runat="server" Text='<%#Eval("TheatreName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                  </MasterTableView>
                                              </telerik:RadGrid>
                                       </ContentTemplate>
                                   </asp:UpdatePanel>
                               </asp:Panel>

                                <asp:Panel ID="Panel1" runat="server" Height="400px" Width="100%" ScrollBars="Auto">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <telerik:RadGrid ID="gvwardlist" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                                AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                                AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                GridLines="Both" AllowPaging="false" Height="99%" >
                                                <GroupingSettings CaseSensitive="false" />
                                                <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                    <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                    <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                        AllowColumnResize="false" />
                                                </ClientSettings>
                                                <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                                    <NoRecordsTemplate>
                                                        <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                                    </NoRecordsTemplate>
                                                    <ItemStyle Wrap="true" />
                                                    <Columns>
                                                        <telerik:GridTemplateColumn UniqueName="chkwardid" HeaderText="" HeaderStyle-Width="30px"
                                                            AllowFiltering="false">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkwardid" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="Ward Id" CurrentFilterFunction="Contains"
                                                            AllowFiltering="False" DataField="WardId" ShowFilterIcon="false" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWardId" runat="server" Text='<%#Eval("WardId")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                        <telerik:GridTemplateColumn HeaderText="List of Wards" CurrentFilterFunction="Contains"
                                                            SortExpression="WardName" UniqueName="WardName" AllowFiltering="False" DataField="WardName"
                                                            AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </telerik:GridTemplateColumn>
                                                    </Columns>
                                                </MasterTableView>
                                            </telerik:RadGrid>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 col-sm-5">
                        <asp:Panel ID="Panel2" runat="server" Height="400px" Width="400" ScrollBars="Auto">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvTagged" Skin="Office2007" BorderWidth="0" PagerStyle-ShowPagerText="false"
                                        AllowFilteringByColumn="false" AllowMultiRowSelection="true" runat="server" Width="100%"
                                        AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                        GridLines="Both" AllowPaging="false" Height="99%" OnItemDataBound="gvTagged_ItemDataBound">
                                        <GroupingSettings CaseSensitive="false" />
                                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                            <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                            <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                AllowColumnResize="false" />
                                        </ClientSettings>
                                        <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                            <NoRecordsTemplate>
                                                <div style="font-weight: bold; color: Red; float: left; text-align: center !important; width: 100% !important; margin: 0.5em 0; padding: 0; font-size:11px;">No Record Found.</div>
                                            </NoRecordsTemplate>
                                            <ItemStyle Wrap="true" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Ward Id" CurrentFilterFunction="Contains"
                                                    AllowFiltering="False" DataField="WardId" ShowFilterIcon="false" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardId" runat="server" Text='<%#Eval("WardId")%>'></asp:Label>
                                                        <asp:HiddenField Id="hdnTagId" runat="server" Value='<%# Eval("TagId") %>'/>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="List of Wards" CurrentFilterFunction="Contains"
                                                    SortExpression="WardName" UniqueName="WardName" AllowFiltering="False" DataField="WardName"
                                                    AutoPostBackOnFilter="False" ShowFilterIcon="false" FilterControlWidth="100%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                    <telerik:GridTemplateColumn>
                                                        <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEmpDelete" runat="server" Text="Delete" OnClick="lnkEmpDelete_OnClick"></asp:LinkButton>
                                                        </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                </div>
                <asp:HiddenField ID ="hdnWardID" runat="server" />
                <asp:HiddenField ID ="hdnEmpId"  runat="server" />
                <asp:HiddenField ID ="hdnOTID" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>