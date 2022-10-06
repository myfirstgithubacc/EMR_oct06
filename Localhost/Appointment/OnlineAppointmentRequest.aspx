<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OnlineAppointmentRequest.aspx.cs"
    Inherits="Appointment_OnlineAppointmentRequest" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="AJAX" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
 
    <title>Online Appointment Details</title>

    <link href="../Include/css/open-sans.css" rel="stylesheet" runat="server" />
   <link href="../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/font-awesome.min.css" rel="stylesheet" runat="server" />
    <link href="../Include/css/mainStyle.css" rel="stylesheet"/>
    <link href="../Include/css/Appointment.css" rel="stylesheet" type="text/css" />
    <link href="../Include/css/emr_new.css" rel="stylesheet" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />    
    <style type="text/css">
         td.rcbInputCell.rcbInputCellLeft .rcbInput {
            padding: 2px 8px !important;
        }
         span{
             padding:0px!important;
       
         }
         div#ddlProvider{
             margin:0px!important;
         }
         span#dtpToDate_dateInput_display{
             padding:3px 8px !important;
         }
         span#dtpFromDate_dateInput_display{
               padding:3px 8px !important;
         }
       
    </style>
    <script type="text/javascript">

        function returnToParent() {
            //create the argument that will be returned to the parent page
            var oArg = new Object();
            oArg.RegistrationId = document.getElementById("hdnRegistrationId").value;
            oArg.RegistrationNo = document.getElementById("hdnRegistrationNo").value
            oArg.PPAppointmentId = document.getElementById("hdnPPAppointmentId").value;

            var oWnd = GetRadWindow();
            oWnd.close(oArg);
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }        
    </script>

