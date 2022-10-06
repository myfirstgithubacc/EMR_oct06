<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OTChecklist.aspx.cs" Inherits="OTScheduler_OTChecklist" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OT checkList Transaction</title>
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;"
            cellpadding="2" cellspacing="2" width="100%">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration,UHID %>'
                        Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblRegistrationNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="DOB:" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblDob" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label8" runat="server" Text='<%$ Resources:PRegistration,IpNo %>'
                        Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
        </table>
        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
        <div id="dvZone1" style="width: 100%; height: 90%">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnPrint" runat="server" SkinID="Button" Text="Print" OnClick="btnPrint_Click" />
                        <asp:Button ID="btnSave" runat="server" SkinID="Button" Text="Save" OnClick="btnSave_Click" />
                        &nbsp;
                    <asp:Button ID="btnClose" runat="server" SkinID="Button" Text="Close" OnClientClick="window.close();" />
                        &nbsp; &nbsp;
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="100%">
                <%--   <tr>
                <td width="80px">
                    <asp:Label ID="Label1" runat="server" Text='<%$ Resources:PRegistration,IpNo %>'
                        Font-Bold="True"></asp:Label>
                </td>
                <td width="150px">
                    <asp:Label ID="lblIpno" runat="server" Width="140px"></asp:Label>
                </td>
                <td width="100px">
                    <asp:Label ID="Label2" runat="server" Text="Patient Name" Font-Bold="True"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblPatientname" runat="server"></asp:Label>
                </td>
            </tr>--%>
                <tr>
                    <td width="80px">
                        <asp:Label ID="Label3" runat="server" Text="Ward :" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="150px">
                        <asp:Label ID="lblWard" runat="server" Width="140px"></asp:Label>
                    </td>
                    <td width="100px">
                        <asp:Label ID="Label4" runat="server" Text="Surgery :" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblSurgey" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Checked By :"></asp:Label>
                    </td>
                    <td colspan="3">
                        <telerik:RadComboBox ID="ddlcheckedby" runat="server" AllowCustomText="true" EnableLoadOnDemand="true"
                            Filter="Contains" AutoPostBack="true" EnableItemCaching="false" MarkFirstMatch="true"
                            Width="200px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Panel runat="server" Height="450px" Width="850px" ScrollBars="Auto">
                            <telerik:RadGrid ID="gvDetails" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="false"
                                Width="99%" Height="100%" ShowFooter="false" GridLines="Both" ForeColor="Black" OnPreRender="gvDetails_PreRender"
                                OnItemDataBound="gvDetails_ItemDataBound">
                                <MasterTableView Width="100%" TableLayout="Fixed">
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Id" UniqueName="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="txtId" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText='<%$ Resources:PRegistration, SerialNo%>'
                                            HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSn" runat="server" Width="30px" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Check List Description" UniqueName="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="txtDescription" runat="server" Text='<%#Eval("Discription")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Check" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkChecked" runat="server" SkinID="checkbox" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Enter value" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                                                <telerik:RadDatePicker ID="dtpdate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"
                                                    Visible="false" Width="100%">
                                                </telerik:RadDatePicker>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemark" runat="server" Width="100%" Text='<%#Eval("Remarks")%>'
                                                    MaxLength="100"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="70px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="txtValueType" runat="server" Text='<%#Eval("ValueType")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Value" HeaderStyle-Width="70px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="txtValue" runat="server" Text='<%#Eval("Value")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnBookinId" runat="server" />
                        <asp:HiddenField ID="hdnSurgeryId" runat="server" />
                        <asp:HiddenField ID="hdnBedId" runat="server" />
                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
