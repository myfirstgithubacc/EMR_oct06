<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="ChangeStation.aspx.cs" Inherits="LIS_Phlebotomy_ChangeStation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />



    
    
    
    
    <asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>
            
            
            <div class="container-fluid header_main">
                <div class="col-md-4"><h2><asp:Label ID="lblHeader" runat="server" Text="Change&nbsp;Station" /></h2></div>
                <div class="col-md-8 text-center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            
            
           
            
            <br /><br />
           
                <div class="container-fluid main_box">
                    <div id="Div1" class="row" runat="server">
                        <div class="col-md-offset-4 col-md-4 header_main">
                            <div id="Div2" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <br /><br />
                                        <div class="col-md-4"><asp:Label ID="Label1" runat="server" Text="Select&nbsp;Station&nbsp;<span style='color: Red'>*</span>" /></div>
                                        <div class="col-md-8">
                                            <telerik:RadComboBox ID="ddlStation" SkinID="DropDown" runat="server" Width="100%"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlStation_OnSelectedIndexChanged" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlStation"
                                                ValidationGroup="SaveData" Display="None" runat="server" ErrorMessage="Select Station !"
                                                Text="" />
                                        </div>
                                        
                                        
                                        <div class="col-md-12 text-center form-groupTop">
                                             <br /><asp:Button ID="btnSave" runat="server" OnClick="btnSave_OnClick" CssClass="btn btn-primary" Text="Proceed" />
                                             <br /> <br />
                                        </div>
                                        
                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
