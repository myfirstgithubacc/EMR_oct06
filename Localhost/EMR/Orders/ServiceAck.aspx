<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master"
    AutoEventWireup="true" CodeFile="ServiceAck.aspx.cs" Inherits="EMR_Orders_ServiceAck" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function OnClientClose(oWnd, args) {
                $get('<%=btnShowData.ClientID%>').click();
            }
            function OnClientAcknowlegdeRemarkClose(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var Ack_Remark = arg.Ack_Remakrs;
                    var IsAck = arg.IsAcknowledge;
                    $get('<%=hdnAckRemarks.ClientID%>').value = Ack_Remark;
                    //alert(Ack_Remark);
                    $get('<%=hdn_setAck.ClientID%>').value = IsAck;
                }
                $get('<%=btnAck_RemarksClose.ClientID%>').click(); 
            }
        </script>
    </telerik:RadScriptBlock>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr class="clsheader">
                    <td style="padding-left: 5px;">
                        <asp:Label ID="lblHeader" runat="server" Text="Procedure Acknowledge"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        <asp:Button ID="btnShowData" runat="server" OnClick="btnShowData_OnClick" Style="visibility: hidden;" />
                    </td>
                </tr>
                <tr id="trPatient" runat="server" visible="false">
                    <td width="100%">
                        <table cellpadding="2" cellspacing="2" class="clsheader" width="100%" style="margin-right: 8px">
                            <tr>
                                <td style="width: 80%;">
                                    &nbsp;
                                    <asp:Label ID="lbl" runat="server" Text="Patient : "></asp:Label>
                                    &nbsp;
                                    <asp:Label ID="lblPatientInfoName" runat="server" SkinID="label" Style="color: #990066;
                                        font-size: 14px"></asp:Label>
                                    <asp:Label ID="lblPatientInfoAge" runat="server" SkinID="label" Font-Bold="false"
                                        Style="font-size: 12px"></asp:Label>
                                    <asp:Label ID="lblRegNo" runat="server" Text='<%$ Resources:PRegistration, regno%>' SkinID="label" Style="font-size: 12px"></asp:Label>
                                    <asp:Label ID="lblPatientInfoRegNo" runat="server" SkinID="label" Font-Bold="false"></asp:Label>
                                    <asp:Label ID="lblDOB" runat="server" Text="DOB" SkinID="label" Style="font-size: 12px"></asp:Label>
                                    <asp:Label ID="lblPatientInfoDOB" runat="server" SkinID="label" Font-Bold="false"></asp:Label>
                                    <asp:Label ID="lblHome" runat="server" Text="Home #" SkinID="label" Style="font-size: 12px"></asp:Label>
                                    <asp:Label ID="lblPatientInfoHome" runat="server" SkinID="label" Font-Bold="false"></asp:Label>
                                    <asp:Label ID="lblMobile" runat="server" Text="Mobile #" SkinID="label" Style="font-size: 12px"></asp:Label>
                                    <asp:Label ID="lblPatientInfoMobile" runat="server" SkinID="label" Font-Bold="false"></asp:Label>
                                </td>
                                <td style="width: 20%; text-align: right; padding-right: 5px;">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;">
                        <%--updated some lines of code in this block to add the service filter also start--%>
                        <table cellpadding="3" cellspacing="0" width="100%" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Source" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlPatientType" runat="server" SkinID="DropDown" Width="50px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                            <telerik:RadComboBoxItem Text="OPD" Value="OPD" />
                                            <telerik:RadComboBoxItem Text="IPD" Value="IPD" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblProcedureType" runat="server" Text="Acknowledge Status" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlProcedureType" runat="server" SkinID="DropDown" Width="150px">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value=" " />
                                            <telerik:RadComboBoxItem Text="Acknowledged" Value="A" />
                                            <telerik:RadComboBoxItem Text="Un-Acknowledged" Value="U" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                
                                    <asp:Label ID="Label5" runat="server" Text="Search by" />
                                    
                                    <telerik:RadComboBox ID="drpSearchby" runat="server" SkinID="DropDown" Width="100px">
                                    <Items>
                                            <telerik:RadComboBoxItem Text="Patient Name" Value="0" />
                                            <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, regno%>' Value="1" />
                                            <telerik:RadComboBoxItem Text="Order No" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                               &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtSerachby" runat="server" SkinID="textbox" />
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department" />
                                </td>
                                <td >
                                    <telerik:RadComboBox ID="ddlDepartment" runat="server" 
                                        AutoPostBack="true" Width="200px" DropDownWidth="300px" EmptyMessage="Select Department"
                                        OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubdept" runat="server" Text="Sub-Department" />
                                </td>
                                <td>
                                    <telerik:RadComboBox ID="ddlSubDepartment" runat="server" 
                                        Width="250px" DropDownWidth="300px" EmptyMessage="Select Sub Department" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSubDepartment_OnSelectedIndexChanged">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label13" runat="server" SkinID="label" Text="Service Name" />
                               &nbsp;&nbsp;&nbsp;<telerik:RadComboBox ID="ddlServiceName" SkinID="DropDown" runat="server" Width="300px"
                                        EmptyMessage="[ Select ]" Filter="Contains" DropDownWidth="450px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <asp:Label ID="Label3" runat="server" Text="Facility" SkinID="label"></asp:Label>&nbsp;  
                                </td>
                                <td>
                                   <telerik:RadComboBox ID="ddlFacility" runat="server" AppendDataBoundItems="true"
                                        Width="120px"  EmptyMessage="Select Facility">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                   <asp:Label ID="Label1" runat="server" Text="From Date" SkinID="label"></asp:Label>&nbsp; 
                                </td>
                                <td>
                                <table>
                                        <tr>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpfromdate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"
                                                    Width="100px">
                                                </telerik:RadDatePicker>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="To" SkinID="label"></asp:Label>&nbsp;
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpTodate" runat="server" MinDate="01/01/1900" DateInput-DateFormat="dd/MM/yyyy"
                                                    Width="100px">
                                                </telerik:RadDatePicker>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btnFilter" runat="server" SkinID="Button" Text="Filter" OnClick="btnShowData_OnClick" />
                                    &nbsp;
                                    <asp:Button ID="btnClear" runat="server" SkinID="Button" Text="Reset" OnClick="btnClear_OnClick" />
                                    <asp:Button ID="btnAck_RemarksClose" runat="server" CausesValidation="false"
                                        Style="visibility: hidden;" OnClick="btnAck_RemarksClose_OnClick" Width="1px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlDtl" runat="server" Width="100%">
                            <asp:GridView ID="gvData" runat="server" Width="100%" AutoGenerateColumns="false"
                                AllowPaging="true" PageSize="10" OnPageIndexChanging="gvData_OnPageIndexChanging"
                                OnRowDataBound="gvData_OnRowDataBound" SkinID="gridview">
                                <Columns>
                                
                                   <asp:TemplateField HeaderText='Order No.' HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderID" runat="server" Text='<% #Eval("OrderNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText='Invoice No.' HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<% #Eval("InvoiceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                
                                   <asp:TemplateField HeaderText='Patient Type' HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="30px" HeaderStyle-Width="30px" >
                                        <ItemTemplate>
                                            <asp:Label ID="lblPatientType" runat="server" Text='<% #Eval("Source") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno %>' HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNo" runat="server" Text='<% #Eval("RegistrationNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Patient Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkPatient" runat="server" Text='<% #Eval("PatientName") %>'
                                                OnClick="lnkPatient_OnClick"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AgeGender" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAgeGender" runat="server" Text='<% #Eval("AgeGender") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CompanyType" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCompanyType" runat="server" Text='<% #Eval("CompanyType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Service Name" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblServiceName" runat="server"  Text='<%#Eval("ServiceName") %>'   OnClick="lnkViewServiceAck_OnClick"    ></asp:LinkButton>
                                            <%--<asp:Label ID="lblServiceName" runat="server" Text='<% #Eval("ServiceName") %>'></asp:Label>--%>
                                            <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId") %>' />
                                            <asp:HiddenField ID="hdnServiceDtlId" runat="server" Value='<%#Eval("OrderDetailId") %>' />
                                            <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderId") %>' />
                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                            <asp:HiddenField ID="hdnFacilityName" runat="server" Value='<%#Eval("FacilityName") %>' />
                                            <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                            <asp:HiddenField ID="hdnProAcId" runat="server"  Value='<%#Eval("ProAckId") %>' />
                                              <asp:HiddenField ID="hdnActive" runat="server"  Value='<%#Eval("Active") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="OrderDate" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderDate" runat="server" Text='<% #Eval("OrderDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Advisory Provider" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocName" runat="server" Text='<%#Eval("DoctorName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Performing Provider" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPerformingDoctorName" runat="server" Text='<%#Eval("PerformingDoctorName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="100px" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAck" runat="server" Text="Acknowledge" OnClick="lnkAck_OnClick"></asp:LinkButton>
                                            <asp:HiddenField ID="hdnIsAcknowledge" runat="server" Value='<%#Eval("IsAcknowledge") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View Details" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkViewDetails" runat="server" Text="View" OnClick="lnkViewDetails_OnClick"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="60px" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                       <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top:10px; padding-left:5px;">
                        <asp:Label ID="lblAcknowledged" runat="server" BorderWidth="1px" Text="&nbsp;" Width="20px" />
                        <asp:Label ID="Label12" runat="server" SkinID="label" Text="Acknowledged Orders"></asp:Label>
                        
                        <asp:Label ID="lblCancelRefund" runat="server" BorderWidth="1px" Text="&nbsp;" Width="20px" />
                        <asp:Label ID="Label7" runat="server" SkinID="label" Text="Cancelled/Refunded Order"></asp:Label>
                        
                        
                        <asp:HiddenField ID="hdnAckRemarks" runat="server"  />
                        <asp:HiddenField Id="hdn_ServiceID" runat="server" />
                        <asp:HiddenField ID="hdn_ServiecDeID" runat="server" />
                        <asp:HiddenField ID="hdn_OrderId" runat="server" /> 
                        <asp:HiddenField ID="hdn_PatientType" runat="server" />
                        <asp:HiddenField ID="hdn_RegistrationId" runat="server" /> 
                        <asp:HiddenField ID="hdn_EncounterId" runat="server" />
                        <asp:HiddenField ID="hdn_IsAcknowledge" runat="server"  />
                        <asp:HiddenField ID="hdn_hdnProAcId" runat="server"  />
                        <asp:HiddenField ID="hdn_setAck" runat="server" /> 
                        
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
