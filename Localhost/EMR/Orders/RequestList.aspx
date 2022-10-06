<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RequestList.aspx.cs" Inherits="EMR_Orders_RequestList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Include/Style.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel='stylesheet' type='text/css'>
    <title>Request List</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scriptmgr1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
           
            <div class="container-fluid header_main">
                 <div class="col-md-3">
                  <h2></h2>
                 </div>
				 
                 <div class="col-md-5 text-center"><asp:Label ID="lblMessage" runat="server" Text="" CssClass="relativ alert_new"></asp:Label> </div>
                 
                 <div class="col-md-3 text-right pull-right"> 
                     <asp:Button ID="ibtnClose" runat="server" AccessKey="C" cssClass="btn btn-primary" Text="Close"
                                ToolTip="Close" OnClientClick="window.close();" />
                     
                 </div>
</div>
            
            
          <div class="container-fluid">  
            <asp:Xml ID="xmlPatientInfo" runat="server"></asp:Xml>
              </div>    






            </ContentTemplate>
            </asp:UpdatePanel>
        <div class="container-fluid">
                        <asp:GridView ID="gvData" runat="server" Width="100%" AutoGenerateColumns="false"
                            AllowPaging="true" PageSize="10" OnPageIndexChanging="gvData_OnPageIndexChanging"
                             OnRowDeleting="gvData_OnRowDeleting" SkinID="gridview" OnRowDataBound="gvData_OnDataBinding">
                            <Columns>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, Serialno%>' HeaderStyle-Width="30px"
                                    ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex+1  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='Request No.' HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderID" runat="server" Text='<% #Eval("OrderID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width="120px" ItemStyle-Width="120px"
                                    HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderDate" runat="server" Text='<% #Eval("OrderDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Source" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientType" runat="server" Text='<% #Eval("Source") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, regno %>' HeaderStyle-Width="70px"
                                    Visible="false" ItemStyle-Width="70px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<% #Eval("RegistrationNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText='<%$ Resources:PRegistration, encounterno %>' HeaderStyle-Width="50px"
                                    Visible="false" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<% #Eval("EncounterNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Department Name" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDepartmentName" runat="server" Text='<% #Eval("DepartmentName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Department Name" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblSunbDepartmentName" runat="server" Text='<%#Eval("SubDepName") %>'
                                            OnClick="lnkViewService_OnClick"></asp:LinkButton>
                                        <asp:HiddenField ID="hdnOrderId" runat="server" Value='<%#Eval("OrderId") %>' />
                                        <asp:HiddenField ID="hdnRegistrationId" runat="server" Value='<%#Eval("RegistrationId") %>' />
                                        <asp:HiddenField ID="hdnEncounterId" runat="server" Value='<%#Eval("EncounterId") %>' />
                                        <asp:HiddenField ID="hdnSubDepId" runat="server" Value='<%#Eval("SubDeptId") %>' />
                                        <asp:HiddenField ID="hdnDepId" runat="server" Value='<%#Eval("DepartmentId") %>' />
                                        <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId") %>' />
                                        <asp:HiddenField ID="hdnCode" runat="server" Value='<%#Eval("StatusCode") %>' />
                                        <asp:HiddenField ID="hdnIsAcknowledge" runat="server" Value='<%#Eval("IsAcknowledged") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doctor Name" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctorName" runat="server" Text='<% #Eval("DoctorName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Encoded By" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncodedBy" runat="server" Text='<% #Eval("EncodedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" Text="Select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkViewDetails" runat="server" Text="Edit" OnClick="lnkViewDetails_OnClick"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                      
                                      <asp:ImageButton ID="ibtndaDelete" runat="server" ToolTip="Click here to delete this row"
                                                CommandName="Delete" CausesValidation="false" 
                                                ImageUrl="~/Images/DeleteRow.png"  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <%--<asp:CommandField ButtonType="Image" ItemStyle-HorizontalAlign="Center" DeleteImageUrl="~/Images/DeleteRow.png" HeaderStyle-Width="50px" ItemStyle-Width="50px"
                                                                    ShowDeleteButton="true" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    

        <div class="container-fluid">
                        <asp:Label ID="lblAcknowledged" runat="server" BorderWidth="1px" Text="&nbsp;" Width="20px" />
                        <asp:Label ID="Label12" runat="server" SkinID="label" Text="Acknowledged Orders"></asp:Label>
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                        <telerik:RadWindowManager ID="RadWindowManager3" EnableViewState="false" runat="server">
                            <Windows>
                                <telerik:RadWindow ID="RadWindow3" runat="server" Behaviors="Close,Move">
                                </telerik:RadWindow>
                            </Windows>
                        </telerik:RadWindowManager>
                        
                        <asp:Panel ID="Panel1" runat="server">
                    <div id="divDelete" runat="server" visible="false" style="width: 250px; z-index: 100;
                        border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; background-color: #FFF8DC;
                        border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; position: absolute;
                        bottom: 0; height: 75px; left: 450px; top: 300px">
                        <table width="100%" border="0">
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Label ID="lblTitle" runat="server" Text="Delete ?" SkinID="label" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="btnYes_OnClick" SkinID="Button" />
                                    <asp:HiddenField ID="hdnSubDepId" runat="server" />
                                    <asp:HiddenField ID="hdnRequestId" runat="server" />
                                     <asp:HiddenField ID="hdnRegistrationId" runat="server" />
                                      <asp:HiddenField ID="hdnEncounterId" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="btnNo" runat="server" Text="No" OnClick="btnNo_OnClick" SkinID="Button" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                    <asp:PostBackTrigger ControlID="btnYes" />
                    </Triggers>
                    </asp:UpdatePanel>
                  </div>  
        <%-- </ContentTemplate>
        <Triggers>
                <asp:PostBackTrigger ControlID="gvData" />
            </Triggers>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
