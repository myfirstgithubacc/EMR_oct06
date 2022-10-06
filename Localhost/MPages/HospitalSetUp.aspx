<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="HospitalSetUp.aspx.cs" Inherits="MPages_HospitalSetUp" Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
            
            

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <table width="100%" border="0" cellpadding="0" cellspacing="1">
                                <div class="container-fluid header_main">
	                                <div class="col-md-3">
		                                <h2><asp:Label ID="Label2" runat="server" SkinID="label" Text="EMR Setup(Flag)" /></h2>
	                                </div>
	                                <div class="col-md-5">
	                                     <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:Label ID="lblMessage" runat="server" Text=""  CssClass="relativ text-center text-success alert_new"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
	                                
	                                </div>
	                                
	                                <div class="col-md-3 text-right pull-right">
                                	 <asp:Button ID="btnSave" runat="server" cssClass="btn btn-primary" Text="Save" OnClick="Save_OnClick" />
                                            <asp:ValidationSummary ID="VSHospital" runat="server" ShowMessageBox="True" ShowSummary="False" />
	                                </div>

                                </div>
                                
                                    
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                
            
            
            
            <br />
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-4 form-group">
                        <div class="col-md-4"> <asp:Label ID="Label1" runat="server" SkinID="label" Text="Facility" /></div>
                        <div class="col-md-8"><telerik:RadComboBox ID="ddlFacility" runat="server" Width="200px" AutoPostBack="true"
                                        EmptyMessage="[ Select ]" OnSelectedIndexChanged="ddlFacility_OnSelectedIndexChanged" /></div>
                    </div>
                    <div class="col-md-4 form-group">
                        <div class="col-md-4"> <asp:Label ID="Label3" runat="server" SkinID="label" Text="Module" /></div>
                        <div class="col-md-8"><telerik:RadComboBox ID="ddlModule" runat="server" Width="200px" AutoPostBack="true"
                                        EmptyMessage="[ Select ]" OnSelectedIndexChanged="ddlModule_OnSelectedIndexChanged" /></div>
                    </div>
                </div>
            </div>
          

               <div class="container-fluid form-group">
                    <div class="row">
                        <asp:Panel ID="Panel1" runat="server" SkinID="Panel" Width="850px" Height="450px"
                            ScrollBars="Vertical">
                            <asp:UpdatePanel ID="upgvServices" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvDetails" runat="server" SkinID="gridview" AutoGenerateColumns="false"
                                        Width="100%" OnRowDataBound="gvDetails_OnRowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" SkinID="label" Text='<%#Eval("Description") %>' />
                                                    <asp:HiddenField ID="FlagId" runat="server" Value='<%#Eval("FlagId") %>' />
                                                    <asp:HiddenField ID="hdnFlag" runat="server" Value='<%#Eval("Flag") %>' />
                                                    <asp:HiddenField ID="hdnFlagType" runat="server" Value='<%#Eval("FlagType") %>' />
                                                    <asp:HiddenField ID="hdnValue" runat="server" Value='<%#Eval("Value") %>' />
                                                    <asp:HiddenField ID="hdnModuleId" runat="server" Value='<%#Eval("ModuleId") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value(s)" HeaderStyle-Width="300px">
                                                <ItemTemplate>
                                                   
                                                   
                                                                <asp:TextBox ID="txtT" runat="server" SkinID="textbox" Width="300px" MaxLength="80" CssClass="pull-right" />
                                                            
                                                            
                                                                <asp:TextBox ID="txtN" runat="server" SkinID="textbox" MaxLength="7" Width="50px" CssClass="pull-right"
                                                                    Style="text-align: right" />
                                                                <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                    FilterType="Custom,Numbers" TargetControlID="txtN" ValidChars="0123456789." />
                                                            
                                                            
                                                                <asp:DropDownList ID="DDL" SkinID="DropDown" runat="server" Width="300px">
                                                                    <asp:ListItem Text="Select" Value="0" />
                                                                </asp:DropDownList>
                                                            
                                                            
                                                                <asp:RadioButtonList ID="RDO" SkinID="DropDown" runat="server" RepeatDirection="Horizontal" />
                                                            
                                                    
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
               </div>
               
               
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
