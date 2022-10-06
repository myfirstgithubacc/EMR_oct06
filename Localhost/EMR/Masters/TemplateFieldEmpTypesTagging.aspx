<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TemplateFieldEmpTypesTagging.aspx.cs"
    Inherits="EMR_Masters_TemplateFieldEmpTypesTagging" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

    <script type="text/javascript">

        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvEmpType.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>

    <asp:ScriptManager runat="server" />
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr class="clsheader">
                        <td id="tdHeader" align="left" style="padding-left: 10px; width: 300px" runat="server">
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" ToolTip="Template&nbsp;Field&nbsp;Employees&nbsp;Types&nbsp;Tagging"
                                Text="Template&nbsp;Field&nbsp;Employee&nbsp;Type(s)&nbsp;Tagging" Font-Bold="true" />
                        </td>
                        <td align="right">
                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                SkinID="Button" ValidationGroup="SaveData" Text="Save" />
                            &nbsp;
                            <asp:Button ID="btnClose" Text="Close" runat="server" ToolTip="Close" SkinID="Button"
                                OnClientClick="window.close();" />&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" />
                        </td>
                    </tr>
                </table>
                <table border="0" style="margin-left: 10px;" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Field(s) List" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Employee Type(s) List" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 300px;">
                            <asp:Panel ID="Panel2" runat="server" Height="400px" ScrollBars="Auto">
                                <asp:GridView ID="gvTemplateField" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                    Width="100%" AllowPaging="false" OnRowDataBound="gvTemplateField_RowDataBound"
                                    OnRowCommand="gvTemplateField_OnRowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText='Select' HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSelectFieldId" runat="server" Text="Select" CommandName="FieldSelect" />
                                                <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId")%>' />
                                                <asp:HiddenField ID="hdnIsChk" runat="server" Value='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FieldName" HeaderText="Field Name" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td style="width: 300px;">
                            <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Auto">
                                <asp:GridView ID="gvEmpType" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                    Width="100%" AllowPaging="false" OnRowDataBound="gvEmpType_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="30px" ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                                <asp:HiddenField ID="hdnEmployeeTypeId" runat="server" Value='<%#Eval("EmployeeTypeId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EmployeeType" HeaderText="Employee Type" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
