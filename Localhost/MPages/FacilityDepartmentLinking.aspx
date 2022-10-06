<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="FacilityDepartmentLinking.aspx.cs" Inherits="EMR_Masters_FacilityDepartmentLinking"
    Title="Facility Wise Department" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />



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
            <div class="container-fluid header_main form-group">
                <div class="col-md-3 col-sm-4 PaddingRightSpacing">
                    <h2>Facility Tagging</h2>
                </div>
                <div class="col-md-7 col-sm-6 text-center">
                    <asp:Label ID="lblMessage" runat="server" Text="&nbsp;" ForeColor="Green" Font-Bold="true" />
                </div>
                <div class="col-md-2 col-sm-2 text-right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                    <asp:HiddenField ID="hdnisSaveChk" runat="server" Value="0" />
                </div>
            </div>

            <div class="container-fluid">

                <div class="row form-group">
                    <div class="col-md-6 col-sm-6">
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-6">Facility Tagging With</div>
                            <div class="col-md-8 col-sm-6 PaddingLeftSpacing">
                                <div class="row">
                                    <div class="col-md-10 col-sm-9 PaddingRightSpacing">
                                        <telerik:RadComboBox ID="ddlTaggedFor" runat="server" Width="100%"
                                            AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlTaggedFor_OnSelectedIndexChanged" />
                                    </div>
                                    <div class="col-md-2 col-sm-3">
                                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_OnClick" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row form-group" id="trItemSearch" visible="false" runat="server">
                            <div class="col-md-4 col-sm-7 form-group">
                                <asp:Label ID="lblSearchCategory" runat="server" Text="Group" /><asp:Label ID="lblItemgrpStar" runat="server" Text="*" ForeColor="Red" Visible="false" />
                            </div>
                            <div class="col-md-8 col-sm-5 form-group PaddingLeftSpacing">
                                <telerik:RadComboBox ID="ddlItemGroup" runat="server" Width="100%"
                                    AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true"
                                    OnSelectedIndexChanged="ddlItemGroup_OnSelectedIndexChanged" />
                            </div>
                            <div class="col-md-4 col-sm-7">
                                <asp:Label ID="lblSearchSubCategory" runat="server" Text="Sub-Group" /><asp:Label ID="lblItemSubgrpStar" runat="server" Text="*" ForeColor="Red" Visible="false" />
                            </div>
                            <div class="col-md-8 col-sm-5 PaddingLeftSpacing">
                                <telerik:RadComboBox ID="ddlItemSubGroup" runat="server" Width="100%"
                                    AutoPostBack="false" EmptyMessage="[ Select ]" MarkFirstMatch="true" />
                            </div>
                        </div>

                        <div class="row form-group">
                            <div class="col-md-4 col-sm-7">
                                <asp:Label ID="lblServiceName" runat="server" Text="Service Name"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-5 PaddingLeftSpacing">
                                <asp:TextBox ID="txtSeviceName" runat="server" Width="100%" MaxLength="200"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 col-sm-6">
                        <div class="row form-group">
                            <div class="col-md-4 col-sm-7">
                                <asp:Label ID="lblfacility" runat="server" Text="Facility To Tag"></asp:Label>
                            </div>
                            <div class="col-md-8 col-sm-5 PaddingLeftSpacing">
                                <telerik:RadComboBox ID="ddlFacility" runat="server" Width="100%" AutoPostBack="true" EmptyMessage="[ Select ]" MarkFirstMatch="true" OnSelectedIndexChanged="ddlFacility_OnSelectedIndexChanged" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row form-group">
                    <div class="col-md-6 col-sm-6">
                        <div class="row form-group header_main">
                            <div class="col-md-12 col-sm-12" style="height: 18px;">
                                <asp:Label ID="lblMainFacilityTag" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row form-group">
                            <asp:Panel ID="pnlDepart" runat="server" Height="400px" Width="100%" ScrollBars="Vertical">
                                <asp:GridView ID="gvDepartment" SkinID="gridviewOrderNew" runat="server" BorderWidth="0"
                                    AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="10px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" Checked="false" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10px" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="90%" ItemStyle-VerticalAlign="Top" HeaderText="Department Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblColumnName" runat="server" Text='<%# Eval("ColumnName") %>'></asp:Label>
                                                <asp:HiddenField ID="hdnColumnId" runat="server" Value='<%#Eval("ColumnId")%>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>

                        <div class="row form-group">
                            <div class="col-md-3 col-sm-3 margin_Top">
                                <div class="PD-TabRadioNew01 margin_z">
                                    <asp:CheckBox ID="chkUnSelect" runat="server" Text=" Select All" Checked="false" AutoPostBack="true" OnCheckedChanged="chkUnSelect_OnCheckedChanged" />
                                </div>
                            </div>
                            <div class="col-md-5 col-sm-5 text-center margin_Top">
                                <asp:Label ID="lblMainFacilityCount" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-md-4 col-sm-4 text-right margin_Top">
                                <asp:Button ID="btnSend4WO" runat="server" CssClass="btn btn-primary" Text="Select" OnClick="btnSend4WO_OnClick" />
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 col-sm-6">
                        <div class="row form-group header_main">
                            <div class="col-md-12 col-sm-12" style="height: 18px;">
                                <asp:Label ID="lblTaggingFacility" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div class="row form-group">
                            <asp:Panel ID="Panel1" runat="server" Height="400px" Width="100%" ScrollBars="Vertical">
                                <asp:GridView ID="grvFacilityWiseTag" SkinID="gridviewOrderNew" runat="server" BorderWidth="0"
                                    AutoGenerateColumns="False" Width="100%" AlternatingRowStyle-BackColor="Beige"
                                    ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="90%" ItemStyle-VerticalAlign="Top">
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
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClick="lnkDelete_OnClick"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </div>

                        <div class="row form-group">
                            <div class="col-md-6 col-sm-6 text-left">
                                <asp:Label ID="lblSelFacilityCount" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-6 col-sm-6 text-right">
                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
