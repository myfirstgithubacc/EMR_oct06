<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VisitTypeTaggingFacilityWise.aspx.cs"
    Inherits="EMR_Masters_VisitTypeTaggingFacilityWise" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Visit Type / Facility Tagging </title>
    
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />   
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/Administration-Item.css" rel="stylesheet" type="text/css" />
    
    
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    
    
</head>
<body>




    <form id="form1" runat="server">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                
                
                <div class="VisitHistoryDiv">
                    <div class="container-fluid">
                        <div class="row">
                        
                            <div class="col-md-8">
                                <div class="AuditMessage01"><asp:Label ID="lblMessage" runat="server" Text="" /></div>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="VisitTypeBtn01" OnClientClick="window.close();" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="VisitTypeBtn01" OnClick="btnSave_Click" />
                                <asp:Button ID="btnNew" runat="server" Text="New" CssClass="VisitTypeBtn01" OnClick="btnNew_Click" />
                            </div>
                        
                        </div>
                    </div>
                </div>
                
                
                
                <div class="ItemServiceDiv">
                    <div class="container-fluid">
                    
                        <div class="row">
                        
                            <div class="col-md-6">
                                <div class="VT-Department01">
                                    <h2><asp:Label ID="Label1" runat="server" Text="Visit&nbsp;<span style='color: Red'>*</span>"></asp:Label></h2>
                                    <h3><telerik:RadComboBox ID="ddlVisitType" runat="server" EmptyMessage="/Select" Filter="Contains" AutoPostBack="true" EnableLoadOnDemand="true" CssClass="VT-DepartmentDown01" OnSelectedIndexChanged="ddlVisitType_OnSelectedIndexChanged"></telerik:RadComboBox></h3>
                                </div>
                            </div>
                            
                            <div class="col-md-6">
                                <div class="VT-Department01">
                                    <h2><asp:Label ID="Label2" runat="server" Text="Facility&nbsp;<span style='color: Red'>*</span>"></asp:Label></h2>
                                    <h3>
                                        <telerik:RadComboBox ID="ddlFacility" runat="server" EmptyMessage="/Select" Filter="Contains" EnableCheckAllItemsCheckBox="true" CheckBoxes="true" CssClass="VT-DepartmentDown01" EnableLoadOnDemand="true"></telerik:RadComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddlFacility" ValidationGroup="SaveVisit" Display="None" runat="server" InitialValue="0" ErrorMessage="Select Facility">*</asp:RequiredFieldValidator>
                                    </h3>
                                </div>    
                            </div>
                        </div>
                        
                        
                        
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gvVisitType" SkinID="gridview" CellPadding="4" runat="server" AutoGenerateColumns="false"
                                    ShowHeader="true" Width="100%" AllowPaging="false" PagerSettings-Mode="NumericFirstLast"
                                    ShowFooter="false" PagerSettings-Visible="true" HeaderStyle-HorizontalAlign="Left"
                                    OnRowDataBound="gvVisitType_OnDataBound" OnRowCancelingEdit="gvVisitType_OnRowCancelingEdit"
                                    
                                    HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" 
                                    HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" 
                                    HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" 
                                    
                                    OnRowUpdating="gvVisitType_OnRowUpdating" OnRowEditing="gvVisitType_OnRowEditing">
                                    <HeaderStyle CssClass="GVFixedHeader" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Visit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVisitType" Text='<%#Eval("Name")%>' runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnId" Value='<%#Eval("Id")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <HeaderStyle Width="80px" />
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:CommandField ShowEditButton="true" HeaderText="Edit" ItemStyle-Width="50px"
                                            ValidationGroup="UpdateVisit" HeaderStyle-Width="90px">
                                            <HeaderStyle Width="90px" />
                                            <ItemStyle Width="50px" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="Active" HeaderText="Active" ReadOnly="True" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        
                        </div>
                        
                        
                    
                    </div>
                </div>    
                    
                
                
                
            </ContentTemplate>
        </asp:UpdatePanel>

    </form>
    
    
    
    
    
</body>
</html>
