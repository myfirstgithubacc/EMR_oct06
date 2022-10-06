<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EMRHSetup.aspx.cs" Inherits="EMR_Masters_EMRHSetup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        th{
            padding-left:4px!important;
        }
         td{
            padding-left:4px!important;
        }
        

        #datagrid {
            width: 100% !important;
            margin-top: 10px;
        }
        .btn{
            margin-left:0px!important;
            margin-bottom:4px;
            margin-top:4px;
        }

       
      
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row ">
                <div class="col-sm-12">
                    <div class="row">

                        <div class="col-sm-6 ">
                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                        </div>
                        <div class="col-sm-6 text-right">
                            <asp:Button ID="savebtn" runat="server" CssClass="btn btn-primary" OnClick="Button2_Click" Text="Save" />
                            <asp:Button ID="clearbtn" runat="server" CssClass="btn btn-primary" OnClick="Button3_Click1" Text="Clear" />

                        </div>
                    </div>
                    <div class="row" >
                        <div class="col-sm-12">
                            <div class="row">
                                <div class="col-sm-4 form-group">
                                    <div class="row">
                                        <div class="col-sm-5" style="padding-right:0px;">
                                            <asp:Label ID="Label1" runat="server" Text="Module Name:"></asp:Label>
                                        </div>
                                        <div class="col-sm-7" style="padding-left:0px;">

                                            <asp:DropDownList ID="moduledropdrown" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="moduledropdrown_SelectedIndexChanged">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>EMR</asp:ListItem>
                                                <asp:ListItem>Ward</asp:ListItem>
                                                <asp:ListItem>OT</asp:ListItem>
                                                <asp:ListItem>Nurse</asp:ListItem>
                                                <asp:ListItem>MRD</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4 form-group">
                                    <div class="row">
                                        <div class="col-sm-4" style="padding-right:0px;"><asp:Label ID="Label6" runat="server" Text="ID:"></asp:Label></div>
                                        <div class="col-sm-8" style="padding-left:0px;"> <asp:TextBox ID="idtxtbox" runat="server" ></asp:TextBox></div>
                                    </div>
                                </div>
                                <div class="col-sm-4 form-group">
                                    <div class="row">
                                        <div class="col-sm-4" style="padding-right:0px;"><asp:Label ID="Label2" runat="server" Text="Flag Name:"></asp:Label></div>
                                        <div class="col-sm-8" style="padding-left:0px;"><asp:TextBox ID="flagtxtbox" runat="server" ></asp:TextBox></div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 form-group">
                                    <div class="row">
                                        <div class="col-sm-5" style="padding-right:0px;"><asp:Label ID="Label3" runat="server" Text="Module Name:"></asp:Label></div>
                                        <div class="col-sm-7" style="padding-left:0px;"><asp:DropDownList ID="moduledropdown1" runat="server" >
                            <asp:ListItem>All</asp:ListItem>
                            <asp:ListItem>EMR</asp:ListItem>
                            <asp:ListItem>Ward</asp:ListItem>
                            <asp:ListItem>OT</asp:ListItem>
                            <asp:ListItem>Nurse</asp:ListItem>
                            <asp:ListItem>MRD</asp:ListItem>
                        </asp:DropDownList></div>
                                    </div>
                                </div>
                                <div class="col-sm-4 form-group">
                                    <div class="row">
                                        <div class="col-sm-4" style="padding-right:0px;"><asp:Label ID="Label4" runat="server" Text="Page Name:"></asp:Label></div>
                                        <div class="col-sm-8" style="padding-left:0px;"> <asp:TextBox ID="pagetxtbox" runat="server"></asp:TextBox></div>
                                    </div>
                                </div>
                                <div class="col-sm-4 form-group">
                                    <div class="row">
                                        <div class="col-sm-4" style="padding-right:0px;"><asp:Label ID="Label5" runat="server" Text="Status:"></asp:Label></div>
                                        <div class="col-sm-8" style="padding-left:0px;"> <asp:DropDownList ID="statusdropdown" runat="server" >
                            <asp:ListItem>Select</asp:ListItem>
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>Inactive</asp:ListItem>
                        </asp:DropDownList></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <table class="auto-style1">
                        <tr>
                            <td colspan="12">
                                <asp:GridView ID="datagrid" runat="server" AllowPaging="true" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" Height="303px" ShowHeaderWhenEmpty="True" Width="1189px" OnPageIndexChanging="datagrid_PageIndexChanging" PageSize="15">

                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="Flag" HeaderText="Flag" />
                                        <asp:BoundField DataField="Module" HeaderText="Module" />
                                        <asp:BoundField DataField="PageName" HeaderText="Page Name" />
                                        <asp:BoundField DataField="Active" HeaderText="Status" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Edit</asp:LinkButton>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
