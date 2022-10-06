<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/EMRMaster.master"
    CodeFile="DocumentCategory.aspx.cs" Inherits="EMR_DocumentCategory" Title="Docment Type" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
 <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="/Include/JS/Common1.js"></script>

    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
           
            
                 <div class="container-fluid header_main">
                    <div class="col-xs-3">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                                    <h2 style="line-height:23px;"><asp:Label ID="lblCatName" runat="server" Text=""></asp:Label></h2>
                     </div>
                 
                     <div class="col-xs-4 form-group11">
                        <asp:TextBox ID="txtCatName" runat="server" ></asp:TextBox>
                     </div>
                     <div class="col-xs-5">
                         <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" CssClass="btn btn-primary" />
                        &nbsp;
                        <asp:Button ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" CssClass="btn btn-primary"  />
                     
                     </div>
                     
                     
                 </div>
            
            
            
                        <div style="overflow-y: scroll; overflow-x: visible; height: 89vh;">
                            <asp:GridView ID="gvDocumentCategory" runat="server" SkinID="gridview" AutoGenerateColumns="False"
                                OnRowDataBound="gvDocumentCategory_RowDataBound" Width="99%" OnRowCancelingEdit="gvDocumentCategory_RowCancelingEdit"
                                OnRowEditing="gvDocumentCategory_RowEditing" OnRowUpdating="gvDocumentCategory_RowUpdating">
                                <Columns>
                                    <asp:BoundField DataField="id" ReadOnly="true" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Name</HeaderTemplate>
                                        <ItemTemplate>
                                            <%#Eval("Description")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txteditdescription" runat="server" SkinID="textbox" Columns="35"
                                                MaxLength="50" Text='<%#Eval("Description") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="250px" HorizontalAlign="Left" />
                                        <ItemStyle Width="250px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlactive" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="true" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                                        <HeaderStyle Width="90px" />
                                        <ItemStyle Width="90px" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnValue" runat="server" />
</asp:Content>
