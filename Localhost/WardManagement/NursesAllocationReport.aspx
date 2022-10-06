<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NursesAllocationReport.aspx.cs" Inherits="WardManagement_NursesAllocationReport" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Nurses Allocation Report</title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="_ScriptManager" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
              <div style="vertical-align:central;width:100%;">

                    <table style="vertical-align:central;width:50%;" align="center" >
                        <tr>
                            <td >
                                <asp:Label ID="lblDate" runat="server" Text="Date From"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100px">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <asp:Label ID="lblTo" runat="server" Text="To"></asp:Label>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100px">
                                    <DateInput DateFormat="dd/MM/yyyy">
                                    </DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td>  <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print" Width="100px"
                                                        OnClick="btnPrint_Click" /></td>

                        </tr>

                    </table>

            <%--    </div>--%>

                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                    <Windows>
                        <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" />
                    </Windows>
                </telerik:RadWindowManager>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
