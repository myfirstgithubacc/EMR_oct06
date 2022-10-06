<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImmunizationMain.aspx.cs"
    Inherits="EMR_Immunization_ImmunizationMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">

    function CloseScreen() {
            window.close();             
            return true;
        }

    
     </script>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
    <tr><td> <b>Status</b>&nbsp;&nbsp; <asp:DropDownList ID="ddlstatus" SkinID ="DropDown" AutoPostBack="true" Width="100px"  
                runat="server" onselectedindexchanged="ddlstatus_SelectedIndexChanged">
                    <asp:ListItem Text="Open" Value="O"></asp:ListItem>
                    <asp:ListItem Text="Post" Value="P"></asp:ListItem>
                 
            </asp:DropDownList></td>
    <td align="center"><asp:Label ID="lblMessage" runat="server"></asp:Label></td>
    <td align="right"><asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" 
                            SkinID="Button"  OnClientClick="CloseScreen();" /></td>
    </tr>
    <%--<div style="text-align:center"></div>--%>
    
   
           
           <tr><td colspan="3">
      
        <asp:GridView ID="gvImmunizationStock" SkinID="gridview" runat="server"
            AutoGenerateColumns="False" DataKeyNames="Id" 
            Width="97%" PageSize="15"
            AllowPaging="True" PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" 
            OnPageIndexChanging="gvImmunizationStock_OnPageIndexChanging" OnRowCancelingEdit="gvImmunizationStock_OnRowCancelingEdit"
            OnSelectedIndexChanged="gvImmunizationStock_SelectedIndexChanged" 
            OnRowDataBound="gvImmunizationStock_RowDataBound" 
            >
            <%--OnRowDataBound="gvImmunizationStock_OnRowDataBound"   OnRowEditing="gvImmunizationStock_OnRowEditing" OnRowUpdating="gvImmunizationStock_OnRowUpdating"--%>
            <EmptyDataTemplate>
                <div style="font-weight: bold; color: Red; width: 100%">
                    No Record Found.</div>
            </EmptyDataTemplate>
            <PagerSettings Mode="NumericFirstLast"></PagerSettings>
            <Columns>
                
                <asp:TemplateField Visible="True" HeaderText="S No">
                    <ItemTemplate>
                        <%# Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Id" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblId" SkinID="label" runat="server" Text=' <%#Eval("Id")%>'></asp:Label>
                        <%--<asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                             ToolTip="DeActivate" OnClick="btngo_OnClick"
                                                            ValidationGroup="Cancel" CausesValidation="true" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="HospitalLocationId" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblHLocationId" SkinID="label" runat="server" Text='<%#Eval("HospitalLocationId")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DocumentNo" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblDocumentNo" SkinID="label" runat="server" Text='<%#Eval("DocumentNo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Lot No">
                    <ItemTemplate>
                        <asp:Label ID="lblLotno" SkinID="label" runat="server" Text='<%#Eval("LotNo")%>'></asp:Label>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Received Date">
                    <ItemTemplate>
                        <asp:Label ID="lblReceivedDate" SkinID="label" runat="server" Text='<%#Eval("ReceivedDate")%>'></asp:Label>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <asp:Label ID="lblQtyReceived" SkinID="label" runat="server" Text='<%#Eval("QtyReceived")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnview" Text="Edit" runat="server" Width="100%" Font-Underline="false"
                            CommandName="Select"></asp:LinkButton>
                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Post">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnStockPost" runat="server" SkinID="Button" Text="Stock Post" Font-Underline="false" ToolTip="Stock Post"
                            OnClick="btnStockPost_OnClick" CausesValidation="true" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField >
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                            ToolTip="Delete" OnClick="btngo_OnClick"   ValidationGroup="Cancel" CausesValidation="true" />

                    </ItemTemplate>
                      <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                
            </Columns>
        </asp:GridView>
       </td></tr> 
        </table>
   
    </form>
</body>
</html>
