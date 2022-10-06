<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="SendingFileFromWord.aspx.cs" Inherits="MRD_SendingFileFromWord" Title="Untitled Page" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="../Include/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../Include/css/mainNew.css" />
    <link rel="stylesheet" href="../Include/EMRStyle.css" />

    <script type="text/javascript">
        function showMenu(e, menu, requestId) {
            $get('<%=hdnGRequestId.ClientID%>').value = $get(requestId).value;

            var menu = $find(menu);
            menu.show(e);
        }
  
    </script>

    <%--<asp:UpdatePanel ID="upd1" runat="server">
        <ContentTemplate>--%>
    <div class="container-fluid">
        <div class="row header_main">
            <div class="col-md-3 col-sm-3 col-xs-12"></div>
            <div class="col-md-9 col-sm-9 col-xs-12 text-right">
                <asp:Button ID="btnExport" runat="server" Text="Export To Excel" ToolTip="Export To Excel" CssClass="btn btn-primary"
                                onclick="btnExport_Click" />
            </div>
        </div>
        <div class="row text-center">
            <asp:Label ID="lblMessage" SkinID="label" runat="server" Text="" />
        </div>
        <div class="row">
            <div class="col-md-1 col-sm-1 col-xs-2 text-nowrap p-t-b-5">
                        <asp:Label ID="Label2" runat="server" SkinID="label" Text="Date Range" />
                    </div>
            <div class="col-md-4 col-sm-4 col-xs-12">
                
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">                            
                            <ContentTemplate>
                                <div class="row p-t-b-5" id="tblDate" runat="server" visible="true">
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label17" runat="server" SkinID="label" Text="From&nbsp;Date" />
                                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <telerik:RadDatePicker ID="txtFromDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                    </div>
                                </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="row">
                                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                                        <asp:Label ID="Label18" runat="server" SkinID="label" Text="To&nbsp;Date" />
                                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8 text-nowrap">
                        <telerik:RadDatePicker ID="txtToDate" runat="server" Width="100%" DateInput-ReadOnly="true" />
                    </div>
                                </div>
                                </div>
                                     </div>
                                 </ContentTemplate>
                        </asp:UpdatePanel>
               
            </div>


                

            <div class="col-md-3 col-sm-3 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-4 col-sm-4 col-xs-4 text-nowrap">
                        <asp:Label ID="Label4" runat="server" SkinID="label" Text="Search On" />
                    </div>
                    <div class="col-md-8 col-sm-8 col-xs-8">
                        <div class="row">
                            <div class="col-md-5 col-sm-5 col-xs-5">
                                <telerik:RadComboBox ID="ddlSearchOn" Width="100%" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSearchOn_SelectedIndexChanged">
                            <Items>
                                
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, Regno%>' Value="UHID" Selected="true" />
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, EncounterNo%>' Value="Enc" />
                                <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, PatientName%>' Value="PatN" />
                            </Items>
                        </telerik:RadComboBox>
                            </div>
                            <div class="col-md-7 col-sm-7 col-xs-7">
                                 <asp:TextBox ID ="txtSearchOnForUHID" Width="100%" runat="server" />
                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                        FilterType="Custom" TargetControlID="txtSearchOnForUHID" ValidChars="0123456789" />
                        <%-- <asp:TextBox ID ="txtSearchOn" runat="server" Visible="false"/>--%>
                         
                        <asp:TextBox ID ="txtSearchOnEnc" runat="server" Visible="false"/>
                         <asp:TextBox ID ="txtSearchOnPat" runat="server" Visible="false"/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-2 col-sm-2 col-xs-12">
                <div class="row p-t-b-5">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Filter" OnClick="btnFilter_OnClick" />
                    </div>
                </div>
            </div>
            
        </div>

        <div class="row m-t">
            <div class="col-md-12 col-sm-12 col-xs-12 gridview">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvData" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" Height="500px" Width="100%" ScrollBars="Auto"
                                    BorderWidth="1px" BorderColor="LightBlue">
                                    <asp:GridView ID="gvData" SkinID="gridview" runat="server" AutoGenerateColumns="False"
                                        Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="false"
                                        OnRowDataBound="gvData_RowDataBound" OnRowCommand="gvData_OnRowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegistrationNo" runat="server" Width="100%" Text='<%#Eval("RegistrationNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, EncounterNo%>' ItemStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncounterNo" runat="server" Width="100%" Text='<%#Eval("EncounterNo")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ Resources:PRegistration, PatientName%>' ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientName" runat="server" Width="100%" Text='<%#Eval("PatientName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Age / Gender' ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPatientAgeGender" runat="server" Width="100%" Text='<%#Eval("PatientAgeGender")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText='Ward Name' ItemStyle-Width="100px">
                                                <ItemTemplate>

                                             <asp:Label ID="lblCurrentWard" runat="server" Text='<%#Eval("CurrentWard")%>' />
                                               
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText='Bed No' ItemStyle-Width="100px">
                                                <ItemTemplate>

                                             <asp:Label ID="lblCurrentBedNo" runat="server" Text='<%#Eval("CurrentBedNo")%>' />
                                               
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                         
                                            <asp:TemplateField HeaderText='DischargeDate Date' ItemStyle-Width="60px">
                                                <ItemTemplate>

                                             <asp:Label ID="lblDischargeDate" runat="server" Text='<%#Eval("DischargeDate")%>' />
                                               
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='Sending Date' ItemStyle-Width="60px">
                                                <ItemTemplate>

                                             <asp:Label ID="lblFileRequestDate" runat="server" Text='<%#Eval("SendingDate")%>' />
                                               
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                             <asp:TemplateField HeaderText='Sending By' ItemStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEncodedBy" runat="server" Width="100%" Text='<%#Eval("SendingBy") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText='Return Status' ItemStyle-Width="80px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReturnStatus" runat="server" Width="100%" Text='<%#Eval("ReturnStatus") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
            </div>
        </div>

    </div>
            <table cellpadding="0" cellspacing="0" runat="server" border="0" width="99%">
                <tr>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnGRequestId" runat="server" />
                    </td>
                </tr>
            </table>
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
