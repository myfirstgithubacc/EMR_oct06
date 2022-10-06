<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DoctorProfile.aspx.cs" Inherits="Appointment_DoctorProfile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    
    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet">
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" /> 
    
    
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    
    
    
    <form id="form1" runat="server">
    
         <div class="ALPTop">
            <div class="container-fluid">
	            <div class="row">
	            
	                <div class="col-md-12 col-xs-12 features02">
              		    <div class="ListDetailsText-TopRight">
	                        <span>Doctor Profile</span>
	                        <asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" OnClientClick="window.close();" />
	                   </div>     
	                </div>
	                    
	            </div>
            </div>
        </div>
	      
      
	    <div class="DoctorProfile">
            <table class="table table-bordered" id="tblDocProfile" runat="server">
                
                <tbody>
                    <tr>
                        <td class="DoctorFirstTD">Name</td>
                        <td Class="DoctorSecondTD"><asp:Label ID="lblDoctorname" runat="server" Width="175px"></asp:Label></td>
                        <td class="DoctorFirstTD">Qualification</td>
                        <td Class="DoctorSecondTD"><asp:Label ID="lblQualification" runat="server" Width="175px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="DoctorFirstTD">Designation</td>
                        <td CssClass="DoctorSecondTD"><asp:Label ID="lblDesignation" runat="server" Width="175px"></asp:Label></td>
                        <td class="DoctorFirstTD">Contact No</td>
                        <td CssClass="DoctorSecondTD"><asp:Label ID="lblContactNo" runat="server" Width="175px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="DoctorFirstTD">Email</td>
                        <td CssClass="DoctorSecondTD"><asp:Label ID="lblEmail" runat="server" Width="175px"></asp:Label></td>
                    </tr>
                </tbody>
                
            </table>
        </div>

    </form>
    
    
    
</body>
</html>
