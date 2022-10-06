<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddTreatmentPlan.aspx.cs" Inherits="EMR_Masters_AddTreatmentPlan" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updMessage" runat="server">
            <ContentTemplate>
            
            <div class="margin_bottom container-fluid header_main">
	            <div class="col-sm-3">
		            <h2><asp:Label ID="lblHeaderText" runat="server" Text="Add Treatment Plan"></asp:Label></h2>
	            </div>
            	
	            <div class="col-sm-5"><asp:Label ID="lblMessage" runat="server" CssClass="relativ alert_new text-success text-center"></asp:Label></div>
            	
	            <div class="col-sm-3 text-right pull-right">
            	
	            </div>
            </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-6 form-group">
                            <div class="col-sm-5"><asp:Label ID="Label2" runat="server" SkinID="label" Text="Template Name"></asp:Label></div>
                            <div class="col-sm-7"><asp:TextBox ID="txtInvestigationSet" SkinID="textbox" runat="server" MaxLength="80"
                                Width="100%"></asp:TextBox></div>
                        </div>
                        <div class="col-sm-6 form-group">
                            <div class="col-sm-5"> <asp:Label ID="Label1" runat="server" SkinID="label" Text="Department"></asp:Label></div>
                            <div class="col-sm-7"><telerik:RadComboBox ID="ddlDepartment" runat="server" AllowCustomText="true" Filter="StartsWith"
                                Width="100%" AppendDataBoundItems="true" 
                                 onselectedindexchanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true" >
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" />
                                </Items>
                            </telerik:RadComboBox></div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-sm-6 form-group">
                            <div class="col-sm-5"><asp:Label ID="lblProvider" runat="server" Text='<%$ Resources:PRegistration, Doctor%>' /></div>
                            <div class="col-sm-7"> <telerik:RadComboBox ID="ddlProvider" runat="server"  Filter="Contains"
                                Width="100%"  AutoPostBack="true"  AllowCustomText="true" >
                                 <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="" />
                                </Items>
                                </telerik:RadComboBox> </div>
                        </div>
                        
                          <div class="col-sm-6 form-group">
                            <div class="col-sm-4"><asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" /> 
                                <asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close"  OnClientClick ="window.close();" />
                                 <%--<asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClick="btnClose_Click" />--%>

                            </div>
                          
                        </div>
                        
                    </div>
                </div>
                
                
                
                
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlOrderSet" runat="server" ScrollBars="Vertical">
                                <asp:GridView ID="gvOrderSet" SkinID="gridview" runat="server" AutoGenerateColumns="false"
                                    Width="97%" ShowHeader="true" OnRowCommand="gvOrderSet_RowCommand" PageSize="20"
                                    PageIndex="0" PagerSettings-Mode="NumericFirstLast">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="100px" HeaderText="Template Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSetName" runat="server" SkinID="label" Text='<%#Eval("SetName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="Department Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label11" runat="server" SkinID="label" Text='<%#Eval("DepartmentName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="150px" HeaderText="DoctorName">
                                            <ItemTemplate>
                                                <asp:Label ID="Label66" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="lnkEdit_OnClik"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                    CommandName="Del" CommandArgument='<%#Eval("SetId")%>' />
                                                <asp:HiddenField ID="hdnSetId" runat="server" Value='<%#Eval("SetId")%>' />
                                                <asp:HiddenField ID="hdnDepartmentId" runat="server" Value='<%#Eval("DepartmentId")%>' />
                                                <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:HiddenField ID="hdnSelectedSetId" runat="server" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSave" />
<asp:PostBackTrigger ControlID="ddlDepartment"></asp:PostBackTrigger>

<asp:PostBackTrigger ControlID="ddlDepartment"></asp:PostBackTrigger>

<asp:PostBackTrigger ControlID="ddlDepartment"></asp:PostBackTrigger>

<asp:PostBackTrigger ControlID="ddlDepartment"></asp:PostBackTrigger>

            </Triggers>
            <Triggers>
            <asp:PostBackTrigger ControlID ="ddlDepartment" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
