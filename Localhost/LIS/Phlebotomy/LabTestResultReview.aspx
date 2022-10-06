<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Include/Master/BlankMaster.master"
    CodeFile="LabTestResultReview.aspx.cs" Inherits="LIS_Phlebotomy_LabTestResultReview" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.css" rel="stylesheet" type="text/css" />

    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>





                <div class="container-fluid header_main form-group">
                    <div class="col-xs-3">
                        <h2>
                            <asp:Label ID="lblHeader" runat="server" SkinID="label" Text="Lab Test Review" Font-Bold="true" /></h2>
                    </div>

                    <div class="col-xs-9 text-right">
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click" Text="Save" />
                        <asp:Button ID="btnClear" runat="server" CssClass="btn btn-primary" OnClick="btnClear_Click"
                            Text="Clear" />
                        <asp:Button ID="btnclose" runat="server" CssClass="btn btn-primary" Text="Close" OnClientClick="window.close();" />

                    </div>
                </div>





                <div class="container-fluid">
                    <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                </div>

                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-xs-3">
                            <asp:Label ID="Label1" runat="server" SkinID="label" Text="Staus" />
                        </div>
                        <div class="col-xs-9">
                            <asp:RadioButtonList ID="rblReviewedStatus" runat="server" RepeatDirection="Horizontal" CssClass="radioo pull-left"
                                OnSelectedIndexChanged="rblReviewedStatus_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Normal Reviewed" Value="1" Selected="True" />
                                <asp:ListItem Text="Critical Reviewed" Value="2" Selected="True" />
                                <asp:ListItem Text="Not Reviewed" Value="0" />
                            </asp:RadioButtonList>
                            <asp:CheckBox ID="chkNoSMS" runat="server" Text="No SMS" Visible="false" ToolTip="By Checking this, SMS will not sent to patient"
                                Checked="false" />

                        </div>

                    </div>

                    <div class="row form-group">
                        <div class="col-xs-3">
                            <asp:Label ID="Label2" runat="server" SkinID="label" Text="Date&nbsp;Time" /></div>
                        <div class="col-xs-9">
                            <asp:UpdatePanel ID="Updatepanel1" runat="server">
                                <ContentTemplate>
                                    <telerik:RadDateTimePicker ID="dtpReviewedDate" runat="server" AutoPostBackControl="Both"
                                        TabIndex="37" Width="170px" DateInput-DateFormat="MM/dd/yyyy hh:MM" Enabled="false" />
                                    <telerik:RadComboBox ID="ddlMinute" runat="server" Width="50px" OnSelectedIndexChanged="ddlMinute_OnSelectedIndexChanged"
                                        Skin="Outlook" AutoPostBack="True" Enabled="false" />
                                    &nbsp;<asp:Label ID="lblDisplay" runat="server" SkinID="label" Text="HH   MM" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-xs-3">
                            <asp:Label ID="Label3" runat="server" SkinID="label" Visible="false" Text="Lab&nbsp;Flag&nbsp;Value" /></div>
                        <div class="col-xs-9"></div>
                    </div>

                    <div class="row form-group">
                        <div class="col-xs-3"></div>
                        <div class="col-xs-9">
                            <asp:TextBox ID="txtLabFlagValue" Visible="false" Width="20px" runat="server" MaxLength="2" />

                            <asp:Label ID="Label4" Visible="false" runat="server" SkinID="label" Text="Test&nbsp;Result&nbsp;Status" />

                            <asp:TextBox ID="txtTestResultStatus" Visible="false" Width="20px" runat="server"
                                MaxLength="2" />
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-xs-3">
                            <asp:Label ID="Label5" runat="server" SkinID="label" Text="Comments" /></div>
                        <div class="col-xs-9">
                            <asp:TextBox ID="txtReviewedComments" TextMode="MultiLine" Rows="4" Width="360px"
                                runat="server" MaxLength="100" />
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-xs-3">
                            <asp:Label ID="Label6" runat="server" SkinID="label" Text="Reviewed&nbsp;By" /></div>
                        <div class="col-xs-9">
                            <telerik:RadComboBox ID="ddlReviewedBy" SkinID="DropDown" runat="server" EmptyMessage="[ Select ]"
                                Width="250px" />
                        </div>
                    </div>
                </div>


                <div id="dvReviewAlert" runat="server" style="height: 100px; width: 100%; top: 10%; background-color: Gray; font-size: large; z-index: 10000!important; color: Blue; position: absolute;"
                    visible="false">
                    <center>
                        <b>Result will be reviewed with Critical Flag, Are you Sure to Review ?</b><br />
                        <asp:Button ID="btnYes" Text="Yes! This is Critical" runat="server" OnClick="btnYes_Click"
                            Width="150px" CssClass="buttonBlue" />
                        <asp:Button ID="btnNo" Text="No!" runat="server" OnClick="btnNo_Click" Width="100px"
                            CssClass="buttonBlue" />
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
