<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tasks.ascx.cs" Inherits="EMR_Dashboard_Parts_Tasks" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/Tasks/Component/Tasks.ascx" TagName="Tasks" TagPrefix="uc11" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc;">
    <tr>
        <td style="width: 100%">
            <asp:Panel ID="Panel5" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                      <telerik:radformdecorator id="RadFormDecorator1" decoratedcontrols="All" runat="server"
                            decorationzoneid="dvZone1" skin="Metro"></telerik:radformdecorator>
                        <div id="dvZone1" style="width: 100%">
                        <asp:GridView ID="gvTasks" ForeColor="#333333" GridLines="Both" runat="server" AllowPaging="true"
                            PageSize="6" Width="95%" AutoGenerateColumns="false" CellPadding="0" ShowHeader="true"
                            SkinID="gridview" onpageindexchanging="gvTasks_PageIndexChanging" 
                             >
                            <HeaderStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:BoundField HeaderText="Date" DataField="CreatedDate" />
                                
                                <asp:TemplateField>
                                <HeaderTemplate>
                                        <asp:Literal ID="ltTask" runat="server" Text="Sender"></asp:Literal>
                                    </HeaderTemplate>
                                <ItemTemplate>
                                        <asp:LinkButton ID="lnkName" Font-Strikeout="false" onmouseover="this.style.textDecoration='underline';"
                                            OnClick="lnkName_Click" onmouseout="this.style.textDecoration='none';" Text='<%#Eval("assignedby")%>'
                                            runat="server" CommandName="Select" CommandArgument='<%#Eval("TaskID")%>'
                                            Width="100px" ForeColor="DodgerBlue" Font-Underline="true" ></asp:LinkButton>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                <asp:BoundField HeaderText="Type" DataField="TaskType" />
                                <%--<asp:TemplateField>
                                <ItemTemplate>
                                <asp:Literal ID="ltTaskID" runat="server" Text='<%#Eval("TaskID")%>'></asp:Literal>
                                </ItemTemplate>
                                </asp:TemplateField>--%>
                                
                            </Columns>
                        </asp:GridView>
                        </div>
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
                        <%--<asp:LinkButton ID="lnkNotesRedirect" runat="server" Text="Add Task" 
                            Font-Bold="true" onclick="lnkNotesRedirect_Click"></asp:LinkButton>--%>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Panel ID="Panel7" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
 <asp:TextBox ID="hdnToDate" runat="server"  style="visibility:hidden; position:absolute;" />
            <asp:TextBox ID="hdnFromDate" runat="server" style="visibility:hidden; position:absolute;" />
            <asp:TextBox ID="hdnEncounterNumber" runat="server" style="visibility:hidden; position:absolute;" />
            <asp:TextBox ID="hdnDateVale" runat="server" style="visibility:hidden; position:absolute;" />
