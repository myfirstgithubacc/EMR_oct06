<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientDetails.aspx.cs" Inherits="Appointment_PatientDetails" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appointment</title>
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />    
    <link href="../Include/css/mainStyle.css" rel="stylesheet">
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />    
    
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    
    

    
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
       
            
            <div class="ALPTop01">
    	        <div class="container-fluid">
    		        <div class="row">
                
                        <div class="col-md-12 col-xs-12 features02">
                            <div class="ListDetailsText-TopRight">
                                <asp:Label ID="PatientStatus" runat="server" Text=""></asp:Label>
                                <asp:Button ID="btnCloseW" Text="Close" CssClass="AppointBtnRight" runat="server" ToolTip="Close" OnClientClick="window.close();" />
                            </div>
                        </div>    
                                            
                    </div>    
                </div>                        
            </div>                            
                            
            
            <div class="AppointmentWhite">
                <div class="row ALP-Spacing">            
                            
                    <div class="col-md-3">
                        <asp:Label ID="lblPatient" runat="server" CssClass="AP-FacilityText" Text="Search&nbsp;On" />
                        <telerik:RadComboBox ID="ddlName" runat="server" AppendDataBoundItems="true" Width="60px" AutoPostBack="true" OnSelectedIndexChanged="ddlName_OnTextChanged">
                            <Items>
                                <telerik:RadComboBoxItem Text="Reg No" Value="R" />
                                <telerik:RadComboBoxItem Text="Patient Name" Value="N" />
                                <telerik:RadComboBoxItem Text="Enrolle No." Value="EN" />
                                <telerik:RadComboBoxItem Text="Mobile No." Value="MN" />
                            </Items>
                        </telerik:RadComboBox>
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnRefresh" CssClass="findPatientInput-Mobile">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="findPatientInput-Mobile" MaxLength="50" Visible="false" />
                            <asp:TextBox ID="txtSearchN" CssClass="findPatientInput-Mobile" Height="23px" runat="server" MaxLength="10" onkeyup="return validateMaxLength();" />
                            <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchN" ValidChars="0123456789" />
                        </asp:Panel>
                    </div>   
                    
                    
                   <div class="col-md-3">
                        <asp:Label ID="lblProvider" runat="server" CssClass="AP-FacilityText" Text='<%$ Resources:PRegistration, Doctor%>' />
                        <telerik:RadComboBox ID="ddlProvider" MarkFirstMatch="true" Filter="Contains" runat="server" Height="300px" ItemsPerRequest="10" EnableVirtualScrolling="true" TabIndex="0" AutoPostBack="false" Width="185px" DropDownWidth="300px" />
                    </div>
                    
                     <div class="col-md-3">
                        <asp:Label ID="lblAppointmentStatus" runat="server" CssClass="ToDateText" Text="Status" />
                        <telerik:RadComboBox ID="ddlAppointmentStatus" runat="server" Width="195px" AutoPostBack="false"
                            ShowMoreResultsBox="false" AppendDataBoundItems="true" Skin="Metro">
                            <Items>
                                <telerik:RadComboBoxItem Value="0" Text="All" />
                            </Items>
                        </telerik:RadComboBox>
                     </div> 
                    
                    <div class="col-md-3">
                        <asp:Button ID="btnRefresh" runat="server" CausesValidation="true" CssClass="AppointBtnLeft" OnClick="btnRefresh_OnClick" Text="Refresh" />
                        <asp:Button ID="btnResetFilter" runat="server" CausesValidation="false" CssClass="AppointBtnLeft" OnClick="btnResetFilter_OnClick" Text="Reset Filter" />
                    </div> 
                      
                </div>
                
                
               
                
           </div>     
                
                
        
        
            <div class="GeneralDiv">
                <div class="container-fluid">
                    <div class="row">
                    
                        
        
                            <table id="tblDocProfile" cellpadding="0" cellspacing="0" runat="server" border="0" class="tableRecurrence">
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvPatientDetails" runat="server" SkinID="gridviewOrderNew" AutoGenerateColumns="false" Width="100%" Height="100%" AllowPaging="True" PageSize="15" OnPageIndexChanging="gvPatientDetails_PageIndexChanging" OnRowDataBound="gvPatientDetails_RowDataBound">
                                            <Columns>
                                                
                                                    <asp:TemplateField HeaderText="S No">
                                                        <ItemTemplate><%#Container .DataItemIndex +1 %></ItemTemplate>
                                                    </asp:TemplateField>
                                                
                                                    <asp:TemplateField HeaderText="Appointment Date" HeaderStyle-Width="150px">
                                                        <ItemTemplate><asp:Label ID="LblAppointmentDate" runat="server" Text='<%#Eval("AppointmentDate")%>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="100px">
                                                        <ItemTemplate><asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="PatientName" HeaderStyle-Width="150px">
                                                        <ItemTemplate><asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="AgeGender" SortExpression="AgeGender">
                                                        <ItemTemplate><asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' /></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Doctor%>' HeaderStyle-Width="250px">
                                                        <ItemTemplate><asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider")%>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Company">
                                                        <ItemTemplate><asp:Label ID="lblCompany" runat="server" Text='<%#Eval("CompanyName")%>' /></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status")%>' />
                                                            <asp:HiddenField ID="hdnStatusColor" runat="server" Value='<%#Eval("StatusColor")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                
                                                </Columns>
                                            </asp:GridView>
                                    
                                        </td>
                                    </tr>
                                </table>
                            
                          
                    
                        </div>                        
                    </div>
                </div>        
        
    </form>
</body>
</html>