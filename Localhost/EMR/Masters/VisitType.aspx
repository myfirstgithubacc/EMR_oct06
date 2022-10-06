<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="VisitType.aspx.cs" Inherits="EMR_Masters_DoctorDuration" Title="" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />   
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/Administration-Item.css" rel="stylesheet" type="text/css" />
    
    
    

        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
            
                <div class="VisitHistoryDiv01a">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-9"><div class="OrderSetDivText"><h2><asp:Label ID="lblDoctorVisitDuration" runat="server" Text="Visit Types"></asp:Label></h2></div></div>
                            <div class="col-md-3">
                                <asp:Button ID="ibtnSaveVisit" runat="server"  ToolTip="Save Visit" OnClick="SaveVisit_OnClick" CssClass="VisitTypeBtn" ValidationGroup="SaveVisit" Text="Save" />
                                <asp:LinkButton ID="lnkvisittypetaging" runat="server" CssClass="VisitTypeBtn" Text="Visit Type/Facility Tagging" OnClick="lnkvisittypetaging_Click"></asp:LinkButton>

                                <asp:ValidationSummary ID="VSDoctorVisitDuration" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="SaveVisit" />
                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="SaveVisitDuration" />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="Update" />
                                <asp:ValidationSummary ID="ValidationSummary4" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="UpdateVisit" />
                                <asp:HiddenField ID="hdnCurrentDate" runat="server" />
                                <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="850" Height="500" Left="10" Top="10" VisibleStatusbar="false" Behaviors="Close,Move,Minimize,Maximize,Resize,Pin" Modal="true"></telerik:RadWindowManager>
                                <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                    <Windows><telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move" /></Windows>
                                </telerik:RadWindowManager>
                            </div>
                        </div>
                    </div>
                </div>
            
            
            
                <div class="AuditTrailDiv">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="AuditMessage02">
                                    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                                        <Triggers><asp:AsyncPostBackTrigger ControlID="ibtnSaveVisit" EventName="Click" /></Triggers>
                                        <ContentTemplate><asp:Label ID="lblMessage" runat="server" Text="" /></ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>    
                     </div>
                  </div> 
                  
                  
                  
                  <div class="ItemServiceDiv">
                        <div class="container-fluid">
                        
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="VT-Department">
                                        <h2><asp:Label ID="ltrlVisit" runat="server" Text="Visit&nbsp;Name&nbsp;<span class='red'>*</span>"></asp:Label></h2>
                                        <h3>
                                            <asp:TextBox ID="txtVisitName" runat="server" CssClass="VT-DepartmentInput" MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtVisitName" ValidationGroup="SaveVisit" Display="None" runat="server" ErrorMessage="Enter Visit Name" Text="*"></asp:RequiredFieldValidator>
                                        </h3>
                                    </div>
                                </div>
                                
                                
                                <div class="col-md-6">
                                    <div class="VT-Department">
                                        <h2><asp:Label ID="Label1" runat="server" Text="Visit Type&nbsp;<span class='red'>*</span>"></asp:Label></h2>
                                        <h3>
                                            <asp:DropDownList ID="ddlEM" CssClass="VT-DepartmentDown" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlEM" ValidationGroup="SaveVisit" Display="None" runat="server" InitialValue="0" ErrorMessage="Select E&M Type">*</asp:RequiredFieldValidator>
                                        </h3>
                                    </div>
                                
                                </div>
                            </div>
                        
                        </div>
                  </div>
                        
            
            
                <div class="ItemServiceDiv">
                    <div class="container-fluid">
                    
                        <div class="row">
                            <div class="col-md-12"><div class="VT-forVisit"><h2><asp:Label ID="Label2" runat="server" Text=" For Visit "></asp:Label></h2></div></div>
                        
                        
                            <div class="col-md-12">
                            
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="ibtnSaveVisit" EventName="Click" /></Triggers>
                                    
                                    <ContentTemplate>
                                        <asp:GridView ID="gvVisit" SkinID="gridview" CellPadding="4" runat="server" AutoGenerateColumns="False" DataKeyNames="VisitTypeID" Width="99%" PageSize="15" AllowPaging="True" 
                                            PagerSettings-Mode="NumericFirstLast" PagerSettings-Visible="true" OnRowDataBound="gvVisit_OnRowDataBound" OnRowCommand="gvVisit_OnRowCommand" OnRowCancelingEdit="gvVisit_OnRowCancelingEdit" 
                                            
                                            HeaderStyle-ForeColor="#15428B" HeaderStyle-Height="25px" 
                                            HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" HeaderStyle-BorderWidth="0" BackColor="White" BorderColor="#eeeeee" BorderStyle="None" BorderWidth="1px" 
                                            
                                            OnRowUpdating="gvVisit_OnRowUpdating" OnPageIndexChanging="gvVisit_OnPageIndexChanging" HeaderStyle-HorizontalAlign="Left" OnRowEditing="gvVisit_OnRowEditing">
                                            <PagerSettings Mode="NumericFirstLast" />
                                            
                                            <RowStyle Height="21px" />
                                            <Columns>
                                                <asp:BoundField DataField="VisitTypeID" HeaderText="VisitTypeID" Visible="true" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="S No" HeaderStyle-Width="35px">
                                                    <ItemTemplate><asp:Label ID="lblSerial" runat="server"></asp:Label></ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Visit Name" HeaderStyle-Width="230px">
                                                    <ItemTemplate><asp:Literal ID="ltrlGridVisitName" runat="server" Text='<%#Eval("Type")%>'></asp:Literal></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtGridVisitName" SkinID="textbox" runat="server" Text='<%#Eval("Type")%>' MaxLength="50" Width="98%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFVtxtGridVisitName" runat="server" ControlToValidate="txtGridVisitName" ErrorMessage="Name Cannot Be Blank" ForeColor="White" ValidationGroup="UpdateVisit" Display="None" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Width="230px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Visit Type" HeaderStyle-Width="240px">
                                                    <ItemTemplate><asp:Label ID="lblemtype" runat="server" Text='<%# Eval("EMType") %>'></asp:Label></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlEMtype" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("EMTypeId") %>' DataSourceID="sqDSource" DataTextField="EMType" DataValueField="EMTypeId" AppendDataBoundItems="true"></asp:DropDownList>
                                                        <asp:SqlDataSource ID="sqDSource" EnableCaching="true" runat="server" ConnectionString="<%$ ConnectionStrings:akl%>" SelectCommandType="Text" SelectCommand="select EMTypeId,EMType from EMRVisitTypeEM"></asp:SqlDataSource>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Width="240px" />
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Is Package Visit" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false">
                                                    <ItemTemplate><asp:Label ID="lblPackageVisit" SkinID="label" runat="server" Text='<%#Eval("IsPackageVisitStatus")%>'></asp:Label></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlGridPackage" SkinID="DropDown" Width="98%" runat="server" SelectedValue='<%#Eval("IsPackageVisit")%>'>
                                                            <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="No PA For InsurancePatient" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false">
                                                    <ItemTemplate><asp:Label ID="lblNoPAForInsurancePatient" SkinID="label" runat="server" Text='<%#Eval("NoPAForInsurancePatientStatus")%>'></asp:Label></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlGridNoPAForInsurancePatient" SkinID="DropDown" Width="98%" runat="server" SelectedValue='<%#Eval("NoPAForInsurancePatient")%>'>
                                                            <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="No Bill For PrivatePatient " ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false">
                                                    <ItemTemplate><asp:Label ID="lblNoBillForPrivatePatient" SkinID="label" runat="server" Text='<%#Eval("NoBillForPrivatePatientStatus")%>'></asp:Label></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlGridNoBillForPrivatePatient" SkinID="DropDown" Width="98%" runat="server" SelectedValue='<%#Eval("NoBillForPrivatePatient")%>'>
                                                            <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                    <ItemTemplate><asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label></ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                
                                                <asp:CommandField ShowEditButton="true" HeaderText="Edit" ItemStyle-Width="50px" ValidationGroup="UpdateVisit" HeaderStyle-Width="90px">
                                                    <HeaderStyle Width="90px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="Active" HeaderText="Active" ReadOnly="True" />
                                            </Columns>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            
                            </div>
                        
                        </div>
                    
                    
                    
                    
                    
                    </div>
                </div>
            
            
            
            
               
                <table border="0" width="97%" cellpadding="2" cellspacing="0">
                    
                   
                    

                   
                    
                    <tr>
                        <td colspan="5">
                            
                        </td>
                    </tr>
                    
                </table>
                
                
                
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>
