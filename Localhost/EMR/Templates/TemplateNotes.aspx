<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="TemplateNotes.aspx.cs" Inherits="EMR_Templates_TemplateNotes" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

    <style type="text/css">
        td input {
            float: none !important;
        }

        .paddingtop {
            padding-top: 6px !important;
        }
    </style>
    <script type="text/javascript">
        window.onbeforeunload = function(evt) {
            var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            if (IsUnsave == 1) {
                 return false;
            }
        }

        $('iframe').attr('scrolling', 'no');
    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="container-fluid">
                <div class="row header_main">
                    <div class="col-md-5 col-sm-5 col-xs-4" id="tdHeader" runat="server"><asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Template Notes" Font-Bold="true" /></div>
                    <div class="col-md-7 col-sm-7 col-xs-7 text-right">
                        <asp:Button ID="btnClose" Visible="false" runat="server" Text="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                                    <asp:HiddenField ID="hdnIsUnSavedData" runat="server" /></div>
                </div>
           
                <div class="row" style="background:#ffe8d0;">
                    <div class="col-md-12 p-t-b-5">
                     <asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label>
                        </div>
                     </div>
                <div class="row">
                    <div class="col-md-12 p-t-b-5">
                         <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-7 col-sm-7 col-xs-12 " style="margin-left: 20%;">
                        <div class="col-md-12 col-sm-12 col-xs-12 border-all" id="tblAll" runat="server">
                            <div class="row" style="background:#cbd6fa;">
                                <div class="col-md-12 col-sm-12 col-xs-12 p-t-b-5 text-center">
                                    <asp:RadioButtonList ID="rdbList" Font-Bold="true" runat="server" AutoPostBack="true" Width="93%"
                                        OnSelectedIndexChanged="rdblist_OnSelectedIndexChanged" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Favourite" Value="0" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Speciality" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Template Group" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="All" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-7 col-sm-7 col-xs-12">
                                    <div class="row p-t-b-5">
                                        <div class="col-md-4 colsm-4 col-xs-4 text-nowrap paddingtop">
                                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="All Template(s)" Style="color: #444 !important;" />
                                        </div>
                                        <div class="col-md-8 colsm-8 col-xs-8">
                                            <div class="row">
                                        <div class="col-md-8 col-sm-8 col-x-8">
                                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAllTemplateSearch">
                                        <asp:TextBox ID="txtAllTemplateSearch" runat="server" SkinID="textbox" MaxLength="50"
                                            Width="100%" />
                                    </asp:Panel>
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-x-4">
                                             <asp:Button ID="btnAllTemplateSearch" runat="server" CssClass="btn btn-primary" Text="Search"
                                        OnClick="btnAllTemplateSearch_OnClick" />
                                        </div>
                                    </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-xs-12" style="padding-top: 6px;">

                                    <telerik:RadComboBox ID="ddlTempGroup" Visible="false" Width="100%" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlTempGroup_OnSelectedIndexChanged" runat="server">
                                    </telerik:RadComboBox>
                                </div>
                            </div>
                            <div class="row m-t">
                                <div class=" gridview">
                                    <asp:Panel ID="PanelN" runat="server" SkinID="Panel" Width="100%" style="max-height: 400px; overflow: auto;">
                                        <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>--%>
                                        <asp:GridView ID="gvAllTemplate" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                            AllowPaging="True" PageSize="15" OnRowDataBound="gvAllTemplate_OnRowDataBound"
                                            OnRowCommand="gvAllTemplate_RowCommand" OnPageIndexChanging="gvAllTemplate_PageIndexChanging">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Template Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName") %>' />
                                                        <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId") %>' />
                                                        <%-- <asp:HiddenField ID="hdnTemplateTypeID" runat="server" Value='<%#Eval("TemplateTypeID") %>' />
                                                                <asp:HiddenField ID="hdnTemplateTypeCode" runat="server" Value='<%#Eval("TemplateTypeCode") %>' />
                                                                <asp:HiddenField ID="hdnEntryType" runat="server" Value='<%#Eval("EntryType") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Add Favourite" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkAddFavourite" runat="server" SkinID="label" CommandName="Add"
                                                            CommandArgument='<%#Eval("TemplateId") %>' Text="Add" />
                                                        <asp:HiddenField ID="hdnIsMandatory" runat="server" Value= '<%#Eval("IsMandatory") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remove" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lnkRemoveFavourite" runat="server" SkinID="label" CommandName="Del" CommandArgument='<%#Eval("TemplateId") %>'
                                                                    Text="Remove" />--%>
                                                        <asp:ImageButton ID="lnkRemoveFavourite" runat="server" ImageUrl="~/Images/DeleteRow.png"
                                                            ToolTip="Remove" Width="16px" CommandName="Del" CommandArgument='<%#Eval("TemplateId") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDetails" runat="server" SkinID="label" CommandName="TMP" CommandArgument='<%#Eval("TemplateId") %>'
                                                            Text="Select" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
