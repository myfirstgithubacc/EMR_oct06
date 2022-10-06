<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MRDReportAgeRange.aspx.cs"
    Inherits="MRD_Master_MRDReportAgeRange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Age Range</title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr style="background-color: #E0EBFD">
                        <td align="center" style="font-size: 12px;">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </td>
                        <td align="right" style="width: 200px">                            
                            <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />&nbsp;
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvAgeRange" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="gvAgeRange_RowCancelingEdit"
                    OnRowEditing="gvAgeRange_RowEditing" 
                    OnRowUpdating="gvAgeRange_RowUpdating" SkinID="gridview">
                    <Columns>
                        <asp:TemplateField HeaderText="From" HeaderStyle-Width="55px" ItemStyle-Width="55px" ItemStyle-Height="20px">
                            <ItemTemplate>
                                <asp:Label ID="lblagefrom" runat="server" SkinID="label" Text='<%#Eval("AgeFrom") %>'
                                    Width="50px" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtagefrom" runat="server" SkinID="textbox" Text='<%#Eval("AgeFrom") %>'
                                    Width="50px" MaxLength="3" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To" HeaderStyle-Width="55px" ItemStyle-Width="55px" ItemStyle-Height="20px">
                            <ItemTemplate>
                                <asp:Label ID="lblageto" runat="server" SkinID="label" Text='<%#Eval("AgeTo") %>'
                                    Width="50px" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtageto" runat="server" SkinID="textbox" Text='<%#Eval("AgeTo") %>'
                                    Width="50px" MaxLength="3" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active" HeaderStyle-Width="55px" ItemStyle-Width="55px" ItemStyle-Height="20px">
                            <ItemTemplate>
                                <asp:Label ID="lblActive" runat="server" SkinID="label" Text='<%#Eval("ActiveStatus") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkboxActive" runat="server" SkinID="checkbox" Checked='<%# Eval("Active") %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="45px" ItemStyle-Width="45px" ItemStyle-Height="20px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkedit" runat="server" CommandName="EDIT" Text="Edit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                <asp:LinkButton ID="lnkupdate" runat="server" CommandName="UPDATE" Text="Update" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="45px" ItemStyle-Width="45px" ItemStyle-Height="20px">
                            <EditItemTemplate>
                                <asp:LinkButton ID="lnkcancel" runat="server" CommandName="CANCEL" Text="Cancel" />
                            </EditItemTemplate>                            
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
