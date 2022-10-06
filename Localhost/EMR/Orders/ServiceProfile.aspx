<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServiceProfile.aspx.cs"
    Inherits="LIS_Format_ServiceProfile" Title="Service Profile" %>

<%@ Import Namespace="System.Drawing" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="Stylesheet" type="text/css" />


    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>

</head>
<body>
    <form id="form2" runat="server" visible="True">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>


            <div class="container-fluid header_main">
                 <div class="col-sm-8">
                  <h2>
                      <asp:Label ID="Label1" runat="server" SkinID="label" Text="Service Name" />&nbsp;
                        <asp:Label ID="ServiceName" runat="server" SkinID="label" Columns="50" />

                  </h2>
                 </div>
				 
                
                 
                 <div class="col-sm-4 text-right pull-right"><asp:Button ID="btnclose" Text="Close" runat="server" CssClass="btn btn-primary" ToolTip="Close" CausesValidation="false"
                             OnClientClick="window.close();" /> </div>
</div>
        <div class="container-fluid">
             <div class="text-center">  <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;"  />

                 </div>

        </div>


            



            <div class="container-fluid">

           
                        <telerik:RadGrid ID="gvSelectedFields" runat="server" Skin="Office2007" Width="100%" 
                            PagerStyle-ShowPagerText="false" AllowSorting="False" AllowMultiRowSelection="False"
                            EnableLinqExpressions="false" ShowGroupPanel="false" AutoGenerateColumns="False"
                            GroupHeaderItemStyle-Font-Bold="true" GridLines="none" 
                            OnItemDataBound="gvSelectedFields_ItemDataBound">
                            <MasterTableView Width="100%">
                                <NoRecordsTemplate>
                                    <div style="font-weight: bold; color: Red;">
                                        No Record Found.</div>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="SNo." HeaderStyle-Width="6%">
                                        <ItemTemplate>
                                           <asp:Label runat="server" ID="lblRowNumber" Width="50px" Text='<%# Container.DataSetIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Field Name" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnFieldID" runat="server" Value='<%#Eval("FieldID") %>' />
                                            <asp:HiddenField ID="hdnSeqNo" runat="server" Value='<%#Eval("SequenceNo") %>' />
                                            <asp:HiddenField ID="hdnFieldType" runat="server" Value='<%#Eval("FieldType") %>' />
                                            <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Unit / Range" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRangeValue" runat="server" Text='<%#Eval("RangeValue") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                   </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
