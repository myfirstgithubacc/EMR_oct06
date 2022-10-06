<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="VisitHistory.aspx.cs" Inherits="Editor_VisitHistory" %>
<%@ Register TagPrefix="aspl" TagName="VisitHostory" Src="~/Include/Components/MasterComponent/ucCaseSheetNew.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <aspl:VisitHostory ID="VisitHostory" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
