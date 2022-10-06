<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Status.aspx.cs" Inherits="EMR_Assessment_Status" Title="" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <style type="text/css">  </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
       <asp:UpdatePanel ID="upAddType" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlAddType" BackColor="White" runat="server" Width="100%" ScrollBars="Auto">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12 text-center">
                                 <asp:ValidationSummary ID="valsumaury1" runat="server" ValidationGroup="s" ShowMessageBox="true" ShowSummary="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-12"  id="ns" runat="server">
                                <div class="row p-t-b-5">
                                    <div class="col-md-3 col-sm-3 col-xs-3 text-nowrap">
                                        <asp:Literal ID="ltrName" runat="server" Text="Condition Name"></asp:Literal>
                                    </div>
                                    <div class="col-md-9 col-sm-9 col-xs-9">
                                        <div class="row">
                                            <div class="col-md-10 col-sm-10 col-xs-9">
                                                <asp:TextBox ID="txtstatus" runat="server" SkinID="textbox"  
                                                MaxLength="50" Width="100%"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="Reqval1" runat="server" ControlToValidate="txtstatus" Text="Enter Description" ErrorMessage="Please enter description" Display="None" 
                                                  ValidationGroup="s"  ></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-2 col-sm-2 col-xs-3">
                                                <asp:Button ID="btnAdd" Text="Save" runat="server" CssClass="btn btn-primary" ValidationGroup="s"
                                                OnClick="btnAdd_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row m-t">
                            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                                <asp:GridView ID="lstTypeList" runat="server" AutoGenerateColumns="False"
                                    DataKeyNames="StatusId" HeaderStyle-HorizontalAlign="Left" Style="margin-bottom: 0px"
                                    Width="100%" OnRowEditing="lstTypeList_RowEditing" EditRowStyle-HorizontalAlign="Left"
                                    OnRowUpdating="lstTypeList_RowUpdating" OnRowCancelingEdit="lstTypeList_RowCancelingEdit"
                                    OnRowDataBound="lstTypeList_RowDataBound" 
                                    OnRowDeleting="lstTypeList_RowDeleting" 
                                    onpageindexchanging="lstTypeList_PageIndexChanging" AllowPaging="True" CssClass="table-ui">
                                    <PagerSettings PageButtonCount="6" />
                                    <RowStyle Wrap="false" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Description" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltasktype" runat="server" Text='<%# Eval("Description")%>'> </asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtstatusname" runat="server" SkinID="textbox" Text='<%# Eval("Description")%>'
                                                    MaxLength="50"></asp:TextBox>
                                            </EditItemTemplate>
                                            <HeaderStyle Wrap="False" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="True" />
                                        </asp:TemplateField>
                                        <asp:CommandField ButtonType="Image" ValidationGroup="Delete" DeleteImageUrl="~/Images/DeleteRow.png"
                                            ShowDeleteButton="true">
                                            <ItemStyle Width="20px" />
                                        </asp:CommandField>
                                        <asp:CommandField ButtonType="Link" SelectText="Edit" ValidationGroup="Edit" ShowEditButton="true">
                                            <ItemStyle Width="20px" />
                                        </asp:CommandField>
                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditRowStyle HorizontalAlign="Left" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row">
                            <asp:Label ID ="casetype" runat="server" style=" visibility:hidden; " ></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>

</body>
</html>
