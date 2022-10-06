<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true"
    CodeFile="AuditTrail.aspx.cs" Inherits="EMR_Masters_AuditTrail" Title="" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
    <!-- Bootstrap -->
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />   
    <link href="../../Include/css/Administration.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />

           
            <div class="VisitHistoryDiv">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-3"><div class="VisitHistoryDivText"><h2>Audit Trail</h2></div></div>
                        <div class="col-md-8"></div>
                        <div class="col-md-1"><asp:Button ID="btnprint" runat="server" CssClass="AdminBtn01" Text="Print" ToolTip="Save" CausesValidation="false" OnClick="btnprint_Click" /></div>
                    </div>
                </div>
            </div>
            
            
            
             <div class="AuditMessage">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-md-12"><tr><td align="center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></td></tr></div>
                    </div>
                </div>
             </div>           
            
            
            
            
            
            
            
     
            
            <table border="0" cellpadding="0" cellspacing="2" style="padding-left: 5px;">
                
               <tr>     
                        <td><asp:Label ID="Label2" runat="server" SkinID="label" Text="Options" /></td>
                        <td>
                            <telerik:RadComboBox ID="ddlOptions" runat="server" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlOptions_OnSelectedIndexChanged">
                                <Items>
                                  <telerik:RadComboBoxItem Text="Employee" Value="E" />
                                  <telerik:RadComboBoxItem Text="Registration" Value="R" />

                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        
                        
                        <td><asp:Label ID="Label22" runat="server" SkinID="label" Text="Facility" /></td>
                        <td><telerik:RadComboBox ID="ddlFacility" runat="server" Width="155px" /></td>
                        
                        <td><asp:Label ID="Label1" runat="server" SkinID="label" Text="Date" /></td>
                        <asp:UpdatePanel ID="upNewSave" runat="server">
        <ContentTemplate>
                        <td>
                            <telerik:RadComboBox ID="ddlRange" runat="server" SkinID="DropDown" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlRange_SelectedIndexChanged">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Selected="true" Value="0" />
                                    <telerik:RadComboBoxItem Text="Today" Value="DD0" />
                                    <telerik:RadComboBoxItem Text="Date Range" Value="4" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                       </ContentTemplate>
                        </asp:UpdatePanel>
                        
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                        <td>
                            <table id="tdDate" runat="server" cellpadding="0" cellspacing="0" visible="false">
                                <tr>
                                    <td align="left"><telerik:RadDatePicker ID="dtpfromDate" runat="server" MinDate="1900-01-01 00:00" Width="100px"></telerik:RadDatePicker></td>
                                    <td><asp:Label ID="lblTo" runat="server" Text="To" SkinID="label" /></td>
                                    <td><telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="1900-01-01 00:00" Width="100px" /></td>
                                </tr>
                            </table>
                        </td>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </tr>
                
                
                
                <tr>
                    <td><asp:Label ID="Label3" runat="server" SkinID="label" Text="Search&nbsp;On" /></td>
                    <td><telerik:RadComboBox ID="ddlSearchOn" runat="server" Skin="Outlook" Width="120px" /></td>
                    <td colspan="2">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch"><asp:TextBox ID="txtSearch" runat="server" MaxLength="50" Width="200px" SkinID="textbox" /></asp:Panel>
                        <telerik:RadComboBox ID="ddlServiceId" runat="server" Width="200px" DropDownWidth="300px" MarkFirstMatch="true" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlServiceId_OnSelectedIndexChanged" />
                    </td>
                    
                    <td><asp:Button ID="btnSearch" runat="server" CausesValidation="true" OnClick="btnSearch_Click" SkinID="Button" Text="Filter" /></td>
                    <td><asp:Button ID="btn_ClearFilter" runat="server" CausesValidation="false" OnClick="btn_ClearFilter_Click" SkinID="Button" Text="Clear Filter" /></td>
                </tr>
            </table>
            
  
            
            
            
            
 
            <div class="AuditTrailDiv">
                <div class="container-fluid">                
                    <div class="row">                       
                        <div class="col-md-12">                                 
                    
                        <asp:Panel ID="Panel2" runat="server" Height="240px" Width="100%" ScrollBars="Auto">
                            <asp:GridView ID="gvAudit" SkinID="gridview" runat="server" AutoGenerateColumns="False" Height="100%" Width="100%" CellPadding="0" CellSpacing="0" AllowPaging="true" PageSize="15" OnPageIndexChanging="gvAudit_OnPageIndexChanging" OnRowDataBound="gvAudit_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, SerialNo%>' HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="60px">
                                        <ItemTemplate><asp:Label ID="lblRegistrationNo" runat="server" SkinID="label" Width="100%" Text='<%#Eval("RegistrationNo")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, ipno%>' HeaderStyle-Width="60px">
                                        <ItemTemplate><asp:Label ID="lblEncounterNo" runat="server" SkinID="label" Width="100%" Text='<%#Eval("EncounterNo")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Patient&nbsp;Name'>
                                        <ItemTemplate><asp:Label ID="lblPatientName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("PatientName")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Service&nbsp;Name'>
                                        <ItemTemplate><asp:Label ID="lblServiceName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("ServiceName")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Company&nbsp;Name'>
                                        <ItemTemplate><asp:Label ID="lblCompanyName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("CompanyName")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Employee No' HeaderStyle-Width="80px">
                                        <ItemTemplate><asp:Label ID="lblEmployeeNo" runat="server" SkinID="label" Width="100%" Text='<%#Eval("EmployeeNo")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Employee&nbsp;Name'>
                                        <ItemTemplate><asp:Label ID="lblEmployeeName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("EmployeeName")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Facility&nbsp;Name' HeaderStyle-Width="110px">
                                        <ItemTemplate><asp:Label ID="lblFacilityName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("FacilityName")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Field&nbsp;Name' HeaderStyle-Width="100px">
                                        <ItemTemplate><asp:Label ID="lblFieldName" runat="server" SkinID="label" Width="100%" Text='<%#Eval("FieldName")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Old&nbsp;Value' HeaderStyle-Width="140px">
                                        <ItemTemplate><asp:Label ID="lblDisplayOldValue" runat="server" SkinID="label" Width="100%" Text='<%#Eval("DisplayOldValue")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='New&nbsp;Value' HeaderStyle-Width="140px">
                                        <ItemTemplate><asp:Label ID="lblDisplayNewValue" runat="server" SkinID="label" Width="100%" Text='<%#Eval("DisplayNewValue")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Changed&nbsp;By' HeaderStyle-Width="140px">
                                        <ItemTemplate><asp:Label ID="lblLastChangedBy" runat="server" SkinID="label" Width="100%" Text='<%#Eval("LastChangedBy")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='Changed&nbsp;Date' HeaderStyle-Width="125px">
                                        <ItemTemplate><asp:Label ID="lblLastChangedDateUTC" runat="server" SkinID="label" Width="100%" Text='<%#Eval("LastChangedDateUTC")%>' /></ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                                                
                        </div>
                   </div>     
               </div>
           </div>    
                        
             
                    
            <div class="AuditTrailDiv">
                <div class="container-fluid">
                
                    <div class="row">   
                    
                        <div class="col-md-12">       
                            <asp:TextBox ID="txtAccountNumber" CssClass="Textbox" Style="visibility: hidden;" runat="server" />
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" /></Windows>
                            </telerik:RadWindowManager>
                      </div>  
                        
                  </div>
                  
             </div>           
          </div>              
                        
                        
                        
                    
    <%--        
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
