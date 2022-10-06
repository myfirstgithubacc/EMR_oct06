<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="GroupTaggingEMRTemplate.aspx.cs" Inherits="EMR_Masters_GroupTaggingEMRTemplate"
    Title="Group Tag With Template" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
         <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .RadGrid .rgFilterBox { height:22px !important;}
    </style>
    <script language="javascript" type="text/javascript">
        function checkCategoryName() {
            var txtCategoryName = document.getElementById('?ctl00_ContentPlaceHolder1_txtCategoryName');
            if (txtCategoryName.value == '') {
                alert('Please Enter The Category')
                return false;
            }
        }
        function lnkGroupMasterOnClick() {
           

            var x = screen.width / 2 - 1200 / 4;
            var y = screen.height / 2 - 550 / 2;
            var popup;

            popup = window.open("GroupMaster.aspx?", "Popup", "height=550,width=700,left=" + x + ",top=" + y + ", status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no");
            popup.focus();
            return false
        }
         
    </script>

    <asp:UpdatePanel ID="Update1" runat="server">
        <ContentTemplate>
            
            
            <div class="container-fluid header_main">
	            
	            <div class="col-md-3">
		            <h2>Template Group</h2>
	            </div>
	            <div class="col-md-5 text-center">
	                <asp:Label ID="lblMessage"  runat="server" Text="&nbsp;"  CssClass="alert_new text-center text-success relativ" />
	            </div>
	            <div class="col-md-3 text-right pull-right">
            	<asp:Button ID="btnSave" runat="server" Width="100px" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                        <asp:HiddenField ID="hdnisSaveChk" runat="server" Value="0" />
	            </div>

            </div>
            
          <div class="container-fluid subheading_main">
                <div class="col-md-1 label1"> Group</div>
                <div class="col-md-3 form-group">
                    <telerik:RadComboBox ID="ddlTaggedFor" SkinID="DropDown" runat="server" Width="100%"
                             AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                            OnSelectedIndexChanged="ddlTaggedFor_OnSelectedIndexChanged" />
                </div>
                <div class="col-md-1 label1">
                    <asp:LinkButton ID="lnkGroupMaster" runat="server" OnClientClick="lnkGroupMasterOnClick();">Add Group</asp:LinkButton>
                </div>
                <div id="trItemSearch" visible="false" runat="server"></div>
          </div>
            
            
            
            
            <div class="col-md-12 form-group">
            <div class="row">
                    <div class="col-md-6">
                            <div class=" table table-responsive">
                            <h5 class="h5"><asp:Label ID="lblEMRTemplate" runat="server" Text="EMR Template"  ></asp:Label></h5>
                    
                             <asp:Panel ID="pnlDepart" runat="server" Height="470px" Width="100%" ScrollBars="Vertical">
                                    
                                        <telerik:RadGrid ID="gvTemplate" Skin="Office2007" BorderWidth="0px" PagerStyle-ShowPagerText="false"
                                            AllowFilteringByColumn="True" runat="server" Width="100%"
                                            AutoGenerateColumns="False" ShowStatusBar="True" 
                                            EnableLinqExpressions="False" AllowPaging="True" Height="350px" PageSize="15" 
                                             OnPreRender="gvTemplate_PreRender" OnPageIndexChanged="gvTemplate_OnPageIndexChanged"
                                              OnItemCommand="gvTemplate_OnItemCommand" 
                                            OnItemDataBound="gvTemplate_ItemDataBound" CellSpacing="0"  >
                                            <GroupingSettings CaseSensitive="false" />
                                            <FilterMenu EnableImageSprites="False">
                                            </FilterMenu>
                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                <Selecting AllowRowSelect="false" UseClientSelectColumnOnly="true" />
                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                    AllowColumnResize="false" />
                                            </ClientSettings>
                                            <PagerStyle Mode="NumericPages"/>
                                            <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true">
                                                <NoRecordsTemplate>
                                                    <div style="font-weight: bold; color: Red;">
                                                        No Record Found.</div>
                                                </NoRecordsTemplate>
                                                <EditFormSettings>
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                </EditFormSettings>
                                                <ItemStyle Wrap="true" />
                                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                                                    Visible="True">
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                                                    Visible="True">
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridTemplateColumn UniqueName="SNo" HeaderText="S. No." AllowFiltering="false">
                                                        <ItemTemplate>
                                                           <asp:Literal ID="ltrSno" runat="server" Text='<%#(Container.DataSetIndex+1)%>'></asp:Literal>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="TemplateName" ShowFilterIcon="false" DefaultInsertValue=""
                                                        DataField="TemplateName" AutoPostBackOnFilter="true" CurrentFilterFunction="StartsWith"
                                                        HeaderText='Template Name' FilterControlWidth="99%"
                                                        ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="70%">
                                                        <ItemTemplate>
                                                             <asp:Label ID="lblColumnName" runat="server" Text='<%# Eval("TemplateName") %>'></asp:Label>
                                                             <asp:HiddenField ID="hdnColumnId" runat="server" Value='<%#Eval("id")%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="70%" Wrap="False" />
                                                        <ItemStyle Wrap="False" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn UniqueName="Add" AllowFiltering="false" HeaderText='Add'
                                                        HeaderStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" CommandName="Add" CommandArgument='<%#Eval("id")%>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle Width="20%" />
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                            
                                        
                                    </asp:Panel>
                            </div>
                    
                    
                    
                    
                    
                    
                    
                        
                    </div>
                    
                    
                    <div class="col-md-6"><table width="100%">
                            <tr>
                                <td class="clsheader" colspan="2">
                                    <asp:Label ID="lblTaggingGroup" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="Panel1" runat="server" Height="430px" Width="100%" ScrollBars="Vertical">
                                        <asp:GridView ID="grvGroupWiseTag" SkinID="gridview2" runat="server" BorderWidth="0"
                                            AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                            ShowHeader="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SNo" >
                                                    <ItemTemplate>
                                                       <asp:Literal ID="ltrSno" runat="server" Text='<%#(Container.DataItemIndex+1)%>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="90%" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>                                                        
                                                        <asp:Label ID="lblSelTemplateName" runat="server" Text='<%# Eval("TemplateName") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnSelTemplateId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                        <asp:HiddenField ID="hdnGroupId" runat="server" Value='<%#Eval("GroupId") %>' />
                                                        <asp:HiddenField ID="hdnGroupName" runat="server" Value='<%#Eval("GroupName") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_OnClick"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%; text-align: left;">
                                    &nbsp;</td>
                                <td align="right" style="padding-right: 20px;">
                                    &nbsp;</td>
                            </tr>
                        </table></div>
            </div><!-- end of row -->
            </div><!--end of main-container -->
            
            
         
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
