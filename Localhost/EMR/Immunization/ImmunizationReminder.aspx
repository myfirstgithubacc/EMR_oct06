<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="ImmunizationReminder.aspx.cs" Inherits="EMR_Immunization_ImmunizationReminder"  %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
     <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlOption" />
                </Triggers>
        <ContentTemplate>


    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>

            <div class="container-fluid header_main">
                 <div class="col-md-3">
                  <h2>Immunization Reminder</h2>
                 </div>
				 
                 <div class="col-md-5 text-center"><asp:HiddenField ID="hdnCurrentDate" runat="server" />
                                        <asp:Label ID="lblmsg" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label> </div>
                 
                 <div class="col-md-3 text-right pull-right"> </div>
</div>


            <div class="container-fluid subheading_main">
                <div class="col-md-3">
                     <asp:Label ID="Label1" runat="server" Text="Filter" SkinID="label"></asp:Label>
                        &nbsp;&nbsp;
                        <telerik:RadComboBox ID="ddlOption" runat="server" AutoPostBack="true" Width="100px"
                            OnSelectedIndexChanged="ddlOption_OnSelectedIndexChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="0" />
                                <telerik:RadComboBoxItem Text="Next 7 Days" Value="7" />
                                <telerik:RadComboBoxItem Text="Next 15 Days" Value="15" />
                                <telerik:RadComboBoxItem Text="Next 30 Days" Value="30" />
                                <telerik:RadComboBoxItem Text="Next 45 Days" Value="45" />
                                <telerik:RadComboBoxItem Text="Next 60 Days" Value="60" />
                                <telerik:RadComboBoxItem Text="Date Range" Value="4" />
                            </Items>
                        </telerik:RadComboBox>

                </div>
                <div class="col-md-3">
                     <table id="tblDateRange" runat="server">
                            <tr>
                                <td align="left">
                                    <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100px">
                                    <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                                <td>
                                    &nbsp;To&nbsp;
                                    <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100px">
                                    <DateInput DateFormat="dd/MM/yyyy"></DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                            </tr>
                        </table>

                </div>

                <div class="col-md-3 pull-right text-right">
                     <asp:Button ID="btnSearch" runat="server" cssClass="btn btn-primary" OnClick="btnSearch_OnClick"
                            Text="Refresh" />
                        &nbsp;&nbsp; &nbsp;&nbsp;
                        <asp:Button ID="btnSend" runat="server" cssClass="btn btn-primary" OnClick="btnSend_OnClick"
                            Text="Send SMS" Visible="false"  />

                </div>
            </div>

           



            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4">
                        <div style="height: 620px; width: 100%; overflow: auto;">
                            <asp:GridView ID="gvReminder" SkinID="gridview" runat="server" AutoGenerateColumns="false"
                                ShowHeader="true" Width="100%" AllowPaging="false" PagerSettings-Mode="NumericFirstLast"
                                ShowFooter="false" PagerSettings-Visible="true" HeaderStyle-HorizontalAlign="Left"
                                OnRowDataBound="gvReminder_OnDataBound" 
                                onrowcreated="gvReminder_RowCreated">
                                <HeaderStyle CssClass="GVFixedHeader" />
                                <EmptyDataTemplate>
                                    <div style="font-weight: bold; color: Red; width: 100%">
                                        No Record Found.</div>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="30px"
                                        ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" Text='<%#Eval("RegistrationNo")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Patient Name" HeaderStyle-Width="220px" ItemStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientName" Text='<%#Eval("PatientName")%>' runat="server"></asp:Label>
                                            <asp:HiddenField ID="lblImmunizationId" Value='<%#Eval("ImmunizationId")%>' runat="server" />
                                            <asp:HiddenField ID="hdnRegistrationId" Value='<%#Eval("RegistrationId")%>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Immunization Name" HeaderStyle-Width="250px" ItemStyle-Width="220px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblImmunizationName" Text='<%#Eval("ImmunizationName")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Due Date" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDate" Text='<%#Eval("ImmunizationDueDate")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="MobileNo" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMobileNo" Text='<%#Eval("MobileNo")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmail" Text='<%#Eval("Email")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSend" runat="server" Text="" SkinID="checkbox" Checked="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblTotalCount" runat="server" SkinID="label" Font-Bold="True" Font-Size="Medium" ></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
       
    </asp:UpdatePanel>
</asp:Content>

