<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Assessment.aspx.cs" Inherits="EMR_Problems_Assessment" Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../Include/JS/Functions.js" language="javascript"></script>

    <script type="text/javascript">
        function __addKeyword() {
            window.opener.__doPostBack('hdnProblems', '');
            self.close();
        }
    </script>

</head>
<body onload="$get('txtKeywords').focus();" style="background-color: White;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="_ScriptManager" runat="server">
    </asp:ScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td style="height: 20px;">
                <table width="100%" class="clsheader" bgcolor="#e3dcco">
                    <tr>
                        <td width="3%" align="center" valign="middle">
                            <asp:Image ID="Image1" ImageUrl="/Images/Assessment.png" Height="22" runat="server" />
                        </td>
                        <td width="25%" valign="middle">
                            Assessment
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td width="2%" align="center" valign="middle">
                                    </td>
                                    <td width="1%">
                                        &nbsp;
                                    </td>
                                    <td width="2%" align="center">
                                    </td>
                                    <td width="1%">
                                        &nbsp;
                                    </td>
                                    <td width="2%" align="center">
                                    </td>
                                    <td width="1%">
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="30%">
                            &nbsp;
                        </td>
                        <td width="30%" align="right">
                            <asp:ImageButton ID="ibtnClose" ToolTip="Close Window" OnClientClick="self.close(); return false;"
                                runat="server" ImageUrl="/images/icon-close.jpg" Height="16" Width="16" AccessKey="P" />
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="20px;">
            </td>
        </tr>
        <tr>
            <td align="left" style="border-top: solid 1px gray; border-bottom: solid 1px gray;"
                runat="server" visible="false">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="background-color: #e0ebfd;">
                        </td>
                        <td align="left" valign="top" id="td1" runat="server" style="background-color: #e0ebfd;">
                            <asp:Xml ID="xmlPatientInfo" runat="server"></asp:Xml>
                            <asp:HiddenField ID="hdnPaymentType" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <%--    <asp:Panel ID="pnlAssessment" runat="server" GroupingText="<strong>Assessment</strong>">--%>
                <table width="100%" border="0" cellpadding="2" cellspacing="2">
                    <colgroup>
                        <col width="17%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td align="left" valign="middle">
                            <asp:Label ID="lblCategory" SkinID="label" Width="150px" Text="Categories" runat="server" />
                        </td>
                        <td align="left" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDisease" SkinID="label" Font-Bold="true" Text="Search&nbsp;By"
                                            runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;<asp:DropDownList ID="ddlSortBy" SkinID="DropDown" onkeydown="Tab();" runat="server">
                                            <asp:ListItem Text="ICD Code" Value="1" />
                                            <asp:ListItem Text="Diagnosis" Value="2" Selected="True" />
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                &nbsp;<asp:TextBox ID="txtKeywords" SkinID="textbox" ToolTip="Enter text to Search..."
                                                    runat="server" onkeydown="return keyDown(this,'btnSearch',event);" Columns="20"
                                                    Width="450px" TextMode="SingleLine" Rows="3"></asp:TextBox>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="LstCategory" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        &nbsp;<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/Images/Search1.jpg"
                                            CausesValidation="false" OnClick="SearchProblem" ToolTip="Click to Search" />
                                    </td>
                                    <td align="left">
                                        <asp:LinkButton ID="lnkPrevAssess" ToolTip="Previous Assessment" Text="Previous Assessment"
                                            runat="server" Visible="false" OnClick="lnkPrevAssess_Click" />
                                    </td>
                                    <td width="1%">
                                        &nbsp;
                                    </td>
                                    <td align="center" valign="top" style="margin-top: 5px;">
                                        <table>
                                            <tr>
                                                <td class="middlebtn" valign="top">
                                                    <asp:LinkButton ID="lnkFavorities" ForeColor="White" Font-Underline="false" ToolTip="My Favorites"
                                                        Text="My&nbsp;Favorites" runat="server" OnClick="lnkFavorities_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:ListBox ID="LstCategory" SkinID="listbox" Width="100%" AutoPostBack="true" runat="server"
                                Height="310px" OnSelectedIndexChanged="LstCategory_SelectedIndexChanged" />
                        </td>
                        <td align="left" valign="top">
                            <table cellpadding="0" cellspacing="0" style="height: 310px" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <asp:UpdatePanel ID="updDisease" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="GVDisease" runat="server" AllowPaging="true" PageSize="10" Width="100%"
                                                    AutoGenerateColumns="false" CellPadding="1" OnPageIndexChanging="GVDisease_PageIndexChanging"
                                                    PagerSettings-Mode="NumericFirstLast" OnRowDataBound="GVDisease_RowDataBound">
                                                    <PagerSettings Mode="NumericFirstLast" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle CssClass="pagination" BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="Pink" Font-Bold="false" ForeColor="#333333" />
                                                    <HeaderStyle CssClass="clsGridheader" />
                                                    <EditRowStyle BackColor="skyBlue" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <%--<RowStyle BackColor="#EFF3FB" />
                                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <EditRowStyle BackColor="#2461BF" />
                                                        <AlternatingRowStyle BackColor="White" />--%>
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="20px">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkHeader" SkinID="checkbox" ToolTip="Click to Check/Uncheck all"
                                                                    AutoPostBack="true" runat="server" OnCheckedChanged="chkHeader_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkInner" SkinID="checkbox" runat="server" AutoPostBack="true"
                                                                    OnCheckedChanged="chkInner_CheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ICD&nbsp;Code" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblICDCode" SkinID="label" runat="server" Text='<%#Eval("ICDCode")%>' />
                                                                <asp:HiddenField ID="hiddenICDCode" Value='<%#Eval("ICDID")%>' runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td style="width: 50%;" align="left">
                                                                            <asp:Label ID="lblDescription" SkinID="label" Text="Diagnosis" runat="server" />
                                                                        </td>
                                                                        <td style="width: 50%;" align="right">
                                                                            <asp:Label ID="lblStat" SkinID="label" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" SkinID="label" runat="server" Text='<%#Eval("Description")%>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMess" SkinID="label" ForeColor="Red" Font-Size="13px" runat="server"
                                                            Text="No record(s) found!" />
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="LstCategory" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkFavorities" />
                                                <asp:AsyncPostBackTrigger ControlID="lnkPrevAssess" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom" align="left">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="height: 23px">
                                                    <asp:Label ID="lblDiagnosis" SkinID="label" Text="Diagnosis&nbsp;" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="updDiagnosis" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtDiagnosis" SkinID="textbox" onkeydown="return keyDown(this,'btnSubmit',event);"
                                                                Width="641px" runat="server" MaxLength="250" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="btnSubmit" SkinID="Button" Text="Add" Width="40px" runat="server"
                                                        OnClick="btnSubmit_Click" ToolTip="Click to Add" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <%-- </asp:Panel>--%>
                <asp:UpdatePanel ID="updDownGV" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView2" SkinID="gridview" runat="server" Width="100%" AutoGenerateColumns="false"
                            CellPadding="1" OnRowDataBound="GridView2_RowDataBound" OnRowCancelingEdit="GridView2_RowCancelingEdit"
                            OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating">
                            <%--<RowStyle BackColor="#EFF3FB" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle Height="25px" />--%>
                            <Columns>
                                <asp:TemplateField HeaderText="Sl&nbsp;No." HeaderStyle-Width="44px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSno" SkinID="label" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ICD&nbsp;Code" HeaderStyle-Width="66px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblICDCode" SkinID="label" runat="server" Text='<%#Eval("ICDCode")%>' />
                                        <asp:HiddenField ID="hdnICDID" runat="server" Value='<%#Eval("ICDID")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="480px">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" SkinID="textbox" MaxLength="250" onkeydown="if(event.keyCode==13)event.returnValue=false;"
                                            Width="99%" Text='<%#Eval("Description")%>' runat="server" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" SkinID="label" Text='<%#Eval("Description")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="90px">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="ibtnUpdate" Text="Update" ToolTip="Update" runat="server" ForeColor="White"
                                            CommandName="Update" />
                                        <asp:LinkButton ID="ibtnCancel" Text="Cancel" ToolTip="Cancel" runat="server" ForeColor="White"
                                            CommandName="Cancel" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnEdit" ImageUrl="~/Images/edit.jpg" ToolTip="Click to Edit"
                                            runat="server" CommandName="Edit" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remove" HeaderStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnRemove" ImageUrl="~/Images/DeleteRow.png" runat="server"
                                            ToolTip="Click to Remove" OnClick="btnRemove_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <asp:Label ID="lblMess" SkinID="label" runat="server" Text="No record(s) added!"
                                    ForeColor="Red" Font-Size="13px" />
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                        <%--<asp:AsyncPostBackTrigger ControlID="ibtnSave" />--%>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <center>
        <asp:UpdatePanel ID="upOk" runat="server">
            <ContentTemplate>
                <%--OnClientClick="__addKeyword(); return false;"--%>
                <asp:Button ID="btnOK" SkinID="Button" Text="Submit" ToolTip="Submit" Visible="false"
                    runat="server" OnClick="btnOK_Click" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <asp:UpdatePanel ID="updStat" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdnStatus" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="LstCategory" />
            <asp:AsyncPostBackTrigger ControlID="lnkFavorities" />
            <asp:AsyncPostBackTrigger ControlID="lnkPrevAssess" />
        </Triggers>
    </asp:UpdatePanel>
    </form>

</body>
</html>
