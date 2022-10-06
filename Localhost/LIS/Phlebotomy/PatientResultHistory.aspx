<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientResultHistory.aspx.cs"
    Inherits="LIS_Phlebotomy_PatientResultHistory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" />

    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scrpmgr" runat="server"></asp:ScriptManager>

        <div class="container-fluid header_main">
            <div class="col-md-3 col-sm-3"><h2 style="color:#333;"><asp:Label ID="lblHeader" runat="server" Text="Patient Result History"></asp:Label><asp:Label ID="lblServiceName" runat="server" /></h2></div>
            <div class="col-md-6 col-sm-6 text-center">
                <asp:UpdatePanel ID="uplblmsg" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="col-md-3 col-sm-3 text-right"><asp:Button ID="btnClose" Text="Close" runat="server" CssClass="btn btn-default" CausesValidation="false" OnClientClick="window.close();" /></div>
        </div>
        

        <table cellspacing="0" class="table table-small-font table-bordered table-striped">
            <tbody>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <tr align="center">
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-1">
                                <asp:Label ID="Label3" runat="server" Text='<%$ Resources:PRegistration, regno%>'  Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblRegNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            </td>
                            <td data-priority="3" colspan="1" data-columns="tech-companies-1-col-2">
                                <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            </td>
                            <td data-priority="1" colspan="1" data-columns="tech-companies-1-col-3">
                                <asp:Label ID="Label5" runat="server" Text="Age/Gender:" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblAgeGender" runat="server" Text="" SkinID="label"></asp:Label>
                            </td>
                        </tr> 
                    </ContentTemplate>
                </asp:UpdatePanel>
            </tbody>
        </table>


        <div class="container-fluid">
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="true" 
                        AllowPaging="false"
                            AllowSorting="true" SkinID="gridviewOrderNew" OnRowDataBound="gvResult_OnRowDataBound" Width="100%">
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>



    <div>
        <table id="table1" width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table id="tbl" runat="server" cellpadding="1" cellspacing="1" border="0" visible="false" >
                                <tr>
                                    <td>
                                    <asp:Label ID="Label4" runat="server" SkinID="label" Text="Period" />
                                        <asp:DropDownList ID="ddlrange" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlrange_SelectedIndexChanged"
                                            SkinID="DropDown" Width="140px" CausesValidation="false">
                                            <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Today"  Value="DD0"></asp:ListItem>
                                            <asp:ListItem Text="Last Week"  Value="WW-1"></asp:ListItem>
                                            <asp:ListItem Text="This Month" Value="MM0"></asp:ListItem>
                                            <asp:ListItem Text="Last One Month" Value="MM-1"></asp:ListItem>                                            
                                            <asp:ListItem Text="Last Three Month"  Value="MM-3"></asp:ListItem>
                                            <asp:ListItem Text="Last six Month"  Value="MM-6"></asp:ListItem>
                                            <asp:ListItem Text="Last Year"  Value="YY-1"></asp:ListItem>
                                            <asp:ListItem Text="Date Range" Value="4"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <table id="tblDateRange" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server" SkinID="label" Text="From" />
                                                    <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                                    <asp:Label ID="Label7" runat="server" SkinID="label" Text="To" />
                                                    <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100px" DateInput-ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="No of Results" />
                                        <asp:TextBox ID="txtNoOfResult" runat="server" SkinID="textbox" Text="5" Width="40px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnFilter" runat="server" Text="Filter" SkinID="Button" OnClick="btnFilter_OnClick" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
