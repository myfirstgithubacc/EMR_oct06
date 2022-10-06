<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImmunizationMaster.aspx.cs"
    Inherits="EMR_Immunization_ImmunizationMaster" MasterPageFile="~/Include/Master/EMRMaster.master"
    Title="" %>
    <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
            <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    
                
                
                
                <div class="container-fluid header_main">
                    <div class="col-md-3"><h2>Immunization Master</h2></div>
                    <div class="col-md-3"><asp:Label ID="lblMessage" runat="server" CssClass=" alert_new relativ text-center text-success" /></div>
                    <div class="col-md-3 pull-right text-right">
                            <asp:Button ID="ibtnSaveImmunizationMaster" runat="server" CssClass="btn btn-primary" CausesValidation="true"
                                        OnClick="SaveImmunizationMaster_OnClick" ToolTip="Save" ValidationGroup="SaveUpdate"
                                        Text="Save" />
                                    <asp:ValidationSummary ID="VSImmunizationMaster" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" ValidationGroup="SaveUpdate" />
                    </div>
                </div>
                
                
                <div class="container-fluid subheading_main">
                           <div class="col-md-3 form-group">
                                <div class="col-md-5 label1"><asp:Literal ID="ltrlName" runat="server" Text="Name"></asp:Literal><span style="color: Red;">
                                        *</span></div>
                                <div class="col-md-7"><asp:TextBox ID="txtName"  SkinID="textbox" runat="server" TextMode="SingleLine"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="RFVtxtName" runat="server" ErrorMessage="Name Cannot Be Blank..."
                                        SetFocusOnError="true" ControlToValidate="txtName" Display="None" ValidationGroup="SaveUpdate"></asp:RequiredFieldValidator></div>
                           </div>
                           
                           <div class="col-md-3 form-group">
                                <div class="col-md-5 label1"> <asp:Literal ID="ltrlCPTCode" runat="server" Text="CPT Code"></asp:Literal><span></span></div>
                                <div class="col-md-7">  <asp:TextBox ID="txtCPTCode" SkinID="textbox" runat="server"  ></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="RFVCPTCode" runat="server" ErrorMessage="CPT Code Cannot Be Blank..."
                                        Display="None" SetFocusOnError="true" ControlToValidate="txtCPTCode" ValidationGroup="SaveUpdate"></asp:RequiredFieldValidator>--%></div>
                           </div>
                           
                           <div class="col-md-3 form-group">
                                <div class="col-md-5 label1"><asp:Literal ID="ltrlCVXCode" runat="server" Text="CVX Code"></asp:Literal><span></span></div>
                                <div class="col-md-7"> <asp:TextBox ID="txtCVXCode" SkinID="textbox" runat="server"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="CVX Code Cannot Be Blank..."
                                        Display="None" SetFocusOnError="true" ControlToValidate="txtCVXCode" ValidationGroup="SaveUpdate"></asp:RequiredFieldValidator>--%></div>
                           </div>
                           
                           <div class="col-md-3 text-right label1">
                            <asp:LinkButton ID="lnkImmunizationMaster" runat="server"  Text="Immunization Scheduler" OnClick="lnkImmunizationMaster_OnClick"></asp:LinkButton>
                           </div>
                           
                        
                </div>
                
                
                
                
                
                
                      
                              
                                    <asp:GridView ID="gvImmunization" SkinID="gridview" CellPadding="4" runat="server"
                                        AutoGenerateColumns="false" DataKeyNames="ImmunizationId" ShowHeader="true" Width="99%"
                                        PagerSettings-Mode="NumericFirstLast" ShowFooter="false" PagerSettings-Visible="true"
                                        OnRowDataBound="gvImmunization_OnRowDataBound" OnRowCommand="gvImmunization_OnRowCommand"
                                        OnRowEditing="gvImmunization_OnRowEditing" OnRowCancelingEdit="gvImmunization_OnRowCancelingEdit"
                                        OnRowUpdating="gvImmunization_OnRowUpdating" HeaderStyle-HorizontalAlign="Left">
                                        <Columns>
                                            <asp:BoundField DataField="ImmunizationId" HeaderText="ImmunizationId" ReadOnly="true" />
                                            <asp:TemplateField Visible="True" HeaderText="S No">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ImmunizationName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNameImmunizationGrid" SkinID="label" runat="server" Text=' <%#Eval("ImmunizationName")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtNameGrid" SkinID="textbox" runat="server" Text='<%#Eval("ImmunizationName")%>'
                                                        MaxLength="100" TextMode="SingleLine" Width="99%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RFVtxtNameGrid" runat="server" ErrorMessage="Name Cannot Be Blank..."
                                                        Display="None" SetFocusOnError="true" ControlToValidate="txtNameGrid" ValidationGroup="Edit"></asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CPTCode" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#Eval("CPTCode")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtCPTCodeGrid" SkinID="textbox" Text='<%#Eval("CPTCode") %>' runat="server"
                                                        MaxLength="10" Width="80px"></asp:TextBox>
                                                    
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CVXCode">
                                                <ItemTemplate>
                                                    <%#Eval("CVXCode")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtCVXCOdeGrid" SkinID="textbox" runat="server" Text='<%#Eval("CVXCode")%>'
                                                        MaxLength="100" TextMode="SingleLine" Width="80px"></asp:TextBox>
                                                    
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="80px" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGridActive1" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblGridActive2" SkinID="label" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                                                    <asp:DropDownList ID="ddlGridStatus" SkinID="DropDown" runat="server" SelectedValue='<%#Eval("Active")%>'>
                                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Active" HeaderText="Active" ReadOnly="true" ItemStyle-Width="60px"
                                                HeaderStyle-Width="60px" />
                                            <asp:TemplateField ItemStyle-Width="30px" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="/Images/DeleteRow.png"
                                                        CommandName="DeActivate" CommandArgument='<%#Eval("ImmunizationId")%>' ToolTip="DeActivate"
                                                        ValidationGroup="Cancel" CausesValidation="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CommandField ShowEditButton="true" ItemStyle-Width="35px" HeaderStyle-Width="35px"
                                                CausesValidation="true" ValidationGroup="Edit" />
                                        </Columns>
                                    </asp:GridView>
                                  
                                    <asp:ValidationSummary ID="VSGrid" runat="server" ShowMessageBox="True" ShowSummary="False"
                                        ValidationGroup="Edit" />
                           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
