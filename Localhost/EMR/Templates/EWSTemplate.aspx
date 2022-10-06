<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="EWSTemplate.aspx.cs" Inherits="EMR_Templates_EWSTemplate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />

    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <style>
                #ctl00_ContentPlaceHolder1_Label2 {
                    margin: 0px !important;
                }
            </style>

            <div class="container-fluid header_main">
                <div class="col-md-3 col-sm-4">
                    <h2>
                        <asp:Label ID="lblTitle" runat="server" Text="Early Warning Score" /></h2>
                </div>
                <div class="col-md-5 col-sm-8">
                    <asp:Label ID="lblMessage" runat="server" />
                </div>

                <div class="col-md-4  col-sm-12 text-right">
                    <asp:Button ID="btnScoreCardDetail" runat="server" Text="Score History" ValidationGroup="Save"
                        OnClick="btnScoreCardDetail_Click" CssClass="btn btn-primary" Font-Bold="true" />

                    <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record"
                        Text="New" OnClick="btnNew_OnClick" CssClass="btn btn-primary" />
                    <asp:Button ID="btnsave" runat="server" Text="Save" ValidationGroup="Save" ToolTip="Save (F3)"
                        OnClick="btnsave_Click" CssClass="btn btn-primary" Font-Bold="true" />
                </div>
            </div>
            <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
            <div class="container-fluid">
                <div class="col-md-12 subheading_main">
                    <div class="row">
                        <div class="col-6">
                            <div class="row">
                                <div class="col-lg-2 col-sm-3">
                                    <asp:Label ID="lblTemplate2" runat="server" Text="Template" />
                                </div>
                                <div class="col-sm-9">
                                    <telerik:RadComboBox ID="ddlTemplateMain" runat="server" EmptyMessage="[ Select ]"
                                        Width="100%" Height="400px" DropDownWidth="350px" Filter="Contains"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateMain_SelectedIndexChanged" />
                                </div>
                            </div>
                        </div>


                        <div class="col-6">
                            <div class=" row PatientHistoryText01">
                                <div class="col-lg-2 col-sm-3">
                                    <h2>
                                        <asp:Label ID="Label3" runat="server" Text="Session" />
                                    </h2>
                                </div>
                                <div class="col-sm-9">

                                    <telerik:RadComboBox ID="ddlRecord" SkinID="DropDown" runat="server" Width="100%"
                                        DropDownWidth="185px" EmptyMessage="[ Select ]" AutoPostBack="true" OnSelectedIndexChanged="ddlRecord_OnSelectedIndexChanged" />

                                    <asp:Button ID="btnNewRecord" runat="server" Style="margin-top: 3px;" CssClass="PatientBtn03a" OnClick="btnNewRecord_OnClick"
                                        Text="Add" ToolTip="New Record" />
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-5">
                        <asp:GridView ID="gvScoreCard" runat="server" AutoGenerateColumns="False" ForeColor="#333333"
                            OnRowDataBound="gvScoreCard_RowDataBound" SkinID="gridview" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="20px">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Field Name" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFieldName" runat="server" SkinID="lebel" Text='<%#Eval("FieldName") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnFieldId" runat="server" Value='<%#Eval("FieldId") %>' />
                                        <asp:HiddenField ID="hdnFieldType" runat="server" Value='<%#Eval("FieldType") %>' />
                                        <asp:HiddenField ID="hdnSectionId" runat="server" Value='<%#Eval("SectionId") %>' />
                                        <asp:HiddenField ID="hdnFieldCode" runat="server" Value='<%#Eval("FieldCode") %>' />
                                        <asp:HiddenField ID="hdnIsMandatory" runat="server" Value='<%#Eval("IsMandatory") %>' />
                                        <span id="spnId" runat="server" style="color: Red">*</span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Field Value" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFieldValue" runat="server" SkinID="textbox" Text="" Width="80px"></asp:TextBox>
                                        <asp:TextBox ID="txtM" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                            TextMode="MultiLine" Style="width: 250px;"
                                            Visible="false" />
                                        <asp:Button ID="btnScoreCalculate" runat="server" Text="Click to Calculate" OnClick="btnScoreCalculate_OnClick"
                                            SkinID="Button" />
                                        <asp:DropDownList ID="ddlDropDown" runat="server" SkinID="DropDown" Width="150px"
                                            AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Value" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastValue" runat="server" SkinID="lebel" Text=""></asp:Label>
                                        &nbsp;&nbsp;<asp:ImageButton ID="ibtnEWSGraph" runat="server" ToolTip="Click me to display the graph" Visible="false" Height="15px" ImageUrl="~/Images/Growth-Chart.png" OnClick="ibtnEWSGraph_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-md-7 mt-2 m-md-0">
                        <div style="margin: 0px 20px 10px 0px; display: inline-block; float: left">
                            <asp:Button ID="btnCopyLastEWS" runat="server" Text="Copy Last EWS" ValidationGroup="Save" ToolTip="Copy Last EWS"
                                OnClick="btnCopyLastEWS_Click" CssClass="btn btn-primary pull-left" Font-Bold="true" />
                        </div>
                        <div>
                            <div style="float: left">
                                <asp:Label ID="Label1" runat="server" Text="Last EWS Time : " Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblEntryDateTime" runat="server"></asp:Label>
                            </div>
                            <div style="float: right">
                                <asp:Label ID="Label2" runat="server" Text="User : " Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblLastEnteredBy" runat="server"></asp:Label>
                            </div>
                        </div>
                        <asp:Panel ID="pnlimg" runat="server" Height="600px" Width="100%" ScrollBars="Auto">
                            <iframe id="ifrmpdf" runat="server" width="100%" height="98%" frameborder="1"></iframe>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <telerik:RadWindowManager ID="RadWindowManager1" Skin="Office2007" EnableViewState="false" runat="server">
                <Windows>
                    <telerik:RadWindow ID="RadWindow1" runat="server" Skin="Office2007" Behaviors="Close" />
                </Windows>
            </telerik:RadWindowManager>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

