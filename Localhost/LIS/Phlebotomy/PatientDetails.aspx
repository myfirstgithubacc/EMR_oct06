<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatientDetails.aspx.cs" Inherits="LIS_Phlebotomy_PatientDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patient Details</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />


    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <div class="container-fluid header_main form-group">
            <div class="col-sm-6"><asp:Label ID="lblPatientDetails" runat="server"></asp:Label></div>
            <div class="col-sm-6 text-right"><asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" /></div>
        </div>


        <div class="container-fluid">

            <div class="row">
                <div class="col-sm-6">
                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblFacility" runat="server" Text="Facility"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtFacility" runat="server" Width="100%"  ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblDob" runat="server" Text="DOB"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtDob" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblGender" runat="server" Text="Gender"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtGender" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>


                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblMartialStatus" runat="server" Text="Martial Status"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtMartialStatus" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblPhoneNo" runat="server" Text="Phone No"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtPhoneNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblMobileNo" runat="server" Text="Mobile No"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtMobileNo" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblNationality" runat="server" Text="Nationality"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtnationality" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblPayerName" runat="server" Text="Payer Name"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtPayername" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    
                </div>




                <div class="col-sm-6">
                    <div class="row form-groupTop02">
                        <div class="col-sm-3 PaddingRightSpacing"><asp:Label ID="lblLocalAddress1" runat="server" Text="Local Address1"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtlocalAddress1" runat="server" Width="100%" Height="47px" ReadOnly="true" TextMode="MultiLine"></asp:TextBox></div>
                    </div>

                   
                    <div class="row form-groupTop02">
                        <div class="col-sm-3 PaddingRightSpacing"><asp:Label ID="lblLocalAddress2" runat="server" Text="Local Address2"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtlocalAddress2" runat="server" Width="100%" Height="47px" ReadOnly="true" TextMode="MultiLine"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblPin" runat="server" Text="Pin"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtPin" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblCity" runat="server" Text="City"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtCity" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>                                
                    </div>

                    <div class="row form-groupTop02">
                        <div class="col-sm-3"><asp:Label ID="lblState" runat="server" Text="Country"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtState" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>                                
                    </div>

                    
                    <div class="row form-groupTop02">
                        <div class="col-sm-3 PaddingRightSpacing"><asp:Label ID="lblCompanyName" runat="server" Text="Company Name"></asp:Label></div>
                        <div class="col-sm-9"><asp:TextBox ID="txtCompanyName" runat="server" Width="100%" ReadOnly="true"></asp:TextBox></div>
                    </div>

                </div>


            </div>


        </div>





       
    </div>
    </form>
</body>
</html>
