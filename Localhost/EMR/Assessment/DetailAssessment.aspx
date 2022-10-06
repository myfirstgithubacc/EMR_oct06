<%@ Page Language="C#" Theme="DefaultControls" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="DetailAssessment.aspx.cs" Inherits="EMR_Assessment_DetailAssessment"
    Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
    

    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="clsheader">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td style="padding-left: 10px;">
                            Assessment
                        </td>
                        <td align="right">
                        <asp:Button ID="btnNew" SkinID="Button" runat="server" Text="New" 
                                onclick="btnNew_Click" />
                            <asp:Button ID="btnSave" SkinID="Button" runat="server" Text="Save" 
                                onclick="btnSave_Click" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lblPatientInfo" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
        <td align="center">
            <asp:Label ID="lblMsg" Font-Bold="true" Font-Size="12px" ForeColor="Green" runat="server" Text=""></asp:Label>
        </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td style="width:300px;" align="left">
                            <asp:RadioButtonList ID="rbolist" AutoPostBack="true" RepeatDirection="Horizontal"
                                runat="server" OnSelectedIndexChanged="rbolist_SelectedIndexChanged">
                                <asp:ListItem Text="Today's&nbsp;Diagnosis" Selected="True" Value="P"></asp:ListItem>
                                <asp:ListItem Text="Chronic&nbsp;Diagnosis" Value="C"></asp:ListItem>                               
                            </asp:RadioButtonList>
                        </td>
                        <td style="width:430px;" align="right">
                       <asp:HyperLink ID="hlBackToDiagnosis" Text="New&nbsp;Diagnosis" runat="server" NavigateUrl="~/EMR/Assessment/Diagnosis.aspx"></asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="1" cellspacing="1" width="92%">
                    <tr>
                        <td valign="top">
                        <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>                        
                            <asp:GridView ID="gvComplain" SkinID="gridview" runat="server" AutoGenerateColumns="false"
                                OnRowDataBound="gvComplain_RowDataBound" Width="83%" OnSelectedIndexChanged="gvComplain_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="ICDDescription" HeaderText="Diagnosis" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:CommandField ShowSelectButton="true" />
                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                </Columns>
                            </asp:GridView>                         
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                        </asp:UpdatePanel>                       
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td rowspan="3">
                        Assessment List <br />
                            <asp:ListBox ID="lstaddProblem" Font-Size="Smaller" AutoPostBack="true" Height="110px" Width="300px" 
                                runat="server" onselectedindexchanged="lstaddProblem_SelectedIndexChanged"></asp:ListBox>
                        </td>
                        <td>
                            Diagnosis
                        </td>
                        <td>
                            <asp:TextBox ID="txtproblem" SkinID="textbox" ReadOnly="true" Columns="30" runat="server"></asp:TextBox>
                            
                        </td>
                        <td>
                        <asp:DropDownList ID="ddlResolved" runat="server" Visible="false">
                        <asp:ListItem Selected="True" Text="Un-Resolved" Value="False"></asp:ListItem>
                            <asp:ListItem  Text="Resolved" Value="True"></asp:ListItem>                            
                            </asp:DropDownList>
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            Condition
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtcondition" SkinID="textbox" Columns="30" runat="server"></asp:TextBox>
                            <asp:Button ID="btnstable" runat="server" SkinID="Button" Text="S" ToolTip="Stable" Width="30px" OnClick="btnstable_Click" />
                            <asp:Button ID="btnunstable" runat="server" SkinID="Button" Text="U" ToolTip="Unstable" Width="30px"
                                OnClick="btnunstable_Click" />
                            <asp:Button ID="btnimproving" runat="server" SkinID="Button" Text="I" ToolTip="Improving" Width="30px"
                                OnClick="btnimproving_Click" />
                            <asp:Button ID="btndeteriorating" runat="server" SkinID="Button" Text="D" ToolTip="Deteriorating" Width="30px"
                                OnClick="btndeteriorating_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Prognosis
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtprognosis" SkinID="textbox" Columns="30" runat="server"></asp:TextBox>
                            <asp:Button ID="btngood" runat="server" SkinID="Button" Text="G" Width="30px" OnClick="btngood_Click" />
                            <asp:Button ID="btnguarded" runat="server" SkinID="Button" Text="Gu" Width="30px"
                                OnClick="btnguarded_Click" />
                            <asp:Button ID="btnpoor" runat="server" SkinID="Button" Text="P" Width="30px" OnClick="btnpoor_Click" />
                            <asp:Button ID="btnfair" runat="server" SkinID="Button" Text="F" Width="30px" OnClick="btnfair_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <%--<asp:Button ID="btnnew" runat="server" Text="New" SkinID="Button" Width="70px" OnClick="btnnew_Click" />--%>
                            <asp:Button ID="btndelete" runat="server" Text="Delete" SkinID="Button" Width="70px"
                                OnClick="btndelete_Click" />
                            <asp:Button ID="btnup" runat="server" Text="Up" SkinID="Button" Visible="false" Width="70px" OnClick="btnup_Click" />
                            <asp:Button ID="btndown" runat="server" Text="Down" SkinID="Button" Width="70px" Visible="false"
                                OnClick="btndown_Click" />
                        </td>
                        <%--<td style="color:Red; font-weight:bold;">
        * Seperate word by a period and space (eg. Chest pain. Anxiety)
        </td>--%>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellpadding="2" cellspacing="2">
                    <tr visible="false">
                        <td valign="top">
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtdiagnosis" SkinID="textbox"  Columns="110"
                                runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr visible="false">
                        <td valign="top">
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtdifferential" SkinID="textbox" Columns="110" runat="server" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Test
                        </td>
                        <td>
                            <asp:TextBox ID="txttest" SkinID="textbox"  Columns="111"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Plan
                        </td>
                        <td>
                            <asp:TextBox ID="txtplan" SkinID="textbox" Rows="3" TextMode="MultiLine" Columns="112"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Comments
                        </td>
                        <td>
                            <asp:TextBox ID="txtcomment" SkinID="textbox" Rows="3" TextMode="MultiLine" Columns="112"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
