<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="VisitHistory.aspx.cs" Inherits="WardManagement_VisitHistory" %>
<%@ Register TagPrefix="aspl" TagName="VisitHostory" Src="~/Include/Components/MasterComponent/ucCaseSheet.ascx" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <%: Styles.Render("~/bundles/EMRMasterWithTopDetailsCss") %>     
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <aspl:VisitHostory ID="VisitHostory" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