</head>
<body>
    <form id="form1" runat="server" style="overflow-x:hidden;">
    <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
    
    
    <div>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
               

                <div class="ALPTop01">
    	            <div class="container-fluid header_main">
    		            <div class="row">
                
                            <div class="col-md-12 col-xs-12 features02 text-right">
                                <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                               
                                <asp:Button ID="btnfilter" Text="Filter" runat="server" CssClass="btn btn-primary" OnClick="btnfilter_OnClick" />
                                 <asp:Button ID="btnCloseW" Text="Close" CssClass="btn btn-primary" runat="server" ToolTip="Close" OnClientClick="window.close();" />
                            </div>
                        </div>
                    </div>
                </div>

                 <div class="AppointmentWhite">
                    <div class="row ALP-Spacing">               
                    
                        <div class="col-md-3 col-6">
                            <div class="row">
                                <div class="col-6">
                                     <asp:Label ID="Label17" runat="server" CssClass="FromDateText" Text="From&nbsp;Date" /> 
                                    <telerik:RadDatePicker ID="dtpFromDate" runat="server" Width="100%" />
                                </div>
                                <div class="col-6">
                                      <asp:Label ID="Label18" runat="server" CssClass="ToDateText" Text="To&nbsp;Date" />
                            <telerik:RadDatePicker ID="dtpToDate" runat="server" MinDate="01/01/1900" Width="100%" />
                                </div>
                            </div>
                        </div>
                        
                        
                        <div class="col-md-3 col-6">
                            <asp:Label ID="Label19" runat="server" CssClass="ToDateText" Text="Provider" />
                            <telerik:RadComboBox ID="ddlProvider" Filter="Contains" runat="server" ShowMoreResultsBox="true" Width="100%"  AppendDataBoundItems="true"></telerik:RadComboBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlProvider" Display="None" ErrorMessage="Select Provider" ValidationGroup="Save" />
                            </div>
                            <div class="col-md-3 col-6">
                            <asp:Label ID="Label1" runat="server" CssClass="ToDateText" Text="To&nbsp;Date" />
                            <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="100%">
                                <Items>
                                    <telerik:RadComboBoxItem Text="All" Value="2" />
                                    <telerik:RadComboBoxItem Text="Confirm" Value="1" BackColor="#F1DCFF" />
                                    <telerik:RadComboBoxItem Text="UnConfirm" Value="0" />
                                </Items>
                            </telerik:RadComboBox>
                        </div>
                        
                         <div class="col-md-3 col-6" style="align-self:center;">
                            <span class="ToDateText pr-3" >Legends</span>
                            <asp:TextBox ID="txtConfirm" runat="server" Text="" ReadOnly="true" />
                            <span class="ToDateText pl-3">Confirm</span>
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
                                            <asp:Panel ID="pnlgrid" runat="server" Width="99%" BorderWidth="1" BorderColor="SkyBlue" ScrollBars="Auto">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <telerik:RadGrid ID="gvEncounter" runat="server" Skin="Office2007" BorderWidth="0"
                                                            PagerStyle-ShowPagerText="true" AllowFilteringByColumn="false" AllowMultiRowSelection="false"
                                                            Width="100%" AutoGenerateColumns="False" ShowStatusBar="true" EnableLinqExpressions="false"
                                                            GridLines="Both" AllowPaging="true" OnItemDataBound="gvEncounter_OnItemDataBound"
                                                            AllowAutomaticDeletes="false" Height="99%" ShowFooter="true" AllowSorting="false"
                                                            OnItemCommand="gvEncounter_OnItemCommand" OnPageIndexChanged="gvEncounter_OnPageIndexChanged"
                                                            PageSize="20">
                                                        
                                                            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="true">
                                                                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                                                                <Resizing AllowRowResize="false" EnableRealTimeResize="True" ResizeGridOnColumnResize="True"
                                                                    AllowColumnResize="false" />
                                                            </ClientSettings>
                                                        
                                                            <PagerStyle ShowPagerText="true" />
                                                            
                                                            <MasterTableView TableLayout="Auto" GroupLoadMode="Client" AllowFilteringByColumn="false" Width="100%">
                                                                <NoRecordsTemplate>
                                                                    <div class="NoRecordText red">No Record Found.</div>
                                                                </NoRecordsTemplate>
                                                                
                                                                <ItemStyle Wrap="true" />
                                                                
                                                                <Columns>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="Select" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderText="Select" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                        <ItemTemplate><asp:LinkButton ID="IbtnSelect" runat="server" Text="Select" CommandName="Select" /></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="RegistrationNo" HeaderText='<%$ Resources:PRegistration, regno%>' ShowFilterIcon="false" DefaultInsertValue="" DataField="RegistrationNo" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                        <ItemTemplate><asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="AppointmentRequestDate" HeaderText="Request Date" ShowFilterIcon="false" DefaultInsertValue="" DataField="AppointmentDate" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                        <ItemTemplate><asp:Label ID="lblAppointmentRequestDate" runat="server" Text='<%#Eval("AppointmentRequestDate")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="Name" HeaderText="Patient Name" ShowFilterIcon="false" DefaultInsertValue="" DataField="Name" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate><asp:Label ID="lblName" runat="server" Text='<%#Eval("Name")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="GenderAge" HeaderText="Gender/Age" ShowFilterIcon="false" DefaultInsertValue="" DataField="GenderAge" SortExpression="GenderAge" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                        <ItemTemplate><asp:Label ID="lblGenderAge" runat="server" Text='<%#Eval("GenderAge")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="MobileNo" HeaderText="MobileNo" ShowFilterIcon="false" DefaultInsertValue="" DataField="MobileNo" SortExpression="MobileNo" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                                                        <ItemTemplate><asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="DoctorName" HeaderText="Doctor" ShowFilterIcon="false"
                                                                        DefaultInsertValue="" DataField="DoctorName" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="FromTime" HeaderText="From Time" ShowFilterIcon="false" DefaultInsertValue="" DataField="FromTime" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                                        <ItemTemplate><asp:Label ID="lblFromTime" runat="server" Text='<%#Eval("FromTime")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="AppointmentDate" HeaderText="Appointment Date" ShowFilterIcon="false" DefaultInsertValue="" DataField="AppointmentDate" SortExpression="AppointmentDate" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                                                        <ItemTemplate><asp:Label ID="lblAppointmentDate" runat="server" Text='<%#Eval("AppointmentDate")%>'></asp:Label></ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                   
                                                                    <telerik:GridTemplateColumn UniqueName="DOB" HeaderText="DOB" ShowFilterIcon="false"
                                                                        Visible="false" DefaultInsertValue="" DataField="DOB" SortExpression="RegistrationNo">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDOB" runat="server" Text='<%#Eval("DOB")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                    <telerik:GridTemplateColumn UniqueName="REGID" HeaderText="REGID" Visible="false" ShowFilterIcon="false" DefaultInsertValue="" DataField="REGID">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblREGID" runat="server" Text='<%#Eval("REGID")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hdnPPAppointmentId" runat="server" Value='<%#Eval("PPAppointmentId")%>' />
                                                                            <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                                                            <asp:HiddenField ID="hdnAppointmentConfirmed" runat="server" Value='<%#Eval("AppointmentConfirmed")%>' />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                    
                                                                </Columns>
                                                                
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                            
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdnRegistrationNo" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnRegistrationId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnPPAppointmentId" runat="server" />
                                        </td>
                                    </tr>
                                    
                                </table>
                        
                            </div>
                        
                        </div>                  
                    </div>
                </div>
                
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
