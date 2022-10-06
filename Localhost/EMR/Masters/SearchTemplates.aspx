<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchTemplates.aspx.cs"
    Inherits="EMR_Masters_SearchTemplates" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.TemplateId = document.getElementById("hdnRTemplateId").value;
            oArg.TemplateTypeId = document.getElementById("hdnRTemplateTypeId").value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdatePanel ID="UpdTemplateView" runat="server">
            <ContentTemplate>
            
            
           <div class="container-fluid header_main">
	            <div class="col-md-3">
		            <h2>   <asp:Label ID="Label1" runat="server" SkinID="label" Text="Search Template" /></h2>
	            </div>
	            <div class="col-md-5">
	                 <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="&nbsp;" CssClass="relativ text-success text-center alert_new" />
	            
	            </div>
	            
	            <div class="col-md-3 text-right pull-right">
            	  <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CausesValidation="false"
                                 CssClass="btn btn-primary" OnClientClick="window.close();" />
	            </div>

            </div>
            
              
                <br />
                
                <DIV class="container-fluid">
                    <div class="row">
                        <div class="col-md-4  form-group">
                            <div class="col-md-4"><asp:Label ID="Label4" runat="server" SkinID="label" Text='Specialisation' /></div>
                            <div class="col-md-8"><telerik:RadComboBox ID="ddlSpecialisation" SkinID="DropDown" runat="server" Width="100%"
                                MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlSpecialisation_SelectedIndexChanged" /></div>
                        </div>
                        <div class="col-md-4  form-group">
                            <div class="col-md-4"><asp:Label ID="Label5" runat="server" Text="Template&nbsp;For" /></div>
                            <div class="col-md-8"> <telerik:RadComboBox ID="ddlApplicableFor" SkinID="DropDown" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlApplicableFor_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="OP" Value="O" />
                                    <telerik:RadComboBoxItem Text="IP" Value="I" />
                                    <telerik:RadComboBoxItem Text="Both" Value="B" Selected="True" />
                                </Items>
                            </telerik:RadComboBox></div>
                        </div>
                    </div>
                
                
                <div class="row">
                    <div class="col-md-4 form-group">
                        <div class="col-md-4"><asp:Label ID="Label2" runat="server" SkinID="label" Text="Search&nbsp;Template" /></div>
                        <div class="col-md-8"><asp:Panel ID="Panel2" runat="server" DefaultButton="btnSearch">
                                <asp:TextBox ID="txtSearchValue" SkinID="textbox" runat="server" MaxLength="50" Width="100%" />
                            </asp:Panel></div>
                    </div>
                    <div class="col-md-4 form-group">
                        <div class="col-md-4"><asp:Label ID="Label7" runat="server" SkinID="label" Text="Template&nbsp;Type" /></div>
                        <div class="col-md-8"><telerik:RadComboBox ID="ddlTemplateType" SkinID="DropDown" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateType_SelectedIndexChanged" /></div>
                    </div>    
                    <div class="col-md-2 form-group">
                        <div class="col-md-4"><asp:Label ID="Label6" runat="server" SkinID="label" Text='<%$ Resources:PRegistration, status%>' /></div>
                        <div class="col-md-8"> <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" runat="server" Width="100%"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="2" />
                                    <telerik:RadComboBoxItem Text="Active" Value="1" />
                                    <telerik:RadComboBoxItem Text="In-Active" Value="0" />
                                </Items>
                            </telerik:RadComboBox></div>
                    </div>
                    <div class="col-md-1 form-group">
                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" Text="Search" Width="100%" runat="server"
                                ToolTip="Click to Search" OnClick="btnSearch_OnClick" />
                    </div>
                      <asp:Label ID="Label3" runat="server" SkinID="label" Text="Template&nbsp;Type" Visible="false" />
                            <asp:RadioButtonList ID="rdoTemplateType" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rdoTemplateType_SelectedIndexChanged"
                                Visible="false">
                                <asp:ListItem Text="Dynamic" Value="1" Selected="True" />
                                <asp:ListItem Text="Static" Value="2" />
                            </asp:RadioButtonList>
                </div>
                
                
                
                
                
                </DIV>
                
            
                   
                        
               
                            <%--OnPageIndexChanged="gvResultFinal_OnPageIndexChanged"--%>
                            <asp:Panel ID="PanelN" runat="server" BorderColor="#6699CC" BorderWidth="1" BorderStyle="Solid"
                                Width="99%" Height="450px" ScrollBars="None">
                                <telerik:RadGrid ID="gvResultFinal" runat="server" Width="100%" Skin="Office2007"
                                    BorderWidth="0px" AllowPaging="false" AllowCustomPaging="True" Height="99%" AllowMultiRowSelection="True"
                                    AutoGenerateColumns="False" ShowStatusBar="True" EnableLinqExpressions="False"
                                    GridLines="None" PageSize="15" OnColumnCreated="gvResultFinal_ColumnCreated"
                                    OnItemDataBound="gvResultFinal_OnItemDataBound" OnItemCommand="gvResultFinal_OnItemCommand"
                                    OnPreRender="gvResultFinal_OnPreRender" CellSpacing="0">
                                    <ClientSettings AllowColumnsReorder="false" Scrolling-AllowScroll="true" Scrolling-UseStaticHeaders="true"
                                        Scrolling-SaveScrollPosition="true">
                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" EnableDragToSelectRows="false" />
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                    </ClientSettings>
                                    <MasterTableView TableLayout="Fixed" GroupLoadMode="Client">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.</div>
                                        </NoRecordsTemplate>
                                        <EditFormSettings>
                                            <EditColumn FilterControlAltText="Filter EditCommandColumn column" />
                                        </EditFormSettings>
                                        <GroupHeaderItemStyle Font-Bold="true" />
                                        <GroupByExpressions>
                                            <telerik:GridGroupByExpression>
                                                <SelectFields>
                                                    <telerik:GridGroupByField FieldAlias="Specialisation" FieldName="Specialisation"
                                                        HeaderText='Specialisation' FormatString="" />
                                                </SelectFields>
                                                <GroupByFields>
                                                    <telerik:GridGroupByField FieldName="SpecialisationId" SortOrder="None" />
                                                </GroupByFields>
                                            </telerik:GridGroupByExpression>
                                        </GroupByExpressions>
                                        <CommandItemSettings ExportToPdfText="Export to PDF" />
                                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        </RowIndicatorColumn>
                                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        </ExpandCollapseColumn>
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="Template" HeaderText="Template">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkTemplateName" runat="server" Text='<%#Eval("TemplateName")%>'
                                                        CommandName="TempLink" CommandArgument='<%#Eval("TemplateId")%>' ToolTip="Click to set Sections and Fields of this Template" />
                                                    <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId")%>' />
                                                    <asp:HiddenField ID="hdnTemplateTypeId" runat="server" Value='<%#Eval("TemplateTypeID")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="MenuSequence" HeaderText="Sequence" HeaderStyle-Width="65px"
                                                ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMenuSequence" runat="server" SkinID="label" Text='<%#Eval("MenuSequence")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="ApplicableFor" HeaderText="Template&nbsp;For"
                                                HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApplicableFor" runat="server" SkinID="label" />
                                                    <asp:HiddenField ID="hdnApplicableFor" runat="server" Value='<%#Eval("ApplicableFor")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Type" HeaderText="Template&nbsp;Type" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server" SkinID="label" Text='<%#Eval("Type")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Code" HeaderText="Code" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCode" runat="server" SkinID="label" Text='<%#Eval("Code")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <%--<telerik:GridTemplateColumn UniqueName="Specialisation" HeaderText="Specialisation"
                                        HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSpecialisation" runat="server" SkinID="label" Text='<%#Eval("Specialisation")%>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                            <telerik:GridTemplateColumn UniqueName="Active" HeaderText='<%$ Resources:PRegistration, status%>'
                                                HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActive" runat="server" SkinID="label" Text='<%#Eval("Active")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Select" HeaderText='Edit' HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="SelectButton" runat="server" Text="Edit" CommandName="Select"
                                                        CommandArgument='<%#Eval("TemplateId")%>' ToolTip="Click to edit" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn UniqueName="Preview" HeaderText="" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnTempPreview" runat="server" Text="Preview" CommandName="Preview"
                                                         CssClass="btn btn-primary" CommandArgument='<%#Eval("TemplateId")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <FilterMenu EnableImageSprites="False">
                                    </FilterMenu>
                                </telerik:RadGrid>
                                <asp:HiddenField ID="hf1" runat="server" />
                            </asp:Panel>
                        
                        
                        
                        
                        
                            <asp:HiddenField ID="hdnRTemplateId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnRTemplateTypeId" runat="server" Value="0" />
                            <asp:UpdatePanel ID="updatepanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
