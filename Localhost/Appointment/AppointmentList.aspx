<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppointmentList.aspx.cs"
    Inherits="Appointment_AppointmentList" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet">
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />       
    
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OnClientClose(oWnd, args) {
        var arg = args.get_argument();
       }
    </script>

</head>



<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    
    
     <div class="ALPTop">
        <div class="container-fluid">
            <div class="row">
    
                <div class="col-md-12 col-xs-12 features02">
  		            <div class="AppointList">
                        <h2><asp:Label ID="lbl" runat="server" Text="Appointment For :"></asp:Label></h2>
                        <asp:DropDownList ID="ddlrange" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlrange_OnSelectedIndexChanged">
                            <asp:ListItem Text="Last 10" Value="L" Selected="True" />
                            <asp:ListItem Text="Future" Value="F" />
                            <asp:ListItem Text="All" Value="A" />
                        </asp:DropDownList>
                        
                        <span>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate><asp:Label ID="lblMessage" runat="server"></asp:Label></ContentTemplate>
                            </asp:UpdatePanel>
                        </span>
                        
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate><asp:Button ID="btnClose" CssClass="CloseBtn pull-right" runat="server" Text="Close" OnClientClick="window.close();" /></ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </div>
               </div>     
    
            </div>
       </div>     
    </div>
    
    
    
    <div class="VisitHistoryBorder">
        <div class="container-fluid">
            <div class="row">
    
                <div class="col-md-12 col-sm-12">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblPatientName" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text="DOB:" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblDob" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text="Mobile No:" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text="IP No:" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblEncounterNo" runat="server" Text="" ForeColor="#990066" Font-Bold="true"></asp:Label>
                            <asp:Label ID="Label6" runat="server" Text="Admission Date:" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblAdmissionDate" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="hdnRegId" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
   </div> 
   
   
   
   
   
   
    <div class="GeneralDiv01">
        <div class="container-fluid">
            <div class="row">
                        
    	        <div class="col-md-12"> 
                
                    <asp:GridView ID="gvAppointment" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="False" SkinID="gridview" AllowPaging="true" PageSize="20" OnRowCommand="gvAppointment_OnRowCommand" OnRowDataBound="gvAppointment_OnRowDataBound" 
                        HeaderStyle-HorizontalAlign="Right" HeaderStyle-ForeColor="#15428B"  HeaderStyle-Height="30px" 
                        HeaderStyle-Wrap="false" HeaderStyle-BackColor="#eeeeee" HeaderStyle-BorderColor="#ffffff" Font-Bold="false" HeaderStyle-BorderWidth="0" 
                        BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4"
                    >
                        <Columns>
                            <asp:TemplateField HeaderText="Appointment Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblAppDate" runat="server" Text='<%#Eval("AppointmentDate") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("Id")%>' />
                                    <asp:HiddenField ID="hdnAppointmentId" runat="server" Value='<%#Eval("AppointmentId")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Appointment Time">
                                <ItemTemplate><asp:Label ID="lblAppTime" runat="server" Text='<%#Eval("AppointmentTimeFromTo") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Appointment Day">
                                <ItemTemplate><asp:Label ID="lblAppointmentDay" runat="server" Text='<%#Eval("AppointmentDay") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Doctor Name">
                                <ItemTemplate><asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location Name">
                                <ItemTemplate><asp:Label ID="lblFacilityName" runat="server" Text='<%#Eval("FacilityName") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    <asp:HiddenField ID="hdnStatusCode" runat="server" Value='<%#Eval("StatusCode")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Booked By">
                                <ItemTemplate><asp:Label ID="lblBookedBy" runat="server" Text='<%#Eval("BookedBy") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Booked Date">
                                <ItemTemplate><asp:Label ID="lblBookedDate" runat="server" Text='<%#Eval("BookedDate") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="RecurrenceRule" HeaderText="Recurrence" />
                            <asp:TemplateField HeaderText="Print">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPrintSlip" runat="server" CommandName="Print" Text="Print"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cancel">
                                <ItemTemplate><asp:ImageButton ID="ibtnDelete" runat="server" CommandName="CancelApp" ImageUrl="~/Images/DeleteRow.png" ToolTip="Delete" Width="13px" /></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                        <Windows><telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move"></telerik:RadWindow></Windows>
                    </telerik:RadWindowManager>
                
                
                </div>
                
            </div>
        </div>
    </div>                    
    
    
    
    
    
    
    
    
    <div id="dvDelete" runat="server" visible="false" style="width: 300px; z-index: 100; border-bottom: 1px solid #000000; border-left: 1px solid #000000; background-color: White; border-right: 1px solid #000000; border-top: 1px solid #000000; position: absolute; bottom: 0; height: 150px; left: 450px; top: 300px">
        <%-- <asp:UpdatePanel ID="UpdatePanel7" runat="server"><ContentTemplate>--%>
        
        <table>
            <tr>
                <td colspan="2"><asp:Label ID="lblDeleteApp" runat="server" Text="Delete Appointment ?"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Label ID="lblDeleteEncounterMessage" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            
            <tr>
                <td><asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Reason:"></asp:Label><font color='Red'>*</font></td>
                <td><asp:DropDownList ID="ddlRemarkss" runat="server" AppendDataBoundItems="true" Width="200px" SkinID="DropDown"></asp:DropDownList></td>
            </tr>
            
            <tr>
                <td><asp:Label ID="Label1" runat="server" Text="Remarks"></asp:Label></td>
                <td><asp:TextBox ID="txtCancel" runat="server" SkinID="textbox" TextMode="MultiLine" style="max-height:70px; min-height:70px; max-width:200px; min-width:200px;"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnDeleteApp" SkinID="Button" runat="server" Text="Cancel" OnClick="btnDeleteApp_Click" />
                    <asp:Button ID="btnCancelApp" SkinID="Button" runat="server" Text="Close" OnClick="btnCancelApp_Click" />
                </td>
            </tr>
            
        </table>



    </div>
    </form>
</body>
</html>
