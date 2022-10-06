<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NonDrugOrder.ascx.cs" Inherits="EMR_Dashboard_PatientParts_NonDrugOrder" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table border="0" width="100%" cellpadding="0" cellspacing="0" style="border: solid 1px #ccc">
    <tr valign="top">
        <td style="width: 100%">
             <asp:Label ID="lblMessage" runat="server" Font-Bold="true" Text="" Width="100%" />
            <asp:Panel ID="pnlNonDrugOrderGrid" runat="server" ScrollBars="None" Width="100%" Height="90%">
                <asp:UpdatePanel ID="upNondrugorder" runat="server">
                    <ContentTemplate>
                        <telerik:RadFormDecorator ID="RadFormDecorator1" DecoratedControls="All" runat="server"
                            DecorationZoneID="dvZone1" Skin="Metro"></telerik:RadFormDecorator>
                        <div id="dvZone1" style="width: 100%">
                            <asp:GridView ID="gvNonDrugOrder" runat="server" AllowPaging="true" PageSize="7" ForeColor="#333333"
                                SkinID="gridview" GridLines="Both" OnPageIndexChanging="gvNonDrugOrder_PageIndexChanging" Width="95%" AutoGenerateColumns="false" CellPadding="0"
                                ShowHeader="true"  >
                                <Columns>
                                    <asp:TemplateField HeaderText = "Id" Visible="false">
                                         <ItemTemplate>
                                            <asp:Label ID="lblNonDrugOrderId" runat="server"  Text='<%#Eval("NonDrugOrderId")%>' />
                                            <asp:HiddenField ID="hdnAcknowledge" runat="server" Value='<%#Eval("Acknowledge")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Order Date">
                                         <ItemTemplate>
                                            <asp:Label ID="lblOrderDate" runat="server"  Text='<%#Eval("OrderDate")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Non Drug Orders" >
                                         <ItemTemplate>
                                              <asp:Label ID="lblPrescription" runat="server"  Text='<%#Eval("Prescription")%>' />
                                               <asp:HiddenField ID="hdnEncodedById" runat="server" Value='<%#Eval("EncodedById") %>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Order Type" >
                                         <ItemTemplate>
                                              <asp:Label ID="lblOrderType" runat="server"  Text='<%#Eval("OrderTypeName")%>' />
                                               <asp:HiddenField ID="hdnOrderType" runat="server" Value='<%#Eval("OrderType")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Doctor Name" >
                                         <ItemTemplate>
                                               <asp:Label ID="lblDoctorName" runat="server" SkinID="label" Text='<%#Eval("DoctorName")%>' />
                                                                <asp:HiddenField ID="hdnDoctorId" runat="server" Value='<%#Eval("DoctorId")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText = "Encoded By" >
                                         <ItemTemplate>
                                               <asp:Label ID="lblAcknowledgeBy" runat="server"  Text='<%#Eval("AcknowledgeBy")%>' />
                                                <asp:HiddenField ID="hdnNurseId" runat="server" Value='<%#Eval("NurseId")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Encoded Date" >
                                         <ItemTemplate>
                                              <asp:Label ID="lblAcknowledgeDate" runat="server"  Text='<%#Eval("AcknowledgeDate")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Modify By" Visible="false" >
                                         <ItemTemplate>
                                              <asp:Label ID="lblModifyBy" runat="server"  Text='<%#Eval("ModifyBy")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                     <asp:TemplateField HeaderText = "Modify Date" Visible="false" >
                                         <ItemTemplate>
                                           <asp:Label ID="lblModifyDate" runat="server"  Text='<%#Eval("ModifyDate")%>' />
                                        </ItemTemplate>                             
                                </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            
                                           
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </td>
    </tr>
   
    
</table>
