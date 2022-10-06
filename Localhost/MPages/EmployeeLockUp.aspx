<%@ Page Language="C#" MasterPageFile="~/Include/Master/BlankMaster.master" Theme="DefaultControls"
    AutoEventWireup="true" CodeFile="EmployeeLockUp.aspx.cs" Inherits="MPages_EmployeeLockUp"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" type="text/css" />    
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />

    
    
    <div class="container-fluid header_main form-group">
        <div class="col-md-2 col-sm-3"><h2>Search Employee</h2></div>
        <div class="col-md-4 col-sm-1 text-center">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="true" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-6 col-sm-8 text-right">
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div id="DivMenu" runat="server">
                        <asp:LinkButton ID="lnkEmployee" runat="server" CausesValidation="false" Text="Employee" CssClass="btnNew"
                            onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="lnkEmployee_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkProviderProfile" runat="server" CausesValidation="false" Text="Employee Profile"
                            CssClass="btnNew" onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            OnClick="lnkProviderProfile_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkAppointmentTemplate" runat="server" CausesValidation="false"
                            Text="Appointment Template" onmouseover="LinkBtnMouseOver(this.id);" CssClass="btnNew"
                            onmouseout="LinkBtnMouseOut(this.id);" OnClick="lnkAppointmentTemplate_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkProviderDetails" runat="server" CausesValidation="false" CssClass="btnNew"
                            onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            Text="Doctor Details" OnClick="lnkProviderDetails_OnClick"></asp:LinkButton>
                        <asp:LinkButton ID="lnkClassification" runat="server" CausesValidation="false" CssClass="btnNew"
                            onmouseover="LinkBtnMouseOver(this.id);" onmouseout="LinkBtnMouseOut(this.id);"
                            Text="Classification" OnClick="lnkClassification_OnClick"></asp:LinkButton>
                        <script language="JavaScript" type="text/javascript">
                                function LinkBtnMouseOver(lnk) {
                                    document.getElementById(lnk).style.color = "SteelBlue";
                                }
                                function LinkBtnMouseOut(lnk) {
                                    document.getElementById(lnk).style.color = "SteelBlue";
                                }
                        </script>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>



    </div>
    
    <asp:Panel ID="pnlSearch" runat="server" BackColor="White">
        
        <div class="container-fluid">
            
            <div class="row">
                <div class="col-md-3 col-sm-6">
                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4 PaddingRightSpacing">
                            <asp:Literal ID="ltrSearchOn" runat="server" Text="Employee Name"></asp:Literal>
                            <%-- <asp:TextBox ID="txtUHID"  SkinID="textbox" runat="server"></asp:TextBox>--%>
                        </div>
                        <div class="col-md-8 col-sm-8">
                            <%-- <asp:Literal ID="ltrFullName" runat="server" Text="Name"></asp:Literal>--%>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtFullName" Width="100%" MaxLength="30" runat="server"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <div class="col-md-3 col-sm-6">
                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4"><asp:Literal ID="ltrMobileNo" runat="server" Text="Mobile No."></asp:Literal></div>
                        <div class="col-md-8 col-sm-8">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtMobileNo" Width="100%" runat="server"></asp:TextBox>
                                    <AJAX:FilteredTextBoxExtender ID="MobileFilteredTextBoxExtender" runat="server" Enabled="True"
                                        TargetControlID="txtMobileNo" FilterType="Custom, Numbers">
                                    </AJAX:FilteredTextBoxExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <div class="col-md-2 col-sm-6">
                    <div class="row form-group">
                        <div class="col-md-4 col-sm-4"><asp:Literal ID="ltrEmployeeType" runat="server" Text="Type"></asp:Literal></div>
                        <div class="col-md-8 col-sm-8">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlEmployeeType" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <div class="col-md-4 col-sm-6">
                    <div class="row form-group">
                        <div class="col-md-2 col-sm-4"><asp:Literal ID="ltrStatus" runat="server" Text="Status"></asp:Literal></div>
                        <div class="col-md-3 col-sm-3 PaddingRightSpacing">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlStatus" Width="100%" runat="server">
                                        <asp:ListItem Selected="True" Text="All" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="In-Active" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="col-md-7 col-sm-5 PaddingRightSpacing">
                            <asp:UpdatePanel ID="up1" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="gvEmployeeDetail" />
                                    <asp:AsyncPostBackTrigger ControlID="txtFullName" />
                                    <asp:AsyncPostBackTrigger ControlID="txtMobileNo" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlEmployeeType" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlStatus" />
                                    <asp:PostBackTrigger ControlID="btnExportToExcel"/>
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" CssClass="btn btn-primary"
                                        OnClick="btnSearch_Click" Text="Filter" />
                                    <asp:Button ID="btnClearFilter" runat="server" CausesValidation="true" CssClass="btn btn-primary"
                                        OnClick="btnClearFilter_Click" Text="Clear Filter" />
                                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn btn-primary"
                                        OnClick="btnExportToExcel_OnClick" Text="Export To Excel" ToolTip="Export To Excel" />

                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </div>
                    </div>
                </div>

            </div>

            <div class="row form-group">
                <asp:Panel ID="pnlemployee" ScrollBars="Vertical" runat="server" Height="500px">
                    <asp:UpdatePanel ID="up2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvEmployeeDetail" runat="server" Width="100%" SkinID="gridviewOrderNew"
                                AutoGenerateColumns="false" OnSelectedIndexChanged="gvEmployeeDetail_SelectedIndexChanged"
                                OnRowDataBound="gvEmployeeDetail_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="EmployeeId" />
                                    <asp:BoundField HeaderText="Name" DataField="EmployeeName" />
                                    <asp:BoundField HeaderText="Phone Home" DataField="PhoneHome" />
                                    <asp:BoundField HeaderText="Mobile" DataField="Mobile" />
                                    <asp:BoundField HeaderText="Email" DataField="Email" />
                                    <asp:BoundField HeaderText="Employee Type" DataField="Description" />
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:CommandField ShowSelectButton="true" SelectText="Edit" ItemStyle-ForeColor="DodgerBlue" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <span style="text-decoration: bold; font-weight: bold; color: Red; float: left; text-align: center; width: 100% !important; margin: 1em 0; padding: 0; font-size:11px;">No Rows Found. </span>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>


        </div>


            <table width="100%" cellpadding="0" cellspacing="0" style="height: 100%">
                
                <tr>
                    <td>
                        
                    </td>
                </tr>
            </table>
        </asp:Panel>
    
</asp:Content>
