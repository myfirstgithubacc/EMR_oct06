<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="CustomErrorPage.aspx.cs" Inherits="CustomErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" class="clsheader">
        <tr>
            <td id="tdName" runat="server">
                Error...
            </td>
        </tr>
    </table>
    <table width="100%" style="align-content:center; text-align:center; font:700; ">
        <tr>
            <td>
                <h1>OOPS, Something Went Wrong.</h1>
            </td>
        </tr>
        <tr>
            <td>
                <h3>Error already logged. You can contact IT for further clarifications.</h3>
            </td>
        </tr>
    </table>
</asp:Content>

