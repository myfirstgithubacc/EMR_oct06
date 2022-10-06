<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="FacilityDepartmentLinking.aspx.cs" Inherits="EMR_Masters_FacilityDepartmentLinking"
    Title="Facility Wise Department" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
     <style>
        .RadComboBox .rcbArrowCell a{
            height:22px!important;
        }
         </style>
    <script language="javascript" type="text/javascript">
        function checkCategoryName() {
            var txtCategoryName = document.getElementById('?ctl00_ContentPlaceHolder1_txtCategoryName');
            if (txtCategoryName.value == '') {
                alert('Please Enter The Category')
                return false;
            }
        }

    </script>

    <asp:UpdatePanel ID="Update1" runat="server">
        <ContentTemplate>

            <div class="container-fluid header_main">
                <div class="col-md-2">
                    <h2>Facility Tagging</h2>
                </div>

                <div class="col-md-6 text-center ">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" CssClass="alert_new relativ text-success" />
                </div>

                <div class="col-md-4 pull-right text-right">

                    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                    <asp:Button ID="btnTemplate" runat="server" Text="Create New Template" OnClick="NewTemplate_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                    <asp:HiddenField ID="hdnisSaveChk" runat="server" Value="0" />
                </div>
            </div>

            <div class="container-fluid subheading_main">
                <div class="row">
                    <div class="col-md-3 form-group">
                        <div class="col-md-4">
                            <asp:Label ID="Label1" runat="server" Text="Specialisation " CssClass="label1"></asp:Label>

                        </div>
                        <div class="col-md-8">
                            <telerik:RadComboBox ID="ddlSpecialization" SkinID="DropDown" runat="server" Skin="Office2010Black" Width="100%"
                                DropDownWidth="250px" AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                                OnSelectedIndexChanged="ddlSpecialization_OnSelectedIndexChanged" />
                        </div>
                    </div>
                    <div class="col-md-3 form-group">

                        <telerik:RadComboBox ID="ddlTaggedFor" SkinID="DropDown" runat="server" Width=""
                            DropDownWidth="250px" AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                            OnSelectedIndexChanged="ddlTaggedFor_OnSelectedIndexChanged" />

                        <asp:Button ID="btnFilter" runat="server" Text="Filter" Width="100px" OnClick="btnFilter_OnClick" CssClass="btn btn-primary" />
                    </div>


                    <div class="col-md-3 form-group">
                        <div class="col-md-6">
                            <asp:Label ID="Label2" runat="server" Text="Specialisation " CssClass="label1"></asp:Label>

                        </div>
                        <div class="col-md-6">
                            <telerik:RadComboBox ID="ddlSpecialization1" SkinID="DropDown" runat="server" Width=""
                                DropDownWidth="250px" AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlSpecialization1_OnSelectedIndexChanged" />
                        </div>
                    </div>
                    <div class="col-md-3 form-group">
                        <div class="col-md-6">
                            <asp:Label ID="lblServiceName" runat="server" Text="Service Name" CssClass="label1"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtSeviceName" runat="server" SkinID="textbox"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-md-3 form-group">
                        <div class="col-md-6">
                            <asp:Label ID="lblfacility" runat="server" Text="Facility To Tag " CssClass="label1"></asp:Label>
                        </div>
                        <div class="col-md-6">
                            <telerik:RadComboBox ID="ddlFacility" SkinID="DropDown" runat="server" Width="100%"
                                AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlFacility_OnSelectedIndexChanged" />
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div id="trItemSearch" visible="false" runat="server" align="left">
                        <div class="col-md-5"></div>
                        <div class="col-md-3">
                            <div class="col-md-6">
                                <asp:Label ID="lblSearchCategory" runat="server" Text="Group"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <telerik:RadComboBox ID="ddlItemGroup" SkinID="DropDown" runat="server" Width="100%"
                                    DropDownWidth="250px" AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                                    OnSelectedIndexChanged="ddlItemGroup_OnSelectedIndexChanged" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="col-md-6">
                                <asp:Label ID="lblSearchSubCategory" runat="server" Text="Sub-Group"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <telerik:RadComboBox ID="ddlItemSubGroup" SkinID="DropDown" runat="server" Width="100%" DropDownWidth="250px"
                                    AutoPostBack="false" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                            </div>
                        </div>


                    </div>

                </div>
            </div>

            <div class="container-fluid">
                <div class="row">

                    <div class="col-md-6">
                        <strong>
                            <asp:Label ID="lblMainFacilityTag" runat="server"></asp:Label></strong>
                        <asp:Panel ID="pnlDepart" runat="server" Height="430px" Width="100%" ScrollBars="Vertical">
                            <asp:GridView ID="gvDepartment" SkinID="gridview2" runat="server" BorderWidth="0"
                                AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                OnRowDataBound="gvDepartment_RowDataBound" OnRowCommand="gvDepartment_RowCommand">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="10px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked="false" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40%" ItemStyle-VerticalAlign="Top" HeaderText="Specialization Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblspecialization" runat="server" Text='<%# Eval("SpecialityName") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnspecializationId" runat="server" Value='<%#Eval("SpecialityId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40%" ItemStyle-VerticalAlign="Top" HeaderText="Department Name">
                                        <ItemTemplate>

                                            <asp:LinkButton ID="lnkTemplateName" runat="server" Text='<%#Eval("ColumnName")%>'
                                                CommandName="TempLink" CommandArgument='<%#Eval("ColumnId")%>' ToolTip="Click to set Sections and Fields of this Template" />
                                            <asp:HiddenField ID="hdnColumnId" runat="server" Value='<%#Eval("ColumnId")%>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnTempPreview" runat="server" Text="Preview" CommandName="Preview" data-toggle="tooltip" title="View Detail!"
                                                CssClass="btn btn-primary" CommandArgument='<%#Eval("ColumnId")%>' ><i class="fa fa-search" ></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindow1" runat="server" Behaviors="Close,Move">
                                    </telerik:RadWindow>
                                </Windows>
                            </telerik:RadWindowManager>
                        </asp:Panel>
                    </div>



                    <div class="col-md-6">

                        <strong>
                            <asp:Label ID="lblTaggingFacility" runat="server"></asp:Label></strong>

                        <asp:Panel ID="Panel1" runat="server" Height="430px" Width="100%" ScrollBars="Vertical">
                            <asp:GridView ID="grvFacilityWiseTag" SkinID="gridview2" runat="server" BorderWidth="0"
                                AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                ShowHeader="true" OnRowCommand="grvFacilityWiseTag_RowCommand">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="40%" ItemStyle-VerticalAlign="Top" HeaderText="Specialization Name">
                                        <ItemTemplate>
                                        <asp:Label ID="lblspecilizationName" runat="server" Text='<%# Eval("SpecialityName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40%" ItemStyle-VerticalAlign="Top" HeaderText="Department Name">
                                        <ItemTemplate>
                                           
                                            <asp:Label ID="lblSelColumnName" runat="server" Text='<%# Eval("ColumnName") %>'></asp:Label>
                                            <asp:HiddenField ID="hdnID" runat="server" Value='<%#Eval("Id")%>' />
                                            <asp:HiddenField ID="hdnSelColumnId" runat="server" Value='<%#Eval("ColumnId")%>' />
                                            <asp:HiddenField ID="hdnSelFacilityID" runat="server" Value='<%#Eval("FacilityID") %>' />
                                            <asp:HiddenField ID="hdnSelFacilityName" runat="server" Value='<%#Eval("FacilityName") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                                        <%--<HeaderTemplate><i class="fa fa-trash" ></i></HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="" OnClick="lnkDelete_OnClick" CssClass="btn btn-danger" data-toggle="tooltip" title="Delete Record!"><i class="fa fa-trash" ></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View" HeaderStyle-Width="80px">
                                        <%-- <HeaderTemplate><i class="fa fa-search" ></i></HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnTempPreview" runat="server" Text="" CommandName="Preview" data-toggle="tooltip" title="View Detail!"
                                                CssClass="btn btn-primary" CommandArgument='<%#Eval("ColumnId")%>' ><i class="fa fa-search" ></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>



                    </div>
                </div>
            </div>







            <div class="spac" style="padding: 0px">
                <div class="col-md-1">
                    <asp:CheckBox ID="chkUnSelect" runat="server" Text=" Select All" Checked="false" AutoPostBack="true"
                        OnCheckedChanged="chkUnSelect_OnCheckedChanged" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="lblMainFacilityCount" runat="server" CssClass="label1"></asp:Label>


                    <asp:Button ID="btnSend4WO" runat="server" CssClass="btn btn-primary" Text="Select" OnClick="btnSend4WO_OnClick" />

                </div>

                <div class="col-md-2 pull-right text-right">

                    <asp:Label ID="lblSelFacilityCount" runat="server" CssClass="label1"></asp:Label>

                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnClear_Click" />

                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnFilter" />

<asp:PostBackTrigger ControlID="btnSend4WO"></asp:PostBackTrigger>

        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSend4WO" />
        </Triggers>
    </asp:UpdatePanel>

<script type="text/javascript">
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();   
});
</script>
</asp:Content>
