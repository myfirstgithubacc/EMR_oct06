<%@ Page Title="Extension" Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" AutoEventWireup="true" CodeFile="Extensions.aspx.cs" Inherits="Newpreauth" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/bootstrap.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="Stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="Stylesheet" type="text/css" />

     <asp:UpdatePanel ID="updt" runat="server">
        <ContentTemplate>
    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-3 col-sm-3 col-xs-3">
                <h2>
                    Extension Requests
                </h2>
            </div>
            <div class="col-md-5 col-sm-5 col-xs-4">
                <asp:Label ID="lblerror" runat="server"></asp:Label>
            
            <asp:HiddenField ID="hdnencounter" runat="server" />
            <asp:HiddenField ID="hdnregid" runat="server" />
            <asp:HiddenField ID="hdnextensionid" runat="server" />
            </div>
            <div class="col-md-4 col-sm-4 col-xs-5 text-right">
                <asp:Button ID="btnsubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnsubmit_Click" />
            <asp:Button ID="btnHistory" runat="server" Text="History" CssClass="btn btn-primary" OnClick="btnHistory_Click" />
            </div>
        </div>

        <div class="row text-center">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>

        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="lblUHID" runat="server" Text="UHID"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:Panel ID="pnlenter" runat="server" DefaultButton="lnkfind">
                        <asp:LinkButton ID="lnkfind" runat="server" OnClick="txtUHID_TextChanged"></asp:LinkButton>
                        <asp:TextBox ID="txtUHID" runat="server" placeholder="UHID" ToolTip="Please Enter UHID" Width="100%" CssClass="Textbox" ></asp:TextBox>
                    </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label9" runat="server" Text="Approved Until"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:TextBox ID="txtapproveddays" runat="server" placeholder="Approved until" ToolTip="Approval End Date" Width="100%" CssClass="Textbox" AutoPostBack="true" OnTextChanged="txtEmirateID_TextChanged" data-inputmask="'mask':999-9999-9999999-9"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="lblMobileNo" runat="server" Text="MobileNo"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:TextBox ID="txtMobileNo" runat="server" placeholder="MobileNo" CssClass="Textbox" ToolTip="Please Enter Patient MobileNo" Width="100%" AutoPostBack="true" OnTextChanged="txtMobileNo_TextChanged"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="lblname" runat="server" Text="Patient Name"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:TextBox ID="txtName" runat="server" placeholder="Patient Name" CssClass="Textbox" ToolTip="Please Enter Patient Name" Width="100%" ></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="lblapprovalfor" runat="server" Text="Extension For"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <div class="row">
                            <div class="col-md-7 col-sm-7 col-xs-7">
                                <asp:TextBox ID="txtDays" runat="server" CssClass="Textbox" TextMode="Number" min="1" max="7" MaxLength="2" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-md-5 col-sm-5 col-xs-5 no-p-l">
                                <asp:DropDownList ID="ddlextfor" runat="server" Width="100%">
                        <asp:ListItem Text=" Day(s)" Value="1"></asp:ListItem>
                        <asp:ListItem Text=" Week(s)" Value="2"></asp:ListItem>
                        <asp:ListItem Text=" Month(s)" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label6" runat="server" Text="Services"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <div class="row">
                            <div class="col-md-9 col-sm-9 col-xs-9">
                        <telerik:RadComboBox ID="RadServices" runat="server" Filter="Contains" Width="100%" OnItemsRequested="RadServices_ItemsRequested" EnableLoadOnDemand="true"></telerik:RadComboBox>
                                 </div>
                            <div class="col-md-3 col-sm-3 col-xs-3 no-p-l">
                    <asp:Button ID="btnAddServ" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddServ_Click" />
                                </div>
                            </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label8" runat="server" Text="Date of Admission"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:TextBox ID="txtEdd" ReadOnly="false" CssClass="Textbox" runat="server" placeholder="Expected Date of Admission" ToolTip="Expected Date of Admission" Width="100%" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:Label ID="Label10" runat="server" Text="IP No"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:TextBox ID="txtIPno" runat="server" CssClass="Textbox" placeholder="Encounter No" ToolTip="Please Enter Emirate ID" Width="100%" AutoPostBack="true" OnTextChanged="txtEmirateID_TextChanged" data-inputmask="'mask':999-9999-9999999-9"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12" id="dvy" runat="server" visible="false">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="Label7" runat="server" Text="Diagnosis"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       <div class="row">
                           <div class="col-md-8 col-sm-8 col-xs-8">
                           <telerik:RadComboBox ID="RadDiagnosis" CssClass="Textbox" runat="server" Filter="Contains" OnItemsRequested="RadDiagnosis_ItemsRequested" AutoPostBack="true" EnableLoadOnDemand="True"></telerik:RadComboBox>
                               </div>
                           <div class="col-md-4 col-sm-4 col-xs-4">
                    <asp:Button ID="btnAddtoList" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAddtoList_Click" />
                               </div>
                       </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12" runat="server" visible="false">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="lblEmirateID" runat="server" Text="Emirates ID"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       <asp:TextBox ID="txtEmirateID" runat="server" CssClass="Textbox" placeholder="Emirate ID" ToolTip="Please Enter Emirate ID" AutoPostBack="true" OnTextChanged="txtEmirateID_TextChanged" data-inputmask="'mask':999-9999-9999999-9"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12" id="dvx" runat="server" visible="false">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="Label1" runat="server" Text="Card No "></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       <asp:TextBox ID="txtcardNo" runat="server" CssClass="Textbox" placeholder="Card No" ToolTip="Please Enter Card No" Width="100%" ></asp:TextBox>
                    </div>
                </div>
            </div>
            
        </div>
        <div class="row">
            <asp:Panel ID="pnl" runat="server" Visible="false">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="Label2" runat="server" Text="Payer"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       <asp:DropDownList ID="ddlpayer" runat="server" CssClass="Textbox" placeholder="Payer" ToolTip="Please Select Payer" AutoPostBack="true" OnSelectedIndexChanged="ddlpayer_SelectedIndexChanged" ></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="Label3" runat="server" Text="Sponsor "></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <asp:DropDownList ID="ddlsponsor" runat="server" CssClass="Textbox" placeholder="Sponsor" ToolTip="Please Select Sponsor" AutoPostBack="true" OnSelectedIndexChanged="ddlsponsor_SelectedIndexChanged" ></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       <asp:Label ID="Label4" runat="server" Text="Card/Network"></asp:Label>
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       <asp:DropDownList ID="ddlCard" runat="server" CssClass="Textbox" placeholder="Card/Network" ToolTip="Please Select Network" ></asp:DropDownList>
                    </div>
                </div>
            </div>
            </asp:Panel>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-3 col-sm-3 col-xs-4">
                       <asp:Label ID="lblchifcomplaint" runat="server" Text="Chief Complaint"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-9 col-sm-9 col-xs-8">
                        <asp:TextBox ID="txtreatmentdetail" runat="server" CssClass="Textbox" Width="100%" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-3 col-sm-3 col-xs-4">
                       <asp:Label ID="Label5" runat="server" Text="Reason for Exnsion"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-9 col-sm-9 col-xs-8">
                       <asp:TextBox ID="txtExtensionReason" runat="server" CssClass="Textbox" Width="100%" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
            </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12 p-t-b-5">
                <telerik:RadGrid RenderMode="LightWeight" ID="GvDiagnosis" runat="server"></telerik:RadGrid>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-12 p-t-b-5">
                <telerik:RadGrid ID="GvServices" runat="server"></telerik:RadGrid>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                       
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                       
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-8 col-sm-8 col-xs-8">
                      <asp:FileUpload ID="uptfiles" runat="server" CssClass="btns" AllowMultiple="true" />
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                      <asp:Button ID="btnupload" runat="server" Text="Upload" OnClick="btnupload_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 m-t gridview">
                <asp:GridView ID="grdfiles" runat="server" AutoGenerateColumns="false" OnRowCommand="grdfiles_RowCommand">
        <Columns>
            <asp:BoundField HeaderText="Filename" DataField="FileName" />
            <asp:BoundField HeaderText="Filenpath" DataField="FilePath" Visible="false" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btndelete" runat="server" Text="Delete" OnClick="btndelete_Click" CommandName='<%#Eval("FileName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
            </div>
        </div>
    </div>

        </ContentTemplate>

    </asp:UpdatePanel>
    
    

    <div id="dvHistory" runat="server" style="position: absolute; top: 1%; height: 800px; width: 100%; background-color: gainsboro;" visible="false">
        <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
        <asp:RadioButtonList ID="rbpending" runat="server" OnSelectedIndexChanged="rbpending_SelectedIndexChanged" AutoPostBack="true" RepeatColumns="2">
            <asp:ListItem Text="Pending" Value="0" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Uploaded" Value="1"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:GridView ID="gvhistory" runat="server" SkinID="gridview">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnselect" runat="server" Text="Select" OnClick="btnselect_Click" CommandName='<%#Eval("ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

