<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="EMRTemplatesSetup.aspx.cs" Inherits="EMR_Masters_EMRTemplatesSetup" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function SelectAll(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvTemplates.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function SelectAllManditory(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvTemplates.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[2];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function SelectAllCollapse(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvTemplates.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[3];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

        function SelectAllFreeTextTemplates(id) {
            //get reference of GridView control
            var grid = document.getElementById("<%=gvFreeTextTemplates.ClientID%>");
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (ridx = 1; ridx < grid.rows.length; ridx++) {
                    //get the reference of first column
                    cell = grid.rows[ridx].cells[0];

                    //loop according to the number of childNodes in the cell
                    for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                        //if childNode type is CheckBox
                        if (cell.childNodes[cIdx].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }

    </script>

    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main" id="tdHeader" runat="server">
                <div class="col-md-3">
                    <h2>
                        <asp:Label ID="lblHeader" runat="server" Text="EMR Templates Setup" />
                    </h2>
                </div>
                <div class="col-md-5 text-center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSaveData" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="text-center text-success alert_new relativ" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <div class="col-md-3 text-right pull-right">
                    <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" CssClass="btn btn-primary"
                        Text="New" OnClick="btnNew_OnClick" />
                    &nbsp;
                        <asp:Button ID="btnSaveData" runat="server" ToolTip="Save&nbsp;Data" OnClick="btnSaveData_OnClick"
                            CssClass="btn btn-primary" Text="Save" />

                </div>

            </div>
            <!-- end of  header container -->


            <div class="container-fluid form-group subheading_main">
                <div class="col-md-1">
                    <asp:Label ID="Label12" runat="server" SkinID="label" Text="Facility" CssClass="Label1" />
                    <span style='color: Red'>*</span>
                </div>
                <div class="col-md-3">

                    <asp:DropDownList ID="ddlFacility" SkinID="DropDown" runat="server" Width="100%" />
                </div>

                <div class="col-md-1" style="padding: 0">
                    <asp:Label ID="Label2" runat="server" SkinID="label" Text="Specialisation" />
                    <span style='color: Red'>*</span>
                </div>
                <div class="col-md-3">
                    <telerik:RadComboBox ID="ddlSpecialisation" runat="server" Skin="Metro" AppendDataBoundItems="true"
                        Height="300px" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSpecialisation_OnSelectedIndexChanged" />
                    <%--    <asp:DropDownList ID="ddlSpecialisation" SkinID="DropDown" runat="server" Width="300px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSpecialisation_OnSelectedIndexChanged" />--%>
                </div>

                <div class="col-md-1">
                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="Provider" />
                    <%--<span style='color: Red'>*</span>--%>
                </div>
                <div class="col-md-3">
                    <telerik:RadComboBox ID="ddlProvider" runat="server" Skin="Metro" Height="300px" Width="100%"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProvider_OnSelectedIndexChanged" />
                </div>

                 <div class="col-md-1">
                    <asp:Label ID="Label5" runat="server" SkinID="label" Text="Doctor Panel Setting" />
                    <%--<span style='color: Red'>*</span>--%>
                </div>
                <div class="col-md-3">
                    <telerik:RadComboBox ID="ddlDPSetting" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Skin="Metro" Height="300px" Width="100%"
                         />

                    
                </div>
            </div

            <div class="row" runat="server">
                <div class="col-md-6">
                    <div class="row" runat="server">
                        <div class="col-md-12">
                            <asp:Label ID="Label4" runat="server" SkinID="label" Text="All Templates Lists" />
                        </div>
                    </div>
                    <div class="row" runat="server">
                        <div class="col-md-12">
                            <asp:GridView ID="gvTemplates" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                AllowPaging="false" OnRowDataBound="gvTemplates_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="100px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAll" Text="Template" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Template Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName") %>' />
                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("Id") %>' />
                                            <asp:HiddenField ID="hdnIsChk" runat="server" Value='<%#Eval("IsChk") %>' />
                                            <asp:HiddenField ID="hdnIsMandatory" runat="server" Value='<%#Eval("IsMandatory") %>' />
                                            <asp:HiddenField ID="hdnIsCollapse" runat="server" Value='<%#Eval("IsCollapse") %>' />
                                            <asp:HiddenField ID="hdnTemplateCode" runat="server" Value='<%#Eval("TemplateCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllManditory" Text="Mandatory" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRowManditory" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="100px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllCollapse" Text="Collapse" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRowCollapse" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="row" runat="server">
                        <div class="col-md-3">
                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="Template Lists" />
                        </div>
                        <div class="col-md-9">
                            <telerik:RadComboBox ID="ddlFreeTextUserTemplate" runat="server" Skin="Metro" AppendDataBoundItems="true"
                                Height="300px" Width="80%" AutoPostBack="true" OnSelectedIndexChanged="ddlFreeTextUserTemplate_OnSelectedIndexChanged" EmptyMessage="--Select--" />
                        </div>
                    </div>
                    <div class="row" runat="server">
                        <div class="col-md-12">
                            <asp:GridView ID="gvFreeTextTemplates" runat="server" SkinID="gridview" Width="100%" AutoGenerateColumns="false"
                                AllowPaging="false" OnRowDataBound="gvFreeTextTemplates_OnRowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="100px">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllFreeTextTemplates" Text="Template" runat="server" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRowFreeTextTemplates" runat="server" Checked='<%#Eval("IsChk").ToString().Equals("1")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--  <asp:TemplateField HeaderText="Template Type">
                            <ItemTemplate>
                                <asp:Label ID="lblTemplateType" runat="server" SkinID="label" Text='<%#Eval("TemplateType") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Template Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTemplateName" runat="server" SkinID="label" Text='<%#Eval("TemplateName") %>' />
                                            <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%#Eval("TemplateId") %>' />
                                            <asp:HiddenField ID="hdnIsChk" runat="server" Value='<%#Eval("IsChk") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
