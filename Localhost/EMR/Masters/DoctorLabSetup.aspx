<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="DoctorLabSetup.aspx.cs" Inherits="EMR_Masters_DoctorLabSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />



    <asp:UpdatePanel ID="Update1" runat="server">
        <ContentTemplate>


            <div class="container-fluid header_main">

                <div class="col-md-3">
                    <h2>Doctor Outside Lab Setup</h2>
                </div>
                <div class="col-md-5 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="alert_new text-center text-success relativ" />
                </div>
                <div class="col-md-3 text-right pull-right">
                    <asp:Button ID="btnNew" runat="server" Width="100px" Text="New" OnClick="btnNew_Click" CssClass="btn btn-default" />
                    <asp:Button ID="btnSave" runat="server" Width="100px" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />

                </div>

            </div>
            <div class="col-md-12 form-group">
                <div class="row">
                    <div class="col-md-6">
                    <div class="col-md-1 label1"> Doctor</div>
                        <%--<asp:Label ID="Label1" runat="server" Text="Doctor"></asp:Label>--%>
                        <div class="col-md-5 form-group">
                            <telerik:RadComboBox ID="ddlDoctor" SkinID="Metro" runat="server" Width="100%" MaxHeight="400px"
                                AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                                OnSelectedIndexChanged="ddlDoctor_SelectedIndexChanged" />
                        </div>
                        <div class="col-md-1 label1"> Service</div>
                        <%--<asp:Label ID="Label2" runat="server" Text="Service"></asp:Label>--%>
                        <div class="col-md-5 form-group">
                            <telerik:RadComboBox ID="ddlServices" SkinID="Metro" runat="server" Width="100%" MaxHeight="400px"
                                AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                                OnSelectedIndexChanged="ddlServices_SelectedIndexChanged" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12 form-group">
                <div class="row">
                    <div class="col-md-6">
                        <div class=" table table-responsive">
                            <asp:Panel ID="pnlDepart" runat="server" Height="470px" Width="100%">
                                <asp:GridView ID="gvFields" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                    Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvFields_PageIndexChanging">
                                    <HeaderStyle Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSelect" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                                <asp:HiddenField ID="hdnFieldID" runat="server" Value='<%#Eval("FieldID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field Name" HeaderStyle-Width="95%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbFieldName" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Record(s)
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <asp:Panel ID="Panel1" runat="server" Height="450px" Width="100%" ScrollBars="Vertical">
                            <asp:GridView ID="gvFieldData" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false"
                                Width="100%" ShowHeader="true" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="20" OnPageIndexChanging="gvFieldData_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Docor Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbDoctorName" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbServiceName" runat="server" Text='<%#Eval("ServiceName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Field Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbFieldName" runat="server" Text='<%#Eval("FieldName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png" OnClick="ibtnDelete_Click" />
                                            <asp:HiddenField ID="hdnLabResultSetupId" runat="server" Value='<%#Eval("LabResultSetupId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No record(s)
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>
                <!-- end of row -->
            </div>
            <!--end of main-container -->
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

