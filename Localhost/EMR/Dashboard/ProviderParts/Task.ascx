<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Task.ascx.cs" Inherits="EMR_Dashboard_ProviderParts_Task" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">   
    <tr>
        <td>
        <asp:TextBox ID="txtTasks" runat="server" style="visibility:hidden; position:absolute;"  ></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel4" runat="server" ScrollBars="Vertical" Width="100%" Height="210px">
                <asp:UpdatePanel ID="upnlTask" runat="server">
                    <ContentTemplate>
                    
                    <asp:GridView ID="GVTask" ForeColor="#333333" runat="server" AllowPaging="True" SkinID="gridview" 
                        PageSize="10" Width="96%" AutoGenerateColumns="false" CellPadding="0" ShowHeader="true"
                        EmptyDataText="No Tasks!" EmptyDataRowStyle-ForeColor="Red" 
                            onpageindexchanging="GVTask_PageIndexChanging">
                                                    
                            <Columns>
                            <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltTask" runat="server" Text="Task"></asp:Literal>
                                    </HeaderTemplate>
                                <ItemTemplate>
                                        <asp:LinkButton ID="lnkName" Font-Strikeout="false" onmouseover="this.style.textDecoration='underline';"
                                            OnClick="lnkName_Click" onmouseout="this.style.textDecoration='none';" Text='<%#Eval("TaskType")%>'
                                            runat="server" CommandName="Select"  CommandArgument='<%#Eval("TaskID")%>'
                                            Width="100px"></asp:LinkButton>
                                    </ItemTemplate>
                                    <%--<ItemTemplate>                                   
                                        <asp:Label ID="lblTaskType" runat="server" Text='<%#Eval("TaskType")%>' Width="120px"></asp:Label>
                                    </ItemTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltName" runat="server" Text="Patient Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' Width="100px"></asp:Label>
                                      
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltTaskPriority" runat="server" Text="Priority"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaskPriority" runat="server" Text='<%#Eval("TaskPriority")%>' Width="50px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="ltDueTime" runat="server" Text="Due Time"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDueTime" runat="server" Text='<%#Eval("DueTime")%>' Width="90px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                            </Columns>
                        </asp:GridView>
                        
                        <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                                            <Windows>
                                                                <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move">
                                                                </telerik:RadWindow>
                                                            </Windows>
                                                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
           </asp:Panel>
        </td>
    </tr>
  <tr>
        <td align="left">
       
            <table border="0" style="height: 17px; width: 100%;" cellpadding="0" cellspacing="0">
                <tr style="background-color: #EFF3FB">
                    <td>
                      
                    </td>
                    <td colspan="2" align="center">
                        <asp:Panel ID="pnlOrders" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnTask" runat="server" />
            <asp:HiddenField ID="hdnTaskCtrl" runat="server" />
        </td>
    </tr>
</table>