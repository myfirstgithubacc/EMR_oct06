<%@ Page Language="C#" AutoEventWireup="true" CodeFile="POCServiceDetails.aspx.cs" Inherits="EMRBILLING_Popup_Servicedetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>POC Service Details</title>
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" />
    <style>
       
        * { font-family: Arial;}
        .patient-info { padding: 5px 0;}
        .table { font-size: 12px;}
        .table:focus { outline: 0;}

    </style>
     
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="Scrpit1" runat="server">
        </asp:ScriptManager>
         
        <telerik:RadFormDecorator ID="RadFormDecorator2" DecoratedControls="All" runat="server"
            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
        <div id="dvZone1" class="container-fluid">

            <div class="row">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <div class="row bg-info patient-info">
                <div class="col-md-8">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblinfoPatientName" runat="server" Text="Patient:" SkinID="label"
                                Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblPatientName" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                Font-Bold="true"></asp:Label>
                            <asp:Label ID="Label5" runat="server" Text="DOB:" SkinID="label" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblDob" runat="server" Text="" SkinID="label"></asp:Label>
                            <asp:Label ID="Label4" runat="server" Text="Mobile No:" SkinID="label" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblMobile" runat="server" Text="" SkinID="label"></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text="IP No:" SkinID="label" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblEncounterNo" runat="server" Text="" SkinID="label" ForeColor="#990066"
                                Font-Bold="true"></asp:Label>
                            <asp:Label ID="Label6" runat="server" Text="Admission Date:" SkinID="label" Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblAdmissionDate" runat="server" Text="" SkinID="label"></asp:Label>
                            <asp:HiddenField ID="hdnRegId" runat="server" />
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div style="display:none;"><asp:Button runat="server" ID="btnRefreshServices" Text="" OnClick="btnRefresh_Click"  /></div>
                <div class="col-md-4 text-right">
                    <asp:Button ID="btnUpdateToLIS" runat="server" class="btn btn-xs btn-primary" OnClick="btnUpdateToLIS_Click" Text="Machine Result" />
                    
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" style="display: inline-block;">
                        <ContentTemplate>
                           
                            <asp:Button ID="btnClose" CssClass="btn btn-xs btn-primary" runat="server" Text="Close" OnClientClick="window.close();" />
                        </ContentTemplate>
                        
                    </asp:UpdatePanel>

                </div>
            </div>

            <div class="row">


                <div class="col-md-12" style="display: none;">
                    <asp:UpdatePanel ID="updatepanel6" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <telerik:RadWindowManager ID="RadWindowManager" EnableViewState="false" runat="server">
                                <Windows>
                                    <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Close,Move" />
                                </Windows>
                            </telerik:RadWindowManager>
                            
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRefreshServices" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>



                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvService" TabIndex="3" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true" PageSize="20" OnSorting="SortRecords" ShowFooter="true"
                            CssClass="table table-bordered table-condensed table-striped" OnRowCommand="gvService_RowCommand" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#3366ff" OnRowDataBound="gvService_OnRowDataBound" OnPageIndexChanging="gvService_OnPageIndexChanging">


                            <EmptyDataTemplate>
                                <div style="font-weight: bold; color: Red;">
                                    No Order Found.
                                </div>
                            </EmptyDataTemplate>
                            <FooterStyle BackColor="LightGray" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sno" HeaderStyle-Width="3%">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                        <%--<asp:Label ID="lblSno" Visible="false" runat="Server" Text='<%#Eval("SNo")%>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalServices" runat="server" Text="" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab No" HeaderStyle-Width="7%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDiagSampleId" runat="Server" Text='<%#Eval("DiagSampleId")%>'></asp:Label>
                                        <asp:Label ID="lblEncounterNo" runat="Server" Text='<%#Eval("EncounterNo")%>'></asp:Label>
                                        <asp:Label ID="lblServiceId" runat="Server" Text='<%#Eval("ServiceId")%>'></asp:Label>
                                        <asp:Label ID="lblStatusCode" runat="Server" Text='<%#Eval("StatusCode")%>'></asp:Label>
                                        <asp:Label ID="lblStationId" runat="Server" Text='<%#Eval("StationId")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lab No" HeaderStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLabNo" runat="Server" Text='<%#Eval("LabNo")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order No" HeaderStyle-Width="7%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderNo" runat="Server" Text='<%#Eval("OrderNo")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width="5%" SortExpression="OrderDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="Server" Text='<%#Eval("OrderDate")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Service Name" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceName" runat="server" Text='<%#Eval("ServiceName") %>' ToolTip='<%#Eval("ServiceName")%>' />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 50px">
                                                    <asp:LinkButton ID="lblservicename" runat="server" Text="Service" Style="color: #fff;" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlService" SkinID="DropDown" Filter="Contains"
                                                        DropDownWidth="300px" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlService_OnSelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sample Status" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSampleStatus" DropDownWidth="300px" runat="server" Text='<%#Eval("Status") %>' ToolTip='<%#Eval("Status")%>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 50px">
                                                    <asp:LinkButton ID="lblStatus" runat="server" Text="Status" Style="color: #fff;" />
                                                </td>
                                                <td>
                                                    <telerik:RadComboBox ID="ddlStatus" SkinID="DropDown" Filter="Contains"
                                                        Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Encoded&nbsp;By" HeaderStyle-Width="5%" SortExpression="EncodedBy">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncodedBy" runat="Server" Text='<%#Eval("EncodedBy")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Encoded Date" HeaderStyle-Width="5%" SortExpression="EncodedDate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncodedDate" runat="Server" Text='<%#Eval("EncodedDate")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Print Label" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkLabel" runat="server" ToolTip="Print Label" CommandName="PrintLabel" ForeColor="Gray"
                                            SkinID="Button" Text="Print Label"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Result / Print" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkResult" runat="server" CommandName="Result" ForeColor="Gray"
                                            SkinID="Button" Text="Result"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkPrint" runat="server" CommandName="Print" ForeColor="Gray"
                                            SkinID="Button" Text="Print"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" />
                            <RowStyle Wrap="False" />
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="gvService" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <div class="">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>          
    </form>
    <script type="text/javascript">
       function OnClientCloseServices(oWnd, args) {
              $get('<%=btnRefreshServices.ClientID %>').click();
            }
    </script>
</body>
</html>
