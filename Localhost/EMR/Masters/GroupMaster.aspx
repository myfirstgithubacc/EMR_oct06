<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRBlankMaster.master" AutoEventWireup="true" CodeFile="GroupMaster.aspx.cs" Inherits="EMR_Masters_GroupMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

 <%--<asp:HiddenField ID="hdnGroupid" runat="server" />--%>
 <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
        
        
            <div class="container-fluid header_main margin_bottom">
	            <div class="col-xs-3 col-md-3">
	            
		            <h2  id="tdHeader" runat="server"><asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Group Master" Font-Bold="true" /></h2>
	            </div>
            	
	            <div class="col-xs-5 col-md-5">
	                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" Width="100%" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
	            
	            </div>
            	
	            <div class="col-xs-4  col-md-5 text-right pull-right">
            	        <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" CssClass="btn btn-primary"
                            Text="New" OnClick="btnNew_OnClick" />
                        &nbsp;
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                            CssClass="btn btn-primary" Text="Save" />
                            <asp:Button ID="Button1" runat="server" ToolTip="Save&nbsp;Data" OnClientClick="window.close();"
                            CssClass="btn btn-primary" Text="Close" />
	            </div>

            </div>
        
        
          
             <div class="container-fluid">
                <div class="row">
                    <div class="col-md-6 col-xs-6 form-group">
                        <div class="col-md-6 col-xs-6"><asp:Label ID="Label1" runat="server"  Text="Group Name" /> 
                        <span style='color: Red'>*</span></div>
                        <div class="col-md-6 col-xs-6"> <asp:TextBox ID="hdnGroupid" runat="server" Visible="false" ></asp:TextBox>
                        <asp:TextBox ID="txtSpecialisationName"  runat="server" Width="100%" MaxLength="50" /></div>
                    </div>
                    
                    <div class="col-md-6 col-xs-6 form-group">
                        <div class="col-md-6 col-xs-6"><asp:Label ID="Label3" runat="server" SkinID="label" Text="Status"  /></div>
                        <div class="col-md-6 col-xs-6"><asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="Active" Value="1" Selected="true"/>
                        <asp:ListItem Text="In-Active" Value="0" />
                        
                        </asp:DropDownList></div>
                    </div>
                    
                </div>
                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        
                        <div class="col-xs-3 col-md-4"> <asp:CheckBox ID="chkDisplayInMenuNB" Text="Display In Nurse Workbench" runat="server" /></div>
                        <div class="col-xs-3 col-md-4"><asp:CheckBox ID="chkDisplayInMenuEMR" Text="Display In EMR " runat="server" /></div>
                        <div class="col-xs-3 col-md-4"><asp:CheckBox ID="chkDisplayInWard" Text="Display In Ward" runat="server" /></div>
                        <div class="col-xs-3 col-md-4 pull-right"><asp:CheckBox ID="chkPackageGroup" Text="Package Group" runat="server" /></div>
                        
                    </div>
                </div>
             </div>
             
             
            
            
            
            
            
            
                                 
                <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:GridView ID="gvDetails" runat="server" SkinID= "gridview2" Width="100%" AutoGenerateColumns="false"
                                        AllowPaging="True" PageSize="20" OnRowCommand="gvDetails_RowCommand" 
                                        OnPageIndexChanging="gvDetails_PageIndexChanging" 
                                        onrowcreated="gvDetails_RowCreated" 
                                        onrowdatabound="gvDetails_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="320px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpecialisationName" runat="server" SkinID="label" Text='<%#Eval("GroupName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Code" HeaderStyle-Width="120px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSpecialisationCode" runat="server" SkinID="label" Text='<%#Eval("Groupid") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActiveStatus" runat="server" SkinID="label" Text='<%#Eval("ActiveStatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="DisplayInEMR">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDisplayInEMR" runat="server" SkinID="label" Text='<%#Eval("DisplayInEMR") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DisplayInNurseWorkbench">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDisplayInNurseWorkbench" runat="server" SkinID="label" Text='<%#Eval("DisplayInNurseWorkbench") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                              <asp:TemplateField HeaderText="DisplayInWard">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDisplayInWard" runat="server" SkinID="label" Text='<%#Eval("DisplayInWard") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" SkinID="label" CommandName="EDT" CommandArgument='<%#Eval("Groupid") %>'
                                                        Text="Edit" />
                                                        
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           
                                            
                                        </Columns>
                                    </asp:GridView>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
              
    
            </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

