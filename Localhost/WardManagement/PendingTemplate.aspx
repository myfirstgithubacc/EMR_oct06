<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingTemplate.aspx.cs" Inherits="WardManagement_PendingTemplate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Template Lists</title>
    <link href="../Include/css/open-sans.css" rel="stylesheet" />
    <link href="../Include/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Include/css/mainNew.css" rel="stylesheet" />
    <link href="../Include/EMRStyle.css" rel="stylesheet" type="text/css" />
    <link href="../Include/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <asp:ScriptManager ID="scriptmgr1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upd1" runat="server">
            <ContentTemplate>
                <div class="container-fluid header_main form-group">
                    <div class="col-sm-8 text-left"><asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" /></div>
                    <div class="col-sm-4 text-right"><asp:Button ID="btnCloseW" Text="Close" runat="server" ToolTip="Close" CssClass="btn btn-default" OnClientClick="window.close();" /></div>
                </div>

                <div class="container-fluid">
                    <div class="row">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvTemplateList" AllowPaging="true" PageSize="15" runat="server" SkinID="gridviewOrderNew" Width="100%"
                            AutoGenerateColumns="false" OnPageIndexChanging="gvTemplateList_PageIndexChanging">
                            <EmptyDataTemplate>
                                <p style="color: Red;">
                                    No Record(s) Found</p>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText='Registration No' ItemStyle-Width="100px">
                                    <ItemTemplate>
                                       
                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Eval("RegistrationNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Encounter No" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEncounterNo" runat="server" Text='<%#Eval("EncounterNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Patient Name" ItemStyle-Width="180px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPatientName" runat="server" Text='<%#Eval("PatientName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Admission Date" ItemStyle-Width="140px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAdmissionDate" runat="server" Text='<%#Eval("AdmissionDate")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                             
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Doctor Name" ItemStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDoctorName" runat="server" Text='<%#Eval("DoctorName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Ward" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblWard" runat="server" Text='<%#Eval("Ward")%>' />
                                    
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Bed No" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBedNo" runat="server" Text='<%#Eval("BedNo")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Template Name" ItemStyle-Width="120px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("TemplateName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                             
                                <asp:TemplateField HeaderStyle-HorizontalAlign="left" HeaderText="Over Due" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOverDue" runat="server" Text='<%#Eval("OverDue")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                              
                            </Columns>
                        </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                       
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
