<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="EMRReportSetup.aspx.cs" Inherits="EMR_Masters_EMRReportSetup" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
        <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

        <style type="text/css">
            #RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew.RadWindow.RadWindow_Default ul.rwControlButtons {
                right: auto !important;
                left: auto !important;
                width: 130px !important;
            }

            #RadWindowWrapper_ctl00_ContentPlaceHolder1_RadWindowForNew em {
              margin-left:24px!important;
            }
        </style>
        <script type="text/javascript" src="/Include/JS/Functions.js" language="javascript">
        </script>

        <script type="text/javascript">
            function LinkBtnMouseOver(lnk) {
                document.getElementById(lnk).style.color = "red";
            }
            function LinkBtnMouseOut(lnk) {
                document.getElementById(lnk).style.color = "blue";
            }

            function OnClientClose() {
            }
        </script>

        <div>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                        <Windows>
                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Pin, Move, Reload" />
                        </Windows>
                    </telerik:RadWindowManager>


                    <div class="container-fluid header_main" id="tdHeader" runat="server">
                        <div class="col-md-3">
                            <h2>
                                <asp:Label ID="lblHeader" runat="server" SkinID="label" ToolTip="EMR Report Setup"
                                    Text="EMR Report Setup" Font-Bold="true" /></h2>
                        </div>
                        <div class="col-md-5">
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="relativ alert_new text-center text-success" />
                        </div>
                        <div class="col-md-3 text-right pull-right">
                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                                CssClass="btn btn-primary" ValidationGroup="SaveData" Text="Save" />
                            <asp:Button ID="btnClose" runat="server" SkinID="button" Text="Close" Visible="false" CssClass="btn btn-primary"
                                OnClientClick="window.close();" />
                        </div>

                    </div>

                    <br />

                    <div class="col-md-5">
                        <div class="row">
                            <div class="col-md-12 radioo  form-group">
                                <div class="container">
                                    <asp:RadioButtonList ID="rdoTemplate" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                        OnSelectedIndexChanged="rdoTemplate_SelectedIndexChanged">
                                        <asp:ListItem Text="Template Sections" Value="D" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Template Fields" Value="F" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Static" Value="S"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>

                            </div>

                            <div class="col-md-12  form-group">
                                <div class="col-md-2">
                                    <asp:Label ID="label4" runat="server" SkinID="label" Text="Report" />&nbsp;<span
                                        style='color: Red'>*</span>
                                </div>
                                <div class="col-md-6">
                                    <telerik:RadComboBox ID="ddlReport" runat="server" Width="100%" EmptyMessage="[Select]"
                                        Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged" CssClass="margin_z" />
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnNew" runat="server" ToolTip="Add / Edit Report Format" CssClass="btn btn-primary btn-block"
                                        Text="Add / Edit Report Format" OnClick="btnNew_OnClick" />
                                </div>
                            </div>


                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>


                                    <div class="col-md-12  form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="label9" runat="server" SkinID="label" Text="Heading" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtHeadingName" runat="server" Text="" AutoPostBack="true" />

                                        </div>
                                        <div class="col-md-4">
                                            <asp:Button ID="btnAddHeading" runat="server" Text="Add" ToolTip="Add Heading Only"
                                                OnClick="btnAddHeading_Click" CssClass="btn btn-primary btn-block" />
                                        </div>
                                    </div>




                                    <div class="col-md-12  form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="label2" runat="server" SkinID="label" Text="Template" />
                                        </div>
                                        <div class="col-md-6">
                                            <telerik:RadComboBox ID="ddlTemplatename" runat="server" Width="100%" EmptyMessage="[ Select ]"
                                                Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_OnSelectedIndexChanged" />
                                        </div>
                                    </div>


                                    <div class="col-md-12  form-group">
                                        <div class="col-md-2">
                                            <asp:Label ID="label8" runat="server" SkinID="label" Text="Sections" Width="50px" />
                                        </div>
                                        <div class="col-md-6">

                                            <telerik:RadComboBox ID="ddlSections" runat="server" Width="100%" EmptyMessage="[ Select ]"
                                                Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlSections_OnSelectedIndexChanged" />
                                        </div>
                                    </div>


                                    <div class="col-md-12  form-group">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkShowTemplateName" runat="server" Text="Show Template Name In Note" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkShowSectionName" runat="server" Text="Show Section Name In Note" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkShowFieldName" runat="server" Text="Show Field Name In Note" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkAllSectionManditory" Visible="false" runat="server" Text="All Section Mandatory" />
                                        </div>



                                    </div>

                                    <div class="col-md-12">

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <telerik:RadGrid ID="RadgvPages" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="False"
                                                    AllowSorting="False" ShowGroupPanel="false" GridLines="none" OnItemCommand="RadgvPages_ItemCommand"
                                                    Width="100%" Skin="Office2007" Height="310px">
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="307px" />
                                                    </ClientSettings>
                                                    <MasterTableView Width="96%">
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Section Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSectionName" runat="server" Text='<%#Eval("SectionName")%>' />
                                                                    <asp:HiddenField ID="hdnSectionId" runat="server" Value='<%#Eval("SectionId")%>' />
                                                                    <asp:HiddenField ID="hdnParentId" runat="server" Value='<%#Eval("ParentId")%>' />
                                                                    <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type")%>' />
                                                                    <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                    <asp:HiddenField ID="hdnTemplateTypeName" runat="server" Value='<%#Eval("TemplateTypeName")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="Select" ItemStyle-ForeColor="Blue" CommandName="AddToList"
                                                                ItemStyle-Width="35px" HeaderText="">
                                                            </telerik:GridButtonColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                                <telerik:RadGrid ID="radgvFields" runat="server" AutoGenerateColumns="false" AllowMultiRowSelection="False"
                                                    AllowSorting="False" ShowGroupPanel="false" GridLines="none" Width="100%" Skin="Office2007"
                                                    OnItemCommand="radgvFields_ItemCommand" Height="350px">
                                                    <ClientSettings>
                                                        <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="false" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="307px" />
                                                    </ClientSettings>
                                                    <MasterTableView Width="100%">
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Field Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName")%>' />
                                                                    <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId")%>' />
                                                                    <asp:HiddenField ID="hdnSectionId" runat="server" Value='<%#Eval("SectionId")%>' />
                                                                    <asp:HiddenField ID="hdnSectionName" runat="server" Value='<%#Eval("SectionName")%>' />
                                                                    <asp:HiddenField ID="hdnType" runat="server" Value='<%#Eval("Type")%>' />
                                                                    <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TemplateName")%>' />
                                                                    <asp:HiddenField ID="hdnTemplateTypeName" runat="server" Value='<%#Eval("TemplateTypeName")%>' />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridButtonColumn Text="Select" ItemStyle-ForeColor="Blue" CommandName="AddToList"
                                                                ItemStyle-Width="35px" HeaderText="">
                                                            </telerik:GridButtonColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </div>


















                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTemplatename" />
                                </Triggers>
                            </asp:UpdatePanel>





                        </div>









                    </div>


                    <div class="col-md-7">

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <telerik:RadGrid ID="gvTagged" runat="server" Skin="Office2007" Width="100%" PagerStyle-ShowPagerText="false"
                                    AllowSorting="False" AllowMultiRowSelection="False" EnableLinqExpressions="false"
                                    ShowGroupPanel="false" AutoGenerateColumns="False" GroupHeaderItemStyle-Font-Bold="true"
                                    GridLines="none" OnItemCommand="gvTagged_ItemCommand" Height="490px">
                                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                        <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                            AllowColumnResize="false" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="307px" />
                                    </ClientSettings>
                                    <MasterTableView Width="100%">
                                        <NoRecordsTemplate>
                                            <div style="font-weight: bold; color: Red;">
                                                No Record Found.
                                            </div>
                                        </NoRecordsTemplate>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="DetailId" HeaderStyle-Width="0%" Visible="false"
                                                ItemStyle-Width="0%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDetailId" runat="server" Text='<%#Eval("DetailId") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="SNo." HeaderStyle-Width="0%" Visible="false"
                                                ItemStyle-Width="0%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSno" runat="server" Text='<%#Eval("ID") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Heading Name" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                   <%-- <asp:Label ID="lblHeadingName" runat="server" Text='<%#Eval("HeadingName") %>' />--%>
                                                     <asp:LinkButton ID="lblHeadingName" runat="server" Text='<%#Eval("HeadingName") %>' OnClick="lblHeadingName_OnClick" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Template Name" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Section Name" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSectionName" runat="server" Text='<%#Eval("SectionName")%>' />
                                                    <asp:HiddenField ID="hdnSectionId" runat="server" Value='<%#Eval("SectionId")%>' />
                                                    <asp:HiddenField ID="hdnSeqNo" runat="server" Value='<%#Eval("SequenceNo") %>' />
                                                    <asp:HiddenField ID="HdnType" runat="server" Value='<%#Eval("Type") %>' />
                                                    <asp:HiddenField ID="hdnShowTemplateNameInNote" runat="server" Value='<%#Eval("ShowTemplateNameInNote") %>' />
                                                    <asp:HiddenField ID="hdnShowSectionNameInNote" runat="server" Value='<%#Eval("ShowSectionNameInNote") %>' />
                                                    <asp:HiddenField ID="hdnShowFieldNameInNote" runat="server" Value='<%#Eval("ShowFieldNameInNote") %>' />
                                                    <%--<asp:HiddenField ID="hdnIsCheckListRequired" runat="server" Value='<%#Eval("IsCheckListRequired") %>' />--%>
                                                    <asp:HiddenField ID="hdnAllSectionManditory" runat="server" Value='<%#Eval("AllSectionManditory") %>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Field Name" HeaderStyle-Width="12%" ItemStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#Eval("FieldName")%>' />
                                                    <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTemplateTypeName" runat="server" Text='<%#Eval("TemplateTypeName")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Up" HeaderTooltip="Up" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="4%" ItemStyle-Width="4%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnMoveUP" runat="server" Text="UP" ToolTip="Move Up" CausesValidation="false"
                                                        CommandName="MoveUP" ImageUrl="/images/group_arrow_bottom.png" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Down" HeaderTooltip="Down" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnMoveDown" runat="server" Text="Down" ToolTip="Move Down"
                                                        CausesValidation="false" CommandName="MoveDown" ImageUrl="/images/group_arrow_top.png" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_OnClick"
                                                        CommandArgument='<%#Eval("DetailId")%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Delete" HeaderTooltip="Delete a row" HeaderStyle-Width="6%"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="false" CommandName="Delete1"
                                                        CommandArgument='<%#Eval("ID")%>' ImageUrl="/images/DeleteRow.png" ToolTip="Click here to delete a row" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>


                    <asp:HiddenField ID="hdnMainReportId" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
