<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ProgressNoteDateWise.aspx.cs" Inherits="WardManagement_VisitHistory" %>

<%@ Register TagPrefix="aspl" TagName="ProgressNoteDateWise" Src="~/Include/Components/MasterComponent/ucProgressNoteDateWise.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr.css" rel="stylesheet" type="text/css" />
    
    
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <aspl:ProgressNoteDateWise ID="ProgressNoteDateWise" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
