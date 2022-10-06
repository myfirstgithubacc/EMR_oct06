<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PriorityStatus.aspx.cs" Inherits="EMR_Problems_PriorityStatus" Title="Condition Name" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.css" rel="stylesheet" />
    <style type="text/css">  </style>
    <style type="text/css">
        .MyImageButton
        {
           cursor: hand;
        }
        .EditFormHeader td
        {
            font-size: 14px;
            padding: 4px !important;
            color: #0066cc;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>

<div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>

                <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-6 col-xs-6"><span class="hidden"><asp:Label ID="lblTitle" runat="server"  Font-Bold="true" /></span>
                       
                                        <label class="pull-left" style="margin: 2px 5px 0 0;"><asp:Label ID="lblValueName" runat="server" Text="Description<span style='color: Red'>*</span>"></asp:Label></label>
                                    
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:TextBox ID="txtValueName" SkinID="textbox" runat="server" Width="225px" 
                                                    MaxLength="100"></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    
                    </div>
                    <div class="col-md-6  col-xs-6 text-right">
                        
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save Data" OnClick="btnSaveData_OnClick" CssClass="btn btn-xs btn-primary"
                                                 ValidationGroup="SaveData" Text="Save" /> 
                                            
                                            <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="btn btn-xs btn-primary" 
                                                CausesValidation="false" onclientclick="window.close();" /> 
                                            <asp:Button ID="btnSetOrder" runat="server" ToolTip="Set Sequence" OnClick="btnSetOrder_OnClick"
                                                CssClass="btn btn-xs btn-primary" ValidationGroup="SaveData" Text="Set Sequence" /></div>
                        </div>
                </div>
                           
                <table border="0" width="100%" cellpadding="2" cellspacing="0">
                    <tr>
                       <td colspan="2" align="center" style="height: 13px; color: green; font-size: 12px; font-weight: bold;">
                            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Label ID="lblMsg" runat="server" Text="" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>                  
                    <tr>
                    <td>
                        
                                              
                                <telerik:RadGrid ID="gvDetails" Skin="Office2007" BorderWidth="0" AllowFilteringByColumn="True"
                                 runat="server" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"      
                                OnSortCommand="gvDetails_SortCommand"  
                                    onupdatecommand="gvDetails_UpdateCommand" oneditcommand="gvDetails_EditCommand" 
                                 OnItemCommand="gvDetails_ItemCommand" 
                                    oncancelcommand="gvDetails_CancelCommand" OnPreRender="gvDetails_PreRender" 
                                    onitemdatabound="gvDetails_ItemDataBound"   >
                                <MasterTableView AllowFilteringByColumn="true" DataKeyNames="ID" TableLayout="Fixed"  CssClass="table table-bordered" >
                                    <NoRecordsTemplate>
                                        <div style="font-weight: bold; color: Red;">
                                            No Record Found.</div>
                                    </NoRecordsTemplate>
                                    <Columns>
                                    <telerik:GridTemplateColumn Visible="false"  HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                    <asp:Label ID="hdnFieldID" runat="server" Text='<%# Eval("ID")%>'> </asp:Label>
                                                        <%--<asp:HiddenField ID="hdnFieldID"  runat="server" Value='<%#Eval("ID") %>'></asp:HiddenField>--%>
                                                        
                                                    </ItemTemplate>
                                      </telerik:GridTemplateColumn>
                                      <telerik:GridTemplateColumn Visible="false"  HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="hdnSeqNo" runat="server" Text='<%# Eval("SequenceNo")%>'> </asp:Label>
                                                        <%--<asp:HiddenField ID="hdnSeqNo" runat="server" Value='<%#Eval("SequenceNo") %>'></asp:HiddenField>--%>
                                                    </ItemTemplate>
                                      </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="Description" DefaultInsertValue="" HeaderText="Description"
                                           DataField="Description"  SortExpression="Description" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" 
                                            ShowFilterIcon="false"
                                            AllowFiltering="true" FilterControlWidth="70%" HeaderStyle-Width="50%">  
                                            <ItemTemplate>
                                                <asp:Label ID="lbltasktype" runat="server" Text='<%# Eval("Description")%>'> </asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>                                   
                                                <asp:TextBox ID="txtstatusname1" runat="server" SkinID="textbox" Text='<%# Eval("Description")%>'
                                                    MaxLength="100"></asp:TextBox>
                                            </EditItemTemplate>                                
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Up" AllowFiltering="false" HeaderTooltip="Up" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnMoveUP" runat="server" Text="UP" ToolTip="Move Up" CausesValidation="false"
                                                            CommandName="MoveUP" ImageUrl="/images/group_arrow_bottom.png" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Down" AllowFiltering="false" HeaderTooltip="Down" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnMoveDown" runat="server" Text="Down" ToolTip="Move Down"
                                                            CausesValidation="false" CommandName="MoveDown" ImageUrl="/images/group_arrow_top.png" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                
                                                <telerik:GridEditCommandColumn UniqueName="EditCommandColumn"  HeaderText="Edit"  />
                                                
                                                <telerik:GridTemplateColumn HeaderTooltip="Delete a row" AllowFiltering="false" HeaderText="Delete"  HeaderStyle-Width="50px"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" CommandName="Delete1"
                                                            ImageUrl="/images/DeleteRow.png" ToolTip="Click here to delete" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                        
                                        <%--<telerik:GridButtonColumn  ButtonType="ImageButton"
                                        CommandName="Delete" UniqueName="DeleteColumn">
                                        <HeaderStyle Width="20px" />
                                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                    </telerik:GridButtonColumn>--%>
                                    
                                    </Columns>
                                </MasterTableView>
                           <%--     <ClientSettings AllowColumnsReorder="True">
                                    <Resizing AllowColumnResize="true" />
                                </ClientSettings>--%>
                                </telerik:RadGrid>
                                
                            
                        </td>
                    </tr>
                    
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>

</body>
</html>
