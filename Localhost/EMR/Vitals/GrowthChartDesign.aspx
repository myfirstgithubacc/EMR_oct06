<%@ Page Language="C#" Theme="DefaultControls" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="GrowthChartDesign.aspx.cs" Inherits="MPages_GrowthChartDesign"
    Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="JavaScript" type="text/javascript">
        
    </script>

<asp:UpdatePanel ID="UpdatePane1" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" class="clsheader">
                <tr>
                    <td>
                    </td>
                    <td style="width: 250px; padding-left: 10px;">
                        Growth Chart Details
                    </td>
                    <td>
                        <table cellspacing="0" cellpadding="0">
                            <tr>
                                <td valign="top">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" valign="middle">
                        <asp:Button ID="btnSaveDetails" SkinID="Button" runat="server" Text="Save" OnClick="btnSaveDetails_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                <colgroup>
                    <col width="25%" />
                    <col width="30%" />
                    <col width="15%" />
                    <col width="30%" />
                </colgroup>
        
             
                <tr>
                    <td align="center" colspan="4" style="height: 13px; color: green; font-size: 12px;
                        font-weight: bold;">
                        <asp:Label ID="lblMessage" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblAgeGroup" runat="server" SkinID="label" Text="Age Group"></asp:Label>
                    </td>
                    <td>
                 
                        <asp:TextBox ID="txtAgeGroup" runat="server" Width="150px"></asp:TextBox>
                      
                    </td>
                    <td>
                        <asp:Label ID="lblGrowthChartName" runat="server" SkinID="label" Text="Growth Chart Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGrowthChartName" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                         <asp:HiddenField ID="hdGrowthID"  runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDisplayName" runat="server" SkinID="label" Text="Display Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDisplayName" SkinID="textbox" MaxLength="20" Width="175px" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblVitalGrowthChart" runat="server" SkinID="label" Text="Vital Growth Chart"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVitalGrowthChart" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                  
                    
                    <td>
                        <asp:Label ID="lblXAxisDisplayCap" runat="server" SkinID="label" Text="X Axis Display Caption"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtXAxisDisplayCap" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                    
                    
                     <td>
                        <asp:Label ID="lblYAxisDisplayCap" runat="server" SkinID="label" Text="Y Axis Display Caption"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisDisplayCap" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                    
                    
                </tr>
                <tr>
                
                
                  <td>
                        <asp:Label ID="lblXAxisCapInterval" runat="server" SkinID="label" Text=" X Axis Caption Interval"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtXAxisCapInterval" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                
                   <td>
                        <asp:Label ID="lblYAxisCapInterval" runat="server" SkinID="label" Text="Y Axis Caption Interval"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisCapInterval" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                
                 
                </tr>
                <tr>
                
                
                   <td>
                        <asp:Label ID="lblXAxisMinValue" runat="server" SkinID="label" Text="X Axis Minimum Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtXAxisMinValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                    
                             <td>
                        <asp:Label ID="lblYAxisMinValue" runat="server" SkinID="label" Text="Y Axis Minimum Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisMinValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                    
                    
                    
                    
                    
                    
                    
                    
                
                
                
                
                    
                   
                </tr>
                <tr>
                
                
                
                <td>
                        <asp:Label ID="lblXAxisMaxValue" runat="server" SkinID="label" Text="X Axis Maximum Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtXAxisMaxValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                
                
                    <td>
                        <asp:Label ID="lblYAxisMaxValue" runat="server" SkinID="label" Text="Y Axis Maximum Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisMaxValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                
               
                 
                    
              
                 
                    
                    
                    
                </tr>
                
                          <tr>
                
                <td>
                        <asp:Label ID="lblXAxisInterval" runat="server" SkinID="label" Text="X Axis Interval"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtXAxisInterval" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                
                    <td>
                        <asp:Label ID="lblYAxisInterval" runat="server" SkinID="label" Text="Y Axis Interval"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisInterval" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                
                
                <tr>
                <td>
                </td>
                <td>
                </td>
                   <td>
                        <asp:Label ID="lblYAxisCapMinValue" runat="server" SkinID="label" Text="Y Axis Caption Minimum Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisCapMinValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                
                </tr>
                
                <tr>
                </tr>
                
                
                <tr>
                <td>
                </td>
                <td></td>
                
                  <td>
                        <asp:Label ID="lblYAxisCapMaxValue" runat="server" SkinID="label" Text="Y Axis Caption Maximum Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisCapMaxValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                
                
                <tr>
                
                  <td></td>
                  <td></td>
                    
                 
                    <td>
                        <asp:Label ID="lblYAxisCapStartValue" runat="server" SkinID="label" Text="YAxis Caption Start Value"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtYAxisCapStartValue" SkinID="textbox" MaxLength="50" Width="175px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                 
                </tr>
      
                <tr>
                    <td>
                        <asp:Label ID="lblChartPercentiles" runat="server" SkinID="label" Text="Chart Percentiles"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkPercentiles" runat="server" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:UpdatePanel ID="upgrd" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="GrdGrowthChart" runat="server" AutoGenerateColumns="false" SkinID="gridview" 
                                    OnRowCommand="GrdGrowthChart_OnRowCommand"   OnRowEditing="GrdGrowthChart_RowEditing">
                                    <Columns>
                                        <asp:BoundField DataField="AgeGroup" HeaderText="Age Group" />
                                        <asp:BoundField DataField="GrowthChartName" HeaderText="Growth Chart Name" />
                                        <asp:BoundField DataField="DisplayName" HeaderText="Display Name" />
                                        <%--<asp:BoundField DataField="EMRVitalID" HeaderText="EMRVitalID" />--%>
                                        <asp:BoundField DataField="XAxisCapInterval" HeaderText="X Axis Cap Interval" />
                                        <asp:BoundField DataField="XAxisDisplayCap" HeaderText="X Axis Display Caption" />
                                        <asp:BoundField DataField="XAxisMinValue" HeaderText="X Axis Min Value in Months" />
                                        <asp:BoundField DataField="XAxisMaxValue" HeaderText="X Axis Max Value in Months"  />
                                        <asp:BoundField DataField="XAxisInterval" HeaderText="X Axis Interval in Months" />
                                        
                                        <asp:BoundField DataField="YAxisDisplayCap" HeaderText="Y Axis Display Caption" />
                                        <asp:BoundField DataField="YAxisCapMinValue" HeaderText="Y Axis Cap Min Value" />
                                        <asp:BoundField DataField="YAxisCapMaxValue" HeaderText="Y Axis Cap Max Value" />
                                        <asp:BoundField DataField="YAxisCapStartValue" HeaderText="Y Axis Cap Start Value" />
                                        <asp:BoundField DataField="YAxisMinValue" HeaderText="Y Axis Min Value" />
                                        <asp:BoundField DataField="YAxisMaxValue" HeaderText="Y Axis Max Value " />
                                        <asp:BoundField DataField="YAxisInterval" HeaderText="Y Axis Interval" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit"      CommandName="Edit"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnGrowthChartId" runat="server" Value='<%#Eval("GrowthChartId") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                       </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                
            </table>
    </ContentTemplate>
       
    </asp:UpdatePanel>
    
    
    <table >
    <tr>
    <td>
    
   
                        </td>
    </tr>
    </table> 
    
</asp:Content>
