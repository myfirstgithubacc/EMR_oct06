<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ImmunizationBrandTagging.aspx.cs" Inherits="EMR_Masters_ImmunizationBrandTagging" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />


    <asp:UpdatePanel ID="Update1" runat="server">
        <ContentTemplate>


            <div class="container-fluid header_main">

                <div class="col-md-3">
                    <h2>Immunization Brand Tagging</h2>
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
                    <div class="col-md-1 label1" style="margin-right:25px;"><asp:Label ID="lbl" runat="server" Text="Immunization"></asp:Label></div>
                        <div class="col-md-5 form-group">
                            <telerik:RadComboBox ID="ddlImmunization" runat="server" SkinID="Metro" Width="100%" MaxHeight="400px"
                                AutoPostBack="true" EmptyMessage="[ Select ]" OnSelectedIndexChanged="ddlImmunization_SelectedIndexChanged"
                                ></telerik:RadComboBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12 form-group">
                <div class="row">
                    <div class="col-md-6">
                        <div class=" table table-responsive">
                            <asp:Panel ID="pnlDepart" runat="server" Height="470px" Width="100%">
                                <asp:GridView ID="gvBrand" SkinID="gridviewOrderNew" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvBrand_RowDataBound"
                                    Width="80%" ShowHeader="true" PageSize="10" PagerSettings-Mode="Numeric"  ShowHeaderWhenEmpty="true" OnPageIndexChanging="gvBrand_PageIndexChanging">
                                    <HeaderStyle Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbSelect" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                                <asp:HiddenField ID="hdnItemBrandID" runat="server" Value='<%#Eval("ItemBrandID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Brand Name" HeaderStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbItemBrandName" runat="server" Text='<%#Eval("ItemBrandName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="3%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnImmuBrandId" runat="server" Value='<%#Eval("ImmuBrandId") %>' />
                                                <asp:ImageButton ID="ibtnDelete" runat="server" OnClick="ibtnDelete_Click" ImageUrl="~/Images/DeleteRow.png" />
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
                    
                </div>
                <!-- end of row -->
            </div>
            <!--end of main-container -->
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

