<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OralGiven.aspx.cs"  Inherits="WardManagement_OralGiven"%>
       

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
 <script language="javascript" type="text/javascript" src="../Include/JS/Common.js"></script>

    <script type="text/javascript">
            function showMenu(e, menu) {
                var menu = $find(menu);
                menu.show(e);
            }

            
        </script>

<html xmlns="http://www.w3.org/1999/xhtml">
   
<head id="Head1" runat="server">
    <title>Oral Given Details</title>
    <link href="../../Include/bootstrap4/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Include/css/emr.css" rel='stylesheet' type='text/css' />
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css' />
    <link href="../Include/EMRStyle.css" rel='stylesheet' type='text/css' />

    <style type="text/css">
            .GridView_Office2007 .rgHeader, .GridView_Office2007 th.rgResizeCol {
                border: solid #868686 1px !important;
                border-top: none !important;
                border-left: none !important;
                outline: none !important;
                color: #333;
                background: 0 -2300px repeat-x #c1e5ef !important;
            }

            .GridView_Office2007 td.rgGroupCol, GridView_Office2007 td.rgExpandCol {
                background-color: #fff !important;
            }

            #ctl00_ContentPlaceHolder1_Panel1 {
                background-color: #c1e5ef !important;
            }

            .GridView .rgFilterBox {
                height: 20px !important;
            }

            .GridView_Office2007 .rgFilterRow {
                background: #c1e5ef !important;
            }

            .GridView_Office2007 .rgPager {
                background: #c1e5ef 0 -7000px repeat-x !important;
                color: #00156e !important;
            }

        #ctl00_ContentPlaceHolder1_gvWardDtl_GridHeader {
            margin: 0 !important;
        }

        tr.clsGridheaderorderNew th {
            color: #fff !important;
            background: #337ab7 !important;
            padding: 6px 10px !important;
            white-space: nowrap !important;
        }
        tr.clsGridRoworderNew{
             padding: 6px 10px !important;
        }
        div#pnlgrid{
            overflow-x:auto!important;
        }
    </style>



</head>
<body>
    <form id="form1" runat="server" style="overflow: hidden;">
        <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>


            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
               
                    


                <div class="container-fluid header_main form-group">
                    <div class="row">
                        <div class="col-md-3 col-4">
                            <h2 style="color: #333;">
                                <asp:Label ID="Label8" runat="server" Text="Oral Given Time" /></h2>
                        </div>
                        <div class="col-md-6 col-8 text-center">
                            <asp:Label ID="lblMessage" runat="server" SkinID="label" Text="&nbsp;" />
                        </div>
                        <div class="col-md-3 col-12 text-right">

                            <asp:Button ID="btnCloseW" Text="Close" Visible="true" runat="server" ToolTip="Close" CssClass="btn btn-primary" OnClientClick="window.close();" />
                            <%-- <asp:Button ID="btnRefresh" Text="Search" Visible="true" runat="server" ToolTip="Refresh" OnClick="btnSearch_Click" CssClass="btn btn-default" />--%>
                        </div>
                    </div>
                </div>




                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="row">
                                <div class="col-lg-3 col-sm-4 col-6">
                                    <div class="row">
                                        <div class="col-4">
                                            <asp:Label ID="lblLocation" runat="server" Text="Facility" />
                                        </div>
                                        <div class="col-8">
                                            <telerik:RadComboBox ID="ddlLocation" runat="server" Width="100%" AutoPostBack="false" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-sm-4 col-6">
                                    <div class="row">
                                        <div class="col-6">
                                            <telerik:RadComboBox ID="ddlSearchCriteria" Width="100%" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlSearchCriteria_SelectedIndexChanged">
                                                <Items>
                                                    <%-- <telerik:RadComboBoxItem Text="Ward" Value="W" Selected="true" />--%>
                                                    <telerik:RadComboBoxItem Text='<%$ Resources:PRegistration, UHID%>' Value="R" />
                                                    <telerik:RadComboBoxItem Text="Encounter No" Value="ENC" />
                                                    <telerik:RadComboBoxItem Text="Patient Name" Value="P" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </div>
                                        <div class="col-6">
                                            <asp:TextBox ID="txtSearchRegNo" runat="server" Text="" Width="100%" MaxLength="10" onkeyup="return validateMaxLength();" Visible="false" />
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom" TargetControlID="txtSearchRegNo" ValidChars="0123456789" />
                                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="30" Width="100%" Visible="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-sm-4 col-6">
                                    <div class="row">
                                        <div class="col-4">
                                            <asp:Label ID="Label1" runat="server" Text="Ward" />
                                        </div>
                                        <div class="col-8">
                                            <telerik:RadComboBox ID="ddlWard" runat="server" Width="100%" Height="300px" DropDownWidth="250px" EmptyMessage="[ Select ]" Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlWard_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container-fluid">
                    <div class="row form-group">
                        <div class="col-md-12">
                            <asp:Label ID="lblGridStatus" runat="server" Font-Bold="true" />
                        </div>
                    </div>

                    <div class="row form-group">
                        <div class="col-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlgrid" runat="server" Height="480px" Width="100%"
                                       >
                                        <asp:GridView ID="gvOralGivenList" runat="server" AutoGenerateColumns="False" Width="100%"
                                            Height="100%" SkinID="gridviewOrderNew" AllowPaging="True" PageSize="25" Skin="Office2007">
                                            <Columns>
                                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno%>' HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblregistrationno" runat="server" Text='<%#Eval("registrationno")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText='Encounter No.' HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Patient Name" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Age/Gender" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAgeGender" runat="server" Text='<%#Eval("AgeGender")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Bed No." HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbedno" runat="server" Text='<%#Eval("Bedno")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ward" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWardName" runat="server" Text='<%#Eval("WardName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField HeaderText="Service Name" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                                                                                 
                                                                                                                                       
                                           
                                                <asp:TemplateField HeaderText="Appointment Date" HeaderStyle-Width="40px"  ItemStyle-Width="40px" >
                                                    <ItemTemplate>
                                                        <asp:Label ID ="lblAppointmentDate" runat="server" Text='<%#Eval("AppointmentDate")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Time" HeaderStyle-Width="40px"  ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID ="lblShowTime" runat="server" Text='<%#Eval("ShowTime")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Oral Given Time" HeaderStyle-Width="40px"  ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:Label ID ="lblOralGivenTime" runat="server" Text='<%#Eval("OralGivenTime")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>                                   
                                    <asp:AsyncPostBackTrigger ControlID="gvOralGivenList" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                      
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
         </form>
      </body>
 </html>

