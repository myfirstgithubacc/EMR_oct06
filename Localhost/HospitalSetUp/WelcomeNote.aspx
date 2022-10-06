<%--<%@ Page Language="C#" MasterPageFile="~/Include/Master/HSEMRMaster.master"  AutoEventWireup = "true"
    CodeFile="WelcomeNote.aspx.cs" Inherits="HospitalSetUp_WelcomeNote" Title="" %>--%>

<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"  AutoEventWireup = "true"
    CodeFile="WelcomeNote.aspx.cs" Inherits="HospitalSetUp_WelcomeNote" Title="" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="font-style: oblique" width="100%" cellpadding="2" cellspacing="2">
        <tr>
            <td style="height: 600px;" valign="middle">
                <table width="100%" cellpadding="2" cellspacing="2" style="background-color: Silver">
                    <tr>
                        <td>
                            <asp:Label Font-Size="Medium" Font-Bold="true" ID="lblWelcomeMessage" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Thank you for registering with  EMR. Your organization is set up and EMR
                            is ready to be used for 30 days trial period.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Next Step:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            1. Click on OK button, which will take you to login screen. Enter your username
                            and password to start the EMR.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            2. After you login, click on Administration module to configure your practice information
                            such as Facilities, Employees, Provider’s Templates, Appointment Templates, and
                            other details.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            For further assistance, please contact us at (000) 000-0000
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Button ID="btnOk" Text="Ok" Width="100px" runat="server" OnClick="btnOk_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
