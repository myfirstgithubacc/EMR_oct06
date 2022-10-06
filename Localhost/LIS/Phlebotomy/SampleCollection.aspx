<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SampleCollection.aspx.cs"
    Inherits="LIS_Phlebotomy_SampleCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sample Collection Details</title>

    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainNew.css" rel="stylesheet" type="text/css" />


    <link href="../../Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <style>
        .header_main {
            background: #C1E5EF;
            padding: 0px 0px;
        }
        .rwControlButtons {
                width: -1px !important;
        }

    </style>
</head>
    
<body>
 <script type="text/javascript" language="javascript">
function OnClientIsValidPasswordClose(oWnd, args) {

                var arg = args.get_argument();
                if (arg) {
                    var IsValidPassword = arg.IsValidPassword;                  

                    $get('<%=hdnIsValidPassword.ClientID%>').value = IsValidPassword;                    
                } 
                $get('<%=btnIsValidPasswordClose.ClientID%>').click();
            }


            function returnToParent() {
                var oArg = new Object();
                oArg.IsValidPassword = document.getElementById("hdnIsValidPassword").value;
                oArg.DailySerialNo = document.getElementById("hdnDailySerialNo").value;

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function backToParent() {
                var oArg = new Object();
                oArg.IsValidPassword = document.getElementById("hdnIsValidPassword").value;
                oArg.SelectedUserId = document.getElementById("hdnDailySerialNo").value;                
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
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>


        <div class="container-fluid header_main form-group">
            <div class="col-xs-12 col-md-12"><h2 style="color:#333;"><asp:Label ID="lblPatientDetails" runat="server" /></h2></div>
        </div>

        <div class="container-fluid" id="Table1" runat="server">
            <div class="row form-group">
                <div class="col-xs-12 text-center"><asp:Label ID="lblMessage" runat="server" Text="&nbsp;" /></div>
            </div>

            <div class="row form-group">

                <div class="col-xs-12 col-md-6">
                    <div class="row form-group">
                        <div class="col-xs-4 col-md-3">
                            <asp:Label ID="Label2" runat="server" Text="Sample&nbsp;Collected&nbsp;By" />
                            <span id="Span1" style='color: Red' runat="server">*</span>&nbsp;:
                        </div>
                        <div class="col-xs-8 col-md-9"><telerik:RadComboBox ID="ddlCollectedBy" runat="server" Width="100%"
                                EmptyMessage="[ Select ]" MarkFirstMatch="true" Filter="Contains"  /></div>
                    </div>
                </div>

                <div class="col-xs-12 col-md-6">
                    <div class="row form-group">
                        <div class="col-xs-4 col-md-3">
                            <asp:Label ID="Label5" runat="server" Text="Collection Date" />
                            <span id="Span3" style='color: Red' runat="server">*</span>&nbsp;:
                        </div>
                        <div class="col-xs-8 col-md-9">
                            <asp:UpdatePanel ID="UpdatePanelDate" runat="server">
                                <ContentTemplate>
                                    <telerik:RadDateTimePicker ID="txtDate" runat="server" Width="170px" DateInput-DateFormat="dd/MM/yyyy hh:mm tt"
                                        AutoPostBackControl="Both" Calendar-Height="150px" Calendar-Width="150px" Enabled="false" />
                                    <telerik:RadComboBox ID="RadComboBox1" runat="server" AutoPostBack="True" Height="170px"
                                        MarkFirstMatch="true" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"
                                        Skin="Outlook" Width="50px" Enabled="false" />
                                    &nbsp;<asp:Literal ID="ltDateTime" runat="server" Text="HH   MM" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                             <telerik:RadWindowManager ID="RadWindowManager1" EnableViewState="false" runat="server"
                                        Behaviors="Close,Move,Pin,Resize,Maximize">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                             <asp:HiddenField ID="hdnIsValidPassword" runat="server" />
                              <asp:HiddenField ID="hdnDailySerialNo" runat="server" />
                        </div>
                    </div>
                </div>
                <div style="display: block;" runat="server" id="divVacutainer">
                <div class="col-xs-12 col-md-12" >
                    <div style="align-items:center;height:10px;margin-top: -15px;"><hr /></div>
                    <div class="row form-group">
                        <div class="col-xs-6 col-md-6" align="right">
                            <telerik:RadComboBox ID="rcbVacutainer" Width="240px" runat="server" EmptyMessage="[ Select Vacutainer ]" MarkFirstMatch="true" Filter="Contains"  />
                            <%--<asp:Label ID="lblVacutainerQuantity" runat="server" Text="Quantity Vacutainers" />--%>
                        </div>
                       <div class="col-xs-6 col-md-6" align="left">
                            <asp:TextBox ID="txtVacutainerQuantity" runat="server" Width="100px" Text="1" TextMode="Number"></asp:TextBox>
                            <asp:Button ID="btnAddVacutainer" runat="server" ToolTip="Save" OnClick="btnAddVacutainer_OnClick" CssClass="btn btn-primary" Text="Add" />
                        </div>
                    </div>
                </div>
                
                 <div class="col-xs-12 col-md-12" >
                <div class="row form-group">
                    <div class="col-xs-12 col-md-12">
                        <asp:GridView ID="gvVacutainer" runat="server" Width="400px" CssClass="Grid" AutoGenerateColumns="false" EmptyDataText="No records has been added.">
                            <Columns>
                                <asp:BoundField DataField="VacutainerId" Visible="false" />
                                <asp:BoundField DataField="VacutainerName" HeaderText="Vacutainer Name" ItemStyle-Width="200" HeaderStyle-CssClass="header_main" />
                                <asp:BoundField DataField="VacutainerQuantity" HeaderText="Quantity" ItemStyle-Width="60" HeaderStyle-CssClass="header_main" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
                </div>
                <div class="col-xs-12 col-md-12" style="display:block;" runat="server" id="divPregnant">
                    <div class="row form-group">
                        <div class="col-xs-3 col-md-3">
                            <asp:Label ID="LblPregnant" runat="server" Text="Pregnant"/>
                            <span id="Span4" style='color: Red' runat="server">*</span>
                        </div>
                        <div class="col-xs-9 col-md-9" align="left">
                          <asp:DropDownList ID="rbdPregnant" runat="server" Width="25%">   
                            <asp:ListItem Text="----Select----" Value=""></asp:ListItem>   
                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem> 
                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                           </asp:DropDownList>     
                        </div>
                    </div>
                </div>
                 
                <div class="col-xs-12 col-md-12">
                    <div class="row form-group">
                        <div class="col-xs-12 col-md-12 text-center">
                            <div style="align-items:center;height:8px;margin-top: -15px;"><hr /></div>
                            <asp:Button ID="btnSaveData" runat="server" ToolTip="Save" OnClick="btnSaveData_OnClick" CssClass="btn btn-primary" Text="Save" />
                            <asp:Button ID="btnclose" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-default" OnClientClick="window.close();" />
                            <asp:Button ID="btnIsValidPasswordClose" runat="server" CausesValidation="false" Style="visibility: hidden;" OnClick="btnIsValidPasswordClose_OnClick" Width="1px" />
                        </div>
                    </div>
                </div>

                
            </div>
            

        </div>


    </form>
</body>
</html>
