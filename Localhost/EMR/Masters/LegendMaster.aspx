<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="LegendMaster.aspx.cs" Inherits="EMR_Masters_LegendMaster" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />   
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/Administration-Item.css" rel="stylesheet" type="text/css" />
    
    

        <asp:UpdatePanel ID="updsave" runat="server">
            <ContentTemplate>
            
            
                <div class="VisitHistoryDiv">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-11"><div class="OrderSetDivText"><h2>Color Legend</h2></div></div>
                            <div class="col-md-1"><asp:Button ID="ibtnLegendColorSave" runat="server" CausesValidation="true" OnClick="ibtnLegendColorSave_OnClick" ToolTip="Save" CssClass="AdminBtn01" ValidationGroup="SaveUpdate" Text="Save" /></div>
                        </div>
                    </div>
                </div>
            
            
                <div class="AuditTrailDiv">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="AuditMessage">
                                    <asp:Label ID="lblMessage" runat="server" Text="Error msg" />
                                    <asp:ValidationSummary ID="VS" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="SaveUpdate" />
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="SaveColor" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="true" ShowSummary="false" ValidationGroup="SaveStatus" />
                                
                                </div>
                            </div>
                        </div>    
                     </div>
                  </div>    
                  
                  
                  
                  
                 <div class="ItemService-Div">
                    <div class="container-fluid">
                        
                        <div class="row">
                            <div class="col-md-6">
                                <div class="ColorLegend-Div">
                                    <div class="row">
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Type</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend"><h3><asp:DropDownList ID="ddlType" runat="server" Width="100%" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"></asp:DropDownList></h3></div></div>
                                    </div>
                                    
                                    
                                    <div class="row">
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Status</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend"><h3><asp:DropDownList ID="ddlLegendType" runat="server" DataTextField="FieldDescription" DataValueField="FieldID" Width="100%"></asp:DropDownList></h3></div></div>
                                    </div>
                                
                                    <div class="row">
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Select Color</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:DropDownList ID="ddlColorList" runat="server" Width="100%"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvColorList" runat="server" ControlToValidate="ddlColorList" InitialValue="Select" ErrorMessage="Please select color list." Display="None" ValidationGroup="SaveUpdate"></asp:RequiredFieldValidator>
                                            </h3>
                                       </div></div>
                                    </div>
                                    
                                    <div class="row" id="trCustomStatus" runat="server" visible="false">
                                        <div class="col-md-3"><div class="ColorLegend"><h2></h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend"><h3>
                                            <asp:LinkButton ID="lnkbAddCustomColor" runat="server" Text="Add Custom Color" OnClick="lnkbAddCustomColor_OnClick" />&nbsp;
                                            <asp:LinkButton ID="lnkbAddStatus" runat="server" Text="Add Appointment Status" OnClick="lnkbAddStatus_OnClick" />
                                        
                                        </h3></div></div>
                                    </div>
                                
                                    <div class="row" id="trAddCustomColor" runat="server" visible="false">
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Color Name</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:TextBox ID="txtColorName" Width="100%" runat="server" MaxLength="40" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtColorName" ValidationGroup="SaveColor" Display="None" runat="server" ErrorMessage="Enter Color Name !" Text="" />
                                            </h3>
                                        </div></div>
                                        
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Color Code</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:TextBox ID="txtColorCode" Width="100%" runat="server" MaxLength="10" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtColorCode" ValidationGroup="SaveColor" Display="None" runat="server" ErrorMessage="Enter Color Code !" Text="" />
                                            </h3>
                                        </div></div>
                                        
                                        <div class="col-md-3"><div class="ColorLegend"><h2></h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:Button ID="btnSaveColor" runat="server" CausesValidation="true" OnClick="btnSaveColor_OnClick" ToolTip="Save Color" CssClass="AdminBtn01" ValidationGroup="SaveColor" Text="Save Color" />&nbsp;&nbsp;
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="true" OnClick="btnCancel_OnClick" ToolTip="Cancel" CssClass="AdminBtn01" Text="Cancel" />
                                            </h3>
                                        </div></div>
                                        
                                    </div>
                                    
                                    
                                    
                                    <div class="row" id="trAddStatus" runat="server" visible="false">
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Status</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:TextBox ID="txtstatus" Width="100%" runat="server" MaxLength="40" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txtstatus" ValidationGroup="SaveStatus" Display="None" runat="server" ErrorMessage="Enter Status !" Text="" />
                                            </h3>
                                        </div></div>
                                        
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Status Color</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:DropDownList ID="ddlStatusColor" runat="server" Width="100%"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlStatusColor" InitialValue="Select" ErrorMessage="Please Select Status color." Display="None" ValidationGroup="SaveStatus"></asp:RequiredFieldValidator>
                                            </h3>
                                        </div></div>
                                        
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Status Code</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:TextBox ID="txtStatusCode" Width="100%" runat="server" MaxLength="10" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txtStatusCode" ValidationGroup="SaveStatus" Display="None" runat="server" ErrorMessage="Enter Status Code !" Text="" />
                                            </h3>
                                        </div></div>
                                        
                                        <div class="col-md-3"><div class="ColorLegend"><h2>Active</h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:DropDownList ID="ddlStatusActive" runat="server">
                                                    <asp:ListItem Text="Active" Value="1" Selected="True" />
                                                    <asp:ListItem Text="In-Active" Value="0" />
                                                </asp:DropDownList>
                                            </h3>
                                        </div></div>
                                        
                                        <div class="col-md-3"><div class="ColorLegend"><h2></h2></div></div>
                                        <div class="col-md-9"><div class="ColorLegend">
                                            <h3>
                                                <asp:Button ID="btnSaveStatus" runat="server" CausesValidation="true" OnClick="btnSaveStatus_OnClick" ToolTip="Save Status" CssClass="AdminBtn01" ValidationGroup="SaveStatus" Text="Save Status" />&nbsp;&nbsp;
                                                <asp:Button ID="btnCancelStatus" runat="server" CausesValidation="true" OnClick="btnCancelStatus_OnClick" ToolTip="Cancel Status" CssClass="AdminBtn01" Text="Cancel" />
                                            </h3>
                                        </div></div>
                                    </div>
                            
                                </div>
                            </div>
                        
                        
                        
                        
                            <div class="col-md-6">
                                <div class="ColorLegend01">
                                    <h2><asp:Label ID="lblOrders" runat="server" Text="Color Legends"></asp:Label></h2>
                                    <asp:Panel ID="pnlgvSelectedLegends" runat="server" ScrollBars="Auto" Height="200px" Width="100%">
                                        <asp:GridView SkinID="gridviewOrderNew" ID="gvSelectedLegends" CellPadding="1" CellSpacing="0" runat="server" AutoGenerateColumns="false" DataKeyNames="StatusId" ShowHeader="true"
                                            Width="100%" ForeColor="#333333" GridLines="None" PageSize="10" AllowPaging="false" ShowFooter="false" PageIndex="0" PagerSettings-Mode="NumericFirstLast" 
                                            HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" HeaderStyle-Wrap="false" HeaderStyle-BackColor="#DADADA" HeaderStyle-BorderColor="#DADADA" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" 
                                            
                                            PagerSettings-Visible="true" OnRowDataBound="gvSelectedLegends_OnRowDataBound">
                                            
                                            <Columns>
                                                <asp:BoundField DataField="StatusId" HeaderText="ServiceID" Visible="true" ReadOnly="true" />
                                                <asp:BoundField DataField="Status" HeaderText="Description" Visible="true" ReadOnly="true" />
                                                <asp:BoundField DataField="StatusColor" HeaderStyle-HorizontalAlign="Center" HeaderText="Color" Visible="true" ReadOnly="true" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>    
                            
                            
                            
                            
                            
                        </div>

                    
                    </div>
                 </div> 
                  
                  
                  
                  
            
                <table width="100%" cellpadding="0" cellspacing="0">
                                       
                    
                    <tr align="left" valign="top">
                        
                        
                        
                        
                        
                        <td style="width: 60%;">
                            
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>

    
    
</asp:Content>
