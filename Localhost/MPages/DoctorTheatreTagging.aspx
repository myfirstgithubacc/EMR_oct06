<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="DoctorTheatreTagging.aspx.cs" Inherits="MPages_DoctorTheatreTagging" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    

    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-3"><h2>Doctor and Operation Theater Tagging</h2></div>
                <div class="col-md-6 col-sm-6 text-center"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></div>
                <div class="col-md-3 col-sm-3 text-right">
                    <asp:Button ID="btnNew" runat="server" CausesValidation="false" CssClass="btn btn-default" Text="New" OnClick="btnNew_OnClick" />
                    <asp:Button ID="btnSave" ToolTip="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="save" CausesValidation="true" Text="Save" OnClick="btnSave_OnClick" />
                </div>
            </div>
            <div class="container-fluid">
                <div class="row form-group">
                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-4 label2"><asp:Label ID="lblDept" runat="server" Text="Doctor" /></div>
                            <div class="col-md-9 col-sm-8">
                                <telerik:RadComboBox ID="ddlDoctor" runat="server" MarkFirstMatch="true" Filter="Contains"
                                    EnableLoadOnDemand="true" EmptyMessage="[Select Doctor]" Width="100%" Height="350px"
                                    EnableVirtualScrolling="true" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-4 label2"><asp:Label ID="lblTheatre" runat="server" Text="Theater" /></div>
                            <div class="col-md-9 col-sm-8">
                                <telerik:RadComboBox ID="ddlTheatre" runat="server" EmptyMessage="Select Theatre" Width="100%"
                                    EnableVirtualScrolling="true" CheckBoxes="false" EnableCheckAllItemsCheckBox="false" CssClass="drapDrowHeight"/>
                            </div>
                        </div>
                    </div>

                      <div class="col-md-4 col-sm-4">
                        <div class="row">
                            <div class="col-md-3 col-sm-4 label2"><asp:Label ID="LblWeekDay" runat="server" Text="WeekDay" /></div>
                            <div class="col-md-6 col-sm-5">
                                <telerik:RadComboBox ID="ddlWeekDay" runat="server" EmptyMessage="Select WeekDay" Width="100%"
                                    EnableVirtualScrolling="true" CheckBoxes="True" EnableCheckAllItemsCheckBox="True" CssClass="drapDrowHeight"/>                                 
                            </div>
                        </div>
                       </div>
                  </div>

                <div class="row form-group">
                    <asp:Panel ID="pnlGrd" runat="server" ScrollBars="Horizontal" Height="430px" Width="100%">
                        <asp:GridView ID="gvDoctor" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="false" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" OnClick="lnkSelect_OnClick"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<% #Eval("EmployeeNo") %>' />
                                        <asp:HiddenField ID="hdnTheatreId" runat="server" Value='<% #Eval("TheatreId") %>' />
                                        <asp:HiddenField ID="hdnWeekDay" runat="server" Value='<% #Eval("WeekDay") %>' />
                                        <asp:HiddenField ID="hdnWeekDayName" runat="server" Value='<% #Eval("WeekDayName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EmployeeName" HeaderText="Doctor Name" />
                                <asp:BoundField DataField="TheatreName" HeaderText="Theatre Name" />
                                <asp:BoundField DataField="WeekDayName" HeaderText="Week Day Name" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
