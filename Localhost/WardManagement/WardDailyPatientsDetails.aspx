<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WardDailyPatientsDetails.aspx.cs" Inherits="WardManagement_WardDailyPatientsDetails" MasterPageFile="~/Include/Master/EMRMaster.master"%>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="aspl" TagName="ICD" Src="~/Include/Components/ICDPanel.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register TagPrefix="asplUD" TagName="UserDetails" Src="~/Include/Components/TopPanelNew.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" /> 
    <link href="../../Include/css/mainStyle.css" rel='stylesheet' type='text/css'>   
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr1.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />

    
    
    <div class="container-fluid header_main">

        <div>
             <asp:UpdatePanel ID="aa14" runat="server">
                            <ContentTemplate>
                                <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                                    <Windows>
                                        <telerik:RadWindow ID="RadWindowForReport" runat="server" Behaviors="Close,Move">
                                        </telerik:RadWindow>
                                    </Windows>
                                </telerik:RadWindowManager>
                                <asp:TextBox ID="lblPharmacyId" runat="server" Text="0" Width="0" Style="visibility: hidden;"></asp:TextBox>
                                <asp:TextBox ID="txtPatientImageId" runat="server" Style="visibility: hidden;" Text=""></asp:TextBox>
                            </ContentTemplate>
             </asp:UpdatePanel>
        </div>
             
            <div class="row">
                <div class="col-lg-2">
                </div>
                <div class="col-lg-8">
                    <div class="row" style="margin-top:07px;">
                         <div class="col-lg-1"></div>
                        <div class="col-lg-5">
                            <label>Print Report Type</label>
                            <asp:DropDownList runat="server" ID="ddldailypatientdetails" width="188">
                                <asp:ListItem Text="--select--" Value="0"/>
                                <asp:ListItem Text="Daily Patient Report" Value="1"/>
                                <asp:ListItem Text="Daily Diet Patient Report" Value="2"/>
                            </asp:DropDownList>
                        </div> 
                        <div class="col-lg-4">
                            <label>Ward Name</label>
                             <telerik:RadComboBox ID="ddlWard" runat="server" Width="120" DropDownWidth="250px" />
                    
                        </div>
                         <div class="col-lg-2">
                             <asp:CheckBox ID="chkexport" runat="server" Text="Export"/>
                         </div>
                    </div>
          
                </div>
                        <div class="col-lg-2">
                            <asp:Button ID="btnprint"  Text="Print (Ctrl+F9)"  CssClass="PatientBtn01" runat="server" OnClick="btnprint_Click"/>
                         </div>
           </div>  
           </div>
</asp:Content>