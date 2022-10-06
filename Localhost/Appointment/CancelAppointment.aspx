<%@ Page Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="CancelAppointment.aspx.cs" Inherits="Appointment_CancelAppointment"  %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet">
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    

<script type="text/javascript">
        function SelectAll(id, GridCtrl) {
            //get reference of GridView control
            var grid = document.getElementById(GridCtrl);
            //variable to contain the cell of the grid
            var cell;

            if (grid.rows.length > 0) {
                //loop starts from 1. rows[0] points to the header.
                for (i = 1; i < grid.rows.length; i++) {
                    //get the reference of first column
                    cell = grid.rows[i].cells[1];

                    //loop according to the number of childNodes in the cell
                    for (j = 0; j < cell.childNodes.length; j++) {
                        //if childNode type is CheckBox                 
                        if (cell.childNodes[j].type == "checkbox") {
                            //assign the status of the Select All checkbox to the cell checkbox within the grid
                            cell.childNodes[j].checked = document.getElementById(id).checked;
                        }
                    }
                }
            }
        }
    </script>
        
        
        
       
       
       
        
        
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            
            
                <!-- Cancel/Move Appointment Part -->
   	            <div class="ALPTop">
    	            <div class="container-fluid">
    		            <div class="row">
                
                            <div class="col-md-12 col-xs-12 features02">
              		            <div class="ListDetailsText-TopRight">
              		                <span>Cancel/Move Appointment</span>
              		                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                           </div>     
                
                        </div>
                   </div>     
                </div>
                            
                            
                            
                            
            
          
               <!--Filler Part -->
                <div class="AppointmentWhite">
                    
                    
                    <div class="row ALP-Spacing">
                        
                        <div class="col-md-5">
                            <div class="cancelDiv">
                                <h3>Facility</h3>
                                <telerik:RadComboBox ID="ddlFacility" runat="server" AllowCustomText="False" Skin="Outlook" MarkFirstMatch="False"></telerik:RadComboBox>
                            </div>
                        </div>
            
                        <div class="col-md-3">
                            <div class="cancelDiv">
                                <asp:Label ID="Label1" runat="server" Text="Options"></asp:Label>
                                <telerik:RadComboBox ID="ddlOptions" runat="server" Skin="Outlook" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlOptions_OnSelectedIndexChanged">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Select" Value="" />
                                        <telerik:RadComboBoxItem Text="Cancel" Value="C" />
                                        <telerik:RadComboBoxItem Text="Move To" Value="M" />
                                    </Items>
                                </telerik:RadComboBox>
                            </div>
                        </div>
                        
                        
                         <div class="col-md-4">
                            <div class="cancelDiv" id="tdMovePro" runat="server">
                                <h5>Move Doctor :<span style="color: Red">*</span></h5>
                                <telerik:RadComboBox ID="ddlMoveProvider" runat="server" Skin="Outlook" MarkFirstMatch="true" AllowCustomText="true" Filter="Contains" ></telerik:RadComboBox>
                            </div>
                        </div>
            
                    </div>
                    
                    
                    
                    
                    
                    <div class="row ALP-Spacing">
                        <div class="col-md-5">
                            <div class="cancelDiv">
                                <asp:Label ID="lblProvider" runat="server" Text="Appointment Doctor"></asp:Label>
                                <telerik:RadComboBox ID="ddlProvider" runat="server" Skin="Outlook" MarkFirstMatch="true" AllowCustomText="true" Filter="Contains"></telerik:RadComboBox>
                            </div>
                        </div>
                    
                        <div class="col-md-3">
                            <div class="cancelDiv">
                                 <h2>Remarks</h2>
                                 <asp:TextBox ID="txtCancelRemarks" runat="server"></asp:TextBox>
                            </div>
                       </div>     
                          
                       <div class="col-md-4"  id="tdMoveDate" cellpadding="0" cellspacing="0" runat="server">
                            <div class="cancelDiv-left">
                                <asp:CheckBox ID="chkMoveDate" runat="server" Text="Exclude Date" AutoPostBack="true" OnCheckedChanged="chkMoveDate_OnCheckedChanged" />
                            </div>   
                            <div class="cancelDiv-right">
                                <h3>Move Date</h3>
                                <telerik:RadDatePicker ID="dtpMoveDate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy" MinDate="01/01/1901"></telerik:RadDatePicker>
                            </div> 
                        </div>  
                            
                            
                    </div>
                    
                    
                    
                    
                    <div class="row ALP-Spacing">
                       
                       
                        <div class="col-md-5">
                            <div class="cancelDiv">
                                <h3>From Date</h3>
                                <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy" MinDate="01/01/1901"></telerik:RadDatePicker>
                                <h4>To</h4>
                                <telerik:RadDatePicker ID="dtpToDate" runat="server" Width="100px" DateInput-DateFormat="dd/MM/yyyy" MinDate="01/01/1901"></telerik:RadDatePicker>
                            </div>
                        </div>
                        
                        
                        <div class="col-md-7">
                            <div class="cancelDiv01">
                                <h3><asp:Button ID="btnRefresh" CssClass="AppointmentNewBtn" runat="server" Text="Refresh" OnClick="btnRefresh_Click" /></h3>
                                <h3>
                                    <asp:RadioButtonList ID="rdoSend" runat="server" RepeatDirection="Horizontal" Width="200px"  Visible="false" >
                                        <asp:ListItem Text="None" Value="NN" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Send SMS" Value="SS"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </h3>
                                <h3><asp:Button ID="btnUpdate" CssClass="AppointmentNewBtn" runat="server" Text="Update Patient Info" OnClick="btnUpdate_Click" /></h3>
                                <h3><asp:Button ID="btnCancelMove" CssClass="AppointmentNewBtn" runat="server" Text="Cancel/Move Appointment" OnClick="btnCancelMove_Click" /></h3>
                            </div>
                        </div>
                        
                        
                    </div>      
                        
                    
                    
                    
                    
                </div>    
                    
            
            
            
            
            
         
            
            
            
            
            
            
                <div class="GeneralDiv">
    	            <div class="container-fluid">
                        <div class="row">  
                        
                            <div class="col-md-12"> 
                            
                                <table id="Table1" cellpadding="0" cellspacing="0" runat="server" border="0" class="tableRecurrence">
                                    <tr>
                                        <td>

                                            <asp:GridView ID="gvAppointment" runat="server" SkinID="gridview" Width="100%" GridLines="None" AutoGenerateColumns="false" OnRowDataBound="gvAppointment_RowDataBound">
                                                <Columns>
                                                   
                                                    <asp:TemplateField>
                                                        <ItemTemplate><asp:Label ID="lblAppointmentID" runat="server" Text='<%#Eval("AppointmentID")%>'></asp:Label></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField Visible="True" HeaderText="SNo">
                                                        <ItemTemplate><%# Container.DataItemIndex+1 %></ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-Width="20px">
                                                        <HeaderTemplate><asp:CheckBox ID="chkAllItems" runat="server"></asp:CheckBox></HeaderTemplate>
                                                        <ItemTemplate><asp:CheckBox ID="chkT" runat="server"></asp:CheckBox></ItemTemplate>
                                                        <HeaderStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Regno%>' HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label>
                                                            <asp:HiddenField ID="hdnStatusColor" runat="server" Value='<%#Eval("StatusColor")%>'></asp:HiddenField>
                                                            <asp:HiddenField ID="hdnStatusCode" runat="server" Value='<%#Eval("StatusCode")%>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Patient Name" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                                        <ItemTemplate><asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("Name")%>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Age/Gender" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                        <ItemTemplate><asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Appt. Date" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                        <ItemTemplate><asp:Label ID="lblAppointmentDate" runat="server" Text='<%# Eval("DateOfAppointment")%>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Start&nbsp;Time" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate><asp:Label ID="lblStartTime" runat="server" Text='<%#Eval("FromTime")%>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="End&nbsp;Time" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate><asp:Label ID="lblEndTime" runat="server" Text='<%#Eval("ToTime")%>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile No." HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                        <ItemTemplate><asp:TextBox ID="txtMobile" runat="server" SkinID="textbox" Text='<%#Eval("Mobile")%>'></asp:TextBox></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Email" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                        <ItemTemplate><asp:TextBox ID="txtEmail" runat="server" Width="150px" SkinID="textbox" Text='<%#Eval("Email")%>'></asp:TextBox></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Remarks" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                        <ItemTemplate><asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label></ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    
                                                </Columns>
                                            </asp:GridView>

                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td><asp:Label ID="lblLegendNote" runat="server" SkinID="label" Text="Only UnConfirmed/Confirmed Appointment Can be Move or Cancel" ForeColor="DarkBlue" Font-Bold="true"></asp:Label></td>
                                    </tr>
                                    
                                    
                                </table>
                                
                            </div>            
                    
                        </div>            
                    </div>
                </div>
                
                
                
                
                
              
                
                                                
            

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>