<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DoseTime.aspx.cs" Inherits="EMR_Medication_DoseTime" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Dose Time</title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">
        function returnToParent() {
            var oArg = new Object();
            oArg.xmlFrequencyString = document.getElementById("hdnXmlString").value;
            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        function doseExtend(frequencyDetailId, doseTime, DoseEnable, colIdx) {
            $get('<%=hdnFrequencyDetailId.ClientID%>').value = frequencyDetailId;
            $get('<%=hdnDoseTime.ClientID%>').value = doseTime;
            $get('<%=hdnDoseEnable.ClientID%>').value = DoseEnable;
            $get('<%=hdnColIdx.ClientID%>').value = colIdx;

            $get('<%=btnDoseExtend.ClientID%>').click();
        }
        
    </script>


    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            <table border="0" width="100%" class="clsheader" bgcolor="#e3dcco" cellpadding="0"
                cellspacing="0">
                <tr>
                    <td style="width: 70%">
                    
                    <asp:Label ID="lblMessage" runat="server" Text="" SkinID="label"></asp:Label>
                    </td>
                    <td>
                        
                    </td>
                    <td align="right" style="width: 30%">
                     <%--<asp:Button ID="btnSave" Text="Save" runat="server" CausesValidation="false" ToolTip="Add multiple dose  to main page..."
                            SkinID="Button" OnClick="btnSave_Click" />--%>
                            
                        <asp:Button ID="btnCloseW" Text="Save" runat="server" CausesValidation="false" ToolTip="Add multiple dose  to main page..."
                            SkinID="Button" OnClick="btnCloseW_Click" />
                        <asp:Button ID="btnClose" runat="server" OnClientClick="window.close();" SkinID="Button"
                            Text="Close" />
                    </td>
                </tr>
            </table>
            <table border="0" width="99%" cellpadding="0" cellspacing="5">
                <tr>
                    <td>
                        <asp:Label ID="lblItemName" Font-Bold="true" runat="server" Text="" SkinID="label"></asp:Label>
                    </td>
                    <td>
                    <asp:Button ID="btnApplyAll" Text="Apply All" runat="server" CausesValidation="false" ToolTip="Apply All"
                            SkinID="Button" OnClick="btnApplyAll_Click" />
                    </td>
                </tr>
            </table>
            </br>
            <table border="0" width="99%" cellpadding="0" cellspacing="0">
             <tr>
                    <td>
                        <table id="tblDoseExtend" runat="server" cellspacing="0" cellpadding="2" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Date" />
                                </td>
                                <td>
                                    <telerik:RadDatePicker ID="dtExtendDate" runat="server" Width="100px" />
                                </td>                                
                                <td>
                                    <asp:Label ID="Label3" runat="server" SkinID="label" Text="Time" />
                                </td>
                                <td>
                                    <telerik:RadTimePicker ID="dtExtendTime" runat="server" Width="90px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSaveDoseExtend" Text="Update" runat="server" CausesValidation="false"
                                        Font-Bold="true" SkinID="Button" OnClick="btnSaveDoseExtend_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvData" runat="server" OnRowCreated="gvData_OnRowCreated" AllowPaging="false"
                            SkinID="gridview" OnRowDataBound="gvData_OnRowDataBound">
                            <EmptyDataTemplate>
                                <h1>
                                    No record found.</h1>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDosageTimeMessage" runat="server" SkinID="label" Text="Note:If you want to exclude the drug administration time, please click the checkbox."></asp:Label>
                        <asp:HiddenField ID="hdnXmlString" runat="server" />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:TextBox ID="txtAdministered" runat="server" Text="" BorderWidth="2" SkinID="label"
                            BackColor="Gray" ReadOnly="true" Width="20px" />
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Administered" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnDoseTime" runat="server" />
                        <asp:HiddenField ID="hdnFrequencyDetailId" runat="server" />
                        <asp:HiddenField ID="hdnDoseEnable" runat="server" />
                        <asp:HiddenField ID="hdnColIdx" runat="server" />
                        <asp:Button ID="btnDoseExtend" runat="server" Style="visibility: hidden" OnClick="btnDoseExtend_OnClick" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
